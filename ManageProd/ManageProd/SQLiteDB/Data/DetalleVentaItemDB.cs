using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageProd.SQLiteDB.Models;
using SQLite;

namespace ManageProd.SQLiteDB.Data
{
    public class DetalleVentaItemDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<DetalleVentaItemDB> Instance = new AsyncLazy<DetalleVentaItemDB>(async () =>
        {
            var instance = new DetalleVentaItemDB();
            CreateTableResult result = await Database.CreateTableAsync<DetalleVentaItem>();
            return instance;
        });

        public DetalleVentaItemDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<DetalleVentaItem>> GetDetalleVentasAsync()
        {
            return Database.Table<DetalleVentaItem>().ToListAsync();
        }

        public Task<DetalleVentaItem> GetDetalleVentaIdAsync(DetalleVentaItem request)
        {
            return Database.Table<DetalleVentaItem>().Where(i => i.IdDetalleVenta == request.IdDetalleVenta).FirstOrDefaultAsync();
        }

        public Task<List<DetalleVentaItem>> GetDetalleVentaIdAsync(int IdOrdenVenta)
        {
            return Database.QueryAsync<DetalleVentaItem>(
                "SELECT * FROM [DetalleVentaItem] WHERE [IdOrdenVenta] = " + IdOrdenVenta);
        }

        public Task<List<DetalleVentaItem>> GetDetalleVentaALLIdsAsync(int IdDetalleVenta)
        {
            return Database.QueryAsync<DetalleVentaItem>(
                "SELECT * FROM [DetalleVentaItem] WHERE [IdDetalleVenta] = " + IdDetalleVenta);
        }

        public Task<int> SaveDetalleVentaAsync(DetalleVentaItem request)
        {
            if (request.IdDetalleVenta != 0)
            {
                return Database.UpdateAsync(request);
            }
            else
            {
                return Database.InsertAsync(request);
            }
        }

        public Task<Decimal> GetSumaImporteAsync(int IdOrdenVenta)
        {
            return Database.ExecuteScalarAsync<Decimal>(
                 "SELECT SUM(Importe) FROM [DetalleVentaItem] WHERE [IdOrdenVenta] = " + IdOrdenVenta);
        }

        public Task<int> DeleteDetalleVentaAsync(DetalleVentaItem request)
        {
            return Database.DeleteAsync(request);
        }

        public Task<int> DeleteAllOrdenComprasAsync()
        {
            return Database.ExecuteAsync("DELETE FROM [DetalleVentaItem]");
        }
    }
}
