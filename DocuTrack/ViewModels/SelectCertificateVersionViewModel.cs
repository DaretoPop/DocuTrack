using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.DataModels;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class SelectCertificateVersionViewModel : BaseViewModel
    {
        public ObservableCollection<Certificate> Versions { get; set; }
        [ObservableProperty] private Certificate? selectedVersion;

        public SelectCertificateVersionViewModel(List<Certificate> versions)
        {
            Versions = new ObservableCollection<Certificate>(versions);
        }

        [RelayCommand]
        private void ConfirmSelection(Window window)
        {
            if (SelectedVersion != null)
            {
                // Close the window and return the selected version
                window.Close();
            }
        }
    }
}