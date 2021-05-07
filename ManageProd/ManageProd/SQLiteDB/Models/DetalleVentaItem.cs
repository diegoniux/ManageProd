using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    [Table("DetalleVentaItem")]
    public class DetalleVentaItem
    {
        public DetalleVentaItem()
        {
            this.IdDetalleVenta = 0;
            this.IdOrdenCompra = 0;
            this.IdProducto = 0;
            this.Cantidad = 0;
            this.Precio = 0;
            this.Descuento = 0;
            this.Importe = 0;
        }

        [PrimaryKey, AutoIncrement, Column("IdDetalleVenta")]
        public int IdDetalleVenta { get; set; }

        [ForeignKey(typeof(OrdenCompraItem)), Column("IdOrdenCompra")]
        public int IdOrdenCompra { get; set; }

        [ForeignKey(typeof(ProductoItem)), Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("Cantidad")]
        public decimal Cantidad { get; set; }

        [Column("Precio")]
        public decimal Precio { get; set; }

        [Column("Descuento")]
        public decimal Descuento { get; set; }

        [Column("Importe")]
        public decimal Importe { get; set; }
    }
}
