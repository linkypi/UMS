using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace Common
{
    public class ExportHelp
    {
        #region 导出
        public static void ExportFile(RadGridView dataGrid1, string selectedItem)
        {
            string extension = "";
            ExportFormat format = ExportFormat.Html;

            //RadComboBoxItem comboItem = ComboBox1.SelectedItem as RadComboBoxItem;
            //string selectedItem = comboItem.Content.ToString();

            switch (selectedItem)
            {
                case "Excel":
                    extension = "xls";
                    format = ExportFormat.Html;
                    break;
                case "Word":
                    extension = "doc";
                    format = ExportFormat.Html;
                    break;
                case "CSV":
                    extension = "csv";
                    format = ExportFormat.Csv;
                    break;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = extension;
            dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, selectedItem);
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    stream.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);
                    GridViewExportOptions exportOptions = new GridViewExportOptions();
                    exportOptions.Format = format;
                    exportOptions.ShowColumnFooters = true;
                    exportOptions.ShowColumnHeaders = true;
                    exportOptions.ShowGroupFooters = true;

                    dataGrid1.Export(stream, exportOptions);
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出完成");
                }
            }
        }
         
        public static void RadGridView1_ElementExporting(GridViewElementExportingEventArgs e, Color FCOlor, Color bgColor)
        {
            if (e.Element == ExportElement.HeaderRow || e.Element == ExportElement.FooterRow || e.Element == ExportElement.GroupFooterRow)
            {
                e.Background = bgColor;
                e.FontSize = 13;
                e.FontWeight = FontWeights.Bold;
            }
            else if (e.Element == ExportElement.Row)
            {
                e.Background = bgColor;
            }
            else if (e.Element == ExportElement.Cell &&
                     e.Value != null && e.Value.Equals("Chocolade"))
            {
                e.FontFamily = new FontFamily("Verdana");
                e.Background = Colors.LightGray;
                e.Foreground = Colors.Blue;
            }
            else if (e.Element == ExportElement.GroupHeaderRow)
            {
                e.FontFamily = new FontFamily("Verdana");
                e.Background = Colors.LightGray;
                e.Height = 30;
            }
            else if (e.Element == ExportElement.GroupHeaderCell &&
                     e.Value != null && e.Value.Equals("Chocolade"))
            {
                e.Value = "MyNewValue";
            }
            else if (e.Element == ExportElement.GroupFooterCell)
            {
                GridViewDataColumn column = e.Context as GridViewDataColumn;
                QueryableCollectionViewGroup qcvGroup = e.Value as QueryableCollectionViewGroup;

                if (column != null && qcvGroup != null && column.AggregateFunctions.Count() > 0)
                {
                    e.Value = GetAggregates(qcvGroup, column);
                }
            }
        }

        private static string GetAggregates(QueryableCollectionViewGroup group, GridViewDataColumn column)
        {
            List<string> aggregates = new List<string>();

            foreach (AggregateFunction f in column.AggregateFunctions)
            {
                foreach (AggregateResult r in group.AggregateResults)
                {
                    if (f.FunctionName == r.FunctionName && r.FormattedValue != null)
                    {
                        aggregates.Add(r.FormattedValue.ToString());
                    }
                }
            }

            return String.Join(",", aggregates.ToArray());
        }
        #endregion
    }
}