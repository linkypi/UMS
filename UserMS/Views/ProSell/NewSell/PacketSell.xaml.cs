using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;
using Label = System.Windows.Controls.Label;

namespace UserMS
{
	public partial class PacketSell
    {
        private VIP_VIPInfo SellVIP;
	    private int GetPackageModels_Method_ID = 258;
	    private int GetOffList_Method_ID = 149;
        private int Yanbao_CheckIMEI_MethodID = 109;
        private int MethodID_IMEIGetInfo = 5;
        private int VIP_GetService = 40;
        private int Yanbao_GetCurrentPrice_MethodID = 146;
	    private int GetTicketUsed_MethodID = 271;
        private int Save_MethodID = 51;
	    private int GetAllRules_MethodID = 285;
        private List<API.View_YanBoPriceStepInfo> Yanbaoprices;
	    private API.UserMsServiceClient wsclient = Store.wsclient;
        private List<Package_SalesNameInfo> Packages = new List<Package_SalesNameInfo>();
        private List<Model.PackSellGridModel> SellGridModels = new List<PackSellGridModel>();
	    private List<VIP_OffList> OffLists = new List<VIP_OffList>();
        private List<API.View_PackageGroupTypeInfo> groupTypeInfos = new List<View_PackageGroupTypeInfo>();
	    private API.Pro_HallInfo HallInfo;
	    private API.VIP_OffList selectedOff;
	    private List<API.Pro_SellListInfo> SellListinfos=new List<Pro_SellListInfo>();
	    private bool isOK=false;
        public List<API.Rules_AllCurrentRulesInfo> AllRulesInfos = new List<Rules_AllCurrentRulesInfo>();
        private List<UserOpModel> UserOpList = new List<UserOpModel>();
		public PacketSell()
		{
			this.InitializeComponent();
//		    this.Gridvews.ItemsSource = new List<Model.ProSellGridModel>()
//		    {
//                 new PackSellGridModel()
//                 {
//                     ProID="1",ProCount = 1,ProPrice = (decimal) 1234.56,IMEI="A1234567890123",
//                     GridTemplate = "JiPeiKaDataTemplate",TicketNum = "123"
//                 },
//                 new PackSellGridModel()
//                 {
//                     ProID="1",ProCount = 1,ProPrice = (decimal) 1234.56,IMEI="A1234567890123",
//                     GridTemplate = "VIPTemplate", GridTemplateData = new VIPSellGridModel()
//                     {
//                         QQ="123"
//                     }
//                 },
//                 new PackSellGridModel(){ProID="1",ProCount = 1,ProPrice = (decimal) 1234.56,IMEI="A1234567890123"},
//                 new PackSellGridModel(){ProID="1",ProCount = 1,ProPrice = (decimal) 1234.56,IMEI="A1234567890123"},
//
//		    };
//            this.listbox.ItemsSource=

		    // 在此点之下插入创建对象所需的代码。
#if HZ
            VIPBTN.Visibility=Visibility.Collapsed;
            VIPPOINT.Visibility=Visibility.Collapsed;
#endif
		}



	    private void Prev_OnClick(object sender, RadRoutedEventArgs e)
	    {
	        if (Common.CommonHelper.ButtonNotic(sender))
	        {
	            return;
	        }
            this.NavigationService.Navigate(new PacketSell());
	    }

	    private void Save_OnClick(object sender, RadRoutedEventArgs e)
	    {
            calcprices();
	        if (!isOK)
	        {
	            MessageBox.Show(Application.Current.MainWindow, "本单有必须的商品未填, 无法提交");
	            return;
	        }
	        if (Common.CommonHelper.ButtonNotic(sender))
	        {
	            return;
	        }
            if (SellListinfos.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "没有任何商品");
                return;
            }

            if (string.IsNullOrEmpty(VIPName.Text) || string.IsNullOrEmpty(VIPPhone.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入客户资料");
                return;
            }
            if (string.IsNullOrEmpty(SellOldID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入原始单号");
                return;
            }

            System.Text.RegularExpressions.Regex ex = new Regex(@"^.{7}$");
            if (!ex.IsMatch(SellOldID.Text))
            {

                MessageBox.Show(Application.Current.MainWindow, "请输入正确的原始单号");
                return;
            }
            Pro_SellInfo sellinfo = new Pro_SellInfo();
            try
            {
                sellinfo.Seller = Store.UserInfos.First(p => p.RealName == Seller.Text).UserID;
            }
            catch (Exception)
            {

                MessageBox.Show(System.Windows.Application.Current.MainWindow, "销售员不存在");
                return;
            }

	        foreach (var proSellListInfo in SellListinfos)
	        {
	            if (proSellListInfo.VIP_VIPInfo != null)
	            {
	                if (string.IsNullOrEmpty(proSellListInfo.VIP_VIPInfo.MemberName))
	                {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员注册姓名不能为空");
                        return;
	                }
                    if (string.IsNullOrEmpty(proSellListInfo.VIP_VIPInfo.MobiPhone))
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员注册请填写正确的手机号码");
                        return;
                    }
                    if (!PormptPage.isNumeric(proSellListInfo.VIP_VIPInfo.MobiPhone))
	                {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员注册请填写正确的手机号码！");
                        return;
	                }
                    if (proSellListInfo.VIP_VIPInfo.StartTime == null )
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员注册请选择加入日期");
                        return;
                    }
	                proSellListInfo.VIP_VIPInfo.Seller = sellinfo.Seller;
	            }
	            if (proSellListInfo.Pro_Sell_Yanbao != null)
	            {
                    if (string.IsNullOrEmpty(proSellListInfo.Pro_Sell_Yanbao.FatureNum))
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "延保发票号码不能为空");
                        return;
                    }
                    if (string.IsNullOrEmpty(proSellListInfo.Pro_Sell_Yanbao.BateriNum))
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "延保电池编码不能为空");
                        return;
                    }
                    if (string.IsNullOrEmpty(proSellListInfo.Pro_Sell_Yanbao.NgarkuesNum))
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "延保充电器编码不能为空");
                        return;
                    }
                    if (string.IsNullOrEmpty(proSellListInfo.Pro_Sell_Yanbao.KufjeNum))
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "延保耳机编码不能为空");
                        return;
                    }

                    
                    
                    if (proSellListInfo.Pro_Sell_Yanbao.MobilePrice == 0)
	                {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "有延保未录入, 无法销售.");
                        return;
	                }

	            }
	            if (proSellListInfo.Pro_Sell_JiPeiKa != null)
	            {
	                if (
	                    !SellListinfos.Select(p => p.IMEI)
	                        .Where(p => !string.IsNullOrEmpty(p))
	                        .Contains(proSellListInfo.Pro_Sell_JiPeiKa.IMEI))
	                {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "机配卡所配的终端串码必须要同时销售.");
                        return;
	                }
	            }
	            if (proSellListInfo.Pro_BillInfo != null)
	            {
	                if (string.IsNullOrEmpty(proSellListInfo.Pro_BillInfo.MobileProID))
	                {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "合同未填终端串码.");
                        return;
	                }
	            }
	        }

            if (this.SellVIP != null)
	        {
                sellinfo.VIP_ID = SellVIP.ID;
                
	        }


            sellinfo.SysDate = DateTime.Now;
            sellinfo.CusName = this.VIPName.Text.Trim();
            sellinfo.CusPhone = this.VIPPhone.Text.Trim();
            sellinfo.HallID = this.HallInfo.HallID;
            sellinfo.SellDate = DateTime.Now;
            sellinfo.OldID = this.SellOldID.Text;
            sellinfo.CashPay = Convert.ToDecimal(CashPrice.Value);
            sellinfo.CardPay = Convert.ToDecimal(CardPrice.Value);
	        sellinfo.CashTotle = Convert.ToDecimal(this.CashPrice.Value);
            sellinfo.UserID = Store.LoginUserInfo.UserID;
            sellinfo.Note = this.Note.Text;
	        sellinfo.Pro_SellSpecalOffList=new List<Pro_SellSpecalOffList>();
            API.Pro_SellSpecalOffList offlist = new Pro_SellSpecalOffList();

            offlist.SpecalOffID = selectedOff.ID;
            offlist.ID = 1;
            offlist.OffMoney = 0;
	        for (int i = 0; i < SellListinfos.Count; i++)
	        {
                
	            SellListinfos[i].SpecialID = 1;
                offlist.OffMoney = offlist.OffMoney+SellListinfos[i].CashPrice;
	            
	        }
            sellinfo.Pro_SellSpecalOffList.Add(offlist);
	        sellinfo.Pro_SellListInfo = SellListinfos;
	        new PublicRequestHelp(this.busy, Save_MethodID, new object[] {sellinfo, new List<int>()}, SaveEvent);

	    }

	    private void SaveEvent(object sender, MainCompletedEventArgs e)
	    {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    List<API.Print_SellListInfo> list = (List<Print_SellListInfo>)e.Result.Obj;
                    if (list == null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存成功, 但需要审核");
                  
                            this.NavigationService.Navigate(new PacketSell());
                     
                    }
                    else
                    {
                        //MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                        MessageBoxResult rsltMessageBox = MessageBox.Show(
                            System.Windows.Application.Current.MainWindow, "保存成功。是否打印", "提示", MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);
                        if (rsltMessageBox == MessageBoxResult.Yes)
                        {

                            List<ReportService.Print_SellListInfo> newlist =
                                new List<ReportService.Print_SellListInfo>();
                            foreach (var l in list)
                            {
                                newlist.Add(new ReportService.Print_SellListInfo()
                                {
                                    串码_号码_合同号 = l.串码_号码_合同号,
                                    Print_SellInfo = null,
                                    优惠 = l.优惠,
                                    优惠券名称 = l.优惠券名称,
                                    优惠券金额 = l.优惠券金额,
                                    会员卡号 = l.会员卡号,
                                    刷卡 = l.刷卡,
                                    券号_合约号 = l.券号_合约号,
                                    券面值 = l.券面值,
                                    单价 = l.单价,
                                    原始单号 = l.原始单号,
                                    商品名称 = l.商品名称,
                                    备注 = l.备注,
                                    实收总额 = l.实收总额,
                                    实收总额大写 = l.实收总额大写,
                                    客户电姓名 = l.客户电姓名,
                                    客户电话 = l.客户电话,
                                    应收总额 = l.应收总额,
                                    数量 = l.数量,
                                    现金 = l.现金,
                                    系统单号 = l.系统单号,
                                    系统自增外键编号 = l.系统自增外键编号,
                                    自增主键编号 = l.自增主键编号,
                                    //补差金额 = l.补差金额,
                                    金额小计 = l.金额小计,
                                    销售公司 = l.销售公司,
                                    销售单号 = l.销售单号,
                                    销售员 = l.销售员,
                                    销售日期 = l.销售日期,
                                    销售门店 = l.销售门店
                                });
                            }
                            var newpage = new UserMS.Report.Print.SellPrint.PrintSellBillOne(newlist);
                            
                            this.NavigationService.Navigate(newpage);

                        }
                        else
                        {
                            
                                this.NavigationService.Navigate(new PacketSell());
                           
                        }
                    }
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


                VIPName.IsEnabled = false;
                VIPPhone.IsEnabled = false;


            }
            else
            {
                SellVIP = null;
                VIPCard.DataContext = SellVIP;
                VIPName.DataContext = SellVIP;
                VIPPhone.DataContext = SellVIP;
                VIPPoint.DataContext = SellVIP;

                VIPName.IsEnabled = true;
                VIPPhone.IsEnabled = true;


            }
        }

	    private void PacketSell_OnLoaded(object sender, RoutedEventArgs e)
	    {
            
	        this.Loaded -= PacketSell_OnLoaded;
            if (CommonHelper.GetHalls(99).Count >= 1)
            {
                HallInfo = CommonHelper.GetHalls(99)[0];
                this.HallName.Content = HallInfo.HallName;
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "权限错误");
                this.IsEnabled = false;
                return;
            }
	       wsclient = Store.wsclient;
	        var result0 = wsclient.Main(264, new List<object>());
	        if (result0.ReturnValue)
	        {
                groupTypeInfos = (List<View_PackageGroupTypeInfo>)result0.Obj;
	        }


	        var result = wsclient.Main(GetPackageModels_Method_ID, new List<object>());

	        if (result.ReturnValue)
	        {
               
	            Packages= (List<API.Package_SalesNameInfo>) result.Obj;
                var ParentInfo = Packages.Where(p => p.Parent == 0);
	            foreach (var Item in ParentInfo)
	            {
                    LeftTree.Items.Add(GetChildItem(Item.ID, Packages));

	            }
	            //this.LeftTree.ItemsSource = (List < View_PackageSalesNameInfo >) result.Obj;
	        }
	        else
	        {
	            
	        }

            var result2 = wsclient.Main(Yanbao_GetCurrentPrice_MethodID, new List<object>());
            if (result2.ReturnValue)
            { Yanbaoprices = (List<View_YanBoPriceStepInfo>)result2.Obj; }
            else
            {
                Yanbaoprices = new List<View_YanBoPriceStepInfo>();
            }
#if !HZ
            VIP_OnMouseLeftButtonUp(null, null);
#endif
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
                                                  new string[] { "用户名", "职位" });

            w.Closed += SellerSearchWindowClose;
            w.ShowDialog();
        }
        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
           

        }
        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
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

	    private RadTreeViewItem GetChildItem(int salesNameID, List<Package_SalesNameInfo> SalesNameInfo)
	    {
	        var Children = from b in SalesNameInfo
	            where b.Parent == salesNameID
	            select b;
	        var pnode = from b in SalesNameInfo
	            where b.ID == salesNameID
	            select b;
	        RadTreeViewItem parent = new RadTreeViewItem() {Header = pnode.First().SalesName};
            foreach (var vipOffList in pnode.First().VIP_OffList.Where(p => p.Flag == true && p.StartDate < DateTime.Now && p.EndDate > DateTime.Now))
            {
                parent.Items.Add(new RadTreeViewItem()
                {
                    Header = vipOffList.Name,
                    Tag = vipOffList
                });
            }
	        if (pnode.First().Parent == 0) parent.IsExpanded = true;
	        if (Children.Count() != 0)
	        {
	            foreach (var Item in Children)
	            {
                    RadTreeViewItem child = GetChildItem(Item.ID, SalesNameInfo);
	                parent.Items.Add(child);
//	                foreach (var vipOffList in Item.VIP_OffList.Where(p=>p.Flag==true&&p.StartDate<DateTime.Now&&p.EndDate>DateTime.Now))
//	                {
//                        child.Items.Add(new RadTreeViewItem()
//	                    {
//	                        Header = vipOffList.Name,
//	                        Tag = vipOffList
//	                    });
//	                }
	            }
	        }
            
            
	        return parent;
	    }

	    private void Gridvews_OnRowLoaded(object sender, RowLoadedEventArgs e)
	    {
	        var obj = (PackSellGridModel) e.DataElement;
            GridViewRow row = e.Row as GridViewRow;
            if (obj == null || row == null)
            {
                return;
            }
	        try
	        {
	            row.DetailsTemplate = this.Gridvews.Resources[obj.GridTemplate] as DataTemplate;
	        }
	        catch
	        {
                row.DetailsTemplate = this.Gridvews.Resources["Default"] as DataTemplate;
	        }
	        if (row.DetailsTemplate == null)
	        {
                row.DetailsTemplate = this.Gridvews.Resources["Default"] as DataTemplate;
	        }

	    }

	    private void Gridvews_OnLoadingRowDetails(object sender, GridViewRowDetailsEventArgs e)
	    {
	       // throw new NotImplementedException();
	    }

	    private void LeftTree_OnItemClick(object sender, RadRoutedEventArgs e)
	    {
	       RadTreeViewItem s= e.Source as RadTreeViewItem;
	        if (s == null) return;
            if (s.Tag == null) return;

	        var off = s.Tag as API.VIP_OffList;
	        if (off == null) return;
	        if (SellGridModels.Any(p => !string.IsNullOrEmpty(p.ProID)))
	        {
	            MessageBoxResult rsltMessageBox = MessageBox.Show(System.Windows.Application.Current.MainWindow,
	                "是否切换到" + off.Name, "提示", MessageBoxButton.YesNo,
	                MessageBoxImage.Warning);
	            if (rsltMessageBox == MessageBoxResult.No)
	            {
	                
                    return;
	            }
	        }
	        this.CurrentPack.Text = off.Name;
            selectedOff = off;
	        ProNameTextBox.Text = selectedOff.Note;
            SellGridModels = new List<PackSellGridModel>();
	        foreach (var packageGroupInfo in off.Package_GroupInfo)
	        {
	            if (groupTypeInfos.Any(p => p.ID == packageGroupInfo.GroupID))
	            {
	                var grouptype = groupTypeInfos.First(p => p.ID == packageGroupInfo.GroupID);
	                var gridmodel = new Model.PackSellGridModel();
	                gridmodel.GroupInfo = packageGroupInfo;
	                gridmodel.GridTemplate = grouptype.ClassName;
                    gridmodel.ProInfos = new Dictionary<int, List<Pro_ProInfo>>();
                    gridmodel.PropertyChanged += gridmodel_PropertyChanged;
                    //gridmodel.ProInfos=packageGroupInfo.Package_ProInfo
                    gridmodel.Rules=new List<Pro_SellList_RulesInfo>();
	                foreach (var packageProInfo in packageGroupInfo.Package_ProInfo)
	                {
	                    if (Store.ProNameInfo.Any(p => p.ID == packageProInfo.ProMainNameID))
	                    {
	                        gridmodel.ProInfos.Add(packageProInfo.ID,
	                            Store.ProInfo.Join(
	                                Store.ProMainInfo.Where(q => q.ProNameID == packageProInfo.ProMainNameID),
	                                p => p.ProMainID, o => o.ProMainID, (info, mainInfo) => info).ToList());

	                    }
	                  
	                    
	                }


	                switch (grouptype.ClassName)
	                {
	                    case "VIPTemplate":
	                        gridmodel.GridTemplateData = new Model.VIPSellGridModel();
	                        break;
                        case "YanBaoDataTemplate":
	                        gridmodel.GridTemplateData = new Model.YanbaoModel();
	                        break;

                        case "BillDataTemplate":
                             gridmodel.GridTemplateData = new API.Pro_BillInfo();
	                        break;
	                }

                SellGridModels.Add(gridmodel);
	                
	            }
                
	        }
            this.Gridvews.ItemsSource = SellGridModels;
	        //calcprices();
	        this.SellPrice.Value = off.ArriveMoney;

            this.CashPrice.Value = this.SellPrice.Value;
//	        if (Packages.Any(p => p.ID == s.Tag as int?))
//	        {
//	            //var package = Packages.First(p => p.ID == s.Tag as int?);
//	            
//	        }

	    }

        void gridmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PackSellGridModel model=sender as PackSellGridModel;
            if ((e.PropertyName == "TicketPrice"||e.PropertyName=="TicketNum" )&& model!=null && model.TicketPrice>0)

            {

                if (string.IsNullOrEmpty(model.TicketNum))
                {
                    MessageBox.Show(Application.Current.MainWindow, "错误: 未填兑券码");
                    model.TicketPrice = 0;
                    model.TicketUsed = 0;
                    calcprices();
                    return;
                }
                if (!Store.ProInfo.Any(p => p.ProID == model.ProID))
                {
                    MessageBox.Show(Application.Current.MainWindow, "错误: 商品未选择");
                    model.TicketPrice = 0;
                    model.TicketUsed = 0;
                    calcprices();
                    return;
                }
                try
                {
                    
                    model.TicketUsed = CommonHelper.CheckProCashTicket(
                    new API.Pro_SellListInfo()
                    {
                        ProID = model.ProID,
                        ProPrice = model.ProPrice,
                        CashTicket = model.TicketPrice,
                        TicketID = model.TicketNum,
                        SellType = model.GroupInfo.SellType,
                    },
                    Store.ProInfo.First(p => p.ProID == model.ProID)

                );
                    //calcprices();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Application.Current.MainWindow, "错误: " + ex.Message);
                    model.TicketPrice = 0;
                    model.TicketUsed = 0;
                    //calcprices();
                }
                
               
            }
            if (model != null && model.TicketPrice == 0) model.TicketUsed = 0;
            if ((e.PropertyName == "TicketPrice"||e.PropertyName=="TicketNum" ))calcprices();
        }

	    private void _CheckIMEI_OnClick(object sender, RoutedEventArgs e)
	    {
//            this._IMEI.IsReadOnly = true;
//            b.parent
            Button b=sender as Button;
	        var stackpanel = b.ParentOfType<StackPanel>();
            var _IMEI = stackpanel.FindChildByType<TextBox>();
	        var row = b.ParentOfType<RadRowItem>();
	        var sellmodel = row.DataContext as PackSellGridModel;
	        _IMEI.IsReadOnly = true;
	        if (b == null) return;
	        YanbaoModel model=b.DataContext as YanbaoModel;
	        if (model == null)
	        {
	            return;
	        }
	        var imei = model.IMEI.ToUpper();
	        if (!SellGridModels.Select(p => p.IMEI).Contains(imei))
	        {
	            MessageBox.Show(Application.Current.MainWindow, "套餐内的延保只允许加入在本套餐销售的终端");
                _IMEI.IsReadOnly = false;
	            return;
	        }
	        new Action(() =>
	        {
	            try
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        this.busy.IsBusy = true;
                    }));
                    
	                var result = wsclient.Main(Yanbao_CheckIMEI_MethodID, new List<object>() { imei });
	                if (result.ReturnValue)
	                {
	                    API.Pro_IMEI imeiinfo = result.ArrList[0] as Pro_IMEI;
	                    API.Pro_SellListInfo selllist = result.ArrList[1] as Pro_SellListInfo;
	                    if (imeiinfo != null)
	                    {
	                        var yanbaoModelProInfo = Store.ProInfo.First(p => p.ProID == imeiinfo.ProID);
	                        model.Model = yanbaoModelProInfo.ProName;


	                        model.Class = yanbaoModelProInfo.ProClassName;
	                        //                                Store.ProClassInfo.First(p => p.ClassID == Pro.Pro_ClassID).ClassName;
                            if (selllist != null)
                            {

                                model.ModelPrice = selllist.YanbaoModelPrice;


                            }
                            else
                            {
                                model.ModelPrice = yanbaoModelProInfo.Pro_SellTypeProduct.Any(p => p.SellType == 1)
                                    ? yanbaoModelProInfo.Pro_SellTypeProduct.First(p => p.SellType == 1).Price
                                    : 0;
                            }

                            if (yanbaoModelProInfo.YanBaoModelID != null && yanbaoModelProInfo.YanBaoModelID!=0)
	                        {

	                            if (Yanbaoprices.Any(p => p.ID == yanbaoModelProInfo.YanBaoModelID))
	                            {
	                                model.YanbaoPrice =
	                                    Convert.ToDecimal(
	                                        Yanbaoprices.First(p => p.ID == yanbaoModelProInfo.YanBaoModelID).ProPrice);
	                                model.YanbaoName =
	                                    Yanbaoprices.First(p => p.ID == yanbaoModelProInfo.YanBaoModelID).Name;
	                            }
	                        }
	                        else
	                        {
	                            var yanbaoproid = Store.Options.First(p => p.ClassName == "GXYanBao").Value;
	                            if (
	                                Yanbaoprices.Any(p => p.ProID == yanbaoproid && p.StepPrice >= model.ModelPrice))
	                            {

	                                model.YanbaoPrice =
	                                    Convert.ToDecimal(
	                                        Yanbaoprices.Where(
	                                            p => p.ProID == yanbaoproid && p.StepPrice >= model.ModelPrice)
	                                            .OrderBy(p => p.StepPrice).First().ProPrice);
	                                model.YanbaoName =
	                                    Yanbaoprices.Where(p => p.ProID == yanbaoproid && p.StepPrice >= model.ModelPrice)
	                                        .OrderBy(p => p.StepPrice).First().Name;
	                            }
	                        }
	                        sellmodel.ProPrice = model.YanbaoPrice;
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                _IMEI.IsReadOnly = true;
                                this.busy.IsBusy = false;
                            }));
	                    }
	                    else
	                    {
	                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
	                        {
	                            MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 串码不存在");
                                _IMEI.IsReadOnly = false;
                                this.busy.IsBusy = false;
	                        }));
	                        return;
	                    }

	                  

	                }
	                else
	                {
	                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
	                    {
	                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + result.Message);
                            _IMEI.IsReadOnly = false;
                            this.busy.IsBusy = false;
	                    }));

	                }
	            }
	            catch (Exception ex)
	            {
	                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
	                {
	                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + ex.Message);
                        _IMEI.IsReadOnly = false;
                        this.busy.IsBusy = false;
	                }));
	            }

	        }).BeginInvoke(null, null);
	    }

	    private void AddButton_OnClick(object sender, RoutedEventArgs e)
	    {
            var imei = this.IMEITextBox.Text.Trim().ToUpper();
            new Action(() =>
            {
                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    busy.IsBusy = true;
                }));
                try
                {
                    var result = wsclient.Main(this.MethodID_IMEIGetInfo, new List<object>() { imei, this.HallInfo.HallID });
                    if (result.ReturnValue)
                    {
                        API.Pro_ProInfo i = (Pro_ProInfo)result.Obj;
                        var t =
                            SellGridModels.Where(
                                p => string.IsNullOrEmpty(p.ProID) && p.ProInfos.Any(pair => pair.Value.Any(info => info.ProID == i.ProID))).ToList();
                        if (t.Any())
                        {
                            var model = t.First();
//                        }
//                        if (SellGridModels.Any(p => p.ProInfos.Select(o => o.ProID).Contains(i.ProID) && string.IsNullOrEmpty(p.ProID)))
//                        {
//                            var model = SellGridModels.First(p => p.ProInfos.Select(o => o.ProID).Contains(i.ProID));
                            var query = Store.ProInfo.Where(p => p.ProID == i.ProID).ToList();
                            if (query.Any()&&query.First().Pro_SellTypeProduct.Any(p => p.SellType == model.GroupInfo.SellType))
                            {
                                model.ProID = i.ProID;
                                model.ProPrice =
                                    i.Pro_SellTypeProduct.First(p => p.SellType == model.GroupInfo.SellType).Price;
                                model.ProCount = 1;
                                model.IMEI = imei;
                                model.package_proinfoID =
                                    model.ProInfos.First(pair => pair.Value.Any(p => p.ProID == i.ProID)).Key;
                                var rulesresult = wsclient.Main(this.GetAllRules_MethodID, new List<object>() {HallInfo.HallID,  new API.Pro_SellListInfo()
                                {
                                    ProID=i.ProID,SellType=model.GroupInfo.SellType
                                } });
                                if (rulesresult.ReturnValue)
                                {
                                    AllRulesInfos.AddRange((List<Rules_AllCurrentRulesInfo>) rulesresult.Obj);
                                    AllRulesInfos =
                                        AllRulesInfos.GroupBy(p => p.ID).Select(infos => infos.First()).ToList();
                                }
                                model.Rules=new List<Pro_SellList_RulesInfo>();
                                if (model.GridTemplate == "BillDataTemplate")
                                {
                                    Pro_BillInfo billmodel = model.GridTemplateData as Pro_BillInfo;
                                    billmodel.BillIMEI = imei;
                                    
                                }

                                if (model.GridTemplate == "YanBaoDataTemplate")
                                {
                                    YanbaoModel yanbaomodel=model.GridTemplateData as YanbaoModel;
                                    yanbaomodel.OldID = imei;


                                }

                                if (model.GridTemplate == "VIPTemplate")
                                {
                                    VIPSellGridModel vipmodel = model.GridTemplateData as VIPSellGridModel;
                                    vipmodel.IMEI = imei;
                                    //TODO: VIP get info;
                                    new Action(
                                        () =>
                                        {
                                            try
                                            {

                                                var resultv = wsclient.Main(VIP_GetService, new List<object>()
                                                {
                                                    imei
                                                });

                                                if (resultv.ReturnValue)
                                                {
                                                    if (resultv.ArrList[0] == null)
                                                    {
                                                        this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                            new Action(() =>
                                                            {
                                                                MessageBox.Show(
                                                                    System.Windows.Application.Current.MainWindow,
                                                                    "查询失败: 该会员卡无仓库信息");

                                                            }));
                                                    }
                                                    else
                                                    {
                                                        string hallid = resultv.ArrList[0].ToString();
                                                        if (hallid != this.HallInfo.HallID)
                                                        {
                                                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                                new Action(() =>
                                                                {
                                                                    MessageBox.Show(
                                                                        System.Windows.Application.Current.MainWindow,
                                                                        "查询失败: 该会员卡不在本仓库");
                                                                    this.busy.IsBusy = false;

                                                                }));
                                                        }
                                                        else
                                                        {
                                                            var viptype = resultv.Obj as API.VIP_VIPType;
                                                            vipmodel.VIPType = viptype;
                                                            vipmodel.ProID = resultv.ArrList[1].ToString();
                                                            vipmodel.IMEI = imei;
                                                            
                                                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                                new Action(() =>
                                                                {
                                                                    this.busy.IsBusy = false;

                                                                }));
                                                        }
                                                    }



                                                }
                                                else
                                                {
                                                    this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                        new Action(() =>
                                                        {
                                                            MessageBox.Show(
                                                                System.Windows.Application.Current.MainWindow,
                                                                "查询失败: " + result.Message);
                                                            this.busy.IsBusy = false;


                                                        }));
                                                }


                                            }
                                            catch (Exception ex)
                                            {
                                                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                    new Action(() =>
                                                    {
                                                        MessageBox.Show(System.Windows.Application.Current.MainWindow,
                                                            "查询失败: 服务器错误\n" + ex.Message);
                                                        this.busy.IsBusy = false;

                                                    }));
                                            }
                                        }
                                        ).BeginInvoke(null, null);

                                }
                            }
                            else
                            {
                                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                           Application.Current
                               .MainWindow,
                           "添加失败: 商品未定价, 无法添加到本套餐")));
                            }
                           

                           

                           

                        }
//                        if (IMEISellProInfos.Any(p => p.ProID == i.ProID))
//                        {
//                            if (IMEIGridModels.All(p => p.IMEI != imei))
//                                this.IMEIGridModels.Add(new ProSellGridModel()
//                                {
//                                    ProID = i.ProID,
//                                    ProCount = 1,
//                                    IMEI = imei,
//
//                                });
//                            //                            this.IMEISellGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
//                            //                                new Action(() => IMEISellGrid.Rebind()));
//                        }
                        else
                        {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                            Application.Current
                                .MainWindow,
                            "添加失败: 商品不在本套餐, 无法添加到本套餐")));
                        }
                    }
                    else
                    {
                        this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                            Application.Current
                                .MainWindow,
                            "添加失败:" + result.Message)));
                    }

                }
                catch (Exception ex)
                {
                    this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                             Application.Current
                                 .MainWindow,
                             "添加失败: 服务器错误\n" + ex.Message)));
                }

                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    busy.IsBusy = false;
                    this.IMEITextBox.Text = "";
                }));
                calcprices();
                
            }).BeginInvoke(null, null);
	    }

	    private void ProAddButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        List<Pro_ProInfo> proinfos=new List<Pro_ProInfo>();
	        foreach (var packSellGridModel in SellGridModels)
	        {
                foreach (var proProInfo in packSellGridModel.ProInfos.Select(p => p.Value.Where(o => o.NeedIMEI == false))  )
	            {
                    proinfos.AddRange(proProInfo);
	            }
                
	        }
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();

            Common.CommonHelper.ProFilterGen(proinfos, ref pros, ref t);

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

                List<SlModel.ProductionModel> i = (from object b in s.SelectedItems select (SlModel.ProductionModel)b).ToList();
                foreach (var proProInfo in i)
                {
                    var t =
                            SellGridModels.Where(
                                p =>string.IsNullOrEmpty(p.ProID)&& p.ProInfos.Any(pair => pair.Value.Any(info => info.ProID == proProInfo.ProID))).ToList();
                    if (t.Any())
                    {
                        var model = t.First();

                        var proinfo = Store.ProInfo.First(p => p.ProID == proProInfo.ProID);
                        if (proinfo.Pro_SellTypeProduct.Any(p => p.SellType == model.GroupInfo.SellType))
                        {

                            model.ProID = proProInfo.ProID;
                            model.ProCount = 1;
                            //model.IMEI = " ";
                            model.ProPrice =
                                proinfo.Pro_SellTypeProduct.First(p => p.SellType == model.GroupInfo.SellType).Price;
                            model.package_proinfoID = model.ProInfos.First(pair => pair.Value.Any(p => p.ProID == proProInfo.ProID)).Key;
                            var rulesresult = wsclient.Main(this.GetAllRules_MethodID, new List<object>() { HallInfo.HallID, new API.Pro_SellListInfo()
                                {
                                    ProID=proProInfo.ProID,SellType=model.GroupInfo.SellType
                                } });
                            if (rulesresult.ReturnValue)
                            {
                                AllRulesInfos.AddRange((List<Rules_AllCurrentRulesInfo>)rulesresult.Obj);
                                AllRulesInfos =
                                    AllRulesInfos.GroupBy(p => p.ID).Select(infos => infos.First()).ToList();
                            }
                            model.Rules=new List<Pro_SellList_RulesInfo>();
                        }
                        else
                        {
                            MessageBox.Show(
                                Application.Current
                                    .MainWindow,
                                "添加失败: 商品未定价, 无法添加到本套餐");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            Application.Current
                                .MainWindow,
                            "添加失败: 商品不在本套餐中, 无法添加到本套餐");
                        return;
                    }
                        
                }


                calcprices();
                //this.ProID.TextBox.SearchText = "";
                //this.Grid.Rebind();
            }
        }

	    void calcprices()
	    {
	        var act = new Action(() =>
	        {
	            if (selectedOff == null) return;
	            SellListinfos = new List<Pro_SellListInfo>();


	            decimal packetprice = selectedOff.ArriveMoney;
	            SellListinfos = new List<Pro_SellListInfo>();
	            //this.SellPrice.Value = packetprice;
	            bool is1st = true;
	            PackSellGridModel firstmodel = null;
	            decimal temp = 0;
	            Pro_SellListInfo firstselllist = null;
	            foreach (var packSellGridModel in SellGridModels)
	            {

	                if (string.IsNullOrEmpty(packSellGridModel.ProID) && packSellGridModel.GroupInfo.IsMust == true)
	                {
	                    isOK = false;
	                    return;
	                }
	                if (string.IsNullOrEmpty(packSellGridModel.ProID))
	                    continue;



	                if (!is1st)
	                {
	                    temp += packSellGridModel.ProPrice;
	                    //temp -= packSellGridModel.TicketUsed;
	                }
	                var query = Store.ProInfo.First(p => p.ProID == packSellGridModel.ProID)
	                    .Pro_SellTypeProduct.Where(o => o.SellType == 1);
	                decimal yanbaomodelprice = 0;
	                if (query.Any())
	                {
	                    yanbaomodelprice = query.First().Price;
	                }
	                var m = new Pro_SellListInfo()
	                    {
	                        ProID = packSellGridModel.ProID,
	                        ProCount = 1,
	                        ProPrice = packSellGridModel.ProPrice,
	                        IMEI = packSellGridModel.IMEI,
	                        SellType = packSellGridModel.GroupInfo.SellType,
	                        SellType_Pro_ID =
	                            Store.ProInfo.First(p => p.ProID == packSellGridModel.ProID)
	                                .Pro_SellTypeProduct.First(p => p.SellType == packSellGridModel.GroupInfo.SellType)
	                                .ID,
	                        ProOffListID = packSellGridModel.package_proinfoID,
                            YanbaoModelPrice = yanbaomodelprice
                            
	                    };
	                    if (packSellGridModel.GridTemplate == "JiPeiKaDataTemplate")
	                    {
	                        m.Pro_Sell_JiPeiKa=new Pro_Sell_JiPeiKa()
	                        {
	                            IMEI = packSellGridModel.TicketNum,
                                
                            
	                        };
                            
	                    }
	                    if (packSellGridModel.GridTemplate == "TicketDataTemplate")
	                    {
                            m.TicketID = packSellGridModel.TicketNum;
	                        m.CashTicket = packSellGridModel.TicketPrice;
	                        m.TicketUsed = packSellGridModel.TicketUsed;
	                    }
	                    if (packSellGridModel.GridTemplate == "VIPTemplate")
	                    {
	                        var g = packSellGridModel.GridTemplateData as VIPSellGridModel;
                            m.VIP_VIPInfo = new VIP_VIPInfo()
                            {
                                IMEI=g.IMEI,
                                MemberName = g.MemberName,
                                MobiPhone=g.MobiPhone,
                                Address = g.Address,
                                ProPrice=g.ProPrice,
                                Flag=true,
                                Birthday = g.Birthday,
                                IDCard = g.IDCard,
                                LZUser = g.LZUserID,
                                HallID = this.HallInfo.HallID,
                                OldID = g.OldID,
                                QQ = g.QQ,
                                StartTime = g.JoinTime,
                                TelePhone = g.TelePhone,
                                Point = g.Point,
                                Sex = g.Sex,
                                Note = g.Note,
                                SysDate = DateTime.Now,
                                TypeID = g.TypeID,
                                Validity = g.Validity,

                            };

	                    }
	                    if (packSellGridModel.GridTemplate == "YanBaoDataTemplate")
	                    {
                            var yanbaoModel=packSellGridModel.GridTemplateData as YanbaoModel;
	                        var yanbao = new Pro_Sell_Yanbao();
	                        if (yanbaoModel != null)
	                        {
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
	                        }
	                        m.Pro_Sell_Yanbao = yanbao;
	                    }
	                if (packSellGridModel.GridTemplate == "BillDataTemplate")
	                {
                        m.Pro_BillInfo=packSellGridModel.GridTemplateData as Pro_BillInfo;

	                }
	                SellListinfos.Add(m);
                        if (is1st)
                        {
                            firstmodel = packSellGridModel;
                            firstselllist = m;
                            is1st = false;

                        }
	                m.Pro_SellList_RulesInfo = packSellGridModel.Rules;
	            }
	            var modelprice = selectedOff.ArriveMoney - temp;

	            if (firstmodel != null)
	            {

	                if (modelprice > firstmodel.ProPrice)
	                {
	                    firstselllist.OtherCash = modelprice - firstmodel.ProPrice;
	                }
	                else
	                {
	                    firstselllist.OtherCash = 0;
	                }
	                if (firstmodel.TicketUsed > 0)
	                {
	                    if (modelprice < firstmodel.ProPrice - firstmodel.TicketUsed)
	                    {
	                        firstselllist.OffSepecialPrice = firstmodel.ProPrice - firstmodel.TicketUsed - modelprice;

	                    }
	                }
	                else
	                {
	                    if (modelprice < firstmodel.ProPrice)
	                    {
	                        firstselllist.OffSepecialPrice = firstmodel.ProPrice - modelprice;
	                    }
	                    
	                }
                    firstselllist.OtherCash += Convert.ToDecimal(OtherPrice.Value);
	                SellListinfos.Remove(firstselllist);
	                SellListinfos.Insert(0, firstselllist);
	                isOK = true;
	            }
	            else
	            {
	                isOK = false;

	            }
	            foreach (var p in SellListinfos)
	            {



	                p.CashPrice = p.ProPrice - p.TicketUsed - p.OffSepecialPrice + p.OtherCash;
	                decimal d = 0;
	                if (p.Pro_SellList_RulesInfo != null)
	                {
	                    foreach (var proSellListRulesInfo in p.Pro_SellList_RulesInfo)
	                    {
	                        if (p.CashPrice > proSellListRulesInfo.OffPrice)
	                        {
	                            p.CashPrice -= proSellListRulesInfo.OffPrice;
	                            proSellListRulesInfo.RealPrice = proSellListRulesInfo.OffPrice;
	                        }
	                        else
	                        {
	                            proSellListRulesInfo.RealPrice = p.CashPrice;
	                            p.CashPrice -= proSellListRulesInfo.RealPrice;
	                        }
	                    }
	                }
	            }
	            this.SellPrice.Value = SellListinfos.Sum(p => p.CashPrice);
	            this.CashPrice.Value = this.SellPrice.Value;
	            //isOK = true;

	        });

	        if (this.Dispatcher.CheckAccess())
	        {
	            act.Invoke();
	        }
	        this.Dispatcher.BeginInvoke(act);
	        
	    }


	    private void TicketPriceChanged(object sender, TextChangedEventArgs textChangedEventArgs)
	    {
	        
	    }

        private void CashPrice_ValueChanged(object sender, RadRoutedEventArgs e)
        {
            if (this.CashPrice.Value > this.SellPrice.Value)
            {
                this.CashPrice.Value = this.SellPrice.Value;
            }
            this.CardPrice.Value = this.SellPrice.Value - this.CashPrice.Value;
        }

        private void CardPrice_ValueChanged(object sender, RadRoutedEventArgs e)
        {
            if (CardPrice.Value > this.SellPrice.Value)
            {
                this.CardPrice.Value = this.SellPrice.Value;
            }
            this.CashPrice.Value = this.SellPrice.Value - this.CardPrice.Value;
        }

	    private void IMEITextBox_KeyUp(object sender, KeyEventArgs e)
	    {
	        if (e.Key == Key.Enter)
	        {
	            AddButton_OnClick(null,null);
	        }
	    }

	    private void DelButton_OnClick(object sender, RoutedEventArgs e)
	    {
	        throw new NotImplementedException();
	    }

	    private void DeleteButton_Click(object sender, RoutedEventArgs e)
	    {
	        if (Common.CommonHelper.ButtonNotic(sender)) return;
            Control c = sender as Control;
	        if (c == null || c.DataContext == null) return;
            PackSellGridModel model=c.DataContext as PackSellGridModel;

	        if (model == null) return;
	        model.PropertyChanged -= gridmodel_PropertyChanged;
            model.SellListModel=null;
            model.IMEI = "";
            
	        model.ProPrice = 0;
	        model.ProCount = 0;
            model.ProID = "";
	        model.TicketPrice = 0;
	        model.TicketNum = "";
            
            switch (model.GridTemplate)
            {
                case "VIPTemplate":
                    model.GridTemplateData = new Model.VIPSellGridModel();
                    break;
                case "YanBaoDataTemplate":
                    model.GridTemplateData = new Model.YanbaoModel();
                    break;
                case "BillDataTemplate":
                    model.GridTemplateData = new API.Pro_BillInfo();
                    break;
            }
	        calcprices();
            model.PropertyChanged += gridmodel_PropertyChanged;
	        this.Gridvews.Rebind();
	       


	    }

	    private void Exp_OnClick(object sender, RadRoutedEventArgs e)
	    {
	        var b = this.Gridvews.ChildrenOfType<GridViewRow>();
	        foreach (var gridViewRow in b)
	        {
	            if (gridViewRow.DataContext != null)
	            {
	                gridViewRow.DetailsVisibility=Visibility.Visible;
	              
	            }
	        }
	    }

	    private void Clo_OnClick(object sender, RadRoutedEventArgs e)
	    {
            var b = this.Gridvews.ChildrenOfType<GridViewRow>();
            foreach (var gridViewRow in b)
            {
                if (gridViewRow.DataContext != null)
                {
                    gridViewRow.DetailsVisibility = Visibility.Collapsed;

                }
            }
	    }

	    private void RuleTree_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
	    {
            var m = this.Gridvews.SelectedItem as Model.PackSellGridModel;
            if (m == null) return;
            m.Rules.Clear();
            var selected =
                this.RuleTree.SelectedItems.Select(p => (API.Rules_AllCurrentRulesInfo)p)
                    .OrderBy(p => p.RulesTypeID)
                    .ToList();
            foreach (var selectedItem in selected)
            {
                var curr = (API.Rules_AllCurrentRulesInfo)selectedItem;
                m.Rules.Add(new Pro_SellList_RulesInfo()
                {
                    Rules_ProMain_ID = curr.Rules_ProMain_ID,
                    OffPrice = curr.OffPrice,
                    ShowToCus = curr.ShowToCus,
                    CanGetBack = curr.CanGetBack,

                });
            }
            calcprices();
	    }

	    private void Gridvews_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
	    {
	        this.RuleTree.ItemsSource = new List<Rules_AllCurrentRulesInfo>();
            var item = this.Gridvews.SelectedItem as PackSellGridModel;
            if (item == null) return;
            var proproinfoquery = Store.ProInfo.Where(p => p.ProID == item.ProID);
            if (!proproinfoquery.Any()) return;
            var proinfo = proproinfoquery.First();
            var promaininfoquery = Store.ProMainInfo.Where(p => p.ProMainID == proinfo.ProMainID);
            if (!promaininfoquery.Any())
            {
               // ProNameTextBox.Text = "";
                return;
            }
            var promaininfo = promaininfoquery.First();
           // ProNameTextBox.Text = promaininfo.Introduction;
            var rules = AllRulesInfos.Where(p => p.ProMainID == proinfo.ProMainID && p.SellType == item.GroupInfo.SellType);
            this.RuleTree.ItemsSource = rules;
            foreach (var rulesAllCurrentRulesInfo in rules)
            {
                if (
                    this.SellGridModels.Any(
                        p =>
                            p.Rules.Select(o => o.Rules_ProMain_ID)
                                .Contains(rulesAllCurrentRulesInfo.Rules_ProMain_ID)))
                {
                    this.RuleTree.SelectedItems.Add(rulesAllCurrentRulesInfo);
                }
            }
	    }
        private void RuleTree_OnCellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            var m = this.Gridvews.SelectedItem as PackSellGridModel;
            if (m == null) return;
            m.SellListModel.Pro_SellList_RulesInfo.Clear();
            var selected =
                this.RuleTree.SelectedItems.Select(p => (API.Rules_AllCurrentRulesInfo)p)
                    .OrderBy(p => p.OrderBy)
                    .ToList();
            foreach (var selectedItem in selected)
            {
                var curr = (API.Rules_AllCurrentRulesInfo)selectedItem;
                m.SellListModel.Pro_SellList_RulesInfo.Add(new Pro_SellList_RulesInfo()
                {
                    Rules_ProMain_ID = curr.Rules_ProMain_ID,
                    OffPrice = curr.OffPrice,
                    ShowToCus = curr.ShowToCus,
                    CanGetBack = curr.CanGetBack,

                });
            }
            calcprices();
        }

        private void RuleTree_OnCellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            var model = e.Cell.DataContext as Rules_AllCurrentRulesInfo;
            if (model == null) return;
            if (e.Cell.Column.UniqueName == "OffPrice")
            {
                decimal newValue = decimal.Parse(e.NewValue.ToString());
                if (newValue < model.MinPrice || newValue > model.MaxPrice)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "规则价格必须在" + model.MinPrice + "和" + model.MaxPrice + "之间";
                }
            }
        }

	    private void OtherPrice_ValueChanged(object sender, RadRoutedEventArgs e)
	    {
	        calcprices();
	    }

	    private void BillMobileIMEISearch_Onclick(object sender, RoutedEventArgs e)
	    {
	        Button b = sender as Button;
	        var stackpanel = b.ParentOfType<StackPanel>();
	        var MobileIMEI = stackpanel.FindChildByType<TextBox>();
	        var row = b.ParentOfType<RadRowItem>();
            var sellmodel = row.DataContext as PackSellGridModel;
	        var billinfo = sellmodel.GridTemplateData as Pro_BillInfo;
	        var mainpanel = b.ParentOfType<StackPanel>().ParentOfType<StackPanel>();
	        MobileIMEI.Text = MobileIMEI.Text.ToUpper();
	        MobileIMEI.IsReadOnly = true;

	        string IMEI = MobileIMEI.Text;
            if (!SellGridModels.Select(p => p.IMEI).Contains(IMEI))
            {
                MessageBox.Show(Application.Current.MainWindow, "套餐内的合同只允许加入在本套餐销售的终端");
                MobileIMEI.IsReadOnly = false;
                return;
            }
	        new Action(() =>
	        {
	            try
	            {
                    var result = wsclient.Main(Yanbao_CheckIMEI_MethodID, new List<object>() { IMEI });
	                if (result.ReturnValue)
	                {
	                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
	                    {
	                        API.Pro_IMEI imeiinfo = result.ArrList[0] as Pro_IMEI;
	                        API.Pro_SellListInfo selllist = result.ArrList[1] as Pro_SellListInfo;
	                        if (imeiinfo != null)
	                        {
	                            var BillPro = Store.ProInfo.First(p => p.ProID == imeiinfo.ProID);

	                            ((TextBox) (mainpanel.FindName("MobileName"))).Text = BillPro.ProName;


	                            ((TextBox) (mainpanel.FindName("ModelClass"))).Text =
	                                Store.ProClassInfo.First(p => p.ClassID == BillPro.Pro_ClassID).ClassName;
	                            MobileIMEI.IsReadOnly = true;
                                billinfo.MobileProID = BillPro.ProID;
                                billinfo.MobileClassID = BillPro.Pro_ClassID;

	                            billinfo.MobileIMEI = IMEI;


	                            var query = Store.BillFields.Where(p => p.ProID == sellmodel.ProID);
	                            if (query.Any())
	                            {
	                                foreach (var proBillFieldInfo in query)
	                                {

	                                    var num = proBillFieldInfo.BillFieldName.Replace("BillField", "");
	                                    var obj = mainpanel.FindName("FieldLabel" + num) as Label;
	                                    if (obj != null)
	                                    {
	                                        obj.Content = proBillFieldInfo.BillFieldValue;
                                            var obj2 = mainpanel.FindName("StackPanel" + num) as StackPanel;
	                                        if (obj2 != null)
	                                        {
	                                            obj2.Visibility = Visibility.Visible;
	                                        }
	                                    }
	                                }


	                            }


	                        }
	                    }));
	                }
	                else
	                {
	                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
	                    {
	                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + result.Message);
	                        MobileIMEI.IsReadOnly = false;
	                        this.busy.IsBusy = false;
	                    }));

	                }
	            }
	            catch (Exception ex)
	            {
	                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
	                {
	                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + ex.Message);
                        MobileIMEI.IsReadOnly = false;
	                    this.busy.IsBusy = false;
	                }));
	            }
	        }).BeginInvoke(null, null);
//            PublicRequestHelp helper = new PublicRequestHelp(busy, Yanbao_CheckIMEI_MethodID, new object[] { this.MobileIMEI.Text.Trim() }, CheckIMEI_Completed);

	    }

//        private void CheckIMEI_Completed(object sender, MainCompletedEventArgs e)
//        {
//
//            this.busy.IsBusy = false;
//            var model = this.MainPanel.DataContext as API.Pro_BillInfo_temp;
//            if (model == null)
//                return;
//            if (e.Error == null)
//            {
//                if (e.Result.ReturnValue)
//                {
//                    try
//                    {
//                        API.Pro_IMEI imeiinfo = e.Result.ArrList[0] as Pro_IMEI;
//                        API.Pro_SellListInfo selllist = e.Result.ArrList[1] as Pro_SellListInfo;
//                        if (imeiinfo != null)
//                        {
//                            this.BillPro = Store.ProInfo.First(p => p.ProID == imeiinfo.ProID);
//
//                            this.MobileName.Text = BillPro.ProName;
//
//
//                            this.ModelClass.Text =
//                                Store.ProClassInfo.First(p => p.ClassID == BillPro.Pro_ClassID).ClassName;
//                            this.MobileIMEI.IsReadOnly = true;
//                            model.MobileProID = this.BillPro.ProID;
//                            model.MobileClassID = this.BillPro.Pro_ClassID;
//                        }
//                        else
//                        {
//                            MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 串码不存在");
//                            this.MobileIMEI.IsReadOnly = false;
//                            return;
//                        }
//
//
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 数据错误\n" + ex.Message);
//                        this.MobileIMEI.IsReadOnly = false;
//                    }
//
//                }
//
//
//                else
//                {
//                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + e.Result.Message);
//                    this.MobileIMEI.IsReadOnly = false;
//                }
//            }
//            else
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
//            }
//        }

	    }
    }

