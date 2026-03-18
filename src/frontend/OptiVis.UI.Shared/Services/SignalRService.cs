using Microsoft.AspNetCore.SignalR.Client;
using OptiVis.UI.Shared.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OptiVis.UI.Shared.Services;

public interface ISignalRService : IAsyncDisposable
{
    event Action<CallRecord>? OnNewCall;
    event Action<DashboardSummary>? OnDashboardUpdate;
    event Action<List<OperatorStats>>? OnOperatorStatsUpdate;
    event Action<bool>? OnConnectionChanged;
    
    bool IsConnected { get; }
    Task ConnectAsync();
    Task DisconnectAsync();
}

public class SignalRService : ISignalRService
{
    private readonly HubConnection _connection;
    private readonly JsonSerializerOptions _jsonOptions;

    public event Action<CallRecord>? OnNewCall;
    public event Action<DashboardSummary>? OnDashboardUpdate;
    public event Action<List<OperatorStats>>? OnOperatorStatsUpdate;
    public event Action<bool>? OnConnectionChanged;

    public bool IsConnected => _connection.State == HubConnectionState.Connected;

    public SignalRService(string hubUrl)
    {
        Console.WriteLine($"[SignalR] Initializing with URL: {hubUrl}");
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(), new TimeSpanConverter() }
        };

        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _connection.On<object>("ReceiveNewCall", data =>
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [SignalR] ReceiveNewCall event received!");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] ReceiveNewCall event received");
            
            try
            {
                var json = JsonSerializer.Serialize(data);
                Console.WriteLine($"[{timestamp}] [SignalR] Raw data: {json}");
                
                var call = JsonSerializer.Deserialize<CallRecord>(json, _jsonOptions);
                if (call != null)
                {
                    Console.WriteLine($"[{timestamp}] [SignalR] New call: {call.Src} -> {call.Dst} at {call.CallDate}");
                    System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] New call: {call.Src} -> {call.Dst}");
                    OnNewCall?.Invoke(call);
                    Console.WriteLine($"[{timestamp}] [SignalR] OnNewCall invoked, subscribers: {OnNewCall?.GetInvocationList().Length ?? 0}");
                }
                else
                {
                    Console.WriteLine($"[{timestamp}] [SignalR] Failed to deserialize call");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{timestamp}] [SignalR] Error processing ReceiveNewCall: {ex.Message}");
            }
        });

        _connection.On<object>("ReceiveDashboardUpdate", data =>
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [SignalR] ReceiveDashboardUpdate event received!");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] ReceiveDashboardUpdate event received");
            
            try
            {
                var json = JsonSerializer.Serialize(data);
                var summary = JsonSerializer.Deserialize<DashboardSummary>(json, _jsonOptions);
                if (summary != null)
                {
                    Console.WriteLine($"[{timestamp}] [SignalR] Dashboard update: {summary.TotalCalls} calls");
                    System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Dashboard update: {summary.TotalCalls} calls");
                    OnDashboardUpdate?.Invoke(summary);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{timestamp}] [SignalR] Error processing ReceiveDashboardUpdate: {ex.Message}");
            }
        });

        _connection.On<object>("ReceiveOperatorStatsUpdate", data =>
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [SignalR] ReceiveOperatorStatsUpdate event received!");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] ReceiveOperatorStatsUpdate event received");
            
            try
            {
                var json = JsonSerializer.Serialize(data);
                var stats = JsonSerializer.Deserialize<List<OperatorStats>>(json, _jsonOptions);
                if (stats != null)
                {
                    Console.WriteLine($"[{timestamp}] [SignalR] Operator stats: {stats.Count} operators");
                    System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Operator stats: {stats.Count} operators");
                    OnOperatorStatsUpdate?.Invoke(stats);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{timestamp}] [SignalR] Error processing ReceiveOperatorStatsUpdate: {ex.Message}");
            }
        });

        _connection.Closed += async error =>
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [SignalR] Connection closed: {error?.Message}");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Connection closed: {error?.Message}");
            OnConnectionChanged?.Invoke(false);
        };

        _connection.Reconnected += connectionId =>
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [SignalR] Reconnected: {connectionId}");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Reconnected: {connectionId}");
            OnConnectionChanged?.Invoke(true);
            return Task.CompletedTask;
        };

        _connection.Reconnecting += error =>
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [SignalR] Reconnecting: {error?.Message}");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Reconnecting: {error?.Message}");
            OnConnectionChanged?.Invoke(false);
            return Task.CompletedTask;
        };
    }

    public async Task ConnectAsync()
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        
        if (_connection.State == HubConnectionState.Connected || _connection.State == HubConnectionState.Connecting)
        {
            Console.WriteLine($"[{timestamp}] [SignalR] Already connected or connecting. State: {_connection.State}");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Already connected or connecting");
            return;
        }

        try
        {
            Console.WriteLine($"[{timestamp}] [SignalR] Connecting to hub...");
            Console.WriteLine($"[{timestamp}] [SignalR] Current state: {_connection.State}");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Connecting to hub...");
            
            await _connection.StartAsync();
            
            timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [SignalR] ✅ Connected successfully!");
            Console.WriteLine($"[{timestamp}] [SignalR] Connection ID: {_connection.ConnectionId}");
            Console.WriteLine($"[{timestamp}] [SignalR] State: {_connection.State}");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Connected successfully!");
            
            OnConnectionChanged?.Invoke(true);
        }
        catch (Exception ex)
        {
            timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [SignalR] ❌ Connection failed: {ex.Message}");
            Console.WriteLine($"[{timestamp}] [SignalR] Stack trace: {ex.StackTrace}");
            System.Diagnostics.Debug.WriteLine($"[{timestamp}] [SignalR] Connection failed: {ex.Message}");
            OnConnectionChanged?.Invoke(false);
        }
    }

    public async Task DisconnectAsync()
    {
        System.Diagnostics.Debug.WriteLine($"[SignalR] Disconnecting...");
        await _connection.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}
