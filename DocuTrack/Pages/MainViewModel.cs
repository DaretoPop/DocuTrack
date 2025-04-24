using Avalonia;
using Avalonia.Controls;
using Avalonia.Remote.Protocol.Designer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.DataModels;
using DocuTrack.Pages;
using DocuTrack.ViewModels;

namespace DocuTrack.Pages
{
    public partial class MainViewModel : BaseViewModel
    {

        public MainViewModel()
        {
            //Default CurrentPage State
            IsLoggedIn = false;
            IsPocetnaView = false;
            CurrentPage = new LoginViewModel(this);

            PocetnaPage = new ExpirationsViewModel(this);
            KreirajKorisnikaPage = new UpsertSailorViewModel(this, null);
            PomorciPage = new SailorsViewModel(this);
            ProfilPomorcaPanelPage = new SailorViewModel(this, null);
            AddNewDocumentPage = new UpsertCertificateTypeViewModel(this, null);

            SailorsPage = new SailorsViewModel(this);
            CertificateTypesPage = new CertificateTypesViewModel(this);



            //temp
            CurrentPage = PocetnaPage;
            IsLoggedIn = true;

        }


        // DataContex
        [ObservableProperty] private string app = "DocuTrack";
        [ObservableProperty] private string pocetna = "• Pocetna";
        [ObservableProperty] private string pomorci = "• Pomorac";
        [ObservableProperty] private string kreirajKorisnika = "Kreiraj Pomorca";
        [ObservableProperty] private string dokumenta = "• Tip Dokumenta";
        [ObservableProperty] private string sertifikati = "Sertifikati";
        [ObservableProperty] private string izadji = "• Izadji"; //todo: app.exit(izadji button)


// Pages/States Members

        [ObservableProperty] private BaseViewModel? _currentPage;
        [ObservableProperty] private ExpirationsViewModel? _pocetnaPage;
        [ObservableProperty] private SailorsViewModel? _pomorciPage;
        [ObservableProperty] private BaseViewModel? _profilPomorcaPanelPage;
        [ObservableProperty] private UpsertSailorViewModel? _kreirajKorisnikaPage;
        [ObservableProperty] private BaseViewModel? _loginPage;
        [ObservableProperty] private BaseViewModel? _addNewDocumentPage;


        [ObservableProperty] private BaseViewModel? _sailorsPage;
        [ObservableProperty] private BaseViewModel? _certificateTypesPage;



        [ObservableProperty] private bool _isLoggedIn;
        [ObservableProperty] private bool _isVisable;
        [ObservableProperty] private bool isPocetnaView;




// Change States

        public void OnLoginSuccessful()
        {
            CurrentPage = PocetnaPage;
            IsLoggedIn = true;
        }

        [RelayCommand]
        private void GoToPocetna()
        {
            CurrentPage = PocetnaPage;
        }

        [RelayCommand]
        public void GoToPomorci()
        {
            CurrentPage = PomorciPage;

        }

        [RelayCommand]
        public void GoToPomorciPanel()
        {
            CurrentPage = ProfilPomorcaPanelPage;
        }

        [RelayCommand]
        public void GoToSailors()
        {
            CurrentPage = SailorsPage;
        }


        public void ReinitializeCertificateTypes()
        {
            CertificateTypesPage = new CertificateTypesViewModel(this);
        }

        public void GoToCertificateType(CertificateType certificateType)
        {
            CurrentPage = new CertificateTypeViewModel(this, certificateType);
        }

        public void GoToUpsertCertificateType(CertificateType certificateType )
        {
            CurrentPage = new UpsertCertificateTypeViewModel(this, certificateType);
        }

        public void ReinitializeSailors()
        {
            SailorsPage = new SailorsViewModel(this);
        }
        public void GoToSailorsAndReinitialize()
        {
            ReinitializeSailors();
            CurrentPage = SailorsPage;
        }



        [RelayCommand]
        public void GoToAddEditSailorPage(Sailor? sailor = null)
        {
            CurrentPage = new UpsertSailorViewModel(this, sailor);
        }

        [RelayCommand]
        public void GoToSailorPage(Sailor sailor)
        {
            CurrentPage = new SailorViewModel(this, sailor);
        }

        [RelayCommand]
        private void GoToKreirajKorisnika()
        {
            CurrentPage = new UpsertSailorViewModel(this, null);
        }

        [RelayCommand]
        public void GoToDokumenta()
        {
            CurrentPage = CertificateTypesPage;
        }

        [RelayCommand]
        private void Exit(Window window)
        {
            window.Close(); //! Bad immplementation, try another way
        }


    } // End of class

}