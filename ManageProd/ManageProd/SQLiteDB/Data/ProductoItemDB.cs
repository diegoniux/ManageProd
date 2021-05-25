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

        public Task<ProductoItem> GetProductsIdAsync(ProductoItem request)
        {
            return Database.Table<ProductoItem>().Where(i => i.IdProducto == request.IdProducto).FirstOrDefaultAsync();           
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

        public Task<int> DeleteProductAsync(ProductoItem request)
        {
            return Database.DeleteAsync(request);
        }

        public Task<int> DeleteAllProductsAsync()
        {
            return Database.ExecuteAsync("DELETE FROM [ProductoItem]");
        }
    }
}
