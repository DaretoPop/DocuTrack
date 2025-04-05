using Avalonia.Controls;
using DocuTrack.ViewModels;

namespace DocuTrack;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
     
    }   

    
        // BUTTON LOGIC foo()

}