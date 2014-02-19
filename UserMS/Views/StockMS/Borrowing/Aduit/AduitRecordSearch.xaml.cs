using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SlModel;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.Borrowing
{
    public partial class AduitRecordSearch : Page
    {
        private List<API.View_BorowAduit> models = null;
        private ROHallAdder hAdder;
        private List<API.GetBorowAduitInfoByAIDResult> details = null;
        private int pageindex;
        private string menuid = "";
        private bool flag = false;


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
                if (menuid == null)
                {
                    menuid = "98";
                }
            }
            catch
            {
                menuid = "98";
            }
            finally
            {
                details = new List<API.GetBorowAduitInfoByAIDResult>();
                models = new List<API.View_BorowAduit>();
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
               // this.ckb.ItemsSource = list;
                this.ckbPassed.ItemsSource = list;
                this.ckbUsed.ItemsSource = list;
                //this.ckb.SelectedIndex = 2;
                this.ckbPassed.SelectedIndex = 2;
                this.ckbUsed.SelectedIndex = 2;

                //this.ckbAduited2.ItemsSource = list;
                //this.ckbPassed2.ItemsSource = list;
                //this.ckbAduited2.SelectedIndex = 2;
                //this.ckbPassed2.SelectedIndex = 2;

                //this.ckbAduited3.ItemsSource = list;
                //this.ckbPassed3.ItemsSource = list;
                //this.ckbAduited3.SelectedIndex = 2;
                //this.ckbPassed3.SelectedIndex = 2;
                flag = true;
                Search();

                this.KeyDown += BorowAduit_KeyDown;
                this.applyUser.KeyDown += BorowAduit_KeyDown;
                this.hallid.KeyDown += BorowAduit_KeyDown;
                this.fromDate.KeyDown += BorowAduit_KeyDown;
                this.toDate.KeyDown += BorowAduit_KeyDown;
                this.search.Click += search_Click;
              
            }
        }

        void AduitRecordSearch_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        public AduitRecordSearch()
        {
            InitializeComponent();
            this.SizeChanged += AduitRecordSearch_SizeChanged;
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
            rpp.PageIndex = page.PageIndex;
            rpp.PageSize =(int) pagesize.Value;
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
            if (!string.IsNullOrEmpty(this.aduitid.Text))
            {
                API.ReportSqlParams_String aid = new API.ReportSqlParams_String();
                aid.ParamName = "AduitID";
                aid.ParamValues = this.aduitid.Text;
                rpp.ParamList.Add(aid);
            }

            //if (this.ckb.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool aduit = new API.ReportSqlParams_Bool();
            //    aduit.ParamName = "Aduited";
            //    CkbModel cb = this.ckb.SelectedItem as CkbModel;
            //    aduit.ParamValues = cb.Flag;
            //    rpp.ParamList.Add(aduit);
            //}

            if (this.ckbPassed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool passed = new API.ReportSqlParams_Bool();
                passed.ParamName = "Passed";
                CkbModel cb1 = this.ckbPassed.SelectedItem as CkbModel;
                passed.ParamValues = cb1.Flag;
                rpp.ParamList.Add(passed);
            }
            //////////////////// 二级  ////////////////////
            //if (this.ckbAduited2.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool aduit = new API.ReportSqlParams_Bool();
            //    aduit.ParamName = "Aduited2";
            //    CkbModel cb = this.ckbAduited2.SelectedItem as CkbModel;
            //    aduit.ParamValues = cb.Flag;
            //    rpp.ParamList.Add(aduit);
            //}

            //if (this.ckbPassed2.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool passed = new API.ReportSqlParams_Bool();
            //    passed.ParamName = "Passed2";
            //    CkbModel cb1 = this.ckbPassed2.SelectedItem as CkbModel;
            //    passed.ParamValues = cb1.Flag;
            //    rpp.ParamList.Add(passed);
            //}
            ////////////////////// 二级  ////////////////////
            //if (this.ckbAduited3.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool aduit3 = new API.ReportSqlParams_Bool();
            //    aduit3.ParamName = "Aduited3";
            //    CkbModel cb = this.ckbAduited3.SelectedItem as CkbModel;
            //    aduit3.ParamValues = cb.Flag;
            //    rpp.ParamList.Add(aduit3);
            //}

            //if (this.ckbPassed3.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool passed = new API.ReportSqlParams_Bool();
            //    passed.ParamName = "Passed3";
            //    CkbModel cb1 = this.ckbPassed3.SelectedItem as CkbModel;
            //    passed.ParamValues = cb1.Flag;
            //    rpp.ParamList.Add(passed);
            //}

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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 268, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
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
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;

            GridAuitList.Rebind();

            if (e.Error!=null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_BorowAduit> aduitList = pageParam.Obj as List<API.View_BorowAduit>;

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
        }

        #endregion

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
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批单复制成功");
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

        private void GridAuitList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridAuitList.SelectedItem == null)
            {
                return;
            }
            API.View_BorowAduit ai = GridAuitList.SelectedItem as API.View_BorowAduit;

            this.aduitID.Text = ai.AduitID;
            this.applyDate.Text = ai.ApplyDate;
            this.HallID.Text = ai.HallName;

            this.note.Text = ai.Note;
            this.note1.Text = ai.Note1;
            this.note2.Text = ai.Note2;
            this.note3.Text = ai.Note3;
            mbphone.Text = ai.MobilPhone;
            estimateDate.Text = ((DateTime)ai.EstimateReturnTime).ToShortDateString();
            this.borrower.Text = string.IsNullOrEmpty(ai.Borrower) ? "" : ai.Borrower;
            this.borowType.Text = string.IsNullOrEmpty(ai.BorrowType) ? "" : ai.BorrowType;
            this.dept.Text = string.IsNullOrEmpty(ai.Dept) ? "" : ai.Dept;
     
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 36, new object[] { ai.AduitID },
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            this.isbusy.IsBusy = false;
            details.Clear();
            if (e.Result.ReturnValue)
            {
                #region 获取申请详情

                List<API.GetBorowAduitInfoByAIDResult> list = e.Result.Obj as List<API.GetBorowAduitInfoByAIDResult>;
                details.AddRange(list);  
                this.GridApplyList.Rebind();

                #endregion

                #region 历史借贷记录

                this.creadit.Text = Math.Round(Convert.ToDouble(e.Result.ArrList[1]), 2).ToString();
                List<API.View_UserBorowInfo> blist = e.Result.ArrList[0] as List<API.View_UserBorowInfo>;
                GridUnReturn.ItemsSource = blist; 
                GridUnReturn.Rebind();

                #endregion
            }

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

        /// <summary>
        /// 删除申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delChecked_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridAuitList.SelectedItems == null)
            {
                MessageBox.Show("请选择要删除的申请单！");
                return;
            }
            if (MessageBox.Show("确定删除选中项吗？") == MessageBoxResult.Cancel)
            {
                return;
            }
            List<int> arr = new List<int>();
            foreach(var item in GridAuitList.SelectedItems)
            {
                API.View_BorowAduit mod = item as API.View_BorowAduit;
                if (mod.Used1 == true)
                {
                    MessageBox.Show("审批单 " + mod.AduitID + " 已使用，无法删除！");
                    return;
                }
                arr.Add(mod.ID);
            }
        

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 214, new object[] { arr },new EventHandler<API.MainCompletedEventArgs>(DeleteCompleted));
        }

        private void DeleteCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
             details.Clear();
             MessageBox.Show(e.Result.Message);
             if (e.Result.ReturnValue)
             {
                 GridUnReturn.ItemsSource = null;
                 GridUnReturn.Rebind();
                 Search();
             }
        }

    }
}
