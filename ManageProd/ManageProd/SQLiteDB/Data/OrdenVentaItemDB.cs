using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageProd.SQLiteDB.Models;
using SQLite;

namespace ManageProd.SQLiteDB.Data
{
    public class OrdenVentaItemDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<OrdenVentaItemDB> Instance = new AsyncLazy<OrdenVentaItemDB>(async () =>
        {
            var instance = new OrdenVentaItemDB();
            CreateTableResult result = await Database.CreateTableAsync<OrdenVentaItem>();
            return instance;
        });

        public OrdenVentaItemDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<OrdenVentaItem>> GetOrdenVentaAsync()
        {
            return Database.Table<OrdenVentaItem>().ToListAsync();
        }

        public Task<OrdenVentaItem> GetOrdenVentasAsync(OrdenVentaItem request)
        {
            return Database.Table<OrdenVentaItem>().Where(i => i.IdOrdenVenta == request.IdOrdenVenta).FirstOrDefaultAsync();
        }        

        public Task<List<OrdenVentaItem>> GetOrdenVentaIdsAsync(OrdenVentaItem request)
        {
            return Database.QueryAsync<OrdenVentaItem>("SELECT * FROM [OrdenVentaItem] WHERE [IdOrdenVenta] = " + request.IdOrdenVenta + " AND [IdCliente] = " + request.IdCliente);
        }

        public Task<List<OrdenVentaItem>> GetProductsIdClienteAsync(OrdenVentaItem request)
        {
            return Database.QueryAsync<OrdenVentaItem>("SELECT * FROM [OrdenVentaItem] WHERE [IdCliente] = " + request.IdCliente);
        }

        public Task<int> SaveOrdenVentaAsync(OrdenVentaItem request)
        {
            if (request.IdOrdenVenta != 0)
            {
                return Database.UpdateAsync(request);
            }
            else
            {
                return Database.InsertAsync(request);
            }
        }

        public Task<int> DeleteOrdenVentaAsync(OrdenVentaItem request)
        {
            return Database.DeleteAsync(request);
        }

        public Task<int> DeleteAllOrdenVentasAsync()
        {
            return Database.ExecuteAsync("DELETE FROM [OrdenVentaItem]");
        }
    }
}
