using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    /// <summary>
    /// 拣货工具
    /// </summary>
    public class PickingDevicer
    {
        private string hid;
        private MyAutoTextBox txtHall;
        private List<SlModel.ViewModel> checkedModels;
        private List<SlModel.ViewModel> unIMELPros;
        private List<SlModel.CheckModel> uncheckIMEI;
        private RadGridView GridCheckedPro;
        private RadGridView GridUnCheckPro;
        private RadGridView GridUnCheckIMEI;
        private RadBusyIndicator Busy;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hallid">仓库编号</param>
        /// <param name="checkedModels">已拣列表</param>
        /// <param name="unIMELPros">无串码商品列表</param>
        /// <param name="uncheckIMEI">未拣货的串码列表</param>
        /// <param name="GridCheckedPro">拣货成功的Grid</param>
        /// <param name="GridUnCheckPro">无串码商品的Grid</param>
        /// <param name="GridUnCheckIMEI">串码列表的Grid</param>
        public PickingDevicer(
            ref MyAutoTextBox hall,
            ref List<SlModel.ViewModel> checkedModels,
            ref List<SlModel.ViewModel> unIMELPros,
            ref List<SlModel.CheckModel> uncheckIMEI,
            ref RadGridView GridCheckedPro,
            ref RadGridView GridUnCheckPro,
            ref RadGridView GridUnCheckIMEI,
            ref RadBusyIndicator busy)
        {
            Busy = busy;
            txtHall = hall; 
            this.checkedModels = checkedModels;
            this.unIMELPros = unIMELPros;
            this.uncheckIMEI = uncheckIMEI;
            this.GridCheckedPro = GridCheckedPro;
            this.GridUnCheckPro = GridUnCheckPro;
            this.GridUnCheckIMEI = GridUnCheckIMEI;
        }

        /// <summary>
        ///  拣货
        /// </summary>
        public void Picking(string aduitid)
        {
            if (txtHall.Tag == null)
            { MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加仓库"); return; }

            this.hid = txtHall.Tag.ToString().Trim();
            //清除已拣列表
            checkedModels.Clear();
            GridCheckedPro.Rebind();
            //清除无串码商品列表
            foreach (SlModel.ViewModel vm in unIMELPros)
            {
                vm.Note = string.Empty;
            }
            GridUnCheckPro.Rebind();
            //清除串码列表
            foreach (SlModel.CheckModel cm in uncheckIMEI)
            {
                cm.Note = string.Empty;
            }
            GridUnCheckIMEI.Rebind();
            foreach (SlModel.ViewModel vm in unIMELPros)
            {
                if (vm.ProCount <= 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"拣货数量必须大于0");
                    return;
                }
                //若是整数却输入了小数则报错
                if ((!vm.IsDecimal) && ((int)vm.ProCount != vm.ProCount))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品" + vm.ProName + "的数量不能为小数");
                    return;
                }
            }
            //拣货 
            List<string> imeilist = new List<string>();
            foreach (SlModel.CheckModel cm in uncheckIMEI)
            {
                imeilist.Add(cm.IMEI);
            }
      
            //有串码拣货
            //if (imeilist.Count != 0)
            //{
            //    PublicRequestHelp help = new PublicRequestHelp(null, 10,
            //        new object[] { imeilist,hid},
            //        new EventHandler<API.MainCompletedEventArgs>(CheckedIMEICompleted)
            //        );
            //}
            //PublicRequestHelp help2 = null;
            ////无串码拣货
            //if (unIMELPros.Count != 0)
            //{
            List<API.NoIMEIModel> list = new List<API.NoIMEIModel>();

            API.NoIMEIModel nm = null;

            foreach (SlModel.ViewModel vm in unIMELPros)
            {
                nm = new API.NoIMEIModel();
                nm.ProID = vm.ProID;
                nm.ProCount = vm.ProCount;
                list.Add(nm);
            }
            //}
            GridCheckedPro.Rebind();
            //if (uncheckIMEI.Count == 0 && unIMELPros.Count() == 0)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加选货内容");
            //}

            if (string.IsNullOrEmpty(aduitid))
            {
                PublicRequestHelp help2 = new PublicRequestHelp(this.Busy, 12, new object[] { imeilist, hid, list },
                               new EventHandler<API.MainCompletedEventArgs>(CheckedCompleted));
            }
            else
            {
                PublicRequestHelp help = new PublicRequestHelp(this.Busy, 75, new object[] { imeilist, hid, aduitid, list },
                               new EventHandler<API.MainCompletedEventArgs>(CheckedCompleted));
            }

        }

        private void CheckedCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.Busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "拣货失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                //MessageBox.Show(System.Windows.Application.Current.MainWindow,"拣货成功");
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"拣货失败");
            }
            Logger.Log("拣货完毕");

            CheckedIMEI(e.Result.ArrList);
            CheckedNoIMEI(e.Result.ArrList);

            if (!e.Result.ReturnValue)
            {
                checkedModels.Clear();
            }
            GridCheckedPro.Rebind();
        }

        /// <summary>
        /// 无串码商品拣货完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedNoIMEI(List<object> arr)
        {

            List<API.SetSelection> models =arr[3] as List<API.SetSelection>;

            //if (models == null || models.Count() == 0)
            //{
            //    foreach (SlModel.ViewModel cm in unIMELPros)
            //    {
            //        if (cm.Note.Trim() == "")
            //        {
            //            cm.Note = "库存不足";
            //        }
            //    }
            //    GridUnCheckPro.Rebind();
            //    return;
            //}
         
               foreach (API.SetSelection s in models)
               {
                    foreach (SlModel.ViewModel vi in unIMELPros)
                    {
                        if (vi.ProID == s.Proid)
                        {
                            vi.Note = "成功";
                        }
                    }
                    if (Convert.ToBoolean(arr[2].ToString()))
                    {
                        SlModel.ViewModel model = new SlModel.ViewModel();
                        var product = from b in Store.ProInfo
                                        where b.ProID == s.Proid
                                        select b;
                        model.ProID = s.Proid;
                        model.Pro_TypeID = product.First().Pro_TypeID.ToString() ;
                        model.Pro_ClassID = product.First().Pro_ClassID.ToString();

                        var type = from t in Store.ProTypeInfo
                                    where t.TypeID == int.Parse(model.Pro_TypeID)
                                    select t;
                        model.TypeName = type.First().TypeName;

                        var ca = from c in Store.ProClassInfo
                                    where c.ClassID == int.Parse(model.Pro_ClassID)
                                    select c;
                        model.ClassName = ca.First().ClassName;

                        model.ProName = product.First().ProName;
                        model.Inlist = s.InList;
                        model.IMEI = s.ReturnIMEI;
                        model.ProCount = s.Countnum;
                        checkedModels.Add(model);
                   }
             }
            

            //if (uncheckIMEI != null && uncheckIMEI.Count() != 0)
            //{
            //    var query = from b in uncheckIMEI
            //                where b.Note != "成功"
            //                select b;
            //    if (query.Count() != 0)
            //    {
            //        GridUnCheckPro.Rebind();
            //        checkedModels.Clear();
            //        return;
            //    }
            //}
            //var query1 = from b in unIMELPros
            //             where b.Note != "成功"|| b.Note.Trim()!=""
            //             select b;
            //if (query1.Count() != 0)
            //{
            //    checkedModels.Clear();
            //    GridUnCheckPro.Rebind();
            //    return;
            //}
            foreach (SlModel.ViewModel cm in unIMELPros)
            {
                if (cm.Note.Trim() == "")
                {
                    cm.Note = "库存不足";
                }
            }
            //GridUnCheckPro.Rebind();

            GridCheckedPro.Rebind();
            GridUnCheckPro.Rebind();
        }

        /// <summary>
        /// 串码拣货完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedIMEI(List<object> arr)
        {
            if (arr.Count == 0) { return; }
            List<string> zslist = new List<string>();
            if (uncheckIMEI.Count() != 0)
            {
                foreach (var i in uncheckIMEI)
                {
                    zslist.Add(i.IMEI);
                }
            }
            List<API.SetSelection> models =arr[1] as List<API.SetSelection>;

            if (models == null || models.Count() == 0)
            {
                if (uncheckIMEI.Count != 0 )
                {
                    foreach (SlModel.CheckModel cm in uncheckIMEI)
                    {
                        cm.Note = "失败";
                    }
                    GridUnCheckIMEI.Rebind();
                }
                return;
            }
            foreach (var s in models)
            {
                foreach (SlModel.CheckModel cm in uncheckIMEI)
                {
                    var query_zq = from b in models
                                   where b.ReturnIMEI.Contains(cm.IMEI.ToUpper())
                                   select b;
                    if (query_zq != null && query_zq.Count() != 0)
                    {
                        cm.Note = "成功";
                    }
                    else
                    {
                        cm.Note = "无此串码或存在其他操作";
                    }
                }
                if (Convert.ToBoolean(arr[0].ToString()))
                {
                    SlModel.ViewModel model = new SlModel.ViewModel();
                    var product = from b in Store.ProInfo
                                  where b.ProID == s.Proid
                                  select b;
                    model.ProID = s.Proid;
                    model.Pro_TypeID = product.First().Pro_TypeID.ToString();
                    model.Pro_ClassID = product.First().Pro_ClassID.ToString();
                    model.IsNeedIMEI = product.First().NeedIMEI;
                    var type = from t in Store.ProTypeInfo
                               where t.TypeID == int.Parse(model.Pro_TypeID)
                               select t;
                    model.TypeName = type.First().TypeName;

                    var ca = from c in Store.ProClassInfo
                             where c.ClassID == int.Parse(model.Pro_ClassID)
                             select c;
                    model.ClassName = ca.First().ClassName;

                    model.ProName = product.First().ProName;
                    model.Inlist = s.InList;
                    model.IMEI = s.ReturnIMEI;
                    model.ProCount = s.Countnum;
                    checkedModels.Add(model);
                }
            } 
            GridUnCheckIMEI.Rebind();
            //var query = from b in uncheckIMEI
            //            where b.Note != "成功"
            //            select b;
            //if (query.Count() != 0)
            //{
            //    GridUnCheckIMEI.Rebind();
            //    return;
            //}
          
            //if (unIMELPros!= null&&unIMELPros.Count()!=0)
            //{
            //    var query1 = from b in uncheckIMEI
            //                 where b.Note != "成功"||b.Note!="失败"
            //                 select b;
            //    if (query1.Count() != 0)
            //    {
            //        return;
            //    }
            //}  
            //GridCheckedPro.Rebind();
        }

    }
}
