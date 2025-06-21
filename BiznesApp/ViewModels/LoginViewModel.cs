using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace BiznesApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        public LoginViewModel()
        {
            // Konstruktor pozostaje pusty
        }

        [RelayCommand]
        private async Task Login()
        {
            // TODO: Wywołaj logowanie do backendu/Azure (API)
            await Shell.Current.GoToAsync("//OrdersPage");
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(Views.RegisterPage));
        }
    }
} 