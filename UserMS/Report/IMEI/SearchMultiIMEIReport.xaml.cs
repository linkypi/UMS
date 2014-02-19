using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using UserMS.ReportService;

namespace UserMS.Report.IMEI
{
    public partial class SearchMultiIMEIReport : BasePage
    {
        private string reportname = "Report_IMEIInfo";
        private Stream Exportfs;
        private int PageSize = 20;
        System.Data.Services.Client.DataServiceQuery xx;
        Entities a = new ReportServiceContext();

        public SearchMultiIMEIReport()
        {
            InitializeComponent();
            
           
        }

    
      
        // 当用户导航到此页面时执行。
        private void ReportTest_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ReportTest_OnLoaded;
         
            try
            {
                 
                //Entities a = new ReportServiceContext();

                //this.datasource.LoadingData += datasource_LoadingData;
                //this.datasource.LoadedData += datasource_LoadedData;
                //this.datasource.BeginInit();
                //this.datasource.AutoLoad = true;
                //this.datasource.PageSize = 20;
                //this.datasource.DataServiceContext = a;
                Telerik.Windows.Data.SortDescriptor sd = new Telerik.Windows.Data.SortDescriptor();
                sd.Member = "序号";
                sd.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                this.Grid.SortDescriptors.Add(sd);
                //            this.datasource.DataServiceContext = view;
                //this.datasource.QueryName = reportname;
                //this.datasource.Load();
                //this.datasource.EndInit();
                
                //this.InitReportInfo(this.Grid, busy, reportname);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + "");
            }
        }

        void datasource_LoadingData()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                this.busy.IsBusy = true;
                string[] strs = this.txt_iMEI.Text.Trim().Split(new Char[] { '\n' });
                // var query = a.CreateQuery<ReportService.Report_IMEIInfo>(this.reportname);

                //CompositeFilterDescriptorCollection CFS = new CompositeFilterDescriptorCollection();
                //foreach (string s in strs)
                //{
                //    if (string.IsNullOrEmpty(s))
                //        continue;
                //    FilterDescriptor f = new FilterDescriptor();
                //    f.Member = "串码";
                //    f.Value = s;
                //    f.Operator = FilterOperator.IsEqualTo;

                //    CFS.Add(f);
                //}
                //query.Where(CFS);

                //query.BeginExecute(datasource_LoadedData, query);



                var customerQuery = from cust in a.Report_IMEIInfo
                                    where 1 == 1
                                    select cust;

                foreach (string s in strs)
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    customerQuery = from cust in customerQuery
                                    where cust.串码.ToLower() == s.ToLower()
                                    select cust;
                }

                System.Data.Services.Client.DataServiceCollection<ReportService.Report_IMEIInfo> trackedCustomers =
                    new System.Data.Services.Client.DataServiceCollection<ReportService.Report_IMEIInfo>
                    (customerQuery);

                var list = trackedCustomers.ToList();

                this.Grid.ItemsSource = list;

                this.busy.IsBusy = false;
            });
            Thread.CurrentThread.Abort();
        }
        private void datasource_MultiMethod()
        {
            //var query = a.CreateQuery<ReportService.Report_IMEIInfo>("Report_IMEIInfoMulti").AddQueryOption("IMEIS", "'"+this.txt_iMEI.Text.Trim()+"'");

            //query.BeginExecute(datasource_LoadedData,query);
            string[] strs = this.txt_iMEI.Text.Trim().Split(new string[] { "\r\n" } , StringSplitOptions.RemoveEmptyEntries);
            PublicRequestHelp p = new PublicRequestHelp(this.busy, MethodIDInfo.Report_IMEIMulti_Search, new object[] { strs.ToList() }, datasource_LoadedData);

        }
        private void datasource_LoadedData(IAsyncResult result)
        {
            try
            {
                //string[] strs=null ;
                //Application.Current.Dispatcher.Invoke((Action)delegate { strs=this.txt_iMEI.Text.Trim().Split(new Char[] { '\n' }); });
                
                var query = (System.Data.Services.Client.DataServiceQuery<ReportService.Report_IMEIInfo>)result.AsyncState;
                
                var list=query.EndExecute(result).ToList();

                //list = list.Where(p => p.串码 == "TA04GN37DT58C7008125").ToList();
            

                Application.Current.Dispatcher.Invoke((Action)delegate { this.Grid.ItemsSource = list; });
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败，" + ex.Message);
            }
            finally
            {
                Application.Current.Dispatcher.Invoke((Action)delegate { this.busy.IsBusy = false; });

            }
        }

        private void datasource_LoadedData(object sender,API.MainCompletedEventArgs e)
        {
            List<API.Report_IMEIInfo> imeis = new List<API.Report_IMEIInfo>();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Error.Message);
                this.Grid.ItemsSource = imeis;
               
            }
            if (e.Result.ReturnValue != true)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                this.Grid.ItemsSource = imeis;
                 
            }
            else
            {
                this.Grid.ItemsSource = e.Result.Obj;
            }
            this.busy.IsBusy = false;
        }
        
       
       

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {

            try
            {
                 
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

                            this.Grid.Export(stream,
                             new GridViewExportOptions()
                             {
                                 Format = ExportFormat.Html,
                                 ShowColumnHeaders = true,
                                 ShowColumnFooters = true,
                                 ShowGroupFooters = false,
                             });



                        }
                    }
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"导出失败: \n" + ex.Message);
            }
        }

        
      

   

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_iMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入查询串码");
                return;
            }

            //Thread th = new Thread(new ThreadStart(datasource_LoadingData));
            //th.Start();
            //th.IsBackground = true;
            datasource_MultiMethod();
        }

       
        
      
    }
}
