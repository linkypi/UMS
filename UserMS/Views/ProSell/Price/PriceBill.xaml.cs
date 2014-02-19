using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SlModel;
using Telerik.Windows.Controls;
using System.Windows.Controls;

namespace UserMS.Views.ProSell.Price
{
    public partial class PriceBill : Page
    {
        private List<TreeViewModel> treeModels = null;
        List<API.PriceBill> models = null;
        List<TreeViewModel> ParentTree = new List<TreeViewModel>();
        List<API.PriceBill> PricePro = new List<API.PriceBill>();
        List<API.Pro_ProInfo> prods = new List<API.Pro_ProInfo>();
        List<SlModel.ProductionModel> proMains = new List<ProductionModel>();

        private string menuid = "";

       private void Page_Loaded(object sender, RoutedEventArgs e)
      {
           this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "109";
            }
            models = new List<API.PriceBill>();
            GridProList.ItemsSource = models;

            List<CkbModel> list = new List<CkbModel>() { 
            new  CkbModel(true,"是"),
            new  CkbModel(false,"否") };
            GridViewComboBoxColumn comcol1 = GridProDetail.Columns[3] as GridViewComboBoxColumn;
            comcol1.ItemsSource = list;
            GridViewComboBoxColumn comcol2= GridProDetail.Columns[4] as GridViewComboBoxColumn;
            comcol2.ItemsSource = list;

            //初始化总商品
            #region 
            var mains = from a in Store.ProMainInfo
                         join b in Store.ProInfo
                         on a.ProMainID equals b.ProMainID
                         join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                         join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                         where b.ProMainID != null
                         select new
                         {
                             a.ProMainID,
                             b.ProID,
                             c.ClassName,
                             d.TypeName,
                             a.ProMainName,
                             c.ClassID,
                             d.TypeID,
                             b.ProFormat
                         };
              foreach (var index in mains)
              {
                  bool flag = false;
                  foreach (var vm in proMains)
                  {
                      if (vm.ProMainID == index.ProMainID)
                      {
                          flag = true;
                          break;
                      }
                  }
                  if (flag)
                  {
                      continue;
                  }
                  SlModel.ProductionModel pro = new SlModel.ProductionModel();
                  pro.ProID = index.ProID;
                  pro.ProMainID = Convert.ToInt32(index.ProMainID);
                  pro.ProName = index.ProMainName;
                  //pro.ProFormat = index.ProFormat;
                  // pro.IsNeedIMEI = index.IsNeedIMEI;
                  pro.ClassName = index.ClassName;
                  pro.ClassID = index.ClassID.ToString();
                  pro.TypeName = index.TypeName;
                  pro.TypeID = index.TypeID.ToString();
                  proMains.Add(pro);
              }
            #endregion 
      }

        public PriceBill()
        {
            InitializeComponent();
        }

        #region  添加商品

        /// <summary>
        /// 加载商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Load_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();  //.Where(p=>p.ProMainID==null).ToList()

            Common.CommonHelper.ProFilterGen(Store.ProInfo, ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
              pros, "ProName", new string[] { "ClassName", "TypeName", "ProName", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "商品属性" });
            m.Tag = false; //商品
            m.Closed += msFrm_Closed;
            m.ShowDialog();
        }

        /// <summary>
        /// 加载总商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMain_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();

            Common.CommonHelper.ProFilterGen(Store.ProInfo.Where(p => p.ProMainID != null).ToList(), ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
             proMains, "ProName", new string[] { "ClassName", "TypeName", "ProName" },
            new string[] { "商品类别", "商品品牌", "商品型号" });
            m.Tag = true; //总商品
            m.Closed += msFrm_Closed;
            m.ShowDialog();
        }

        private void msFrm_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<SlModel.ProductionModel> piList = result.SelectedItems.OfType<SlModel.ProductionModel>().ToList();
                if (piList.Count == 0) return;

                List<API.PriceBill> list = new List<API.PriceBill>();
                foreach (var item in piList)
                {
                    if ((bool)result.Tag == false)
                    {
                        if (!ValidateProduction(item.ProID, models))
                        {
                            AddProc((bool)result.Tag, item,list);
                        }
                    }
                    else
                    {
                        if (!ValidateMainPro(item.ProMainID, models))
                        {
                            AddProc((bool)result.Tag, item,list);
                        }
                        GridProList.Rebind();
                    }
                }
                if ((bool)result.Tag == false)
                {
                    //获取商品价格
                    PublicRequestHelp prh = new PublicRequestHelp(this.busy, 163, new object[] { list }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
                }
            }
        }

        private void AddProc(bool IsMainPro, ProductionModel item, List<API.PriceBill> list)
        {
            API.PriceBill p = new API.PriceBill();
            p.ClassID = item.ClassID.ToString();
            p.ClassName = item.ClassName;
            p.TypeID = item.TypeID.ToString();
            p.TypeName = item.TypeName;
            p.ProName = item.ProName;
            p.ProID = item.ProID;
            p.ProMainID = item.ProMainID;
            p.ProFormat = item.ProFormat;
            p.IsDecimal = item.Isdecimals;
            p.IsMainPro = IsMainPro;
            list.Add(p);
            models.Add(p);
        }

        /// <summary>
        /// 验证总商品是否已经存在
        /// </summary>
        /// <param name="mainid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool ValidateMainPro(int mainid, List<API.PriceBill> list)
        {
            if (list == null)
                return false;
            foreach (var vm in list)
            {
                if (vm.ProMainID == mainid && vm.IsMainPro)
                {
                    return true;
                }
            }
            return false;
        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
               // models.Clear();
                List<API.PriceBill> list = e.Result.Obj as List<API.PriceBill>;
                foreach (var item in list)
                {
                    foreach (var child in models)
                    {
                        if (item.IsMainPro == child.IsMainPro &&
                            (item.ProID == child.ProID) &&
                            (item.TypeID == child.TypeID) &&
                            (item.ClassID == child.ClassID))
                        {
                            if (child.Children == null)
                            {
                                child.Children = new List<API.PriceBillChild>();
                            }
                            child.Children.AddRange(item.Children);
                        }
                    }
                    //if (!Exist(item))
                    //{
                    //    models.Add(item);
                    //}
                }
            }
            GridProList.Rebind();
        }

        private bool Exist(API.PriceBill mod)
        {
            foreach (var item in models)
            {
                if (item.IsMainPro==mod.IsMainPro &&(item.ProName==mod.ProName)&&(item.TypeName ==mod.TypeName)&&(item.ClassName==mod.ClassName))
                {
                    return true;
                }
            }
            return false;
        }

        private void GetTreeCompleted(object sender, API.MainCompletedEventArgs e)
        {
            var queryp = (from b in Store.ProInfo
                        // join c in Store.RoleInfo.First().Sys_Role_Menu_ProInfo
                        // on b.Pro_ClassID equals c.ClassID
                         //where c.MenuID == int.Parse(menuid)
                         select b).Distinct().ToList();
            if (queryp.Count() > 0)
            {
                prods = queryp;
            }

            if (prods.Count() != 0)
            {
                var query2 = (from b in prods
                              join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                              join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                              orderby b.ProName
                              select new
                              {
                                  ProID = b.ProID,
                                  ProName = b.ProName,
                                  b.ISdecimals,
                                  ProFormat = b.ProFormat,
                                  IsNeedIMEI = b.NeedIMEI,
                                  ClassName = c.ClassName,
                                  ClassID = c.ClassID,
                                  d.TypeID,
                                  TypeName = d.TypeName
                              }).Distinct().ToList();

                foreach (var item in query2)
                {
                    API.PriceBill p = new API.PriceBill();
                    p.ClassID = item.ClassID.ToString();
                    p.ClassName = item.ClassName;
                    p.TypeID = item.TypeID.ToString();
                    p.TypeName = item.TypeName;
                    p.ProName = item.ProName;
                    p.ProID = item.ProID;
                    p.ProFormat = item.ProFormat;
                    p.IsDecimal = item.ISdecimals;
                     PricePro.Add(p);
                }
            }

            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                #region  获取左树

                List<API.TreeModel> list = e.Result.Obj as List<API.TreeModel>;
                var query =( from c in list
                            join p in PricePro
                            on c.ID equals p.ClassID
                             orderby c.Name
                            select c).Distinct();
                List<string> proidList = e.Result.ArrList[0] as List<string>;

                List<TreeViewModel> mods = new List<TreeViewModel>();
                foreach (var item in query)
                { 
                    TreeViewModel p2 = new TreeViewModel();
                    p2.Fields = new string[] { "ClassName", "ClassID" };
                    p2.Values = new object[] { item.Name, item.ID.ToString() };

                    p2.ID = item.ID;
                    p2.Title = item.Name;
                    p2.Children = new List<TreeViewModel>();

                    foreach (var child in item.Children)
                    {
                        TreeViewModel t = new TreeViewModel();
                        t.Fields = new string[] { "TypeName", "TypeID", "ClassName" };
                        t.Values = new object[] { child.Name, child.ID.ToString(), item.Name };
                        t.ID = child.ID;
                        t.Title = child.Name;
                        p2.Children.Add(t);
                    }
                    mods.Add(p2);
                }

                treeModels.AddRange(mods);
                #endregion
            }
        }

        private bool ValidateProduction(string pid, List<API.PriceBill> list)
        {
            if (list == null)
                return false;
            foreach (var vm in list)
            {
                if (vm.ProID == pid && !vm.IsMainPro)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion 

        #region   添加销售类别

        private void addSellType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (models.Count == 0)
            {
                return;
            }
            if (GridProList.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择商品");
                return;
            }

            MultSelecter2 msFrm = new MultSelecter2(
              null, Store.SellTypes, "Name",
              new string[] { "Name"},
              new string[] { "销售类别"});
            msFrm.Closed += SellSelect_Closed;
            msFrm.ShowDialog();
        }

        private void SellSelect_Closed(object sender, WindowClosedEventArgs e)
        {
              UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
              if (result.DialogResult == true)
              {
                
                  List<API.Pro_SellType> piList = result.SelectedItems.OfType<API.Pro_SellType>().ToList();
                  if (piList.Count == 0) return;

                  foreach(var childx in GridProList.SelectedItems)
                  {
                      API.PriceBill model =childx  as API.PriceBill;
                      if (model.Children == null)
                      {
                          model.Children = new List<API.PriceBillChild>();
                      }
                      int index = model.Children.Count + 1;
                      foreach (var item in piList)
                      {
                          bool flag = false;
                          foreach (var child in model.Children)
                          {
                              if (child.SellTypeID == item.ID)
                              {
                                  flag = true;
                                  break;
                              }
                          }
                          if (flag)
                          {
                              continue;
                          }
                          API.PriceBillChild pc = new API.PriceBillChild();
                          pc.IsDecimal = model.IsDecimal;
                          pc.SellTypeID = item.ID;
                          pc.HasPrice = false;
                          pc.SellTypeName = item.Name;
                          pc.ID = index;
                          if (model.Children == null)
                          {
                              model.Children = new List<API.PriceBillChild>();
                          }
                          model.Children.Add(pc);
                          index++;
                      }
                  }
                  GridProDetail.ItemsSource =(GridProList.SelectedItems[0]as API.PriceBill ).Children;
                  GridProDetail.Rebind();
              }
        }

        #endregion 

        #region  事件

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridProList.SelectedItems == null)
            {
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            foreach (var item in GridProList.SelectedItems)
            {
                API.PriceBill vpc = item as API.PriceBill;
                foreach (var child in models)
                {
                    if (vpc.ProID== child.ProID)
                    {
                        models.Remove(child);
                        break;
                    }
                }
            }
            GridProDetail.ItemsSource = null;
            GridProDetail.Rebind();
            GridProList.Rebind();
        }
        
        /// <summary>
        /// 删除销售类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteSellType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridProDetail.SelectedItems == null)
            {
                return;
            }
           API.PriceBill model =  GridProList.SelectedItem as API.PriceBill;

            
            foreach (var item in GridProDetail.SelectedItems)
            {
                API.PriceBillChild vpc = item as API.PriceBillChild;
                foreach (var child in model.Children)
                {
                    if (vpc.ID== child.ID && !vpc.HasPrice)
                    {  
                        model.Children.Remove(child);
                        break;
                    }
                    if (vpc.ID == child.ID && vpc.HasPrice)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"此销售类别已定价，不可删除");
                        return;
                    }
                }
            }
            GridProDetail.Rebind();
        }

        private void GridProList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridProList.SelectedItem == null)
            {
                return;
            }

            API.PriceBill model = GridProList.SelectedItem as API.PriceBill;
            GridProDetail.ItemsSource = model.Children;
            GridProDetail.Rebind();
        }

        #endregion  

        #region  提交数据

        private void btnOk_Click(object sender, RoutedEventArgs e)
        { 
            #region 验证用户输入是否有效

            if (models.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"表单无数据");
                return;
            }
            foreach (var item in models)
            {
                foreach (var child in item.Children)
                {
                    if (child.MinPrice < 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"最低价不能小于0 ");
                        return;
                    }
                    if (child.MaxPrice < 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"最高价不能小于0 ");
                        return;
                    }
                    if (child.LowPrice < 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"结算价不能小于0 ");
                        return;
                    }
                    if (child.Price < 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"零售价不能小于0 ");
                        return;
                    }
                }
            }
            #endregion

            List<API.PriceBill> submitModels = new List<API.PriceBill>();
            List<API.PriceBill> returnModels = new List<API.PriceBill>();

            foreach (var item in models)
            {
                API.PriceBill pb = new API.PriceBill();
                pb.ProID = item.ProID;
                pb.Children = new List<API.PriceBillChild>();
                pb.IsMainPro = item.IsMainPro;
                pb.ProMainID = item.ProMainID;

                returnModels.Add(pb);
                #region  获取新增数据

                foreach (var child in item.Children)
                {
                    if (!child.HasPrice)
                    {
                        pb.Children.Add(child);
                    }
                }
                #endregion

                #region  获取有改动的数据

                var query2 = from c in item.Children
                             where c.HasPrice && (c.OldPrice != c.Price || c.OldMinPrice != c.MinPrice|| c.IsAduit !=c.OldIsAduit
                             || c.OldMaxPrice != c.MaxPrice || c.OldLowPrice != c.LowPrice|| c.IsTicketUseful!=c.OldIsTicketUseful)
                             select c;

                if (query2.Count() > 0)
                {
                    foreach (var child in query2)
                    {
                        API.PriceBillChild pc = new API.PriceBillChild();
                        pc.ID = child.ID;
                        pc.HasPrice = child.HasPrice;
                        pc.IsTicketUseful = child.IsTicketUseful;
                        pc.LowPrice = child.LowPrice;
                        pc.MaxPrice = child.MaxPrice;
                        pc.MinPrice = child.MinPrice;
                        pc.Price = child.Price;
                        pc.IsAduit = child.IsAduit;
                        pc.SellTypeID = child.SellTypeID;
                        pb.Children.Add(pc);
                    }
                }
                #endregion
                if (pb.Children.Count != 0)
                {
                    submitModels.Add(pb);
                }
            }
            if (submitModels.Count == 0)
            {
                MessageBox.Show("无数据可保存！");
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 119, new object[] { submitModels, returnModels }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"调价成功");
                //调价成功后更新定价商品数据
                List<API.PriceBill> list = e.Result.Obj as  List<API.PriceBill>;

                foreach (var item in list)
                {
                    foreach (var child in models)
                    {
                        if (item.ProID == child.ProID)
                        {
                            item.ProName = child.ProName;
                            item.TypeName = child.TypeName;
                            item.ClassName = child.ClassName;
                            item.ProFormat = child.ProFormat;
                        }
                    }
                }
                models.Clear();
                models.AddRange(list);
            
                GridProList.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"调价失败");
                return;
            }
        }

        #endregion 

        private void GridProDetail_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            List<API.PriceBillChild> ms = GridProDetail.ItemsSource as List<API.PriceBillChild>;

            string msg = "价格不能小于0 ";

            foreach (var item in ms)
            {
                if (item.MinPrice < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,msg);
                    item.MinPrice = item.OldMinPrice;
                    return;
                }
                if (item.MaxPrice < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,msg);
                    item.MaxPrice = item.OldMaxPrice;
                    return;
                }
                if (item.LowPrice < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,msg);
                    item.LowPrice = item.OldLowPrice;
                    return;
                }
                if (item.Price < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,msg);
                    item.Price = item.OldPrice;
                    return;
                }
                item.MinPrice = Decimal.Truncate(Convert.ToDecimal(item.MinPrice * 100)) / 100;
                item.MaxPrice = Decimal.Truncate(Convert.ToDecimal(item.MaxPrice * 100)) / 100;
                item.LowPrice = Decimal.Truncate(Convert.ToDecimal(item.LowPrice * 100)) / 100;
                item.Price = Decimal.Truncate(Convert.ToDecimal(item.Price * 100)) / 100;
             
            }
        }

    

    }
}
