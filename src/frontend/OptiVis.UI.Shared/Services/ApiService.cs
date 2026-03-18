using OptiVis.UI.Shared.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OptiVis.UI.Shared.Services;

public interface IApiService
{
    Task<DashboardSummary?> GetDashboardSummaryAsync(DateTime from, DateTime to);
    Task<List<HourlyCalls>?> GetHourlyCallsAsync(DateTime from, DateTime to);
    Task<List<TrendPoint>?> GetCallTrendAsync(DateTime from, DateTime to);
    Task<List<OperatorStats>?> GetOperatorStatsAsync(DateTime from, DateTime to);
    Task<List<string>?> GetActiveExtensionsAsync();
    Task<List<CallSearchResult>?> SearchCallsAsync(string number, DateTime from, DateTime to);
    Task<List<CallLog>?> GetCallLogsAsync(DateTime from, DateTime to, string? number = null);
    Task<List<CallRecord>?> GetRecentCallsAsync(int count);
    Task<List<IdlePeriod>?> GetOperatorIdlePeriodsAsync(string extension, DateTime from, DateTime to);
    Task<List<PhoneNumberStats>?> GetPhoneNumberStatsAsync(DateTime from, DateTime to);
    Task<List<PhoneNumberCallDetail>?> GetPhoneNumberDetailsAsync(string phoneNumber, DateTime from, DateTime to);
    Task<List<OperatorCallItem>?> GetOperatorCallsAsync(string extension, DateTime from, DateTime to);
    void UpdateBaseUrl(string newUrl);
}

public class ApiService : IApiService
{
    private HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(string baseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = NormalizeBaseAddress(baseUrl) };
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(), new TimeSpanConverter() }
        };
    }

    public void UpdateBaseUrl(string newUrl)
    {
        _httpClient = new HttpClient { BaseAddress = NormalizeBaseAddress(newUrl) };
    }

    private static Uri NormalizeBaseAddress(string rawUrl)
    {
        var candidate = rawUrl?.Trim() ?? string.Empty;
        if (!candidate.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !candidate.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            candidate = "https://" + candidate;
        }

        if (!Uri.TryCreate(candidate, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException(
                $"Backend URL noto'g'ri: '{rawUrl}'. To'liq http:// yoki https:// manzil kiriting.");
        }

        return new Uri(uri.ToString().TrimEnd('/') + "/");
    }

    private static string Fmt(DateTime dt) => Uri.EscapeDataString(dt.ToString("O"));

    public async Task<DashboardSummary?> GetDashboardSummaryAsync(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync($"api/dashboard/summary?from={Fmt(from)}&to={Fmt(to)}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DashboardSummary>(json, _jsonOptions);
    }

    public async Task<List<HourlyCalls>?> GetHourlyCallsAsync(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync($"api/dashboard/hourly?from={Fmt(from)}&to={Fmt(to)}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<HourlyCalls>>(json, _jsonOptions);
    }

    public async Task<List<TrendPoint>?> GetCallTrendAsync(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync($"api/dashboard/trend?from={Fmt(from)}&to={Fmt(to)}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<TrendPoint>>(json, _jsonOptions);
    }

    public async Task<List<OperatorStats>?> GetOperatorStatsAsync(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync($"api/operators/stats?from={Fmt(from)}&to={Fmt(to)}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<OperatorStats>>(json, _jsonOptions);
    }

    public async Task<List<string>?> GetActiveExtensionsAsync()
    {
        var response = await _httpClient.GetAsync("api/operators/active-today");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<string>>(json, _jsonOptions);
    }

    public async Task<List<CallSearchResult>?> SearchCallsAsync(string number, DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync(
            $"api/calls/search?number={Uri.EscapeDataString(number)}&from={Fmt(from)}&to={Fmt(to)}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CallSearchResult>>(json, _jsonOptions);
    }

    public async Task<List<CallLog>?> GetCallLogsAsync(DateTime from, DateTime to, string? number = null)
    {
        var query = $"api/calls/logs?from={Fmt(from)}&to={Fmt(to)}";
        if (!string.IsNullOrWhiteSpace(number))
            query += $"&number={Uri.EscapeDataString(number)}";
        var response = await _httpClient.GetAsync(query);
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CallLog>>(json, _jsonOptions);
    }

    public async Task<List<CallRecord>?> GetRecentCallsAsync(int count)
    {
        var response = await _httpClient.GetAsync($"api/calls/recent?count={count}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CallRecord>>(json, _jsonOptions);
    }

    public async Task<List<IdlePeriod>?> GetOperatorIdlePeriodsAsync(string extension, DateTime from, DateTime to)
    {
        await Task.CompletedTask;
        return new List<IdlePeriod>();
    }

    public async Task<List<PhoneNumberStats>?> GetPhoneNumberStatsAsync(DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync($"api/calls/phone-stats?from={Fmt(from)}&to={Fmt(to)}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<PhoneNumberStats>>(json, _jsonOptions);
    }

    public async Task<List<PhoneNumberCallDetail>?> GetPhoneNumberDetailsAsync(string phoneNumber, DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync(
            $"api/calls/phone-details?number={Uri.EscapeDataString(phoneNumber)}&from={Fmt(from)}&to={Fmt(to)}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<PhoneNumberCallDetail>>(json, _jsonOptions);
    }

    public async Task<List<OperatorCallItem>?> GetOperatorCallsAsync(string extension, DateTime from, DateTime to)
    {
        var response = await _httpClient.GetAsync(
            $"api/operators/{Uri.EscapeDataString(extension)}/calls?from={Fmt(from)}&to={Fmt(to)}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<OperatorCallItem>>(json, _jsonOptions);
    }
}

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        return TimeSpan.TryParse(str, out var ts) ? ts : TimeSpan.Zero;
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}
