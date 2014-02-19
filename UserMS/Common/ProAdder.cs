using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    /// <summary>
    /// 商品添加器（添加无串码商品）
    /// </summary>
    public class ProAdder<T> where T : class, new()
    {
        private List<T> unIMEIModels;
        private RadGridView GridUnCheckPro;
        private MultSelecter2 msFrm = null;
        private List<TreeViewModel> treeModels;
        private List<SlModel.BaseModel> models = null;

        public ProAdder()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unimeiModels">无串码商品列表</param>
        /// <param name="uncheckProGrid">未拣货的商品Grid</param>
        /// <param name="menuID"></param>
        /// <param name="flag">1 表示获取有串码商品  2 无串码商品   3全部获取</param>
        public ProAdder(ref List<T> unimeiModels,ref RadGridView uncheckProGrid, int menuID,int flag )
        {
            this.unIMEIModels = unimeiModels;
            this.GridUnCheckPro = uncheckProGrid;

            treeModels = new List<TreeViewModel>();

            List<API.Pro_ProInfo> prods = new List<API.Pro_ProInfo>();
            var menuInfo = Store.RoleInfo.First().Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
            if (menuInfo != null)
            {
                bool need = false;
                if (flag == 3)
                {
                    // 左连接获取商品列表              
                   var  query = (from b in Store.ProInfo
                             join c in Store.RoleInfo.First().Sys_Role_Menu_ProInfo
                             on b.Pro_ClassID equals c.ClassID
                             where c.MenuID == menuID
                             select b).ToList();
                    if (query.Count() > 0)
                    {
                        prods = query;
                    }
                }
                else
                {
                    if (flag == 1)
                    {
                        need = true;
                    }
                    if (flag == 2)
                    {
                        need = false;
                    }
                    // 左连接获取商品列表              
                    var query = (from b in Store.ProInfo
                             join c in Store.RoleInfo.First().Sys_Role_Menu_ProInfo
                             on b.Pro_ClassID equals c.ClassID
                             where c.MenuID == menuID
                             && b.NeedIMEI == need
                             select b).ToList();
                    if (query.Count() > 0)
                    {
                        prods = query;
                    }
                }
            }
            SlModel.BaseModel bm = null;

            if (models == null)
            {
                models = new List<SlModel.BaseModel>();
                var query = (from b in prods
                             join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                             join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                             orderby c.ClassName
                             select new
                             {
                                 ProID = b.ProID,
                                 ProName = b.ProName,
                                 ProFormat = b.ProFormat,
                                 IsNeedIMEI = b.NeedIMEI,
                                 ClassName = c.ClassName,
                                 ClassID = c.ClassID,
                                 TypeName = d.TypeName,
                                 
                                 d.TypeID,
                                 b.ISdecimals
                             }).ToList();
                foreach (var item in query)
                {
                    bm = new SlModel.BaseModel();
                    bm.ProID = item.ProID;
                    bm.ProName = item.ProName;
                    bm.ProFormat = item.ProFormat;
                    bm.ClassName = item.ClassName;
                    bm.Pro_ClassID = item.ClassID.ToString();
                    bm.TypeName = item.TypeName;
                    bm.Pro_TypeID = item.TypeID.ToString();
                    bm.IsDecimal =Convert.ToBoolean(item.ISdecimals) ;

                    TreeViewModel p2 = null;
                    if ((p2 = Exist(item.ClassName)) == null)
                    {
                        p2 = new TreeViewModel();
                        p2.Fields = new string[] { "ClassName" ,"Pro_ClassID"};
                        p2.Values = new object[] { item.ClassName ,item.ClassID.ToString()};
                        p2.ID = item.ClassID.ToString();
                        p2.Title = item.ClassName;
                        p2.Children = new List<TreeViewModel>();

                        TreeViewModel t = new TreeViewModel();
                        t.Fields = new string[] { "TypeName", "ClassName"};
                        t.Values = new object[] { item.TypeName ,item.ClassName};
                        t.ID = item.TypeID.ToString();
                        t.Title = item.TypeName;
                        p2.Children.Add(t);
                        treeModels.Add(p2);
                    }
                    else
                    {
                        if (!ExistChild(item.TypeName, p2))
                        {
                            TreeViewModel t = new TreeViewModel();
                            t.Fields = new string[] { "TypeName", "ClassName" };
                            t.Values = new object[] { item.TypeName,item.ClassName};
                            t.ID = item.TypeID.ToString();
                            t.Title = item.TypeName;
                            p2.Children.Add(t);
                        }
                    }
                    models.Add(bm);
                }
            }
          // PublicRequestHelp prh = new PublicRequestHelp(null, 114, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetTreeCompleted));
  
        }

        private TreeViewModel Exist(string name)
        {
            foreach (var item in treeModels)
            {

                if (item.Title == name)
                {
                    return item;
                }
            }
            return null;
        }

        private bool ExistChild(string name,TreeViewModel model)
        {
            foreach (var item in model.Children)
            {
                if (item.Title == name)
                {
                    return true;
                }
            }
            return false;
        }
        //private void GetTreeCompleted(object sender, API.MainCompletedEventArgs e)
        //{
        //  //  this.busy.IsBusy = false;
        //    if (e.Result.ReturnValue)
        //    {
        //        #region  获取左树

        //        List<API.TreeModel> list = e.Result.Obj as List<API.TreeModel>;
        //        List<string> proidList = e.Result.ArrList[0] as List<string>;

        //        List<TreeViewModel> mods = new List<TreeViewModel>();
        //        foreach (var item in list)
        //        {
        //            TreeViewModel p2 = new TreeViewModel();
        //            p2.Fields = new string[] { "TypeName", "TypeID" };
        //            p2.Values = new object[] { item.Name, item.ID.ToString() };
        //            p2.ID = item.ID.ToString();
        //            p2.Title = item.Name;
        //            p2.Children = new List<TreeViewModel>();
        //            foreach (var child in item.Children)
        //            {
        //                TreeViewModel t = new TreeViewModel();
        //                t.Fields = new string[] { "ClassName", "ClassID" };
        //                t.Values = new object[] { child.Name, child.ID.ToString() };
        //                t.ID = child.ID;
        //                t.Title = child.Name;
        //                p2.Children.Add(t);
        //            }
        //            mods.Add(p2);
        //        }

        //        treeModels.AddRange(mods);
        //        #endregion
        //    }
        //}

        /// <summary>
        /// 添加无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Add()
        {
             msFrm = new MultSelecter2(
                treeModels,
                models, "ProName", 
                new string[] { "ClassName",  "TypeName","ProName", "ProFormat" },
                new string[] {"商品品牌",  "商品类别", "商品型号", "商品属性" });
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();
        }

        /// <summary>
        /// 确定添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void msFrm_Closed(object sender, WindowClosedEventArgs e)
        {
            List<SlModel.BaseModel> piList = ((UserMS.MultSelecter2)sender).SelectedItems.OfType<SlModel.BaseModel>().ToList();
            if (piList.Count == 0) return;

            SlModel.BaseModel bm = new SlModel.BaseModel();

            PropertyInfo[] bpis = bm.GetType().GetProperties();

            foreach (SlModel.BaseModel item in piList)
            {
                if (!ValidateProc(item.ProID))
                {
                    T t = new T();
                    PropertyInfo[] pis = t.GetType().GetProperties();
                    foreach (var child in pis)
                    {
                        foreach (var item2 in bpis)
                        {                                                     // Type.GetType("System.String")
                            if (child.Name == item2.Name && child.PropertyType ==item2.PropertyType && child.GetSetMethod()!=null)//Type.GetType("System.String")
                            {  
                                child.SetValue(t, Convert.ChangeType(item2.GetValue(item, null), item2.PropertyType),null);
                                //child.SetValue(t, item2.GetValue(item, null), null);
                                break;
                            }
                        }
                    }
                    unIMEIModels.Add(t);
                }
            } 
            GridUnCheckPro.Rebind();
        }


        public  object ToType(Type type, string value)
        {
            if (type == typeof(string))
            {
                return value;
            }

            MethodInfo parseMethod = null;

            foreach (MethodInfo mi in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                if (mi.Name == "Parse" && mi.GetParameters().Length == 1)
                {
                    parseMethod = mi;
                    break;
                }
            }

            if (parseMethod == null)
            {
                throw new ArgumentException(string.Format("Type: {0} has not Parse static method!", type));
            }

            return parseMethod.Invoke(null, new object[] { value });
        }

        /// <summary>
        /// 验证是否存在同种商品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool ValidateProc(string pid)
        {
           // List<T> list = GridUnCheckPro.selete
            foreach (var vm in unIMEIModels)
            {
                 PropertyInfo pi = vm.GetType().GetProperty("ProID");
               
                if (   pi.GetValue(vm,null).ToString()== pid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
