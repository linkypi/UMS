using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using Telerik.Windows.Controls;

namespace UserMS.Report.InOrder
{
    public partial class InOrder_Report : BasePage
    {
        public int MethodID = 64;
        public int ExportMethodID = 65;

        public InOrder_Report()
        {
            InitializeComponent();
            InitGrid2();
        }


        private void InitGrid2()
        { 
            #region 列
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("Inorderid");
            col.Header = "入库单号";
            this.dataGrid1.Columns.Add(col);

 
            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("Pro_HallID");
            col2.Header = "仓库编码";
            this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col22 = new GridViewDataColumn();
            col22.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            col22.Header = "仓库名称";
            this.dataGrid1.Columns.Add(col22);


            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("OldID");
            col3.Header = "原始单号";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("InDate");
            col4.Header = "入库日期";
            this.dataGrid1.Columns.Add(col4);

            GridViewDataColumn col44 = new GridViewDataColumn();
            col44.DataMemberBinding = new System.Windows.Data.Binding("SysDate");
            col44.Header = "系统日期";
            this.dataGrid1.Columns.Add(col44);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col41.Header = "备注";
            this.dataGrid1.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("UserName");
            col5.Header = "录单人";
            this.dataGrid1.Columns.Add(col5);

          
           
            #endregion

            #region 取第一页的数据
            API.ReportPagingParam pageParam = new API.ReportPagingParam() { 
                PageIndex=0,
                PageSize=this.RadPager.PageSize,
                ParamList=new List<API.ReportSqlParams>()
            };
            this.InitPageEntity(MethodID,this.dataGrid1 ,this.busy, this.RadPager, pageParam);
	        #endregion

            
        }

        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取第一页的数据
            API.ReportPagingParam pageParam = new API.ReportPagingParam()
            {
                PageIndex = e.NewPageIndex,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>()
            };
            this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "97-2003   Excel文件(*.xls)|*.xls";
            if (saveFileDialog.ShowDialog() ?? false)
            {
                System.IO.Stream fs = saveFileDialog.OpenFile();
                SlModel.SheetColumn[] sheetColumns = new SlModel.SheetColumn[]
                {
                    new SlModel.SheetColumn(){Header="入库单号",ObjectProperty="InOrderID"},
                    new SlModel.SheetColumn(){Header="仓库编码",ObjectProperty="Pro_HallID"},
                    new SlModel.SheetColumn(){Header="仓库名称",ObjectProperty="HallName"},
                    new SlModel.SheetColumn(){Header="原始单号",ObjectProperty="OldID"},
                    new SlModel.SheetColumn(){Header="入库日期",ObjectProperty="InDate"},
                    new SlModel.SheetColumn(){Header="录入人",ObjectProperty="UserName"},
                    new SlModel.SheetColumn(){Header="备注",ObjectProperty="Note"}

                };
                string sheetName="入库记录";
                    try
                    {
                        #region 获取导出数据
                        API.ReportPagingParam pageParam = new API.ReportPagingParam()
                        {

                            ParamList = new List<API.ReportSqlParams>()
                        };
                        this.InitPageEntity(ExportMethodID, this.busy, pageParam, fs, sheetColumns, sheetName);
                
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,string.Format("导出失败：", ex.Message));
                    }
                }
            

           
           

        }
         

    }
}
