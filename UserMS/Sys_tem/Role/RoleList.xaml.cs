using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace UserMS.Sys_tem.Role
{
    public partial class RoleList : BasePage
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



        public RoleList()
        {
            InitializeComponent();
            InitMenu(new List<API.Sys_MenuInfo>());
            InitHall(new List<API.Sys_Role_Menu_HallInfo>());
            InitPro(new List<API.Sys_Role_Menu_ProInfo>());
            #region 取第一页的数据
            API.ReportPagingParam pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>()
            };
            this.InitPageEntity(MethodIDInfo.Sys_Role_GetList, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }

        // 当用户导航到此页面时执行。
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;

        }
        #region 初始化菜单

        //private void InitMenu()
        //{
        //    List<API.Sys_MenuInfo> menuList = (from b in Store.MenuInfos
        //                                       where (b.Parent == 0 || b.Parent == null) && b.Flag == true
        //                                       select new API.Sys_MenuInfo
        //                                       {
        //                                           Flag = false,
        //                                           HasHallRole = b.HasHallRole,
        //                                           HasProRole = b.HasProRole,
        //                                           Menu = b.Menu,
        //                                           MenuID = b.MenuID,
        //                                           MenuImg = b.MenuImg,
        //                                           MenuText = b.MenuText,
        //                                           MenuValue = b.MenuValue,
        //                                           Note = b.Note,
        //                                           Order = b.Order,
        //                                           Parent = b.Parent
        //                                       }).ToList();
        //    InitMenu(menuList);
        //}

        /// <summary>
        /// 初始化全部菜单
        /// </summary>
        private void InitMenu(List<API.Sys_MenuInfo> role_menus)
        {
            this.RadTreeView1.Items.Clear();
            NewMenu.Clear();
            this.NewRole.Sys_Role_MenuInfo = new List<API.Sys_Role_MenuInfo>();
            var menuList = (from b in Store.MenuInfos
                                               where (b.Parent == 0 || b.Parent == null) && b.Flag == true
                                               join c in role_menus
                                               on b.MenuID equals c.MenuID
                                               into temp1
                                               from c1 in temp1.DefaultIfEmpty()
                                               select new { 
                                                   b,
                                                   c1
                                               }).ToList();
            foreach (var  menu in menuList)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                API.Sys_MenuInfo role_menu=null;
                if (menu.c1 == null)
                {
                    //item.CheckState = System.Windows.Automation.ToggleState.Off;
                    role_menu = new API.Sys_MenuInfo() { 
                    MenuID=menu.b.MenuID,
                    MenuText=menu.b.MenuText,
                    HasHallRole = menu.b.HasHallRole, 
                    HasProRole = menu.b.HasProRole,
                    Flag=false
                    };
                }
                else
                {
                    //switch (menu.c1.Flag)
                    //{
                    //    case true: item.CheckState = System.Windows.Automation.ToggleState.On; break;
                    //    case false: item.CheckState = System.Windows.Automation.ToggleState.Off; break;
                    //    default: item.CheckState = System.Windows.Automation.ToggleState.Indeterminate; break;
                    //}
                    role_menu = menu.c1;
                }
                item.Header = menu.b.MenuText;
                
                role_menu.MenuID = menu.b.MenuID;
                item.DataContext = role_menu;
                NewMenu.Add(role_menu);
                #region 是否有子节点
                var menuList_temp = (from b in Store.MenuInfos
                                     where b.Parent == menu.b.MenuID && b.Flag == true
                                     select b 
                                ).ToList();
                if (menuList_temp.Count == 0)
                {
                    if (role_menu.Flag == true) item.CheckState = System.Windows.Automation.ToggleState.On;
                    else
                    {
                        item.CheckState = System.Windows.Automation.ToggleState.Off;
                        role_menu.Flag = false;
                    }
                }
                #endregion

                else
                {

                    System.Windows.Automation.ToggleState ischeck = InitMenu(menu.b.MenuID, menuList_temp, role_menus, item);//menu.b.Menu = InitMenu(menu.b.MenuID, role_menus, item);
                    item.CheckState = ischeck;
                    switch (ischeck)
                    {
                        case System.Windows.Automation.ToggleState.On: role_menu.Flag = true; break;
                        case System.Windows.Automation.ToggleState.Off: role_menu.Flag = false; break;
                        default: role_menu.Flag = null; break;
                    }
                }
                this.RadTreeView1.Items.Add(item);
            }
            //this.RadTreeView1.ItemsSource = menuList;
            //this.RadTreeView2.ItemsSource = menuList;
        }
        private System.Windows.Automation.ToggleState InitMenu(int ParentMenuID, List<API.Sys_MenuInfo> ChildMenus, List<API.Sys_MenuInfo> role_menus, RadTreeViewItem itemParent)
        {
            var menuList_temp = (from b in ChildMenus
                                                    where b.Parent == ParentMenuID && b.Flag==true
                                join c in role_menus
                           on b.MenuID equals c.MenuID
                           into temp1
                                from c1 in temp1.DefaultIfEmpty()
                                select new
                                {
                                    b,
                                    c1
                                }
                                ).ToList();
            
           List<API.Sys_MenuInfo> t = new List<API.Sys_MenuInfo>();
           bool HasChildChecked = false;//是否子节点有选中的
           bool HasChildUnChecked = false;//是否子节点有未选中的
           bool HasChildNeitherOr = false;//是否子节点有半选中的
            foreach (var menu in menuList_temp)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                API.Sys_MenuInfo role_menu = null;
                if (menu.c1 == null)
                {
                    //item.CheckState = System.Windows.Automation.ToggleState.Off;
                    role_menu = new API.Sys_MenuInfo()
                    {
                        MenuID=menu.b.MenuID,
                        MenuText=menu.b.MenuText,
                        HasHallRole = menu.b.HasHallRole,
                        HasProRole = menu.b.HasProRole,
                        Flag=false
                    };
                }
                else
                {
                    //switch (menu.c1.Flag)
                    //{
                    //    case true: item.CheckState = System.Windows.Automation.ToggleState.On; break;
                    //    case false: item.CheckState = System.Windows.Automation.ToggleState.Off; break;
                    //    default: item.CheckState = System.Windows.Automation.ToggleState.Indeterminate; break;
                    //}
                    role_menu = menu.c1;
                }

                item.Header = menu.b.MenuText;
                
                role_menu.MenuID = menu.b.MenuID; 
                item.DataContext = role_menu;
                NewMenu.Add(role_menu);
                
                 

                #region 是否有子节点
                var menuList_temp2 = (from b in Store.MenuInfos
                                      where b.Parent == menu.b.MenuID && b.Flag == true
                                     select b
                                ).ToList();
                if (menuList_temp2.Count == 0)
                {
                    if (role_menu.Flag == true)
                    {
                        item.CheckState = System.Windows.Automation.ToggleState.On;
                        HasChildChecked = true;
                    }
                    else
                    {
                        item.CheckState = System.Windows.Automation.ToggleState.Off;
                        HasChildUnChecked = true;
                        role_menu.Flag = false;
                    }
                }
                #endregion

                else
                {

                    System.Windows.Automation.ToggleState ischeck = InitMenu(menu.b.MenuID, menuList_temp2, role_menus, item);//menu.b.Menu = InitMenu(menu.b.MenuID, role_menus, item);
                    item.CheckState = ischeck;
                    //if (ischeck == System.Windows.Automation.ToggleState.Off) role_menu.Flag = false;
                    switch (ischeck)
                    {
                        case System.Windows.Automation.ToggleState.On: role_menu.Flag = true; HasChildChecked = true; break;
                        case System.Windows.Automation.ToggleState.Off: role_menu.Flag = false; HasChildUnChecked = true; break;
                        default: role_menu.Flag = null; HasChildNeitherOr = true; break;
                    }
                }

                itemParent.Items.Add(item);
                if (!t.Contains(menu.b))
                    t.Add(menu.b);
            }
            //return t;
            //有半选中
            if (HasChildNeitherOr == true)
                return System.Windows.Automation.ToggleState.Indeterminate;
            //只有选中 和 未选中
            else if(HasChildChecked==true && HasChildUnChecked==true)
                return System.Windows.Automation.ToggleState.Indeterminate;
            //全部选中
            else if(HasChildChecked==true)
                return System.Windows.Automation.ToggleState.On;
            //全部未选中
            else 
                return System.Windows.Automation.ToggleState.Off;
        }

         
        #endregion

        #region 初始化全部仓库
        private void InitHall(List<API.Sys_Role_Menu_HallInfo> AllCheckHall)
        {
            this.RadTreeView2.Items.Clear();
            this.NewRole.Sys_Role_Menu_HallInfo = new List<API.Sys_Role_Menu_HallInfo>();

            var hall_unoin_checkhall = from b in Store.ProHallInfo
                                       where b.Flag==true
                                       join c in AllCheckHall
                                       on b.HallID equals c.HallID
                                       into temp
                                       from c1 in temp.DefaultIfEmpty()
                                       group c1 by new { b } into temp2
                                       select new { 
                                        Pro_HallInfo= temp2.Key.b,
                                        Sys_Role_Menu_HallInfo= temp2.Where(p=>p!=null)
                                       };

            var Area_union_Hall = from b in Store.AreaInfo
                                  where b.Flag==true
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


                    if (hall.Sys_Role_Menu_HallInfo.Count()!=0)
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

            var pro_union_menuPro=from b in Store.ProClassInfo
                                  join c in AllCheckedPro
                                  on b.ClassID equals c.ClassID
                                  into temp2
                                  from c1 in temp2.DefaultIfEmpty()
                                  select new {
                                      ProClassInfo = b,
                                   Sys_Role_Menu_ProInfo= c1
                                  };

            //var class_union_pro = from b in Store.ProClassInfo
            //                      where b.IsDeleted !=true
            //                      join c in pro_union_menuPro
            //                      on b.ClassID equals c.Pro_ProInfo.Pro_ClassID
            //                      into temp
            //                      from c1 in temp
            //                      group c1 by new { b } into temp2
            //                      select new { 
            //                        ClassID=temp2.Key.b.ClassID,
            //                        ClassName=temp2.Key.b.ClassName,
            //                        ProInfo_MenuProInfo=temp2.ToList()
            //                      };


            foreach (var c in pro_union_menuPro)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                item.Header = c.ProClassInfo.ClassName;
                item.Tag = c.ProClassInfo.ClassID;
                API.Sys_Role_Menu_ProInfo menu_pro = new API.Sys_Role_Menu_ProInfo();
                menu_pro.ClassID = c.ProClassInfo.ClassID;
                
                 
                if (c.Sys_Role_Menu_ProInfo != null)
                {
                    item.CheckState = System.Windows.Automation.ToggleState.On;
                    item.DataContext = c.Sys_Role_Menu_ProInfo;
                }
                else item.CheckState = System.Windows.Automation.ToggleState.Off;
                this.RadTreeView3.Items.Add(item);
            }
        }
        #endregion


        #region 全选 反选
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
            menu.Flag = true;
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
            menu.Flag = false;
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
            //if (this.RadTreeView1.SelectedItem == null || this.NewRole==null) return;
            //if (this.NewRole == null)
            //{
            //    this.NewRole = new API.Sys_RoleInfo();
            //    this.NewRole.Sys_Role_Menu_HallInfo = new List<API.Sys_Role_Menu_HallInfo>();
            //}
            //RadTreeViewItem item = e.Source as RadTreeViewItem;
            //if (item.Tag != null)
            //{
            //    RadTreeViewItem MenuItem = (RadTreeViewItem)this.RadTreeView1.SelectedItem;
            //    //API.Sys_MenuInfo menu = (API.Sys_MenuInfo)MenuItem.DataContext;
            //    API.Sys_Role_Menu_HallInfo menuPro = new API.Sys_Role_Menu_HallInfo();
            //    menuPro.HallID = item.Tag+"";
            //    //menuPro.MenuID = menu.MenuID;
            //    //item.DataContext = menuPro;
            //    //menu.Sys_Role_Menu_HallInfo.Add(menuPro);
            //    this.NewRole.Sys_Role_Menu_HallInfo.Add(menuPro);

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
            //this.dataGrid1.ItemsSource = Store.RoleInfo;
            
            
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取第一页的数据
            API.ReportPagingParam pageParam = new API.ReportPagingParam()
            {
                PageIndex = e.NewPageIndex,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>() { 
                new API.ReportSqlParams_String(){ParamName="RoleName",ParamValues=this.roleName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="Note",ParamValues=this.note.Text.Trim()}
                }
            };
            this.InitPageEntity(MethodIDInfo.Sys_Role_GetList, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            #region 取第一页的数据
            API.ReportPagingParam pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>() { 
                new API.ReportSqlParams_String(){ParamName="RoleName",ParamValues=this.roleName.Text.Trim()},
                new API.ReportSqlParams_String(){ParamName="Note",ParamValues=this.note.Text.Trim()}
                }
            };
            this.InitPageEntity(MethodIDInfo.Sys_Role_GetList, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
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
            this.note2.Text = "";
            this.roleName2.Text = "";
            InitMenu(new List<API.Sys_MenuInfo>());
            InitHall(new List<API.Sys_Role_Menu_HallInfo>());
            InitPro(new List<API.Sys_Role_Menu_ProInfo>());
            this.dataGrid1.SelectedItems.Clear();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(this.roleName2.Text.Trim()) )
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"角色名称必填？");
                return;
            }
            //API.Sys_RoleInfo role = (API.Sys_RoleInfo)this.dataGrid1.SelectedItem;

            MessageBoxResult r = (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定要保存吗？", "", MessageBoxButton.OKCancel));
            if (r == MessageBoxResult.Cancel) return;
            this.NewRole.Sys_Role_MenuInfo = new List<API.Sys_Role_MenuInfo>();
            this.NewRole.Sys_Role_Menu_ProInfo = new List<API.Sys_Role_Menu_ProInfo>();
            this.NewRole.Sys_Role_Menu_HallInfo = new List<API.Sys_Role_Menu_HallInfo>();
            //this.NewRole.RoleID=mo
            foreach (API.Sys_MenuInfo menu in this.NewMenu)
            {
                if (menu.Flag !=false)
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
            foreach(RadTreeViewItem hall in this.RadTreeView2.Items)
            {
                if (hall.CheckState != System.Windows.Automation.ToggleState.Off)
                {
                    foreach (RadTreeViewItem child in hall.Items)
                    {
                        if (child.CheckState == System.Windows.Automation.ToggleState.On)
                        {
                            this.NewRole.Sys_Role_Menu_HallInfo.Add(new API.Sys_Role_Menu_HallInfo() { 
                            HallID=child.Tag+""
                           
                            });
                        }
                    }
                }
            }
            this.NewRole.RoleName = this.roleName2.Text.Trim();
            this.NewRole.Note = this.note2.Text.Trim();
            PublicRequestHelp req = new PublicRequestHelp(this.busy, MethodIDInfo.Sys_Role_Update, new object[] { this.NewRole }, AddRole_MainCompleted);
        }
        /// <summary>
        /// 保存后返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddRole_MainCompleted(object sender, API.MainCompletedEventArgs e)
        {
            
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败，可能原因网络问题");
                Logger.Log("保存失败，可能原因网络问题");
                this.busy.IsBusy = false;
                return;
            }
            API.WebReturn r = e.Result;
            if (!r.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败，"+r.Message);
                Logger.Log("保存失败，" + r.Message);
                this.busy.IsBusy = false;
                return;
                
            }
            MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
            Logger.Log("保存成功");

            API.Sys_RoleInfo role = (API.Sys_RoleInfo)this.dataGrid1.SelectedItem;
            role.Sys_Role_MenuInfo.Clear();
            role.Sys_Role_MenuInfo.AddRange(this.NewRole.Sys_Role_MenuInfo);
            role.RoleName = this.NewRole.RoleName;
            role.Note = this.NewRole.Note;

            InitMenu(new List<API.Sys_MenuInfo>());
            InitHall(new List<API.Sys_Role_Menu_HallInfo>());
            InitPro(new List<API.Sys_Role_Menu_ProInfo>());
            
            
            this.dataGrid1.Rebind();
            this.dataGrid1.SelectedItems.Clear();
            this.roleName2.Text = "";
            this.note2.Text = "";
            this.busy.IsBusy = false;
        }
        #endregion

        /// <summary>
        /// Grid 选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid1_SelectionChanging(object sender, SelectionChangingEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            API.Sys_RoleInfo role=(API.Sys_RoleInfo) e.AddedItems[0];
            this.NewRole.RoleID = role.RoleID;
            this.note2.Text = role.Note+"";
            this.roleName2.Text = role.RoleName+"";
            PublicRequestHelp req = new PublicRequestHelp(this.busy, MethodIDInfo.Sys_Role_Hall_GetList, new object[]{ role.RoleID }, CommpleteGetRole_Hall_Pro);


            
        }


        #region 获取角色的仓库、商品

        protected void CommpleteGetRole_Hall_Pro(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"系统出错");
                this.busy.IsBusy = false;
                return;
            }
            if (e.Result.ReturnValue == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                this.busy.IsBusy = false;
                return;
            }
            try
            {

                API.Sys_RoleInfo role = (API.Sys_RoleInfo)this.dataGrid1.SelectedItem;
                role.Sys_Role_Menu_HallInfo = (List<API.Sys_Role_Menu_HallInfo>)e.Result.Obj;
                this.busy.IsBusy = false;
                PublicRequestHelp req = new PublicRequestHelp(this.busy, MethodIDInfo.Sys_Role_Pro_GetList, new object[]{ role.RoleID }, CommpleteGetRole_Hall_Pro2);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"系统出错");
                this.busy.IsBusy = false;
                 
            }
             
        }
        protected void CommpleteGetRole_Hall_Pro2(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"系统出错");
                this.busy.IsBusy = false;
                return;
            }
            if (e.Result.ReturnValue == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                this.busy.IsBusy = false;
                return;
            }
            try
            {

                API.Sys_RoleInfo role = (API.Sys_RoleInfo)this.dataGrid1.SelectedItem;
                role.Sys_Role_Menu_ProInfo = (List<API.Sys_Role_Menu_ProInfo>)e.Result.Obj;
                if (role.Sys_Role_Menu_HallInfo == null)
                    role.Sys_Role_Menu_HallInfo = new List<API.Sys_Role_Menu_HallInfo>();
                if (role.Sys_Role_Menu_ProInfo == null)
                    role.Sys_Role_Menu_ProInfo = new List<API.Sys_Role_Menu_ProInfo>();

                #region 有权限的菜单
                 var Role_Menu1 = from b in Store.MenuInfos 
                                 join c in role.Sys_Role_MenuInfo
                                 on b.MenuID equals c.MenuID
                                 into temp1
                                 from c1 in temp1
                                 select new API.Sys_MenuInfo
                                 {
                                     Flag = c1.IsChecked,
                                     HasHallRole = b.HasHallRole,
                                     HasProRole = b.HasProRole,
                                     Menu = b.Menu,
                                     MenuID = b.MenuID,
                                     MenuImg = b.MenuImg,
                                     MenuText = b.MenuText,
                                     MenuValue = b.MenuValue,
                                     Note = b.Note,
                                     Order = b.Order,
                                     Parent = b.Parent,
                                     //Sys_Role_Menu_HallInfo = temp4.Key.Sys_Role_Menu_HallInfo,
                                     //Sys_Role_Menu_ProInfo = temp4.Where(p => p != null).ToList()
                                 };
                #endregion


                #region 每个菜单对应的门店权限
                 //var Role_Menu = from b in Role_Menu1 

                 //               join d in role.Sys_Role_Menu_HallInfo
                 //               on b.MenuID equals d.MenuID
                 //               into temp2
                 //               from d1 in temp2.DefaultIfEmpty()
                 //               //join f in role.Sys_Role_Menu_ProInfo
                 //               //on b.MenuID equals f.MenuID
                 //               //into temp3
                 //               //from f1 in temp2.DefaultIfEmpty()
                 //               group d1 by b
                 //                   into temp4

                 //                   select new
                 //                   {
                 //                       Sys_MenuInfo = temp4.Key, 
                 //                       Sys_Role_Menu_HallInfo = temp4.Where(p => p != null).ToList()
                 //                   };
                #endregion


                #region 每个菜单对应的商品权限
                 var Role_Menu2 = (from b in Role_Menu1

                                  join f in role.Sys_Role_Menu_ProInfo
                                  on b.MenuID equals f.MenuID
                                  into temp3
                                  from f1 in temp3.DefaultIfEmpty()
                                  group f1 by b
                                      into temp4

                                      select new API.Sys_MenuInfo
                                      {
                                          Flag = (temp4.Key.Flag),
                                          HasHallRole = temp4.Key.HasHallRole,
                                          HasProRole = temp4.Key.HasProRole,
                                          Menu = temp4.Key.Menu,
                                          MenuID = temp4.Key.MenuID,
                                          MenuImg = temp4.Key.MenuImg,
                                          MenuText = temp4.Key.MenuText,
                                          MenuValue = temp4.Key.MenuValue,
                                          Note = temp4.Key.Note,
                                          Order = temp4.Key.Order,
                                          Parent = temp4.Key.Parent,
                                          Sys_Role_Menu_HallInfo = temp4.Key.Sys_Role_Menu_HallInfo,
                                          Sys_Role_Menu_ProInfo = temp4.Where(p => p != null).ToList()
                                      }).ToList();
                #endregion
                

               

                InitMenu(Role_Menu2);
                InitHall(role.Sys_Role_Menu_HallInfo);
                InitPro(new List<API.Sys_Role_Menu_ProInfo>());

            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"系统出错");
                this.busy.IsBusy = false;

            }
            finally
            {
                this.busy.IsBusy = false;
            }
        }
        #endregion

       

       

      
        
        

       

        

       

        









    }
}
