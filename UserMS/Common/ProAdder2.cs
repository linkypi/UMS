using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Telerik.Windows.Controls;
using UserMS.MyControl;

namespace UserMS.Common
{
    public class ProAdder2<T> where T : class, new()
    {
        private List<T> oldmodels;
        private RadGridView GridUnCheckPro;
        private NoFileterMulSelect msFrm = null;
        private List<TreeViewModel> treeModels;
        private List<SlModel.BaseModel> models =  new List<SlModel.BaseModel>();

        private event EventHandler<MyEventArgs> AddCompleted;

        public ProAdder2()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Models">待添加实体</param>
        /// <param name="ProGrid">Grid</param>
        /// <param name="pros"></param>
        public ProAdder2( List<T> Models, RadGridView ProGrid,List<API.Pro_ProInfo> pros)
        {
            Init(Models, ProGrid, pros);
         
        }

        public ProAdder2(List<T> Models, RadGridView ProGrid, List<API.Pro_ProInfo> pros,EventHandler<MyEventArgs> addCompleted)
        {
            AddCompleted += addCompleted;
            Init(Models, ProGrid, pros);

        }

        private void Init(List<T> Models, RadGridView ProGrid, List<API.Pro_ProInfo> pros)
        {
            this.oldmodels = Models;
            this.GridUnCheckPro = ProGrid;

            treeModels = new List<TreeViewModel>();

            SlModel.BaseModel bm = null;

            models = new List<SlModel.BaseModel>();
            var query = (from b in pros
                         join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                         join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                         orderby c.ClassName
                         select new
                         {
                             ProID = b.ProID,
                             ProName = b.ProName,
                             ProFormat = b.ProFormat,
                             NeedIMEI = b.NeedIMEI,
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
                bm.NeedIMEI = item.NeedIMEI;
                bm.ClassName = item.ClassName;
                bm.Pro_ClassID = item.ClassID.ToString();
                bm.TypeName = item.TypeName;
                bm.Pro_TypeID = item.TypeID.ToString();
                bm.IsDecimal = Convert.ToBoolean(item.ISdecimals);

                TreeViewModel p2 = null;
                if ((p2 = Exist(item.ClassName)) == null)
                {
                    p2 = new TreeViewModel();
                    p2.Fields = new string[] { "ClassName", "Pro_ClassID" };
                    p2.Values = new object[] { item.ClassName, item.ClassID.ToString() };
                    p2.ID = item.ClassID.ToString();
                    p2.Title = item.ClassName;
                    p2.Children = new List<TreeViewModel>();

                    TreeViewModel t = new TreeViewModel();
                    t.Fields = new string[] { "TypeName", "ClassName" };
                    t.Values = new object[] { item.TypeName, item.ClassName };
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
                        t.Values = new object[] { item.TypeName, item.ClassName };
                        t.ID = item.TypeID.ToString();
                        t.Title = item.TypeName;
                        p2.Children.Add(t);
                    }
                }
                models.Add(bm);
            }
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

        private bool ExistChild(string name, TreeViewModel model)
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
     

        /// <summary>
        /// 添加无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Add()
        {
            msFrm = new NoFileterMulSelect(
               treeModels,
               models, "ProName",
               new string[] { "ClassName", "TypeName", "ProName", "ProFormat","NeedIMEI" },
               new string[] { "商品品牌", "商品类别", "商品型号", "商品属性","需要串码" });
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
            List<SlModel.BaseModel> piList = ((NoFileterMulSelect)sender).SelectedItems.OfType<SlModel.BaseModel>().ToList();
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
                            if (child.Name == item2.Name && child.PropertyType == item2.PropertyType && child.GetSetMethod() != null)//Type.GetType("System.String")
                            {
                                child.SetValue(t, Convert.ChangeType(item2.GetValue(item, null), item2.PropertyType), null);
                                //child.SetValue(t, item2.GetValue(item, null), null);
                              
                                break;
                            }
                        }
                    }
                    //pis.Where(p => p.Name == "ProCount").First().SetValue(t, 
                    //    Convert.ChangeType(1, typeof(System.Decimal)), null);
                    oldmodels.Add(t);
                }
            }
            GridUnCheckPro.Rebind();

            if (AddCompleted != null)
            {
                AddCompleted(sender, new MyEventArgs() { });
            }
        }


        public object ToType(Type type, string value)
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
            foreach (var vm in oldmodels)
            {
                PropertyInfo pi = vm.GetType().GetProperty("ProID");

                if (pi.GetValue(vm, null).ToString() == pid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
