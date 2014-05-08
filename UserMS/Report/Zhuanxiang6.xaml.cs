using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Telerik.Pivot.Core;
using Telerik.Windows;
using Telerik.Windows.Controls.DataServices;
using Telerik.Windows.Controls.Pivot.Export;
using Telerik.Windows.Data;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;
using UserMS.ReportService;
using UserMS.App_Code;
using Convert = System.Convert;

namespace UserMS.Report
{
    /// <summary>
    /// Zhuanxiang.xaml 的交互逻辑
    /// </summary>
    public partial class Zhuanxiang6 : Page
    {
        private DateTime FirstDay;
        private DateTime LastDay;
        public Zhuanxiang6()
        {
            InitializeComponent();
            

            Entities e = new ReportServiceContext();
            this.datasource.AutoLoad = false;
            this.datasource.QueryName = "Chart_MapReport";
            this.datasource.DataServiceContext = e;
            this.datasource.LoadedData += datasource_LoadedData;
            this.datepicker.SelectionChanged+=DatepickerOnSelectionChanged;
            this.datepicker.SelectedDate = DateTime.Today;
             
        }

        private void DatepickerOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.busy.IsBusy = true;
            DateTime? d = this.datepicker.SelectedDate;
            if (d == null) return;
            FirstDay = new DateTime(d.Value.Year, d.Value.Month, 1);

            LastDay = FirstDay.AddMonths(1).AddDays(-1);
           this.datasource.FilterDescriptors.Clear();
            CompositeFilterDescriptor cfd = new CompositeFilterDescriptor();
        cfd.LogicalOperator = FilterCompositionLogicalOperator.And;
        FilterDescriptor f1 = new FilterDescriptor("DATE", FilterOperator.IsGreaterThanOrEqualTo, FirstDay);
        cfd.FilterDescriptors.Add(f1);
        FilterDescriptor f2 = new FilterDescriptor("DATE", FilterOperator.IsLessThanOrEqualTo, LastDay);
        cfd.FilterDescriptors.Add(f2);
//        FilterDescriptor f3 = new FilterDescriptor("ClassTypeID", FilterOperator.IsNotEqualTo,null);
//        cfd.FilterDescriptors.Add(f3);
           
        this.datasource.FilterDescriptors.Add(cfd);
            // this.datasource.LoadingData += datasource_LoadingData;
           



            this.datasource.Load();
        
        }

        private void datasource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.busy.IsBusy = false;

            this.textblock1.Text = this.datepicker.SelectedDate.HasValue? this.datepicker.SelectedDate.Value.ToString("yyyy年MM月dd日") + "":"";
            this.textblock2.Text = FirstDay.Year+"年"+FirstDay.Month+"月";
            (this.Resources["LocalDataProvider1"] as LocalDataSourceProvider).ItemsSource = e.Entities.OfType<Chart_MapReport>().Where(p => p.DATE == Convert.ToDateTime(this.datepicker.SelectedDate));
            (this.Resources["LocalDataProvider2"] as LocalDataSourceProvider).ItemsSource =
                e.Entities.OfType<Chart_MapReport>().Where(p => p.DATE >= FirstDay && p.DATE <= LastDay);
            
        }

        private void Export_OnClick(object sender, RadRoutedEventArgs e)
        {
            ExportToExcel();
        }



        private void ExportToExcel()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "xlsx";
            dialog.Filter = "Excel Workbook (xlsx) |*.xlsx|All Files (*.*) |*.*";

            var result = dialog.ShowDialog();
            if ((bool)result)
            {
                try
                {
                    using (var stream = dialog.OpenFile())
                    {
                        var workbook = GenerateWorkbook();

                        XlsxFormatProvider provider = new XlsxFormatProvider();
                        provider.Export(workbook, stream);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private Workbook GenerateWorkbook()
        {
            var export = pg1.GenerateExport();
            var export2 = pg2.GenerateExport();
            Workbook workbook = new Workbook();
            workbook.History.IsEnabled = false;

            var worksheet = workbook.Worksheets.Add();

            workbook.SuspendLayoutUpdate();
            int rowCount = Math.Max( export.RowCount,export2.RowCount)+1;
            int columnCount = export.ColumnCount + export2.ColumnCount;

            var allCells = worksheet.Cells[0, 0, rowCount - 1, columnCount - 1];
            allCells.SetFontFamily(new ThemableFontFamily(pg1.FontFamily));
            allCells.SetFontSize(12);
            allCells.SetFill(ExcelExportHelper.GenerateFill(pg1.Background));
            CellSelection title1 = worksheet.Cells[0, 0];
            CellSelection title2 = worksheet.Cells[0, export.ColumnCount];
            title1.SetValue(this.textblock1.Text+"专项通报");
            title2.SetValue(this.textblock2.Text + "专项通报");
            foreach (var cellInfo in export.Cells)
            {
                int rowStartIndex = cellInfo.Row+1;
                int rowEndIndex = rowStartIndex + cellInfo.RowSpan - 1;
                int columnStartIndex = cellInfo.Column;
                int columnEndIndex = columnStartIndex + cellInfo.ColumnSpan - 1;

                CellSelection cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex];

                var value = cellInfo.Value;
                if (value != null)
                {
                    cellSelection.SetValue(Convert.ToString(value));
                    cellSelection.SetVerticalAlignment(RadVerticalAlignment.Center);
                    cellSelection.SetHorizontalAlignment(ExcelExportHelper.GetHorizontalAlignment(cellInfo.TextAlignment));
                    int indent = cellInfo.Indent;
                    if (indent > 0)
                    {
                        cellSelection.SetIndent(indent);
                    }
                }

                cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex];

                ExcelExportHelper.SetCellProperties(cellInfo, cellSelection);
            }
            foreach (var cellInfo in export2.Cells)
            {
                int rowStartIndex = cellInfo.Row+1;
                int rowEndIndex = rowStartIndex + cellInfo.RowSpan - 1;
                int columnStartIndex = cellInfo.Column+export.ColumnCount;
                int columnEndIndex = columnStartIndex + cellInfo.ColumnSpan - 1;

                CellSelection cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex];

                var value = cellInfo.Value;
                if (value != null)
                {
                    cellSelection.SetValue(Convert.ToString(value));
                    cellSelection.SetVerticalAlignment(RadVerticalAlignment.Center);
                    cellSelection.SetHorizontalAlignment(ExcelExportHelper.GetHorizontalAlignment(cellInfo.TextAlignment));
                    int indent = cellInfo.Indent;
                    if (indent > 0)
                    {
                        cellSelection.SetIndent(indent);
                    }
                }

                cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex];

                ExcelExportHelper.SetCellProperties(cellInfo, cellSelection);
            }
            for (int i = 0; i < columnCount; i++)
            {
                var columnSelection = worksheet.Columns[i];
                columnSelection.AutoFitWidth();

                //NOTE: workaround for incorrect autofit.
                var newWidth = worksheet.Columns[i].GetWidth().Value.Value + 15;
                columnSelection.SetWidth(new ColumnWidth(newWidth, false));
            }

            workbook.ResumeLayoutUpdate();
            return workbook;
        }



    }
}
