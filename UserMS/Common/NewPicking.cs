using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    public class NewPicking
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
        public NewPicking(
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

            PublicRequestHelp help = new PublicRequestHelp(Isbusy, 66, new object[] { SubModels }, CheckedCompleted);
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
                        unIMELPros.Clear();

                        //获取成功后的商品列表
                        var query_UnIMEI = (from b in ReModels
                                            where b.IsNeedIMEI == false
                                            group b by b.ProID into g
                                            select new
                                            {
                                                g.Key,
                                                g.First().ProFormat,
                                                g.First().ISdecimals,
                                                g.First().ProClassName,
                                                g.First().ProTypeName,
                                                g.First().ProName,
                                                g.First().IsNeedIMEI,
                                                g.First().IsServicePro,
                                                g.First().Note,
                                                total = g.Select(p => p.Count).Sum()
                                            }).ToList();
                        if (query_UnIMEI.Count() != 0)
                        {
                            foreach (var UnIMEIPro in query_UnIMEI)
                            {
                                API.SeleterModel model = new API.SeleterModel();
                                model.ProID = UnIMEIPro.Key;
                                model.ProClassName = UnIMEIPro.ProClassName;
                                model.ProTypeName = UnIMEIPro.ProTypeName;
                                model.ProName = UnIMEIPro.ProName;
                                model.NewNote = UnIMEIPro.Note;
                                model.ISdecimals = UnIMEIPro.ISdecimals;
                                model.Count = decimal.Parse(UnIMEIPro.total.ToString("#0.00"));
                                unIMELPros.Add(model);
                            }
                        }
                        //获取成功后的串码列表
                        var query_IMEI = from b in ReModels
                                          where b.IsNeedIMEI == true 
                                          select b.IsIMEI;
                        if (query_IMEI.Count()!= 0)
                        {
                            foreach (var imei in query_IMEI)
                            {
                                uncheckIMEI.AddRange(imei);
                            }
                          
                        }
                        if (e.Result.ArrList != null && e.Result.ArrList.Count != 0)
                        {
                            checkedModels.AddRange(ReModels);
                        }

                    }
                }
                GVCheckedPro.Rebind();
                GVUnCheckPro.Rebind();
                try
                {
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
