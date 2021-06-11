using ManageProd.DependencyServices;
using ManageProd.iOS.DependencyServices;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(ApplePrintService))]
namespace ManageProd.iOS.DependencyServices
{
    public class ApplePrintService: IPrintService
    {
        public void Print(WebView viewToPrint)
        {
            var appleViewToPrint = Platform.CreateRenderer(viewToPrint).NativeView;

            var printInfo = UIPrintInfo.PrintInfo;

            printInfo.OutputType = UIPrintInfoOutputType.General;
            printInfo.JobName = "Forms EZ-Print";
            printInfo.Orientation = UIPrintInfoOrientation.Portrait;
            printInfo.Duplex = UIPrintInfoDuplex.None;

            var printController = UIPrintInteractionController.SharedPrintController;

            printController.PrintInfo = printInfo;
            printController.ShowsPageRange = true;
            printController.PrintFormatter = appleViewToPrint.ViewPrintFormatter;

            printController.Present(true, (printInteractionController, completed, error) => { });
        }
    }
}
