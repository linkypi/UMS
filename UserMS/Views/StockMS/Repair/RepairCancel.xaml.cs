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

namespace UserMS.Views.StockMS.Repair
{
    public partial class RepairCancel : Page
    {
        private bool flag = false;
        private List<API.View_RepairInfo> models = null;
        List<API.CancelListModel> detailModels = null;
         private ROHallAdder hall = null;
        private int currentRID;
        private int pageindex;

        private string menuid = "";
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "90";
            }
            finally
            {
                if (menuid == null)
                {
                    menuid = "90";
                }
                models = new List<API.View_RepairInfo>();
                this.GridReturnList.ItemsSource = models;
                detailModels = new List<API.CancelListModel>();
                this.GridBorowDetail.ItemsSource = detailModels;

                hall = new ROHallAdder(ref this.hallName, 90);

                List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
                    };
                this.ckbReturn.ItemsSource = list;
                this.ckbReceive.ItemsSource = list;
                this.ckbReceive.SelectedIndex = 1;
                this.ckbReturn.SelectedIndex = 1;
                this.fromDate.DateTimeText = DateTime.Now.ToShortDateString();
                flag = true;
                Search();

                this.KeyDown += BorowSearch_KeyDown;
                this.hallName.KeyDown += BorowSearch_KeyDown;
                this.fromDate.KeyDown += BorowSearch_KeyDown;
                this.toDate.KeyDown += BorowSearch_KeyDown;
                this.search.Click += search_Click;
             
            }
        }

        void RepairCancel_SizeChanged(object sender, SizeChangedEventArgs e)
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

        public RepairCancel()
        {
            InitializeComponent();
            this.SizeChanged += RepairCancel_SizeChanged;
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

        /// <summary>
        /// KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BorowSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        #region 查看详情

        /// <summary>
        /// 选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridBorowList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            detailModels.Clear();
            GridBorowDetail.Rebind();

            API.View_RepairInfo vr = GridReturnList.SelectedItem as API.View_RepairInfo;
            if (vr != null)
            {
                if (vr.IsReceive == "Y" ||vr.IsReturn == "Y")
                {
                    this.cancel.IsEnabled = false;
                }
                else
                {
                    this.cancel.IsEnabled = true;
                }
                //if ()
                //{
                //    this.cancel.IsEnabled = false;
                //}
                //else
                //{
                //    this.cancel.IsEnabled = true;
                //}

                currentRID = vr.ID;
                PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 101, new object[] { vr.ID }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
            }
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.CancelListModel> list = e.Result.Obj as List<API.CancelListModel>;
                detailModels.Clear();
           
                detailModels.AddRange(list);
                GridBorowDetail.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
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
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = page.PageIndex;
            rpp.PageSize = (int)pagesize.Value;
            rpp.ParamList = new List<API.ReportSqlParams>();

            if (this.ckbReceive.SelectedIndex != 2)
            {
                API.ReportSqlParams_String state = new API.ReportSqlParams_String();
                state.ParamName = "IsReceive";
                if (this.ckbReceive.SelectedIndex == 1)
                {
                    state.ParamValues = "N";
                }
                else
                {
                    state.ParamValues = "Y";
                }

                rpp.ParamList.Add(state);
            }

            if (this.ckbReturn.SelectedIndex != 2)
            {
                API.ReportSqlParams_String isreturn = new API.ReportSqlParams_String();
                isreturn.ParamName = "IsReturn";
                if (this.ckbReturn.SelectedIndex == 1)
                {
                    isreturn.ParamValues = "N";
                }
                else
                {
                    isreturn.ParamValues = "Y";
                }

                rpp.ParamList.Add(isreturn);
            }

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

            if (!string.IsNullOrEmpty(this.hallName.Text))//.TextBox.SearchText
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "HallID";
                hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(this.hallName.Tag.ToString().Split(",".ToCharArray()));
                rpp.ParamList.Add(hall);
            }


            if (!string.IsNullOrEmpty(this.oldID.Text.ToString()))
            {
                API.ReportSqlParams_String old = new API.ReportSqlParams_String();
                old.ParamName = "OldID";
                old.ParamValues = this.oldID.Text;
                rpp.ParamList.Add(old);
            }

            if (!string.IsNullOrEmpty(this.user.Text.ToString()))
            {
                API.ReportSqlParams_String aduitUser = new API.ReportSqlParams_String();
                aduitUser.ParamName = "User";
                aduitUser.ParamValues = this.user.Text;
                rpp.ParamList.Add(aduitUser);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 102, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
        }

        /// <summary>
        /// 查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;

            models.Clear();
            GridReturnList.Rebind();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_RepairInfo> aduitList = pageParam.Obj as List<API.View_RepairInfo>;
                if (aduitList.Count != 0)
                {
                    models.AddRange(aduitList);
                    GridReturnList.Rebind();

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

        #region 取消

        void Cancel_Click(object sender, RoutedEventArgs e)
        {

            if (detailModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"详情无数据");
                return;
            }
            if (currentRID == 0)
            {
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定取消送修吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 103, new object[] { currentRID }, new EventHandler<API.MainCompletedEventArgs>(CancelCompleted));
        }

        private void CancelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "取消失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                foreach (var item in models)
                {
                    if (item.ID == currentRID)
                    {
                        models.Remove(item);
                        GridReturnList.Rebind();

                        detailModels.Clear();
                        GridBorowDetail.Rebind();
                        break;
                    }
                }
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                Logger.Log(e.Result.Message);
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                Logger.Log(e.Result.Message);
            }
        }

        #endregion 
    }
}
