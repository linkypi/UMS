using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using UserMS.Common;
using UserMS.MyControl;

namespace UserMS.Views.StockMS.Borrowing
{
    public partial class BorowApply : BasePage
    {
        private List<API.BAduitModel> checkedModels = null;
        private List<API.BAduitModel> unCheckModels = null;

        private HallFilter hadder = null;
        private Brush oldBrush = null;
     
        string menuid ;
        List<TreeViewModel> parent = new List<TreeViewModel>();
        private List<TreeViewModel> treeModels;
        private List<SlModel.BaseModel> proModels = null;
        private MultSelecter2 msFrm = null;

        //内部借贷
        private bool internalBorow = false;

        public BorowApply()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
                if (string.IsNullOrEmpty(menuid))
                {
                    menuid = "17";
                }
            }
            catch (Exception ex)
            {
                menuid = "17";
            }

          
            foreach (var item in Store.DeptInfo)
            {
                if (item.Parent == 0)
                {
                    TreeViewModel t1 = new TreeViewModel();
                    t1.Fields = new string[] { "DtpID" };
                    t1.Values = new object[] { item.DtpID };
                    t1.ID = item.DtpID.ToString();
                    t1.Title = item.DtpName;
                    Recursion(item, t1);
                    parent.Add(t1);
                }
            }

            List<SlModel.CkbModel> bt = new List<SlModel.CkbModel>();
            bt.Add(new SlModel.CkbModel(false, "内部借机"));
            bt.Add(new SlModel.CkbModel(false, "政企借机"));//社渠借机
            bt.Add(new SlModel.CkbModel(false, "社渠借机"));
            this.borowType.ItemsSource = bt;
            this.borowType.SelectedIndex = 0;

            List<SlModel.CkbModel> bd = new List<SlModel.CkbModel>();
            bd.Add(new SlModel.CkbModel(false, "主业"));
            bd.Add(new SlModel.CkbModel(false, "广信"));
            this.borrowdept.ItemsSource = bd;
            this.borrowdept.SelectedIndex = 0;

            checkedModels = new List<API.BAduitModel>();
            unCheckModels = new List<API.BAduitModel>();

            GridCheckedPro.ItemsSource = checkedModels;
            //GridApplyPro.ItemsSource = unCheckModels;
           
            hadder = new HallFilter(false, ref HallID);
             List<API.Pro_HallInfo> halls = hadder.FilterHall(int.Parse(menuid), Store.ProHallInfo);
             if (halls.Count != 0)
             {
                 this.HallID.Tag = halls.First().HallID;
                 this.HallID.TextBox.SearchText = halls.First().HallName;
             }

             #region   初始化商品列表

              treeModels = new List<TreeViewModel>();

            List<API.Pro_ProInfo> prods = new List<API.Pro_ProInfo>();
            var menuInfo = Store.RoleInfo.First().Sys_Role_MenuInfo.Where(p => p.MenuID == int.Parse(menuid));
            if (menuInfo != null)
            {
                // 左连接获取商品列表              
                var  query = (from b in Store.ProInfo
                            join c in Store.RoleInfo.First().Sys_Role_Menu_ProInfo
                            on b.Pro_ClassID equals c.ClassID
                            where c.MenuID ==  int.Parse(menuid)
                            select b).ToList();
                if (query.Count() > 0)
                {
                    prods = query;
                }
            }
            SlModel.BaseModel bm = null;

            if (proModels == null)
            {
                proModels = new List<SlModel.BaseModel>();
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
                    proModels.Add(bm);
                }
            }

             #endregion 

            this.applyUser.Text = Store.LoginUserInfo.UserName;
            this.applyDate.Text = DateTime.Now.ToShortDateString();
            this.oldBrush = this.applyDate.Background;

            this.Sumbit.Click += Sumbit_Click;
            this.HallID.SearchButton.Click += SearchButton_Click;
            this.delChecked.Click += delChecked_Click;
            this.cancel.Click += cancel_Click;
        }


        void cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定取消吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            Clear();
        }

        #region "删除"

        /// <summary>
        /// 右键删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delunCheck_CTM_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DeleteUnCheckPro();
        }

        /// <summary>
        /// 删除未拣商品
        /// </summary>
        private void DeleteUnCheckPro()
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            if (GridCheckedPro.SelectedItems != null)
            {
                API.BAduitModel am = null;
                foreach (var item in GridCheckedPro.SelectedItems)
                {
                    am = item as API.BAduitModel;
                    foreach (var ucm in unCheckModels)
                    {
                        if (am == ucm)
                        {
                            unCheckModels.Remove(ucm);
                            break;
                        }
                    }
                }
            }
            GridCheckedPro.Rebind();
        }

        /// <summary>
        /// 右键删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheck_CTM_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DeleteCheckedPro();
        }

        /// <summary>
        /// 删除已拣商品
        /// </summary>
        private void DeleteCheckedPro()
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            if (GridCheckedPro.SelectedItems != null)
            {
                foreach (var item in GridCheckedPro.SelectedItems)
                {
                   API.BAduitModel am = item as API.BAduitModel;
                    foreach (var ucm in checkedModels)
                    {
                        if (am.ProID == ucm.ProID)
                        {
                            checkedModels.Remove(ucm);
                            break;
                        }
                    }
                }
            }
            GridCheckedPro.Rebind();
        }

        void delUnCheck_Click(object sender, RoutedEventArgs e)
        {
            DeleteUnCheckPro();
        }

        void delChecked_Click(object sender, RoutedEventArgs e)
        {
            DeleteCheckedPro();
           // ViewCommon.DelCheckedPro(ref checkedModels, ref GridCheckedPro);
        }

        #endregion

        #region  "拣货"

        /// <summary>
        /// 拣货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.HallID.TextBox.SearchText.ToString().Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择仓库");
               // ShowFlash(this.HallID.TextBox);
                return;
            }
            List<API.SetSelection > list = new List<API.SetSelection>();

            API.SetSelection ss = null;
            foreach (var item in unCheckModels)
            {
                if (item.ProCount == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品数量不能为0");
                    return;
                }
                //判断数量不为小数的情况
                if (!item.IsDecimal && ((int)item.ProCount != item.ProCount))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品" + item.ProName + "的数量不能为小数！");
                    return;
                }
                ss = new API.SetSelection();
                ss.Proid = item.ProID;

                ss.Countnum = item.ProCount;
                list.Add(ss);
            }

            if (list.Count() != 0)
            {
                PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 23, new object[]{list, this.HallID.Tag.ToString() },new EventHandler<API.MainCompletedEventArgs>(CheckCompeleted));
            }
        }

        /// <summary>
        /// 拣货完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckCompeleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            Logger.Log("拣货完成");
            checkedModels.Clear();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器错误\n" + e.Error.Message);
                return;
            }
            List<API.SetSelection> models = new List<API.SetSelection>();
          //  if (e.Result.ReturnValue)
            
               models =  e.Result.Obj as List<API.SetSelection>;
            

            API.BAduitModel am = null;
        
            foreach (var item in models)
            {
                if (item.Sucess)
                {
                    item.Note = "成功";
                }
                //else
                //{
                //    item.Note = "失败";
                //}
                foreach (var key in unCheckModels)
                {
                    if (key.ProID == item.Proid)
                    {
                        key.Note = item.Note;
                    }
               }
                if (e.Result.ReturnValue)
                {
                    am = new API.BAduitModel();
                    am.ProID = item.Proid;
                    var product = from p in Store.ProInfo
                                  join t in Store.ProTypeInfo on p.Pro_TypeID equals t.TypeID
                                  join c in Store.ProClassInfo on p.Pro_ClassID equals c.ClassID
                                  where p.ProID == am.ProID
                                  select new { 
                                       p.ProName,
                                       t.TypeName,
                                       c.ClassName
                                  };
                    am.ProName = product.First().ProName;
                    am.ClassName = product.First().ClassName;
                    am.TypeName = product.First().TypeName;
                    am.ProCount = item.Countnum;
                    am.Note = item.Note;
                    checkedModels.Add(am); 
                }
            }
            GridCheckedPro.Rebind();
           // GridApplyPro.Rebind();
        }

        #endregion 


        #region  新增商品
           /// <summary>
        /// 验证是否存在同种商品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool ValidateProc(string pid)
        {
            foreach (var vm in checkedModels)
            {
                if (vm.ProID== pid)
                {
                    return true;
                }
            }
            return false;
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

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            msFrm = new MultSelecter2(
              treeModels,
              proModels, "ProName",
              new string[] { "ClassName", "TypeName", "ProName", "ProFormat" },
              new string[] { "商品品牌", "商品类别", "商品型号", "商品属性" });
            msFrm.Closed += proFrm_Closed;
            msFrm.ShowDialog();
        }

        /// <summary>
        /// 确定添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proFrm_Closed(object sender, WindowClosedEventArgs e)
        {
            List<SlModel.BaseModel> piList = ((UserMS.MultSelecter2)sender).SelectedItems.OfType<SlModel.BaseModel>().ToList();
            if (piList.Count == 0) return;

            SlModel.BaseModel bm = new SlModel.BaseModel();

            PropertyInfo[] bpis = bm.GetType().GetProperties();

            foreach (SlModel.BaseModel item in piList)
            {
                if (!ValidateProc(item.ProID))
                {
                    API.BAduitModel t = new API.BAduitModel();
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
                  checkedModels.Add(t);
                }
            }
            //GridCheckedPro.Rebind();

            PublicRequestHelp prh = new PublicRequestHelp(null, 240, new object[] { checkedModels }, new EventHandler<API.MainCompletedEventArgs>(GetPriceCompleted));
           
        }

        private void GetPriceCompleted(object sender, API.MainCompletedEventArgs e)
        {
            //if (e.Error != null)
            //{
            //    return;
            //}
            if (e.Result.ReturnValue)
            {
                List<API.BAduitModel> list =e.Result.Obj as List<API.BAduitModel>;
                checkedModels.Clear();
                checkedModels.AddRange(list);
            }
            GridCheckedPro.Rebind();
        }


        #endregion 

        /// <summary>
        /// 添加营业厅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hadder.GetHall(hadder.FilterHall(17,Store.ProHallInfo));
        }

        #region "提交数据"
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Sumbit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (checkedModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"拣货列表无数据");
                return;
            }
            foreach (API.BAduitModel vm in checkedModels)
            {
                if (vm.ProCount <= 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品数量无效");
                    return;
                }
                //判断数量不为小数的情况
                if (!vm.IsDecimal && ((int)vm.ProCount != vm.ProCount))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品" + vm.ProName + "的数量不能为小数！");
                    return;
                }
            }
            //if (string.IsNullOrEmpty(this.orderid.Text))
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入原始单号！");
            //    return;
            //}
            //if (this.orderid.Text.Length != 7)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"原始单号不正确！");
            //    return;
            //}
            if (string.IsNullOrEmpty(this.borowType.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入借贷方式");
                return;
            }

            if (internalBorow)
            {
                MyMulSelect mysel = this.bowrer.Children[1] as MyMulSelect;
                if (string.IsNullOrEmpty(mysel.txt.Text))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择借贷人");
                    return;
                }
            }
            else
            {
                TextBox txt = this.bowrer.Children[1] as TextBox;
                if (string.IsNullOrEmpty(txt.Text))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入借贷人");
                    return;
                }
            }
          
            if (string.IsNullOrEmpty(this.borrowdept.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入借贷部门");
                return;
            }
            if (string.IsNullOrEmpty(this.mobilphone.Text.Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入联系电话");
                return;
            }
            if (!PormptPage.isNumeric(this.mobilphone.Text.Trim()))
            {
                MessageBox.Show("联系电话无效");
                return;
            }
            if (string.IsNullOrEmpty(this.estimateReturnTime.DateTimeText.Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入预计归还时间");
                return;
            }
            foreach (API.BAduitModel vm in checkedModels)
            {
                if (vm.ProPrice <= 0)
                {
                    MessageBox.Show("商品"+vm.ProName+"未定价！");
                    return;
                }
            }

            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定提交申请吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            } 
            
            API.Pro_BorowAduit pba = new API.Pro_BorowAduit();
            if (internalBorow)
            {
                MyMulSelect txt = this.bowrer.Children[1] as MyMulSelect;
              
                pba.Borrower = txt.txt.Text;
            }
            else
            {
                TextBox txt = this.bowrer.Children[1] as TextBox;
                pba.Borrower =txt.Text;
            }

            pba.InternalBorow = internalBorow;
            pba.EstimateReturnTime = this.estimateReturnTime.SelectedDate;
            pba.Note = this.note.Text;
            pba.MobilPhone = this.mobilphone.Text;
            pba.HallID = this.HallID.Tag.ToString().Trim();
            pba.SysDate = DateTime.Now;
            pba.ApplyDate = DateTime.Now;
          
            pba.BorrowType = (this.borowType.SelectedItem as SlModel.CkbModel).Text;
            pba.Dept = (this.borrowdept.SelectedItem as SlModel.CkbModel).Text;
            pba.ApplyUser = Store.LoginUserInfo.UserID;
          

            if (checkedModels.Count != 0)
            {
                pba.Pro_BorowAduitList = new List<API.Pro_BorowAduitList>();
                decimal total = 0;
                API.Pro_BorowAduitList pbd = null;
                foreach (API.BAduitModel vm in checkedModels)
                {
                    pbd = new API.Pro_BorowAduitList();
                    pbd.ProCount = vm.ProCount;
                    pbd.Note = vm.Note;
                    pbd.ProID = vm.ProID;
                    pbd.ProPrice = vm.ProPrice;
                    pba.Pro_BorowAduitList.Add(pbd);
                    total += vm.ProCount * vm.ProPrice;
                }
                pba.TotalMoney = total;
                PublicRequestHelp help = new PublicRequestHelp(this.isbusy, 22, new object[] {pba }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
            }
        }

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "申请失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (!e.Result.ReturnValue)
            {
                Logger.Log("申请失败");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                return;
            }
            else
            {
                Logger.Log("申请成功");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"申请成功");

               // this.HallID.TextBox.SearchText = string.Empty;

                Clear();
            }
        }

        private void Clear()
        {
            this.borowType.Text = string.Empty;
            this.borrowdept.Text = string.Empty;
            this.borrower.Text = string.Empty;
            this.note.Text = string.Empty;
            this.mobilphone.Text = string.Empty;
            this.estimateReturnTime.DateTimeText = string.Empty;
            checkedModels.Clear();
            unCheckModels.Clear();
            //GridApplyPro.Rebind();
            GridCheckedPro.Rebind();
        }

        #endregion

        private void GridCheckedPro_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
           
            foreach (var item in checkedModels)
            {
                if (item.ProCount < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"申请数量不能为负数！");
                    item.ProCount = 0;
                    return;
                }
                if (!item.IsDecimal)
                {
                    item.ProCount = (int)(Decimal.Truncate(Convert.ToDecimal(item.ProCount * 100)) / 100);
                    continue;
                }
                item.ProCount = Decimal.Truncate(Convert.ToDecimal(item.ProCount * 100)) / 100;
                
              //  item.ProCount = Convert.ToDecimal(count.Substring(0, count.IndexOf(".") + 3));
            }
        }

        private void estimateReturnTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadDatePicker rp = sender as RadDatePicker;
            if (rp.SelectedDate < DateTime.Now.Date)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"预计归还时间无效！");
                rp.DateTimeText = string.Empty;//DateTime.Now.Date.ToShortDateString();
            }
        }

        #region 内部借贷 选择借贷人

        /// <summary>
        /// 选择借贷方式时改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void borowType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bowrer.Children.RemoveAt(1);
            if (borowType.SelectedIndex == 0)
            {
                internalBorow = true;
                MyMulSelect cb = new MyMulSelect();
                cb.Name = "borrower";
                cb.Width = 120;
                cb.SearchEvent = new RoutedEventHandler(SearchUser_Click);
                bowrer.Children.Add(cb);
            }
            else
            {
                internalBorow = false;
                TextBox txt = new TextBox();
                txt.Name = "borrower";
                txt.Width = 120;
                bowrer.Children.Add(txt);
            }
        }

        private void SearchUser_Click(object sender, RoutedEventArgs e)
        {
           // MultSelecter msFrm = new MultSelecter(
           //  parent,
           //  Store.UserInfos,
           // "DtpID", "UserName",
           // new string[] {  "UserName" },
           // new string[] {  "用户名称" },false
           //);
            MultSelecter2 msFrm = new MultSelecter2(
            parent,
            Store.UserInfos,
          "UserName",
           new string[] { "UserName" },
           new string[] { "用户名称" } );
         
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();
        }

        /// <summary>
        /// 递归获取部门
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parent"></param>
        private void Recursion(API.Sys_DeptInfo item,TreeViewModel parent)
        {
            var ss = from a in Store.DeptInfo
                     where a.Parent == item.DtpID
                     select a;

            if (ss.Count() != 0)
            {
                foreach (var child in ss)
                {
                    TreeViewModel t1 = new TreeViewModel();
                    t1.Fields = new string[] { "DtpID" };
                    t1.Values = new object[] { child.DtpID };
                    t1.ID = child.DtpID.ToString();
                    t1.Title = child.DtpName;

                    Recursion(child,t1);
                    if (parent.Children == null)
                    {
                        parent.Children = new List<TreeViewModel>();
                    }
                    parent.Children.Add(t1);
                }
            }
        }

        private void msFrm_Closed(object sender, WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 ms = sender as UserMS.MultSelecter2;
            if (ms == null) { return; }
            if (ms.DialogResult == true)
            {
                List<UserMS.API.Sys_UserInfo> phList = ms.SelectedItems.OfType<UserMS.API.Sys_UserInfo>().ToList();
                if (phList.Count == 0) return;

                //textbox.Tag = phList[0].HallID;
                MyMulSelect mt = bowrer.Children[1] as MyMulSelect;
                mt.txt.Text = phList[0].UserName;
            }
        }
        #endregion 
    }
}
