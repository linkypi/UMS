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

namespace UserMS.Views.ProSell.Aduit
{
    /// <summary>
    /// SellAduit3.xaml 的交互逻辑
    /// </summary>
    public partial class SellAduit3 : Page
    {
        public SellAduit3()
        {
            InitializeComponent();
            flag = true;
            this.SizeChanged += SellAduit_SizeChanged;
        }

        private List<API.View_Pro_SellAduit> models = null;
        private List<int> idList = new List<int>();

        private  int pageindex ;
        private ROHallAdder hAdder;
        private bool flag = false;
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
                menuid = "30";
            }
            models = new List<API.View_Pro_SellAduit>();
            GridAuitList.ItemsSource = models;
            page.PageSize = (int)pagesize.Value;

            hAdder = new ROHallAdder(ref this.hallid, int.Parse(menuid));
            //this.fromDate.SelectedValue = DateTime.Now.Date;

            List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
            };
            this.ckb.ItemsSource = list;
            this.ckbPassed.ItemsSource = list;
            this.ckbUsed.ItemsSource = list;
            this.ckb.SelectedIndex = 1;
            this.ckbPassed.SelectedIndex = 2;
            this.ckbUsed.SelectedIndex = 2;

            Search();
        }

        void SellAduit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

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
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批单复制成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,ex.Message + "\n请在页面中点击右键选择 Silverlight(S) , 在权\n限选项中将剪切板删除或者将其权限改为允许");
            }

        }

        /// <summary>
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

        #region "审批"

        /// <summary>
        /// 批量审批通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void batchAduitPassed_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BatchAduit(true);
        }

        /// <summary>
        /// 批量审批不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BatchAduitNoPassed_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BatchAduit(false);
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="flag">通过与否</param>
        private void BatchAduit(bool flag)
        {
            if (GridAuitList.SelectedItems.Count == 0 || models.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项");
                return;
            }
            foreach (var item in GridAuitList.SelectedItems)
            {
                API.View_Pro_SellAduit aduit = item as API.View_Pro_SellAduit;

                if (aduit.HasAduited3 == true)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,aduit.AduitID + " 该审批单已审批");
                    return;
                }
            }

            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定审批吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            List<API.Pro_SellAduit> mList = new List<API.Pro_SellAduit>();
            API.Pro_SellAduit ba = null;

            foreach (var item in GridAuitList.SelectedItems)
            {
                API.View_Pro_SellAduit vps = item as API.View_Pro_SellAduit;
               
                ba = new API.Pro_SellAduit();
                ba.ID = vps.ID;
                ba.AduitDate3 = DateTime.Now;
                ba.AduitUser3 = Store.LoginUserInfo.UserID;
                ba.Aduited3 = true;
                ba.Note3 = note3.Text;
                ba.Passed3 = flag;
                mList.Add(ba);
                
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 218, new object[] { mList ,true}, new EventHandler<API.MainCompletedEventArgs>(AduitCompeleted));

        }

        /// <summary>
        /// 审批完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AduitCompeleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "审批失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            note3.Text = string.Empty;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批成功");
                Logger.Log("审批成功");

                Search();
                //List<int> listID = e.Result.Obj as List<int>;
                //foreach (var child in listID)
                //{
                //    foreach (var item in models)
                //    {
                //        if (child == item.ID)
                //        {
                //            models.Remove(item);
                //            break;
                //        }
                //    }
                //}

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                Logger.Log("审批失败");
            }
        }

        /// <summary>
        /// 删除申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GridAuitList.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的记录！");
                return;
            }
            List<int> list = new List<int>();
            foreach (var item in GridAuitList.SelectedItems)
            {
                API.View_Pro_SellAduit aduit = item as API.View_Pro_SellAduit;
                list.Add(aduit.ID);
            }
            if (MessageBox.Show("确定删除该申请记录吗？", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }
            
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 212, new object []{list},new EventHandler<API.MainCompletedEventArgs>(DeleteCompleted));
        }

        private void DeleteCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
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

        void AduitPassed_Click(object sender, RoutedEventArgs e)
        {
            SingleAduit(true);
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="flag">通过与否</param>
        private void SingleAduit(bool flag)
        {
            if (GridAuitList.SelectedItem == null || models.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项");
                return;
            }
            API.View_Pro_SellAduit aduit = GridAuitList.SelectedItem as API.View_Pro_SellAduit;

            if (aduit.HasAduited3== true)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,aduit.AduitID + " 该审批单已审批");
                return;
            }

            List<API.AduitListInfo> list = new List<API.AduitListInfo>();
            //if (GridDetail.ItemsSource != null)
            //{
            //     list = GridDetail.ItemsSource as List<API.AduitListInfo>;

            //    foreach (var item in list)
            //    {
            //        if (item.NewPrice < item.MinPrice)
            //        {
            //            MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品 " + item.ProName + " 审批单价不能小于最低价");
            //            return;
            //        }
            //        if (item.NewPrice > item.MaxPrice)
            //        {
            //            MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品 " + item.ProName + " 审批单价不能大于最高价");
            //            return;
            //        }
            //    }
            //}
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定审批吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            List<API.Pro_SellAduit> mList = new List<API.Pro_SellAduit>();
            API.Pro_SellAduit ba = null;
            API.View_Pro_SellAduit vps = aduit as API.View_Pro_SellAduit;
            ba = new API.Pro_SellAduit();
            ba.ID = vps.ID;
            ba.AduitDate3 = DateTime.Now;
            ba.AduitUser3 = Store.LoginUserInfo.UserID;
            ba.Aduited3 = true;
            ba.Note3 = note3.Text;
            ba.Passed3 = flag;
            ba.Pro_SellAduitList = new List<API.Pro_SellAduitList>();

            if (flag)  
            {
                foreach (var item in list)
                {
                    API.Pro_SellAduitList ps = new API.Pro_SellAduitList();
                    ps.ID = item.ID;
                    ps.ProID = item.ProID;
                    ps.ProPrice = item.ProPrice;
                    //ps.OffMoney = item.ProPrice - item.NewPrice;
                    ba.Pro_SellAduitList.Add(ps);
                }
            }
            mList.Add(ba);
            
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 218, new object[] { mList ,false}, new EventHandler<API.MainCompletedEventArgs>(AduitCompeleted));
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
            this.GridDetail.ItemsSource = null;
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
            if (!string.IsNullOrEmpty(this.aduitid.Text.Trim()))
            {
                API.ReportSqlParams_String aduitID = new API.ReportSqlParams_String();
                aduitID.ParamName = "AduitID";
                aduitID.ParamValues = this.aduitid.Text.Trim();
                rpp.ParamList.Add(aduitID);
            }
            if (this.ckb.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool aduit = new API.ReportSqlParams_Bool();
                aduit.ParamName = "Aduited3";
                CkbModel cb = this.ckb.SelectedItem as CkbModel;
                aduit.ParamValues = cb.Flag;
                rpp.ParamList.Add(aduit);
            }

            if (this.ckbPassed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool passed = new API.ReportSqlParams_Bool();
                passed.ParamName = "Passed3";
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

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 220, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
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
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;

            GridAuitList.Rebind();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            } 
            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_Pro_SellAduit> aduitList = pageParam.Obj as List<API.View_Pro_SellAduit>;
                if (aduitList.Count != 0)
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

            this.AduitNoPassed.IsEnabled = true;
            this.batchAduitPassed.IsEnabled = true;
            if (this.ckb.SelectedIndex == 0)
            {
                this.AduitNoPassed.IsEnabled = false;
                this.batchAduitPassed.IsEnabled = false;
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

        #region 详情

        private void GridAuitList_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            if (GridAuitList.SelectedItem == null)
            {
                return;
            }
            API.View_Pro_SellAduit vp = GridAuitList.SelectedItem as API.View_Pro_SellAduit;

            this.aduitID.Text = vp.AduitID;
            this.HallID.Text = vp.HallName;
            this.applyDate.Text = vp.ApplyDate;
            this.cusname.Text = vp.CustName;
            this.cusPhone.Text = vp.CustPhone;
            applyer.Text = vp.ApplyUser;
            note.Text = vp.Note;
            note1.Text = vp.Note1;
            note2.Text = vp.Note2;
            if (vp.HasAduited3 == true)
            {
                btnP.IsEnabled = false;
                GridDetail.IsReadOnly = true;
            }
            else
            {
                btnP.IsEnabled = true;
                GridDetail.IsReadOnly = false;
            }
            if (vp.HasUsed==true)
            {
                this.btnDelete.IsEnabled = false;
            }
            else
            {
                this.btnDelete.IsEnabled = true;

            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 47, new object[] { vp.ID }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            GridDetail.ItemsSource = null;
            GridDetail.Rebind();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.AduitListInfo> list = e.Result.Obj as List<API.AduitListInfo>;
                GridDetail.ItemsSource = list; 
                GridDetail.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
        }

        #endregion

        private void GridDetail_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            List<API.AduitListInfo> list = GridDetail.ItemsSource as List<API.AduitListInfo>;

            //foreach (var item in list)
            //{
            //    if (item.NewPrice < 0)
            //    {
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow,"数量不能为负数！");
            //        item.NewPrice = item.ProPrice;
            //        return;
            //    }
            //    if (item.NewPrice>item.MaxPrice || item.NewPrice <item.MinPrice)
            //    {
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow,"请确保审批单价在有效范围内！");
            //        item.NewPrice = item.ProPrice;
            //        return;
            //    }
            //    //if (!item.IsDecimal)
            //    //{
            //    //    item.NewPrice = (int)(Decimal.Truncate(Convert.ToDecimal(item.NewPrice * 100)) / 100);
            //    //    continue;
            //    //}

            //    item.NewPrice = Decimal.Truncate(Convert.ToDecimal(item.NewPrice * 100)) / 100;
            //}
        }


    }
}
