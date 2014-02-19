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
using UserMS.Report.Print.SellBackPrint;
using UserMS.Report.Print.SellPrint;

namespace UserMS.Views.ProSell
{
    public partial class ProSellBack2 : Page
    {
        private int GetSellInfoMethod_ID = 0;
        private API.Pro_SellInfo SellInfo;
        private List<ProSellBackGridModel> GridViewSellList = new List<ProSellBackGridModel>();
        private List<ProSellBackGridModel> GridViewSellListOld=new      List<ProSellBackGridModel>();
        public List<API.VIP_OffList> SelectedOffList = new List<VIP_OffList>();
        public List<API.Pro_SellListInfo> newselllist = new List<Pro_SellListInfo>();
        private Pro_SellInfo Orig_Sellinfo;
        public List<VIP_OffList> canselect = new List<VIP_OffList>();
        public int Save_MethodID = 176;
        public List<VIP_OffTicket> VIPTicket;
        public API.Pro_SellBackAduit sellbackaduit { get; set; }
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public List<API.Pro_SellSpecalOffList> SpecalOffLists = new List<Pro_SellSpecalOffList>();
        public List<API.Rules_AllCurrentRulesInfo> AllRulesInfos = new List<Rules_AllCurrentRulesInfo>();
        private decimal sellprice = 0;
        private decimal offprice = 0;
        private decimal cashprice = 0;
        private decimal backprice = 0;
        private decimal lastprice = 0;
        private decimal aduitblock = 0;
        private decimal shouldbackprice = 0;
        public List<API.VIP_OffList> OffList { get; set; }
        public bool cansave;
        public List<int> tempid { get; set; }
        public List<API.Pro_SellSpecalOffList>  backspecalofflists=new List<Pro_SellSpecalOffList>();
        public ProSellBack2()
        {
            InitializeComponent();
            //this.SellID.SearchEvent=SearchEvent;
        }

        

        public ProSellBack2(Pro_SellInfo sellInfo, List<ProSellBackGridModel> model,List<VIP_OffList> OffList ,Pro_SellInfo origsellinfo)
        {
            InitializeComponent();
            this.Orig_Sellinfo = origsellinfo;
            this.GridViewSellListOld = model;
            this.OffList = OffList;
            this.SellInfo = sellInfo;

            this.SellID.Text = Orig_Sellinfo.SellID + "";
            this.SellDate.Text = Orig_Sellinfo.SellDate + "";
            this.VIPName.Text = Orig_Sellinfo.CusName + "";
            this.VIPPhone.Text = Orig_Sellinfo.CusPhone + "";
            this.VIPCard.Text = Orig_Sellinfo.VIP_ID + "";
            this.ORGSellPrice.Text = Orig_Sellinfo.CashTotle.ToString("0.00");
            this.OldID.Text = Orig_Sellinfo.OldID + "";
            
            InitGridSellList(sellInfo.Pro_SellListInfo, this.OffList);
            this.OffListSelected.ItemsSource = SelectedOffList;
            InitNewSellList(SellInfo.Pro_SellListInfo);
            this.OffListSelect.ItemsSource = canselect;

            CalcOffCanselect();
            List<VIP_OffList> sellofflist = OffList.Where(p => p.Type == 2).ToList();
            sellofflist.Insert(0, new VIP_OffList() { ID = 0, Name = "无" });
            this.SellOffSelect.ItemsSource = sellofflist;
            this.SellOffSelect.SelectedIndex = 0;
            calcprices();
            InitGridSellList(newselllist, this.OffList);

            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
            UserOpList =
                userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
                             (list, info) => new {op = list, user = info})
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
            var userinfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model1 => model1.UserID,
                                                 (info, model1) => info).ToList();
            this.Seller.ItemsSource = userinfos;
            this.Seller.TextSearchPath = "RealName";
            this.Seller.SearchEvent = SellerSearchEvent;
            this.Seller.SelectionMode = AutoCompleteSelectionMode.Single;
            this.Seller.TextBox_SelectionChanged = SellerSelectEvent;
            try
            {
                this.Seller.TextBox.SearchText = Store.UserInfos.First(p => p.UserID == origsellinfo.Seller).RealName;
                
                this.Seller.IsEnabled = false;
            }
            catch
            {
                
            }
            
            this.SellList.AddHandler(RadComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(GridViewComboBoxColumn_PropertyChanged_1));
       

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

        private void InitNewSellList(List<Pro_SellListInfo> listInfos)
        {
            newselllist = new List<Pro_SellListInfo>();

            DataContractSerializer bf = new DataContractSerializer(typeof(List<Pro_SellListInfo>));
            using (MemoryStream ms = new MemoryStream())
            {
                bf.WriteObject(ms, listInfos);
                ms.Position = 0;
                newselllist = (List<Pro_SellListInfo>)bf.ReadObject(ms);

            }
        }

        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
           // throw new NotImplementedException();
           
        }

        private void SellerSearchEvent(object sender, RoutedEventArgs e)
        {
           // throw new NotImplementedException();
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
                                                "Username", new string[] { "Username", "opname" },
                                                new string[] { "姓名", "职位" });

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
                   
                    this.Seller.TextBox.SearchText = selected.Username;


                }
            }

        } 
        private void InitGridSellList(List<Pro_SellListInfo> Pro_SellListInfo, List<VIP_OffList> OffList)
        {
            GridViewSellList.Clear();
            foreach (Pro_SellListInfo proSellListInfo in Pro_SellListInfo)
            {
                ProSellBackGridModel p = new ProSellBackGridModel();
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
                
               
                
                p.SellListModel = proSellListInfo;
                if (proSellListInfo.SellID_Temp == null||proSellListInfo.SellID_Temp==0){
                    p.Status = ProSellBackGridModel.SellListStatus.New;
                    p.OffLists = OffList.Where(t => t.Type == 0 && t.VIP_ProOffList.Any(q => q.ProID == p.ProID)).ToList();
                    p.OffLists.Insert(0, new VIP_OffList() { ID = 0, Name = "无" });
                    p.SelectedOffId = Convert.ToInt32(proSellListInfo.OffID);
                }
                else
                {
                    proSellListInfo.SellID = proSellListInfo.SellID_Temp;
                    p.Status = ProSellBackGridModel.SellListStatus.Old;
                    p.OffLists = OffList.Where(t => t.Type == 0 && t.VIP_ProOffList.Any(q => q.ProID == p.ProID && q.OffID == proSellListInfo.OffID)).ToList();

                    p.SelectedOffId = Convert.ToInt32(proSellListInfo.OffID);
                    
                }
                p.IsOK = true;
                GridViewSellList.Add(p);
            }
            foreach (var proSellBackGridModel in GridViewSellListOld)
            {
                proSellBackGridModel.ProCount = -proSellBackGridModel.BackCount;
            }
            GridViewSellList.AddRange(GridViewSellListOld);

            SellList.ItemsSource = GridViewSellList;
            SellList.Rebind();
        }

        private void calcprices()
        {
            sellprice = 0;
            offprice = 0;
            cashprice = 0;
            backprice = 0;
            cansave = true;
            shouldbackprice =
                this.GridViewSellListOld.Sum(
                    p =>
                    Convert.ToDecimal(p.BackCount)*
                    (((Convert.ToDecimal(p.BackMoney - p.SellListModel.AnBuPrice - p.SellListModel.OffPrice - p.SellListModel.OffSepecialPrice -
                                         p.SellListModel.TicketUsed - p.SellListModel.WholeSaleOffPrice - p.SellListModel.OtherOff)) < 0
                          ? 0
                          : (Convert.ToDecimal(p.BackMoney - p.SellListModel.AnBuPrice - p.SellListModel.OffPrice - p.SellListModel.OffSepecialPrice -
                                               p.SellListModel.TicketUsed - p.SellListModel.WholeSaleOffPrice  -p.SellListModel.OtherOff))) -
                     p.SellListModel.LieShouPrice + p.SellListModel.OtherCash));
            shouldbackprice += this.GridViewSellListOld.Sum(p => p.BackCount*p.SellListModel.OffSepecialPrice);
            shouldbackprice =shouldbackprice- this.backspecalofflists.Sum(p => p.OffMoney);
            foreach (var proSellListInfo in newselllist)
            {
                proSellListInfo.AnBuPrice = (proSellListInfo.ProPrice - proSellListInfo.TicketUsed) <
                                            proSellListInfo.AnBu
                                                ? (proSellListInfo.ProPrice - proSellListInfo.TicketUsed)
                                                : proSellListInfo.AnBu;

                decimal temp = proSellListInfo.ProPrice -
                               proSellListInfo.TicketUsed - proSellListInfo.AnBuPrice - proSellListInfo.OffPrice - proSellListInfo.WholeSaleOffPrice -
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


                if (proSellListInfo.CashPrice < proSellListInfo.LieShouPrice)
                {
                    if (GridViewSellList.Any(p => p.SellListModel == proSellListInfo))
                    {
                        var t = GridViewSellList.First(p => p.SellListModel == proSellListInfo);
                        t.IsOK = false;

                    }

                    cansave = false;
                }



                cashprice += proSellListInfo.CashPrice * proSellListInfo.ProCount;



            }

            //cashprice = newselllist.Sum(p => p.CashPrice);
           // if (cashprice < 0) cashprice = 0;
//            cashprice = cashprice +
//                       (this.GridViewSellListOlShouldBackCashd.Sum(
//                           p => Convert.ToDecimal(p.BackCount)*Convert.ToDecimal(p.SellListModel.CashPrice)) -
//                        shouldbackprice);
            //SellInfo.CashTotle = cashprice;

            foreach (var vipOffList in SelectedOffList)
            {
                offprice = offprice + Convert.ToDecimal(vipOffList.OffMoney);
            }
            if (SellInfo.OffID != null && OffList.Any(p => p.ID == SellInfo.OffID))
                offprice = offprice + Convert.ToDecimal(OffList.First(p => p.ID == SellInfo.OffID).OffMoney);

            if (SellInfo.OffTicketPrice != null)
                offprice = offprice + Convert.ToDecimal(SellInfo.OffTicketPrice);
            backprice = cashprice  -Orig_Sellinfo.CashTotle;
            this.ShouldBackPrice.Text = backprice.ToString("0.00");
            aduitblock = this.GridViewSellListOld.Sum(p => p.BackCount * (p.SellListModel.ProPrice -p.BackMoney));
            backprice = backprice + aduitblock;
                        


            this.CashPrice.Value = backprice;
            this.CardPrice.Value = 0;


            this.ProPrice.Text = cashprice.ToString("0.00");
            this.OffPrice.Text = offprice.ToString("0.00");
            this.BackPrice.Text = backprice.ToString("0.00");
            //this.AllPrice.Text = lastprice.ToString("0.00");
            CardPrice_ValueChanged(null, null);
        }

//        private void calcprices_()
//        {
//            sellprice = 0;
//            offprice = 0;
//            cashprice = 0;
//            backprice = 0;
//
//            lastprice = 0;
//            foreach (var model in GridViewSellList.Where(p=>p.Status!=ProSellBackGridModel.SellListStatus.New).GroupBy(p=>p.SellListModel.SpecialID))
//            {
//                if (model.Key == null || model.Key == 0)
//                {
//                    //無特殊優惠組
//                    foreach (var m in model)
//                    {
//                        if (m.BackCount != 0)
//                        {
//                            decimal temp = (m.SellListModel.ProPrice - m.SellListModel.OffPrice -
//                                       m.SellListModel.OffSepecialPrice - m.SellListModel.TicketUsed);
////                            sellprice += temp * (m.SellListModel.ProCount - m.BackCount);
////                            offprice += (m.SellListModel.OffPrice + m.SellListModel.OffSepecialPrice +
////                                         m.SellListModel.CashTicket) * (m.SellListModel.ProCount - m.BackCount);
//                            backprice += temp * m.BackCount;
//                            
//                        }
//                        else
//                        {
////                            sellprice += m.SellListModel.CashPrice;
////                            offprice += (m.SellListModel.OffPrice + m.SellListModel.OffSepecialPrice + m.SellListModel.CashTicket) * m.SellListModel.ProCount;
//
//                        }
//                    }
//                }
//                else
//                {
//                    //特殊優惠組
//                    if (model.Any(p => p.BackCount > 0))
//                    {
//                        //有退貨
//                       
//                        foreach (var m in model)
//                        {
//                            //刪去所有特殊優惠
//                            backprice -= m.SpecalOffPrice*m.SellListModel.ProCount;
////                            m.SpecalOffPrice = 0;
////                            m.SellListModel.OffSepecialPrice = 0;
////                            
//                            if (m.BackCount != 0)
//                            {
//                                decimal temp = (m.SellListModel.ProPrice - m.SellListModel.OffPrice 
//                                            - m.SellListModel.TicketUsed);
////                                sellprice += temp * (m.SellListModel.ProCount - m.BackCount);
////                                offprice += (m.SellListModel.OffPrice + 
////                                             m.SellListModel.CashTicket) * (m.SellListModel.ProCount - m.BackCount);
//                                backprice += temp * m.BackCount;
//                            }
//                            else
//                            {
//                                decimal temp = (m.SellListModel.ProPrice - m.SellListModel.OffPrice  - m.SellListModel.TicketUsed);
////                                sellprice += temp * (m.SellListModel.ProCount - m.BackCount);
////                                offprice += (m.SellListModel.OffPrice +
////                                             m.SellListModel.CashTicket) * (m.SellListModel.ProCount - m.BackCount);
//                                backprice += temp * m.BackCount;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        //無退貨
//                        foreach (var m in model)
//                        {
//                            //sellprice += m.SellListModel.CashPrice;
//                            //offprice += (m.SellListModel.OffPrice + m.SellListModel.OffSepecialPrice + m.SellListModel.CashTicket) * m.SellListModel.ProCount;
//
//                        }
//
//                    }
//                }
//            }
//
//
////
////            foreach (var m in GridViewSellList.Where(p=>p.Status!=ProSellBackGridModel.SellListStatus.New))
////            {
////                if (m.BackCount != 0)
////                {
////                    if (m.SellListModel.SpecialID == null || m.SellListModel.SpecialID == 0)
////                    {
////                        decimal temp = (m.SellListModel.ProPrice - m.SellListModel.OffPrice -
////                                        m.SellListModel.OffSepecialPrice - m.SellListModel.TicketUsed);
////                        sellprice += temp*(m.SellListModel.ProCount - m.BackCount);
////                        offprice += (m.SellListModel.OffPrice + m.SellListModel.OffSepecialPrice +
////                                     m.SellListModel.CashTicket)*(m.SellListModel.ProCount - m.BackCount);
////                        backprice += temp*m.BackCount;
////
////                    }
////                }
////                else
////                {
////                    sellprice += m.SellListModel.CashPrice;
////                    offprice += (m.SellListModel.OffPrice + m.SellListModel.OffSepecialPrice + m.SellListModel.CashTicket)*m.SellListModel.ProCount;
////
////                }
////            }
//
//            foreach (var proSellListInfo in newselllist)
//            {
//                sellprice = sellprice + Convert.ToDecimal(proSellListInfo.ProPrice) * Convert.ToDecimal(proSellListInfo.ProCount);
//                proSellListInfo.CashPrice = (proSellListInfo.ProPrice - proSellListInfo.OffPrice -
//                                             proSellListInfo.OffSepecialPrice - proSellListInfo.TicketUsed + proSellListInfo.OtherCash) *
//                                            proSellListInfo.ProCount;
//
//            }
//            foreach (var vipOffList in SelectedOffList)
//            {
//                offprice = offprice + Convert.ToDecimal(vipOffList.OffMoney);
//            }
//            if (SellInfo.OffID != null && OffList.Any(p => p.ID == SellInfo.OffID))
//                offprice = offprice + Convert.ToDecimal(OffList.First(p => p.ID == SellInfo.OffID).OffMoney);
//
//            if (SellInfo.OffTicketPrice != null)
//                offprice = offprice + Convert.ToDecimal(SellInfo.OffTicketPrice);
//            cashprice = newselllist.Sum(p => p.CashPrice);
//            if (cashprice < 0) cashprice = 0;
//            SellInfo.CashTotle = cashprice;
//            lastprice = sellprice - offprice - backprice;
//            this.ProPrice.Text = sellprice.ToString("0.00");
//            this.OffPrice.Text = offprice.ToString("0.00");
//            this.BackPrice.Text = backprice.ToString("0.00");
//            //this.AllPrice.Text = lastprice.ToString("0.00");
//            //this.SellPrice.Text = cashprice.ToString("0.00");
//            // this.SellPrice.Value = cashprice;
//
//
//            //CardPrice_ValueChanged(null, null);
//        }


        private void CashPrice_ValueChanged(object sender, RadRoutedEventArgs e)
        {
//            if (this.CashPrice.Value > this.backprice)
//            {
//                this.CashPrice.Value = this.backprice;
//            }
            this.CardPrice.Value = this.backprice - this.CashPrice.Value;
        }

        private void CardPrice_ValueChanged(object sender, RadRoutedEventArgs e)
        {
//            if (CardPrice.Value > this.backprice)
//            {
//                this.CardPrice.Value = this.backprice;
//            }
            this.CashPrice.Value = this.backprice - this.CardPrice.Value;
        }
        

        private void CalcOffCanselect()
        {

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

                if (vipOffList.Type == 1)
                {
                    try
                    {
                        foreach (VIP_ProOffList vipProOffList in a)
                        {
                            var query =
                                newselllist.Where(p=>p.OldSellListID==null||p.OldSellListID==0).Any(
                                    p =>
                                    p.ProID == vipProOffList.ProID && (p.SpecialID == 0 || p.SpecialID == null) &&
                                    p.ProCount >= vipProOffList.ProCount);

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

        private void SearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            PublicRequestHelp a =new PublicRequestHelp(SellBusy,GetSellInfoMethod_ID,new object[]{this.Seller.TextBox.SearchText}, SearchSellID_Completed);
        }

        private void SearchSellID_Completed(object sender, MainCompletedEventArgs e)
        {
            this.SellBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    this.SellInfo = (Pro_SellInfo) e.Result.Obj;
                    this.SellID.IsEnabled = false;

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

        // 当用户导航到此页面时执行。
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            calcprices();
        }

        private void SellSelect_OnClick(object sender, RadRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Prev_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(new ProSellBack());
        }

        private void Save_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;



            API.Pro_SellBack sellBack=new       Pro_SellBack();


           
        try
        {
            sellBack.UserID = Store.UserInfos.Where(p => p.RealName == Seller.Text).First().UserID;
        }
        catch (Exception)
        {

            MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售员不存在");
            return;
        }

                        if (string.IsNullOrEmpty(OldID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入原始单号");
                return;
            }

            System.Text.RegularExpressions.Regex  ex=new System.Text.RegularExpressions.Regex(@"^.{7}$");
            if (!ex.IsMatch(OldID.Text))
            {
                MessageBox.Show(Application.Current.MainWindow, "请输入正确的原始单号");
                return;
            }
            if (!cansave)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,
                    "有部分延保或商品无法销售 请检查明细");
                return;
            }

            sellBack.AduitID = "";//TODO
            //sellBack.BackMoney = this.shouldbackprice;
            sellBack.CusName = this.VIPName.Text;
            sellBack.CusPhone = this.VIPPhone.Text;
            sellBack.BillID = this.OldID.Text;
            sellBack.UpdDate = DateTime.Now;
            sellBack.SysDate = DateTime.Now;
            sellBack.UpdUser = Store.LoginUserInfo.UserID;

            sellBack.SellID = this.Orig_Sellinfo.ID;
            sellBack.OldCashTotle = this.Orig_Sellinfo.CashTotle-this.Orig_Sellinfo.OffTicketPrice;
            if (sellbackaduit!=null){
            sellBack.AduitID = sellbackaduit.AduitID;
            }
            List<Pro_SellBackList> sellbacklist=new List<Pro_SellBackList>();
            List<Pro_SellBackSpecalOffList> sellBackSpecalOffLists=new List<Pro_SellBackSpecalOffList>();
            foreach (var proSellSpecalOffList in backspecalofflists)
            {
               Pro_SellBackSpecalOffList specaloff=new Pro_SellBackSpecalOffList();
                specaloff.OffID = Convert.ToInt32(proSellSpecalOffList.SpecalOffID);
                specaloff.ID = proSellSpecalOffList.ID;
                specaloff.OffPrice = proSellSpecalOffList.OffMoney;
                if (!(sellBackSpecalOffLists.Select(p => p.ID).Contains(proSellSpecalOffList.ID)))
                {
                    sellBackSpecalOffLists.Add(specaloff);
                }

            }
            sellBack.Pro_SellBackSpecalOffList = sellBackSpecalOffLists;
            foreach (var proSellBackGridModel in GridViewSellList.Where(p=>p.BackCount>0))
            {
                Pro_SellBackList backlist = new Pro_SellBackList();
                backlist.InListID = proSellBackGridModel.SellListModel.InListID;
                backlist.ProID = proSellBackGridModel.ProID;
                backlist.IMEI = proSellBackGridModel.SellListModel.IMEI;
                backlist.LowPrice = proSellBackGridModel.SellListModel.LowPrice;
                backlist.OffID = proSellBackGridModel.SellListModel.OffID;
                backlist.OffPoint = proSellBackGridModel.SellListModel.OffPoint;
                backlist.OffPrice = proSellBackGridModel.SellListModel.OffPrice;
                backlist.OffSepecialPrice = proSellBackGridModel.SellListModel.OffSepecialPrice;
                backlist.ProCost = proSellBackGridModel.SellListModel.ProCost;
                backlist.ProPrice = proSellBackGridModel.SellListModel.ProPrice;
                backlist.SellType = proSellBackGridModel.SellListModel.SellType;
                backlist.SellType_Pro_ID = proSellBackGridModel.SellListModel.SellType_Pro_ID;
                backlist.SpecialID = proSellBackGridModel.SellListModel.SpecialID;
                backlist.TicketID = proSellBackGridModel.SellListModel.TicketID;
                backlist.TicketUsed = proSellBackGridModel.SellListModel.TicketUsed;
                backlist.WholeSaleOffPrice = proSellBackGridModel.SellListModel.WholeSaleOffPrice;
                backlist.ProCount = proSellBackGridModel.BackCount;
                backlist.SellListID = proSellBackGridModel.SellListModel.ID;
                 backlist.OtherCash = proSellBackGridModel.SellListModel.OtherCash;
                backlist.AnBu = proSellBackGridModel.AnBu;
                //backlist.LieShou = proSellBackGridModel.LieShou;
                backlist.LieShouPrice = proSellBackGridModel.LieShouPrice;

                backlist.OtherOff = proSellBackGridModel.OtherOff;
                backlist.CashTicket = proSellBackGridModel.SellListModel.CashTicket;
                backlist.AnBuPrice = proSellBackGridModel.SellListModel.AnBuPrice;
                decimal rules = proSellBackGridModel.SellListModel.Pro_SellList_RulesInfo.Sum(p => p.RealPrice);
                decimal temp = (proSellBackGridModel.BackMoney - backlist.AnBuPrice - backlist.OffPrice - backlist.OffSepecialPrice -
                                backlist.TicketUsed - backlist.WholeSaleOffPrice - backlist.OtherOff);
                if (temp < 0) temp = 0;
                backlist.CashPrice = temp  + backlist.OtherCash-rules;
                 temp = (backlist.ProPrice - backlist.AnBuPrice - backlist.OffPrice - backlist.OffSepecialPrice-
                                backlist.TicketUsed - backlist.WholeSaleOffPrice - backlist.OtherOff );
                if (temp < 0) temp = 0;
                backlist.ShouldBackCash = temp + backlist.OtherCash - rules;
                backlist.AduidedNewPrice = proSellBackGridModel.BackMoney;
                
                sellbacklist.Add(backlist);
            }

            sellBack.Pro_SellBackList = sellbacklist;

            foreach (var proSellListInfo in newselllist)
            {
                proSellListInfo.SellID = null;
                if (proSellListInfo.Pro_Sell_JiPeiKa != null)
                {
                    proSellListInfo.Pro_Sell_JiPeiKa.ID = 0;
                }
                if (proSellListInfo.Pro_Sell_Service != null && proSellListInfo.Pro_Sell_Service.Count>0)
                {
                    foreach (Pro_Sell_Service proSellService in proSellListInfo.Pro_Sell_Service)
                    {
                        proSellService.ID = 0;
                    }
                }
                if (proSellListInfo.Pro_Sell_Yanbao != null)
                {
                    proSellListInfo.Pro_Sell_Yanbao.ID = 0;
                }

                if (proSellListInfo.OtherOff != 0)
                    proSellListInfo.NeedAduit = true;
            }
            sellBack.Pro_SellListInfo = newselllist;
            sellBack.Pro_SellSpecalOffList = SpecalOffLists;
            sellBack.CashTotle = backprice;
            sellBack.NewCashTotle = cashprice;
            sellBack.CashPay = Convert.ToDecimal(this.CashPrice.Value);
            sellBack.CardPay = Convert.ToDecimal(this.CardPrice.Value);
            //sellBack.CashPay = sellprice ;

            //sellBack.OldCashTotle = Orig_Sellinfo.CashTotle;
            sellBack.BackMoney = sellbacklist.Sum(p => (p.CashPrice + p.OffSepecialPrice)*p.ProCount) -
                                 sellBackSpecalOffLists.Sum(p => p.OffPrice);
                                 
//            sellBack.BackMoney
            sellBack.ShouldBackCash = sellbacklist.Sum(p => (p.ShouldBackCash + p.OffSepecialPrice)*p.ProCount) -
                                      sellBackSpecalOffLists.Sum(p => p.OffPrice);
            PublicRequestHelp a=new PublicRequestHelp(this.IsBusy,Save_MethodID,new object[]{sellBack,tempid},MainEvent );

        }

        private void MainEvent(object sender, MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    List<API.Print_SellListInfo> list = (List<Print_SellListInfo>)e.Result.Obj;
                    if (list == null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存成功, 但需要审核");
                        this.NavigationService.Navigate(new ProSellBack());
                    }
                    else
                    {
                        MessageBoxResult rsltMessageBox = MessageBox.Show(
                            System.Windows.Application.Current.MainWindow, "保存成功。是否打印", "提示", MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);
                        if (rsltMessageBox == MessageBoxResult.Yes)
                        {

                            List<ReportService.Print_SellListInfo> newlist =
                                new List<ReportService.Print_SellListInfo>();
                            newlist = list.Select(b => new ReportService.Print_SellListInfo()
                                {
                                    串码_号码_合同号 = b.串码_号码_合同号,
                                    优惠 = b.优惠,
                                    优惠券名称 = b.优惠券名称,
                                    优惠券金额 = b.优惠券金额,
                                    会员卡号 = b.会员卡号,
                                    刷卡 = b.刷卡,
                                    券号_合约号 = b.券号_合约号,
                                    券面值 = b.券面值,
                                    单价 = b.单价,
                                    原始单号 = b.原始单号,
                                    商品名称 = b.商品名称,
                                    备注 = b.备注,
                                    实收总额 = b.实收总额,
                                    实收总额大写 = b.实收总额大写,
                                    客户电姓名 = b.客户电姓名,
                                    客户电话 = b.客户电话,
                                    应收总额 = b.应收总额,
                                    数量 = b.数量,
                                    现金 = b.现金,
                                    系统自增外键编号 = b.系统自增外键编号,
                                    自增主键编号 = b.自增主键编号,
                                    金额小计 = b.金额小计,
                                    销售公司 = b.销售公司,
                                    销售员 = b.销售员,
                                    销售日期 = b.销售日期,
                                    //退货单号 = b.退货单号,
                                    销售门店 = b.销售门店
                                }).ToList();
                            PrintSellBillOne newpage =
                                new PrintSellBillOne(newlist);
                            newpage.oldpage = new ProSellBack();

                            this.NavigationService.Navigate(newpage);
                        }
                        else
                        {
                            this.NavigationService.Navigate(new ProSellBack());
                        }
                        //this.Content = new NewProSell(true);
                    }
                }
                else
                {
                    string errmsg = "保存失败: " + e.Result.Message;
                    if (e.Result.Obj is Pro_SellBack)
                    {
                        var r = e.Result.Obj as Pro_SellBack;
                        foreach (var v in r.Pro_SellListInfo)
                        {
                            if (!string.IsNullOrEmpty(v.Note))
                            errmsg += "\n" + v.Note;
                        }
                    }
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, errmsg);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败: 服务器错误\n" + e.Error.Message);
            }
        }

        private void Reset_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
        }

        private void IMEIBack_OnClick(object sender, RadRoutedEventArgs e)
        {
            IMEISell w=new  IMEISell();
            w.OnSelectedPro += w_OnSelectedPro;
            w.ShowDialog();
            
        }

        void w_OnSelectedPro(object sender, SelectedProInfoArgs e)
        {
            var selected = e.ProInfo;
            var l = new ProSellGridModel();
            API.Pro_ProInfo i = e.ProInfo;
            l.ProID = i.ProID;
//            l.ProName = i.ProName;
            l.ProCount = 1;
            l.IMEI = e.IMEI;
            //SellGridModels.Add(l);
            //TODO: 搜索串码退库
            //this.Grid.Rebind();

            var q = SellInfo.Pro_SellListInfo.Where(p => p.IMEI == e.IMEI);
            if (q.Any())
            {
                //TODO: 退指定串碼
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串碼不存在");
            }


            this.SellList.Rebind();
        }



        private void OffTicket_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((VIP_OffTicket)OffTicket.SelectedItem).ID != 0)
            {
                this.SellInfo.OffTicketID = ((VIP_OffTicket)OffTicket.SelectedItem).ID;
            }
            calcprices();
        }

        private void SellOffSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((VIP_OffList)SellOffSelect.SelectedItem).ID != 0)
            {
                this.SellInfo.OffID = ((VIP_OffList)SellOffSelect.SelectedItem).ID;
            }
            calcprices();
        }

        private void OffListSelect_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
           
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
                            p.ProCount >= vipProOffList.ProCount).OrderByDescending(p => p.ProCount);
                    decimal need = (decimal)vipProOffList.ProCount;


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
                            
                            if (query.ProPrice - query.OffPrice <= vipProOffList.AfterOffPrice)
                            {
                                query.OffSepecialPrice = query.ProPrice - query.OffPrice;
                            }
                            else
                            {
                                //query.OffSepecialPrice = query.ProPrice - query.OffPrice - vipProOffList.AfterOffPrice;
                                query.OffSepecialPrice = query.ProPrice - vipProOffList.AfterOffPrice;
                            }

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
                            if (b.ProPrice - b.OffPrice <= vipProOffList.AfterOffPrice)
                            {
                                b.OffSepecialPrice = b.ProPrice - b.OffPrice;
                            }
                            else
                            {
                                b.OffSepecialPrice = b.ProPrice - b.OffPrice - vipProOffList.AfterOffPrice;
                            }
                            b.IMEI = query.IMEI;
                            b.SpecialID = specalOff.ID;
                            b.OffID = query.OffID;
                            //specalOff.Pro_SellListInfo.Add(b);
                            newselllist.Add(b);
                            break;
                        }
                        else
                        {
                            need = (decimal)(need - query.ProCount);
                            query.SpecialID = specalOff.ID;
                            if (query.ProPrice - query.OffPrice <= vipProOffList.AfterOffPrice)
                            {
                                query.OffSepecialPrice = query.ProPrice - query.OffPrice;
                            }
                            else
                            {
                                query.OffSepecialPrice = query.ProPrice - query.OffPrice - vipProOffList.AfterOffPrice;
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

        private void DelOffClick(object sender, RoutedEventArgs e)
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
            }

            SpecalOffLists.Remove(specaloff);
            this.SelectedOffList.Remove(selected);
            this.OffListSelected.Rebind();
            CalcOffCanselect();

       
        }
         

        private void GetSellInfoEnd(object sender, MainCompletedEventArgs e)
        {
            
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    this.SellInfo=(Pro_SellInfo) e.Result.Obj;
                    //TODO 绑定各种
                    this.SellDate.Text = SellInfo.SellDate.ToString();
                    this.VIPCard.Text = SellInfo.VIP_ID.ToString();
                    this.SellPrice.Text = SellInfo.CashPay.ToString();
                    this.OffPrice.Text = "0";
                    


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

        private void SellList_OnCellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            calcprices();
        }

        private void RuleTree_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var m = this.SellList.SelectedItem as ProSellBackGridModel;
            if (m == null) return;
            if (m.SellListModel.SellID > 0) return;
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

        private void SellList_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var item = this.SellList.SelectedItem as ProSellBackGridModel;
            if (item == null) return;
            if (item.SellListModel.SellID > 0) return;
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

            var rules = AllRulesInfos.Where(p => p.ProMainID == proinfo.ProMainID && p.SellType == item.SellListModel.SellType);
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

        private void RuleTree_OnCellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            var m = this.SellList.SelectedItem as Model.ProSellGridModel;
            if (m == null) return;
            if (m.SellListModel.SellID > 0) return;
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
                if (newValue < model.MinPrice || newValue > model.MaxPrice)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "规则价格必须在" + model.MinPrice + "和" + model.MaxPrice + "之间";
                }
            }
        }
    }
}
