using System;
using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    public class UsuarioItem
    {
        [PrimaryKey, AutoIncrement]
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
