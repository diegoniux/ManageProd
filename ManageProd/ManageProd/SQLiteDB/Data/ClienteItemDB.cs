using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageProd.SQLiteDB.Models;
using SQLite;

namespace ManageProd.SQLiteDB.Data
{
    public class ClienteItemDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<ClienteItemDB> Instance = new AsyncLazy<ClienteItemDB>(async () =>
        {
            var instance = new ClienteItemDB();
            CreateTableResult result = await Database.CreateTableAsync<ClienteItem>();
            return instance;
        });

        public ClienteItemDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<ClienteItem>> GetClientsAsync()
        {
            return Database.Table<ClienteItem>().ToListAsync();
        }

        public Task<ClienteItem> GetClientIdAsync(ClienteItem request)
        {
            return Database.Table<ClienteItem>().Where(i => i.IdCliente == request.IdCliente).FirstOrDefaultAsync();
        }

        public Task<int> SaveClientsAsync(ClienteItem request)
        {
            if (request.IdCliente != 0)
            {
                return Database.UpdateAsync(request);
            }
            else
            {
                return Database.InsertAsync(request);
            }
        }

        public Task<int> InsertClientsAsync(ClienteItem request)
        {
            return Database.InsertAsync(request);
        }

        public Task<int> DeleteClientAsync(ClienteItem request)
        {
            return Database.DeleteAsync(request);
        }

        public Task<int> DeleteAllClientsAsync()
        {
            return Database.ExecuteAsync("DELETE FROM [ClienteItem]");
        }
    }
}
