using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    public class ClienteItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdCliente { get; set; }

        public string Nombre { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public string Telefono { get; set; }

        public string CorreoElectronico { get; set; }

        public DireccionItem Direccion { get; set; }
    }
}
