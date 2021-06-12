using System;
using System.Collections.Generic;
using System.Text;

namespace ManageProd.Models.DTO
{
    public class OrdenVentaDTO
    {

        public OrdenVentaDTO()
        {
            this.UsuarioCreacion = string.Empty;
            this.Cliente = string.Empty;
            this.FechaVenta = DateTime.Now.ToString("dd/MMMM/yyyy");
            this.MontoTotal = "$0.00";
            this.MontoLetra = "Cero pesos 00/100 MN";
            this.CreditosAnteriores = string.Empty;
            this.ExistenciasAnteriores = string.Empty;
            this.Productos = new List<ProductoVenta>();
        }
        public string UsuarioCreacion { get; set; }
        public string Cliente { get; set; }
        public string FechaVenta { get; set; }
        public string MontoTotal { get; set; }
        public string MontoLetra { get; set; }
        public string CreditosAnteriores { get; set; }
        public string ExistenciasAnteriores { get; set; }
        public List<ProductoVenta> Productos { get; set; }

        public class ProductoVenta
        {
            public string ProductoDesc { get; set; }
            public string Cantidad { get; set; }
            public string PesoNeto { get; set; }
            public string Precio { get; set; }
            public string Descuento { get; set; }
            public string Importe { get; set; }
            public ProductoVenta()
            {
                this.ProductoDesc = string.Empty;
                this.Cantidad = string.Empty;
                this.PesoNeto = string.Empty;
                this.Precio = string.Empty;
                this.Descuento = string.Empty;
                this.Importe = string.Empty;
            }

        }
    }
}
