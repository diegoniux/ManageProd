using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ManageProd.Views
{
    public partial class GestionProductosPage : ContentPage
    {
        public GestionProductosPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var file = await FilePicker.PickAsync();

                if (file == null)
                {
                    return;
                }

                lblArchivo.Text = file.FileName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
