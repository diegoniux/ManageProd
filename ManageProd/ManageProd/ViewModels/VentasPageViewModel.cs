using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
using ManageProd.Views;
using Xamarin.Forms;
using ManageProd.Models.DTO;
using ManageProd.Shared;
using ManageProd.Templates;
using Rg.Plugins.Popup.Services;
using ManageProd.Views.Popup;

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

        public ICommand FinishOrder { get; set; }

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
            ClienteSelected = new ClienteItem();

            SaveOrder = new Command(async () => await Save());
            AddProduct = new Command(async () => await Add());
            DeleteProduct = new Command(async () => await Delete());
            ProductoSelectionChanged = new Command(async () => await SelectChanged());
            NewProduct = new Command(async () => await NewProd());
            FinishOrder = new Command(async () => await PrintOrder());

            LoadCatalogs();

            MessagingCenter.Subscribe<VentasPage, ProductoItem>(this, "productoChanged", (sender, arg) =>
            {
                var producto = arg;
                var detalle = DetalleSelected;
                detalle.IdProducto = producto.IdProducto;
                detalle.Producto = producto.Producto;
                detalle.Precio = producto.PrecioVenta;

                DetalleSelected = detalle;
            });

            MessagingCenter.Subscribe<VentasPage, ProveedorItem>(this, "proveedorChanged", async (sender, arg) =>
            {
                var proveedor = arg;
                var detalle = DetalleSelected;
                detalle.IdProveedor = proveedor.IdProveedor;
                detalle.Proveedor = proveedor.Proveedor;

                ProductoItemDB ProductoDB = await ProductoItemDB.Instance;
                ListProduct = new ObservableCollection<ProductoItem>(await ProductoDB.GetProductsIdProveedorAsync(proveedor.IdProveedor));

                DetalleSelected = detalle;
            });

            MessagingCenter.Subscribe<GestionProductosPageViewModel, ObservableCollection<ProductoItem>>(this, "loadProductsVenta", (sender, arg) =>
            {
                var productos = arg;
                ListProduct = productos;
            });

            MessagingCenter.Subscribe<GestionProductosPageViewModel, ObservableCollection<ProveedorItem>>(this, "loadProveedoresVenta", (sender, arg) =>
            {
                var proveedores = arg;
                ListProveedores = proveedores;
            });
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

                var idProveedor = DetalleSelected?.IdProveedor;
                var idProducto = DetalleSelected?.IdProducto;

                var provItem = ListProveedores.ToList().Find(x => x.IdProveedor == idProveedor);
                ProveedorSelected = provItem;

                ProductoItemDB ProductoDB = await ProductoItemDB.Instance;
                ListProduct = new ObservableCollection<ProductoItem>(await ProductoDB.GetProductsIdProveedorAsync(provItem.IdProveedor));

                if (idProducto > 0)
                {
                    var ProdItem = ListProduct.ToList().Find(x => x.IdProducto == idProducto);
                    ProductoSelected = ProdItem;
                }
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
                //var confirmaConf = new ConfirmConfig()
                //{
                //    Title = "Confirmación",
                //    Message = "¿Deseas agregar este producto en la lista?",
                //    OkText = "Si",
                //    CancelText = "No"
                //};

                //bool resp = await UserDialogs.Instance.ConfirmAsync(confirmaConf);
                //if (!resp)
                //{
                //    return;
                //}

                using (UserDialogs.Instance.Loading("Procesando...", null, null, true, MaskType.Black))
                {

                    if (DetalleSelected != null)
                    {
                        if (DetalleSelected.IdProveedor > 0)
                        {
                            if (DetalleSelected.IdProducto > 0)
                            {
                                if (DetalleSelected.Cantidad > 0)
                                {
                                    if (DetalleSelected.PesoNeto > 0)
                                    {
                                        if (DetalleSelected.Descuento < 0)
                                        {
                                            await UserDialogs.Instance.AlertAsync("El descuento no puede ser negativo.", "Aviso", "Ok");
                                            return;
                                        }
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
                                    else
                                        await UserDialogs.Instance.AlertAsync("Debe ingresar el peso neto (puede ser igual a la cantidad).", "Aviso", "Ok");
                                }
                                else
                                    await UserDialogs.Instance.AlertAsync("Debe ingresar la cantidad del producto.", "Aviso", "Ok");
                            }
                            else
                                await UserDialogs.Instance.AlertAsync("Debe seleccionar un producto.", "Aviso", "Ok");
                        }
                        else
                            await UserDialogs.Instance.AlertAsync("Debe seleccionar un proveedor.", "Aviso", "Ok");
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
                DetalleVentaItemDB DetalleDB = await DetalleVentaItemDB.Instance;
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
                        if (ClienteSelected != null)
                        {
                            Order.IdCliente = ClienteSelected.IdCliente;
                            OrdenVentaItemDB OrderDB = await OrdenVentaItemDB.Instance;
                            await OrderDB.SaveOrdenVentaAsync(Order);
                            Order = await OrderDB.GetOrdenVentaIdsAsync(Order.IdOrdenVenta);
                            await LoadProductsProveedor();
                            HayOrden = true;
                        }
                        else
                            await UserDialogs.Instance.AlertAsync("Debe de seleccionar un cliente.", "Aviso", "Ok");
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
                    var products = new ObservableCollection<ProductoItem>(await ProductoDB.GetProductsIdProveedorAsync(ProveedorSelected.IdProveedor));
                    if (products?.Count > 0)
                    {
                        ListProduct = products;
                    }
                    else
                    {
                        ListProveedores = new ObservableCollection<ProveedorItem>();
                    }
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

        private async Task PrintOrder()
        {
            bool ExisteDescuento = false;
            try
            {
                if (ListDetail.Count == 0)
                {
                    throw new Exception("Para finalizar la venta, debes agregar productos.");
                }


                var confirmaConf = new ConfirmConfig()
                {
                    Title = "Confirmación",
                    Message = "¿Deseas finalizar la venta?",
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

                    if (Order?.IdOrdenVenta != 0)
                    {
                        OrdenVentaDTO orden = new OrdenVentaDTO();

                        orden.Cliente = ClienteSelected.Nombre;
                        orden.FechaVenta = Order.FechaVenta.ToString("dd/MMMM/yyyy hh:mm:s tt");
                        orden.UsuarioCreacion = App.UserLogin.Name;
                        orden.MontoTotal = Order.MontoTotal;
                        decimal.TryParse(Order.MontoTotal.Replace("$", "").Replace(",", ""), out var monto);
                        orden.MontoLetra = new Funciones().ConvertirNumeroALetras(Order.MontoTotal.Replace("$", "").Replace(",", ""), true, "pesos");
                        orden.CreditosAnteriores = Order.CreditosAnteriores;
                        orden.ExistenciasAnteriores = Order.ExistenciasAnteriores;

                        foreach (var item in ListDetail)
                        {
                            ProductoItemDB db = await ProductoItemDB.Instance;
                            var productoItem = await db.GetProductsIdAsync(item.IdProducto);
                            var prod = new OrdenVentaDTO.ProductoVenta();
                            prod.ProductoDesc = productoItem.Producto;
                            prod.Cantidad = item.Cantidad.ToString();
                            prod.Descuento = item.Descuento.ToString("$0,0.00");
                            prod.Importe = item.Importe.ToString("$0,0.00");
                            prod.PesoNeto = item.PesoNeto.ToString();
                            prod.Precio = item.Precio.ToString("$0,0.00");  
                            orden.Productos.Add(prod);
                        }

                        foreach (var item in ListDetail)
                        {
                            if (item.Descuento > 0)
                                ExisteDescuento = true;
                        }

                        if (!ExisteDescuento)
                        {
                            var ticket = new TicketVenta();
                            ticket.Model = orden;
                            var htmlString = ticket.GenerateString();

                            var htmlSource = new HtmlWebViewSource();
                            htmlSource.Html = htmlString;

                            await PopupNavigation.Instance.PushAsync(new TicketPopUpVentas());

                            MessagingCenter.Send<VentasPageViewModel, HtmlWebViewSource>(this, "ShowTicketVenta", htmlSource);
                        }
                        else
                        {
                            var ticket = new TicketVentaDesc();
                            ticket.Model = orden;
                            var htmlString = ticket.GenerateString();

                            var htmlSource = new HtmlWebViewSource();
                            htmlSource.Html = htmlString;

                            await PopupNavigation.Instance.PushAsync(new TicketPopUpVentas());

                            MessagingCenter.Send<VentasPageViewModel, HtmlWebViewSource>(this, "ShowTicketVenta", htmlSource);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

    }
}
