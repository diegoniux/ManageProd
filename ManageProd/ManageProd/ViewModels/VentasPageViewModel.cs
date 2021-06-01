using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
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
        public ObservableCollection<DetalleVentaItem> ListDetail { get; set; }

        public ClienteItem ClienteSelected { get; set; }
        public ProveedorItem ProveedorSelected { get; set; }
        public ProductoItem ProductoSelected { get; set; }
        public DetalleVentaItem DetalleSelected { get; set; }

        public ICommand SaveOrder { get; set; }
        public ICommand AddProduct { get; set; }
        public ICommand DeleteProduct { get; set; }
        public ICommand ProductoSelectionChanged { get; set; }
        public ICommand NewProduct { get; set; }

        public VentasPageViewModel()
        {
            IsBusy = false;
            HayOrden = false;
            IsSelected = false;
            Order = new OrdenVentaItem();
            
            ListDetail = new ObservableCollection<DetalleVentaItem>();
            ListProveedores = new ObservableCollection<ProveedorItem>();
            ListProduct = new ObservableCollection<ProductoItem>();
            ListClientes = new ObservableCollection<ClienteItem>();
            DetalleSelected = new DetalleVentaItem();
            ProductoSelected = new ProductoItem();
            ProveedorSelected = new ProveedorItem();

            SaveOrder = new Command(async () => await Save());
            AddProduct = new Command(async () => await Add());
            DeleteProduct = new Command(async () => await Delete());
            ProductoSelectionChanged = new Command(async () => await SelectChanged());
            NewProduct = new Command(async () => await NewProd());




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

        private async Task NewProd()
        {
            try
            {
                IsSelected = false;

                DetalleSelected = new DetalleVentaItem();
                ProductoSelected = new ProductoItem();
                ProveedorSelected = new ProveedorItem();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

        private async Task SelectChanged()
        {
            try
            {
                IsSelected = DetalleSelected?.IdDetalleVenta != 0;

                if (!IsSelected)
                {
                    return;
                }

                var idProveedor = DetalleSelected.IdProveedor;
                var idProducto = DetalleSelected.IdProducto;

                var ProdItem = ListProduct.ToList().Find(x => x.IdProducto == idProducto);
                ProductoSelected = ProdItem;

                var provItem = ListProveedores.ToList().Find(x => x.IdProveedor == idProveedor);
                ProveedorSelected = provItem;

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

        private async Task Delete()
        {
            try
            {
                var confirmaConf = new ConfirmConfig()
                {
                    Title = "Confirmación",
                    Message = "¿Deseas eliminar este producto de la lista?",
                    OkText = "Si",
                    CancelText = "No"
                };

                bool resp = await UserDialogs.Instance.ConfirmAsync(confirmaConf);
                if (!resp)
                {
                    return;
                }

                using (UserDialogs.Instance.Loading("Procesando...", null, null, true, MaskType.Black))
                {

                    if (DetalleSelected?.IdDetalleVenta != 0)
                    {
                        DetalleVentaItemDB OrderDB = await DetalleVentaItemDB.Instance;
                        await OrderDB.DeleteDetalleVentaAsync(DetalleSelected);
                        await LoadOrderDetail();
                        await ActualizarMontoTotal();
                        IsSelected = false;
                        DetalleSelected = new DetalleVentaItem();
                        ProductoSelected = new ProductoItem();
                        ProveedorSelected = new ProveedorItem();
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

        private async Task Add()
        {
            try
            {
                var confirmaConf = new ConfirmConfig()
                {
                    Title = "Confirmación",
                    Message = "¿Deseas agregar este producto en la lista?",
                    OkText = "Si",
                    CancelText = "No"
                };

                bool resp = await UserDialogs.Instance.ConfirmAsync(confirmaConf);
                if (!resp)
                {
                    return;
                }

                using (UserDialogs.Instance.Loading("Procesando...", null, null, true, MaskType.Black))
                {

                    if (DetalleSelected != null)
                    {
                        DetalleSelected.IdOrdenVenta = Order.IdOrdenVenta;
                        DetalleVentaItemDB DetalleDB = await DetalleVentaItemDB.Instance;
                        await DetalleDB.SaveDetalleVentaAsync(DetalleSelected);
                        await LoadOrderDetail();
                        await ActualizarMontoTotal();
                        IsSelected = false;
                        DetalleSelected = new DetalleVentaItem();
                        ProductoSelected = new ProductoItem();
                        ProveedorSelected = new ProveedorItem();
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

        private async Task<bool> ActualizarMontoTotal()
        {
            try
            {
                DetalleCompraItemDB DetalleDB = await DetalleCompraItemDB.Instance;
                var MontoTotal = await DetalleDB.GetSumaImporteAsync(Order.IdOrdenVenta);
                Order.MontoTotal = MontoTotal.ToString("$0,0.00");
                OrdenVentaItemDB OrderDB = await OrdenVentaItemDB.Instance;
                await OrderDB.SaveOrdenVentaAsync(Order);

                MessagingCenter.Send<VentasPageViewModel, OrdenVentaItem>(this, "MontoChanged", Order);

                return true;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
                return false;
            }
        }

        private async Task Save()
        {
            try
            {
                var confirmaConf = new ConfirmConfig()
                {
                    Title = "Confirmación",
                    Message = "¿Deseas dar de alta la orden de venta capturada?",
                    OkText = "Si",
                    CancelText = "No"
                };

                bool resp = await UserDialogs.Instance.ConfirmAsync(confirmaConf);
                if (!resp)
                {
                    return;
                }

                using (UserDialogs.Instance.Loading("Procesando...", null, null, true, MaskType.Black))
                {
                    if (Order != null)
                    {
                        Order.IdCliente = ClienteSelected.IdCliente;
                        OrdenVentaItemDB OrderDB = await OrdenVentaItemDB.Instance;
                        await OrderDB.SaveOrdenVentaAsync(Order);
                        Order = await OrderDB.GetOrdenVentaIdsAsync(Order.IdOrdenVenta);
                        await LoadProductsProveedor();
                        HayOrden = true;
                    }
                }
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
                if (ProveedorSelected == null)
                {
                    return;
                }

                ListProduct = new ObservableCollection<ProductoItem>();
                if (ProveedorSelected.IdProveedor != 0)
                {
                    ProductoItemDB ProductoDB = await ProductoItemDB.Instance;
                    ListProduct = new ObservableCollection<ProductoItem>(await ProductoDB.GetProductsIdProveedorAsync(ProveedorSelected.IdProveedor));
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }

        }

        private async Task LoadOrderDetail()
        {
            try
            {
                ListDetail = new ObservableCollection<DetalleVentaItem>();
                if (Order?.IdOrdenVenta != 0)
                {
                    DetalleVentaItemDB DetalleOrdenDB = await DetalleVentaItemDB.Instance;
                    ListDetail = new ObservableCollection<DetalleVentaItem>(await DetalleOrdenDB.GetDetalleVentaIdAsync(Order.IdOrdenVenta));
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }

        }

    }
}
