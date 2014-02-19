using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.EnteringStock
{
    public partial class Back_store : BasePage
    {
        API.Pro_BackInfo back_head = new API.Pro_BackInfo();

        /// <summary>
        /// 已拣货列表
        /// </summary>
        private List<API.SeleterModel> checkedModels;
        /// <summary>
        /// 无串码商品
        /// </summary>
        private List<API.SeleterModel> unIMEIModels;
        /// <summary>
        /// 未拣货的串码
        /// </summary>
        private List<API.SelecterIMEI> uncheckIMEI;

        /// <summary>
        /// 拣货器
        /// </summary>
        private NewPicking pickder;

        /// <summary>
        /// 无串码商品添加器
        /// </summary>
        private ProductionFilter adder;

        /// <summary>
        /// 仓库添加器
        /// </summary>
        private HallFilter hAdder;

        /// <summary>
        /// 仓库列表
        /// </summary>
        List<API.Pro_HallInfo> hall;

        string r = "";

        public Back_store()
        {
            InitializeComponent();
            HallID.TextBox.IsEnabled = false;
            checkedModels = new List<API.SeleterModel>();
            uncheckIMEI = new List<API.SelecterIMEI>();
            unIMEIModels = new List<API.SeleterModel>();
            //绑定数据
            GridUnCheckPro.ItemsSource = unIMEIModels;
            GridCheckedPro.ItemsSource = checkedModels;
            GridUnCheckIMEI.ItemsSource = uncheckIMEI;
            HallID.TextBox.ItemsSource = Store.ProHallInfo;
            HallID.TextBox.DisplayMemberPath = "HallName";

            //绑定事件
            GridCheckedPro.SelectionChanged += GridCheckedPro_SelectionChanged;
            this.addNoIMEIPros.Click += Add_Click;
        

            this.delIMEI.Click += delCheckedIMEI_Click; //删除选中的串码
            this.delPro.Click += delCheckedPro_Click;//删除选中的商品

            pickder = new NewPicking(ref this.HallID, ref this.IsBusy, ref checkedModels, ref unIMEIModels,
                            ref uncheckIMEI, ref GridCheckedPro, ref GridUnCheckPro, ref GridUnCheckIMEI);
            adder = new ProductionFilter(ref unIMEIModels, ref GridUnCheckPro);
  
            this.userID.Text = Store.LoginUserInfo.UserName;
            this.cancel.Click += cancel_Click;
            this.Sumbit.Click += new Telerik.Windows.RadRoutedEventHandler(Sumbit_Click);
            this.HallID.SearchButton.Click += new RoutedEventHandler(HallIDSearch_Click);
            this.checkPro.Click += CheckProduct_Click;
            this.toDate.SelectedValue = DateTime.Now;
            this.txtIMEI.KeyUp += txtIMEI_KeyUp;
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
                r = "8";
            }
            finally
            {
                GetFristHall();
            }
        }

        private void GetFristHall()
        {
            hAdder = new HallFilter(false, ref this.HallID);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo.Where(p => p.CanBack == true).ToList();
            hall = hAdder.FilterHall(int.Parse(r.Trim()), HallInfo);
          
            if (hall == null)
            {
                return;
            }
            this.HallID.TextBox.SearchText = hall.First().HallName;
            this.HallID.Tag = hall.First().HallID;
        }
        /// <summary>
        /// 按下确定键添加代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtIMEI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewOperate.BatchAdd(this.txtIMEI.Text.Trim(), ref uncheckIMEI, ref GridUnCheckIMEI);
                txtIMEI.Text = string.Empty;
            }
        }

        void cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if(PormptPage.PormptMessage("是否清空数据？","提示"))
            Clear();
        }
        #region "事件"

        /// <summary>
        /// 拣货单击事件
        /// </summary>
        void CheckProduct_Click(object sender, RoutedEventArgs e)
        {
            if (uncheckIMEI.Count() == 0 && unIMEIModels.Count() == 0) { return; }
            pickder.Packing();
         }


        /// <summary>
        /// 添加无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(HallID.TextBox.SearchText))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择仓库");
                return;
            }
            adder.ProFilter(adder.GetPro(int.Parse(r)), false);
         }

        /// <summary>
        /// 选中已拣货码商品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridCheckedPro_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            ViewOperate.GridSelectChanged(ref GridCheckedPro, ref GridCheckedIMEI);
        }

        /// <summary>
        /// 批量添加商品串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BatchAddIMEI_Click(object sender, RoutedEventArgs e)
        {
            ViewOperate.BatchAdd(this.txtIMEI.Text.Trim(), ref uncheckIMEI, ref GridUnCheckIMEI);
            txtIMEI.Text = string.Empty;
        }

        /// <summary>
        /// 选择营业厅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HallIDSearch_Click(object sender, RoutedEventArgs e)
        {
            if (hall == null)
                hall = new List<API.Pro_HallInfo>();
            hAdder.GetHall(hall);
        }

        #endregion

        #region  "删除操作"

        /// <summary>
        /// 右键删除无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
            {
            // ViewCommon.DeleteSinglePro(ref unIMEIModels, ref GridUnCheckPro);
            }

        /// <summary>
        /// 右键删除选中的串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delIMEI_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
                {
            //  ViewCommon.DeleteSingleIMEI(ref uncheckIMEI, ref GridUnCheckIMEI);
                }
        /// <summary>
        /// 删除Checked选中的商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedPro_Click(object sender, RoutedEventArgs e)
                                {
            ViewOperate.DelCheckedPro(ref unIMEIModels, ref GridUnCheckPro);
            }

        /// <summary>
        /// 删除Checked选中的串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedIMEI_Click(object sender, RoutedEventArgs e)
                    {
            ViewOperate.DelCheckIMEI(ref  uncheckIMEI, ref GridUnCheckIMEI);
        }

        #endregion

        #region 修改接口
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region 提交model

        /// <summary>
        /// 保存提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            if (this.HallID.TextBox.SearchText.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加仓库");
                return;
            }
            //添加表头
            API.Pro_BackInfo order = new API.Pro_BackInfo();
            //原始单号
            order.OldID = this.OldOrderID.Text.Trim();
            //添加仓库
            var query = from b in Store.ProHallInfo
                        where b.HallName == this.HallID.TextBox.SearchText.Trim()
                        select b;
            if (query.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加正确的仓库");
                return;
            }

            order.HallID =HallID.Tag.ToString();
            order.Note = tbNote.Text;
            //获取退库时间
            if (String.IsNullOrEmpty(toDate.SelectedValue.ToString()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写时间");
                return;
            }
            order.BackDate = toDate.SelectedValue;
            //系统时间
            order.SysDate = DateTime.Now;
            if (checkedModels == null || checkedModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加商品");
                return;
            }
            List<string> s = new List<string>();
            List<API.Pro_IMEI_Deleted> del = new List<API.Pro_IMEI_Deleted>();
            foreach (var vm in checkedModels)
            {
                API.Pro_BackListInfo OrderList = new API.Pro_BackListInfo();
                //添加退库明细
                OrderList.ProID = vm.ProID;
                if (vm.Count == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"无串码商品请添加数量");
                    return;
                }
                OrderList.ProCount = vm.Count;
                OrderList.InListID = vm.ProInListID;
                if (!string.IsNullOrEmpty(vm.Note))
                {
                    OrderList.Note = vm.Note;
                }
                //添加串码 和串码明细
                API.Pro_BackOrderIMEI imei = null;
                if (vm.IsIMEI != null)
                {
                    foreach (var i in vm.IsIMEI)
                    {
                        imei = new API.Pro_BackOrderIMEI();
                        imei.IMEI = i.IMEI;
                        //添加删除串码
                        API.Pro_IMEI_Deleted del_imei = new API.Pro_IMEI_Deleted();
                        del_imei.IMEI = i.IMEI;
                        del.Add(del_imei);
                        //添加串码
                        s.Add(i.IMEI);
                        if (OrderList.Pro_BackOrderIMEI == null)
                        {
                            OrderList.Pro_BackOrderIMEI = new List<API.Pro_BackOrderIMEI>();
                        }
                        OrderList.Pro_BackOrderIMEI.Add(imei);
                    }
                }
                if (order.Pro_BackListInfo == null)
                {
                    order.Pro_BackListInfo = new List<API.Pro_BackListInfo>();
                }
                order.Pro_BackListInfo.Add(OrderList);
            }
            PublicRequestHelp help = new PublicRequestHelp(this.IsBusy, 2, new object[] { order, s, del }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mcea"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs mcea)
        {
            this.IsBusy.IsBusy = false;
            try
            {
                API.WebReturn eb = mcea.Result;

            
                if (eb.ReturnValue == false)
                {
                    Logger.Log("操作失败！");
                    if (eb.Message != null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,eb.Message);
                    }
                }
                else
                {
                    ///清空数据

                    Clear();
                    Logger.Log("退库操作成功！");
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,eb.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Clear()
        {
            GetFristHall();
            this.tbNote.Text = string.Empty;
            this.OldOrderID.Text = String.Empty;
            this.txtIMEI.Text = string.Empty;
            //this.HallID.Tag = null;
            this.toDate.SelectedValue = DateTime.Now;

            checkedModels.Clear();
            uncheckIMEI.Clear();
            unIMEIModels.Clear();
            GridCheckedIMEI.ItemsSource = null;
            GridUnCheckPro.Rebind();
            GridCheckedPro.Rebind();
            GridCheckedIMEI.Rebind();
            GridUnCheckIMEI.Rebind();
        }

        #endregion

        private void DGCardType_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            API.SeleterModel Item = this.GridUnCheckPro.SelectedItem as API.SeleterModel;
         
            if (Item.Count < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品数量不能为负数");
                Item.Count = 0;
                return;
            }
            if (Item.ISdecimals == false || Item.ISdecimals == null)
            {
                try
                {
                    Item.Count = (int)Item.Count;
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入正确数值！");

                }
            }
            try
            {
                int value = (int)(Item.Count * 100);
                Item.Count = (decimal)(value * 0.01);

            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入正确的数值！");
                Item.Count = 0;
            }
        }

    }
}
