using System;
using System.Collections.Generic;
using System.Globalization;
using Acr.UserDialogs;
using CsvHelper;
using ManageProd.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using ManageProd.ViewModels;
using Splat;

namespace ManageProd.Views
{
    public partial class GestionProductosPage : ContentPage
    {
        internal GestionProductosPageViewModel ViewModel { get; set; } = Locator.Current.GetService<GestionProductosPageViewModel>();

        public GestionProductosPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }

    }
}
