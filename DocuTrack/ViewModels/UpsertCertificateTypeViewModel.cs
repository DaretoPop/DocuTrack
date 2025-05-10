using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class UpsertCertificateTypeViewModel : BaseViewModel

    {

        public UpsertCertificateTypeViewModel(MainViewModel mainViewModel, CertificateType type)
        {
            MainViewModel = mainViewModel;

            if (type == null)
            {
                type = new CertificateType();
            }
            _certificateType = type;

            BindScaling(mainViewModel);
        }



        [ObservableProperty] private MainViewModel _mainViewModel;
        

        private CertificateType _certificateType = new CertificateType();

        public string UpsertTitle  => _certificateType?.ID == 0 ? "Add Certificate Type" : "Edit Certificate Type";
        public bool IsNew => _certificateType.ID != 0;

        public string Naziv
        {
            get
            {
                return _certificateType?.Name;
            }
            set
            {
                _certificateType.Name = value;
            }
        }



        [RelayCommand] 
        private void GoToBack()
        {
            if(MainViewModel != null)
            {
                MainViewModel.GoToDokumenta();
            }
        }

        [ObservableProperty] private string errorMessage = string.Empty;

        [RelayCommand]
        private void Save()
        {
            if (_certificateType?.Name?.Length == 0 )
            {
                ErrorMessage = "Ime je obavezno";
                return;
            }
            ErrorMessage = string.Empty;
            try
            {
                if (_certificateType.ID == 0)
                {
                    var certType = DatabaseHelper.addCertificateType(_certificateType);
                    MainViewModel.ReinitializeCertificateTypes();
                    MainViewModel.GoToCertificateType(certType);
                }
                else
                {
                    DatabaseHelper.updateCertificateType(_certificateType);
                    MainViewModel.ReinitializeCertificateTypes();

                    MainViewModel.GoToCertificateType(_certificateType);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        [RelayCommand]
        private async Task Delete(Window parentWindow)
        {
            if (MainViewModel == null || _certificateType == null)
                return;

            var dialog = new ConfirmationDialog
            {
                Message = $"Are you sure you want to delete {_certificateType.Name}?"
            };

            // Show the dialog as a modal window
            var result = await dialog.ShowDialog<bool>(parentWindow);

            if (result)
            {

                DatabaseHelper.deleteCertificateType(_certificateType);
                MainViewModel.ReinitializeCertificateTypes();
                MainViewModel.GoToDokumenta();
            }
        }


    }
}