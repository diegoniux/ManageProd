using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ManageProd.Models;
using ManageProd.ViewModels;
using ManageProd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ManageProd
{
    [QueryProperty(nameof(UserLogedIn), "UserLogin")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public static UserModel UserLogedIn { get; set; }

        public AppShell()
        {
            InitializeComponent();

            UserLogedIn = App.UserLogin;
            

            Routing.RegisterRoute("main/login", typeof(LoginPage));
            BindingContext = this;

            CheckLogin();

        }

        public ICommand ExecuteLogout => new Command(async () =>
        {
            App.IsUserLoggedIn = false;
            await GoToAsync("main/login");
        });

        private async Task CheckLogin()
        {
            if (App.IsUserLoggedIn)
            {
                await Shell.Current.GoToAsync($"//main");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}