using BiznesApp.Models;
using BiznesApp.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace BiznesApp.Services
{
    public class DataService
    {
        private readonly HttpClient _httpClient;
        private readonly DatabaseService _databaseService;

        public DataService(HttpClient httpClient, DatabaseService databaseService)
        {
            _httpClient = httpClient;
            _databaseService = databaseService;
        }

        private async Task SetAuthorizationHeader()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // Zamówienia
        public async Task<List<Order>> GetOrders()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync("/api/orders");
                if (response.IsSuccessStatusCode)
                {
                    var orders = await response.Content.ReadFromJsonAsync<List<Order>>();
                    if (orders != null)
                    {
                        await _databaseService.SaveItems(orders);
                        return orders;
                    }
                }
            }
            catch (Exception)
            {
                // Ignoruj błędy połączenia, zwróć dane z lokalnej bazy danych
            }

            return await _databaseService.GetItems<Order>();
        }

        public async Task AddOrder(Order order)
        {
            await SetAuthorizationHeader();
            await _httpClient.PostAsJsonAsync("/api/orders", order);
        }

        public async Task UpdateOrder(Order updatedOrder)
        {
            await SetAuthorizationHeader();
            await _httpClient.PutAsJsonAsync($"/api/orders/{updatedOrder.Id}", updatedOrder);
        }

        public async Task DeleteOrder(Order order)
        {
            await SetAuthorizationHeader();
            await _httpClient.DeleteAsync($"/api/orders/{order.Id}");
        }

        // Oferty
        public async Task<List<Offer>> GetOffers()
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync("/api/offers");
                if (response.IsSuccessStatusCode)
                {
                    var offers = await response.Content.ReadFromJsonAsync<List<Offer>>();
                    if (offers != null)
                    {
                        await _databaseService.SaveItems(offers);
                        return offers;
                    }
                }
            }
            catch (Exception)
            {
                // Ignoruj błędy połączenia, zwróć dane z lokalnej bazy danych
            }
            return await _databaseService.GetItems<Offer>();
        }

        public async Task AddOffer(Offer offer)
        {
            await SetAuthorizationHeader();
            await _httpClient.PostAsJsonAsync("/api/offers", offer);
        }

        public async Task UpdateOffer(Offer updatedOffer)
        {
            await SetAuthorizationHeader();
            await _httpClient.PutAsJsonAsync($"/api/offers/{updatedOffer.Id}", updatedOffer);
        }

        public async Task DeleteOffer(Offer offer)
        {
            await SetAuthorizationHeader();
            await _httpClient.DeleteAsync($"/api/offers/{offer.Id}");
        }
    }
} 