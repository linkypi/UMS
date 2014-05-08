using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.API;

namespace UserMS
{
    public class IMEIAddArgs:EventArgs
    {
        public string IMEI { get; set; }
        public IMEIAddArgs(string IMEI)
        {
            this.IMEI = IMEI;
        }
    }

    public class SelectedProInfoArgs : EventArgs
    {
//        public List<API.Pro_ProInfo> ProInfo { get; set; }
//        public List<string> IMEI { get; set; }
        public Dictionary<string, API.Pro_ProInfo> Results; 
        public SelectedProInfoArgs(Dictionary<string, API.Pro_ProInfo> res)
        {
            this.Results = res;
//            this.ProInfo = p;
//            this.IMEI = imei;
        }
    }

    public partial class IMEISell
	{
        public delegate void SelectedPro(object sender, SelectedProInfoArgs e);
        private int MethodID_IMEIGetInfo = 5;
        public event SelectedPro OnSelectedPro;
	    public Pro_HallInfo Hall;
	    public string SelectedIMEI;
		public IMEISell()
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
                IMEI.Text = IMEI.Text.ToUpper().Trim();
                SelectedIMEI = IMEI.Text;
                string hallid = "";
                if (this.Hall != null)
                {
                    hallid = Hall.HallID;
                }
                PublicRequestHelp helper = new PublicRequestHelp(ProBusy, MethodID_IMEIGetInfo, new object[] { SelectedIMEI.Split('\r').Select(p=>p.Trim()).ToList(), hallid }, IMEISearchComp);

                
            }
           


	    }

	    private void IMEISearchComp(object sender, MainCompletedEventArgs e)
	    {
            
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    
//                    API.Pro_ProInfo i = (Pro_ProInfo)e.Result.Obj;
//                    SelectedProInfoArgs a = new SelectedProInfoArgs(i, SelectedIMEI);
                    List<API.Pro_IMEI> i = (List<Pro_IMEI>) e.Result.Obj;
                    Dictionary<string, API.Pro_ProInfo> Results=new Dictionary<string, Pro_ProInfo>();
                    foreach (var proImei in i)
                    {
                        Results.Add(proImei.IMEI, Store.ProInfo.First(p => p.ProID == proImei.ProID));
                    }
                    SelectedProInfoArgs a = new SelectedProInfoArgs(Results);
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