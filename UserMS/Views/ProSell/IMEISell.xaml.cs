using System;
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
    public class SelectedProInfoArgs:EventArgs
    { public API.Pro_ProInfo ProInfo { get; set; }
    public string IMEI { get; set; }
    public SelectedProInfoArgs(API.Pro_ProInfo p,string imei)
    {
        this.ProInfo = p;
        this.IMEI = imei;
    }}

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
                IMEI.Text = IMEI.Text.ToUpper();
                SelectedIMEI = IMEI.Text.Trim();
                string hallid = "";
                if (this.Hall != null)
                {
                    hallid = Hall.HallID;
                }
                PublicRequestHelp helper = new PublicRequestHelp(ProBusy, MethodID_IMEIGetInfo, new object[] { SelectedIMEI, hallid }, IMEISearchComp);

                
            }
           


	    }

	    private void IMEISearchComp(object sender, MainCompletedEventArgs e)
	    {
            
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    
                    API.Pro_ProInfo i = (Pro_ProInfo)e.Result.Obj;
                    SelectedProInfoArgs a = new SelectedProInfoArgs(i, SelectedIMEI);
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