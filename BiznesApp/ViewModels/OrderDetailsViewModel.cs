using BiznesApp.Models;
using BiznesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace BiznesApp.ViewModels
{
    [QueryProperty(nameof(Order), "SelectedOrder")]
    public partial class OrderDetailsViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private Order? order;

        public OrderDetailsViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        [RelayCommand]
        private async Task Edit()
        {
            if (Order == null) return;
            await Shell.Current.GoToAsync("EditOrderPage", new Dictionary<string, object>
            {
                { "SelectedOrder", Order }
            });
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (Order == null) return;

            bool answer = await Shell.Current.DisplayAlert("Potwierdzenie", $"Czy na pewno chcesz usunąć zamówienie '{Order.Name}'?", "Tak", "Nie");
            if (answer)
            {
                await _dataService.DeleteOrder(Order);
                await Shell.Current.GoToAsync("..");
            }
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
} 