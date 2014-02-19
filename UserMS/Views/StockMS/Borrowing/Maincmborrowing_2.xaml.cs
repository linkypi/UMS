using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.Borrowing
{
    public partial class Maincmborrowing_2 : UserControl
    {
        /// <summary>
        /// 已拣货列表
        /// </summary>
        private List<API.BorowListModel> checkedModels = null;

        private List<string> UnImeiListIDs = new List<string>();
        /// <summary>
        /// 无码商品
        /// </summary>
        private List<API.BorowListModel> unIMEIModels = null;

        /// <summary>
        /// 无串商品  临时存储  拣货用
        /// </summary>
        private List<API.BorowListModel> tempModels = null;

        /// <summary>
        /// 串码商品
        /// </summary>
        private List<API.BorowListModel> tempIMEIModels = null;

        private  int Index =0;

        /// <summary>
        /// 审批单中串码商品的数量
        /// </summary>
        private decimal IMEICount = 0; 

        /// <summary>
        /// 未拣货的串码
        /// </summary>
        private List<API.IMEIModel> uncheckIMEI = null;

        private ProAdder<API.BorowListModel> adder;

        private InputAduit input;

        /// <summary>
        /// 串码商品编号列表
        /// </summary>
        private List<string> ProIDList = new List<string>();

        /// <summary>
        /// 拣货成功与否
        /// </summary>
        private bool checkSucess = false;

        public Maincmborrowing_2()
        {
            InitializeComponent();
            this.InputAduitID.Click += InputAduitID_Click;
            //DialogParameters d=new DialogParameters ();
            //d.Closed+=casdf;
            //d.DialogStartupLocation=Telerik.Windows.Controls.WindowStartupLocation.CenterOwner;
            //d.Content="请输入审批单";

            //d.Header="title";
           this.isbusy.IsBusy = true;
            input = new InputAduit();
            input.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            input.Closed += ia_Closed;
            input.Show();
        }

        /// <summary>
        /// 重新输入审批单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InputAduitID_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            input.Show();
        }

        /// <summary>
        /// 审批单通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ia_Closed(object sender, EventArgs e)
        {
            InputAduit ia = sender as InputAduit;
            if (ia.DialogResult == false)
            {
                return;
            }

            this.isbusy.IsBusy = false;

            this.aduitID.Text = ia.am.AduitID;
            var query = from h in Store.ProHallInfo
                        where h.HallID == ia.am.HallID
                        select h;
            this.HallID.Text =query.First().HallName;
            this.HallID.Tag = ia.am.HallID;
            this.borowType.Text = string.IsNullOrEmpty(ia.am.BorrowType) ? "" : ia.am.BorrowType;
            this.borrowdept.Text = string.IsNullOrEmpty(ia.am.Dept) ? "" : ia.am.Dept;
            this.borrower.Text = string.IsNullOrEmpty(ia.am.Borrower) ? "" : ia.am.Borrower;

            uncheckIMEI = new List<API.IMEIModel>();
            unIMEIModels = new List<API.BorowListModel>();
            checkedModels = new List<API.BorowListModel>();

            //绑定数据
            GridUnCheckPro.ItemsSource = unIMEIModels;
            GridCheckedPro.ItemsSource = checkedModels;
            GridUnCheckIMEI.ItemsSource = uncheckIMEI;

            tempModels = new List<API.BorowListModel>();
            tempIMEIModels = new List<API.BorowListModel>();

            GridCheckedPro.Rebind();
            IMEICount = 0;
            List<API.BorowListModel> templist = Clone(ia.models);
            foreach (var item in templist)
            {
                if (!item.NeedIMEI)
                {
                    unIMEIModels.Add(item);
                    tempModels.Add(new API.BorowListModel()
                    {
                        InListID = item.InListID,
                        ProID = item.ProID,
                        NeedIMEI = item.NeedIMEI,
                        ClassName = item.ClassName,
                        ProName = item.ProName,
                        TypeName = item.TypeName,
                        AduitCount = item.ProCount,
                        ProCount =0
                    });
                }
                else
                {   
                    unIMEIModels.Add(item);
                    IMEICount += item.ProCount;
                    ProIDList.Add(item.ProID);
                    item.IIMEIList = new List<API.IMEIModel>();
                    tempIMEIModels.Add(item);
                }
            }

            foreach (var item in unIMEIModels)
            {
                item.AduitCount = 0;
            }

            GridUnCheckPro.Rebind();

            //绑定事件
            GridCheckedPro.SelectionChanged += GridCheckedPro_SelectionChanged;
            this.addNoIMEIPros.Click += Add_Click;
            this.checkPro.Click += CheckProduct_Click;   //拣货

            this.delIMEI.Click += delCheckedIMEI_Click; //删除选中的串码
            this.delPro.Click += delCheckedPro_Click;//删除选中的商品

            //pickder = new PickingDevicer(ref this.HallID, ref checkedModels, ref unIMEIModels,
            //                ref uncheckIMEI, ref GridCheckedPro, ref GridUnCheckPro, ref GridUnCheckIMEI, ref this.isbusy);
            adder = new ProAdder<API.BorowListModel>(ref unIMEIModels,ref  GridUnCheckPro, 15, 2);

            this.InUserID.Text = Store.LoginUserInfo.UserName;
            this.Sumbit.Click += new Telerik.Windows.RadRoutedEventHandler(Sumbit_Click);
  
        }

        /// <summary>
        /// 克隆一个集合
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private  List<API.BorowListModel> Clone( List<API.BorowListModel> models)
        {
            List<API.BorowListModel> list = new List<API.BorowListModel>();
            API.BorowListModel bl = null;

            foreach (var item in models)
            {
                bl = new API.BorowListModel()
                {
                    InListID = item.InListID,
                    ProID = item.ProID,
                    NeedIMEI = item.NeedIMEI,
                    ClassName = item.ClassName,
                    ProName = item.ProName,
                    TypeName = item.TypeName,
                    AduitCount = item.ProCount,
                    ProCount = item.ProCount
                };

                list.Add(bl);
            }

            return list;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Clear();
        }

        #region  "删除操作"

        /// <summary>
        /// 删除Checked选中的商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedPro_Click(object sender, RoutedEventArgs e)
        {
           if (GridUnCheckPro.SelectedItems.Count == 0)
           {
               MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要上删除的商品");
               return;
           }

           if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除选中的商品吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
           {
               return;
           }
           if (GridUnCheckPro.SelectedItems.Count == unIMEIModels.Count)
           {
               unIMEIModels.RemoveAll(viewModel => true);
               GridUnCheckPro.Rebind();
               return;
           }
           API.BorowListModel model = null;
           foreach (var item in GridUnCheckPro.SelectedItems)
           {
               model = item as API.BorowListModel;
               foreach (var vm in unIMEIModels)
               {
                   if (model.ProID == vm.ProID)
                   {
                       unIMEIModels.Remove(vm);
                       break;
                   }
               }
           }
           GridUnCheckPro.Rebind();
        }

        /// <summary>
        /// 删除Checked选中的串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedIMEI_Click(object sender, RoutedEventArgs e)
        {
            if (GridUnCheckIMEI.SelectedItems.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要上删除的串码");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除选中的串码吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            if (GridUnCheckIMEI.SelectedItems.Count == uncheckIMEI.Count)
            {
                uncheckIMEI.RemoveAll(viewModel => true);
                GridUnCheckIMEI.Rebind();
                return;
            }
            API.IMEIModel model = null;
            foreach (var item in GridUnCheckIMEI.SelectedItems)
            {
                model = item as API.IMEIModel;
                foreach (var vm in uncheckIMEI)
                {
                    if (vm.OldIMEI == model.OldIMEI)
                    {
                        uncheckIMEI.Remove(vm);
                        break;
                    }
                }
            }
            GridUnCheckIMEI.Rebind();
        }

        #endregion

        #region "拣货"

        /// <summary>
        /// 拣货单击事件
        /// </summary>
        void CheckProduct_Click(object sender, RoutedEventArgs e)
        {
            #region  清空备注及已拣货数据

            foreach (var item in unIMEIModels)
            {
                item.Note = string.Empty;
                if (item.NeedIMEI)
                {
                    item.AduitCount = 0;
                }
            }
            foreach (var item in uncheckIMEI)
            {
                item.Note = string.Empty;
            }
            checkedModels.Clear();
            GridCheckedPro.Rebind();

            GridCheckedIMEI.ItemsSource = null;
            GridCheckedIMEI.Rebind();
            GridUnCheckPro.Rebind();
            GridUnCheckIMEI.Rebind();
            #endregion

            checkSucess = false;
            bool flag = false;
            List<API.BorowListModel> unimeiModel = new List<API.BorowListModel>();
            foreach (var item in unIMEIModels)
            {
                if (!item.NeedIMEI)
                {
                    unimeiModel.Add(item);
                }
            }
            if (unimeiModel.Count == 0 && uncheckIMEI.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入待拣货的商品数量或串码");
                return;
            }
            #region 非串码商品拣货
            foreach (var item in unimeiModel)
            {
                if (item.AduitCount == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"借贷数量不能为0");
                    return;
                }
                foreach (var child in tempModels)
                {
                    if (item.ProID == child.ProID && item.AduitCount > item.ProCount)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品 " + item.ProName + " 的数量已超出审批数量");
                        item.Note = "数量过多";
                        return;
                    }
                    if (item.ProID == child.ProID && item.AduitCount <= item.ProCount)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"无串码商品 " + item.ProName + "未审批");
                    return;
                }
            }
          
            #endregion 

            #region  串码拣货

            if (IMEICount < uncheckIMEI.Count)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码数量已大于审批数量");
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 7, new object[] { unimeiModel, uncheckIMEI, ProIDList, this.HallID.Tag.ToString() }, new EventHandler<API.MainCompletedEventArgs>(CheckCompleted));
           
            #endregion 

        }

        /// <summary>
        /// 拣货完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckCompleted(object sender, API.MainCompletedEventArgs e)
        {
            Index++;
            this.isbusy.IsBusy = false;
            Logger.Log("拣货完成");
      
            List<API.IMEIModel> list = e.Result.Obj as List<API.IMEIModel>;

            //清空已经拣货的数据
            if (Index == 1 && (!e.Result.ReturnValue))
            {
                //第一次拣货失败不清空数据
            }
            else
            {
                checkedModels.Clear();
            }
            #region 串码拣货完成

            uncheckIMEI.Clear();
            uncheckIMEI.AddRange(list);
            GridUnCheckIMEI.Rebind();

            if (e.Result.ReturnValue)
            {
                checkSucess = true;
                foreach (var item in list)
                {
                    foreach (var child in tempIMEIModels)
                    {
                        if (item.ProID == child.ProID)
                        {
                            if (child.IIMEIList.Count == 0)
                            {
                                AddChildToIMEIModel(item, child);
                                break;
                            }
                        }
                    }
                 }
            }
            if (e.Result.ReturnValue)
            {
                foreach (var child in checkedModels)
                {
                    foreach (var item in unIMEIModels)
                    {
                        if (child.ProID == item.ProID)
                        {
                            item.AduitCount = child.IIMEIList.Count;
                            item.Note = "成功";
                            item.Sucess = true;
                        }
                    }
                }
                GridCheckedPro.Rebind();
            }
            //else
            //{
            //    foreach (var item in unIMEIModels)
            //    {
            //        foreach (var child in list)
            //        {
            //            if (child.ProID == item.ProID)
            //            {
                            
            //            }
            //        }
            //    }
            //}
            #endregion

            #region 无串码拣货完成

            List<API.BorowListModel> unlist = e.Result.ArrList[0] as List<API.BorowListModel>;

            foreach (var child in unlist)
            {
                foreach (var item in unIMEIModels)
                {
                    if (child.ProID == item.ProID)
                    {
                        item.Note = child.Note;
                        item.Sucess = true;
                    }
                }
            }

            if (e.Result.ReturnValue)
            {
                List<API.BorowListModel> sucessList = e.Result.ArrList[1] as List<API.BorowListModel>;
                foreach (var item in sucessList)
                {
                    checkedModels.Add(item);
                }
                GridCheckedPro.Rebind();
            }
            foreach (var item in tempModels)
            {
                foreach (var child in checkedModels)
                {
                    if (item.ProID == child.ProID)
                    {
                        child.AduitCount = item.AduitCount;
                    }
                }
            }
            this.GridUnCheckPro.Rebind();

            #endregion

            //foreach (var child in checkedModels)
            //{
            //    foreach (var item in unIMEIModels)
            //    {
            //        if (child.ProID == item.ProID)
            //        {
            //            if(item.NeedIMEI)
            //            {
            //                item.AduitCount = child.IIMEIList.Count;
            //            }
            //            item.Note = "成功";
            //            item.Sucess = true;
            //        }

            //    }
            //}
            //foreach (var item in unIMEIModels)
            //{
            //    if (!item.Sucess)
            //    {
            //        item.Note = "失败";
            //    }
            //}
            GridCheckedPro.Rebind();

            if (!e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"拣货失败");
                Logger.Log("拣货失败");
            }
        }

        private void AddChildToIMEIModel(API.IMEIModel item, API.BorowListModel child)
        {
            API.BorowListModel bl = new API.BorowListModel();
            bl.IIMEIList = new List<API.IMEIModel>();

            foreach (var item2 in checkedModels)
            {
                if ((!string.IsNullOrEmpty(item2.InListID))&&item.InListID == item2.InListID&&item.ProID==item2.ProID)
                {
                    API.IMEIModel im = new API.IMEIModel();
                    im.OldIMEI = item.OldIMEI;
                    child.InListID = item.InListID;
                    item2.IIMEIList.Add(im);
                    item2.ProCount++;
                    item2.AduitCount = child.ProCount;
                    return;
                }
            }
         
            bl.InListID = item.InListID;
            bl.ProCount++;
            bl.AduitCount = child.ProCount;
            bl.ProID = child.ProID;
            bl.ProName = child.ProName;
            bl.TypeName = child.TypeName;
            bl.ClassName = child.ClassName;
            bl.ProFormat = child.ProFormat;

            API.IMEIModel imei = new API.IMEIModel();
            imei.OldIMEI = item.OldIMEI;
            child.InListID = item.InListID;
            bl.IIMEIList.Add(imei);

            checkedModels.Add(bl);
        }

       #endregion 

        #region  事件

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
            if (checkedModels != null && GridCheckedPro.SelectedItem != null)
            {
                API.BorowListModel pinfo = GridCheckedPro.SelectedItem as API.BorowListModel;

                foreach (var item in checkedModels)
                {
                    if (item.ProID == pinfo.ProID && item.InListID == pinfo.InListID && pinfo.InListID != "")
                    {
                        GridCheckedIMEI.ItemsSource = item.IIMEIList;
                        break;
                    }
                }
                GridCheckedIMEI.Rebind();
            }

        }

        void txtIMEI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BatchAdd();
                this.txtIMEI.Text = string.Empty;
            }
        }

        /// <summary>
        /// 批量添加商品串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BatchAddIMEI_Click(object sender, RoutedEventArgs e)
        {
            BatchAdd();
        }

        private void BatchAdd()
        {
            List<string> list = new List<string>(this.txtIMEI.Text.Trim().Split("\r\n".ToCharArray()));

            if (list.Count + uncheckIMEI.Count > IMEICount)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码数量已超出审批数量");
                return;
            }

            string IMEI = this.txtIMEI.Text.Trim();
            if (String.IsNullOrEmpty(IMEI))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码不能为空");
                return;
            }

            API.IMEIModel cm = null;
            foreach (string s in list)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (!ValidateIMEI(s, uncheckIMEI))  //去除重复项
                    {
                        cm = new API.IMEIModel();
                        cm.OldIMEI = s;
                        uncheckIMEI.Add(cm);
                    }
                }
            }
            GridUnCheckIMEI.Rebind();
            this.txtIMEI.Text = "";
        }

          bool ValidateIMEI(string imei, List<API.IMEIModel> uncheckIMEI)
        {
            if (uncheckIMEI.Count == 0)
            {
                return false;
            }
            foreach (var cm in uncheckIMEI)
            {
                if (cm.OldIMEI == null)
                {
                    continue;
                }
                if (cm.OldIMEI.Equals(imei))
                {
                    return true;
                }
            }
            return false;

        }

        #endregion

        #region "提交"

        /// <summary>
        /// 保存提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            if (!checkSucess)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"拣货未通过");
                return;
            }
            //添加表头
            API.Pro_BorowInfo order = new API.Pro_BorowInfo();
            //原始单号
            order.OldID = this.oldID.Text.Trim();
            order.HallID = this.HallID.Tag.ToString();
            order.AduitID = this.aduitID.Text.Trim();
            order.IsReturn = false;
            order.IsDelete = false;
            order.Dept = borrowdept.Text;
            
            //添加操作人ID
            order.UserID = Store.LoginUserInfo.UserID;

            order.BorrowType = this.borowType.Text;
           order.Borrower = this.borrower.Text.Trim();
           order.BorowDate = DateTime.Now;
       
            //系统时间
            order.SysDate = DateTime.Now;
            if (checkedModels == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加商品");
                return;
            }

            API.Pro_BorowOrderIMEI imei = null;
            foreach (var vm in checkedModels)
            {
                API.Pro_BorowListInfo OrderList = new API.Pro_BorowListInfo();

                //添加退库明细
                OrderList.ProID = vm.ProID;
                OrderList.ProCount = vm.ProCount;
                OrderList.InListID = vm.InListID;
                //添加串码 和串码明细
                if (vm.IIMEIList != null)
                {
                    foreach (var i in vm.IIMEIList)
                    {
                        imei = new API.Pro_BorowOrderIMEI();
                        imei.IMEI = i.OldIMEI;
                
                        if (OrderList.Pro_BorowOrderIMEI== null)
                        {
                            OrderList.Pro_BorowOrderIMEI = new List<API.Pro_BorowOrderIMEI>();
                        }
                        OrderList.Pro_BorowOrderIMEI.Add(imei);
                    }
                }
                if (order.Pro_BorowListInfo == null)
                {
                    order.Pro_BorowListInfo = new List<API.Pro_BorowListInfo>();
                }
                order.Pro_BorowListInfo.Add(OrderList);
            }
            PublicRequestHelp help = new PublicRequestHelp(this.isbusy, 6, new object[] { order }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            try
            {
                if (!e.Result.ReturnValue)
                {
                    Logger.Log("借贷出错！");
                    if (e.Result.Message != null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                    }
                }
                else
                {
                    ///清空数据
                    Clear();
                 
                    Logger.Log("借贷成功！");
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Clear()
        {
            this.aduitID.Text = string.Empty;
            this.borrowID.Text = String.Empty;
            this.oldID.Text = String.Empty;
            this.HallID.Text = String.Empty;
            this.HallID.Tag = null;
            this.borrower.Text = String.Empty;
            this.borrowdept.SelectedText = string.Empty;
            this.borowType.Text = string.Empty;
            this.txtIMEI.Text = String.Empty;
            borrowdept.Text = string.Empty;
            this.InUserID.Text = string.Empty;

            checkedModels.Clear();
            uncheckIMEI.Clear();
            unIMEIModels.Clear();
            GridCheckedIMEI.ItemsSource = null;
            GridUnCheckPro.Rebind();
            GridCheckedPro.Rebind();
            GridCheckedIMEI.Rebind();
            GridUnCheckIMEI.Rebind();
           // GridDetail.ItemsSource = null;
            //GridDetail.Rebind();
        }

        #endregion  

    }
}
