using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell.Car
{
    /// <summary>
    /// CarSell.xaml 的交互逻辑
    /// </summary>
    public partial class CarSell
    {
        private int MethodID_SellNext =51;
        private int MethodID_Search = 315;
        public API.Pro_HallInfo Hall;
        public API.VIP_VIPInfo SellVIP;
        public List<ProSellCarGridModel> SellGridModels = new List<ProSellCarGridModel>();
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        private decimal cashprice;
        public string SellerID;
        public CarSell()
        {
            InitializeComponent();
            if (CommonHelper.GetHalls(147).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(147)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "权限错误");
                this.IsEnabled = false;
            }

            this.HallName.DataContext = Hall;

            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
            //var userops
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
            this.Grid.ItemsSource = SellGridModels;
            var userinfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model => model.UserID,
                                                 (info, model) => info).ToList();
            this.Seller.ItemsSource = userinfos;
            this.Seller.TextSearchPath = "RealName";
            this.Seller.SearchEvent = SellerSearchEvent;
            this.Seller.SelectionMode = AutoCompleteSelectionMode.Single;
            this.Seller.TextBox_SelectionChanged = SellerSelectEvent;
            VIP_OnMouseLeftButtonUp(null, null);
        }

        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
                                                "Username", new string[] { "Username", "opname" },
                                                new string[] { "姓名", "职位" });

            w.Closed += SellerSearchWindowClose;
            w.ShowDialog();
        }

        private void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
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

        private void SellerSelectEvent(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            Sys_UserInfo selected = Seller.SelectedItem as Sys_UserInfo;
            if (selected != null)
            {
                this.SellerID = selected.UserID;
            }

        }
        private void VIP_OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            VIPWindow w = new VIPWindow();
            w.Closed += WOnClosed;
            w.ShowDialog();
        }

        private void WOnClosed(object sender, WindowClosedEventArgs windowClosedEventArgs)
        {
            VIPWindow w = (VIPWindow)sender;
            if (w.DialogResult == true)
            {
                SellVIP = w.SellVIP;
                VIPCard.DataContext = SellVIP;
                VIPName.DataContext = SellVIP;
                VIPPhone.DataContext = SellVIP;
                VIPPoint.DataContext = SellVIP;
                this.VIPName.IsReadOnly = true;
                this.VIPPhone.IsReadOnly = true;
                
                
            }
            else
            {
                SellVIP = null;
                VIPCard.DataContext = SellVIP;
                VIPName.DataContext = SellVIP;
                VIPPhone.DataContext = SellVIP;
                VIPPoint.DataContext = SellVIP;
                this.VIPName.IsReadOnly = false;
                this.VIPPhone.IsReadOnly = false;
                  }
        }


        private void New_Click(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(new CarSell());
           
        }
        private void Next_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;

            if (this.Hall == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择仓库");
                return;
            }

            if (SellGridModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "没有任何商品");
                return;
            }

            //            if (string.IsNullOrEmpty(SellOldID.Text))
            //            {
            //                MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售单号不能为空");
            //                return;
            //            }


            if (SellVIP == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "非会员不可代办");
                return;
            }


            calcprice();
            API.Pro_SellInfo sellInfo = new Pro_SellInfo();
            //sellInfo.Seller = Store.LoginUserInfo.UserID;
            try
            {
                sellInfo.Seller = Store.UserInfos.Where(p => p.RealName == Seller.Text).First().UserID;
            }
            catch (Exception)
            {

                MessageBox.Show(System.Windows.Application.Current.MainWindow, "销售员不存在");
                return;
            }
            sellInfo.UserID = Store.LoginUserInfo.UserID;
            sellInfo.HallID = this.Hall.HallID;
            sellInfo.CashPay = Convert.ToDecimal(CashPrice.Value);
            sellInfo.CardPay = Convert.ToDecimal(CardPrice.Value);
            sellInfo.CashTotle = Convert.ToDecimal(this.SellPrice.Value);
            if (this.SellVIP != null) sellInfo.VIP_ID = this.SellVIP.ID;
            sellInfo.SysDate = DateTime.Now;
            sellInfo.SellDate = DateTime.Now;
            sellInfo.Note = this.SellNote.Text;
            //TODO: 优惠券
            //            if (SellVIP != null) sellInfo.VIP_ID = SellVIP.ID;
            //            sellInfo.HallID = proHallInfo.HallID;
            //            sellInfo.SellDate = this.SellTime.SelectedValue;
            List<Pro_SellListInfo> sellList = new List<Pro_SellListInfo>();
            foreach (var proSellGridModel in SellGridModels)
            {




                proSellGridModel.SellListModel.Note = proSellGridModel.Note;
                    sellList.Add(proSellGridModel.SellListModel);
                
            }

            sellInfo.Pro_SellListInfo = sellList;
            var a = new PublicRequestHelp(PageBusy, MethodID_SellNext, new object[] { sellInfo }, Save_End);



        }

        private void Save_End(object sender, MainCompletedEventArgs e)
        {
            this.PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存成功");
                    this.NavigationService.Navigate(new CarSell());
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存失败: " + e.Result.Message);
                }

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存失败: 服务器错误\n" + e.Error.Message);
            }
        }
        private void calcprice()
        {

            
           
           
            this.cashprice = SellGridModels.Sum(p => p.ProMoney) ;
            this.SellPrice.Value = this.cashprice;

            CardPrice_ValueChanged(null, null);
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


        private void SearchBtnClick(object sender, RoutedEventArgs e)
        {

            var a = new PublicRequestHelp(PageBusy, MethodID_Search, new object[] { this.CName.Text.Trim(), this.CID.Text.Trim() }, Search_End);


        }

        private List<Pro_SellListInfo> Selllists=new List<Pro_SellListInfo>();
        private void Search_End(object sender, MainCompletedEventArgs e)
        {


            this.PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    Selllists = e.Result.Obj as List<Pro_SellListInfo> ?? new List<Pro_SellListInfo>();
                    SellGridModels.Clear();
                    foreach (var proSellListInfo in Selllists)
                    {
                        SellGridModels.Add(new ProSellCarGridModel()
                        {
                            ProID = proSellListInfo.ProID,
                            ProCount = proSellListInfo.ProCount,
                            ProPrice = proSellListInfo.ProPrice,
                            OtherCash = proSellListInfo.OtherCash,
                            CName = proSellListInfo.Pro_Sell_Car.First().CarName,
                            CID = proSellListInfo.Pro_Sell_Car.First().CarID,
                            Address = proSellListInfo.Pro_Sell_Car.First().Address,
                            Desc = proSellListInfo.Pro_Sell_Car.First().Desc,
                            SellListModel = proSellListInfo

                        });
                    }
                    this.Grid.ItemsSource = SellGridModels;
                    this.Grid.Rebind();
                    calcprice();
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + e.Result.Message);
                }

            }

            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
            }
        }
    }
}
