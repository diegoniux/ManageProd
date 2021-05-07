using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageProd.SQLiteDB.Models;
using SQLite;

namespace ManageProd.SQLiteDB.Data
{
    public class OrdenCompraItemDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<OrdenCompraItemDB> Instance = new AsyncLazy<OrdenCompraItemDB>(async () =>
        {
            var instance = new OrdenCompraItemDB();
            CreateTableResult result = await Database.CreateTableAsync<OrdenCompraItem>();
            return instance;
        });

        public OrdenCompraItemDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<OrdenCompraItem>> GetOrdenCompraAsync()
        {
            return Database.Table<OrdenCompraItem>().ToListAsync();
        }

        public Task<OrdenCompraItem> GetOrdenComprasAsync(OrdenCompraItem request)
        {
            return Database.Table<OrdenCompraItem>().Where(i => i.IdOrdenCompra == request.IdOrdenCompra).FirstOrDefaultAsync();
        }       

        public Task<List<OrdenCompraItem>> GetOrdenComprasIdsAsync(OrdenCompraItem request)
        {
            return Database.QueryAsync<OrdenCompraItem>("SELECT * FROM [OrdenCompraItem] WHERE [IdOrdenCompra] = " + request.IdOrdenCompra + " AND [IdProveedor] = " + request.IdProveedor);
        }

        public Task<List<OrdenCompraItem>> GetOrdenComprasIdProveedorAsync(OrdenCompraItem request)
        {
            return Database.QueryAsync<OrdenCompraItem>("SELECT * FROM [OrdenCompraItem] WHERE [IdProveedor] = " + request.IdProveedor);
        }

        public Task<int> SaveOrdenVentaAsync(OrdenCompraItem request)
        {
            if (request.IdOrdenCompra != 0)
            {
                return Database.UpdateAsync(request);
            }
            else
            {
                return Database.InsertAsync(request);
            }
        }

        public Task<int> DeleteOrdenComprasAsync(OrdenCompraItem request)
        {
            return Database.DeleteAsync(request);
        }

        public Task<int> DeleteAllOrdenComprasAsync()
        {
            return Database.ExecuteAsync("DELETE FROM [OrdenCompraItem]");
        }
    }
}
