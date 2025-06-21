using BiznesApp.Models;
using BiznesApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BiznesApp.Services
{
    public class DataService
    {
        private ObservableCollection<Order> Orders { get; set; }
        private ObservableCollection<Offer> Offers { get; set; }

        public DataService()
        {
            // Inicjalizacja z danymi przykładowymi
            Orders = new ObservableCollection<Order>
            {
                new Order { Id = 1, Name = "Zamówienie na komputery", Status = "W realizacji", Amount = 12000 },
                new Order { Id = 2, Name = "Licencje na oprogramowanie", Status = "Zakończone", Amount = 4500 },
                new Order { Id = 3, Name = "Szkolenie dla zespołu", Status = "Nowe", Amount = 8000 }
            };

            Offers = new ObservableCollection<Offer>
            {
                new Offer { Id = 1, Name = "Strona internetowa dla firmy X", Description = "Stworzenie nowoczesnej strony WWW.", Price = 5000, Status = "Wysłana" },
                new Offer { Id = 2, Name = "Aplikacja mobilna dla sklepu", Description = "Projekt i wdrożenie aplikacji na Android/iOS.", Price = 25000, Status = "Zaakceptowana" },
                new Offer { Id = 3, Name = "Optymalizacja SEO", Description = "Audyt i pozycjonowanie serwisu.", Price = 2000, Status = "Odrzucona" }
            };
        }

        public Task<ObservableCollection<Order>> GetOrders() => Task.FromResult(Orders);
        public Task<ObservableCollection<Offer>> GetOffers() => Task.FromResult(Offers);

        // Metody do zarządzania danymi (CRUD)

        public Task AddOrder(Order order)
        {
            order.Id = Orders.Any() ? Orders.Max(o => o.Id) + 1 : 1;
            Orders.Add(order);
            return Task.CompletedTask;
        }

        public Task AddOffer(Offer offer)
        {
            offer.Id = Offers.Any() ? Offers.Max(o => o.Id) + 1 : 1;
            Offers.Add(offer);
            return Task.CompletedTask;
        }

        public Task UpdateOffer(Offer updatedOffer)
        {
            var offer = Offers.FirstOrDefault(o => o.Id == updatedOffer.Id);
            if (offer != null)
            {
                offer.Name = updatedOffer.Name;
                offer.Description = updatedOffer.Description;
                offer.Price = updatedOffer.Price;
                offer.Status = updatedOffer.Status;
            }
            return Task.CompletedTask;
        }

        public Task UpdateOrder(Order updatedOrder)
        {
            var order = Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
            if (order != null)
            {
                order.Name = updatedOrder.Name;
                order.Amount = updatedOrder.Amount;
                order.Status = updatedOrder.Status;
                order.PhotoPath = updatedOrder.PhotoPath;
            }
            return Task.CompletedTask;
        }

        public Task DeleteOffer(Offer offer)
        {
            if (offer != null)
            {
                Offers.Remove(offer);
            }
            return Task.CompletedTask;
        }

        public Task DeleteOrder(Order order)
        {
            if (order != null)
            {
                Orders.Remove(order);
            }
            return Task.CompletedTask;
        }
    }
} 