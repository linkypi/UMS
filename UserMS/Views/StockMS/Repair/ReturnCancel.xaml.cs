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
    public partial class ReturnCancel : Page
    {
        private List<API.View_Pro_RepairReturnInfo> models = null;
        private bool flag = false;
        private int pageindex;

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
                menuid = "22";
            }
            finally
            {
                if (menuid == null)
                {
                    menuid = "22";
                }
                models = new List<API.View_Pro_RepairReturnInfo>();
                DGrepairInfo.ItemsSource = models;
                this.dataPager.PageSize = (int)pagesize.Value;
                hAdder = new ROHallAdder(ref this.hallid, int.Parse(menuid));
                this.fromDate.SelectedValue = DateTime.Now.Date;

                List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
               };
                //this.ckbCancel.ItemsSource = list;
                //this.ckbCancel.SelectedIndex = 1;
                this.ckbReceive.ItemsSource = list;
                this.ckbReceive.SelectedIndex = 1;
                flag = true;
                Search();

                this.KeyDown += Repair_Return_KeyDown;
                //this.ckb.KeyDown += Repair_Return_KeyDown;
                this.user.KeyDown += Repair_Return_KeyDown;
                this.hallid.KeyDown += Repair_Return_KeyDown;
                this.fromDate.KeyDown += Repair_Return_KeyDown;
                this.toDate.KeyDown += Repair_Return_KeyDown;
              
            }
        }

        void ReturnCancel_SizeChanged(object sender, SizeChangedEventArgs e)
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

        public ReturnCancel()
        {
            InitializeComponent();
            this.SizeChanged += ReturnCancel_SizeChanged;
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

        void Repair_Return_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        #region 取消返库

        /// <summary>
        /// 取消返库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ReturnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.repairReturnID.Tag == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要返库的单号");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定取消返库吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            int rrid  = Convert.ToInt32(this.repairReturnID.Tag.ToString());

            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 83, new object[] { rrid }, new EventHandler<API.MainCompletedEventArgs>(CancelCompleted));
        }

        private void CancelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "取消失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"取消成功");

                foreach (var item in models)
                {
                    if (item.ID.ToString() == e.Result.Obj.ToString())
                    {
                        models.Remove(item);
                        DGrepairInfo.Rebind();
                        break;
                    }
                }

                Search();
                this.repairReturnID.Tag = null;
                this.repairReturnID.Text = string.Empty;
                this.hallName.Tag = null;
                this.hallName.Text = string.Empty;
                this.repairReturnDate.Text = string.Empty;
                this.UserName.Text = string.Empty;
                
                return;
            }
            MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
        }

        #endregion 

        #region 查看详情

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGrepairInfo_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            if (DGrepairInfo.SelectedItem == null)
            {
                return;
            }

            API.View_Pro_RepairReturnInfo rm = DGrepairInfo.SelectedItem as API.View_Pro_RepairReturnInfo;

            if (rm.Canceled == "Y"|| rm.IsReceived== "Y")
            {
                this.cancel.IsEnabled = false;
            }
            else
            {
                this.cancel.IsEnabled = true;           
            }
            this.repairReturnID.Tag = rm.ID;
            this.repairReturnID.Text = rm.RepairReturnID;
            this.hallName.Tag = rm.ID;
            this.hallName.Text = rm.HallName;
            this.repairReturnDate.Text = rm.RepairReturnDate;
            this.UserName.Text = rm.UserName;
            this.orderId.Text = rm.OldID;
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

           // if (this.ckbCancel.SelectedIndex!=2)
           // {
                API.ReportSqlParams_String cancel = new API.ReportSqlParams_String();
                cancel.ParamName = "Canceled";
                cancel.ParamValues = "N";
                //if(this.ckbCancel.SelectedIndex==0)
                //{
                //    cancel.ParamValues = "Y";
                //}
                //else
                //{
                //    cancel.ParamValues = "N";
                //}
                rpp.ParamList.Add(cancel);

           // }

            if (this.ckbReceive.SelectedIndex != 2)
            {
                API.ReportSqlParams_String receive = new API.ReportSqlParams_String();
                receive.ParamName = "IsReceived";

                if (this.ckbReceive.SelectedIndex == 0)
                {
                    receive.ParamValues = "Y";
                }
                else
                {
                    receive.ParamValues = "N";
                }
                rpp.ParamList.Add(receive);

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

            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 81, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

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

                List<API.View_Pro_RepairReturnInfo> repairList = pageParam.Obj as List<API.View_Pro_RepairReturnInfo>;
                models.Clear();
                models.AddRange(repairList);
                DGrepairInfo.Rebind();

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

    }
}
