using Avalonia.Media;

namespace OptiVis.UI.Shared.Models;

public enum CallStatus
{
    Answered,
    NoAnswer,
    Busy,
    Failed,
    Congestion,
    Cancel
}

/// <summary>
/// Dashboard umumiy statistika — backend TZ bo'yicha yangilangan.
/// linkedId bazasida, ext-queues qatorlari.
/// </summary>
public class DashboardSummary
{
    public int TotalCalls { get; set; }

    public int TotalIncoming { get; set; }
    public int AnsweredIncoming { get; set; }
    public int MissedIncoming { get; set; }
    public int Abandoned { get; set; }

    public int TotalOutgoing { get; set; }
    public int AnsweredOutgoing { get; set; }

    public double AnswerRate { get; set; }
    public TimeSpan AvgWaitTime { get; set; }
    public TimeSpan AvgTalkTime { get; set; }
    public TimeSpan AvgDuration { get; set; }
    public TimeSpan TotalTalkTime { get; set; }
    public TimeSpan TotalDuration { get; set; }
    
    public TimeSpan AvgOutgoingTalkTime { get; set; }
    public TimeSpan AvgOutgoingDuration { get; set; }
    public TimeSpan AvgIncomingTalkTime { get; set; }
    public TimeSpan AvgIncomingDuration { get; set; }
    public double OutgoingAnswerRate { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public int MissedOutgoing => TotalOutgoing - AnsweredOutgoing;

    [System.Text.Json.Serialization.JsonIgnore]
    public int TotalAnswered => AnsweredIncoming + AnsweredOutgoing;
    
    [System.Text.Json.Serialization.JsonIgnore]
    public int TotalNotAnswered => TotalCalls - TotalAnswered;

    [System.Text.Json.Serialization.JsonIgnore]
    public int Answered => AnsweredIncoming;
    [System.Text.Json.Serialization.JsonIgnore]
    public int SuccessfulCalls => AnsweredIncoming;
    [System.Text.Json.Serialization.JsonIgnore]
    public int CancelledCalls => Abandoned;
    [System.Text.Json.Serialization.JsonIgnore]
    public int BusyCalls => 0;
    [System.Text.Json.Serialization.JsonIgnore]
    public int BusyOrCongestion => 0;
    [System.Text.Json.Serialization.JsonIgnore]
    public int NoAnswer => MissedIncoming;
    [System.Text.Json.Serialization.JsonIgnore]
    public int NoAnswerCalls => MissedIncoming;
    [System.Text.Json.Serialization.JsonIgnore]
    public double SuccessRate => AnswerRate;
    [System.Text.Json.Serialization.JsonIgnore]
    public TimeSpan AvgDurationOld => AvgTalkTime;
}

public class HourlyCalls
{
    public int Hour { get; set; }
    public int Inbound { get; set; }
    public int Outbound { get; set; }
}

public class TrendPoint
{
    public string Label { get; set; } = string.Empty;
    public int Inbound { get; set; }
    public int Outbound { get; set; }
}

/// <summary>
/// Operator statistikasi — backend TZ bo'yicha to'liq yangilangan.
/// Kiruvchi/chiquvchi alohida; linkedId bazasida dublikatsiz.
/// </summary>
public class OperatorStats
{
    public string Extension { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? MobileNumber { get; set; }
    public string? AvatarColor { get; set; }

    // Kiruvchi (queue orqali, linkedId bazasida)
    public int TotalIncoming { get; set; }
    public int AnsweredIncoming { get; set; }
    public int MissedIncoming { get; set; }

    // Chiquvchi
    public int TotalOutgoing { get; set; }
    public int AnsweredOutgoing { get; set; }

    // Vaqt
    public TimeSpan TotalTalkTime { get; set; }
    public TimeSpan AvgTalkTime { get; set; }
    public TimeSpan TotalDuration { get; set; }

    // Hisoblangan
    public double IncomingAnswerRate { get; set; }

    // Jami aloqa foizi (TotalAnswered / TotalCalls * 100)
    public double OverallAnswerRate => TotalCalls > 0
        ? Math.Round((double)TotalAnswered / TotalCalls * 100, 1)
        : 0;

    // Jami (kiruvchi + chiquvchi)
    public int TotalCalls { get; set; }
    public int TotalAnswered { get; set; }

    // Holat
    public bool IsActiveToday { get; set; }

    // UI computed
    public string Initials => GetInitials(Name, Extension);

    public string TalkTimeFormatted
    {
        get
        {
            var ts = TotalTalkTime;
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}h {ts.Minutes}m";
            if (ts.TotalMinutes >= 1)
                return $"{ts.Minutes}m {ts.Seconds}s";
            return ts.TotalSeconds > 0 ? $"{ts.Seconds}s" : "—";
        }
    }

    public string DurationFormatted
    {
        get
        {
            var ts = TotalDuration;
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}h {ts.Minutes}m";
            if (ts.TotalMinutes >= 1)
                return $"{ts.Minutes}m {ts.Seconds}s";
            return ts.TotalSeconds > 0 ? $"{ts.Seconds}s" : "—";
        }
    }

    public string AvgTalkFormatted
    {
        get
        {
            var ts = AvgTalkTime;
            if (ts.TotalMinutes >= 1)
                return $"{(int)ts.TotalMinutes}:{ts.Seconds:D2}";
            return ts.TotalSeconds > 0 ? $"0:{ts.Seconds:D2}" : "—";
        }
    }

    private static string GetInitials(string name, string ext)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return $"{parts[0][0]}{parts[1][0]}".ToUpper();
            return parts[0][..Math.Min(2, parts[0].Length)].ToUpper();
        }
        return ext.Length <= 2 ? ext.ToUpper() : ext[^Math.Min(2, ext.Length)..].ToUpper();
    }

    // Eski nomlar compatibility
    [System.Text.Json.Serialization.JsonIgnore]
    public int AnsweredCalls => TotalAnswered;
    [System.Text.Json.Serialization.JsonIgnore]
    public int MissedCalls => MissedIncoming;
    [System.Text.Json.Serialization.JsonIgnore]
    public int CancelledCalls => 0;
    [System.Text.Json.Serialization.JsonIgnore]
    public int BusyCalls => 0;
    [System.Text.Json.Serialization.JsonIgnore]
    public int NoAnswerCalls => 0;
    [System.Text.Json.Serialization.JsonIgnore]
    public double SuccessRate => TotalCalls > 0
        ? Math.Round((double)TotalAnswered / TotalCalls * 100, 1)
        : 0;
    [System.Text.Json.Serialization.JsonIgnore]
    public TimeSpan TotalTimeSpent => TotalTalkTime;
}

public class CallRecord
{
    public int Sequence { get; set; }
    public DateTime CallDate { get; set; }
    public string CallerNumber { get; set; } = string.Empty;
    public string OperatorExtension { get; set; } = string.Empty;
    public string OperatorName { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int BillSec { get; set; }
    public string Disposition { get; set; } = string.Empty;
    public bool IsIncoming { get; set; }
    public string RecordingFile { get; set; } = string.Empty;
    public string Src { get; set; } = string.Empty;
    public string Dst { get; set; } = string.Empty;

    public string DurationFormatted => Duration > 0
        ? TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss")
        : "--:--";

    public IBrush StatusColor => Disposition?.ToUpper() switch
    {
        "ANSWERED" => SolidColorBrush.Parse("#10B981"),
        "NO ANSWER" => SolidColorBrush.Parse("#F59E0B"),
        "NOANSWER" => SolidColorBrush.Parse("#F59E0B"),
        "BUSY" => SolidColorBrush.Parse("#EF4444"),
        "FAILED" => SolidColorBrush.Parse("#EF4444"),
        "CANCEL" => SolidColorBrush.Parse("#6B7280"),
        "CONGESTION" => SolidColorBrush.Parse("#6B7280"),
        _ => SolidColorBrush.Parse("#6B7280")
    };

    public string StatusIcon => Disposition?.ToUpper() switch
    {
        "ANSWERED" => "mdi-phone-incoming",
        "NO ANSWER" => "mdi-phone-missed",
        "NOANSWER" => "mdi-phone-missed",
        "BUSY" => "mdi-phone-in-talk",
        "FAILED" => "mdi-alert-circle",
        "CANCEL" => "mdi-cancel",
        "CONGESTION" => "mdi-network-off",
        _ => "mdi-phone"
    };
}

public class CallSearchResult
{
    public int Id { get; set; }
    public DateTime CallDate { get; set; }
    public string CallerNumber { get; set; } = string.Empty;
    public string OperatorName { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public CallStatus Status { get; set; }
}

public class CallLog
{
    public int Sequence { get; set; }
    public DateTime CallDate { get; set; }
    public string Src { get; set; } = string.Empty;
    public string Dst { get; set; } = string.Empty;
    public string OperatorExtension { get; set; } = string.Empty;
    public string OperatorName { get; set; } = string.Empty;
    public bool IsIncoming { get; set; }
    public int Duration { get; set; }
    public int BillSec { get; set; }
    public int WaitSeconds { get; set; }
    public string Disposition { get; set; } = string.Empty;
    public string Direction => IsIncoming ? "IN" : "OUT";
}

public class IdlePeriod
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration => End - Start;
}

public class PhoneNumberStats
{
    public string PhoneNumber { get; set; } = string.Empty;
    public int TotalCalls { get; set; }
    public int IncomingCalls { get; set; }
    public int OutgoingCalls { get; set; }
    public int SuccessfulCalls { get; set; }
    public int CancelledCalls { get; set; }
    public int NoAnswerCalls { get; set; }
    public int BusyCalls { get; set; }
    public DateTime LastCallDate { get; set; }
}

public class PhoneNumberCallDetail
{
    public DateTime CallDate { get; set; }
    public string OperatorExtension { get; set; } = string.Empty;
    public string OperatorName { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int BillSec { get; set; }
    public int WaitSeconds { get; set; }
    public string Disposition { get; set; } = string.Empty;
    
    public string DurationFormatted => Duration > 0
        ? TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss")
        : "--:--";
    
    public string BillSecFormatted => BillSec > 0
        ? TimeSpan.FromSeconds(BillSec).ToString(@"mm\:ss")
        : "--:--";
    
    public string WaitFormatted => WaitSeconds > 0
        ? TimeSpan.FromSeconds(WaitSeconds).ToString(@"mm\:ss")
        : "--:--";
}

public class OperatorCallItem
{
    public DateTime CallDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsIncoming { get; set; }
    public int Duration { get; set; }
    public int BillSec { get; set; }
    public string Disposition { get; set; } = string.Empty;

    public string DurationFormatted => Duration > 0
        ? TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss")
        : "--:--";

    public string BillSecFormatted => BillSec > 0
        ? TimeSpan.FromSeconds(BillSec).ToString(@"mm\:ss")
        : "--:--";

    public string DirectionIcon => IsIncoming ? "mdi-phone-incoming" : "mdi-phone-outgoing";
    public string DirectionColor => IsIncoming ? "#3B82F6" : "#8B5CF6";

    public IBrush StatusBrush => Disposition?.ToUpperInvariant() switch
    {
        "ANSWERED" => SolidColorBrush.Parse("#10B981"),
        "NO ANSWER" => SolidColorBrush.Parse("#F59E0B"),
        "NOANSWER" => SolidColorBrush.Parse("#F59E0B"),
        "BUSY" => SolidColorBrush.Parse("#EF4444"),
        "FAILED" => SolidColorBrush.Parse("#EF4444"),
        "CANCEL" => SolidColorBrush.Parse("#6B7280"),
        _ => SolidColorBrush.Parse("#6B7280")
    };
}
