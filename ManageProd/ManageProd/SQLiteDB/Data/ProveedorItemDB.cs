using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageProd.SQLiteDB.Models;
using SQLite;

namespace ManageProd.SQLiteDB.Data
{
    public class ProveedorItemDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<ProveedorItemDB> Instance = new AsyncLazy<ProveedorItemDB>(async () =>
        {
            var instance = new ProveedorItemDB();
            CreateTableResult result = await Database.CreateTableAsync<ProveedorItem>();
            return instance;
        });

        public ProveedorItemDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<ProveedorItem>> GetProveedoresAsync()
        {
            return Database.Table<ProveedorItem>().ToListAsync();
        }        

        public Task<ProveedorItem> GetProveedoresAsync(int id)
        {
            return Database.Table<ProveedorItem>().Where(i => i.IdProveedor == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveProveedoresAsync(ProveedorItem item)
        {
            if (item.IdProveedor != 0)            
                return Database.UpdateAsync(item);           
            else            
                return Database.InsertAsync(item);           
        }

        public Task<int> InsertProveedoresAsync(ProveedorItem item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> DeleteProveedoresAsync(ProveedorItem item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<int> DeleteAllProveedoresAsync()
        {
            return Database.ExecuteAsync("DELETE FROM [ProveedorItem]");
        }
    }
}
