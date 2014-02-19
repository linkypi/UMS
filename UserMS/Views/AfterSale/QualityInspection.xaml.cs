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
using Telerik.Windows.Controls.Primitives;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// QualityInspection.xaml 的交互逻辑
    /// </summary>
    public partial class QualityInspection : Page
    {

        private List<UserOpModel> UserOpList = new List<UserOpModel>();
        public QualityInspection()
        {
            InitializeComponent();

            //hadder = new HallFilter(false, ref hall);
            //List<API.Pro_HallInfo> halls = hadder.FilterHall(menuid, Store.ProHallInfo);
            //if (halls.Count != 0)
            //{
            //    this.hall.Tag = halls.First().HallID;
            //    this.hall.TextBox.SearchText = halls.First().HallName;
            //}
            //this.hall.SearchButton.Click += SearchButton_Click;
            hadder = new ROHallAdder(ref this.hall, menuid);
            searchGrid.ItemsSource = models;
            radDataPager1.PageSize = 20;
            flag = true;

            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
            UserOpList = userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
                          (list, info) => new { op = list, user = info })
             .Join(Store.UserOp, arg => arg.op.OpID, op => op.OpID, (a, t) => new UserOpModel()
             {
                 ID = a.op.ID,
                 HallID = a.op.HallID,
                 OpID = a.op.OpID,
                 UserID = a.op.UserID,
                 Username = a.user.RealName,
                 opname = t.Name
             }).ToList();
            var userinfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model => model.UserID,
                                              (info, model) => info).ToList();
            this.repairer.ItemsSource = userinfos;
            this.repairer.TextSearchPath = "RealName";
            this.repairer.SearchEvent = SellerSearchEvent;
            this.repairer.SelectionMode = AutoCompleteSelectionMode.Single;

            Search();
        }

        private ROHallAdder hadder = null;
        List<API.View_ASPRepairInfo> models = new List<API.View_ASPRepairInfo>();
        private bool flag = false;
        int pageIndex = 0;
        private int menuid = 325;

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           // hadder.GetHall(hadder.FilterHall(menuid, Store.ProHallInfo));
        }

        #region 查询

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Ckb_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                Search();
            }
        }

        private void Search()
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageSize = (int)pagesize.Value;
            rpp.PageIndex = radDataPager1.PageIndex;

            rpp.ParamList = new List<API.ReportSqlParams>();


            API.ReportSqlParams_Bool HasRepaired = new API.ReportSqlParams_Bool();
            HasRepaired.ParamName = "HasRepaired";
            HasRepaired.ParamValues = true;
            rpp.ParamList.Add(HasRepaired);

            API.ReportSqlParams_Bool NoToFactOrToFacIsBack = new API.ReportSqlParams_Bool();
            NoToFactOrToFacIsBack.ParamName = "NoToFactOrToFacIsBack"; ////送厂后已返厂 或者  无需送厂已维修的  （质检过滤此条件）
            NoToFactOrToFacIsBack.ParamValues = true;
            rpp.ParamList.Add(NoToFactOrToFacIsBack);

            API.ReportSqlParams_Bool pass = new API.ReportSqlParams_Bool();
            pass.ParamName = "ZJPassed";
            pass.ParamValues = false;
            rpp.ParamList.Add(pass);

            API.ReportSqlParams_Bool Finished = new API.ReportSqlParams_Bool();
            Finished.ParamName = "Finished";
            Finished.ParamValues = false;
            rpp.ParamList.Add(Finished);

            if (!string.IsNullOrEmpty(this.hall.Tag.ToString()))
            {
                API.ReportSqlParams_String hall = new API.ReportSqlParams_String();
                hall.ParamName = "HallID";
                hall.ParamValues = this.hall.Tag.ToString();
                rpp.ParamList.Add(hall);
            }

            //if (!string.IsNullOrEmpty(this.sysdate.DateTimeText??""))
            //{
            //    API.ReportSqlParams_DataTime date = new API.ReportSqlParams_DataTime();
            //    date.ParamName = "SysDate";
            //    date.ParamValues = this.sysdate.SelectedDate;
            //    rpp.ParamList.Add(date);
            //}

            if (!string.IsNullOrEmpty(this.oldid.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "OldID";
                users.ParamValues = this.oldid.Text.Trim();
                rpp.ParamList.Add(users);
            }

            if (!string.IsNullOrEmpty(this.pro_imei.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Pro_IMEI";
                bt.ParamValues = this.pro_imei.Text;
                rpp.ParamList.Add(bt);
            }
            if (!string.IsNullOrEmpty(this.vipimei.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "VIP_IMEI";
                bt.ParamValues = this.vipimei.Text;
                rpp.ParamList.Add(bt);
            }

            if (!string.IsNullOrEmpty(this.cus_name.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Cus_Name";
                bt.ParamValues = this.cus_name.Text;
                rpp.ParamList.Add(bt);
            }

            if (!string.IsNullOrEmpty(this.cus_phone.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Cus_Phone";
                bt.ParamValues = this.cus_phone.Text;
                rpp.ParamList.Add(bt);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 325, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        private void Clear()
        {
            zjNote.Text = string.Empty;
            serviceHall.Text = string.Empty;
            serviceHallID.Text = string.Empty;
            repairCount.Text = string.Empty;
            oldID.Text = string.Empty;
            repairer.Text = string.Empty;
            chkInOut.Text = string.Empty;
            proHall.Text = string.Empty;
            repairNote.Text = string.Empty;
            zjNote.Text = string.Empty;

            oldErrGrid.ItemsSource = null;
            newErrGrid.ItemsSource = null;
            oldErrGrid.Rebind();
            newErrGrid.Rebind();
            models.Clear();
            searchGrid.Rebind();

            prosGrid.ItemsSource = null;
            prosGrid.Rebind();
            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.radDataPager1.PageIndexChanged -= radDataPager1_PageIndexChanged;
            this.radDataPager1.Source = pcv1;
            this.radDataPager1.PageIndexChanged += radDataPager1_PageIndexChanged;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                if (pageParam == null) { return; }
                List<API.View_ASPRepairInfo> list = pageParam.Obj as List<API.View_ASPRepairInfo>;
                if (list == null) { return; }
                models.Clear();
                models.AddRange(list);
                searchGrid.Rebind();
                this.radDataPager1.PageSize = (int)pagesize.Value;
                string[] data = new string[pageParam.RecordCount];
                PagedCollectionView pcv = new PagedCollectionView(data);
                this.radDataPager1.PageIndexChanged -= radDataPager1_PageIndexChanged;
                this.radDataPager1.Source = pcv;
                this.radDataPager1.PageIndexChanged += radDataPager1_PageIndexChanged;
                this.radDataPager1.PageIndex = pageIndex;
            }
            else
            {
                models.Clear();
                searchGrid.Rebind();
            }

        }

        private void radDataPager1_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                pageIndex = e.NewPageIndex;
                Search();
            }
        }

        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                radDataPager1.PageSize = (int)pagesize.Value;
                Search();
            }
        }

        #endregion

        #region 详情

        private void RepairGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                return;
            }

            API.View_ASPRepairInfo model = searchGrid.SelectedItem as API.View_ASPRepairInfo;

            chk_price.Text = model.Chk_Price == null ? "" : model.Chk_Price.ToString();
            repairCount.Text = model.RepairCount.ToString();
            proHall.Text = model.RepairHallName;
            this.chkInOut.Text = model.Chk_InOut;
            this.repairer.TextBox.SearchText = model.Repairer;
            oldID.Text = model.OldID;
            serviceHall.Text = model.RecHallName;
            serviceHallID.Text = model.HallID;
            repairNote.Text = model.RepairNote;
            //repairCount .Text = model.

            PublicRequestHelp peh = new PublicRequestHelp(this.isbusy, 326, new object[] { model.ID },
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
          
            if (e.Result.ReturnValue)
            {
                List<API.ASP_ErrorInfo> list1 = e.Result.Obj as List<API.ASP_ErrorInfo>;
                newErrGrid.ItemsSource = list1;
                newErrGrid.Rebind();

                //配件
                List<API.View_ASPCurrentOrderPros> list = e.Result.ArrList[0] as List<API.View_ASPCurrentOrderPros>;
                prosGrid.ItemsSource = list;
                prosGrid.Rebind();

                //e.Result.ArrList[1]  备机
                List<API.View_BJModels> bjlist = e.Result.ArrList[1] as List<API.View_BJModels>;
                //bjGrid.ItemsSource = bjlist;
                //bjGrid.Rebind();

                //e.Result.ArrList[2]  受理故障
                List<API.ASP_ErrorInfo> list2 = e.Result.ArrList[2] as List<API.ASP_ErrorInfo>;
                oldErrGrid.ItemsSource = list2;
                oldErrGrid.Rebind();
            }

        }

        #endregion

        #region 指定维修师

        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
                                                  "Username", new string[] { "Username", "opname" },
                                                  new string[] { "用户名", "职位" });
            w.Closed += SellerSearchWindowClose;
            w.ShowDialog();
        }

        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    UserOpModel selected = (UserOpModel)window.SelectedItem;
                    repairer.Tag = selected.UserID;
                    this.repairer.TextBox.SearchText = selected.Username;

                }
            }

        }

        #endregion 

     
        /// <summary>
        /// 质检通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pass_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择数据！");
                return;
            }

            List<API.View_ASPRepairInfo> list = new List<API.View_ASPRepairInfo>();
            foreach (var item in searchGrid.SelectedItems)
            {
                API.View_ASPRepairInfo m = item as API.View_ASPRepairInfo;
                m.ZJNote = zjNote.Text;
                m.ZJPassed = true;
                list.Add(m);
            }

            if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,329,new object[]{list},
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                
                Clear();
                Search();
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }
        }

        /// <summary>
        /// 质检不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unPass_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择数据！");
                return;
            }
            #region  验证指定维修员

            List<API.Sys_UserInfo> users = repairer.ItemsSource as List<API.Sys_UserInfo>;

            var usr = from a in users
                      where a.UserName == repairer.TextBox.SearchText
                      select a;

            if (usr.Count() > 0)
            {
                repairer.SelectedItem = usr.First();
            }
            else
            {
                MessageBox.Show("维修师不存在！");
                return;
            }

            #endregion 

            API.Sys_UserInfo user = repairer.SelectedItem as API.Sys_UserInfo;
            if (user == null) { MessageBox.Show("请选择指定维修师！"); return; }

            List<API.View_ASPRepairInfo> list = new List<API.View_ASPRepairInfo>();
            foreach (var item in searchGrid.SelectedItems)
            {
                API.View_ASPRepairInfo m = item as API.View_ASPRepairInfo;
                m.ZJNote = zjNote.Text;
                m.ZJPassed = false;
                m.Repairer = user.UserID;
                list.Add(m);
            }


            if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 329, new object[] {list },
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

    }
}
