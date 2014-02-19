using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls;
using UserMS.API;
using UserMS.Model;

namespace UserMS.Views.ProSell.Air
{
    public partial class AddSingleAir
    {
        public API.Pro_HallInfo Hall;
        public API.Pro_ProInfo Pro;
        public string SellerID;
        public List<AirChargeModel> GridViewModel=new List<AirChargeModel>();
        private int GetProStore_Method = 88;
        private int Save_Method = 94;
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public API.VIP_VIPInfo VipInfo;
        public List<API.Pro_SellListInfo_Temp> selllist = new List<Pro_SellListInfo_Temp>();
        protected List<Pro_HallInfo> userhalls;
        public List<API.Pro_ProInfo> StoreInfos = new List<Pro_ProInfo>();
        private bool canselecthall = true;
        private bool jump = true;
        public override string ToString()
        {
            return "空中充值";
        }
        public AddSingleAir()
        {
            InitializeComponent();

            this.dataGrid.ItemsSource = GridViewModel;
            this.dataGrid.Rebind();

//            foreach (var sysUserOpList in Store.UserOpList)
//            {
//                UserOpModel p = new UserOpModel();
//                p.ID = sysUserOpList.ID;
//                p.HallID = sysUserOpList.HallID;
//                p.OpID = sysUserOpList.OpID;
//                p.UserID = sysUserOpList.UserID;
//                p.Username = Store.UserInfos.First(q => q.UserID == sysUserOpList.UserID).UserName;
//                p.opname = Store.UserOp.First(q => q.OpID == sysUserOpList.OpID).Name;
//                UserOpList.Add(p);
//            }
//            this._Seller.ItemsSource = Store.UserInfos;
//            this._Seller.TextSearchPath = "RealName";
//            this._Seller.SearchEvent = SellerSearchEvent;
//            this._Seller.SelectionMode = AutoCompleteSelectionMode.Single;
//            this._Seller.TextBox_SelectionChanged = SellerSelectEvent;

            userhalls = Common.CommonHelper.FilterHall(102, Store.ProHallInfo);
            if (userhalls.Count>0)
            {
                this.Hall = userhalls.First();
            }

            this._HallInfo.DataContext = Hall;
            this._ProInfo.DataContext = Pro;
            this._VIP.DataContext = VipInfo;


         

        }

        private void ProStoreLoadCompleted(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
             if (e.Error == null)
             {
                 if (e.Result.ReturnValue)
                 {
                     this.StoreInfos = (List<Pro_ProInfo>) e.Result.Obj;
                     this.Pro = StoreInfos.First();
                     this._ProInfo.DataContext = Pro;
                     this._Balance.Value =
                    StoreInfos.First(p => p.ProID == Pro.ProID).Pro_StoreInfo.Where(p => p.HallID == this.Hall.HallID).Sum(p => p.ProCount);
               
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


        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
            Sys_UserInfo selected = _Seller.SelectedItem as Sys_UserInfo;
            if (selected != null)
            {
                this.SellerID = selected.UserID;
            }

        }



//        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
//        {
//            SingleSelecter w = new SingleSelecter(null, Store.UserInfos, null,
//                                                  "RealName", new string[] { "RealName", },
//                                                  new string[] { "姓名", });
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
//                if (window.DialogResult == true)
//                {
//                    Sys_UserInfo selected = (Sys_UserInfo)window.SelectedItem;
//                    this.SellerID = selected.UserID;
//                    this._Seller.TextBox.SearchText = selected.RealName;
//
//                }
//            }
//
//        } 
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
                    this.SellerID = selected.UserID;
                    this._Seller.TextBox.SearchText = selected.Username;

                }
            }

        } 
        
        List<Pro_ProInfo> UserProInfo=new List<Pro_ProInfo>();

        // 当用户导航到此页面时执行。
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            try
            {
                UserProInfo = Common.CommonHelper.GetPro(Convert.ToInt32(System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"]));
            }
            catch
            {
                UserProInfo = Common.CommonHelper.GetPro(102);
            }
            PublicRequestHelp helper = new PublicRequestHelp(busy, GetProStore_Method, new object[] { Common.CommonHelper.GetPro(102).Select(p => p.ProID).ToList() }, ProStoreLoadCompleted);


        }

        public void rebindgrid()
        {
            
            this.GridViewModel=new List<AirChargeModel>();
            foreach (var proSellListInfo in selllist)
            {
                AirChargeModel model=new AirChargeModel();
                model.ProID = proSellListInfo.ProID;
                model.ProCount = proSellListInfo.ProCount;
                model.ProPrice = proSellListInfo.ProPrice;
                model.ProMoney = proSellListInfo.CashPrice;
                model.ProName = Store.ProInfo.First(p => p.ProID == model.ProID).ProName;
                model.ChargePhone = proSellListInfo.ChargePhoneNum;
                model.SellListModel = proSellListInfo;
                model.OldID = proSellListInfo.OldID;
                model.Note = proSellListInfo.Note;
                model.Name = proSellListInfo.ChargePhoneName;
                GridViewModel.Add(model);

            }
            this.dataGrid.ItemsSource = GridViewModel;
            this.dataGrid.Rebind();

        }


        private void clearfrom ()
        {
            //this.Hall = null;
           // this.Pro = null;
            this._OLDID.Text = "";
           // this._ProInfo.DataContext = null;
//            this._HallInfo.DataContext = null;
           // this._Balance.Value = 0;
            this._ChargePrice.Value = 0;
            this._VIP.DataContext = null;
            this.VipInfo = null;
            this._Name.Text = "";
            this._Phone.Value = "";
            this._Note.Text = "";


        }

        private void _Add_Click_1(object sender, RoutedEventArgs e)
        {
            
            if (this.Hall == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择仓库");
                return;
            }

            if (this.Pro == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择空充号码");
                return;
            }

            if (this._Phone.Value.Length != 11)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"电话号码长度不对");
                return;
            }
            if (this._ChargePrice.Value == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入充值金额");
                return;
            }
            Pro_SellListInfo_Temp sells = new Pro_SellListInfo_Temp();
            sells.ProID = Pro.ProID;
            sells.ProCount = Convert.ToDecimal(_ChargePrice.Value);
            sells.ProPrice = Pro.Pro_SellTypeProduct.First(p => p.SellType == 1).Price;
            sells.CashPrice = sells.ProPrice*sells.ProCount;
            sells.ChargePhoneNum = this._Phone.Value;
            sells.ChargePhoneName = this._Name.Text;
            sells.OldID = this._OLDID.Text;
            sells.SellType = 1;
            sells.UserID = Store.LoginUserInfo.UserID;
            sells.Note = this._Note.Text;
            sells.HallID = this.Hall.HallID;
            sells.InsertDate = DateTime.Now;
            selllist.Add(sells);
            rebindgrid();
            clearfrom();
            canselecthall = false;




            //throw new NotImplementedException();
        }

        private void _HallInfo_Click(object sender, RoutedEventArgs e)
        {
//            if (this.Hall != null)
//            {
//                return;
//            }
            if(canselecthall){
            SingleSelecter w =new SingleSelecter(null,userhalls,null,null,new []{"HallID","HallName"},new []{"仓库编号","仓库名称"} );
            w.Closed += hall_select_Closed;
            w.ShowDialog();
            }
        }

        void hall_select_Closed(object sender, WindowClosedEventArgs e)
        {
            SingleSelecter w =sender as SingleSelecter;
            if (w == null)
            {
                return;
            }
            if (w.DialogResult == true)
            {
                Hall=w.SelectedItem as Pro_HallInfo;
                this._HallInfo.DataContext = this.Hall;
                canselecthall = false;
                this.Pro = null;
                this._ProInfo.DataContext = null;
                this._Balance.Value = 0;
            }
        }

        

        private void _ProInfo_OnClick(object sender, RoutedEventArgs e)
        {
            if (Hall == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请先选择仓库");
                return;
            }

            //var pros = Common.CommonHelper.GetPro(105);


            SingleSelecter w = new SingleSelecter(null, StoreInfos.Where(p => p.Pro_StoreInfo.Where(o => o.HallID == this.Hall.HallID).Sum(q => q.ProCount) > 0), null, "ProName", new[] { "ProID", "ProName" }, new[] { "商品编号", "商品名称" });

            w.Closed += pro_select_Closed;
            w.ShowDialog();
        }

        private void pro_select_Closed(object sender, WindowClosedEventArgs e)
        {

            SingleSelecter w = sender as SingleSelecter;
            if (w == null)
            {
                return;
            }
            if (w.DialogResult == true)
            {
                Pro = w.SelectedItem as Pro_ProInfo;
                if (Pro==null || !Pro.Pro_SellTypeProduct.Any(p => p.SellType == 1))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "该号码未定价, 无法进行空中充值");
                    return;
                }
                this._ProInfo.DataContext = Pro;
                this._Balance.Value =
                    StoreInfos.First(p => p.ProID == Pro.ProID).Pro_StoreInfo.Where(p=>p.HallID==this.Hall.HallID).Sum(p=>p.ProCount);
               // PublicRequestHelp helper=new PublicRequestHelp(busy,GetProStore_Method,new object[]{Hall.HallID,Pro.ProID},ProStoreCheckCompleted );

            }
        }

        private void ProStoreCheckCompleted(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {

                if (e.Result.ReturnValue)
                {
                    Pro_ProInfo store = e.Result.Obj as Pro_ProInfo;
                    this.Pro = store;
                    this._ProInfo.DataContext = store;
                    if (store != null) this._Balance.Value = store.Pro_StoreInfo.Sum(p => p.ProCount);
                    else
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 返回值错误");
                    }
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

        private void _VIP_OnClick(object sender, RoutedEventArgs e)
        {
            if (VipInfo != null)
            {
                return;
            }
            VIPWindow v=new VIPWindow();
            v.Closed += v_Closed;
            v.ShowDialog();
        }

        void v_Closed(object sender, WindowClosedEventArgs e)
        {
            VIPWindow v =sender as VIPWindow;
            if (v == null)
            {
                return;
            }
            if (v.DialogResult == true)
            {
                this.VipInfo = v.SellVIP;
                this._VIP.DataContext = VipInfo;
                this._Phone.Value = VipInfo.MobiPhone;
                this._Name.Text = VipInfo.MemberName;
                this._Name.IsReadOnly = true;
            }
        }

        private void _Next_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            jump = true;
            SaveSellInfos();
        }

        private void SaveSellInfos()
        {
            if (selllist.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"没有可提交内容");
                return;
            }
            PublicRequestHelp help = new PublicRequestHelp(this.busy, Save_Method, new object[] {selllist}, _Save_Completed);
        }

        private void newpage()
        {
            this.NavigationService.Navigate(new AddSingleAir());
        }

        

        private void _Save_Completed(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {


                    List<API.Pro_SellListInfo_Temp> models = (List<Pro_SellListInfo_Temp>)e.Result.Obj;
                    if (jump)
                    {
                        var newpage = new NewProSell(models);
                        newpage.oldpage = new AddSingleAir();
                        this.NavigationService.Navigate(newpage);
                    }
                    else
                    {
                        this.NavigationService.Navigate(new AddSingleAir());
                    }
//
//                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
//                    newpage();

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

        private void _New_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            newpage();
        }

        private void _Del_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            AirChargeModel item = this.dataGrid.SelectedItem as AirChargeModel;
            if (item != null)
            {
                selllist.Remove(item.SellListModel);
                rebindgrid();
            }
        }

        private void _Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            clearfrom();
        }

        private void _Save_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            jump = false;
            SaveSellInfos();
            
        }

        private void _ChargePrice_OnValueChanged(object sender, RadRoutedEventArgs e)
        {
         if (_ChargePrice.Value > _Balance.Value)
         {
             MessageBox.Show(System.Windows.Application.Current.MainWindow,"充值金额不能大于剩余金额");
             _ChargePrice.Value = 0;
         }
         if (_ChargePrice.Value < 0)
         {
             _ChargePrice.Value = 0;
         }
        }
    }
}
