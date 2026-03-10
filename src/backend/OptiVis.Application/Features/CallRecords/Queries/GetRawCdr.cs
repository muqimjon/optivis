using MediatR;
using OptiVis.Application.DTOs;
using OptiVis.Domain.Interfaces;

namespace OptiVis.Application.Features.CallRecords.Queries;

public record GetRawCdrQuery(DateTime From, DateTime To, int Limit = 1000) : IRequest<IReadOnlyList<RawCdrDto>>;

public class GetRawCdrHandler : IRequestHandler<GetRawCdrQuery, IReadOnlyList<RawCdrDto>>
{
    private readonly ICallRecordRepository _repository;

    public GetRawCdrHandler(ICallRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<RawCdrDto>> Handle(GetRawCdrQuery request, CancellationToken cancellationToken)
    {
        var calls = await _repository.GetCallsAsync(request.From, request.To, cancellationToken);

        return calls
            .OrderByDescending(c => c.CallDate)
            .Take(request.Limit)
            .Select(c => new RawCdrDto(
                c.Sequence,
                c.CallDate,
                c.Clid,
                c.Src,
                c.Dst,
                c.DContext,
                c.Channel,
                c.DstChannel,
                c.LastApp,
                c.LastData,
                c.Duration,
                c.BillSec,
                c.DispositionRaw,
                c.AmaFlags,
                c.AccountCode,
                c.UniqueId,
                c.UserField,
                c.Did,
                c.RecordingFile,
                c.CNum,
                c.CName,
                c.OutboundCNum,
                c.OutboundCName,
                c.DstCName,
                c.LinkedId,
                c.PeerAccount
            ))
            .ToList();
    }
}
