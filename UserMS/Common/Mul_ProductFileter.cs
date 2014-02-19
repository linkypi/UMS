using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    public class Mul_ProductFileter
    {
        List<API.SeleterModel> unIMEIModels;


        List<API.ProModel> ProListModel;
        RadGridView DGCardType;


        List<API.Pro_SellTypeProduct> SellTypeProduct;
        List<API.Pro_Property> PropertyModel;
        /// <summary>
        /// 入库商品添加器
        /// </summary>
        /// <param name="Vmodel"></param>
        /// <param name="DGproduction"></param>

        public Mul_ProductFileter(ref List<API.SeleterModel> unIMEIModels, ref RadGridView DGproduction, List<API.Pro_SellTypeProduct> sellTypePro)
        {
            SellTypeProduct = sellTypePro;
            this.unIMEIModels = unIMEIModels;
            DGCardType = DGproduction;

        }
        public Mul_ProductFileter(ref List<API.ProModel> ProList, ref RadGridView DGproduction)
        {
            ProListModel = ProList;
            DGCardType = DGproduction;


        }

        #region 添加总商品
        /// <summary>
        /// 筛选商品
        /// </summary>
        /// <param name="proInfo"></param>
        /// <param name="hasIMEI"></param>
        public void GetMainPro()
        {

            //获取左边树
            List<TreeViewModel> ParentTree = new List<TreeViewModel>();


            var classList = from b in Store.ProMainInfo
                            join c in Store.ProClassInfo on b.ClassID equals c.ClassID
                            join d in Store.ProTypeInfo on b.TypeID equals d.TypeID
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

            List<API.ProModel> DateSource = new List<API.ProModel>();
            var query = (from b in Store.ProMainInfo
                         join c in Store.ProClassInfo on b.ClassID equals c.ClassID
                         join d in Store.ProTypeInfo on b.TypeID equals d.TypeID
                         select new
                         {
                             b.ProMainID,
                             b.ProMainName,
                             c.ClassID,
                             c.ClassName,
                             d.TypeID,
                             d.TypeName
                         }).ToList();
            foreach (var index in query)
            {
                API.ProModel pro = new API.ProModel();
                pro.ProMainID = index.ProMainID;
                pro.ProName = index.ProMainName;
                pro.ClassID = index.ClassID;
                pro.ProClassName = index.ClassName;
                pro.TypeID = index.TypeID;
                pro.ProTypeName = index.TypeName;
                pro.NeedIMEI = "是";
                pro.IsService = "否";
                pro.Isdecimal = "否";
                pro.IsNeedMoreorLess = "是";
                pro.BeforeSep = "否";
                pro.AfterSep = "否";
                pro.SepDate = null;
                DateSource.Add(pro);
            }

            MultSelecter2 msFrm = new MultSelecter2(
            ParentTree,
            DateSource, "ProName",
            new string[] { "ProClassName", "ProTypeName", "ProName" },
            new string[] { "商品类别", "商品品牌", "商品型号" }
          );
            msFrm.Closed += msFrm_Closed1;
            msFrm.ShowDialog();

        }
        #endregion
        private void msFrm_Closed1(object sender, EventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<API.ProModel> piList = result.SelectedItems.OfType<API.ProModel>().ToList();
                if (piList.Count == 0) return;

                if (ProListModel != null)
                {
                    ProListModel.AddRange(piList);
                    DGCardType.ItemsSource = ProListModel;
                }
                DGCardType.Rebind();

            }
        }


        #region
        //var query_groud = (from b in hallInfo
        //                   group b by b.AreaID into g
        //                   from hall in g.DefaultIfEmpty()
        //                   join c in Store.AreaInfo on hall.AreaID equals c.AreaID
        //                   select new
        //                   {
        //                       g.Key,
        //                       c.AreaName,
        //                       hall
        //                   }).ToList();

        //if (query_groud.Count() > 0)
        //{
        //    TreeViewModel tvm=null;
        //    foreach (var hi in query_groud)
        //    {     
        //        if (tvm == null||hi.Key!=tvm.ID)
        //        {                     
        //            tvm = new TreeViewModel();
        //            tvm.ID = hi.Key;
        //            tvm.Title = hi.AreaName;

        //        }

        //        if (exist(hi.Key)==false)
        //        {                                         
        //            parent.Add(tvm);
        //            tvm.Children = new List<TreeViewModel>();
        //        }

        //        if (exist(hi.Key) == true)
        //        {
        //            TreeViewModel childModel = new TreeViewModel();
        //            childModel.ID = hi.hall.HallID;
        //            childModel.Title = hi.hall.HallName;
        //            tvm.Children.Add(childModel);
        //        }
        //    }
        //}
        #endregion

        #region 优惠添加商品
        /// <summary>
        /// 筛选商品
        /// </summary>
        /// <param name="proInfo"></param>
        /// <param name="hasIMEI"></param>
        public void ProFilter(decimal Rebate, decimal ReduceCash, decimal Point)
        {

            List<API.Pro_SellType> SellTypeList = new List<API.Pro_SellType>();//销售类型
            //获取左边树
            List<TreeViewModel> ParentTree = new List<TreeViewModel>();
            List<TreeViewModel> ChildTree = new List<TreeViewModel>();


            List<API.Pro_ProInfo> ProList = (from b in Store.ProInfo
                                             select b).ToList();

            var query = from b in ProList
                        join c in Store.ProClassInfo
                        on b.Pro_ClassID equals c.ClassID
                        join d in SellTypeProduct
                        on b.ProID equals d.ProID
                        join e in Store.SellTypes
                        on d.SellType equals e.ID
                        group c by e.Name into ClassInfo
                        select new
                        {
                            ClassInfo.Key,
                            ClassInfo = ClassInfo.Distinct().ToList()
                        };
            if (query.Count() == 0)
            {
                return;
            }
            foreach (var list in query)
            {
                if (list.ClassInfo == null)
                {
                    continue;
                }
                TreeViewModel ParentModel = new TreeViewModel();
                ParentModel.Title = list.Key.ToString();

                if (ParentModel.Fields == null)
                {
                    ParentModel.Fields = new string[] { "SellTypeName" };
                    ParentModel.Values = new object[] { list.Key.ToString() };

                }

                ParentModel.Children = new List<TreeViewModel>();

                foreach (var Newlist in list.ClassInfo)
                {

                    TreeViewModel ChildModel = new TreeViewModel() { ID = Newlist.ClassID.ToString(), Title = Newlist.ClassName };
                    if (ChildModel.Fields == null)
                    {
                        ChildModel.Fields = new string[] { "SellTypeName", "ClassID" };
                        ChildModel.Values = new object[] { list.Key.ToString(), Newlist.ClassID };

                    }
                    ParentModel.Children.Add(ChildModel);
                }

                ParentTree.Add(ParentModel);
            }

            //三向连接获取商品信息
            List<API.SeleterModel> production = new List<API.SeleterModel>();
            var queryPro = (from b in ProList
                            join c in SellTypeProduct
                            on b.ProID equals c.ProID
                            join d in Store.SellTypes
                            on c.SellType equals d.ID
                            join e in Store.ProClassInfo on b.Pro_ClassID equals e.ClassID
                            join f in Store.ProTypeInfo on b.Pro_TypeID equals f.TypeID
                            select new
                            {
                                ProID = b.ProID,
                                ProName = b.ProName,
                                ProFormat = b.ProFormat,
                                IsNeedIMEI = b.NeedIMEI,
                                b.NeedIMEI,
                                b.IsService,
                                b.ISdecimals,
                                ClassName = e.ClassName,
                                ClassID = e.ClassID,
                                TypeID = f.TypeID,
                                TypeName = f.TypeName,
                                d.Name,
                                SellTypeID = d.ID,
                                c.Price

                            }).ToList();
            foreach (var index in queryPro)
            {

                API.SeleterModel pro = new API.SeleterModel();
                pro.ProID = index.ProID;
                pro.ProName = index.ProName;
                pro.ProFormat = index.ProFormat;
                pro.IsNeedIMEI = index.IsNeedIMEI;

                pro.ProClassName = index.ClassName;
                pro.ClassID = index.ClassID;
                pro.ProTypeName = index.TypeName;
                pro.TypeID = index.TypeID;
                pro.SellTypeName = index.Name;
                pro.Rate = Rebate;
                pro.ReduceMoney = ReduceCash;
                pro.Point = Point;
                pro.Price = decimal.Parse(index.Price.ToString("#0.00"));
                pro.SellTypeID = index.SellTypeID;
                production.Add(pro);
            }
            MultSelecter2 msFrm = new MultSelecter2(
            ParentTree,
            production, "ProName",
            new string[] { "ProClassName", "ProTypeName", "ProName", "SellTypeName", "Price", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "销售类型", "价格", "商品属性" }
          );
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();

        }
        #endregion


        #region 门店优惠添加商品
        public  void ProFilter(decimal? Price)
        {

            List<API.Pro_SellType> SellTypeList = new List<API.Pro_SellType>();//销售类型
            //获取左边树
            List<TreeViewModel> ParentTree = new List<TreeViewModel>();
            List<TreeViewModel> ChildTree = new List<TreeViewModel>();


            List<API.Pro_ProMainInfo> ProList = (from b in Store.ProMainInfo
                                             select b).ToList();

            var query = from b in ProList
                        join c in Store.ProClassInfo
                        on b.ClassID equals c.ClassID
                        join e in Store.SellTypes
                        on 1 equals 1
                        group c by e.Name into ClassInfo
                        select new
                        {
                            ClassInfo.Key,
                            ClassInfo = ClassInfo.Distinct().ToList()
                        };
            if (query.Count() == 0)
            {
                return;
            }
            foreach (var list in query)
            {
                if (list.ClassInfo == null)
                {
                    continue;
                }
                TreeViewModel ParentModel = new TreeViewModel();
                ParentModel.Title = list.Key.ToString();

                if (ParentModel.Fields == null)
                {
                    ParentModel.Fields = new string[] { "SellTypeName" };
                    ParentModel.Values = new object[] { list.Key.ToString() };

                }

                ParentModel.Children = new List<TreeViewModel>();

                foreach (var Newlist in list.ClassInfo)
                {

                    TreeViewModel ChildModel = new TreeViewModel() { ID = Newlist.ClassID.ToString(), Title = Newlist.ClassName };
                    if (ChildModel.Fields == null)
                    {
                        ChildModel.Fields = new string[] { "SellTypeName", "ClassID" };
                        ChildModel.Values = new object[] { list.Key.ToString(), Newlist.ClassID };

                    }
                    ParentModel.Children.Add(ChildModel);
                }

                ParentTree.Add(ParentModel);
            }

            //三向连接获取商品信息
            List<API.SeleterModel> production = new List<API.SeleterModel>();
            var queryPro = (from b in ProList
                        
                            join d in Store.SellTypes
                            on 1 equals 1
                            join e in Store.ProClassInfo on b.ClassID equals e.ClassID
                            join f in Store.ProTypeInfo on b.TypeID equals f.TypeID
                            select new
                            {
                                ProMainID = b.ProMainID,
                                ProMainName = b.ProMainName,                        
                                ClassName = e.ClassName,
                                ClassID = e.ClassID,
                                TypeID = f.TypeID,
                                TypeName = f.TypeName,
                                d.Name,
                                SellTypeID = d.ID,

                            }).ToList();
            foreach (var index in queryPro)
            {

                API.SeleterModel pro = new API.SeleterModel();
                pro.ProID = index.ProMainID.ToString();
                pro.ProName = index.ProMainName;
                pro.ProClassName = index.ClassName;
                pro.ClassID = index.ClassID;
                pro.ProTypeName = index.TypeName;
                pro.TypeID = index.TypeID;
                pro.SellTypeName = index.Name;
                pro.SellTypeID = index.SellTypeID;
                pro.Price = Price==null?0:(decimal) Price;
                production.Add(pro);
            }
            MultSelecter2 msFrm = new MultSelecter2(
            ParentTree,
            production, "ProName",
            new string[] { "ProClassName", "ProTypeName", "ProName", "SellTypeName" },
            new string[] { "商品类别", "商品品牌", "商品型号", "销售类型"}
          );
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();

        }
        #endregion
        /// <summary>
        /// 确定添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region 确定添加商品
        void msFrm_Closed(object sender, WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<API.SeleterModel> piList = result.SelectedItems.OfType<API.SeleterModel>().ToList();
                if (piList.Count == 0) return;

                this.unIMEIModels.AddRange(GetOther(piList));

                this.DGCardType.Rebind();
            }
        }
        private List<API.SeleterModel> GetOther(List<API.SeleterModel> piList)
        {
            List<string> ProIDList = (from b in unIMEIModels
                                      select b.ProID).ToList();
            List<API.SeleterModel> HasProList = (from b in unIMEIModels
                                                 where ProIDList.Contains(b.ProID)
                                                 select b).ToList();
            if (HasProList.Count() != 0)
            {
                foreach (var Item in HasProList)
                {
                    List<API.SeleterModel> query = (from b in piList
                                                    where b.ProID == Item.ProID && b.SellTypeID == Item.SellTypeID
                                                    select b).ToList();
                    for (int i = 0; i < query.Count(); i++)
                    {
                        piList.Remove(query[i]);
                    }
                }
            }
            return piList;
        }
        #endregion
    }
}
