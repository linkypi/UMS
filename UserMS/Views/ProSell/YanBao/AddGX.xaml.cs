using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Telerik.Windows;
using Telerik.Windows.Controls;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell.YanBao
{
    public partial class AddGX
    {
        public API.Pro_HallInfo Hall;
        public API.Pro_SellListInfo_Temp SellList;
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        private int SellerID;
        private int CheckIMEI_MethodID = 109;
        private int Save_MethodID = 110;
        private List<YanbaoModel>  Yanbaomodellist=new List<YanbaoModel>();
        private API.Pro_ProInfo Pro;
        private int GetCurrentPrice_MethodID = 146;
        private List<API.Pro_ProInfo> RolePros;
        private List<API.View_YanBoPriceStepInfo> Yanbaoprices;
        private bool jump = true;
        public override string ToString()
        {
            return "广信延保";
        }
        public AddGX()
        {
            InitializeComponent();

//            foreach (var sysUserOpList in Store.UserOpList)
//            {
//                UserOpModel p = new UserOpModel();
//                p.ID = sysUserOpList.ID;
//                p.HallID = sysUserOpList.HallID;
//                p.OpID = sysUserOpList.OpID;
//                p.UserID = sysUserOpList.UserID;
//                p.Username = Store.UserInfos.First(q => q.UserID == sysUserOpList.UserID).RealName;
//                p.opname = Store.UserOp.First(q => q.OpID == sysUserOpList.OpID).Name;
//                UserOpList.Add(p);
//            }
//            this._Seller.ItemsSource = UserOpList;
//            this._Seller.TextSearchPath = "RealName";
//            this._Seller.SearchEvent = SellerSearchEvent;
//            this._Seller.SelectionMode = AutoCompleteSelectionMode.Single;
//            this._Seller.TextBox_SelectionChanged = SellerSelectEvent;

            this.dataGrid.ItemsSource = Yanbaomodellist;

            clearform();
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
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"获得延保价格失败: " + e.Result.Message);
                    this.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获得延保价格失败: 服务器错误\n" + e.Error.Message);
                this.IsEnabled = false;
            }
        }

        private void SellerSelectEvent(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            UserOpModel selected = _Seller.SelectedItem as UserOpModel;
            if (selected != null)
            {
                this.SellerID = selected.ID;
            }

        }

        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
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
                    this.SellerID = selected.ID;
                    this._Seller.TextBox.SearchText = selected.Username;

                }
            }

        }


        // 当用户导航到此页面时执行。
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            NavigationService.LoadCompleted+=new LoadCompletedEventHandler(LoadCompleted);

            string r="";
            int MenuID =0;
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
            RolePros=Common.CommonHelper.GetPro(MenuID).Where(p=>p.ProID==r).ToList();
            if (RolePros.Count != 1)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"页面权限错误");
                this.IsEnabled = false;

            }
            else
            {
                

               var a= new PublicRequestHelp(this.busy, GetCurrentPrice_MethodID, new object[] { }, GetCurrentPrice_End);
 
            }

            
            
        }

        private void LoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {
            
        }

        private void _Hall_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Hall != null)
            {
                return;
            }
            SingleSelecter w = new SingleSelecter(null, CommonHelper.GetHalls(101), null, null, new[] { "HallID", "HallName" }, new[] { "仓库编号", "仓库名称" });
            w.Closed += hall_select_Closed;
            w.ShowDialog();

        }

        void hall_select_Closed(object sender, WindowClosedEventArgs e)
        {
            SingleSelecter w = sender as SingleSelecter;
            if (w == null)
            {
                return;
            }
            if (w.DialogResult == true)
            {
                Hall = w.SelectedItem as Pro_HallInfo;
                this._Hall.DataContext = this.Hall;

            }
        }

        private void _CheckIMEI_Click_1(object sender, RoutedEventArgs e)
        {
            this._IMEI.IsReadOnly = true;
            PublicRequestHelp  helper=new PublicRequestHelp(this.busy,CheckIMEI_MethodID,new object[]{this._IMEI.Text.Trim()},CheckIMEI_Completed );

        }

        private void CheckIMEI_Completed(object sender, MainCompletedEventArgs e)
        {
            
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)

                {
                    try
                    {
                        API.Pro_IMEI imeiinfo = e.Result.ArrList[0] as Pro_IMEI;
                        API.Pro_SellListInfo selllist = e.Result.ArrList[1] as Pro_SellListInfo;
                        if (imeiinfo != null)
                        {
                            this.Pro = Store.ProInfo.First(p => p.ProID == imeiinfo.ProID);
                            
                            this._Model.Text = Pro.ProName;


                            this._ModelClass.Text =
                                Store.ProClassInfo.First(p => p.ClassID == Pro.Pro_ClassID).ClassName;

                        }
                        else
                        {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 串码不存在");
                            this._IMEI.IsReadOnly = false;
                            return;
                        }

                        if (selllist != null)
                        {
                            this._ModelPrice.Value = selllist.YanbaoModelPrice;


                        }
                        else
                        {
                            this._ModelPrice.Value = this.Pro.Pro_SellTypeProduct.Any(p => p.SellType == 1)
                                ? this.Pro.Pro_SellTypeProduct.First(p => p.SellType == 1).Price
                                : 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 数据错误\n" + ex.Message);
                        this._IMEI.IsReadOnly = false;
                    }
                  
                }
               
                
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message);
                    this._IMEI.IsReadOnly = false;
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
            }
        }

        private void _Add_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Pro == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未验证终端型号");
                return;
            }
//            if (string.IsNullOrEmpty(_SellID.Text))
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售单号不能为空");
//                return;
//            }
            if (string.IsNullOrEmpty(_OldID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"合同号不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_InvoiceNumber.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"发票号码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_BatteryIMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"电池编码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_ChargerIMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"充电器编码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_HandphoneIMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"耳机编码不能为空");
                return;
            }

            if (Convert.ToDecimal(this._ModelPrice.Value) == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "终端价格为0 不可销售延保");
                return;
            }
            YanbaoModel model=new YanbaoModel();
            model.ProID = this.Pro.ProID;
            model.Name = this._Name.Text;
            model.Phone = this._Phone.Text;
            model.OldID = this._OldID.Text;
            model.IMEI = this._IMEI.Text;
            model.ModelPrice = Convert.ToDecimal(this._ModelPrice.Value);
            model.InvoiceNumber = this._InvoiceNumber.Text;
            model.BatteryIMEI = this._BatteryIMEI.Text;
            model.ChargerIMEI = this._ChargerIMEI.Text;
            model.HandphoneIMEI = this._HandphoneIMEI.Text;
            model.Note = this._Note.Text;
            model.Model = this._Model.Text;
            model.Class = this._ModelClass.Text;
            model.IMEI = this._IMEI.Text;
            model.SellID = this._SellID.Text;
            
            var pro = RolePros.First();
            var modelpro = this.Pro;
            if (modelpro.YanBaoModelID != null && modelpro.YanBaoModelID!=0)
            {
                if (Yanbaoprices.Any(p => p.ID == modelpro.YanBaoModelID))
             {
                 model.YanbaoPrice = Convert.ToDecimal(Yanbaoprices.First(p => p.ID == modelpro.YanBaoModelID).ProPrice);
                 model.YanbaoName = Yanbaoprices.First(p => p.ID == modelpro.YanBaoModelID).Name;
             }
            }
            else
            if (
    Yanbaoprices.Any(p => p.ProID == pro.ProID && p.StepPrice >= model.ModelPrice))
            {
                
                model.YanbaoPrice =
                    Convert.ToDecimal(Yanbaoprices.Where(p => p.ProID == pro.ProID && p.StepPrice >= model.ModelPrice)
                                                  .OrderBy(p => p.StepPrice).First().ProPrice);
                model.YanbaoName = Yanbaoprices.Where(p => p.ProID == pro.ProID && p.StepPrice >= model.ModelPrice)
                                               .OrderBy(p => p.StepPrice).First().Name;
            }
            else
            {
//                model.YanbaoPrice =
//                    Convert.ToDecimal(Yanbaoprices.Where(p => p.ProID == pro.ProID)
//                                                  .OrderByDescending(p => p.StepPrice).First().ProPrice);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该终端无法购买延保服务");
                return;
                
            }



            Yanbaomodellist.Add(model);
            this.dataGrid.Rebind();
            clearform();

        }

        private void clearform()
        {
            this.Pro = null;
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
            this._Seller.TextBox.SelectedItem = null;
            this._Seller.TextBox.SearchText = "";
            this._Name.Text = "";
            this._Phone.Text="";
            this._IMEI.IsReadOnly = false;
            this._IMEI.Text = "";
            this._Model.Text = "";
            this._ModelClass.Text = "";
            this._ModelPrice.Value = 0;
            this._InvoiceNumber.Text = "";
            this._BatteryIMEI.Text = "";
            this._ChargerIMEI.Text = "";
            this._HandphoneIMEI.Text = "";
            this._Note.Text = "";
            this._SellID.Text = "";
            this._OldID.Text = "";

        }

        private void _Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            clearform();
        }

        private void _Del_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            if (this.dataGrid.SelectedItem != null)
            {
                this.Yanbaomodellist.Remove((YanbaoModel) this.dataGrid.SelectedItem);
            
            }
            this.dataGrid.Rebind();
        }

        private void _CancelALL_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.Yanbaomodellist.Clear();
            this.dataGrid.Rebind();
            clearform();
        }

        private void _Next_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            jump = true;
            SaveSellInfos();
        }

        private void SaveSellInfos()
        {
            List<Pro_SellListInfo_Temp> sellListInfos = new List<Pro_SellListInfo_Temp>();
            List<Pro_Sell_Yanbao_temp> YanbaoInfos = new List<Pro_Sell_Yanbao_temp>();
            foreach (var yanbaoModel in Yanbaomodellist)
            {
                Pro_SellListInfo_Temp selllist = new Pro_SellListInfo_Temp();
//                selllist.ProID = RolePros[0].ProID;
//                selllist.ProCount=1;
                selllist.OldID = yanbaoModel.SellID;
                Pro_Sell_Yanbao_temp yanbao = new Pro_Sell_Yanbao_temp();
                yanbao.BillID = yanbaoModel.OldID;
                yanbao.BateriNum = yanbaoModel.BatteryIMEI;
                yanbao.FatureNum = yanbaoModel.InvoiceNumber;
                yanbao.NgarkuesNum = yanbaoModel.ChargerIMEI;
                yanbao.KufjeNum = yanbaoModel.HandphoneIMEI;
                yanbao.MobileType = yanbaoModel.Class;
                yanbao.MobileName = yanbaoModel.Model;
                yanbao.MobilePrice = yanbaoModel.ModelPrice;
                //yanbao.MobileDate = ""; //BUG
                yanbao.MobileIMEI = yanbaoModel.IMEI;
                yanbao.UserName = yanbaoModel.Name;
                yanbao.UserPhoneNum = yanbaoModel.Phone;
                yanbao.Note = yanbaoModel.Note;
                yanbao.YanBaoName = yanbaoModel.YanbaoName;
                selllist.Pro_Sell_Yanbao_temp = yanbao;
                selllist.UserID = Store.LoginUserInfo.UserID;
                selllist.ProID = RolePros.First().ProID;
                selllist.HallID = this.Hall.HallID;
                selllist.IMEI = yanbao.BillID;
                selllist.InsertDate = DateTime.Now;
//                if (
//                    Yanbaoprices.Any(p => p.ProID == selllist.ProID && p.StepPrice > yanbaoModel.ModelPrice))
//                {
//                    YanbaoModel model = yanbaoModel;
//                    selllist.ProPrice =
//                        Convert.ToDecimal(Yanbaoprices.Where(p => p.ProID == selllist.ProID && p.StepPrice > model.ModelPrice)
//                                                      .OrderByDescending(p => p.StepPrice).First().ProPrice);
//                }
//                else
//                {
//                    selllist.ProPrice =
//                        Convert.ToDecimal(Yanbaoprices.Where(p => p.ProID == selllist.ProID)
//                                                      .OrderByDescending(p => p.StepPrice).First().ProPrice);
//                }
                selllist.ProPrice = yanbaoModel.YanbaoPrice;

                sellListInfos.Add(selllist);
                YanbaoInfos.Add(yanbao);
            }

            PublicRequestHelp helper = new PublicRequestHelp(this.busy, Save_MethodID, new object[] {sellListInfos},
                                                             Save_Completed);
        }

        private void Save_Completed(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
//                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
//                    this.Yanbaomodellist.Clear();
//                    this.dataGrid.Rebind();
//                    this.clearform();
                    List<API.Pro_SellListInfo_Temp> models = (List<Pro_SellListInfo_Temp>)e.Result.Obj;
                    if (jump)
                    {
                        var newpage = new NewProSell(models);
                        newpage.oldpage = new AddGX();
                        this.NavigationService.Navigate(newpage);
                    }
                    else
                    {
                        this.NavigationService.Navigate(new AddGX());
                    }
//                    this.Content = newpage;
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

            //throw new NotImplementedException();
        }

        private void _Save_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            jump = false;
            SaveSellInfos();
        }
    }
}
