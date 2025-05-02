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
        private void DeleteFile()
        {
            if (_requestFile != null)
            {
                // Remove the file from the collection and delete it from storage
                // Example: Notify the parent view model to handle deletion
                //ParentViewModel?.DeleteRequestFile(file);
            }
        }











    }
}