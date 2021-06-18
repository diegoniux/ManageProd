using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageProd.SQLiteDB.Models;
using SQLite;

namespace ManageProd.SQLiteDB.Data
{
    public class ProductoItemDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<ProductoItemDB> Instance = new AsyncLazy<ProductoItemDB>(async () =>
        {
            var instance = new ProductoItemDB();
            CreateTableResult result = await Database.CreateTableAsync<ProductoItem>();
            return instance;
        });

        public ProductoItemDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<ProductoItem>> GetProductsAsync()
        {
            return Database.Table<ProductoItem>().ToListAsync();
        }

        public Task<ProductoItem> GetProductsIdAsync(int IdProducto)
        {
            return Database.Table<ProductoItem>().Where(i => i.IdProducto == IdProducto).FirstOrDefaultAsync();           
        }

        public Task<List<ProductoItem>> GetProductsIdProveedorAsync(int IdProveedor)
        {
            return Database.QueryAsync<ProductoItem>("SELECT * FROM [ProductoItem] WHERE [IdProveedor] = " + IdProveedor);
        }

        public Task<List<ProductoItem>> GetProductsIdsAsync(ProductoItem request)
        {
            return Database.QueryAsync<ProductoItem>("SELECT * FROM [ProductoItem] WHERE [IdProveedor] = " + request.IdProveedor + " AND [IdProducto] = " + request.IdProducto);
        }
        public Task<int> SaveProductsAsync(ProductoItem request)
        {
            if (request.IdProducto != 0)
            {
                return Database.UpdateAsync(request);
            }
            else
            {
                return Database.InsertAsync(request);
            }
        }

        public Task<int> InsertProductsAsync(ProductoItem request)
        {
            return Database.InsertAsync(request);
        }

        public async Task<int> DeleteProductAsync(ProductoItem request)
        {
            return await Database.DeleteAsync(request);
        }

        public async Task<int> DeleteAllProductsAsync()
        {
            try
            {
                await Database.ExecuteAsync("DELETE FROM [ProductoItem]");
                await Database.ExecuteAsync("DELETE FROM SQLITE_SEQUENCE WHERE NAME = 'ProductoItem'");

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
