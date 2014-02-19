using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using Telerik.Windows.Controls;

namespace UserMS.Report.InOrder
{
    public partial class InOutSell_Report : BasePage
    {
        //public int MethodID = 64;
        //public int ExportMethodID = 65;
        System.IO.Stream fs = null;

        public InOutSell_Report()
        {
            InitializeComponent();
            
            this.begintime.SelectedDate = DateTime.Now.AddDays(-30);
            this.endtime.SelectedDate = DateTime.Now;
            InitGrid2();
        }


        private void InitGrid2()
        { 
            #region 列
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("序号");
            col.Header = "序号";
            this.dataGrid1.Columns.Add(col);

 
            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("类别");
            col2.Header = "类别";
            this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col22 = new GridViewDataColumn();
            col22.DataMemberBinding = new System.Windows.Data.Binding("品牌");
            col22.Header = "品牌";
            this.dataGrid1.Columns.Add(col22);


            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("商品名称");
            col3.Header = "商品名称";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("商品属性");
            col4.Header = "商品属性";
            this.dataGrid1.Columns.Add(col4);

            GridViewDataColumn col44 = new GridViewDataColumn();
            col44.DataMemberBinding = new System.Windows.Data.Binding("期初库存");
            col44.Header = "期初库存";
            this.dataGrid1.Columns.Add(col44);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("本期初始入库");
            col41.Header = "本期初始入库";
            this.dataGrid1.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("本期调入");
            col5.Header = "本期调入";
            this.dataGrid1.Columns.Add(col5);

            GridViewDataColumn col6 = new GridViewDataColumn();
            col6.DataMemberBinding = new System.Windows.Data.Binding("本期调出");
            col6.Header = "本期调出";
            this.dataGrid1.Columns.Add(col6);

            GridViewDataColumn col7 = new GridViewDataColumn();
            col7.DataMemberBinding = new System.Windows.Data.Binding("本期销售");
            col7.Header = "本期销售";
            this.dataGrid1.Columns.Add(col7);

            GridViewDataColumn col8 = new GridViewDataColumn();
            col8.DataMemberBinding = new System.Windows.Data.Binding("本期退货");
            col8.Header = "本期退货";
            this.dataGrid1.Columns.Add(col8);

            GridViewDataColumn col9 = new GridViewDataColumn();
            col9.DataMemberBinding = new System.Windows.Data.Binding("本期送修");
            col9.Header = "本期送修";
            this.dataGrid1.Columns.Add(col9);

            GridViewDataColumn col10 = new GridViewDataColumn();
            col10.DataMemberBinding = new System.Windows.Data.Binding("本期返库");
            col10.Header = "本期返库";
            this.dataGrid1.Columns.Add(col10);

            GridViewDataColumn col11 = new GridViewDataColumn();
            col11.DataMemberBinding = new System.Windows.Data.Binding("本期借贷");
            col11.Header = "本期借贷";
            this.dataGrid1.Columns.Add(col11);

            GridViewDataColumn col12 = new GridViewDataColumn();
            col12.DataMemberBinding = new System.Windows.Data.Binding("本期归还");
            col12.Header = "本期归还";
            this.dataGrid1.Columns.Add(col12);

            GridViewDataColumn col13 = new GridViewDataColumn();
            col13.DataMemberBinding = new System.Windows.Data.Binding("期末库存");
            col13.Header = "期末库存";
            this.dataGrid1.Columns.Add(col13);

            GridViewDataColumn col14 = new GridViewDataColumn();
            col14.DataMemberBinding = new System.Windows.Data.Binding("送须累计");
            col14.Header = "送须累计";
            this.dataGrid1.Columns.Add(col14);

            GridViewDataColumn col15 = new GridViewDataColumn();
            col15.DataMemberBinding = new System.Windows.Data.Binding("借贷累计");
            col15.Header = "借贷累计";
            this.dataGrid1.Columns.Add(col15);

            GridViewDataColumn col16 = new GridViewDataColumn();
            col16.DataMemberBinding = new System.Windows.Data.Binding("门店");
            col16.Header = "门店";
            this.dataGrid1.Columns.Add(col16);

            GridViewDataColumn col17 = new GridViewDataColumn();
            col17.DataMemberBinding = new System.Windows.Data.Binding("区域");
            col17.Header = "区域";
            this.dataGrid1.Columns.Add(col17);
           
            #endregion

            #region 取第一页的数据
            API.ReportPagingParam pageParam = new API.ReportPagingParam() { 
                PageIndex=0,
                PageSize=this.RadPager.PageSize, 
                ParamList=new List<API.ReportSqlParams>()
            };
            this.InitPageEntity(MethodIDInfo.Report_InOutSellInfo_GetList,this.dataGrid1 ,this.busy, this.RadPager, pageParam,begintime.SelectedDate,endtime.SelectedDate);
	        #endregion

            
        }

        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取第一页的数据
            if (begintime.SelectedDate == null || endtime.SelectedDate == null)
            {
                this.begintime.SelectedDate = DateTime.Now.AddDays(-30);
                this.endtime.SelectedDate = DateTime.Now;
            }
            API.ReportPagingParam pageParam = new API.ReportPagingParam()
            {
                PageIndex = e.NewPageIndex,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>() { 
                new API.ReportSqlParams_String(){ParamName="类别" , ParamValues=ClassName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="品牌" , ParamValues=TypeName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="商品名称" , ParamValues=ProName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="门店" , ParamValues=HallName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="区域" , ParamValues=AreaName.Text.Trim()}
                }
            };
            this.InitPageEntity(MethodIDInfo.Report_InOutSellInfo_GetList, this.dataGrid1, this.busy, this.RadPager, pageParam,begintime.SelectedDate,endtime.SelectedDate);
            #endregion
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "97-2003   Excel文件(*.xls)|*.xls";
            if (saveFileDialog.ShowDialog() ?? false)
            {
                fs = saveFileDialog.OpenFile();
           
                    try
                    {
                        #region 获取导出数据
                        API.ReportPagingParam pageParam = new API.ReportPagingParam()
                        {

                            ParamList = new List<API.ReportSqlParams>() { 
                                 new API.ReportSqlParams_String(){ParamName="类别" , ParamValues=ClassName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="品牌" , ParamValues=TypeName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="商品名称" , ParamValues=ProName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="门店" , ParamValues=HallName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="区域" , ParamValues=AreaName.Text.Trim()}
                            }
                        };
                        this.InitPageEntity(MethodIDInfo.Report_InOutSellInfo_Export, this.busy, pageParam,  this.begintime.SelectedDate, this.endtime.SelectedDate, MyClient_MainReportExportCompleted);
                
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,string.Format("导出失败：", ex.Message));
                    }
               }
            

           
           

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

                   
                        SlModel.SheetColumn[] sheetColumns = new SlModel.SheetColumn[]
                    {
                        new SlModel.SheetColumn(){Header="序号",ObjectProperty="序号"},
                        new SlModel.SheetColumn(){Header="类别",ObjectProperty="类别"},
                        new SlModel.SheetColumn(){Header="品牌",ObjectProperty="品牌"},
                        new SlModel.SheetColumn(){Header="商品名称",ObjectProperty="商品名称"},
                        new SlModel.SheetColumn(){Header="商品属性",ObjectProperty="商品属性"},
                        new SlModel.SheetColumn(){Header="期初库存",ObjectProperty="期初库存"},
                        new SlModel.SheetColumn(){Header="本期初始入库",ObjectProperty="本期初始入库"},
                        new SlModel.SheetColumn(){Header="本期调入",ObjectProperty="本期调入"},
                        new SlModel.SheetColumn(){Header="本期调出",ObjectProperty="本期调出"},
                        new SlModel.SheetColumn(){Header="本期销售",ObjectProperty="本期销售"},
                        new SlModel.SheetColumn(){Header="本期退货",ObjectProperty="本期退货"},
                        new SlModel.SheetColumn(){Header="本期送修",ObjectProperty="本期送修"},
                        new SlModel.SheetColumn(){Header="本期返库",ObjectProperty="本期返库"},
                        new SlModel.SheetColumn(){Header="本期借贷",ObjectProperty="本期借贷"},
                        new SlModel.SheetColumn(){Header="本期归还",ObjectProperty="本期归还"},
                        new SlModel.SheetColumn(){Header="期末库存",ObjectProperty="期末库存"},
                        new SlModel.SheetColumn(){Header="送修累计",ObjectProperty="送修累计"},
                        new SlModel.SheetColumn(){Header="借贷累计",ObjectProperty="借贷累计"},
                        new SlModel.SheetColumn(){Header="门店",ObjectProperty="门店"},
                        new SlModel.SheetColumn(){Header="区域",ObjectProperty="区域"} 

                    };
                        string sheetName = "进销存" ;

                        //this.ParentRadGrid.ItemsSource = pageParem.Obj;

                        ExportToExcel<API.GetInOutSellInfoResult>(pageParem.Obj, fs, sheetColumns, sheetName);
                
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region 取第一页的数据
            if (begintime.SelectedDate == null || endtime.SelectedDate == null)
            {
                this.begintime.SelectedDate = DateTime.Now.AddDays(-30);
                this.endtime.SelectedDate = DateTime.Now;
            }
            this.RadPager.PageIndex = 0;
            API.ReportPagingParam pageParam = new API.ReportPagingParam()
            {
                PageIndex =this.RadPager.PageIndex,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>() { 
                new API.ReportSqlParams_String(){ParamName="类别" , ParamValues=ClassName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="品牌" , ParamValues=TypeName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="商品名称" , ParamValues=ProName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="门店" , ParamValues=HallName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="区域" , ParamValues=AreaName.Text.Trim()}
                }
            };
            this.InitPageEntity(MethodIDInfo.Report_InOutSellInfo_GetList, this.dataGrid1, this.busy, this.RadPager, pageParam, begintime.SelectedDate, endtime.SelectedDate);
            #endregion
        }
         

    }
}
