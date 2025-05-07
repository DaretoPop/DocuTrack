using Avalonia.Controls;

namespace DocuTrack.Views
{
    public partial class SelectCertificateVersionView : Window
    {
        public SelectCertificateVersionView()
        {
            InitializeComponent();
        }

        public bool Result { get; private set; }

    }
}