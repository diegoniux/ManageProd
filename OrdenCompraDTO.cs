using System;
using System.Collections.Generic;
using ManageProd.SQLiteDB.Models;

namespace ManageProd.Models.DTO
{
    public class OrdenCompraDTO
    {
        public OrdenCompraItem OrdenCompra { get; set; }
        public List<DetalleCompraItem> DetalleCompra { get; set; }

        public OrdenCompraDTO()
        {
            this.OrdenCompra = new OrdenCompraItem();
            this.DetalleCompra = new List<DetalleCompraItem>();
        }
    }
}
