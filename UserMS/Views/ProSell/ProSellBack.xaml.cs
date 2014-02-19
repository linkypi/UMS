using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell
{
    public partial class ProSellBack : Page
    {
        private int GetSellInfoMethod_ID = 170;
        private int AddSelllist_Method_ID = 154;
        private int MethodID_SellNext = 8;
        private int GetAduitMethod_ID = 0;
        private API.Pro_SellInfo SellInfo;
        private List<ProSellBackGridModel> SellGridModels = new List<ProSellBackGridModel>();
        public List<NewSellListInfoGridModel> SellGridModels2 = new List<NewSellListInfoGridModel>();
        private  List<API.VIP_OffList> canceledoff=new List<VIP_OffList>(); 
        private List<API.Pro_SellSpecalOffList> canceledofflist=new List<Pro_SellSpecalOffList>();
        private API.Pro_HallInfo Hall;
        public List<int> tempid =new List<int>();
        public override string ToString()
        {
            return "退货";
        }
        public ProSellBack()
        {
            InitializeComponent();
            this.SellID.SearchEvent=SearchEvent;
            this.SellID2.SearchEvent=NewSellSearchEvent;
            this.SellList.ItemsSource = SellGridModels;
            this.NewSellGrid.ItemsSource = SellGridModels2;
            this.BackOffGrid.ItemsSource = canceledoff;
            if (CommonHelper.GetHalls(107).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(107)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "权限错误");
                this.IsEnabled = false;
            }

        }
        private void calcprice()
        {
            this.ProPrice.Value = this.SellInfo.CashTotle;
            decimal off = 0;
            foreach (var proSellBackGridModel in SellGridModels)
            {
                off = off +
                      (proSellBackGridModel.ProPrice - proSellBackGridModel.OffPrice -
                       proSellBackGridModel.SpecalOffPrice + proSellBackGridModel.OtherCash)*
                      proSellBackGridModel.BackCount;

            }
            this.OffPrice.Value = off;
        }
        private void NewSellSearchEvent(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.SellID2.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入销售单号");
                return;
            }
            

        }
        private void GetSellList_End(object sender, MainCompletedEventArgs e)
        {



            this.IsBusy.IsBusy = false;
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
                            SellGridModels2.Add((NewSellListInfoGridModel)obj);
                        }
                    }
                    //SellGridModels.Clear();

                    this.NewSellGrid.Rebind();



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
            this.IsBusy.IsBusy = false;
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

                    SellGridModels2.Clear();
                    SellGridModels2.AddRange(l);

                    this.NewSellGrid.Rebind();
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


        private void SearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            if (string.IsNullOrEmpty(this.SellID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入销售单号");
                return;
            }
            PublicRequestHelp a =new PublicRequestHelp(SellBusy,GetSellInfoMethod_ID,new object[]{this.SellID.Text}, SearchSellID_Completed);
        }

        private void SearchSellID_Completed(object sender, MainCompletedEventArgs e)
        {
            this.SellBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    this.SellInfo = (Pro_SellInfo) e.Result.Obj;
                    if (this.SellInfo.HallID != this.Hall.HallID)
                    {
                        MessageBox.Show("该销售单无权操作");
                        this.SellInfo = null;
                        return;
                    }



                    this.SellID.IsEnabled = false;
                    this.VIPName.Text = this.SellInfo.CusName;
                    this.VIPPhone.Text = this.SellInfo.CusPhone;
                    foreach (var proSellListInfo in this.SellInfo.Pro_SellListInfo)
                    {
                        ProSellBackGridModel b = new ProSellBackGridModel();
                        b.SellListModel = proSellListInfo;
                        b.ProID = proSellListInfo.ProID;
                        //                        b.ProName = Store.ProInfo.First(p => p.ProID == proSellListInfo.ProID).ProName;
                        b.ProCount = proSellListInfo.ProCount;
                        b.ProPrice = proSellListInfo.ProPrice;
                        b.OffPrice = proSellListInfo.OffPrice;
                        b.SpecalOffPrice = proSellListInfo.OffSepecialPrice;
                        b.TicketPrice = proSellListInfo.CashTicket;
                        b.OtherOff = proSellListInfo.OtherOff;
                        b.IMEI = proSellListInfo.IMEI;
                        b.TicketNum = proSellListInfo.TicketID;

                        Pro_SellListInfo info = proSellListInfo;
                        
                        if ( this.SellInfo.Pro_SellBackAduit.Count(aduit => aduit.Aduited == true && aduit.Passed==true && aduit.Used == false) ==1)

                        {
                            var sellbackaduit = this.SellInfo.Pro_SellBackAduit.First(aduit => aduit.Aduited == true && aduit.Passed == true && aduit.Used == false);
                            var query = sellbackaduit.Pro_SellBackAduitList.Where(p => p.SellListID == info.ID);

                            if (query.Any())
                            {
                                b.BackCount = Convert.ToDecimal(query.First().ProCount);
                                b.BackMoney = Convert.ToDecimal(query.First().AduitBackPrice);
                            }
                            else
                            {
                                b.BackCount = 0;
                            }
                        }
                        else
                        {
                            b.BackMoney = b.ProPrice ;
                        }
                        SellGridModels.Add(b);
                    }
                    

                    this.SellList.Rebind();
                    this.SellDate.Text = SellInfo.SellDate.ToString();
                    this.VIPCard.Text = SellInfo.VIP_ID.ToString();


                    //PublicRequestHelp a=new PublicRequestHelp(this.IsBusy,this.GetAduitMethod_ID,new object[]{this.SellInfo.ID},GetAduitComp );
                    calcoff();
                    calcprice();
                    if (!( this.SellInfo.Pro_SellBackAduit.Count(aduit => aduit.Aduited == true && aduit.Used == false) ==1))
                    
                
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"注意: 该次退货并未申请审核" );
                        backcountcol.IsReadOnly = false;
                    }
                }
                    //                    else
                    //                    {
                    //                        this.SellInfo = null;
                    //                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"该销售单未申请退货或退货申请未审批");
                    //                    }
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
        {this.Loaded -= Page_Loaded;
        var a = new PublicRequestHelp(this.IsBusy, AddSelllist_Method_ID, new object[] { }, GetSellList_End2);
        }


        private void GetSellList_End2(object sender, MainCompletedEventArgs e)
        {

            this.IsBusy.IsBusy = false;
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
                       
                            this.SellGridModels2.Add(newSellListInfoGridModel);
                        
                    }
                    this.NewSellGrid.Rebind();
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


        private void Prev_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(new ProSellBack());
//            this.Content = new ProSellBack();
            
        }

        private void Save_OnClick_(object sender, RadRoutedEventArgs e)
        {
            MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
            Prev_OnClick(null,null);

        }
        
        private void Save_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (this.SellInfo == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请先搜索");
                return;
            }
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            API.Pro_SellInfo sellInfo = new Pro_SellInfo();
            //sellInfo.Seller = Store.LoginUserInfo.UserID;
            sellInfo.SysDate = DateTime.Now;
            //TODO: 优惠券

            if (this.SellInfo.VIP_ID != null) sellInfo.VIP_ID = this.SellInfo.VIP_ID;
            sellInfo.HallID = this.SellInfo.HallID;
            sellInfo.SellDate = this.SellInfo.SellDate;
            sellInfo.CusName = this.SellInfo.CusName;
            sellInfo.CusPhone = this.SellInfo.CusPhone;
            
            List<Pro_SellListInfo> sellList = new List<Pro_SellListInfo>();
            var items = this.NewSellGrid.SelectedItems.Select(p => (NewSellListInfoGridModel) p).ToList();
            tempid = items.Select(p => p.selllist.ID).ToList();
            foreach (var m in items)
            {
                Pro_SellListInfo info = new Pro_SellListInfo();
                var t = m.selllist;
                //info.ID = t.ID;
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
                    info.VIP_VIPInfo =  vipinfo ;
                    //                    info.VIP_VIPInfo_Temp.Add(vipinfo);
                }

                

                

                sellList.Add(info);
            }
            var specaloffs = canceledofflist.Distinct().ToList();


            foreach (var gridmodel in SellGridModels)
            {
                if (specaloffs.Select(p => p.ID).Contains(Convert.ToInt32(gridmodel.SellListModel.SpecialID)))
                {
                    gridmodel.BackCount = gridmodel.SellListModel.ProCount;
                    gridmodel.BackMoney = gridmodel.SellListModel.ProPrice;
                }
                
            }
            foreach (var m in SellGridModels.Where(p=>p.BackCount==0||p.BackCount<p.ProCount))
            {
                m.SellListModel.ProCount = m.SellListModel.ProCount - m.BackCount;
                m.SellListModel.SellID_Temp = m.SellListModel.SellID;
                m.SellListModel.OldSellListID = m.SellListModel.ID;
                if (canceledofflist.Select(p => p.ID).Contains(Convert.ToInt32(m.SellListModel.SpecialID)))
                {
                    m.SellListModel.OffSepecialPrice = 0;
                    m.SellListModel.SpecalOffId = null;
                    
                }
                    
                sellList.Add(m.SellListModel);
            }



            sellInfo.Pro_SellListInfo = sellList;
            var a = new PublicRequestHelp(this.IsBusy, MethodID_SellNext, new object[] { sellInfo }, NextComp);



            
        }

        private void NextComp(object sender, MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                     foreach (var proSellBackGridModel in SellGridModels)
                    {
                        if (proSellBackGridModel.BackCount > 0)
                        {
                            proSellBackGridModel.Status = ProSellBackGridModel.SellListStatus.Back;
                        }
                        else
                        {
                            proSellBackGridModel.Status=ProSellBackGridModel.SellListStatus.Old;
                        }

                    }

                     ProSellBack2 newpage = new ProSellBack2((Pro_SellInfo)e.Result.Obj, SellGridModels.Where(p => p.BackCount > 0).ToList(), (from b in (List<API.VIP_OffList>)e.Result.ArrList[0] select (API.VIP_OffList)b).ToList(), this.SellInfo);
                    newpage.tempid = tempid;
                    newpage.backspecalofflists = canceledofflist;
                    newpage.AllRulesInfos = (List<Rules_AllCurrentRulesInfo>) e.Result.ArrList[1];

                    try
                    {
                        var sellbackaduit = this.SellInfo.Pro_SellBackAduit.First(aduit => aduit.Aduited == true&&aduit.Passed==true && aduit.Used!=true);
                        newpage.sellbackaduit = sellbackaduit;
                    }
                    catch 
                    {
                        
                    }
                    this.NavigationService.Navigate(newpage);
//                    this.Content = newpage;

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


        private void calcoff()
        {
            canceledoff.Clear();
            canceledofflist.Clear();
            foreach (var proSellBackGridModel in SellGridModels)
            {
                if (proSellBackGridModel.BackCount > 0)
                {
                    if (proSellBackGridModel.SellListModel.OffID != null &&
                        proSellBackGridModel.SellListModel.OffID != 0)
                    {
                        if (canceledoff.All(p => p.ID != proSellBackGridModel.SellListModel.OffID))
                        {
                            canceledoff.Add(new VIP_OffList(){Name=proSellBackGridModel.SellListModel.OffName,ID=Convert.ToInt32(proSellBackGridModel.SellListModel.OffID)});

                        }
                    }
                    if (proSellBackGridModel.SellListModel.SpecalOffId != null &&
                        proSellBackGridModel.SellListModel.SpecalOffId != 0)
                    {
                        canceledofflist.Add(proSellBackGridModel.SellListModel.SpecalOff);
                       if (canceledoff.All(p => p.ID != proSellBackGridModel.SellListModel.SpecalOffId))
                       {
                           canceledoff.Add(new VIP_OffList()
                               {
                                   Name = proSellBackGridModel.SellListModel.SpecalOffName,
                                   ID = Convert.ToInt32(proSellBackGridModel.SellListModel.SpecalOffId)
                               });
                       }
                    }
                }


                
                
            }
            this.BackOffGrid.Rebind();
        }

        private void SellList_OnCellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            calcoff();
            calcprice();
        }

        private void AddSellList_OnClick(object sender, RoutedEventArgs e)
        {
            var a = new PublicRequestHelp(this.IsBusy, AddSelllist_Method_ID, new object[] { }, GetSellList_End);
        }
    }
}
