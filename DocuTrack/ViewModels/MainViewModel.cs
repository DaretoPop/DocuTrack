using Avalonia;
using Avalonia.Controls;
using Avalonia.Remote.Protocol.Designer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
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
            KreirajKorisnikaPage = new UpsertSailorViewModel();
            DokumentaPage = new CertificateTypesViewModel();
            PomorciPage = new SailorsViewModel(this);
            ProfilPomorcaPanelPage = new SailorViewModel(this);



            //temp
            CurrentPage = PomorciPage;
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
        [ObservableProperty] private BaseViewModel? _dokumentaPage;



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
        private void GoToKreirajKorisnika()
        {
            CurrentPage = KreirajKorisnikaPage;
        }

        [RelayCommand]
        private void GoToDokumenta()
        {
            CurrentPage = DokumentaPage;
        }

        [RelayCommand]
        private void Exit(Window window)
        {
            window.Close(); //! Bad immplementation, try another way
        }


    } // End of class

}