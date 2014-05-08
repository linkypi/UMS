using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
using SlModel;
using Telerik.Windows;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell.YanBao
{
    /// <summary>
    /// import.xaml 的交互逻辑
    /// </summary>
    public partial class import : Page
    {
        private List<YanbaoModelImport> Yanbaomodellist = new List<YanbaoModelImport>();
        private List<API.Pro_ProInfo> RolePros;
        private List<API.View_YanBoPriceStepInfo> Yanbaoprices;
        private int GetCurrentPrice_MethodID = 146;
        private int Save_MethodID = 365;
        public API.Pro_HallInfo Hall;
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public import()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            

            string r = "";
            int MenuID = 0;
            try
            {
                var a = NavigationService.Source.OriginalString.Split('?').Reverse().First();

                //r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["ProID"];
                MenuID = Convert.ToInt32(System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"]);
            }
            catch
            {
                //r = "9";
                MenuID = 101;
            }
            r = Store.Options.First(option => option.ClassName == "GXYanBao").Value;
            RolePros = Common.CommonHelper.GetPro(MenuID).Where(p => p.ProID == r).ToList();
            if (RolePros.Count != 1)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "页面权限错误");
                this.IsEnabled = false;

            }
            else
            {


                var a = new PublicRequestHelp(this.busy, GetCurrentPrice_MethodID, new object[] { }, GetCurrentPrice_End);

            }

            if (Store.ProHallInfo.Count() == 1)
            {
                Hall = CommonHelper.GetHalls(101)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                Hall = CommonHelper.GetHalls(101)[0];
            }
            this._Hall.DataContext = this.Hall;


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

        }
        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
            Sys_UserInfo selected = Seller.SelectedItem as Sys_UserInfo;
//            if (selected != null)
//            {
//                this.SellInfo.Seller = selected.UserID;
//            }
//            else
//            {
//                this.SellInfo.Seller = null;
//            }

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
//                    this.SellInfo.Seller = selected.UserID;
                    this.Seller.TextBox.SearchText = selected.Username;


                }
            }

        } 


        private void GetCurrentPrice_End(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    this.Yanbaoprices = (List<View_YanBoPriceStepInfo>)e.Result.Obj;

                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "获得延保价格失败: " + e.Result.Message);
                    this.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "获得延保价格失败: 服务器错误\n" + e.Error.Message);
                this.IsEnabled = false;
            }
        }

        private void _Del_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            if (this.dataGrid.SelectedItem != null)
            {
                this.Yanbaomodellist.Remove((YanbaoModelImport)this.dataGrid.SelectedItem);

            }
            this.dataGrid.Rebind();
            calcprice();
        }

        private void _CancelALL_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.Yanbaomodellist.Clear();
            this.dataGrid.Rebind();
            calcprice();
          
        }

        private List<Pro_SellListInfo> sellListInfos;
        private void _Save_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            //            
            //            {
            //                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
            //                this.Content = new NewProSell(true);
            //                //TODO:Clear
            //                return;
            //            }
            List<API.Pro_SellInfo> SellInfoList=new List<Pro_SellInfo>();
            
            //API.Pro_SellInfo Sellinfo = new Pro_SellInfo();

            string seller;
                try
                {
                    seller = Store.UserInfos.First(p => p.RealName == Seller.Text).UserID;
                }
                catch (Exception)
                {

                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "销售员不存在");
                    return;
                }

            foreach (var yanbaoModel in Yanbaomodellist)
            {
                API.Pro_SellInfo Sellinfo = new Pro_SellInfo();
                
                Pro_SellListInfo selllist = new Pro_SellListInfo();
                //                selllist.ProID = RolePros[0].ProID;
                //                selllist.ProCount=1;
                selllist.OldID = yanbaoModel.SellID;
                Pro_Sell_Yanbao yanbao = new Pro_Sell_Yanbao();
                yanbao.BillID = yanbaoModel.OldID;
                yanbao.BateriNum = yanbaoModel.BatteryIMEI;
                yanbao.FatureNum = yanbaoModel.InvoiceNumber;
                yanbao.NgarkuesNum = yanbaoModel.ChargerIMEI;
                yanbao.KufjeNum = yanbaoModel.HandphoneIMEI;
                yanbao.MobileType = yanbaoModel.Class;
                yanbao.MobileName = yanbaoModel.Model;
                yanbao.MobilePrice = yanbaoModel.ModelPrice;

                yanbao.MobileIMEI = yanbaoModel.IMEI;
                yanbao.UserName = yanbaoModel.Name;
                yanbao.UserPhoneNum = yanbaoModel.Phone;
                yanbao.Note = yanbaoModel.Note;
                yanbao.YanBaoName = yanbaoModel.YanbaoName;
                selllist.Pro_Sell_Yanbao = yanbao;

                selllist.ProID = RolePros.First().ProID;

                selllist.IMEI = yanbao.BillID;


                selllist.ProCount = 1;
                selllist.SellType = 1;
                selllist.ProPrice = Convert.ToDecimal(
                                   Store.YanbaoPriceStep.Where(
                                       p => p.ProID == selllist.ProID && p.StepPrice >= yanbao.MobilePrice)
                                        .OrderBy(p => p.StepPrice).First().ProPrice);

                yanbaoModel.YanbaoPrice = selllist.ProPrice;
                selllist.CashPrice = selllist.ProPrice;
                yanbao.YanBaoName = Store.YanbaoPriceStep.Where(
                    p => p.ProID == selllist.ProID && p.StepPrice >= yanbao.MobilePrice)
                                               .OrderBy(p => p.StepPrice).First().Name;

                Sellinfo.Pro_SellListInfo = new List<Pro_SellListInfo>() { selllist };
                Sellinfo.UserID = Store.LoginUserInfo.UserID;
                Sellinfo.Note = this.Note.Text;
                Sellinfo.CashPay = selllist.ProPrice;
                Sellinfo.CardPay = 0;
                Sellinfo.CashTotle = selllist.ProPrice;
                Sellinfo.SysDate = DateTime.Now;
                Sellinfo.UserID = Store.LoginUserInfo.UserID;
                Sellinfo.SellDate = DateTime.Now;
                Sellinfo.HallID = this.Hall.HallID;
                Sellinfo.Seller = seller;
                Sellinfo.OldID = yanbaoModel.SellOLDID;
                Sellinfo.CusName = yanbaoModel.CustName;
                Sellinfo.CusPhone = yanbaoModel.CustPhone;
                SellInfoList.Add(Sellinfo);
            }


            PublicRequestHelp helper = new PublicRequestHelp(this.busy, Save_MethodID, new object[] { SellInfoList }, Save_Temp_Event);
    

//            PublicRequestHelp helper = new PublicRequestHelp(this.busy, Save_MethodID, new object[] { sellListInfos },
//                                                             Save_Completed);
        }

        private void Save_Temp_Event(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    List<API.Print_SellListInfo> list = (List<Print_SellListInfo>)e.Result.Obj;
                    
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存成功");
                       
                                this.NavigationService.Navigate(new import());
                       
                
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

        private void _File_OnClick(object sender, RoutedEventArgs e)
        {
            SlModel.operateExcel<YanbaoModelImport> m = new operateExcel<YanbaoModelImport>();
            OpenFileDialog d=new    OpenFileDialog();
            d.Filter = "Excel (*.xls)|*.xls";
            if (d.ShowDialog()==true)
            {
                var o = m.fromExcel(new Hashtable()
                {
                    
                    {"OldID","合同号"},
                    {"IMEI","终端串码"},
                    {"Model","终端型号"},
                    {"Class","终端品牌"},
                    {"ModelPrice","终端价格"},
                    {"InvoiceNumber","发票号码"},

                    {"BatteryIMEI","电池编码"},
                    {"ChargerIMEI","充电器编码"},
                    {"HandphoneIMEI","耳机编码"},
                    {"Note","备注"},
                    {"CustPhone","客户电话"},
                    {"CustName","客户姓名"},
                    {"SellOLDID","原始单号"},
                    
                }, d.FileName);
                if (o != null)
                {
                    foreach (var yanbaoModel in o)
                    {
                        yanbaoModel.IMEI = yanbaoModel.IMEI.ToUpper();
                        yanbaoModel.OldID = yanbaoModel.OldID.ToUpper();
                    }
                    Yanbaomodellist = o;
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "文件打开错误" );
                    Yanbaomodellist = new List<YanbaoModelImport>();
                }
                this.dataGrid.ItemsSource = Yanbaomodellist;
                calcprice();
            };
        }

        private void calcprice()
        {
            cashprice = 0;
            sellListInfos = new List<Pro_SellListInfo>();
            List<Pro_Sell_Yanbao> YanbaoInfos = new List<Pro_Sell_Yanbao>();
            foreach (var yanbaoModel in Yanbaomodellist)
            {
                Pro_SellListInfo selllist = new Pro_SellListInfo();
                //                selllist.ProID = RolePros[0].ProID;
                //                selllist.ProCount=1;
                selllist.OldID = yanbaoModel.SellID;
                Pro_Sell_Yanbao yanbao = new Pro_Sell_Yanbao();
                yanbao.BillID = yanbaoModel.OldID;
                yanbao.BateriNum = yanbaoModel.BatteryIMEI;
                yanbao.FatureNum = yanbaoModel.InvoiceNumber;
                yanbao.NgarkuesNum = yanbaoModel.ChargerIMEI;
                yanbao.KufjeNum = yanbaoModel.HandphoneIMEI;
                yanbao.MobileType = yanbaoModel.Class;
                yanbao.MobileName = yanbaoModel.Model;
                yanbao.MobilePrice = yanbaoModel.ModelPrice;

                yanbao.MobileIMEI = yanbaoModel.IMEI;
                yanbao.UserName = yanbaoModel.Name;
                yanbao.UserPhoneNum = yanbaoModel.Phone;
                yanbao.Note = yanbaoModel.Note;
                yanbao.YanBaoName = yanbaoModel.YanbaoName;
                selllist.Pro_Sell_Yanbao = yanbao;

                selllist.ProID = RolePros.First().ProID;

                selllist.IMEI = yanbao.BillID;

                
                selllist.ProCount = 1;
                selllist.SellType = 1;
                selllist.ProPrice = Convert.ToDecimal(
                                   Store.YanbaoPriceStep.Where(
                                       p => p.ProID == selllist.ProID && p.StepPrice >= yanbao.MobilePrice)
                                        .OrderBy(p => p.StepPrice).First().ProPrice);

                yanbaoModel.YanbaoPrice = selllist.ProPrice;
                selllist.CashPrice = selllist.ProPrice;
                yanbao.YanBaoName = Store.YanbaoPriceStep.Where(
                    p => p.ProID == selllist.ProID && p.StepPrice >= yanbao.MobilePrice)
                                               .OrderBy(p => p.StepPrice).First().Name;
                
                sellListInfos.Add(selllist);
                cashprice += selllist.ProPrice;
            }
            
           
            //this.SellPrice.Text = cashprice.ToString("0.00");
            this.SellPrice.Value = cashprice;

            this.CashPrice.Value = cashprice;
            this.CardPrice.Value = 0;
        }


        private decimal cashprice;
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
    }
}
