using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Pages;
using DocuTrack.Views;

namespace DocuTrack.ViewModels {
    public partial class ExpirationsViewModel : BaseViewModel
    {

        public ObservableCollection<Certificate> Certificates { get; set; } = new();
        [ObservableProperty] private Certificate? _selectedCertificate = null;

        public ExpirationsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Certificates = new ObservableCollection<Certificate>(DatabaseHelper.GetCertificatesWhichWillExpire());


        }


          [ObservableProperty]
           private  MainViewModel? _mainViewModel;


        [RelayCommand]
        public void GoToProfilPanel()
        {
            if(MainViewModel != null) {
                    MainViewModel.GoToPomorciPanel();

                        if(MainViewModel?.IsPocetnaView != null)
                        {
                            MainViewModel.IsPocetnaView = true;
                        }
            }
        }


        partial void OnSelectedCertificateChanged(Certificate? value)
        {

            if (MainViewModel != null)
            {
                MainViewModel.CurrentPage = new SailorViewModel(MainViewModel, value.Sailor, value);
            }
        }
    }
}