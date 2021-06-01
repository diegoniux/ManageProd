using System;
using System.Collections.Generic;
using System.Text;
using ManageProd.ViewModels;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    [Table("ProveedorItem")]
    public class ProveedorItem: NotificationObject
    {
        public ProveedorItem()
        {
            this.IdProveedor = 0;
            this.Proveedor = string.Empty;
            this.Telefono = string.Empty;
            this.Mail = string.Empty;
            this.NotasProveedor = string.Empty;
        }

        [PrimaryKey, AutoIncrement, Column("IdProveedor")]
        public int IdProveedor { get; set; }

        [Column("Proveedor")]
        public string Proveedor { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; }

        [Column("Mail")]
        public string Mail { get; set; }

        [Column("NotasProveedor")]
        public string NotasProveedor { get; set; }     
    }
}
    