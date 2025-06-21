using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.IO;
using BiznesApp.Services;
using BiznesApp.Models;
using System.Threading.Tasks;
using Plugin.LocalNotification;

namespace BiznesApp.ViewModels
{
    [QueryProperty(nameof(CurrentOrder), "SelectedOrder")]
    public partial class EditOrderViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private Order _currentOrder;

        [ObservableProperty]
        private ImageSource? _attachedPhoto;

        [ObservableProperty]
        private string? _displayFileName;

        public EditOrderViewModel(DataService dataService)
        {
            _dataService = dataService;
            _currentOrder = new Order();
        }
        
        partial void OnCurrentOrderChanged(Order value)
        {
            if (value != null && !string.IsNullOrEmpty(value.PhotoPath))
            {
                AttachedPhoto = ImageSource.FromFile(value.PhotoPath);
                DisplayFileName = Path.GetFileName(value.PhotoPath);
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
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlert("Błąd", "Twoje urządzenie nie obsługuje robienia zdjęć.", "OK");
                return;
            }

            try
            {
                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using (Stream sourceStream = await photo.OpenReadAsync())
                    using (FileStream localFileStream = File.Create(localFilePath))
                    {
                        await sourceStream.CopyToAsync(localFileStream);
                    }

                    CurrentOrder.PhotoPath = localFilePath;
                    AttachedPhoto = ImageSource.FromFile(localFilePath);
                    DisplayFileName = photo.FileName;
                }
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się zrobić zdjęcia.", "OK");
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
            }
            else
            {
                await _dataService.UpdateOrder(CurrentOrder);
            }
            
            await Shell.Current.DisplayAlert("Sukces", "Zamówienie zostało zapisane.", "OK");

            if(isNew)
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1337,
                    Title = "Dodano nowe zamówienie",
                    Subtitle = CurrentOrder.Name,
                    Description = $"Kwota: {CurrentOrder.Amount:C}",
                    BadgeNumber = 42,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5)
                    }
                };
                await LocalNotificationCenter.Current.Show(request);
            }

            await Shell.Current.GoToAsync("..");
        }
    }
} 