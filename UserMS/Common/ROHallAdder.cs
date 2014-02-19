using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using UserMS.MyControl;

namespace UserMS.Common
{
    public class ROHallAdder
    {
        MyMulSelect Hall;
        MultSelecter msFrm;
        MultSelecter2 msFrm2;
        List<API.Pro_HallInfo> hallInfo;
        List<TreeViewModel> parent;

        private event EventHandler<MyEventArgs> AddCompleted;

        public ROHallAdder() { }  // 

        public ROHallAdder(ref MyMulSelect tb, int menuID,EventHandler<MyEventArgs> addCompleted)
        {
            Hall = tb;
            AddCompleted += addCompleted;

            //var menuinfo = Store.RoleInfo.First().Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
            //if (menuinfo.Count() != 0)
            //{
            //    if (menuinfo.First().Note == null)
            //    {
            // 左连接获取仓库列表              
            var query = (from b in Store.RoleInfo.First().Sys_Role_Menu_HallInfo
                         // where b.MenuID == menuID
                         join c in Store.ProHallInfo
                         on b.HallID equals c.HallID into hall
                         from b1 in hall.DefaultIfEmpty()
                         select b1).ToList();
            //if (query.Count() > 0)
            //    return query;
            hallInfo = query.Where(c => c != null).ToList();
            List<TreeViewModel> child = new List<TreeViewModel>();

            //生成左边树
            var AreaList = (from b in hallInfo
                            join c in Store.AreaInfo.Where(c => c.AreaID != null) on b.AreaID equals c.AreaID
                            where b != null && b.AreaID != null && c.AreaID != null
                            select c).Distinct().ToList();
            parent = CommonHelper.AreaTreeModel(AreaList, hallInfo);
            //    }
            //}
            // tb.btnSearch.Click += SearchHall;
            tb.SearchEvent = new RoutedEventHandler(SearchHall);
            //默认选中第一个
            if (hallInfo.Count > 0)
            {
                this.Hall.Tag += hallInfo[0].HallID;
                this.Hall.Text += hallInfo[0].HallName;
            }

            if (AddCompleted != null)
            {
                AddCompleted(Hall, new MyEventArgs() { Value = Hall.Tag });
            }
        }

        public ROHallAdder(ref MyMulSelect tb, int menuID)
        {
                Hall = tb;


                //var menuinfo = Store.RoleInfo.First().Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
                //if (menuinfo.Count() != 0)
                //{
                //    if (menuinfo.First().Note == null)
                //    {
                        // 左连接获取仓库列表              
                        var query = (from b in Store.RoleInfo.First().Sys_Role_Menu_HallInfo
                                    // where b.MenuID == menuID
                                     join c in Store.ProHallInfo
                                     on b.HallID equals c.HallID into hall
                                     from b1 in hall.DefaultIfEmpty()
                                     select b1).ToList();
                        //if (query.Count() > 0)
                        //    return query;
                        hallInfo = query.Where(c => c != null).ToList();
                        List<TreeViewModel> child = new List<TreeViewModel>();

                        //生成左边树
                        var AreaList = (from b in hallInfo
                                        join c in Store.AreaInfo.Where(c => c.AreaID != null) on b.AreaID equals c.AreaID
                                       where b!=null && b.AreaID !=null && c.AreaID !=null
                                        select c).Distinct().ToList();
                        parent = CommonHelper.AreaTreeModel(AreaList,hallInfo);
                //    }
                //}
                       // tb.btnSearch.Click += SearchHall;
                tb.SearchEvent = new RoutedEventHandler(SearchHall);
            //默认选中第一个
                if (hallInfo.Count > 0)
                {
                    this.Hall.Tag += hallInfo[0].HallID;
                    this.Hall.Text += hallInfo[0].HallName;
                }
        }

        public void SearchHall(object sender, RoutedEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// 弹窗
        /// </summary>
        private void Search()
        {
            //MultSelecter2 s = new MultSelecter2(,,,,);
            msFrm2 = new MultSelecter2(parent,
                hallInfo, "HallName",
                new string[] { "HallID", "HallName" },
                new string[] { "仓库编码", "仓库名称" });

            //msFrm = new MultSelecter(
            //     parent,
            //     hallInfo, "AreaID", "HallName",
            //    new string[] { "HallID", "HallName" },
            //    new string[] { "仓库编码", "仓库名称" },false
            //   );  msFrm.Closed += HallSelect_Closed;
            msFrm2.Closed += msFrm2_Closed;
            msFrm2.ShowDialog();
        }

        void msFrm2_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;

            if (selecter.DialogResult == true)
            {
                this.Hall.Tag = "";
                this.Hall.Text = "";
                List<UserMS.API.Pro_HallInfo> phList = selecter.SelectedItems.OfType<API.Pro_HallInfo>().ToList();
                if (phList.Count == 0) return;

                int index = 0;
                foreach (var item in phList)
                {
                    index++;
                    this.Hall.Tag += item.HallID;
                    this.Hall.Text += item.HallName;

                    if (index < phList.Count)
                    {
                        this.Hall.Tag += ",";
                        this.Hall.Text += ",";
                    }
                }

                if (AddCompleted != null)
                {
                    AddCompleted(Hall, new MyEventArgs() { Value = Hall.Tag });
                }
            }
        }
  
        /// <summary>
        /// 确定添加仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HallSelect_Closed(object sender, EventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;
       
            if (selecter.DialogResult == true)
            {
                this.Hall.Tag = "";
                this.Hall.Text = "";
                List<UserMS.API.Pro_HallInfo> phList = selecter.SelectedItems.OfType<API.Pro_HallInfo>().ToList();
                if (phList.Count == 0) return;

                int index = 0;
                foreach (var item in phList)
                {
                    index++;
                    this.Hall.Tag += item.HallID;
                    this.Hall.Text+= item.HallName;

                    if (index < phList.Count)
                    {
                        this.Hall.Tag += ",";
                        this.Hall.Text += ",";
                    }
                }

                if (AddCompleted != null)
                {
                    AddCompleted(Hall, new MyEventArgs() { Value = Hall.Tag });
                }
            }
        }

    }
}
