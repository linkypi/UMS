using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using Telerik.Pivot.Core;
using Telerik.Windows;
using Telerik.Windows.Controls.DataServices;
using Telerik.Windows.Data;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;
using UserMS.App_Code;
using UserMS.ReportService;

namespace UserMS.DemoReport
{
    public partial class Zhuanxiang2 : Page
    {
        public DateTime v;
        public Zhuanxiang2()
        {
            InitializeComponent();
//            DemoHallInfo d=new DemoHallInfo();
//            
            this.datepicker1.SelectedValue = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
            this.datepicker2.SelectedValue = DateTime.Today;

            Entities e = new ReportServiceContext();

            // this.datasource.LoadingData += datasource_LoadingData;
            this.datasource.LoadedData += datasource_LoadedData;
           
            this.datasource.AutoLoad = false;
            this.datasource.QueryName = "Report_YANBAO2";
         
            this.datasource.DataServiceContext = e;
            #region 报表






            ReportDataSource reportDataSource = new ReportDataSource();

            reportDataSource.Name = "DataSet1"; // Name of the DataSet we set in .rdlc

            _reportViewer.LocalReport.DisplayName = "销售报表";
            _reportViewer.LocalReport.ReportPath = "Report\\Zhuanxiang2.rdlc"; // Path of the rdlc file

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

        private void datasource_LoadedData(object sender, LoadedDataEventArgs e)
        {
//            var localDataProvider = this.Resources["datas"] as LocalDataSourceProvider;
//
//
//            localDataProvider.ItemsSource = e.Entities.Cast<Report_SellListInfo>().ToList();
            ReportDataSource reportDataSource = _reportViewer.LocalReport.DataSources.First();



            reportDataSource.Value = e.Entities;
            _reportViewer.LocalReport.SetParameters(new ReportParameter("ReportP1",v.Day+""));

            _reportViewer.RefreshReport();
        }

        private void Datas_OnPrepareDescriptionForField(object sender, PrepareDescriptionForFieldEventArgs e)
        {
//            if (e.DescriptionType == DataProviderDescriptionType.Group)
//            {
//                var d = e.Description as DateTimeGroupDescription;
//                if (d == null) return;
//
//            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {



            var LastDay = this.datepicker2.SelectedDate;
           
            if (!LastDay.HasValue) return;
            var FirstDay = new DateTime(LastDay.Value.Year, LastDay.Value.Month, 1);
            this.datepicker1.SelectedDate = FirstDay;
            v = LastDay.Value.AddDays(1);
            this.datasource.BeginInit();
            this.datasource.FilterDescriptors.Clear();
            CompositeFilterDescriptor cfd = new CompositeFilterDescriptor();
            cfd.LogicalOperator = FilterCompositionLogicalOperator.And;
            FilterDescriptor f1 = new FilterDescriptor("延保购买日期", FilterOperator.IsGreaterThanOrEqualTo, FirstDay);
            cfd.FilterDescriptors.Add(f1);
            FilterDescriptor f2 = new FilterDescriptor("延保购买日期", FilterOperator.IsLessThan, LastDay.Value.AddDays(1));
            cfd.FilterDescriptors.Add(f2);


            this.datasource.FilterDescriptors.Add(cfd);
            this.datasource.EndInit();
            this.datasource.Load();
        }

        private void RadMenuItem_OnClick(object sender, RadRoutedEventArgs e)
        {
//            ExportToExcel();
        }
//        private void ExportToExcel()
//        {
//            SaveFileDialog dialog = new SaveFileDialog();
//            dialog.DefaultExt = "xlsx";
//            dialog.Filter = "Excel Workbook (xlsx) |*.xlsx|All Files (*.*) |*.*";
//
//            var result = dialog.ShowDialog();
//            if ((bool)result)
//            {
//                try
//                {
//                    using (var stream = dialog.OpenFile())
//                    {
//                        var workbook = GenerateWorkbook();
//
//                        XlsxFormatProvider provider = new XlsxFormatProvider();
//                        provider.Export(workbook, stream);
//                    }
//                }
//                catch (IOException ex)
//                {
//                    MessageBox.Show(ex.Message);
//                }
//            }
//        }

//        private Workbook GenerateWorkbook()
//        {
//            var export = radPivotGrid1.GenerateExport();
//          
//            Workbook workbook = new Workbook();
//            workbook.History.IsEnabled = false;
//
//            var worksheet = workbook.Worksheets.Add();
//
//            workbook.SuspendLayoutUpdate();
//            int rowCount = export.RowCount+1;
//            int columnCount = export.ColumnCount +1;
//
//            var allCells = worksheet.Cells[0, 0, rowCount - 1, columnCount - 1];
//            allCells.SetFontFamily(new ThemableFontFamily(radPivotGrid1.FontFamily));
//            allCells.SetFontSize(12);
//            allCells.SetFill(ExcelExportHelper.GenerateFill(radPivotGrid1.Background));
//            
//            foreach (var cellInfo in export.Cells)
//            {
//                int rowStartIndex = cellInfo.Row +1;
//                int rowEndIndex = rowStartIndex + cellInfo.RowSpan - 1;
//                int columnStartIndex = cellInfo.Column;
//                if (columnStartIndex > 0) columnStartIndex += 1;
//                var value = cellInfo.Value;
//                if (value != null && columnStartIndex == 0)
//                {
//                    int indent = cellInfo.Indent;
//                    if (indent > 0)
//                    {
//                        columnStartIndex = 1;
//                    }
//                }
//                int columnEndIndex = columnStartIndex + cellInfo.ColumnSpan - 1;
//                
//                CellSelection cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex];
//
//                if (value != null)
//                {
//                    cellSelection.Merge();
//                    cellSelection.SetValue(Convert.ToString(value));
//                    cellSelection.SetVerticalAlignment(RadVerticalAlignment.Center);
//                    cellSelection.SetHorizontalAlignment(ExcelExportHelper.GetHorizontalAlignment(cellInfo.TextAlignment));
////                    int indent = cellInfo.Indent;
////                    if (indent > 0)
////                    {
////                        cellSelection.SetIndent(indent);
////                    }
//                }
//
//                cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex];
//
//                ExcelExportHelper.SetCellProperties(cellInfo, cellSelection);
//            }
//           
//            for (int i = 0; i < columnCount; i++)
//            {
//                var columnSelection = worksheet.Columns[i];
//                columnSelection.AutoFitWidth();
//
//                //NOTE: workaround for incorrect autofit.
//                var newWidth = worksheet.Columns[i].GetWidth().Value.Value + 15;
//                columnSelection.SetWidth(new ColumnWidth(newWidth, false));
//            }
//
//            workbook.ResumeLayoutUpdate();
//            return workbook;
//        }


    }
}
