using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell
{
    public partial class NewJiangMenTicketSell_temp : INotifyPropertyChanged
    {
        //        private int MethodID_VIPGetInfo = 3;
        //        private int MethodID_IMEIGetInfo = 5;
        private int MethodID_SellNext = 153;
//        public API.VIP_VIPInfo SellVIP;
//        public API.Pro_SellInfo SellInfo = new Pro_SellInfo();
        public List<ProSellGridModel> SellGridModels = new List<ProSellGridModel>();
//        public List<VIP_OffTicket> VIPTicket;
        public API.Pro_HallInfo Hall;
        private bool jump = true;
//
//        public List<Pro_ProInfo> UserProInfos =
//            Store.ProInfo.Where(
//                o =>
//                Store.LoginRoleInfo.Sys_Role_Menu_ProInfo.Where(p => p.MenuID == 24)
//                     .Select(q => q.ProID)
//                     .Contains(o.ProID)).ToList();
//        public List<Pro_ProInfo> UserProInfos = 
//    Store.LoginRoleInfo.Sys_Role_Menu_ProInfo.Where(p => p.MenuID == 24)
//         .Join(Store.ProInfo, p => p.ProID, q => q.ProID, (p, q) => q).ToList();
        public List<Pro_ProInfo> UserProInfos = Common.CommonHelper.GetPro(220).Where(p=>p.IsTicketUsedable==true).ToList();
        public override string ToString()
        {
            return "江门兑券";
        }
        public NewJiangMenTicketSell_temp(bool NoDialog)
        {
            InitializeComponent();
            this.Grid.ItemsSource = SellGridModels;
            this.SellTime.SelectedValue = DateTime.Now;
            if (CommonHelper.GetHalls(220).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(220)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"权限错误");
                this.IsEnabled = false;
            }
           
            this.HallName.DataContext = Hall;
        }

        public NewJiangMenTicketSell_temp()
        {

            InitializeComponent();
            //            this.ProID.ItemsSource = Store.ProInfo;
            //            this.ProID.SelectionMode = AutoCompleteSelectionMode.Single;
            //            this.ProID.SearchEvent=ProIDSearchEvent;
            //            this.ProID.TextBox_SelectionChanged=ProIDSelected;
            //            this.ProID.DisplayMemberPath = "ProName";
            this.Grid.ItemsSource = SellGridModels;
            this.SellTime.SelectedValue = DateTime.Now;
//            VIP_OnMouseLeftButtonUp(null, null);

            if (CommonHelper.GetHalls(220).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(220)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"权限错误");
                this.IsEnabled = false;
            }
//
            this.HallName.DataContext = Hall;


            DispatcherTimer a = new DispatcherTimer();
            a.Interval = new TimeSpan(0, 0, 0, 1);
            a.Tick += a_Tick;
            //a.Start(); //FOR DEBUG



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



        public NewJiangMenTicketSell_temp(Pro_SellInfo SellInfo, VIP_VIPInfo vipInfo)
        {
            InitializeComponent();
//            SellVIP = vipInfo;
//            VIPCard.DataContext = SellVIP;
//            VIPName.DataContext = SellVIP;
//            VIPPhone.DataContext = SellVIP;
//            VIPPoint.DataContext = SellVIP;
            this.SellTime.SelectedValue = DateTime.Now;
//            if (SellVIP != null && SellVIP.VIP_OffTicket != null) VIPTicket = SellVIP.VIP_OffTicket;

            foreach (var i in SellInfo.Pro_SellListInfo)
            {
                var l = new ProSellGridModel();

                l.ProID = i.ProID;
//                l.ProName = Store.ProInfo.First(p => p.ProID == i.ProID).ProName;
                l.ProCount = 1;
                l.IMEI = i.IMEI;
                SellGridModels.Add(l);
            }
//            Hall = Store.ProHallInfo.First(p => p.HallID == SellInfo.HallID);
//            this.HallName.DataContext = Hall;
            this.Grid.ItemsSource = SellGridModels;
            this.Grid.Rebind();
            if (CommonHelper.GetHalls(220).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(220)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"权限错误");
                this.IsEnabled = false;
            }
        }

//        private void WOnClosed(object sender, WindowClosedEventArgs windowClosedEventArgs)
//        {
//            VIPWindow w = (VIPWindow)sender;
//            if (w.DialogResult == true)
//            {
//                SellVIP = w.SellVIP;
//                VIPCard.DataContext = SellVIP;
//                VIPName.DataContext = SellVIP;
//                VIPPhone.DataContext = SellVIP;
//                VIPPoint.DataContext = SellVIP;
//
//                VIPTicket = SellVIP.VIP_OffTicket;
//
//                if (VIPTicket == null)
//                {
//                    VIPTicket = new List<VIP_OffTicket>();
//
//                }
//                VIPTicket.Insert(0, new VIP_OffTicket() { Name = "无", TicketID = "无", ID = 0 });
//                OffTicket.ItemsSource = VIPTicket;
//            }
//        }

        private void ProIDSelected(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            //            var l = new ProSellGridModel();
            //            API.Pro_ProInfo i = (Pro_ProInfo) this.ProID.SelectedItem;
            //            l.ProID = i.ProID;
            //            l.ProName = i.ProName;
            //            l.ProCount = 1;
            //            SellGridModels.Add(l);
            //
            //
            //            this.ProID.TextBox.SearchText = "";
            //
            //            this.Grid.Rebind();






            //throw new NotImplementedException();
        }

        private void ProIDSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {

            MultSelecter m = new MultSelecter(null, Store.ProInfo, "TypeID", "ProName", new string[] { "ProID", "ProName" }, new string[] { "商品编码", "商品名称" });
            m.Closed += m_Closed;
            m.ShowDialog();

        }

        void m_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {

            MultSelecter2 s = (MultSelecter2)sender;
            if (s.DialogResult == true)
            {

                List<SlModel.ProductionModel> i = (from object b in s.SelectedItems select (SlModel.ProductionModel)b).ToList();
                foreach (var proProInfo in i)
                {
                    var l = new ProSellGridModel();
                    var proinfo = Store.ProInfo.First(p => p.ProID == proProInfo.ProID);
                    l.ProID = proProInfo.ProID;
//                    l.ProName = proProInfo.ProName;
                    l.ProCount = 1;
                    l.ProPrice = proinfo.Pro_SellTypeProduct.Any(p => p.SellType == 9)
                                      ? proinfo.Pro_SellTypeProduct.First(p => p.SellType == 9).Price
                                      : 0;
                    SellGridModels.Add(l);
                }



                //this.ProID.TextBox.SearchText = "";
                this.Grid.Rebind();
            }
        }


        private void ClearForm()
        {
            //TODO: 清空本体
            //this.Content = new NewTicketSell_temp(false);
            this.NavigationService.Navigate(new NewJiangMenTicketSell_temp(false));
        }

        private void New_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            ClearForm();
        }



        private void Del_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            SellGridModels.Remove((ProSellGridModel)this.Grid.SelectedItem);
            this.Grid.Rebind();

        }


//        private void Next_Click_(object sender, Telerik.Windows.RadRoutedEventArgs e)
//        {
//
//            if (SellGridModels.Count == 0)
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"没有任何商品");
//                return;
//            }
//            API.Pro_SellInfo sellInfo = new Pro_SellInfo();
//            sellInfo.SysDate = DateTime.Now;
//
//            sellInfo.OffTicketID = (int)(OffTicket.SelectedValue ?? 0);
////            if (SellVIP != null) sellInfo.VIP_ID = SellVIP.ID;
//            //            sellInfo.HallID = proHallInfo.HallID;
//            sellInfo.SellDate = this.SellTime.SelectedValue;
//            List<Pro_SellListInfo> sellList = new List<Pro_SellListInfo>();
//            foreach (var proSellGridModel in SellGridModels)
//            {
//                Pro_SellListInfo info = new Pro_SellListInfo();
//                info.ProID = proSellGridModel.ProID;
//                info.ProCount = proSellGridModel.ProCount;
//                info.IMEI = proSellGridModel.IMEI;
//                info.TicketID = proSellGridModel.TicketNum;
//                info.CashTicket = proSellGridModel.TicketPrice;
//                info.SellType = String.IsNullOrEmpty(proSellGridModel.TicketNum) ? 1 : 2;
//                sellList.Add(info);
//            }
//            sellInfo.Pro_SellListInfo = sellList;
//            var a = new PublicRequestHelp(PageBusy, MethodID_SellNext, new object[] { sellInfo }, SellNextEvent);
//
//
//
//
//        }


        private void Next_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            jump = true;
            SaveSellInfos();
        }

        private void SaveSellInfos()
        {
            var proHallInfo = this.Hall;
            if (proHallInfo == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择仓库");
                return;
            }

            if (SellGridModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"没有任何商品");
                return;
            }
//            if (string.IsNullOrEmpty(_SellID.Text))
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售单号不能为空");
//                return;
//            }

            API.Pro_SellInfo sellInfo = new Pro_SellInfo();
            //sellInfo.Seller = Store.LoginUserInfo.UserID;
            sellInfo.SysDate = DateTime.Now;
            //TODO: 优惠券
            sellInfo.OffTicketID = (int) (OffTicket.SelectedValue ?? 0);
//            if (SellVIP != null) sellInfo.VIP_ID = SellVIP.ID;
            sellInfo.HallID = proHallInfo.HallID;
            sellInfo.SellDate = this.SellTime.SelectedValue;
            List<Pro_SellListInfo> sellList = new List<Pro_SellListInfo>();
            foreach (var proSellGridModel in SellGridModels)
            {
                Pro_SellListInfo info = new Pro_SellListInfo();
                info.ProID = proSellGridModel.ProID;
                info.ProCount = proSellGridModel.ProCount;
                info.IMEI = proSellGridModel.IMEI;
                if (String.IsNullOrEmpty(proSellGridModel.TicketNum))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"兑券码不能为空");
                    return;
                }
                if (proSellGridModel.TicketPrice == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"兑券面值不能为0");
                    return;
                }
                info.TicketID = proSellGridModel.TicketNum;
                info.CashTicket = proSellGridModel.TicketPrice;
                info.SellType = 9; // String.IsNullOrEmpty(proSellGridModel.TicketNum) ? 1 : 2;
//                info.OldID = _SellID.Text;
                info.Note = proSellGridModel.Note;
                info.AnBu = proSellGridModel.AnBu;
//                info.ProPrice = proSellGridModel.ProPrice;
                info.LieShouPrice = proSellGridModel.LieShouPrice;
//                info.NeedAduit = true;
                sellList.Add(info);
            }
            sellInfo.Pro_SellListInfo = sellList;
            var a = new PublicRequestHelp(PageBusy, MethodID_SellNext, new object[] {sellInfo}, SellNextEvent);
        }

        private void SellNextEvent(object sender, MainCompletedEventArgs e)
        {
            PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
//                    if (((Pro_SellInfo)e.Result.Obj).Pro_SellListInfo.Count == 0)
//                    {
//                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"无可销售商品");
//                        return;
//                    }
//                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
//                    New_Click(null,null);
                    List<API.Pro_SellListInfo_Temp> models = (List<Pro_SellListInfo_Temp>)e.Result.Obj;
                    if (jump)
                    {
                        var newpage = new NewProSell(models);
                        newpage.oldpage = new NewJiangMenTicketSell_temp();
                        this.NavigationService.Navigate(newpage);
                    }
                    else
                    {
                        this.NavigationService.Navigate(new NewJiangMenTicketSell_temp());
                    }
                    //this.Content = newpage;
//                    var newpage = new NewProSellSetp2((Pro_SellInfo)e.Result.Obj, (from b in e.Result.ArrList select (API.VIP_OffList)b).ToList(), this.SellVIP);
//
//
//
//                    this.Content = newpage;
//

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
            switch (e.Key)
            {
                case (Key.F1):
                    IMEISell_OnClick(null, null);
                    break;
                case (Key.F2):
                    ProSell_OnClick(null, null);
                    break;


                default:
                    break;
            }
        }

        private void Grid_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.EditAction == GridViewEditAction.Commit)
            {
                //TODO: 查询兑券
            }

        }

        private void IMEISell_OnClick(object sender, RoutedEventArgs e)
        {
//            if (this.Hall == null)
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请先选择仓库");
//                return;
//            }

            UserMS.IMEISell w = new UserMS.IMEISell();
            w.Hall = Hall;
            w.OnSelectedPro += w_OnSelectedPro;
           
            w.ShowDialog();
            w.Left = 222;
            w.Top = 100;
//           w.Left = (Application.Current.Host.Content.ActualWidth - w.Width);
//           w.Top = (Application.Current.Host.Content.ActualHeight - w.Height);

        }

        void w_OnSelectedPro(object sender, SelectedProInfoArgs e)
        {

            foreach (var imei in e.Results.Keys)
            {
                if (UserProInfos.Select(p => p.ProID).Contains(e.Results[imei].ProID))
                {

                    var l = new ProSellGridModel();

                    if (SellGridModels.All(p => p.IMEI != imei))
                    {
                        API.Pro_ProInfo i = e.Results[imei];
                        l.ProID = i.ProID;
                        //                l.ProName = i.ProName;
                        l.ProCount = 1;
                        l.IMEI = imei;
                        l.ProPrice = i.Pro_SellTypeProduct.Any(p => p.SellType == 9)
                                    ? i.Pro_SellTypeProduct.First(p => p.SellType == 9).Price
                                    : 0;
                        SellGridModels.Add(l);

                        this.Grid.Rebind();
                    }
                }
                else
                {

                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "串码: " + imei + " 无该商品操作权限或该商品未定价");
                }
            }

            

//            if (UserProInfos.Select(p => p.ProID).Contains(e.ProInfo.ProID))
//            {
//                var l = new ProSellGridModel();
//
//                if (SellGridModels.All(p => p.IMEI != e.IMEI))
//                {
//                    API.Pro_ProInfo i = e.ProInfo;
//                    l.ProID = i.ProID;
//                    //                l.ProName = i.ProName;
//                    l.ProCount = 1;
//                    l.IMEI = e.IMEI;
//                    l.ProPrice = i.Pro_SellTypeProduct.Any(p => p.SellType == 9)
//                                 ? i.Pro_SellTypeProduct.First(p => p.SellType == 9).Price
//                                 : 0;
//                    SellGridModels.Add(l);
//
//                    this.Grid.Rebind();
//                }
//            }
//            else
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无该商品操作权限或该商品未定价");
//            }
        }

        private void ProSell_OnClick(object sender, RoutedEventArgs e)
        {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();

            Common.CommonHelper.ProFilterGen(UserProInfos.Where(p => p.NeedIMEI == false).ToList(), ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
                                            pros, "ProName", new string[] { "ClassName", "TypeName", "ProName", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "商品属性" });
            m.Closed += m_Closed;
            m.ShowDialog();
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

            SingleSelecter m = new SingleSelecter(parent, Store.ProHallInfo, "AreaID", "HallName", new string[] { "HallID", "HallName" }, new string[] { "仓库编号", "仓库名称" }, true);
            m.Closed += Hall_select_closed;
            m.ShowDialog();

        }
//
        void Hall_select_closed(object sender, WindowClosedEventArgs e)
        {
            SingleSelecter m = (SingleSelecter)sender;

            if (e.DialogResult == true)
            {
                this.Hall = (Pro_HallInfo)m.SelectedItem;
                this.HallName.DataContext = this.Hall;

            }
        }

//        private void VIP_OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
//        {
//            VIPWindow w = new VIPWindow();
//            w.Closed += WOnClosed;
//            w.ShowDialog();
//        }
        private void Save_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            jump = false;
            SaveSellInfos();
        }

        
    }
}
