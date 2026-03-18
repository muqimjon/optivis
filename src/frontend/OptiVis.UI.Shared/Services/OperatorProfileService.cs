using System.Text.Json;
using OptiVis.UI.Shared.Models;

namespace OptiVis.UI.Shared.Services;

public interface IOperatorProfileService
{
    Task<string> GetOperatorNameAsync(string extension);
    Task<OperatorProfile?> GetProfileAsync(string extension);
    Task<Dictionary<string, OperatorProfile>> GetAllProfilesAsync();
    Task SaveProfileAsync(OperatorProfile profile);
    Task<string> GetInitialsAsync(string extension);
    Task<string?> FindExtensionByMobileAsync(string mobileNumber);
    Task RemoveProfileAsync(string extension);

    // Eski interface metodi — compatibility
    Task UpdateOperatorNameAsync(string extension, string name);
}

/// <summary>
/// Operator profillarini local faylda saqlaydi: %AppData%\OptiVis\operators.json
/// Backend bu ma'lumotlarni bilmaydi — nomlar, mobil raqamlar faqat shu xizmatda.
/// </summary>
public class LocalOperatorProfileService : IOperatorProfileService
{
    private readonly string _filePath;
    private Dictionary<string, OperatorProfile> _cache = new();
    private bool _loaded = false;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private static readonly string[] AvatarColors =
    [
        "#3B82F6", "#8B5CF6", "#10B981", "#F59E0B",
        "#EF4444", "#06B6D4", "#84CC16", "#F97316"
    ];

    public LocalOperatorProfileService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folder = Path.Combine(appData, "OptiVis");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, "operators.json");
    }

    public static string GetAvatarColor(string extension)
    {
        var num = int.TryParse(extension.TrimStart('0'), out var n) ? n : Math.Abs(extension.GetHashCode());
        return AvatarColors[Math.Abs(num) % AvatarColors.Length];
    }

    private async Task LoadAsync()
    {
        if (_loaded) return;

        await _lock.WaitAsync();
        try
        {
            if (_loaded) return; // double-check

            if (File.Exists(_filePath))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(_filePath);

                    // Eski format (Dictionary<string, string>) ni ham qabul qilish
                    if (json.TrimStart().StartsWith('{') && json.Contains("\"Extension\""))
                    {
                        _cache = JsonSerializer.Deserialize<Dictionary<string, OperatorProfile>>(json)
                                 ?? new();
                    }
                    else
                    {
                        // Eski format: { "1001": "Ism Familiya" }
                        var oldFormat = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                        if (oldFormat != null)
                        {
                            _cache = oldFormat.ToDictionary(
                                kvp => kvp.Key,
                                kvp => new OperatorProfile { Extension = kvp.Key, Name = kvp.Value }
                            );
                            await SaveAsync();
                        }
                    }
                }
                catch
                {
                    _cache = new();
                }
            }
            else
            {
                _cache = new();
                await SaveAsync();
            }
            _loaded = true;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_cache, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<string> GetOperatorNameAsync(string extension)
    {
        await LoadAsync();
        return _cache.TryGetValue(extension, out var p) && !string.IsNullOrEmpty(p.Name)
            ? p.Name
            : $"Ext {extension}";
    }

    public async Task<OperatorProfile?> GetProfileAsync(string extension)
    {
        await LoadAsync();
        return _cache.TryGetValue(extension, out var p) ? p : null;
    }

    public async Task<Dictionary<string, OperatorProfile>> GetAllProfilesAsync()
    {
        await LoadAsync();
        return new Dictionary<string, OperatorProfile>(_cache);
    }

    public async Task SaveProfileAsync(OperatorProfile profile)
    {
        await LoadAsync();
        _cache[profile.Extension] = profile;
        await SaveAsync();
    }

    public async Task UpdateOperatorNameAsync(string extension, string name)
    {
        await LoadAsync();
        if (_cache.TryGetValue(extension, out var existing))
            existing.Name = name;
        else
            _cache[extension] = new OperatorProfile { Extension = extension, Name = name };
        await SaveAsync();
    }

    public async Task<string> GetInitialsAsync(string extension)
    {
        var profile = await GetProfileAsync(extension);
        if (profile == null || string.IsNullOrWhiteSpace(profile.Name))
            return extension.Length <= 2 ? extension.ToUpper() : extension[^2..].ToUpper();

        var parts = profile.Name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[1][0]}".ToUpper()
            : parts[0][..Math.Min(2, parts[0].Length)].ToUpper();
    }

    public async Task<string?> FindExtensionByMobileAsync(string mobileNumber)
    {
        await LoadAsync();
        return _cache.Values
            .FirstOrDefault(p => p.MobileNumber == mobileNumber)
            ?.Extension;
    }

    public async Task RemoveProfileAsync(string extension)
    {
        await LoadAsync();
        if (_cache.Remove(extension))
        {
            await SaveAsync();
        }
    }
}
