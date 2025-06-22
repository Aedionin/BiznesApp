using BiznesApp.Models;
using BiznesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Devices.Sensors;

namespace BiznesApp.ViewModels
{
    [QueryProperty(nameof(CurrentOffer), "SelectedOffer")]
    public partial class EditOfferViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private readonly IGeolocation _geolocation;
        private readonly HttpClient _httpClient;
        private bool _isLocationLoading;

        [ObservableProperty]
        private Offer currentOffer;

        public EditOfferViewModel(DataService dataService, IGeolocation geolocation, HttpClient httpClient)
        {
            _dataService = dataService;
            _geolocation = geolocation;
            _httpClient = httpClient;
            currentOffer = new Offer { Status = "Wysłana" }; // Domyślny status dla nowej oferty
        }

        public async Task OnAppearing()
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
                CurrentOffer.Latitude = location.Latitude;
                CurrentOffer.Longitude = location.Longitude;
                
                // Pobierz adres na podstawie współrzędnych
                await GetAddressFromCoordinates(location.Latitude, location.Longitude);
            }
        }

        private async Task GetAddressFromCoordinates(double latitude, double longitude)
        {
            try
            {
                // Użyj darmowego API Nominatim (OpenStreetMap) do geokodowania
                var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}&zoom=18&addressdetails=1";
                
                var response = await _httpClient.GetStringAsync(url);
                
                // Prosty parsing JSON (w produkcji lepiej użyć System.Text.Json)
                if (response.Contains("\"display_name\":"))
                {
                    var startIndex = response.IndexOf("\"display_name\":\"") + 16;
                    var endIndex = response.IndexOf("\"", startIndex);
                    if (startIndex > 15 && endIndex > startIndex)
                    {
                        var address = response.Substring(startIndex, endIndex - startIndex);
                        CurrentOffer.Location = address;
                    }
                }
            }
            catch (Exception ex)
            {
                // Jeśli nie udało się pobrać adresu, użyj współrzędnych jako lokalizacji
                CurrentOffer.Location = $"GPS: {latitude:F6}, {longitude:F6}";
                System.Diagnostics.Debug.WriteLine($"Błąd geokodowania: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task SaveOffer()
        {
            if (string.IsNullOrWhiteSpace(CurrentOffer.Name) || CurrentOffer.Price <= 0)
            {
                await Shell.Current.DisplayAlert("Błąd", "Nazwa oferty i cena są wymagane.", "OK");
                return;
            }

            if (CurrentOffer.Id == 0)
            {
                // Nowa oferta
                await _dataService.AddOffer(CurrentOffer);
            }
            else
            {
                // Aktualizacja istniejącej
                await _dataService.UpdateOffer(CurrentOffer);
            }

            await Shell.Current.DisplayAlert("Sukces", "Oferta została zapisana.", "OK");

            await Shell.Current.GoToAsync("..");
        }
    }
} 