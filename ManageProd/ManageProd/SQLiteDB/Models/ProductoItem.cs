using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    [Table("ProductoItem")]
    public class ProductoItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public string Producto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }

        public decimal Tara { get; set; }

    }
}
