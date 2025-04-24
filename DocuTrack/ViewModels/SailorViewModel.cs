using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.DataModels;
using DocuTrack.Pages;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{


    public partial class SailorViewModel : BaseViewModel

    {
        [ObservableProperty] private Sailor? _sailor;
        [ObservableProperty] private MainViewModel? _mainViewModel;


        public SailorViewModel()
        {
        }

        public SailorViewModel(MainViewModel mainViewModel, Sailor sailor)
        {
            _mainViewModel = mainViewModel;
            _sailor = sailor;
        }




        public string Fullname
        {
            get
            {
                return $"{Sailor?.Name} {Sailor?.Surname}";
            }
        }

        public string GID
        {
            get
            {
                return Sailor?.GID.ToString() ?? string.Empty;
            }
        }

        public string Refresh
        {
            get
            {
                return Sailor?.Refresh ?? string.Empty;
            }
        }

        // Back Button to navigate to ProfilPage or PocentaPage(sertifikati)
        [RelayCommand]
        public void GoBack()
        {
            if (MainViewModel != null)
            {
                if (MainViewModel?.IsPocetnaView == true)
                {
                    MainViewModel.CurrentPage = MainViewModel.PocetnaPage;
                }
                else
                {
                    MainViewModel?.GoToPomorci();
                }
            }
        }


        //When you go from SailorRowView to SailorView
        [RelayCommand]
        private void GoToSailorPage()
        {
            if (MainViewModel != null)
            {
                MainViewModel?.GoToSailorPage(_sailor);

                if (MainViewModel?.IsPocetnaView != null)
                {
                    MainViewModel.IsPocetnaView = false;
                }
            }
        }

        //When you want to edit
        [RelayCommand]
        private void GoToEditSailorPage()
        {
            if (MainViewModel != null)
            {
                MainViewModel?.GoToAddEditSailorPage(_sailor);

                if (MainViewModel?.IsPocetnaView != null)
                {
                    MainViewModel.IsPocetnaView = false;
                }
            }
        }

        [RelayCommand]
        private async Task DeleteSailor()
        {
            if (MainViewModel == null || Sailor == null)
                return;

            // Create and configure the confirmation dialog
            var dialog = new ConfirmationDialog
            {
                DataContext = new
                {
                    Message = $"Are you sure you want to delete {Sailor.Name} {Sailor.Surname}?",
                    ConfirmCommand = new RelayCommand(() =>
                    {
                        // Perform the deletion
                        // MainViewModel.DeleteSailor(Sailor);
                        dialog.Close(); // Fixed: 'dialog' is now properly declared before usage
                    }),
                    CancelCommand = new RelayCommand(() =>
                    {
                        // Close the dialog without doing anything
                        dialog.Close(); // Fixed: 'dialog' is now properly declared before usage
                    })
                }
            };

            // Show the dialog as a modal window
            await dialog.ShowDialog(MainViewModel.Window);
        }



    }
}