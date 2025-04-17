using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Pages;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class SailorsViewModel : BaseViewModel

    {
        private const int PageSize = 10;
        
        [ObservableProperty] 
        private MainViewModel? _mainViewModel;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty]
        private string searchTerm = string.Empty;


        public ObservableCollection<SailorViewModel> Sailors { get; set; } = new();
        public ObservableCollection<SailorViewModel> FilteredSailors { get; set; } = new();
        public ObservableCollection<SailorViewModel> PaginatedSailors { get; set; } = new();


        public SailorsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoadSailors();
            UpdateFilteredSailors();
            UpdatePagination();

            //Sailors = new ObservableCollection<SailorViewModel>(_getSailors());
        }


        private List<SailorViewModel> _getSailors()
        {
            var sailors = new List<SailorViewModel>();

            var sailorList = DatabaseHelper.GetSailors();
            foreach (var sailor in sailorList)
            {
                sailors.Add(new SailorViewModel(_mainViewModel, sailor));
            }

            return sailors;

        }

        private void LoadSailors()
        {
            // Load all sailors from the database
            var allSailors = _getSailors();
            Sailors = new ObservableCollection<SailorViewModel>(allSailors);

            // Calculate total pages
            TotalPages = (int)Math.Ceiling((double)Sailors.Count / PageSize);
        }

        private void UpdateFilteredSailors()
        {
            FilteredSailors.Clear();
            var filtered = Sailors.Where(s =>
                string.IsNullOrEmpty(SearchTerm) ||
                s.Fullname.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                s.GID.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));

            foreach (var sailor in filtered)
            {
                FilteredSailors.Add(sailor);
            }

            // Update total pages based on filtered results
            TotalPages = (int)Math.Ceiling((double)FilteredSailors.Count / PageSize);
            CurrentPage = 1; // Reset to the first page
            UpdatePagination();
        }
        private void UpdatePagination()
        {
            PaginatedSailors.Clear();
            var startIndex = (CurrentPage - 1) * PageSize;
            var paginatedItems = FilteredSailors.Skip(startIndex).Take(PageSize);

            foreach (var sailor in paginatedItems)
            {
                PaginatedSailors.Add(sailor);
            }
        }


        [RelayCommand]
        private void GoToKreirajPomorcaPage()
        {
            if (MainViewModel != null)
            {
                MainViewModel.CurrentPage = MainViewModel.KreirajKorisnikaPage;
            }
        }

        [RelayCommand]
        private void GoToProfilPanelPage()
        {
            if (MainViewModel != null)
            {
                MainViewModel?.GoToPomorciPanel();

                if (MainViewModel?.IsPocetnaView != null)
                {
                    MainViewModel.IsPocetnaView = false;
                }
            }
        }
        public bool CanGoToFirstOrPrevious => CurrentPage > 1;
        public bool CanGoToNextOrLast => CurrentPage < TotalPages;
        partial void OnSearchTermChanged(string value)
        {
            UpdateFilteredSailors();
        }
        partial void OnCurrentPageChanged(int value)
        {
            UpdatePagination();
            //OnPropertyChanged(nameof(CanGoToFirstOrPrevious));
            //OnPropertyChanged(nameof(CanGoToNextOrLast));
        }

        [RelayCommand]
        private void GoToSailorPage()
        {

        }

        [RelayCommand]
        private void GoToFirstPage()
        {
            CurrentPage = 1;
            //UpdatePagination();
        }

        [RelayCommand]
        private void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                //UpdatePagination();
            }
        }

        [RelayCommand]
        private void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                //UpdatePagination();
            }
        }

        [RelayCommand]
        private void GoToLastPage()
        {
            CurrentPage = TotalPages;
            //UpdatePagination();
        }
    }
} 