using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SlModel;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Apply
{
    public partial class SellBackApply : Page
    {
        private ROHallAdder hadder = null;
        private int pageSize = 30;
        private List<API.View_Pro_SellInfo> models = null;
        private string menuid = "";

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "28";
            }         
            hadder = new ROHallAdder(ref this.hallid, int.Parse(menuid));
            models = new List<API.View_Pro_SellInfo>();
            GridSellList.ItemsSource = models;

            List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"全部"),
                new  CkbModel(false,"已申请"),
                new  CkbModel(false,"未申请"),
            };
            this.ckbApply.ItemsSource = list;
            this.ckbApply.SelectedIndex = 2;

            this.fromDate.SelectedValue = DateTime.Now.Date;
            this.dataPager.PageSize = pageSize;
            this.applyUser.Text = Store.LoginUserInfo.UserName;
            this.aduitID.Text = "系统自动生成";
            this.submit.Click += submit_Click;
            // this.HallID.SearchButton.Click += SearchButton_Click;
            this.dataPager.PageIndexChanged += dataPager_PageIndexChanged;
            GridSellList.SelectionChanged += GridSellList_SelectionChanged;

            Search();
        }

        public SellBackApply()
        {
            InitializeComponent();
        }

        void Clear()
        {
            this.sellID.Text = string.Empty;
            this.hallname.Text = string.Empty;
            this.applyUser.Text = string.Empty;
            //this.aduitMoney.Text = string.Empty;
            this.orderID.Text = string.Empty;
        }

        void GridSellList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            RadGridView rg = sender as RadGridView;
            if (rg.SelectedItem != null)
            {
                API.View_Pro_SellInfo vp = rg.SelectedItem as API.View_Pro_SellInfo;
                this.applyUser.Text = Store.LoginUserInfo.UserName;
                this.orderID.Text = string.IsNullOrEmpty(vp.OldID) ? "" : vp.OldID;
                this.hallname.Text = vp.HallName;
                this.hallname.Tag = vp.HallID;
                this.sellID.Text = vp.SellID;
                this.sellID.Tag = vp.ID;
                //this.aduitID.Text = string.IsNullOrEmpty(vp.AuditID) ? "" : vp.AuditID;
            }
        }

        void dataPager_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            Search();
        }

        #region 提交数据

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void submit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridSellList.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要提交的数据");
                return;
            }

            API.View_Pro_SellInfo vp = GridSellList.SelectedItem as API.View_Pro_SellInfo;

            if (vp.Applyed == "Y")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该条记录已申请");
                return;
            }
            if (applyMoney.Value == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"申请金额不能为0");
                return;
            }

            API.Pro_SellBackAduit psa = new API.Pro_SellBackAduit();
            psa.HallID = this.hallname.Tag.ToString();
            psa.ApplyMoney =(decimal) applyMoney.Value;
            //try
            //{
            //    psa.AduitMoney = decimal.Parse(this.aduitMoney.Text.ToString().Trim());
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"退货金额无效");
            //    return;
            //}
            psa.SellID = Convert.ToInt32(this.sellID.Tag.ToString());
            psa.ApplyUser = Store.LoginUserInfo.UserID;
            psa.ApplyDate = DateTime.Today;
            psa.SysDate = DateTime.Now;
            psa.SellID = vp.ID;

            PublicRequestHelp prh = new PublicRequestHelp(null, 31, new object[] { psa}, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"申请成功");
                Logger.Log("申请成功");

               // this.aduitMoney.Text = string.Empty;
               // this.applyUser.Text = Store.LoginUserInfo.UserName;
                this.orderID.Text = string.Empty;
                this.hallname.Text = string.Empty;
                this.hallname.Tag = string.Empty;

                //申请成功后移除
                int sellinfoid = Convert.ToInt32(e.Result.Obj.ToString());
                foreach (var item in models)
                {
                    if (sellinfoid == item.ID)
                    {
                        models.Remove(item);
                        GridSellList.Rebind();
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"申请失败");
                Logger.Log("申请失败");
            }
        }

        #endregion

        #region  查询

        void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = this.dataPager.PageIndex;
            rpp.PageSize = pageSize;
            rpp.ParamList = new List<API.ReportSqlParams>();


            if (this.ckbApply.SelectedIndex != 0)
            {
                API.ReportSqlParams_String state = new API.ReportSqlParams_String();
                state.ParamName = "Applyed";
                if (this.ckbApply.SelectedIndex == 2)
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

            if (!string.IsNullOrEmpty(this.hallid.Text))
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "HallID";
                hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(this.hallid.Tag.ToString().Split(",".ToCharArray()));
                rpp.ParamList.Add(hall);
            }

            if (!string.IsNullOrEmpty(this.seller.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "UserName";
                users.ParamValues = this.seller.Text;
                rpp.ParamList.Add(users);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 84, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;

            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_Pro_SellInfo> repairList = pageParam.Obj as List<API.View_Pro_SellInfo>;
                models.Clear();
                models.AddRange(repairList);
                GridSellList.Rebind();

                double page = double.Parse((pageParam.RecordCount / (pageSize * 1.0)).ToString());
                dataPager.NumericButtonCount = (int)Math.Ceiling(page);

                string[] data = new string[pageParam.RecordCount];

                PagedCollectionView pcv = new PagedCollectionView(data);
                if (this.dataPager.Source == null)
                {
                    this.dataPager.Source = pcv;
                }
              
            }
            else
            {
                models.Clear();
                GridSellList.Rebind();
            }

        }

        #endregion
    }
}
