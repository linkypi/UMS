using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserMS.Common;
using UserMS.Model;
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
            errGrid.ItemsSource = errInfo;
            dealer.ItemsSource = Store.Dealers;

            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID == 93 && p.HallID != null
          );
            List<API.Sys_UserInfo> users = userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
                          (list, info) => new { op = list, user = info })
             .Join(Store.UserOp, arg => arg.op.OpID, op => op.OpID, (a, t) => new API.Sys_UserInfo()
             {
                 UserID = a.op.UserID,
                 UserName = a.user.RealName
             }).ToList();
            reper.ItemsSource = users;
            reper.SelectedIndex = -1;
            dealer.SelectedIndex = -1;
            radDataPager1.PageSize = 20;

            UserOpList = userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
                          (list, info) => new { op = list, user = info })
             .Join(Store.UserOp, arg => arg.op.OpID, op => op.OpID, (a, t) => new UserOpModel()
             {
                 ID = a.op.ID,
                 HallID = a.op.HallID,
                 OpID = a.op.OpID,
                 UserID = a.op.UserID,
                 Username = a.user.RealName,
                 opname = t.Name
             }).ToList();
            repairer.SearchButton.Click += SellerSearchEvent;
            pro_other.SearchButton.Click += SearchOther_Click;
            pro_Name.SearchEvent += SearchProName_Click;  

            flag = true;
            Search((int)pagesize.Value, SearchCompleted);
        }

        #region 选择商品名称

        private TreeViewModel Exist(string name, List<TreeViewModel> models)
        {
            foreach (var item in models)
            {
                if (item.Title == name)
                {
                    return item;
                }
            }
            return null;
        }

        private void SearchProName_Click(object sender, RoutedEventArgs e)
        {
            List<TreeViewModel> treeModels = new List<TreeViewModel>();
            List<SlModel.BaseModel> list = new List<SlModel.BaseModel>();
            var query = (from b in Store.ProInfo
                         join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                         join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                         orderby c.ClassName
                         where c.ClassID == 128 || c.ClassID == 129
                         select new
                         {
                             ProID = b.ProID,
                             ProName = b.ProName,
                             ProFormat = b.ProFormat,
                             NeedIMEI = b.NeedIMEI,
                             TypeName = d.TypeName,
                             d.TypeID
                         }).ToList();
            foreach (var item in query)
            {
                SlModel.BaseModel bm = new SlModel.BaseModel();
                bm.ProID = item.ProID;
                bm.ProName = item.ProName;
                bm.ProFormat = item.ProFormat;
                bm.NeedIMEI = item.NeedIMEI;
                bm.TypeName = item.TypeName;
                bm.Pro_TypeID = item.TypeID.ToString();

                TreeViewModel p2 = null;
                if ((p2 = Exist(item.TypeName, treeModels)) == null)
                {
                    p2 = new TreeViewModel();
                    p2.Fields = new string[] { "TypeName", "Pro_TypeID" };
                    p2.Values = new object[] { item.TypeName, item.TypeID.ToString() };
                    p2.ID = item.TypeID.ToString();
                    p2.Title = item.TypeName;
                    treeModels.Add(p2);
                }

                list.Add(bm);
            }

            SingleSelecter msFrm = new SingleSelecter(
             treeModels,
             list, "Pro_TypeID", "ProName",
             new string[] { "TypeName", "ProName", "ProFormat" },
             new string[] { "商品品牌", "商品名称", "属性" });
            msFrm.Closed += ss_Closed;
            msFrm.ShowDialog();

        }

        void ss_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter selecter = sender as SingleSelecter;

            if (selecter.DialogResult == true)
            {
                SlModel.BaseModel pro = selecter.SelectedItem as SlModel.BaseModel;
                pro_col.Text = pro.ProFormat;
                pro_type.Text = pro.TypeName;
                pro_Name.txt.Text = pro.ProName;

                pro_Name.Tag = pro.ProID;
                repairer.TextBox.Tag = new List<int>() { Convert.ToInt32(pro.Pro_TypeID) };
            }
        }

        #endregion 

        List<API.ASP_ErrorInfo> errInfo = new List<API.ASP_ErrorInfo>();
        private ROHallAdder hadder = null;
        private int pageIndex = 0;
        private bool flag = false;
        List<API.View_ASPReceiveInfo> models = new List<API.View_ASPReceiveInfo>();

        private int menuid = 328;

        private void Button_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search((int)pagesize.Value, SearchCompleted);
            }
        }

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        #region 查  询

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            Search((int)pagesize.Value, SearchCompleted);
        }

        private void Ckb_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                Search((int)pagesize.Value,SearchCompleted);
            }
        }

        private void Search(int pageSize , EventHandler<API.MainCompletedEventArgs> completed)
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageSize = pageSize;
            rpp.PageIndex = radDataPager1.PageIndex;

            rpp.ParamList = new List<API.ReportSqlParams>();

            if (dealer.SelectedIndex >= 0)
            {
                API.ReportSqlParams_String d = new API.ReportSqlParams_String();
                d.ParamName = "Dealer";
                d.ParamValues = dealer.SelectedValue.ToString();
                rpp.ParamList.Add(d);
            }
            //reper

            if (reper.SelectedIndex >= 0)
            {
                API.ReportSqlParams_String d = new API.ReportSqlParams_String();
                d.ParamName = "Repairer";
                d.ParamValues = reper.SelectedValue.ToString();
                rpp.ParamList.Add(d);
            }
            if (!string.IsNullOrEmpty(this.hall.Tag + ""))
            {
                API.ReportSqlParams_ListString halls = new API.ReportSqlParams_ListString();
                halls.ParamName = "HallID";
                if (hall.Tag == null)
                {
                    halls.ParamValues = new List<string>();
                }
                else
                {
                    halls.ParamValues = hall.Tag.ToString().Split(',').ToList();
                }
                rpp.ParamList.Add(halls);
            }
            if (state.SelectedIndex!=0)
            {
                API.ReportSqlParams_String repstate = new API.ReportSqlParams_String();
                repstate.ParamName = "RpState";
                object obj =  (state.SelectedItem as ComboBoxItem).Content;
                repstate.ParamValues = obj==null?"":obj.ToString();
                rpp.ParamList.Add(repstate);
            }
            if (sTime.SelectedDate != null)
            {
                API.ReportSqlParams_DataTime date = new API.ReportSqlParams_DataTime();
                date.ParamName = "StartTime";
                date.ParamValues = sTime.SelectedDate;
                rpp.ParamList.Add(date);
            }
            if (endTime.SelectedDate != null)
            {
                API.ReportSqlParams_DataTime date = new API.ReportSqlParams_DataTime();
                date.ParamName = "EndTime";
                date.ParamValues = endTime.SelectedDate;
                rpp.ParamList.Add(date);
            }
            //StartTime  EndTime

            //API.ReportSqlParams_Bool del = new API.ReportSqlParams_Bool();
            //del.ParamName = "Delete";
            //del.ParamValues = true;
            //rpp.ParamList.Add(del);
            

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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 332, new object[] { rpp }, completed);

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
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                if (pageParam == null) { return; }
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
                Search((int)pagesize.Value, SearchCompleted);
            }
        }

        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                radDataPager1.PageSize = (int)pagesize.Value;
                Search((int)pagesize.Value, SearchCompleted);
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

            repairer.Tag ="";
            this.repairer.TextBox.SearchText = "";


            API.View_ASPReceiveInfo model = searchGrid.SelectedItem as API.View_ASPReceiveInfo;

            changErrs.Text = model.ChangPros;
            pro_Name.txt.Text = model.Pro_Name;
            pro_Name.Tag = model.ProID;
            var pro = from p in Store.ProInfo
                      join b in Store.ProTypeInfo
                      on p.Pro_TypeID equals b.TypeID
                      select new
                      {
                          b.TypeName,
                          p.ProFormat
                      };
            if (pro.Count() > 0)
            {
                pro_col.Text = pro.First().ProFormat;
                pro_type.Text = pro.First().TypeName;
            }
            bjPosition.Text = model.BJPosition;
            this.hallName.Text = model.HallName;
            sendRer.Text = model.Sender;
            repairer.TextBox.SearchText = model.Repairer;

            if (model.Pro_BuyT == null)
            {
                pro_BuyT.DateTimeText = "";
            }
            else
            {
                pro_BuyT.SelectedDate = model.Pro_BuyT;
            }

            facName.Text = model.厂家名称;
            facListID.Text = model.送厂批次;
            facNote.Text =   model.ToFacNote;
            toFacUser.Text = model.送厂人;
            toFacDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", model.送厂时间); ;
            backListID.Text= model.返厂批次;
            newIMEI.Text =  model.NewIMEI;
            newSN.Text =    model.NewSN;
            backNote.Text = model.返厂备注;
            backUser.Text = model.返厂人;
            backDate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", model.返厂日期);

            repairnote.Text = model.RepairNote;
            position.Text = model.Position;
            sendRer_phone.Text = model.Sender_Phone;
            repairedCount.Text = model.RepairCount==null?"": model.RepairCount.ToString();
            receiver.Text = model.Receiver2;
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
            pro_BuyT.SelectedDate = Convert.ToDateTime(model.Pro_BuyT);
            pro_col.Text = model.ProFormat;
            pro_Error.Text = model.Pro_Error;
            this.headerIMEI.Text = model.Pro_HeaderIMEI;
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
                errInfo.AddRange(list1);
                //errGrid.ItemsSource = list1;
                errGrid.Rebind();

                List<API.View_BJModels> list = e.Result.ArrList[0] as List<API.View_BJModels>;
                bjGrid.ItemsSource = list;
                bjGrid.Rebind();

                API.ASP_CallBackInfo cb = e.Result.ArrList[1] as API.ASP_CallBackInfo;
                if(cb!=null)
                {
                    answ.Text = cb.Answer;
                    lowMoney.Text = Convert.ToString(cb.RealMoney);
                    cusSug.Text = cb.Suggest;
                    cbnote.Text = cb.Note;
                    if (cb.SysDate != null)
                    {
                        cbdate.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", cb.SysDate);
                    }

                }
            }

        }
     
        #endregion

        #region 指定维修师
        private List<UserOpModel> UserOpList = new List<UserOpModel>();

        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.RepairerHallTree(),
                UserOpList, "HallID",
                                    "Username", new string[] { "Username", "opname" },
                                    new string[] { "用户名", "职位" });
            w.Closed += SellerSearchWindowClose;
            w.ShowDialog();
        }

        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    UserOpModel selected = (UserOpModel)window.SelectedItem;
                    API.Sys_UserInfo u = Store.UserInfos.Where(a => a.UserID == selected.UserID).First();
                    if (u != null)
                    {
                        repairer.Tag = selected.UserID;
                        this.repairer.TextBox.SearchText = u.RealName;
                    }
                }
            }
        }

        #endregion 

        #region  选择随机附件

        private void SearchOther_Click(object sender, RoutedEventArgs e)
        {
            MultSelecter2 mul = new MultSelecter2(null, Store.ProOthers, "",
              new string[] { "ID", "Name" }, new string[] { "编码", "附件名称" });
            mul.Closed += mul3_Closed;
            mul.ShowDialog();
        }

        private void mul3_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;

            if (selecter.DialogResult == true)
            {
                List<UserMS.API.ASP_ProOther> phList = selecter.SelectedItems.OfType<API.ASP_ProOther>().ToList();
                string msg = "";
                int index = 1;
                foreach (var item in phList)
                {
                    msg += item.Name;
                    if (index < phList.Count)
                    {
                        msg += " , ";
                    }
                    index++;
                }
                pro_other.Text = msg;
            }
        }

        #endregion 

        void Clear()
        {
            facName.Text = string.Empty;
            facListID.Text = string.Empty;
            facNote.Text = string.Empty;
            toFacUser.Text = string.Empty;
            toFacDate.Text = string.Empty;
            backListID.Text = string.Empty;
            newIMEI.Text = string.Empty;
            newSN.Text = string.Empty;
            backNote.Text = string.Empty;
            backUser.Text = string.Empty;
            backDate.Text = string.Empty;
            
            bjPosition.Text = string.Empty;
            this.hallName.Text = "";
            pro_other.Text = string.Empty;
            repairedCount.Text = "";
            receiver.Text = "";
            repairer.TextBox.SearchText = string.Empty;
            oldID.Text = "";
            vipIMEI.Text = "";
            cus_addr.Text = "";
            changErrs.Text = null;
            cus_cpc.Text = "";
            cus_email.Text = "";
            cusName.Text = "";
            cusPhone.Text = "";
            cus_phone2.Text = "";
            cus_type.Text = "";
            pro_bill.Text = "";
            pro_BuyT.DateTimeText = "";
            pro_col.Text = "";
            pro_Error.Text = "";
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

            errInfo.Clear();
            errGrid.Rebind();
            bjGrid.ItemsSource = null;
            bjGrid.Rebind();
        }

        private void delChecked_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("请选择数据！");
                return;
            }
            List<API.View_ASPReceiveInfo> list = new List<API.View_ASPReceiveInfo>();

            //foreach (var item in searchGrid.SelectedItems)
            //{
            //    API.View_ASPReceiveInfo ord = item as API.View_ASPReceiveInfo;

            //    list.Add(ord);
            //}
        
            API.View_ASPReceiveInfo ord = searchGrid.SelectedItem as API.View_ASPReceiveInfo;
            ord.DelNote = delnote.Text.Trim();
            list.Add(ord);
            if (string.IsNullOrEmpty(delnote.Text))
            {
                System.Windows.MessageBox.Show("请输入删除备注！"); return;
            }

            if (System.Windows.MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
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
                System.Windows.MessageBox.Show(e.Result.Message);
                Clear();
                Search((int)pagesize.Value, SearchCompleted);
            }
            else
            {
                System.Windows.MessageBox.Show(e.Result.Message);
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
           // PrintReceive print = new PrintReceive(new List<API.View_ASPReceiveInfo>() { model });
            print.SrcPage = "/Views/AfterSale/ReceiveSearch.xaml";
            this.NavigationService.Navigate(print);
        }

        #region  导出

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void export_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Search(100000,GetSumCompleted);
        }

        private void GetSumCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_ASPReceiveInfo> list = pageParam.Obj as List<API.View_ASPReceiveInfo>;
                SlModel.operateExcel<API.View_ASPReceiveInfo> excel = new SlModel.operateExcel<API.View_ASPReceiveInfo>();

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "xls";
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "xls");
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog()==DialogResult.OK)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        List<string> strs = new List<string>() { 
                       "受理单号",  "状态",  "维修师","受理人", "手工单号",
                       "客户姓名",  "手机号码", "会员卡号", "服务网点",
                       "商品品牌",  "商品名称", "商品属性",
                       "串码" , "受理日期" ,"接机日期",
                        "维修完成时间",  "需送厂" ,
                        "送厂时间", "送厂人" , "送厂批次",
                        "厂家名称", "返厂人" , "返厂批次",
                        "返厂备注", "返厂日期", "质检日期",
                        "质检人",  "质检备注", "审核人", 
                        "审核备注", "审核日期", "备机押金",
                        "配件费", "劳务费", "实收",
                        "应收",  "挂账类型", "挂账金额",
                        "取机人" , "取机备注", "取机日期",
                        "回访人" , "回访日期", "审计人" ,
                        "审计金额", "结算金额", "审计备注",
                        "审计日期",  "撤销人",  "撤销时间",
                        "撤销备注"};

                        List<string> fields = new List<string>() { 
                        "ServiceID", "RpState","Repairer","Receiver2",
                        "OldID", "Cus_Name", "Cus_Phone", "IMEI",
                        "HallName", "Pro_Type ","Pro_Name", "ProFormat",
                        "Pro_HeaderIMEI","SysDate","接机日期",
                        "维修完成时间",  "需送厂" ,
                        "送厂时间", "送厂人" , "送厂批次",
                        "厂家名称", "返厂人" , "返厂批次",
                        "返厂备注", "返厂日期", "质检日期",
                        "质检人",  "质检备注", "审核人", 
                        "审核备注", "审核日期", "备机押金",
                        "配件费", "劳务费", "实收",
                        "应收",  "挂账类型", "挂账金额",
                        "取机人" , "取机备注", "取机日期",
                        "回访人" , "回访日期", "审计人" ,
                        "审计金额", "结算金额", "审计备注",
                        "审计日期",  "撤销人",  "撤销时间",
                        "撤销备注"
                        
                        };
                        excel.getExcel(list, strs, fields, stream);

                        System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出完成！");
                        this.isbusy.IsBusy = false;
                    }
                }

            }
            else
            {
                this.isbusy.IsBusy = false;
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message);
            }
        }

        #endregion 

        #region  保存

        private void save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("请选择需保存的数据！"); return;
            }
            API.View_ASPReceiveInfo model = searchGrid.SelectedItem as API.View_ASPReceiveInfo;
            model.OldID = oldID.Text;
            model.Cus_Name = cusName.Text;
            model.Cus_Phone = cusPhone.Text;
            model.Cus_Phone2 = cus_phone2.Text;
            model.Cus_Add = cus_addr.Text;
            model.Pro_Name = pro_Name.txt.Text;
            model.Position = position.Text ;
            //model.Pro_BuyT = pro_BuyT.SelectedDate;
            model.Pro_Bill = pro_bill.Text;
            if (pro_Name.Tag == null)
            {
                System.Windows.MessageBox.Show("请选择商品！");
                return;
            }
            
            model.ProID = pro_Name.Tag.ToString();
            model.Pro_Type = pro_type.Text;
            model.ProFormat = pro_col.Text;
            model.Pro_Other = pro_other.Text;
            model.Pro_OutSide = pro_outside.Text;
            //model.Errors = 
            if (string.IsNullOrEmpty(repairer.TextBox.SearchText))
            {
                System.Windows.MessageBox.Show("请指定维修师！");
                return;
            }
            var user = Store.UserInfos.Where(u => u.UserName == repairer.TextBox.SearchText).First();
            if (user == null)
            {
                System.Windows.MessageBox.Show("指定维修师不存在！");
                return;
            }
            model.Repairer = user.UserID;
            model.Pro_Note = pro_note.Text;

            PublicRequestHelp p = new PublicRequestHelp(this.isbusy, 380, new object[] { model,errInfo }, SaveCompleted);
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            System.Windows.MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Search((int)pagesize.Value, SearchCompleted);
            }
        }

        #endregion 

        #region  故障操作

        /// <summary>
        /// 添加故障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addErr_Click(object sender, RoutedEventArgs e)
        {
            List<TreeViewModel> list = new List<TreeViewModel>();
            foreach (var item in Store.ErrorTypes)
            {
                TreeViewModel p = new TreeViewModel();
                p.Fields = new string[] { "TypeID" };
                p.Values = new object[] { item.ID };
                p.NewID = item.ID;
                p.Title = item.Name;
                list.Add(p);

            }
            MultSelecter2 msFrm = new MultSelecter2(
              list,
              Store.ErrorInfo, "ErrorName",
              new string[] { "ErrorID", "ErrorName" },
              new string[] { "编码", "故障名称" });
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();
        }

        private void msFrm_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            List<API.ASP_ErrorInfo> piList = ((UserMS.MultSelecter2)sender).SelectedItems.OfType<API.ASP_ErrorInfo>().ToList();
            if (piList.Count == 0) return;

            errInfo.AddRange(piList.Where(p => !errInfo.Contains(p)));
            errGrid.Rebind();
        }

        /// <summary>
        /// 删除故障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delErr_Click(object sender, RoutedEventArgs e)
        {
            if (errGrid.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("请选择需删除的数据！");
                return;
            }
            foreach (var item in errGrid.SelectedItems)
            {
                errInfo.Remove(item as API.ASP_ErrorInfo);
            }
            errGrid.Rebind();
        }

        #endregion 
    }
}
