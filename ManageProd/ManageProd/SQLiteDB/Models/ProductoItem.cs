using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    public class ProductoItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public string Producto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }

    }
}
