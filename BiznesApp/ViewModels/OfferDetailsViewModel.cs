using BiznesApp.Models;
using BiznesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace BiznesApp.ViewModels
{
    [QueryProperty(nameof(Offer), "SelectedOffer")]
    public partial class OfferDetailsViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private Offer? offer;

        public OfferDetailsViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        [RelayCommand]
        private async Task Edit()
        {
            if (Offer == null) return;
            await Shell.Current.GoToAsync("EditOfferPage", new Dictionary<string, object>
            {
                { "SelectedOffer", Offer }
            });
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (Offer == null) return;

            bool answer = await Shell.Current.DisplayAlert("Potwierdzenie", $"Czy na pewno chcesz usunąć ofertę '{Offer.Name}'?", "Tak", "Nie");
            if (answer)
            {
                await _dataService.DeleteOffer(Offer);
                await Shell.Current.GoToAsync("..");
            }
        }

        [RelayCommand]
        private async Task ConvertToOrder()
        {
            if (Offer == null) return;

            if (Offer.Status != "Zaakceptowana")
            {
                await Shell.Current.DisplayAlert("Informacja", "Można przekształcić tylko oferty o statusie 'Zaakceptowana'.", "OK");
                return;
            }

            var newOrder = new Order
            {
                Name = $"Zamówienie z oferty: {Offer.Name}",
                Amount = Offer.Price,
                Status = "Nowe"
            };

            await _dataService.AddOrder(newOrder);

            await Shell.Current.DisplayAlert("Sukces", $"Oferta '{Offer.Name}' została przekształcona w zamówienie!", "OK");

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
} 