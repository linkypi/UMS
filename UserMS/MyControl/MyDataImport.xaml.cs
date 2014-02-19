using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel;
using Telerik.Windows;
using Telerik.Windows.Controls;
using MessageBox = System.Windows.MessageBox;

namespace UserMS
{
    /// <summary>
    /// MyDataImport.xaml 的交互逻辑
    /// </summary>
    public class DataImportArgs:EventArgs
    {
        public DataTable Datas;
    public DataImportArgs()
    {
        
    }}
    public partial class MyDataImport : RadMenuItem
    {
        static bool IsNullable(Type T)
        {
           
            Type type =T;
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }
        public delegate void Imported(object sender, DataImportArgs e);
        public Type ImportType;
        public List<object> ImportResult;
        public event Imported OnImported;

        public MyDataImport()
        {
            InitializeComponent();
        }

        private void MyDataImport_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Excel 文件 (*.xls, *.xlsx)|*.xlsx;*.xls";
            Stream myStream;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            IExcelDataReader excelReader=null;
                            if (System.IO.Path.GetExtension(openFileDialog.SafeFileName) == ".xls")
                            {
                               excelReader= ExcelReaderFactory.CreateBinaryReader(myStream);
                            }
                            else  if (System.IO.Path.GetExtension(openFileDialog.SafeFileName) == ".xlsx")
                            {
                                excelReader = ExcelReaderFactory.CreateOpenXmlReader(myStream);
                            }
                            else
                            {
                                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导入失败");
                                return;
                            }
                            excelReader.IsFirstRowAsColumnNames = true;
                            DataSet result = excelReader.AsDataSet();
                            DataTable table = result.Tables[0];
                            this.ImportResult=new List<object>();
                            var fieldlist = Store.ExcelDataColumns[ImportType];
                            
                            //object model = Activator.CreateInstance(ImportType);
                            foreach (KeyValuePair<string, string> keyValuePair in fieldlist)
                            {
                                if (table.Columns.Contains(keyValuePair.Key))
                                {
                                    var fieldinfo = ImportType.GetProperty(keyValuePair.Value);
                                    Type propertytype;
                                    
                                        propertytype = Nullable.GetUnderlyingType(fieldinfo.PropertyType) ??
                                                       fieldinfo.PropertyType;

                                    DataColumn newcol = new DataColumn(keyValuePair.Value, propertytype, "[" + keyValuePair.Key + "]");
                                    table.Columns.Add(newcol);
                                }
                            }
                            OnImported(this, new DataImportArgs() { Datas = table });


                            myStream.Close();


                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "导入失败");
                
                }
            }
        }
    }
}
