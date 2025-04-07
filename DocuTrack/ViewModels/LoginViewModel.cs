using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;


namespace DocuTrack.ViewModels;

public partial class LoginViewModel : BaseViewModel

{


    public LoginViewModel(){}
    public LoginViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

   
   
   [ObservableProperty] public string login = "Login";
   [ObservableProperty] public string errorMessage = string.Empty;
   [ObservableProperty] public string username = "";
   [ObservableProperty] public string password = "";




   [ObservableProperty]
   private  MainViewModel? _mainViewModel;

   [RelayCommand]
   private void OnLogin()
   {
      if((Username == "" || Username == "Marko") && Password == "")
         {
            // MainViewModel.CurrentPage = MainViewModel.PomorciPage; // this works also
            MainViewModel?.OnLoginSuccessful();
        }
        else
         {
            ErrorMessage = "Invalid Credentials";
         }
   }

 

}