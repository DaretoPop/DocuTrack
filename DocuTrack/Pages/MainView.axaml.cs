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
   
    //  Register window with scaling provider (initially with null VM, or delay)
    ScalingProvider.Register(this, null);

    //  Get scaling manager
    var manager = ScalingProvider.GetInstance<MainWindow>();

    //  Create VM with injected manager
    var viewModel = new MainViewModel(manager);

    // Register again with correct VM (optional if needed by your design)
    ScalingProvider.Register(this, viewModel);

    //  Assign DataContext here
    this.DataContext = viewModel;

        }

        

        public override event EventHandler<PointerPressedEventArgs> BeginResize;

        public override event EventHandler<PointerEventArgs> Resize;

        public override event EventHandler<PointerReleasedEventArgs> EndResize;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            WindowState = WindowState.Normal;
            StackPanel bottomRightCorner = this.FindControl<StackPanel>("BottomRightCorner"); // Change this to StackPanel
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

