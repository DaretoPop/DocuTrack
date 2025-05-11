using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Views;

namespace DocuTrack.ViewModels {
    public partial class ExpirationsViewModel : BaseViewModel
    {

        private const int PageSize = 10;
        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty]
        private string searchTerm = string.Empty;
        public ObservableCollection<Certificate> Certificates { get; set; } = new();
        public ObservableCollection<Certificate> FilteredCertificates { get; set; } = new();
        public ObservableCollection<Certificate> PaginatedCertificates { get; set; } = new();

        [ObservableProperty] private Certificate? _selectedCertificate = null;

        [ObservableProperty] private MainViewModel? _mainViewModel;

        public int TotalSertificateCount => Certificates.Count;

        public ExpirationsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            LoadCertificates();
            UpdateFilteredCertificates();
            UpdatePagination();

            BindScaling(mainViewModel);
        }


        private void LoadCertificates()
        {
            // Load all sailors from the database
            var all = DatabaseHelper.GetCertificatesWhichWillExpire();
            Certificates = new ObservableCollection<Certificate>(all);

            // Calculate total pages
            TotalPages = (int)Math.Ceiling((double)Certificates.Count / PageSize);

            //Show update count of Sailors
            OnPropertyChanged(nameof(TotalSertificateCount));
        }

        private void UpdateFilteredCertificates()
        {
            FilteredCertificates.Clear();
            var filtered = Certificates.Where(s =>
                string.IsNullOrEmpty(SearchTerm) ||
                s.SailorName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                s.CertificateTypeName.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));

            foreach (var sailor in filtered)
            {
                FilteredCertificates.Add(sailor);
            }

            // Update total pages based on filtered results
            TotalPages = (int)Math.Ceiling((double)FilteredCertificates.Count / PageSize);
            CurrentPage = 1; // Reset to the first page
            UpdatePagination();
        }
        private void UpdatePagination()
        {
            PaginatedCertificates.Clear();
            var startIndex = (CurrentPage - 1) * PageSize;
            var paginatedItems = FilteredCertificates.Skip(startIndex).Take(PageSize);

            foreach (var sailor in paginatedItems)
            {
                PaginatedCertificates.Add(sailor);
            }
        }



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


        public bool CanGoToFirstOrPrevious => CurrentPage > 1;
        public bool CanGoToNextOrLast => CurrentPage < TotalPages;
        partial void OnSearchTermChanged(string value)
        {
            UpdateFilteredCertificates();
            OnPropertyChanged(nameof(CanGoToFirstOrPrevious));
            OnPropertyChanged(nameof(CanGoToNextOrLast));
        }
        partial void OnCurrentPageChanged(int value)
        {
            UpdatePagination();
            OnPropertyChanged(nameof(CanGoToFirstOrPrevious));
            OnPropertyChanged(nameof(CanGoToNextOrLast));
        }


        [RelayCommand]
        private void GoToFirstPage()
        {
            CurrentPage = 1;
        }

        [RelayCommand]
        private void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        [RelayCommand]
        private void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
        }

        [RelayCommand]
        private void GoToLastPage()
        {
            CurrentPage = TotalPages;
        }
    }
}