using OptiVis.Domain.Enums;

namespace OptiVis.Application.DTOs;

public record DashboardSummaryDto(
    int TotalCalls,
    int TotalIncoming,
    int AnsweredIncoming,
    int MissedIncoming,
    int Abandoned,
    int TotalOutgoing,
    int AnsweredOutgoing,
    double AnswerRate,
    TimeSpan AvgWaitTime,
    TimeSpan AvgTalkTime,
    TimeSpan AvgDuration,
    TimeSpan TotalTalkTime,
    TimeSpan TotalDuration,
    TimeSpan AvgOutgoingTalkTime,
    TimeSpan AvgOutgoingDuration,
    TimeSpan AvgIncomingTalkTime,
    TimeSpan AvgIncomingDuration,
    double OutgoingAnswerRate
);

public record HourlyCallsDto(int Hour, int Inbound, int Outbound);
public record TrendPointDto(string Label, int Inbound, int Outbound);
public record StatusDistributionDto(CallDisposition Status, int Count);

public record OperatorStatsDto(
    string Extension,
    string Name,
    int TotalIncoming,
    int AnsweredIncoming,
    int MissedIncoming,
    int TotalOutgoing,
    int AnsweredOutgoing,
    TimeSpan TotalTalkTime,
    TimeSpan AvgTalkTime,
    double IncomingAnswerRate,
    int TotalCalls,
    int TotalAnswered,
    TimeSpan TotalDuration
);

public record CallRecordDto(
    int Sequence,
    DateTime CallDate,
    string CallerNumber,
    string OperatorExtension,
    string OperatorName,
    int Duration,
    int BillSec,
    CallDisposition Disposition,
    bool IsIncoming,
    string RecordingFile
);

public record CallSearchResultDto(
    DateTime CallDate,
    string OperatorName,
    TimeSpan Duration,
    CallDisposition Status
);

public record RecentCallDto(
    int Sequence,
    DateTime CallDate,
    string Src,
    string Dst,
    string OperatorName,
    int Duration,
    int BillSec,
    string Disposition,
    string Direction = ""
);

public record CallLogDto(
    int Sequence,
    DateTime CallDate,
    string Src,
    string Dst,
    string OperatorExtension,
    string OperatorName,
    bool IsIncoming,
    int Duration,
    int BillSec,
    int WaitSeconds,
    string Disposition
);

public record PhoneNumberStatsDto(
    string PhoneNumber,
    int TotalCalls,
    int IncomingCalls,
    int OutgoingCalls,
    int SuccessfulCalls,
    int CancelledCalls,
    int NoAnswerCalls,
    int BusyCalls,
    DateTime LastCallDate
);

public record PhoneNumberCallDetailDto(
    DateTime CallDate,
    string OperatorExtension,
    string OperatorName,
    string Direction,
    int Duration,
    int BillSec,
    int WaitSeconds,
    string Disposition
);

public record RawCdrDto(
    int Sequence,
    DateTime CallDate,
    string Clid,
    string Src,
    string Dst,
    string DContext,
    string Channel,
    string DstChannel,
    string LastApp,
    string LastData,
    int Duration,
    int BillSec,
    string Disposition,
    int AmaFlags,
    string AccountCode,
    string UniqueId,
    string UserField,
    string Did,
    string RecordingFile,
    string CNum,
    string CName,
    string OutboundCNum,
    string OutboundCName,
    string DstCName,
    string LinkedId,
    string PeerAccount
);
