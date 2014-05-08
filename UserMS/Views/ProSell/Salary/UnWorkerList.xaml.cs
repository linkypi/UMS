using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Salary
{
    /// <summary>
    /// 员工入职   menuid = 114
    /// </summary>
    public partial class UnWorkerList : BasePage
    {
        const int AddMethodID = 156;
        const int GetRole = 167;
        List<API.Sys_UserInfo> UserInfo_List;
        private ROHallAdder hadder;

        List<TreeViewModel> deptinfo = new List<TreeViewModel>();

        public UnWorkerList()
        {
            InitializeComponent();
            StaffDepart.btnSearch.Click += btnSearch_Click;
            StaffPosition.btnSearch.Click += OPSearch_Click;
            EnterTime.SelectedValue = DateTime.Now;
            hadder = new ROHallAdder(ref Hall, 114);
            InitGrid2();
            GetRoleSourece();
            GetLeftTree(Store.DeptInfo);
            //获取提醒列表
            PublicRequestHelp prh = new PublicRequestHelp(null, 261, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetRemindCompleted));
        }

        #region 获取提醒列表

        private void GetRemindCompleted(object sender, API.MainCompletedEventArgs e)
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
                List<API.Sys_RemindList> arr = e.Result.Obj as List<API.Sys_RemindList>;
                if (arr == null) { return; }

                InitRemindMenu(arr, new List<int>());
            }
        }


        /// <summary>
        /// 初始化提醒菜单
        /// </summary>
        /// <param name="remindList">所有提醒</param>
        /// <param name="userRemindids">用户定制的菜单编号集合</param>
        private void InitRemindMenu(List<API.Sys_RemindList> remindList, List<int> userRemindids)
        {
            var menu = from a in Store.MenuInfos
                       where a.Parent == 0
                       select a;
            foreach (var item in menu)
            {
                RadTreeViewItem p = Find(item, remindList, userRemindids);
                if (p != null)
                {
                    p.Header = item.MenuText;
                    p.DataContext = item;
                    p.Tag = item.MenuID;
                    RadTreeView1.Items.Add(p);
                }
            }
        }

        private RadTreeViewItem Find(API.Sys_MenuInfo child, List<API.Sys_RemindList> remindList, List<int> userRemindids)
        {
            var mm = from a in Store.MenuInfos
                     where a.Parent == child.MenuID
                     select a;
            //若该菜单为叶子节点   则判断菜单是否是有提醒
            if (mm.Count() == 0)
            {

                var remind = from a in remindList
                             where a.MenuID == child.MenuID
                             select a;
                if (remind.Count() > 0)
                {
                    //该叶子节点有提醒则添加提醒   注意每个菜单可有多个提醒  所有需添加一个菜单父节点
                    RadTreeViewItem p = new RadTreeViewItem();
                    p.Header = child.MenuText;
                    p.DataContext = child;

                    int Count = 0;
                    foreach (var item in remind)
                    {
                        RadTreeViewItem rad = new RadTreeViewItem();
                        rad.Header = item.Name;
                        rad.DataContext = item;
                        rad.Tag = item.ID;
                        if (userRemindids.Contains((int)item.ID))
                        {
                            rad.IsChecked = true;
                            Count++;
                            rad.CheckState = System.Windows.Automation.ToggleState.On;
                        }
                        else
                        {
                            rad.IsChecked = false;
                            rad.CheckState = System.Windows.Automation.ToggleState.Off;
                        }
                        p.Items.Add(rad);
                    }

                    if (Count == 0)
                    {
                        p.IsChecked = false;
                        p.CheckState = System.Windows.Automation.ToggleState.Off;
                    }
                    else if (Count == p.Items.Count)
                    {
                        p.IsChecked = true;
                        p.CheckState = System.Windows.Automation.ToggleState.On;
                    }
                    else
                    {
                        p.CheckState = System.Windows.Automation.ToggleState.Indeterminate;
                    }

                    if (p.Items.Count == 0)
                    {
                        return null;
                    }
                    return p;
                }
                else
                {
                    return null;
                }
            }

            RadTreeViewItem parent = new RadTreeViewItem();


            //若该菜单不是叶子节点   则继续循环查找
            foreach (var item in mm)
            {
                RadTreeViewItem obj = Find(item, remindList, userRemindids);

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

        #endregion

        #region 角色资源
        void GetRoleSourece()
        {
            PublicRequestHelp helper = new PublicRequestHelp(busy, GetRole, new object[] { }, GetRole_Completed);

        }
        protected void GetRole_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    CbRole.ItemsSource = e.Result.Obj as List<API.Sys_RoleInfo>;
                    CbRole.SelectedValuePath = "RoleID";
                    CbRole.DisplayMemberPath = "RoleName";
                }
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion

        #region 生成列
        private void InitGrid2()
        {
            //优惠表头GridView
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("RealName");

            col.Header = "员工姓名";
            this.StaffGrid.Columns.Add(col);


            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("Name");
            col2.Header = "职位";
            this.StaffGrid.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("DtpName");
            col3.Header = "部门";
            this.StaffGrid.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("SysTime");
            col4.Header = "入职时间";
            this.StaffGrid.Columns.Add(col4);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("RoleName");
            col5.Header = "角色";
            this.StaffGrid.Columns.Add(col5);

            GridViewDataColumn col6 = new GridViewDataColumn();
            col6.DataMemberBinding = new System.Windows.Data.Binding("UserName");
            col6.Header = "帐号";
            this.StaffGrid.Columns.Add(col6);

            GridViewDataColumn col8 = new GridViewDataColumn();
            col8.DataMemberBinding = new System.Windows.Data.Binding("CancelLimit");
            col8.Header = "取消时间限制";
            this.StaffGrid.Columns.Add(col8);


            GridViewDataColumn col9 = new GridViewDataColumn();
            col9.DataMemberBinding = new System.Windows.Data.Binding("AduitLimit");
            col9.Header = "申请时间限制";
            this.StaffGrid.Columns.Add(col9);
        }
        #endregion

        #region 获取部门信息

        /// <summary>
        /// 获取父级节点
        /// </summary>
        /// <param name="Dept"></param>
        private void GetLeftTree(List<API.Sys_DeptInfo> Dept)
        {
            var ParentInfo = Dept.Where(p => p.Parent == 0);
            foreach (var item in ParentInfo)
            {
                deptinfo.Add(GetChildItem(item.DtpID));
            }
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        private TreeViewModel GetChildItem(int DeptID)
        {
            var Children = from b in Store.DeptInfo
                           where b.Parent == DeptID
                           select b;
            var pnode = from b in Store.DeptInfo
                        where b.DtpID == DeptID
                        select b; //pnode[1]
            TreeViewModel p2 = new TreeViewModel();
            p2.Fields = new string[] { "ClassName", "Pro_ClassID" };
            p2.Values = new object[] { pnode.First().DtpName, pnode.First().DtpID };
            p2.ID = pnode.First().DtpID.ToString();
            p2.Title = pnode.First().DtpName;
            if (Children.Count() != 0)
            {
                foreach (var Item in Children)
                {
                    TreeViewModel child = GetChildItem(Item.DtpID);
                    if (p2.Children == null)
                    {
                        p2.Children = new List<TreeViewModel>();
                    }
                    p2.Children.Add(child);

                }
            }
            return p2;

        }

        void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MultSelecter msFrm = new MultSelecter(
             deptinfo,
             Store.DeptInfo,
            "DtpID", "DtpName",
             new string[] { "DtpID", "DtpName" },
             new string[] { "ID", "部门" },
             true
            );
            msFrm.Closed += VIPTypeWin_Closed;
            msFrm.ShowDialog();
        }
        private void VIPTypeWin_Closed(object sender, EventArgs e)
        {
            UserMS.MultSelecter selecter = sender as UserMS.MultSelecter;
            if (selecter.DialogResult == true)
            {
                List<UserMS.API.Sys_DeptInfo> phList = selecter.SelectedItems.OfType<API.Sys_DeptInfo>().ToList();
                if (phList.Count() == 0)
                {
                    StaffDepart.Text = string.Empty;
                    StaffDepart.Tag = null;
                    return;
                }
                StaffDepart.Text = phList[0].DtpName;
                StaffDepart.Tag = phList[0].DtpID;
            }
        }
        #endregion

        #region 获取职位信息
        void OPSearch_Click(object sender, RoutedEventArgs e)
        {
            MultSelecter msFrm = new MultSelecter(
             null,
             Store.UserOp,
            "OpID", "Name",
             new string[] { "OpID", "Name" },
             new string[] { "ID", "职位" },
             true
            );
            msFrm.Closed += PositionWin_Closed;
            msFrm.ShowDialog();

        }
        private void PositionWin_Closed(object sender, EventArgs e)
        {
            UserMS.MultSelecter selecter = sender as UserMS.MultSelecter;
            if (selecter.DialogResult == true)
            {
                List<UserMS.API.Sys_UserOp> phList = selecter.SelectedItems.OfType<API.Sys_UserOp>().ToList();
                if (phList.Count() == 0)
                {
                    StaffPosition.Text = string.Empty;
                    StaffPosition.Tag = null;
                    return;
                }
                StaffPosition.Text = phList[0].Name;
                StaffPosition.Tag = phList[0].OpID;
            }
        }
        #endregion


   
        void AddGrid1()
        {
            List<API.View_Sys_UserInfo> UserList = StaffGrid.ItemsSource as List<API.View_Sys_UserInfo>;
            UserList = UserList == null ? new List<API.View_Sys_UserInfo>() : UserList;
            API.View_Sys_UserInfo UserInfo = new API.View_Sys_UserInfo();
            if (UserList.Where(p => p.RealName == StaffName.Text.Trim()).Count() > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"员工姓名重复！");
                return;
            }
            #region 后面添加的数据
            if (CbRole.SelectedValue != null)
            {
                UserInfo.RoleID = (int)CbRole.SelectedValue;
            }
            if (CbCanLogin.Text == "可登录")
                UserInfo.CanLogin = true;
            else
                UserInfo.CanLogin = false;

            UserInfo.UserName = TbIDName.Text.Trim();
            //UserInfo.UserPwd = TbPassWord.Password.Trim();
            try
            {
                UserInfo.CancelLimit = decimal.Parse(TbCancelTime.Text.Trim());

            }
            catch { }
            try
            {
                UserInfo.AduitLimit = decimal.Parse(TbAskTime.Text.Trim());
            }
            catch { }
            #endregion
            UserInfo.RoleName = CbRole.Text;
            UserInfo.RealName = StaffName.Text.Trim();
            try
            {
                UserInfo.DtpID = Convert.ToInt32(StaffDepart.Tag);
            }
            catch
            {

            }
            UserInfo.DtpName = StaffDepart.Text.Trim();
            UserInfo.SysDate = EnterTime.SelectedValue;
            UserInfo.SysTime = EnterTime.SelectedValue.Value.Date.ToString();
            try
            {
                UserInfo.OpID = (int)StaffPosition.Tag;
            }
            catch { }
            UserInfo.Name = StaffPosition.Text.Trim();
            UserList.Add(UserInfo);
            StaffGrid.ItemsSource = UserList;
            StaffGrid.Rebind();
            PartCancel();
        }
        #region 提交数据
        private void BtEntry_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            #region 判断数据正确性
            if (string.IsNullOrEmpty(StaffName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入姓名！");
                return;
            }

            //if (string.IsNullOrEmpty(StaffPosition.Text))
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写职位！");
            //    return;
            //}
            //if (string.IsNullOrEmpty(StaffDepart.Text))
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择部门！");
            //    return;
            //}
            if (EnterTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"填写入职时间！");
                return;
            }

            #endregion
            UserInfo_List = new List<API.Sys_UserInfo>();
            API.Sys_UserInfo User = new API.Sys_UserInfo();
            #region 后面添加的数据
            if (CbRole.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择角色！");
                return;
            }
            try
            {
                User.RoleID = (int)CbRole.SelectedValue;
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择正确的角色！");
                return;
            }
            if (CbCanLogin.SelectedIndex == 0)
            {
                User.CanLogin = true;
                if (string.IsNullOrEmpty(TbIDName.Text) || string.IsNullOrEmpty(TbPassWord.Password) || string.IsNullOrEmpty(TbAgainWord.Password))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"可登陆状态需输入帐号和密码！");
                    return;
                }
            }
            else
                User.CanLogin = false;
            if (!string.IsNullOrEmpty(TbIDName.Text))
            {
                if (string.IsNullOrEmpty(TbPassWord.Password))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"输入帐号后需输入密码！");
                    return;
                }

                if (string.IsNullOrEmpty(TbAgainWord.Password))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请再次输入密码！");
                    return;
                }
                if (TbPassWord.Password != TbAgainWord.Password)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"两次输入的密码不一致！");
                    return;
                }
                User.UserName = TbIDName.Text.Trim();
                User.UserPwd = TbPassWord.Password.Trim();

            }
            if (!string.IsNullOrEmpty(TbCancelTime.Text))
            {
                try
                {
                    User.CancelLimit = decimal.Parse(TbCancelTime.Text.Trim());
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入整数！");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(TbAskTime.Text))
            {
                try
                {
                    User.AduitLimit = decimal.Parse(TbAskTime.Text.Trim());
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入整数！");
                    return;
                }
            }
            if (StaffDepart.Tag != null)
            {
                User.DtpID = Convert.ToInt32(StaffDepart.Tag);
            }
            User.RealName = StaffName.Text.Trim();
            User.SysDate = DateTime.Now;
            User.IsDefault = this.CbIsDefault.Text == "是" ? true : false;
            User.IsBoss = this.IsBoss.IsChecked == true ? true : false;
            User.AuditOffPrice = Convert.ToDecimal(this.BossValue.Value);
            User.BorowAduitPrice = Convert.ToDecimal(BorowValue.Value);

            User.Sys_UserOPList = new List<API.Sys_UserOPList>();
            API.Sys_UserOPList UserOP = new API.Sys_UserOPList();
            UserOP.CreateDate = EnterTime.SelectedValue;
            if(StaffPosition.Tag!=null)
            {
                UserOP.OpID = (int)StaffPosition.Tag;
            }
            if (Hall.Tag != null)
            {
                UserOP.HallID = Hall.Tag.ToString();
            }
            UserOP.UpdUserID = Store.LoginUserInfo.UserID;
            UserOP.Flag = true;
            User.Sys_UserOPList.Add(UserOP);
            //UserInfo_List.Add(User);
            #endregion


            #region  获取用户定制的提醒
            List<int> remindids = new List<int>();
            foreach (var item in RadTreeView1.Items)
            {
                RadTreeViewItem xxd = item as RadTreeViewItem;
                if (xxd.CheckState == System.Windows.Automation.ToggleState.Off)
                {
                    continue;
                }
                FindChid(xxd, remindids);
            }

            #endregion 
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定入职？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;                                                                                       //, remindids 
            PublicRequestHelp helper = new PublicRequestHelp(busy, AddMethodID, new object[] { User, remindids }, MyClient_Completed);
        }

        /// <summary>
        /// 递归 获取用户定制的提醒
        /// </summary>
        /// <param name="xxd"></param>
        /// <param name="remindids"></param>
        private void FindChid(RadTreeViewItem xxd, List<int> remindids)
        {
            if (xxd.Items.Count == 0 && xxd.CheckState == System.Windows.Automation.ToggleState.On)
            {
                remindids.Add((int)(xxd.DataContext as API.Sys_RemindList).ID);
                return;
            }
            foreach (var item in xxd.Items)
            {
                RadTreeViewItem child = item as RadTreeViewItem;
                if (child.CheckState == System.Windows.Automation.ToggleState.Off)
                {
                    continue;
                }
                FindChid(child, remindids);
            }
        }

        #endregion


        #region 提交完成
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    AddGrid1();
                }

                Logger.Log(e.Result.Message + "");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion
        #region 清空数据
        void PartCancel()
        {
            StaffDepart.Text = string.Empty;
            StaffName.Text = string.Empty;
            StaffPosition.Text = string.Empty;
            EnterTime.SelectedValue = DateTime.Now;

            TbAskTime.Text = string.Empty;
            TbCancelTime.Text = string.Empty;
            TbIDName.Text = string.Empty;
            TbPassWord.Password = string.Empty;
            TbAgainWord.Password = string.Empty;
            CbCanLogin.SelectedIndex=1;
            CbRole.Text = string.Empty;
        }
        void AllCance()
        {
            StaffDepart.Text = string.Empty;
            StaffName.Text = string.Empty;
            StaffPosition.Text = string.Empty;
            EnterTime.SelectedValue = DateTime.Now;
            StaffGrid.ItemsSource = null;
            StaffGrid.Rebind();

            TbAskTime.Text = string.Empty;
            TbCancelTime.Text = string.Empty;
            TbIDName.Text = string.Empty;
            TbPassWord.Password = string.Empty;
            TbAgainWord.Password = string.Empty;
            CbCanLogin.Text = string.Empty;
            CbRole.Text = string.Empty;
        }
        #endregion

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

        private void reFreshRM_Click_1(object sender, RoutedEventArgs e)
        {
            //获取提醒列表
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 261, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetRemindCompleted));
        }


        private void IsBoss_Checked(object sender, RoutedEventArgs e)
        {
            BossValue.Value = 99999;
            BossValue.IsEnabled = false;
        }

        private void IsBoss_Unchecked(object sender, RoutedEventArgs e)
        {
            BossValue.Value = 0;
            BossValue.IsEnabled = true;
        }

    }
}
