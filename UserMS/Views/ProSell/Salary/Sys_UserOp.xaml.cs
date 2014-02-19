using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Salary
{
    public partial class Sys_UserOp : BasePage
    {
        API.ReportPagingParam pageParam;//全局变量分页内容
        private List<API.Sys_UserOp> addUserOpList = new List<API.Sys_UserOp>();
        /// <summary>
        /// 将新增的职位列表
        /// </summary>

        public List<API.Sys_UserOp> AddUserOpList
        {
            get { return addUserOpList; }
            set { addUserOpList = value; }
        }

        public Sys_UserOp()
        {
            InitializeComponent();
            GetSource();
        }
        private void GetSource()
        {
            //#region 取第一页的数据
            pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>()
            };
            this.InitPageEntity(MethodIDStore.GetModelUserOp, this.StaffGrid, this.busy, this.RadPager, pageParam);
        }
        #region 新增职位
        private void BtEntry_Click(object sender, RoutedEventArgs e)
        {
            API.Sys_UserOp UserOp = new API.Sys_UserOp() { Name = TbName.Text.Trim(), Note = TbNote.Text.Trim() };
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定新增？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodIDStore.AddUserOp, new object[] { UserOp }, Sumbit_Completed);
        }
        protected void Sumbit_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                    GetNext();
                TbName.Text = string.Empty;
                TbNote.Text = string.Empty;
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion

        private void AddGrid_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.Sys_UserOp> UserOpList = StaffGrid.ItemsSource as List<API.Sys_UserOp>;
            List<string> NameList = (from c in Store.UserOp
                                     select c.Name).ToList();
            List<string> NoteList = (from c in Store.UserOp
                                     select c.Note).ToList();
            var query = (from b in UserOpList
                         where !NameList.Contains(b.Name)||!NoteList.Contains(b.Note)
                         select b).Distinct().ToList();
            if (query.Count() == 0) return;
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定修改？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodIDStore.UpdateUserOp, new object[] { query }, Sumbit_Completed);
        }

        private void CleanAll_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            GetSource();
            TbName.Text = string.Empty;
            TbNote.Text = string.Empty;
        }
        #region 下一页事件
        /// <summary>
        /// 点击下一页时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取下一页的数据
            pageParam.PageIndex = e.NewPageIndex;
            GetNext();
            #endregion
        }
        private void GetNext()
        {
            this.InitPageEntity(MethodIDStore.GetModelUserOp, this.StaffGrid, this.busy, this.RadPager, pageParam);
        }
        #endregion
        private void DelGrid_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.Sys_UserOp> UserOpList = new List<API.Sys_UserOp>();
            foreach (var Item in StaffGrid.SelectedItems)
            {
                UserOpList.Add(Item as API.Sys_UserOp);
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodIDStore.DelUserOp, new object[] { UserOpList }, Sumbit_Completed);
        }
    }
}
