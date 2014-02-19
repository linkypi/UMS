using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using UserMS.API;
using UserMS.Model;

namespace UserMS.Views.ProSell.Apply
{
    /// <summary>
    /// AddSellBack.xaml 的交互逻辑
    /// </summary>
    public partial class AddSellBack : Page
    {
        private int GetSellInfoMethod_ID = 170;
        private int AddSellBackAduit_Method_ID = 31;
        private API.Pro_SellInfo SellInfo;
        private List<ProSellBackGridModel> SellGridModels = new List<ProSellBackGridModel>();

        private List<API.VIP_OffList> canceledoff = new List<VIP_OffList>();
        private List<API.Pro_SellSpecalOffList> canceledofflist = new List<Pro_SellSpecalOffList>(); 
        
        public AddSellBack()
        {
            InitializeComponent();
            this.SellList.ItemsSource = SellGridModels;
            this.SellID.SearchEvent = SearchEvent;
            this.BackOffGrid.ItemsSource = canceledoff;

        }

        private void SearchEvent(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SellID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入销售单号");
                return;
            }
            PublicRequestHelp a = new PublicRequestHelp(SellBusy, GetSellInfoMethod_ID, new object[] { this.SellID.Text }, SearchSellID_Completed);
    
        }
        private void SearchSellID_Completed(object sender, MainCompletedEventArgs e)
        {
            this.SellBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    this.SellInfo = (Pro_SellInfo)e.Result.Obj;
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
                        b.IMEI = proSellListInfo.IMEI;
                        b.TicketNum = proSellListInfo.TicketID;




                        SellGridModels.Add(b);
                    }
                    this.SellList.Rebind();
                    this.SellDate.Text = SellInfo.SellDate.ToString();
                    this.VIPCard.Text = SellInfo.VIP_ID.ToString();


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
        private void calcprice()
        {
            this.ProPrice.Value = this.SellInfo.CashTotle;
            decimal off = 0;
            foreach (var proSellBackGridModel in SellGridModels)
            {
                off = off +
                      (proSellBackGridModel.SellListModel.CashPrice+proSellBackGridModel.SellListModel.OffSepecialPrice) *
                      proSellBackGridModel.BackCount;


            }
            off -=
                canceledofflist.Where(o => canceledofflist.Select(p => p.ID).Distinct().Contains(o.ID))
                               .Sum(q => q.OffMoney);
            


            foreach (var proSellBackGridModel in SellGridModels)
            {
                if (proSellBackGridModel.BackCount != 0)
                {
                    proSellBackGridModel.BackMoney = proSellBackGridModel.ProPrice - proSellBackGridModel.OffPrice -
                                                     proSellBackGridModel.TicketPrice +
                                                     proSellBackGridModel.OtherCash;
                }
               
            }

            this.OffPrice.Value = off;
            this.BackCash.Text = off.ToString("0.00");
            
        }
        private void Prev_OnClick(object e, object arg)
        {
            this.NavigationService.Navigate(new AddSellBack());
        }
        private void Save_OnClick(object e, object arg)
        { 
            if (SellInfo == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请先搜索");
                return;
            }
            API.Pro_SellBackAduit info=new Pro_SellBackAduit();
            info.ApplyMoney = this.OffPrice.Value;
            info.HallID = SellInfo.HallID;
            info.SellID = SellInfo.ID;
            info.SysDate = DateTime.Now;
            info.ApplyUser = Store.LoginUserInfo.UserID;
            info.ApplyDate = DateTime.Now;
            info.CusName = SellInfo.CusName;
            info.CusPhone = SellInfo.CusPhone;
            info.VIPID = SellInfo.VIP_ID;
            info.Note = this.Note.Text;

            List<API.Pro_SellBackAduitList> list=new List<Pro_SellBackAduitList>();
            if (!SellGridModels.Any(p => p.BackCount != 0))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入退货数量");
                return;
            }
            var specaloffs = canceledofflist.GroupBy(p=>p.ID).Select(p=>p.First()).ToList();
            
        
            foreach (var gridmodel in SellGridModels )
            {
                if (specaloffs.Select(p => p.ID).Contains(Convert.ToInt32(gridmodel.SellListModel.SpecialID)))
                {
                    gridmodel.BackCount = gridmodel.SellListModel.ProCount;
                    gridmodel.BackMoney = gridmodel.SellListModel.ProPrice;
                }
                
                
                if (gridmodel.BackCount > 0)
                {
                    API.Pro_SellBackAduitList l=new Pro_SellBackAduitList();
                    l.ProCount = gridmodel.BackCount;
                    l.SellListID = gridmodel.SellListModel.ID;
                    l.BackPrice = gridmodel.ProPrice;
                    list.Add(l);
                }

                
            }
            info.Pro_SellBackAduitList = list;
            info.Pro_SellBackAduitOffList=new   List<Pro_SellBackAduitOffList>();
            
            foreach (var specaloff in specaloffs)
            {
                info.Pro_SellBackAduitOffList.Add(new Pro_SellBackAduitOffList(){SpecalOffID = specaloff.ID , OffPrice = specaloff.OffMoney});
            }

            PublicRequestHelp a =new PublicRequestHelp(this.IsBusy,AddSellBackAduit_Method_ID,new object[]{info},MainEvent );

        }

        private void MainEvent(object sender, MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                    this.NavigationService.Navigate(new AddSellBack());
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败:"+e.Result.Message);
                }

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败: 服务器错误\n" + e.Error.Message);
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
                            canceledoff.Add(new VIP_OffList() { Name = proSellBackGridModel.SellListModel.OffName, ID = Convert.ToInt32(proSellBackGridModel.SellListModel.OffID) });

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

        private void SellList_OnCellEditEnded(object sender, GridViewCellEditEndedEventArgs gridViewCellEditEndedEventArgs)
        {
            calcoff();
            calcprice();
        }
    }
}
