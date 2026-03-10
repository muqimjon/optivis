using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Enums;
using OptiVis.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OptiVis.Application.Features.CallRecords.Queries;

public record GetOperatorCallsQuery(string Extension, DateTime From, DateTime To) : IRequest<IReadOnlyList<OperatorCallDto>>;

public record OperatorCallDto(
    DateTime CallDate,
    string PhoneNumber,
    bool IsIncoming,
    int Duration,
    int BillSec,
    string Disposition
);

public class GetOperatorCallsHandler : IRequestHandler<GetOperatorCallsQuery, IReadOnlyList<OperatorCallDto>>
{
    private readonly ICallRecordRepository _repository;

    public GetOperatorCallsHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<OperatorCallDto>> Handle(GetOperatorCallsQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetCallsAsync(request.From, request.To, cancellationToken);
        var ext = request.Extension.TrimStart('0').PadLeft(2, '0');
        var extVariants = new[] { ext, ext.TrimStart('0'), "0" + ext.TrimStart('0') };

        var results = new List<OperatorCallDto>();
        var groupedByLinkedId = calls.GroupBy(c => c.LinkedId);

        foreach (var group in groupedByLinkedId)
        {
            var allInGroup = group.ToList();
            var first = allInGroup.OrderBy(c => c.Sequence).First();

            bool isIncoming = first.DContext == "ext-queues" && first.Channel.Contains("_in-");
            bool isOutgoing = first.DContext == "from-internal"
                && Regex.IsMatch(first.Channel, @"SIP/\d{1,5}-\w+")
                && !first.Channel.Contains("@from-queue");

            if (isIncoming)
            {
                var answeredByOperator = allInGroup
                    .Where(c => c.DContext == "from-internal" && Regex.IsMatch(c.Channel, @"Local/\d{1,5}@from-queue"))
                    .FirstOrDefault(c =>
                    {
                        var match = Regex.Match(c.Channel, @"Local/(\d{1,5})@from-queue");
                        if (match.Success)
                        {
                            var opExt = match.Groups[1].Value.TrimStart('0').PadLeft(2, '0');
                            return extVariants.Contains(opExt) && c.Disposition == CallDisposition.Answered && c.BillSec > 0;
                        }
                        return false;
                    });

                if (answeredByOperator != null)
                {
                    string phoneNumber = first.Src;
                    if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 5) continue;

                    results.Add(new OperatorCallDto(
                        first.CallDate,
                        phoneNumber,
                        true,
                        answeredByOperator.Duration,
                        answeredByOperator.BillSec,
                        "ANSWERED"
                    ));
                }
            }
            else if (isOutgoing)
            {
                var sipMatch = Regex.Match(first.Channel, @"SIP/(\d{1,5})-");
                if (!sipMatch.Success) continue;

                var opExt = sipMatch.Groups[1].Value.TrimStart('0').PadLeft(2, '0');
                if (!extVariants.Contains(opExt)) continue;

                string phoneNumber = first.Dst;
                if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 5) continue;
                if (extVariants.Contains(phoneNumber.TrimStart('0').PadLeft(2, '0'))) continue;

                var hasAnswered = allInGroup.Any(c => c.Disposition == CallDisposition.Answered && c.BillSec > 0);
                var answeredRecord = allInGroup.FirstOrDefault(c => c.Disposition == CallDisposition.Answered && c.BillSec > 0);

                CallDisposition disp;
                if (hasAnswered)
                    disp = CallDisposition.Answered;
                else if (allInGroup.Any(c => c.Disposition == CallDisposition.Cancel))
                    disp = CallDisposition.Cancel;
                else if (allInGroup.Any(c => c.Disposition == CallDisposition.Busy))
                    disp = CallDisposition.Busy;
                else if (allInGroup.Any(c => c.Disposition == CallDisposition.NoAnswer))
                    disp = CallDisposition.NoAnswer;
                else
                    disp = CallDisposition.Failed;

                int billSec = answeredRecord?.BillSec ?? 0;
                int duration = allInGroup.Max(c => c.Duration);

                results.Add(new OperatorCallDto(
                    first.CallDate,
                    phoneNumber,
                    false,
                    duration,
                    billSec,
                    disp.ToString().ToUpperInvariant().Replace("NOANSWER", "NO ANSWER")
                ));
            }
        }

        return results.OrderByDescending(x => x.CallDate).ToList();
    }
}
