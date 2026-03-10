using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Enums;
using OptiVis.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OptiVis.Application.Features.Dashboard.Queries;

public record GetDashboardSummaryQuery(DateTime From, DateTime To) : IRequest<DashboardSummaryDto>;

public class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly ICallRecordRepository _repository;

    public GetDashboardSummaryHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetCallsAsync(request.From, request.To, cancellationToken);

        var incomingLegs = calls
            .Where(c => c.DContext == "ext-queues" && c.Channel.Contains("_in-"))
            .GroupBy(c => c.LinkedId)
            .Select(g => g.OrderBy(c => c.Sequence).First())
            .ToList();

        var outgoingLegs = calls
            .Where(c =>
                c.DContext == "from-internal"
                && Regex.IsMatch(c.Channel, @"SIP/\d{1,5}-\w+")
                && !c.Channel.Contains("@from-queue"))
            .GroupBy(c => c.LinkedId)
            .Select(g => g.OrderBy(c => c.Sequence).First())
            .ToList();

        var answeredLinkedIds = calls
            .Where(c => c.Disposition == CallDisposition.Answered && c.BillSec > 0)
            .Select(c => c.LinkedId)
            .ToHashSet();

        int totalIncoming = incomingLegs.Count;
        int answeredIncoming = incomingLegs.Count(c => answeredLinkedIds.Contains(c.LinkedId));
        int missedIncoming = totalIncoming - answeredIncoming;

        var cancelledLinkedIds = calls
            .Where(c => c.Disposition == CallDisposition.Cancel)
            .Select(c => c.LinkedId)
            .ToHashSet();
        int abandoned = incomingLegs.Count(c =>
            cancelledLinkedIds.Contains(c.LinkedId) && !answeredLinkedIds.Contains(c.LinkedId));

        int totalOutgoing = outgoingLegs.Count;
        int answeredOutgoing = outgoingLegs.Count(c => answeredLinkedIds.Contains(c.LinkedId));

        int totalCalls = totalIncoming + totalOutgoing;

        var answeredRows = calls
            .Where(c => answeredLinkedIds.Contains(c.LinkedId)
                        && c.Disposition == CallDisposition.Answered
                        && c.BillSec > 0)
            .ToList();

        var incomingAnsweredRows = answeredRows
            .Where(c => incomingLegs.Any(i => i.LinkedId == c.LinkedId))
            .ToList();

        var outgoingAnsweredRows = answeredRows
            .Where(c => outgoingLegs.Any(o => o.LinkedId == c.LinkedId))
            .ToList();

        var avgWait = answeredRows.Count > 0
            ? TimeSpan.FromSeconds(answeredRows.Average(c => Math.Max(c.Duration - c.BillSec, 0)))
            : TimeSpan.Zero;

        var avgTalk = answeredRows.Count > 0
            ? TimeSpan.FromSeconds(answeredRows.Average(c => c.BillSec))
            : TimeSpan.Zero;

        var avgDuration = answeredRows.Count > 0
            ? TimeSpan.FromSeconds(answeredRows.Average(c => c.Duration))
            : TimeSpan.Zero;

        var totalTalkTime = TimeSpan.FromSeconds(answeredRows.Sum(c => c.BillSec));
        var totalDuration = TimeSpan.FromSeconds(answeredRows.Sum(c => c.Duration));

        var avgOutgoingTalkTime = outgoingAnsweredRows.Count > 0
            ? TimeSpan.FromSeconds(outgoingAnsweredRows.Average(c => c.BillSec))
            : TimeSpan.Zero;

        var avgOutgoingDuration = outgoingAnsweredRows.Count > 0
            ? TimeSpan.FromSeconds(outgoingAnsweredRows.Average(c => c.Duration))
            : TimeSpan.Zero;

        var avgIncomingTalkTime = incomingAnsweredRows.Count > 0
            ? TimeSpan.FromSeconds(incomingAnsweredRows.Average(c => c.BillSec))
            : TimeSpan.Zero;

        var avgIncomingDuration = incomingAnsweredRows.Count > 0
            ? TimeSpan.FromSeconds(incomingAnsweredRows.Average(c => c.Duration))
            : TimeSpan.Zero;

        double answerRate = totalIncoming > 0
            ? Math.Round((double)answeredIncoming / totalIncoming * 100, 1) : 0;

        double outgoingAnswerRate = totalOutgoing > 0
            ? Math.Round((double)answeredOutgoing / totalOutgoing * 100, 1) : 0;

        return new DashboardSummaryDto(
            totalCalls,
            totalIncoming,
            answeredIncoming,
            missedIncoming,
            abandoned,
            totalOutgoing,
            answeredOutgoing,
            answerRate,
            avgWait,
            avgTalk,
            avgDuration,
            totalTalkTime,
            totalDuration,
            avgOutgoingTalkTime,
            avgOutgoingDuration,
            avgIncomingTalkTime,
            avgIncomingDuration,
            outgoingAnswerRate
        );
    }
}
