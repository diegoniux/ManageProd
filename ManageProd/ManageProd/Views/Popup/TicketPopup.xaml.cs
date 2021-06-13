using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using ManageProd.Models.DTO;
using ManageProd.ViewModels;
using ManageProd.DependencyServices;
using Acr.UserDialogs;

namespace ManageProd.Views.Popup
{
    public partial class TicketPopup : PopupPage
    {
        public TicketPopup()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<ComprasPageViewModel, HtmlWebViewSource>(this, "ShowTicketCompra", (sender, arg) =>
            {
                browser.Source = arg;
            });
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var confirmaConf = new ConfirmConfig()
                {
                    Title = "Confirmación",
                    Message = "¿Deseas imprimir el comprobante?",
                    OkText = "Si",
                    CancelText = "No"
                };

                bool resp = await UserDialogs.Instance.ConfirmAsync(confirmaConf);
                if (!resp)
                {
                    return;
                }

                using (UserDialogs.Instance.Loading("Imprimiendo...", null, null, true, MaskType.Black))
                {

                    var printService = DependencyService.Get<IPrintService>();
                    printService.Print(browser);
                    MessagingCenter.Send<TicketPopup, bool>(this, "FinishPrint", true);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message, "Aviso", "Ok");
            }
        }
    }
}
