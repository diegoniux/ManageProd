using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using ManageProd.Models.DTO;
using ManageProd.ViewModels;

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
    }
}
