using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;

namespace BiznesApp.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLight))]
        private bool _isDark;

        public bool IsLight => !IsDark;

        public SettingsViewModel()
        {
            if (Application.Current != null)
            {
                _isDark = Application.Current.UserAppTheme == AppTheme.Dark;
            }
        }

        partial void OnIsDarkChanged(bool value)
        {
            if (Application.Current != null)
            {
                Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            SecureStorage.Remove("auth_token");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
} 