using Avalonia;
using Avalonia.Styling;
using System.Text.Json;

namespace OptiVis.UI.Shared.Services;

public enum AppTheme
{
    Dark,
    Light
}

/// <summary>
/// Real-time theme boshqarish. App.Current.RequestedThemeVariant ni o'zgartiradi.
/// Settings %AppData%\OptiVis\settings.json da saqlanadi.
/// </summary>
public class ThemeManager
{
    private static ThemeManager? _instance;
    public static ThemeManager Instance => _instance ??= new ThemeManager();

    private AppTheme _currentTheme = AppTheme.Dark;
    private readonly string _settingsPath;

    public AppTheme CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme == value) return;
            _currentTheme = value;
            ApplyTheme();
            SaveSettings();
            OnThemeChanged?.Invoke(_currentTheme);
        }
    }

    public event Action<AppTheme>? OnThemeChanged;

    private ThemeManager()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folder = Path.Combine(appData, "OptiVis");
        Directory.CreateDirectory(folder);
        _settingsPath = Path.Combine(folder, "settings.json");
        LoadSettings();
    }

    /// <summary>
    /// Theme ni Avalonia RequestedThemeVariant ga qo'llash
    /// </summary>
    public void ApplyTheme()
    {
        if (Application.Current == null) return;

        var variant = _currentTheme switch
        {
            AppTheme.Light => ThemeVariant.Light,
            _ => ThemeVariant.Dark
        };
        Application.Current.RequestedThemeVariant = variant;
    }

    /// <summary>
    /// Dastur ishga tushganda sozlamalarni yuklash va qo'llash
    /// </summary>
    public void Initialize()
    {
        LoadSettings();
        ApplyTheme();
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);
                if (settings != null)
                {
                    _currentTheme = settings.Theme;
                }
            }
        }
        catch
        {
            _currentTheme = AppTheme.Dark;
        }
    }

    private void SaveSettings()
    {
        try
        {
            var settings = LoadFullSettings();
            settings.Theme = _currentTheme;
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsPath, json);
        }
        catch
        {
            // Saqlashda xatolik — ignore
        }
    }

    private AppSettings LoadFullSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
        }
        catch { }
        return new AppSettings();
    }

    public string GetThemeDisplayName(AppTheme theme) => theme switch
    {
        AppTheme.Light => "Light",
        AppTheme.Dark => "Dark",
        _ => "Dark"
    };
}

/// <summary>
/// Barcha app sozlamalari — settings.json da saqlanadi
/// </summary>
public class AppSettings
{
    public AppTheme Theme { get; set; } = AppTheme.Dark;
    public string Language { get; set; } = "UzbekLatin";
    public string BackendUrl { get; set; } = "https://tel-mon.hamrohmmt.uz/";
}
