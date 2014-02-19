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
using UserMS.Common;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// BJReturnAudit.xaml 的交互逻辑
    /// </summary>
    public partial class BJReturnAudit : Page
    {
        public BJReturnAudit()
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
            Search();
        }

        private ROHallAdder hadder = null;
        int pageIndex = 0;
        bool flag = false;
        private int menuid = 326;
        private List<API.View_ASPGetPhoneInfo> models = new List<API.View_ASPGetPhoneInfo>();

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


            //IsFetchAduitStr FetchAuditPassedStr HasPassed
            rpp.ParamList = new List<API.ReportSqlParams>();

            if ((HasAudited.SelectedItem as ComboBoxItem).Content.ToString() != "全部")
            {
                API.ReportSqlParams_Bool hasAudited = new API.ReportSqlParams_Bool();
                hasAudited.ParamName = "IsFetchAduit";
                hasAudited.ParamValues = (HasAudited.SelectedItem as ComboBoxItem).Content.ToString()=="是"?true:false;
                rpp.ParamList.Add(hasAudited);
            }
            if ((HasPassed.SelectedItem as ComboBoxItem).Content.ToString() != "全部")
            {
                API.ReportSqlParams_Bool hasPassed = new API.ReportSqlParams_Bool();
                hasPassed.ParamName = "FetchAuditPassed";
                hasPassed.ParamValues = (HasPassed.SelectedItem as ComboBoxItem).Content.ToString() == "是" ? true : false;
                rpp.ParamList.Add(hasPassed);
            }

            API.ReportSqlParams_Bool HasFetch = new API.ReportSqlParams_Bool();
            HasFetch.ParamName = "HasFetch";
            HasFetch.ParamValues = true;
            rpp.ParamList.Add(HasFetch);

            API.ReportSqlParams_Bool IsToFact = new API.ReportSqlParams_Bool();
            IsToFact.ParamName = "FetchNeedAudit";  
            IsToFact.ParamValues = true;
            rpp.ParamList.Add(IsToFact);

            API.ReportSqlParams_Bool IsAudit = new API.ReportSqlParams_Bool();
            IsAudit.ParamName = "IsAudit";
            IsAudit.ParamValues = true;
            rpp.ParamList.Add(IsAudit);


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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 359, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        private void Clear()
        {
            serviceHall.Text = string.Empty;
            repairCount.Text = string.Empty;
            oldID.Text = string.Empty;
            receiver.Text = string.Empty;
            bjMoney.Text = string.Empty;
            bjDate.Text = string.Empty;
            fetchNote.Text = string.Empty;
            fetchAuditNote.Text = string.Empty;
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

                List<API.View_ASPGetPhoneInfo> list = pageParam.Obj as List<API.View_ASPGetPhoneInfo>;
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

            API.View_ASPGetPhoneInfo model = searchGrid.SelectedItem as API.View_ASPGetPhoneInfo;
            serviceHall.Text = model.RecHallName;
            oldID.Text = model.OldID;
          //repairCount.Text = ;
            receiver.Text = model.Receiver;
            bjMoney.Text =model.BJ_Money.ToString();
            bjDate.Text = model.BJDate ;
            fetchNote.Text = model.FetchNote;
            PublicRequestHelp peh = new PublicRequestHelp(this.isbusy, 337, new object[] { model.ID },
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                List<API.View_BJModels> pros = e.Result.Obj as List<API.View_BJModels>;

                prosGrid.ItemsSource = pros;
                prosGrid.Rebind();
            }

        }

        #endregion

        #region   审批

        private void audit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需审批的数据！");
                return;
            }

            API.View_ASPGetPhoneInfo model = searchGrid.SelectedItem as API.View_ASPGetPhoneInfo;
            model.FetchAuditNote = fetchAuditNote.Text.Trim();
            model.FetchAuditPassed = true;
           
            if (MessageBox.Show("确定审批吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,338,new object[]{model},
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                fetchAuditNote.Text = string.Empty;
                MessageBox.Show(e.Result.Message);
                Search();
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }

        }

        private void Unaudit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需审批的数据！");
                return;
            }

            API.View_ASPGetPhoneInfo model = searchGrid.SelectedItem as API.View_ASPGetPhoneInfo;
            model.FetchAuditNote = fetchAuditNote.Text.Trim();
            model.FetchAuditPassed = false;
            if (MessageBox.Show("确定审批吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 338, new object[] { model },
             new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        #endregion 

    }
}
