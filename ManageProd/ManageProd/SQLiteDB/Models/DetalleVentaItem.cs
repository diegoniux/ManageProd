using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    class DetalleVentaItem
    {
        [PrimaryKey, AutoIncrement] //primaria
        public int IdDetalleVenta { get; set; }

        [PrimaryKey, AutoIncrement] //foranea
        public int IdOrdenCompra { get; set; }

        [PrimaryKey, AutoIncrement] //foranea
        public int IdProducto { get; set; }

        public decimal Cantidad { get; set; }       

        public decimal Precio { get; set; }

        public decimal Descuento { get; set; }

        public decimal Importe { get; set; }
    }
}
