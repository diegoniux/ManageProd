using ManageProd.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ManageProd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        internal LoginPageViewModel ViewModel { get; set; } = Locator.Current.GetService<LoginPageViewModel>();

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            var viewModel = (LoginPageViewModel)BindingContext;
            viewModel.ExecuteLoadRememberUser.Execute(null);
            BindingContext = viewModel;

            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}