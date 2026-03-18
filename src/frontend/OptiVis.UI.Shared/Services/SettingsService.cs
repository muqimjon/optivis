using System.Text.Json;

namespace OptiVis.UI.Shared.Services;

public class SettingsService
{
    public const string DefaultBackendUrl = "https://tel-mon.hamrohmmt.uz/";

    private static SettingsService? _instance;
    public static SettingsService Instance => _instance ??= new SettingsService();

    private readonly string _settingsPath;
    private AppSettings _settings;
    private string? _startupWarningMessage;

    public event Action<string>? OnBackendUrlChanged;

    public string BackendUrl
    {
        get => _settings.BackendUrl;
        set => TryUpdateBackendUrl(value, out _, out _);
    }

    public string SignalRHubUrl => BuildSignalRHubUrl(_settings.BackendUrl);

    private SettingsService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folder = Path.Combine(appData, "OptiVis");
        Directory.CreateDirectory(folder);
        _settingsPath = Path.Combine(folder, "settings.json");

        _settings = Load(out var changed);
        if (changed)
        {
            Save();
        }
    }

    public void Initialize()
    {
        _settings = Load(out var changed);
        if (changed)
        {
            Save();
        }
    }

    public bool TryUpdateBackendUrl(string? rawUrl, out string normalizedUrl, out string validationError)
    {
        if (!TryNormalizeBackendUrl(rawUrl, out normalizedUrl, out validationError))
        {
            return false;
        }

        if (string.Equals(_settings.BackendUrl, normalizedUrl, StringComparison.Ordinal))
        {
            validationError = string.Empty;
            return true;
        }

        _settings.BackendUrl = normalizedUrl;
        Save();
        OnBackendUrlChanged?.Invoke(normalizedUrl);

        validationError = string.Empty;
        return true;
    }

    public string? ConsumeStartupWarning()
    {
        var message = _startupWarningMessage;
        _startupWarningMessage = null;
        return message;
    }

    public AppSettings GetSettings() => _settings;

    public void SaveSettings(AppSettings settings)
    {
        _settings = NormalizeSettings(settings ?? new AppSettings(), out _);
        Save();
    }

    public static string BuildSignalRHubUrl(string backendUrl) =>
        backendUrl.TrimEnd('/') + "/hubs/dashboard";

    public static bool TryNormalizeBackendUrl(string? rawUrl, out string normalizedUrl, out string validationError)
    {
        normalizedUrl = DefaultBackendUrl;
        validationError = string.Empty;

        if (string.IsNullOrWhiteSpace(rawUrl))
        {
            validationError = "Backend URL bo'sh. To'liq manzil kiriting (https://example.com/).";
            return false;
        }

        var candidate = rawUrl.Trim();
        if (!candidate.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !candidate.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            candidate = "https://" + candidate;
        }

        if (!Uri.TryCreate(candidate, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
            string.IsNullOrWhiteSpace(uri.Host))
        {
            validationError = "Backend URL noto'g'ri. To'liq manzil kiriting (http:// yoki https:// bilan).";
            return false;
        }

        normalizedUrl = uri.ToString().TrimEnd('/') + "/";
        return true;
    }

    private AppSettings Load(out bool changed)
    {
        changed = false;

        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = File.ReadAllText(_settingsPath);
                var loaded = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                return NormalizeSettings(loaded, out changed);
            }
        }
        catch
        {
            changed = true;
            _startupWarningMessage = "settings.json o'qishda xatolik bo'ldi. Standart sozlamalar qo'llandi.";
        }

        return NormalizeSettings(new AppSettings(), out changed);
    }

    private AppSettings NormalizeSettings(AppSettings settings, out bool changed)
    {
        changed = false;

        if (!TryNormalizeBackendUrl(settings.BackendUrl, out var normalizedUrl, out _))
        {
            var invalidValue = settings.BackendUrl;
            settings.BackendUrl = DefaultBackendUrl;
            changed = true;
            _startupWarningMessage =
                $"Noto'g'ri Backend URL aniqlandi ({invalidValue}). Standart manzil tiklandi: {DefaultBackendUrl}";
            return settings;
        }

        if (!string.Equals(settings.BackendUrl, normalizedUrl, StringComparison.Ordinal))
        {
            settings.BackendUrl = normalizedUrl;
            changed = true;
        }

        return settings;
    }

    private void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsPath, json);
        }
        catch
        {
        }
    }
}
