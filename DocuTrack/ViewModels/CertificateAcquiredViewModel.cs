using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Pages;

namespace DocuTrack.ViewModels
{
    public partial class CertificateAcquiredViewModel : BaseViewModel
    {

        

        [ObservableProperty]
        public string test = "test from CERTACQ";

        [ObservableProperty]
        private ObservableCollection<CertificateType> certificateTypes = new();

        [ObservableProperty]
        private Certificate certificate = new();

        [ObservableProperty]
        private CertificateType? selectedCertificateType;

        public CertificateAcquiredViewModel(Certificate cert)
        {
            if (cert != null)
            {
                Certificate = cert;
                //SelectedCertificateType = DatabaseHelper.GetCertificateTypeByID(cert.CertificateTypeID);
            }
            else
            {
                Certificate = new Certificate();
                SelectedCertificateType = new CertificateType();
            }
            CertificateTypes = new ObservableCollection<CertificateType>(DatabaseHelper.geCertificateTypes());
        }

    }
}