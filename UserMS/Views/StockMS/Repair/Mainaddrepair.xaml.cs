using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.Repair
{
    public partial class Mainaddrepair : Page
    {
        /// <summary>
        /// 已拣货列表
        /// </summary>
        private List<SlModel.ViewModel> checkedModels = null;
        /// <summary>
        /// 无串码商品
        /// </summary>
        private List<SlModel.ViewModel> unIMEIModels = null;
        /// <summary>
        /// 未拣货的串码
        /// </summary>
        private List<SlModel.CheckModel> uncheckIMEI = null;

        /// <summary>
        /// 拣货器
        /// </summary>
        private PickingDevicer pickder;

        /// <summary>
        /// 无串码商品添加器
        /// </summary>
        private ProAdder<SlModel.ViewModel> adder;

        /// <summary>
        /// 仓库添加器
        /// </summary>
        private HallFilter hAdder;
        private string menuid = "";

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
                if (menuid == null)
                {
                    menuid = "20";
                }
            }
            catch
            {
                menuid = "20";
            }
            finally
            {
                checkedModels = new List<SlModel.ViewModel>();
                uncheckIMEI = new List<SlModel.CheckModel>();
                unIMEIModels = new List<SlModel.ViewModel>();
                //绑定数据
                GridUnCheckPro.ItemsSource = unIMEIModels;
                GridCheckedPro.ItemsSource = checkedModels;
                GridUnCheckIMEI.ItemsSource = uncheckIMEI;
               // HallID.TextBox.ItemsSource = Store.ProHallInfo;
                HallID.TextBox.DisplayMemberPath = "HallName";
                this.repairDate.Content = DateTime.Now.ToShortDateString();
                //绑定事件
                GridCheckedPro.SelectionChanged += GridCheckedPro_SelectionChanged;
                this.addNoIMEIPros.Click += Add_Click;
                this.BatchAddIMEI.Click += BatchAddIMEI_Click; //批量添加

                this.delIMEI.Click += delCheckedIMEI_Click; //删除选中的串码
                this.delPro.Click += delCheckedPro_Click;//删除选中的商品
                //this.delIMEI.Click += delIMEI_Click; //右键删除串码
                //this.delPro.Click += delPro_Click;  //右键删除商品

                pickder = new PickingDevicer(ref this.HallID, ref checkedModels, ref unIMEIModels,
                                ref uncheckIMEI, ref GridCheckedPro, ref GridUnCheckPro, ref GridUnCheckIMEI, ref this.busyIndic);
                adder = new ProAdder<SlModel.ViewModel>(ref unIMEIModels, ref GridUnCheckPro, int.Parse(menuid),2);
                hAdder = new HallFilter(false, ref this.HallID);

                List<API.Pro_HallInfo> Halls = hAdder.FilterHall(int.Parse(menuid), Store.ProHallInfo);
                if (Halls.Count != 0)
                {
                    API.Pro_HallInfo hall = Halls.First();
                    this.HallID.Tag = hall.HallID;
                    this.HallID.TextBox.SearchText = hall.HallName;
                }

                this.userID.Text = Store.LoginUserInfo.UserName;
                this.cancel.Click += cancel_Click;
                this.Sumbit.Click += new Telerik.Windows.RadRoutedEventHandler(Sumbit_Click);
                this.HallID.SearchButton.Click += new RoutedEventHandler(HallID_Click);
                this.txtIMEI.KeyUp += txtIMEI_KeyUp;
            }
        }

        public Mainaddrepair()
        {
            InitializeComponent();
            this.SizeChanged += Mainaddrepair_SizeChanged;
        }

        void Mainaddrepair_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //WrapPanel wp = this.FindName("searchResult") as WrapPanel;
            //RadGridView rg = this.FindName("GridCheckedIMEI") as RadGridView;
            //GridCheckedPro.Width = wp.ActualWidth - rg.Width;
        }

        void txtIMEI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewCommon.BatchAdd(this.txtIMEI.Text.Trim(), ref uncheckIMEI, ref GridUnCheckIMEI);
                this.txtIMEI.Text = string.Empty;
            }
        }

        void cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
           if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定取消吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
           {
               Clear();
           }
        }

        #region   保存提交

        /// <summary>
        /// 保存提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            if (this.HallID.TextBox.SearchText == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加营业厅");
                return;
            }
            if (checkedModels.Count ==0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"列表无数据");
                return;
            }
            if (string.IsNullOrEmpty(this.orderID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入原始单号！");
                return;
            }
            if (this.orderID.Text.Length != 7)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"原始单号有误！");
                return;
            }

            //添加表头
            API.Pro_RepairInfo rinfo = new API.Pro_RepairInfo();

            var query = from b in Store.ProHallInfo
                        where b.HallID == this.HallID.Tag.ToString()
                        select b;
            if (query == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加正确的仓库");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定提交送修吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            rinfo.OldID = this.orderID.Text;
            rinfo.HallID = query.First().HallID;
            rinfo.UserID = Store.LoginUserInfo.UserID;

            rinfo.RepairDate = DateTime.Now;
            rinfo.Note = this.note.Text;
            rinfo.SysDate = DateTime.Now;

           if (rinfo.Pro_RepairListInfo == null)
            {
                rinfo.Pro_RepairListInfo = new List<API.Pro_RepairListInfo>();
            }
            //添加串码 和串码明细
            API.Pro_RepairListInfo rlist = null;
          
          //  List<string> s = new List<string>();
            foreach (var vm in checkedModels)
            { 
                if (vm.IMEI != null)  
                {
                    //foreach (var i in vm.IMEI)
                    //{
                    //    s.Add(i);
                    //}
                    //串码送修
                    foreach (string im in vm.IMEI)
                    {
                        //添加维修明细
                        rlist = new API.Pro_RepairListInfo();
                        rlist.InListID = vm.Inlist;
                        rlist.ProID = vm.ProID;
                        rlist.ProCount = 1;
                        rlist.IMEI = im;
                        rlist.Note = vm.Note;
                        rinfo.Pro_RepairListInfo.Add(rlist);
                    }  
                    continue;
                }
                else // 无串码送修
                {
                    rlist = new API.Pro_RepairListInfo();
                    rlist.InListID = vm.Inlist;
                    rlist.ProCount = vm.ProCount;
                    rlist.ProID = vm.ProID;
                    rlist.Note = vm.Note;
                    rinfo.Pro_RepairListInfo.Add(rlist);
                }
            }

            PublicRequestHelp help = new PublicRequestHelp(this.busyIndic, 14, new object[] { rinfo }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));

        }

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busyIndic.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "送修失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Error.Message);
                return;
            }
            if (!e.Result.ReturnValue )
            {
                Logger.Log("送修失败！");
                if (e.Result.Message != null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                }
            }
            else
            {
                ///清空数据
                Clear();

                Logger.Log("送修成功！");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
        }

        #endregion 

        private void Clear()
        {
                this.orderID.Text = String.Empty;
                this.txtIMEI.Text = String.Empty;
                //this.HallID.TextBox.SearchText = String.Empty;
                //this.HallID.Tag = null;

                //拣货成功 清空数据
                checkedModels.Clear();
                uncheckIMEI.Clear();
                unIMEIModels.Clear();
                GridCheckedIMEI.ItemsSource = null;
                GridCheckedIMEI.Rebind();
                GridUnCheckPro.Rebind();
                GridCheckedPro.Rebind();
                GridUnCheckIMEI.Rebind();
        }

        #region  "删除操作"

        /// <summary>
        /// 右键删除无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewCommon.DeleteSinglePro(ref unIMEIModels, ref GridUnCheckPro);
        }

        /// <summary>
        /// 右键删除选中的串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delIMEI_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewCommon.DeleteSingleIMEI(ref uncheckIMEI, ref GridUnCheckIMEI);
        }
        /// <summary>
        /// 删除Checked选中的商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedPro_Click(object sender, RoutedEventArgs e)
        {
             ViewCommon.DelCheckedPro(ref unIMEIModels, ref GridUnCheckPro);
        }

        /// <summary>
        /// 删除Checked选中的串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedIMEI_Click(object sender, RoutedEventArgs e)
        {
            ViewCommon.DelCheckedIMEI(ref  uncheckIMEI, ref GridUnCheckIMEI);
        }

        #endregion

        #region "事件"

        /// <summary>
        /// 选择营业厅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HallID_Click(object sender, RoutedEventArgs e)
        {
            hAdder.GetHall(hAdder.FilterHall(20, Store.ProHallInfo));
        }

        /// <summary>
        /// 拣货单击事件
        /// </summary>
        void CheckProduct_Click(object sender, RoutedEventArgs e)
        {
            if (uncheckIMEI.Count() == 0 && unIMEIModels.Count() == 0) { return; }
            pickder.Picking("");
        }

        /// <summary>
        /// 添加无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (this.HallID.Tag == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择仓库");
                return;
            }
            adder.Add();
        }

        /// <summary>
        /// 选中已拣货码商品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridCheckedPro_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            ViewCommon.GridSelectChanged(ref checkedModels, ref GridCheckedPro, ref GridCheckedIMEI);
        }

        /// <summary>
        /// 批量添加商品串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BatchAddIMEI_Click(object sender, RoutedEventArgs e)
        {
            ViewCommon.BatchAdd(this.txtIMEI.Text.Trim(), ref uncheckIMEI, ref GridUnCheckIMEI);
            this.txtIMEI.Text = string.Empty;
        }

        #endregion

        private void GridUnCheckPro_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
           
            foreach (var item in unIMEIModels)
            {
                if (item.ProCount < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"拣货数量不能为负数！");
                    item.ProCount = 0;
                    return;
                }
                if (!item.IsDecimal)
                {
                    item.ProCount = (int)(Decimal.Truncate(Convert.ToDecimal(item.ProCount * 100)) / 100);
                    continue;
                }

                item.ProCount = Decimal.Truncate(Convert.ToDecimal(item.ProCount * 100)) / 100;
            }
        }

     
    }
}
