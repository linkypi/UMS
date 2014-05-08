using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell
{
    public partial class NewProSell : INotifyPropertyChanged  
    {
//        private int MethodID_VIPGetInfo = 3;
//        private int MethodID_IMEIGetInfo = 5;
        private int MethodID_SellNext = 8;
        private int AddSelllist_Method_ID = 154;
        private int GetAllSellOldID_Method_ID = 165;
        public API.VIP_VIPInfo SellVIP;
        public API.Pro_SellInfo SellInfo=new Pro_SellInfo();
        public List<NewSellListInfoGridModel> SellGridModels = new List<NewSellListInfoGridModel>();
        public List<VIP_OffTicket> VIPTicket;
        public API.Pro_HallInfo Hall;
        public object oldpage { get; set; }
//        public List<Pro_ProInfo> UserProInfos =
//            Store.ProInfo.Where(
//                o =>
//                Store.LoginRoleInfo.Sys_Role_Menu_ProInfo.Where(p => p.MenuID == 24)
//                     .Select(q => q.ProID)
//                     .Contains(o.ProID)).ToList();

        public List<Pro_ProInfo> UserProInfos =
            CommonHelper.GetPro(24);

        public NewProSell(bool NoDialog)
        {
            InitializeComponent();
#if HZ
            this.VIPBTN.Visibility = Visibility.Collapsed;
            this.VIPPOINT.Visibility = Visibility.Collapsed;
#endif
            this.Grid.ItemsSource = SellGridModels;
            this.SellTime.SelectedValue = DateTime.Now;
            //VIP_OnMouseLeftButtonUp(null, null);

            if (CommonHelper.GetHalls(103).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(103)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"权限错误");
                this.IsEnabled = false;
            }

            this.HallName.DataContext = Hall;



            //a.Start(); //FOR DEBUG
//            this.SellOldID.SelectionMode = AutoCompleteSelectionMode.Single;
//            this.SellOldID.SearchEvent = GetSellListInfo_Event;
            //PublicRequestHelp b = new PublicRequestHelp(this.PageBusy, GetAllSellOldID_Method_ID,new object[]{},GetSellOldID_Completed );
            var a = new PublicRequestHelp(this.PageBusy, AddSelllist_Method_ID, new object[] { }, GetSellList_End2);

        }

        public NewProSell()
        {
            
            InitializeComponent();
//            this.ProID.ItemsSource = Store.ProInfo;
//            this.ProID.SelectionMode = AutoCompleteSelectionMode.Single;
//            this.ProID.SearchEvent=ProIDSearchEvent;
//            this.ProID.TextBox_SelectionChanged=ProIDSelected;
//            this.ProID.DisplayMemberPath = "ProName";
            this.Grid.ItemsSource = SellGridModels;
            this.SellTime.SelectedValue = DateTime.Now;
#if HZ
            this.VIPBTN.Visibility = Visibility.Collapsed;
            this.VIPPOINT.Visibility = Visibility.Collapsed;

#else
            VIP_OnMouseLeftButtonUp(null, null);
#endif
            if (CommonHelper.GetHalls(103).Count >= 1)

            {
                Hall = CommonHelper.GetHalls(103)[0];
                this.HallName.DataContext = Hall;
                var a = new PublicRequestHelp(this.PageBusy, AddSelllist_Method_ID, new object[] { }, GetSellList_End2);
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"权限错误");
                this.IsEnabled = false;
            }

            


           
            //a.Start(); //FOR DEBUG
//            this.SellOldID.SelectionMode = AutoCompleteSelectionMode.Single;
//            this.SellOldID.SearchEvent=GetSellListInfo_Event;
            //PublicRequestHelp b = new PublicRequestHelp(this.PageBusy, GetAllSellOldID_Method_ID,new object[]{},GetSellOldID_Completed );
            

        }

        private void GetSellList_End2(object sender, MainCompletedEventArgs e)
        {
           
            this.PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {



                    List<NewSellListInfoGridModel> l = new List<NewSellListInfoGridModel>();
                    List<Pro_SellListInfo_Temp> m = e.Result.Obj as List<Pro_SellListInfo_Temp>;
                    m = m.Where(p => p.HallID == this.Hall.HallID).ToList();
                    foreach (var proSellListInfoTemp in m)
                    {
                        NewSellListInfoGridModel model = new NewSellListInfoGridModel();
                        model.selllist = proSellListInfoTemp;
                        l.Add(model);
                    }
                    foreach (var newSellListInfoGridModel in l)
                    {
                        if (this.SellGridModels.Select(p => p.selllist.ID)
                                .Contains(newSellListInfoGridModel.selllist.ID))
                        {
                            NewSellListInfoGridModel model = newSellListInfoGridModel;
                            this.Grid.Select(this.SellGridModels.Where(p => p.selllist.ID == model.selllist.ID));
                        }
                        else
                        {
                            this.SellGridModels.Add(newSellListInfoGridModel);
                        }
                    }
                    this.Grid.Rebind();
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message);
                }

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
            }
        }

        private void GetSellListInfo_Event(object sender, RoutedEventArgs e)
        {
            var a = new PublicRequestHelp(this.PageBusy, AddSelllist_Method_ID, new object[] { this.SellOldID.Text }, GetSellList_End);
       
        }


        void a_Tick(object sender, EventArgs e)
        {
            try
            {
                //Logger.Log(FocusManager.GetFocusedElement().ToString());
            }
            catch
            {
                
            }
        }

        public NewProSell(List<API.Pro_SellListInfo_Temp> selllists)
        {
            InitializeComponent();
            foreach (var proSellListInfoTemp in selllists)
            {
                NewSellListInfoGridModel m=new NewSellListInfoGridModel();
                m.selllist = proSellListInfoTemp;
                SellGridModels.Add(m);
            }
            this.Grid.ItemsSource = SellGridModels;
            this.SellTime.SelectedValue = DateTime.Now;
#if HZ
            this.VIPBTN.Visibility = Visibility.Collapsed;
            this.VIPPOINT.Visibility = Visibility.Collapsed;
#else
            VIP_OnMouseLeftButtonUp(null, null);
#endif

            if (CommonHelper.GetHalls(103).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(103)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"权限错误");
                this.IsEnabled = false;
            }

            this.HallName.DataContext = Hall;


//            //a.Start(); //FOR DEBUG
//            this.SellOldID.SelectionMode = AutoCompleteSelectionMode.Single;
//            this.SellOldID.SearchEvent = GetSellListInfo_Event;
            //PublicRequestHelp b = new PublicRequestHelp(this.PageBusy, GetAllSellOldID_Method_ID, new object[] { }, GetSellOldID_Completed);
            var a = new PublicRequestHelp(this.PageBusy, AddSelllist_Method_ID, new object[] {}, GetSellList_End2);


        }

        public NewProSell(Pro_SellInfo SellInfo, VIP_VIPInfo vipInfo)
        {
            InitializeComponent();
           // SellOldID.TextBox.SearchText = SellInfo.OldID??"";
//            SellOldID.IsEnabled = false;
            SellVIP = vipInfo;
            VIPCard.DataContext = SellVIP;
            VIPName.DataContext = SellVIP;
            VIPPhone.DataContext = SellVIP;
            VIPPoint.DataContext = SellVIP;

            if (vipInfo != null)
            {
                this.VIPName.IsEnabled = false;
                this.VIPPhone.IsEnabled = false;
            }
            this.SellTime.SelectedValue = DateTime.Now;
            if (SellVIP!=null && SellVIP.VIP_OffTicket != null) VIPTicket = SellVIP.VIP_OffTicket;
            foreach (var t in SellInfo.Pro_SellListInfo)
            {
                var l = new NewSellListInfoGridModel();
                Pro_SellListInfo_Temp info = new Pro_SellListInfo_Temp();
                info.ID = t.ID;
                info.ProID = t.ProID;
                info.ProCount = t.ProCount;
                info.SellType = t.SellType;
                info.CashTicket = t.CashTicket;
                info.ProPrice = t.ProPrice;
                info.TicketID = t.TicketID;
                info.CashPrice = t.CashPrice;
                info.IMEI = t.IMEI;
                info.ServiceInfo = t.ServiceInfo;
                info.ProCost = t.ProCost;
                info.LowPrice = t.LowPrice;
                info.AduidID = t.AduidID;
                info.AduidedOldPrice = t.AduidedOldPrice;
                info.OffID = t.OffID;
                info.OffPoint = t.OffPoint;
                info.SpecialID = t.SpecialID;
                info.OffPrice = t.OffPrice;
                info.OffSepecialPrice = t.OffSepecialPrice;
                info.WholeSaleOffPrice = t.WholeSaleOffPrice;
                info.BackID = t.BackID;
                info.OldSellListID = t.OldSellListID;
                info.IsFree = t.IsFree;
                info.ChargePhoneNum = t.ChargePhoneNum;
                info.OldID = t.OldID;
                info.Note = t.Note;
                l.selllist = info;
                SellGridModels.Add(l);
            }
            Hall = Store.ProHallInfo.First(p=>p.HallID==SellInfo.HallID);
            this.HallName.DataContext = Hall;
            this.Grid.ItemsSource = SellGridModels;
            this.Grid.Rebind();
            var a = new PublicRequestHelp(this.PageBusy, AddSelllist_Method_ID, new object[] { }, GetSellList_End2);
        }

        private void WOnClosed(object sender, WindowClosedEventArgs windowClosedEventArgs)
        {
            VIPWindow w = (VIPWindow) sender;
            if (w.DialogResult == true)
            {
                SellVIP = w.SellVIP;
                VIPCard.DataContext = SellVIP;
                VIPName.DataContext = SellVIP;
                VIPPhone.DataContext = SellVIP;
                VIPPoint.DataContext = SellVIP;

                VIPTicket = SellVIP.VIP_OffTicket;
                VIPName.IsEnabled = false;
                VIPPhone.IsEnabled = false;

                if (VIPTicket == null)
                {
                    VIPTicket = new List<VIP_OffTicket>();

                }
                VIPTicket.Insert(0, new VIP_OffTicket() {Name = "无", TicketID = "无", ID = 0});
                OffTicket.ItemsSource = VIPTicket;
            }
            else
            {
                SellVIP = null;
                VIPCard.DataContext = SellVIP;
                VIPName.DataContext = SellVIP;
                VIPPhone.DataContext = SellVIP;
                VIPPoint.DataContext = SellVIP;
                VIPTicket = null;
                VIPName.IsEnabled = true;
                VIPPhone.IsEnabled = true;

                if (VIPTicket == null)
                {
                    VIPTicket = new List<VIP_OffTicket>();

                }
                VIPTicket.Insert(0, new VIP_OffTicket() { Name = "无", TicketID = "无", ID = 0 });
                OffTicket.ItemsSource = VIPTicket;
            }
        }



        private void ClearForm()
        {
            //TODO: 清空本体
            //this.Content = new NewProSell();
            this.NavigationService.Navigate(new NewProSell());

        }

        private void New_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            ClearForm();
        }



        private void Del_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            SellGridModels.Remove((NewSellListInfoGridModel)this.Grid.SelectedItem);
            this.Grid.Rebind();

        }




        private void Next_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            //TODO:生成结算
            var proHallInfo = this.Hall;
            if (proHallInfo == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择仓库");
                return;
            }

            if (this.Grid.SelectedItems.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"没有任何商品");
                return;
            }

            if (string.IsNullOrEmpty(VIPName.Text) || string.IsNullOrEmpty(VIPPhone.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入客户资料");
                return;
            }
            if (string.IsNullOrEmpty(SellOldID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入原始单号");
                return;
            }

            System.Text.RegularExpressions.Regex  ex=new Regex(@"^.{7}$");
            if (!ex.IsMatch(SellOldID.Text))
            {
                MessageBox.Show(Application.Current.MainWindow, "请输入正确的原始单号");
                return;
            }

           API.Pro_SellInfo sellInfo=new Pro_SellInfo();
            //sellInfo.Seller = Store.LoginUserInfo.UserID;
            sellInfo.SysDate = DateTime.Now;
            //TODO: 优惠券
            sellInfo.OffTicketID = (int) (OffTicket.SelectedValue ?? 0);
            if (SellVIP != null) sellInfo.VIP_ID = SellVIP.ID;
            sellInfo.HallID = proHallInfo.HallID;
            sellInfo.SellDate = this.SellTime.SelectedValue;
            sellInfo.CusName = this.VIPName.Text;
            sellInfo.CusPhone = this.VIPPhone.Text;
            sellInfo.Note = this.Note.Text;
            List<Pro_SellListInfo> sellList=new List<Pro_SellListInfo>();

            var selectedmodels = this.Grid.SelectedItems.Select(p => (NewSellListInfoGridModel) p).ToList();

            foreach (var m in selectedmodels)
            {
                
                Pro_SellListInfo info=new Pro_SellListInfo();
                var t = m.selllist;
                info.ID = t.ID;
                info.ProID = t.ProID;
                info.ProCount = t.ProCount;
                info.SellType = t.SellType;
                info.CashTicket = t.CashTicket;
                info.ProPrice = t.ProPrice;
                info.TicketID = t.TicketID;
                info.CashPrice = t.CashPrice;
                info.IMEI = t.IMEI;
                info.ServiceInfo = t.ServiceInfo;
                info.ProCost = t.ProCost;
                info.LowPrice = t.LowPrice;
                info.AduidID = t.AduidID;
                info.AduidedOldPrice = t.AduidedOldPrice;
                info.OffID = t.OffID;
                info.OffPoint = t.OffPoint;
                info.SpecialID = t.SpecialID;
                info.OffPrice = t.OffPrice;
                info.OffSepecialPrice = t.OffSepecialPrice;
                info.WholeSaleOffPrice = t.WholeSaleOffPrice;
                info.BackID = t.BackID;
                info.OldSellListID = t.OldSellListID;
                info.IsFree = t.IsFree;
                info.ChargePhoneNum = t.ChargePhoneNum;
                info.ChargePhoneName = t.ChargePhoneName;
                info.OldID = t.OldID;
                info.Note = t.Note;
                info.AnBu = t.AnBu;
                info.LieShouPrice = t.LieShouPrice;
                info.YanbaoModelPrice = t.YanBaoModelPrice;
                info.NeedAduit = t.NeedAduit;
                if (t.Pro_Sell_Yanbao_temp != null)
                {
                    Pro_Sell_Yanbao yanbao = new Pro_Sell_Yanbao();
                    yanbao.BillID = t.Pro_Sell_Yanbao_temp.BillID;
                    yanbao.SellListID = t.Pro_Sell_Yanbao_temp.SellListID;
                    yanbao.MobileType = t.Pro_Sell_Yanbao_temp.MobileType;
                    yanbao.MobileName = t.Pro_Sell_Yanbao_temp.MobileName;
                    yanbao.MobilePrice = t.Pro_Sell_Yanbao_temp.MobilePrice;
                    yanbao.MobileIMEI = t.Pro_Sell_Yanbao_temp.MobileIMEI;
                    yanbao.MobileDate = t.Pro_Sell_Yanbao_temp.MobileDate;
                    yanbao.FatureNum = t.Pro_Sell_Yanbao_temp.FatureNum;
                    yanbao.BateriNum = t.Pro_Sell_Yanbao_temp.BateriNum;
                    yanbao.NgarkuesNum = t.Pro_Sell_Yanbao_temp.NgarkuesNum;
                    yanbao.KufjeNum = t.Pro_Sell_Yanbao_temp.KufjeNum;
                    yanbao.Note = t.Pro_Sell_Yanbao_temp.Note;
                    yanbao.BackListID = t.Pro_Sell_Yanbao_temp.BackListID;
                    yanbao.UserName = t.Pro_Sell_Yanbao_temp.UserName;
                    yanbao.UserPhoneNum = t.Pro_Sell_Yanbao_temp.UserPhoneNum;
                    yanbao.YanBaoName = t.Pro_Sell_Yanbao_temp.YanBaoName;
                    info.Pro_Sell_Yanbao = yanbao;

                }
                if (t.VIP_VIPInfo_Temp != null && t.VIP_VIPInfo_Temp.Count > 0)
                {
                    VIP_VIPInfo vipinfo = new VIP_VIPInfo();
                    var tempinfo = t.VIP_VIPInfo_Temp[0];
                    vipinfo.TypeID = tempinfo.TypeID;
                    vipinfo.MemberName = tempinfo.MemberName;
                    vipinfo.Sex = tempinfo.Sex;
                    vipinfo.Birthday = tempinfo.Birthday;
                    vipinfo.MobiPhone = tempinfo.MobiPhone;
                    vipinfo.TelePhone = tempinfo.TelePhone;
                    vipinfo.QQ = tempinfo.QQ;
                    vipinfo.Address = tempinfo.Address;
                    vipinfo.IDCard_ID = tempinfo.IDCard_ID;
                    vipinfo.IDCard = tempinfo.IDCard;
                    vipinfo.Validity = tempinfo.Validity;
                    vipinfo.StartTime = tempinfo.StartTime;
                    vipinfo.Flag = tempinfo.Flag;
                    vipinfo.Point = tempinfo.Point;
                    vipinfo.Balance = tempinfo.Balance;
                    vipinfo.Seller = tempinfo.Seller;
                    vipinfo.UpdUser = tempinfo.UpdUser;
                    vipinfo.SysDate = tempinfo.SysDate;
                    vipinfo.Note = tempinfo.Note;
                    vipinfo.ProPrice = tempinfo.ProPrice;
                    vipinfo.IMEI = tempinfo.IMEI;
                    vipinfo.Password = tempinfo.Password;
                    vipinfo.UserName = tempinfo.UserName;
                    vipinfo.HallID = tempinfo.HallID;
                    vipinfo.OldID = tempinfo.OldID;
                    vipinfo.EndTime = tempinfo.EndTime;
                    vipinfo.LZUser = tempinfo.LZUser;
                    info.VIP_VIPInfo = vipinfo;
                    //                    info.VIP_VIPInfo_Temp.Add(vipinfo);
                }

                if (t.Pro_Sell_JiPeiKa_temp != null)
                {
                    Pro_Sell_JiPeiKa ji = new Pro_Sell_JiPeiKa();
                    var ji_temp = t.Pro_Sell_JiPeiKa_temp;
                    if (selectedmodels.Select(p => p.selllist.IMEI).Contains(ji_temp.IMEI) &&
                        sellList.All(p => p.Pro_Sell_JiPeiKa == null || p.Pro_Sell_JiPeiKa.IMEI != ji_temp.IMEI))
                    {
                        ji.IMEI = ji_temp.IMEI;
                        info.Pro_Sell_JiPeiKa = ji;
                    }
                    else
                    {
                        MessageBox.Show(Application.Current.MainWindow, "机配卡必须要与对应机身同时销售");
                        return;
                    }
                }

                if (t.Pro_BillInfo_temp != null)
                {
                    Pro_BillInfo billinfo=new Pro_BillInfo();
                    
                    billinfo.ProID = t.Pro_BillInfo_temp.ProID;
                    billinfo.BillIMEI = t.Pro_BillInfo_temp.BillIMEI;
                    billinfo.MobileIMEI = t.Pro_BillInfo_temp.MobileIMEI;
                    billinfo.BillField1 = t.Pro_BillInfo_temp.BillField1;
                    billinfo.BillField2 = t.Pro_BillInfo_temp.BillField2;
                    billinfo.BillField3 = t.Pro_BillInfo_temp.BillField3;
                    billinfo.BillField4 = t.Pro_BillInfo_temp.BillField4;
                    billinfo.BillField5 = t.Pro_BillInfo_temp.BillField5;
                    billinfo.BillField6 = t.Pro_BillInfo_temp.BillField6;
                    billinfo.BillField7 = t.Pro_BillInfo_temp.BillField7;
                    billinfo.BillField8 = t.Pro_BillInfo_temp.BillField8;
                    billinfo.BillField9 = t.Pro_BillInfo_temp.BillField9;
                    billinfo.BillField10 = t.Pro_BillInfo_temp.BillField10;
                    billinfo.Note = t.Pro_BillInfo_temp.Note;
                    billinfo.MobileProID = t.Pro_BillInfo_temp.MobileProID;
                    billinfo.MobileClassID = t.Pro_BillInfo_temp.MobileClassID;
                    info.Pro_BillInfo = billinfo;
                }
                sellList.Add(info);
            }
            sellInfo.Pro_SellListInfo = sellList;
            sellInfo.OldID = this.SellOldID.Text;
             
            var tempIDS= selectedmodels.Select(p => p.selllist.ID).ToList();
            var a = new PublicRequestHelp(PageBusy, MethodID_SellNext, new object[] { sellInfo, tempIDS }, SellNextEvent);




        }

        private void SellNextEvent(object sender, MainCompletedEventArgs e)
        {
            PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    if (((Pro_SellInfo) e.Result.Obj).Pro_SellListInfo.Count == 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"无可销售商品");
                        return;
                    }
                    var newpage = new NewProSellSetp2((Pro_SellInfo)e.Result.Obj, (from b in (List<API.VIP_OffList>)e.Result.ArrList[0] select (API.VIP_OffList)b).ToList(), this.SellVIP);
                    var selectedmodels = this.Grid.SelectedItems.Select(p => (NewSellListInfoGridModel)p).ToList();
                    newpage.AllRulesInfos = (List<Rules_AllCurrentRulesInfo>)e.Result.ArrList[1];
                    newpage.SellTempIds = selectedmodels.Select(p => p.selllist.ID).ToList();
                    newpage.oldpage = oldpage;
                    this.NavigationService.Navigate(newpage);
//                        this.Content = newpage;


                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"提交失败: " + e.Result.Message);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"提交失败: 服务器错误\n" + e.Error.Message);
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }  
        }

        private void LayoutRoot_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
//            switch (e.Key)
//            {
//                case (Key.F1):
//                    IMEISell_OnClick(null,null);
//                    break;
//                case(Key.F2):
//                    ProSell_OnClick(null,null);
//                    break;
//
//               
//                default:
//                    break;
//            }
        }

        private void Grid_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
        	if (e.EditAction == GridViewEditAction.Commit)
        	{
        	    //TODO: 查询兑券
        	}

        }




        private void HallName_OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            if (this.Hall != null)
            {
                return;
            }
            var AreaList = (from b in Store.ProHallInfo
                            join c in Store.AreaInfo on b.AreaID equals c.AreaID
                            select c).Distinct().ToList();
            List<TreeViewModel> parent = CommonHelper.AreaTreeViewModel(AreaList);

            SingleSelecter m = new SingleSelecter(parent, Store.ProHallInfo, "AreaID", "HallName", new string[] { "HallID", "HallName" }, new string[] { "仓库编号", "仓库名称" },true);
            m.Closed += Hall_select_closed;
            m.ShowDialog();
            
        }

        void Hall_select_closed(object sender, WindowClosedEventArgs e)
        {
            SingleSelecter m = (SingleSelecter)sender;

               if (e.DialogResult ==true)
               {
                   this.Hall = (Pro_HallInfo) m.SelectedItem;
                   this.HallName.DataContext = this.Hall;

               }
        }

        private void VIP_OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            VIPWindow w = new VIPWindow();
            w.Closed += WOnClosed;
            
            w.ShowDialog();
        }

        private void AddSellList_OnClick(object sender, RoutedEventArgs e)
        {

            var a = new PublicRequestHelp(this.PageBusy, AddSelllist_Method_ID, new object[] {}, GetSellList_End);
        }

        private void GetSellList_End(object sender, MainCompletedEventArgs e)
        {



            this.PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {

                    

                    List<NewSellListInfoGridModel> l = new List<NewSellListInfoGridModel>();
                    List<Pro_SellListInfo_Temp> m = e.Result.Obj as List<Pro_SellListInfo_Temp>;

                    foreach (var proSellListInfoTemp in m)
                    {
                        NewSellListInfoGridModel model = new NewSellListInfoGridModel();
                        model.selllist = proSellListInfoTemp;
                        l.Add(model);
                    }

                    /* <telerik:GridViewDataColumn Header="商品类别" DataMemberBinding="{Binding ProClass}" IsReadOnly="True" />
                    <telerik:GridViewDataColumn Header="商品品牌" DataMemberBinding="{Binding ProType}" IsReadOnly="True" />
                    <telerik:GridViewDataColumn Header="商品名称" DataMemberBinding="{Binding ProName}" IsReadOnly="True" />
                   
                    <telerik:GridViewDataColumn Header="数量" DataMemberBinding="{Binding ProCount}"/>
                    
<!--					<telerik:GridViewDataColumn Header="单位" DataMemberBinding="{Binding Unit}" IsReadOnly="True"/>-->
					<telerik:GridViewDataColumn Header="串号" DataMemberBinding="{Binding IMEI}" IsReadOnly="True"/>
					<telerik:GridViewDataColumn Header="兑券码" DataMemberBinding="{Binding Ticket}" />
                    <telerik:GridViewDataColumn Header="兑券面值" DataMemberBinding="{Binding TicketPrice}" />
                    <!--TicketPrice-->
                    <telerik:GridViewDataColumn Header="属性" DataMemberBinding="{Binding ProFormat}"></telerik:GridViewDataColumn>
                    <telerik:GridViewDataColumn Header="备注" DataMemberBinding="{Binding Note}" IsRe
                        */
                    MultSelecter w = new MultSelecter(null, l, null, "IMEI",
    new string[]
                            {
                                "ProClass",
                                "ProType",
                                "ProName",
                                "ProCount",
                                "IMEI",
                                "Ticket","TicketPrice","ProFormat"
                            },
        new string[]
                                {
                                    "商品类别","商品品牌","商品名称","数量","串号","兑券码","兑券面值","属性",

                                });
                    w.ShowDialog();
                    
                    if (w.DialogResult == true)
                    {
                        foreach (var obj in w.SelectedItems)
                        {
                            SellGridModels.Add((NewSellListInfoGridModel) obj);
                        }
                    }
                    //SellGridModels.Clear();
                    
                    this.Grid.Rebind();
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message);
                }

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
            }
        }


        private void GetSellList_End_(object sender, MainCompletedEventArgs e)
        {
            this.PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    List<NewSellListInfoGridModel> l=new List<NewSellListInfoGridModel>();
                    List<Pro_SellListInfo_Temp> m = e.Result.Obj as List<Pro_SellListInfo_Temp>;
                    foreach (var proSellListInfoTemp in m)
                    {
                        NewSellListInfoGridModel  model=new NewSellListInfoGridModel();
                        model.selllist = proSellListInfoTemp;
                        l.Add(model);
                    }


                    MultSelecter w = new MultSelecter(null, l, null, null,
                                                      new string[] {"ProName", "ProCount", "IMEI", "ChargeNum","SellType"},
                                                      new string[] {"商品名称", "数量", "串号", "号码","销售类别"});
                    w.Closed += selectlistinfo_close;

                    w.ShowDialog();
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message);
                }

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
            }
                
        }

        void selectlistinfo_close(object sender, WindowClosedEventArgs e)
        {
           MultSelecter m=sender as MultSelecter;
           if (m == null)
           {
               return;
           }
            SellGridModels.AddRange(m.SelectedItems.Select(p => (NewSellListInfoGridModel) p));
            this.Grid.Rebind();
        }

        private void Back_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            if (oldpage != null)
            {
                this.NavigationService.Navigate(oldpage);
            }
        }


        private void NewProSell_OnLoaded(object sender, RoutedEventArgs e)
        {
            
            this.Back.IsEnabled = oldpage != null;
            this.Back.Header = "返回" + oldpage;
        }
    } 
}
