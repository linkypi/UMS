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

namespace UserMS.Views.HallOff
{
    public partial class SellOffAduit : BasePage
    {
        string menuid = string.Empty;
        List<API.View_SellOffAduitInfo> models = new List<API.View_SellOffAduitInfo>();

        ROHallAdder hAdder = null;
        int pageindex;
        bool flag;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "217";
            }
            finally
            {
                models = new List<API.View_SellOffAduitInfo>();
        
                GridAuitList.ItemsSource = models;
                page.PageSize = (int)pagesize.Value;

           
                hAdder = new ROHallAdder(ref this.hallid, int.Parse(menuid));
              //  this.fromDate.SelectedValue = DateTime.Now.Date;

                //List<CkbModel> list = new List<CkbModel>() { 
                //new  CkbModel(true,"是"),
                //new  CkbModel(false,"否"),
                //new  CkbModel(false,"全部"),
                // };
                //this.ckb.ItemsSource = list;
                //this.ckbPassed.ItemsSource = list;
                //this.ckbUsed.ItemsSource = list;
                //this.ckb.SelectedIndex = 1;
                //this.ckbPassed.SelectedIndex = 2;
                //this.ckbUsed.SelectedIndex = 2;
                flag = true;

                Search();

            }
        }

        void SellOffAduit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DockPanel wp = this.FindName("panel") as DockPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        public SellOffAduit()
        {
            InitializeComponent(); 
            this.SizeChanged+=SellOffAduit_SizeChanged;
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

            //if (this.ckb.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool aduit = new API.ReportSqlParams_Bool();
            //    aduit.ParamName = "Aduited";
            //    CkbModel cb = this.ckb.SelectedItem as CkbModel;
            //    aduit.ParamValues = cb.Flag;
            //    rpp.ParamList.Add(aduit);
            //}

            //if (this.ckbPassed.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool passed = new API.ReportSqlParams_Bool();
            //    passed.ParamName = "Passed";
            //    CkbModel cb1 = this.ckbPassed.SelectedItem as CkbModel;
            //    passed.ParamValues = cb1.Flag;
            //    rpp.ParamList.Add(passed);
            //}

            //if (this.ckbUsed.SelectedIndex != 2)
            //{
            //    API.ReportSqlParams_Bool used = new API.ReportSqlParams_Bool();
            //    used.ParamName = "Used";
            //    CkbModel cb2 = this.ckbUsed.SelectedItem as CkbModel;
            //    used.ParamValues = cb2.Flag;
            //    rpp.ParamList.Add(used);
            //}

            if (!string.IsNullOrEmpty(this.applyUser.Text.ToString()))
            {
                API.ReportSqlParams_String aduitUser = new API.ReportSqlParams_String();
                aduitUser.ParamName = "ApplyUser";
                aduitUser.ParamValues = this.applyUser.Text;
                rpp.ParamList.Add(aduitUser);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 229, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
        }

        /// <summary>
        /// 查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            this.isbusy.IsBusy = false;
            models.Clear();
            GridAuitList.Rebind();
            //if (this.ckb.SelectedIndex == 0)
            //{
            //    this.AduitNoPassed.IsEnabled = false;
            //    this.batchAduitPassed.IsEnabled = false;
            //}

            //if (this.ckb.SelectedIndex == 1)
            //{
            //    this.AduitNoPassed.IsEnabled = true;
            //    this.batchAduitPassed.IsEnabled = true;
            //    GridAuitList.Columns[GridAuitList.Columns.Count - 1].IsVisible = false;
            //}
            //else
            //{
            //    GridAuitList.Columns[GridAuitList.Columns.Count - 1].IsVisible = true;
            //}

            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_SellOffAduitInfo> aduitList = pageParam.Obj as List<API.View_SellOffAduitInfo>;
                if (aduitList == null) { return; }
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
            API.View_SellOffAduitInfo ai = GridAuitList.SelectedItem as API.View_SellOffAduitInfo;
        
            this.applyDate.Text = ai.ApplyDate;
            this.HallName.Text = ai.HallName;
            this.note.Text = ai.ApplyNote;
            bool isSellID = false;
            int id;
            if (Convert.ToInt32(ai.SellID) != 0)
            {
                isSellID = true;
                id = Convert.ToInt32(ai.SellID);
            }
            else
            {
                id = Convert.ToInt32(ai.BackID);
            }
            if (ai.IsAduited == "Y")
            {
                GridAuitList.Columns[9].IsReadOnly = true;
            }
            else
            {
                GridAuitList.Columns[9].IsReadOnly = false;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 230, new object[] { ai.ID, isSellID, id },
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            GridApplyList.ItemsSource = null;
            GridApplyList.Rebind();
            GridAduitDetail.ItemsSource = null;
            GridAduitDetail.Rebind();

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            this.isbusy.IsBusy = false;
          
            if (e.Result.ReturnValue)
            {
                #region 获取申请详情

                List<API.View_SellOffAduitProList> list = e.Result.ArrList[1] as List<API.View_SellOffAduitProList>;
                GridApplyList.ItemsSource = list;
                GridApplyList.Rebind();
         
                #endregion

                #region 审批记录

                List<API.View_SellOffAduitInfoList> blist = e.Result.ArrList[0] as List<API.View_SellOffAduitInfoList>;
                GridAduitDetail.ItemsSource = blist;
                GridAduitDetail.Rebind();
                #endregion
            }

        }

        #endregion 

        private void Clear()
        {
            this.applyDate.Text = string.Empty;
            this.HallName.Text = string.Empty;
            this.note.Text = string.Empty;

            GridApplyList.ItemsSource = null;
            GridApplyList.Rebind();
            GridAduitDetail.ItemsSource = null;
            GridAduitDetail.Rebind();

            models.Clear();
            GridAuitList.Rebind();

        }


        void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            Search();
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        #region  审批

        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aduitPassed_Click(object sender, RoutedEventArgs e)
        {
            if (GridAuitList.SelectedItem == null)
            {
                MessageBox.Show("请选择要审批的记录！");
                return;
            }
            if (MessageBox.Show("确定审批吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            API.View_SellOffAduitInfo ai = GridAuitList.SelectedItem as API.View_SellOffAduitInfo;

            API.Pro_SellOffAduitInfo psa = new API.Pro_SellOffAduitInfo();
            psa.ID = ai.ID;
            psa.IsAduited = true;
            psa.HallID = ai.HallID;
            psa.SellID = ai.SellID;
            psa.NextPrice = ai.NextPrice;
            psa.AduitNote = ai.AduitNote;

            API.Pro_SellOffAduitInfoList sellofflist = new API.Pro_SellOffAduitInfoList();
            sellofflist.AduitDate = DateTime.Now;
            sellofflist.AduitID = ai.ID;
            sellofflist.IsPass = true;
            sellofflist.Note = ai.AduitNote;
            sellofflist.UserID = Store.LoginUserInfo.UserID;

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 231, new object[] { sellofflist, psa },
            new EventHandler<API.MainCompletedEventArgs>(AduitCompleted));
        }


        private void AduitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }

            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                Search();
                Logger.Log("审批成功！");
            }
            else 
            {
                MessageBox.Show(e.Result.Message);
                Logger.Log("审批失败！");
            }
        }

        /// <summary>
        /// 审批不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnPassed_Click(object sender, RoutedEventArgs e)
        {
            if (GridAuitList.SelectedItem == null)
            {
                MessageBox.Show("请选择要审批的记录！");
                return;
            }
            if (MessageBox.Show("确定审批吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            API.View_SellOffAduitInfo ai = GridAuitList.SelectedItem as API.View_SellOffAduitInfo;

            API.Pro_SellOffAduitInfoList sellofflist = new API.Pro_SellOffAduitInfoList();
            sellofflist.AduitDate = DateTime.Now;
            sellofflist.AduitID = ai.ID;
            sellofflist.IsPass = false;
            sellofflist.Note = ai.AduitNote;
            sellofflist.UserID = Store.LoginUserInfo.UserID;
            string note = string.IsNullOrEmpty(ai.AduitNote) ? "" : ai.AduitNote;
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 242, new object[] {sellofflist, ai.ID,note},
            new EventHandler<API.MainCompletedEventArgs>(AduitCompleted));
        }

        #endregion 
    }

}
 