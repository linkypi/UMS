using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace UserMS.Views.ProSell.Price
{
    public partial class CostBill : BasePage
    {
        private List<TreeViewModel> treeModels = null;
        private List<API.CostBill> models = null;
        private List<API.CostBill> proinfo = null;
       // private List<API.View_Pro_CostChangeList> proNoCostinfo = null;

        /// <summary>
        /// 商品ID集合
        /// </summary>
        List<string> proList = new List<string>();
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
                menuid = "110";
            }
            models = new List<API.CostBill>();
            proinfo = new List<API.CostBill>();
            GridCostList.ItemsSource = models; 
        }

        public CostBill()
        {
            InitializeComponent();
        }


        #region 删除

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridCostList.SelectedItems == null)
            {
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            foreach (var item in GridCostList.SelectedItems)
            {
                API.CostBill pc = item as API.CostBill;
                foreach (var child in models)
                {
                    if (child.ProID == pc.ProID) 
                    {
                        models.Remove(child);
                        break;
                    }
                }
            }
            GridCostList.Rebind();
            GridCostDetail.ItemsSource = null;
            GridCostDetail.Rebind();
        }

        /// <summary>
        /// 删除成本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCost_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
         {
             if (GridCostList.SelectedItem == null)
             {
                 return;
             }
             if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
             {
                 return;
             }

             API.CostBill cb = GridCostList.SelectedItem as API.CostBill;

             bool breakFlag = false;
             foreach (var item in cb.Children)
             {
                 foreach (var child in GridCostDetail.SelectedItems)
                 {
                     API.CostBillChild cc = child as API.CostBillChild;
                     if (item == cc && !cc.UpdateFlag)
                     {
                         cb.Children.Remove(item);
                         breakFlag = true;
                         break;
                     }
                 }
                 if(breakFlag)
                 {
                     break;
                 }
             }

             GridCostList.Rebind();
             GridCostDetail.ItemsSource = cb.Children;
             GridCostDetail.Rebind();
         }

        #endregion 

        /// <summary>
        /// 添加成本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddCost_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridCostList.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择商品");
                return;
            }

            API.CostBill cb = GridCostList.SelectedItem as API.CostBill;
            API.CostBillChild cc = new API.CostBillChild() { };
            cc.UpdateFlag = false;
            cc.StartDate = DateTime.Now.Date;
            cc.EndDate = DateTime.Now.Date;
            cc.OldStartDate = cc.StartDate;
            cc.OldEndDate = cc.EndDate;
            if (cb.Children == null)
            {
                cb.Children = new List<API.CostBillChild>();
            }
            cb.Children.Add(cc);
            GridCostDetail.ItemsSource = cb.Children;
            GridCostDetail.Rebind();

        }

        #region 添加商品

        /// <summary>
        /// 获取左边树完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetTreeCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.TreeModel> list = e.Result.Obj as List<API.TreeModel>;

                var query = (from c in list
                             join p in proinfo
                             on c.ID equals p.ClassID
                             orderby c.Name
                             select c).Distinct();

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
                    treeModels.Add(p2);
                }

            }
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();

            Common.CommonHelper.ProFilterGen(Store.ProInfo.ToList(), ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
                                            pros, "ProName", new string[] { "ClassName", "TypeName", "ProName", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "商品属性" });
            m.Closed += ProSelect_Closed;
            m.ShowDialog();
        }

        /// <summary>
        /// 确定添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProSelect_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;
          
            if (selecter.DialogResult == true)
            {
                List<SlModel.ProductionModel> phList = selecter.SelectedItems.OfType<SlModel.ProductionModel>().ToList();
                if (phList.Count == 0) return;

                foreach (var item in phList)
                {
                    bool exist = false;
                    foreach (var child in models)
                    {
                        if (item.ProID == child.ProID)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (exist)
                    {
                        continue;
                    }
                    API.CostBill cb = new API.CostBill();
                    cb.ClassID = item.ClassID;
                    cb.ClassName = item.ClassName;
                    cb.IsDecimal = item.Isdecimals;
                    cb.ProFormat = item.ProFormat;
                    cb.ProID = item.ProID;
                    cb.ProName = item.ProName;
                    cb.TypeID = item.TypeID;
                    cb.TypeName = item.TypeName;

                    models.Add(cb);
                }
                GridCostList.Rebind();
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy,140,new object[]{models},new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                models.Clear();
                models.AddRange(e.Result.Obj as List<API.CostBill>);
                foreach(var item in models)
                {
                    if (item.Children == null)
                    {
                        continue;
                    }
                    foreach (var child in item.Children)
                    {
                        if (child.EndDate.ToString() == "0001/1/1 0:00:00")
                        {
                            child.EndDate = DateTime.Now.Date;
                        }
                        if (child.StartDate.ToString() == "0001/1/1 0:00:00")
                        {
                            child.StartDate = DateTime.Now.Date;
                        }
                    }
                }
                GridCostList.Rebind();
            }
    
        }

        #endregion 

        #region 提交数据

        /// <summary>
        /// 确定添加新成本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            #region 
            foreach (var child in models)
            {
                if (child.Children == null)
                {
                    continue;
                }
                foreach (var item in child.Children)
                {
                    if (item.NewCostPrice < 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"成本价不能为0");
                        return;
                    }
                    if (item.StartDate > item.EndDate)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"开始时间不能大于截止时间");
                        return;
                    }

                    // item.EndDate = item.EndDate.AddDays(1).AddSeconds(-1);
                   
                }
            }
            #endregion 

            if (models.Count == 0)
            {
                return;
            }
           #region 验证时间是否重复

            foreach (var child in models)
            {
                if (child.Children == null)
                {
                    continue;
                }
                foreach (var item in child.Children)
                {
                    if (item.StartDate == item.EndDate)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"请为商品"+child.ProName+"输入有效时间段");
                        return;
                    }
                    foreach (var item2 in child.Children)
                    {
                        if (item != item2)
                        {
                            if (item.StartDate <= item2.EndDate && item.EndDate >= item2.StartDate)
                            {
                                MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品"+child.ProName +"的成本存在重复时间段");
                                return;
                            }
                        }
                    }
                }
            }
           #endregion 

            List<API.CostBill> update_models = new List<API.CostBill>();

            foreach (var item in models)
            {
                API.CostBill cb = new API.CostBill();
                cb.ProID = item.ProID;
                cb.Children = new List<API.CostBillChild>();

                if (item.Children == null)
                {
                    continue;
                }
                //新增
                foreach (var child in item.Children)
                {
                    if (!child.UpdateFlag)
                    {
                        cb.Children.Add(child);
                    }
                }
                //更新
                var query = from c in item.Children
                            where c.UpdateFlag && ( c.NewCostPrice != c.CurCostPrice 
                            || c.StartDate != c.OldStartDate|| c.EndDate != c.OldEndDate|| c.RetailPrice!=c.OldRetailPrice )
                            select c;
                foreach (var child in query)
                {
                    cb.Children.Add(child);
                }
                update_models.Add(cb);
            }


            if(MessageBox.Show("确定保存吗？")==MessageBoxResult.Cancel)
            {
                return;
            }

         
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 141, new object[] { update_models }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "调价失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"添加成功");
                Logger.Log("添加成功");

                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 140, new object[] { models }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));

                //models.Clear();
                //GridCostList.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                Logger.Log("添加成失败");
            }
        }

        #endregion

        private void GridCostList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (GridCostList.SelectedItem == null)
            {
                return;
            }

            API.CostBill cb = GridCostList.SelectedItem as API.CostBill;
            if (cb == null || cb.Children==null)
            {
                GridCostDetail.ItemsSource =null;
                GridCostDetail.Rebind();
                return;
            }
            foreach (var item in cb.Children)
            {
                item.IsDecimal = cb.IsDecimal;
            }
            GridCostDetail.ItemsSource = cb.Children;
            GridCostDetail.Rebind();
        }

        private void GridCostDetail_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
           List<API.CostBillChild> list =   GridCostDetail.ItemsSource as List<API.CostBillChild>;
           foreach (var item in list)
           {
               if (item.NewCostPrice < 0)
               {
                   MessageBox.Show(System.Windows.Application.Current.MainWindow,"成本不能小于0");
                   item.NewCostPrice = item.OldCostPrice;
                   return;
                }

               if (item.RetailPrice < 0)
               {
                   MessageBox.Show(System.Windows.Application.Current.MainWindow, "零售价不能小于0");
                   item.RetailPrice = item.RetailPrice;
                   return;
               }
               if (item.EndDate!= null)
               {
                   item.EndDate = item.EndDate.AddDays(1).AddSeconds(-1);
               }

               item.NewCostPrice = Decimal.Truncate(Convert.ToDecimal(item.NewCostPrice * 100)) / 100;
            }
        }

    }
}
