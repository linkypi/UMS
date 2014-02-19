using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using UserMS.ReportService;
using UserMS.MyControl;
using Telerik.Windows.Data;
using Telerik.Windows.Controls.DataServices;
using System.Collections.Generic;
using System.Collections;
using System.Threading;


namespace UserMS.Report.SellAduit
{
    /// <summary>
    /// PrintOutOrder.xaml 的交互逻辑
    /// </summary>
    public partial class SellAduit : BasePage
    {
        private int PageSize = 20;
        public string ReportName = "Report_SellAduitInfo";
        Entities a = new ReportServiceContext();
        System.Data.Services.Client.DataServiceQuery q;
        


        public SellAduit()
        {
            InitializeComponent();
            this.Txt_PageSize.ValueChanged += Txt_PageSize_ValueChanged;
            //Colhead = MyColumnSettings.GetColumns(this.ReportName).ToList();
            //this.datasource.LoadingData += datasource_LoadingData;
        }

        //private void datasource_LoadingData(object sender, LoadingDataEventArgs e)
        //{
        //    q=e.Query;
            
        //}


        private void Txt_PageSize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            this.datasource.PageSize = Convert.ToInt32(e.NewValue);
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= BasePage_Loaded;
            this.datasource.LoadedData += datasource_LoadedData;
            try
            {

                
                //var customerQuery = from cust in a.Report_OutInfo.Expand("Report_OutOrderListInfo")
                //                    where true
                //                    select cust;
                //System.Data.Services.Client.DataServiceCollection<ReportService.Report_OutInfo> trackedCustomers =
                //    new System.Data.Services.Client.DataServiceCollection<ReportService.Report_OutInfo>
                //    (customerQuery);

              
                
                this.datasource.BeginInit();
                this.datasource.AutoLoad = true;
                this.datasource.PageSize = 20;
                this.datasource.DataServiceContext = a;
                Telerik.Windows.Data.SortDescriptor sd = new Telerik.Windows.Data.SortDescriptor();
                sd.Member = "序号";
                sd.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                this.datasource.SortDescriptors.Add(sd);
                //            this.datasource.DataServiceContext = view;
                this.datasource.QueryName = ReportName;
                this.datasource.Expand = "Report_SellAduitListInfo";
                 
                this.datasource.Load();
                this.datasource.EndInit();

                
                this.InitReportInfo(this.Grid, busy, ReportName);
                
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + "");
            }
        }
        private void datasource_LoadedData(object sender, Telerik.Windows.Controls.DataServices.LoadedDataEventArgs e)
        {
            //var xx = ((UserMS.ReportServiceContext)this.datasource.DataServiceContext).Report_OutInfo.Expand("Report_OutOrderListInfo");
            
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获取数据失败: 服务器错误");
                Logger.Log("获取数据失败: 服务器错误");
                e.MarkErrorAsHandled();
                //return;

            }
            
        }
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {

            //this.PageSize = this.datasource.PageSize;
            //this.datasource.PageSize = 0;
            //this.datasource.LoadedData -= datasource_LoadedData;
            //this.datasource.LoadedData += ExportDataLoaded;
            //this.radDataPager.PageIndexChanged += radDataPager_PageIndexChanged;
            //this.datasource.Load();
            var query = a.CreateQuery<ReportService.Report_SellAduitInfo>(this.ReportName).Where(this.datasource.FilterDescriptors);
            this.busy.IsBusy = true;
            Thread oThread = new Thread(delegate()
            {


                try
                {
                    var list = (List<ReportService.Report_SellAduitInfo>)query.ToIList();
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

                            var x = new SlModel.operateExcel<ReportService.Report_SellAduitInfo>();
                            //var hs = new System.Collections.Hashtable();
                            //hs.Add("类别", "类别");

                            var headers =new List<string>();
                            if(this.Colhead.Count()>0)
                                headers = (from b in this.Colhead

                                           select b.ColName).ToList();
                            else
                                headers = (from b in this.ParentHead

                                           select b.ColName).ToList();

                            x.getExcel(list, headers, stream);
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出成功");
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate { MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出失败，" + ex.Message); });

                    
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate { this.busy.IsBusy = false; });

                }


            });
            oThread.Start();
            oThread.IsBackground = true;
        }  
        private void ExportDataLoaded(object sender, Telerik.Windows.Controls.DataServices.LoadedDataEventArgs e)
        {
            this.datasource.LoadedData -= ExportDataLoaded;
            this.datasource.LoadedData += datasource_LoadedData;
            try
            {
                if (e.Error == null)
                {

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
        void radDataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            //this.datasource.CanLoad=true;
            this.radDataPager.PageSize = this.PageSize <= 0 ? 20 : this.PageSize;
            this.radDataPager.PageIndexChanged -= radDataPager_PageIndexChanged;
            //this.radDataPager.PageIndex = 1;
        }

        private void Grid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            //ReportDataSource reportDataSource = _reportViewer.LocalReport.DataSources.First();

            if (this.Grid.SelectedItems.Count == 0)
                this.GridList.ItemsSource = null;
            else
                this.GridList.ItemsSource = ((ReportService.Report_SellAduitInfo)this.Grid.SelectedItems[0]).Report_SellAduitListInfo;


            //_reportViewer.RefreshReport();
        }

        private void BookColumnHeader_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MyColumnSettings my = new MyColumnSettings();

            bool flag = my.OpenColumnSetting(this.ReportName, this.ParentHead);
            if (flag == true)
            {
                this.Colhead = MyColumnSettings.GetColumns(this.ReportName).ToList();
                this.InitGridColumnOwn(new API.Demo_ReportViewInfo() { Demo_ReportViewColumnInfo = this.Colhead });
            }
        }
    }
}
