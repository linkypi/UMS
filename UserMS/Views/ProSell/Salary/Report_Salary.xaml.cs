using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using UserMS.Common;

namespace UserMS.Views.ProSell.Salary
{
    public partial class Report_Salary : Page
    {
        string menuid = "";
        private ROHallAdder hall = null;
       private bool flag = false;
       private int pageindex;
       private int detailPageIndex;

        public Report_Salary()
        {
            InitializeComponent();
            this.AddHandler(GridViewRow.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.GridViewRow_OnMouseLeftButtonUp), true);

           // this.SizeChanged += new SizeChangedEventHandler(SizeChangedx);
        }

        private void SizeChangedx(object sender, SizeChangedEventArgs e)
        {
            ReSize(e.NewSize,  "panel1",  "page","pagesize");
            ReSize(e.NewSize, "panel2", "detailPage", "detailPS");
        }

        /// <summary>
        /// 调整分页控件长度
        /// </summary>
        /// <param name="size"></param>
        /// <param name="panel"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        private void ReSize(Size size  ,string panel,string page,string pagesize)
        {
            WrapPanel wp = this.FindName(panel) as WrapPanel;
            wp.Width = size.Width;

            RadDataPager rdp = this.FindName(page) as RadDataPager;
            RadNumericUpDown nud = this.FindName(pagesize) as RadNumericUpDown;
            rdp.Width = size.Width - nud.Width;
        }

        private void GridViewRow_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/Views/ProSell/Salary/Report_SalaryDetail?startdate=&amp;enddate=&amp;seller=&amp;hallid=", UriKind.Relative));
        }

        // 当用户导航到此页面时执行。
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "163";
            }
            this.startdate.SelectedValue = DateTime.Now.Date;
            hall = new ROHallAdder(ref this.hallName, int.Parse(menuid));
            flag = true;
            //List<API.Pro_HallInfo> hallinfo = new List<API.Pro_HallInfo>();
            //List<API.Pro_AreaInfo> areainfo = new List<API.Pro_AreaInfo>();
            //var menuInfo = Store.RoleInfo.First().Sys_Role_MenuInfo.Where(p => p.MenuID.ToString() == menuid);
            //if (menuInfo.Count() != 0)
            //{
            //    if (menuInfo.First().Note == null)
            //    {
            //        // 左连接获取仓库列表              
            //        var query = (from b in Store.RoleInfo.First().Sys_Role_Menu_HallInfo
            //                     where b.MenuID.ToString() == menuid
            //                     join c in Store.ProHallInfo
            //                     on b.HallID equals c.HallID into hall
            //                     from b1 in hall.DefaultIfEmpty()
            //                     select b1).ToList();
            //        //if (query.Count() > 0)
            //        //    return query;
            //        hallinfo = query.Where(c => c != null).ToList();
            //        List<TreeViewModel> child = new List<TreeViewModel>();

            //        //生成左边树
            //        areainfo = (from b in hallinfo
            //                        join c in Store.AreaInfo.Where(c => c.AreaID != null) on b.AreaID equals c.AreaID
            //                        where b != null && b.AreaID != null && c.AreaID != null
            //                        select c).Distinct().ToList();
            //    }
            //}

            //
            //this.area.ItemsSource = areainfo;
            //this.area.SelectedIndex = 0;

            //this.hall.ItemsSource = hallinfo;
            //this.hall.SelectedIndex = 0;
           if (flag)
           {
                Search();
           }
        }

        #region  查询

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            if (!flag) { return; }
           // this.detailPage.Source = null;
           // this.GridSalaryDetail.ItemsSource = null;
           // GridSalaryDetail.Rebind();

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

            API.ReportSqlParams_String rp = new API.ReportSqlParams_String();
            rp.ParamName = "startdate";
            rp.ParamValues = this.startdate.SelectedValue.ToString() == null ? "" : this.startdate.SelectedValue.ToString();
            rpp.ParamList.Add(rp);
            API.ReportSqlParams_String rp1 = new API.ReportSqlParams_String();
            rp1.ParamName = "enddate";
            rp1.ParamValues = this.enddate.SelectedValue.ToString() == null ? "" : this.enddate.SelectedValue.ToString();
            rpp.ParamList.Add(rp1);

            API.ReportSqlParams_String sell = new API.ReportSqlParams_String();
            sell.ParamValues = this.seller.Text.ToString();
            rpp.ParamList.Add(sell);
            //API.ReportSqlParams_String areaid = new API.ReportSqlParams_String();
            //rp.ParamValues = this.area.SelectedItem.ToString();
            //rpp.ParamList.Add(areaid);
            API.ReportSqlParams_String hallid = new API.ReportSqlParams_String();
            hallid.ParamValues = this.hallName.Tag== null ? "" : this.hallName.Tag.ToString();
            rpp.ParamList.Add(hallid);

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 183, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
     
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;

            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;
            this.GridSalaryList.ItemsSource = null;
            GridSalaryList.Rebind();

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                if (pageParam == null) { return;  }
                List<API.Proc_SalaryReportResult> list = pageParam.Obj as List<API.Proc_SalaryReportResult>;
                if (list == null) { return; }
                if (list.Count != 0)
                {
                    this.GridSalaryList.ItemsSource = list; 
                    GridSalaryList.Rebind();

                    page.PageSize = (int)pagesize.Value;

                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);
                    this.page.PageIndexChanged -= page_PageIndexChanged;
                    this.page.Source = pcv;
                    this.page.PageIndexChanged += page_PageIndexChanged;
                    this.page.PageIndex = pageindex;
                }

            }
        }

        #endregion 

        #region  详情

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridSalaryList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            //if (GridSalaryList.SelectedItem == null) return;

            //SearchDetail();
        }

        private void SearchDetail()
        {
            if (flag)
            {
                if (GridSalaryList.SelectedItem == null) 
                {
                    //this.detailPage.Source = null;
                    //this.GridSalaryDetail.ItemsSource = null;
                    //GridSalaryDetail.Rebind();
                    return; 
                }


                API.Proc_SalaryReportResult model = GridSalaryList.SelectedItem as API.Proc_SalaryReportResult;

                API.ReportPagingParam rpp = new API.ReportPagingParam();
                //if (detailPage == null)
                //{
                //    rpp.PageIndex = 1;
                //}
                //else
                //{
                //    rpp.PageIndex = detailPage.PageIndex;
                //}
                //rpp.PageSize = (int)detailPS.Value;
                rpp.ParamList = new List<API.ReportSqlParams>();

                API.ReportSqlParams_String rp = new API.ReportSqlParams_String();
                rp.ParamName = "startdate";
                rp.ParamValues = this.startdate.SelectedValue.ToString() == null ? "" : this.startdate.SelectedValue.ToString();
                rpp.ParamList.Add(rp);
                API.ReportSqlParams_String rp1 = new API.ReportSqlParams_String();
                rp1.ParamName = "enddate";
                rp1.ParamValues = this.enddate.SelectedValue.ToString() == null ? "" : this.enddate.SelectedValue.ToString();
                rpp.ParamList.Add(rp1);

                API.ReportSqlParams_String sell = new API.ReportSqlParams_String();
                sell.ParamValues = model.Seller;
                rpp.ParamList.Add(sell);

                API.ReportSqlParams_String hallid = new API.ReportSqlParams_String();
                hallid.ParamValues = this.hallName.Tag == null ? "" : this.hallName.Tag.ToString();
                rpp.ParamList.Add(hallid);

                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 184, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
            }
        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            //this.detailPage.PageIndexChanged -= detailPage_PageIndexChanged;
            //this.detailPage.Source = pcv1;
            //this.detailPage.PageIndexChanged += detailPage_PageIndexChanged;
            //this.GridSalaryDetail.ItemsSource = null;
            //GridSalaryDetail.Rebind();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                List<API.Proc_SalaryReportDetailResult> list = pageParam.Obj as List<API.Proc_SalaryReportDetailResult>;
                if (list == null) { return; }
                if (list.Count != 0)
                {
                    //this.GridSalaryDetail.ItemsSource = list;
                    //GridSalaryDetail.Rebind();

                    //detailPage.PageSize = (int)detailPS.Value;

                    //string[] data = new string[pageParam.RecordCount];
                    //PagedCollectionView pcv = new PagedCollectionView(data);
                    //this.detailPage.PageIndexChanged -= detailPage_PageIndexChanged;
                    //this.detailPage.Source = pcv;
                    //this.detailPage.PageIndexChanged += detailPage_PageIndexChanged;
                    //this.detailPage.PageIndex = detailPageIndex;
                }
            }
        }

       #endregion

        #region  换页及调整每页行数

        private void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
             if(flag)
            {
                pageindex = e.NewPageIndex;
               // this.detailPage.PageIndexChanged -= detailPage_PageIndexChanged;
                Search();
                //this.detailPage.PageIndexChanged += detailPage_PageIndexChanged;
            }
        }

        private void detailPage_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            detailPageIndex = e.NewPageIndex;
            SearchDetail();
        }

        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        private void detailPS_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            SearchDetail();
        }

        #endregion 

        #region 导出

        /// <summary>
        /// 导出汇总
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
           {
            if (!flag) { return; }
          
            API.ReportPagingParam rpp = new API.ReportPagingParam();
           
            rpp.PageIndex = 1;
          
            rpp.PageSize = 50000;
            rpp.ParamList = new List<API.ReportSqlParams>();

            API.ReportSqlParams_String rp = new API.ReportSqlParams_String();
            rp.ParamName = "startdate";
            rp.ParamValues = this.startdate.SelectedValue.ToString() == null ? "" : this.startdate.SelectedValue.ToString();
            rpp.ParamList.Add(rp);
            API.ReportSqlParams_String rp1 = new API.ReportSqlParams_String();
            rp1.ParamName = "enddate";
            rp1.ParamValues = this.enddate.SelectedValue.ToString() == null ? "" : this.enddate.SelectedValue.ToString();
            rpp.ParamList.Add(rp1);

            API.ReportSqlParams_String sell = new API.ReportSqlParams_String();
            sell.ParamValues = this.seller.Text.ToString();
            rpp.ParamList.Add(sell);

            API.ReportSqlParams_String hallid = new API.ReportSqlParams_String();
            hallid.ParamValues = this.hallName.Tag== null ? "" : this.hallName.Tag.ToString();
            rpp.ParamList.Add(hallid);

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 183, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(GetSumCompleted));
     
        }

        private void GetSumCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.Proc_SalaryReportResult> list = pageParam.Obj as List<API.Proc_SalaryReportResult>;


                SlModel.operateExcel<API.Proc_SalaryReportResult> excel = new SlModel.operateExcel<API.Proc_SalaryReportResult>();

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "xls";
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "xls");
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog()==true)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("HallName", "营业厅");
                        ht.Add("Seller", "营销员");
                        ht.Add("SellCount", "本人销售数量");
                        ht.Add("BaseSalary", "本人销售提成");
                        ht.Add("BackCount", "本人销售退机");
                        ht.Add("BackMoney", "本人销售退机金额");
                        ht.Add("OtherSellCount", "非本人销售数量");
                        ht.Add("OtherSellSalary", "非本人销售金额");
                        ht.Add("OtherBackCount", "非本人销售退机数");
                        ht.Add("OtherBackSalary", "非本人销售退机数");
                        ht.Add("TotalSalary", "终端提成总额");
                        Application.Current.Dispatcher.Invoke((Action)delegate { excel.getExcel(pageParam.Obj as List<API.Proc_SalaryReportResult>, ht, stream); });
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出完成");
                        this.busy.IsBusy = false;
                    }
                }

            }
            else
            {
                 this.busy.IsBusy = false;
                 MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message);
            }
        }

        /// <summary>
        /// 导出明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportDetail_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
          {
            API.Proc_SalaryReportResult model = GridSalaryList.SelectedItem as API.Proc_SalaryReportResult;

            API.ReportPagingParam rpp = new API.ReportPagingParam();
          
            rpp.PageIndex = 1;
         
            rpp.PageSize = 50000;
            rpp.ParamList = new List<API.ReportSqlParams>();

            API.ReportSqlParams_String rp = new API.ReportSqlParams_String();
            rp.ParamName = "startdate";
            rp.ParamValues = this.startdate.SelectedValue.ToString() == null ? "" : this.startdate.SelectedValue.ToString();
            rpp.ParamList.Add(rp);
            API.ReportSqlParams_String rp1 = new API.ReportSqlParams_String();
            rp1.ParamName = "enddate";
            rp1.ParamValues = this.enddate.SelectedValue.ToString() == null ? "" : this.enddate.SelectedValue.ToString();
            rpp.ParamList.Add(rp1);

            API.ReportSqlParams_String sell = new API.ReportSqlParams_String();
            sell.ParamValues = "";
            rpp.ParamList.Add(sell);

            API.ReportSqlParams_String hallid = new API.ReportSqlParams_String();
            hallid.ParamValues = this.hallName.Tag == null ? "" : this.hallName.Tag.ToString();
            rpp.ParamList.Add(hallid);

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 184, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(GetXLSDetailCompleted));
           
        }

        private void GetXLSDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                List<API.Proc_SalaryReportDetailResult> list = pageParam.Obj as List<API.Proc_SalaryReportDetailResult>;

               
                SlModel.operateExcel<API.Proc_SalaryReportDetailResult> excel = new SlModel.operateExcel<API.Proc_SalaryReportDetailResult>();

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "xls";
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "xls");
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == true)
                {
                    using (Stream stream = dialog.OpenFile())  
                    {
                        Hashtable ht = new Hashtable();
                       

                        List<string> strs = new List<string>() { 
                        "营销员","商品类别","商品型号","商品品牌","商品属性",
                        "串码","销售类别","售价","实际售价","片区","营业厅", "本人销售数量", "本人销售提成", "本人退机数量"
                        , "本人退机提成", "非本人退机数", "非本人退机提成", "销售日期"
                        };

                        List<string> fields = new List<string>() { 
                        "Seller",    "ClassName",  "ProName", "TypeName" ,
                        "ProFormat",  "IMEI",  "SellType", 
                        "Price", "RealPrice",  "AreaName", 
                        "HallName",  "ProCount", "Salary", "BackCount", "BackMoney",
                        "OtherBackCount","OtherBackMoney",  "SellDate"
                        };
                        Application.Current.Dispatcher.Invoke((Action)delegate { excel.getExcel(list,strs,fields, stream); });
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出完成");
                        this.busy.IsBusy = false;
                    }
                }
            
            }
            else
            {
                this.busy.IsBusy = false;
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
        }

       #endregion 

    }
}
