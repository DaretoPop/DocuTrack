using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Remote.Protocol.Designer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.DataModels;
using DocuTrack.Views;
using DocuTrack.ViewModels;
using DocuTrack.UI.ViewModels;
using DocuTrack.UI.Scaling;
using System.ComponentModel;
using System.Timers;
using DocuTrack.Services;


namespace DocuTrack.Views
{
    public partial class MainViewModel : BaseViewModel
    {

            //SET TIMER 
            
            public event PropertyChangedEventHandler? PropertyChanged;
            private readonly Timer _uiTimer;
            private TimeSpan _remainingTime;

            public TimeSpan RemainingTime
    {
        get => _remainingTime;
        private set
        {
            if (_remainingTime != value)
            {
                _remainingTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingTime)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingTimeDisplay)));
            }
        }
    }

    
    public string RemainingTimeDisplay =>
        RemainingTime > TimeSpan.Zero
            ? RemainingTime.ToString(@"dd\:hh\:mm")
            : "Expired";


        public MainViewModel(ScalingManager? scalingManager = null)
        {

                // Shpw update after startup APP
            RemainingTime = TrialManager.GetTimeRemaining();

            // set up a timer to tick every second (or whatever interval you like)
            _uiTimer = new Timer(1_000) { AutoReset = true };
            _uiTimer.Elapsed += (s, e) => RemainingTime = TrialManager.GetTimeRemaining();
            _uiTimer.Start();
                

            _scalingManager = scalingManager;


            //Default CurrentPage State
            IsLoggedIn = false;
            IsPocetnaView = false;
            CurrentPage = new LoginViewModel(this);

            PocetnaPage = new ExpirationsViewModel(this);
            KreirajKorisnikaPage = new UpsertSailorViewModel(this, null);
            ProfilPomorcaPanelPage = new SailorViewModel(this, null);
            AddNewDocumentPage = new UpsertCertificateTypeViewModel(this, null);

            SailorsPage = new SailorsViewModel(this);
            CertificateTypesPage = new CertificateTypesViewModel(this);

            //temp
            // CurrentPage = PocetnaPage;
            // IsLoggedIn = true;

        }




        // Scaling properties
        private readonly ScalingManager _scalingManager;
        public int ScalingPercent => (int)(ScalingFactor * 100);
        public double ScalingFactor => _scalingManager.CurrentScaling;

        [RelayCommand]
        private void IncreaseScale()
        {
            const double maxScale = 1.15;  // 115% maximum
            const double increment = 0.05; // 5% increment

            var newScale = _scalingManager.CurrentScaling + increment;

            // Apply upper limit
            if (newScale > maxScale)
            {
                newScale = maxScale;
            }

            _scalingManager.SetScaling(newScale);
            OnPropertyChanged(nameof(ScalingFactor));
            OnPropertyChanged(nameof(ScalingPercent));
        }

        [RelayCommand]
        private void DecreaseScale()
        {
            const double minScale = 0.5;  // 50% minimum
            const double decrement = 0.05; // 5% decrement

            var newScale = _scalingManager.CurrentScaling - decrement;

            // Apply lower limit
            if (newScale < minScale)
            {
                newScale = minScale;
            }

            _scalingManager.SetScaling(newScale);
            OnPropertyChanged(nameof(ScalingFactor));
            OnPropertyChanged(nameof(ScalingPercent));
        }



        // DataContex
        [ObservableProperty] private string app = "DocuTrack";
        [ObservableProperty] private string pocetna = "Pocetna";
        [ObservableProperty] private string pomorci = "Pomorac";
        [ObservableProperty] private string kreirajKorisnika = "Kreiraj Pomorca";
        [ObservableProperty] private string dokumenta = "Dokumenta";
        [ObservableProperty] private string sertifikati = "Sertifikati";
        [ObservableProperty] private string izadji = "• Izadji"; //todo: app.exit(izadji button)


        // Pages/States Members

        [ObservableProperty] private BaseViewModel? _currentPage;
        [ObservableProperty] private ExpirationsViewModel? _pocetnaPage;
        [ObservableProperty] private BaseViewModel? _profilPomorcaPanelPage;
        [ObservableProperty] private UpsertSailorViewModel? _kreirajKorisnikaPage;
        [ObservableProperty] private BaseViewModel? _loginPage;
        [ObservableProperty] private BaseViewModel? _addNewDocumentPage;


        [ObservableProperty] private BaseViewModel? _sailorsPage;
        [ObservableProperty] private BaseViewModel? _certificateTypesPage;
        [ObservableProperty] private CertificateTypeViewModel? _certificateTypePage;



        [ObservableProperty] private bool _isLoggedIn;
        [ObservableProperty] private bool _isVisable;
        [ObservableProperty] private bool isPocetnaView;

        [ObservableProperty] private bool _isPocetnaSelected;
        [ObservableProperty] private bool _isPomorciSelected;
        [ObservableProperty] private bool _isDokumentaSelected;


        // Display arrow 2 based on the navigation
        [RelayCommand]
        private void SetSelectedPage(string page)
        {
            // Restart
            IsPocetnaSelected = false;
            IsPomorciSelected = false;
            IsDokumentaSelected = false;


            if (page == "Pocetna")
            {
                IsPocetnaSelected = true;
            }
            else if (page == "Pomorci")
            {
                IsPomorciSelected = true;
            }
            else if (page == "Dokumenta")
            {
                IsDokumentaSelected = true;
            }
        }

        // Change States

        public void OnLoginSuccessful()
        {
            CurrentPage = new ExpirationsViewModel(this);
            IsLoggedIn = true;
            IsPocetnaSelected = true;
        }

        [RelayCommand]
        private void GoToPocetna()
        {
            CurrentPage = new ExpirationsViewModel(this);
            SetSelectedPage("Pocetna");
        }

        [RelayCommand]
        public void GoToPomorci()
        {
            CurrentPage = new SailorsViewModel(this);
            SetSelectedPage("Pomorci");
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
            CertificateTypePage = new CertificateTypeViewModel(this, certificateType);
            CurrentPage = CertificateTypePage;
        }

        internal void ReinitializeCertificateType()
        {
            CertificateTypePage = new CertificateTypeViewModel(this, CertificateTypePage.CertificateType);
            CurrentPage = CertificateTypePage;
        }

        public void GoToUpsertCertificateType(CertificateType certificateType)
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
            SetSelectedPage("Dokumenta");

        }

        [RelayCommand]
        private void Exit(Window window)
        {
            if (window != null) window.Close(); //! Bad immplementation, try another way
        }

    } // End of class

}