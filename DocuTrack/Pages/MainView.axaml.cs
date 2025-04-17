using Avalonia.Controls;
using DocuTrack.ViewModels;

namespace DocuTrack.Pages
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

        }


        // BUTTON LOGIC foo()

    }
}