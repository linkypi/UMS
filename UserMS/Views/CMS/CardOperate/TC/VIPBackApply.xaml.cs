using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.CMS.CardOperate.TC
{
    public partial class VIPBackApply : Page
    {
        private HallFilter hadder = null;
        private int pageindex ;
        private bool flag = false;
        private List<API.View_VIPBackApply> models = null;
        private List<API.View_VIPService> serviceDetail = null;
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
                menuid = "45";
            }

            models = new List<API.View_VIPBackApply>();
            serviceDetail = new List<API.View_VIPService>();
            ServiceDetail.ItemsSource = serviceDetail;
            dataGridOffList.ItemsSource = models;
            this.pager.PageSize = (int)pagesize.Value;
            this.fromdate.SelectedValue = DateTime.Now.Date;

            hadder = new HallFilter(false, ref this.hallName);
            List<API.Pro_HallInfo> halls = hadder.FilterHall(45, Store.ProHallInfo);
            if (halls.Count != 0)
            {
                hallName.Tag = halls[0].HallID;
                hallName.TextBox.SearchText = halls[0].HallName;
            }

            Search();
            this.dataGridOffList.SelectionChanged += dataGridOffList_SelectionChanged;
            this.hallName.SearchButton.Click += SearchButton_Click;
            this.imei.KeyDown += EnterKeyDown;
            this.cardnum.KeyDown += EnterKeyDown;
            this.membername.KeyDown += EnterKeyDown;
            this.mobiphone.KeyDown += EnterKeyDown;
            this.KeyUp += EnterKeyDown;
        }

        public VIPBackApply()
        {
            InitializeComponent();
            flag = true;
            this.SizeChanged += VIPBackApply_SizeChanged;
        }

        void VIPBackApply_SizeChanged(object sender, SizeChangedEventArgs e)
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
                API.View_VIPBackApply va = dataGridOffList.SelectedItem as API.View_VIPBackApply;

                this.showApplyed.Text = va.Applyed == "Y" ? "已申请" : "未申请";
                if (va.Applyed == "Y")
                {
                    this.apply.IsEnabled = false;
                }
                else
                {
                    this.apply.IsEnabled = true;
                }
                this.apply.Tag = va.ID;  //标记是否已选择

                this.showCardNum.Text = va.IDCard;
                this.showMemName.Text = va.MemberName;
                this.showMobi.Text = va.MobiPhone;
                this.showPoint.Text = va.Point.ToString();
                this.showRegDate.Text = va.StartTime;
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

                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 91, new object[] { va.ID }, new EventHandler<API.MainCompletedEventArgs>(GetServiceCompleted));
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

           List< API.View_VIPService> list = e.Result.Obj as List<API.View_VIPService>;
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

        /// <summary>
        /// 添加仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hadder.GetHall(hadder.FilterHall(45,Store.ProHallInfo));
        }

        /// <summary>
        /// 暂时不申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void unapply_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            this.dataGridOffList.SelectedItem = null;
            this.apply.Tag = null;
            this.showApplyed.Text = string.Empty;
            this.apply.IsEnabled = true;

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

            if (this.Applyed.SelectedIndex != 0)
            {
                API.ReportSqlParams_String state = new API.ReportSqlParams_String();
                state.ParamName = "Applyed";
                if (this.Applyed.SelectedIndex == 1)
                {
                    state.ParamValues = "N";
                }
                else
                {
                    state.ParamValues = "Y";
                }

                rpp.ParamList.Add(state);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 90 , new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
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

                List<API.View_VIPBackApply> aduitList = pageParam.Obj as List<API.View_VIPBackApply>;
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

        #region 申请

        /// <summary>
        /// 申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void apply_Click(object sender, RoutedEventArgs e)
        {

            if (apply.Tag == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项");
                return;
            }

            if (string.IsNullOrEmpty(this.hallName.TextBox.SearchText))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择营业厅");
                return;
            }
          //         服务使用量
            decimal serviceCount = 0;
     
            foreach (var item in serviceDetail)
            {
                serviceCount +=(decimal) item.UsedCount;
            }
            if (serviceCount != 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该会员已使用服务不能退卡");
                return;
            }
            //if (serviceCount < serviceDetail[0].SCount)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"该会员使用的服务过少不能申请退卡");
            //    return;
            //}
            API.View_VIPBackApply vba = dataGridOffList.SelectedItem as API.View_VIPBackApply;

            API.VIP_VIPBackAduit va = new API.VIP_VIPBackAduit();

            va.VIP_ID= vba.ID;
            va.ApplyDate = DateTime.Today;
            va.ApplyUser = Store.LoginUserInfo.UserID;
            va.HallID = this.hallName.Tag.ToString();
            va.SysDate = DateTime.Now;
            va.Note = this.note.Text;
           
            PublicRequestHelp prh = new PublicRequestHelp(null, 48, new object[] { va }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));

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
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"申请成功");
                Logger.Log("申请成功");
                Clear();
                Search();
            }
            else
            {
                Logger.Log(e.Result.Message+", 申请失败");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + ", 申请失败");
            }

        }

        #endregion  
    }
}
