using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class CertificateTypesViewModel : BaseViewModel

    {

        private const int PageSize = 10;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int totalPages;

        [ObservableProperty] 
        private string _searchTerm = string.Empty;


        public ObservableCollection<CertificateTypeViewModel> CertificateTypes { get; set; } = new();
        public ObservableCollection<CertificateTypeViewModel> FilteredCertificateTypes { get; set; } = new();
        public ObservableCollection<CertificateTypeViewModel> PaginatedCertificateTypes { get; set; } = new();

        public int TotalCertificateTypes => CertificateTypes.Count;


        public CertificateTypesViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            LoadCertificateTypes();
            UpdateFilteredCertificateTypes();
            UpdatePagination();

            BindScaling(mainViewModel);
        }


        [ObservableProperty] private MainViewModel _mainViewModel;
        [ObservableProperty] private bool _isRightPanelVisible;

        
        [RelayCommand]
        private void ToggleRightPanel()
        {
            IsRightPanelVisible = !IsRightPanelVisible;
        }


        [RelayCommand]
        private void GoToAddNewDocument()
        {
            if(MainViewModel != null)
            {
                MainViewModel.CurrentPage = new UpsertCertificateTypeViewModel(MainViewModel,null);
            }
        }



        private List<CertificateTypeViewModel> _getCertificateTypes()
        {
            var output = new List<CertificateTypeViewModel>();

            var typeList = DatabaseHelper.geCertificateTypes();
            foreach (var type in typeList)
            {
                output.Add(new CertificateTypeViewModel(_mainViewModel, type));
            }

            return output;

        }

        private void LoadCertificateTypes()
        {
            // Load all sailors from the database
            var allTypes = _getCertificateTypes();
            CertificateTypes = new ObservableCollection<CertificateTypeViewModel>(allTypes);

            OnPropertyChanged(nameof(TotalCertificateTypes));
            // Calculate total pages
            TotalPages = (int)Math.Ceiling((double)CertificateTypes.Count / PageSize);
        }


        private void UpdatePagination()
        {
            PaginatedCertificateTypes.Clear();
            var startIndex = (CurrentPage - 1) * PageSize;
// Update to be Filtered
            var paginatedItems = FilteredCertificateTypes.Skip(startIndex).Take(PageSize);
            foreach (var sailor in paginatedItems)
            {
                PaginatedCertificateTypes.Add(sailor);
            }
        }

    private void UpdateFilteredCertificateTypes()
    {
        FilteredCertificateTypes.Clear();
        var filtered = CertificateTypes.Where(s =>
            string.IsNullOrEmpty(SearchTerm) ||
            s.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
            (s.Naziv?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false));

        foreach (var type in filtered)
        {
            FilteredCertificateTypes.Add(type);
        }

        TotalPages = (int)Math.Ceiling((double)FilteredCertificateTypes.Count / PageSize);
        CurrentPage = 1; // Reset to first page when filtering
        UpdatePagination();
    }

    partial void OnSearchTermChanged(string value)
    {
        UpdateFilteredCertificateTypes();
        OnPropertyChanged(nameof(CanGoToFirstOrPrevious));
        OnPropertyChanged(nameof(CanGoToNextOrLast));
    }




        public bool CanGoToFirstOrPrevious => CurrentPage > 1;
        public bool CanGoToNextOrLast => CurrentPage < TotalPages;

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