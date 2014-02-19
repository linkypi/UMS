using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;
using UserMS.API;

namespace UserMS.Common
{
    public class HallFilter
    {
        MyAutoTextBox Hall;
        RadGridView ProGrid;
        MyControl.MyMulSelect NewHall;
        bool IsMultiSelect = false;//判断仓库是哪种操作
        bool IsNewHall = false;
        string menuID = "";

        private event EventHandler<MyEventArgs> AddCompleted ;
        private event AddHallEventHandler OnAddCompleted;

        public HallFilter()
        {
          
        }
        public HallFilter(bool isMultiselect,ref  MyAutoTextBox tb)
        {
            IsMultiSelect = isMultiselect;
            Hall = tb;
        }

        public HallFilter(ref RadGridView tb)
        {
            ProGrid = tb;
        }
        public HallFilter(bool isNewHall, ref MyControl.MyMulSelect tb)
        {
            IsNewHall = isNewHall;
            NewHall = tb;
        }

        public HallFilter(string menuid, bool isMultiselect, MyAutoTextBox tb)
        {
            IsMultiSelect = isMultiselect;
            Hall = tb;
            menuID = menuid;
            List<API.Pro_HallInfo> halls = FilterHall(int.Parse(menuid), Store.ProHallInfo);
            if (halls.Count != 0)
            {
                Hall.Tag = halls.First().HallID;
                Hall.TextBox.SearchText = halls.First().HallName;
            }
        }


        public HallFilter(string menuid, bool isMultiselect, MyAutoTextBox tb, EventHandler<MyEventArgs> addCompleted)
        {
            IsMultiSelect = isMultiselect;
            Hall = tb;
            menuID = menuid;
            List<API.Pro_HallInfo> halls = FilterHall(int.Parse(menuid), Store.ProHallInfo);
             AddCompleted +=  addCompleted;
            
            if (halls.Count != 0)
            {
                Hall.Tag = halls.First().HallID;
                Hall.TextBox.SearchText = halls.First().HallName;

                if (AddCompleted != null)
                {
                    AddCompleted(Hall, new MyEventArgs() { Value = Hall.Tag });
                }
            }
        }

        public HallFilter(string menuid, bool isMultiselect, MyAutoTextBox tb,AddHallEventHandler onAddCompleted)
        {
            IsMultiSelect = isMultiselect;
            Hall = tb;
            menuID = menuid;
            List<API.Pro_HallInfo> halls = FilterHall(int.Parse(menuid), Store.ProHallInfo);
            OnAddCompleted += onAddCompleted;

            if (halls.Count != 0)
            {
                Hall.Tag = halls.First().HallID;
                Hall.TextBox.SearchText = halls.First().HallName;

                if (AddCompleted != null)
                {
                    AddCompleted(Hall, new MyEventArgs() { Value = Hall.Tag });
                }
            }
        }

        /// <summary>
        /// 过滤仓库
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public List<API.Pro_HallInfo> FilterHall(int menuID, List<API.Pro_HallInfo> hallList)
        {
            try
            {
                if (Store.LoginRoleInfo == null || Store.LoginRoleInfo.Sys_Role_MenuInfo == null)
                    return null;
                //var menuInfo = Store.LoginRoleInfo.Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
                //if (menuInfo != null && menuInfo.Count() > 0)
                //{
                    // 左连接获取仓库列表              
                    var query = (from b in Store.LoginRoleInfo.Sys_Role_Menu_HallInfo
                                 //where b != null && b.MenuID == menuID
                                 join c in hallList on b.HallID equals c.HallID
                                 where c != null && c.Flag==true
                                 select c).Distinct().ToList();
                    if (query.Count() > 0)
                    {
                        return query;
                    }
               // }

                return new List<API.Pro_HallInfo>();
            }
            catch
            {
                return new List<API.Pro_HallInfo>();
            }

        }


        /// <summary>
        /// 过滤仓库
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public List<API.Pro_HallInfo> FilterHall( List<API.Pro_HallInfo> hallList)
        {
            try
            {
                if (Store.LoginRoleInfo == null || Store.LoginRoleInfo.Sys_Role_MenuInfo == null)
                    return null;
               
                // 左连接获取仓库列表              
                var query = (from b in Store.LoginRoleInfo.Sys_Role_Menu_HallInfo
                             //where b != null && b.MenuID == menuID
                             join c in hallList on b.HallID equals c.HallID
                             where c != null && c.Flag == true
                             select c).Distinct().ToList();
                if (query.Count() > 0)
                {
                    return query;
                }
                return new List<API.Pro_HallInfo>();
            }
            catch
            {
                return new List<API.Pro_HallInfo>();
            }

        }

        /// <summary>
        /// 弹窗
        /// </summary> 
        public void GetHall(List<API.Pro_HallInfo> hallInfo)
        {
            List<TreeViewModel> child = new List<TreeViewModel>();
            //生成左边树
            List<API.Pro_AreaInfo> AreaList = new List<API.Pro_AreaInfo>();
            try
            {
                AreaList = (from b in hallInfo
                            where b != null
                            join c in Store.AreaInfo on b.AreaID equals c.AreaID
                            select c).Distinct().ToList();
            }
            catch
            {
                AreaList = new List<API.Pro_AreaInfo>();
            }
            List<TreeViewModel> Parent = CommonHelper.AreaTreeModel(AreaList,hallInfo);
            MultSelecter2 msFrm = new MultSelecter2(
             Parent,
             hallInfo,
             "HallName",
             new string[] { "HallID", "HallName" },
             new string[] { "仓库编码", "仓库名称" }
            );  
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();
           
        }

        /// <summary>
        /// 弹窗
        /// </summary> 
        public void Add()
        {
            var hallInfo = from a in Store.ProHallInfo
                           join b in Store.LoginRoleInfo.Sys_Role_Menu_HallInfo
                           on a.HallID equals b.HallID
                           select a;
            List<TreeViewModel> child = new List<TreeViewModel>();
            //生成左边树
            List<API.Pro_AreaInfo> AreaList = new List<API.Pro_AreaInfo>();
            try
            {
                AreaList = (from b in hallInfo
                            where b != null
                            join c in Store.AreaInfo on b.AreaID equals c.AreaID
                            select c).Distinct().ToList();
            }
            catch
            {
                AreaList = new List<API.Pro_AreaInfo>();
            }
            List<TreeViewModel> Parent = CommonHelper.AreaTreeModel(AreaList, hallInfo.ToList());
            MultSelecter2 msFrm = new MultSelecter2(
             Parent,
             hallInfo,
             "HallName",
             new string[] { "HallID", "HallName" },
             new string[] { "仓库编码", "仓库名称" }
            );
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();

        }


      

        /// <summary>
        /// 确定添加仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void msFrm_Closed(object sender, WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;
            #region 
            if (selecter.DialogResult == true)
            {
                List<UserMS.API.Pro_HallInfo> phList = selecter.SelectedItems.OfType<API.Pro_HallInfo>().ToList();
                if (ProGrid != null)
                {
                    ProGrid.ItemsSource = phList;
                    return;
                }

                if (IsNewHall == true)
                {
                    if (phList.Count == 0)
                    {
                        this.NewHall.Text = string.Empty;
                        return;
                    }
                    this.NewHall.Tag = phList[0].HallID;
                    this.NewHall.Text = phList[0].HallName;
                    return;
                }
                if (IsMultiSelect == true)//查询所需仓库则可以多选
                {
                    if (phList.Count == 0)
                    {
                        this.Hall.TextBox.SearchText = string.Empty;
                        return;
                    }
                    for (int i = 0; i < phList.Count(); i++)
                    {
                        if (i == 0)
                        {
                            Hall.TextBox.SearchText = phList[i].HallName;
                            Hall.Tag = phList[i].HallID;
                        }
                        else
                        {
                            Hall.TextBox.SearchText += "/" + phList[i].HallName;
                            Hall.Tag = "/" + phList[i].HallID;
                        }
                    }
                }
                else
                {
                    if (phList.Count == 0)
                    {
                        this.Hall.TextBox.SearchText = string.Empty;
                        return;
                    }
                    this.Hall.Tag = phList[0].HallID;
                    this.Hall.TextBox.SearchText = phList[0].HallName;
                }
            }
            #endregion 

            if (OnAddCompleted != null)
            {
                OnAddCompleted(Hall, Hall.Tag.ToString());
            }

            if (AddCompleted != null)
            {
                AddCompleted(Hall, new MyEventArgs() { Value = Hall.Tag });
            }
        }

    }
}


//        private void HallSelect_Closed(object sender, WindowClosedEventArgs e)
//        {
//            UserMS.MultSelecter selecter = sender as UserMS.MultSelecter;
//            if (selecter.DialogResult == true)
//            {
//                List<UserMS.API.Pro_HallInfo> phList = selecter.SelectedItems.OfType<API.Pro_HallInfo>().ToList();
//                if (ProGrid != null)
//                {
//                    ProGrid.ItemsSource = phList;
//                    return;
//                }

//                if (IsNewHall == true)
//                {
//                    if (phList.Count == 0)
//                    {
//                        this.NewHall.Text = string.Empty;
//                        return;
//                    }
//                    this.NewHall.Tag = phList[0].HallID;
//                    this.NewHall.Text = phList[0].HallName;
//                    return;
//                }
//                if (IsSearch == true)//查询所需仓库则可以多选
//                {
//                    if (phList.Count == 0)
//                    {
//                        this.Hall.TextBox.SearchText = string.Empty;
//                        return;
//                    }
//                    for (int i = 0; i < phList.Count(); i++)
//                    {
//                        if (i == 0)
//                        {
//                            Hall.TextBox.SearchText = phList[i].HallName;
//                            Hall.Tag=phList[i].HallID;
//                        }
//                        else
//                        {
//                            Hall.TextBox.SearchText += "/" + phList[i].HallName;
//                            Hall.Tag = "/" + phList[i].HallID;
//                        }
//                    }
//                }
//                else
//                {
//                    if (phList.Count == 0)
//                    {
//                        this.Hall.TextBox.SearchText = string.Empty;
//                        return;
//                    }
//                    this.Hall.Tag = phList[0].HallID;
//                    this.Hall.TextBox.SearchText = phList[0].HallName;
//                }
//            }

//        }

//    }
//}
