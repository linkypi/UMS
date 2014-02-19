using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// VIPWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VIPWindow : Window
    {
        public VIPWindow()
        {
            InitializeComponent();
        }

        public API.VIP_VIPInfo vipInfo = new API.VIP_VIPInfo();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 319,
                new object []{ this.vipimei.Text.Trim()} ,GetCompleted);
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                vipInfo = e.Result.Obj as API.VIP_VIPInfo;
                if (vipInfo == null)
                {
                    MessageBox.Show("会员卡不存在！");
                    return;
                }
            }
            else
            {
                vipInfo = null;
                MessageBox.Show("会员卡不存在！"); return;
            }

            DialogResult = true;
        }
    }
}
