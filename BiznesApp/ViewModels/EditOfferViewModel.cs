using BiznesApp.Models;
using BiznesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BiznesApp.ViewModels
{
    [QueryProperty(nameof(CurrentOffer), "SelectedOffer")]
    public partial class EditOfferViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private Offer currentOffer;

        public EditOfferViewModel(DataService dataService)
        {
            _dataService = dataService;
            currentOffer = new Offer { Status = "Wysłana" }; // Domyślny status dla nowej oferty
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