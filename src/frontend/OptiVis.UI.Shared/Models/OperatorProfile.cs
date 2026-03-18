using ReactiveUI;

namespace OptiVis.UI.Shared.Models;

/// <summary>
/// Operator profili — faqat client local xotirasida saqlanadi (%AppData%\OptiVis\operators.json).
/// Backend bu ma'lumotlarni bilmaydi.
/// </summary>
public class OperatorProfile
{
    public string Extension { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    /// <summary>998XXXXXXXXX formatida mobil raqam (ixtiyoriy)</summary>
    public string? MobileNumber { get; set; }

    /// <summary>Avatar rang — hex format, masalan #3B82F6</summary>
    public string? Color { get; set; }

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Settings viewda tahrirlash uchun wrapper — IsSaved animation va AvatarColor qo'shimcha properties bilan
/// </summary>
public class OperatorProfileItem : ReactiveObject
{
    public string Extension { get; set; } = string.Empty;
    
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string? _mobileNumber;
    public string? MobileNumber
    {
        get => _mobileNumber;
        set => this.RaiseAndSetIfChanged(ref _mobileNumber, value);
    }

    public string AvatarColor { get; set; } = "#3B82F6";

    private bool _isSaved;
    public bool IsSaved
    {
        get => _isSaved;
        set => this.RaiseAndSetIfChanged(ref _isSaved, value);
    }
}
