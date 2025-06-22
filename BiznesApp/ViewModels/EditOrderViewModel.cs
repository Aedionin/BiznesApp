using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.IO;
using BiznesApp.Services;
using BiznesApp.Models;
using System.Threading.Tasks;
using Plugin.LocalNotification;
using Microsoft.Maui.Media;
using System.Collections.ObjectModel;
using Microsoft.Maui.Devices.Sensors;

namespace BiznesApp.ViewModels
{
    [QueryProperty(nameof(CurrentOrder), "SelectedOrder")]
    [QueryProperty(nameof(CurrentOrder), "CurrentOrder")]
    public partial class EditOrderViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private readonly IMediaPicker _mediaPicker;
        private readonly IGeolocation _geolocation;
        private readonly HttpClient _httpClient;
        private bool _isLocationLoading;

        [ObservableProperty]
        private Order _currentOrder = new();

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> _statuses = new();
        
        [ObservableProperty]
        private ImageSource? _attachedPhoto;

        [ObservableProperty]
        private string? _displayFileName;

        [ObservableProperty]
        private bool _isExistingOrder;

        public EditOrderViewModel(DataService dataService, IMediaPicker mediaPicker, IGeolocation geolocation, HttpClient httpClient)
        {
            _dataService = dataService;
            _mediaPicker = mediaPicker;
            _geolocation = geolocation;
            _httpClient = httpClient;
            Title = "Nowe zamówienie";
            Statuses = new ObservableCollection<string> { "Nowe", "W realizacji", "Zakończone" };
            CurrentOrder = new Order { Status = "Nowe" };
        }
        
        public async Task OnAppearing()
        {
            if (CurrentOrder != null && CurrentOrder.Id == 0)
            {
                if (_isLocationLoading)
                    return;

                try
                {
                    _isLocationLoading = true;
                    await GetCurrentLocation();
                }
                catch (System.Exception ex)
                {
                    await Shell.Current.DisplayAlert("Błąd lokalizacji", $"Nie udało się pobrać lokalizacji: {ex.Message}", "OK");
                }
                finally
                {
                    _isLocationLoading = false;
                }
            }
        }

        private async Task GetCurrentLocation()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert("Brak uprawnień", "Nie udzielono zgody na dostęp do lokalizacji.", "OK");
                    return;
                }
            }

            var location = await _geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, System.TimeSpan.FromSeconds(10)));

            if (location != null)
            {
                await GetAddressFromCoordinates(location.Latitude, location.Longitude);
            }
        }

        private async Task GetAddressFromCoordinates(double latitude, double longitude)
        {
            try
            {
                var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}&zoom=18&addressdetails=1";
                
                var response = await _httpClient.GetStringAsync(url);
                
                if (response.Contains("\"display_name\":"))
                {
                    var startIndex = response.IndexOf("\"display_name\":\"") + 16;
                    var endIndex = response.IndexOf("\"", startIndex);
                    if (startIndex > 15 && endIndex > startIndex)
                    {
                        var address = response.Substring(startIndex, endIndex - startIndex);
                        CurrentOrder.Location = address;
                        OnPropertyChanged(nameof(CurrentOrder)); // Powiadom UI o zmianie
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentOrder.Location = $"GPS: {latitude:F6}, {longitude:F6}";
                OnPropertyChanged(nameof(CurrentOrder)); // Powiadom UI o zmianie
                System.Diagnostics.Debug.WriteLine($"Błąd geokodowania: {ex.Message}");
            }
        }

        partial void OnCurrentOrderChanged(Order value)
        {
            if (value != null)
            {
                Title = value.Id == 0 ? "Nowe zamówienie" : "Edytuj zamówienie";

                if (!string.IsNullOrEmpty(value.PhotoPath))
                {
                    AttachedPhoto = ImageSource.FromFile(value.PhotoPath);
                    DisplayFileName = Path.GetFileName(value.PhotoPath);
                }
            }
        }

        [RelayCommand]
        private async Task PickFile()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Wybierz plik lub zdjęcie"
                });

                if (result != null)
                {
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                    using (var sourceStream = await result.OpenReadAsync())
                    using (var localStream = File.Create(localFilePath))
                    {
                        await sourceStream.CopyToAsync(localStream);
                    }

                    CurrentOrder.PhotoPath = localFilePath;
                    DisplayFileName = result.FileName;

                    // Spróbuj wyświetlić podgląd, jeśli to obrazek
                    try
                    {
                        AttachedPhoto = ImageSource.FromFile(localFilePath);
                    }
                    catch (Exception)
                    {
                        AttachedPhoto = null; // To nie jest obrazek, który MAUI może wyświetlić
                    }
                }
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się wybrać pliku.", "OK");
            }
        }

        [RelayCommand]
        private async Task TakePhoto()
        {
            if (_mediaPicker.IsCaptureSupported)
            {
                FileResult photo = await _mediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    CurrentOrder.PhotoPath = photo.FullPath;
                }
            }
        }

        [RelayCommand]
        private async Task PickPhoto()
        {
            FileResult photo = await _mediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                CurrentOrder.PhotoPath = photo.FullPath;
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentOrder.Name) || CurrentOrder.Amount <= 0)
            {
                await Shell.Current.DisplayAlert("Błąd", "Nazwa zamówienia i kwota są wymagane.", "OK");
                return;
            }

            bool isNew = CurrentOrder.Id == 0;

            if (isNew)
            {
                await _dataService.AddOrder(CurrentOrder);

#if !WINDOWS
                var request = new NotificationRequest
                {
                    NotificationId = 1000,
                    Title = "Nowe zamówienie",
                    Description = $"Dodano nowe zamówienie: {CurrentOrder.Name}",
                    BadgeNumber = 42,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5)
                    }
                };
                await LocalNotificationCenter.Current.Show(request);
#endif
            }
            else
            {
                await _dataService.UpdateOrder(CurrentOrder);
            }
            
            await Shell.Current.DisplayAlert("Sukces", "Zamówienie zostało zapisane.", "OK");

            await Shell.Current.GoToAsync("..");
        }
    }
} 