using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Common;
using Telerik.Windows.Controls;

namespace UserMS.Views.ProSell.Price
{
    /// <summary>
    /// CostBillReport.xaml 的交互逻辑
    /// </summary>
    public partial class CostBillReport : Page
    {  
        private List<API.View_CostBillReport> models = null;
 

        public CostBillReport()
        {
            InitializeComponent();

            this.SizeChanged += CostBillReport_SizeChanged;
            models = new List<API.View_CostBillReport>();
            GridPriceList.ItemsSource = models;
            flag = true;
            GetReport();
        }

        private int pageindex = 0;
        private bool flag = false;

        void CostBillReport_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

   
        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            GetReport();
        }

        private void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            GetReport();
        }

        private void GetReport()
        {
            if (!flag) { return; }
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 187, new object[] { page.PageIndex, (int)pagesize.Value }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
      
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                models.Clear();
                List<API.View_CostBillReport> list = e.Result.Obj as List<API.View_CostBillReport>;
                models.AddRange(list);
                GridPriceList.Rebind();

                this.page.PageSize = (int)pagesize.Value;
                string[] data = new string[(int)e.Result.ArrList[0]];
                PagedCollectionView pcv = new PagedCollectionView(data);

                this.page.PageIndexChanged -= page_PageIndexChanged;
                this.page.Source = pcv;
                this.page.PageIndexChanged += page_PageIndexChanged;
                this.page.PageIndex = pageindex;
            }

        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            ExportHelp.ExportFile(GridPriceList, "xls");
        }

    }
}
