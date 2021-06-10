using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    [Table("OrdenVentaItem")]
    public class OrdenVentaItem
    {
        public OrdenVentaItem()
        {
            this.IdOrdenVenta = 0;
            this.IdCliente = 0;
            this.FechaVenta = DateTime.Now;
            this.MontoTotal = "0";
        }

        [PrimaryKey, AutoIncrement, Column("IdOrdenCompra")]
        public int IdOrdenVenta { get; set; }

        [ForeignKey(typeof(ClienteItem)), Column("IdCliente")]
        public int IdCliente { get; set; }

        [Column("FechaVenta")]
        public DateTime FechaVenta { get; set; }

        [Column("MontoTotal")]
        public string MontoTotal { get; set; }

        [Column("CreditosAnteriores")]
        public string CreditosAnteriores { get; set; }

        [Column("ExistenciasAnteriores")]
        public string ExistenciasAnteriores { get; set; }

    }
}
