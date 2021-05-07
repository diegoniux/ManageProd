using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    [Table("OrdenCompraItem")]
    public class OrdenCompraItem
    {
        public OrdenCompraItem()
        {
            this.IdOrdenCompra = 0;
            this.IdProveedor = 0;
            this.FechaCompra = DateTime.Now;
            this.Notas = string.Empty;
            this.MontoTotal = string.Empty;
        }

        [PrimaryKey, AutoIncrement, Column("IdOrdenCompra")]
        public int IdOrdenCompra { get; set; }

        [ForeignKey(typeof(ProveedorItem)), Column("IdProducto")]
        public int IdProveedor { get; set; }

        [Column("FechaCompra")]
        public DateTime FechaCompra { get; set; }

        [Column("Notas")]
        public string Notas { get; set; }

        [Column("MontoTotal")]
        public string MontoTotal { get; set; }

    }
}
