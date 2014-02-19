using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Salary
{
    public partial class WorderMS : BasePage
    {
        API.ReportPagingParam pageParam;
        const int GetRole = 167;
        string DtpID;
        int? OPID;
        bool HasPwd=true;
        private List<int> curRemindIDS = new List<int>();
        private bool fresh = false;
        private ROHallAdder hadder = null;
        List<TreeViewModel> deptinfo = new List<TreeViewModel>();

        public WorderMS()
        {
            InitializeComponent();
            InitGrid2();

            TbStaffName.IsReadOnly = true;
            EnterTime.IsEnabled = false;
            CbRole.IsEnabled = false;
            CbCanLogin.IsEnabled = false;
            TbIDName.IsReadOnly = true;
            TbPassWord.IsEnabled = false;
            TbAgainWord.IsEnabled = false;
            TbCancelTime.IsReadOnly = true;
            TbAskTime.IsReadOnly = true;

            hadder = new ROHallAdder(ref Hall,165);
        
            StaffDepart.btnSearch.Click += btnSearch_Click;
            TbStaffPosition.btnSearch.Click += OPSearch_Click;
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

                if (!fresh)
                {
                    curRemindIDS.Clear();
                }

                InitRemindMenu(arr, curRemindIDS);
            }

            fresh = false;
        }

        /// <summary>
        /// 初始化提醒菜单
        /// </summary>
        /// <param name="remindList">所有提醒</param>
        /// <param name="userRemindids">用户定制的菜单编号集合</param>
        private void InitRemindMenu( List<API.Sys_RemindList> remindList,List<int> userRemindids)
        {
            var menu = from a in Store.MenuInfos
                       where a.Parent == 0
                       select a;
            foreach (var item in menu)
            {
                RadTreeViewItem p = Find(item, remindList, userRemindids);
               if (p!= null)
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
                        rad.Tag =item.ID;
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
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务端出错！");
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
                    return;

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
                    return;

                TbStaffPosition.Text = phList[0].Name;
                TbStaffPosition.Tag = phList[0].OpID;
            }
        }
        #endregion

        #region 生成列
        private void InitGrid2()
        {
            //优惠表头GridView
            GridViewDataColumn col0 = new GridViewDataColumn();
            col0.DataMemberBinding = new System.Windows.Data.Binding("UserID");

            col0.Header = "员工编号";
            this.StaffGrid.Columns.Add(col0);

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

            GridViewDataColumn col42 = new GridViewDataColumn();
            col42.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            col42.Header = "门店";
            this.StaffGrid.Columns.Add(col42);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("SysTime");
            col4.Header = "入职时间";
            this.StaffGrid.Columns.Add(col4);


            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("LeaveTime");
            col41.Header = "离职时间";
            this.StaffGrid.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("Aduit");
            col5.Header = "状态";
            this.StaffGrid.Columns.Add(col5);
        }
        #endregion

        #region 查询
        private void Search_Click_1(object sender, RoutedEventArgs e)
        {
            SearchOff();
        }

        void SearchOff()
        {
            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.RadPager.PageIndex;
            pageParam.PageSize = this.RadPager.PageSize;
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
                this.InitPageEntity(MethodIDStore.GetStaffMethodID, this.StaffGrid, this.busy, this.RadPager, pageParam);
            }
        }
        #endregion

        private void StaffGrid_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            API.View_Sys_UserInfo StaffList = this.StaffGrid.SelectedItem as API.View_Sys_UserInfo;
            if (StaffList == null)
            {
                StaffContent.DataContext = null;
                StaffDepart.Text = string.Empty;
                TbStaffPosition.Text = string.Empty;
                CbRole.Text = string.Empty;
                CbCanLogin.Text = string.Empty;
                CbIsDefault.Text = string.Empty;
                return;
            }
            HasPwd = string.IsNullOrEmpty(StaffList.UserPwd) ? false : true; 
            StaffContent.DataContext = StaffList;
            StaffDepart.Text = StaffList.DtpName;
            Hall.Text = StaffList.HallName;
            Hall.Tag = StaffList.HallID;
            TbStaffPosition.Text = StaffList.Name;
            CbRole.Text = StaffList.RoleName;
            CbCanLogin.Text = StaffList.IsLogin;
            CbIsDefault.Text = StaffList.HasDefault;
            IsBoss.IsChecked = StaffList.IsBoss;
            BorowValue.Value = Convert.ToDouble(StaffList.BorowAduitPrice);
            if (IsBoss.IsChecked == true)
                BossValue.IsEditable = false;
            else 
                BossValue.IsEditable = true;
            if (StaffList.AuditOffPrice == null)
                BossValue.Value = 0;
            else
               BossValue.Value = (double)StaffList.AuditOffPrice;
            if (StaffList.Flag == true)
            {
                TbStaffName.IsReadOnly = false;
                //EnterTime.IsEnabled = true;
                CbRole.IsEnabled = true;
                CbCanLogin.IsEnabled = true;
                TbIDName.IsReadOnly = false;
                TbPassWord.IsEnabled = true;
                TbAgainWord.IsEnabled = true;
                TbCancelTime.IsReadOnly = false;
                TbAskTime.IsReadOnly = false;

                StaffDepart.btnSearch.IsEnabled = true;
                TbStaffPosition.btnSearch.IsEnabled = true;
                
                CbIsDefault.IsEnabled = true;
                IsBoss.IsEnabled = true;
                BossValue.IsEditable = true;

            }
            else
            {
                TbStaffName.IsReadOnly = true;
                //EnterTime.IsEnabled = false;
                CbRole.IsEnabled = false;
                CbCanLogin.IsEnabled = false;
                TbIDName.IsReadOnly = true;
                TbPassWord.IsEnabled = false;
                TbAgainWord.IsEnabled = false;
                TbCancelTime.IsReadOnly = true;
                TbAskTime.IsReadOnly = true;

                StaffDepart.btnSearch.IsEnabled = false;
                TbStaffPosition.btnSearch.IsEnabled = false;


                CbIsDefault.IsEnabled = false;
                IsBoss.IsEnabled = false;
                BossValue.IsEditable = false;
            }
           PublicRequestHelp prh = new PublicRequestHelp(this.busy, 262, new object[] { StaffList.UserID}, new EventHandler<API.MainCompletedEventArgs>(GetUserRemindCompleted));
        }

        private void GetUserRemindCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }

            if (e.Result.ReturnValue)
            {
                RadTreeView1.Items.Clear();
                List<int> userRemindids = e.Result.Obj as List<int>;
                curRemindIDS.Clear();
                curRemindIDS.AddRange(userRemindids);
                List<API.Sys_RemindList> list = e.Result.ArrList[0] as List<API.Sys_RemindList>;
                InitRemindMenu(list, userRemindids);

            }
        }

  

        #region 确定修改

        private void BtEnter_Click_1(object sender, RoutedEventArgs e)
        {
            API.View_Sys_UserInfo StaffList = this.StaffGrid.SelectedItem as API.View_Sys_UserInfo;
            API.Sys_UserInfo model = new API.Sys_UserInfo();

            #region  验证有效性

            if (string.IsNullOrEmpty(TbStaffName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "员工姓名不能为空！");
                return;
            }
            if (model == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未选择员工！");
                return;
            }
            model.UserID = StaffList.UserID;
            model.RealName = TbStaffName.Text.Trim();
            if (!string.IsNullOrEmpty(TbAskTime.Text))
            {
                try
                {
                    model.AduitLimit = int.Parse(TbAskTime.Text);
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "审批时间限制请输入正整数");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(TbCancelTime.Text))
            {
                try
                {
                    model.CancelLimit = int.Parse(TbCancelTime.Text);
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "取消申请限制请输入正整数");
                    return;
                }
            }

            #endregion

            model.CanLogin = CbCanLogin.Text == "可登录" ? true : false;
            model.IsBoss = this.IsBoss.IsChecked == true ? true : false;
            model.IsDefault = this.CbIsDefault.Text == "是" ? true : false;
            model.AuditOffPrice = (decimal)this.BossValue.Value;
            model.BorowAduitPrice = (decimal)BorowValue.Value;
            if (string.IsNullOrEmpty(TbIDName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入帐号！");
                return;
            }

            if (StaffDepart.Tag != null)
                model.DtpID = Convert.ToInt32(StaffDepart.Tag);
            if (CbRole.SelectedValue != null)
                model.RoleID = (int)CbRole.SelectedValue;
            model.UserName = TbIDName.Text.Trim();


            if (model.CanLogin == true && HasPwd == false && string.IsNullOrEmpty(TbPassWord.Password))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "原帐号无初始密码，可登陆状态下需添加初始密码！");
                return;
            }
            if (!string.IsNullOrEmpty(TbPassWord.Password))
            {
                if (TbPassWord.Password != TbAgainWord.Password)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "两次输入的密码不一致！");
                    return;
                }
                model.UserPwd = TbPassWord.Password.Trim();
            }          
            API.Sys_UserOPList OPList = new API.Sys_UserOPList();
            if (TbStaffPosition.Tag != null)
                OPList.OpID = (int)TbStaffPosition.Tag;
            if (Hall.Tag != null)
            {
                OPList.HallID = Hall.Tag.ToString();
            }
            if (model.Sys_UserOPList == null)
                model.Sys_UserOPList = new List<API.Sys_UserOPList>();
            model.Sys_UserOPList.Add(OPList);

            if (StaffList.Flag == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "离职状态，无法修改资料！");
                return;
            }

            //获取用户定制的提醒
            List<int> remindids = new List<int>();
            foreach (var item in RadTreeView1.Items)
            {
                RadTreeViewItem xxd = item as RadTreeViewItem;
                if (xxd.CheckState == System.Windows.Automation.ToggleState.Off)
                {
                    continue;
                }
                FindChid(xxd,remindids);
            }

            //获取新增提醒
            var add = from a in remindids
                      where !curRemindIDS.Contains(a)
                      select a;
            var remind = from a  in remindids
                         where !add.Contains(a)
                         select a;
            //获取已删除的数据
            var del = from a in curRemindIDS
                      where !remind.Contains(a)
                      select a;

            //ArrayList arr = new ArrayList();

            //arr.Add(add.ToList());
            //arr.Add(del.ToList());

            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定修改资料？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodIDStore.UpdateSatffMethodID, new object[] { model, add.ToList(),del.ToList() }, MyClient_Completed);
        }

        private void FindChid(RadTreeViewItem xxd, List<int> remindids)
        {
             if(xxd.Items.Count==0&& xxd.CheckState== System.Windows.Automation.ToggleState.On)
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


        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                Logger.Log(e.Result.Message + "");
                MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message + "");
                if (e.Result.ReturnValue == true)
                {
                    StaffContent.DataContext = null;
                    StaffDepart.Text = string.Empty;
                    TbStaffPosition.Text = string.Empty;
                    CbRole.Text = string.Empty;
                    CbCanLogin.Text = string.Empty;
                    TbPassWord.Password = string.Empty;
                    TbAgainWord.Password = string.Empty;
                    Hall.Text = "";
                    Hall.Tag = null;
                    this.InitPageEntity(MethodIDStore.GetStaffMethodID, this.StaffGrid, this.busy, this.RadPager, pageParam);
                }

            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务端出错！");

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
            pageParam.PageIndex = e.NewPageIndex;
            this.InitPageEntity(MethodIDStore.GetStaffMethodID, this.StaffGrid, this.busy, this.RadPager, pageParam);
            #endregion
        }

        #endregion

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

        /// <summary>
        /// 刷新提醒列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reFreshRM_Click_1(object sender, RoutedEventArgs e)
        {
            fresh = true;
            //获取提醒列表
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 261, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetRemindCompleted));
        }
      
      
    }
}
