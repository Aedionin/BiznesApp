using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace BiznesApp.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        public RegisterViewModel()
        {
            // Konstruktor pozostaje pusty
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wszystkie pola są wymagane.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Błąd", "Hasła nie są zgodne.", "OK");
                return;
            }

            // TODO: Tutaj w przyszłości będzie wywołanie API do rejestracji

            await Shell.Current.DisplayAlert("Sukces", "Rejestracja zakończona pomyślnie! Możesz się teraz zalogować.", "OK");

            await GoToLogin();
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
} 