using System.Windows;
using System.Windows.Controls;

namespace UserMS.Views.CMS.CardOperate.recharge
{
    public partial class MainRecharge : UserControl
    {
        public MainRecharge()
        {
            InitializeComponent();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            UserMS.CMS.Operate_Recharge mywin = new UserMS.CMS.Operate_Recharge();
//            mywin.Title = new TextBlock()
//            {
//                Text = "修改充值",
//                FontSize = 13
//            };
            mywin.Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserMS.CMS.Operate_Recharge mywin = new UserMS.CMS.Operate_Recharge();
//            mywin.Title = new TextBlock()
//            {
//                Text = "新增充值",
//                FontSize = 13
//            };
            mywin.Show();
        }
    }
}
