using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml.Linq;
using Telerik.Windows.Controls;
using UserMS.MyControl;

namespace UserMS
{
    public partial class Index 
    {
        private NotifyMessageManager _notifyMessageMgr;
        private DateTime currentTime ;
        private int TimeItvOfRem;
        System.Timers.Timer timer;

        public Index()
        {
            if (Store.IsTesting)
            {
                Windows8Palette.Palette.AccentColor = Colors.Tomato;
                
            }
            else
            {
                Windows8Palette.Palette.AccentColor = Store.defaultcolor;
                
            }

            //获取消息提醒时间间隔
            var interval = from a in Store.Options
                           where a.ClassName == "TimeItvOfRem"
                           select a;
            if (interval.Count() > 0)
            {
                TimeItvOfRem = Convert.ToInt32(interval.First().Value)*1000;
            }
            else
            {
                TimeItvOfRem = 15000;
            }
            InitializeComponent();
            //API.UserMsServiceClient client = new API.UserMsServiceClient();
            //client.MainAsync(1,null);
            //client.MainCompleted += client_MainCompleted;
            
            //this.busy.IsBusy = true;
            InitMenu();

            #region 消息提示框初始化
            _notifyMessageMgr = new NotifyMessageManager
                (
                    Screen.Width,
                    Screen.Height,
                    250,
                    120,
                    this.tabMain
                );
            _notifyMessageMgr.Start();
            
            #endregion

            //获取用户默认打开页面
            PublicRequestHelp prh = new PublicRequestHelp(null, 273, new object []{ Store.LoginUserInfo.UserID },new EventHandler<API.MainCompletedEventArgs>(GetUserDefaultPageComplted));
        }

        private void GetUserDefaultPageComplted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<int> list = e.Result.Obj as List<int>;

                #region  初始化用户默认打开页面

                var menuinfo = from a in Store.MenuInfos
                               where list.Contains(a.MenuID)
                               select a;
                foreach (var item in menuinfo)
                {
                    MyTabItem tbi = new MyTabItem();
                    tbi.Name = item.MenuText;
                    MainTabHeader header = new MainTabHeader();

                    header.xButton.MouseLeftButtonDown += new MouseButtonEventHandler(xButton_MouseLeftButtonDown);
                    header.xButton.MouseMove += new MouseEventHandler(xButton_MouseMove);
                    header.xButton.MouseLeave += new MouseEventHandler(xButton_MouseLeave);
                    tbi.tabContextMenu.ItemClick += tabCloseMenu_ItemClick;

                    header.TextBlockHeader.Text = item.MenuText;
                    header.xButton.Tag = item.MenuText;

                    tbi.Header = header;

                    Frame newframe = new Frame();
                    newframe.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                    newframe.JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;
                    newframe.BorderThickness = new Thickness(1);
                    newframe.Source =new Uri( item.MenuValue,UriKind.Relative);

                    tbi.Content = newframe;

                    this.tabMain.Items.Add(tbi);
                    tbi.IsSelected = true;
                }
                #endregion 
            }
        }

        public void Searchevent(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(System.Windows.Application.Current.MainWindow,"text");
        }

        //void client_MainCompleted(object sender, API.MainCompletedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
        public Theme Theme
        {
            get { return (Theme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }  
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(Theme), typeof(Index), null); 
        

        // 当用户导航到此页面时执行。

        private void RadMenuItem_Click_Theme(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }
        private void InitMenu()
        {
           


            #region xml
            string xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<root>
  <node name=""库存管理"" address="""">
  <node name=""进货管理"" address="""">
   <node name=""串码类入库"" address="" /UserMS.Views.StockMS.EnteringStock.addfcmaddgoods.xaml"" />
   <node name=""非串码类入库"" address=""/Page2.xaml"" />
    <node name=""入库记录查询"" address=""""  />
  </node> 
  <node name=""商品调拨"" address="""">
   <node name=""串码类调拨"" address="""" />
   <node name=""非串码类调拨"" address=""/Page2.xaml"" />
    <node name=""调拨记录查询"" address=""""  />
  </node> 
  <node name=""串码借贷"" address="""">
   <node name=""串码类借贷"" address="""" />
   <node name=""非串码类借贷"" address=""/Page2.xaml"" />
    <node name=""无原单归还"" address=""""  />
  <node name=""借贷/归还记录查询"" address=""""  />
  </node> 
   <node name=""商品送修"" address="""">
   <node name=""新增送修"" address="""" />
   <node name=""无原单返库"" address=""/Page2.xaml"" /> 
  <node name=""送修/返库记录查询"" address=""""  />
  </node> 
  </node>

<node name=""销售管理"" address="""">
  <node name=""商品销售"" address="""">
   <node name=""新消费"" address="""" />
   <node name=""消费管理"" address=""/Page2.xaml"" />
  </node> 
  <node name=""商品退货"" address="""">
   <node name=""无原单退货"" address="""" />
   <node name=""无记录退货"" address=""/Page2.xaml"" />
  </node> 
  </node>

<node name=""会员管理"" address="""">
  <node name=""会员卡管理"" address="""">
   <node name=""卡类型维护"" address="""" />
   <node name=""卡维护记录"" address=""/Page2.xaml"" />
    <node name=""卡续期"" address=""""  />
   <node name=""挂失/解挂"" address=""""  />
   <node name=""暂停使用/启用"" address=""""  /> 
<node name=""换/补/升级卡"" address=""""  />
<node name=""退卡/退款"" address=""""  />
<node name=""卡充值"" address=""""  />
<node name=""储值调整"" address=""""  />
<node name=""设置/修改密码"" address=""""  />
  </node> 
  <node name=""会员营销管理"" address="""">
   <node name=""新卡入库"" address="""" />
   <node name=""会员注册"" address=""/Page2.xaml"" />
    <node name=""会员管理"" address=""""  />
<node name=""积分规则设置"" address=""""  />
<node name=""积分管理"" address=""""  />
  </node> 

  <node name=""会员服务管理"" address="""">
   <node name=""维修保养"" address="""" />
   <node name=""回访联系"" address=""/Page2.xaml"" />
    <node name=""投诉"" address=""""  />
  <node name=""短信助手"" address=""""  />
  </node> 
   <node name=""会员活动管理"" address="""">
   <node name=""发放代金券"" address="""" />
   <node name=""代金券管理"" address=""/Page2.xaml"" /> 
  <node name=""新增会员活动"" address=""""  />
<node name=""会员活动管理"" address=""""  />
  </node> 
  </node>

<node name=""统计报表与分析"" address="""">
 <node name=""库存类"" address="""">
  <node name=""库存汇总"" address=""""/>
   <node name=""各厅库存"" address="""" />
   <node name=""库存销售汇总"" address=""/Page2.xaml"" />
    <node name=""进销存报表"" address=""""  />
  </node> 

  <node name=""销售类"" address="""">
   <node name=""销售明细"" address="""" />
   <node name=""销售汇总"" address=""/Page2.xaml"" />
    <node name=""退货明细"" address=""""  />
  <node name=""借贷明细"" address=""""  />
  </node> 
  <node name=""会员类"" address="""">
   <node name=""会员业务明细"" address="""" />
   <node name=""会员服务明细"" address=""/Page2.xaml"" />
    <node name=""会员消费明细"" address=""""  />
  <node name=""消费分析"" address=""""  />
 <node name=""会员分析"" address=""""  />
 <node name=""服务分析"" address=""""  />
 <node name=""收款分析"" address=""""  />
 <node name=""优惠分析"" address=""""  />
  </node> 
   <node name=""商品送修"" address="""">
   <node name=""新增送修"" address="""" />
   <node name=""无原单返库"" address=""/Page2.xaml"" /> 
  <node name=""送修/返库记录查询"" address=""""  />
  </node> 
  </node>
</root>

";
            #endregion


            //xml = Store.LoginUserInfo.AduitLimit;

            API.Sys_RoleInfo role = Store.RoleInfo.First(e => Store.LoginUserInfo.RoleID != null && e.RoleID == (int) Store.LoginUserInfo.RoleID);
            //Store.LoginUserInfo.RoleInfo = role;
            Store.LoginRoleInfo = role;

            xml = role.MenuXML;
            XElement root = XElement.Parse(xml);
            //XElement root = XElement.Parse
            var result = this.LoadData(root);

            //this.LeftRadPanelBar.ItemsSource= result;
            this.outlookbar.ItemsSource= result;
            List<TreeViewModel> list = new List<TreeViewModel>();
            //list.Add(new TreeViewModel() {  Title="常用功能 >>"});
            list.AddRange(result);
            //this.allwaysBar.ItemsSource = list;
        }
        private List<TreeViewModel> LoadData(XElement root)
        {
            if (root==null)
            {
                return null;
            }

            var items = from n in root.Elements("node")
                        select new TreeViewModel
                        {
                            Title = (string)n.Attribute("name"),
                            Address = new Uri((string)n.Attribute("address"), UriKind.Relative),
                            cando = (string)n.Attribute("address") != "",
                            Children = this.LoadData(n)
                        };

            return items.ToList();
        }

         
        private void BasePage_Loaded_1(object sender, RoutedEventArgs e)
        {
            Logger.StatusBar = this.errorMsg;
            Logger.Log("载入完毕. 欢迎使用渠道运营部综合管理系统.");
            this.CurrentUser.Text = "当前用户："+Store.LoginUserInfo.UserName;

            currentTime = DateTime.Now;
            timer = new System.Timers.Timer();
            timer.Interval = TimeItvOfRem;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                API.WebReturn ret = Store.wsclient.Main(259,
                    new List<object>()
                    {
                        currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    });
            currentTime = DateTime.Now;

            if (ret.ReturnValue)
            {
                List<API.GetUserRemindListResult> list = ret.Obj as List<API.GetUserRemindListResult>;
                if (list.Count > 0)
                {
                    Show delShow = ShowMessage;
                    foreach (var item in list)
                    {
                        //字符串过长则截取字符串换行显示
                        if (item.Note.Length > 10)
                        {
                            int count = item.Note.Length / 10 + 1;

                            string temp="";

                            for (int i = 0; i < count; i++)
                            {
                                int subc = item.Note.Length - i * 10;
                                temp += item.Note.Substring(i*10, subc>10?10:subc) + "\r\n" ;
                            }
                            item.Note = temp;
                        }
                        this.Dispatcher.BeginInvoke(delShow, item.MenuValue,item.MenuText, item.Note,item.Name);
                        Thread.Sleep(1000);
                    }
                }
                }
            }
            catch
            {
                
            }
        }

        private void OpenPage() { }

        private void LeftRadPanelBar_Loaded(object sender, RoutedEventArgs e)
        {
            //foreach (RadTreeViewItem item in this.LeftRadPanelBar.Items)
            //{
            //    RadTreeView tree = new RadTreeView();
            //    tree.ItemsSource = ((TreeViewModel)item.DataContext).Children;
            //    item.Items.Add(tree);

            //}
        }

        private void RadTreeView_ItemClick_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

//            if (e.Source != typeof(RadTreeViewItem))
//                return;
            TreeViewModel m = (TreeViewModel)((Telerik.Windows.Controls.RadTreeViewItem)e.OriginalSource).DataContext;
            if (!m.cando)
                return;
            int count = this.tabMain.Items.Count;

            if (count > 1)
            {

                List<object> items = new List<object>();
                foreach (var item in this.tabMain.Items)
                {
                    items.Add(item);
                }

                var query = items.Where(p => ((RadTabItem)p).Name == m.Title);
                if (query.Count() > 0)//已打开
                {
                    ((RadTabItem)query.First()).IsSelected = true;
                    return;
                }
                if (count >= 11)//打开太多
                {
                    this.errorMsg.Text = "您打开的页面的太多了，请先关闭一些";
                    return;
                }

            }

            try
            {

                MyTabItem tbi = new MyTabItem();

                tbi.Name = m.Title;
                MainTabHeader h = new MainTabHeader();

                h.xButton.MouseLeftButtonDown += new MouseButtonEventHandler(xButton_MouseLeftButtonDown);
                h.xButton.MouseMove += new MouseEventHandler(xButton_MouseMove);
                h.xButton.MouseLeave += new MouseEventHandler(xButton_MouseLeave);
                //h.gridHeader.MouseLeftButtonDown += new MouseButtonEventHandler(Header_MouseLeftButtonDown);
                //h.gridHeader.MouseLeftButtonUp += new MouseButtonEventHandler(gridHeader_MouseLeftButtonUp);
                tbi.tabContextMenu.ItemClick += tabCloseMenu_ItemClick;


                h.TextBlockHeader.Text = m.Title;
                h.xButton.Tag = m.Title;


                tbi.Header = h;

                Frame newframe = new Frame();
                newframe.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                newframe.JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;
                newframe.BorderThickness=new Thickness(1);
                newframe.Source = m.Address;
                
                tbi.Content = newframe;

                this.tabMain.Items.Add(tbi);
                tbi.IsSelected = true;
            }
            catch (Exception ex) {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"页面地址有误");
            }
        }

        private void tabCloseMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadMenuItem menu = (RadMenuItem)e.Source;
            MyTabItem current = (MyTabItem)((RadContextMenu)((RadMenuItem)e.Source).Parent).UIElement;
                //((UserMS.MyControl.MainTabHeader)((RadContextMenu)((RadMenuItem)e.Source).Parent).UIElement).GetVisualParent<RadTabItem>();
            
            if (menu.Name == "CloseOthers")
            {
                RemoveTabItem(this.tabMain, new List<RadTabItem>() { (RadTabItem)this.tabMain.Items[0], current });
            }
            else if (menu.Name == "CloseAll")
            {
                RemoveTabItem(this.tabMain, new List<RadTabItem>() { (RadTabItem)this.tabMain.Items[0] });
            }
            else if (menu.Name == "CloseCurrent")
            {
                this.tabMain.Items.Remove(current);
            }
        }
 

        private void xButton_MouseMove(object sender, MouseEventArgs e)
        {
//            TextBlock b = sender as TextBlock;
//            
//            b.Foreground = new SolidColorBrush(Colors.Red);


        }

        private void xButton_MouseLeave(object sender, MouseEventArgs e)
        {
//            TextBlock b = sender as TextBlock;
//            
//
//            ColorConverter convtr = new ColorConverter();
//            b.Foreground = new SolidColorBrush(MyColors.GetColorFromString("FFBFBFBF")); 

        }
        private void xButton_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //Button b = sender as Button;
            Image b = sender as Image;
            for (int i = 1; i < tabMain.Items.Count; i++)
            {
                RadTabItem tb = (RadTabItem)tabMain.Items[i];

                MainTabHeader tch = (MainTabHeader)tb.Header;
                if (tch.xButton.Equals(b))
                {
                    tabMain.Items.RemoveAt(i);
                }
            }
        }

        private void RemoveTabItem(RadTabControl tab, List<RadTabItem> ExceptItem)
        { 
            for(int k=tab.Items.Count-1;k>0;k--)
            {
                RadTabItem i = (RadTabItem)tab.Items[k];
                if (ExceptItem.Contains(i))
                    continue;  
                tab.Items.Remove(i);
            }
        }

        private void Office_Black_Click(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            switch (r.Content+"")
            {
                case "Office Black": StyleManager.ApplicationTheme= new Office_BlackTheme();  break;
                case "Office Blue": StyleManager.ApplicationTheme=( new Office_BlueTheme()); break;
                case "Office Silver": this.Theme= new Office_SilverTheme(); break;
                case "Expression Dark": this.Theme= new Expression_DarkTheme(); break;
                case "Summer": this.Theme= new SummerTheme(); break;
                case "Vista": this.Theme= new VistaTheme(); break;
                case "Windows 7": this.Theme= new  Windows7Theme(); break;
                case "Transparent ": this.Theme= new TransparentTheme(); break;
                case "Windows8": this.Theme= new Windows8Theme(); break;
                case "Windows8Touch": this.Theme= new Windows8TouchTheme(); break;
                //case "Metro (obsolete)": this.Theme= new  m(); break;
                default: break;
            }
        }

        private void RadListBoxItem_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
//            if (sender.GetType() != typeof(RadListBoxItem))
//                return;
            TreeViewModel m = (TreeViewModel)((Telerik.Windows.Controls.RadListBoxItem)sender).DataContext;
            if (!m.cando)
                return;
            int count = this.tabMain.Items.Count;

            if (count > 1)
            {
                List<object> items = new List<object>();
                foreach (var item in this.tabMain.Items)
                {
                    items.Add(item);
                }

                var query = items.Where(p => ((RadTabItem)p).Name == m.Title);
                //var query = this.tabMain.Items.AsQueryable().Where(p => ((RadTabItem)p).Name == m.Title);
                if (query.Count() > 0)//已打开
                {
                    ((RadTabItem)query.First()).IsSelected = true;
                    return;
                }
                if (count >= 11)//打开太多
                {
                    this.errorMsg.Text = "您打开的页面的太多了，请先关闭一些";
                    return;
                }

            }




            MyTabItem tbi = new MyTabItem();

            tbi.Name = m.Title;
            MainTabHeader h = new MainTabHeader();
            
            h.xButton.MouseLeftButtonDown += new MouseButtonEventHandler(xButton_MouseLeftButtonDown);
            h.xButton.MouseMove += new MouseEventHandler(xButton_MouseMove);
            h.xButton.MouseLeave += new MouseEventHandler(xButton_MouseLeave);
            //h.gridHeader.MouseLeftButtonDown += new MouseButtonEventHandler(Header_MouseLeftButtonDown);
            //h.gridHeader.MouseLeftButtonUp += new MouseButtonEventHandler(gridHeader_MouseLeftButtonUp);
            tbi.tabContextMenu.ItemClick += tabCloseMenu_ItemClick;


            h.TextBlockHeader.Text = m.Title;
            h.xButton.Tag = m.Title;
            

            tbi.Header = h;

            Frame newframe = new Frame();
            newframe.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            newframe.JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;

            newframe.Source = m.Address;
            newframe.BorderThickness = new Thickness(1);
            tbi.Content = newframe;


            this.tabMain.Items.Add(tbi);
            tbi.IsSelected = true;
        }

        private void BasePage_Unloaded_1(object sender, RoutedEventArgs e)
        {
            Logger.StatusBar = null;
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Dispose();
            }
        }

        private void Logo_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var l = new List<TreeViewModel>();
            foreach (var proTypeInfo in Store.ProTypeInfo)
            {
                var n = new TreeViewModel();
                n.ID = proTypeInfo.TypeID.ToString();
                n.Title = proTypeInfo.TypeName;
                l.Add(n);
            }
            MultSelecter m = new MultSelecter(
                l,
                Store.ProInfo, "Pro_TypeID", "ProName",
                new string[] { "ProID", "Pro_TypeID", "ProName" },
            new string[] { "ProID", "Pro_TypeID", "ProName" });
            m.Show();

        }

        private void Louout_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Login();
            
        }

        private void Lock_OnClick(object sender, RoutedEventArgs e)
        {
            LockChild l=new LockChild();
            l.ShowDialog();

        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            
            int count = this.tabMain.Items.Count;

            if (count > 1)
            {
                List<object> items = new List<object>();
                foreach (var item in this.tabMain.Items)
                {
                    items.Add(item);
                }

                var query = items.Where(p => ((RadTabItem)p).Name == this.SetPassWord.Name);
                //var query = this.tabMain.Items.AsQueryable().Where(p => ((RadTabItem)p).Name == m.Title);
                if (query.Count() > 0)//已打开
                {
                    ((RadTabItem)query.First()).IsSelected = true;
                    return;
                }
                if (count >= 11)//打开太多
                {
                    this.errorMsg.Text = "您打开的页面的太多了，请先关闭一些";
                    return;
                }

            }


            try
            {

                MyTabItem tbi = new MyTabItem();

                tbi.Name = this.SetPassWord.Name;
                MainTabHeader h = new MainTabHeader();

                h.xButton.MouseLeftButtonDown += new MouseButtonEventHandler(xButton_MouseLeftButtonDown);
                h.xButton.MouseMove += new MouseEventHandler(xButton_MouseMove);
                h.xButton.MouseLeave += new MouseEventHandler(xButton_MouseLeave);
                //h.gridHeader.MouseLeftButtonDown += new MouseButtonEventHandler(Header_MouseLeftButtonDown);
                //h.gridHeader.MouseLeftButtonUp += new MouseButtonEventHandler(gridHeader_MouseLeftButtonUp);
                tbi.tabContextMenu.ItemClick += tabCloseMenu_ItemClick;


                h.TextBlockHeader.Text = "修改密码";
                h.xButton.Tag = "修改密码";


                tbi.Header = h;

                Frame newframe = new Frame();
                newframe.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                newframe.JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;

                newframe.Source = new Uri("/" + this.SetPassWord.Name + ".xaml", UriKind.Relative);

                newframe.BorderThickness = new Thickness(1);
                tbi.Content = newframe;


                this.tabMain.Items.Add(tbi);
                tbi.IsSelected = true;
            }
            catch (Exception ex) { 
            
            }
        }


        #region 弹出消息框

        private delegate void Show(string url, string note, string menutext, string title);
       // private event Show ShowEvent;
        private void ShowMessage(string url,string menutext, string msgnote, string title)
        {
            #region 新建消息框中的消息
            NotifyMessage msg = null;
            msg = new NotifyMessage(url, menutext, msgnote, title, 
                this.xButton_MouseLeftButtonDown,
                this.xButton_MouseMove,
                this.xButton_MouseLeave,
                this.tabCloseMenu_ItemClick,
                                        () =>
                                        OpenPage());
            _notifyMessageMgr.EnqueueMessage(msg);
            #endregion
        }
        #endregion

        private void ReloadButton_click(object sender, RoutedEventArgs e)
        {
          PublicRequestHelp.UpdateInitData(this.busy);
        }
    }
}
