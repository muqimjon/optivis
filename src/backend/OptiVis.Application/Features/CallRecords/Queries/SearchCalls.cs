using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Entities;
using OptiVis.Domain.Interfaces;

namespace OptiVis.Application.Features.CallRecords.Queries;

public record SearchCallsQuery(string Number, DateTime From, DateTime To) : IRequest<IReadOnlyList<CallSearchResultDto>>;

public class SearchCallsHandler : IRequestHandler<SearchCallsQuery, IReadOnlyList<CallSearchResultDto>>
{
    private readonly ICallRecordRepository _repository;

    public SearchCallsHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CallSearchResultDto>> Handle(SearchCallsQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.SearchByNumberAsync(request.Number, request.From, request.To, cancellationToken);

        return calls
            .GroupBy(c => c.LinkedId)
            .Select(g => SelectRepresentative(g))
            .OrderByDescending(c => c.CallDate)
            .Select(c => new CallSearchResultDto(
                c.CallDate,
                GetExtLabel(c.OperatorExtension),
                TimeSpan.FromSeconds(c.BillSec),
                c.Disposition
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
