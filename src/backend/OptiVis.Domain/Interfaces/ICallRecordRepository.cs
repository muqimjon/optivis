using OptiVis.Domain.Entities;

namespace OptiVis.Domain.Interfaces;

public interface ICallRecordRepository
{
    Task<IReadOnlyList<CallRecord>> GetCallsAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IReadOnlyList<CallRecord>> GetCallsSinceAsync(int lastSequence, CancellationToken ct = default);
    Task<IReadOnlyList<CallRecord>> SearchByNumberAsync(string number, DateTime from, DateTime to, CancellationToken ct = default);
    Task<IReadOnlyList<CallRecord>> GetLogsAsync(DateTime from, DateTime to, string? number = null, CancellationToken ct = default);
    Task<IReadOnlyList<CallRecord>> GetRecentAsync(int count, CancellationToken ct = default);
    Task<int> GetLastSequenceAsync(CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetActiveExtensionsAsync(DateTime from, DateTime to, CancellationToken ct = default);
}
