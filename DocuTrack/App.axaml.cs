using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DocuTrack.ViewModels;

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
            // Default State of App on startup
            desktop.MainWindow = new MainWindow();
            {
                DataContext = new MainViewModel(); // This set DataContex! 
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}