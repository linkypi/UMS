using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Views.UsersellMS.Usercenter
{
    public partial class IDCardMS : BasePage
    {
        const int MethodID = 106;
        const int UpdateMethodID = 107;
        const int DeleteMethodID = 111;
 
        API.ReportPagingParam pageParam;//全局变量分页内容

        public IDCardMS()
        {
            InitializeComponent();
            InitGrid2();

            #region 添加参数事件
            //添加搜索参数
            #endregion
        }

        void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                API.VIP_IDCardType IDCard = dataGrid1.SelectedItem as API.VIP_IDCardType;
                IDCardPanel.DataContext = IDCard;
            }
        }
        #region 清空数据
        void Cancel()
        {
            IDCardName.Text = string.Empty;
            Note.Text = string.Empty;
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
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定修改证件类别？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"选择要修改的证件类别！");
                return;
            }
            API.VIP_IDCardType NewIDCard = dataGrid1.SelectedItem as API.VIP_IDCardType;
            API.VIP_IDCardType IDCard = new API.VIP_IDCardType();
            IDCard.ID = NewIDCard.ID;
            if (IDCardName.Text != null && NewIDCard.Name != IDCardName.Text.Trim())
                IDCard.Name = IDCardName.Text.Trim();
            if (!string.IsNullOrEmpty(Note.Text))
                IDCard.Note = Note.Text.Trim();

            PublicRequestHelp help = new PublicRequestHelp(this.busy, UpdateMethodID, new object[] { IDCard }, Completed);
      
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
                        API.VIP_IDCardType IDCard = re.Result.Obj as API.VIP_IDCardType;
                        API.VIP_IDCardType NewIDCard = dataGrid1.SelectedItem as API.VIP_IDCardType;
                        if (IDCard.ID == NewIDCard.ID)
                        {
                            NewIDCard.Name = IDCard.Name;
                            NewIDCard.Note = IDCard.Note;
                        }
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
            //GridViewDataColumn col = new GridViewDataColumn();
            //col.DataMemberBinding = new System.Windows.Data.Binding("ID");
            //col.Header = "ID";
            //this.dataGrid1.Columns.Add(col);


            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("Name");
            col2.Header = "证件类别";
            this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col3.Header = "备注";
            this.dataGrid1.Columns.Add(col3);
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
            //pageParam = new API.ReportPagingParam()
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
        #region 删除证件类别
        private void TbDelete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除证件类别？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"选择要删除的证件类别！");
                return;
            }
            API.VIP_IDCardType NewIDCard = dataGrid1.SelectedItem as API.VIP_IDCardType;
            API.VIP_IDCardType IDCard = new API.VIP_IDCardType();
            IDCard.ID = NewIDCard.ID;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, DeleteMethodID, new object[] { IDCard }, Completed1);
        }
        private void Completed1(object sender, API.MainCompletedEventArgs re)
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
    }
}
