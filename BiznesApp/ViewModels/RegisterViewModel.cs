using BiznesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace BiznesApp.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public RegisterViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Nazwa użytkownika i hasło są wymagane.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Hasła nie są zgodne.";
                return;
            }

            try
            {
                var success = await _authService.RegisterAsync(Username, Password);
                if (success)
                {
                    // Powrót do strony logowania z komunikatem
                    await Shell.Current.GoToAsync("..?success=true");
                }
                else
                {
                    ErrorMessage = "Rejestracja nie powiodła się. Użytkownik może już istnieć.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wystąpił błąd: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
} 