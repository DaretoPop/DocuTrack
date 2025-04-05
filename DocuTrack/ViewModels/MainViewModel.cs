using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels;





public partial class MainViewModel : BaseViewModel

{
    // DataContex
[ObservableProperty] private string app ="DocuTrack";
[ObservableProperty] private string pocetna ="Pocetna";
[ObservableProperty] private string pomorci ="Pomorci";
[ObservableProperty] private string dodajKorisnika ="Dodaj Korisnika";
[ObservableProperty] private string dokumenta ="Dokumenta";
[ObservableProperty] private string sertifikati ="Sertifikati";
[ObservableProperty] private string izadji ="Izadji";



// States

// Same as GameState* -> Pokazuje na sve klase koje su nasledile BaseViewModel klasu
[ObservableProperty]
private BaseViewModel? _currentPage;

private readonly PocetnaViewModel _pocetnaPage = new PocetnaViewModel();
private readonly PomorciViewModel _pomorciPage = new PomorciViewModel();


public MainViewModel()
{
    CurrentPage = _pocetnaPage;
}

};