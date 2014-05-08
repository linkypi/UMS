using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.Common;
using UserMS.Report.Print;
using UserMS.Report.Print.RepairPrint;

namespace UserMS.Views.StockMS.Allot
{
    public partial class Main_allot : BasePage
    {
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

        bool forRepairOut = false;

        List<int> repIDs = new List<int>();

        //缺料串码数量
        int LackCount = 0; 

        string r = "";
        public Main_allot()
        {
            Init();
        }

        public Main_allot(List<int> repModels,string hallid)
        {
            Init();
            repIDs = repModels;
            forRepairOut = true;
           // oldID.Tag = model.ID; //绑定售后维修单号ID
            var HallInfo = Store.ProHallInfo.Where(h => h.HallID == hallid);
            if (HallInfo.Count() == 0) { return; }
            this.SHHall.TextBox.SearchText = HallInfo.First().HallName;
            this.SHHall.Tag = HallInfo.First().HallID;
            LackCount = 0;
            PublicRequestHelp p = new PublicRequestHelp(this.IsBusy, 375, new object[] {repModels }, GetProsCompleted);

        }

        /// <summary>
        /// 获取缺料商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetProsCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                List<API.View_ASPCurrentOrderPros> list = e.Result.Obj as List<API.View_ASPCurrentOrderPros>;
                var pros = list.Where(a=>a.NeedIMEI==false);
                LackCount = list.Where(a => a.NeedIMEI).Count();
                foreach (var item in pros)
	            {
	            	 API.SeleterModel sm =ValUnImeiModel(item.ProID);
                     if (sm == null)
                     {
                         sm = new API.SeleterModel();
                         sm.ISdecimals = item.ISdecimals;
                         sm.ProID = item.ProID;
                         sm.ClassID = Convert.ToInt32(item.ClassID);
                         sm.ProClassName = item.ClassName;
                         sm.TypeID = Convert.ToInt32(item.TypeID);
                         sm.ProTypeName = item.TypeName;
                         sm.ProName = item.ProName;
                         sm.ProFormat = item.ProFormat;
                         sm.IsNeedIMEI = false;
                         sm.Count = Convert.ToDecimal(item.ProCount);
                         unIMEIModels.Add(sm);
                     }
                     else
                     {
                         sm.Count += Convert.ToDecimal(item.ProCount);
                     }
	            }
                GridUnCheckPro.Rebind();
            }
            else
            {
                MessageBox.Show("获取维修单缺料配件失败，请联系技术人员！");
            }
        }

        private API.SeleterModel ValUnImeiModel(string proid)
        {
            foreach (var item in  unIMEIModels)
            {
                if (item.ProID == proid)
                {
                    return item;
                }
            }
            return null;
        }

        private void Init()
        {
            InitializeComponent();
            this.GHHall.TextBox.IsEnabled = false;
            this.SHHall.TextBox.IsEnabled = false;
            uncheckIMEI = new List<API.SelecterIMEI>();
            unIMEIModels = new List<API.SeleterModel>();
            checkedModels = new List<API.SeleterModel>();
            //绑定数据
            GridUnCheckPro.ItemsSource = unIMEIModels;
            GridCheckedPro.ItemsSource = checkedModels;
            GridUnCheckIMEI.ItemsSource = uncheckIMEI;

            //添加搜索资源
            GHHall.ItemsSource = Store.ProHallInfo;
            GHHall.DisplayMemberPath = "HallName";

            SHHall.ItemsSource = Store.ProHallInfo;
            SHHall.DisplayMemberPath = "HallName";

            GridCheckedPro.SelectionChanged += GridCheckedPro_SelectionChanged;
            this.addNoIMEIPros.Click += Add_Click;
            this.BatchAddIMEI.Click += BatchAddIMEI_Click; //批量添加
            this.checkPro.Click += CheckProduct_Click;   //拣货

            this.delIMEI.Click += delCheckedIMEI_Click; //删除选中的串码
            this.delPro.Click += delCheckedPro_Click;//删除选中的商品

            pickder = new NewPicking(ref this.GHHall, ref this.IsBusy, ref checkedModels, ref unIMEIModels,
                            ref uncheckIMEI, ref GridCheckedPro, ref GridUnCheckPro, ref GridUnCheckIMEI);
            adder = new ProductionFilter(ref unIMEIModels, ref GridUnCheckPro);

            this.toDate.SelectedValue = DateTime.Now;
            this.userID.Text = Store.LoginUserInfo.UserName;
            this.cancel.Click += cancel_Click;
            this.Sumbit.Click += new Telerik.Windows.RadRoutedEventHandler(Sumbit_Click);
            this.GHHall.SearchButton.Click += new RoutedEventHandler(GHHall_Click);
            this.SHHall.SearchButton.Click += new RoutedEventHandler(SHHall_Click);
            this.txtIMEI.KeyUp += txtIMEI_KeyUp;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];

            }
            catch
            {
                r = "11";
            }
            finally
            {
                GetFirstHall();
            }
        }

        private void GetFirstHall()
        {
            hAdder = new HallFilter(false,ref this.GHHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hall = hAdder.FilterHall(int.Parse(r.Trim()), HallInfo);
            if (hall == null)
            {
                return;
            }
            if (hall.Count == 0) { return; }
            this.GHHall.TextBox.SearchText = hall.First().HallName;
            this.GHHall.Tag = hall.First().HallID;
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

        /// <summary>
        /// 添加无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (this.GHHall.TextBox.SearchText.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择供货仓库");
                return;
            }
            List<API.Pro_ProInfo> pro = adder.GetPro(int.Parse(r));
            if(pro==null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该角色无此权限，请联系管理员 ");
                return;
            }
            adder.ProFilter(pro, false);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (PormptPage.PormptMessage("是否清空数据？", "提示"))
            Clear();
        }

        /// <summary>
        /// 批量添加商品串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BatchAddIMEI_Click(object sender, RoutedEventArgs e)
        {
            ViewOperate.BatchAdd(this.txtIMEI.Text.Trim(), ref uncheckIMEI, ref GridUnCheckIMEI);
            txtIMEI.Text = "";
        }

        /// <summary>
        /// 拣货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckProduct_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (uncheckIMEI.Count() == 0 && unIMEIModels.Count() == 0) { return; }
            if (string.IsNullOrEmpty(GHHall.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入仓库再捡货！");
                return;
            }
            pickder.Packing();
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
            //ViewCommon.DeleteSingleIMEI(ref uncheckIMEI, ref GridUnCheckIMEI);
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

        #region 提交
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            #region 
            //添加表头
            API.Pro_OutInfo head = new API.Pro_OutInfo();
            
            head.OldID = this.oldID.Text.Trim();
            //调货仓库
            if (this.GHHall.TextBox.SearchText.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择供货仓库");
                return;
            }
 
            head.FromHallID = this.GHHall.Tag.ToString();
            //收货仓库
            if (this.SHHall.TextBox.SearchText.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择收货仓库");
                return;
            }
            if (this.GHHall.Tag == this.SHHall.Tag)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"不能为同一仓库调拨");
                return;
            }
            head.Pro_HallID = this.SHHall.Tag.ToString();
            head.Note = this.tbNote.Text.Trim();
            //调货人
            head.FromUserID = Store.LoginUserInfo.UserID;
           
            if (checkedModels == null||checkedModels.Count()==0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加商品");
                return;
            }

            head.SysDate = DateTime.Now;

            if (String.IsNullOrEmpty(toDate.SelectedValue.ToString()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写时间");
                return;
            }
            head.OutDate =toDate.SelectedValue;
            #endregion 

            List<string> imeiList = new List<string>();
            foreach (var vm in checkedModels)
            {
                API.Pro_OutOrderList List = new API.Pro_OutOrderList();
                if (vm.Count == 0 )
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"无串码商品请添加数量");
                    return;
                }
                List.ProCount = vm.Count;
                List.InListID = vm.ProInListID;
                List.ProID = vm.ProID;
                List.Note = vm.Note;
                //添加串码明细
                API.Pro_OutOrderIMEI imei = null;

                if (vm.IsIMEI != null)
                {
                    foreach (var i in vm.IsIMEI)
                    {
                        imei = new API.Pro_OutOrderIMEI();
                        imei.IMEI = i.IMEI;

                        imeiList.Add(i.IMEI);//添加串码
                        if (List.Pro_OutOrderIMEI == null)
                        {
                            List.Pro_OutOrderIMEI = new List<API.Pro_OutOrderIMEI>();
                        }
                        List.Pro_OutOrderIMEI.Add(imei);
                    }
                }
               
                if (head.Pro_OutOrderList == null)
                {
                    head.Pro_OutOrderList = new List<API.Pro_OutOrderList>();
                }
                head.Pro_OutOrderList.Add(List);
            }
            API.Sys_UserInfo user = Store.LoginUserInfo;

            if (forRepairOut)
            {
                if (imeiList.Count != LackCount)
                {
                    MessageBox.Show("实际调拨串码数量与缺料串码数量不匹配！");
                    return;
                }
            }

            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定执行调拨操作吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                PublicRequestHelp help = new PublicRequestHelp(this.IsBusy, 4, new object[] { head, imeiList, forRepairOut, repIDs },
                    SubmitCompleted);
            }
          
        }

        private void SubmitCompleted(object sender, API.MainCompletedEventArgs mcea)
        {
            this.IsBusy.IsBusy = false;
            if (mcea.Error == null)
            {

                //MessageBox.Show(System.Windows.Application.Current.MainWindow, mcea.Result.Message);
                Logger.Log(mcea.Result.Message + "");
                if (mcea.Result.ReturnValue == true)
                {
                    Clear();
                    repIDs.Clear();
                    forRepairOut = false;
                    LackCount = 0;

                    if (MessageBox.Show("保存成功，是否打印？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    PrintOutOrder2 print = new PrintOutOrder2(mcea.Result.Obj as List<API.Report_OutOrderListInfoWithIMEI>);

                    print.SrcPage = "/Views/StockMS/Allot/Main_allot.xaml?MenuID=11";
                    this.NavigationService.Navigate(print);
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, mcea.Result.Message);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器异常！");
            }
        }

        private void Clear()
        {
            this.tbNote.Text = string.Empty;
            GetFirstHall();
            this.SHHall.TextBox.SearchText = string.Empty;
          //  this.GHHall.TextBox.SearchText = string.Empty;
           // this.GHHall.Tag = null;
            this.toDate.SelectedValue = DateTime.Now;
            this.oldID.Text = string.Empty;
            this.txtIMEI.Text = string.Empty;
            //拣货成功 清空数据
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

        #region 添加仓库
        /// <summary>
        /// 选择收货仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SHHall_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(false,ref this.SHHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hAdder.GetHall(HallInfo);
        }

        /// <summary>
        /// 选择供货仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GHHall_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(false,ref this.GHHall);
            if (hall == null)
                hall = new List<API.Pro_HallInfo>();
            hAdder.GetHall(hall);
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
