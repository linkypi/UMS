using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace UserMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            StyleManager.ApplicationTheme = new Windows8Theme();
            Windows8Palette.Palette.FontFamily = new FontFamily("Segoe UI,Tahoma,SimSun");
            Windows8Palette.Palette.FontFamilyLight = new FontFamily("Segoe UI Light,SimSun");
            Windows8Palette.Palette.FontFamilyStrong = new FontFamily("Segoe UI Semibold,SimSun");
            
            LocalizationManager.Manager = new LocalizationManager()
            {
                ResourceManager = UserMS.Localization.RadControlResources.ResourceManager,
                // Culture = Thread.CurrentThread.CurrentCulture
            };
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");

        }



        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(System.Windows.Application.Current.MainWindow, "程序出错: 请重新登陆后再试一次. \n开发人员信息\n" + e.Exception.Message);
            
            e.Handled = true;


            try
            {
                using (MyWebClient client = new MyWebClient())
                {
                    
                    byte[] response = client.UploadValues("http://www.zs96000.com/errorlog.aspx", new NameValueCollection()
       {
           { "UserID", Store.LoginUserName+"" },
           { "Msg", e.Exception+"" }
       });
                }
            }
            catch{}

        } 


    }
}
