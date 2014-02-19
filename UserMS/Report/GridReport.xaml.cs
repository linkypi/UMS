using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.DataFilter;
using Telerik.Windows.Data;
using UserMS.ReportService;
using System.Windows.Media;
using System.Collections.Generic;
using UserMS.MyControl;

namespace UserMS.Report
{
    public partial class GridReport : BasePage
    {
        private string reportname;
        private Stream Exportfs;
        private int PageSize = 20;
        System.Data.Services.Client.DataServiceQuery xx;



        public GridReport()
        {
            InitializeComponent();
            
            this.Txt_PageSize.ValueChanged +=Txt_PageSize_ValueChanged;
        }

    
      
        // 当用户导航到此页面时执行。
        private void ReportTest_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ReportTest_OnLoaded;
            reportname = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["name"];
        
            try
            {
                 
                Entities a = new ReportServiceContext();

                this.datasource.LoadingData += datasource_LoadingData;
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

        void datasource_LoadingData(object sender, Telerik.Windows.Controls.DataServices.LoadingDataEventArgs e)
        {
 
        }
        void datasource_LoadingData_all(object sender, Telerik.Windows.Controls.DataServices.LoadingDataEventArgs e)
        {
            this.datasource.LoadingData += datasource_LoadingData;
            this.datasource.LoadingData -= datasource_LoadingData_all;
            xx = e.Query;
            e.Cancel = true;
            this.datasource.PageSize= this.PageSize ;
            var list = xx.ToIList().Cast<object>().ToList(); ;
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

                    var x = new SlModel.operateExcel<object>();
                    var headers = new List<string>();
                    if (this.Colhead.Count() > 0)
                        headers = (from b in this.Colhead

                                   select b.ColName).ToList();
                    else
                        headers = (from b in this.ParentHead

                                   select b.ColName).ToList();
                    x.getExcel(list, headers, stream);

                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出成功");
                }
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
        }
        
       
       

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            this.datasource.LoadingData -= datasource_LoadingData;
            this.datasource.LoadingData += datasource_LoadingData_all;
            //this.PageSize = this.datasource.PageSize;
            this.PageSize = this.datasource.PageSize;
            this.datasource.PageSize = 0;
            
             
            
        }

        //private void ExportDataLoaded(object sender, Telerik.Windows.Controls.DataServices.LoadedDataEventArgs e)

        //{
            
 
            
        //    //((ReportServiceContext)this.datasource.DataContext).get
        //    this.datasource.LoadedData -= ExportDataLoaded;
        //    this.datasource.LoadedData += datasource_LoadedData;
        //    try
        //    {
        //        if (e.Error == null)
        //        {


 

        //            string extension = "xls";
        //            SaveFileDialog dialog = new SaveFileDialog()
        //            {
        //                DefaultExt = extension,
        //                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
        //                FilterIndex = 1
        //            };
        //            if (dialog.ShowDialog() == true)
        //            {
        //                using (Stream stream = dialog.OpenFile())
        //                {

        //                    this.Grid.Export(stream,
        //                     new GridViewExportOptions()
        //                     {
        //                         Format = ExportFormat.Html,
        //                         ShowColumnHeaders = true,
        //                         ShowColumnFooters = true,
        //                         ShowGroupFooters = false,
        //                     });



        //                }
        //            }

        //        }
        //        else
        //        {
        //            e.MarkErrorAsHandled();
                    
        //            MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败: 服务器错误\n" + e.Error.Message);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败: \n" + e.Error.Message);
        //    }
       
        //}
        private void Txt_PageSize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (this.datasource.CanLoad == false)
            {
                
                return;
            }
            this.datasource.PageSize = Convert.ToInt32(e.NewValue);
        }
 

        void radDataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            //this.datasource.CanLoad=true;
            this.radDataPager.PageSize = this.PageSize<=0?20:this.PageSize;
            this.radDataPager.PageIndexChanged -= radDataPager_PageIndexChanged;
            //this.radDataPager.PageIndex = 1;
        }


        private void RadDataFilter_OnEditorCreated(object sender, EditorCreatedEventArgs e)
        {
            if (e.ItemPropertyDefinition.PropertyType == typeof(DateTime) || e.ItemPropertyDefinition.PropertyType == typeof(DateTime?))
            {
                RadDateTimePicker picker = (RadDateTimePicker)e.Editor;
                picker.InputMode=InputMode.DateTimePicker;
            }
        }

        private void Grid_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            var x = e.DataElement;
            if (x != null)
            {
                #region 调拨明细 调拨串码明细 未接受字体红色


                if (x is ReportService.Report_OutOrderListInfo &&
                    ((ReportService.Report_OutOrderListInfo)e.DataElement).已接收 != "是"
                    )
                {
                    e.Row.Foreground = Brushes.Red;
                }
                else if (x is ReportService.Report_OutOrderIMEIInfo &&
                    ((ReportService.Report_OutOrderIMEIInfo)e.DataElement).已接收 != "是"
                    )
                {
                    e.Row.Foreground = Brushes.Red;
                }
                else e.Row.Foreground = Brushes.Black;
                #endregion
            }
        }

        private void BookColHeader_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MyColumnSettings my = new MyColumnSettings();

            bool flag= my.OpenColumnSetting(this.reportname, this.ParentHead);
            if (flag == true)
            {
                this.Colhead = MyColumnSettings.GetColumns(this.reportname).ToList();
                this.InitGridColumnOwn(new API.Demo_ReportViewInfo() { Demo_ReportViewColumnInfo = this.Colhead });
            }
        }
    }
}
