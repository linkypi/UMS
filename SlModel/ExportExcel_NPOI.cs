using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace SlModel
{

    public class operateExcel<T>
    {
        public operateExcel()
        {
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="head">中文列名对照</param>
        /// <param name="workbookFile">保存路径</param>
        public void getExcel(List<T> lists, Hashtable head, Stream fs)
        {
            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
                HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                bool h = false;
                int j = 1;
                if (lists == null || lists.Count == 0)
                {
                    return;
                }
                var a = lists.First();
                Type type = a.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (T item in lists)
                {
                    HSSFRow dataRow = sheet.CreateRow(j) as HSSFRow;
                    int i = 0;
                    foreach (PropertyInfo column in properties)
                    {
                        if (column.Name.Equals("ExtensionData"))
                        {
                            continue;
                        }
                        if (head[column.Name] == null)
                        {
                            continue;
                        }
                        if (!h)
                        {
                            //column.PropertyType
                            headerRow.CreateCell(i).SetCellValue(head[column.Name] == null ? column.Name : head[column.Name].ToString());

                            //dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                            SetCellValue(column, dataRow, i, item);
                        }
                        else
                        {
                            //dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                            SetCellValue(column, dataRow, i, item);
                        }

                        i++;
                    }
                    h = true;
                    j++;
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                sheet = null;
                headerRow = null;
                workbook = null;
                //FileStream fs = new FileStream(workbookFile, FileMode.Create, FileAccess.Write);
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                data = null;
                ms = null;
                fs = null;
            }
            catch (Exception ee)
            {
                string see = ee.Message;
            }
        }

        public void getExcel(List<T> lists, List<string> headerNames,List<string> headerFields, Stream fs)
        {
            try
            {
                if (headerFields == null || headerNames == null)
                {
                    return;
                }
                if (headerNames.Count != headerFields.Count)
                {
                    return;
                }
                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
                HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                bool h = false;
                int j = 1;
                if (lists == null || lists.Count == 0)
                {
                    return;
                }
                var a = lists.First();
                Type type = a.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (T item in lists)
                {
                    HSSFRow dataRow = sheet.CreateRow(j) as HSSFRow;
                    int i = 0;
                    foreach (var child in headerFields)
                    {

                        var field = from b in properties
                                    where b.Name == child
                                    select b;

                        if (field.Count() == 0)
                        {
                            continue;
                        }
                        PropertyInfo prop = field.First();
                        int index = headerFields.IndexOf(child);
                        if (!h)
                        {
                            //column.PropertyType
                            headerRow.CreateCell(i).SetCellValue( headerNames[index]);

                            //dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                            SetCellValue(prop, dataRow, i, item);
                        }
                        else
                        {
                            //dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                            SetCellValue(prop, dataRow, i, item);
                        }

                        i++;

                    }

                    #region 
                    //foreach (PropertyInfo column in properties)
                    //{
                    //    if (column.Name.Equals("ExtensionData"))
                    //    {
                    //        continue;
                    //    }
                    //    var field = from b in headerFields
                    //            where b == column.Name
                    //            select b;

                    //    if (field.Count() == 0)
                    //    {
                    //        continue;
                    //    }
                    //    string fd = field.First();
                    //    int index = headerFields.IndexOf(fd);
                    //    if (!h)
                    //    {
                    //        //column.PropertyType
                    //        headerRow.CreateCell(i).SetCellValue(headerNames[index] == null ? column.Name : headerNames[index]);

                    //        //dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                    //        SetCellValue(column, dataRow, i, item);
                    //    }
                    //    else
                    //    {
                    //        //dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                    //        SetCellValue(column, dataRow, i, item);
                    //    }

                    //    i++;
                    //}
                    #endregion 

                    h = true;
                    j++;
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                sheet = null;
                headerRow = null;
                workbook = null;
                //FileStream fs = new FileStream(workbookFile, FileMode.Create, FileAccess.Write);
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                data = null;
                ms = null;
                fs = null;
            }
            catch (Exception ee)
            {
                string see = ee.Message;
            }
        }
      
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="head">中文列名对照</param>
        /// <param name="workbookFile">保存路径</param>
        public void getExcel(List<T> lists, List<string> head, Stream fs)
        {
            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
                HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                bool h = false;
                int j = 1;
                if (lists == null || lists.Count == 0)
                {
                    return;
                }
                var a = lists.First();
                Type type = a.GetType();
                PropertyInfo[] properties = type.GetProperties();

                for (int col = 0; col < head.Count; col++)
                {
                    headerRow.CreateCell(col).SetCellValue(head[col]);
                }

                foreach (T item in lists)
                {
                    HSSFRow dataRow = sheet.CreateRow(j) as HSSFRow;

                    int i = 0;
                    foreach (PropertyInfo column in properties)
                    {
                        if (column.Name.Equals("ExtensionData"))
                        {
                            continue;
                        }
                        i = head.IndexOf(column.Name);
                        if (i < 0) continue;
                        SetCellValue(column, dataRow, i, item);

                    }
                    h = true;
                    j++;
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                sheet = null;
                headerRow = null;
                workbook = null;
                //FileStream fs = new FileStream(workbookFile, FileMode.Create, FileAccess.Write);
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                data = null;
                ms = null;
                fs = null;
            }
            catch (Exception ee)
            {
                string see = ee.Message;
            }
        }

        private void SetCellValue(PropertyInfo column, HSSFRow dataRow, int i, T item)
        {
            string typeName = column.PropertyType.FullName;
            switch (typeName)
            {
                case "System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                    dataRow.CreateCell(i).SetCellValue(Convert.ToDouble(column.GetValue(item, null)));
                    break;
                case "System.Decimal":
                    dataRow.CreateCell(i).SetCellValue(Convert.ToDouble(column.GetValue(item, null)));
                    break;
                case "System.Int":
                    dataRow.CreateCell(i).SetCellValue(Convert.ToInt32(column.GetValue(item, null)));
                    break;
                case "System.Int32":
                    dataRow.CreateCell(i).SetCellValue(Convert.ToInt32(column.GetValue(item, null)));
                    break;
                case "System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                    dataRow.CreateCell(i).SetCellValue(Convert.ToInt32(column.GetValue(item, null)));
                    break;
                case "System.Int64":
                    dataRow.CreateCell(i).SetCellValue(Convert.ToInt64(column.GetValue(item, null)));
                    break;
                case "System.Nullable`1[[System.Int64, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                    dataRow.CreateCell(i).SetCellValue(Convert.ToInt64(column.GetValue(item, null)));
                    break;

                case "System.Float":
                    dataRow.CreateCell(i).SetCellValue(Convert.ToDouble(column.GetValue(item, null)));
                    break;
                case "System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                    dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? null : Convert.ToDateTime(column.GetValue(item, null)).ToShortDateString());
                    break;
                case "System.String":
                    dataRow.CreateCell(i).SetCellValue((column.GetValue(item, null)) + "");
                    break;
                default:
                    //o = value;
                    break;
            }
        }
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="head">中文列名对照</param>
        /// <param name="workbookFile">Excel所在路径</param>
        /// <returns></returns>
        public List<T> fromExcel(Hashtable head, string workbookFile)
        {
            try
            {
                HSSFWorkbook hssfworkbook;
                List<T> lists = new List<T>();
                using (FileStream file = new FileStream(workbookFile, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                HSSFSheet sheet = hssfworkbook.GetSheetAt(0) as HSSFSheet;
                IEnumerator rows = sheet.GetRowEnumerator();
                HSSFRow headerRow = sheet.GetRow(0) as HSSFRow;
                int cellCount = headerRow.LastCellNum;
                //Type type = typeof(T);
                PropertyInfo[] properties;
                T t = default(T);
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    HSSFRow row = sheet.GetRow(i) as HSSFRow;
                    t = Activator.CreateInstance<T>();
                    properties = t.GetType().GetProperties();
                    foreach (PropertyInfo column in properties)
                    {
                        int j = headerRow.Cells.FindIndex(delegate(ICell c)
                        {
                            return c.StringCellValue == (head[column.Name] == null ? column.Name : head[column.Name].ToString());
                        });
                        if (j >= 0 && row.GetCell(j) != null)
                        {
                            object value = valueType(column.PropertyType, row.GetCell(j).ToString());
                            column.SetValue(t, value, null);
                        }
                    }
                    lists.Add(t);
                }
                return lists;
            }
            catch (Exception ee)
            {
                string see = ee.Message;
                return null;
            }
        }

        object valueType(Type t, string value)
        {
            object o = null;
            string strt = "String";
            if (t.Name == "Nullable`1")
            {
                strt = t.GetGenericArguments()[0].Name;
            }
            switch (strt)
            {
                case "Decimal":
                    o = decimal.Parse(value);
                    break;
                case "Int":
                    o = int.Parse(value);
                    break;
                case "Float":
                    o = float.Parse(value);
                    break;
                case "DateTime":
                    o = DateTime.Parse(value);
                    break;
                default:
                    o = value;
                    break;
            }
            return o;
        }
    }

}
