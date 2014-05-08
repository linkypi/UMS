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
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// GetMobile.xaml 的交互逻辑
    /// </summary>
    public partial class GetMobile : Page
    {
        private List<API.View_ASPSHInfo> models = new List<API.View_ASPSHInfo>();
        private bool flag = false;
        private ROHallAdder hadder = null;
        private int pageIndex = 0;
        private int menuid = 321;

        public GetMobile()
        {
            InitializeComponent();
            searchGrid.ItemsSource = models;
            //hadder = new HallFilter(false, ref hall);
            //List<API.Pro_HallInfo> halls = hadder.FilterHall(menuid, Store.ProHallInfo);
            //hadder = new ROHallAdder(ref this.hall, menuid);
            //if (halls.Count != 0)
            //{
            //    this.hall.Tag = halls.First().HallID;
            //    this.hall.TextBox.SearchText = halls.First().HallName;
            //}
            //this.hall.SearchButton.Click += SearchButton_Click;
            hadder = new ROHallAdder(ref this.hall, menuid);
            flag = true;
            serviceTime.Text = DateTime.Now.ToShortDateString();
            radDataPager1.PageSize = 20;
        }
  
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           // hadder.GetHall(hadder.FilterHall(menuid, Store.ProHallInfo));
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


            //API.ReportSqlParams_Bool HasRepaired = new API.ReportSqlParams_Bool();
            //HasRepaired.ParamName = "HasRepaired";
            //HasRepaired.ParamValues = true;
            //rpp.ParamList.Add(HasRepaired);  //HasFetch

            //API.ReportSqlParams_Bool HasFetch = new API.ReportSqlParams_Bool();
            //HasFetch.ParamName = "HasFetch";
            //HasFetch.ParamValues = false;
            //rpp.ParamList.Add(HasFetch);

            //API.ReportSqlParams_Bool Finished = new API.ReportSqlParams_Bool();
            //Finished.ParamName = "Finished";
            //Finished.ParamValues = false;
            //rpp.ParamList.Add(Finished);

            ////审核未通过
            //API.ReportSqlParams_Bool IsPassed = new API.ReportSqlParams_Bool();
            //IsPassed.ParamName = "IsPassed";
            //IsPassed.ParamValues = true;
            //rpp.ParamList.Add(IsPassed);

            //API.ReportSqlParams_Bool IsAudit = new API.ReportSqlParams_Bool();
            //IsAudit.ParamName = "IsAudit";
            //IsAudit.ParamValues = true;
            //rpp.ParamList.Add(IsAudit);

            if (state.SelectedIndex != 0)
            {
                API.ReportSqlParams_String repstate = new API.ReportSqlParams_String();
                repstate.ParamName = "RpState";
                object obj = (state.SelectedItem as ComboBoxItem).Content;
                repstate.ParamValues = obj == null ? "" : obj.ToString();
                rpp.ParamList.Add(repstate);
            }
            if (!string.IsNullOrEmpty(this.hall.Tag + ""))
            {
                API.ReportSqlParams_String hall = new API.ReportSqlParams_String();
                hall.ParamName = "HallID";
                hall.ParamValues = this.hall.Tag.ToString();
                rpp.ParamList.Add(hall);
            }

            //if (!string.IsNullOrEmpty(this.sysdate.DateTimeText??""))
            //{
            //    API.ReportSqlParams_DataTime date = new API.ReportSqlParams_DataTime();
            //    date.ParamName = "SysDate";
            //    date.ParamValues = this.sysdate.SelectedDate;
            //    rpp.ParamList.Add(date);
            //}

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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 358, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        private void Clear()
        {
            proNote.Text = string.Empty;
            repairnote.Text = string.Empty;
            position.Text = string.Empty;
            serviceHall.Text = string.Empty;
            chkInOut.Text = string.Empty;
            repairHall.Text = string.Empty;
            fetchNote.Text = string.Empty;
            cusPhone.Text = string.Empty;
            // this.repairer.Text = model.Repairer;
            oldID.Text = string.Empty;
            receiver.Text = string.Empty;
            vipIMEI.Text = string.Empty;
            cusName.Text = string.Empty;
            cus_Name.Text = string.Empty;
            cus_phone.Text = string.Empty;
            cus_Type.Text = string.Empty;
            cusAddr.Text = string.Empty;
            cusPhone2.Text = string.Empty;
            cusEmail.Text = string.Empty;
            cusCPC.Text = string.Empty;
            senderer.Text = string.Empty;
            senderPhone.Text = string.Empty;

            headerIMEI.Text = string.Empty;
            //proIMEI.Text = string.Empty;
            sn.Text = string.Empty;
            proName.Text = string.Empty;
            proType.Text = string.Empty;
            proColor.Text = string.Empty;
            proSeq.Text = string.Empty;
            proOther.Text = string.Empty;

            serviceHall.Text = string.Empty;
            repairNote.Text = string.Empty;
            bj_money.Text = string.Empty;
            bj_date.Text = string.Empty;
            this.BJ_UserID.Text = string.Empty;
            yjDate.Text = string.Empty;

            serviceTime.Text = string.Empty;
            proMoney2.Text = string.Empty;
            bjMoney.Text = string.Empty;
            workMoney.Text = string.Empty;
            proMoney.Text = string.Empty;

            total.Text = string.Empty;
            shouldPay.Text = string.Empty;
            realPay.Value = 0;

            oldErrGrid.ItemsSource = null;
            newErrGrid.ItemsSource = null;
            oldErrGrid.Rebind();
            newErrGrid.Rebind();
            models.Clear();
            searchGrid.Rebind();
            prosGrid.ItemsSource = null;
            prosGrid.Rebind();

            bjGrid.ItemsSource = null;
            bjGrid.Rebind();

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

                List<API.View_ASPSHInfo> list = pageParam.Obj as List<API.View_ASPSHInfo>;
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

            API.View_ASPSHInfo model = searchGrid.SelectedItem as API.View_ASPSHInfo;

            int count = (int)model.RepairCount;

            repCount.Text = count.ToString();
            if (count > 0)
            {
                HasRepaired.IsChecked = true;
            }
            else
            {
                HasRepaired.IsChecked = false;
            }
            repairnote.Text = model.RepairNote;
            position.Text = model.Position;
            serviceHall.Text = model.RecHallName;
            chkInOut.Text = model.Chk_InOut;
            repairHall.Text = model.RepairHallName;
           // this.repairer.Text = model.Repairer;
            proNote.Text = model.Pro_Note;
            oldID.Text = model.OldID;
            receiver.Text = model.Receiver;
            vipIMEI.Text = model.IMEI;
            cusName.Text = model.Cus_Name;
            cus_Name.Text = model.Cus_Name;
           // cus_phone.Text = model.Cus_Phone;
            this.cusPhone.Text = model.Cus_Phone;
            cus_Type.Text = model.Cus_CusType;
            cusAddr.Text = model.Cus_Add;
            cusPhone2.Text = model.Cus_Phone2;
            cusEmail.Text = model.Cus_Email;
            cusCPC.Text = model.Cus_CPC;
            senderer.Text = model.Sender;
            senderPhone.Text = model.SenderPhone;

            headerIMEI.Text = model.Pro_HeaderIMEI;
            //proIMEI.Text = model.Pro_IMEI;
            sn.Text = model.Pro_SN;
            proName.Text = model.Pro_Name;
            proType.Text = model.Pro_Type;
            proColor.Text = model.Pro_Color;
            proSeq.Text = model.Pro_Seq;
            proOther.Text = model.Pro_Other;

            serviceHall.Text = model.RecHallName;
            repairNote.Text = model.RepairNote;
            bj_money.Text = model.BJ_Money.ToString();
            bj_date.Text = model.BJDate == null ? "" : model.BJDate.ToString();
            this.BJ_UserID.Text = model.BJUser;
            yjDate.Text = model.BJDate == null ? "" : model.BJDate.ToString();
          

            proMoney2.Text = model.ProMoney.ToString();
            bjMoney.Text = model.BJ_Money.ToString();
            workMoney.Text = model.WorkMoney.ToString();
            proMoney.Text = model.ProMoney.ToString();
            decimal tot = (decimal)(model.WorkMoney + model.ProMoney - model.BJ_Money);
            total.Text = tot.ToString();
            shouldPay.Text = (model.WorkMoney + model.ProMoney).ToString();
            realPay.Value =Convert.ToDouble( tot);
            //repairCount .Text = model.

            PublicRequestHelp peh = new PublicRequestHelp(this.isbusy, 326, new object[] { model.ID },
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                List<API.ASP_ErrorInfo> list1 = e.Result.Obj as List<API.ASP_ErrorInfo>;
                newErrGrid.ItemsSource = list1;
                newErrGrid.Rebind();

                //配件
                List<API.View_ASPCurrentOrderPros> list = e.Result.ArrList[0] as List<API.View_ASPCurrentOrderPros>;
                prosGrid.ItemsSource = list;
                prosGrid.Rebind();

                //e.Result.ArrList[1]  备机
                List<API.View_BJModels> bjlist = e.Result.ArrList[1] as List<API.View_BJModels>;
                bjGrid.ItemsSource = bjlist;
                bjGrid.Rebind();

                //e.Result.ArrList[2]  受理故障
                List<API.ASP_ErrorInfo> list2 = e.Result.ArrList[2] as List<API.ASP_ErrorInfo>;
                oldErrGrid.ItemsSource = list2;
                oldErrGrid.Rebind();
            }

        }

        #endregion

        private void addPro_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delPro_Click(object sender, RoutedEventArgs e)
        {
             
        }

        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Audit(true);
        }

        private void Audit(bool passed)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择数据！");
                return;
            }

            API.VIP_VIPInfo vip = null;
            List<API.View_ASPSHInfo> list = new List<API.View_ASPSHInfo>();
            List<API.View_BJModels> bjinfo = bjGrid.ItemsSource as List<API.View_BJModels>;

            #region  单个取机
            if (searchGrid.SelectedItems.Count() == 1)
            {
                API.View_ASPSHInfo model = searchGrid.SelectedItem as API.View_ASPSHInfo;
                if (model.RpState != "待取机")
                {
                    MessageBox.Show("单号 " + model.OldID + "处于" + model.RpState + "状态，无法取机！");
                    return;
                }

                #region  输入会员卡号

                VIPWindow vw = new VIPWindow();
                vw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (vw.ShowDialog() == true)
                {
                    vip = vw.vipInfo;
                    vw.Close();
                }

                #endregion

                if (passed)
                {
                    model.FetchNote = fetchNote.Text.Trim();
                    model.FetchNeedAudit = false;

                    foreach (var item in bjinfo)
                    {
                        if (string.IsNullOrEmpty(item.NewIMEI))
                        {
                            continue;
                        }

                        if (!item.IMEI.ToUpper().Equals(item.NewIMEI))
                        {
                            model.FetchNeedAudit = true;
                        }
                    }


                    decimal tot = 0;

                    if (vip != null)
                    {
                        tot = (decimal)(model.WorkMoney * Convert.ToDecimal(0.80) + model.ProMoney - model.BJ_Money);
                    }
                    else
                    {
                        tot = (decimal)(model.WorkMoney + model.ProMoney - model.BJ_Money);
                    }
                    // if (tot < 0) { tot = 0; }
                    model.ShouldPay = model.WorkMoney + model.ProMoney;
                    model.RealPay = tot;
                    if (MessageBox.Show("实收金额为：￥ " + model.RealPay + " , 确定取机吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    list.Add(model);
                }
                else
                {
                    if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
            }
            #endregion 

            #region  批量取机

            if (searchGrid.SelectedItems.Count() > 1)
            {
                foreach (var item in searchGrid.SelectedItems)
                {
                    API.View_ASPSHInfo model = item as API.View_ASPSHInfo;
                    if (model.RpState != "待取机")
                    {
                        MessageBox.Show("单号 " + model.OldID + "处于" + model.RpState + "状态，无法取机！");
                        return;
                    }
                    model.FetchNote = fetchNote.Text.Trim();
                    model.FetchNeedAudit = false;
                    decimal tot = 0;

                    if (vip != null)
                    {
                        tot = (decimal)(model.WorkMoney * Convert.ToDecimal(0.80) + model.ProMoney - model.BJ_Money);
                    }
                    else
                    {
                        tot = (decimal)(model.WorkMoney + model.ProMoney - model.BJ_Money);
                    }
                    model.ShouldPay = model.WorkMoney + model.ProMoney;
                    model.RealPay = tot;
                    list.Add(model);
                }
                if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            #endregion 

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 334,
            new object[] { list, bjinfo, passed, vip??new API.VIP_VIPInfo() }, new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                Search();
                fetchNote.Text = string.Empty;
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }
        }

        private void bjGrid_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            //API.View_BJModels model = e.Row.DataContext as API.View_BJModels;
           // if (model.NeedIMEI)
           // {
            API.View_BJModels model = e.Row.Item as API.View_BJModels;

            if (model == null) {
                return;
            }

                if (model.NeedIMEI==false)
                {
                    e.Row.Cells[9].IsEnabled = false;
                    //TextBlock tb = e.Row.Cells[9].Content as TextBlock;
                   // tb.IsEnabled = false;
                }

               // e.Row.Cells[9].IsEnabled = false;
           // }

        }

        private void gzType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!flag) { return; }
            //gzMoney.IsEnabled = true;
            if (gzType.SelectedItem == null) { return; }
            if ((gzType.SelectedItem as ComboBoxItem).Content.ToString() == "绿色通道")
            {
                gzMoney.IsEnabled = true;
            }
            else
            {
                gzMoney.IsEnabled = false;  
            }
        }

        private void gzMoney_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            double sp = Convert.ToDouble(total.Text.ToString());
            realPay.Value = sp - gzMoney.Value;
            if (realPay.Value < 0) { realPay.Value = 0; }
        }

        private void UnSave_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Audit(false);
        }
    }
}
