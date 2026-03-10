using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Enums;
using OptiVis.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OptiVis.Application.Features.CallRecords.Queries;

public record GetRecentCallsQuery(int Count = 10) : IRequest<IReadOnlyList<RecentCallDto>>;

public class GetRecentCallsHandler : IRequestHandler<GetRecentCallsQuery, IReadOnlyList<RecentCallDto>>
{
    private readonly ICallRecordRepository _repository;

    public GetRecentCallsHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<RecentCallDto>> Handle(GetRecentCallsQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetRecentAsync(request.Count * 10, cancellationToken);

        var grouped = calls
            .GroupBy(c => c.LinkedId)
            .Select(g =>
            {
                var first = g.OrderBy(c => c.Sequence).First();
                var answered = g.FirstOrDefault(c => c.Disposition == CallDisposition.Answered && c.BillSec > 0);

                string callerNumber = first.Src;
                string destination = first.Dst;
                bool isIncoming = first.DContext == "ext-queues" && first.Channel.Contains("_in-");
                bool isOutgoing = first.DContext == "from-internal"
                    && Regex.IsMatch(first.Channel, @"SIP/\d{1,5}-\w+")
                    && !first.Channel.Contains("@from-queue");

                string operatorExt = "";
                if (answered != null)
                {
                    var match = Regex.Match(answered.DstChannel ?? "", @"SIP/(\d{1,5})-");
                    if (match.Success)
                        operatorExt = match.Groups[1].Value;
                }
                else if (isOutgoing)
                {
                    var match = Regex.Match(first.Channel, @"SIP/(\d{1,5})-");
                    if (match.Success)
                        operatorExt = match.Groups[1].Value;
                }

                int duration = answered?.Duration ?? g.Max(c => c.Duration);
                int billSec = answered?.BillSec ?? 0;
                string disposition = answered != null ? "ANSWERED" : first.DispositionRaw;

                return new RecentCallDto(
                    first.Sequence,
                    first.CallDate,
                    callerNumber,
                    destination,
                    string.IsNullOrEmpty(operatorExt) ? "" : $"Ext {operatorExt}",
                    duration,
                    billSec,
                    disposition,
                    isIncoming ? "IN" : isOutgoing ? "OUT" : "?"
                );
            })
            .OrderByDescending(c => c.CallDate)
            .Take(request.Count)
            .ToList();

        return grouped;
    }
}
