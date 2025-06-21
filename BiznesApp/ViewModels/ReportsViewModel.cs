using BiznesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Threading.Tasks;

namespace BiznesApp.ViewModels
{
    public partial class ReportsViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private string _title = "Raporty";

        [ObservableProperty]
        private int activeOrdersCount;

        [ObservableProperty]
        private decimal completedOrdersValue;
        
        [ObservableProperty]
        private int pendingOffersCount;

        [ObservableProperty]
        private double conversionRate;

        public ReportsViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        [RelayCommand]
        private async Task LoadData()
        {
            var orders = await _dataService.GetOrders();
            var offers = await _dataService.GetOffers();

            ActiveOrdersCount = orders.Count(o => o.Status == "W realizacji" || o.Status == "Nowe");
            CompletedOrdersValue = orders.Where(o => o.Status == "Zakończone").Sum(o => o.Amount);
            PendingOffersCount = offers.Count(o => o.Status == "Wysłana");

            int totalOffers = offers.Count;
            if (totalOffers > 0)
            {
                int acceptedOffers = offers.Count(o => o.Status == "Zaakceptowana");
                // Załóżmy, że każda zaakceptowana oferta staje się zamówieniem
                ConversionRate = (double)acceptedOffers / totalOffers * 100;
            }
            else
            {
                ConversionRate = 0;
            }
        }
    }
} 