using System;
using System.Collections.Generic;
using System.Text;
using ManageProd.ViewModels;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    [Table("ProductoItem")]
    public class ProductoItem: NotificationObject
    {
        [PrimaryKey, AutoIncrement, Column("IdProducto")]
        public int IdProducto { get; set; }

        [ForeignKey(typeof(ProveedorItem)), Column("IdProveedor")]
        public int IdProveedor { get; set; }

        [Column("Producto")]
        public string Producto { get; set; }

        [Column("PrecioCompra")]
        public decimal PrecioCompra { get; set; }

        [Column("PrecioVenta")]
        public decimal PrecioVenta { get; set; }

        [Column("Tara")]
        public decimal Tara { get; set; }

        [Column("Existencia")]
        public decimal Existencia { get; set; }

    }
}
