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
    public class GestionProductosPageViewModel: BaseViewModel
    {
        public ObservableCollection<LayoutModel> ListProduct { get; set; }
        public bool HayInfo { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand SaveData { get; set; }

        private IRoutingService _navigationService; 

        public GestionProductosPageViewModel(IRoutingService navigationService = null)
        {
            _navigationService = navigationService ?? Locator.Current.GetService<IRoutingService>();
            LoadData = new Command(async () => await LoadInfo());
            SaveData = new Command(async() => await UserDialogs.Instance.AlertAsync("Guardado !!", "Aviso", "Ok"));
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

    }
}
