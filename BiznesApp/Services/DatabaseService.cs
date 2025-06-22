using BiznesApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiznesApp.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        private async Task Init()
        {
            if (_database is not null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "BiznesApp.db");
            _database = new SQLiteAsyncConnection(databasePath);
            await _database.CreateTableAsync<Order>();
            await _database.CreateTableAsync<Offer>();
        }

        public async Task<List<T>> GetItems<T>() where T : new()
        {
            await Init();
            return await _database.Table<T>().ToListAsync();
        }

        public async Task SaveItems<T>(List<T> items)
        {
            await Init();
            await _database.DeleteAllAsync<T>();
            await _database.InsertAllAsync(items);
        }
    }
} 