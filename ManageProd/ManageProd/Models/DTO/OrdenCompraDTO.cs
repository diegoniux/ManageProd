using System;
using System.Collections.Generic;
using ManageProd.SQLiteDB.Models;

namespace ManageProd.Models.DTO
{
    public class OrdenCompraDTO
    {
        public string UsuarioCreacion { get; set; }
        public string Proveedor { get; set; }
        public string Notas { get; set; }
        public string FechaCompra { get; set; }
        public string MontoTotal { get; set; }
        public string Cantidad { get; set; }
        public string MontoLetra { get; set; }
        public List<Producto> Productos { get; set; }


        public OrdenCompraDTO()
        {
            this.UsuarioCreacion = string.Empty;
            this.Proveedor = string.Empty;
            this.FechaCompra = DateTime.Now.ToString("dd/MMMM/yyyy");
            this.MontoTotal = "$0.00";
            this.MontoLetra = "Cero pesos 00/100 MN";
            this.Notas = string.Empty;
            this.Productos = new List<Producto>();
        }

        public class Producto
        {
            public string ProductoDesc { get; set; }
            public string Cantidad { get; set; }
            public string PesoBruto { get; set; }
            public string PesoNeto { get; set; }
            public string Precio { get; set; }
            public string Importe { get; set; }


        }

    }
}
