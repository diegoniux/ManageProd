using ManageProd.Models;
using ManageProd.Services.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Splat;
using Acr.UserDialogs;
using ManageProd.SQLiteDB.Data;
using ManageProd.SQLiteDB.Models;
using System.Threading.Tasks;
using Xamarin.Essentials;
using CsvHelper;
using System.Globalization;
using System.Collections.ObjectModel;

namespace ManageProd.ViewModels
{
    public class GestionProductosPageViewModel: NotificationObject
    {
        public ObservableCollection<LayoutModel> ListProduct { get; set; }
        public bool HayInfo { get; set; }
        public bool IsBusy { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand SaveData { get; set; }


        public GestionProductosPageViewModel()
        {
            ListProduct = new ObservableCollection<LayoutModel>();
            HayInfo = false;
            LoadData = new Command(async () => await LoadInfo());
            SaveData = new Command(async() => await SaveInfo());
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
                        using (var reader = new System.IO.StreamReader(stream))
                        {
                            if (reader != null)
                            {
                                using (var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture))
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
                                                Producto = csvReader.GetField<string>(1),
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
                        IdProducto = int.Parse(item.IdProducto),
                        IdProveedor = int.Parse(item.ProveedorId),
                        Producto = item.Producto,
                        PrecioCompra = precioVenta,
                        PrecioVenta = precioVenta,
                        Tara = tara
                    };

                    await productoDB.InsertProductsAsync(productoItem);
                    
                }
                await UserDialogs.Instance.AlertAsync("Información guardada con éxito", "Aviso", "Ok");

                List<ProductoItem> productos = await productoDB.GetProductsAsync();
                List<ProveedorItem> proveedores = await proveedorDB.GetProveedoresAsync();
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

    }
}
