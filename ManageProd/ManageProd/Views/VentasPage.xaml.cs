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
                this.Precio.Text = producto.PrecioCompra.ToString();
                MessagingCenter.Send<VentasPage, ProductoItem>(this, "productoChanged", producto);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }
    }
}
