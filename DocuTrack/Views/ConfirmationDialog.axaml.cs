using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;

namespace DocuTrack.Views
{
    public partial class ConfirmationDialog : Window
    {
        public ConfirmationDialog()
        {
            InitializeComponent();
        }

        public string Message
        {
            get => MessageTextBlock.Text;
            set => MessageTextBlock.Text = value;
        }

        public bool Result { get; private set; }

        private void OnYesClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Result = true;
            Close(true);
        }

        private void OnNoClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}