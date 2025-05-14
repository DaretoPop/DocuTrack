using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DocuTrack.Views;
using DocuTrack.Services;
using System;
using System.Timers;            

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
                // 1) Initial check at startup
            if (TrialManager.IsTrialExpired())
            {
                Console.WriteLine("⚠️ Trial Version expired at startup.");
                Environment.Exit(0);
            }

            // 2) Set up a timer to re-check after 1 minute
            _trialTimer = new Timer(60_000)  // 60,000 ms = 1 minute
            {
                AutoReset = false,            // or true if you want to check repeatedly
                Enabled = true
            };
            _trialTimer.Elapsed += (s, e) =>
            {
                if (TrialManager.IsTrialExpired())
                {
                    Console.WriteLine("⚠️ Trial Version expired.");
                    
                    Environment.Exit(0);
                }
            };


    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        desktop.MainWindow = new MainWindow(); 
    }
    base.OnFrameworkInitializationCompleted();
}
}