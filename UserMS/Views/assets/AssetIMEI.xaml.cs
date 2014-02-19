using System;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.API;

namespace UserMS
{
    public class AssetIMEIAddArgs:EventArgs
    {
        public string IMEI { get; set; }
        public AssetIMEIAddArgs(string IMEI)
        {
            this.IMEI = IMEI;
        }
    }

    public class AssetSelectedProInfoArgs : EventArgs
    {
        public API.Pro_ProInfo ProInfo { get; set; }
        public string IMEI { get; set; }
        public API.Pro_InOrder InOrder { get; set; }
        public API.Asset_UseInfo AssUseInfo { get; set; }
        public API.Pro_InOrderList InOrderList { get; set; }
        public AssetSelectedProInfoArgs(API.Pro_ProInfo p, string imei,Pro_InOrder inorder,Asset_UseInfo ass,Pro_InOrderList inlist)
        {
            this.ProInfo = p;
            this.IMEI = imei;
            this.InOrder = inorder;
            this.AssUseInfo = ass;
            this.InOrderList = inlist;
        }
    }

    public partial class AssetIMEI
	{
        public delegate void SelectedPro(object sender, AssetSelectedProInfoArgs e);
        private int MethodID_IMEIGetInfo = 5;
        public event SelectedPro OnSelectedPro;
	    public Pro_HallInfo Hall;
	    public string SelectedIMEI;
        public AssetIMEI()
		{
			// Required to initialize variables
            this.Owner = Application.Current.MainWindow;
			InitializeComponent();

		    this.IMEI.Focus();

		}

	    private void IMEI_OnKeyUp(object sender, KeyEventArgs e)
	    {

            if (e.Key == Key.Enter)
            {
                IMEI.Text = IMEI.Text.ToUpper();
                SelectedIMEI = IMEI.Text.Trim();
                string hallid = "";
                if (this.Hall != null)
                {
                    hallid = Hall.HallID;
                }
                PublicRequestHelp helper = new PublicRequestHelp(ProBusy, MethodID_IMEIGetInfo, new object[] { SelectedIMEI, hallid,0 }, IMEISearchComp);

                
            }
           


	    }

	    private void IMEISearchComp(object sender, MainCompletedEventArgs e)
	    {
            
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    
                    API.Pro_ProInfo i = (Pro_ProInfo)e.Result.Obj;
                    AssetSelectedProInfoArgs a = new AssetSelectedProInfoArgs(i, SelectedIMEI,e.Result.ArrList[0] as Pro_InOrder,e.Result.ArrList[1] as Asset_UseInfo,e.Result.ArrList[2] as Pro_InOrderList);
                    OnSelectedPro(this, a);
                    this.IMEI.Text = "";
                    this.ProBusy.IsBusy = false;
                }
                else
                {
                   //Dispatcher.BeginInvoke(() => { MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message); this.ProBusy.IsBusy = false; });
                    RadWindow.Alert(new DialogParameters() { Content = "查询失败: " + e.Result.Message,Header = "错误",Closed = Closed});
                }
            }
            else
            {
                RadWindow.Alert(new DialogParameters() { Content = "查询失败: 服务器错误\n" + e.Error.Message, Header = "错误",Closed = Closed});
               // Dispatcher.BeginInvoke(() =>
//                    {
//                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
//                        this.ProBusy.IsBusy = false;
//                    });
            }
            

            //Dispatcher.BeginInvoke(() => this.IMEI.Focus());
            
        
           
	    }

	    private void Closed(object sender, WindowClosedEventArgs windowClosedEventArgs)
	    {
            this.ProBusy.IsBusy = false;
	    }

	    private void OK_OnClick(object sender, RoutedEventArgs e)
	    {
            
	        this.Close();
	    }
	}
}