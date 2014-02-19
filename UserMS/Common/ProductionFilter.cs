using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    public class ProductionFilter
    {
        public ProductionFilter()
        { }
        List<SlModel.ViewModel> vmodels;
        List<API.SeleterModel> unIMEIModels;
        RadGridView DGCardType;
        List<SlModel.ProductionModel> production;
        /// <summary>
        /// 入库商品添加器
        /// </summary>
        /// <param name="Vmodel"></param>
        /// <param name="DGproduction"></param>
        public ProductionFilter(ref List<SlModel.ViewModel> Vmodel, ref RadGridView DGproduction)
        {
            vmodels = Vmodel;
            DGCardType = DGproduction;
        }
        public ProductionFilter(ref List<API.SeleterModel> unIMEIModels, ref RadGridView DGproduction)
        {
            this.unIMEIModels = unIMEIModels;
            DGCardType = DGproduction;
        }
        /// <summary>
        /// 获取可操作权限商品
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public List<API.Pro_ProInfo> GetPro(int menuID)
        {
            var menuInfo = Store.LoginRoleInfo.Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
            if (menuInfo != null && menuInfo.Count() > 0)
            {
                // 左连接获取商品列表              
                var query = (from b in Store.LoginRoleInfo.Sys_Role_Menu_ProInfo
                             where b.MenuID == menuID
                             join c in Store.ProInfo
                             on b.ClassID equals c.Pro_ClassID
                             select c).Distinct().ToList();
                if (query.Count() > 0)
                    return query;

            }
            return null;
        }
    
        /// <summary>
        /// 筛选商品
        /// </summary>
        /// <param name="proInfo"></param>
        /// <param name="hasIMEI"></param>
        public void ProFilter(List<API.Pro_ProInfo> proInfo, bool hasIMEI)
        {
            if (proInfo == null || proInfo.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无商品权限，请联系管理员！");
                return;
            }
            //获取左边树
            List<TreeViewModel> ParentTree = new List<TreeViewModel>();

            if (hasIMEI == false)
            {
                proInfo = (from b in proInfo
                           where b.NeedIMEI == false
                           select b).ToList();
            }
            var classList = from b in proInfo
                            join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                            join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                            group d by c into TypeList
                            select new
                            {
                                TypeList.Key,
                                k = TypeList.Distinct().ToList()
                            };
            if (classList.Count() == 0)
            {
                return;
            }
            foreach (var list in classList)
            {
                if (list.Key == null)
                {
                    continue;
                }
                TreeViewModel ParentModel = new TreeViewModel();

                ParentModel.Title = list.Key.ClassName;

                if (ParentModel.Fields == null)
                {
                    ParentModel.Fields = new string[] { "ClassID" };
                    ParentModel.Values = new object[] { list.Key.ClassID.ToString() };

                }

                ParentModel.Children = new List<TreeViewModel>();

                foreach (var Newlist in list.k)
                {
                    TreeViewModel ChildModel = new TreeViewModel() { ID = Newlist.TypeID.ToString(), Title = Newlist.TypeName };
                    if (ChildModel.Fields == null)
                    {
                        ChildModel.Fields = new string[] { "ClassID", "TypeID" };
                        ChildModel.Values = new object[] { list.Key.ClassID.ToString(), Newlist.TypeID.ToString() };

                    }
                    ParentModel.Children.Add(ChildModel);
                }
                ParentTree.Add(ParentModel);
            }

            //三向连接获取商品信息
            if (production == null)
            {
                production = new List<SlModel.ProductionModel>();
                var query = (from b in proInfo
                             join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                             join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                             select new
                             {
                                 ProID = b.ProID,
                                 ProName = b.ProName,
                                 ProFormat = b.ProFormat,
                                 IsNeedIMEI = b.NeedIMEI,
                                 ClassName = c.ClassName,
                                 ClassID = c.ClassID,
                                 TypeID = d.TypeID,
                                 TypeName = d.TypeName,
                                 b.ISdecimals,
                                 
                             }).ToList();
                foreach (var index in query)
                {
                    SlModel.ProductionModel pro = new SlModel.ProductionModel();
                    pro.Isdecimals = index.ISdecimals;
                    pro.ProID = index.ProID;
                    pro.ProName = index.ProName;
                    pro.ProFormat = index.ProFormat;
                    pro.IsNeedIMEI = index.IsNeedIMEI;
                    pro.ClassName = index.ClassName;
                    pro.ClassID = index.ClassID.ToString();
                    pro.TypeName = index.TypeName;
                    pro.TypeID = index.TypeID.ToString();
                    pro.Isdecimals = index.ISdecimals;
                    production.Add(pro);

                }
            }
            MultSelecter2 msFrm = new MultSelecter2(
            ParentTree,
            production, "ProName",
            new string[] { "ClassName", "TypeName", "ProName", "ProFormat", "Isdecimals", "IsNeedIMEI" },
            new string[] { "商品类别", "商品品牌", "商品型号", "商品属性","可为小数","需要串码"}
          );
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();
        
        }

 
        /// <summary>
        /// 确定添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       void msFrm_Closed(object sender, WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<SlModel.ProductionModel> piList = result.SelectedItems.OfType<SlModel.ProductionModel>().ToList();
                if (piList.Count == 0) return;

                SlModel.ViewModel vm;
                if (vmodels != null)
                {
                    foreach (SlModel.ProductionModel ppi in piList)
                    {
                        if (!ValidateProc(ppi.ProID))
                        {
                            vm = new SlModel.ViewModel();
                            vm.ProID = ppi.ProID;
                            vm.Pro_ClassID = ppi.ClassName;
                            vm.Pro_TypeID = ppi.TypeName;
                            vm.ProName = ppi.ProName;
                            vm.ProFormat = ppi.ProFormat;
                            vm.IsNeedIMEI = ppi.IsNeedIMEI;
                            vm.IsDecimal = ppi.Isdecimals;
                            vm.ProCount = 0;
                         
                            vmodels.Add(vm);
                        }
                    }
                }
                API.SeleterModel model;
                if (unIMEIModels != null)
                {
                    foreach (SlModel.ProductionModel ppi in piList)
                    {
                        if (!ValidateProduction(ppi.ProID))
                        {
                            model = new API.SeleterModel();
                            model.ISdecimals = ppi.Isdecimals;
                            model.ProID = ppi.ProID;
                            model.ProFormat = ppi.ProFormat;
                            model.ClassID = Convert.ToInt32(ppi.ClassID);
                            model.ProClassName = ppi.ClassName;
                            model.TypeID = Convert.ToInt32(ppi.TypeID);
                            model.ProTypeName = ppi.TypeName;
                            model.ProName = ppi.ProName;
                            model.ProFormat = ppi.ProFormat;
                            model.IsNeedIMEI = ppi.IsNeedIMEI;
                            model.Count = 0;
                            model.ProCount = 0;
                            unIMEIModels.Add(model);
                        }
                    }
                }
              //  DGCardType.ItemsSource = unIMEIModels;
                DGCardType.Rebind();
            }
        }
        /// <summary>
        /// 验证是否存在同种商品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool ValidateProc(string pid)
        {
            if (vmodels == null)
                return false;
            foreach (SlModel.ViewModel vm in vmodels)
            {
                if (vm.ProID == pid)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ValidateProduction(string pid)
        {
            if (unIMEIModels == null)
                return false;
            foreach (API.SeleterModel vm in unIMEIModels)
            {
                if (vm.ProID == pid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
