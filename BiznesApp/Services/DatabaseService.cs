using BiznesApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

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

        public async Task<int> SaveItemAsync<T>(T item) where T : new()
        {
            await Init();
            var pk = typeof(T).GetProperty("Id")?.GetValue(item);
            if (pk != null && (int)pk != 0)
            {
                var existingItem = await _database.FindAsync<T>(pk);
                if (existingItem != null)
                {
                    return await _database.UpdateAsync(item);
                }
            }
            return await _database.InsertAsync(item);
        }
    }
} 