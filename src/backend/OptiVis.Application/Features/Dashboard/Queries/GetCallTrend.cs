using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OptiVis.Application.Features.Dashboard.Queries;

public record GetCallTrendQuery(DateTime From, DateTime To) : IRequest<IReadOnlyList<TrendPointDto>>;

public class GetCallTrendHandler : IRequestHandler<GetCallTrendQuery, IReadOnlyList<TrendPointDto>>
{
    private readonly ICallRecordRepository _repository;

    public GetCallTrendHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<TrendPointDto>> Handle(GetCallTrendQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetCallsAsync(request.From, request.To, cancellationToken);
        var span = request.To - request.From;

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

        if (span.TotalHours <= 24)
        {
            var inboundByHour = incomingLegs
                .GroupBy(c => c.CallDate.Hour)
                .ToDictionary(g => g.Key, g => g.Count());
            var outboundByHour = outgoingLegs
                .GroupBy(c => c.CallDate.Hour)
                .ToDictionary(g => g.Key, g => g.Count());

            var hours = inboundByHour.Keys.Union(outboundByHour.Keys).OrderBy(h => h);
            return hours.Select(h => new TrendPointDto(
                $"{h:00}:00",
                inboundByHour.GetValueOrDefault(h, 0),
                outboundByHour.GetValueOrDefault(h, 0)
            )).ToList();
        }

        if (span.TotalDays <= 60)
        {
            var inboundByDay = incomingLegs
                .GroupBy(c => c.CallDate.Date)
                .ToDictionary(g => g.Key, g => g.Count());
            var outboundByDay = outgoingLegs
                .GroupBy(c => c.CallDate.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            var days = inboundByDay.Keys.Union(outboundByDay.Keys).OrderBy(d => d);
            return days.Select(d => new TrendPointDto(
                d.ToString("dd.MM"),
                inboundByDay.GetValueOrDefault(d, 0),
                outboundByDay.GetValueOrDefault(d, 0)
            )).ToList();
        }

        var inboundByMonth = incomingLegs
            .GroupBy(c => new DateTime(c.CallDate.Year, c.CallDate.Month, 1))
            .ToDictionary(g => g.Key, g => g.Count());
        var outboundByMonth = outgoingLegs
            .GroupBy(c => new DateTime(c.CallDate.Year, c.CallDate.Month, 1))
            .ToDictionary(g => g.Key, g => g.Count());

        var months = inboundByMonth.Keys.Union(outboundByMonth.Keys).OrderBy(m => m);
        return months.Select(m => new TrendPointDto(
            m.ToString("MMM yy"),
            inboundByMonth.GetValueOrDefault(m, 0),
            outboundByMonth.GetValueOrDefault(m, 0)
        )).ToList();
    }
}
