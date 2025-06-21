using BiznesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace BiznesApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Nazwa użytkownika i hasło są wymagane.";
                return;
            }

            try
            {
                var token = await _authService.LoginAsync(Username, Password);
                if (!string.IsNullOrEmpty(token))
                {
                    await SecureStorage.SetAsync("auth_token", token);
                    // Nawigacja do głównej części aplikacji
                    await Shell.Current.GoToAsync("//MainFlow/OrdersPage");
                }
                else
                {
                    ErrorMessage = "Logowanie nie powiodło się. Sprawdź dane uwierzytelniające.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wystąpił błąd: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(Views.RegisterPage));
        }
    }
} 