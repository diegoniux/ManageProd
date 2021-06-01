using System;
using Acr.UserDialogs;
using ManageProd.Renderers;
using ManageProd.SQLiteDB.Models;
using ManageProd.ViewModels;
using Xamarin.Forms;

namespace ManageProd.Views
{
    public partial class VentasPage : ContentPage
    {

        public VentasPageViewModel ViewModel { get; set; }

        public VentasPage()
        {
            InitializeComponent();
            ViewModel = new VentasPageViewModel();
            BindingContext = ViewModel;
        }
    }
}
