using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace UserMS.Common
{
    public class Pro_GetPro
    {
        private MyAutoTextBox hall;
        private RadBusyIndicator Isbusy;
        private List<API.SeleterModel> checkedModels;
        private List<API.SeleterModel> ReModels;
        private List<API.SeleterModel> SubModels;
        private List<API.SeleterModel> unIMELPros;
        private List<API.SelecterIMEI> uncheckIMEI;
        private RadGridView GVCheckedPro;
        private RadGridView GVUnCheckPro;
        private RadGridView GVUnCheckIMEI;
        public Pro_GetPro(
            ref MyAutoTextBox hall,
            ref RadBusyIndicator Isbusy,
            ref List<API.SeleterModel> checkedModels,
            ref List<API.SeleterModel> unIMELPros,
            ref List<API.SelecterIMEI> uncheckIMEI,
            ref RadGridView GVCheckedPro,
            ref RadGridView GVUnCheckPro,
            ref RadGridView GVUnCheckIMEI)
        {
            this.hall = hall;
            this.Isbusy = Isbusy;
            this.checkedModels = checkedModels;
            this.unIMELPros = unIMELPros;
            this.uncheckIMEI = uncheckIMEI;
            this.GVCheckedPro = GVCheckedPro;
            this.GVUnCheckPro = GVUnCheckPro;
            this.GVUnCheckIMEI = GVUnCheckIMEI;
        }

        public void Packing()
        {
            //清除已拣列表
            if (checkedModels != null)
                checkedModels.Clear();
            GVCheckedPro.Rebind();
            foreach (API.SeleterModel vm in unIMELPros)
            {
                if (vm.Count == 0 )
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"拣货数量不能为0");
                    return;
                }   
                if(vm.IsNeedIMEI==false&&vm.NewIsNeedIMEI==true)
                {
                    if (vm.IsIMEI == null || vm.Count != vm.IsIMEI.Count())
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "添加新串码的数量应和商品数量一致！");
                        return;
                    }
                }
            }
            //捡货
            if (unIMELPros == null && uncheckIMEI == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加商品或串码");
                return;
            }
            SubModels = new List<API.SeleterModel>();
            if (uncheckIMEI != null && uncheckIMEI.Count() != 0)
            {
                API.SeleterModel model = new API.SeleterModel();
                model.IsIMEI = uncheckIMEI;
                model.IsNeedIMEI = true;
                SubModels.Add(model);
            }
            if (unIMELPros != null && unIMELPros.Count() != 0)
                SubModels.AddRange(unIMELPros);
            if (hall.Tag == null&&string.IsNullOrEmpty(hall.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加仓库");
                return;
            }
            string hallID;
            try
            {
                hallID = hall.Tag.ToString();
            }
            catch
            {
                var query = (from b in Store.ProHallInfo
                         where b.HallName == hall.Text
                         select b.HallID).ToList();
                if (query.Count == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"无此仓库，请联系管理员！");
                    return;
                }
                hallID =query.First();
                hall.Tag = hallID;
            }
            foreach (var index in SubModels)
            {
                index.HallID = hallID;
            }

            PublicRequestHelp help = new PublicRequestHelp(Isbusy, 238, new object[] { SubModels }, CheckedCompleted);
        }
        private void CheckedCompleted(object sender, API.MainCompletedEventArgs e)
        {
            Isbusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.Obj != null)
                {
                    ReModels = e.Result.Obj as List<API.SeleterModel>;
                    //成功返回列表
                    if (e.Result.ReturnValue == true)
                    {
                        uncheckIMEI.Clear();


                        //获取成功后的商品列表
                        //var query_UnIMEI = (from b in ReModels
                        //                    group b by b.ProID into g
                        //                    select new
                        //                    {
                        //                        g.Key,
                        //                        g.First().ProFormat,
                        //                        g.First().ISdecimals,
                        //                        g.First().ProClassName,
                        //                        g.First().ProTypeName,
                        //                        g.First().ProName,
                        //                        g.First().IsNeedIMEI,
                        //                        g.First().IsServicePro,
                        //                        g.First().Note,

                        //                        g.First().NewProID,
                        //                        g.First().NewClassName,
                        //                        g.First().NewTypeName,
                        //                        g.First().NewProName,
                        //                        g.First().NewIsNeedIMEI,
                        //                        g.First().NewCount,
                        //                        g.First().NewNote,
                        //                        g.First().NewProFormat,
                        //                        g.First().IsIMEI,
                        //                        total = g.Select(p => p.Count).Sum()
                        //                    }).ToList();
                        //if (query_UnIMEI.Count() != 0)
                        //{
                        foreach (var UnIMEIPro in unIMELPros)
                        {
                            UnIMEIPro.Note = "成功";
                            if (UnIMEIPro.IsNeedIMEI == true)
                            {
                                var queryIMEI = from b in ReModels
                                                where b.ProID == UnIMEIPro.ProID
                                                select b.IsIMEI;
                                UnIMEIPro.IsIMEI = new List<API.SelecterIMEI>();
                                foreach (var stringList in queryIMEI)
                                {
                                    UnIMEIPro.IsIMEI.AddRange(stringList);
                                }

                            }

                        }
                        //}
                        //获取成功后的串码列表
                        var query_IMEI = from b in unIMELPros
                                         where b.IsNeedIMEI == false && b.IsIMEI != null && b.IsIMEI.Count() > 0
                                         select new
                                         {
                                             b.ProID,
                                             b.IsIMEI
                                         };
                        var ReModel = from b in ReModels
                                      join c in query_IMEI on b.ProID equals c.ProID
                                      select b;
                        if (query_IMEI.Count() != 0)
                        {
                            foreach (var item in unIMELPros)
                            {
                                var query = from b in ReModel
                                            where b.ProID == item.ProID
                                            select b;
                                int i = 0;
                                if (query.Count() > 0)
                                {
                                    foreach (var queryItem in query)
                                    {
                                        if (queryItem.IsIMEI == null) queryItem.IsIMEI = new List<API.SelecterIMEI>();
                                        queryItem.IsIMEI.AddRange(item.IsIMEI.Skip(i).Take(Convert.ToInt32(queryItem.Count)));
                                        i += Convert.ToInt32(queryItem.Count);
                                    }
                                }
                            }

                        }
                        if (e.Result.ArrList != null && e.Result.ArrList.Count != 0)
                        {
                            checkedModels.AddRange(ReModels);
                        }

                    }
                    else
                    {
                        unIMELPros.Clear();
                        unIMELPros.AddRange(ReModels);
                    }
                }
                GVCheckedPro.Rebind();
                GVUnCheckPro.Rebind();
                try
                {
                    GVUnCheckPro.SelectedItems.Clear();
                    GVUnCheckIMEI.Rebind();
                }
                catch { }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常，请联系管理员");
            }
        }
    }
}
