using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageProd.SQLiteDB.Models;
using SQLite;

namespace ManageProd.SQLiteDB.Data
{
    public class DetalleCompraItemDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<DetalleCompraItemDB> Instance = new AsyncLazy<DetalleCompraItemDB>(async () =>
        {
            var instance = new DetalleCompraItemDB();
            CreateTableResult result = await Database.CreateTableAsync<DetalleCompraItem>();
            return instance;
        });

        public DetalleCompraItemDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<DetalleCompraItem>> GetDetalleComprasAsync()
        {
            return Database.Table<DetalleCompraItem>().ToListAsync();
        }

        public Task<List<DetalleCompraItem>> GetDetalleCompraIdOrdenCompraAsync(int IdOrdenCompra)
        {
            return Database.QueryAsync<DetalleCompraItem>(
                "SELECT * FROM [DetalleCompraItem] WHERE [IdOrdenCompra] = " + IdOrdenCompra);
        }

        public Task<List<DetalleCompraItem>> GetDetalleVentaIdDetalleVentaCompraAsync(DetalleCompraItem request)
        {
            return Database.QueryAsync<DetalleCompraItem>(
                "SELECT * FROM [DetalleCompraItem] WHERE [IdDetalleVenta] = " + request.IdDetalleCompra + " AND [IdOrdenCompra] = " + request.IdOrdenCompra);
        }

        public Task<List<DetalleCompraItem>> GetDetalleCompraALLIdsAsync(DetalleCompraItem request)
        {
            return Database.QueryAsync<DetalleCompraItem>(
                "SELECT * FROM [DetalleCompraItem] WHERE [IdDetalleVenta] = " + request.IdDetalleCompra + " AND [IdOrdenCompra] = " + request.IdOrdenCompra
                 + " AND [IdProducto] = " + request.IdProducto);
        }

        public Task<int> SaveDetalleCompraAsync(DetalleCompraItem request)
        {
            if (request.IdDetalleCompra != 0)
            {
                return Database.UpdateAsync(request);
            }
            else
            {
                return Database.InsertAsync(request);
            }
        }

        public Task<Decimal> GetSumaImporteAsync(int IdOrdenCompra)
        {
           return Database.ExecuteScalarAsync<Decimal>(
                "SELECT SUM(Importe) FROM [DetalleCompraItem] WHERE [IdOrdenCompra] = " + IdOrdenCompra);
        }

        public Task<int> DeleteDetalleCompraAsync(DetalleCompraItem request)
        {
            return Database.DeleteAsync(request);
        }

        public Task<int> DeleteAllDetalleComprasAsync()
        {
            return Database.ExecuteAsync("DELETE FROM [DetalleCompraItem]");
        }
    }
}
