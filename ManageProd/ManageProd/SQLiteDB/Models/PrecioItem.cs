using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    public class PrecioItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdPrecio { get; set; }

        public decimal Precio { get; set; }

        public decimal Descuento { get; set; }
    }
}
