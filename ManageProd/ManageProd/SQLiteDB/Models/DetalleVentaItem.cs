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
            this.IdOrdenVenta = 0;
            this.IdProducto = 0;
            this.IdProveedor = 0;
            this.Proveedor = "";
            this.Cantidad = 0;
            this.Precio = 0;
            this.Descuento = 0;
            this.Importe = 0;
            this.PesoNeto = 0;
        }

        [PrimaryKey, AutoIncrement, Column("IdDetalleVenta")]
        public int IdDetalleVenta { get; set; }

        [ForeignKey(typeof(OrdenVentaItem)), Column("IdOrdenVenta")]
        public int IdOrdenVenta { get; set; }

        [Column("IdProveedor")]
        public int IdProveedor { get; set; }

        [Column("Proveedor")]
        public string Proveedor { get; set; }

        [ForeignKey(typeof(ProductoItem)), Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("Producto")]
        public string Producto { get; set; }

        [Column("Cantidad")]
        public decimal Cantidad { get; set; }

        [Column("PesoNeto")]
        public decimal PesoNeto { get; set; }

        [Column("Precio")]
        public decimal Precio { get; set; }

        [Column("Descuento")]
        public decimal Descuento { get; set; }

        [Column("Importe")]
        public decimal Importe { get; set; }

        [Column("CreditosAnteriores")]
        public string CreditosAnteriores { get; set; }

        [Column("ExistenciasAnteriores")]
        public string ExistenciasAnteriores { get; set; }
    }
}
