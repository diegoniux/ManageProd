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
            await clientesDB.DeleteAllClientsAsync();
            var Clientes = await clientesDB.GetClientsAsync();

            if (Clientes == null || Clientes.Count == 0)
            {
                //incializamos la Bd con los usuario de la app
                List<ClienteItem> clientes = new List<ClienteItem>();
                clientes.Add(new ClienteItem()
                {
                    Nombre = "Ruta 01"
                });
                clientes.Add(new ClienteItem()
                {
                    Nombre = "Ruta 02"
                });
                clientes.Add(new ClienteItem()
                {
                    Nombre = "Ruta 03"
                });
                clientes.Add(new ClienteItem()
                {
                    Nombre = "Ruta 04"
                });
                clientes.Add(new ClienteItem()
                {
                    Nombre = "Ruta 05"
                });
                clientes.Add(new ClienteItem()
                {
                    Nombre = "Ruta 06"
                });

                foreach (var item in clientes)
                {
                    await clientesDB.InsertClientsAsync(item);
                }
            }



            UsuarioItemDB database = await UsuarioItemDB.Instance;
           await database.DeleteAllUserAsync();
            var Usuarios = await database.GetUsersAsync();

            if (Usuarios == null || Usuarios.Count == 0 )
            {
                //incializamos la Bd con los usuario de la app
                var Usuario = new UsuarioItem()
                {
                    Nombre = "Jorge Alberto Peréz Villeda",
                    Usuario = "jperez",
                    Password = "magochi04",
                    Remember = false
                };

                var Usuario2 = new UsuarioItem()
                {
                    Nombre = "Jorge Alberto Sánchez Molina",
                    Usuario = "jsanchez",
                    Password = "rosario14",
                    Remember = false
                };

                var Usuario3 = new UsuarioItem()
                {
                    Nombre = "Jaime Castorena Villeda",
                    Usuario = "jcastorena",
                    Password = "montevideo14",
                    Remember = false
                };


                await database.SaveUserAsync(Usuario);
                await database.SaveUserAsync(Usuario2);
                await database.SaveUserAsync(Usuario3);
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
