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
using UserMS.Views.ProSell.Price;

namespace UserMS.DemoReport

{
    public class Zhuanxiang5ReportModel
    {
        public string 大区 { get; set; }
        public string 区域 { get; set; }
        public string 店名 { get; set; }
        public string 网点属性 { get; set; }
        public decimal 累计金额 { get; set; }

        public decimal 终端销量
        {
            get;
            set;
        }
        public decimal 终端其他
        {
            get { return 终端销量 - 终端单卖 - 终端兑券; }
            
        }
        public decimal 终端单卖 { get; set; }
        public decimal 终端兑券 { get; set; }
        public decimal 终端合约 { get; set; }
        public decimal 终端促销 { get; set; }
        public decimal 延保销量
        {
            get; set;
        }
        public decimal 延保其他
        {
            get { return 延保销量 - 延保单卖 - 延保兑券; }
        }
        public decimal 延保单卖 { get; set; }
        public decimal 延保兑券 { get; set; }
        public decimal 延保合约 { get; set; }
        public decimal 延保促销 { get; set; }
        
    }



    public partial class Zhuanxiang5 : Page
    {
        public DateTime v;
        private List<Report_SellListInfo2> selllists;
        private List<Report_YANBAO2> yanbaolists;
        private int loaded = 0;
        public Zhuanxiang5()
        {
            InitializeComponent();
//            DemoHallInfo d=new DemoHallInfo();
//            
            this.datepicker1.SelectedValue = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
            this.datepicker2.SelectedValue = DateTime.Today;

            Entities e = new ReportServiceContext();
            Entities e2 = new ReportServiceContext();
            // this.datasource.LoadingData += datasource_LoadingData;
            this.datasource.LoadedData += datasource_LoadedData;
           
            this.datasource.AutoLoad = false;
            this.datasource.QueryName = "Report_SellListInfo2";
         
            this.datasource.DataServiceContext = e;
            this.datasource2.LoadedData += datasource_LoadedData2;

            this.datasource2.AutoLoad = false;
            this.datasource2.QueryName = "Report_YANBAO2";

            this.datasource2.DataServiceContext = e2;
            #region 报表






            ReportDataSource reportDataSource = new ReportDataSource();

            reportDataSource.Name = "DataSet1"; // Name of the DataSet we set in .rdlc

            _reportViewer.LocalReport.DisplayName = "销售报表";
            _reportViewer.LocalReport.ReportPath = "Report\\Zhuanxiang5.rdlc"; // Path of the rdlc file

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

        void genreport()
        {

            var a = selllists.GroupBy(p => p.门店编码).Select(p => p.Key);
            var b = yanbaolists.GroupBy(p => p.门店编码).Select(p => p.Key);
            var c = a.ToList();
            c.AddRange(b);
            c=c.Distinct().ToList();
            List<Zhuanxiang5ReportModel> result =new List<Zhuanxiang5ReportModel>();
            foreach (string s in c)
            {
                string v1;
                string v2;
                Zhuanxiang5ReportModel m=new Zhuanxiang5ReportModel();
                if (selllists.Any(p => p.门店编码 == s))
                {
                    var query = selllists.Where(p => p.门店编码 == s);
                    m.大区 = query.First().大区;
                    m.区域 = query.First().片区;
                    m.店名 = query.First().门店;
                    m.终端促销 = Convert.ToDecimal(query.Where(p => p.销售方式.Contains("促销")).Sum(p => p.数量));
                    m.终端单卖 = Convert.ToDecimal(query.Where(p => p.销售方式.Contains("单卖")).Sum(p => p.数量));
                    m.终端兑券 = Convert.ToDecimal(query.Where(p => p.销售方式.Contains("兑券")).Sum(p => p.数量));
                    m.终端合约 = Convert.ToDecimal(query.Where(p => p.销售方式.Contains("合约")).Sum(p => p.数量));
                    m.终端销量 = Convert.ToDecimal(query.Sum(p => p.数量));

                }
                if (yanbaolists.Any(p => p.门店编码 == s))
                {
                    var query = yanbaolists.Where(p => p.门店编码 == s);
                    m.大区 = query.First().大区;
                    m.区域 = query.First().区域;
                    m.店名 = query.First().店名;
                    m.延保促销 = Convert.ToDecimal(query.Where(p => p.手机购买方式.Contains("促销")).Sum(p => p.销售数量));
                    m.延保单卖 = Convert.ToDecimal(query.Where(p => p.手机购买方式.Contains("单卖")).Sum(p => p.销售数量));
                    m.延保兑券 = Convert.ToDecimal(query.Where(p => p.手机购买方式.Contains("兑券")).Sum(p => p.销售数量));
                    m.延保合约 = Convert.ToDecimal(query.Where(p => p.手机购买方式.Contains("合约")).Sum(p => p.销售数量));
                    m.延保销量 = Convert.ToDecimal(query.Sum(p => p.销售数量));
                    m.累计金额 = Convert.ToDecimal(query.Sum(p => p.销售数量*p.延保价格));
                }
                result.Add(m);
            }

            


            

            ReportDataSource reportDataSource = _reportViewer.LocalReport.DataSources.First();



            reportDataSource.Value = result;
            //_reportViewer.LocalReport.SetParameters(new ReportParameter("ReportP1", v.Day + ""));

            _reportViewer.RefreshReport();
        }
        private void datasource_LoadedData(object sender, LoadedDataEventArgs e)
        {
//            var localDataProvider = this.Resources["datas"] as LocalDataSourceProvider;
//
//
//            localDataProvider.ItemsSource = e.Entities.Cast<Report_SellListInfo>().ToList();
            if (!e.HasError)
            {
                loaded += 1;
                selllists = e.Entities.Cast<Report_SellListInfo2>().ToList();
                if (loaded == 2)
                {
                    
                    genreport();
                }
            }
           
        }
        private void datasource_LoadedData2(object sender, LoadedDataEventArgs e)
        {
            //            var localDataProvider = this.Resources["datas"] as LocalDataSourceProvider;
            //
            //
            //            localDataProvider.ItemsSource = e.Entities.Cast<Report_SellListInfo>().ToList();
            if (!e.HasError)
            {
                loaded += 1;
                yanbaolists = e.Entities.Cast<Report_YANBAO2>().ToList();
                if (loaded == 2)
                {

                    genreport();
                }
            }
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
            loaded = 0;
            var LastDay = this.datepicker2.SelectedDate;

            if (!LastDay.HasValue) return;
            var FirstDay = new DateTime(LastDay.Value.Year, LastDay.Value.Month, 1);
            this.datepicker1.SelectedDate = FirstDay;
            v = LastDay.Value.AddDays(1);
            this.datasource.BeginInit();
            this.datasource.FilterDescriptors.Clear();
            CompositeFilterDescriptor cfd = new CompositeFilterDescriptor();
            cfd.LogicalOperator = FilterCompositionLogicalOperator.And;
            FilterDescriptor f1 = new FilterDescriptor("销售日期", FilterOperator.IsGreaterThanOrEqualTo, FirstDay);
            cfd.FilterDescriptors.Add(f1);
            FilterDescriptor f2 = new FilterDescriptor("销售日期", FilterOperator.IsLessThan, LastDay.Value.AddDays(1));
            cfd.FilterDescriptors.Add(f2);
            FilterDescriptor f3 = new FilterDescriptor("商品大类编码", FilterOperator.IsEqualTo, 1);
            cfd.FilterDescriptors.Add(f3);


            this.datasource.FilterDescriptors.Add(cfd);
            this.datasource.EndInit();
            

            this.datasource2.BeginInit();
            this.datasource2.FilterDescriptors.Clear();
            CompositeFilterDescriptor cfd2 = new CompositeFilterDescriptor();
            cfd2.LogicalOperator = FilterCompositionLogicalOperator.And;
            FilterDescriptor f21 = new FilterDescriptor("延保购买日期", FilterOperator.IsGreaterThanOrEqualTo, FirstDay);
            cfd2.FilterDescriptors.Add(f21);
            FilterDescriptor f22 = new FilterDescriptor("延保购买日期", FilterOperator.IsLessThan, LastDay.Value.AddDays(1));
            cfd2.FilterDescriptors.Add(f22);



            this.datasource2.FilterDescriptors.Add(cfd2);
            this.datasource2.EndInit();
            this.datasource.Load();
            this.datasource2.Load();
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
