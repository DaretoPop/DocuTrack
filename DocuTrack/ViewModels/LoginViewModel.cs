using CommunityToolkit.Mvvm.ComponentModel;


namespace DocuTrack.ViewModels;

public partial class LoginViewModel : BaseViewModel

{

   [ObservableProperty] public string login = "Login";
   [ObservableProperty] public string errorMessage = "invalid credentials";
   [ObservableProperty] public string username = "Username";
   [ObservableProperty] public string password = "Password";
   
}