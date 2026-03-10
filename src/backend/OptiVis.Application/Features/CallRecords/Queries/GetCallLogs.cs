using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Entities;
using OptiVis.Domain.Interfaces;

namespace OptiVis.Application.Features.CallRecords.Queries;

public record GetCallLogsQuery(DateTime From, DateTime To, string? Number = null) : IRequest<IReadOnlyList<CallLogDto>>;

public class GetCallLogsHandler : IRequestHandler<GetCallLogsQuery, IReadOnlyList<CallLogDto>>
{
    private readonly ICallRecordRepository _repository;

    public GetCallLogsHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CallLogDto>> Handle(GetCallLogsQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetLogsAsync(request.From, request.To, request.Number, cancellationToken);

        return calls
            .GroupBy(c => c.LinkedId)
            .Select(g => SelectRepresentative(g))
            .OrderByDescending(c => c.CallDate)
            .Select(c => new CallLogDto(
                c.Sequence,
                c.CallDate,
                c.Src,
                c.Dst,
                c.OperatorExtension,
                GetExtLabel(c.OperatorExtension),
                c.IsIncoming,
                c.Duration,
                c.BillSec,
                Math.Max(c.Duration - c.BillSec, 0),
                c.DispositionRaw
            ))
            .ToList();
    }

    private static CallRecord SelectRepresentative(IGrouping<string, CallRecord> group)
    {
        var answered = group.FirstOrDefault(r => r.DispositionRaw == "ANSWERED" && r.BillSec > 0);
        return answered ?? group.OrderBy(r => r.Sequence).First();
    }

    private static string GetExtLabel(string ext) =>
        string.IsNullOrEmpty(ext) ? "" : $"Ext {ext}";
}
