using Avalonia;
using Avalonia.Controls;
using Avalonia.Remote.Protocol.Designer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels;


public partial class MainViewModel : BaseViewModel

{

public MainViewModel()
{
    // DEFAULT STATE
    CurrentPage             = new LoginViewModel(this);

    PocetnaPage             = new PocetnaViewModel();
    PomorciPage             = new PomorciViewModel(this);
    KreirajKorisnikaPage    = new KreirajPomorcaViewModel();
    SertifikatiPage         = new SertifikatiViewModel();
    DokumentaPage           = new DokumentiViewModel();
    
    IsLoggedIn = false;
}


    // DataContex
[ObservableProperty] private string app ="DocuTrack";
[ObservableProperty] private string pocetna ="Pocetna";
[ObservableProperty] private string pomorci ="Pomorci";
[ObservableProperty] private string kreirajKorisnika ="Kreiraj Pomorca";
[ObservableProperty] private string dokumenta ="Dokumenta";
[ObservableProperty] private string sertifikati ="Sertifikati";
[ObservableProperty] private string izadji ="Izadji"; //todo: app.exit(izadji button)



// Pages/States Members
// Same as GameState* -> Pokazuje na sve klase koje su nasledile BaseViewModel klasu

[ObservableProperty] private BaseViewModel? _currentPage;
[ObservableProperty] private  PocetnaViewModel? _pocetnaPage;
[ObservableProperty] private PomorciViewModel? _pomorciPage;
[ObservableProperty] private KreirajPomorcaViewModel? _kreirajKorisnikaPage;
[ObservableProperty] private BaseViewModel? _loginPage;
[ObservableProperty] private BaseViewModel? _sertifikatiPage;
[ObservableProperty] private BaseViewModel? _dokumentaPage;



[ObservableProperty] private bool  _isLoggedIn;
[ObservableProperty] private bool _isVisable;



// Change States

[RelayCommand]
    private void GoToLogin() //todo: Delete later;
{
   IsLoggedIn = false;
}
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
    private void GoToPomorci()
{
    CurrentPage = PomorciPage;
}
[RelayCommand]
    private void GoToKreirajKorisnika()
{
    CurrentPage = KreirajKorisnikaPage;
}
[RelayCommand]
    private void GoToSertifikati()
{
    CurrentPage = SertifikatiPage;
    
}

[RelayCommand]
    private void GoToDokumenta()
{
    CurrentPage = DokumentaPage;
}

[RelayCommand]
    private void Exit(Window window)
    {
        window.Close();
    }


};

                    //* moze i lambda, nice :O        
// [RelayCommand]
// private void GoToPocetna() => CurrentPage = _pocetnaPage;
