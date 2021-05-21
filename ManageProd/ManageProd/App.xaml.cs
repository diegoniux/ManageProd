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
