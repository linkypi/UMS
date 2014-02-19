using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Lite.ExcelLibrary.SpreadSheet;

namespace SlModel
{
    public class ExcelHelper
    {

        public static List<PropertyInfo> getColomnPropertyInfos(object t0)
        {
            List<PropertyInfo> columnPropertyInfos = new List<PropertyInfo>();
            Type t = t0.GetType();
            if (t.IsClass)
            {
                System.Reflection.PropertyInfo[] properties =
                    t.GetProperties(System.Reflection.BindingFlags.Instance |
                                    System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public);

                foreach (PropertyInfo item in properties)
                {
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        if (!columnPropertyInfos.Contains(item))
                        { columnPropertyInfos.Add(item); }
                    }
                }
            }
            return columnPropertyInfos; 
        }

       
        public static void Export<T>(IEnumerable<T> source, Stream fs, SheetColumn[] sheetColumns = null, string sheetName = "") where T : class
        {
            

            Type t = typeof(T);
            if (t.IsAutoClass)
            {
                throw new Exception("暂不支持导出匿名类格式数据！");
            } var columnPropertys = getColomnPropertyInfos(source.First());
            var columnHeaders = new List<SheetColumn>();
            if (sheetColumns == null)
            {
                columnHeaders = columnPropertys.Select(x =>
                    new SheetColumn { Header = x.Name, ObjectProperty = x.Name }).ToList();
            }
            else
            {
                columnHeaders = sheetColumns.ToList();
                columnPropertys = columnPropertys.Join(sheetColumns,
                    x => x.Name, y => y.ObjectProperty, (x, y) => x).ToList();
            }
            List<string> HeaderStr = (from b in columnHeaders
                                     select b.ObjectProperty).ToList();

            if (!columnPropertys.Any())
            {
            }
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Sheet1";
            }
            Workbook newWorkBook = new Workbook();

            //需要建立多少个sheet表
            int sheetCount = source.Count() % 50000 == 0 ? source.Count() / 50000 : source.Count() / 50000 + 1;

            for (int i = 0; i < sheetCount;i++ )
            {

                Worksheet newWorkSheet = new Worksheet(sheetName+(i+1));
                newWorkBook.Worksheets.Add(newWorkSheet);
                newWorkSheet.SheetType = SheetType.Worksheet;
                newWorkSheet.Cells.FirstColIndex = 0;
                newWorkSheet.Cells.LastColIndex = columnPropertys.Count;
                newWorkSheet.Cells.FirstRowIndex = 0;
                if (i + 1 == sheetCount)
                    newWorkSheet.Cells.LastRowIndex = source.Count() - i * 50000;
                else
                    newWorkSheet.Cells.LastRowIndex = 50000;
                Row headerRow = new Row();
                headerRow.FirstColIndex = 0;
                var colIndex = 0;
                columnHeaders.Select(x => new Cell(x.Header)).ToList().ForEach(headerCell =>
                { headerRow.SetCell(colIndex++, headerCell); });
                newWorkSheet.Cells.Rows.Add(newWorkSheet.Cells.Rows.Count, headerRow);
                foreach (var item in source)
                {
                    Row dataRow = new Row();
                    colIndex = 0;
                    dataRow.FirstColIndex = 0;
                    foreach (var p in columnPropertys)
                    {
                        int index = HeaderStr.IndexOf(p.Name);
                        if (index < 0) continue;
                        dataRow.SetCell(index, new Cell(p.GetValue(item, null) + ""));
                    }
                    newWorkSheet.Cells.Rows.Add(newWorkSheet.Cells.Rows.Count, dataRow);
                }
                
            }

            
            newWorkBook.Save(fs);
        }
        private static List<PropertyInfo> getColomnPropertyInfos<T>() 
        { 
            List<PropertyInfo> columnPropertyInfos = new List<PropertyInfo>(); 
            Type t = typeof(T); 
            if (t.IsClass) 
            { 
                System.Reflection.PropertyInfo[] properties = 
                    t.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public); 
                foreach (PropertyInfo item in properties) 
                { 
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String")) 
                    { 
                        if (!columnPropertyInfos.Contains(item)) 
                        { columnPropertyInfos.Add(item); }
                    } 
                } 
            } 
            return columnPropertyInfos; 
        }          
        private static List<string> getColomnHeaders<T>() 
        { 
            return getColomnPropertyInfos<T>().Select(x => x.Name).ToList();
        }



    }
}
