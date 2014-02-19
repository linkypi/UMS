using SlModel;
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
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Apply
{
    /// <summary>
    /// SellBackAduitSearch.xaml 的交互逻辑
    /// </summary>
    public partial class SellBackAduitSearch : Page
    {
        public SellBackAduitSearch()
        {
            InitializeComponent();
            flag = true;
            this.SizeChanged += SellBackAduit_SizeChanged;
        }

        private List<API.View_Pro_SellBackAduit> models = null;

        private int pageindex;
        private bool flag = false;
        private List<int> idlist = new List<int>();
        private ROHallAdder hAdder;

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
                menuid = "233";
            }
            page.PageSize = (int)pagesize.Value;
            models = new List<API.View_Pro_SellBackAduit>();
            GridAuitList.ItemsSource = models;
            hAdder = new ROHallAdder(ref this.hallid, int.Parse(menuid));
            this.fromDate.SelectedValue = DateTime.Now.Date;

            List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
            };
            this.ckb.ItemsSource = list;
            this.ckbPassed.ItemsSource = list;
            this.ckbUsed.ItemsSource = list;
            this.ckb.SelectedIndex = 2;
            this.ckbPassed.SelectedIndex = 2;
            this.ckbUsed.SelectedIndex = 2;

            Search();

            this.KeyDown += BorowAduit_KeyDown;
            this.ckb.KeyDown += BorowAduit_KeyDown;
            this.applyUser.KeyDown += BorowAduit_KeyDown;
            this.hallid.KeyDown += BorowAduit_KeyDown;
            this.fromDate.KeyDown += BorowAduit_KeyDown;
            this.toDate.KeyDown += BorowAduit_KeyDown;
        }


        void SellBackAduit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        void Clear()
        {
            //this.orderID.Text = string.Empty;
            hadmoney.Text = string.Empty;
            aduitMoney.Text = string.Empty;
            this.SellID.Text = string.Empty;
            this.hallname.Text = string.Empty;
            this.CusPhone.Text = string.Empty;
            this.seller.Text = string.Empty;
            this.SellDate.Text = string.Empty;
            this.user.Text = string.Empty;
        }

        #region 详情

        void GridAuitList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (this.GridAuitList.SelectedItem == null)
            {
                return;
            }
            API.View_Pro_SellBackAduit vps = this.GridAuitList.SelectedItem as API.View_Pro_SellBackAduit;

            this.SellID.Text = vps.SellIDS;
            this.hallname.Text = Convert.ToString(vps.HallName);
            this.CusPhone.Text = vps.CusPhone;
            this.seller.Text = Convert.ToString(vps.Seller);
            this.SellDate.Text = vps.SellDate;
            this.user.Text = Convert.ToString(vps.UserName);
            this.CusName.Text = Convert.ToString(vps.CusName);

            PublicRequestHelp peh = new PublicRequestHelp(this.isbusy, 196, new object[] { vps.SellID, vps.ID }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.View_SellBackOffList> arr = e.Result.ArrList[0] as List<API.View_SellBackOffList>;
                List<API.View_ProSellBackAduitDetail> list = e.Result.Obj as List<API.View_ProSellBackAduitDetail>;

                decimal aMoney = 0; //实退
                decimal sbMoney = 0;//应退
                foreach (var item in list)
                {
                    aMoney += (item.CashPrice - Convert.ToDecimal(item.BackPrice - item.AduitBackPrice) + item.OffSepecialPrice) * item.ProCount;
                    sbMoney += (item.CashPrice - Convert.ToDecimal(item.BackPrice - item.AduitBackPrice) + item.OffSepecialPrice) * item.ProCount;
                }
                foreach (var item in arr)
                {
                    aMoney -= Convert.ToDecimal(item.OffMoney);
                    sbMoney -= Convert.ToDecimal(item.OffMoney);
                }

                this.aduitMoney.Text = Math.Round(aMoney, 4).ToString();
                hadmoney.Text = Math.Round(sbMoney, 4).ToString();

                GridDetail.ItemsSource = list;
                GridDetail.Rebind();
                GridOff.ItemsSource = arr;
                GridOff.Rebind();
            }
        }

        #endregion

        private void BorowAduit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        /// <summary>
        /// 复制审批单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CopyAduitID_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;

            try
            {
                System.Windows.Clipboard.SetText(btn.Tag.ToString());
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "审批单复制成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, ex.Message + "\n请在页面中点击右键选择 Silverlight(S) , 在权\n限选项中将剪切板删除或者将其权限改为允许");
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
            GridDetail.ItemsSource = null;
            GridDetail.Rebind();

            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = page.PageIndex;
            rpp.PageSize = (int)pagesize.Value;
            rpp.ParamList = new List<API.ReportSqlParams>();

            if (!string.IsNullOrEmpty(this.fromDate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime startTime = new API.ReportSqlParams_DataTime();
                startTime.ParamName = "StartTime";
                startTime.ParamValues = this.fromDate.SelectedValue;
                rpp.ParamList.Add(startTime);
            }

            if (!string.IsNullOrEmpty(this.toDate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime endTime = new API.ReportSqlParams_DataTime();
                endTime.ParamName = "EndTime";
                endTime.ParamValues = this.toDate.SelectedValue;
                rpp.ParamList.Add(endTime);
            }

            if (!string.IsNullOrEmpty(this.hallid.Text))//.TextBox.SearchText
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "HallID";
                hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(this.hallid.Tag.ToString().Split(",".ToCharArray()));
                rpp.ParamList.Add(hall);
            }

            if (this.ckb.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool aduit = new API.ReportSqlParams_Bool();
                aduit.ParamName = "Aduited";
                CkbModel cb = this.ckb.SelectedItem as CkbModel;
                aduit.ParamValues = cb.Flag;
                rpp.ParamList.Add(aduit);
            }

            if (this.ckbPassed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool passed = new API.ReportSqlParams_Bool();
                passed.ParamName = "Passed";
                CkbModel cb1 = this.ckbPassed.SelectedItem as CkbModel;
                passed.ParamValues = cb1.Flag;
                rpp.ParamList.Add(passed);
            }

            if (this.ckbUsed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool used = new API.ReportSqlParams_Bool();
                used.ParamName = "Used";
                CkbModel cb2 = this.ckbUsed.SelectedItem as CkbModel;
                used.ParamValues = cb2.Flag;
                rpp.ParamList.Add(used);
            }

            if (!string.IsNullOrEmpty(this.applyUser.Text.ToString()))
            {
                API.ReportSqlParams_String aduitUser = new API.ReportSqlParams_String();
                aduitUser.ParamName = "ApplyUser";
                aduitUser.ParamValues = this.applyUser.Text;
                rpp.ParamList.Add(aduitUser);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 291, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
        }

        /// <summary>
        /// 查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            models.Clear();
            GridAuitList.Rebind();
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_Pro_SellBackAduit> aduitList = pageParam.Obj as List<API.View_Pro_SellBackAduit>;
                if (aduitList.Count != 0 && aduitList != null)
                {
                    models.AddRange(aduitList);
                    GridAuitList.Rebind();

                    this.page.PageSize = (int)pagesize.Value;
                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);

                    this.page.PageIndexChanged -= page_PageIndexChanged;
                    this.page.Source = pcv;
                    this.page.PageIndexChanged += page_PageIndexChanged;
                    this.page.PageIndex = pageindex;

                }
            }

            if (this.ckb.SelectedIndex == 1)
            {
                GridAuitList.Columns[GridAuitList.Columns.Count - 1].IsVisible = false;
            }
            else
            {
                GridAuitList.Columns[GridAuitList.Columns.Count - 1].IsVisible = true;
            }

        }

        #endregion

        private void GridDetail_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            List<API.View_SellBackOffList> arr = GridOff.ItemsSource as List<API.View_SellBackOffList>;
            List<API.View_ProSellBackAduitDetail> list = GridDetail.ItemsSource as List<API.View_ProSellBackAduitDetail>;

            decimal aMoney = 0;
            foreach (var item in list)
            {
                if (item.AduitBackPrice > item.BackPrice || item.AduitBackPrice < 0 || item.AduitBackPrice < item.CashPrice)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请确保审批单价在申请单价范围内");
                    this.aduitMoney.Text = "0";
                    item.AduitBackPrice = 0;
                    return;
                }

                item.AduitBackPrice = Decimal.Truncate(Convert.ToDecimal(item.AduitBackPrice * 100)) / 100;
                //OffPrice 最原始单价   OtherCash 多收   CashTicket 代金卷抵扣金额
                //aMoney += (decimal)item.BackCount * (decimal)(item.AduitBackPrice - item.OffPrice + item.OtherCash  - item.CashTicket
                //   );                                      //减去差价                           //加上特殊优惠                       
                aMoney += (decimal)(item.CashPrice - (item.BackPrice - item.AduitBackPrice) + item.OffSepecialPrice) * item.ProCount;

            }
            foreach (var item in arr)
            {
                aMoney -= Convert.ToDecimal(item.OffMoney);
            }

            this.aduitMoney.Text = (Decimal.Truncate(Convert.ToDecimal(aMoney * 100)) / 100).ToString();
        }

        private void GridDetail_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {

        }

        private void delChecked_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridAuitList.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的数据！");
                return;
            }
            List<int> list = new List<int>();
            foreach (var item in GridAuitList.SelectedItems)
            {
                API.View_Pro_SellBackAduit vp = item as API.View_Pro_SellBackAduit;
                if (vp.Used == "Y")
                {
                    MessageBox.Show("审批单 " + vp.AduitID + " 已使用，无法删除！");
                    return;
                }
                list.Add(vp.ID);
            }
            //API.View_Pro_SellAduit vp = GridAuitList.SelectedItem as API.View_Pro_SellAduit;
            if (MessageBox.Show("确定删除选中项吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 211, new object[] { list }, new EventHandler<API.MainCompletedEventArgs>(DelCompleted));
        }

        private void DelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            this.isbusy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                GridDetail.ItemsSource = null;
                GridDetail.Rebind();
                GridOff.ItemsSource = null;
                GridOff.Rebind();
                Search();
            }
        }

        private void GridOff_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {

        }
    }
}
