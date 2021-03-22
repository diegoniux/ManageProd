using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ManageProd.Models;
using ManageProd.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ManageProd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public static UserModel Usuario { get; set; }

        public AppShell()
        {
            InitializeComponent();

            App.IsUserLoggedIn = false;
            Routing.RegisterRoute("main/login", typeof(LoginPage));
            BindingContext = this;
        }

        public ICommand ExecuteLogout => new Command(async () =>
        {
            App.IsUserLoggedIn = false;
            await GoToAsync("main/login");
        });
    }
}