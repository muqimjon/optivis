using OptiVis.Application.DTOs;

namespace OptiVis.Application.Interfaces;

public interface IDashboardHubClient
{
    Task ReceiveNewCall(CallRecordDto call);
    Task ReceiveDashboardUpdate(DashboardSummaryDto summary);
    Task ReceiveOperatorStatsUpdate(IEnumerable<OperatorStatsDto> stats);
}
