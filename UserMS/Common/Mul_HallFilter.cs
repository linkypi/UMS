using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    public class Mul_HallFilter
    {

        RadGridView HallGrid;
        public Mul_HallFilter(ref RadGridView tb)
        {
            HallGrid = tb;
        }

        /// <summary>
        /// 过滤仓库
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public List<API.Pro_HallInfo> FilterHall(int menuID, List<API.Pro_HallInfo> hallList)
        {
            var menuInfo = Store.RoleInfo.First().Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
            if (menuInfo != null)
            {
                if (menuInfo.First().Note == null)
                {
                    // 左连接获取仓库列表              
                    var query = (from b in Store.RoleInfo.First().Sys_Role_Menu_HallInfo
                                 where b.MenuID==menuID
                                 join c in hallList
                                 on b.HallID equals c.HallID into hall                             
                                 from b1 in hall.DefaultIfEmpty()
                                 select b1).ToList();
                    if (query.Count() > 0)
                        return query;
                }
            }
       
            return new List<API.Pro_HallInfo>();
        }

        /// <summary>
        /// 弹窗
        /// </summary>
        public void GetHall(List<API.Pro_HallInfo> hallInfo)
        {
            List<TreeViewModel> child = new List<TreeViewModel>();

            //生成左边树
            var AreaList = (from b in hallInfo
                            join c in Store.AreaInfo on b.AreaID equals c.AreaID
                            select c).Distinct().ToList();
            List<TreeViewModel> parent = CommonHelper.AreaTreeModel(AreaList,hallInfo);
               
            MultSelecter2 msFrm = new MultSelecter2(
             parent,
             hallInfo,
             "HallName",
             new string[] { "HallID", "HallName" },
             new string[] { "仓库编码", "仓库名称" }
            );
            msFrm.Closed += HallSelect_Closed;
            msFrm.ShowDialog();

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
                List<UserMS.API.Pro_HallInfo> phList = selecter.SelectedItems.OfType<API.Pro_HallInfo>().ToList();
                if (HallGrid.ItemsSource != null)
                {
                    List<API.Pro_HallInfo> HallList = HallGrid.ItemsSource as List<API.Pro_HallInfo>;
                    phList.AddRange(HallList);
                }
                HallGrid.ItemsSource = phList.Distinct().ToList();
            }
        
        }

    }
}

