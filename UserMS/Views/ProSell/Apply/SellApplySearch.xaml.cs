using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SlModel;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Apply
{
    /// <summary>
    /// SellApplySearch.xaml 的交互逻辑
    /// </summary>
    public partial class SellApplySearch : Page
    {
        private bool flag = false;
        private int pageindex = 0;
        ROHallAdder hadder =null;

        public SellApplySearch()
        {
            InitializeComponent();
            flag = true;
            this.SizeChanged += SellApply_SizeChanged;
            hadder = new ROHallAdder(ref hallid,196);
            //fromDate.DateTimeText = DateTime.Now.ToShortDateString();
            List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
            };
            //this.ckb.ItemsSource = list;
            this.ckbPassed.ItemsSource = list;
            this.ckbUsed.ItemsSource = list;
            //this.ckbPassed1.ItemsSource = list;
            //this.ckbAduited1.ItemsSource = list;
            //this.ckbPassed2.ItemsSource = list;
            //this.ckbAduited2.ItemsSource = list;
            //this.ckbPassed3.ItemsSource = list;
            //this.ckbAduited3.ItemsSource = list;

            //this.ckbPassed1.SelectedIndex = 2;
            //this.ckbAduited1.SelectedIndex = 2;
            //this.ckbPassed2.SelectedIndex = 2;
            //this.ckbAduited2.SelectedIndex = 2;
            //this.ckbPassed3.SelectedIndex = 2;
            //this.ckbAduited3.SelectedIndex = 2;

            //this.ckb.SelectedIndex = 2;
            this.ckbPassed.SelectedIndex = 0;
            this.ckbUsed.SelectedIndex = 1;

            Search();
        }

        void SellApply_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
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
            aduitID.Text = string.Empty;
            this.GridAuitList.ItemsSource = null;
            GridAuitList.Rebind();

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
                API.ReportSqlParams_String aduitID2 = new API.ReportSqlParams_String();
                aduitID2.ParamName = "AduitID";
                aduitID2.ParamValues = this.aduitid.Text.Trim();
                rpp.ParamList.Add(aduitID2);
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

            //if (this.ckbAduited1.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool aduit1 = new API.ReportSqlParams_Bool();
            //    aduit1.ParamName = "Aduited1";
            //    CkbModel cb = this.ckbAduited1.SelectedItem as CkbModel;
            //    aduit1.ParamValues = cb.Flag;
            //    rpp.ParamList.Add(aduit1);
            //}

            //if (this.ckbPassed1.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool passed1 = new API.ReportSqlParams_Bool();
            //    passed1.ParamName = "Passed1";
            //    CkbModel cb1 = this.ckbPassed1.SelectedItem as CkbModel;
            //    passed1.ParamValues = cb1.Flag;
            //    rpp.ParamList.Add(passed1);
            //}

            //if (this.ckbAduited2.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool aduit2 = new API.ReportSqlParams_Bool();
            //    aduit2.ParamName = "Aduited2";
            //    CkbModel cb = this.ckbAduited2.SelectedItem as CkbModel;
            //    aduit2.ParamValues = cb.Flag;
            //    rpp.ParamList.Add(aduit2);
            //}

            //if (this.ckbPassed2.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool passed2 = new API.ReportSqlParams_Bool();
            //    passed2.ParamName = "Passed2";
            //    CkbModel cb1 = this.ckbPassed2.SelectedItem as CkbModel;
            //    passed2.ParamValues = cb1.Flag;
            //    rpp.ParamList.Add(passed2);
            //}


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
            //    API.ReportSqlParams_Bool passed3 = new API.ReportSqlParams_Bool();
            //    passed3.ParamName = "Passed3";
            //    CkbModel cb1 = this.ckbPassed3.SelectedItem as CkbModel;
            //    passed3.ParamValues = cb1.Flag;
            //    rpp.ParamList.Add(passed3);
            //}



            if (this.ckbUsed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool used = new API.ReportSqlParams_Bool();
                used.ParamName = "Used";
                CkbModel cb2 = this.ckbUsed.SelectedItem as CkbModel;
                if (cb2 != null) used.ParamValues = cb2.Flag;
                else used.ParamValues = false;
                rpp.ParamList.Add(used);
            }
            if (!string.IsNullOrEmpty(this.applyUser.Text.ToString()))
            {
                API.ReportSqlParams_String aduitUser = new API.ReportSqlParams_String();
                aduitUser.ParamName = "ApplyUser";
                aduitUser.ParamValues = this.applyUser.Text;
                rpp.ParamList.Add(aduitUser);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 269, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
        }

        /// <summary>
        /// 查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            GridAuitList.ItemsSource = null;
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

                List<API.View_Pro_SellAduit> aduitList = pageParam.Obj as List<API.View_Pro_SellAduit>;
                if (aduitList.Count != 0)
                {
                    GridAuitList.ItemsSource = aduitList;
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

        #region  详情

        private void GridAuitList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridAuitList.SelectedItem == null)
            {
                return;
            }
            API.View_Pro_SellAduit vp = GridAuitList.SelectedItem as API.View_Pro_SellAduit;
            aduitID.Text = vp.AduitID;
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
                API.View_Pro_SellAduit vp = item as API.View_Pro_SellAduit;
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
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 212, new object[] { list }, new EventHandler<API.MainCompletedEventArgs>(DelCompleted));
        }

        private void DelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                GridDetail.ItemsSource = null;
                GridDetail.Rebind();
                Search();
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(this.aduitID.Text);
        }
    }
}
