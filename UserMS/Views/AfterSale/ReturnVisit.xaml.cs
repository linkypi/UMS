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
using UserMS.Common;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// ReturnVisit.xaml 的交互逻辑
    /// </summary>
    public partial class ReturnVisit : Page
    {
        public ReturnVisit()
        {
            InitializeComponent();

            hadder = new ROHallAdder(ref this.hall, menuid);

            //hadder = new HallFilter(false, ref hall);
            //List<API.Pro_HallInfo> halls = hadder.FilterHall(menuid, Store.ProHallInfo);
            //if (halls.Count != 0)
            //{
            //    this.hall.Tag = halls.First().HallID;
            //    this.hall.TextBox.SearchText = halls.First().HallName;
            //}
            searchGrid.ItemsSource = models;
            //this.hall.SearchButton.Click += SearchButton_Click;
            radDataPager1.PageSize = 20;
            flag = true;
        }

        private List<API.View_ASPRepairInfo> models = new List<API.View_ASPRepairInfo>();
        private ROHallAdder hadder = null;
        bool flag = false;
        int pageIndex = 0;
        private int menuid = 320;

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //hadder.GetHall(hadder.FilterHall(menuid, Store.ProHallInfo));
        }


        private void Button_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        #region 查询

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Ckb_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                Search();
            }
        }

        private void Search()
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageSize = (int)pagesize.Value;
            rpp.PageIndex = radDataPager1.PageIndex;

            rpp.ParamList = new List<API.ReportSqlParams>();

        

            //已取机
            //API.ReportSqlParams_Bool HasFetch = new API.ReportSqlParams_Bool();
            //HasFetch.ParamName = "HasFetch";
            //HasFetch.ParamValues = true;
            //rpp.ParamList.Add(HasFetch); 

            //API.ReportSqlParams_Bool HasAudited = new API.ReportSqlParams_Bool();
            //HasAudited.ParamName = "HasAudited";
            //HasAudited.ParamValues = false;

            //API.ReportSqlParams_Bool HasCallBack = new API.ReportSqlParams_Bool();
            //HasCallBack.ParamName = "HasCallBack";
            //HasCallBack.ParamValues = false;
            //rpp.ParamList.Add(HasCallBack);
            if (state.SelectedIndex != 0)
            {
                API.ReportSqlParams_String repstate = new API.ReportSqlParams_String();
                repstate.ParamName = "RpState";
                object obj = (state.SelectedItem as ComboBoxItem).Content;
                repstate.ParamValues = obj == null ? "" : obj.ToString();
                rpp.ParamList.Add(repstate);
            }

            if (!string.IsNullOrEmpty(this.hall.Tag==null?"":hall.Tag.ToString()))
            {
                API.ReportSqlParams_String h = new API.ReportSqlParams_String();
                h.ParamName = "HallID";
                h.ParamValues = this.hall.Tag.ToString();
                rpp.ParamList.Add(h);
            }

            //if (!string.IsNullOrEmpty(this.sysdate.DateTimeText ?? ""))
            //{
            //    API.ReportSqlParams_DataTime date = new API.ReportSqlParams_DataTime();
            //    date.ParamName = "SysDate";
            //    date.ParamValues = this.sysdate.SelectedDate;
            //    rpp.ParamList.Add(date);
            //}

            if (!string.IsNullOrEmpty(this.searchOldid.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "OldID";
                users.ParamValues = this.searchOldid.Text.Trim();
                rpp.ParamList.Add(users);
            }

            if (!string.IsNullOrEmpty(this.vipimei.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "VIP_IMEI";
                bt.ParamValues = this.vipimei.Text;
                rpp.ParamList.Add(bt);
            }

            if (!string.IsNullOrEmpty(this.cus_name.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Cus_Name";
                bt.ParamValues = this.cus_name.Text;
                rpp.ParamList.Add(bt);
            }

            if (!string.IsNullOrEmpty(this.cus_phone.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Cus_Phone";
                bt.ParamValues = this.cus_phone.Text;
                rpp.ParamList.Add(bt);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 363, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        private void Clear()
        {
            serviceHall.Text = string.Empty;
            receiver.Text = string.Empty;
            cus_phone.Text = string.Empty;
            serviceHall.Text = string.Empty;
            oldid.Text = string.Empty;
            repCount.Text = string.Empty;
            models.Clear();
            searchGrid.Rebind();

            foreach (var item in allAnswer.Children)
            {
                CheckBox cbox = item as CheckBox;
                cbox.IsChecked = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.radDataPager1.PageIndexChanged -= radDataPager1_PageIndexChanged;
            this.radDataPager1.Source = pcv1;
            this.radDataPager1.PageIndexChanged += radDataPager1_PageIndexChanged;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_ASPRepairInfo> list = pageParam.Obj as List<API.View_ASPRepairInfo>;
                if (list == null) { return; }
                models.Clear();
                models.AddRange(list);
                searchGrid.Rebind();
                this.radDataPager1.PageSize = (int)pagesize.Value;
                string[] data = new string[pageParam.RecordCount];
                PagedCollectionView pcv = new PagedCollectionView(data);
                this.radDataPager1.PageIndexChanged -= radDataPager1_PageIndexChanged;
                this.radDataPager1.Source = pcv;
                this.radDataPager1.PageIndexChanged += radDataPager1_PageIndexChanged;
                this.radDataPager1.PageIndex = pageIndex;
            }
            else
            {
                models.Clear();
                searchGrid.Rebind();
            }

        }

        private void radDataPager1_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                flag = true;
                pageIndex = e.NewPageIndex;
                Search();
            }
        }


        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                flag = true;
                radDataPager1.PageSize = (int)pagesize.Value;
                Search();
            }
        }

        #endregion

        #region 详情

        private void RepairGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                return;
            }
            
            API.View_ASPRepairInfo model = searchGrid.SelectedItem as API.View_ASPRepairInfo;
            bjMoney.Text = Convert.ToString(model.BJ_Money);
            workMoney.Text = Convert.ToString(model.WorkMoney);
            proMoney.Text = Convert.ToString(model.ProMoney);
            decimal to = Convert.ToDecimal(model.WorkMoney) + Convert.ToDecimal(model.ProMoney)
                - Convert.ToDecimal(model.BJ_Money);
            total.Text = to.ToString();
            shouldPay.Text = to.ToString();
            gzType.Text = model.GzType;
            gzMoney.Text = Convert.ToString(model.GzMoney);
            realPay.Text = Convert.ToString(model.RealPay);

            repCount.Text = model.RepairCount==null?"0":model.RepairCount.ToString();
            serviceHall.Text = model.RecHallName;
            oldid.Text = model.OldID;
            realMoney.Value = Convert.ToDouble(model.RealPay);
            receiver.Text = model.Receiver;
        }

 

        #endregion

        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择数据！");
                return;
            }
            API.View_ASPRepairInfo model = searchGrid.SelectedItem as API.View_ASPRepairInfo;

            if (model.RpState != "待回访")
            {
                MessageBox.Show("单号 " + model.OldID + "处于" + model.RpState + "状态，无法回访！");
                return;
            }
            //if (realMoney.Value == 0)
            //{
            //    MessageBox.Show("实收不能为0！");
            //    return;
            //}

            string msg = "";
            bool nonChecked = true;
            foreach (var item in allAnswer.Children)
            {
                CheckBox cbox = item as CheckBox;
                if (cbox.IsChecked == true)
                {
                    nonChecked = false;
                    msg += cbox.Content+" ";
                }
            }

            if (nonChecked)
            {
                MessageBox.Show("请选择应答情况！");
                return;
            }

       
            API.ASP_CallBackInfo cb = new API.ASP_CallBackInfo();
            cb.OrderID = model.OrderID;
            cb.RealMoney = Convert.ToDecimal(realMoney.Value);
            cb.Answer = msg;
            cb.Suggest = cusSuggest.Text.Trim();
            cb.Note =    cbNote.Text.Trim();

            if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,336,new object[]{model.ID, cb },
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                cusSuggest.Text = string.Empty;
                cbNote.Text = string.Empty;
                realMoney.Value = 0;
                Search();
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }
        }
    }
}
