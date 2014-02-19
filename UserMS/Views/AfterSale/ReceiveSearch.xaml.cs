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
using UserMS.Report.Print.RepairPrint;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// ReceiveSearch.xaml 的交互逻辑
    /// </summary>
    public partial class ReceiveSearch : Page
    {
        public ReceiveSearch()
        {
            InitializeComponent();
            searchGrid.ItemsSource = models;
            hadder = new ROHallAdder(ref this.hall, menuid);
           
            radDataPager1.PageSize = 20;
            flag = true;
            Search();
        }

        private ROHallAdder hadder = null;
        private int pageIndex = 0;
        private bool flag = false;
        List<API.View_ASPReceiveInfo> models = new List<API.View_ASPReceiveInfo>();

        private int menuid = 328;

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        #region 查  询

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Clear();
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

            if (!string.IsNullOrEmpty(this.hall.Tag.ToString()))
            {
                API.ReportSqlParams_String hall = new API.ReportSqlParams_String();
                hall.ParamName = "HallID";
                hall.ParamValues = this.hall.Tag.ToString();
                rpp.ParamList.Add(hall);
            }


            API.ReportSqlParams_Bool date = new API.ReportSqlParams_Bool();
            date.ParamName = "Delete";
            date.ParamValues = true;
            rpp.ParamList.Add(date);
            

            if (!string.IsNullOrEmpty(this.oldid.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "OldID";
                users.ParamValues = this.oldid.Text.Trim();
                rpp.ParamList.Add(users);
            }

            if (!string.IsNullOrEmpty(this.pro_imei.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Pro_IMEI";
                bt.ParamValues = this.pro_imei.Text;
                rpp.ParamList.Add(bt);
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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 332, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

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

                List<API.View_ASPReceiveInfo> list = pageParam.Obj as List<API.View_ASPReceiveInfo>;
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
                pageIndex = e.NewPageIndex;
                Search();
            }
        }

        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
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

            API.View_ASPReceiveInfo model = searchGrid.SelectedItem as API.View_ASPReceiveInfo;

            this.hallName.Text = model.BJ_HallName;
            repairedCount.Text = model.RepairCount==null?"": model.RepairCount.ToString();
            receiver.Text = model.Receiver;
            oldID.Text = model.OldID;
            vipIMEI.Text = model.IMEI;
            cus_addr.Text = model.Cus_Add;
            cus_cpc.Text = model.Cus_CPC;
            cus_email.Text = model.Cus_Email;
            cusName.Text = model.Cus_Name;
            cusPhone.Text = model.Cus_Phone;
            cus_phone2.Text = model.Cus_Phone2;
            cus_type.Text = model.Cus_CusType;
            pro_bill.Text = model.Pro_Bill;
            pro_BuyT.Text = Convert.ToDateTime(model.Pro_BuyT).ToShortDateString();
            pro_col.Text = model.ProFormat;
            pro_Error.Text = model.Pro_Error;
            pro_GetT.Text = Convert.ToDateTime(model.Pro_GetT).ToShortDateString();
            this.IMEI.Text = model.Pro_IMEI;
            pro_Name.Text = model.Pro_Name;
            pro_note.Text = model.Pro_Note;
            pro_other.Text = model.Pro_Other;
            pro_outside.Text = model.Pro_OutSide;
            pro_seq.Text = model.Pro_Seq;
            pro_sn.Text = model.Pro_SN;
            pro_type.Text = model.Pro_Type;
            chk_fid.Text = model.ChkName;
            chkInOut.Text = model.Chk_InOut;
            chkPrice.Text = model.Chk_price.ToString();
            chk_note.Text = model.Chk_Note;
            bj_money.Text = model.BJ_Money.ToString();
            bj_date.Text = Convert.ToDateTime(model.BJ_Date).ToShortDateString();
            bj_userid.Text = model.BJ_UserID == null ? "" : model.BJ_UserID.ToString();
            bjHall.Text = model.BJ_HallName;


            PublicRequestHelp peh = new PublicRequestHelp(this.isbusy, 341, new object[] { model.ID },
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                List<API.ASP_ErrorInfo> list1 = e.Result.Obj as List<API.ASP_ErrorInfo>;
                errGrid.ItemsSource = list1;
                errGrid.Rebind();

                List<API.View_BJModels> list = e.Result.ArrList[0] as List<API.View_BJModels>;
                bjGrid.ItemsSource = list;
                bjGrid.Rebind();
            }

        }
     
        #endregion

        void Clear()
        {
            this.hallName.Text = "";
            repairedCount.Text = "";
            receiver.Text = "";
            oldID.Text = "";
            vipIMEI.Text = "";
            cus_addr.Text = "";
            cus_cpc.Text = "";
            cus_email.Text = "";
            cusName.Text = "";
            cusPhone.Text = "";
            cus_phone2.Text = "";
            cus_type.Text = "";
            pro_bill.Text = "";
            pro_BuyT.Text = "";
            pro_col.Text = "";
            pro_Error.Text = "";
            pro_GetT.Text = "";
            IMEI.Text = "";
            pro_Name.Text = "";
            pro_note.Text = "";
            pro_other.Text = "";
            pro_outside.Text = "";
            pro_seq.Text = "";
            pro_sn.Text = "";
            pro_type.Text = "";
            chk_fid.Text = "";
            chkInOut.Text = "";
            chkPrice.Text = "";
            chk_note.Text = "";
            bj_money.Text = "";
            bj_date.Text = "";
            bj_userid.Text = "";
            bjHall.Text = "";

            errGrid.ItemsSource = null ;
            errGrid.Rebind();
            bjGrid.ItemsSource = null;
            bjGrid.Rebind();
        }

        private void delChecked_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择数据！");
                return;
            }
            List<API.View_ASPReceiveInfo> list = new List<API.View_ASPReceiveInfo>();

            foreach (var item in searchGrid.SelectedItems)
            {
                API.View_ASPReceiveInfo ord = item as API.View_ASPReceiveInfo;
                list.Add(ord);
            }

            if (MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,340,new object[]{ list },
               DeleteCompleted);
        }

        private void DeleteCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                Clear();
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }
        }

        private void print_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                return;
            }

            API.View_ASPReceiveInfo model = searchGrid.SelectedItem as API.View_ASPReceiveInfo;

            PrintRepairBill print = new PrintRepairBill(new List<API.View_ASPReceiveInfo>() { model });
            print.SrcPage = "/Views/AfterSale/ReceiveSearch.xaml";
            this.NavigationService.Navigate(print);
        }

    }
}
