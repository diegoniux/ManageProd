using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    [Table("DetalleCompraItem")]
    public class DetalleCompraItem
    {
        public DetalleCompraItem()
        {
            this.IdDetalleCompra = 0;
            this.IdOrdenCompra = 0;
            this.IdProducto = 0;
            this.Cantidad = 0;
            this.PesoBruto = 0;
            this.PesoNeto = 0;
            this.Precio = 0;
            this.Importe = 0;
        }

        [PrimaryKey, AutoIncrement, Column("IdDetalleCompra")]
        public int IdDetalleCompra { get; set; }

        [ForeignKey(typeof(OrdenCompraItem)), Column("IdOrdenCompra")]
        public int IdOrdenCompra { get; set; }

        [ForeignKey(typeof(ProductoItem)), Column("IdProducto")]       
        public int IdProducto { get; set; }

        [Column("Producto")]
        public string Producto { get; set; }

        [Column("Cantidad")]
        public decimal Cantidad { get; set; }

        [Column("PesoBruto")]
        public decimal PesoBruto { get; set; }

        [Column("PesoNeto")]
        public decimal PesoNeto { get; set; }

        [Column("Precio")]
        public decimal Precio { get; set; }

        [Column("Importe")]
        public decimal Importe { get; set; }
    }
}
