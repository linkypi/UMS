using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SlModel;
using Telerik.Windows.Controls.DataServices;
using UserMS.ReportService;

namespace UserMS.DemoReport
{
    public partial class ReportTest : Page
    {
        private string reportname;
        private Stream Exportfs;

        public ReportTest()
        {
            InitializeComponent();
            
            
        }



        // 当用户导航到此页面时执行。
        private void ReportTest_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ReportTest_OnLoaded;
            reportname = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["name"];
            if (String.IsNullOrEmpty(reportname))
            {
                reportname = "GetPro_ProInfo";
            }
            //reportname = "Report_SellListInfo";
//            this.datasource.BeginInit();
            Entities a =new  ReportServiceContext();
//            var b=a.CreateQuery<Demo_各厅库存>(reportname);
//            //
//            
//            QueryableDataServiceCollectionView<Demo_各厅库存> view=new QueryableDataServiceCollectionView<Demo_各厅库存>(a,b);
//            var b = a.Execute<bool>(new Uri(string.Format("Login?username='{0}'&password='{1}'&sign=''", "1", "1"), UriKind.Relative));
//            a.Credentials=new NetworkCredential(
//                "1","1");

//           this.DataContext = view;
//            //this.radDataPager.DataContext = view;
//            
//            view.AutoLoad = true;
//            view.PageSize = 30;
//            view.Load();
            this.datasource.LoadedData += datasource_LoadedData;
            this.datasource.BeginInit();
            this.datasource.AutoLoad = true;
            this.datasource.PageSize = 30;
            this.datasource.DataServiceContext = a;
//            this.datasource.DataServiceContext = view;
            this.datasource.QueryName = reportname;
            this.datasource.Load();
            this.datasource.EndInit();
            
        }

        void datasource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.Error!=null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获取数据失败: 服务器错误");
                e.MarkErrorAsHandled();
            }
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
           
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.DefaultExt = "xls";
                    dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "Excel");
                    dialog.FilterIndex = 1;

                    if (dialog.ShowDialog() == true)
                    {
                        Exportfs = dialog.OpenFile();
                        this.datasource.BeginInit();
                        this.datasource.PageSize = 0;
                        
                        
                        this.datasource.EndInit();
                        this.datasource.LoadedData += ExportDataLoaded;
                        this.datasource.Load();

                    }
                    
            
        }

        private void ExportDataLoaded(object sender, LoadedDataEventArgs e)

        {
            
            this.datasource.LoadedData -= ExportDataLoaded;
            if (e.Error == null)
            {
                
                if (!e.Entities.Cast<object>().Any())
                {


                    var t = e.Entities.Cast<object>().Select(p => (object)p).ToList();
                    ExcelHelper.Export(t, Exportfs);
                    
                    this.datasource.BeginInit();
                    this.datasource.PageSize = 30;

                    this.datasource.EndInit();
                    this.datasource.Load();
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败: 无可用数据");
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败: 服务器错误\n" + e.Error.Message);
            }

            Exportfs.Close();
        }

      
    }
}
