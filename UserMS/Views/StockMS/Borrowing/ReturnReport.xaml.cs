using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace UserMS.Views.StockMS.Borrowing
{
    public partial class ReturnReport : Page
    {
        public ReturnReport()
        {
            InitializeComponent();
            PublicRequestHelp prh = new PublicRequestHelp(this.busy,175,new object[]{},new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.Report_Return> list = e.Result.Obj as List<API.Report_Return>;
                GridReturnList.ItemsSource = list; 
                GridReturnList.Rebind();
            }
        }

    }
}
