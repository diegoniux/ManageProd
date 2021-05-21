using System;
using ManageProd.SQLiteDB.Models;

namespace ManageProd.Models
{
    public class LayoutModel
    {
        public string IdProducto { get; set; }
        public string Producto { get; set; }
        public string ProveedorId { get; set; }
        public string Proveedor { get; set; }
        public string PrecioCompra { get; set; }
        public string PrecioVenta { get; set; }
        public string Tara { get; set; }
    }
}
