using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Enums;
using OptiVis.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OptiVis.Application.Features.CallRecords.Queries;

public record GetPhoneNumberDetailsQuery(string Number, DateTime From, DateTime To) : IRequest<IReadOnlyList<PhoneNumberCallDetailDto>>;

public class GetPhoneNumberDetailsHandler : IRequestHandler<GetPhoneNumberDetailsQuery, IReadOnlyList<PhoneNumberCallDetailDto>>
{
    private readonly ICallRecordRepository _repository;

    public GetPhoneNumberDetailsHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PhoneNumberCallDetailDto>> Handle(GetPhoneNumberDetailsQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.SearchByNumberAsync(request.Number, request.From, request.To, cancellationToken);
        var searchNumber = NormalizePhoneNumber(request.Number);

        var results = new List<PhoneNumberCallDetailDto>();
        var processedLinkedIds = new HashSet<string>();

        var groupedByLinkedId = calls.GroupBy(c => c.LinkedId);

        foreach (var group in groupedByLinkedId)
        {
            var linkedId = group.Key;
            if (processedLinkedIds.Contains(linkedId)) continue;
            processedLinkedIds.Add(linkedId);

            var allInGroup = group.ToList();
            var first = allInGroup.OrderBy(c => c.Sequence).First();

            bool isIncoming = first.DContext == "ext-queues" && first.Channel.Contains("_in-");
            bool isOutgoing = first.DContext == "from-internal"
                && Regex.IsMatch(first.Channel, @"SIP/\d{1,5}-\w+")
                && !first.Channel.Contains("@from-queue");

            if (!isIncoming && !isOutgoing) continue;

            string rawPhoneNumber = isIncoming ? first.Src : first.Dst;
            string normalizedPhone = NormalizePhoneNumber(rawPhoneNumber);
            
            if (!normalizedPhone.Contains(searchNumber) && !searchNumber.Contains(normalizedPhone)) continue;

            var hasAnswered = allInGroup.Any(c => c.Disposition == CallDisposition.Answered && c.BillSec > 0);
            var answeredRecord = allInGroup.FirstOrDefault(c => c.Disposition == CallDisposition.Answered && c.BillSec > 0);

            string operatorExt = "";
            if (isIncoming && hasAnswered)
            {
                var queueMatch = allInGroup
                    .Where(c => c.DContext == "from-internal" && Regex.IsMatch(c.Channel, @"Local/\d{1,5}@from-queue"))
                    .FirstOrDefault(c => c.Disposition == CallDisposition.Answered && c.BillSec > 0);

                if (queueMatch != null)
                {
                    var match = Regex.Match(queueMatch.Channel, @"Local/(\d{1,5})@from-queue");
                    if (match.Success)
                        operatorExt = match.Groups[1].Value.PadLeft(2, '0');
                }
            }
            else if (isOutgoing)
            {
                var sipMatch = Regex.Match(first.Channel, @"SIP/(\d{1,5})-");
                if (sipMatch.Success)
                    operatorExt = sipMatch.Groups[1].Value.PadLeft(2, '0');
            }

            CallDisposition finalDisposition;
            if (hasAnswered)
                finalDisposition = CallDisposition.Answered;
            else if (allInGroup.Any(c => c.Disposition == CallDisposition.Cancel))
                finalDisposition = CallDisposition.Cancel;
            else if (allInGroup.Any(c => c.Disposition == CallDisposition.Busy))
                finalDisposition = CallDisposition.Busy;
            else if (allInGroup.Any(c => c.Disposition == CallDisposition.NoAnswer))
                finalDisposition = CallDisposition.NoAnswer;
            else
                finalDisposition = CallDisposition.Failed;

            int billSec = answeredRecord?.BillSec ?? 0;
            int duration = allInGroup.Max(c => c.Duration);

            results.Add(new PhoneNumberCallDetailDto(
                first.CallDate,
                operatorExt,
                string.IsNullOrEmpty(operatorExt) ? "" : $"Ext {operatorExt}",
                isIncoming ? "IN" : "OUT",
                duration,
                billSec,
                Math.Max(duration - billSec, 0),
                finalDisposition.ToString()
            ));
        }

        return results.OrderByDescending(x => x.CallDate).Take(100).ToList();
    }

    private static string NormalizePhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone)) return phone;
        
        var digits = Regex.Replace(phone, @"[^\d]", "");
        
        if (digits.StartsWith("998") && digits.Length == 12)
            return digits.Substring(3);
        
        if (digits.StartsWith("8") && digits.Length == 10)
            return digits.Substring(1);
            
        return digits;
    }
}
