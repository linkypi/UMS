﻿using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Views.Configuration.Configuration_SellType
{
    public partial class AddConfiguration_SellType : BasePage
    {
        public AddConfiguration_SellType()
        {
            InitializeComponent();
            InitGrid2();
        }
         public int MethodID = 136;
        public int AddMethodID = 137;
 
        API.ReportPagingParam pageParam;//全局变量分页内容
        #region 新增数据
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TbSubit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {


            API.Pro_SellType SellType = new API.Pro_SellType();
            if (string.IsNullOrEmpty(SellTypeName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入销售类别名称");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定新增销售类别？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            SellType.Name = SellTypeName.Text.Trim();
            SellType.Note = SellTypeNote.Text.Trim();
            PublicRequestHelp help = new PublicRequestHelp(this.busy, AddMethodID, new object[] { SellType }, Completed);
      
            //if (pageParam != null)
            //    this.InitPageEntity(UpdateMethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
        }
        private void Completed(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.ReturnValue == true)
                {
                    Cancel();
                    this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
                }
                if (re.Result.Message != null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
                    Logger.Log(re.Result.Message);
                }
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
        }
        #endregion

        #region 生成列
        private void InitGrid2()
        {
            #region 列
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("Name");
            col.Header = "销售类别";
            this.dataGrid1.Columns.Add(col);


            //GridViewDataColumn col2 = new GridViewDataColumn();
            //col2.DataMemberBinding = new System.Windows.Data.Binding("Flag");
            //col2.Header = "使用状态";
            //this.dataGrid1.Columns.Add(col2);


            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col4.Header = "备注";
            this.dataGrid1.Columns.Add(col4);
            #endregion

            //#region 取第一页的数据
            pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>()
            };

            this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
        }
        #endregion
        #region 下一页事件
        /// <summary>
        /// 点击下一页时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取下一页的数据
            //API.ReportPagingParam pageParam = new API.ReportPagingParam()
            //{
            //    PageIndex = e.NewPageIndex,
            //    PageSize = this.RadPager.PageSize,
            //    ParamList = new List<API.ReportSqlParams>()
            //};
            pageParam.PageIndex = e.NewPageIndex;
            this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion
        #region 清空数据
        void Cancel()
        {
            SellTypeName.Text = string.Empty;

            SellTypeNote.Text = string.Empty;
        }
        #endregion 

        private void Cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"清空数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return; 
        
            Cancel();
        }

    }
}
