using ManageProd.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
using System.Threading.Tasks;
using Xamarin.Essentials;
using CsvHelper;
using System.Globalization;
using System.Collections.ObjectModel;
using CsvHelper.Configuration;
using System.Text;

namespace ManageProd.ViewModels
{
    public class GestionProductosPageViewModel: NotificationObject
    {
        public ObservableCollection<LayoutModel> ListProduct { get; set; }
        public bool HayInfo { get; set; }
        public bool IsBusy { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand SaveData { get; set; }
        public ICommand Cancel { get; set; }


        public GestionProductosPageViewModel()
        {
            ListProduct = new ObservableCollection<LayoutModel>();
            HayInfo = false;
            LoadData = new Command(async () => await LoadInfo());
            SaveData = new Command(async() => await SaveInfo());
            Cancel = new Command(async () => await CancelInfo());
        }

        private async Task LoadInfo()
        {
            try
            {
                var file = await FilePicker.PickAsync();

                if (file != null)
                {
                    if (file.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
                    {
                        var stream = await file.OpenReadAsync();
                        using (var reader = new System.IO.StreamReader(stream, Encoding.UTF8))
                        {
                            if (reader != null)
                            {

                                using (var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture))
                                {
                                    using (UserDialogs.Instance.Loading("Procesando...", null, null, true, MaskType.Black))
                                    {
                                        // leemos el primer registro, que contiene el encabezado
                                        if (!csvReader.Read())
                                        {
                                            return;
                                        }

                                        // leemos el resto de los registros que contrendrá la información
                                        while (csvReader.Read())
                                        {
                                            if (csvReader.GetField<string>(0) != string.Empty)
                                            {
                                                ListProduct.Add(new LayoutModel
                                                {
                                                    IdProducto = csvReader.GetField<string>(0),
                                                    Producto = csvReader.GetField<string>(1).ToString().Replace("�o", "ño").Replace("s�", "só"),
                                                    ProveedorId = csvReader.GetField<string>(2),
                                                    Proveedor = csvReader.GetField<string>(3),
                                                    PrecioCompra = csvReader.GetField<string>(4),
                                                    PrecioVenta = csvReader.GetField<string>(5),
                                                    Tara = csvReader.GetField<string>(6)
                                                });
                                            }

                                        }
                                        HayInfo = ListProduct.Count > 0;
                                    }
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        throw new Exception("Formato de archivo incorrecto, favor de revisar");
                    }

                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }

        private async Task SaveInfo()
        {
            try
            {
                IsBusy = true;
                if (ListProduct.Count == 0)
                {
                    throw new Exception("No hay información para guardar, favor de revisar.");
                }

                var confirmaConf = new ConfirmConfig()
                {
                    Title = "Guardar Información",
                    Message = "¿Deseas guardar la información?",
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
                    ProveedorItemDB proveedorDB = await ProveedorItemDB.Instance;
                    ProductoItemDB productoDB = await ProductoItemDB.Instance;

                    //Eliminamos la información de productos u proveedores
                    await productoDB.DeleteAllProductsAsync();
                    await proveedorDB.DeleteAllProveedoresAsync();


                    //////////////////////////////////
                    // Guaradar información en SQLite//
                    /////////////////////////////////
                    foreach (var item in ListProduct)
                    {
                        //Proveedores
                        ProveedorItem proveedorItem = new ProveedorItem()
                        {
                            IdProveedor = int.Parse(item.ProveedorId),
                            Proveedor = item.Proveedor,
                            Mail = "",
                            NotasProveedor = "",
                            Telefono = ""
                        };

                        ProveedorItem proveedor = await proveedorDB.GetProveedoresAsync(proveedorItem.IdProveedor);
                        if (proveedor == null)
                        {
                            await proveedorDB.InsertProveedoresAsync(proveedorItem);
                        }

                        //Productos
                        decimal.TryParse(item.PrecioCompra, out decimal precioCompra);
                        decimal.TryParse(item.PrecioVenta, out decimal precioVenta);
                        decimal.TryParse(item.Tara, out decimal tara);

                        ProductoItem productoItem = new ProductoItem()
                        {
                            IdProducto = 0, // int.Parse(item.IdProducto),
                            IdProveedor = int.Parse(item.ProveedorId),
                            Producto = item.Producto,
                            PrecioCompra = precioCompra,
                            PrecioVenta = precioVenta,
                            Tara = tara
                        };

                        await productoDB.InsertProductsAsync(productoItem);
                        HayInfo = false;

                    }
                    await UserDialogs.Instance.AlertAsync("Información guardada con éxito", "Aviso", "Ok");

                    ListProduct = new ObservableCollection<LayoutModel>();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CancelInfo()
        {
            ListProduct = new ObservableCollection<LayoutModel>();
            HayInfo = false;
        }

    }
}
