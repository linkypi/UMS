using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace UserMS.Views.CMS.CardOperate.TC
{
    public partial class VIPBackAduit : Page
    {
        private List<API.View_VIPBackAduit> models = null;
        private List<API.View_VIPService> serviceDetail = null;

        private  int pageindex ;
        private string menuid = "";
        private bool flag = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "46";
            }
            models = new List<API.View_VIPBackAduit>();
            serviceDetail = new List<API.View_VIPService>();

            this.ServiceDetail.ItemsSource = serviceDetail;
            this.dataGridOffList.ItemsSource = models;
            this.fromdate.SelectedValue = DateTime.Now.Date;
            pager.PageSize = (int)pagesize.Value;

            Search();
            this.dataGridOffList.SelectionChanged += dataGridOffList_SelectionChanged;
            this.imei.KeyDown += EnterKeyDown;
            this.cardnum.KeyDown += EnterKeyDown;
            this.membername.KeyDown += EnterKeyDown;
            this.mobiphone.KeyDown += EnterKeyDown;
            this.KeyUp += EnterKeyDown;
        }

        public VIPBackAduit()
        {
            InitializeComponent();
            flag = true;
            this.SizeChanged += VIPBackAduit_SizeChanged;
        }

        void VIPBackAduit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("pager") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        /// <summary>
        /// 选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dataGridOffList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (dataGridOffList.SelectedItem != null)
            {
                API.View_VIPBackAduit va = dataGridOffList.SelectedItem as API.View_VIPBackAduit;

                this.showApplyed.Text = va.Aduited == "Y" ? "已审批" : "未审批";
                if (va.Aduited == "Y")
                {
                    this.aduit.IsEnabled = false;
                }
                else
                {
                    this.aduit.IsEnabled = true;
                }
                this.aduit.Tag = va.ID;  //标记是否已选择
                this.aduitID.Text = va.AduitID;
                this.showCardNum.Text = va.IDCard;
                this.showMemName.Text = va.MemberName;
                this.showMobi.Text = va.MobiPhone;
                this.showPoint.Text = va.Point.ToString();
                this.showRegDate.Text = DateTime.Parse(va.StartTime.ToString()).Date.ToString();
                this.showSeller.Text = string.IsNullOrEmpty(va.Seller) ? "" : va.Seller;

                this.showVIPType.Text = va.VIPType;
                this.sohwimei.Text = va.IMEI;

                int count;
                if (string.IsNullOrEmpty(va.Validity.ToString()))
                {
                    count = 0;
                }
                else
                {
                    count = (int)va.Validity;
                }
                this.showValidity.Text = count.ToString();
                timeto.SelectedValue = DateTime.Now.AddDays(count);

                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 91, new object[] { va.VIP_ID }, new EventHandler<API.MainCompletedEventArgs>(GetServiceCompleted));
            }
        }

        /// <summary>
        /// 获取服务完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetServiceCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            serviceDetail.Clear();
            ServiceDetail.Rebind();
            if (!e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获取服务失败");
                return;
            }

            List<API.View_VIPService> list = e.Result.Obj as List<API.View_VIPService>;
            serviceDetail.AddRange(list);
            ServiceDetail.Rebind();
        }

        void EnterKeyDown(object sender, KeyEventArgs e)
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
            if (!string.IsNullOrEmpty(this.mobiphone.Text.ToString()))
            {
                API.ReportSqlParams_String phone = new API.ReportSqlParams_String();
                phone.ParamName = "MobiPhone";
                phone.ParamValues = this.mobiphone.Text;
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
                API.ReportSqlParams_String state = new API.ReportSqlParams_String();
                state.ParamName = "Aduited";
                if (this.aduited.SelectedIndex == 1)
                {
                    state.ParamValues = "N";
                }
                else
                {
                    state.ParamValues = "Y";
                }

                rpp.ParamList.Add(state);
            }

            if (this.Used.SelectedIndex != 0)
            {
                API.ReportSqlParams_String use= new API.ReportSqlParams_String();
                use.ParamName = "Used";
                if (this.Used.SelectedIndex == 1)
                {
                    use.ParamValues = "N";
                }
                else
                {
                    use.ParamValues = "Y";
                }

                rpp.ParamList.Add(use);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 93, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
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

            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_VIPBackAduit> aduitList = pageParam.Obj as List<API.View_VIPBackAduit>;
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

        #region  审批

        /// <summary>
        /// 暂时不审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void unAduit_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            this.dataGridOffList.SelectedItem = null;
            this.aduit.Tag = null;
            this.showApplyed.Text = string.Empty;
            this.aduit.IsEnabled = true;

            this.showCardNum.Text = string.Empty;
            this.showMemName.Text = string.Empty;
            this.showMobi.Text = string.Empty;
            this.showPoint.Text = string.Empty;
            this.showRegDate.Text = string.Empty;
            this.showSeller.Text = string.Empty;

            this.showVIPType.Text = string.Empty;
            this.sohwimei.Text = string.Empty;
            this.showValidity.Text = string.Empty;
            timeto.DateTimeText = string.Empty;

            serviceDetail.Clear();
            ServiceDetail.Rebind();
        }


        void aduit_Click(object sender, RoutedEventArgs e)
        {
            API.VIP_VIPBackAduit vip = null;

            if (this.backMoney.Value == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入退款金额");
                return;
            }
            API.View_VIPBackAduit vvb = dataGridOffList.SelectedItem as API.View_VIPBackAduit;

            vip = new API.VIP_VIPBackAduit();
            vip.ID = vvb.ID.ToString();
            vip.AduitDate = DateTime.Now;
            vip.AduitUser = Store.LoginUserInfo.UserID;
            vip.Aduited = true;
            vip.Money =(decimal) backMoney.Value;
            vip.Note = this.note.Text;
            PublicRequestHelp prh = new PublicRequestHelp(null,49,new object[]{vip},new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"提交成功");
                Logger.Log("提交成功");
          
                this.dataGridOffList.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"提交失败");
                Logger.Log("提交失败");
            }
        }

        #endregion

    }
}
