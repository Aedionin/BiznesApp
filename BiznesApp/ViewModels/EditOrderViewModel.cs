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

namespace BiznesApp.ViewModels
{
    [QueryProperty(nameof(OrderId), "orderId")]
    public partial class EditOrderViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private readonly IMediaPicker _mediaPicker;

        [ObservableProperty]
        private Order _currentOrder = new();

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> _statuses = new();
        
        [ObservableProperty]
        private string _orderId = string.Empty;

        [ObservableProperty]
        private ImageSource? _attachedPhoto;

        [ObservableProperty]
        private string? _displayFileName;

        [ObservableProperty]
        private bool _isExistingOrder;

        public EditOrderViewModel(DataService dataService, IMediaPicker mediaPicker)
        {
            _dataService = dataService;
            _mediaPicker = mediaPicker;
            Title = "Nowe zamówienie";
            Statuses = new ObservableCollection<string> { "Nowe", "W realizacji", "Zakończone" };
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