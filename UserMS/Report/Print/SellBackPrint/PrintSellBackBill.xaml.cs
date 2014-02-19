using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using UserMS.ReportService;

namespace UserMS.Report.Print.SellBackPrint
{
    /// <summary>
    /// PrintSellBill.xaml 的交互逻辑
    /// </summary>
    public partial class PrintSellBackBill : BasePage
    {
        private int PageSize = 20;
        private string ReportName = "Print_SellBackInfo";
        public PrintSellBackBill()
        {
            InitializeComponent();
            this.Txt_PageSize.ValueChanged += Txt_PageSize_ValueChanged; 
        }

       
        private void Txt_PageSize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            this.datasource.PageSize = Convert.ToInt32(e.NewValue);
        }
        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= BasePage_Loaded;

            try
            {

                Entities a = new ReportServiceContext();


                this.datasource.LoadedData += datasource_LoadedData;
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
                this.datasource.Expand = "Print_SellBackListInfo";
                this.datasource.Load();
                this.datasource.EndInit();

                //this.InitReportInfo(this.Grid, busy, ReportName);
                #region 报表






                ReportDataSource reportDataSource = new ReportDataSource();

                reportDataSource.Name = "PrintSellBackListInfo"; // Name of the DataSet we set in .rdlc
                //ReportService.Print_SellInfo sell = e.Entities.Cast<ReportService.Print_SellInfo>().First();

                List<ReportService.Print_SellBackListInfo> sellList = new List<Print_SellBackListInfo>();

                for (int i = 0; i < 10; i++)
                {
                    sellList.Add(new ReportService.Print_SellBackListInfo());
                }
             
                reportDataSource.Value = sellList;
            
                _reportViewer.LocalReport.ReportPath = "Report\\Print\\SellBackPrint\\SellBackBill.rdlc"; // Path of the rdlc file

                _reportViewer.LocalReport.DisplayName="销售单";

                _reportViewer.LocalReport.DataSources.Add(reportDataSource);

                _reportViewer.RefreshReport();

                #endregion

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
                //return;
            }
            
        }
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {

            this.PageSize = this.datasource.PageSize;
            this.datasource.PageSize = 0;
            this.datasource.LoadedData -= datasource_LoadedData;
            this.datasource.LoadedData += ExportDataLoaded;
            this.radDataPager.PageIndexChanged += radDataPager_PageIndexChanged;
            //this.datasource.Load();



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
        void radDataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            //this.datasource.CanLoad=true;
            this.radDataPager.PageSize = this.PageSize <= 0 ? 20 : this.PageSize;
            this.radDataPager.PageIndexChanged -= radDataPager_PageIndexChanged;
            //this.radDataPager.PageIndex = 1;
        }

        private void Grid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
           
            ReportDataSource reportDataSource=null;
            if (_reportViewer.LocalReport.DataSources.Count() == 0)
            {
                reportDataSource = new ReportDataSource();
                reportDataSource.Name = "PrintSellBackListInfo";
                _reportViewer.LocalReport.DataSources.Add(reportDataSource);
            }
            else
                reportDataSource = _reportViewer.LocalReport.DataSources[0];


            ReportService.Print_SellBackInfo sell = (ReportService.Print_SellBackInfo)e.AddedItems[0];
            List<ReportService.Print_SellBackListInfo> sellList = new List<Print_SellBackListInfo>();
            reportDataSource.Value = sellList;
            if (sell.Print_SellBackListInfo.Count() > 10)
            {
                Logger.Log("上级规定，超过10条销售明细的销售单无法打印");
                return;
            }

            sellList.AddRange(sell.Print_SellBackListInfo);
            int j = sell.Print_SellBackListInfo.Count();
            for (int i = 10; i > j; i--)
            {
                sellList.Add(new ReportService.Print_SellBackListInfo());
            }
            
             

            _reportViewer.RefreshReport();
        }
    }
}
