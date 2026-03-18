using System.Text.Json;
using System.ComponentModel;
using OptiVis.UI.Shared.i18n;
using OptiVis.UI.Shared.Extensions;
using Avalonia.Threading;

namespace OptiVis.UI.Shared.Services;

public class LanguageManager : INotifyPropertyChanged
{
    private static LanguageManager? _instance;
    public static LanguageManager Instance => _instance ??= new LanguageManager();

    private AppLanguage _currentLanguage = AppLanguage.UzbekLatin;
    private readonly string _settingsPath;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<AppLanguage>? OnLanguageChanged;

    public AppLanguage CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage == value) return;
            _currentLanguage = value;
            
            Translations.SetLanguageWithoutNotify(_currentLanguage);
            SaveSettings();
            
            if (Dispatcher.UIThread.CheckAccess())
            {
                NotifyAll();
            }
            else
            {
                Dispatcher.UIThread.InvokeAsync(NotifyAll);
            }
        }
    }

    public static readonly string[] AvailableLanguageNames =
	[
		"O'zbekcha (Lotin)",
        "Ўзбекча (Кирил)",
        "Русский",
        "English"
    ];

    private LanguageManager()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folder = Path.Combine(appData, "OptiVis");
        Directory.CreateDirectory(folder);
        _settingsPath = Path.Combine(folder, "settings.json");
        LoadSettings();
        Translations.SetLanguageWithoutNotify(_currentLanguage);
    }

    private void NotifyAll()
    {
        LocalizationSource.Instance.NotifyLanguageChanged();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
        OnLanguageChanged?.Invoke(_currentLanguage);
    }

    public void Initialize()
    {
        LoadSettings();
        Translations.SetLanguageWithoutNotify(_currentLanguage);
    }

    public static AppLanguage FromDisplayName(string displayName) => displayName switch
    {
        "O'zbekcha (Lotin)" => AppLanguage.UzbekLatin,
        "Ўзбекча (Кирил)" => AppLanguage.UzbekCyrillic,
        "Русский" => AppLanguage.Russian,
        "English" => AppLanguage.English,
        _ => AppLanguage.UzbekLatin
    };

    public static string ToDisplayName(AppLanguage language) => language switch
    {
        AppLanguage.UzbekLatin => "O'zbekcha (Lotin)",
        AppLanguage.UzbekCyrillic => "Ўзбекча (Кирил)",
        AppLanguage.Russian => "Русский",
        AppLanguage.English => "English",
        _ => "O'zbekcha (Lotin)"
    };

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);
                if (settings != null && Enum.TryParse<AppLanguage>(settings.Language, out var lang))
                {
                    _currentLanguage = lang;
                }
            }
        }
        catch
        {
            _currentLanguage = AppLanguage.UzbekLatin;
        }
    }

    private void SaveSettings()
    {
        try
        {
            var settings = LoadFullSettings();
            settings.Language = _currentLanguage.ToString();
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsPath, json);
        }
        catch
        {
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
}
