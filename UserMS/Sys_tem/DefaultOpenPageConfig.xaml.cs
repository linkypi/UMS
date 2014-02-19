using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Sys_tem
{
    /// <summary>
    /// DefaultOpenPageConfig.xaml 的交互逻辑
    /// </summary>
    public partial class DefaultOpenPageConfig : Page
    {
        API.ReportPagingParam pageParam;
        List<API.View_Sys_UserInfo> models = new List<API.View_Sys_UserInfo>();
        private int pageindex;
        private List<int> curMenuIDS;
        private bool flag = false;

        public DefaultOpenPageConfig()
        {
            InitializeComponent();
            flag = true;
            curMenuIDS = new List<int>();
            StaffGrid.ItemsSource = models;
            InitDefaultMenu(new List<int>());
            this.SizeChanged += DefaultOpenPageConfig_SizeChanged;
        }

        void DefaultOpenPageConfig_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        #region  查询

        void SearchUser()
        {
            if (!flag)
            {
                return;
            }
            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.page.PageIndex;
            pageParam.PageSize = this.page.PageSize;
            pageParam.ParamList = new List<API.ReportSqlParams>();
            //按姓名查询
            if (!string.IsNullOrEmpty(this.StaffName.Text))
            {
                API.ReportSqlParams_String StaffName = new API.ReportSqlParams_String();
                StaffName.ParamName = "RealName";
                StaffName.ParamValues = this.StaffName.Text.Trim();
                pageParam.ParamList.Add(StaffName);
            }
            //在职状态查询
            if (this.StaffFlag.SelectedIndex == 0)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "Flag";
                Flag.ParamValues = "All";
                pageParam.ParamList.Add(Flag);
            }
            if (this.StaffFlag.SelectedIndex == 1)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "Flag";
                Flag.ParamValues = "Now";
                pageParam.ParamList.Add(Flag);
            }
            if (this.StaffFlag.SelectedIndex == 2)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "Flag";
                Flag.ParamValues = "Leave";
                pageParam.ParamList.Add(Flag);
            }

            //时间
            if (StaffStartTime.SelectedValue != null)
            {
                API.ReportSqlParams_DataTime CreateDate = new API.ReportSqlParams_DataTime();
                CreateDate.ParamName = "StartDate";
                CreateDate.ParamValues = this.StaffStartTime.SelectedValue.Value.Date;
                pageParam.ParamList.Add(CreateDate);
            }
            if (this.StaffEndTime.SelectedValue != null)
            {
                API.ReportSqlParams_DataTime StaffEndTime = new API.ReportSqlParams_DataTime();
                StaffEndTime.ParamName = "EndDate";
                StaffEndTime.ParamValues = this.StaffEndTime.SelectedValue.Value.Date;
                pageParam.ParamList.Add(StaffEndTime);
            }
            if (pageParam.ParamList.Count() > 0)
            {
                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 158, new object[] { pageParam }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
              //  this.InitPageEntity(MethodIDStore.GetStaffMethodID, this.StaffGrid, this.busy, this.RadPager, pageParam);
            }
        }

        /// <summary>
        /// 查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            this.busy.IsBusy = false;
            models.Clear();
            StaffGrid.Rebind();
          
            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_Sys_UserInfo> aduitList = pageParam.Obj as List<API.View_Sys_UserInfo>;
                if (aduitList.Count != 0)
                {
                    models.AddRange(aduitList);
                    StaffGrid.Rebind();

                    this.page.PageSize = (int)pagesize.Value;
                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);

                    this.page.PageIndexChanged -= page_PageIndexChanged;
                    this.page.Source = pcv;
                    this.page.PageIndexChanged += page_PageIndexChanged;
                    this.page.PageIndex = pageindex;
                }
            }
        }


        #endregion 

        #region  初始化

        /// <summary>
        /// 初始化用户默认菜单
        /// </summary>
        /// <param name="remindList">所有菜单</param>
        /// <param name="userRemindids">用户定制的默认打开菜单</param>
        private void InitDefaultMenu(List<int> userMenuID)
        {
            var menu = from a in Store.MenuInfos
                       where a.Parent == 0
                       select a;
            foreach (var item in menu)
            {
                RadTreeViewItem p = Find(item, userMenuID);
                if (p != null)
                {
                    p.Header = item.MenuText;
                    p.DataContext = item;
                    p.Tag = item.MenuID;
                    RadTreeView1.Items.Add(p);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        /// <param name="remindList"></param>
        /// <param name="userMenuIDs"></param>
        /// <returns></returns>
        private RadTreeViewItem Find(API.Sys_MenuInfo child,  List<int> userMenuIDs)
        {
            var mm = from a in Store.MenuInfos
                     where a.Parent == child.MenuID
                     select a;
            //若该菜单为叶子节点   则判断菜单是否是有提醒
            if (mm.Count() == 0)
            {

                var remind = from a in userMenuIDs
                             where a == child.MenuID
                             select a;
                RadTreeViewItem p = new RadTreeViewItem();
                p.Header = child.MenuText;
                p.DataContext = child;

                if (remind.Count() > 0)
                {
                    //该叶子节点有提醒则添加提醒   注意每个菜单可有多个提醒  所有需添加一个菜单父节点
                    p.IsChecked = true;
                    p.CheckState = System.Windows.Automation.ToggleState.On;
                }
                else
                {
                    p.IsChecked = false;
                    p.CheckState = System.Windows.Automation.ToggleState.Off;
                }
                return p;
            }

            RadTreeViewItem parent = new RadTreeViewItem();


            //若该菜单不是叶子节点   则继续循环查找
            foreach (var item in mm)
            {
                RadTreeViewItem obj = Find(item, userMenuIDs);

                if (obj != null)
                {
                    obj.Header = item.MenuText;
                    //obj.DataContext = item;
                    obj.Tag = item.MenuID;
                    parent.Items.Add(obj);
                }
            }

            //查找该父节点中的状态
            int checkedCount = 0;
            System.Windows.Automation.ToggleState state = System.Windows.Automation.ToggleState.Off;
            foreach (var item in parent.Items)
            {
                RadTreeViewItem xxd = item as RadTreeViewItem;
                if (xxd.CheckState == System.Windows.Automation.ToggleState.Indeterminate)
                {
                    state = System.Windows.Automation.ToggleState.Indeterminate;
                    break;
                }
                if (xxd.IsChecked == true)
                {
                    checkedCount++;
                }
            }

            if (state == System.Windows.Automation.ToggleState.Indeterminate)
            {
                parent.CheckState = System.Windows.Automation.ToggleState.Indeterminate;
            }
            else
            {
                if (checkedCount == 0)
                {
                    parent.IsChecked = false;
                    parent.CheckState = System.Windows.Automation.ToggleState.Off;
                }
                else if (checkedCount == parent.Items.Count)
                {
                    parent.IsChecked = true;
                    parent.CheckState = System.Windows.Automation.ToggleState.On;
                }
                else
                {
                    parent.CheckState = System.Windows.Automation.ToggleState.Indeterminate;
                }
            }
            if (parent.Items.Count == 0)
            {
                return null;
            }
            return parent;

        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAll_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in RadTreeView1.Items)
            {
                RadTreeViewItem rt = item as RadTreeViewItem;
                rt.IsChecked = true;
                rt.CheckState = System.Windows.Automation.ToggleState.On;
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnSelect(object sender, RoutedEventArgs e)
        {
            foreach (var item in RadTreeView1.Items)
            {
                RadTreeViewItem rt = item as RadTreeViewItem;
                rt.IsChecked = false;
                rt.CheckState = System.Windows.Automation.ToggleState.Off;
            }
        }

        #endregion 

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchUser();
        }

        private void page_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            SearchUser();
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            SearchUser();
        }

        private void StaffGrid_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            if (StaffGrid.SelectedItem == null)
            {
                return;
            }
            API.View_Sys_UserInfo item = StaffGrid.SelectedItem as API.View_Sys_UserInfo;
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 273, new object[] { item .UserID}, new EventHandler<API.MainCompletedEventArgs>(GetComplted));
        }

        private void GetComplted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            RadTreeView1.Items.Clear();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<int> list = e.Result.Obj as List<int>;
                curMenuIDS.Clear();
                if (list == null)
                {
                    return;
                }
                curMenuIDS.AddRange(list);
                InitDefaultMenu(list);
            }
        }

        #region 保存

        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //获取用户定制的默认打开页面
            List<int> menuids = new List<int>();
            foreach (var item in RadTreeView1.Items)
            {
                RadTreeViewItem xxd = item as RadTreeViewItem;
                if (xxd.CheckState == System.Windows.Automation.ToggleState.Off)
                {
                    continue;
                }
                FindChid(xxd, menuids);
            }

            //获取新增提醒
            var add = from a in menuids
                      where !curMenuIDS.Contains(a)
                      select a;
            var remind = from a in menuids
                         where !add.Contains(a)
                         select a;
            //获取已删除的数据
            var del = from a in curMenuIDS
                      where !remind.Contains(a)
                      select a;
            API.View_Sys_UserInfo model = StaffGrid.SelectedItem as API.View_Sys_UserInfo;
            if (model == null)
            {
                MessageBox.Show("请选择需定制的用户！");
                return;
            }
            if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.busy,274,new object[]{model.UserID,add.ToList(),del.ToList()},new EventHandler<API.MainCompletedEventArgs>(SaveComplted));
        }

        private void SaveComplted(object sender, API.MainCompletedEventArgs e)
        {
             this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            MessageBox.Show(e.Result.Message);

        }

        private void FindChid(RadTreeViewItem xxd, List<int> menuids)
        {
            if (xxd.Items.Count == 0 && xxd.CheckState == System.Windows.Automation.ToggleState.On)
            {
                menuids.Add((int)(xxd.DataContext as API.Sys_MenuInfo).MenuID);
                return;
            }
            foreach (var item in xxd.Items)
            {
                RadTreeViewItem child = item as RadTreeViewItem;
                if (child.CheckState == System.Windows.Automation.ToggleState.Off)
                {
                    continue;
                }
                FindChid(child, menuids);
            }
        }

        #endregion 

       
    }
}
