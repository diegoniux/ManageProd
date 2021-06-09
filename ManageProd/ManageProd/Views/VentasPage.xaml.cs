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

            MessagingCenter.Subscribe<VentasPageViewModel, OrdenVentaItem>(this, "MontoChanged", (sender, arg) =>
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
                ProveedorItem proveedor = picker.SelectedItem as ProveedorItem;
                MessagingCenter.Send<VentasPage, ProveedorItem>(this, "proveedorChanged", proveedor);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

        async void pickerProducto_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                var picker = sender as Picker;
                ProductoItem producto = picker.SelectedItem as ProductoItem;
                this.Precio.Text = producto.PrecioVenta.ToString();
                MessagingCenter.Send<VentasPage, ProductoItem>(this, "productoChanged", producto);
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

                decimal.TryParse(Descuento.Text, out var descuento);
                decimal.TryParse(PesoNeto.Text, out var pesoNeto);
                decimal.TryParse(Precio.Text, out var precio);

                if (ViewModel.ProductoSelected == null)
                {
                    return;
                }

                if (pesoNeto <= 0)
                {
                    return;
                }

                var importe = (pesoNeto * precio) - descuento;
                if (importe < 0)
                {
                    importe = 0;
                }

                Importe.Text = importe.ToString();

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

    }
}
