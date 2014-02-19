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

namespace UserMS.Views.PackageOff
{
    /// <summary>
    /// StopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StopWindow : Window
    {
        private List<int> listIDs = new List<int>();
        private int methodID = 0;
        public delegate void OnSearchEvent();
        public event OnSearchEvent OnSearch; 

        public StopWindow()
        {
            InitializeComponent();
        }

        public StopWindow(int methodid ,List<int> idlist)
        {
            InitializeComponent();
            methodID = methodid;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            listIDs.Clear();
            listIDs.AddRange(idlist);
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(enddate.DateTimeText))
            {
                MessageBox.Show("请选择结束时间！");
                return;
            }
            if (MessageBox.Show("确定修改吗？","",MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp p = new PublicRequestHelp(this.busy,methodID,new object[]{listIDs,enddate.DateTimeText},new EventHandler<API.MainCompletedEventArgs>(UpdateCompleted)) ;
        }

        private void UpdateCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
               OnSearch();
            }

            this.Close();
            MessageBox.Show(e.Result.Message);

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
