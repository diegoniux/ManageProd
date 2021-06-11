
using ManageProd.DependencyServices;
using ManageProd.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DroidPrintService))]
namespace ManageProd.Droid.DependencyServices
{
    public class DroidPrintService: IPrintService
    {
        [System.Obsolete]
        public void Print(WebView viewToPrint)
        {
            var vRenderer = Xamarin.Forms.Platform.Android.Platform.GetRenderer(viewToPrint);
            var viewGroup = vRenderer.ViewGroup;

            for (int i = 0; i < viewGroup.ChildCount; i++)
            {
                if (viewGroup.GetChildAt(i).GetType().Name == "WebView")
                {
                    var AndroidWebView = viewGroup.GetChildAt(i) as Android.Webkit.WebView;
                    var tmp = AndroidWebView.CreatePrintDocumentAdapter("print");
                    var printMgr = (Android.Print.PrintManager)Forms.Context.GetSystemService(Android.Content.Context.PrintService);
                    printMgr.Print("print", tmp, null);
                    break;
                }
            }
        }
    }
}
