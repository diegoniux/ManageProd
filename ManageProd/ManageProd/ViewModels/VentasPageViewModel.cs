using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
using ManageProd.Views;
using PropertyChanged;
using Xamarin.Forms;

namespace ManageProd.ViewModels
{    
    public class VentasPageViewModel : NotificationObject
    {
        public bool IsBusy { get; set; }
        public bool HayOrden { get; set; }
        public bool IsSelected { get; set; }

        public OrdenVentaItem Order { get; set; }
        public ObservableCollection<ClienteItem> ListClientes { get; set; }
        public ObservableCollection<ProveedorItem> ListProveedores { get; set; }
        public ObservableCollection<ProductoItem> ListProduct { get; set; }

        public VentasPageViewModel()
        {
            IsBusy = false;
            HayOrden = false;
            IsSelected = false;
            Order = new OrdenVentaItem();
            ListClientes = new ObservableCollection<ClienteItem>();           

            LoadCatalogs();
        }

        private async Task LoadCatalogs()
        {
            try
            {
                ProveedorItemDB ProveedorDB = await ProveedorItemDB.Instance;
                ClienteItemDB clienteDB = await ClienteItemDB.Instance;
                ListProveedores = new ObservableCollection<ProveedorItem>(await ProveedorDB.GetProveedoresAsync());
                ListClientes = new ObservableCollection<ClienteItem>(await clienteDB.GetClientsAsync());             
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }

        }

        private async Task LoadProductsProveedor()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }

        }
    }
}
