using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ManageProd.Models.DTO;
using ManageProd.Shared;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
using ManageProd.Templates;
using ManageProd.Views;
using ManageProd.Views.Popup;
using PropertyChanged;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ManageProd.ViewModels
{
    public class ComprasPageViewModel: NotificationObject
    {
        public bool IsBusy { get; set; }
        public bool HayOrden { get; set; }
        public bool IsSelected { get; set; }
        public bool HayProductos { get; set; }
        public OrdenCompraItem Order { get; set; }
        public ObservableCollection<DetalleCompraItem> ListDetail { get; set; }
        public ObservableCollection<ProveedorItem> ListProveedores { get; set; }
        public ObservableCollection<ProductoItem> ListProduct { get; set; }
        public DetalleCompraItem DetalleSelected { get; set; }

        public ProveedorItem ProveedorSelected { get; set; }

        // Personalizamos la propiedad para que asigne el precio al propducto
        public ProductoItem ProductoSelected { get; set; }

        public ICommand SaveOrder { get; set; }
        public ICommand AddProduct { get; set; }
        public ICommand DeleteProduct { get; set; }
        public ICommand ProductoSelectionChanged { get; set; }
        public ICommand NewProduct { get; set; }
        public ICommand FinishOrder { get; set; }


        public ComprasPageViewModel()
        {
            IsBusy = false;
            HayOrden = false;
            IsSelected = false;
            HayProductos = false;

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
            ProductoSelectionChanged = new Command(async () => await SelectChanged());
            NewProduct = new Command(async () =>  await NewProd());
            FinishOrder = new Command(async () => await PrintOrder());

            LoadCatalogs();

            MessagingCenter.Subscribe<ComprasPage, ProductoItem>(this, "productoChanged", (sender, arg) =>
            {
                var producto = arg;
                var detalle = DetalleSelected;
                detalle.IdProducto = producto.IdProducto;
                detalle.Producto = producto.Producto;
                detalle.Precio = producto.PrecioCompra;

                DetalleSelected = detalle;
            });

            MessagingCenter.Subscribe<GestionProductosPageViewModel, ObservableCollection<ProductoItem>>(this, "loadProductsCompra", (sender, arg) =>
            {
                var productos = arg;
                ListProduct = productos;
            });

        }

        private async Task PrintOrder()
        {
            try
            {
                if (ListDetail.Count == 0)
                {
                    throw new Exception("Para finalizar la compra, debes agregar productos.");
                }


                var confirmaConf = new ConfirmConfig()
                {
                    Title = "Confirmación",
                    Message = "¿Deseas finalizar la compra?",
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

                    if (Order?.IdOrdenCompra != 0)
                    {
                        OrdenCompraDTO orden = new OrdenCompraDTO();

                        orden.Proveedor = ProveedorSelected.Proveedor;
                        orden.FechaCompra = Order.FechaCompra.ToString("dd/MMMM/yyyy hh:mm:s tt");
                        orden.UsuarioCreacion = App.UserLogin.Name;
                        orden.Notas = Order.Notas;
                        orden.MontoTotal = Order.MontoTotal;
                        decimal.TryParse(Order.MontoTotal.Replace("$","").Replace(",",""), out var monto);
                        orden.MontoLetra = new Funciones().ConvertirNumeroALetras(Order.MontoTotal.Replace("$", "").Replace(",", ""),true,"pesos");

                        foreach (var item in ListDetail)
                        {
                            ProductoItemDB db = await ProductoItemDB.Instance;
                            var productoItem = await db.GetProductsIdAsync(item.IdProducto);

                            var prod = new OrdenCompraDTO.Producto()
                            {
                                ProductoDesc = productoItem.Producto,
                                Cantidad = item.Cantidad.ToString(),
                                PesoBruto = item.PesoBruto.ToString(),
                                PesoNeto = item.PesoNeto.ToString(),
                                Precio = item.Precio.ToString("$0,0.00"),
                                Importe = item.Importe.ToString("$0,0.00")
                            };
                            
                            orden.Productos.Add(prod);
                        }

                        // generamos el tmeplaste del html
                        var ticket = new TicketCompra();
                        ticket.Model = orden;
                        var htmlString = ticket.GenerateString();


                        //Creamos un html source
                        var htmlSource = new HtmlWebViewSource();
                        htmlSource.Html = htmlString;

                        // cargamos el html en un webview
                        //var browser = new WebView();
                        //browser.Source = htmlSource;

                        await PopupNavigation.Instance.PushAsync(new TicketPopup());

                        MessagingCenter.Send<ComprasPageViewModel, HtmlWebViewSource>(this, "ShowTicketCompra", htmlSource);



                    }
                }
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

                DetalleSelected = new DetalleCompraItem();
                ProductoSelected = new ProductoItem();
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
                IsSelected = DetalleSelected?.IdDetalleCompra != 0;

                if (!IsSelected)
                {
                    return;
                }

                var idProducto = DetalleSelected.IdProducto;

                var item = ListProduct.ToList().Find(x => x.IdProducto == idProducto);
                ProductoSelected = item;
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

                    if (DetalleSelected?.IdDetalleCompra != 0)
                    {
                        DetalleCompraItemDB OrderDB = await DetalleCompraItemDB.Instance;
                        await OrderDB.DeleteDetalleCompraAsync(DetalleSelected);
                        await LoadOrderDetail();
                        await ActualizarMontoTotal();
                        IsSelected = false;
                        DetalleSelected = new DetalleCompraItem();
                        ProductoSelected = new ProductoItem();
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
                        if (DetalleSelected.IdProducto > 0)
                        {
                            if (DetalleSelected.Cantidad > 0)
                            {
                                if (DetalleSelected.PesoBruto > 0)
                                {
                                    DetalleSelected.IdOrdenCompra = Order.IdOrdenCompra;
                                    DetalleCompraItemDB DetalleDB = await DetalleCompraItemDB.Instance;
                                    await DetalleDB.SaveDetalleCompraAsync(DetalleSelected);
                                    await LoadOrderDetail();
                                    await ActualizarMontoTotal();
                                    IsSelected = false;
                                    DetalleSelected = new DetalleCompraItem();
                                    ProductoSelected = new ProductoItem();
                                }
                                else
                                    await UserDialogs.Instance.AlertAsync("Debe ingresar el peso bruto (puede ser igual a la cantidad).", "Aviso", "Ok");
                            }
                            else
                                await UserDialogs.Instance.AlertAsync("Debe ingresar la cantidad del producto.", "Aviso", "Ok");
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
                DetalleCompraItemDB DetalleDB = await DetalleCompraItemDB.Instance;
                var MontoTotal = await DetalleDB.GetSumaImporteAsync(Order.IdOrdenCompra);
                Order.MontoTotal = MontoTotal.ToString("$0,0.00");
                OrdenCompraItemDB OrderDB = await OrdenCompraItemDB.Instance;
                await OrderDB.SaveOrdenCompraAsync(Order);

                MessagingCenter.Send<ComprasPageViewModel, OrdenCompraItem>(this, "MontoChanged", Order);

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
                        if (ProveedorSelected != null)
                        {
                            // en caso de que exista detalle con esta orden, eliminamos los productos
                            if (Order.IdOrdenCompra > 0)
                            {
                                DetalleCompraItemDB detalleDB = await DetalleCompraItemDB.Instance;
                                await detalleDB.DeleteAllDetalleComprasAsync(Order.IdOrdenCompra);
                                Order.MontoTotal = "0";
                                await LoadOrderDetail();
                            }

                            Order.IdProveedor = ProveedorSelected.IdProveedor;
                            OrdenCompraItemDB OrderDB = await OrdenCompraItemDB.Instance;
                            await OrderDB.SaveOrdenCompraAsync(Order);
                            Order = await OrderDB.GetOrdenCompraIdsAsync(Order.IdOrdenCompra);
                            await LoadProductsProveedor();
                            HayOrden = true;
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
                    HayProductos = ListDetail.Count > 0;
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }

        }

    }
}
