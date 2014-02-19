using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Views.Configuration.Configuration_Class
{
    public partial class Configuration_Class : BasePage
    {
        const int MethodID = 124;
        const int UpdateMethodID = 126;
        const int DeleteMethodID = 127;
        API.ReportPagingParam pageParam;//全局变量分页内容
        public Configuration_Class()
        {
            InitializeComponent();
             InitGrid2();
         
        }
              
        void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            Cancel();
            if (dataGrid1.SelectedItem != null)
            {
                API.Pro_ClassInfo ClassInfo = dataGrid1.SelectedItem as API.Pro_ClassInfo;
                IDCardPanel.DataContext = ClassInfo;
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
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"选择要修改商品类别！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定修改商品类别？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            API.Pro_ClassInfo ClassInfo = dataGrid1.SelectedItem as API.Pro_ClassInfo;
            API.Pro_ClassInfo NewClassInof = new API.Pro_ClassInfo();
            NewClassInof.ClassID = ClassInfo.ClassID;
            if (ClassName.Text != null && ClassInfo.ClassName != ClassName.Text.Trim())
                NewClassInof.ClassName = ClassName.Text.Trim();
            if (!string.IsNullOrEmpty(ClassOrder.Text))
            {
                try
                {
                    NewClassInof.Order = int.Parse(ClassOrder.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("请输入整数！");
                    return;
                }
            }
            //if (AreaFlag.Text != null)
            //    NewArea.Flag = AreaFlag.Text.Trim() == "正在使用" ? true : false;
            if (!string.IsNullOrEmpty(ClassNote.Text))
                NewClassInof.Note = ClassNote.Text.Trim();
            NewClassInof.HasSalary = HasSalary.IsChecked;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, UpdateMethodID, new object[] { NewClassInof }, Completed);
      
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
                        API.Pro_ClassInfo ClassInfo = re.Result.Obj as API.Pro_ClassInfo;
                        API.Pro_ClassInfo NewClassInfo = dataGrid1.SelectedItem as API.Pro_ClassInfo;
                        if (ClassInfo.ClassID == NewClassInfo.ClassID)
                        {
                            NewClassInfo.ClassName = ClassInfo.ClassName;
                            NewClassInfo.Order = ClassInfo.Order;
                            NewClassInfo.Note = ClassInfo.Note;
                            NewClassInfo.HasSalary = ClassInfo.HasSalary;
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
            col.DataMemberBinding = new System.Windows.Data.Binding("ClassName");
            col.Header = "类型名称";
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
            this.dataGrid1.ItemsSource = Store.ProClassInfo;
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
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除商品类别？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"选择要删除的商品类别！");
                return;
            }
            API.Pro_ClassInfo ClassInfo = dataGrid1.SelectedItem as API.Pro_ClassInfo;
            API.Pro_ClassInfo NewClassInfo = new API.Pro_ClassInfo();
            NewClassInfo.ClassID = ClassInfo.ClassID;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, DeleteMethodID, new object[] { NewClassInfo }, Completed1);
        }
        private void Completed1(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.ReturnValue == true)
                {
                    API.Pro_ClassInfo NewClassInfo = dataGrid1.SelectedItem as API.Pro_ClassInfo;
                    Store.ProClassInfo.RemoveAll(p => p.ClassID == NewClassInfo.ClassID);
                    Cancel();
                    //this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
                    this.dataGrid1.Rebind();
                    
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


