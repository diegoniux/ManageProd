using System;
using System.Collections.Generic;
using ManageProd.ViewModels;
using Xamarin.Forms;

namespace ManageProd.Views
{
    public partial class ComprasPage : ContentPage
    {
        public ComprasPageViewModel ViewModel { get; set; }

        public ComprasPage()
        {
            InitializeComponent();
            ViewModel = new ComprasPageViewModel();
            BindingContext = ViewModel;
        }

        void Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            //try
            //{
            //    ViewModel.DetalleSelected.Precio = 100;
            //    BindingContext = ViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
    }
}
