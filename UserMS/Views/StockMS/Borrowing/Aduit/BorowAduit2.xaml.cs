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

namespace UserMS.Views.StockMS.Borrowing.Aduit
{
    /// <summary>
    /// 二级审批--机核
    /// </summary>
    public partial class BorowAduit2 : Page
    {
        public BorowAduit2()
        {
            InitializeComponent();
            this.SizeChanged += BorowAduit_SizeChanged;
        }

        private List<API.View_BorowAduit2> models = null;
        private List<API.GetBorowAduitInfoByAIDResult> details = null;

        private ROHallAdder hAdder;
        private bool flag =false;
        private string menuid = "";
        private int pageindex;
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "204";
            }
            finally
            {
                if (menuid == null)
                {
                    menuid = "204";
                }
                models = new List<API.View_BorowAduit2>();
                details = new List<API.GetBorowAduitInfoByAIDResult>();
                GridAuitList.ItemsSource = models;
                GridApplyList.ItemsSource = details;
                page.PageSize = (int)pagesize.Value;

                //  hAdder = new HallFilter(ref this.hallid);
                hAdder = new ROHallAdder(ref this.hallid, int.Parse(menuid));
                

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
                flag = true;
                Search();

                this.KeyDown += BorowAduit_KeyDown;
                this.ckb.KeyDown += BorowAduit_KeyDown;
                this.applyUser.KeyDown += BorowAduit_KeyDown;
                this.hallid.KeyDown += BorowAduit_KeyDown;
                this.fromDate.KeyDown += BorowAduit_KeyDown;
                this.toDate.KeyDown += BorowAduit_KeyDown;
                this.AduitNoPassed.Click += AduitNoPassed_Click;
                this.batchAduitPassed.Click += batchAduitPassed_Click;
              
            }
        }

        void BorowAduit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        #region "审批"

        /// <summary>
        /// 批量审批通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void batchAduitPassed_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
           
            if (GridAuitList.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需审批的数据！");
                return;
            }
            //if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定审批您当前所选中的数据吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            //{
            //    return;
            //}
            Aduit(true);
        }

        /// <summary>
        /// 删除申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GridAuitList.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的记录！");
                return;
            }
            if (MessageBox.Show("确定删除该申请记录吗？", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }
            API.View_BorowAduit2 vb = GridAuitList.SelectedItem as API.View_BorowAduit2;

            Clear();

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 214, new object[] { vb.ID }, new EventHandler<API.MainCompletedEventArgs>(DeleteCompleted));
        }

        private void DeleteCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "删除失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            this.isbusy.IsBusy = false;
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

        private void aduitPassed_Click(object sender, RoutedEventArgs e)
        {
            if (GridAuitList.SelectedItem==null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要审批的数据！");
                return;
            }
            API.View_BorowAduit2 aduit = GridAuitList.SelectedItem as API.View_BorowAduit2;

            if (aduit.HasAduited2 == true)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,aduit.AduitID + " 该审批单已审批");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定审批吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            List<API.Pro_BorowAduit> mList = new List<API.Pro_BorowAduit>();
            API.Pro_BorowAduit ba = null;

            ba = new API.Pro_BorowAduit();
            ba.ID = aduit.ID;
            ba.AduitDate2 = DateTime.Now;
            ba.AduitUser2 = Store.LoginUserInfo.UserID;
            ba.Aduited2 = true;
            ba.Note2 = note2.Text;
            ba.Passed2 = true;
            mList.Add(ba);
            
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 207, new object[] { mList }, new EventHandler<API.MainCompletedEventArgs>(AduitCompeleted));

        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="flag"></param>
        private void Aduit(bool flag)
        {
            if (GridAuitList.SelectedItems.Count == 0 || models.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项");
                return;
            }
            foreach (var item in GridAuitList.SelectedItems)
            {
                API.View_BorowAduit2 aduit = item as API.View_BorowAduit2;

                if (aduit.HasAduited2==true)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,aduit.AduitID + " 该审批单已审批");
                    return;
                }
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定审批您当前所选中的数据吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            List<API.Pro_BorowAduit> mList = new List<API.Pro_BorowAduit>();
            API.Pro_BorowAduit ba = null;

            foreach (var item in GridAuitList.SelectedItems)
            {
                API.View_BorowAduit2 vb = item as API.View_BorowAduit2;
                ba = new API.Pro_BorowAduit();
                ba.ID = vb.ID;
                ba.AduitDate2 = DateTime.Now;
                ba.AduitUser2 = Store.LoginUserInfo.UserID;
                ba.Aduited2 = true;
               // ba.Note = vb.Note;
                ba.Passed2 = flag;
                mList.Add(ba);
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 207, new object[] { mList }, new EventHandler<API.MainCompletedEventArgs>(AduitCompeleted));

        }

        /// <summary>
        /// 审批完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AduitCompeleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "审批失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            note2.Text = string.Empty;
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批成功");
                Logger.Log("审批成功");

                //Clear();
                Search();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                Logger.Log(e.Result.Message);
            }
        }

        /// <summary>
        /// 批量审批不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AduitNoPassed_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Aduit(false);
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
                aduit.ParamName = "Aduited2";
                CkbModel cb = this.ckb.SelectedItem as CkbModel;
                aduit.ParamValues = cb.Flag;
                rpp.ParamList.Add(aduit);
            }

            if (this.ckbPassed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool passed = new API.ReportSqlParams_Bool();
                passed.ParamName = "Passed2";
                CkbModel cb1 = this.ckbPassed.SelectedItem as CkbModel;
                passed.ParamValues = cb1.Flag;
                rpp.ParamList.Add(passed);
            }
            if (!string.IsNullOrEmpty(this.aduitid.Text))
            {
                API.ReportSqlParams_String aid = new API.ReportSqlParams_String();
                aid.ParamName = "AduitID";
                aid.ParamValues = this.aduitid.Text;
                rpp.ParamList.Add(aid);
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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 209, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
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
            if (this.ckb.SelectedIndex == 0)
            {
                this.AduitNoPassed.IsEnabled = false;
                this.batchAduitPassed.IsEnabled = false;
            }
           
            if (this.ckb.SelectedIndex ==1)
            {
                this.AduitNoPassed.IsEnabled = true;
                this.batchAduitPassed.IsEnabled = true;
                GridAuitList.Columns[GridAuitList.Columns.Count - 1].IsVisible = false;
            }
            else
            {
                GridAuitList.Columns[GridAuitList.Columns.Count - 1].IsVisible = true;
            }

            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                if (pageParam == null) { return; }
                List<API.View_BorowAduit2> aduitList = pageParam.Obj as List<API.View_BorowAduit2>;
                if (aduitList != null)
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
        }
     
        #endregion 

        #region "详情'

        /// <summary>
        /// 选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GridAuitList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridAuitList.SelectedItem == null)
            {
                return;
            }
            API.View_BorowAduit2 ai = GridAuitList.SelectedItem as  API.View_BorowAduit2;

            if (ai.HasUsed==true)
            {
                this.btnDelete.IsEnabled = false;
            }
            else
            {
                this.btnDelete.IsEnabled = true;
            }
            this.aduitID.Text = ai.AduitID;
            this.applyDate.Text = ai.ApplyDate;
            this.HallID.Text = ai.HallName;
            this.note1.Text = ai.Note1;
            mbphone.Text = ai.MobilPhone;
            estimateDate.Text = ((DateTime)ai.EstimateReturnTime).ToShortDateString();
            this.borrower.Text = string.IsNullOrEmpty(ai.Borrower) ? "" : ai.Borrower;
            this.borowType.Text = string.IsNullOrEmpty(ai.BorrowType) ? "" : ai.BorrowType;
            this.dept.Text = string.IsNullOrEmpty(ai.Dept) ? "" : ai.Dept;
            this.note.Text = ai.Note;
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 36, new object[] { ai.AduitID }, 
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            details.Clear();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                #region 获取申请详情

                List<API.GetBorowAduitInfoByAIDResult> list = e.Result.Obj as List<API.GetBorowAduitInfoByAIDResult>;
                details.AddRange(list);
                this.GridApplyList.Rebind();

                #endregion

                #region 历史借贷记录

                this.creadit.Text = Math.Round(Convert.ToDouble(e.Result.ArrList[1]),2).ToString();
                List<API.View_UserBorowInfo> blist = e.Result.ArrList[0] as List<API.View_UserBorowInfo>;
                GridUnReturn.ItemsSource = blist;
                GridUnReturn.Rebind();

                #endregion 
            }

        }

        #endregion 

        #region  事件

        void BorowAduit_KeyDown(object sender, KeyEventArgs e)
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
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"复制成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,ex.Message + "\n请在页面中点击右键选择 Silverlight(S) , 在权\n限选项中将剪切板删除或者将其权限改为允许");
            }
         
        }

        void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            Search();
        }

        private void Clear()
        {
            this.aduitID.Text = string.Empty;
            this.applyDate.Text = string.Empty;
            this.HallID.Text = string.Empty;
            this.borrower.Text = string.Empty;
            this.borowType.Text = string.Empty;
            this.dept.Text = string.Empty;
            this.creadit.Text = string.Empty;
            this.note.Text = string.Empty;
            details.Clear();
            GridApplyList.Rebind();
            GridUnReturn.ItemsSource = null;
            GridUnReturn.Rebind();

            models.Clear();
            GridAuitList.Rebind();
           
        }

        #endregion 

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

    }
}
