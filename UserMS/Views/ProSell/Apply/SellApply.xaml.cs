using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Apply
{
    public partial class SellApply : Page
    {
        private HallFilter hadder = null;
        List<API.View_SellTypeProduct> unCheckModels = null;
        List<API.View_SellTypeProduct> checkedModels = null;
       // private ProAdder<API.AduitListInfo> proadder = null;

        private List<API.View_SellTypeProduct> ProInfo = null;
        private List<TreeViewModel> ParentTree = null;

        private string menuid = "";
        private string totalName = "";

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
          
            menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            totalName = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["Name"];
            if (menuid == null)
            {
                menuid = "27";
            }
         
        
            ParentTree = new List<TreeViewModel>();
            ProInfo = new List<API.View_SellTypeProduct>();
            unCheckModels = new List<API.View_SellTypeProduct>();
            checkedModels = new List<API.View_SellTypeProduct>(); // checkedModels[1].
            // GridUnCheck.ItemsSource = unCheckModels;
            GridChecked.ItemsSource = checkedModels;

            hadder = new HallFilter(false, ref this.HallID);
            List<API.Pro_HallInfo> halls = hadder.FilterHall(int.Parse(menuid), Store.ProHallInfo);
            if (halls.Count != 0)
            {
                HallID.Tag = halls[0].HallID;
                HallID.TextBox.SearchText = halls[0].HallName;
                //proadder = new ProAdder<API.AduitListInfo>(ref unCheckModels, ref GridUnCheck, 27, 3);
            }
            this.AduitID.Text = "系统自动生成";
            this.applyUser.Text = Store.LoginUserInfo.UserName;

            this.submit.Click += submit_Click;
            this.HallID.SearchButton.Click += SearchButton_Click;
            this.AddPro.Click += AddPro_Click;
            // this.check.Click += check_Click;
            PublicRequestHelp prh2 = new PublicRequestHelp(this.busy, 155, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetProPriceCompleted));
      
        }

        public SellApply()
        {
            InitializeComponent();
        }


        private void GetProPriceCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Error!=null)
            {
                MessageBox.Show(e.Error.Message);
            }
            if (e.Result.ReturnValue)
            {
                ProInfo.Clear();
                List<API.View_SellTypeProduct> list = e.Result.Obj as List<API.View_SellTypeProduct>;

                //初始批发价格默认取原始批发单价
                //foreach (var item in list)
                //{
                //    item.NewPrice = item.Price;
                //}
                ProInfo.AddRange(list);
            }
        }

        #region "拣货"

        /// <summary>
        /// 拣货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void check_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (unCheckModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加商品");
                return;
            }
            foreach (var item in unCheckModels)
            {
                if (item.ProCount == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品 "+item.ProName+" 数量不能为0");
                    return;
                }
          
            }

            if (string.IsNullOrEmpty(HallID.TextBox.SearchText))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择营业厅");
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(null,46,new object[]{unCheckModels,HallID.Tag.ToString()},new EventHandler<API.MainCompletedEventArgs>(CheckCompleted));
        }

        /// <summary>
        /// 拣货完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckCompleted(object sender, API.MainCompletedEventArgs e)
        {
            checkedModels.Clear();
            List<API.View_SellTypeProduct> list = e.Result.Obj as List<API.View_SellTypeProduct>;
            Logger.Log("拣货完毕");

            if (e.Result.ReturnValue)
            {
                checkedModels.AddRange(list);
            }
            unCheckModels.Clear();
            unCheckModels.AddRange(list);
            //GridUnCheck.Rebind();
            GridChecked.Rebind();
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除未拣商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteUnCheck_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridChecked.SelectedItems.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项");
            }
            else
            {
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }
                foreach (var item in GridChecked.SelectedItems)
                {
                    API.AduitListInfo ai = item as API.AduitListInfo;
                    foreach (var child in unCheckModels)
                    {
                        if (ai.ProID == child.ProID)
                        {
                            unCheckModels.Remove(child);
                            break;
                        }
                    }
                }
                GridChecked.Rebind();
            }
        }

        /// <summary>
        /// 删除已拣商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteCheck_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            if (GridChecked.SelectedItems.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项");
            }
            else
            {
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }
                foreach (var item in GridChecked.SelectedItems)
                {
                    API.View_SellTypeProduct ai = item as API.View_SellTypeProduct;
                    foreach (var child in checkedModels)
                    {
                        if (ai.ProID == child.ProID)
                        {
                            checkedModels.Remove(child);
                            break;
                        }
                    }
                } 
                GridChecked.Rebind();
            }
        }

        #endregion 

        #region  提交申请

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void submit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var sell = from s in Store.Options
                       where s.ClassName == totalName
                       select s;
            if (sell.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"找不到商品批发单号，配置有误");
                return;
            }
            //if (string.IsNullOrEmpty(this.orderID.Text))
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入原始单号！");
            //    return;
            //}
            //if (this.orderID.Text.Length != 7)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"原始单号有误！") ;
            //    return;
            //}
            if (string.IsNullOrEmpty(this.cusname.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"客户名称不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(this.cusphone.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"客户电话不能为空！");
                return;
            }
            int selltype = Convert.ToInt32(sell.First().Value); 
            if (string.IsNullOrEmpty(this.HallID.TextBox.SearchText))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择营业厅");
                return;
            }
            foreach (var item in checkedModels)
            {
                if (item.ProCount <= 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品数量必须大于0");
                    return;
                }
            }
            if (!PormptPage.isNumeric(this.cusphone.Text.Trim()))
            {
                MessageBox.Show("联系电话无效");
                return;
            }
            if (checkedModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加您要申请的数据！");
                return;
            }
          

            API.Pro_SellAduit psa = new API.Pro_SellAduit();
            psa.HallID = this.HallID.Tag.ToString();
            psa.ApplyUser = Store.LoginUserInfo.UserID;
            psa.ApplyDate = DateTime.Now;
            psa.SysDate = DateTime.Now;
            psa.Note = this.note.Text;
            psa.CustName = this.cusname.Text;
            psa.CustPhone = this.cusphone.Text;

            psa.Pro_SellAduitList = new List<API.Pro_SellAduitList>();
            API.Pro_SellAduitList salist = null;
            foreach (var item in checkedModels)
            {
                if (item.NewPrice > item.MaxPrice || item.NewPrice < item.MinPrice)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请确保批发单价在有效范围内！");
                    item.NewPrice = 0;
                    return;
                }

                salist = new API.Pro_SellAduitList();
                salist.ProID = item.ProID; 
                salist.ProCount =(int) item.ProCount;
                salist.ProPrice = (decimal)item.NewPrice;
                salist.OffMoney = (decimal)item.Price - (decimal)item.NewPrice;

                salist.SellTypeID = selltype;
                salist.Note = item.Note;
                psa.Pro_SellAduitList.Add(salist);
              
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定提交申请吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(null, 25, new object[] {psa }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "申请失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"提交成功");
                Logger.Log("提交成功");

                this.HallID.TextBox.SearchText = string.Empty;
                unCheckModels.Clear();
                checkedModels.Clear();
                GridChecked.Rebind();

                List<API.Pro_HallInfo> halls = hadder.FilterHall(int.Parse(menuid), Store.ProHallInfo);
                if (halls.Count != 0)
                {
                    HallID.Tag = halls[0].HallID;
                    HallID.TextBox.SearchText = halls[0].HallName;
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                
                Logger.Log("提交失败");
            }
        }

        #endregion 

        /// <summary>
        /// 选择营业厅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hadder.GetHall(hadder.FilterHall(int.Parse(menuid),Store.ProHallInfo));
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

            List<string> proids = new List<string>();
            foreach (var item in ProInfo)
            {
                proids.Add(item.ProID);
            }

            Common.CommonHelper.ProFilterGen(Store.ProInfo.Where(p => proids.Contains(p.ProID)).ToList(), ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
            ProInfo, "ProName", new string[] { "ClassName", "TypeName", "ProName", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "商品属性"});
            m.Closed += msFrm_Closed;
            m.ShowDialog();

            //MultSelecter2 msFrm = new MultSelecter2(
            //ParentTree,
            //ProInfo, "ProName",
            //new string[] { "ClassName", "TypeName", "ProName", "ProFormat", "Price", "MinPrice", "MaxPrice" },
            //new string[] { "商品类别", "商品品牌", "商品型号", "商品属性", "价格", "最低价", "最高价" });

            //msFrm.Closed += msFrm_Closed;
            //msFrm.ShowDialog();
            
        }

        private void msFrm_Closed(object sender, WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                if (result.SelectedItems.Count == 0) return; 

                foreach (var item in result.SelectedItems)
                {
                    API.View_SellTypeProduct vp  =item as API.View_SellTypeProduct;
                    API.View_SellTypeProduct child = new API.View_SellTypeProduct();
                 
                    child = vp;
                    child.ProCount = 0;
                    child.Note = string.Empty;
                    child.NewPrice = 0;
                    if (!ValidateProduction(child.ProID))
                    {
                        checkedModels.Add(child);
                        //unCheckModels.Add(item);
                    }
                }
                GridChecked.Rebind();
               // GridUnCheck.Rebind();
            }
        }

        private bool ValidateProduction(string proid)
        {
            foreach (var item in checkedModels)
            {
                if (proid == item.ProID)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 验证是否存在同种商品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool ValidateProc(string pid)
        {
            foreach (var a in unCheckModels)
            {
                if (a.ProID == pid)
                {
                    return true;
                }
            }
            return false;
        }

        private void GridChecked_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            foreach (var item in checkedModels)
            {
                if (item.ProCount < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"数量不能为负数！");
                    item.ProCount = 0;
                   // return;
                }
                if (!Convert.ToBoolean(item.ISdecimals))
                {
                    item.ProCount = (int)(Decimal.Truncate(Convert.ToDecimal(item.ProCount * 100)) / 100);
                   // continue;
                }
                if (item.NewPrice < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "数量不能为负数！");
                    item.NewPrice = 0;
                    return;
                }
               
                //if (!item.IsDecimal)
                //{
                //    item.NewPrice = (int)(Decimal.Truncate(Convert.ToDecimal(item.NewPrice * 100)) / 100);
                //    continue;
                //}

                item.NewPrice = Decimal.Truncate(Convert.ToDecimal(item.NewPrice * 100)) / 100;
                item.ProCount = Decimal.Truncate(Convert.ToDecimal(item.ProCount * 100)) / 100;
            }
        }

      
    }
}
