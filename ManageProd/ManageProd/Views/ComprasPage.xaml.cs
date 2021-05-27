using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using ManageProd.Renderers;
using ManageProd.SQLiteDB.Models;
using ManageProd.ViewModels;
using Newtonsoft.Json;
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

            MessagingCenter.Subscribe<ComprasPageViewModel, OrdenCompraItem>(this, "MontoChanged", (sender, arg) =>
            {
                var orden = arg;
                this.Monto.Text = orden.MontoTotal;
            });

        }

        async void Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                var picker = sender as Picker;
                ProductoItem producto = picker.SelectedItem as ProductoItem;
                this.Precio.Text = "150";
                MessagingCenter.Send<ComprasPage, ProductoItem>(this, "productoChanged" , producto);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }

        }

        async void TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                var entry = sender as EntryIcon;
                if (entry.Text == string.Empty)
                {
                    Importe.Text = "0";
                    return;
                }

                int.TryParse(Cantidad.Text, out var cantidad);
                int.TryParse(PesoBruto.Text, out var pesoBruto);
                int.TryParse(PesoNeto.Text, out var pesoNeto);
                int.TryParse(Precio.Text, out var precio);

                var importe = cantidad * pesoNeto * precio;
                Importe.Text = importe.ToString();



            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
            

        }
    }
}
