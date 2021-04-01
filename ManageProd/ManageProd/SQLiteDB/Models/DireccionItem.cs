using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ManageProd.SQLiteDB.Models
{
    [Table("DireccionItem")]
    public class DireccionItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdDireccion { get; set; }

        //[ForeignKey(typeof(ClienteItem))]
        //public int IdCliente { get; set; }

        public string Calle { get; set; }

        public string Colonia { get; set; }

        public string NumeroExterior { get; set; }

        public string NumeroInterior { get; set; }

        public string CodigoPostal { get; set; }

        public string NotasUbicacion { get; set; }
    }
}
