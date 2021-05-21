using ManageProd.Models;
using ManageProd.Services.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Splat;
using Acr.UserDialogs;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
using System.Threading.Tasks;

namespace ManageProd.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        public UserModel User { get; set; }
        public ICommand ExecuteLogin { get; set; }
        public ICommand ExecuteLoadRememberUser { get; set; }


        private IRoutingService _navigationService;

        public LoginPageViewModel(IRoutingService navigationService = null)
        {
            _navigationService = navigationService ?? Locator.Current.GetService<IRoutingService>();
            ExecuteLogin = new Command(async () => await LoginAsync());
            ExecuteLoadRememberUser = new Command(async () => await LoadRememberUserAsync());

        }

        private async Task LoginAsync()
        {
            try
            {
                if (User == null)
                {
                    throw new Exception("Favor de ingresar usuario y password");
                }
                if (string.IsNullOrWhiteSpace(User.User))
                {
                    throw new Exception("Favor de capturar el usuario");
                }
                if (string.IsNullOrWhiteSpace(User.Password))
                {
                    throw new Exception("Favor de capturar el password");
                }

                UsuarioItemDB database = await UsuarioItemDB.Instance;

                var usuario = await database.LoginUserdAsync(new UsuarioItem() { Usuario = User.User, Password = User.Password });
                if (usuario == null)
                {
                    throw new Exception("Clave y/o usuario incorrecto, favor de verificar.");
                }

                //Actualizamos el usuario a recordar en la BD
                if (User.Remember)
                {
                    //Actualizamos los usuarios en el campo remember false
                    var usuarios = await database.GetUsersAsync();
                    foreach (var item in usuarios)
                    {
                        item.Remember = false;
                        await database.SaveUserAsync(item);
                    }

                    //Asignamos al usuario actual el valor de remember en true
                    usuario.Remember = true;
                    await database.SaveUserAsync(usuario);
                }


                User.User = usuario.Usuario;
                User.Name = usuario.Nombre;

                

                App.IsUserLoggedIn = true;
                App.UserLogin = User;
                Application.Current.MainPage = new AppShell();
            
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }



        private async System.Threading.Tasks.Task LoadRememberUserAsync()
        {
            try
            {
                UsuarioItemDB database = await UsuarioItemDB.Instance;
                var Remember = await database.GetUserRememberAsync();

                if (Remember != null && Remember.Count > 0)
                {
                    User = new UserModel()
                    {
                        User = Remember[0].Usuario,
                        Name = Remember[0].Nombre,
                        Remember = Remember[0].Remember
                        
                    };
                }

            }
            catch (Exception)
            {

            }
        }

    }
}
