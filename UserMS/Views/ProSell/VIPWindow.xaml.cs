using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UserMS.API;

namespace UserMS
{
	public partial class VIPWindow 
	{
        private int MethodID_VIPGetInfo = 142;
        public API.VIP_VIPInfo SellVIP;
		public VIPWindow()
		{
			// Required to initialize variables
            
			InitializeComponent();
            this.Owner = Application.Current.MainWindow;
		    this.VIPCard.Focus();
		}

	    private void VIPCard_OnKeyUp(object sender, KeyEventArgs e)
	    {
            TextBox textBox = sender as TextBox;
            if (textBox != null && e.Key == Key.Enter)
            {

                textBox.Text = (textBox.Text + "").ToUpper();
                PublicRequestHelp helper = new PublicRequestHelp(VIPBusy, MethodID_VIPGetInfo, new object[] { textBox.Text.Trim(),"","","" }, VipRequestEnd);
            }
	    }

	    private void VipRequestEnd(object sender, MainCompletedEventArgs e)
	    {
            this.VIPBusy.IsBusy = false;
            //SellVIP.MemberName = "t";
            //NotifyPropertyChanged("SellVIP");  
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    
                    var list = (List<VIP_VIPInfo>)e.Result.Obj;
                    if (list.Count > 0)
                    {
                        SellVIP = list[0];
                    }
                    else
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器无返回");
                        return;
                    }

                    this.DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
            }
        
	    }

	    private void OK_OnClick(object sender, RoutedEventArgs e)
	    {
            PublicRequestHelp helper = new PublicRequestHelp(VIPBusy, MethodID_VIPGetInfo, new object[] { VIPCard.Text.Trim(),"","","" }, VipRequestEnd); 
	    }

	    private void Cancel_OnClick(object sender, RoutedEventArgs e)
	    {
	        this.DialogResult = false;
            Close();
	    }
	}
}