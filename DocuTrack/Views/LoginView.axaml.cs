using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DocuTrack.ViewModels;



namespace DocuTrack.Views;
                
                // Change form usercontrol to window(for preview)
public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        // DataContext = new LoginViewModel(); // -> Handle to MainView 

    }
}