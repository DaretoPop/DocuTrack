using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Pages;
using DocuTrack.Views;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DocuTrack.ViewModels
{
    public partial class RequestFileViewModel : BaseViewModel

    {
        [ObservableProperty] private RequestFile? _requestFile;
       
        
        public RequestFileViewModel(MainViewModel mainViewModel, RequestFile file)
        {
            MainViewModel = mainViewModel;
            _requestFile = file;
        }


        [ObservableProperty] private MainViewModel _mainViewModel;


        public string FilePath
        {
            get { return _requestFile?.FilePath ?? string.Empty; }
            set { _requestFile.FilePath = value; }
        }

        public string FileName
        {
            get { return _requestFile?.FileName ?? string.Empty; }
            set { _requestFile.FileName = value; }
        }



        [RelayCommand]
        private void OpenFile()
        {
            if (_requestFile != null && !string.IsNullOrEmpty(_requestFile.FilePath))
            {
                // Open the file using the default application
                Process.Start(new ProcessStartInfo
                {
                    FileName = _requestFile.FilePath,
                    UseShellExecute = true
                });
            }
        }

        [RelayCommand]
        private async Task  DeleteFile(Window parentWindow)
        {
            


            if (MainViewModel == null || _requestFile == null)
                return;

            var dialog = new ConfirmationDialog
            {
                Message = $"Are you sure you want to delete {_requestFile.FileName}?"
            };

            // Show the dialog as a modal window
            var result = await dialog.ShowDialog<bool>(parentWindow);

            if (result)
            {

                DatabaseHelper.deleteCertificateTypeDocument(this._requestFile);
                MainViewModel.ReinitializeCertificateType();
            }
        }











    }
}