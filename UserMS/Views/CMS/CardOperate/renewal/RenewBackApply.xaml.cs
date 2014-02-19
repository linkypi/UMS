using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.CMS.CardOperate.renewal
{
    public partial class RenewBackApply : Page
    {
        private int pageindex ;
        private bool hasInit = false;
        private List<API.View_VIP_Renew> models = null;

        private HallFilter hall = null;
        private string menuid = "";

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "38";
            }
            this.pager.PageSize = (int)pagesize.Value;
            models = new List<API.View_VIP_Renew>();
            dataGridOffList.ItemsSource = models;
            //MyAutoTextBox hallname = new MyAutoTextBox();
            //hall = new HallFilter(false, ref hallname); //this.hallName
            hall = new HallFilter();
            List<API.Pro_HallInfo> halls = hall.FilterHall(38, Store.ProHallInfo);
            //  if (halls.Count!=0)
            //{
            //    this.hallName.Tag = halls[0].HallID;
            //    this.hallName.TextBox.SearchText = halls[0].HallName;
            //}

            this.fromdate.SelectedValue = DateTime.Now.Date;
            this.aduited.SelectedIndex = 1;
            this.seller.Text = Store.LoginUserInfo.UserName;
            this.seller.Tag = Store.LoginUserInfo.UserID;
            this.flag.Tag = false;

            money.IsEnabled = false;
            cashRenew.IsEnabled = false;
            renewPoint.IsEnabled = false;
            pointRenew.IsEnabled = false;

            var cash = from c in Store.Options
                       where c.ClassName == "CashRenew"
                       select c;

            this.cashPercent.IsReadOnly = true;
            this.cashPercent.Text = cash.First().Value.ToString() + " : " + cash.First().Value2.ToString();

            var pquery = from c in Store.Options
                         where c.ClassName == "PointRenew"
                         select c;
            this.pointPercent.IsReadOnly = true;
            this.pointPercent.Text = pquery.First().Value.ToString() + " : " + pquery.First().Value2.ToString();

            //this.hallName.SearchButton.Click += SearchButton_Click;
            dataGridOffList.SelectionChanged += dataGridOffList_SelectionChanged;
        }

        public RenewBackApply()
        {
            InitializeComponent();
            hasInit = true;
            this.SizeChanged += RenewBackApply_SizeChanged;
        }

        void RenewBackApply_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("pager") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        /// 换页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            Search();
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// 选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dataGridOffList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            API.View_VIP_Renew vp = dataGridOffList.SelectedItem as API.View_VIP_Renew;
            if (vp != null)
            {
                this.seller.Text = string.IsNullOrEmpty(vp.Seller) ? "" : vp.Seller;
                this.imei_s.Text = vp.IMEI;
                this.apply.Tag = vp.State;
                this.mobiphone.Text = vp.MobiPhone;
                this.cardid_s.Text = vp.IDCard;
                this.cardType.Text = vp.VIPType;
                this.memname.Text = vp.MemberName;
                this.memname.Tag = vp.VIPID;
                this.starttime.Text = vp.StartTime;
                this.point.Text = vp.CurrentPoint.ToString();
                this.validity.Text = vp.CurrentValidity.ToString();
                this.validity.Tag = vp.RenewID;
                this.currentTimeTo.SelectedValue = DateTime.Now.Date.AddDays((int)vp.CurrentValidity);
                this.flag.Tag = true;

                if (vp.RenewTypeName=="现金续期")
                {
                   money.Value=(double) vp.RenewMoney;
                   this.cashRenew.IsChecked = true;
                }
                else
                {
                    renewPoint.Value = (double)vp.Point;
                    pointRenew.IsChecked = true;
                }
            }
        }

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hall.GetHall(hall.FilterHall(38,Store.ProHallInfo));
        }

        #region "查询"

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            if (!hasInit) { return; }
            Clear(); 
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = pager.PageIndex;
            rpp.PageSize = (int)pagesize.Value;
            rpp.ParamList = new List<API.ReportSqlParams>();

            if (!string.IsNullOrEmpty(this.fromdate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime startTime = new API.ReportSqlParams_DataTime();
                startTime.ParamName = "StartTime";
                startTime.ParamValues = this.fromdate.SelectedValue;
                rpp.ParamList.Add(startTime);
            }

            if (!string.IsNullOrEmpty(this.todate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime endTime = new API.ReportSqlParams_DataTime();
                endTime.ParamName = "EndTime";
                endTime.ParamValues = this.todate.SelectedValue;
                rpp.ParamList.Add(endTime);
            }

            if (!string.IsNullOrEmpty(this.imei.Text.ToString()))
            {
                API.ReportSqlParams_String im = new API.ReportSqlParams_String();
                im.ParamName = "IMEI";
                im.ParamValues = this.imei.Text;
                rpp.ParamList.Add(im);
            }
            if (!string.IsNullOrEmpty(this.mobilePhone.Text.ToString()))
            {
                API.ReportSqlParams_String phone = new API.ReportSqlParams_String();
                phone.ParamName = "MobiPhone";
                phone.ParamValues = this.mobilePhone.Text;
                rpp.ParamList.Add(phone);
            }
            if (!string.IsNullOrEmpty(this.cardid.Text.ToString()))
            {
                API.ReportSqlParams_String card = new API.ReportSqlParams_String();
                card.ParamName = "IDCard";
                card.ParamValues = this.cardid.Text;
                rpp.ParamList.Add(card);
            }
            if (!string.IsNullOrEmpty(this.membername.Text.ToString()))
            {
                API.ReportSqlParams_String name = new API.ReportSqlParams_String();
                name.ParamName = "MemberName";
                name.ParamValues = this.membername.Text;
                rpp.ParamList.Add(name);
            }

            if (this.aduited.SelectedIndex != 0)
            {
                API.ReportSqlParams_String state = new API.ReportSqlParams_String();
                state.ParamName = "State";
                if (this.aduited.SelectedIndex == 1)
                {
                    state.ParamValues = "No";
                }
                else
                {
                    state.ParamValues = "Yes";
                }
                
                rpp.ParamList.Add(state);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 87, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
        }

        /// <summary>
        /// 查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            models.Clear();
            this.flag.Tag = false;
            dataGridOffList.Rebind();
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.pager.PageIndexChanged -= page_PageIndexChanged;
            this.pager.Source = pcv1;
            this.pager.PageIndexChanged += page_PageIndexChanged;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_VIP_Renew> aduitList = pageParam.Obj as List<API.View_VIP_Renew>;
                if (aduitList.Count != 0)
                {
                    models.AddRange(aduitList);
                    dataGridOffList.Rebind();

                    this.pager.PageSize = (int)pagesize.Value;
                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);

                    this.pager.PageIndexChanged -= page_PageIndexChanged;
                    this.pager.Source = pcv;
                    this.pager.PageIndexChanged += page_PageIndexChanged;
                    this.pager.PageIndex = pageindex;

                }
            }
        }

        #endregion

        #region  申请

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"申请成功");
                Logger.Log("申请成功");

                Clear();
                Search();
                //this.hallName.TextBox.SearchText = string.Empty;
                this.renewPoint.Value = 0;
                this.money.Value = 0;
            }
            else
            {
                Logger.Log(e.Result.Message + ", 申请失败");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + ", 申请失败");
            }
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (!Convert.ToBoolean(this.flag.Tag))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要申请的续期单");
                return;
            }
            //if (string.IsNullOrEmpty(this.hallName.TextBox.SearchText))
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择营业厅");
            //    return;
            //}

            if (this.apply.Tag.ToString() == "Y")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该记录已经发起申请");
                return;
            }
       
            API.VIP_RenewBackAduit vrb = new API.VIP_RenewBackAduit();
            vrb.ApplyDate = DateTime.Now;
            vrb.ApplyUser = Store.LoginUserInfo.UserID;

            //models
           API.View_VIP_Renew vvr = dataGridOffList.SelectedItem as  API.View_VIP_Renew;
            var hall = from h in Store.ProHallInfo
                    where h.HallName==vvr.HallName
                    select h;
            vrb.HallID = hall.First().HallID; ///this.hallName.Tag.ToString();

            vrb.ReNewID = Convert.ToInt32(this.validity.Tag.ToString());
            vrb.VIP_ID =Convert.ToInt32( this.memname.Tag.ToString());
            vrb.UserID = this.seller.Tag.ToString();
            vrb.Note = this.note.Text;
            vrb.SysDate = DateTime.Now;

            PublicRequestHelp prh = new PublicRequestHelp(null, 33, new object[] { vrb}, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
     
        }

        #endregion

        /// <summary>
        /// 暂时不申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnApplly(object sender, RoutedEventArgs e)
        {
            Clear();
            dataGridOffList.SelectedItem = null;
        }

        private void Clear()
        {
            this.imei_s.Text = string.Empty;
            this.mobiphone.Text = string.Empty;
            this.cardid_s.Text = string.Empty;
            this.cardType.Text = string.Empty;
            this.memname.Text = string.Empty;
            this.starttime.Text = string.Empty;
            this.point.Text = string.Empty;
            this.validity.Text = string.Empty;
            this.seller.Text = string.Empty;
            this.currentTimeTo.DateTimeText = string.Empty;
        }


    }
}
