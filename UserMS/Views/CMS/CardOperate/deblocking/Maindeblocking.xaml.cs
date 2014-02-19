using System.Windows;
using System.Windows.Controls;

namespace UserMS.Views.CMS.CardOperate.deblocking
{
    public partial class Maindeblocking : UserControl
    {
        public Maindeblocking()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserMS.CMS.Operate_deblocking mywin = new UserMS.CMS.Operate_deblocking();
//            mywin.Title = new TextBlock()
//            {
//                Text = "新增解挂/解锁",
//                FontSize = 13
//            };
            mywin.Show();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            UserMS.CMS.Operate_deblocking mywin = new UserMS.CMS.Operate_deblocking();
//            mywin.Title = new TextBlock()
//            {
//                Text = "新增解挂/解锁",
//                FontSize = 13
//            };
            mywin.Show();
        }
    }
}
