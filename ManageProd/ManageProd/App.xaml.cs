using Acr.UserDialogs;
using ManageProd.Models;
using ManageProd.Services.Routing;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
using ManageProd.ViewModels;
using ManageProd.Views;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ManageProd
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static UserModel UserLogin { get; set; }

        public App()
        {
            InitializeComponent();

            if (IsUserLoggedIn)
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        protected override async void OnStart()
        {
            ///////////////
            ///Clientes
            //////////////
            ClienteItemDB clientesDB = await ClienteItemDB.Instance;
            var Clientes = await clientesDB.GetClientsAsync();

            if (Clientes == null || Clientes.Count == 0)
            {
                //incializamos la Bd con los usuario de la app
                List<ClienteItem> clientes = new List<ClienteItem>();
                clientes.Add(new ClienteItem()
                {
                    IdCliente = 1,
                    Nombre = "Ruta 01"
                });
                clientes.Add(new ClienteItem()
                {
                    IdCliente = 2,
                    Nombre = "Ruta 02"
                });
                clientes.Add(new ClienteItem()
                {
                    IdCliente = 3,
                    Nombre = "Ruta 03"
                });
                clientes.Add(new ClienteItem()
                {
                    IdCliente = 4,
                    Nombre = "Ruta 04"
                });
                clientes.Add(new ClienteItem()
                {
                    IdCliente = 5,
                    Nombre = "Ruta 05"
                });
                clientes.Add(new ClienteItem()
                {
                    IdCliente = 6,
                    Nombre = "Ruta 06"
                });

                foreach (var item in clientes)
                {
                    await clientesDB.InsertClientsAsync(item);
                }
            }


            UsuarioItemDB database = await UsuarioItemDB.Instance;
            var Usuarios = await database.GetUsersAsync();

            if (Usuarios == null || Usuarios.Count == 0 )
            {
                //incializamos la Bd con los usuario de la app
                var Usuario = new UsuarioItem()
                {
                    Nombre = "Nombre de Usuario 01",
                    Usuario = "Usuario01",
                    Password = "Tqwerty1",
                    Remember = false
                };

                var Usuario2 = new UsuarioItem()
                {
                    Nombre = "Nombre de Usuario 02",
                    Usuario = "Usuario02",
                    Password = "Tqwerty1",
                    Remember = false
                };


                await database.SaveUserAsync(Usuario);
                await database.SaveUserAsync(Usuario2);

            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
