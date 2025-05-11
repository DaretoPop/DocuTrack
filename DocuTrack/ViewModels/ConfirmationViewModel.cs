using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class ConfirmationViewModel : BaseViewModel
    {

        public string Message { get; set; } = string.Empty;

        
        public IRelayCommand ConfirmCommand { get; set; }
        

       
        public IRelayCommand CancelCommand { get; set; }
        
    }
}
