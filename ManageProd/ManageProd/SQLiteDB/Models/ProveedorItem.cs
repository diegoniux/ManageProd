using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    public class ProveedorItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdProveedor { get; set; }
        
        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public string CorreoElectronico { get; set; }

        public string NotasProveedor { get; set; }       
    }
}
