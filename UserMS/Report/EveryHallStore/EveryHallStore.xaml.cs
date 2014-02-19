using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using UserMS.ReportService;

namespace UserMS.Report.Print
{
    /// <summary>
    /// PrintOutOrder.xaml 的交互逻辑
    /// </summary>
    public partial class EveryHallStore : BasePage
    {
        //private int PageSize = 20;
        private string ReportName = "Report_EveryHallStoreInfo";
        public EveryHallStore()
        {
            InitializeComponent();
            //this.Txt_PageSize.ValueChanged += Txt_PageSize_ValueChanged;
            
        }

        

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

                Entities a = new ReportServiceContext();
                //var customerQuery = from cust in a.Report_OutInfo.Expand("Report_OutOrderListInfo")
                //                    where true
                //                    select cust;
                //System.Data.Services.Client.DataServiceCollection<ReportService.Report_OutInfo> trackedCustomers =
                //    new System.Data.Services.Client.DataServiceCollection<ReportService.Report_OutInfo>
                //    (customerQuery);

              
                
                this.datasource.BeginInit();
                this.datasource.AutoLoad = true;
                 
                this.datasource.DataServiceContext = a;
                Telerik.Windows.Data.SortDescriptor sd = new Telerik.Windows.Data.SortDescriptor();
                sd.Member = "序号";
                sd.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                this.datasource.SortDescriptors.Add(sd);
                //            this.datasource.DataServiceContext = view;
                this.datasource.QueryName = ReportName;
                //this.datasource.Expand = "Report_OutOrderListInfo";
                 
                this.datasource.Load();
                this.datasource.EndInit();
                
                //this.InitReportInfo(this.Grid, busy, ReportName);

                #region 报表






                ReportDataSource reportDataSource = new ReportDataSource();

                reportDataSource.Name = "ReportEveryHallStore"; // Name of the DataSet we set in .rdlc

                _reportViewer.LocalReport.DisplayName = "各厅库存";
                _reportViewer.LocalReport.ReportPath = "Report\\EveryHallStore\\demandStore.rdlc"; // Path of the rdlc file

                //System.Reflection.FieldInfo info;
                //foreach (RenderingExtension extension in _reportViewer.LocalReport.ListRenderingExtensions())
                //{
                //    if (extension.Name.ToLower() == "pdf" || extension.Name.ToLower().IndexOf("word") >= 0)
                //    {
                //        info = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                //        info.SetValue(extension, false);

                //    }
                //}



                _reportViewer.LocalReport.DataSources.Add(reportDataSource);
            

                #endregion

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
            ReportDataSource reportDataSource = _reportViewer.LocalReport.DataSources.First();

             

            reportDataSource.Value = this.datasource.DataView;
             

            _reportViewer.RefreshReport();

            
        }
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {

            //this.PageSize = this.datasource.PageSize;
            //this.datasource.PageSize = 0;
            //this.datasource.LoadedData -= datasource_LoadedData;
            //this.datasource.LoadedData += ExportDataLoaded;
            //this.radDataPager.PageIndexChanged += radDataPager_PageIndexChanged;
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

                            //this.Grid.Export(stream,
                            // new GridViewExportOptions()
                            // {
                            //     Format = ExportFormat.Html,
                            //     ShowColumnHeaders = true,
                            //     ShowColumnFooters = true,
                            //     ShowGroupFooters = false,
                            // });



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
            //this.radDataPager.PageSize = this.PageSize <= 0 ? 20 : this.PageSize;
            //this.radDataPager.PageIndexChanged -= radDataPager_PageIndexChanged;
            //this.radDataPager.PageIndex = 1;
        }
    }
}
