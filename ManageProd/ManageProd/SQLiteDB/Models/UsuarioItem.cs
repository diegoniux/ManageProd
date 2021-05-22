using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    [Table("UsuarioItem")]
    public class UsuarioItem
    {
        public UsuarioItem()
        {
            this.IdUsuario = 0;
            this.Usuario = string.Empty;
            this.Nombre = string.Empty;
            this.Password = string.Empty;
            this.Remember = false;
        }

        [PrimaryKey, Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("Usuario")]
        public string Usuario { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Remember")]
        public bool Remember { get; set; }
    }
}
