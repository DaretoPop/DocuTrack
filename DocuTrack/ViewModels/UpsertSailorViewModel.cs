using System;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Rendering;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Pages;
using DocuTrack.Views;


namespace DocuTrack.ViewModels {

    public partial class UpsertSailorViewModel : BaseViewModel

    {
        [ObservableProperty] private Sailor? _sailor;
        [ObservableProperty] private MainViewModel _mainViewModel;

        public UpsertSailorViewModel(MainViewModel mainViewModel, Sailor sailor)
        {
            MainViewModel = mainViewModel;
            if (sailor == null)
            {
                _sailor = new Sailor();
            }
            else
            {
                _sailor = sailor;

            }

        }

        public string Name
        {
            get
            {
                return _sailor?.Name;
            }
            set
            {
                _sailor.Name = value;
            }
        }

        public string Surname
        {
            get
            {
                return _sailor?.Surname;
            }
            set
            {
                _sailor.Surname = value;
            }
        }

        public string GID
        {
            get
            {
                return _sailor?.GID;
            }
            set
            {
                _sailor.GID = value;
            }
        }

        public bool IsRefresh
        {
            get
            {
                return _sailor.IsRefresh;
            }
            set
            {
                _sailor.IsRefresh = value;
            }
        }

        [ObservableProperty] private string errorMessage = string.Empty;

       



        [RelayCommand]
        private void GoBack()
        {
            MainViewModel.GoToPomorci();
        }

        [RelayCommand]
        private void Submit()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname) || string.IsNullOrWhiteSpace(GID))
            {
                ErrorMessage = "Sva polja su obavezna.";
                return;
            }
            if (GID.Length != 13)
            {
                ErrorMessage = "JMBG mora imati 13 karaktera.";
                return;
            }
            ErrorMessage = string.Empty;
            try
            {
                if (_sailor.ID == 0)
                {
                    var newSailor = DatabaseHelper.addSailor(_sailor);
                    MainViewModel.GoToSailorPage(newSailor);
                }
                else
                {
                    DatabaseHelper.updateSailor(_sailor);
                    MainViewModel.GoToSailorPage(_sailor);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


    }
}