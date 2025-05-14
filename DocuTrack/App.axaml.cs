using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DocuTrack.Views;
using DocuTrack.Services;
using System;
using System.Timers;
using Avalonia.Threading;

namespace DocuTrack;

public partial class App : Application
{
    private Timer? _trialTimer;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        DataTemplates.Add(new ViewLocator());
    }

public override void OnFrameworkInitializationCompleted()
{
    //  at startup
    if (TrialManager.IsTrialExpired())
    {
        HandleTrialExpiration();
        return; // posle expired!
    }

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        desktop.MainWindow = new MainWindow();
    }
        //Set timer -> provera svakih sekund da li je TrialExpired;
    _trialTimer = new Timer(1_000) 
    {
         AutoReset = true,  
         Enabled = true
    };
    _trialTimer.Elapsed += (s, e) =>
    {
        if (TrialManager.IsTrialExpired())
        {
            HandleTrialExpiration();
        }
    };

    base.OnFrameworkInitializationCompleted();
}

    private void HandleTrialExpiration()
{
    Console.WriteLine(" Trial expired. Deleting database...");
    TrialManager.DeleteTrialData();
    TrialManager.DeleteDatabase();

    // shut down the app after expired !!!
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        Dispatcher.UIThread.Post(() =>
        {
            desktop.Shutdown();
        });
    }
}

}