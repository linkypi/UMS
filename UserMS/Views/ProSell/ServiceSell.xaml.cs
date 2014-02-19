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
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell
{
    public partial class ServiceSell
    {
        //        private int MethodID_GetInListID = 10;
        private int MethodID_SellNext = 169;
        //        private int MethodID_Aduit = 74;
        private int MethodID_GetPrice = 168;
        public API.VIP_VIPInfo SellVIP;
        //public API.Pro_SellInfo SellInfo = new Pro_SellInfo();
        public string SellerID;
        //public List<ProSellGridModel> SellGridModels = new List<ProSellGridModel>();
        public List<VIP_OffTicket> VIPTicket;
        public API.Pro_HallInfo Hall;
        public List<ServiceSellGridModel> SellGridModels = new List<ServiceSellGridModel>();
                private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        private decimal cashprice;
        public string AduitID;
        public List<Pro_ProInfo> UserProInfos = Common.CommonHelper.GetPro(147);

        public List<ServiceGridModel> ServiceList = new List<ServiceGridModel>();
        public ServiceSell()
        {
            InitializeComponent();

            this.Grid.ItemsSource = SellGridModels;


            if (CommonHelper.GetHalls(147).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(147)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"权限错误");
                this.IsEnabled = false;
            }

            this.HallName.DataContext = Hall;

            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
            //var userops
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

        //        private void AduitSearchEvent(object sender, RoutedEventArgs e)
        //        {
        //            AduitID = this.AduitIDTextBox.TextBox.SearchText.Trim();
        //            var a = new PublicRequestHelp(this.AduitBusy, MethodID_Aduit,
        //                                          new object[] { AduitID },
        //                                          GetAduit_End);
        //
        //        }

        //        private void GetAduit_End(object sender, MainCompletedEventArgs e)
        //        {
        //            this.AduitBusy.IsBusy = false;
        //            if (e.Error == null)
        //            {
        //                if (e.Result.ReturnValue)
        //                {
        //                    
        //                    List<API.GetSAModelResult> results = (List<GetSAModelResult>) e.Result.Obj;
        //                    SellGridModels.Clear();
        //                    if (results[0].HallID != this.Hall.HallID)
        //                    {
        //                        AduitID = "";
        //                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 审批单所属仓库错误");
        //                        return;
        //                    }
        //                    foreach (var aduitListInfo in results)
        //                    {
        //                        WholeSellModel model = new WholeSellModel();
        //                        model.ProID = aduitListInfo.Proid;
        //                        model.ProName = aduitListInfo.Proname;
        ////                        model.ProPrice = Convert.ToDecimal(aduitListInfo.Offmoney);
        ////                        model.OffPrice = Convert.ToDecimal(aduitListInfo.ProPrice) -Convert.ToDecimal(aduitListInfo.Offmoney);
        //                        model.ProPrice = Convert.ToDecimal(aduitListInfo.ProPrice);
        //                        model.OffPrice = Convert.ToDecimal(aduitListInfo.ProPrice) -Convert.ToDecimal(aduitListInfo.Offmoney);
        //                       
        //                        model.AduitCount = Convert.ToDecimal(aduitListInfo.Procount);
        //                        model.NeedIMEI = Store.ProInfo.First(p => p.ProID == model.ProID).NeedIMEI;
        //                        SellGridModels.Add(model);
        //
        //
        //                    }
        //                    this.Grid.Rebind();
        //                    this.calcprice();
        //                    this.AduitIDTextBox.IsEnabled = false;
        //
        //                }
        //                else
        //                {
        //                    AduitID = "";
        //                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message);
        //                }
        //            }
        //            else
        //            {
        //                AduitID = "";
        //                MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
        //            }
        //        }


        //        private void ProSell_OnClick(object sender, RoutedEventArgs e)
        //        {
        //            MultSelecter m = new MultSelecter(null, Store.ProInfo, "TypeID", "ProName", new string[] { "ProID", "ProName" }, new string[] { "商品编码", "商品名称" });
        //            m.Closed += m_Closed;
        //            m.ShowDialog();
        //        }

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

        private void Hall_select_closed(object sender, WindowClosedEventArgs e)
        {
            SingleSelecter m = (SingleSelecter)sender;

            if (e.DialogResult == true && m.SelectedItem != null)
            {
                this.Hall = (Pro_HallInfo)m.SelectedItem;
                this.HallName.DataContext = this.Hall;


            }
        }

        private void Next_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;

            if (this.Hall == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择仓库");
                return;
            }

            if (SellGridModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"没有任何商品");
                return;
            }

            //            if (string.IsNullOrEmpty(SellOldID.Text))
            //            {
            //                MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售单号不能为空");
            //                return;
            //            }





            calcprice();
            API.Pro_SellInfo sellInfo = new Pro_SellInfo();
            //sellInfo.Seller = Store.LoginUserInfo.UserID;
            try
            {
                sellInfo.Seller = Store.UserInfos.Where(p => p.RealName == Seller.Text).First().UserID;
            }
            catch (Exception)
            {

                MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售员不存在");
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

                if (proSellGridModel.FreeCount > 0)
                {
                    DataContractSerializer bf = new DataContractSerializer(typeof(Pro_SellListInfo));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bf.WriteObject(ms, proSellGridModel.SellListModel);
                        ms.Position = 0;
                        var list = (Pro_SellListInfo)bf.ReadObject(ms);
                        list.ProCount = proSellGridModel.FreeCount;
                        list.CashPrice = 0;
                        list.ProPrice = 0;
                        list.IsFree = true;
                        list.Note = proSellGridModel.Note;
                        list.Pro_Sell_Service = new List<Pro_Sell_Service>();
                        list.Pro_Sell_Service.Add(new Pro_Sell_Service()
                            {
                                IMEI = proSellGridModel.SIMEI,
                                ProClass = proSellGridModel.SProClass,
                                ProName = proSellGridModel.SProName,
                                IsVIPService = true
                            });
                        sellList.Add(list);
                        if (proSellGridModel.ProCount > proSellGridModel.FreeCount)
                        {
                            proSellGridModel.SellListModel.ProCount = proSellGridModel.ProCount - proSellGridModel.FreeCount;
                            proSellGridModel.SellListModel.ProPrice = proSellGridModel.ProPrice;
                            proSellGridModel.SellListModel.CashPrice = proSellGridModel.ProPrice * proSellGridModel.SellListModel.ProCount;
                            proSellGridModel.SellListModel.Note = proSellGridModel.Note;
                            proSellGridModel.SellListModel.Pro_Sell_Service = new List<Pro_Sell_Service>();
                            proSellGridModel.SellListModel.Pro_Sell_Service.Add(new Pro_Sell_Service()
                            {
                                IMEI = proSellGridModel.SIMEI,
                                ProClass = proSellGridModel.SProClass,
                                ProName = proSellGridModel.SProName,
                                IsVIPService = false
                            });
                            sellList.Add(proSellGridModel.SellListModel);
                        }


                    }
                }
                else
                {

                    proSellGridModel.SellListModel.ProCount = proSellGridModel.ProCount;
                    proSellGridModel.SellListModel.ProPrice = proSellGridModel.ProPrice;
                    proSellGridModel.SellListModel.CashPrice = proSellGridModel.ProMoney;
                    proSellGridModel.SellListModel.Note = proSellGridModel.Note;
                    proSellGridModel.SellListModel.Pro_Sell_Service = new List<Pro_Sell_Service>();
                    proSellGridModel.SellListModel.Pro_Sell_Service.Add(new Pro_Sell_Service()
                    {
                        IMEI = proSellGridModel.SIMEI,
                        ProClass = proSellGridModel.SProClass,
                        ProName = proSellGridModel.SProName,
                        IsVIPService = false
                    });
                    sellList.Add(proSellGridModel.SellListModel);
                }
            }

            sellInfo.Pro_SellListInfo = sellList;
            var a = new PublicRequestHelp(PageBusy, MethodID_SellNext, new object[] { sellInfo }, Save_End);



        }
        private void SellNextEvent(object sender, MainCompletedEventArgs e)
        {
            PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    if (((Pro_SellInfo)e.Result.Obj).Pro_SellListInfo.Count == 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"无可销售商品");
                        return;
                    }
                    var newpage = new NewProSellSetp2((Pro_SellInfo)e.Result.Obj, (from b in (List<API.VIP_OffList>)e.Result.ArrList[0] select (API.VIP_OffList)b).ToList(), this.SellVIP);
                    newpage.AllRulesInfos = (List<Rules_AllCurrentRulesInfo>)e.Result.ArrList[1];
                    this.NavigationService.Navigate(newpage);


                    //this.Content = newpage;


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

        private void Save_End(object sender, MainCompletedEventArgs e)
        {
            this.PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                    this.NavigationService.Navigate(new ServiceSell());
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





        private void New_Click(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(new ServiceSell());
            //            this.Content = new ServiceSell();
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

        private void Grid_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            calcprice();

        }

        private void calcprice()
        {

            foreach (var serviceGridModel in ServiceList)
            {
                serviceGridModel.UsedCount = 0;
            }
            decimal freeprice = 0;
            foreach (var serviceSellGridModel in SellGridModels)
            {
                if (ServiceList.Any(p => p.ServiceModel.ProID == serviceSellGridModel.ProID && p.ProCount - p.UsedCount > 0))
                {
                    var model =
                        ServiceList.First(
                            p =>
                            p.ServiceModel.ProID == serviceSellGridModel.ProID &&
                           p.ProCount - p.UsedCount > 0);
                    if (serviceSellGridModel.ProCount > model.ProCount - model.UsedCount)
                    {
                        freeprice += (model.ProCount - model.UsedCount) * serviceSellGridModel.ProPrice;
                        serviceSellGridModel.FreeCount = model.ProCount - model.UsedCount;
                        model.UsedCount += model.ProCount - model.UsedCount;

                    }
                    else
                    {
                        freeprice += serviceSellGridModel.ProCount * serviceSellGridModel.ProPrice;
                        serviceSellGridModel.FreeCount = serviceSellGridModel.ProCount;
                        model.UsedCount += serviceSellGridModel.ProCount;

                    }

                }

            }
            this.cashprice = SellGridModels.Sum(p => p.ProMoney) - freeprice;
            this.SellPrice.Value = this.cashprice;

            CardPrice_ValueChanged(null, null);
        }

        //        private void IMEIInput_Click(object sender, RadRoutedEventArgs e)
        //        {
        //            IMEIImport w = new IMEIImport();
        //            //w.OnSelectedPro += w_OnSelectedPro;
        //            w.Closed += w_Closed;
        //            w.ShowDialog();
        //
        //        }

        //        private void w_Closed(object sender, WindowClosedEventArgs e)
        //        {
        //            IMEIImport w = sender as IMEIImport;
        //            if (w != null)
        //            {
        //                if (w.DialogResult == true)
        //                {
        //
        //
        //                    string imei = w.IMEI.Text;
        //
        //                    List<string> IMEIS = new List<string>();
        //                    foreach (
        //                        var imei1 in imei.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList()
        //                        )
        //                    {
        //                        if (imei1.Trim() != "")
        //                        {
        //                            IMEIS.Add(imei1);
        //                        }
        //
        //                    }
        //                    PublicRequestHelp helper = new PublicRequestHelp(this.PageBusy, this.MethodID_GetInListID,
        //                                                                     new object[] {IMEIS, Hall.HallID},
        //                                                                     GetInListID_Completed);
        //
        //                }
        //            }
        //        }

        //            private void GetInListID_Completed(object sender, MainCompletedEventArgs e)
        //            {
        //                this.PageBusy.IsBusy = false;
        //                    
        //                if (e.Error == null)
        //                {
        //                    if (e.Result.ReturnValue)
        //                    {
        //                        List<API.SetSelection> selectinlist =(List<SetSelection>) e.Result.Obj;
        //                        foreach (var setSelection in selectinlist)
        //                        {
        ////                           
        //                            SetSelection selection = setSelection;
        //                            var query = SellGridModels.Where(p => p.ProID == selection.Proid);
        //                            if (!query.Any())
        //                            {
        //                                MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品: "+Store.ProInfo.First(p=>p.ProID== setSelection.Proid).ProName+" 不是可批发商品, 忽略");
        //                                
        //                            }
        //                            var pro = query.First();
        //                            if (pro.IMEIList == null)
        //                            {
        //                                pro.IMEIList=new List<string>();
        //                            }
        //                            foreach (var IMEI in setSelection.ReturnIMEI )
        //                            {
        //                                if (pro.ProCount == pro.AduitCount)
        //                                {
        //                                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品: " + pro.ProName + " 串码数量已超出最大批发数量, 将忽略多余串码!");
        //                                    break;
        //                                }
        //                                pro.IMEIList.Add(IMEI);
        //                                
        //                                
        //                            }
        //
        //
        //                        }
        //                        this.Grid.Rebind();
        //                        calcprice();
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message);
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
        //                }
        //        
        //        
        //            }

        private void DelIMEI_OnClick(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            if (btn != null)
            {
                RadListBox box = btn.GetVisualParent<RadListBox>();
                ((List<string>)box.ItemsSource).Remove((string)btn.DataContext);
            }

            this.Grid.Rebind();
            calcprice();

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
                VIPTicket = SellVIP.VIP_OffTicket;
                ServiceList = new List<ServiceGridModel>();
                foreach (var vipVipService in SellVIP.VIP_VIPService)
                {
                    ServiceList.Add(new ServiceGridModel() { ServiceModel = vipVipService });
                }

                this.VIPService.ItemsSource = ServiceList;
                if (VIPTicket == null)
                {
                    VIPTicket = new List<VIP_OffTicket>();

                }
                VIPTicket.Insert(0, new VIP_OffTicket() { Name = "无", TicketID = "无", ID = 0 });
                //                OffTicket.ItemsSource = VIPTicket;
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
                VIPTicket = null;

                if (VIPTicket == null)
                {
                    VIPTicket = new List<VIP_OffTicket>();

                }
                VIPTicket.Insert(0, new VIP_OffTicket() { Name = "无", TicketID = "无", ID = 0 });
            }
        }

        private void AddItem_OnClick(object sender, RadRoutedEventArgs e)
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
        void m_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {

            MultSelecter2 s = (MultSelecter2)sender;
            if (s.DialogResult == true)
            {
                //
                //
                //
                List<SlModel.ProductionModel> i = (from object b in s.SelectedItems select (SlModel.ProductionModel)b).ToList();
                //                foreach (var proProInfo in i)
                //                {
                //                    var l = new ProSellGridModel();
                //                    l.ProID = proProInfo.ProID;
                //                    l.ProName = proProInfo.ProName;
                //                    l.ProCount = 1;
                //                    SellGridModels.Add(l);
                //                }
                //
                //
                //
                //                //this.ProID.TextBox.SearchText = "";
                //                this.Grid.Rebind();

                List<Pro_SellListInfo> list = new List<Pro_SellListInfo>();
                foreach (var proProInfo in i)
                {
                    Pro_SellListInfo model = new Pro_SellListInfo();
                    model.ProID = proProInfo.ProID;
                    model.SellType = 1;
                    list.Add(model);

                }

                PublicRequestHelp a = new PublicRequestHelp(this.PageBusy, MethodID_GetPrice, new object[] { list }, GetPrice_Completed);
            }
        }

        private void GetPrice_Completed(object sender, MainCompletedEventArgs e)
        {
            this.PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    //                    foreach (var proProInfo in i)
                    //                    {
                    //                        var l = new ProSellGridModel();
                    //                        l.ProID = proProInfo.ProID;
                    //                        l.ProName = proProInfo.ProName;
                    //                        l.ProCount = 1;
                    //                        SellGridModels.Add(l);
                    //                    }



                    List<Pro_SellListInfo> sellList = (List<Pro_SellListInfo>)e.Result.Obj;
                    foreach (var proSellListInfo in sellList)
                    {
                        if (SellGridModels.Where(p => string.IsNullOrEmpty(p.IMEI)).Select(p => p.ProID).Contains(proSellListInfo.ProID))
                            continue;

                        //                    if (SellVIP != null)
                        //                    {
                        //
                        //                        
                        //                            if (SellVIP.VIP_VIPService.Any(service => service.ProID == proSellListInfo.ProID))
                        //                            {
                        //                                Pro_SellListInfo listInfo = proSellListInfo;
                        //                                if (
                        //                                    SellVIP.VIP_VIPService.First(service => service.ProID == proSellListInfo.ProID)
                        //                                           .SCount >
                        //                                    SellGridModels.Select(model => model.SellListModel)
                        //                                                  .Where(info => info.ProID == listInfo.ProID)
                        //                                                  .Sum(info => info.ProCount))
                        //                                {
                        //                                    proSellListInfo.IsFree = true;
                        //                                    proSellListInfo.ProPrice = 0;
                        //                                    proSellListInfo.ProCount = 1;
                        //                                }
                        //                            }
                        //                    }

                        var l = new ServiceSellGridModel();
                        l.ProID = proSellListInfo.ProID;
                        l.ProPrice = proSellListInfo.ProPrice;
                        //                    l.ProName = Store.ProInfo.First(info => info.ProID == proSellListInfo.ProID).ProName;
                        l.ProCount = 1;
                        l.SellListModel = proSellListInfo;
                        l.IMEI = proSellListInfo.IMEI;

                        SellGridModels.Add(l);

                    }

                    //this.ProID.TextBox.SearchText = "";
                    this.Grid.Rebind();
                    calcprice();
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

        private void Del_Click(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            SellGridModels.Remove((ServiceSellGridModel)this.Grid.SelectedItem);
            this.Grid.Rebind();
        }

        private void IMEIInput_Click(object sender, RadRoutedEventArgs e)
        {

            IMEISell w = new IMEISell();
            w.Hall = this.Hall;
            w.OnSelectedPro += w_OnSelectedPro;
            w.ShowDialog();


        }

        void w_OnSelectedPro(object sender, SelectedProInfoArgs e)
        {

            if (UserProInfos.Select(p => p.ProID).Contains(e.ProInfo.ProID))
            {
                var l = new ProSellGridModel();

                if (SellGridModels.All(p => p.IMEI != e.IMEI))
                {
                    List<Pro_SellListInfo> list = new List<Pro_SellListInfo>();
                    API.Pro_ProInfo i = e.ProInfo;
                    l.ProID = i.ProID;
                    //                l.ProName = i.ProName;
                    l.ProCount = 1;
                    l.IMEI = e.IMEI;

                    Pro_SellListInfo model = new Pro_SellListInfo();
                    model.ProID = i.ProID;
                    model.SellType = 1;
                    model.IMEI = e.IMEI;
                    list.Add(model);

                    PublicRequestHelp a = new PublicRequestHelp(this.PageBusy, MethodID_GetPrice, new object[] { list }, GetPrice_Completed);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无该商品操作权限");
            }
        }


    }
}

