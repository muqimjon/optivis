using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Threading;
using OptiVis.UI.Shared.i18n;
using System.ComponentModel;

namespace OptiVis.UI.Shared.Extensions;

public class LocalizeExtension : MarkupExtension
{
    public string Key { get; set; } = string.Empty;

    public LocalizeExtension() { }
    
    public LocalizeExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new ReflectionBindingExtension($"[{Key}]")
        {
            Source = LocalizationSource.Instance,
            Mode = BindingMode.OneWay
        };
        return binding.ProvideValue(serviceProvider);
    }
}

public class LocalizationSource : INotifyPropertyChanged
{
    private static LocalizationSource? _instance;
    private int _version;

    public static LocalizationSource Instance => _instance ??= new LocalizationSource();

    public string this[string key] => Translations.Get(key);

    public int Version => _version;

    public event PropertyChangedEventHandler? PropertyChanged;

    private LocalizationSource()
    {
        Translations.OnLanguageChanged += OnTranslationsLanguageChanged;
    }

    private void OnTranslationsLanguageChanged()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            RaiseAllPropertiesChanged();
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync(RaiseAllPropertiesChanged);
        }
    }

    public void NotifyLanguageChanged()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            RaiseAllPropertiesChanged();
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync(RaiseAllPropertiesChanged);
        }
    }

    private void RaiseAllPropertiesChanged()
    {
        _version++;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Version)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
    }
}
