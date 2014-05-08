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
    /// Audit.xaml 的交互逻辑
    /// </summary>
    public partial class Audit : Page
    {
        private int pageIndex;
        private bool flag=false;
        private List<API.View_ASPRepairInfo> models = new List<API.View_ASPRepairInfo>();
        private ROHallAdder hadder = null;
        private int menuid = 322;

        public Audit()
        {
            InitializeComponent();
            searchGrid.ItemsSource = models;
            //hadder = new HallFilter(false, ref hall);
            //List<API.Pro_HallInfo> halls = hadder.FilterHall(menuid, Store.ProHallInfo);
            //if (halls.Count!=0)
            //{
            //    this.hall.Tag = halls.First().HallID;
            //    this.hall.TextBox.SearchText = halls.First().HallName;
            //}
            //this.hall.SearchButton.Click += SearchButton_Click;
            hadder = new ROHallAdder(ref this.hall, menuid);
            flag = true;
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
            //rpp.ParamList.Add(HasRepaired);
            if (state.SelectedIndex != 0)
            {
                API.ReportSqlParams_String repstate = new API.ReportSqlParams_String();
                repstate.ParamName = "RpState";
                object obj = (state.SelectedItem as ComboBoxItem).Content;
                repstate.ParamValues = obj == null ? "" : obj.ToString();
                rpp.ParamList.Add(repstate);
            }

            //API.ReportSqlParams_Bool HasAudited = new API.ReportSqlParams_Bool();
            //HasAudited.ParamName = "HasAudited";
            //HasAudited.ParamValues = false;
            //rpp.ParamList.Add(HasAudited);  // 

            //API.ReportSqlParams_Bool HasCallBack = new API.ReportSqlParams_Bool();
            //HasCallBack.ParamName = "HasCallBack";
            //HasCallBack.ParamValues = true;
            //rpp.ParamList.Add(HasCallBack);
            
            //API.ReportSqlParams_Bool IsToFact = new API.ReportSqlParams_Bool();
            //IsToFact.ParamName = "FetchHasAuditedOrUnNeedAudit";
            //IsToFact.ParamValues = true;
            //rpp.ParamList.Add(IsToFact);

            if (!string.IsNullOrEmpty(this.hall.Tag + ""))
            {
                API.ReportSqlParams_String hall = new API.ReportSqlParams_String();
                hall.ParamName = "HallID";
                hall.ParamValues = this.hall.Tag.ToString();
                rpp.ParamList.Add(hall);
            }

            //if (!string.IsNullOrEmpty(this.sysdate.DateTimeText ?? ""))
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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 362, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        private void Clear()
        {
            serviceHall.Text = string.Empty;
            chkInOut.Text = string.Empty;
            repairHall.Text = string.Empty;
            auditNote.Text = string.Empty;
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
            proIMEI.Text = string.Empty;
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

            proMoney2.Text = string.Empty;
            bjMoney.Text = string.Empty;
            workMoney.Text = string.Empty;
            proMoney.Text = string.Empty;

            total.Text = string.Empty;
            shouldPay.Text = string.Empty;
            realPay.Text = string.Empty;

            oldErrGrid.ItemsSource = null;
            newErrGrid.ItemsSource = null;
            oldErrGrid.Rebind();
            newErrGrid.Rebind();
            models.Clear();
            searchGrid.Rebind();

            bjGrid.ItemsSource = null;
            bjGrid.Rebind();
            prosGrid.ItemsSource = null;
            prosGrid.Rebind();
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

            API.View_ASPRepairInfo model = searchGrid.SelectedItem as API.View_ASPRepairInfo;

            serviceHall.Text = model.RecHallName;
            chkInOut.Text = model.Chk_InOut;
            repairHall.Text = model.RepairHallName;
            cusPhone.Text = model.Cus_Phone;
            gzMoney.Text = model.GzMoney == null ? "" : model.GzMoney.ToString();
            gzType.Text = model.GzType;
            // this.repairer.Text = model.Repairer;
            oldID.Text = model.OldID;
            receiver.Text = model.Receiver;
            vipIMEI.Text = model.IMEI;
            cusName.Text = model.Cus_Name;
            cus_Name.Text = model.Cus_Name;
           
            cus_Type.Text = model.Cus_CusType;
            cusAddr.Text = model.Cus_Add;
            cusPhone2.Text = model.Cus_Phone2;
            cusEmail.Text = model.Cus_Email;
            cusCPC.Text = model.Cus_CPC;
            senderer.Text = model.Sender;
            senderPhone.Text = model.SenderPhone;
            serviceHall.Text = model.RecHallName;


            headerIMEI.Text = model.Pro_HeaderIMEI;
            proIMEI.Text = model.Pro_IMEI;
            sn.Text = model.Pro_SN;
            proName.Text = model.Pro_Name;
            proType.Text = model.Pro_Type;
            proColor.Text = model.Pro_Color;
            proSeq.Text = model.Pro_Seq;
            proOther.Text = model.Pro_Other;

            repairNote.Text = model.RepairNote;
            bj_money.Text = model.BJ_Money.ToString();
            bj_date.Text = model.BJDate == null ? "" : model.BJDate.ToString();
            this.BJ_UserID.Text = model.BJUser;
            yjDate.Text = model.BJDate == null ? "" : model.BJDate.ToString();

            proMoney2.Text = model.ProMoney.ToString();
            bjMoney.Text = model.BJ_Money.ToString();
            workMoney.Text = model.WorkMoney.ToString();
            proMoney.Text = model.ProMoney.ToString();
            decimal tot = (decimal)(Convert.ToDecimal(model.WorkMoney) + 
                Convert.ToDecimal(model.ProMoney) - Convert.ToDecimal(model.BJ_Money));
            total.Text = tot.ToString();
            shouldPay.Text = model.ShouldPay.ToString();
            realPay.Text = model.RealPay.ToString();
            repCount.Text = model.RepairCount == null ? "0" : model.RepairCount.ToString(); ;
            auditMoney.Value = Convert.ToDouble(model.RealPay);
            lowMoney.Value = Convert.ToDouble(model.RealPay);
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

        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //this.isbusy.IsBusy = false;

            if (searchGrid.SelectedItems.Count() == 0)
            {
                MessageBox.Show("请选择数据！");
                return;
            }

            API.View_ASPRepairInfo model = searchGrid.SelectedItem as API.View_ASPRepairInfo;
            if (model.RpState != "待审计")
            {
                MessageBox.Show("单号 " + model.OldID + "处于" + model.RpState + "状态，无法审计！");
                return;
            }
            model.AuditLowMoney = Convert.ToDecimal(lowMoney.Value);
            model.AuditMoney = Convert.ToDecimal(auditMoney.Value);
            model.AuditNote = auditNote.Text.Trim();

            API.ASP_AduitInfo audit = new API.ASP_AduitInfo();
            audit.AduitNote = auditNote.Text.Trim();
            audit.AuditLowMoney = Convert.ToDecimal(lowMoney.Value);
            audit.AuditMoney = Convert.ToDecimal(auditMoney.Value);
            audit.OrderID = model.OrderID;

            if (MessageBox.Show("审计金额为：￥" + auditMoney.Value
                + ", 结算金额为：￥" + lowMoney.Value + ", 确定保存吗？", 
                "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,335,new object[]{ model,audit},
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }
        
        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                lowMoney.Value=0;
                auditMoney.Value=0;
                auditNote.Text = string.Empty;
                Search();
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }
        }
    }
}
