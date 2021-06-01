using SQLite;

namespace ManageProd.SQLiteDB.Models
{
    [Table("ClienteItem")]
    public class ClienteItem
    {
        [PrimaryKey, AutoIncrement, Column("IdCliente")]
        public int IdCliente { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("ApellidoPaterno")]
        public string ApellidoPaterno { get; set; }

        [Column("ApellidoMaterno")]
        public string ApellidoMaterno { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; }

        [Column("CorreoElectronico")]
        public string CorreoElectronico { get; set; }

        public ClienteItem()
        {
            this.IdCliente = 0;
            this.Nombre = string.Empty;
            this.ApellidoPaterno = string.Empty;
            this.ApellidoMaterno = string.Empty;
            this.Telefono = string.Empty;
            this.CorreoElectronico = string.Empty;
        }
    }
}
