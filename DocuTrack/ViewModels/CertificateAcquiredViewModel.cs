using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.DataModels;
using DocuTrack.Pages;

namespace DocuTrack.ViewModels
{
    public partial class CertificateAcquiredViewModel : BaseViewModel
    {

        public CertificateAcquiredViewModel(){}

        [ObservableProperty]
        public string test = "test from CERTACQ";
        
    }
}