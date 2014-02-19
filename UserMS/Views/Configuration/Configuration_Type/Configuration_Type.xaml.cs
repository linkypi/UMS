using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Views.Configuration.Configuration_Type
{
    public partial class Configuration_Type : BasePage
    {
      const int MethodID = 128;
        const int UpdateMethodID = 130;
        const int DeleteMethodID = 131;
        API.ReportPagingParam pageParam;//全局变量分页内容
        public Configuration_Type()
        {
            InitializeComponent();
             InitGrid2();
         
        }
              
        void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            Cancel();
            if (dataGrid1.SelectedItem != null)
            {
                API.Pro_TypeInfo TypeInfo = dataGrid1.SelectedItem as API.Pro_TypeInfo;
                IDCardPanel.DataContext = TypeInfo;
            }
        }
        #region 清空数据
        void Cancel()
        {
            IDCardPanel.DataContext = null;
        }
        #endregion 

        #region 修改数据
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Update_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
 
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"选择要修改的商品品牌！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定修改商品品牌？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            API.Pro_TypeInfo TypeInfo = dataGrid1.SelectedItem as API.Pro_TypeInfo;
            API.Pro_TypeInfo NewTypeInfo = new API.Pro_TypeInfo();
            NewTypeInfo.TypeID = TypeInfo.TypeID;
            if (TypeName.Text != null && TypeInfo.TypeName != TypeName.Text.Trim())
                NewTypeInfo.TypeName = TypeName.Text.Trim();
            if (!string.IsNullOrEmpty(TypeOrder.Text))
                NewTypeInfo.Order = int.Parse(TypeOrder.Text.Trim());
            //if (AreaFlag.Text != null)
            //    NewArea.Flag = AreaFlag.Text.Trim() == "正在使用" ? true : false;
            if (!string.IsNullOrEmpty(TypeNote.Text))
                NewTypeInfo.Note = TypeNote.Text.Trim();

            PublicRequestHelp help = new PublicRequestHelp(this.busy, UpdateMethodID, new object[] { NewTypeInfo }, Completed);
      
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
                    if (re.Result.Obj != null)
                    {
                        API.Pro_TypeInfo TypeInfo = re.Result.Obj as API.Pro_TypeInfo;
                        API.Pro_TypeInfo NewTypeInfo = dataGrid1.SelectedItem as API.Pro_TypeInfo;
                        if (TypeInfo.TypeID == NewTypeInfo.TypeID)
                        {
                            NewTypeInfo.TypeName = TypeInfo.TypeName;
                            NewTypeInfo.Order = TypeInfo.Order;
                            NewTypeInfo.Note = TypeInfo.Note;
                        }
                        this.dataGrid1.Rebind();
                    }
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
            col.DataMemberBinding = new System.Windows.Data.Binding("TypeName");
            col.Header = "品牌名称";
            this.dataGrid1.Columns.Add(col);


            //GridViewDataColumn col2 = new GridViewDataColumn();
            //col2.DataMemberBinding = new System.Windows.Data.Binding("Flag");
            //col2.Header = "使用状态";
            //this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("Order");
            col3.Header = "排序";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col4.Header = "备注";
            this.dataGrid1.Columns.Add(col4);
            #endregion

            //#region 取第一页的数据
            //pageParam = new API.ReportPagingParam()
            //{
            //    PageIndex = 0,
            //    PageSize = this.RadPager.PageSize,
            //    ParamList = new List<API.ReportSqlParams>()
            //};

            //this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            this.dataGrid1.ItemsSource = Store.ProTypeInfo;
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
            //pageParam.PageIndex = e.NewPageIndex;
            //this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion
        #region 删除证件类别
        private void TbDelete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除商品品牌？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"选择要删除的商品品牌！");
                return;
            }
            API.Pro_TypeInfo Type = dataGrid1.SelectedItem as API.Pro_TypeInfo;
            API.Pro_TypeInfo NewType = new API.Pro_TypeInfo();
            NewType.TypeID = Type.TypeID;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, DeleteMethodID, new object[] { NewType }, Completed1);
        }
        private void Completed1(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.ReturnValue == true)
                {
                    API.Pro_TypeInfo NewTypeInfo = dataGrid1.SelectedItem as API.Pro_TypeInfo;
                    Store.ProTypeInfo.RemoveAll(p => p.TypeID == NewTypeInfo.TypeID);
                    this.dataGrid1.Rebind();
                    Cancel();
                    //this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
                    
                    
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
    }
}


