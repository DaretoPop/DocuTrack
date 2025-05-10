using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DocuTrack.Views;

namespace DocuTrack;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        DataTemplates.Add(new ViewLocator());
    }

    public override void OnFrameworkInitializationCompleted()
{
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        var mainVM = new MainViewModel();

        desktop.MainWindow = new MainWindow
        {
            DataContext = mainVM // Dyniamlically change states
        };
    }
    base.OnFrameworkInitializationCompleted();
}
}