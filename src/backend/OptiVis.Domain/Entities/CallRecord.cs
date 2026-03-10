using OptiVis.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace OptiVis.Domain.Entities;

public class CallRecord
{
    [Column("calldate")]
    public DateTime CallDate { get; set; }

    [Column("clid")]
    public string Clid { get; set; } = string.Empty;

    [Column("src")]
    public string Src { get; set; } = string.Empty;

    [Column("dst")]
    public string Dst { get; set; } = string.Empty;

    [Column("dcontext")]
    public string DContext { get; set; } = string.Empty;

    [Column("channel")]
    public string Channel { get; set; } = string.Empty;

    [Column("dstchannel")]
    public string DstChannel { get; set; } = string.Empty;

    [Column("lastapp")]
    public string LastApp { get; set; } = string.Empty;

    [Column("lastdata")]
    public string LastData { get; set; } = string.Empty;

    [Column("duration")]
    public int Duration { get; set; }

    [Column("billsec")]
    public int BillSec { get; set; }

    [Column("disposition")]
    public string DispositionRaw { get; set; } = string.Empty;

    [Column("amaflags")]
    public int AmaFlags { get; set; }

    [Column("accountcode")]
    public string AccountCode { get; set; } = string.Empty;

    [Column("uniqueid")]
    public string UniqueId { get; set; } = string.Empty;

    [Column("userfield")]
    public string UserField { get; set; } = string.Empty;

    [Column("did")]
    public string Did { get; set; } = string.Empty;

    [Column("recordingfile")]
    public string RecordingFile { get; set; } = string.Empty;

    [Column("cnum")]
    public string CNum { get; set; } = string.Empty;

    [Column("cnam")]
    public string CName { get; set; } = string.Empty;

    [Column("outbound_cnum")]
    public string OutboundCNum { get; set; } = string.Empty;

    [Column("outbound_cnam")]
    public string OutboundCName { get; set; } = string.Empty;

    [Column("dst_cnam")]
    public string DstCName { get; set; } = string.Empty;

    [Column("linkedid")]
    public string LinkedId { get; set; } = string.Empty;

    [Column("peeraccount")]
    public string PeerAccount { get; set; } = string.Empty;

    [Column("sequence")]
    public int Sequence { get; set; }

    public CallDisposition Disposition => DispositionRaw?.ToUpperInvariant() switch
    {
        "ANSWERED"   => CallDisposition.Answered,
        "NO ANSWER"  => CallDisposition.NoAnswer,
        "BUSY"       => CallDisposition.Busy,
        "FAILED"     => CallDisposition.Failed,
        "CONGESTION" => CallDisposition.Congestion,
        _            => CallDisposition.Cancel
    };

    public bool IsIncoming =>
        DContext == "ext-queues"
        || DContext == "from-trunk"
        || DContext == "from-pstn"
        || DContext == "from-pstn-toheader"
        || DContext == "ivr"
        || DContext == "from-did-direct"
        || Channel.Contains("_in-", StringComparison.OrdinalIgnoreCase);

    public string OperatorExtension => ExtractOperatorExtension(Channel);

    public string CallerNumber => IsIncoming ? Src : Dst;

    public static string ExtractOperatorExtension(string channel)
    {
        if (string.IsNullOrEmpty(channel)) return string.Empty;

        if (channel.Contains("_in-", StringComparison.OrdinalIgnoreCase) ||
            channel.Contains("_out-", StringComparison.OrdinalIgnoreCase))
            return string.Empty;

        var queueMatch = Regex.Match(channel, @"Local/(\d{1,5})@from-queue");
        if (queueMatch.Success)
            return NormalizeExtension(queueMatch.Groups[1].Value);

        var internalMatch = Regex.Match(channel, @"Local/(\d+)@from-internal");
        if (internalMatch.Success)
        {
            var num = internalMatch.Groups[1].Value;
            if (num.Length >= 6)
                return string.Empty;
            return NormalizeExtension(num);
        }

        var sipMatch = Regex.Match(channel, @"(?:SIP|PJSIP)/(\d{1,5})-");
        if (sipMatch.Success)
            return NormalizeExtension(sipMatch.Groups[1].Value);

        return string.Empty;
    }

    public static string NormalizeExtension(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return string.Empty;
        var trimmed = raw.TrimStart('0');
        if (trimmed.Length == 0) return "0";
        if (trimmed.Length == 1) return "0" + trimmed;
        return trimmed;
    }
}
