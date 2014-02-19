using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace UserMS.Views.CMS.CardOperate.renewal
{
    public partial class RenewBackAduit : Page
    {

        private  int pageindex ;
        private bool flag = false;
        private double CashRatio = 0; 
        private double PointRatio = 0;

        private List<API.View_VIP_RenewBackAduit> models = null;

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
                menuid = "39";
            }
            models = new List<API.View_VIP_RenewBackAduit>();
            dataGridOffList.ItemsSource = models;
            this.fromdate.SelectedValue = DateTime.Now.Date;
            this.aduited.SelectedIndex = 0;
            this.user.IsReadOnly = true;
            this.user.Text = Store.LoginUserInfo.UserName;
            this.user.Tag = Store.LoginUserInfo.UserID;
            var cash = from c in Store.Options
                       where c.ClassName == "CashRenew"
                       select c;

            this.cashPercent.IsReadOnly = true;
            this.cashPercent.Text = cash.First().Value.ToString() + " : " + cash.First().Value2.ToString();

            CashRatio = double.Parse(cash.First().Value2) / double.Parse(cash.First().Value);

            var pquery = from c in Store.Options
                         where c.ClassName == "PointRenew"
                         select c;
            this.pointPercent.IsReadOnly = true;
            this.pointPercent.Text = pquery.First().Value.ToString() + " : " + pquery.First().Value2.ToString();

            PointRatio = double.Parse(pquery.First().Value2) / double.Parse(pquery.First().Value);
            Search();

            dataGridOffList.SelectionChanged += dataGridOffList_SelectionChanged;
            this.aduitMoney.GotFocus += submitMoney_GotFocus;
            this.aduitPoint.GotFocus += submitPoint_GotFocus;
            this.imei.KeyDown += Form_KeyDown;
            this.cardnum.KeyDown += Form_KeyDown;
            this.membername.KeyDown += Form_KeyDown;
            this.mobiphone.KeyDown += Form_KeyDown;
            this.KeyUp += Form_KeyDown;
        }

        public RenewBackAduit()
        {
            InitializeComponent();
            flag = true;
            this.SizeChanged += RenewBackAduit_SizeChanged;
        }

        void RenewBackAduit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("pager") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        #region  事   件

        void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                Search();
            }
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


        void submitPoint_GotFocus(object sender, RoutedEventArgs e)
        {
            pointBack.IsChecked = true;
            aduitMoney.Value = 0;
            aduitMoney.IsEnabled = false;
        }

        void submitMoney_GotFocus(object sender, RoutedEventArgs e)
        {
            cashBack.IsChecked = true;
            aduitPoint.Value = 0;
        }

      
        private void aduitMoney_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (dataGridOffList.SelectedItem == null)
            {
                return;
            }
            if (currentTimeTo.SelectedDate != null)
            {
                int validity = int.Parse((aduitMoney.Value * CashRatio).ToString());
                TimeSpan ts = new TimeSpan(validity, 0, 0, 0);
                this.cancelTimeto.SelectedDate = ((DateTime)currentTimeTo.SelectedDate).Subtract(ts);
            }
        }

        private void aduitPoint_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (dataGridOffList.SelectedItem == null)
            {
                return;
            }
            if (currentTimeTo.SelectedDate != null)
            {
                int validity = int.Parse((aduitPoint.Value * PointRatio).ToString());
                TimeSpan ts = new TimeSpan(validity, 0, 0, 0);
                cancelTimeto.SelectedDate = ((DateTime)currentTimeTo.SelectedDate).Subtract(ts);
            }
        }

        /// <summary>
        /// 选择项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dataGridOffList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            API.View_VIP_RenewBackAduit vp = dataGridOffList.SelectedItem as API.View_VIP_RenewBackAduit;
            if (vp != null)
            {
                if (vp.Aduited == "Y")
                {
                    aduit.IsEnabled = false;
                }
                else
                {
                    aduit.IsEnabled = true;
                }
                this.imei_s.Text = Convert.ToString( vp.IMEI);
                this.aduit.Tag = vp.Aduited;
                this.mobiphone.Text = Convert.ToString( vp.MobiPhone);
                this.cardid_s.Text = Convert.ToString( vp.IDCard);
                this.cardType.Text =Convert.ToString(  vp.VIPType);
                this.memname.Text = Convert.ToString( vp.MemberName);
               // this.memname.Tag = vp.VIPID;
                this.starttime.Text = Convert.ToString( vp.StartTime);
                this.point.Text = vp.Point.ToString();
                this.validity.Text = vp.RenewValidity.ToString();
               // this.currentValidity.Text = vp.CurrentValidity.ToString();
                this.note.Text =Convert.ToString( vp.Note);
               // this.validity.Tag = vp.RenewID;
        
                this.currentTimeTo.SelectedValue =Convert.ToDateTime( vp.EndTime); // DateTime.Now.Date.AddDays(valcount);

                //if (!string.IsNullOrEmpty(vp.EndTime.ToString()) && !string.IsNullOrEmpty(vp.RenewDate))
                //{
                //    TimeSpan ts = (DateTime)vp.EndTime - Convert.ToDateTime(vp.RenewDate);
                //    //续期前有效期为
                //    beforeRenew.Text = (Math.Round(ts.TotalDays) - (int)vp.RenewValidity).ToString();
                //}

                beforeRenew.Text = vp.OldEndDate;
                this.aduitID.Text = vp.AduitID;
                if (vp.RenewTypeName == "现金续期")
                {
                    cashRenew.IsChecked = true;
                    cashRenew.IsEnabled = true;
                    pointRenew.IsEnabled = false;

                    cashBack.IsChecked = true;
                    cashBack.IsEnabled = true;
                    pointBack.IsEnabled = false;

                    aduitMoney.IsEnabled = true;
                    aduitPoint.IsEnabled = false;

                    renewPoint.IsEnabled = false;
                    renewMoney.IsEnabled = true;
                    pointBack.IsEnabled = false;

                    double money =  Convert.ToDouble(vp.RenewMoney);
                    this.renewMoney.Value = money;
                    this.aduitMoney.Value = money;
                    this.renewPoint.Value = 0;
                    this.aduitPoint.Value = 0;
           
                    TimeSpan tspan = new TimeSpan((int)vp.RenewValidity, 0, 0, 0);
                    this.cancelTimeto.SelectedValue = DateTime.Parse(this.currentTimeTo.SelectedValue.ToString()).Subtract(tspan);

                }
                else 
                {
                    this.pointRenew.IsChecked = true;
                    cashRenew.IsEnabled = false;
                    pointRenew.IsEnabled = true;

                    this.pointBack.IsChecked = true;
                    cashBack.IsEnabled = false;
                    pointBack.IsEnabled = true;

                    aduitMoney.IsEnabled = false;
                    aduitPoint.IsEnabled = true;
                    renewPoint.IsEnabled = true;
                    renewMoney.IsEnabled = false;

                    cashBack.IsEnabled = false;
                    pointBack.IsEnabled = true;

                    this.renewMoney.Value = 0;
                    this.aduitMoney.Value = 0;
                    this.renewPoint.Value = Convert.ToDouble(vp.RenewPoint.ToString());
                    this.aduitPoint.Value = Convert.ToDouble(vp.RenewPoint);
                    TimeSpan tspan = new TimeSpan((int)vp.RenewValidity, 0, 0, 0);
                    this.cancelTimeto.SelectedValue = DateTime.Parse(this.currentTimeTo.SelectedValue.ToString()).Subtract(tspan);
                }
                
            }
        }

        /// <summary>
        /// 选择仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           // hall.GetHall(hall.FilterHall(38, Store.ProHallInfo));
        }

        #endregion 

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
            if (!flag) { return; }
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
            if (!string.IsNullOrEmpty(this.mobiPhone.Text.ToString()))
            {
                API.ReportSqlParams_String phone = new API.ReportSqlParams_String();
                phone.ParamName = "MobiPhone";
                phone.ParamValues = this.mobiPhone.Text;
                rpp.ParamList.Add(phone);
            }
            if (!string.IsNullOrEmpty(this.cardnum.Text.ToString()))
            {
                API.ReportSqlParams_String card = new API.ReportSqlParams_String();
                card.ParamName = "IDCard";
                card.ParamValues = this.cardnum.Text;
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
                API.ReportSqlParams_String Aduited = new API.ReportSqlParams_String();
                Aduited.ParamName = "Aduited";
                if (this.aduited.SelectedIndex == 1)
                {
                    Aduited.ParamValues = "N";
                }
                else
                {
                    Aduited.ParamValues = "Y";
                }

                rpp.ParamList.Add(Aduited);
            }

            if (this.Passed.SelectedIndex != 0)
            {
                //API.ReportSqlParams_String passed = new API.ReportSqlParams_String();
                //passed.ParamName = "Passed";
                //CkbModel cb1 = this.Passed.SelectedItem as CkbModel;
                //passed.ParamValues = cb1.Flag;
                //rpp.ParamList.Add(passed);

                API.ReportSqlParams_String passed = new API.ReportSqlParams_String();
                passed.ParamName = "Passed";
                if (this.Passed.SelectedIndex == 1)
                {
                    passed.ParamValues = "N";
                }
                else
                {
                    passed.ParamValues = "Y";
                }

                rpp.ParamList.Add(passed);
            }

            if (this.Used.SelectedIndex != 0)
            {
                //API.ReportSqlParams_Bool used = new API.ReportSqlParams_Bool();
                //used.ParamName = "Used";
                //CkbModel cb2 = this.Used.SelectedItem as CkbModel;
                //used.ParamValues = cb2.Flag;
                //rpp.ParamList.Add(used);

                API.ReportSqlParams_String used = new API.ReportSqlParams_String();
                used.ParamName = "Used";
                if (this.Used.SelectedIndex == 1)
                {
                    used.ParamValues = "N";
                }
                else
                {
                    used.ParamValues = "Y";
                }

                rpp.ParamList.Add(used);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 89, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
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

                List<API.View_VIP_RenewBackAduit> aduitList = pageParam.Obj as List<API.View_VIP_RenewBackAduit>;
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

        #region  "审批"

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
           if (  dataGridOffList.SelectedItem == null)
           {
               MessageBox.Show("请选择要删除的记录！");
               return;
           }
           if (MessageBox.Show("确定删除该申请记录吗？", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
           {
               return;
           }
          
            Clear();
            API.View_VIP_RenewBackAduit rba = dataGridOffList.SelectedItem as API.View_VIP_RenewBackAduit;

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 213, new object[] { rba.ID }, new EventHandler<API.MainCompletedEventArgs>(DeleteCompleted));
        }

        private void DeleteCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "审批失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                Logger.Log(e.Result.Message);
                Search();
            }
            else
            {
                MessageBox.Show(e.Result.Message);
                Logger.Log(e.Result.Message);
            }
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Aduit_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(cashRenew.IsChecked))
            {
                if (aduitMoney.Value > renewMoney.Value)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批金额不能大于续期金额");
                    return;
                }
            }
            if (Convert.ToBoolean(pointRenew.IsChecked))
            {
                if (aduitPoint.Value > renewPoint.Value)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批积分不能大于续期积分");
                    return;
                }
            }

            if (models.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无数据可审批");
                return;
            }
            if (dataGridOffList.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要审批的数据");
                return;
            }
            API.View_VIP_RenewBackAduit rba = dataGridOffList.SelectedItem as API.View_VIP_RenewBackAduit;

            if (rba.Aduited == "Y")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该审批单已审批");
                return;
            }
            List<API.VIP_RenewBackAduit> mList = new List<API.VIP_RenewBackAduit>();
            API.VIP_RenewBackAduit sa = new API.VIP_RenewBackAduit();

            if (Convert.ToBoolean(cashRenew.IsChecked))
            {
                sa.Money = (decimal) aduitMoney.Value;
                double val =double.Parse( sa.Money.ToString() )* double.Parse(CashRatio.ToString());
                sa.Validity =(int)Math.Round(val);
            }
            if (Convert.ToBoolean(pointRenew.IsChecked))
            {
                sa.Point = (decimal)aduitPoint.Value;
                double val = double.Parse(sa.Point.ToString()) * double.Parse(PointRatio.ToString());
                sa.Validity = (int)Math.Round(val);
            }
           
            sa.ID = rba.ID;
            sa.AduitDate = DateTime.Now;
            sa.AduitUser = Store.LoginUserInfo.UserID;
            sa.Aduited = true;
            sa.Passed = true;
            sa.Note = rba.Note;
           // sa.Passed = true;
            mList.Add(sa);
            
            PublicRequestHelp prh = new PublicRequestHelp(null, 34, new object[] {  mList }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        void Clear()
        {
            this.imei_s.Text = string.Empty;
            this.aduit.Tag = string.Empty;
            this.mobiphone.Text = string.Empty;
            this.cardid_s.Text = string.Empty;
            this.cardType.Text = string.Empty;
            this.memname.Text = string.Empty;
            this.starttime.Text = string.Empty;
            this.point.Text = string.Empty;
            this.validity.Text = string.Empty;
            this.currentTimeTo.DateTimeText = string.Empty;

            this.aduitID.Text = string.Empty;
            this.renewMoney.Value = 0;
            this.aduitMoney.Value =0;
            this.cancelTimeto.DateTimeText = string.Empty; 
            this.renewPoint.Value = 0;
            this.aduitPoint.Value =0;
        }

        /// <summary>
        /// 审批完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "审批失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                currentTimeTo.SelectedDate = cancelTimeto.SelectedDate;
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批成功");
                Logger.Log("审批成功");
                Search();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批失败");
                Logger.Log("审批失败");
            }
        }

        #endregion

      
    }
}
