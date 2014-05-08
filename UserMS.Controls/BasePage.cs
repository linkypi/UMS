using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;
//using UserMSService;

namespace UserMS
{

    public class BasePage : Page
    {

        //public ChildWindow myChildFrom; 
        //public API.UserMsServiceClient MyClient;
        //        public Telerik.Windows.Controls.RadDomainDataSource datasource;
        public Telerik.Windows.Controls.RadGridView ParentRadGrid;
        public BusyIndicator Parentbusy;
        public Telerik.Windows.Controls.RadDataPager ParentPage;
        API.ReportPagingParam PageParam;
        System.IO.Stream fs;
        SlModel.SheetColumn[] sheetColumns;
        string sheetName;
        public List<API.Demo_ReportViewColumnInfo> ParentHead;
        public string ParentreportName;
        public List<API.Demo_ReportViewColumnInfo> Colhead=new List<API.Demo_ReportViewColumnInfo>();

        public BasePage()
        {
            
        }



        public void client_MainCompleted(object sender, API.MainCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 初始化分页数据(查询专属)
        /// </summary>
        /// <param name="radGrid"></param>
        protected void InitPageEntity(int MethodID,
        Telerik.Windows.Controls.RadGridView ParentRadGrid,
        BusyIndicator Parentbusy,
        Telerik.Windows.Controls.RadDataPager ParentPage,
        API.ReportPagingParam PageParam, bool IsSearch)
        {
            this.ParentRadGrid = ParentRadGrid;
            this.Parentbusy = Parentbusy;
            this.ParentPage = ParentPage;
            this.PageParam = PageParam;

            PublicRequestHelp h = new PublicRequestHelp(Parentbusy, MethodID, new object[] { PageParam, true }, MyClient_MainReportCompleted);

        }


        /// <summary>
        /// 初始化分页数据
        /// </summary>
        /// <param name="radGrid"></param>
        protected void InitPageEntity(int MethodID,
        Telerik.Windows.Controls.RadGridView ParentRadGrid,
        BusyIndicator Parentbusy,
        Telerik.Windows.Controls.RadDataPager ParentPage,
        API.ReportPagingParam PageParam)
        {
            this.ParentRadGrid = ParentRadGrid;
            this.Parentbusy = Parentbusy;
            this.ParentPage = ParentPage;
            this.PageParam = PageParam;
            PublicRequestHelp h = new PublicRequestHelp(Parentbusy, MethodID, new object[] { PageParam }, MyClient_MainReportCompleted);
        }

        protected void InitPageEntity(int MethodID,
    Telerik.Windows.Controls.RadGridView ParentRadGrid,
    BusyIndicator Parentbusy,
    Telerik.Windows.Controls.RadDataPager ParentPage,
    API.ReportPagingParam PageParam, string type)
        {
            this.ParentRadGrid = ParentRadGrid;
            this.Parentbusy = Parentbusy;
            this.ParentPage = ParentPage;
            this.PageParam = PageParam;
            if (!string.IsNullOrEmpty(type))
            {
                PublicRequestHelp h = new PublicRequestHelp(Parentbusy, MethodID, new object[] { PageParam, type }, MyClient_MainReportCompleted);
            }


        }
        /// <summary>
        /// 初始化分页数据  存储过程
        /// </summary>
        /// <param name="radGrid"></param>
        protected void InitPageEntity(int MethodID,
        Telerik.Windows.Controls.RadGridView ParentRadGrid,
        BusyIndicator Parentbusy,
        Telerik.Windows.Controls.RadDataPager ParentPage,
        API.ReportPagingParam PageParam, DateTime? dt1, DateTime? dt2)
        {
            this.ParentRadGrid = ParentRadGrid;
            this.Parentbusy = Parentbusy;
            this.ParentPage = ParentPage;
            this.PageParam = PageParam;

            PublicRequestHelp h = new PublicRequestHelp(Parentbusy, MethodID, new object[] { PageParam, dt1, dt2 }, MyClient_MainReportCompleted);

        }

        /// <summary>
        /// 取回分页数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyClient_MainReportCompleted(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {

                if (e.Result.ReturnValue == true)
                {
                    API.ReportPagingParam pageParem = (API.ReportPagingParam)e.Result.Obj;

                    this.ParentRadGrid.ItemsSource = pageParem.Obj;
                    PagedCollectionView pagedCollection = new PagedCollectionView(new int[pageParem.RecordCount]);
                    this.ParentPage.Source = pagedCollection;
                    this.ParentPage.PageIndex = pageParem.PageIndex;
                }
                else
                {
                    //this.ParentRadGrid.ItemsSource = null;
                    Logger.Log(e.Result.Message + "");
                }

            }
            catch (Exception ex)
            {
                //this.ParentRadGrid.ItemsSource = null;
                Logger.Log(ex.Message);
            }
            finally
            {
                this.Parentbusy.IsBusy = false;
            }
        }


        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="radGrid"></param>
        protected void InitPageEntity(int MethodID,
        BusyIndicator Parentbusy,
        API.ReportPagingParam PageParam, System.IO.Stream fs, SlModel.SheetColumn[] sheetColumns, string sheetName)
        {
            this.Parentbusy = Parentbusy;
            this.PageParam = PageParam;
            this.fs = fs;
            this.sheetColumns = sheetColumns;
            this.sheetName = sheetName;

            PublicRequestHelp h = new PublicRequestHelp(Parentbusy, MethodID, new object[] { PageParam }, MyClient_MainReportExportCompleted);



        }
        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="radGrid"></param>
        protected void InitPageEntity(int MethodID,
        BusyIndicator Parentbusy,
        API.ReportPagingParam PageParam, DateTime? dt1, DateTime? dt2, EventHandler<API.MainReportCompletedEventArgs> MainEvent)
        {
            this.Parentbusy = Parentbusy;
            this.PageParam = PageParam;


            PublicRequestHelp h = new PublicRequestHelp(Parentbusy, MethodID, new object[] { PageParam, dt1, dt2 }, MainEvent);



        }
        /// <summary>
        ///导出数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyClient_MainReportExportCompleted(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {

                if (e.Result.ReturnValue == true)
                {
                    API.ReportPagingParam pageParem = (API.ReportPagingParam)e.Result.Obj;

                    //this.ParentRadGrid.ItemsSource = pageParem.Obj;

                    ExportToExcel<object>(pageParem.Obj, fs);
                }
                else
                {
                    Logger.Log(e.Result.Message + "");
                }

            }
            catch (Exception ex)
            {
                //this.ParentRadGrid.ItemsSource = null;
                Logger.Log(ex.Message);
            }
            finally
            {
                this.Parentbusy.IsBusy = false;
            }
        }
        //<summary>
        //获取报表信息 请求
        //</summary>
        //<param name="reportID"></param>
        protected void InitReportInfo(Telerik.Windows.Controls.RadGridView ParentRadGrid,
        BusyIndicator Parentbusy, string reportName)
        {




            this.ParentRadGrid = ParentRadGrid;
            this.Parentbusy = Parentbusy;
            this.ParentreportName = reportName;
            PublicRequestHelp h = new PublicRequestHelp(reportName, MainReportComplete);



        }

        //        void datasource_LoadedData(object sender, Telerik.Windows.Controls.DomainServices.LoadedDataEventArgs e)
        //        {
        //            if (e.HasError) this.datasource.CancelLoad();
        //            //e.AllEntities.Count();
        //        }


        /// <summary>
        /// 获取报表信息 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MainReportComplete(object sender, API.MainReportViewInfoCompletedEventArgs e)
        {
            //return;
            try
            {
                if (e.Error != null)
                {
                    Logger.Log("请求出错，" + e.Error.Message);
                }
                else if (e.Result.ReturnValue == true)
                {
                    API.Demo_ReportViewInfo report = (API.Demo_ReportViewInfo)e.Result.Obj;
                    InitGridColumn(report);
        
                }
                else
                {
                    Logger.Log(e.Result.Message + "");
                }

            }
            catch (Exception ex)
            {
                //this.ParentRadGrid.ItemsSource = null;
                Logger.Log(ex.Message);
            }
            finally
            {
                this.Parentbusy.IsBusy = false;
            }
        }
        /// <summary>
        /// 初始化报表的字段
        /// </summary>
        /// <param name="report"></param>
        protected void InitGridColumn(API.Demo_ReportViewInfo report)
        {
            this.ParentRadGrid.Columns.Clear();
            GridViewSelectColumn cCol = new GridViewSelectColumn();
            this.ParentRadGrid.Columns.Add(cCol);
            this.ParentHead = report.Demo_ReportViewColumnInfo;
            Colhead = MyControl.MyColumnSettings.GetColumns(this.ParentreportName).ToList();
            var list = new List<API.Demo_ReportViewColumnInfo>();

            if (Colhead.Count() > 0)
            {
                list = this.Colhead;
            }
            else
            {
                list = this.ParentHead;
            }
            foreach (API.Demo_ReportViewColumnInfo colinfo in list)
            {

                GridViewDataColumn col = new GridViewDataColumn();
                col.DataMemberBinding = new System.Windows.Data.Binding(colinfo.ColName);
                col.Header = colinfo.ColDisPlayName;
                if (!string.IsNullOrEmpty(colinfo.FormatStr))
                {
                    col.DataFormatString = colinfo.FormatStr;
                    if (colinfo.FormatStr == "{0:N2}")
                    {
                        var func = new SumFunction();
                        func.ResultFormatString = "{0:N2}";
                        col.AggregateFunctions.Add(func);
                        //col.FooterAggregateFormatString
                    }
                }

                this.ParentRadGrid.Columns.Add(col);
            }
        }
        /// <summary>
        /// 自定义字段初始化方法
        /// </summary>
        /// <param name="report"></param>
        protected void InitGridColumnOwn(API.Demo_ReportViewInfo report)
        {
            this.ParentRadGrid.Columns.Clear();
            GridViewSelectColumn cCol = new GridViewSelectColumn();
            this.ParentRadGrid.Columns.Add(cCol);
            var list=new List<API.Demo_ReportViewColumnInfo>();

            if (Colhead.Count() > 0)
            {
                list = this.Colhead;
            }
            else
            {
                list = this.ParentHead;
            }
            foreach (API.Demo_ReportViewColumnInfo colinfo in list)
            {

                GridViewDataColumn col = new GridViewDataColumn();
                col.DataMemberBinding = new System.Windows.Data.Binding(colinfo.ColName);
                col.Header = colinfo.ColDisPlayName;
                if (!string.IsNullOrEmpty(colinfo.FormatStr))
                {
                    col.DataFormatString = colinfo.FormatStr;
                    if (colinfo.FormatStr == "{0:N2}")
                    {
                        var func = new SumFunction();
                        func.ResultFormatString = "{0:N2}";
                        col.AggregateFunctions.Add(func);
                        //col.FooterAggregateFormatString
                    }
                }

                this.ParentRadGrid.Columns.Add(col);
            }
        }
        protected void ExportToExcel<T>(object source, System.IO.Stream fs) where T : class
        {
            List<T> source__ = (List<T>)source;




            try
            {
                SlModel.ExcelHelper.Export<T>(source__, fs, this.sheetColumns, this.sheetName);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, string.Format("导出失败：", ex.Message));
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }


        }
        protected void ExportToExcel<T>(object source, System.IO.Stream fs, SlModel.SheetColumn[] sheetColumns, string sheetName) where T : class
        {
            List<T> source__ = (List<T>)source;




            try
            {
                SlModel.ExcelHelper.Export<T>(source__, fs, sheetColumns, sheetName);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, string.Format("导出失败：", ex.Message));
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }


        }
    }
}
