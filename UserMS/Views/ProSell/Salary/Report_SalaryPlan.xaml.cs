using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using Telerik.Windows.Controls;

namespace UserMS.Views.ProSell.Salary
{
    /// <summary>
    /// Report_SalaryPlan.xaml 的交互逻辑
    /// </summary>
    public partial class Report_SalaryPlan : Page
    {
        private List<API.View_SalaryPlanReport> models = null;

        private int pageindex = 0;
        private bool flag = false;

        public Report_SalaryPlan()
        {
            InitializeComponent();

            this.SizeChanged += Report_SalaryPlan_SizeChanged;
            models = new List<API.View_SalaryPlanReport>();
            GridPriceList.ItemsSource = models;

            flag = true;
            GetReport();
        }

        void Report_SalaryPlan_SizeChanged(object sender, SizeChangedEventArgs e)
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
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 188, new object[] { page.PageIndex, (int)pagesize.Value }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

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
                List<API.View_SalaryPlanReport> list = e.Result.Obj as List<API.View_SalaryPlanReport>;
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

        #region 导出

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 188, new object[] { 0, 50000 }, new EventHandler<API.MainCompletedEventArgs>(GetXLSCompleted));

        }

        private void GetXLSCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.View_SalaryPlanReport> list = e.Result.Obj as List<API.View_SalaryPlanReport>;


                SlModel.operateExcel<API.View_SalaryPlanReport> excel = new SlModel.operateExcel<API.View_SalaryPlanReport>();

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "xls";
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "xls");
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == true)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("ClassName", "商品类别");
                        ht.Add("ProName", "商品型号");
                        ht.Add("TypeName", "商品品牌");
                        ht.Add("ProFormat", "商品属性");
                        ht.Add("SellTypeName", "销售类别");
                        ht.Add("BaseSalary", "基本提成");
                        ht.Add("SpecalSalary", "特殊提成");
                        ht.Add("StartDate", "开始日期");
                        ht.Add("EndDate", "截止日期");
                 
                        Application.Current.Dispatcher.Invoke((Action)delegate { excel.getExcel(list, ht, stream); });
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出完成");
                        this.busy.IsBusy = false;
                    }
                }
            }
            else
            {
                this.busy.IsBusy = false;
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出有误！");
            }
        }



        #endregion 

    }
}
