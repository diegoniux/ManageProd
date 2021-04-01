using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    public class ProductoItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdProducto { get; set; }

        public string Descripcion { get; set; }

        public decimal PesoBruto { get; set; }

        public decimal PesoEmpaque { get; set; }

        public decimal PesoNeto { get; set; }
    }
}
