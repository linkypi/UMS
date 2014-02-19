using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    public class ViewCommon
    {
 
        /// <summary>
        /// 删除Checked选中的商品
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="unIMEIModels"></param>
        /// <param name="rgv"></param>
        public static void DelCheckedPro(ref List<SlModel.ViewModel> unIMEIModels, ref RadGridView rgv)
        {
            if (rgv.SelectedItems.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要删除的商品");
                return;
            }

            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除选中的商品吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            if (rgv.SelectedItems.Count == unIMEIModels.Count)
            {
                unIMEIModels.RemoveAll(viewModel => true);
                rgv.Rebind();
                return;
            }
            SlModel.ViewModel model = null; 
            foreach(var item in  rgv.SelectedItems)
            {
                model = item as SlModel.ViewModel;
                foreach (SlModel.ViewModel vm in unIMEIModels)
                {
                    if (model.ProID==vm.ProID)
                    {
                        unIMEIModels.Remove(vm);
                        break;
                    }
                }
            }
            rgv.Rebind();
        }

        /// <summary>
        /// 删除Checked选中的串码
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="unIMEIModels"></param>
        /// <param name="rgv"></param>
       public static void DelCheckedIMEI(ref List<SlModel.CheckModel> unCheckIMEI, ref RadGridView rgv)
        {
            if (rgv.SelectedItems.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要删除的串码");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除选中的串码吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
             if (rgv.SelectedItems.Count == unCheckIMEI.Count)
            {
                unCheckIMEI.RemoveAll(viewModel => true);
                rgv.Rebind();
                return;
            }
            SlModel.CheckModel model = null;
            foreach (var item in rgv.SelectedItems)
            {
                model = item as SlModel.CheckModel;
                foreach (SlModel.CheckModel vm in unCheckIMEI)
                {
                    if (vm.IMEI==model.IMEI)
                    {
                        unCheckIMEI.Remove(vm);
                        break;
                    }
                }
            }
            rgv.Rebind();
        }

        /// <summary>
        /// 右键删除串码
        /// </summary>
        /// <param name="unIMEIModels"></param>
        /// <param name="rgv"></param>
       public static void DeleteSingleIMEI(ref List<SlModel.CheckModel> unCheckIMEI, ref RadGridView rgv)
       {
           if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除选中的串码吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
           {
               return;
           }
           if(rgv.SelectedItem==null){return;}
           SlModel.CheckModel cm = rgv.SelectedItem as  SlModel.CheckModel ;
           foreach (SlModel.CheckModel c in unCheckIMEI)
           {
               if (c.IMEI==cm.IMEI)
               {
                   unCheckIMEI.Remove(c);
                   break;
               }
           }
           rgv.Rebind();
       }

       /// <summary>
       /// 右键删除商品
       /// </summary>
       /// <param name="unIMEIModels"></param>
       /// <param name="rgv"></param>
       public static void DeleteSinglePro(ref List<SlModel.ViewModel> unIMEIModels, ref RadGridView rgv)
       {
           if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除选中的商品吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
           {
               return;
           }
           if (rgv.SelectedItem == null) { return; }
           SlModel.ViewModel vm = rgv.SelectedItem as SlModel.ViewModel;
           foreach (SlModel.ViewModel v in unIMEIModels)
           {
               if (vm.IMEI == v.IMEI)
               {
                   unIMEIModels.Remove(v);
                   break;
               }
           }
           rgv.Rebind();
       }

       /// <summary>
       /// 验证串码是否已存在
       /// </summary>
       public static bool ValidateIMEI(string imei, List<SlModel.CheckModel> uncheckIMEI)
       {
           if (uncheckIMEI.Count == 0)
           {
               return false;
           }
           foreach (SlModel.CheckModel cm in uncheckIMEI)
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

       public static bool ValidateIMEI(string imei, List<string> list)
       {
           if (list.Count == 0)
           {
               return false;
           }
           foreach (var cm in list)
           {
               if (cm == null)
               {
                   continue;
               }
               if (cm.Equals(imei))
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
       public static void GridSelectChanged(ref List<SlModel.ViewModel> checkedModels, ref RadGridView GridCheckedPro, ref RadGridView GridCheckedIMEI)
       {
             if (checkedModels != null && GridCheckedPro.SelectedItem!=null)
            {
                SlModel.ViewModel pinfo = GridCheckedPro.SelectedItem as SlModel.ViewModel;
                foreach (var i in checkedModels)
                {
                    if (i.IsNeedIMEI)
                    {
                        if (i.ID == pinfo.ID)
                        {
                            List<API.IMEIModel> list = new List<API.IMEIModel>();
                            foreach (var item in i.IMEI)
                            {
                                list.Add(new API.IMEIModel() { NewIMEI = item });
                            }

                            GridCheckedIMEI.ItemsSource = list;
                            break;
                        }
                    }
                }
                GridCheckedIMEI.Rebind();
            }
       }

        /// <summary>
        /// 批量添加串码
        /// </summary>
       public static void BatchAdd(string IMEI, ref List<SlModel.CheckModel> uncheckIMEI, ref RadGridView GridUnCheckIMEI)
       {
           if (String.IsNullOrEmpty(IMEI))
           {
               MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码不能为空");
               return;
           }
           List<string> list = new List<string>(IMEI.Split("\r\n".ToCharArray()));
           SlModel.CheckModel cm = null;
           foreach (string s in list)
           {
               if (!string.IsNullOrEmpty(s))
               {
                   if (!ViewCommon.ValidateIMEI(s, uncheckIMEI))  //去除重复项
                   {
                       cm = new SlModel.CheckModel();
                       cm.IMEI = s;
                       uncheckIMEI.Add(cm);
                   }
               }
           }
           GridUnCheckIMEI.Rebind();
       }
    }
}
