using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.DataServices;
using Telerik.Windows.Data;
using UserMS.ReportService;

namespace UserMS.Report
{
    public partial class GridReportByDate : BasePage
    {
        private string reportname = "Report_InOutSellInfo";
        private Stream Exportfs;
        private int PageSize = 20;
        Entities a =new ReportServiceContext();

        public GridReportByDate()
        {
            InitializeComponent();
            //datasource_LoadedData();
            this.Txt_PageSize.ValueChanged +=Txt_PageSize_ValueChanged;
            //this.radDataPager.PageIndexChanged +=radDataPager_PageIndexChanged;
            this.datasource.LoadingData +=datasource_LoadingData;
            this.BeginTime.SelectedValue = DateTime.Now.AddDays(-30).Date;
            this.EndTime.SelectedValue = DateTime.Now.Date;
        }

        private void datasource_LoadingData(object sender, LoadingDataEventArgs e)
        {
            e.Query= ((System.Data.Services.Client.DataServiceQuery<UserMS.ReportService.Report_InOutSellInfo>)e.Query).AddQueryOption(
               "BeginTime", DateTime.Now.AddDays(-30)).AddQueryOption(
               "EndTime", DateTime.Now);
        }

    
      
        // 当用户导航到此页面时执行。
        private void GridReportByDate_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= GridReportByDate_OnLoaded;
            
            //reportname = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["name"];
            //if (String.IsNullOrEmpty(reportname))
            //{
            //    reportname = "GetPro_ProInfo";
            //}
            try
            {

                Entities a = new ReportServiceContext();

                //var xx=a.CreateQuery<ReportService.GetInOutSellInfo_Result>(reportname);

                this.datasource.LoadedData += datasource_LoadedData;
                this.datasource.BeginInit();
                this.datasource.AutoLoad = true;
                this.datasource.PageSize = 20;
                this.datasource.DataServiceContext = a;
                Telerik.Windows.Data.SortDescriptor sd = new Telerik.Windows.Data.SortDescriptor();
                sd.Member = "序号";
                sd.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                this.datasource.SortDescriptors.Add(sd); 
                
                this.datasource.QueryName = reportname;
                this.datasource.Load();
                this.datasource.EndInit();
                

                this.InitReportInfo(this.Grid, busy, reportname);

           
                
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + "");
            }
        }
        
        private void datasource_LoadedData(object sender, Telerik.Windows.Controls.DataServices.LoadedDataEventArgs e)
        
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获取数据失败: 服务器错误");
                Logger.Log("获取数据失败: 服务器错误");
                e.MarkErrorAsHandled();
            }
            
            //var customerQuery = from cust in a.Report_InOutSellInfo
            //                    where true
            //                    select cust;
           
            //System.Data.Services.Client.DataServiceCollection<ReportService.Report_InOutSellInfo> trackedCustomers =
            //    new System.Data.Services.Client.DataServiceCollection<ReportService.Report_InOutSellInfo>
            //    (customerQuery);
            
        }
        private void datasource_LoadedData()
        {
            int NowIndex = this.radDataPager.PageSize * this.radDataPager.PageIndex;
            var query = a.CreateQuery<ReportService.Report_InOutSellInfo>("Report_InOutSellInfo_Pageing").AddQueryOption(
                "BeginTime", DateTime.Now.AddDays(-30)).AddQueryOption(
                "EndTime", DateTime.Now).AddQueryOption(
                "PageSize", this.radDataPager.PageSize).AddQueryOption(
                "PageIndex", NowIndex);

        
            
 
            var count = query.Execute().Count();

            var result = query.Execute().Skip(20).Take(20).ToList();
            this.Grid.ItemsSource = result;
            //var result = query.BeginExecute(new AsyncCallback(datasource_LoadedData), query);
        }
        private void BeginTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.datasource.Load();
            //datasource_LoadedData();
            this.datasource.Load();
        }
        
       

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            
            //this.PageSize = this.datasource.PageSize;
            //this.datasource.PageSize = 0;
            //this.datasource.LoadedData -= datasource_LoadedData;
            //this.datasource.LoadedData += ExportDataLoaded;
            //this.radDataPager.PageIndexChanged += radDataPager_PageIndexChanged;
            //this.datasource.Load();
            
            int NowIndex = this.radDataPager.PageSize * this.radDataPager.PageIndex;
            var query = a.CreateQuery<ReportService.Report_InOutSellInfo>(this.reportname).AddQueryOption(
                "BeginTime", DateTime.Now.AddDays(-30)).AddQueryOption(
                "EndTime", DateTime.Now);


            //var q= query.Where(this.datasource.FilterDescriptors).ToIList();

            //var count = query.Count();

            //var result = query.Execute().Skip(20).Take(20).ToList();
            //this.Grid.ItemsSource = result;
            this.busy.IsBusy = true;
            var result = query.BeginExecute(new AsyncCallback(datasource_LoadedData), query);
            
            
        }
        private void datasource_LoadedData(IAsyncResult result)
        {
            try
            {
                var query = (System.Data.Services.Client.DataServiceQuery<ReportService.Report_InOutSellInfo>)result.AsyncState;
                var list = (List<ReportService.Report_InOutSellInfo>)query.Where(this.datasource.FilterDescriptors).ToIList();
                query.EndExecute(result);
                //this.Grid.ItemsSource = list;
                string extension = "xls";
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    DefaultExt = extension,
                    Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
                    FilterIndex = 1
                };

                if (dialog.ShowDialog() == true)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        //ExcelHelper.Export<ReportService.Report_InOutSellInfo>(list, stream,new SheetColumn[]{ new SheetColumn(){ Header="类别"}},"进销存");

                        var x = new SlModel.operateExcel<ReportService.Report_InOutSellInfo>();
                        var hs = new System.Collections.Hashtable();
                        hs.Add("类别", "类别");

                        x.getExcel(list, hs, stream);

                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出成功");
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败，"+ex.Message);
            }
            finally {
                Application.Current.Dispatcher.Invoke((Action)delegate { this.busy.IsBusy = false; });
                
            }
        }
        private void ExportDataLoaded(object sender, Telerik.Windows.Controls.DataServices.LoadedDataEventArgs e)

        {
            this.datasource.LoadedData -= ExportDataLoaded;
            this.datasource.LoadedData += datasource_LoadedData;
            try
            {
                if (e.Error == null)
                {



                    //var t = e.Entities.AsQueryable().Cast<object>().Select(p => (object)p).ToList();

                    //ExcelHelper.Export(t, Exportfs);




                    string extension = "xls";
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        DefaultExt = extension,
                        Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
                        FilterIndex = 1
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        using (Stream stream = dialog.OpenFile())
                        {

                            this.Grid.Export(stream,
                             new GridViewExportOptions()
                             {
                                 Format = ExportFormat.Html,
                                 ShowColumnHeaders = true,
                                 ShowColumnFooters = true,
                                 ShowGroupFooters = false,
                             });



                        }
                    }

                }
                else
                {
                    e.MarkErrorAsHandled();
                    
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败: 服务器错误\n" + e.Error.Message);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败: \n" + e.Error.Message);
            }
       
        }
        private void Txt_PageSize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (this.datasource.CanLoad == false)
            {

                return;
            }
            this.datasource.PageSize = Convert.ToInt32(e.NewValue); 
        }
        void radDataPager_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
             
        }

        void radDataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            //datasource_LoadedData();
        }

       

       
    }
}
