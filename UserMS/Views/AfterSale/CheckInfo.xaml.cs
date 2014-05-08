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
    /// QualityInspection.xaml 的交互逻辑
    /// </summary> 
    public partial class CheckInfo : Page
    {
        public CheckInfo()
        {
            InitializeComponent();
            searchGrid.ItemsSource = models;
            //hadder = new HallFilter(false, ref hall);
            //List<API.Pro_HallInfo> halls = hadder.FilterHall(menuid, Store.ProHallInfo);
            //if (halls.Count != 0)
            //{
            //    this.hall.Tag = halls.First().HallID;
            //    this.hall.TextBox.SearchText = halls.First().HallName;
            //}
            //this.hall.SearchButton.Click += SearchButton_Click;
            hadder = new ROHallAdder(ref this.hall, menuid);
            flag = true;
            radDataPager1.PageSize = 20;
            Search();
        }

        private ROHallAdder hadder = null;
        private bool flag = false;
        private int pageIndex = 0;

        private List<API.View_ASPZJInfo> models = new List<API.View_ASPZJInfo>();
        private int menuid = 327;

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
          //  hadder.GetHall(hadder.FilterHall(menuid, Store.ProHallInfo));
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

            //API.ReportSqlParams_Bool pass = new API.ReportSqlParams_Bool();
            //pass.ParamName = "ZJPassed";
            //pass.ParamValues =true;
            //rpp.ParamList.Add(pass);

            //API.ReportSqlParams_Bool IsAudit = new API.ReportSqlParams_Bool();
            //IsAudit.ParamName = "IsAudit";
            //IsAudit.ParamValues = true;
            //rpp.ParamList.Add(IsAudit);  //IsAudit

            ////审核未通过
            //API.ReportSqlParams_Bool IsPassed = new API.ReportSqlParams_Bool();
            //IsPassed.ParamName = "IsPassed";
            //IsPassed.ParamValues = false;
            //rpp.ParamList.Add(IsPassed);

            //API.ReportSqlParams_Bool Finished = new API.ReportSqlParams_Bool();
            //Finished.ParamName = "Finished";
            //Finished.ParamValues = false;
            //rpp.ParamList.Add(Finished);

            if (state.SelectedIndex != 0)
            {
                API.ReportSqlParams_String repstate = new API.ReportSqlParams_String();
                repstate.ParamName = "RpState";
                object obj = (state.SelectedItem as ComboBoxItem).Content;
                repstate.ParamValues = obj == null ? "" : obj.ToString();
                rpp.ParamList.Add(repstate);
            }

            if (!string.IsNullOrEmpty(this.hall.Tag+""))
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
            //325
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 357, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        private void Clear()
        {
            serviceHall.Text = string.Empty;
            serviceHallID.Text = string.Empty;
            repairCount.Text = string.Empty;
            oldID.Text = string.Empty;
            repairer.Text = string.Empty;
            proHall.Text = string.Empty;
            chkInOut.Text = string.Empty;
            repairNote.Text = string.Empty;
            chkNote.Text = string.Empty;
            bj_money.Text = string.Empty;
            bjMoney.Text = string.Empty;
            bjHall.Text = string.Empty;
            bj_user.Text = string.Empty;
            bjHall.Text = string.Empty;
            bj_date.Text = string.Empty;
            proMoney.Text = string.Empty;
            total.Text = string.Empty;
            shouldPay.Text = string.Empty;
            realPay.Text = string.Empty;
            workMoney.Text = string.Empty;

            bjGrid.ItemsSource = null;
            bjGrid.Rebind();
            prosGrid.ItemsSource = null;
            prosGrid.Rebind();
            oldErrGrid.ItemsSource = null;
            newErrGrid.ItemsSource = null;
            oldErrGrid.Rebind();
            newErrGrid.Rebind();
            models.Clear();
            searchGrid.Rebind();
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
                List<API.View_ASPZJInfo> list = pageParam.Obj as List<API.View_ASPZJInfo>;
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

            API.View_ASPZJInfo model = searchGrid.SelectedItem as API.View_ASPZJInfo;

           // repairCount.Text = model.repa
            proHall.Text = model.RepairHallName;
            this.chkInOut.Text = model.Chk_InOut;
            this.repairer.Text = model.Repairer;
            oldID.Text = model.OldID;
            serviceHall.Text = model.RecHallName;
            serviceHallID.Text = model.HallID;
            repairNote.Text = model.RepairNote;
            bj_money.Text = model.BJ_Money.ToString();
            bj_date.Text = model.BJDate == null ? "" : model.BJDate.ToString();
            bj_user.Text = model.BJUser??"";
            bjHall.Text = model.BJHallName;
            //gzMoney.Text = model.GzMoney==null?"":model.GzMoney.ToString();
            //gzType.Text = model.GzType??"";
          
            bjMoney.Text = model.BJ_Money.ToString();
            workMoney.Text = model.WorkMoney.ToString();
            proMoney.Text = model.ProMoney.ToString();
            decimal tot = (decimal)(model.WorkMoney + model.ProMoney - model.BJ_Money);
            total.Text = tot.ToString();

            shouldPay.Text = (model.WorkMoney + model.ProMoney).ToString();
            realPay.Text = (tot - Convert.ToDecimal(model.GzMoney)).ToString();
            repairCount.Text = model.RepairCount.ToString();  

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
                bjGrid.ItemsSource = bjlist;
                bjGrid.Rebind();

                //e.Result.ArrList[2]  受理故障
                List<API.ASP_ErrorInfo> list2 = e.Result.ArrList[2] as List<API.ASP_ErrorInfo>;
                oldErrGrid.ItemsSource = list2;
                oldErrGrid.Rebind();
            }

        }

        #endregion
     
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void auditPass_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Audit(true);
        }

        private void Audit( bool pass)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择数据！");
                return;
            }

            List<API.View_ASPZJInfo> list = new List<API.View_ASPZJInfo>();
            foreach (var item in searchGrid.SelectedItems)
            {
                API.View_ASPZJInfo m = item as API.View_ASPZJInfo;
                if (m.RpState != "待审核")
                {
                    MessageBox.Show("单号 " + m.OldID + "处于" + m.RpState + "状态，无法审核！");
                    return;
                }
                m.IsPassed = pass;
                list.Add(m);
            }

            if (MessageBox.Show("确定审核吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 333, new object[] { list },
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                chkNote.Text = string.Empty;
                Clear();
                Search();
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }

        }

        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void auditUnPass_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Audit(false);
        }
    }
}
