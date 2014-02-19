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
using Telerik.Windows.Controls.Primitives;
using UserMS.Common;
using UserMS.Model;
using UserMS.MyControl;
using UserMS.Report.Print.RepairPrint;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// Receive.xaml 的交互逻辑
    /// </summary>
    public partial class Receive : Page
    {
        List<API.View_BJModels> bjModels = new List<API.View_BJModels>();
       // List<API.Pro_ProInfo> pros = new List<API.Pro_ProInfo>();

        List<API.ASP_ErrorInfo> errInfo = new List<API.ASP_ErrorInfo>();

        ProDetailAdder<API.View_BJModels> adder = null;
        HallFilter hadder = null;
        string menuid = "323";
        bool flag = false;

        private List<UserOpModel> UserOpList = new List<UserOpModel>();

        public Receive()
        {
            InitializeComponent();
            bjGrid.ItemsSource = bjModels;

            checkinfo.ItemsSource = errInfo;

            bj_date.DateTimeText = DateTime.Now.ToShortDateString();

            chk_inOut.IsEnabled = false;
            bjHall.TextBox.IsEnabled = false;
            bjHall.SearchButton.IsEnabled = false;
            hadder = new HallFilter(menuid, false, bjHall, new EventHandler<MyEventArgs>(hadder_AddCompleted));
            bjHall.SearchButton.Click += SearchButton_Click;
            
            var h = (from a in Store.ProHallInfo
                    where a.HallID == Store.LoginRoleInfo.Sys_Role_Menu_HallInfo.First().HallID
                    select a).ToList().First();
            hallid.Text = h.HallName;
            hallid.Tag = h.HallID;

            //bjHall.Text = h.HallName;
            //bjHall.Tag = h.HallID;

            receiver.Text = Store.LoginUserName;

            chk_fid.ItemsSource = Store.CheckInfo;
            chk_fid.SelectedIndex = 0;

            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
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
            var userinfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model => model.UserID,
                                              (info, model) => info).ToList();
            this.repairer.ItemsSource = userinfos;
            this.repairer.TextSearchPath = "RealName";
            this.repairer.SearchEvent = SellerSearchEvent;
            this.repairer.SelectionMode = AutoCompleteSelectionMode.Single;

            flag = true;
            cus_cpc.IsReadOnly = true;
            sendRer.IsReadOnly = true;
            sendRer_phone.IsReadOnly = true;

            //List<string> arr = new List<string>() { "白","黑","橙",
            //"粉","红","黄","灰","金","蓝","绿","银","紫","棕","桃红","铂银"};

            //List<SlModel.CkbModel> colors = new List<SlModel.CkbModel>() ;
            //foreach (var item in arr)
            //{
            //   SlModel.CkbModel sm =  new SlModel.CkbModel(false,item);
            //   colors.Add(sm);
            //}
            //pro_col.ItemsSource = colors;
            //pro_col.SelectedIndex = 0;
            pro_Name.SearchButton.Click+=SearchProName_Click;  
            pro_type.SearchButton .Click+=SearchProType_Click;
            pro_other.SearchButton.Click+=SearchOther_Click;
            //pro_Error.SearchButton.Click+=SearchError_Click;
        }

        #region   选择详细故障

        private void SearchError_Click(object sender, RoutedEventArgs e)
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
            msFrm.Closed += mul_Closed;
            msFrm.ShowDialog();

        }

        void mul_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;
        
            if (selecter.DialogResult == true)
            {
                List<UserMS.API.ASP_ErrorInfo> phList = selecter.SelectedItems.OfType<API.ASP_ErrorInfo>().ToList();
                string msg = "";
             
                int index = 0;
                foreach (var item in phList)
                {
                    msg += item.ErrorName;
                    if (index < phList.Count) { msg += " , "; }
                    index++;
                }
                //pro_Error.TextBox.SearchText = msg;
               
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
                pro_other.TextBox.SearchText = msg;                                                               
            }
        }

        #endregion 

        #region 选择商品品牌

        private void SearchProType_Click(object sender, RoutedEventArgs e)
        {
            SingleSelecter ss2 = new SingleSelecter(null, Store.ProTypeInfo, "", "TypeName",
             new string[] { "TypeID", "TypeName" },
             new string[] { "编码", "商品品牌" });
            ss2.Closed += ss2_Closed;
            ss2.ShowDialog();
        }

        void ss2_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.SingleSelecter selecter = sender as UserMS.SingleSelecter;

            if (selecter.DialogResult == true)
            {
                API.Pro_TypeInfo pro = selecter.SelectedItem as API.Pro_TypeInfo;
                pro_type.TextBox.SearchText = pro.TypeName;
            }

        }
        
        #endregion 

        #region 选择商品名称

        private void SearchProName_Click(object sender, RoutedEventArgs e)
        {
            List<API.Pro_ProMainInfo> list = new List<API.Pro_ProMainInfo>();
            foreach (var item in Store.ProMainInfo)
            {
                if (list.Where(p => p.ProMainName == item.ProMainName).Count() == 0)
                {
                    list.Add(item);
                }
            }
            SingleSelecter ss = new SingleSelecter(null, list, "", "ProMainName",
             new string[] { "ProMainID", "ProMainName" },
             new string[] { "商品编码", "商品名称" });
            ss.Closed += ss_Closed;
            ss.ShowDialog();
        }

        void ss_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.SingleSelecter selecter = sender as UserMS.SingleSelecter;
           
              if (selecter.DialogResult == true)
              {
                  API.Pro_ProMainInfo pro = selecter.SelectedItem as API.Pro_ProMainInfo;
                  pro_Name.TextBox.SearchText = pro.ProMainName;
              }

        }

        #endregion 

        #region 指定维修师

        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
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
                    repairer.Tag = selected.UserID;
                    this.repairer.TextBox.SearchText = selected.Username;

                }
            }

        }

        #endregion 

        private void hadder_AddCompleted(object sender, MyEventArgs e)
        {
            PublicRequestHelp peh = new PublicRequestHelp(this.isbusy, 318, new object[] { e.Value == null ? "" : e.Value.ToString(),true },
            new EventHandler<API.MainCompletedEventArgs>(GetProCompleted));
        }

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hadder.GetHall(hadder.FilterHall( Store.ProHallInfo));
        }

        private void GetProCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                List<API.BJModel> pros = e.Result.Obj as List<API.BJModel>;
                adder = new ProDetailAdder<API.View_BJModels>(bjModels, bjGrid, pros, new EventHandler<MyEventArgs>(GetBJCompleted));
            }
            else
            {
                MessageBox.Show("初始化备机商品有误！");
            }
        }

        #region 备机

        private void GetBJCompleted(object sender, MyEventArgs e)
        {
            foreach (var item in bjModels)
            {
                item.ProCount = 1;
            }
            bjGrid.Rebind();
        }
        private void delBJ_Click(object sender, RoutedEventArgs e)
        {
            if (bjGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的商品！");
                return;
            }

            foreach (var item in bjGrid.SelectedItems)
            {
                bjModels.Remove(item as API.View_BJModels);
            }
            bjGrid.Rebind();
        }

        private void addBJ_Click(object sender, RoutedEventArgs e)
        {
            if (adder == null)
            {
                adder = new ProDetailAdder<API.View_BJModels>(bjModels, bjGrid, new List<API.BJModel>(),
                    new EventHandler<MyEventArgs>(GetBJCompleted));
            }
            adder.Add();
        }

        #endregion 

        #region  保存

        private void receive_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            #region  验证主板是否在维修中


            API.WebReturn ret = Store.wsclient.Main(339, new List<object>() { headerIMEI.Text.Trim() });
          
            if (Convert.ToInt32(ret.Obj) == 1)
            {
                MessageBox.Show("该主板维修中，无法再次受理！");
                return;
            }
         
            #endregion 

            #region  验证指定维修员

            List<API.Sys_UserInfo> users = repairer.ItemsSource as List<API.Sys_UserInfo>;

            var usr = from  a in users 
                      where a.RealName == repairer.Text
                      select a;

            if (usr.Count() > 0)
            {
                repairer.SelectedItem = usr.First();
            }
            else
            {
                MessageBox.Show("维修师不存在！");
                return;
            }

            #endregion 

            if (!Validate())
            {
                return;
            }

            if (MessageBox.Show("确定提交吗？","提示",MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            API.ASP_ReceiveInfo rec = new API.ASP_ReceiveInfo();

            #region 

            rec.HallID = hallid.Tag.ToString();
            rec.OldID = oldID.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(vipIMEI.Text))
            {
                rec.Cus_VIPID = Convert.ToInt32(vipIMEI.Tag.ToString());
            }
            rec.Cus_Add = cus_addr.Text;
            API.Sys_UserInfo user =  repairer.SelectedItem as API.Sys_UserInfo;
            if (user == null) { MessageBox.Show("请选择指定维修师！"); return; }
            rec.Repairer = user.UserID;
            rec.Cus_CPC = cus_cpc.Text;
            rec.Cus_CusType = (cus_type.SelectedItem as ComboBoxItem).Content.ToString();
            rec.Cus_Email = cus_email.Text;
            rec.Cus_Name = cus_name.Text;
            rec.Cus_Phone = cus_phone.Text;
            rec.RepairCount = Convert.ToInt32((repairedCount.Tag??0));
            rec.Cus_Phone2 = cus_phone2.Text;
            rec.Pro_Bill = pro_bill.Text;
            rec.Pro_BuyT = pro_BuyT.SelectedDate;
            rec.Pro_Color = pro_col.Text;
            //rec.Pro_Error = pro_Error.TextBox.SearchText;
            rec.Pro_GetT = pro_GetT.SelectedDate;
            rec.Pro_HeaderIMEI = headerIMEI.Text;
            //rec.Pro_IMEI = IMEI.Text;
            rec.Pro_Name = pro_Name.TextBox.SearchText;
            rec.Pro_Note = pro_note.Text;
            rec.Pro_Other = pro_other.TextBox.SearchText;
            rec.Pro_OutSide = pro_outside.Text;
            rec.Pro_Seq = (pro_seq.SelectedItem as ComboBoxItem).Content.ToString();
            rec.Pro_SN = pro_sn.Text;
            rec.Pro_Type = pro_type.Text;
            rec.Receiver = receiver.Text;
            rec.Sender = sendRer.Text;
            rec.Sender_Phone = sendRer_phone.Text;

            if (bjModels.Count > 0)
            {
                rec.BJ_Date = bj_date.SelectedDate;
                rec.BJ_HallID = bjHall.Tag.ToString();
                rec.BJ_Money = Convert.ToDecimal(bj_money.Value);
                rec.BJ_UserID = bj_userid.Text;
            }
            rec.Chk_FID = (chk_fid.SelectedItem as API.ASP_CheckInfo).ID;
            rec.Chk_InOut = chk_inOut.Text;
            rec.Chk_Note = chk_note.Text;
            rec.Chk_price = Convert.ToDecimal(chk_price.Value);
            rec.ASP_CurrentOrder_BackupPhoneInfo = new List<API.ASP_CurrentOrder_BackupPhoneInfo>();

            #endregion 

            //添加备机信息
            rec.HasBJ =bjModels.Count == 0? false:true  ;
            foreach (var item in bjModels)
	        {
                API.ASP_CurrentOrder_BackupPhoneInfo m = new API.ASP_CurrentOrder_BackupPhoneInfo();
                m.IMEI = item.IMEI;
                m.InListID = item.InListID;
                m.ProCount = 1;
                m.ProID = item.ProID;
                rec.ASP_CurrentOrder_BackupPhoneInfo.Add(m);
	        }

            //添加故障信息
           // List<string> arrs = pro_Error.TextBox.SearchText.Split(",".ToArray()).ToList();
            //var et = from o in Store.et
            rec.ASP_CurrentOrder_ErrorInfo = new List<API.ASP_CurrentOrder_ErrorInfo>();
            foreach (var item in errInfo)
            {
                API.ASP_CurrentOrder_ErrorInfo er = new API.ASP_CurrentOrder_ErrorInfo();
                er.ErrorID = item.ID;
                rec.ASP_CurrentOrder_ErrorInfo.Add(er);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,320,new object[]{rec },
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                Clear();
                if (MessageBox.Show("保存成功，是否需要打印？","提示",MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }
               // NavigationService.GetNavigationService(this).Navigate(new Uri("Report.Print.RepairPrint.PrintRepairBill.xaml", UriKind.Relative));
                PrintRepairBill print = new PrintRepairBill(e.Result.Obj as List<API.View_ASPReceiveInfo> );
                print.SrcPage = "/Views/AfterSale/Receive.xaml";
                this.NavigationService.Navigate(print);
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }
        }

        private void Clear()
        {
            errInfo.Clear();
            checkinfo.Rebind();
            bjModels.Clear();
            bjGrid.Rebind();

            repairer.Tag = null ;
            this.repairer.TextBox.SearchText = "";
            sendRer.Text = string.Empty;
            sendRer_phone.Text = string.Empty;
            repairedCount.Text = "";
            oldID.Text = string.Empty;
            vipIMEI.Text = string.Empty;
            cus_name.Text = string.Empty;
            cus_addr.Text = string.Empty;
            cus_cpc.Text = string.Empty;
            cus_email.Text = string.Empty;
            cus_name.Text = string.Empty;
            cus_phone.Text = string.Empty;
            cus_phone2.Text = string.Empty;
            pro_Name.TextBox.SearchText = string.Empty;
            pro_other.TextBox.SearchText = string.Empty;
            pro_outside.Text = string.Empty;
            //pro_Error.TextBox.SearchText = string.Empty;
            //cus_type.Text = string.Empty;

            headerIMEI.Text = string.Empty;
           // IMEI.Text = string.Empty;
            pro_bill.Text = string.Empty;
            pro_BuyT.DateTimeText = string.Empty;
            pro_col.Text = string.Empty;
            //pro_Error.Text = string.Empty;
            pro_GetT.DateTimeText = string.Empty;
           
            pro_note.Text = string.Empty;
            pro_other.Text = string.Empty;
            pro_outside.Text = string.Empty;
            pro_sn.Text = string.Empty;
            pro_type.TextBox.SearchText = string.Empty;

            chk_price.Value = 0;
            chk_note.Text = string.Empty;

            bj_money.Value = 0;
           // bj_date.DateTimeText = "";
            bj_userid.Text = string.Empty;

            this.isbusy.IsBusy = false;
            cus_addr.IsReadOnly = false;
            this.cus_email.IsReadOnly = false;
            cus_name.IsReadOnly = false;
            cus_phone.IsReadOnly = false;
            cus_phone2.IsReadOnly = false;
           
        }

        #endregion

        /// <summary>
        /// 数据验证
        /// </summary>
        private bool Validate()
        {

            #region   客户信息


            if (string.IsNullOrEmpty(oldID.Text))
            {
                MessageBox.Show("手工单号不能为空！");
                return false;
            }


            if ((cus_type.SelectedItem as ComboBoxItem).Content.ToString() == "最终用户")
            {
                if (string.IsNullOrEmpty(this.cus_name.Text))
                {
                    MessageBox.Show("客户姓名不能为空！");
                    return false;
                } if (string.IsNullOrEmpty(this.cus_phone.Text))
                {
                    MessageBox.Show("联系电话不能为空！");
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.cus_cpc.Text))
                {
                    MessageBox.Show("送修单位不能为空！");
                    return false;
                }
                if (string.IsNullOrEmpty(this.sendRer.Text))
                {
                    MessageBox.Show("送修人不能为空！");
                    return false;
                }
                if (string.IsNullOrEmpty(this.sendRer_phone.Text))
                {
                    MessageBox.Show("送修人电话不能为空！");
                    return false;
                }
            }

            #endregion

            #region  产品信息

            if (string.IsNullOrEmpty(this.headerIMEI.Text))
            {
                MessageBox.Show("串码不能为空！");  //机头串码
                return false;
            }
            //if (string.IsNullOrEmpty(this.IMEI.Text))
            //{
            //    MessageBox.Show("主板串码不能为空！");
            //    return false;
            //}
            if (string.IsNullOrEmpty(pro_type.TextBox.SearchText))
            {
                MessageBox.Show("产品品牌不能为空！");
                return false;
            }
            if (string.IsNullOrEmpty(this.pro_sn.Text))
            {
                MessageBox.Show("主板SN不能为空！");
                return false;
            }
            if (string.IsNullOrEmpty(this.pro_Name.TextBox.SearchText))
            {
                MessageBox.Show("产品名称不能为空！");
                return false;
            }
            if (string.IsNullOrEmpty(this.pro_col.Text))
            {
                MessageBox.Show("产品颜色不能为空！");
                return false;
            }
            //if (string.IsNullOrEmpty(pro_type.Text))
            //{
            //    MessageBox.Show("商品类别不能为空！");
            //    return false;
            //}
       

            //if (string.IsNullOrEmpty(this.pro_other.Text))
            //{
            //    MessageBox.Show("随机附件不能为空！");
            //    return false;
            //}
            if (string.IsNullOrEmpty(this.pro_outside.Text))
            {
                MessageBox.Show("外观描述不能为空！");
                return false;
            }

            //if (string.IsNullOrEmpty(this.pro_Error.TextBox.SearchText))
            //{
            //    MessageBox.Show("详细故障不能为空！");
            //    return false;
            //}
            #endregion 

            if ((chk_fid.SelectedItem as API.ASP_CheckInfo).ChkName == "非保")
            {
                if (this.chk_price.Value == 0)
                {
                    MessageBox.Show("客户限价不能为0！");
                    return false;
                }
            }
    
            //受理和维修都可以添加备机  受理录入之后维修不可再录入
            //if (bjModels.Count() == 0)
            //{
            //    MessageBox.Show("请添加备机数据！");
            //    return false;
            //}
            //foreach (var item in bjModels)
            //{
            //    if (string.IsNullOrEmpty(item.IMEI))
            //    {
            //        MessageBox.Show("请输入备机串码！");
            //        return false;
            //    }
            //}
            //if (errInfo.Count == 0)
            //{
            //    MessageBox.Show("请添加初检故障信息！");
            //    return false;
            //}

            if (repairer.Tag == null && string.IsNullOrEmpty(this.repairer.Text))
            {
                MessageBox.Show("请选择指定维修师！");
                return false;
            }


            return true;
        }

        private void vipIMEI_LostFocus(object sender, RoutedEventArgs e)
        {
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,319,new object[]{ vipIMEI.Text.Trim() },
                new EventHandler<API.MainCompletedEventArgs>(GetVipInfo));
        }

        private void GetVipInfo(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            cus_addr.IsReadOnly = false;
            this.cus_email.IsReadOnly = false;
            cus_name.IsReadOnly = false;
            cus_phone.IsReadOnly = false;
            cus_phone2.IsReadOnly = false;

            cus_addr.Text = string.Empty;
            this.cus_email.Text = string.Empty;
            cus_name.Text = string.Empty;
            cus_phone.Text = string.Empty;
            cus_phone2.Text = string.Empty;
            vipIMEI.Tag = string.Empty;

            if (e.Result.ReturnValue)
            {
                API.VIP_VIPInfo vip = e.Result.Obj as API.VIP_VIPInfo;
                cus_addr.Text = vip.Address;
                this.cus_email.Text = vip.Email;
                cus_name.Text = vip.MemberName;
                cus_phone.Text = vip.MobiPhone;
                cus_phone2.Text = vip.TelePhone;
                vipIMEI.Tag = vip.ID;
                cus_addr.IsReadOnly = true;
                this.cus_email.IsReadOnly = true;
                cus_name.IsReadOnly = true;
                cus_phone.IsReadOnly = true;
                cus_phone2.IsReadOnly = true;

            }
           
        }

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
            checkinfo.Rebind();
        }

        /// <summary>
        /// 删除故障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delErr_Click(object sender, RoutedEventArgs e)
        {
            if (checkinfo.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需删除的数据！");
                return;
            }
            foreach (var item in checkinfo.SelectedItems)
            {
                errInfo.Remove(item as API.ASP_ErrorInfo);
            }
            checkinfo.Rebind();
        }

        #endregion 

     
        public void GetRepairedCount(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            repairedCount.Text = string.Empty;
            mulRepared.IsChecked = false;
           // repairing = false;
            if (e.Result.ReturnValue)
            {
                int state = Convert.ToInt32(e.Result.Obj);
                if (state == 1)
                {
                    //串码维修中
                   // repairing = true;
                }
                else
                {
                    int count = (int)e.Result.ArrList[0];
                    repairedCount.Text = count.ToString();
                    repairedCount.Tag = count;
                    if (count > 0)
                    {
                        mulRepared.IsChecked = true;
                    }
                }
            }
        }

        /// <summary>
        /// 检测维修次数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IMEI_LostFocus(object sender, RoutedEventArgs e)
        {
            PublicRequestHelp prh = new PublicRequestHelp(null, 339, new object[] { headerIMEI.Text.Trim() }, GetRepairedCount);
        
        }

        private void repairer_KeyUp(object sender, KeyEventArgs e)
        {
            //List<API.Sys_UserInfo> users = repairer.ItemsSource as List<API.Sys_UserInfo>;
            //foreach (var item in users)
            //{
            //    if (item.UserName == repairer.TextBox.SearchText)
            //    {
            //        repairer.SelectedItem = item;
            //        break;
            //    }
            //}
        }

        private void cus_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!flag) { return; }
            if (cus_type.SelectedItem == null) { return; }
            vipIMEI.IsReadOnly = false;
            cus_name.IsReadOnly = false;
            cus_phone.IsReadOnly = false;
            cus_phone2.IsReadOnly = false;
            cus_addr.IsReadOnly = false;
            cus_email.IsReadOnly = false;

            cus_cpc.IsReadOnly = false;
            sendRer.IsReadOnly = false;
            sendRer_phone.IsReadOnly = false;
            if ((cus_type.SelectedItem as ComboBoxItem).Content.ToString() == "最终用户")
            {
                cus_cpc.Text = string.Empty;
                sendRer.Text = string.Empty;
                sendRer_phone.Text = string.Empty;

                vipIMEI.IsReadOnly = false;
                cus_name.IsReadOnly = false;
                cus_phone.IsReadOnly = false;
                cus_phone2.IsReadOnly = false;
                cus_addr.IsReadOnly = false;
                cus_email.IsReadOnly = false;

                cus_cpc.IsReadOnly = true;
                sendRer.IsReadOnly = true;
                sendRer_phone.IsReadOnly = true;
            }
            else
            {
                vipIMEI.Text = string.Empty;
                cus_name.Text = string.Empty;
                cus_phone.Text = string.Empty;
                cus_phone2.Text = string.Empty;
                cus_addr.Text = string.Empty;
                cus_email.Text = string.Empty; 
                vipIMEI.IsReadOnly = true;
                cus_name.IsReadOnly = true;
                cus_phone.IsReadOnly = true;
                cus_phone2.IsReadOnly = true;
                cus_addr.IsReadOnly = true;
                cus_email.IsReadOnly = true;

                cus_cpc.IsReadOnly = false;
                sendRer.IsReadOnly = false;
                sendRer_phone.IsReadOnly = false;
            }
        }

        private void chk_fid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chk_fid.SelectedItem == null)
            {
                return;
            }

            if ((chk_fid.SelectedItem as API.ASP_CheckInfo).ChkName == "非保")
            {
                (chk_inOut.SelectedItem as ComboBoxItem).Content = "保外";
                chk_price.IsEnabled = true;
            }
            else
            {
                chk_price.IsEnabled = false;
                (chk_inOut.SelectedItem as ComboBoxItem).Content = "保内";
            }
        }

    }

}
