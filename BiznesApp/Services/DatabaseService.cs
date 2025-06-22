using BiznesApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiznesApp.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        private async Task Init()
        {
            if (_database is not null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "BiznesApp.db");
            _database = new SQLiteAsyncConnection(databasePath);
            await _database.CreateTableAsync<Order>();
            await _database.CreateTableAsync<Offer>();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            await Init();
            return await _database.Table<Order>().ToListAsync();
        }

        public async Task<List<Offer>> GetOffersAsync()
        {
            await Init();
            return await _database.Table<Offer>().ToListAsync();
        }

        public async Task SaveOrdersAsync(List<Order> orders)
        {
            await Init();
            // Czyścimy tabelę i wstawiamy nowe dane, aby uniknąć duplikatów
            await _database.DeleteAllAsync<Order>();
            await _database.InsertAllAsync(orders);
        }

        public async Task SaveOffersAsync(List<Offer> offers)
        {
            await Init();
            await _database.DeleteAllAsync<Offer>();
            await _database.InsertAllAsync(offers);
        }
    }
} 