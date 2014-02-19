using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Price
{
    public partial class WorkerList : BasePage
    {
        API.ReportPagingParam pageParam;
        const int MethodID = 158;
        const int AddMethodID = 270;
        string DtpID;
        int? OPID;
        //DateTime CreatTime;
        List<TreeViewModel> deptinfo = new List<TreeViewModel>();

        private ROHallAdder hadder  ;
        public WorkerList()
        {
            InitializeComponent();
            InitGrid2();

            TbCreatTime.IsEnabled = false;
            TbStaffName.IsReadOnly = true;
            Hall.btnSearch.IsEnabled = false;
            StaffDepart.btnSearch.IsEnabled = false;
            TbStaffPosition.btnSearch.IsEnabled = false;
            hadder = new ROHallAdder(ref Hall,152);

            StaffDepart.btnSearch.Click += btnSearch_Click;
            TbStaffPosition.btnSearch.Click += OPSearch_Click;

            GetLeftTree(Store.DeptInfo);
        }
  
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

            GridViewDataColumn col30 = new GridViewDataColumn();
            col30.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            col30.Header = "仓库";
            this.StaffGrid.Columns.Add(col30);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("DtpName");
            col3.Header = "部门";
            this.StaffGrid.Columns.Add(col3);

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
                this.InitPageEntity(MethodID, this.StaffGrid, this.busy, this.RadPager, pageParam);
            }
        }
        #endregion

        private void StaffGrid_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            API.View_Sys_UserInfo StaffList = this.StaffGrid.SelectedItem as API.View_Sys_UserInfo;
            if (StaffList == null)
            {
                StaffContent.DataContext = null;
                Hall.Text = string.Empty;
                Hall.Tag = null;
                StaffDepart.Text = string.Empty;
                StaffDepart.Tag = null;
                TbStaffPosition.Text = string.Empty;
                TbStaffPosition.Tag = null;
                return;
            }


            StaffContent.DataContext = StaffList;
            StaffDepart.Text = StaffList.DtpName;
            StaffDepart.Tag = StaffList.DtpID;
            TbStaffPosition.Text = StaffList.Name;
            TbStaffPosition.Tag = StaffList.OpID;
            Hall.Text = StaffList.HallName;
            Hall.Tag = StaffList.HallID;
            if (StaffList.Flag == true)
            {
                TbCreatTime.IsEnabled = false;
                TbStaffName.IsReadOnly = true;
                Hall.btnSearch.IsEnabled = false;
                StaffDepart.btnSearch.IsEnabled = false;
                TbStaffPosition.btnSearch.IsEnabled = false;
            }
            else
            {
                TbCreatTime.IsEnabled = true;
                TbStaffName.IsReadOnly = false;
                Hall.btnSearch.IsEnabled = true;
                StaffDepart.btnSearch.IsEnabled = true;
                TbStaffPosition.btnSearch.IsEnabled = true;
            }
        }

        #region 提交数据
        private void TbLeave_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.View_Sys_UserInfo StaffList = this.StaffGrid.SelectedItem as API.View_Sys_UserInfo;
            API.Sys_UserInfo model = new API.Sys_UserInfo();
            model.UserID = StaffList.UserID;
            model.Sys_UserOPList = new List<API.Sys_UserOPList>();
            API.Sys_UserOPList OPList = new API.Sys_UserOPList();
            if (StaffList.Flag == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "已离职，无法再次离职！");
                return;
            }
            OPList.ID = Convert.ToInt32(StaffList.ID);
            OPList.Flag = StaffList.Flag;
            model.Sys_UserOPList.Add(OPList);
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定离职？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(busy, AddMethodID, new object[] { model }, MyClient_Completed);
        }



        #endregion

        #region 重新入职
        private void BtEnter_Click_1(object sender, RoutedEventArgs e)
        {
            API.View_Sys_UserInfo StaffList = this.StaffGrid.SelectedItem as API.View_Sys_UserInfo;
            API.Sys_UserInfo model = new API.Sys_UserInfo();
            model.UserID = StaffList.UserID;
            if (StaffDepart.Tag!=null)
                model.DtpID = Convert.ToInt32(StaffDepart.Tag);
            

            model.Sys_UserOPList = new List<API.Sys_UserOPList>();
            API.Sys_UserOPList OPList = new API.Sys_UserOPList();
            if (StaffList.Flag == true)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "在职状态，无法重新入职！");
                return;
            }
            OPList.UserID = StaffList.UserID;
            OPList.Flag = StaffList.Flag;

            if (Hall.Tag != null)
                OPList.HallID = Hall.Tag.ToString();
            if(TbStaffPosition.Tag!=null)
                OPList.OpID = (int)TbStaffPosition.Tag;
           
            OPList.CreateDate = TbCreatTime.SelectedValue;
            model.Sys_UserOPList.Add(OPList);
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定重新入职？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(busy, AddMethodID, new object[] { model }, MyClient_Completed);
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
                    Hall.Text = string.Empty;
                    Hall.Tag = null;
                    StaffDepart.Text = string.Empty;
                    StaffDepart.Tag = null;
                    TbStaffPosition.Text = string.Empty;
                    TbStaffPosition.Tag = null;
                    this.InitPageEntity(MethodID, this.StaffGrid, this.busy, this.RadPager, pageParam);
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
            this.InitPageEntity(MethodID, this.StaffGrid, this.busy, this.RadPager, pageParam);
            #endregion
        }

        #endregion
    }
}
