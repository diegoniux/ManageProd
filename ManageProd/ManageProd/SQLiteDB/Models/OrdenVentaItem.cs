using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    class OrdenVentaItem
    {
        [PrimaryKey, AutoIncrement] //foranea
        public int IdOrdenCompra { get; set; }

        [PrimaryKey, AutoIncrement] //foranea
        public int IdCliente { get; set; }

        public DateTime Fecha { get; set; }

        public decimal MontoTotal { get; set; }

    }
}
