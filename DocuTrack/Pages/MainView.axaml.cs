using Avalonia.Controls;
using System;
using DocuTrack.UI.Scaling;
using DocuTrack.ViewModels;
using Avalonia.Input;
using Avalonia.Markup.Xaml;


namespace DocuTrack.Views
{

    public partial class MainWindow : ScalableWindow
     {
       
        public MainWindow()
        {
            InitializeComponent();
            // Create the ViewModel first
        var viewModel = new MainViewModel();
        
        // Register the window with ScalingProvider
        ScalingProvider.Register(this, viewModel); // Ensure MainViewModel implements IViewModel
        
        // Retrieve the ScalingManager instance
        ScalingManager mainWindowManager = ScalingProvider.GetInstance<MainWindow>();
        mainWindowManager.SetScaling(1.5); // Apply scaling here
        
        // Set DataContext AFTER registration
        this.DataContext = viewModel;
        }

        

        public override event EventHandler<PointerPressedEventArgs> BeginResize;

        public override event EventHandler<PointerEventArgs> Resize;

        public override event EventHandler<PointerReleasedEventArgs> EndResize;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            WindowState = WindowState.Normal;
            Border bottomRightCorner = this.FindControl<Border>("BottomRightCorner"); // Change this to border
            bottomRightCorner.PointerMoved += BottomRightCorner_PointerMoved;
            bottomRightCorner.PointerReleased += BottomRightCorner_PointerReleased;
            bottomRightCorner.PointerPressed += BottomRightCorner_PointerPressed;
        }

        private void BottomRightCorner_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            EndResize?.Invoke(this, e);
        }

        private void BottomRightCorner_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            BeginResize?.Invoke(this, e);
        }

        private void BottomRightCorner_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
        {
            Resize?.Invoke(this, e);
        }

        }


    }

