using BiznesApp.Models;
using BiznesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BiznesApp.ViewModels
{
    public partial class OffersViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private bool isBusy;

        public ObservableCollection<Offer> Offers { get; set; }

        public OffersViewModel(DataService dataService)
        {
            _dataService = dataService;
            Offers = new ObservableCollection<Offer>();
        }

        [RelayCommand]
        private async Task LoadOffers()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                Offers.Clear();
                var offers = await _dataService.GetOffers();
                foreach (var offer in offers)
                {
                    Offers.Add(offer);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToOfferDetails(Offer offer)
        {
            if (offer == null)
                return;

            await Shell.Current.GoToAsync("OfferDetailsPage", new Dictionary<string, object>
            {
                { "SelectedOffer", offer }
            });
        }

        [RelayCommand]
        private async Task AddNewOffer()
        {
            await Shell.Current.GoToAsync("EditOfferPage");
        }
    }
} 