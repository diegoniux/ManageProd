using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageProd.SQLiteDB.Models;
using SQLite;

namespace ManageProd.SQLiteDB.Data
{
    public class UsuarioItemDB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<UsuarioItemDB> Instance = new AsyncLazy<UsuarioItemDB>(async () =>
        {
            var instance = new UsuarioItemDB();
            CreateTableResult result = await Database.CreateTableAsync<UsuarioItem>();
            return instance;
        });

        public UsuarioItemDB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<UsuarioItem>> GetUsersAsync()
        {
            return Database.Table<UsuarioItem>().ToListAsync();
        }

        public async Task<UsuarioItem> LoginUserdAsync(UsuarioItem user)
        {
            UsuarioItem res = await Database.Table<UsuarioItem>().Where(i => i.Usuario == user.Usuario && i.Password == user.Password).FirstOrDefaultAsync();
            return res;
            //return Database.QueryAsync<UsuarioItem>("SELECT * FROM [UsuarioItem] WHERE [Password] = 0");
        }

        public Task<List<UsuarioItem>> GetUserRememberAsync(    )
        {
            return Database.QueryAsync<UsuarioItem>("SELECT * FROM [UsuarioItem] WHERE [Remember] = 1");
        }


        public Task<UsuarioItem> GetUserAsync(int id)
        {
            return Database.Table<UsuarioItem>().Where(i => i.IdUsuario == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(UsuarioItem item)
        {
            if (item.IdUsuario != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteUserAsync(UsuarioItem item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<int> DeleteAllUserAsync()
        {
            return Database.ExecuteAsync("DELETE FROM [UsuarioItem]");
        }
    }
}
