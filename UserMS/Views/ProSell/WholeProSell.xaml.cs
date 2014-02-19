using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell
{
    public partial class WholeMoreProSell
    {
        private int MethodID_GetInListID = 10;
        private int MethodID_SellNext = 76;
        private int MethodID_Aduit = 74;
        public API.VIP_VIPInfo SellVIP;
        //public API.Pro_SellInfo SellInfo = new Pro_SellInfo();
        public string SellerID;
        //public List<ProSellGridModel> SellGridModels = new List<ProSellGridModel>();
        public List<VIP_OffTicket> VIPTicket;
        public API.Pro_HallInfo Hall;
        public List<WholeSellModel> SellGridModels = new List<WholeSellModel>();
//        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        private decimal cashprice;
        public string AduitID;
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public WholeMoreProSell()
        {
            InitializeComponent();
            DataImporter.ImportType = typeof (API.Pro_SellInfo);
            this.Grid.ItemsSource = SellGridModels;


            if (CommonHelper.GetHalls(104).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(104)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"权限错误");
                this.IsEnabled = false;
            }

            this.HallName.DataContext = Hall;

            this.AduitIDTextBox.SearchEvent = AduitSearchEvent;
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

        private void AduitSearchEvent(object sender, RoutedEventArgs e)
        {
            
            AduitID = this.AduitIDTextBox.TextBox.SearchText.Trim();
            if (string.IsNullOrEmpty(AduitID))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入审批单号");
                return;
            }
            var a = new PublicRequestHelp(this.AduitBusy, MethodID_Aduit,
                                          new object[] { AduitID },
                                          GetAduit_End);

        }

        private void GetAduit_End(object sender, MainCompletedEventArgs e)
        {
            this.AduitBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    
                    List<API.GetSAModelResult> results = (List<GetSAModelResult>) e.Result.Obj;
                    SellGridModels.Clear();
                    if (results[0].HallID != this.Hall.HallID)
                    {
                        AduitID = "";
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 审批单所属仓库错误");
                        return;
                    }
                    this.CustName.Text = results[0].CustName;
                    this.CustPhone.Text = results[0].CustPhone;
                    this.Note.Text = results[0].Note;

                    foreach (var aduitListInfo in results)
                    {
                        WholeSellModel model = new WholeSellModel();
                        model.ProID = aduitListInfo.Proid;
//                        model.ProName = aduitListInfo.Proname;
//                        model.ProPrice = Convert.ToDecimal(aduitListInfo.Offmoney);
//                        model.OffPrice = Convert.ToDecimal(aduitListInfo.ProPrice) -Convert.ToDecimal(aduitListInfo.Offmoney);
                        model.ProPrice = Convert.ToDecimal(aduitListInfo.ProPrice) + Convert.ToDecimal(aduitListInfo.Offmoney);
                        model.OffPrice = Convert.ToDecimal(aduitListInfo.Offmoney);
                       
                        model.AduitCount = Convert.ToDecimal(aduitListInfo.Procount);
                        model.NeedIMEI = Store.ProInfo.First(p => p.ProID == model.ProID).NeedIMEI;
                        SellGridModels.Add(model);


                    }
                    
                    this.Grid.Rebind();
                    this.Grid2.Rebind();
                    this.calcprice();
                    this.AduitIDTextBox.IsEnabled = false;

                }
                else
                {
                    AduitID = "";
                    
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message);
                }
            }
            else
            {
                AduitID = "";
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
            }
        }


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
            SingleSelecter m = new SingleSelecter(parent, Store.ProHallInfo, "AreaID", "HallName", new string[] { "HallID", "HallName" }, new string[] { "仓库编号", "仓库名称" },true);
            
             m.Closed += Hall_select_closed;
            m.ShowDialog();

        }

        private void Hall_select_closed(object sender, WindowClosedEventArgs e)
        {
            SingleSelecter m = (SingleSelecter) sender;

            if (e.DialogResult == true && m.SelectedItem != null)
            {
                this.Hall = (Pro_HallInfo) m.SelectedItem;
                this.HallName.DataContext = this.Hall;
                

            }
        }

        private void Next_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            //            if (AduitIDTextBox.Text.Trim() == "")
            //            {
            //                MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批单不能为空!");
            //                return;
            //            }
                        var proHallInfo = this.Hall;
                        if (proHallInfo == null)
                        {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择仓库");
                            return;
                        }
            //

            if (String.IsNullOrEmpty(this.AduitID))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择审批单");
                return;
            }

//            if (string.IsNullOrEmpty(this.SellerID))
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择销售员");
//                return;
//            }
            System.Text.RegularExpressions.Regex ex = new Regex(@"^.{7}$");
            if (!ex.IsMatch(SellOldID.Text))
            {
                MessageBox.Show(Application.Current.MainWindow, "请输入正确的原始单号");
                return;
            }

            calcprice();
            
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
//                New_Click(null, null);
//                return;
////            }
            API.Pro_SellInfo SellInfo=new Pro_SellInfo();
            if (string.IsNullOrEmpty(SellInfo.Seller))
            {
                try
                {
                    SellInfo.Seller = Store.UserInfos.Where(p => p.RealName == Seller.Text).First().UserID;
                }
                catch (Exception)
                {

                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售员不存在");
                    return;
                }

            }
            SellInfo.SysDate = DateTime.Now;
            SellInfo.UserID = Store.LoginUserInfo.UserID;
            SellInfo.HallID = this.Hall.HallID;
            SellInfo.AuditID = this.AduitID;

            SellInfo.CashTotle = this.cashprice;
            SellInfo.CashPay = Convert.ToDecimal(this.CashPrice.Value);
            SellInfo.CardPay = Convert.ToDecimal(this.CardPrice.Value);
            SellInfo.SellDate = DateTime.Now;
            SellInfo.OldID = SellOldID.Text;
            SellInfo.CusName = this.CustName.Text;
            SellInfo.CusPhone = this.CustPhone.Text;
            List<API.Pro_SellListInfo> sellList = new List<Pro_SellListInfo>();
            foreach (var wholeSellModel in SellGridModels)
            {

                if (!wholeSellModel.NeedIMEI)
                {
                    Pro_SellListInfo selllistinfo = new Pro_SellListInfo();
                    selllistinfo.ProID = wholeSellModel.ProID;
                    selllistinfo.SellType = 3;
                    selllistinfo.ProPrice = wholeSellModel.ProPrice;
                    selllistinfo.WholeSaleOffPrice = wholeSellModel.OffPrice;
                    selllistinfo.CashPrice = wholeSellModel.ProPrice - wholeSellModel.OffPrice;
                    selllistinfo.ProCount = wholeSellModel.ProCount;
                    sellList.Add(selllistinfo);

                }
                else
                {
                    foreach (var IMEI in wholeSellModel.IMEIList)
                    {
                        Pro_SellListInfo selllistinfo = new Pro_SellListInfo();
                        selllistinfo.ProID = wholeSellModel.ProID;
                        selllistinfo.SellType = 3;
                        selllistinfo.ProPrice = wholeSellModel.ProPrice;
                        selllistinfo.WholeSaleOffPrice = wholeSellModel.OffPrice;
                        selllistinfo.CashPrice = wholeSellModel.AfterOffPrice;
                        selllistinfo.ProCount = 1;
                        selllistinfo.IMEI = IMEI;
                        sellList.Add(selllistinfo);

                    }
                }
            }
            sellList = sellList.Where(p => p.ProCount > 0).ToList();
            if (!sellList.Any())
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无可销售数量");
                return;
            }
            SellInfo.Pro_SellListInfo = sellList;
            var a = new PublicRequestHelp(this.PageBusy, MethodID_SellNext, new object[] { SellInfo },
                                                      Save_End
                                                      );


        }

        private void Save_End(object sender, MainCompletedEventArgs e)
        {
            this.PageBusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                    this.NavigationService.Navigate(new WholeMoreProSell());
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
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败: 服务器错误\n" + e.Error.Message);
            }
        }


      


        private void New_Click(object sender, RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(new WholeMoreProSell());

//            this.Content = new WholeMoreProSell();
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
            this.cashprice = SellGridModels.Sum(p => p.ProMoney);
            this.SellPrice.Value = this.cashprice;
            CardPrice_ValueChanged(null, null);
        }

        private void IMEIInput_Click(object sender, RadRoutedEventArgs e)
        {
            IMEIImport w = new IMEIImport();
            //w.OnSelectedPro += w_OnSelectedPro;
            w.Closed += w_Closed;
            w.ShowDialog();

        }

        private void w_Closed(object sender, WindowClosedEventArgs e)
        {
            IMEIImport w = sender as IMEIImport;
            if (w != null)
            {
                if (w.DialogResult == true)
                {


                    string imei = w.IMEI.Text;

                    List<string> IMEIS = new List<string>();
                    foreach (
                        var imei1 in imei.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList()
                        )
                    {
                        if (imei1.Trim() != "")
                        {
                            IMEIS.Add(imei1);
                        }

                    }
                    PublicRequestHelp helper = new PublicRequestHelp(this.PageBusy, this.MethodID_GetInListID,
                                                                     new object[] {IMEIS, Hall.HallID},
                                                                     GetInListID_Completed);

                }
            }
        }

            private void GetInListID_Completed(object sender, MainCompletedEventArgs e)
            {
                this.PageBusy.IsBusy = false;
                    
                if (e.Error == null)
                {
                    if (e.Result.ReturnValue)
                    {
                        bool hasotherpro = false;
                        bool muchpro = false;
                        List<API.SetSelection> selectinlist =(List<SetSelection>) e.Result.Obj;
                        foreach (var setSelection in selectinlist)
                        {
//                           
                            SetSelection selection = setSelection;
                            var query = SellGridModels.Where(p => p.ProID == selection.Proid);
                            if (!query.Any())
                            {
                               // MessageBox.Show(System.Windows.Application.Current.MainWindow,Application.Current.MainWindow,"商品: "+Store.ProInfo.First(p=>p.ProID== setSelection.Proid).ProName+" 不是可批发商品, 忽略");
                                hasotherpro = true;
                                break;
                            }
                            var pro = query.First();
                            if (pro.IMEIList == null)
                            {
                                pro.IMEIList=new List<string>();
                            }
                            foreach (var IMEI in setSelection.ReturnIMEI )
                            {
                                if (pro.ProCount == pro.AduitCount)
                                {
                                    //MessageBox.Show(System.Windows.Application.Current.MainWindow,Application.Current.MainWindow, "商品: " + pro.ProName + " 串码数量已超出最大批发数量, 将忽略多余串码!");
                                    muchpro = true;
                                    break;
                                }
                                pro.IMEIList.Add(IMEI);
                                
                                
                            }


                        }
                        this.Grid.Rebind();
                        this.Grid2.Rebind();

                        calcprice();
                        if (hasotherpro || muchpro)
                        {
                            string msg = (hasotherpro
                                          ? "串码中存在不属于本次可批发的商品 已被忽略\n"
                                          : "") + (muchpro ? "部分商品超出最大可批发数量, 多余部分串码被忽略" : "");
                            MessageBox.Show(Application.Current.MainWindow, msg);
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

        private void DelIMEI_OnClick(object sender, RoutedEventArgs e)
        {
          RadButton btn=sender as RadButton;
            if (btn != null)
            {
                RadListBox box = btn.GetVisualParent<RadListBox>();
                ((List<string>) box.ItemsSource).Remove((string) btn.DataContext);
            }

            this.Grid.Rebind();
            this.Grid2.Rebind();
            calcprice();

        }


        private void Grid_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            this.Grid2.ItemsSource = ((WholeSellModel) this.Grid.SelectedItem).IMEIList;
        }

        private void Del_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            var wmodel = ((WholeSellModel) this.Grid.SelectedItem);
            foreach (var selectedItem in this.Grid2.SelectedItems)
            {
                wmodel.IMEIList.Remove((string) selectedItem);
            }
            this.Grid.Rebind();
            this.Grid2.Rebind();
            
        }

        private void MyDataImport_OnOnImported(object sender, DataImportArgs e)
        {
            List<API.Pro_SellInfo> IMEIList = UserMS.Common.DataExtensions.ToList<API.Pro_SellInfo>(e.Datas,false);

        }
    }
}

