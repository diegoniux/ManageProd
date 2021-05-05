using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    class DetalleCompraItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdDetalleCompra { get; set; }

        [PrimaryKey, AutoIncrement] //foranea
        public int IdOrdenCompra { get; set; }

        [PrimaryKey, AutoIncrement] //foranea
        public int IdProducto { get; set; }

        public decimal cantidad { get; set; }

        public decimal PesoBruto { get; set; }

        public decimal PesoNeto { get; set; }

        public decimal Precio { get; set; }

        public decimal Importe { get; set; }
    }
}
