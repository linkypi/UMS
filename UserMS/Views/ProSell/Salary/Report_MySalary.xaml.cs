using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using Telerik.Windows.Controls;

namespace UserMS.Views.ProSell.Salary
{
    public partial class Report_MySalary : Page
    {
        private bool flag = false;
        private string menuid = "";
        private int pageindex;

        public Report_MySalary()
        {
            InitializeComponent();
            this.SizeChanged += Report_MySalary_SizeChanged;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "164";
            }
            flag = true;
            page.PageSize = (int)pagesize.Value;
            this.startdate.SelectedValue = DateTime.Now.Date;
         
            Search();
        }

        void Report_MySalary_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
        /// <summary>
        /// 查询
        /// </summary>
        private void Search()
        {
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            if (page == null)
            {
                rpp.PageIndex = 1;
            }
            else
            {
                rpp.PageIndex = page.PageIndex;
            }
            rpp.PageSize = (int)pagesize.Value;
            rpp.ParamList = new List<API.ReportSqlParams>();

            if (!string.IsNullOrEmpty(this.startdate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime startTime = new API.ReportSqlParams_DataTime();
                startTime.ParamName = "StartTime";
                startTime.ParamValues = this.startdate.SelectedValue;
                rpp.ParamList.Add(startTime);
            }

            if (!string.IsNullOrEmpty(this.enddate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime endTime = new API.ReportSqlParams_DataTime();
                endTime.ParamName = "EndTime";
                endTime.ParamValues = this.enddate.SelectedValue;
                rpp.ParamList.Add(endTime);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 177, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
        }

        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;  
            GridSalaryList.ItemsSource = null;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam rpp = e.Result.Obj as API.ReportPagingParam;
                GridSalaryList.ItemsSource = rpp.Obj as List<API.Proc_SalaryReportDetailResult>;
              
                page.PageSize = (int)pagesize.Value;

                string[] data = new string[rpp.RecordCount];
                PagedCollectionView pcv = new PagedCollectionView(data);
                this.page.PageIndexChanged -= page_PageIndexChanged;
                this.page.Source = pcv;
                this.page.PageIndexChanged += page_PageIndexChanged;
                this.page.PageIndex = pageindex;
            } 
            GridSalaryList.Rebind();  
        }
        
        private void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            if (flag)
            {
                pageindex = e.NewPageIndex;
                Search();
            }
        }

        #region  导出

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            API.ReportPagingParam rpp = new API.ReportPagingParam();
           
            rpp.PageIndex = 0;
            rpp.PageSize = 50000;
            rpp.ParamList = new List<API.ReportSqlParams>();

            if (!string.IsNullOrEmpty(this.startdate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime startTime = new API.ReportSqlParams_DataTime();
                startTime.ParamName = "StartTime";
                startTime.ParamValues = this.startdate.SelectedValue;
                rpp.ParamList.Add(startTime);
            }

            if (!string.IsNullOrEmpty(this.enddate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime endTime = new API.ReportSqlParams_DataTime();
                endTime.ParamName = "EndTime";
                endTime.ParamValues = this.enddate.SelectedValue;
                rpp.ParamList.Add(endTime);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 177, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(GetXLSCompleted));
     
          
        }

        private void GetXLSCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam rpp = e.Result.Obj as API.ReportPagingParam;

                SlModel.operateExcel<API.View_MySalary> excel = new SlModel.operateExcel<API.View_MySalary>();

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "xls";
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "xls");
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == true)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("ClassName","商品类别");
                        ht.Add("ProName","商品型号");
                        ht.Add("TypeName","商品品牌");
                        ht.Add("ProFormat","商品属性");             
                        ht.Add("SellType","销售类别");
                        ht.Add("HallName","仓库");
                        ht.Add("AreaName","区域");
                        ht.Add("ProCount","数量");

                        ht.Add("BaseSalary","基本提成");
                        ht.Add("HMSalary","仓管提成");
                        ht.Add("SellDate","销售日期");
                        Application.Current.Dispatcher.Invoke((Action)delegate { excel.getExcel(rpp.Obj as List<API.View_MySalary>,ht,stream); });
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出完成");
                        this.busy.IsBusy = false;
                    }
                }
            
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败！");
                this.busy.IsBusy = false;
            }
        }

        #endregion 
    }
}
