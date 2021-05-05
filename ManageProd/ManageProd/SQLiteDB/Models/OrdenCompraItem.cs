using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    public class OrdenCompraItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdOrdenCompra { get; set; }

        public int IdProveedor { get; set; }

        public DateTime FechaCompra { get; set; }

        public string Notas { get; set; }

        public string MontoTotal { get; set; }

    }
}
