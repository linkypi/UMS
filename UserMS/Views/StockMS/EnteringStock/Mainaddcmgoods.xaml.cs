using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.EnteringStock
{
    public partial class Mainaddcmgoods : BasePage
    {

        HallFilter filter;//仓库过滤

        ProductionFilter ProFilter;//商品过滤
        List<API.Pro_HallInfo> hall;//全局变量（仓库）
        string r = "";

        public Mainaddcmgoods()
        {
            InitializeComponent();
            if (Store.LoginUserName != "1")
            {
                this.HallID.IsEnabled = false;
            }

            //显示商品源
            HallID.TextBox.ItemsSource = Store.ProHallInfo;
            HallID.TextBox.DisplayMemberPath = "HallName";

            DGPro.SelectionChanged += DGCardType_SelectionChanged;

            this.userID.Text = Store.LoginUserInfo.UserName;//初始化用户

            this.cancel.Click += cancel_Click;//取消操作

            this.Sumbit.Click += new Telerik.Windows.RadRoutedEventHandler(Sumbit_Click);//提交
      

            this.HallID.SearchButton.Click += SearchButton_Click;

           
            inDate.SelectedValue = DateTime.Now;


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                r = "7";
            }
            finally
            {
                GetFristHall();
            }
        }

        private void GetFristHall()
        {
            filter = new HallFilter(false, ref this.HallID);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo.Where(p => p.CanIn == true).ToList();
            hall = filter.FilterHall(int.Parse(r), HallInfo);
            if (hall == null)
            {
                return;
            }
            this.HallID.TextBox.SearchText = hall.First().HallName;
            this.HallID.Tag = hall.First().HallID;
        }

        /// <summary>
        /// 选择商品行，更换串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region
        void DGCardType_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {

            //if (vmodels != null && this.DGPro.SelectedItem != null)
            //{
            //    SlModel.ViewModel pinfo = DGPro.SelectedItem as SlModel.ViewModel;
            //   // 是否需要串码

            //    if (pinfo.IsNeedIMEI == false)//不需要串码
            //        this.DGPro.Columns[4].IsReadOnly = false;
            //    else//需要串码
            //        this.DGPro.Columns[4].IsReadOnly = true;

            //    if (fail_list != null)
            //    {
            //        List<SlModel.Remind_fail> showinfo = new List<SlModel.Remind_fail>();
            //        foreach (var index in fail_list)
            //        {
            //            if (showinfo == null)
            //            {
            //                showinfo = new List<SlModel.Remind_fail>();
            //            }
            //            if (index.ProID == pinfo.ProID)
            //            {
            //                SlModel.Remind_fail info = new SlModel.Remind_fail();
            //                info.Imei = index.Imei;
            //                info.Note = index.Note;
            //                showinfo.Add(info);
            //            }
            //        }
            //        DGCardType1.ItemsSource = showinfo;
            //        DGCardType1.Rebind();
            //    }
            //}
        }
        #endregion
      

        void cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (PormptPage.PormptMessage("以下操作将初始化数据！", "是否取消"))
                Clear();
        }
        

        #region 提交
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            List<API.SeleterModel> vmodels = this.DGPro.ItemsSource as List<API.SeleterModel>;
            if (this.HallID.TextBox.SearchText == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加仓库");
                return;
            }

            //添加表头
            API.Pro_InOrder inorder = inorder = new API.Pro_InOrder();
            inorder.OldID = this.oldID.Text;
            inorder.Pro_HallID = this.HallID.Tag.ToString();
            inorder.Note = this.tbNote.Text.Trim();


            inorder.UserID = Store.LoginUserInfo.UserID;
            if (String.IsNullOrEmpty(inDate.SelectedValue.ToString()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写时间");
                return;
            }
            inorder.InDate = inDate.SelectedValue;
            inorder.SysDate = DateTime.Now;

            if (vmodels == null || vmodels.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加商品");
                return;
            }

            API.Pro_InOrderList inOdList = null;
            foreach (var vm in vmodels)
            {
                //添加明细
                inOdList = new API.Pro_InOrderList();
                inOdList.ProID = vm.ProID;
                //if (vm.Price == 0)
                //{
                //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加成本价");
                //    return;
                //}
                try
                {
                    inOdList.Price = vm.Price;                
                    if (inOdList.Price==0)
                    {
                        if (MessageBox.Show(System.Windows.Application.Current.MainWindow,vm.ProName+"成本价确定为0？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                            return;
                    }
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"输入值无效！");
                    return;
                }
                try
                {
                    inOdList.RetailPrice = vm.RetailPrice;
                    if (inOdList.RetailPrice == 0)
                    {
                        if (MessageBox.Show(System.Windows.Application.Current.MainWindow, vm.ProName + "零售价确定为0？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                            return;
                    }
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "输入值无效！");
                    return;
                }

                if (vm.Note != null)
                    inOdList.Note = vm.Note;
                if (vm.Count == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"无串码商品请添加数量");
                    return;
                }
         
                inOdList.ProCount = vm.Count;
                //添加串码 和串码明细

                inOdList.Pro_InOrderIMEI = null;
                if (vm.IsIMEI != null)
                {
                    foreach (var i in vm.IsIMEI)
                    {
                        API.Pro_InOrderIMEI in_imei = new API.Pro_InOrderIMEI();
                        //添加入库串码所在的仓库 即总仓
                        in_imei.IMEI = i.IMEI;
                        if (inOdList.Pro_InOrderIMEI == null)
                        {
                            inOdList.Pro_InOrderIMEI = new List<API.Pro_InOrderIMEI>();
                        }
                        inOdList.Pro_InOrderIMEI.Add(in_imei);
                    }
                }

                //添加入库明细
                if (inorder.Pro_InOrderList == null)
                {
                    inorder.Pro_InOrderList = new List<API.Pro_InOrderList>();
                }
                inorder.Pro_InOrderList.Add(inOdList);
            }

            PublicRequestHelp help = new PublicRequestHelp(this.PageBusy, 1, new object[] { inorder }, SubmitCompleted);

        }

        #endregion

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mcea"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs mcea)
        {
            PageBusy.IsBusy = false;
            if (mcea.Error == null)
            {
                API.WebReturn eb = mcea.Result;
                bool flag = eb.ReturnValue;
                if (flag == false)
                {
                    Logger.Log("入库操作失败！");
                    if (eb.Message != null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,eb.Message);
                    }
                    if (eb.Obj != null)
                    {
                        this.DGPro.SelectedItems.Clear();
                        API.Pro_InOrder head = eb.Obj as API.Pro_InOrder;
                        List<API.SeleterModel> model=this.DGPro.ItemsSource as List<API.SeleterModel>;
                        if (head != null&&head.Pro_InOrderList!=null)
                        {
                            var query = from b in head.Pro_InOrderList
                                        join c in model on b.ProID equals c.ProID
                                        select new
                                        {
                                            b,
                                            c
                                        };
                            foreach (var Item in query)
                            {
                                //if(Item.b.Pro_IMEI==null) continue;                          
                                if (Item.b.Pro_InOrderIMEI != null && Item.c.IsIMEI != null)
                                {
                                    Item.c.IsIMEI.Clear();
                                    foreach (var IMEI in Item.b.Pro_InOrderIMEI)
                                    {
                                        if (IMEI.IMEI == null) continue;
                                        API.SelecterIMEI FailIMEI = new API.SelecterIMEI() { IMEI = IMEI.IMEI, Note = IMEI.Note };
                                        Item.c.IsIMEI.Add(FailIMEI);
                                    }
                                }
                            }
                        }                     
                    }
                }
                else
                {
                    ///清空数据
                    Clear();
                    Logger.Log("入库操作成功！");
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,eb.Message);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器出现异常!");
            }
        }

        #region 清空数据
        private void Clear()
        {
            GetFristHall();
            this.DGPro.ItemsSource = null;
            this.DGPro.Rebind();
            this.tbNote.Text = string.Empty;
            this.oldID.Text = String.Empty;
            DGIMEI.ItemsSource = null;
            DGIMEI.Rebind();
            this.txt_iMEI.Text = String.Empty;
        }
        #endregion
       
        /// <summary>
        /// 选择仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            if (hall == null)
                hall = new List<API.Pro_HallInfo>();
            filter.GetHall(hall);

        }

        private void DGCardType_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            API.SeleterModel Item = this.DGPro.SelectedItem as API.SeleterModel;

            if (Item.RetailPrice < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "零售价不能为负数");
                Item.RetailPrice = 0;
                return;
            }
            try
            {
                int value = (int)(Item.RetailPrice * 100);
                Item.RetailPrice = (decimal)(value * 0.01);

            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "零售价请输入的正确数值！");
                Item.RetailPrice = 0;
            }

            if (Item.Price < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"成本价不能为负数");
                Item.Price = 0;
                return;
            }
            try
            {
                int value = (int)(Item.Price * 100);
                Item.Price = (decimal)(value * 0.01);

            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请成本价请输入的正确数值！");
                Item.Price = 0;
            }
            if (Item.Count <0 )
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品数量不能为负数");
                Item.Count = 0;
                return;
            }
            if (Item.ISdecimals == false||Item.ISdecimals==null)
            {
                try
                {
                    Item.Count =(int)Item.Count;
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入正确数值！");

                }
            }
            try
            {
                int value = (int)(Item.Count * 100);
                Item.Count = (decimal)(value*0.01);
            
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入正确的数值！");
                Item.Count = 0;
            }
        }
        #region 添加商品
        private void Add_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            try
            {
                List<API.SeleterModel> Source = this.DGPro.ItemsSource as List<API.SeleterModel>;
                if (Source == null)
                    Source = new List<API.SeleterModel>();
                NotNewBaseProFilter AddPro = new NotNewBaseProFilter(ref Source, ref this.DGPro);
                AddPro.ProFilter(AddPro.GetPro(int.Parse(r.Trim())), true);

            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获取菜单ID失败！");
            }
        }
        #endregion

        #region 添加串码
        private void addIMEI_Click_1(object sender, RoutedEventArgs e)
        {
            AddIMEI();
        }
        private void txt_iMEI_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddIMEI();
            }
        }
        private void AddIMEI()
        {
            if (DGPro.SelectedItems.Count() != 1)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择一个商品！");
                return;
            }
            API.SeleterModel pinfo = DGPro.SelectedItem as API.SeleterModel;
            if (pinfo.IsNeedIMEI != true)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无串码商品，请填写数量");
                return;
            }
            if (String.IsNullOrEmpty(this.txt_iMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品串码不能为空");
                return;
            }

            if (pinfo.IsIMEI == null)
            {
                pinfo.IsIMEI = new List<API.SelecterIMEI>();
            }
            List<string> list = new List<string>(txt_iMEI.Text.Split("\r\n".ToCharArray()));
            foreach (string s in list)
            {
                if (!string.IsNullOrEmpty(s.Trim()))
                {
                    if (!ValidateIMEI(s.Trim()))  //去除重复项
                    {
                        API.SelecterIMEI IMEI = new API.SelecterIMEI() { IMEI = s.ToUpper().Trim()};
                        pinfo.IsIMEI.Add(IMEI);
                        pinfo.Count += 1;

                    }
                }
            }
            txt_iMEI.Text = string.Empty;
            this.DGIMEI.ItemsSource = pinfo.IsIMEI;
            this.DGIMEI.Rebind();
            DGPro.Rebind();
        }
        private bool ValidateIMEI(string imei)
        {
            API.SeleterModel pinfo = DGPro.SelectedItem as API.SeleterModel;
            if (PormptPage.IsMax(imei, 12) == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"字符串不能超过24位！");
                return true;
            }
            var query = (from b in pinfo.IsIMEI
                         where b.IMEI == imei
                         select b).ToList();
            if (query.Count() > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 选择发生改变时
        private void DGPro_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (this.DGPro.SelectedItems.Count() != 1)
            {
                this.DGIMEI.ItemsSource = null;
                this.DGIMEI.Rebind();
                return;
            }
            API.SeleterModel pinfo = DGPro.SelectedItem as API.SeleterModel; // 是否需要串码
            if (pinfo.IsNeedIMEI == false)//不需要串码
                this.DGPro.Columns[4].IsReadOnly = false;
            else//需要串码
                this.DGPro.Columns[4].IsReadOnly = true;
            DGIMEI.ItemsSource = pinfo.IsIMEI;
            DGIMEI.Rebind();
        }
        #endregion

        #region 删除串码
        private void DeleteIMEI_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除串码？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            List<API.SelecterIMEI> IMEIList = this.DGIMEI.ItemsSource as List<API.SelecterIMEI>;
            API.SeleterModel proInfo = this.DGPro.SelectedItem as API.SeleterModel;
            if(IMEIList==null) return;
            foreach (var Item in DGIMEI.SelectedItems)
            {
                IMEIList.Remove(Item as API.SelecterIMEI);
                proInfo.Count -= 1;
            }
            this.DGPro.Rebind();
            this.DGIMEI.Rebind();
        }
        #endregion 

        #region 删除商品
        private void Delete_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (PormptPage.PormptMessage("删除商品？", "是否删除"))
            {
                if (DGPro.SelectedItems == null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项！");
                    return;
                }
                List<API.SeleterModel> ProList = this.DGPro.ItemsSource as List<API.SeleterModel>;
                foreach (var item in this.DGPro.SelectedItems)
                {
                    ProList.Remove(item as API.SeleterModel);
                }
                this.DGPro.Rebind();
            }
        }
        #endregion 
    }

}

