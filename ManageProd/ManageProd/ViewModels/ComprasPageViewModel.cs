using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
using ReactiveUI;
using Xamarin.Forms;

namespace ManageProd.ViewModels
{
    public class ComprasPageViewModel: NotificationObject
    {
        public bool IsBusy { get; set; }
        public bool HayOrden { get; set; }
        public OrdenCompraItem Order { get; set; }
        public ObservableCollection<DetalleCompraItem> ListDetail { get; set; }
        public ObservableCollection<ProveedorItem> ListProveedores { get; set; }
        public ObservableCollection<ProductoItem> ListProduct { get; set; }
        public DetalleCompraItem DetalleSelected { get; set; }

        public ProveedorItem ProveedorSelected { get; set; }

        // Personalizamos la propiedad psra que asigne el precio al propducto
        public ProductoItem ProductoSelected { get; set; }

        public ICommand SaveOrder { get; set; }
        public ICommand AddProduct { get; set; }
        public ICommand DeleteProduct { get; set; }


        public ComprasPageViewModel()
        {
            IsBusy = false;
            HayOrden = false;
            Order = new OrdenCompraItem();
            ListDetail = new ObservableCollection<DetalleCompraItem>();
            ListProveedores = new ObservableCollection<ProveedorItem>();
            ListProduct = new ObservableCollection<ProductoItem>();
            DetalleSelected = new DetalleCompraItem();
            ProductoSelected = new ProductoItem();
            ProveedorSelected = new ProveedorItem();

            SaveOrder = new Command(async () => await Save());
            AddProduct = new Command(async () => await Add());
            DeleteProduct = new Command(async () => await Delete());

            LoadCatalogs();

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

                    if (DetalleSelected?.IdDetalleCompra != 0)
                    {
                        DetalleCompraItemDB OrderDB = await DetalleCompraItemDB.Instance;
                        await OrderDB.DeleteDetalleCompraAsync(DetalleSelected);
                        await LoadOrderDetail();
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
                        DetalleSelected.IdProducto = ProductoSelected.IdProducto;
                        DetalleSelected.Producto = ProductoSelected.Producto;
                        DetalleCompraItemDB OrderDB = await DetalleCompraItemDB.Instance;
                        await OrderDB.SaveDetalleCompraAsync(DetalleSelected);
                        await LoadOrderDetail();
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

        private async Task Save()
        {
            try
            {
                var confirmaConf = new ConfirmConfig()
                {
                    Title = "Confirmación",
                    Message = "¿Deseas dar de alta la orden de compra capturada?",
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
                        Order.IdProveedor = ProveedorSelected.IdProveedor;
                        OrdenCompraItemDB OrderDB = await OrdenCompraItemDB.Instance;
                        await OrderDB.SaveOrdenVentaAsync(Order);
                        Order = await OrderDB.GetOrdenCompraIdsAsync(Order.IdOrdenCompra);
                        await LoadProductsProveedor();
                        HayOrden = true;
                        await UserDialogs.Instance.AlertAsync("Orden de Compra regitrada correctamente.", "Aviso", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

        private async Task LoadCatalogs()
        {
            try
            {
                ProveedorItemDB ProveedorDB = await ProveedorItemDB.Instance;
                ListProveedores = new ObservableCollection<ProveedorItem>(await ProveedorDB.GetProveedoresAsync());
                await LoadProductsProveedor();
                await LoadOrderDetail();
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
                ListProduct = new ObservableCollection<ProductoItem>();
                if (Order.IdProveedor != 0)
                {
                    ProductoItemDB ProductoDB = await ProductoItemDB.Instance;
                    ListProduct = new ObservableCollection<ProductoItem>(await ProductoDB.GetProductsIdProveedorAsync(Order.IdProveedor));
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
                ListDetail = new ObservableCollection<DetalleCompraItem>();
                if (Order?.IdOrdenCompra != 0)
                {
                    DetalleCompraItemDB DetalleOrdenDB = await DetalleCompraItemDB.Instance;
                    ListDetail = new ObservableCollection<DetalleCompraItem>(await DetalleOrdenDB.GetDetalleCompraIdOrdenCompraAsync(Order.IdOrdenCompra));
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }

        }

    }
}
