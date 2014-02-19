using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SlModel;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.Repair
{
    public partial class RepairReturnReceive : Page
    {
        private List<API.View_RepairReturnReceive> models = null;
        private bool flag = false;
        private ROHallAdder hAdder;
        private string menuid ="";
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
                menuid = "160";
            }
            finally
            {
                if (menuid == null)
                {
                    menuid = "160";
                }
                models = new List<API.View_RepairReturnReceive>();
                DGrepairInfo.ItemsSource = models;
                this.dataPager.PageSize = (int)pagesize.Value;
                hAdder = new ROHallAdder(ref this.hallid, int.Parse(menuid));
                this.fromDate.SelectedValue = DateTime.Now.Date;

                List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
                 };
                this.ckbCancel.ItemsSource = list;
                this.ckbCancel.SelectedIndex = 1;
                receiver.Text = Store.LoginUserName;
                flag = true;
                Search();
            }
        }

        void RepairReturnReceive_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("dataPager") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        public RepairReturnReceive()
        {
            InitializeComponent();
            this.SizeChanged += RepairReturnReceive_SizeChanged;
        }

        /// <summary>
        /// 换页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dataPager_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            Search();
        }

        #region 查询

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
            GridReturnDetail.ItemsSource = null;
            GridReturnDetail.Rebind();

            this.repairReturnID.Text = "";
            this.hallName.Text = "";
            this.repairReturnDate.Text = "";
            this.UserName.Text = "";

            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = this.dataPager.PageIndex;
            rpp.PageSize = (int)pagesize.Value;
            rpp.ParamList = new List<API.ReportSqlParams>();

            if (this.ckbCancel.SelectedIndex != 2)
            {
                API.ReportSqlParams_String cancel = new API.ReportSqlParams_String();
                cancel.ParamName = "IsReceived";

                if (this.ckbCancel.SelectedIndex == 0)
                {
                    cancel.ParamValues = "Y";
                }
                else
                {
                    cancel.ParamValues = "N";
                }
                rpp.ParamList.Add(cancel);

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

            if (!string.IsNullOrEmpty(this.hallid.Text))
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "HallID";
                hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(this.hallid.Tag.ToString().Split(",".ToCharArray()));
                rpp.ParamList.Add(hall);
            }

            if (!string.IsNullOrEmpty(this.user.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "UserName";
                users.ParamValues = this.user.Text;
                rpp.ParamList.Add(users);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 180, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.dataPager.PageIndexChanged -= dataPager_PageIndexChanged;
            this.dataPager.Source = pcv1;
            this.dataPager.PageIndexChanged += dataPager_PageIndexChanged;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_RepairReturnReceive> repairList = pageParam.Obj as List<API.View_RepairReturnReceive>;
                models.Clear();
                if (repairList != null)
                {
                    models.AddRange(repairList);
                    DGrepairInfo.Rebind();
                }
                

                this.dataPager.PageSize = (int)pagesize.Value;
                string[] data = new string[pageParam.RecordCount];

                PagedCollectionView pcv = new PagedCollectionView(data);
                this.dataPager.PageIndexChanged -= dataPager_PageIndexChanged;
                this.dataPager.Source = pcv;
                this.dataPager.PageIndexChanged += dataPager_PageIndexChanged;
                this.dataPager.PageIndex = pageindex;
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                models.Clear();
                DGrepairInfo.Rebind();
            }

        }

        #endregion

        #region 查看详情

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGrepairInfo_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (DGrepairInfo.SelectedItem == null)
            {
                return;
            }

            API.View_RepairReturnReceive rm = DGrepairInfo.SelectedItem as API.View_RepairReturnReceive;

            if (rm.IsReceived == "Y")
            {
                this.recv.IsEnabled = false;
            }
            else
            {
                this.recv.IsEnabled = true;
            }
            this.repairReturnID.Tag = rm.ID;
            this.repairReturnID.Text = rm.RepairReturnID;
            this.hallName.Tag = rm.ID;
            this.hallName.Text = rm.HallName;
            this.repairReturnDate.Text = rm.RepairReturnDate;
            this.UserName.Text = rm.UserName;

            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 82, new object[] { rm.ID }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.View_Pro_RepairRetrunDetail> list = new List<API.View_Pro_RepairRetrunDetail>();
                list = e.Result.Obj as List<API.View_Pro_RepairRetrunDetail>; 
                GridReturnDetail.ItemsSource = list;
                GridReturnDetail.Rebind();
            }
        }

        #endregion 

        /// <summary>
        /// 单个接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnReceive_Click(object sender, RoutedEventArgs e)
        {
            if (DGrepairInfo.SelectedItems == null)
            {
                return;
            }

             API.View_RepairReturnReceive rm = DGrepairInfo.SelectedItem as API.View_RepairReturnReceive;
             if (rm.IsReceived == "Y")
             {
                 MessageBox.Show(System.Windows.Application.Current.MainWindow,"该返库单已接收");
                 return;
             }
       
             if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定接收吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
             {
                 return;
             }
             PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 181, new object[] { rm.ID,DateTime.Now }, new EventHandler<API.MainCompletedEventArgs>(ReceiveCompleted));
   
        }

        /// <summary>
        /// 批量接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void batchReceive_Click(object sender, RoutedEventArgs e)
        {
            if (DGrepairInfo.SelectedItems == null)
            {
                return;
            }
            List<int> rids = new List<int>();
            foreach(var item in DGrepairInfo.SelectedItems)
            {
                API.View_RepairReturnReceive rm = item as API.View_RepairReturnReceive;
                if (rm.IsReceived == "Y")
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"返库单" + rm.RepairReturnID + "已接收");
                    return;
                }
                rids.Add(rm.ID);
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定全部接收吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 182, new object[] { rids }, new EventHandler<API.MainCompletedEventArgs>(ReceiveCompleted));
   
        }

        /// <summary>
        /// 接收完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "接收失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"接收成功");
                Search();

                this.repairReturnID.Text = string.Empty;
                this.hallName.Text = string.Empty;
                this.repairReturnDate.Text = string.Empty;
                this.UserName.Text = string.Empty;
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
        }

    }
}
