using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    public class ViewOperate
    {
        #region 删除商品
        /// <summary>
        /// 删除Checked选中的商品
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="unIMEIModels"></param>
        /// <param name="rgv"></param>
        public static void DelCheckedPro(ref List<API.SeleterModel> unIMEIModels, ref RadGridView rgv)
        {
            if (PormptPage.PormptMessage("确定删除选中的商品吗？", "提示"))
            {
                foreach (var item in rgv.SelectedItems)
                {
                    unIMEIModels.Remove(item as API.SeleterModel);
                }
                rgv.Rebind();
            }
        }
        #endregion

        #region 删除营业厅     
        public static void DelHall( ref RadGridView rgv)
        {
            if (PormptPage.PormptMessage("确定删除选中的营业厅吗？", "提示"))
            {
                List<UserMS.API.Pro_HallInfo> HallList = rgv.ItemsSource as List<API.Pro_HallInfo>;
                foreach (var item in rgv.SelectedItems)
                {
                    HallList.Remove(item as API.Pro_HallInfo);
                }
                rgv.Rebind();
            }
        }
        #endregion

        #region 删除会员卡类型
        public static void DelVIPType(ref RadGridView rgv)
        {
            if (PormptPage.PormptMessage("确定删除会员卡类型？", "提示"))
            {
                List<UserMS.API.VIP_VIPType> VIPType = rgv.ItemsSource as List<API.VIP_VIPType>;
                foreach (var Item in rgv.SelectedItems)
                {
                    VIPType.Remove(Item as API.VIP_VIPType);
                }
                rgv.Rebind();
            }
        }
        #endregion
        #region 删除会员
        public static void DelVIP(ref RadGridView rgv)
        {
          if (PormptPage.PormptMessage("确定删除会员？", "提示"))
          {
            List<API.View_VIPInfo> VIPType = rgv.ItemsSource as List<API.View_VIPInfo>;
            foreach (var Item in rgv.SelectedItems)
            {
                VIPType.Remove(Item as API.View_VIPInfo);
            }
            rgv.Rebind();
          }
        }
        #endregion 
        #region 删除串码
        /// <summary>
        /// 删除Checked选中的串码
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="unIMEIModels"></param>
        /// <param name="rgv"></param>
        public static void DelCheckIMEI(ref List<API.SelecterIMEI> unIMEIModels, ref RadGridView rgv)
        {
            if (PormptPage.PormptMessage("确定删除选中的串码吗？", "提示"))
            {
                foreach (var item in rgv.SelectedItems)
                {
                    unIMEIModels.Remove(item as API.SelecterIMEI);
                }
                rgv.Rebind();
            }
        }
        #endregion


        #region 添加串码及检验
        /// <summary>
        /// 批量添加串码
        /// </summary>
        public static void BatchAdd(string IMEI, ref List<API.SelecterIMEI> uncheckIMEI, ref RadGridView GridUnCheckIMEI)
        {
            if (String.IsNullOrEmpty(IMEI))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码不能为空");
                return;
            }
            List<string> list = new List<string>(IMEI.Split("\r\n".ToCharArray()));
            API.SelecterIMEI cm;
            foreach (string s in list)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (!ValidateIMEI(s, uncheckIMEI))  //去除重复项
                    {
                        cm = new API.SelecterIMEI();
                        cm.IMEI = s.ToUpper();
                        uncheckIMEI.Add(cm);
                    }
                }
            }
            GridUnCheckIMEI.Rebind();
        }
        /// <summary>
        /// 批量添加会员卡号
        /// </summary>
        public static void IMEIAdd(string IMEI, ref List<API.SelecterIMEI> uncheckIMEI)
        {
            if (String.IsNullOrEmpty(IMEI))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码不能为空");
                return;
            }
            List<string> list = new List<string>(IMEI.Split("\r\n".ToCharArray()));
            API.SelecterIMEI cm;
            foreach (string s in list)
            {
                if (!string.IsNullOrEmpty(s.Trim()))
                {
                    if (!ValidateIMEI(s.Trim(), uncheckIMEI))  //去除重复项
                    {
                        cm = new API.SelecterIMEI();
                        cm.IMEI = s.Trim();
                        uncheckIMEI.Add(cm);
                    }
                }
            }
        }
        /// <summary>
        /// 验证串码是否已存在
        /// </summary>
        public static bool ValidateIMEI(string imei, List<API.SelecterIMEI> uncheckIMEI)
        {
            if (uncheckIMEI.Count == 0)
            {
                return false;
            }
            foreach (API.SelecterIMEI cm in uncheckIMEI)
            {
                if (cm.IMEI == null)
                {
                    continue;
                }
                if (cm.IMEI.Equals(imei))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 选中拣货成功列表中的行
        /// </summary>
        /// <param name="checkedModels">已拣商品列表</param>
        /// <param name="GridCheckedPro">已拣商品Grid</param>
        /// <param name="GridCheckedIMEI">已拣商品的串码Grid</param>
        public static void GridSelectChanged(ref RadGridView GridCheckedPro, ref RadGridView GridCheckedIMEI)
        {
            if (GridCheckedPro.SelectedItems.Count!=1)
            {
                GridCheckedIMEI.ItemsSource = null;
                GridCheckedIMEI.Rebind();
            }
            if (GridCheckedPro.SelectedItem != null)
            {
                API.SeleterModel pinfo = GridCheckedPro.SelectedItem as API.SeleterModel;
                GridCheckedIMEI.ItemsSource = pinfo.IsIMEI;    
                GridCheckedIMEI.Rebind();
            }
        }
        #endregion
    }
}
