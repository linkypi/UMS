using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
   public class NotNewBaseProFilter
    {
     //  List<T> DateSource { get; set; }
       List<API.SeleterModel> Source;
       RadGridView DateGrid;
       public NotNewBaseProFilter(ref List<API.SeleterModel> source, ref RadGridView dateGrid)
        {
            Source = source;
            DateGrid = dateGrid;
        }
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

        

        #region 添加子商品
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
                    ParentModel.Values = new object[] { list.Key.ClassID };

                }

                ParentModel.Children = new List<TreeViewModel>();

                foreach (var Newlist in list.k)
                {
                    TreeViewModel ChildModel = new TreeViewModel() { ID = Newlist.TypeID.ToString(), Title = Newlist.TypeName };
                    if (ChildModel.Fields == null)
                    {
                        ChildModel.Fields = new string[] { "ClassID", "TypeID" };
                        ChildModel.Values = new object[] { list.Key.ClassID, Newlist.TypeID };

                    }
                    ParentModel.Children.Add(ChildModel);
                }
                ParentTree.Add(ParentModel);
            }

            //三向连接获取商品信息

            List<API.SeleterModel> DateSource = new List<API.SeleterModel>();
            var query = (from b in proInfo
                         join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                         join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                         select new
                         {
                             ProID = b.ProID,
                             ProName = b.ProName,
                             ProFormat = b.ProFormat,
                             IsNeedIMEI = b.NeedIMEI,
                             IsService=b.IsService,
                             ClassName = c.ClassName,
                             ClassID = c.ClassID,
                             TypeID = d.TypeID,
                             TypeName = d.TypeName,
                             b.ISdecimals
                         }).ToList();
            foreach (var index in query)
            {
                API.SeleterModel pro = new API.SeleterModel();
                pro.ProID = index.ProID;
                pro.ProName = index.ProName;
                pro.ProFormat = index.ProFormat;
                pro.IsNeedIMEI = index.IsNeedIMEI;
                pro.IsServicePro = index.IsService;
                pro.ProClassName = index.ClassName;
                pro.ClassID = index.ClassID;
                pro.ProTypeName = index.TypeName;
                pro.TypeID = index.TypeID;
                pro.ISdecimals = index.ISdecimals;
                DateSource.Add(pro);
            }

            MultSelecter2 msFrm = new MultSelecter2(
            ParentTree,
            DateSource, "ProName",
            new string[] { "ProClassName", "ProTypeName", "ProName", "IsNeedIMEI", "IsServicePro", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "需要串码","属于服务","商品属性" }
          );
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();

        }
        #endregion 

        #region 确定添加商品
        void msFrm_Closed(object sender, WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<API.SeleterModel> piList = result.SelectedItems.OfType<API.SeleterModel>().ToList();
                if (piList.Count == 0) return;
                Source.AddRange(GetOther(piList));
                this.DateGrid.ItemsSource = Source;
                this.DateGrid.Rebind();
            }
        }
        private List<API.SeleterModel> GetOther(List<API.SeleterModel> piList)
        {
            List<string> ProIDList = (from b in Source
                                      select b.ProID).ToList();
            piList = (from b in piList
                     where !ProIDList.Contains(b.ProID)
                     select b).ToList();
            return piList;
        }
        #endregion 
    }
}



