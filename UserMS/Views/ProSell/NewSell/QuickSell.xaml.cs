using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;
using UserMS.Views.ProSell;
using Label = System.Windows.Controls.Label;

namespace UserMS
{
	public partial class QuickSell
	{
	    private VIP_VIPInfo SellVIP;

	    private List<API.Pro_ProInfo> IMEISellProInfos;//串码销售权限
	    private List<API.Pro_ProInfo> ProSellProInfos;//非串码销售权限
	    private List<API.Pro_ProInfo> ticketSellProInfos;//兑券
	    private List<API.Pro_ProInfo> JiPeiKaProInfos;
	    private List<Pro_ProInfo> ChargeProInfos;
	    private List<Pro_ProInfo> GouJiProInfos;
	    private List<Pro_ProInfo> BillProInfos;
        private ObservableCollection<UserMS.Model.ProSellGridModel> IMEIGridModels = new ObservableCollection<ProSellGridModel>();
        private ObservableCollection<UserMS.Model.ProSellGridModel> ProSellGridModels = new ObservableCollection<ProSellGridModel>();
        private ObservableCollection<UserMS.Model.ProSellGridModel> TicketSellGridModels = new ObservableCollection<ProSellGridModel>();
        private ObservableCollection<UserMS.Model.ProSellGridModel> JiPeiKaGridModels = new ObservableCollection<ProSellGridModel>();
        private ObservableCollection<UserMS.Model.ProSellGridModel> GoujiGridModels=new     ObservableCollection<ProSellGridModel>();
	    private ObservableCollection<API.Pro_BillInfo> BillGridModels = new ObservableCollection<Pro_BillInfo>();
        private ObservableCollection<AirChargeModel> ChargeModels = new ObservableCollection<AirChargeModel>();
	    private int MethodID_IMEIGetInfo = 5;
        private int Yanbao_CheckIMEI_MethodID = 109;
        private int Yanbao_GetCurrentPrice_MethodID = 146;
        private int GetProStore_Method = 88;
	    private int VIP_GetService = 40;
        private int MethodID_SellNext = 8;
	    private UserMsServiceClient wsClient = Store.wsclient;
	    private API.Pro_HallInfo HallInfo;
	    private List<View_YanBoPriceStepInfo> YanBaoPrices;
        private Pro_ProInfo yanbaoModelProInfo;
	    private Pro_ProInfo yanbaoProInfo;
	    private Pro_ProInfo ChargeProInfo;
        private ObservableCollection<YanbaoModel> Yanbaomodellist = new ObservableCollection<YanbaoModel>();
        private ObservableCollection<VIPSellGridModel> vipSellGridModel = new ObservableCollection<VIPSellGridModel>();
	    private List<API.Sys_UserInfo> UserInfos;
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();

        public override string ToString()
        {
            return "快速销售";
        }

        private void QuickSell_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= QuickSell_OnLoaded;
            //初始化所有权限
            var tempinfos = Common.CommonHelper.GetPro(99);
            IMEISellProInfos=tempinfos.Where(p=>p.NeedIMEI).ToList();
            ProSellProInfos = tempinfos.Where(p => p.NeedIMEI == false).ToList();
            ticketSellProInfos = Common.CommonHelper.GetPro(100).Where(p => p.IsTicketUsedable && p.NeedIMEI).ToList();
            JiPeiKaProInfos = Common.CommonHelper.GetPro(213).Where(p=>p.NeedIMEI).ToList();
            ChargeProInfos = CommonHelper.GetPro(102);
            GouJiProInfos = CommonHelper.GetPro(263);
            BillProInfos = CommonHelper.GetPro(344);
            this.IMEISellGrid.ItemsSource = IMEIGridModels;
            this.ProSellGrid.ItemsSource = ProSellGridModels;
            this.TicketSellGrid.ItemsSource = TicketSellGridModels;
            this.JiPeiKaSellGrid.ItemsSource = JiPeiKaGridModels;
            this.YanbaoGridView.ItemsSource = Yanbaomodellist;
            this.ChargeGridView.ItemsSource = ChargeModels;
            this.GoujiSellGrid.ItemsSource = GoujiGridModels;
            this.billGrid.ItemsSource = BillGridModels;
            //this.ProSellProInfos = ProSellProInfos;
            this.ProSellAutoBox.ItemsSource = ProSellProInfos;
            this.ProSellAutoBox.TextSearchPath = "ProName";
            this.ProSellAutoBox.DisplayMemberPath = "ProName";
            this.viplistbox.ItemsSource = vipSellGridModel;
            this.viplistbox.ItemContainerGenerator.ItemsChanged+=ItemContainerGeneratorOnItemsChanged;
            this.viplistbox.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
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
            UserInfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model => model.UserID,
                                                 (info, model) => info).ToList();
            
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
            }

            var r = Store.Options.First(option => option.ClassName == "GXYanBao").Value;
            var temp = Common.CommonHelper.GetPro(101).Where(p => p.ProID == r).ToList();
            if (temp.Any())
            {
                yanbaoProInfo = temp[0];

            }

            new Action(() =>
            {

                var result = wsClient.Main(Yanbao_GetCurrentPrice_MethodID, new List<object>());
                if (result.ReturnValue)
                {YanBaoPrices = (List<View_YanBoPriceStepInfo>) result.Obj;}
                else
                {
                    YanBaoPrices=new List<View_YanBoPriceStepInfo>();
                }
                var result2 = wsClient.Main(GetProStore_Method,
                    new List<object>() {ChargeProInfos.Select(p => p.ProID).ToList()});
                if (result2.ReturnValue)
                {
                    ChargeProInfos = (List<Pro_ProInfo>)result2.Obj;
                    ChargeProInfo = ChargeProInfos.First();
                    this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        this._ProInfo.DataContext = ChargeProInfo;
                    
                    this._Balance.Value =
                        ChargeProInfos.First(p => p.ProID == ChargeProInfo.ProID)
                            .Pro_StoreInfo.Where(p => p.HallID == this.HallInfo.HallID)
                            .Sum(p => p.ProCount);
                    }));
                }

                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {

                    this.busy.IsBusy = false;
                }));

            }).BeginInvoke(null, null);




#if !HZ
            VIP_OnMouseLeftButtonUp(null, null);
#endif

        }

        void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            var generator = sender as ItemContainerGenerator;
            if (generator.Status == GeneratorStatus.ContainersGenerated)
            {
                for (int i = 0; i < this.viplistbox.Items.Count; i++)
                {
                    var myListBoxItem = (RadListBoxItem)this.viplistbox.ItemContainerGenerator.ContainerFromIndex(i);
                    ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);
                    DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                    MyAutoTextBox target = (MyAutoTextBox)myDataTemplate.FindName("Seller", myContentPresenter);
                    target.TextBox_SelectionChanged = SellerSelectEvent;
                    target.SearchEvent = SellerSearchEvent;
                    target.ItemsSource = UserInfos;
                    target.TextBox.SetBinding(RadAutoCompleteBox.SearchTextProperty, new Binding()
                    {
                        Path = new PropertyPath("LZUserName"),
                        Mode = BindingMode.TwoWay
                    });
                }
            }
        }

	    private void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs itemsChangedEventArgs)
	    {
	        
	        
           
            
	    }

	    private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {

            

        }

        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            var autotextbox=((Telerik.Windows.Controls.RadButton) sender).ParentOfType<MyAutoTextBox>();

            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
                                                  "Username", new string[] { "Username", "opname" },
                                                  new string[] { "用户名", "职位" });

            w.Closed +=(o, args) => SellerSearchWindowClose(autotextbox,o,args);
            w.ShowDialog();
        }

        void SellerSearchWindowClose(MyAutoTextBox box,object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    UserOpModel selected = (UserOpModel)window.SelectedItem;
                    
                    box.TextBox.SearchText = selected.Username;


                }
            }

        } 


		public QuickSell()
		{
            
			this.InitializeComponent();
#if HZ
            this.VIPBTN.Visibility = Visibility.Collapsed;
            this.VIPPOINT.Visibility = Visibility.Collapsed;
#endif
            

			// 在此点之下插入创建对象所需的代码。
		  this.ProSellAutoBox.SelectionMode=AutoCompleteSelectionMode.Single;

		    //viplistbox.ItemsSource = new List<string>() {"1", "2"};
		    this.ProSellAutoBox.SelectionChanged += (sender, args) =>
		    {
		        if (args.AddedItems.Count > 0)
		        {
		            API.Pro_ProInfo proinfo = (Pro_ProInfo) args.AddedItems[0];
                    if (ProSellGridModels.All(p=>p.ProID!=proinfo.ProID)){
                    this.ProSellGridModels.Add(new ProSellGridModel()
                    {
                        ProID = proinfo.ProID,
                        ProCount = 1,

                    });
		            this.ProSellAutoBox.SearchText = "";                    
                    //this.ProSellGrid.Rebind();

                    }
		        }
		    };

		}



	    private void Prev_OnClick(object sender, RadRoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
	        this.NavigationService.Navigate(new QuickSell());
	    }

	    private void Save_OnClick(object sender, RadRoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            List<Pro_SellListInfo> Selllists=new List<Pro_SellListInfo>();

	        foreach (var proSellGridModel in IMEIGridModels)
	        {
	            Pro_SellListInfo_SingleSell info = new Pro_SellListInfo_SingleSell();
	            info.ProID = proSellGridModel.ProID;
	            info.ProCount = 1;
	            info.IMEI = proSellGridModel.IMEI;
	            info.SellType = 1;
	            info.Note = proSellGridModel.Note;
                    Selllists.Add(info);
    	        }
	        foreach (var proSellGridModel in ProSellGridModels)
	        {
	            Pro_SellListInfo_SingleSell info =new Pro_SellListInfo_SingleSell();
	            info.ProID = proSellGridModel.ProID;
	            info.ProCount = proSellGridModel.ProCount;
	            info.SellType = 1;
	            info.Note = proSellGridModel.Note;
	            Selllists.Add(info);


	        }
	        foreach (var ticketSellGridModel in TicketSellGridModels)
	        {
	            Pro_SellListInfo_TicketSell info = new Pro_SellListInfo_TicketSell();
	            info.ProID = ticketSellGridModel.ProID;
	            info.ProCount = 1;
                info.SellType = 2;
	            info.IMEI = ticketSellGridModel.IMEI;
	            info.CashTicket = ticketSellGridModel.TicketPrice;
	            info.TicketID = ticketSellGridModel.TicketNum;
                Selllists.Add(info);

	        }
	        foreach (var yanbaoModel in Yanbaomodellist)
	        {
	            Pro_SellListInfo_YanBaoSell info=new Pro_SellListInfo_YanBaoSell();
	            info.ProID = yanbaoProInfo.ProID;
	            info.ProCount = 1;
                info.SellType = 1;
	            info.IMEI = yanbaoModel.OldID;
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
                info.ProPrice = yanbaoModel.YanbaoPrice;
                info.Pro_Sell_Yanbao = yanbao;
                Selllists.Add(info);

	        }
	        foreach (var airChargeModel in ChargeModels)
	        {
	           Pro_SellListInfo_AirSell info =new Pro_SellListInfo_AirSell();

	            info.ProID = airChargeModel.ProID;
	            info.ProCount = airChargeModel.ProCount;
	            info.ChargePhoneName = airChargeModel.Name;
	            info.ChargePhoneNum = airChargeModel.ChargePhone;
	            info.SellType = 1;
                Selllists.Add(info);


	        }
	        foreach (var sellGridModel in vipSellGridModel)
	        {
	            if (string.IsNullOrEmpty(sellGridModel.MemberName))
	            {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员姓名不能为空");
                    return;
	            }
	            if (string.IsNullOrEmpty(sellGridModel.Sex))
	            {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员性別不能为空");
                    return;
	            }
                if (sellGridModel.Birthday==null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员生日不能为空");
                    return;
                }
                if (!PormptPage.isNumeric(sellGridModel.MobiPhone))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请填写正确的手机号码！");
                    return;
                }
                if (string.IsNullOrEmpty(sellGridModel.LZUserName))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请填写揽装人名称");
                    return;
                }
	            if (Store.UserInfos.All(p => p.RealName != sellGridModel.LZUserName))
	            {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, messageBoxText: "揽装人不存在");

                    return;
	            }
	            sellGridModel.LZUserID = Store.UserInfos.First(p => p.RealName == sellGridModel.LZUserName).UserID;
                API.VIP_VIPInfo vipinfo = new API.VIP_VIPInfo();
                
            vipinfo.IMEI = sellGridModel.IMEI;
            vipinfo.MemberName = sellGridModel.MemberName;
	            vipinfo.Birthday = sellGridModel.Birthday;

            vipinfo.MobiPhone = sellGridModel.MobiPhone;
            vipinfo.TelePhone = sellGridModel.TelePhone;
            vipinfo.QQ = sellGridModel.QQ;
            vipinfo.Address = sellGridModel.Address;
            vipinfo.IDCard = sellGridModel.IDCard;
            vipinfo.HallID = this.HallInfo.HallID;
            vipinfo.OldID = sellGridModel.OldID;

            vipinfo.Note = sellGridModel.Note;
	            vipinfo.IDCard_ID = sellGridModel.IDCard_ID;
	            vipinfo.StartTime = sellGridModel.JoinTime;
	            vipinfo.Validity = sellGridModel.Validity;
	            vipinfo.TypeID = sellGridModel.TypeID;
	            vipinfo.Point = sellGridModel.Point;
	            vipinfo.ProPrice = sellGridModel.ProPrice;
	            vipinfo.Flag = true;
                
                Pro_SellListInfo_MemberSell info=new Pro_SellListInfo_MemberSell();
	            info.ProID = sellGridModel.ProID;
	            info.ProCount = 1;
	            info.IMEI = sellGridModel.IMEI;
	            info.SellType = 1;
	            info.VIP_VIPInfo = vipinfo;
                Selllists.Add(info);



	        }
	        foreach (var proSellGridModel in JiPeiKaGridModels)
	        {
                Pro_SellListInfo info = new Pro_SellListInfo();
                info.ProID = proSellGridModel.ProID;
                info.ProCount = proSellGridModel.ProCount;
                info.IMEI = proSellGridModel.IMEI;


                if (String.IsNullOrEmpty(proSellGridModel.TicketNum))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "机身串码不能为空");
                    return;
                }

                info.Pro_Sell_JiPeiKa = new Pro_Sell_JiPeiKa() { IMEI = proSellGridModel.TicketNum.ToUpper() };
                info.SellType = 6;
                info.Note = proSellGridModel.Note;
                Selllists.Add(info);
	        }
	        foreach (var proSellGridModel in GoujiGridModels)
	        {
                Pro_SellListInfo info = new Pro_SellListInfo();
                info.ProID = proSellGridModel.ProID;
                info.ProCount = proSellGridModel.ProCount;
                info.IMEI = proSellGridModel.IMEI;


                if (String.IsNullOrEmpty(proSellGridModel.TicketNum))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "购机送费码不能为空");
                    return;
                }

                info.SellType = 11;
                info.Note = proSellGridModel.Note;
                Selllists.Add(info);
	        }

            foreach (var proBillInfoTemp in BillGridModels)
            {
                API.Pro_SellListInfo sellList = new Pro_SellListInfo();
                var model = proBillInfoTemp;
                
                sellList.ProID = model.ProID;
                sellList.IMEI = model.BillIMEI;
                sellList.ProCount = 1;
                sellList.Pro_BillInfo = model;
               
                sellList.SellType = 1;
                Selllists.Add(sellList);
            }

             if (Selllists.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"没有任何商品");
                return;
            }

            if (string.IsNullOrEmpty(VIPName.Text) || string.IsNullOrEmpty(VIPPhone.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入客户资料");
                return;
            }
            if (string.IsNullOrEmpty(SellOldID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入原始单号");
                return;
            }

            System.Text.RegularExpressions.Regex  ex=new Regex(@"^.{7}$");
            if (!ex.IsMatch(SellOldID.Text))
            {
                MessageBox.Show(Application.Current.MainWindow, "请输入正确的原始单号");
                return;
            }

            Pro_SellInfo sellinfo=new Pro_SellInfo();
            if (this.SellVIP!=null)
            {
                sellinfo.VIP_ID = this.SellVIP.ID;
            }
            sellinfo.SysDate = DateTime.Now;
	        sellinfo.CusName = this.VIPName.Text.Trim();
	        sellinfo.CusPhone = this.VIPPhone.Text.Trim();
	        sellinfo.HallID = this.HallInfo.HallID;
            sellinfo.SellDate = DateTime.Now;
	        sellinfo.OldID = this.SellOldID.Text;
	        sellinfo.Pro_SellListInfo = Selllists;
            new PublicRequestHelp(busy, MethodID_SellNext, new object[] { sellinfo, new List<int>() }, SellNextEvent);




	    }
        private void SellNextEvent(object sender, MainCompletedEventArgs e)
        {
            busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    if (((Pro_SellInfo)e.Result.Obj).Pro_SellListInfo.Count == 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "无可销售商品");
                        return;
                    }
                    var newpage = new NewProSellSetp2((Pro_SellInfo)e.Result.Obj, (from b in (List<API.VIP_OffList>)e.Result.ArrList[0] select (API.VIP_OffList)b).ToList(), this.SellVIP);
                    newpage.AllRulesInfos = (List<Rules_AllCurrentRulesInfo>) e.Result.ArrList[1];
                    newpage.SellTempIds = new List<int>();
                    newpage.oldpage = new QuickSell();
                    this.NavigationService.Navigate(newpage);
                    //                        this.Content = newpage;


                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "提交失败: " + e.Result.Message);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "提交失败: 服务器错误\n" + e.Error.Message);

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

        private void Charge_Add_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.ChargeProInfo == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未选择空充号码");
                return;
            }

            if (this._Phone1.Value.Length != 11)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "电话号码长度不对");
                return;
            }
            if (this._ChargePrice.Value == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入充值金额");
                return;
            }
            AirChargeModel sells = new AirChargeModel();
            sells.ProID = ChargeProInfo.ProID;
            sells.ProCount = Convert.ToDecimal(_ChargePrice.Value);
            sells.ProPrice = ChargeProInfo.Pro_SellTypeProduct.First(p => p.SellType == 1).Price;
            sells.ProMoney = sells.ProPrice * sells.ProCount;
            sells.ProName = ChargeProInfo.ProName;
            sells.ChargePhone = this._Phone1.Value;
            sells.Name = this._Name1.Text;
           
            sells.Note = this._Note1.Text;
           
            ChargeModels.Add(sells);
           //this.ChargeGridView.Rebind();
            chargeclear();

            //throw new NotImplementedException();
          
	    }

	    void clearyanbaofrom()
	    {
            this.yanbaoModelProInfo = null;
            

            this._Name.Text = "";
            this._Phone.Text = "";
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
	        throw new NotImplementedException();
	    }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {

        }


	    private void IMEIAddButton_Click(object sender, RoutedEventArgs e)
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
                    var result = wsClient.Main(this.MethodID_IMEIGetInfo, new List<object>() { imei, this.HallInfo.HallID });
                    if (result.ReturnValue)
                    {
                        API.Pro_ProInfo i = (Pro_ProInfo)result.Obj;
                        if (IMEISellProInfos.Any(p => p.ProID == i.ProID))
                        {
                            if (IMEIGridModels.All(p => p.IMEI != imei))
                                this.IMEIGridModels.Add(new ProSellGridModel()
                                {
                                    ProID = i.ProID,
                                    ProCount = 1,
                                    IMEI = imei,

                                });
//                            this.IMEISellGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
//                                new Action(() => IMEISellGrid.Rebind()));
                        }
                        else
                        {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                            Application.Current
                                .MainWindow,
                            "添加失败: 商品无权操作")));
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
                }));

	        }).BeginInvoke(null,null);

	       

	    }

	    private void ProSellSearchButton_OnClick(object sender, RoutedEventArgs e)
	    {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();

            Common.CommonHelper.ProFilterGen(this.ProSellProInfos, ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
                                            pros, "ProName", new string[] { "ClassName", "TypeName", "ProName", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "商品属性" });
            m.Closed += ProSellSearchWindow_closed;
            m.ShowDialog();
	    }

	    private void ProSellSearchWindow_closed(object sender, WindowClosedEventArgs e)
	    {
            MultSelecter2 s = (MultSelecter2)sender;
            if (s.DialogResult == true)
            {

                List<SlModel.ProductionModel> i = (from object b in s.SelectedItems select (SlModel.ProductionModel)b).ToList();
                foreach (var proProInfo in i)
                {
                    var l = new ProSellGridModel();
                    l.ProID = proProInfo.ProID;
                    //                    l.ProName = proProInfo.ProName;
                    l.ProCount = 1;
                    this.ProSellGridModels.Add(l);
                }



                //this.ProID.TextBox.SearchText = "";
                //this.ProSellGrid.Rebind();
            }
	    }

	    private void TicketSellAddButton_OnClick(object sender, RoutedEventArgs e)
	    {
            var imei = this.TicketIMEITextBox.Text.Trim().ToUpper();
            new Action(() =>
            {
                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    busy.IsBusy = true;
                }));
                try
                {
                    var result = wsClient.Main(this.MethodID_IMEIGetInfo, new List<object>() { imei, this.HallInfo.HallID });
                    if (result.ReturnValue)
                    {
                        API.Pro_ProInfo i = (Pro_ProInfo)result.Obj;
                        if (ticketSellProInfos.Any(p => p.ProID == i.ProID))
                        {
                            if (TicketSellGridModels.All(p => p.IMEI != imei))
                                this.TicketSellGridModels.Add(new ProSellGridModel()
                                {
                                    ProID = i.ProID,
                                    ProCount = 1,
                                    IMEI = imei,

                                });
//                            this.TicketSellGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
//                                new Action(() => TicketSellGrid.Rebind()));
                        }
                        else
                        {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                            Application.Current
                                .MainWindow,
                            "添加失败: 商品无权操作")));
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
                }));

            }).BeginInvoke(null, null);

	       
	    }

	    private void CARDADD_OnClick(object sender, RoutedEventArgs e)
	    {
            var imei = this.CARDIMEI.Text.Trim().ToUpper();
            new Action(() =>
            {
                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    busy.IsBusy = true;
                }));
                try
                {
                    var result = wsClient.Main(this.MethodID_IMEIGetInfo, new List<object>() { imei, this.HallInfo.HallID });
                    if (result.ReturnValue)
                    {
                        API.Pro_ProInfo i = (Pro_ProInfo)result.Obj;
                        if (JiPeiKaProInfos.Any(p => p.ProID == i.ProID))
                        {
                            if (JiPeiKaGridModels.All(p => p.IMEI != imei))
                                this.JiPeiKaGridModels.Add(new ProSellGridModel()
                                {
                                    ProID = i.ProID,
                                    ProCount = 1,
                                    IMEI = imei,

                                });
//                            this.JiPeiKaSellGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
//                                new Action(() => JiPeiKaSellGrid.Rebind()));
                        }
                        else
                        {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                            Application.Current
                                .MainWindow,
                            "添加失败: 商品无权操作")));
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
                }));

            }).BeginInvoke(null, null);
	    }

	    
	    private void _CheckIMEI_OnClick(object sender, RoutedEventArgs e)
	    {
	        this._IMEI.IsReadOnly = true;
	        var imei = this._IMEI.Text.Trim().ToUpper();
	        new Action(() =>
	        {
	            try
	            {
	                var result = wsClient.Main(Yanbao_CheckIMEI_MethodID, new List<object>() {imei});
	                if (result.ReturnValue)
	                {
                        API.Pro_IMEI imeiinfo = result.ArrList[0] as Pro_IMEI;
                        API.Pro_SellListInfo selllist = result.ArrList[1] as Pro_SellListInfo;
                        if (imeiinfo != null)
                        {
                            this.yanbaoModelProInfo = Store.ProInfo.First(p => p.ProID == imeiinfo.ProID);
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                this._Model.Text = yanbaoModelProInfo.ProName;


                                this._ModelClass.Text = yanbaoModelProInfo.ProClassName;
//                                Store.ProClassInfo.First(p => p.ClassID == Pro.Pro_ClassID).ClassName;
                            }));
                        }
                        else
                        {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 串码不存在");
                                this._IMEI.IsReadOnly = false;
                            }));
                        }

                        if (selllist != null)
                        {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                this._ModelPrice.Value = selllist.YanbaoModelPrice;
                            }));

                        }

	                }
	                else
                    {
                        this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: "+result.Message);
                            this._IMEI.IsReadOnly = false;
                        }));
	                    
	                }
	            }
	            catch (Exception ex)
	            {
                    this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + ex.Message);
                        this._IMEI.IsReadOnly = false;
                    }));
	            }

	        }).BeginInvoke(null, null);

	    }

	    private void Charge_Cancel_OnClick(object sender, RoutedEventArgs e)
	    {
	        chargeclear();
	    }

	    void chargeclear()
	    {
            this._ChargePrice.Value = 0;
            this._VIP.DataContext = null;
            
            this._Name1.Text = "";
            this._Phone1.Value = "";
            this._Note1.Text = "";

	    }
	    private void Yanbao_Add_Click(object sender, RoutedEventArgs e)
	    {
            if (this.yanbaoModelProInfo == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未验证终端型号");
                return;
            }
            //            if (string.IsNullOrEmpty(_SellID.Text))
            //            {
            //                MessageBox.Show(System.Windows.Application.Current.MainWindow,"销售单号不能为空");
            //                return;
            //            }
            if (string.IsNullOrEmpty(_OldID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "合同号不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_InvoiceNumber.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "发票号码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_BatteryIMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "电池编码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_ChargerIMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "充电器编码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(_HandphoneIMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "耳机编码不能为空");
                return;
            }

            if (Convert.ToDecimal(this._ModelPrice.Value) == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "终端价格为0 不可销售延保");
                return;
            }

	        var imei = _OldID.Text.Trim().ToUpper();
            new Action(() =>
            {
                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    busy.IsBusy = true;
                }));
                try
                {
                    var result = wsClient.Main(this.MethodID_IMEIGetInfo, new List<object>() { imei, this.HallInfo.HallID });
                    if (result.ReturnValue)
                    {
                        
                        API.Pro_ProInfo i = (Pro_ProInfo)result.Obj;
                        if (i.ProID==yanbaoProInfo.ProID)
                        {

                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                            {

                            YanbaoModel model = new YanbaoModel();
                            model.ProID = this.yanbaoModelProInfo.ProID;
                            model.Name = this._Name.Text;
                            model.Phone = this._Phone.Text;
                            model.OldID = this._OldID.Text;
                            model.IMEI = imei;
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

                            var pro = yanbaoProInfo;
                            var modelpro = this.yanbaoModelProInfo;
                            if (modelpro.YanBaoModelID != null && modelpro.YanBaoModelID != 0)
                            {
                                if (YanBaoPrices.Any(p => p.ID == modelpro.YanBaoModelID ))
                                {
                                    model.YanbaoPrice = Convert.ToDecimal(YanBaoPrices.First(p => p.ID == modelpro.YanBaoModelID).ProPrice);
                                    model.YanbaoName = YanBaoPrices.First(p => p.ID == modelpro.YanBaoModelID).Name;
                                }
                            }
                            else
                                if (
                        YanBaoPrices.Any(p => p.ProID == pro.ProID && p.StepPrice >= model.ModelPrice))
                                {

                                    model.YanbaoPrice =
                                        Convert.ToDecimal(YanBaoPrices.Where(p => p.ProID == pro.ProID && p.StepPrice >= model.ModelPrice)
                                                                      .OrderBy(p => p.StepPrice).First().ProPrice);
                                    model.YanbaoName = YanBaoPrices.Where(p => p.ProID == pro.ProID && p.StepPrice >= model.ModelPrice)
                                                                   .OrderBy(p => p.StepPrice).First().Name;
                                }
                                else
                                {
                                    //                model.YanbaoPrice =
                                    //                    Convert.ToDecimal(Yanbaoprices.Where(p => p.ProID == pro.ProID)
                                    //                                                  .OrderByDescending(p => p.StepPrice).First().ProPrice);
                                    
                                                MessageBox.Show(System.Windows.Application.Current.MainWindow,
                                                    "该终端无法购买延保服务");
                                    return;

                                }



                            
                            //this.YanbaoGridView.Rebind();
                           
                                Yanbaomodellist.Add(model);
                                clearyanbaofrom();
                            }));
                            
                        }
                        else
                        {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                            Application.Current
                                .MainWindow,
                            "添加失败: 合同号错误")));
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
                }));

            }).BeginInvoke(null, null);






	    }

	    private void Yanbao_Cancel_Click(object sender, RoutedEventArgs e)
        {
            clearyanbaofrom(); 
	    }

	    private void Charge_Select_Pro(object sender, RoutedEventArgs e)
	    {

            SingleSelecter w = new SingleSelecter(null, ChargeProInfos.Where(p => p.Pro_StoreInfo.Where(o => o.HallID == this.HallInfo.HallID).Sum(q => q.ProCount) > 0), null, "ProName", new[] { "ProID", "ProName" }, new[] { "商品编号", "商品名称" });

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
                ChargeProInfo = w.SelectedItem as Pro_ProInfo;
                if (ChargeProInfo == null || !ChargeProInfo.Pro_SellTypeProduct.Any(p => p.SellType == 1))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "该号码未定价, 无法进行空中充值");
                    return;
                }
                this._ProInfo.DataContext = ChargeProInfo;
                this._Balance.Value =
                    ChargeProInfos.First(p => p.ProID == ChargeProInfo.ProID).Pro_StoreInfo.Where(p => p.HallID == this.HallInfo.HallID).Sum(p => p.ProCount);
                // PublicRequestHelp helper=new PublicRequestHelp(busy,GetProStore_Method,new object[]{Hall.HallID,Pro.ProID},ProStoreCheckCompleted );

            }
	    }

	    private void VIPAdd_OnClick(object sender, RadRoutedEventArgs e)
	    {
	        var item = new VIPSellGridModel() {CardType = Store.CardType};
            vipSellGridModel.Add(item);
	        //this.viplistbox.ItemsSource = vipSellGridModel;
	        //this.viplistbox.Items.Add(new VIPSellGridModel());
	        
        

	    }
         private childItem FindVisualChild<childItem>(DependencyObject obj)
                   where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

	    private void Tbcardid_OnLostFocus(object sender, RoutedEventArgs e)
	    {
	        var textbox = sender as TextBox;
	        if (textbox == null)
	        {
	            return;
	        }
	        VIPSellGridModel model= textbox.DataContext as VIPSellGridModel;
	        if (model == null) return;
	        if (string.IsNullOrEmpty(textbox.Text.Trim().ToUpper())) return;
	        this.busy.IsBusy = true;
            var imei = textbox.Text.Trim().ToUpper();
	        new Action(
	            () =>
	            {
	                try
	                {
	                   
	                    var result = wsClient.Main(VIP_GetService, new List<object>()
	                    {
	                        imei
	                    });
	                    if (result.ReturnValue)
	                    {
	                        if (result.ArrList[0] == null)
	                        {
	                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
	                            {
	                                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 该会员卡无仓库信息");

	                            }));
	                        }
	                        else
	                        {
	                            string hallid = result.ArrList[0].ToString();
	                            if (hallid != this.HallInfo.HallID)
	                            {
	                                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
	                                {
	                                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 该会员卡不在本仓库");
                                        this.busy.IsBusy = false;
	                                    textbox.Text = "";
	                                }));
	                            }
                                else{
	                            var viptype = result.Obj as API.VIP_VIPType;
	                            model.VIPType = viptype;
                                    model.ProID = result.ArrList[1].ToString();
                                    model.IMEI = imei;
                                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                                {
                                   this.busy.IsBusy = false;
                                   
                                }));
                                }
	                        }



	                    }
	                    else
	                    {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: "+result.Message);
                                this.busy.IsBusy = false;
                                textbox.Text = "";

                            }));
	                    }


	                }
	                catch (Exception ex)
	                {
                        this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + ex.Message);
                            this.busy.IsBusy = false;
                            textbox.Text = "";
                        }));
	                }
	            }
                ).BeginInvoke(null, null);

	    }

	    private void VIPDelete_OnClick(object sender, RadRoutedEventArgs e)
	    {
           

                vipSellGridModel.Remove((VIPSellGridModel)viplistbox.SelectedItem);
	       
            
	    }


	    private void GouJiADD_OnClick(object sender, RoutedEventArgs e)
	    {
            var imei = this.GouJIIMEI.Text.Trim().ToUpper();
            new Action(() =>
            {
                this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    busy.IsBusy = true;
                }));
                try
                {
                    var result = wsClient.Main(this.MethodID_IMEIGetInfo, new List<object>() { imei, this.HallInfo.HallID });
                    if (result.ReturnValue)
                    {
                        API.Pro_ProInfo i = (Pro_ProInfo)result.Obj;
                        if (GoujiGridModels.Any(p => p.ProID == i.ProID))
                        {
                            if (GoujiGridModels.All(p => p.IMEI != imei))
                                this.GoujiGridModels.Add(new ProSellGridModel()
                                {
                                    ProID = i.ProID,
                                    ProCount = 1,
                                    IMEI = imei,

                                });
                            //                            this.JiPeiKaSellGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            //                                new Action(() => JiPeiKaSellGrid.Rebind()));
                        }
                        else
                        {
                            this.busy.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show(
                            Application.Current
                                .MainWindow,
                            "添加失败: 商品无权操作")));
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
                }));

            }).BeginInvoke(null, null);
	    }

	    private void BillIMEISearch_OnClick(object sender, RoutedEventArgs e)
	    {
            this.BillIMEI.Text = this.BillIMEI.Text.ToUpper();
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodID_IMEIGetInfo, new object[] { this.BillIMEI.Text.Trim(), this.HallInfo.HallID, 0 }, BillIMEISearchComp);
	  
	    }

        private void BillIMEISearchComp(object sender, MainCompletedEventArgs e)
	    {

            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    API.Pro_ProInfo i = (Pro_ProInfo)e.Result.Obj;

                    if (BillProInfos.Select(p => p.ProID).Contains(i.ProID))
                    {
                        var query = Store.BillFields.Where(p => p.ProID == i.ProID);
                        if (query.Any())
                        {
                            foreach (var proBillFieldInfo in query)
                            {

                                var num = proBillFieldInfo.BillFieldName.Replace("BillField", "");
                                var obj = this.FindName("FieldLabel" + num) as Label;
                                if (obj != null)
                                {
                                    obj.Content = proBillFieldInfo.BillFieldValue;
                                    var obj2 = this.FindName("StackPanel" + num) as StackPanel;
                                    if (obj2 != null)
                                    {
                                        obj2.Visibility = Visibility.Visible;
                                    }
                                }
                            }


                        }
                        var newinfo = new API.Pro_BillInfo();
                        newinfo.ProID = i.ProID;
                        newinfo.BillIMEI = BillIMEI.Text.Trim();
                        this.MainPanel.DataContext = newinfo;
                        this.BillIMEI.IsReadOnly = true;
                        
                        BillName.Text = i.ProName;
                    }
                    else
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "无该商品操作权限");
                    }

                }
                else
                {
                    //Dispatcher.BeginInvoke(() => { MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message); this.ProBusy.IsBusy = false; });
                    RadWindow.Alert(new DialogParameters() { Content = "查询失败: " + e.Result.Message, Header = "错误", });
                }
            }
            else
            {
                RadWindow.Alert(new DialogParameters() { Content = "查询失败: 服务器错误\n" + e.Error.Message, Header = "错误", });
                // Dispatcher.BeginInvoke(() =>
                //                    {
                //                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
                //                        this.ProBusy.IsBusy = false;
                //                    });
            }
	    }

	    private void _BillAdd_OnClick(object sender, RoutedEventArgs e)
	    {
            var model = this.MainPanel.DataContext as API.Pro_BillInfo;
            if (model == null) return;

            BillGridModels.Add(model);
            billclearform();
	    }

	    private void _BillCancel_OnClick(object sender, RoutedEventArgs e)
	    {
            billclearform();
	    }

	    private void _BillDel_OnClick(object sender, RoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            if (this.billGrid.SelectedItem != null)
            {
                this.BillGridModels.Remove((Pro_BillInfo)this.billGrid.SelectedItem);

            }
	    }
        private void billclearform()
        {
            this.MainPanel.DataContext = null;
            for (int i = 1; i < 11; i++)
            {
                var obj2 = this.FindName("StackPanel" + i) as StackPanel;
                if (obj2 != null)
                {
                    obj2.Visibility = Visibility.Collapsed;
                }
            }
            this.BillIMEI.IsReadOnly = false;
            this.BillIMEI.Text = "";
            BillPro = null;
            BillName.Text = "";
            MobileName.Text = "";
            ModelClass.Text = "";
        }

	    private Pro_ProInfo BillPro;
        private void BillMobileIMEISearch_Onclick(object sender, RoutedEventArgs e)
	    {
            var model = this.MainPanel.DataContext as API.Pro_BillInfo;
            if (model == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未验证合同串码");
                return;
            }
            this.MobileIMEI.Text = this.MobileIMEI.Text.ToUpper();


            PublicRequestHelp helper = new PublicRequestHelp(busy, Yanbao_CheckIMEI_MethodID, new object[] { this.MobileIMEI.Text.Trim() }, CheckIMEI_Completed);
	   
	    }

        private void CheckIMEI_Completed(object sender, MainCompletedEventArgs e)
        {

            this.busy.IsBusy = false;
            var model = this.MainPanel.DataContext as API.Pro_BillInfo_temp;
            if (model == null)
                return;
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
                            this.BillPro = Store.ProInfo.First(p => p.ProID == imeiinfo.ProID);

                            this.MobileName.Text = BillPro.ProName;


                            this.ModelClass.Text =
                                Store.ProClassInfo.First(p => p.ClassID == BillPro.Pro_ClassID).ClassName;
                            this.MobileIMEI.IsReadOnly = true;
                            model.MobileProID = this.BillPro.ProID;
                            model.MobileClassID = this.BillPro.Pro_ClassID;
                        }
                        else
                        {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 串码不存在");
                            this.MobileIMEI.IsReadOnly = false;
                            return;
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 数据错误\n" + ex.Message);
                        this.MobileIMEI.IsReadOnly = false;
                    }

                }


                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + e.Result.Message);
                    this.MobileIMEI.IsReadOnly = false;
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
            }
        }

	}
}
