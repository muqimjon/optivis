using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace OptiVis.Application.Features.Dashboard.Queries;

public record GetHourlyCallsQuery(DateTime From, DateTime To) : IRequest<IReadOnlyList<HourlyCallsDto>>;

public class GetHourlyCallsHandler : IRequestHandler<GetHourlyCallsQuery, IReadOnlyList<HourlyCallsDto>>
{
    private readonly ICallRecordRepository _repository;

    public GetHourlyCallsHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<HourlyCallsDto>> Handle(GetHourlyCallsQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetCallsAsync(request.From, request.To, cancellationToken);

        var inboundByHour = calls
            .Where(c => c.DContext == "ext-queues" && c.Channel.Contains("_in-"))
            .GroupBy(c => c.LinkedId)
            .Select(g => g.OrderBy(c => c.Sequence).First())
            .GroupBy(c => c.CallDate.Hour)
            .ToDictionary(g => g.Key, g => g.Count());

        var outboundByHour = calls
            .Where(c =>
                c.DContext == "from-internal"
                && Regex.IsMatch(c.Channel, @"SIP/\d{1,5}-\w+")
                && !c.Channel.Contains("@from-queue"))
            .GroupBy(c => c.LinkedId)
            .Select(g => g.OrderBy(c => c.Sequence).First())
            .GroupBy(c => c.CallDate.Hour)
            .ToDictionary(g => g.Key, g => g.Count());

        var result = new List<HourlyCallsDto>();
        for (int hour = 0; hour < 24; hour++)
        {
            result.Add(new HourlyCallsDto(
                hour,
                inboundByHour.GetValueOrDefault(hour, 0),
                outboundByHour.GetValueOrDefault(hour, 0)
            ));
        }

        return result;
    }
}
