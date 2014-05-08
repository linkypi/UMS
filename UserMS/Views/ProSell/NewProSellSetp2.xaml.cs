using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Model;

namespace UserMS.Views.ProSell
{
    public partial class NewProSellSetp2
    {
        public API.Pro_SellInfo SellInfo { get; set; }
        public List<API.VIP_OffList> OffList { get; set; }
        public API.VIP_VIPInfo VipInfo;
        private List<ProSellGridModel> GridViewSellList = new List<ProSellGridModel>();
        public List<API.VIP_OffList> SelectedOffList = new List<VIP_OffList>();
        public List<API.Pro_SellListInfo> newselllist = new List<Pro_SellListInfo>();
        
        public List<VIP_OffList> canselect = new List<VIP_OffList>();
        public int Save_MethodID = 51;
        public List<VIP_OffTicket> VIPTicket;
        public object oldpage { get; set; }
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public List<API.Pro_SellSpecalOffList> SpecalOffLists = new List<Pro_SellSpecalOffList>();
        public List<API.Rules_AllCurrentRulesInfo> AllRulesInfos=new List<Rules_AllCurrentRulesInfo>();
        private decimal sellprice = 0;
        private decimal offprice = 0;
        private decimal cashprice = 0;
        private bool cansave = true;
        public List<int> SellTempIds { get; set; }

        decimal calcyanbaoprice(API.Pro_SellListInfo proSellListInfo)
        {
            proSellListInfo.AnBuPrice = (proSellListInfo.ProPrice - proSellListInfo.TicketUsed) <
                                           proSellListInfo.AnBu
                                               ? (proSellListInfo.ProPrice - proSellListInfo.TicketUsed)
                                               : proSellListInfo.AnBu;
            decimal temp = proSellListInfo.ProPrice -
                           proSellListInfo.TicketUsed - proSellListInfo.AnBuPrice - proSellListInfo.OffPrice -
                           proSellListInfo.OtherOff -
                           proSellListInfo.OffSepecialPrice;
            if (temp < 0) temp = 0;
            proSellListInfo.CashPrice = temp + proSellListInfo.OtherCash;


            if (proSellListInfo.Pro_SellList_RulesInfo.Count > 0)
            {

                foreach (var proSellListRulesInfo in proSellListInfo.Pro_SellList_RulesInfo)
                {
                    if (proSellListInfo.CashPrice < proSellListRulesInfo.OffPrice)
                    {
                        proSellListRulesInfo.RealPrice = proSellListInfo.CashPrice;
                    }
                    else
                    {
                        proSellListRulesInfo.RealPrice =
                            proSellListRulesInfo.OffPrice;

                    }
                    proSellListInfo.CashPrice = proSellListInfo.CashPrice - proSellListRulesInfo.RealPrice;
                }


            }

            decimal temps= proSellListInfo.CashPrice + proSellListInfo.TicketUsed +
                   proSellListInfo.Pro_SellList_RulesInfo.Where(p => p.CanGetBack).Sum(o => o.RealPrice);
            if (temps == 0)
            {
                return Store.ProInfo.First(p => p.ProID == proSellListInfo.ProID)
                    .Pro_SellTypeProduct.First(o => o.SellType == proSellListInfo.SellType).LowPrice;
                 
            }
            else
            {
                return temps;
            }

        }
        private void calcprices()
        {

            sellprice = 0;
            offprice = 0;
            cashprice = 0;
            cansave = true;
            foreach (var proSellGridModel in GridViewSellList)
            {
                proSellGridModel.IsOK = true;
            }
            
            #region 延保
            var yanbaoProID = Store.Options.First(p => p.ClassName == "GXYanBao").Value;
            
            foreach (var proSellListInfo in newselllist)
            {
               if (proSellListInfo.Pro_Sell_Yanbao != null)
               {
                  
                   if (newselllist.Any(p => p.IMEI == proSellListInfo.Pro_Sell_Yanbao.MobileIMEI))
                   {
                       var yanbaomobile = newselllist.First(p => p.IMEI == proSellListInfo.Pro_Sell_Yanbao.MobileIMEI);
                       var mobileprice = calcyanbaoprice(yanbaomobile);
                       yanbaomobile.YanbaoModelPrice = mobileprice;
                       var yanbaomodule = proSellListInfo.Pro_Sell_Yanbao;

                       if (
                           Store.YanbaoPriceStep.Any(
                               q => q.ID == Store.ProInfo.First(p => p.ProID == yanbaomobile.ProID).YanBaoModelID))
                       {
                           proSellListInfo.ProPrice =
                               Convert.ToDecimal(
                                   Store.YanbaoPriceStep.First(
                                       q =>
                                       q.ID == Store.ProInfo.First(p => p.ProID == yanbaomobile.ProID).YanBaoModelID)
                                        .ProPrice);
                           yanbaomodule.YanBaoName = Store.YanbaoPriceStep.First(
                               q =>
                               q.ID == Store.ProInfo.First(p => p.ProID == yanbaomobile.ProID).YanBaoModelID).Name;

                           //重新计算套餐价
                           if (proSellListInfo.SpecialID > 0)
                           {
                               decimal afteroffprice =
                                   OffList.First(
                                       p =>
                                       p.ID ==
                                       Convert.ToInt32(
                                           SpecalOffLists.First(q => q.ID == proSellListInfo.SpecialID).SpecalOffID))
                                          .VIP_ProOffList.First(e => e.ProID == proSellListInfo.ProID)
                                          .AfterOffPrice;
                               if (proSellListInfo.ProPrice - proSellListInfo.OffPrice <= afteroffprice)
                            {
                                proSellListInfo.OffSepecialPrice = proSellListInfo.ProPrice - proSellListInfo.OffPrice;
                            }
                            else
                            {
                                proSellListInfo.OffSepecialPrice = proSellListInfo.ProPrice - proSellListInfo.OffPrice - afteroffprice;
                                
                            }
                              
                           }
                           

                       }
                       else if (
                           Store.YanbaoPriceStep.Any(
                               p => p.ProID == proSellListInfo.ProID && p.StepPrice >= mobileprice))
                       {

                           proSellListInfo.ProPrice =
                               Convert.ToDecimal(
                                   Store.YanbaoPriceStep.Where(
                                       p => p.ProID == proSellListInfo.ProID && p.StepPrice >= mobileprice)
                                        .OrderBy(p => p.StepPrice).First().ProPrice);
                           yanbaomodule.YanBaoName = Store.YanbaoPriceStep.Where(
                               p => p.ProID == proSellListInfo.ProID && p.StepPrice >= mobileprice)
                                                          .OrderBy(p => p.StepPrice).First().Name;
                           //重新计算套餐价
                           if (proSellListInfo.SpecialID > 0)
                           {
                               decimal afteroffprice =
                                   OffList.First(
                                       p =>
                                       p.ID ==
                                       Convert.ToInt32(
                                           SpecalOffLists.First(q => q.ID == proSellListInfo.SpecialID).SpecalOffID))
                                          .VIP_ProOffList.First(e => e.ProID == proSellListInfo.ProID)
                                          .AfterOffPrice;
                               if (proSellListInfo.ProPrice - proSellListInfo.OffPrice <= afteroffprice)
                               {
                                   proSellListInfo.OffSepecialPrice = proSellListInfo.ProPrice - proSellListInfo.OffPrice;
                               }
                               else
                               {
                                   proSellListInfo.OffSepecialPrice = proSellListInfo.ProPrice - proSellListInfo.OffPrice - afteroffprice;

                               }

                           }

                       }
                       else
                       {
                           if (GridViewSellList.Any(p => p.SellListModel == proSellListInfo))
                           {
                               var t = GridViewSellList.First(p => p.SellListModel == proSellListInfo);
                               t.IsOK = false;
                           }
                           cansave = false;
                       }


                   }
                
                 

                       if (proSellListInfo.ProPrice == 0)
                       {
                           if (GridViewSellList.Any(p => p.SellListModel == proSellListInfo))
                           {
                               var t = GridViewSellList.First(p => p.SellListModel == proSellListInfo);
                               t.IsOK = false;

                           }
                           cansave = false;
                       }


              
                   
                  
               }
                
            }
           
            #endregion 延保

            foreach (var proSellListInfo in newselllist)
            {
     
                //sellprice = sellprice + Convert.ToDecimal(proSellListInfo.ProPrice) * Convert.ToDecimal(proSellListInfo.ProCount);
//                proSellListInfo.CashPrice = (proSellListInfo.ProPrice -proSellListInfo.AnBu- proSellListInfo.OffPrice  - proSellListInfo.TicketUsed -proSellListInfo.OtherOff +proSellListInfo.OtherCash)*
//                                            proSellListInfo.ProCount;

                proSellListInfo.AnBuPrice = (proSellListInfo.ProPrice - proSellListInfo.TicketUsed) <
                                            proSellListInfo.AnBu
                                                ? (proSellListInfo.ProPrice - proSellListInfo.TicketUsed)
                                                : proSellListInfo.AnBu;
                decimal temp = proSellListInfo.ProPrice -
                               proSellListInfo.TicketUsed - proSellListInfo.AnBuPrice - proSellListInfo.OffPrice -
                               proSellListInfo.OtherOff -
                               proSellListInfo.OffSepecialPrice;
                if (temp < 0) temp = 0;
                proSellListInfo.CashPrice = temp  + proSellListInfo.OtherCash;

               
                if (proSellListInfo.Pro_SellList_RulesInfo.Count > 0)
                {

                    foreach (var proSellListRulesInfo in proSellListInfo.Pro_SellList_RulesInfo)
                    {
                        if (proSellListInfo.CashPrice < proSellListRulesInfo.OffPrice)
                        {
                            proSellListRulesInfo.RealPrice = proSellListInfo.CashPrice;
                        }
                        else
                        {
                            proSellListRulesInfo.RealPrice =
                                proSellListRulesInfo.OffPrice;

                        }
                        proSellListInfo.CashPrice = proSellListInfo.CashPrice - proSellListRulesInfo.RealPrice;
                    }
                    

                }

                if (proSellListInfo.CashPrice < proSellListInfo.LieShouPrice )
                {
                    if (GridViewSellList.Any(p => p.SellListModel == proSellListInfo))
                    {
                        var t = GridViewSellList.First(p => p.SellListModel == proSellListInfo);
                        t.IsOK = false;

                    }
                    
                    cansave = false;
                }
                proSellListInfo.YanbaoModelPrice = calcyanbaoprice(proSellListInfo);
                sellprice += proSellListInfo.CashPrice*proSellListInfo.ProCount;
//                sellprice = sellprice - proSellListInfo.OffSepecialPrice*proSellListInfo.ProCount -
//                            proSellListInfo.LieShouPrice*proSellListInfo.ProCount;


            }
            foreach (var vipOffList in SelectedOffList)
            {
                offprice = offprice + Convert.ToDecimal(vipOffList.OffMoney);
                
            }
            if (SellInfo.OffID != null && OffList.Any(p => p.ID == SellInfo.OffID))
            offprice = offprice + Convert.ToDecimal( OffList.First(p => p.ID == SellInfo.OffID).OffMoney);

            if (SellInfo.OffTicketPrice != null)
                offprice = offprice + Convert.ToDecimal(SellInfo.OffTicketPrice);
            cashprice = sellprice;
            //if (cashprice < 0) cashprice = 0;
            SellInfo.CashTotle = cashprice;
            this.ProPrice.Text = sellprice.ToString("0.00");
            this.OffPrice.Text = offprice.ToString("0.00");
            //this.SellPrice.Text = cashprice.ToString("0.00");
            this.SellPrice.Value = cashprice;

            this.CashPrice.Value = cashprice;
            this.CardPrice.Value = 0;
            CardPrice_ValueChanged(null, null);
            if (cansave)
            {
                Logger.Log("本单验证通过 可以保存");
                foreach (var proSellGridModel in GridViewSellList)
                {
                    proSellGridModel.ProPrice = proSellGridModel.SellListModel.ProPrice;
                }
            }
            else
            {
                Logger.Log("有延保或物品无法销售 本单不可保存");
            }
            
        }

        private void InitNewSellList(List<Pro_SellListInfo> listInfos)
        {
            newselllist=new List<Pro_SellListInfo>();
            
            DataContractSerializer bf = new DataContractSerializer(typeof(List<Pro_SellListInfo>));
            using (MemoryStream ms = new MemoryStream())
            {
                bf.WriteObject(ms,listInfos);
                ms.Position = 0;
                newselllist = (List<Pro_SellListInfo>) bf.ReadObject(ms);

            }
            
        }

        public NewProSellSetp2()
        {
            // Required to initialize variables
            InitializeComponent();
#if HZ
            VIPBTN.Visibility = Visibility.Collapsed;
            VIPPOINT.Visibility = Visibility.Collapsed;
#endif
        }

        public NewProSellSetp2(API.Pro_SellInfo SellInfo, List<API.VIP_OffList> OffList, API.VIP_VIPInfo vipInfo)
        {
            this.SellInfo = SellInfo;
            this.OffList = OffList;


            InitializeComponent();
#if HZ
            VIPBTN.Visibility = Visibility.Collapsed;
            VIPPOINT.Visibility = Visibility.Collapsed;
#endif
            this.VipInfo = vipInfo;
            VIPCard.DataContext = vipInfo;
            VIPName.Text=SellInfo.CusName;
            VIPPhone.Text = SellInfo.CusPhone;
            VIPPoint.DataContext = vipInfo;
            this.SellOldID.Text = this.SellInfo.OldID;
            this.Note.Text = SellInfo.Note;
            if (vipInfo != null) VIPTicket = vipInfo.VIP_OffTicket;

            if (VIPTicket == null)
                VIPTicket = new List<VIP_OffTicket>();
            VIPTicket.Insert(0, new VIP_OffTicket() { Name = "无", TicketID = "无", ID = 0 });
            
            this.OffTicket.ItemsSource = VIPTicket;
            this.OffTicket.SelectedIndex = 0;
            InitGridSellList(SellInfo.Pro_SellListInfo, this.OffList);
            this.OffListSelected.ItemsSource = SelectedOffList;
            InitNewSellList(SellInfo.Pro_SellListInfo);
            this.OffListSelect.ItemsSource = canselect;

            CalcOffCanselect();
            List<VIP_OffList> sellofflist = OffList.Where(p => p.Type == 2).ToList();
            sellofflist.Insert(0,new VIP_OffList(){ID=0,Name="无"});
            this.SellOffSelect.ItemsSource = sellofflist;
            this.SellOffSelect.SelectedIndex = 0;
            calcprices();
            InitGridSellList(newselllist, this.OffList);

            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
            UserOpList =
    userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
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
//            foreach (var sysUserOpList in userops)
//            {
//                try
//                {
//                    UserOpModel p = new UserOpModel();
//                    p.ID = sysUserOpList.ID;
//                    p.HallID = sysUserOpList.HallID;
//                    p.OpID = sysUserOpList.OpID;
//                    p.UserID = sysUserOpList.UserID;
//                    p.Username = Store.UserInfos.First(q => q.UserID == sysUserOpList.UserID).RealName;
//                    p.opname = Store.UserOp.First(q => q.OpID == sysUserOpList.OpID).Name;
//                    UserOpList.Add(p);
//                }
//                catch
//                {
//
//                }
//            }
            var userinfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model => model.UserID,
                                                 (info, model) => info).ToList();
            this.Seller.ItemsSource = userinfos;
            this.Seller.TextSearchPath = "RealName";
            this.Seller.SearchEvent = SellerSearchEvent;
            this.Seller.SelectionMode = AutoCompleteSelectionMode.Single;
            this.Seller.TextBox_SelectionChanged = SellerSelectEvent;

            this.SellList.AddHandler(RadComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(GridViewComboBoxColumn_PropertyChanged_1));
        }

        private void InitGridSellList(List<Pro_SellListInfo> Pro_SellListInfo, List<VIP_OffList> OffList)
        {
            GridViewSellList.Clear();
            foreach (Pro_SellListInfo proSellListInfo in Pro_SellListInfo)
            {
                ProSellGridModel p = new ProSellGridModel();
                p.ProID = proSellListInfo.ProID;
//                p.ProName = Store.ProInfo.First(pp => pp.ProID == proSellListInfo.ProID).ProName;
                p.ProCount = proSellListInfo.ProCount;
                //p.Unit = Store.ProInfo.First(pp => pp.ProID == proSellListInfo.ProID).pro;
                p.ProPrice = proSellListInfo.ProPrice;
                p.OffPrice = proSellListInfo.OffPrice;
                p.SpecalOffPrice = proSellListInfo.OffSepecialPrice;
                
                p.IMEI = proSellListInfo.IMEI;
                p.TicketNum = proSellListInfo.TicketID;
                p.TicketPrice = proSellListInfo.CashTicket;
                p.OffLists = OffList.Where(t => t.Type == 0 && t.VIP_ProOffList.Any(q => q.ProID == p.ProID)).ToList();
                p.OffLists.Insert(0, new VIP_OffList() {ID = 0, Name = "无"});
                p.SelectedOffId = Convert.ToInt32(proSellListInfo.OffID);
                p.SellListModel = proSellListInfo;
                p.Note = proSellListInfo.Note;
                p.IsOK = true;
                GridViewSellList.Add(p);
            }


            SellList.ItemsSource = GridViewSellList;
            SellList.Rebind();
        }

        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
            Sys_UserInfo selected = Seller.SelectedItem as Sys_UserInfo;
            if (selected != null)
            {
                this.SellInfo.Seller = selected.UserID;
            }
            else
            {
                this.SellInfo.Seller = null;
            }

        }

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
                    this.SellInfo.Seller = selected.UserID;
                    this.Seller.TextBox.SearchText = selected.Username;
             

                }
            }

        } 





//        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
//        {
//            SingleSelecter w = new SingleSelecter(null, Store.UserInfos, null,
//                                                  "RealName", new string[] { "RealName", },
//                                                  new string[] {"姓名", });
//
//            w.Closed += SellerSearchWindowClose;
//            w.ShowDialog();
//        }
//
//        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
//        {
//            SingleSelecter window = sender as SingleSelecter;
//            if (window != null)
//            {
//                if (window.DialogResult==true)
//                {
//                    Sys_UserInfo selected = (Sys_UserInfo)window.SelectedItem;
//                    this.SellInfo.Seller = selected.UserID;
//                    this.Seller.TextBox.SearchText = selected.RealName;
//
//                }
//            }
//
//        } 


        private void Save_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
//            
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
//                this.Content = new NewProSell(true);
//                //TODO:Clear
//                return;
//            }

            if (string.IsNullOrEmpty(SellInfo.Seller))
            {
                try
                {
                    SellInfo.Seller = Store.UserInfos.First(p => p.RealName == Seller.Text).UserID;
                }
                catch (Exception)
                {

                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售员不存在");
                    return;
                }
                
            }

            
            API.Pro_SellInfo Sellinfo = this.SellInfo;
            Sellinfo.Pro_SellListInfo = this.newselllist;
            Sellinfo.UserID = Store.LoginUserInfo.UserID;
            Sellinfo.Note = this.Note.Text;
            Sellinfo.CashPay = Convert.ToDecimal(CashPrice.Value);
            Sellinfo.CardPay = Convert.ToDecimal(CardPrice.Value);
            Sellinfo.SellDate = DateTime.Now;
            Sellinfo.Pro_SellSpecalOffList = SpecalOffLists;
            calcprices();
            InitGridSellList(newselllist, this.OffList);
            if (newselllist.Any(p => p.ProCount == 0))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"不允许有0数量的销售");
                return;
            }
            foreach (var proSellListInfo in newselllist)
            {
                if (proSellListInfo.OtherOff != 0)
                {
                    proSellListInfo.NeedAduit = true;
                }
            }
            PublicRequestHelp helper=new PublicRequestHelp(this.IsBusy,Save_MethodID,new object[]{ Sellinfo,SellTempIds}, Save_Temp_Event);
            //TODO 


        }

        private void Save_Temp_Event(object sender, MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    List<API.Print_SellListInfo> list = (List<Print_SellListInfo>) e.Result.Obj;
                    if (list == null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存成功, 但需要审核");
                        if (oldpage == null)
                            this.NavigationService.Navigate(new NewProSell(true));
                        else
                            this.NavigationService.Navigate(oldpage);
                    }
                    else
                    {
                        //MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                        MessageBoxResult rsltMessageBox = MessageBox.Show(
                            System.Windows.Application.Current.MainWindow, "保存成功。是否打印", "提示", MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);
                        if (rsltMessageBox == MessageBoxResult.Yes)
                        {

                            List<ReportService.Print_SellListInfo> newlist =
                                new List<ReportService.Print_SellListInfo>();
                            foreach (var l in list)
                            {
                                newlist.Add(new ReportService.Print_SellListInfo()
                                    {
                                        串码_号码_合同号 = l.串码_号码_合同号,
                                        Print_SellInfo = null,
                                        优惠 = l.优惠,
                                        优惠券名称 = l.优惠券名称,
                                        优惠券金额 = l.优惠券金额,
                                        会员卡号 = l.会员卡号,
                                        刷卡 = l.刷卡,
                                        券号_合约号 = l.券号_合约号,
                                        券面值 = l.券面值,
                                        单价 = l.单价,
                                        原始单号 = l.原始单号,
                                        商品名称 = l.商品名称,
                                        备注 = l.备注,
                                        实收总额 = l.实收总额,
                                        实收总额大写 = l.实收总额大写,
                                        客户电姓名 = l.客户电姓名,
                                        客户电话 = l.客户电话,
                                        应收总额 = l.应收总额,
                                        数量 = l.数量,
                                        现金 = l.现金,
                                        系统单号 = l.系统单号,
                                        系统自增外键编号 = l.系统自增外键编号,
                                        自增主键编号 = l.自增主键编号,
                                        //补差金额 = l.补差金额,
                                        金额小计 = l.金额小计,
                                        销售公司 = l.销售公司,
                                        销售单号 = l.销售单号,
                                        销售员 = l.销售员,
                                        销售日期 = l.销售日期,
                                        销售门店 = l.销售门店
                                    });
                            }
                            var newpage = new UserMS.Report.Print.SellPrint.PrintSellBillOne(newlist);
                            newpage.oldpage = this.oldpage;
                            this.NavigationService.Navigate(newpage);

                        }
                        else
                        {
                            if (oldpage == null)
                                this.NavigationService.Navigate(new NewProSell(true));
                            else
                                this.NavigationService.Navigate(oldpage);
                        }
                    }
                    //this.Content = new NewProSell(true);
                }
                else
                {
                    string errormessage = "保存失败: " + e.Result.Message;

                    API.Pro_SellInfo temp = e.Result.Obj as Pro_SellInfo;
                    if (temp != null)
                    {
                        foreach (var list in temp.Pro_SellListInfo)
                        {
                            if (!string.IsNullOrEmpty(list.Note))
                            {
                                errormessage = errormessage + "\n" + list.Note;
                            }
                        }
                    }
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, errormessage);


                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存失败: 服务器错误\n" + e.Error.Message);
            }
        }

        private void SaveEvent(object sender, MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                    this.Content = new NewProSell();
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败: " + e.Result.Message);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败: 服务器错误\n" + e.Error.Message);
            }
        }

        private void OffListSelect_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
        }


        private void CalcOffCanselect()
        {
            string YanBaoProID = Store.Options.First(p => p.ClassName == "GXYanBao").Value;

            
            canselect.Clear();
            List<Pro_SellListInfo> temp = new List<Pro_SellListInfo>(SellInfo.Pro_SellListInfo);
            var temp1 = (from c in temp
                         group c by c.ProID
                             into g
                             select new { ProID = g.Key, ProCount = g.Sum(p => p.ProCount) }).ToList();

            foreach (VIP_OffList vipOffList in OffList)
            {
                var a = vipOffList.VIP_ProOffList.ToList();
                //                vipOffList.ID

                if (vipOffList.Type==1)
                {
                    try
                    {
                        foreach (VIP_ProOffList vipProOffList in a)
                        {
                            if (vipProOffList.ProID == YanBaoProID)
                            {
                                if (newselllist.Any(p=>p.ProID==YanBaoProID && p.SpecialID>0))
                                    throw new Exception();
                            }
                            var query =
                                newselllist.Any(
                                    p =>
                                    p.ProID == vipProOffList.ProID && (p.SpecialID == 0 || p.SpecialID == null) &&
                                    p.ProCount >= vipProOffList.ProCount && p.SellType==vipProOffList.SellTypeID);

                            if (!query)
                            {
                                throw new Exception();
                            }
                        }
                        vipOffList.OffMoney = a.Sum(p => p.AfterOffPrice);

                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    canselect.Add(vipOffList);


                    //组合
                }
            }
            this.OffListSelect.Rebind();
            calcprices();
            InitGridSellList(newselllist, this.OffList);
        }



        private void AddOffMethod(VIP_OffList selected)
        {

             if (selected.Type == 1)
            {
                API.Pro_SellSpecalOffList specalOff = new Pro_SellSpecalOffList();
                specalOff.Pro_SellListInfo = new List<Pro_SellListInfo>();
                
                foreach (var vipProOffList in selected.VIP_ProOffList)
                {
                    var query1 =
                        newselllist.Where(
                            p =>
                            p.ProID == vipProOffList.ProID && (p.SpecialID == 0 || p.SpecialID == null) &&
                            p.ProCount >= vipProOffList.ProCount).OrderBy(p => p.ProCount);
                    decimal need = (decimal) vipProOffList.ProCount;
                    
                    
                    specalOff.SpecalOffID = selected.ID;
                    if (SpecalOffLists.Count < 1)
                    {
                        specalOff.ID = 1;
                    }
                    else
                    {
                        specalOff.ID = SpecalOffLists.Max(p => p.ID) + 1;
                    }

                    foreach (var query in query1)
                    {
                        if (query.ProCount == need)
                        {
                            query.SpecialID = specalOff.ID;
                            query.ProOffListID = vipProOffList.ID;
                            if (query.ProPrice -query.AnBu-query.TicketUsed - query.OffPrice <= vipProOffList.AfterOffPrice)
                            {
                                query.OffSepecialPrice = 0;
                            }
                            else
                            {
                                query.OffSepecialPrice = query.ProPrice - query.AnBu - query.TicketUsed - query.OffPrice - vipProOffList.AfterOffPrice;
                                //query.OffSepecialPrice =query.ProPrice- vipProOffList.AfterOffPrice;
                            }
                            query.Salary = vipProOffList.Salary;
                            //specalOff.Pro_SellListInfo.Add(query);
                            break;
                        }
                        else if
                            (query.ProCount > need)
                        {
                            query.ProCount -= vipProOffList.ProCount;
                            
                            Pro_SellListInfo b = new Pro_SellListInfo();
                            b.ProID = query.ProID;
                            b.ProCount = vipProOffList.ProCount;
                            b.ProPrice = query.ProPrice;
                            b.OffPrice = query.OffPrice;
                            b.AnBu = query.AnBu;
                            b.LieShouPrice = query.LieShouPrice;
                            b.TicketUsed = b.TicketUsed;
                            if (b.ProPrice-b.AnBu-b.TicketUsed- b.OffPrice <= vipProOffList.AfterOffPrice)
                            {
                                b.OffSepecialPrice = 0;
                            }
                            else
                            {
                                b.OffSepecialPrice = b.ProPrice-b.AnBu-b.TicketUsed - b.OffPrice - vipProOffList.AfterOffPrice;
                            }
                            b.IMEI = query.IMEI;
                            b.SpecialID = specalOff.ID;
                            b.OffID = query.OffID;
                            b.Salary = vipProOffList.Salary;
                            b.SellType = query.SellType;
                            b.SellType_Pro_ID = query.SellType_Pro_ID;
                            b.ChargePhoneName = query.ChargePhoneName;
                            b.ChargePhoneNum = query.ChargePhoneNum;
                            b.Note = query.Note;
                            b.ProOffListID = vipProOffList.ID;
                            //specalOff.Pro_SellListInfo.Add(b);
                            newselllist.Add(b);
                            break;
                        }
                        else
                        {
                            need = (decimal) (need - query.ProCount);
                            query.SpecialID = specalOff.ID;
                            query.ProOffListID = vipProOffList.ID;
                            if (query.ProPrice - query.AnBu - query.TicketUsed - query.OffPrice <= vipProOffList.AfterOffPrice)
                            {
                                query.OffSepecialPrice = 0;
                            }
                            else
                            {
                                query.OffSepecialPrice = query.ProPrice - query.AnBu - query.TicketUsed - query.OffPrice - vipProOffList.AfterOffPrice;
                            }
                            //specalOff.Pro_SellListInfo.Add(query);
                            
                        }
                    }

                    
                }
                
                //selected.Pro_SellSpecalOffList = new List<Pro_SellSpecalOffList>(SpecalOffLists);
                VIP_OffList newselect = new VIP_OffList()
                    {
                        ID = selected.ID,
                        Name = selected.Name,
                        Pro_SellSpecalOffList = new List<Pro_SellSpecalOffList>(),
                        OffMoney = selected.OffMoney,
                        OffPoint = selected.OffPoint,
                        OffRate = selected.OffRate,
                        OffPointMoney = selected.OffPointMoney
                    };
                 newselect.Pro_SellSpecalOffList.Add(specalOff);
                SpecalOffLists.Add(specalOff);
                SelectedOffList.Add(newselect);
                 
                this.OffListSelected.Rebind();
            }
            
        }

        private void DelOffClick(object sender, RoutedEventArgs routedEventArgs)
        {

            VIP_OffList selected = (VIP_OffList)this.OffListSelected.SelectedItem;
            if (selected == null)
            {
                return;
            }
//            foreach (var proSellSpecalOffList in selected.Pro_SellSpecalOffList)
//            {
//                foreach (var proSellListInfo in proSellSpecalOffList.Pro_SellListInfo)
//                {
//                    proSellListInfo.SpecialID = null;
//                    proSellListInfo.OffSepecialPrice = 0;
//                }
//                
//            }

            var specaloff = selected.Pro_SellSpecalOffList[0];

            var query = newselllist.Where(p => p.SpecialID == specaloff.ID).ToList();
            foreach (var proSellListInfo in query)
            {
                proSellListInfo.SpecialID = null;
                proSellListInfo.OffSepecialPrice = 0;
                proSellListInfo.Salary = null;
                proSellListInfo.ProOffListID = null;
            }

            SpecalOffLists.Remove(specaloff);
            this.SelectedOffList.Remove(selected);
            this.OffListSelected.Rebind();
            CalcOffCanselect();

       

        }

        private void Prev_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            //var newpage = new NewProSell(this.SellInfo, this.VipInfo);
            var newpage = new NewProSell(false);
            newpage.oldpage = oldpage;
            this.NavigationService.Navigate(newpage);
//            this.Content = new NewProSell(this.SellInfo, this.VipInfo);

        }

        private void OffAddClick(object sender, RoutedEventArgs e)
        {
            this.SellList.IsReadOnly = true;
            API.VIP_OffList selected = this.OffListSelect.SelectedItem as VIP_OffList;
            if (selected != null)
            {
                AddOffMethod(selected);
            }
            CalcOffCanselect();
        }

        private void OffTicket_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((VIP_OffTicket) OffTicket.SelectedItem).ID != 0)
            {
                this.SellInfo.OffTicketID = ((VIP_OffTicket) OffTicket.SelectedItem).ID;
            }
            calcprices();
            //InitGridSellList(newselllist, this.OffList);
        }

        private void SellOffSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (((VIP_OffList) SellOffSelect.SelectedItem).ID != 0)
           {
               this.SellInfo.OffID = ((VIP_OffList) SellOffSelect.SelectedItem).ID;
           }
           calcprices();
           //InitGridSellList(newselllist, this.OffList);
        }

        private void Reset_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            InitNewSellList(SellInfo.Pro_SellListInfo); 
            GridViewSellList.Clear();
            InitGridSellList(newselllist, this.OffList);
            this.OffTicket.SelectedIndex = 0;
            this.SellOffSelect.SelectedIndex = 0;

            
            
            this.SellList.IsReadOnly = false;
            this.SpecalOffLists.Clear();
            this.SelectedOffList.Clear();
            this.OffListSelected.Rebind();



            CalcOffCanselect();

        }

        private void CashPrice_ValueChanged(object sender, RadRoutedEventArgs e)
        {
            if (this.CashPrice.Value > this.cashprice)
            {
                this.CashPrice.Value = this.cashprice;
            }
            this.CardPrice.Value = this.cashprice - this.CashPrice.Value;
        }

        private void CardPrice_ValueChanged(object sender, RadRoutedEventArgs e)
        {
            if (CardPrice.Value > this.cashprice)
            {
                this.CardPrice.Value = this.cashprice;
            }
            this.CashPrice.Value = this.cashprice - this.CardPrice.Value;
        }

        private void GridViewComboBoxColumn_PropertyChanged_1(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox comboBox = (RadComboBox)e.OriginalSource;
            if (comboBox.SelectedValue == null)
            {
                return;
            }
            ((ProSellGridModel)comboBox.DataContext).SelectedOffId = Convert.ToInt32(comboBox.SelectedValue);
            //this.SellList.Rebind();
            calcprices();
            

        }

        private void NewProSellSetp2_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Back.IsEnabled = this.oldpage != null;
            this.Back.Header = "返回" + oldpage;
            // this.GridViewComboBoxColumn1.PropertyChanged += GridViewComboBoxColumn_PropertyChanged_1;
        }

        private void SellList_OnCellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            //this.SellList.Rebind();
            calcprices();
        }

        private void Back_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(oldpage);
        }

        private void SellList_OnCellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            
        }

        private void SellList_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var item = this.SellList.SelectedItem as ProSellGridModel;
            if (item == null) return;
            var proproinfoquery = Store.ProInfo.Where(p => p.ProID == item.ProID);
            if (!proproinfoquery.Any()) return;
            var proinfo = proproinfoquery.First();
            var promaininfoquery = Store.ProMainInfo.Where(p => p.ProMainID == proinfo.ProMainID);
            if (!promaininfoquery.Any())
            {
                ProNameTextBox.Text = "";
                return;
            }
            var promaininfo = promaininfoquery.First();
            ProNameTextBox.Text = promaininfo.Introduction;

            var rules = AllRulesInfos.Where(p => p.ProMainID == proinfo.ProMainID&& p.SellType==item.SellListModel.SellType);
            this.RuleTree.ItemsSource = rules;
            foreach (var rulesAllCurrentRulesInfo in rules)
            {
                if (
                    newselllist.Any(
                        p =>
                            p.Pro_SellList_RulesInfo.Select(o => o.Rules_ProMain_ID)
                                .Contains(rulesAllCurrentRulesInfo.Rules_ProMain_ID)))
                {
                    this.RuleTree.SelectedItems.Add(rulesAllCurrentRulesInfo);
                }
            }

        }

        private void RuleTree_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {

            var m = this.SellList.SelectedItem as Model.ProSellGridModel;
            if (m == null) return;
            m.SellListModel.Pro_SellList_RulesInfo.Clear();
            var selected =
                this.RuleTree.SelectedItems.Select(p => (API.Rules_AllCurrentRulesInfo) p)
                    .OrderBy(p => p.RulesTypeID)
                    .ToList();

            foreach (var selectedItem in selected)
            {
                var curr = (API.Rules_AllCurrentRulesInfo) selectedItem;
                m.SellListModel.Pro_SellList_RulesInfo.Add(new Pro_SellList_RulesInfo()
                {
                    Rules_ProMain_ID =curr.Rules_ProMain_ID,
                    OffPrice = curr.OffPrice,
                    ShowToCus = curr.ShowToCus,
                    CanGetBack = curr.CanGetBack,

                });
            }
            calcprices();
        }

        private void RuleTree_OnCellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            var m = this.SellList.SelectedItem as Model.ProSellGridModel;
            if (m == null) return;
            m.SellListModel.Pro_SellList_RulesInfo.Clear();
            var selected =
                this.RuleTree.SelectedItems.Select(p => (API.Rules_AllCurrentRulesInfo)p)
                    .OrderBy(p => p.OrderBy)
                    .ToList();
            foreach (var selectedItem in selected)
            {
                var curr = (API.Rules_AllCurrentRulesInfo)selectedItem;
                m.SellListModel.Pro_SellList_RulesInfo.Add(new Pro_SellList_RulesInfo()
                {
                    Rules_ProMain_ID = curr.Rules_ProMain_ID,
                    OffPrice = curr.OffPrice,
                    ShowToCus = curr.ShowToCus,
                    CanGetBack = curr.CanGetBack,

                });
            }
            calcprices();
        }

        private void RuleTree_OnCellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            var model = e.Cell.DataContext as Rules_AllCurrentRulesInfo;
            if (model == null) return;
            if (e.Cell.Column.UniqueName == "OffPrice")
            {
                decimal newValue = decimal.Parse(e.NewValue.ToString());
                if (newValue < model.MinPrice|| newValue > model.MaxPrice)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "规则价格必须在"+model.MinPrice+"和"+model.MaxPrice+"之间";
                }
            }
        }
    }
}