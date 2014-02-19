using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Model;

namespace UserMS.Views.ProSell
{
    public partial class WholeProSellSetp2 : Page
    {
        private readonly Pro_SellInfo _sellInfo;
        private List<ProSellGridModel> GridViewSellList = new List<ProSellGridModel>();
        private decimal sellprice;
        private decimal offprice;
        private decimal cashprice;
        private List<API.Pro_SellListInfo> newselllist = new List<Pro_SellListInfo>();
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public int Save_MethodID = 51;
        public WholeProSellSetp2()
        {
            InitializeComponent();
        }

        public WholeProSellSetp2(Pro_SellInfo SellInfo, Pro_SellAduit sellAduit)
        {
            _sellInfo = SellInfo;
            InitializeComponent();

            
            foreach (Pro_SellListInfo proSellListInfo in SellInfo.Pro_SellListInfo)
            {
                ProSellGridModel p = new ProSellGridModel();
                p.ProID = proSellListInfo.ProID;
//                p.ProName = Store.ProInfo.First(pp => pp.ProID == proSellListInfo.ProID).ProName;
                p.ProCount = proSellListInfo.ProCount;
                //p.Unit = Store.ProInfo.First(pp => pp.ProID == proSellListInfo.ProID).pro;
                proSellListInfo.WholeSaleOffPrice = sellAduit.Pro_SellAduitList.First(q => q.ProID == p.ProID).OffMoney;
                p.ProPrice = proSellListInfo.ProPrice-proSellListInfo.WholeSaleOffPrice;
                
                p.IMEI = proSellListInfo.IMEI;
                p.TicketNum = proSellListInfo.TicketID;
                p.TicketPrice = proSellListInfo.CashTicket;
                //p.OffLists = OffList.Where(t => t.Type == 0 && t.VIP_ProOffList.Any(q => q.ProID == p.ProID)).ToList();
                if (p.OffLists == null) p.OffLists = new List<VIP_OffList>();
                
                p.OffLists.Insert(0, new VIP_OffList() { ID = 0, Name = "无" });
                p.SellListModel = proSellListInfo;
                GridViewSellList.Add(p);
            }
            SellList.ItemsSource = GridViewSellList;
            calcprices();




            foreach (var sysUserOpList in Store.UserOpList)
            {
                UserOpModel p = new UserOpModel();
                p.ID = sysUserOpList.ID;
                p.HallID = sysUserOpList.HallID;
                p.OpID = sysUserOpList.OpID;
                p.UserID = sysUserOpList.UserID;
                p.Username = Store.UserInfos.First(q => q.UserID == sysUserOpList.UserID).RealName;
                p.opname = Store.UserOp.First(q => q.OpID == sysUserOpList.OpID).Name;
                UserOpList.Add(p);
            }

            this.Seller.ItemsSource = UserOpList;
            this.Seller.TextSearchPath = "RealName";
            this.Seller.SearchEvent = SellerSearchEvent;
            this.Seller.SelectionMode = AutoCompleteSelectionMode.Single;
            this.Seller.TextBox_SelectionChanged = SellerSelectEvent;

        }
        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
            UserOpModel selected = Seller.SelectedItem as UserOpModel;
            if (selected != null)
            {
                this._sellInfo.Seller = selected.ID+"";
            }

        }
        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
                                                  "Username", new string[] { "Username", "opname" },
                                                  new string[] { "Username", "opname" });

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
                    this._sellInfo.Seller = selected.ID+"";
                    this.Seller.TextBox.SearchText = selected.Username;

                }
            }

        } 
        private void calcprices()
        {

            sellprice = 0;
            offprice = 0;
            cashprice = 0;
            foreach (var proSellListInfo in _sellInfo.Pro_SellListInfo)
            {
                sellprice = sellprice + Convert.ToDecimal(proSellListInfo.ProPrice) * Convert.ToDecimal(proSellListInfo.ProCount);
                proSellListInfo.CashPrice = (proSellListInfo.ProPrice - proSellListInfo.OffPrice -
                                             proSellListInfo.OffSepecialPrice - proSellListInfo.TicketUsed - proSellListInfo.WholeSaleOffPrice) *
                                            proSellListInfo.ProCount;

            }

//            foreach (var vipOffList in SelectedOffList)
//            {
//                offprice = offprice + Convert.ToDecimal(vipOffList.OffMoney);
//            }
//            if (_sellInfo.OffID != null && OffList.Any(p => p.ID == SellInfo.OffID))
//                offprice = offprice + Convert.ToDecimal(OffList.First(p => p.ID == SellInfo.OffID).OffMoney);

            if (_sellInfo.OffTicketPrice != null)
                offprice = offprice + Convert.ToDecimal(_sellInfo.OffTicketPrice);
            cashprice = sellprice - offprice;
            if (cashprice < 0) cashprice = 0;
            _sellInfo.CashTotle = cashprice;
            this.ProPrice.Text = sellprice.ToString();
            this.OffPrice.Text = offprice.ToString();
            this.SellPrice.Text = cashprice.ToString();

        }

        // 当用户导航到此页面时执行。
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
        }

        private void Prev_OnClick(object sender, RadRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Save_OnClick(object sender, RadRoutedEventArgs e)
        {
            API.Pro_SellInfo Sellinfo = this._sellInfo;
            //Sellinfo.Pro_SellListInfo = this.newselllist;
            Sellinfo.UserID = Store.LoginUserInfo.UserID;



            
            calcprices();


            PublicRequestHelp helper = new PublicRequestHelp(this.IsBusy, Save_MethodID, new object[] { Sellinfo }, SaveEvent);


        }

        private void SaveEvent(object sender, MainCompletedEventArgs e)
        {




            this.IsBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");

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

        private void Reset_OnClick(object sender, RadRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OffTicket_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SellOffSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OffListSelect_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DelOffClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OffAddClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
