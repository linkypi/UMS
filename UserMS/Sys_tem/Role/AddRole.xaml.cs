using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace UserMS.Sys_tem.Role
{
    public partial class AddRole : BasePage
    {
        private API.Sys_RoleInfo _NewRole=new API.Sys_RoleInfo();
        /// <summary>
        /// 新增的角色
        /// </summary>
        public API.Sys_RoleInfo NewRole
        {
            get { return _NewRole; }
            set { _NewRole = value; }
        }

        private List<API.Sys_Role_Menu_HallInfo> _NewMenuHall = new List<API.Sys_Role_Menu_HallInfo>();
        /// <summary>
        /// 菜单-仓库列表
        /// </summary>
        public List<API.Sys_Role_Menu_HallInfo> NewMenuHall
        {
            get { return _NewMenuHall; }
            set { _NewMenuHall = value; }
        }

        private List<API.Sys_Role_Menu_ProInfo> _NewMenuPro = new List<API.Sys_Role_Menu_ProInfo>();
        /// <summary>
        /// 菜单-商品列表
        /// </summary>
        public List<API.Sys_Role_Menu_ProInfo> NewMenuPro
        {
            get { return _NewMenuPro; }
            set { _NewMenuPro = value; }
        }


        private List<API.Sys_MenuInfo> _NewMenu=new List<API.Sys_MenuInfo>();
        /// <summary>
        /// 菜单
        /// </summary>
        public List<API.Sys_MenuInfo> NewMenu
        {
            get { return _NewMenu; }
            set { _NewMenu = value; }
        }

        

        public AddRole()
        {
            InitializeComponent();

            InitMenu();
            InitHall(new List<API.Sys_Role_Menu_HallInfo>());
            InitPro(new List<API.Sys_Role_Menu_ProInfo>());

             
        }

        // 当用户导航到此页面时执行。
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;

        }
        #region 初始化菜单
        /// <summary>
        /// 初始化全部菜单 分配权限的 和 分配结果的
        /// </summary>
        private void InitMenu()
        {
            this.RadTreeView1.Items.Clear(); 
            NewMenu.Clear();
            this.NewRole.Sys_Role_MenuInfo = new List<API.Sys_Role_MenuInfo>();
            List<API.Sys_MenuInfo> menuList = (from b in Store.MenuInfos
                                                  where (b.Parent==0 || b.Parent==null) && b.Flag==true
                                                   select b).ToList();
            foreach (API.Sys_MenuInfo menu in menuList)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                item.Header = menu.MenuText;
                API.Sys_MenuInfo role_menu = new API.Sys_MenuInfo() { HasHallRole=menu.HasHallRole,HasProRole=menu.HasProRole,Flag=false};
                role_menu.MenuID = menu.MenuID;
                item.DataContext = role_menu;
                NewMenu.Add(role_menu);
                
                


                RadTreeViewItem item2 = new RadTreeViewItem();
                item2.Header = menu.MenuText;

                item2.DataContext = role_menu;

                menu.Menu = InitMenu(menu.MenuID, menuList, item, item2);


                RadTreeView1.Items.Add(item); 
            } 
        }
        private List<API.Sys_MenuInfo> InitMenu(int ParentMenuID, List<API.Sys_MenuInfo> menuList,RadTreeViewItem itemParent,RadTreeViewItem itemParent_menu_tree)
        {
            List<API.Sys_MenuInfo> menuList_temp = (from b in Store.MenuInfos
                                                    where b.Parent == ParentMenuID && b.Flag==true
                                                    select b).ToList();
            foreach (API.Sys_MenuInfo menu in menuList_temp)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                item.Header = menu.MenuText;
                API.Sys_MenuInfo role_menu = new API.Sys_MenuInfo() { HasHallRole = menu.HasHallRole, HasProRole = menu.HasProRole, Flag = false };
                role_menu.MenuID = menu.MenuID; 
                item.DataContext = role_menu;
                NewMenu.Add(role_menu);

                RadTreeViewItem item2 = new RadTreeViewItem();
                item2.Header = menu.MenuText;

                item2.DataContext = role_menu;

                menu.Menu = InitMenu(menu.MenuID, menuList_temp,item,item2);
                itemParent.Items.Add(item);
                itemParent_menu_tree.Items.Add(item2); 
            }
            return menuList_temp;
        }
       
      
        #endregion

        #region 初始化全部仓库
        private void InitHall(List<API.Sys_Role_Menu_HallInfo> AllCheckHall)
        {
            this.RadTreeView2.Items.Clear();
            this.NewRole.Sys_Role_Menu_HallInfo = new List<API.Sys_Role_Menu_HallInfo>();

            var hall_unoin_checkhall = from b in Store.ProHallInfo
                                       where b.Flag == true
                                       join c in AllCheckHall
                                       on b.HallID equals c.HallID
                                       into temp
                                       from c1 in temp.DefaultIfEmpty()
                                       group c1 by new { b } into temp2
                                       select new
                                       {
                                           Pro_HallInfo = temp2.Key.b,
                                           Sys_Role_Menu_HallInfo = temp2.Where(p => p != null)
                                       };

            var Area_union_Hall = from b in Store.AreaInfo
                                  where b.Flag == true
                                  join c in hall_unoin_checkhall
                                   on b.AreaID equals c.Pro_HallInfo.AreaID
                                   into temp
                                  from c1 in temp
                                  group c1 by new { b } into temp2
                                  select new
                                  {
                                      AreaID = temp2.Key.b.AreaID,
                                      AreaName = temp2.Key.b.AreaName,
                                      Area_MenuHall = temp2.ToList()
                                  };



            foreach (var area in Area_union_Hall)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                item.Header = area.AreaName;
                bool HasChildChecked = false;
                bool IsCheckALL = true;
                foreach (var hall in area.Area_MenuHall)
                {
                    API.Sys_Role_Menu_HallInfo menu_hall = new API.Sys_Role_Menu_HallInfo();
                    menu_hall.HallID = hall.Pro_HallInfo.HallID;
                    RadTreeViewItem itemchild = new RadTreeViewItem();
                    itemchild.Header = hall.Pro_HallInfo.HallName;
                    itemchild.Tag = hall.Pro_HallInfo.HallID;


                    if (hall.Sys_Role_Menu_HallInfo.Count() != 0)
                    {

                        //itemchild.DataContext = hall.Sys_Role_Menu_HallInfo;
                        itemchild.CheckState = System.Windows.Automation.ToggleState.On;
                        HasChildChecked = true;
                    }
                    else
                    {
                        IsCheckALL = false;
                    }

                    //this.NewMenuHall.Add(menu_hall);


                    item.Items.Add(itemchild);
                }

                if (IsCheckALL) item.CheckState = System.Windows.Automation.ToggleState.On;
                else if (HasChildChecked) item.CheckState = System.Windows.Automation.ToggleState.Indeterminate;
                this.RadTreeView2.Items.Add(item);
            }
        }
        #endregion

        #region 初始化全部商品
        private void InitPro(List<API.Sys_Role_Menu_ProInfo> AllCheckedPro)
        {
            this.RadTreeView3.Items.Clear();

            this.NewRole.Sys_Role_Menu_ProInfo = new List<API.Sys_Role_Menu_ProInfo>();
            //List<API.Pro_ClassInfo> classList = (from b in Store.ProClassInfo
            //                                   where b.IsDeleted != true
            //                                   select b).ToList();
            //List<API.Pro_ProInfo> ProList = (from b in Store.ProInfo
            //                                   select b).ToList();

            var pro_union_menuPro = from b in Store.ProClassInfo
                                    join c in AllCheckedPro
                                    on b.ClassID equals c.ClassID
                                    into temp2
                                    from c1 in temp2.DefaultIfEmpty()
                                    select new
                                    {
                                        ProClassInfo = b,
                                        Sys_Role_Menu_ProInfo = c1
                                    };

            //var class_union_pro = from b in Store.ProClassInfo
            //                      where b.IsDeleted !=true
            //                      //join c in pro_union_menuPro
            //                      //on b.ClassID equals c.Pro_ProInfo.Pro_ClassID
            //                      //into temp
            //                      //from c1 in temp
            //                      //group c1 by new { b } into temp2
            //                      select new API.Pro_ClassInfo { 
            //                        ClassID=b.ClassID,
            //                        ClassName=b.ClassName
            //                      };


            foreach (var c in pro_union_menuPro)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                item.Tag = c.ProClassInfo.ClassID ;
                item.Header = c.ProClassInfo.ClassName;
                //bool HasChildChecked=false;
                //bool IsCheckALL = true;
                //foreach (var Pro in c.ProInfo_MenuProInfo)
                //{
                //    API.Sys_Role_Menu_ProInfo menu_pro = new API.Sys_Role_Menu_ProInfo();
                //    menu_pro.ProID = Pro.Pro_ProInfo.ProID;
                //    RadTreeViewItem itemchild = new RadTreeViewItem();
                //    itemchild.Header = Pro.Pro_ProInfo.ProName +";"+Pro.Pro_ProInfo.ProFormat;
                //    itemchild.Tag = Pro.Pro_ProInfo.ProID;
                //    if (Pro.Sys_Role_Menu_ProInfo != null)
                //    {

                //        itemchild.DataContext = Pro.Sys_Role_Menu_ProInfo;
                //        itemchild.CheckState = System.Windows.Automation.ToggleState.On;
                //        HasChildChecked = true;
                //    }
                //    else {
                //        IsCheckALL = false;
                //    }
                     
                //    this.NewMenuPro.Add(menu_pro);
                //    item.Items.Add(itemchild);
                    
                //}
                    if (c.Sys_Role_Menu_ProInfo != null)
                    {
                        item.CheckState = System.Windows.Automation.ToggleState.On;
                        item.DataContext = c.Sys_Role_Menu_ProInfo;
                    }
                else  item.CheckState = System.Windows.Automation.ToggleState.Off;
                this.RadTreeView3.Items.Add(item);
            }
        }
        #endregion

   

        #region 全选 反选 分配结果
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (RadTreeViewItem item in this.RadTreeView1.Items)
            {
                item.CheckState = System.Windows.Automation.ToggleState.On;
            }
            //((RadTreeViewItem)this.RadTreeView1.Items[5]).CheckState = System.Windows.Automation.ToggleState.On;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (RadTreeViewItem item in this.RadTreeView1.Items)
            {
                item.CheckState = System.Windows.Automation.ToggleState.Off;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (this.RadTreeView2.IsEnabled == false) return;
            foreach (RadTreeViewItem item in this.RadTreeView2.Items)
            {
                item.CheckState = System.Windows.Automation.ToggleState.On;
            }
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (this.RadTreeView2.IsEnabled == false) return;
            foreach (RadTreeViewItem item in this.RadTreeView2.Items)
            {
                item.CheckState = System.Windows.Automation.ToggleState.Off;
            }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (this.RadTreeView3.IsEnabled == false) return;
            foreach (RadTreeViewItem item in this.RadTreeView3.Items)
            {
                item.CheckState = System.Windows.Automation.ToggleState.On;
            }
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (this.RadTreeView3.IsEnabled == false) return;
            foreach (RadTreeViewItem item in this.RadTreeView3.Items)
            {
                item.CheckState = System.Windows.Automation.ToggleState.Off;
            }
        }
        #endregion

        #region 复选框 checkbox
        
        
        private void RadTreeView1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            #region 绑定商品列表 并且选中已经选择的商品
            API.Sys_MenuInfo menu = (API.Sys_MenuInfo)((RadTreeViewItem)e.AddedItems[0]).DataContext;
            if (menu.Sys_Role_Menu_ProInfo == null)
                menu.Sys_Role_Menu_ProInfo = new List<API.Sys_Role_Menu_ProInfo>();
            InitPro(menu.Sys_Role_Menu_ProInfo);
            //if (menu.Sys_Role_Menu_HallInfo == null)
            //    menu.Sys_Role_Menu_HallInfo = new List<API.Sys_Role_Menu_HallInfo>();
            //if(((RadTreeViewItem)e.AddedItems[0]).CheckState==System.Windows.Automation.ToggleState.Off)
            //    ((RadTreeViewItem)e.AddedItems[0]).CheckState = System.Windows.Automation.ToggleState.On;
            //InitHall(menu.Sys_Role_Menu_HallInfo);

            //this.RadTreeView2.IsEnabled = menu.HasHallRole == true ? true : false;

            this.RadTreeView3.IsEnabled = menu.HasProRole == true ? true : false;
           
            #endregion

        }
        /// <summary>
        /// 选中菜单的checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadTreeView1_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            API.Sys_MenuInfo menu = (API.Sys_MenuInfo)item.DataContext; 
            switch (item.CheckState)
            {
                case System.Windows.Automation.ToggleState.On: menu.Flag = true; break;
                case System.Windows.Automation.ToggleState.Indeterminate: menu.Flag = null ; break;
                default: menu.Flag = false; break;
            }
        }
        /// <summary>
        /// 反选中菜单的checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadTreeView1_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            API.Sys_MenuInfo menu = (API.Sys_MenuInfo)item.DataContext;
            //menu.Sys_Role_Menu_HallInfo = null;
            menu.Sys_Role_Menu_ProInfo = null;
            switch (item.CheckState)
            {
                case System.Windows.Automation.ToggleState.On: menu.Flag = true; break;
                case System.Windows.Automation.ToggleState.Indeterminate: menu.Flag = null ; break;
                default: menu.Flag = false; break;
            }
            if (item.IsSelected)
            {
                item.IsSelected = false;
                
                //InitHall(new List<API.Sys_Role_Menu_HallInfo>());
                InitPro(new List<API.Sys_Role_Menu_ProInfo>());
                //this.RadTreeView2.IsEnabled = false;
                this.RadTreeView3.IsEnabled = false;
            }
        }


        /// <summary>
        /// 选中Menu_Tree菜单的checkbox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Tree_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            API.Sys_MenuInfo menu = (API.Sys_MenuInfo)item.DataContext;
            switch (item.CheckState)
            {
                case System.Windows.Automation.ToggleState.On: menu.Flag = true; break;
                case System.Windows.Automation.ToggleState.Indeterminate: menu.Flag = null; break;
                default: menu.Flag = false; break;
            }
        }
        /// <summary>
        /// 反选中Menu_Tree菜单的checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Tree_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            API.Sys_MenuInfo menu = (API.Sys_MenuInfo)item.DataContext;
            switch (item.CheckState)
            {
                case System.Windows.Automation.ToggleState.On: menu.Flag = true; break;
                case System.Windows.Automation.ToggleState.Indeterminate: menu.Flag = null; break;
                default: menu.Flag = false; break;
            }
        }
/// <summary>
/// 选中商品
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void RadTreeView3_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.RadTreeView1.SelectedItem == null) return;
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            if (item.Tag!=null)
            {
                RadTreeViewItem MenuItem=(RadTreeViewItem) this.RadTreeView1.SelectedItem;
                API.Sys_MenuInfo menu=(API.Sys_MenuInfo)MenuItem.DataContext;
                API.Sys_Role_Menu_ProInfo menuPro = new API.Sys_Role_Menu_ProInfo();
                menuPro.ClassID = Convert.ToInt32( item.Tag);
                menuPro.MenuID = menu.MenuID;
                item.DataContext = menuPro;
                menu.Sys_Role_Menu_ProInfo.Add(menuPro);
            }
        }
        /// <summary>
        /// 反选择商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadTreeView3_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.RadTreeView1.SelectedItem == null) return;
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            if (item.Tag != null)
            {
                RadTreeViewItem MenuItem = (RadTreeViewItem)this.RadTreeView1.SelectedItem;
                API.Sys_MenuInfo menu = (API.Sys_MenuInfo)MenuItem.DataContext;
                menu.Sys_Role_Menu_ProInfo.Remove((API.Sys_Role_Menu_ProInfo)item.DataContext);
                item.DataContext = null; 
            }
        }
        /// <summary>
        /// 选中仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadTreeView2_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //if (this.RadTreeView1.SelectedItem == null) return;
            //RadTreeViewItem item = e.Source as RadTreeViewItem;
            //if (item.Tag != null)
            //{
            //    RadTreeViewItem MenuItem = (RadTreeViewItem)this.RadTreeView1.SelectedItem;
            //    API.Sys_MenuInfo menu = (API.Sys_MenuInfo)MenuItem.DataContext;
            //    API.Sys_Role_Menu_HallInfo menuPro = new API.Sys_Role_Menu_HallInfo();
            //    menuPro.HallID = item.Tag+"";
            //    menuPro.MenuID = menu.MenuID;
            //    item.DataContext = menuPro;
            //    menu.Sys_Role_Menu_HallInfo.Add(menuPro);
            //}
        }
        /// <summary>
        /// 反选中仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadTreeView2_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //if (this.RadTreeView1.SelectedItem == null) return;
            //RadTreeViewItem item = e.Source as RadTreeViewItem;
            //if (item.Tag != null)
            //{
            //    RadTreeViewItem MenuItem = (RadTreeViewItem)this.RadTreeView1.SelectedItem;
            //    API.Sys_MenuInfo menu = (API.Sys_MenuInfo)MenuItem.DataContext;
            //    menu.Sys_Role_Menu_HallInfo.Remove((API.Sys_Role_Menu_HallInfo)item.DataContext);
            //    item.DataContext = null;
            //}
        }
        #endregion

        private void BasePage_Loaded_1(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            
        }
        #region 菜单保存  取消
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MessageBoxResult r = (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定要取消吗？", "", MessageBoxButton.OKCancel));
            if(r==MessageBoxResult.Cancel) return;
            InitMenu();
            InitHall(new List<API.Sys_Role_Menu_HallInfo>());
            InitPro(new List<API.Sys_Role_Menu_ProInfo>());
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(this.roleName.Text.Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"角色名称必填？");
                return;
            }
            
            MessageBoxResult r = (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定要保存吗？", "", MessageBoxButton.OKCancel));
            if (r == MessageBoxResult.Cancel) return;
            this.NewRole.Sys_Role_MenuInfo = new List<API.Sys_Role_MenuInfo>();
            this.NewRole.Sys_Role_Menu_ProInfo = new List<API.Sys_Role_Menu_ProInfo>();
            this.NewRole.Sys_Role_Menu_HallInfo = new List<API.Sys_Role_Menu_HallInfo>();
            foreach (API.Sys_MenuInfo menu in this.NewMenu)
            {
                if (menu.Flag != false)
                {
                    API.Sys_Role_MenuInfo role_menu = new API.Sys_Role_MenuInfo();
                    role_menu.MenuID = menu.MenuID;
                    role_menu.IsChecked = menu.Flag;
                    this.NewRole.Sys_Role_MenuInfo.Add(role_menu);

                
                    //if (menu.Sys_Role_Menu_HallInfo != null && menu.Sys_Role_Menu_HallInfo.Count > 0)
                    //    this.NewRole.Sys_Role_Menu_HallInfo.AddRange(menu.Sys_Role_Menu_HallInfo);

                    if (menu.Sys_Role_Menu_ProInfo != null && menu.Sys_Role_Menu_ProInfo.Count > 0)
                        this.NewRole.Sys_Role_Menu_ProInfo.AddRange(menu.Sys_Role_Menu_ProInfo);
                }
            }
            foreach (RadTreeViewItem hall in this.RadTreeView2.Items)
            {
                if (hall.CheckState != System.Windows.Automation.ToggleState.Off)
                {
                    foreach (RadTreeViewItem child in hall.Items)
                    {
                        if (child.CheckState == System.Windows.Automation.ToggleState.On)
                        {
                            this.NewRole.Sys_Role_Menu_HallInfo.Add(new API.Sys_Role_Menu_HallInfo()
                            {
                                HallID = child.Tag + ""

                            });
                        }
                    }
                }
            }
            this.NewRole.RoleName = this.roleName.Text.Trim();
            this.NewRole.Note = this.note.Text.Trim();
            PublicRequestHelp req = new PublicRequestHelp(this.busy, MethodIDInfo.Sys_Role_Add, new object[]{this.NewRole}, AddRole_MainCompleted);
        }
        /// <summary>
        /// 保存后返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddRole_MainCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败，可能原因网络问题");
                Logger.Log("保存失败，可能原因网络问题");
                return;
            }
            API.WebReturn r = e.Result;
            if (!r.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败，"+r.Message);
                Logger.Log("保存失败，" + r.Message);
                return;
                
            }
            MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
            Logger.Log("保存成功");

            InitMenu();
            InitHall(new List<API.Sys_Role_Menu_HallInfo>());
            InitPro(new List<API.Sys_Role_Menu_ProInfo>());
            
        }
        #endregion

         

        

       

      
        
        

       

        

       

        









    }
}
