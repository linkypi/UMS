using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace UserMS.Views.StockMS.Borrowing
{
    public partial class BorowReport : Page
    {
        public BorowReport()
        {
            InitializeComponent();

            PublicRequestHelp prh = new PublicRequestHelp(this.busy,37,new object[]{},new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                this.GridBorowList.ItemsSource = (e.Result.Obj as List<API.Report_Borow>);
                this.GridBorowList.Rebind();
            }
        }

    }
}
