using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SlModel;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Sys_tem.Package
{
    /// <summary>
    /// AddRulesOffList.xaml 的交互逻辑
    /// </summary>
    public partial class AddRulesOffList : Page
    {
        List<API.Pro_HallInfo> hallInfo = new List<API.Pro_HallInfo>();
        List<API.Pro_HallInfo> hallAdds = new List<API.Pro_HallInfo>();
        List<API.Pro_SellType> ProSellType = new List<API.Pro_SellType>();
        List<TreeViewModel> hallTree = new List<TreeViewModel>();
        List<API.RulesProMain> models = new List<API.RulesProMain>();
        List<SlModel.ProductionModel> proMains = new List<ProductionModel>();
        List<API.Pro_ClassInfo> classInfo = new List<API.Pro_ClassInfo>();

        List<API.Rules_ImportModel> uncheckModels = new List<API.Rules_ImportModel>();

        API.Rules_ProMainInfo s = new API.Rules_ProMainInfo();
        List<API.ProModel> proModels = new List<API.ProModel>();

        private Mul_HallFilter hAdder;

        public AddRulesOffList()
        {
            InitializeComponent();

            hAdder = new Mul_HallFilter(ref this.GridHall);
            GridClass.ItemsSource = classInfo;
            this.CreatName.Text = Store.LoginUserName;
            GridPros.ItemsSource = uncheckModels;

            GridHall.ItemsSource = hallAdds;
            GridSellType.ItemsSource = ProSellType;
            GridProMain.ItemsSource = models;
            this.SizeChanged += AddRulesOffList_SizeChanged;

            //GridViewComboBoxColumn cb = GridPros.Columns[2] as GridViewComboBoxColumn;
            //cb.ItemsSource = Store.RulesTypeInfo;

            #region 初始化商品
            var pros = (from a in Store.ProInfo
                        select a.ProName).Distinct();
            foreach (var item in pros)
            {
                API.ProModel p = new API.ProModel();
                p.ProName = item;
                proModels.Add(p);
            }

            #endregion 
            #region  初始化仓库
            var query = (from b in Store.RoleInfo.First().Sys_Role_Menu_HallInfo
                         // where b.MenuID == menuID
                         join c in Store.ProHallInfo
                         on b.HallID equals c.HallID into hall
                         from b1 in hall.DefaultIfEmpty()
                         select b1).ToList();
            //if (query.Count() > 0)
            //    return query;
            hallInfo = query.Where(c => c != null).ToList();
            List<TreeViewModel> child = new List<TreeViewModel>();

            //生成左边树
            var AreaList = (from b in hallInfo
                            join c in Store.AreaInfo.Where(c => c.AreaID != null) on b.AreaID equals c.AreaID
                            where b != null && b.AreaID != null && c.AreaID != null
                            select c).Distinct().ToList();
            hallTree = CommonHelper.AreaTreeViewModel(AreaList);

            #endregion 

            //初始化总商品
            #region
            //var mains = from a in Store.ProMainInfo
            //            //join b in Store.ProInfo
            //            //on a.ProMainID equals b.ProMainID
            //            join c in Store.ProClassInfo on a.ClassID equals c.ClassID
            //            join d in Store.ProTypeInfo on a.TypeID equals d.TypeID
            //            where a.ProMainID != null
            //            select new
            //            {
            //                a.ProMainID,
            //                c.ClassName,
            //                d.TypeName,
            //                a.ProMainName,
            //                c.ClassID,
            //                d.TypeID
            //            };
            //foreach (var index in mains)
            //{
            //    bool flag = false;
            //    foreach (var vm in proMains)
            //    {
            //        if (vm.ProMainID == index.ProMainID)
            //        {
            //            flag = true;
            //            break;
            //        }
            //    }
            //    if (flag)
            //    {
            //        continue;
            //    }
            //    SlModel.ProductionModel pro = new SlModel.ProductionModel();
             
            //    pro.ProMainID = Convert.ToInt32(index.ProMainID);
            //    pro.ProMainName = index.ProMainName;
            //    pro.ClassName = index.ClassName;
            //    pro.ClassID = index.ClassID.ToString();
            //    pro.TypeName = index.TypeName;
            //    pro.TypeID = index.TypeID.ToString();
            //    proMains.Add(pro);
            //}
            #endregion 

            import.ImportType = typeof(API.Rules_ImportModel);
            import.OnImported += import_OnImported;
        }

        void import_OnImported(object sender, DataImportArgs e)
        {
            var ai = e.Datas;
            List<API.Rules_ImportModel> list = Common.DataExtensions.ToList<API.Rules_ImportModel>(ai).ToList();

            #region  验证规则类型

            List<string> types = new List<string>();
            foreach(var item in Store.RulesTypeInfo)
            {
                types.Add(item.RulesName);
            }
            var val = from a in list
                      where !types.Contains(a.RulesName)
                      select a;
            if (val.Count() > 0)
            {
                MessageBox.Show("规则类型："+val.First().RulesName+" 不存在！");
                return;
            }
            #endregion 

            #region  验证商品名称
            //List<string> pros = new List<string>();
            //foreach (var item in Store.ProInfo)
            //{
            //    pros.Add(item.ProName);
            //}

            var valpros = from b in Store.ProInfo
                          select b.ProName.ToUpper();
                          var p = 
                          from a in list
                          where !valpros.Contains(a.ProName.ToUpper())
                          select a;
            if (p.Count() > 0)
            {
                string msg = "";
                int index = 1;
                foreach (var item in p)
                {
                    msg += item.ProName;
                    if (index < p.Count())
                    {
                        msg += " , ";
                    }
                    index++;
                }

                MessageBox.Show("以下商品不存在："+msg);
                return;
            }
            #endregion 

            uncheckModels.Clear();
            uncheckModels.AddRange(list);
            GridPros.Rebind();
            
            models.Clear();
            GridProMain.Rebind();
            classInfo.Clear(); 
            GridClass.Rebind();

            hallAdds.Clear();
            GridHall.Rebind();
            ProSellType.Clear();
            GridSellType.Rebind();
            GridRuleOff.ItemsSource = null;
            GridRuleOff.Rebind();
        }

        void AddRulesOffList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Note.Width = this.ActualWidth - 100;
        }

        #region  添加仓库

        private void AddHall_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            hAdder.GetHall(Store.ProHallInfo);
           // MultSelecter2 msFrm = new MultSelecter2(
           //  hallTree,
           //  hallInfo, "HallName",// "AreaID",
           // new string[] { "HallID", "HallName" },
           // new string[] { "仓库编码", "仓库名称" }//, false
           //);
           // msFrm.Closed += HallSelect_Closed;
           // msFrm.ShowDialog();
        }

        /// <summary>
        /// 确定添加仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HallSelect_Closed(object sender, EventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;

            if (selecter.DialogResult == true)
            {
                if(selecter.SelectedItems==null)
                {
                    return;
                }

                foreach(var item in selecter.SelectedItems)
                {
                    API.Pro_HallInfo ph = item as API.Pro_HallInfo;
                    if (ValideHall(ph.HallID))
                    {
                        hallAdds.Add(ph);
                    }
                }
                GridHall.Rebind();
            }
        }

        private bool ValideHall(string hallid)
        {
            foreach (var item in hallAdds)
            {
                if (item.HallID == hallid)
                {
                    return false;
                }
            }
            return true;
        }

        private void delteHall_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridHall.SelectedItems == null)
            {
                MessageBox.Show("请选择要删除的规则！");
                return;
            }
            List<API.Pro_HallInfo> list = new List<API.Pro_HallInfo>();
            foreach (var item in GridHall.SelectedItems)
            {
                API.Pro_HallInfo rt = item as API.Pro_HallInfo;
               list = GridHall.ItemsSource as List<API.Pro_HallInfo>;
                foreach (var child in list)
                {
                    if (rt == child)
                    {
                        list.Remove(child);
                        break;
                    }
                }
            }
            GridHall.ItemsSource = list;
            GridHall.Rebind();
        }

        #endregion 

        #region  销售类别

        private void AddSellType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MultSelecter2 msFrm = new MultSelecter2(
              null, Store.SellTypes, "Name",
              new string[] { "Name" },
              new string[] { "销售类别" });
            msFrm.Closed += SellSelect_Closed;
            msFrm.ShowDialog();
        }

        private void SellSelect_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<API.Pro_SellType> piList = result.SelectedItems.OfType<API.Pro_SellType>().ToList();
                if (piList.Count == 0) return;

                foreach (var childx in piList)
                {
                    if (ValideSellType(childx.ID))
                    {
                        ProSellType.Add(childx);
                    }
                }
                GridSellType.Rebind();
            }
        }

        private bool ValideSellType(int SellType)
        {
            foreach (var item in ProSellType)
            {
                if (item.ID == SellType)
                {
                    return false;
                }
            }
            return true;
        }

        private void delSellType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridSellType.SelectedItems == null)
            {
                MessageBox.Show("请选择要删除的规则！");
                return;
            }

            foreach (var item in GridSellType.SelectedItems)
            {
                API.Pro_SellType rt = item as API.Pro_SellType;
                foreach (var child in ProSellType)
                {
                    if (rt == child)
                    {
                        ProSellType.Remove(child);
                        break;
                    }
                }
            }
            GridSellType.Rebind();
        }

        #endregion 

        private void GridHall_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {


        }

        private void GridRuleOff_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            API.Pro_RulesTypeInfo prt = GridRuleOff.SelectedItem as API.Pro_RulesTypeInfo;
           
            if (prt.OffPrice < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "优惠价不能小于0");
                prt.OffPrice = 0;
                return;
            }

            prt.OffPrice = Decimal.Truncate(Convert.ToDecimal(prt.OffPrice * 100)) / 100;

            if (prt.MaxPrice < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "最大优惠价不能小于0");
                prt.MaxPrice = 0;
                return;
            }

            prt.MaxPrice = Decimal.Truncate(Convert.ToDecimal(prt.MaxPrice * 100)) / 100;

            if (prt.MinPrice < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "最小优惠价不能小于0");
                prt.MinPrice = 0;
                return;
            }

            prt.MinPrice = Decimal.Truncate(Convert.ToDecimal(prt.MinPrice * 100)) / 100;
        }

        private void GridProMain_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (GridProMain.SelectedItem == null)
            {
                return;
            }
            API.RulesProMain model = this.GridProMain.SelectedItem as API.RulesProMain;

            GridRuleOff.ItemsSource = model.Pro_RulesTypeInfo;
            GridRuleOff.Rebind();
        }

        #region 保存

        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (models.Count == 0)
            {
                MessageBox.Show("请添加数据！");
                return;
            }

            if (StartTime.SelectedDate < DateTime.Now.Date)
            {
                MessageBox.Show("开始时间不能小于当前时间！");
                return;
            }
            if (EndTime.SelectedDate <= DateTime.Now)
            {
                MessageBox.Show("结束时间不能小于等于当前时间！");
                return;
            }
            if (EndTime.SelectedDate <= StartTime.SelectedDate)
            {
                MessageBox.Show("结束时间不能小于等于开始时间！");
                return;
            }

            if (string.IsNullOrEmpty(EndTime.DateTimeText))
            {
                MessageBox.Show("请选择结束时间！");
                return;
            }

            if (string.IsNullOrEmpty(StartTime.DateTimeText))
            {
                MessageBox.Show("请选择开始时间！");
                return;
            }

            if (string.IsNullOrEmpty(this.Note.Text))
            {
                MessageBox.Show("规则描述不为空！");
                return;
            }

            if (models.Count == 0)
            {
                MessageBox.Show("总商品不能为空！");
                return;
            }

            bool had = false;
            foreach(var item in models)
            {
                if (item.Pro_RulesTypeInfo != null && item.Pro_RulesTypeInfo.Count != 0)
                {
                    had = true;
                    break ;
                }
            }

            if (!had)
            {
                MessageBox.Show("请为总商品添加规则活动！");
                return;
            }

            if (ProSellType.Count == 0)
            {
                MessageBox.Show("请添加销售类别！");
                return;
            }
            List<API.Pro_HallInfo> halllist = GridHall.ItemsSource as List<API.Pro_HallInfo>;
            if (halllist.Count == 0)
            {
                MessageBox.Show("请添加门店！");
                return;
            }

            foreach (var item in models)
            {
                if (item.Pro_RulesTypeInfo == null)
                {
                    MessageBox.Show("请为总商品"+item.ProMainName+"添加规则活动！");
                    return;
                }
                if (item.Pro_RulesTypeInfo.Count==0)
                {
                    MessageBox.Show("请为总商品" + item.ProMainName + "添加规则活动！");
                    return;
                }
                foreach (var child in item.Pro_RulesTypeInfo)
                {
                    if (child.MaxPrice < child.OffPrice)
                    {
                        MessageBox.Show("最大优惠不能小于默认优惠！");
                        return;
                    }
                    if (child.OffPrice < child.MinPrice)
                    {
                        MessageBox.Show("默认优惠不能小雨最小优惠！");
                        return;
                    }
                }
            }


            API.Rules_OffList header = new API.Rules_OffList();

            header.EndDate = this.EndTime.SelectedDate;
            header.StartDate = this.StartTime.SelectedDate;
            header.Note = this.Note.Text;
            header.UserID = Store.LoginUserInfo.UserID;
            header.Rules_ProMainInfo = new List<API.Rules_ProMainInfo>();
            foreach (var item in models)
            {
                API.Rules_ProMainInfo rp = new API.Rules_ProMainInfo();
                rp.ProMainID  = item.ProMainID;
             
                rp.Rules_Pro_RulesTypeInfo = new List<API.Rules_Pro_RulesTypeInfo>();
                foreach (var child in item.Pro_RulesTypeInfo)
                {
                    API.Rules_Pro_RulesTypeInfo rrt = new API.Rules_Pro_RulesTypeInfo();
                    rrt.MaxPrice = child.MaxPrice;
                    rrt.MinPrice = child.MinPrice;
                    rrt.OffPrice = child.OffPrice;
                    rrt.OrderBy = child.OrderBy;
                    rrt.RulesTypeID = child.RulesTypeID;
                    rp.Rules_Pro_RulesTypeInfo.Add(rrt);
                }
                header.Rules_ProMainInfo.Add(rp);
            }

            header.Rules_SellTypeInfo = new List<API.Rules_SellTypeInfo>();
            foreach (var item in ProSellType)
            {
                API.Rules_SellTypeInfo rst = new API.Rules_SellTypeInfo();
                rst.SellType = item.ID;
                header.Rules_SellTypeInfo.Add(rst);
            }

            header.Rules_HallOffInfo = new List<API.Rules_HallOffInfo>();
            foreach (var item in halllist)
            {
                API.Rules_HallOffInfo rst = new API.Rules_HallOffInfo();
                rst.HallID = item.HallID;
                header.Rules_HallOffInfo.Add(rst);
            }

            if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            
            PublicRequestHelp prh = new PublicRequestHelp(this.busy,281,new object[]{header},new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                Note.Text = string.Empty;
                StartTime.DateTimeText = string.Empty;
                EndTime.DateTimeText = string.Empty;
                hallAdds.Clear();
                GridHall.Rebind();
                models.Clear();
                GridProMain.Rebind();
                ProSellType.Clear();
                GridSellType.Rebind();
                GridRuleOff.ItemsSource = null;
                GridRuleOff.Rebind();
                uncheckModels.Clear();
                GridPros.Rebind();
                classInfo.Clear();
                GridClass.Rebind();
            }
            MessageBox.Show(e.Result.Message);
        }

        #endregion 

        #region  添加商品

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
           // var pros = new List<SlModel.ProductionModel>();
           // var t = new List<TreeViewModel>();

           // Common.CommonHelper.ProFilterGen(Store.ProInfo.Where(p => p.ProMainID != null).ToList(), ref pros, ref t);
           
            MultSelecter2 m = new MultSelecter2(null,
             proModels, "ProName", new string[] { "ProName" },//, "ProName""商品品牌", "商品型号" 
            new string[] {  "商品名称" });
    
            m.Closed += msFrm2_Closed;
            m.ShowDialog();
        }

        private void msFrm2_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<API.ProModel> piList = result.SelectedItems.OfType<API.ProModel>().ToList();
                if (piList.Count == 0) return;

                foreach (var item in piList)
                {
                    //if (!ValidateMainPro(item.ProMainID, models))
                   // {
                    API.Rules_ImportModel main = new API.Rules_ImportModel();
                    main.ProName = item.ProName;
                     uncheckModels.Add(main);
                     GridPros.Rebind();
                   // }
                }
                GridProMain.Rebind();
            }
        }
        #endregion 

        #region 商品类别

        /// <summary>
        /// 添加类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addClass_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MultSelecter2 msFrm = new MultSelecter2(
                       null, Store.ProClassInfo, "ClassName",
                       new string[] { "ClassName" },
                       new string[] { "商品类别" });
            msFrm.Closed += SelectClass_Closed;
            msFrm.ShowDialog();
        }

        private void SelectClass_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<API.Pro_ClassInfo> piList = result.SelectedItems.OfType<API.Pro_ClassInfo>().ToList();
                if (piList.Count == 0) return;

                foreach (var childx in piList)
                {
                    if (!ClassExist(childx.ClassID))
                    {
                        classInfo.Add(childx);
                    }
                }
                 
                GridClass.Rebind();
            }
        }

        bool ClassExist(int id)
        {
            foreach (var item in classInfo)
            {
                if (id == item.ClassID)
                {
                    return true;
                }
            }
            return false;
        }


        private void delClass_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridClass.SelectedItems.Count == 0)
            {
                return;
            }
            foreach (var child in GridClass.SelectedItems)
            {
                foreach (var item in classInfo)
                {
                    if (child == item)
                    {
                        classInfo.Remove(item);
                        break;
                    }
                }
            }
            GridClass.Rebind();
        }

        #endregion 

        #region 生存规则

        private void genrRules_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (uncheckModels.Count == 0)
            {
                MessageBox.Show("输入数据！");
                return;
            }

            if (classInfo.Count == 0)
            {
                MessageBox.Show("请选择商品类别！");
                return;
            }
            foreach (var item in uncheckModels)
            {
                if (string.IsNullOrEmpty(item.RulesName))
                {
                    MessageBox.Show("规则类型不能为空！");
                    return;
                }
                if (item.OffPrice <= 0)
                {
                    MessageBox.Show("默认优惠金额无效！");
                    return;
                }
                if (item.MaxPrice <= 0)
                {
                    MessageBox.Show("最大优惠金额无效！");
                    return;
                }
                if (item.MinPrice <= 0)
                {
                    MessageBox.Show("最小优惠金额无效！");
                    return;
                }
            }
            List<int> cls = new List<int>();
            foreach (var item in classInfo)
            {
                cls.Add(item.ClassID);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 288, new object[] { uncheckModels,cls }, new EventHandler<API.MainCompletedEventArgs>(GenerateCompleted));
        }

        private void GenerateCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.RulesProMain> list = e.Result.Obj as List<API.RulesProMain>;
                if (list != null)
                {
                    models.Clear();
                    models.AddRange(list);
                    GridProMain.Rebind();
                }

            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }

        }

        #endregion

        #region 废弃

        #region   添加总商品

        /// <summary>
        /// 加载总商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addProMain_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();

            Common.CommonHelper.ProFilterGen(Store.ProInfo.Where(p => p.ProMainID != null).ToList(), ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
             proMains, "ProMainName", new string[] { "ClassName", "TypeName", "ProMainName" },//, "ProName""商品品牌", "商品型号" 
            new string[] { "商品类别", "商品品牌", "总商品名称" });
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

                foreach (var item in piList)
                {
                    if (!ValidateMainPro(item.ProMainID, models))
                    {
                        API.RulesProMain main = new API.RulesProMain();
                        main.ProMainID = item.ProMainID;
                        main.ClassName = item.ClassName;
                        main.TypeName = item.TypeName;
                        main.ProMainName = item.ProMainName;
                        models.Add(main);
                    }
                }
                GridProMain.Rebind();
            }
        }

        /// <summary>
        /// 验证总商品是否已经存在
        /// </summary>
        /// <param name="mainid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool ValidateMainPro(int mainid, List<API.RulesProMain> list)
        {
            if (list == null)
                return false;
            foreach (var vm in list)
            {
                if (vm.ProMainID == mainid)
                {
                    return true;
                }
            }
            return false;
        }

        private void deletePM_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridPros.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的商品！");
                return;
            }
             foreach (var item in GridPros.SelectedItems)
            {
                API.Rules_ImportModel xx = item as API.Rules_ImportModel;
                foreach (var child in uncheckModels)
                {
                    if (child == xx)
                    {
                        uncheckModels.Remove(child);
                        break;
                    }
                }
            }
             GridPros.Rebind();
         
            //if (GridProMain.SelectedItems == null)
            //{
            //    MessageBox.Show("请选择要删除的商品！");
            //    return;
            //}

            //foreach (var item in GridProMain.SelectedItems)
            //{
            //    API.RulesProMain xx = item as API.RulesProMain;
            //    foreach (var child in models)
            //    {
            //        if (child == xx)
            //        {
            //            models.Remove(child);
            //            break;
            //        }
            //    }
            //}

            //GridProMain.Rebind();
        }


        #endregion  

        #region  添加规则类型

        private void addRuleType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridProMain.SelectedItem == null)
            {
                MessageBox.Show("请选择需添加的主商品！");
                return;
            }

            MultSelecter2 msFrm = new MultSelecter2(
              null, Store.RulesTypeInfo, "RulesName",
              new string[] { "RulesName" },
              new string[] { "规则类型" });
            msFrm.Closed += RulesType_Closed;
            msFrm.ShowDialog();
        }

        private void RulesType_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<API.Rules_TypeInfo> piList = result.SelectedItems.OfType<API.Rules_TypeInfo>().ToList();
                if (piList.Count == 0) return;


                foreach (var item in GridProMain.SelectedItems)
                {
                    API.RulesProMain model = item as API.RulesProMain;
                    if (model.Pro_RulesTypeInfo == null)
                    {
                        model.Pro_RulesTypeInfo = new List<API.Pro_RulesTypeInfo>();
                    }

                    foreach (var childx in piList)
                    {
                        if (ValideRuleType(childx.ID, model.Pro_RulesTypeInfo))
                        {
                            API.Pro_RulesTypeInfo rpr = new API.Pro_RulesTypeInfo();
                            rpr.RulesTypeID = childx.ID;
                            rpr.RulesTypeName = childx.RulesName;
                            model.Pro_RulesTypeInfo.Add(rpr);
                        }
                    }
                }
                API.RulesProMain xx = GridProMain.SelectedItem as API.RulesProMain;
                GridRuleOff.ItemsSource = xx.Pro_RulesTypeInfo;
                GridRuleOff.Rebind();
            }
        }

        private bool ValideRuleType(int ruleType, List<API.Pro_RulesTypeInfo> list)
        {
            foreach (var item in list)
            {
                if (item.RulesTypeID == ruleType)
                {
                    return false;
                }
            }
            return true;
        }


        private void delteRuleType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridRuleOff.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的规则！");
                return;
            }
            API.RulesProMain model = GridProMain.SelectedItem as API.RulesProMain;
            if (model.Pro_RulesTypeInfo == null)
            {
                model.Pro_RulesTypeInfo = new List<API.Pro_RulesTypeInfo>();
            }

            foreach (var item in GridRuleOff.SelectedItems)
            {
                API.Pro_RulesTypeInfo rt = item as API.Pro_RulesTypeInfo;
                foreach (var child in model.Pro_RulesTypeInfo)
                {
                    if (rt == child)
                    {
                        model.Pro_RulesTypeInfo.Remove(child);
                        break;
                    }
                }
            }
            GridRuleOff.ItemsSource = model.Pro_RulesTypeInfo;
            GridRuleOff.Rebind();
        }

        #endregion 

        #endregion 
    }
}
