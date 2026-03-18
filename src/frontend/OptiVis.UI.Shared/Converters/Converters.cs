using Avalonia.Data.Converters;
using Avalonia.Media;
using OptiVis.UI.Shared.Models;
using System.Globalization;

namespace OptiVis.UI.Shared.Converters;

public static class BoolConverters
{
    public static FuncMultiValueConverter<bool, bool> And { get; } =
        new FuncMultiValueConverter<bool, bool>(bools => bools.All(b => b));

    public static FuncMultiValueConverter<bool, bool> Or { get; } =
        new FuncMultiValueConverter<bool, bool>(bools => bools.Any(b => b));
}

public class SidebarWidthConverter : IValueConverter
{
    public static readonly SidebarWidthConverter Instance = new();
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true ? 60.0 : 240.0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}

public class SidebarLogoSizeConverter : IValueConverter
{
    public static readonly SidebarLogoSizeConverter Instance = new();
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true ? 32.0 : 40.0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}

public class TimeSpanFormatConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		TimeSpan ts;

		if (value is TimeSpan direct)
		{
			ts = direct;
		}
		else if (value is string text && TimeSpan.TryParse(text, out var parsed))
		{
			ts = parsed;
		}
		else
		{
			return "N/A";
		}

		if (ts.TotalSeconds < 1)
			return "0s";

		if (ts.TotalHours >= 1)
			return $"{(int)ts.TotalHours}h {ts.Minutes}m";
		if (ts.TotalMinutes >= 1)
			return $"{ts.Minutes}m {ts.Seconds}s";
		return $"{ts.Seconds}s";
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}

public class PercentageFormatConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double d)
            return $"{d:F1}%";
        return "0%";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class StatusColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var status = value as string ?? value?.ToString() ?? "";
        return status.ToLowerInvariant() switch
        {
            "answered" or "completed" or "success" => new SolidColorBrush(Color.Parse("#10B981")),
            "noanswer" or "no answer" or "missed" => new SolidColorBrush(Color.Parse("#F59E0B")),
            "busy" => new SolidColorBrush(Color.Parse("#EF4444")),
            "cancel" or "cancelled" or "failed" or "congestion" => new SolidColorBrush(Color.Parse("#6B7280")),
            _ => new SolidColorBrush(Color.Parse("#6B7280"))
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class StatusIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var status = value as string ?? value?.ToString() ?? "";
        return status.ToLowerInvariant() switch
        {
            "answered" or "completed" or "success" => "mdi-phone-check",
            "noanswer" or "no answer" or "missed" => "mdi-phone-missed",
            "busy" => "mdi-phone-off",
            "cancel" or "cancelled" or "congestion" => "mdi-phone-cancel",
            "failed" => "mdi-phone-remove",
            _ => "mdi-phone"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class DirectionIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var direction = value as string ?? "";
        return direction.ToUpperInvariant() switch
        {
            "IN" => "mdi-phone-incoming",
            "OUT" => "mdi-phone-outgoing",
            _ => "mdi-phone"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class DirectionColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var direction = value as string ?? "";
        return direction.ToUpperInvariant() switch
        {
            "IN" => new SolidColorBrush(Color.Parse("#10B981")),
            "OUT" => new SolidColorBrush(Color.Parse("#3B82F6")),
            _ => new SolidColorBrush(Color.Parse("#6B7280"))
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class DurationFormatConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            return $"{ts.Minutes}:{ts.Seconds:D2}";
        }
        return "0:00";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class CallStatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CallStatus status)
        {
            return status switch
            {
                CallStatus.Answered => new SolidColorBrush(Color.Parse("#10B981")),
                CallStatus.NoAnswer => new SolidColorBrush(Color.Parse("#F59E0B")),
                CallStatus.Busy => new SolidColorBrush(Color.Parse("#EF4444")),
                _ => new SolidColorBrush(Color.Parse("#6B7280"))
            };
        }
        return new SolidColorBrush(Color.Parse("#6B7280"));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class BoolToOnlineStatusConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isActive)
            return isActive ? "Online" : "Offline";
        return "Unknown";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class BoolToOnlineColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isActive)
            return isActive ? new SolidColorBrush(Color.Parse("#10B981")) : new SolidColorBrush(Color.Parse("#6B7280"));
        return new SolidColorBrush(Color.Parse("#6B7280"));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class SuccessRateToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double rate)
        {
            return rate switch
            {
                >= 90 => new SolidColorBrush(Color.Parse("#10B981")),
                >= 70 => new SolidColorBrush(Color.Parse("#3B82F6")),
                >= 50 => new SolidColorBrush(Color.Parse("#F59E0B")),
                _ => new SolidColorBrush(Color.Parse("#EF4444"))
            };
        }
        return new SolidColorBrush(Color.Parse("#6B7280"));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class InitialsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string name && !string.IsNullOrWhiteSpace(name))
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return $"{parts[0][0]}{parts[1][0]}".ToUpperInvariant();
            if (parts.Length == 1 && parts[0].Length >= 2)
                return parts[0][..2].ToUpperInvariant();
            if (parts.Length == 1)
                return parts[0][0].ToString().ToUpperInvariant();
        }
        return "??";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class PositiveToVisibleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int count)
            return count > 0;
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

