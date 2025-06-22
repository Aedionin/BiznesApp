using System.Collections.ObjectModel;
using BiznesApp.Models;
using BiznesApp.Services;
using BiznesApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BiznesApp.ViewModels
{
    public partial class OrdersViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        
        [ObservableProperty]
        private bool isBusy;
        
        public ObservableCollection<Order> Orders { get; set; }

        public OrdersViewModel(DataService dataService)
        {
            _dataService = dataService;
            Orders = new ObservableCollection<Order>();
        }

        [RelayCommand]
        private async Task LoadOrders()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                Orders.Clear();
                var orders = await _dataService.GetOrders();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task AddNewOrder()
        {
            var newOrder = new Order();
            await Shell.Current.GoToAsync(nameof(EditOrderPage), new Dictionary<string, object>
            {
                { "CurrentOrder", newOrder }
            });
        }
    }
} 