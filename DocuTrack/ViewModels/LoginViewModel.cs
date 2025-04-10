using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
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
       using var connection = DatabaseHelper.GetConnection();
       var command = connection.CreateCommand();
       command.CommandText = "SELECT COUNT(1) FROM Accounts WHERE Username = @username AND Password = @password";
       command.Parameters.AddWithValue("@username", Username);
       command.Parameters.AddWithValue("@password", Password);

       var result = (long)command.ExecuteScalar();

       if (result > 0)
       {
           MainViewModel?.OnLoginSuccessful();
       }
       else
       {
           ErrorMessage = "Invalid Credentials";
       }
    }

 

}