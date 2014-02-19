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
    public partial class BorowCancel : Page
    {
        private List<API.View_BorowInfo> models = null;
        List<API.BorowListModel> detailModels = null;
        private ROHallAdder hall = null;
        private int currentBID;
        private string menuid = "";
        private bool flag  =false;
        private int pageindex;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
           
            menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            if (menuid == null)
            {
                menuid = "90";
            }
            models = new List<API.View_BorowInfo>();
            this.GridBorowList.ItemsSource = models;
            detailModels = new List<API.BorowListModel>();
            this.GridBorowDetail.ItemsSource = detailModels;

            hall = new ROHallAdder(ref this.hallName, int.Parse(menuid));

            List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
            };
            this.ckbReturn.ItemsSource = list;
            this.ckbReturn.SelectedIndex =1;
            this.fromDate.DateTimeText = DateTime.Now.ToShortDateString();
            flag = true;
            Search();

            this.KeyDown += BorowSearch_KeyDown;
            this.hallName.KeyDown += BorowSearch_KeyDown;
            this.fromDate.KeyDown += BorowSearch_KeyDown;
            this.toDate.KeyDown += BorowSearch_KeyDown;
        }

        void BorowCancel_SizeChanged(object sender, SizeChangedEventArgs e)
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

        public BorowCancel()
        {
            InitializeComponent();
            this.SizeChanged += BorowCancel_SizeChanged;
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
            GridIMEI.ItemsSource = null;
            detailModels.Clear();
            GridBorowDetail.Rebind();
            GridIMEI.Rebind();

            API.View_BorowInfo vb = GridBorowList.SelectedItem as API.View_BorowInfo;
            if (vb != null)
            {
                if (vb.IsReturn == "Y")
                {
                    this.cancel.IsEnabled = false;
                }
                else
                {
                    this.cancel.IsEnabled = true;
                }
                currentBID = vb.ID;
                PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 39, new object[] { vb.ID.ToString() }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
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
                List<API.BorowListModel> list = e.Result.Obj as List<API.BorowListModel>;
                detailModels.Clear(); 

                detailModels.AddRange(list);
                GridBorowDetail.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
        }

        /// <summary>
        /// 选中详情中的商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GridBorowDetail_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            API.BorowListModel bm = GridBorowDetail.SelectedItem as API.BorowListModel;
            if (bm != null)
            {
                GridIMEI.ItemsSource = bm.IIMEIList;
                GridIMEI.Rebind();
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

            if (this.ckbReturn.SelectedIndex != 2)
            {
                API.ReportSqlParams_String state = new API.ReportSqlParams_String();
                state.ParamName = "IsReturn";
                if (this.ckbReturn.SelectedIndex == 1)
                {
                    state.ParamValues = "N";
                }
                else
                {
                    state.ParamValues = "Y";
                }

                rpp.ParamList.Add(state);
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

            if (!string.IsNullOrEmpty(this.borower.Text.ToString()))
            {
                API.ReportSqlParams_String aduitUser = new API.ReportSqlParams_String();
                aduitUser.ParamName = "Borrower";
                aduitUser.ParamValues = this.borower.Text;
                rpp.ParamList.Add(aduitUser);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 95, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
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
            GridBorowList.Rebind();
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

                List<API.View_BorowInfo> aduitList = pageParam.Obj as List<API.View_BorowInfo>;
                if (aduitList != null)
                {
                    models.AddRange(aduitList);
                    GridBorowList.Rebind();

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
           if (currentBID==0)
            {
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定取消借贷吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
      
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,96,new object[]{currentBID},new EventHandler<API.MainCompletedEventArgs>(CancelCompleted));
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
                    if (item.ID == currentBID)
                    {
                        models.Remove(item);
                        GridBorowList.Rebind();

                        detailModels.Clear();
                        GridBorowDetail.Rebind();
                        GridIMEI.ItemsSource = null;
                        GridIMEI.Rebind();
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
