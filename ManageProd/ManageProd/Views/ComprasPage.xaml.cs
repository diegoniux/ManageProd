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
    }
}
