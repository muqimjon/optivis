using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Application.Interfaces;
using OptiVis.Domain.Enums;
using OptiVis.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OptiVis.Application.Features.Operators.Queries;

public record GetOperatorStatsQuery(DateTime From, DateTime To) : IRequest<IReadOnlyList<OperatorStatsDto>>;

public class GetOperatorStatsHandler : IRequestHandler<GetOperatorStatsQuery, IReadOnlyList<OperatorStatsDto>>
{
    private readonly ICallRecordRepository _repository;
    private readonly IOperatorMappingService _mappingService;

    public GetOperatorStatsHandler(ICallRecordRepository repository, IOperatorMappingService mappingService)
    {
        _repository = repository;
        _mappingService = mappingService;
    }

    public async Task<IReadOnlyList<OperatorStatsDto>> Handle(GetOperatorStatsQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetCallsAsync(request.From, request.To, cancellationToken);

        var answeredIncomingByOperator = calls
            .Where(c =>
                c.DContext == "from-internal"
                && Regex.IsMatch(c.Channel, @"Local/\d{1,5}@from-queue")
                && c.Disposition == CallDisposition.Answered
                && c.BillSec > 0)
            .Select(c => new
            {
                c.LinkedId,
                Ext = _mappingService.GetPrimaryExtension(c.OperatorExtension),
                c.BillSec,
                c.Duration
            })
            .Where(x => !string.IsNullOrEmpty(x.Ext))
            .GroupBy(x => new { x.LinkedId, x.Ext })
            .Select(g => new
            {
                g.Key.LinkedId,
                g.Key.Ext,
                BillSec = g.Max(x => x.BillSec),
                Duration = g.Max(x => x.Duration)
            })
            .GroupBy(x => x.LinkedId)
            .Select(g => g.OrderByDescending(x => x.BillSec).First())
            .ToList();

        var outgoingLegs = calls
            .Where(c =>
                c.DContext == "from-internal"
                && Regex.IsMatch(c.Channel, @"SIP/\d{1,5}-\w+")
                && !c.Channel.Contains("@from-queue"))
            .ToList();

        var outgoingByOperator = outgoingLegs
            .Select(c => new
            {
                c.LinkedId,
                Ext = _mappingService.GetPrimaryExtension(ExtractExtensionFromChannel(c.Channel)),
                c.Disposition,
                c.BillSec,
                c.Duration
            })
            .Where(x => !string.IsNullOrEmpty(x.Ext))
            .GroupBy(x => new { x.LinkedId, x.Ext })
            .Select(g => new
            {
                g.Key.LinkedId,
                g.Key.Ext,
                Answered = g.Any(x => x.Disposition == CallDisposition.Answered && x.BillSec > 0),
                BillSec = g.Where(x => x.BillSec > 0).Sum(x => x.BillSec),
                Duration = g.Max(x => x.Duration)
            })
            .ToList();

        var activeExtensions = answeredIncomingByOperator
            .Select(x => x.Ext)
            .Union(outgoingByOperator.Select(x => x.Ext))
            .Distinct()
            .ToList();

        var result = new List<OperatorStatsDto>();

        foreach (var extension in activeExtensions)
        {
            var myAnsweredIncoming = answeredIncomingByOperator
                .Where(x => x.Ext == extension)
                .ToList();

            int answeredIncoming = myAnsweredIncoming.Count;
            int incomingTalkSecs = myAnsweredIncoming.Sum(x => x.BillSec);
            int incomingDurationSecs = myAnsweredIncoming.Sum(x => x.Duration);

            var myOutgoing = outgoingByOperator
                .Where(x => x.Ext == extension)
                .ToList();

            int totalOutgoing = myOutgoing.Select(x => x.LinkedId).Distinct().Count();
            int answeredOutgoing = myOutgoing.Where(x => x.Answered).Select(x => x.LinkedId).Distinct().Count();
            int outgoingTalkSecs = myOutgoing.Sum(x => x.BillSec);
            int outgoingDurationSecs = myOutgoing.Sum(x => x.Duration);

            int totalTalkSecs = incomingTalkSecs + outgoingTalkSecs;
            int totalDurationSecs = incomingDurationSecs + outgoingDurationSecs;
            int totalCalls = answeredIncoming + totalOutgoing;
            int totalAnswered = answeredIncoming + answeredOutgoing;

            var totalTalkTime = TimeSpan.FromSeconds(totalTalkSecs);
            var totalDuration = TimeSpan.FromSeconds(totalDurationSecs);
            var avgTalkTime = totalAnswered > 0
                ? TimeSpan.FromSeconds(totalTalkSecs / (double)totalAnswered)
                : TimeSpan.Zero;

            result.Add(new OperatorStatsDto(
                extension,
                $"Ext {extension}",
                answeredIncoming,
                answeredIncoming,
                0,
                totalOutgoing,
                answeredOutgoing,
                totalTalkTime,
                avgTalkTime,
                100.0,
                totalCalls,
                totalAnswered,
                totalDuration
            ));
        }

        return result
            .OrderByDescending(o => o.TotalTalkTime)
            .ThenByDescending(o => o.TotalCalls)
            .ToList();
    }

    private static string ExtractExtensionFromChannel(string channel)
    {
        if (string.IsNullOrEmpty(channel)) return string.Empty;
        var match = Regex.Match(channel, @"SIP/(\d{1,5})-");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}
