using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Enums;
using OptiVis.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OptiVis.Application.Features.CallRecords.Queries;

public record GetPhoneNumberStatsQuery(DateTime From, DateTime To) : IRequest<IReadOnlyList<PhoneNumberStatsDto>>;

public class GetPhoneNumberStatsHandler : IRequestHandler<GetPhoneNumberStatsQuery, IReadOnlyList<PhoneNumberStatsDto>>
{
    private readonly ICallRecordRepository _repository;

    public GetPhoneNumberStatsHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PhoneNumberStatsDto>> Handle(GetPhoneNumberStatsQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetCallsAsync(request.From, request.To, cancellationToken);

        var phoneNumberEvents = new List<(string PhoneNumber, DateTime CallDate, CallDisposition Disposition, string LinkedId, bool IsIncoming)>();
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
            if (string.IsNullOrEmpty(rawPhoneNumber) || rawPhoneNumber.Length < 5) continue;

            string phoneNumber = NormalizePhoneNumber(rawPhoneNumber);
            if (phoneNumber.Length < 9) continue;

            var hasAnswered = allInGroup.Any(c => c.Disposition == CallDisposition.Answered && c.BillSec > 0);

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

            phoneNumberEvents.Add((phoneNumber, first.CallDate, finalDisposition, linkedId, isIncoming));
        }

        return phoneNumberEvents
            .GroupBy(e => e.PhoneNumber)
            .Select(g => new PhoneNumberStatsDto(
                g.Key,
                g.Select(e => e.LinkedId).Distinct().Count(),
                g.Count(e => e.IsIncoming),
                g.Count(e => !e.IsIncoming),
                g.Count(e => e.Disposition == CallDisposition.Answered),
                g.Count(e => e.Disposition == CallDisposition.Cancel),
                g.Count(e => e.Disposition == CallDisposition.NoAnswer),
                g.Count(e => e.Disposition == CallDisposition.Busy),
                g.Max(e => e.CallDate)
            ))
            .OrderByDescending(x => x.TotalCalls)
            .Take(500)
            .ToList();
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
