using BiznesApp.Models;
using BiznesApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace BiznesApp.Services
{
    public class DataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;

        public DataService()
        {
            _httpClient = new HttpClient();
            // Adres API. Używamy 10.0.2.2 dla emulatora Androida, aby uzyskać dostęp do localhost hosta.
            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5136" : "http://localhost:5136";
        }

        private async Task SetAuthorizationHeader()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                // Wyczyść nagłówek, jeśli token jest pusty - np. po wylogowaniu
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        // --- Metody dla Zamówień ---

        public async Task<ObservableCollection<Order>> GetOrders()
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.GetAsync($"{_baseAddress}/api/orders");
            if (response.IsSuccessStatusCode)
            {
                var orders = await response.Content.ReadFromJsonAsync<ObservableCollection<Order>>();
                return orders ?? new ObservableCollection<Order>();
            }
            return new ObservableCollection<Order>();
        }

        public async Task AddOrder(Order order)
        {
            await SetAuthorizationHeader();
            await _httpClient.PostAsJsonAsync($"{_baseAddress}/api/orders", order);
        }

        public async Task UpdateOrder(Order updatedOrder)
        {
            await SetAuthorizationHeader();
            await _httpClient.PutAsJsonAsync($"{_baseAddress}/api/orders/{updatedOrder.Id}", updatedOrder);
        }

        public async Task DeleteOrder(Order order)
        {
            await SetAuthorizationHeader();
            await _httpClient.DeleteAsync($"{_baseAddress}/api/orders/{order.Id}");
        }

        // --- Metody dla Ofert ---

        public async Task<ObservableCollection<Offer>> GetOffers()
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.GetAsync($"{_baseAddress}/api/offers");
            if (response.IsSuccessStatusCode)
            {
                var offers = await response.Content.ReadFromJsonAsync<ObservableCollection<Offer>>();
                return offers ?? new ObservableCollection<Offer>();
            }
            return new ObservableCollection<Offer>();
        }

        public async Task AddOffer(Offer offer)
        {
            await SetAuthorizationHeader();
            await _httpClient.PostAsJsonAsync($"{_baseAddress}/api/offers", offer);
        }

        public async Task UpdateOffer(Offer updatedOffer)
        {
            await SetAuthorizationHeader();
            await _httpClient.PutAsJsonAsync($"{_baseAddress}/api/offers/{updatedOffer.Id}", updatedOffer);
        }

        public async Task DeleteOffer(Offer offer)
        {
            await SetAuthorizationHeader();
            await _httpClient.DeleteAsync($"{_baseAddress}/api/offers/{offer.Id}");
        }
    }
} 