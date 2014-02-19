using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.Air
{
    /// <summary>
    /// ChangedAir.xaml 的交互逻辑
    /// </summary>

    public partial class ChangedAir : BasePage
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
        /// 仓库添加器
        /// </summary>
        private HallFilter hAdder;

        /// <summary>
        /// 仓库列表
        /// </summary>
        List<API.Pro_HallInfo> hall;


        string r = "";
        private RadGridView GVUnCheckIMEI;
        public ChangedAir()
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
         
            pickder = new NewPicking(ref this.GHHall, ref this.IsBusy, ref checkedModels, ref unIMEIModels,
                            ref uncheckIMEI, ref GridCheckedPro, ref GridUnCheckPro, ref GVUnCheckIMEI);


            this.toDate.SelectedValue = DateTime.Now;
            this.userID.Text = Store.LoginUserInfo.UserName;
            this.cancel.Click += cancel_Click;
            this.Sumbit.Click += new Telerik.Windows.RadRoutedEventHandler(Sumbit_Click);
            this.GHHall.SearchButton.Click += new RoutedEventHandler(GHHall_Click);
            this.SHHall.SearchButton.Click += SearchButton_Click;

        }

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(false, ref this.SHHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hAdder.GetHall(HallInfo);
            if(this.SHHall.Tag!=null)
              SelectPro();
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
                r = "167";
            }
            finally
            {
                GetFirstHall();
            }
        }
        private void GetFirstHall()
        {
            hAdder = new HallFilter(false, ref this.GHHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hall = hAdder.FilterHall(int.Parse(r.Trim()), HallInfo);
            if (hall.Count == 0)
            {
                return;
            }

            this.GHHall.TextBox.SearchText = hall.First().HallName;
            this.GHHall.Tag = hall.First().HallID;
        }

        #region 添加商品
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ProductionFilter adder = new ProductionFilter(ref unIMEIModels, ref GridUnCheckPro);
            if (string.IsNullOrEmpty(this.GHHall.TextBox.SearchText)||this.GHHall.Tag==null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择供货仓库");
                return;
            }
            List<API.Pro_ProInfo> pro = adder.GetPro(int.Parse(r));
            if (pro == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "该角色无此权限，请联系管理员 ");
                return;
            }
            adder.ProFilter(pro.Where(p=>p.AirHallID==this.GHHall.Tag.ToString()).ToList(), false);
        }
        #endregion
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


        #region 拣货

        private void CheckProduct_Click(object sender, RoutedEventArgs e)
        {
            SelectPro();
        }
        private void SelectPro()
        {
            if (uncheckIMEI.Count() == 0 && unIMEIModels.Count() == 0) { return; }
            pickder.Packing();
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


        #endregion

        #region 修改方法接口
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region 提交到后台
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            //添加表头
            List<API.SeleterModel> ProSource = GridCheckedPro.ItemsSource as List<API.SeleterModel>;
            if (ProSource == null || ProSource.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加商品！");
                return;
            }
            List<API.SeleterModel> query = (from b in ProSource
                                            where !string.IsNullOrEmpty(b.NewProID)
                                            select b).ToList();
            if (query.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加新商品！");
                return;
            }
            if (this.GHHall.Tag == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加调出仓库！");
                return;
            }
            if (this.SHHall.Tag == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加接收仓库！");
                return;
            }
            API.Pro_AirOutInfo ListHear = new API.Pro_AirOutInfo() {  FromHallID = this.GHHall.Tag.ToString(), Pro_HallID=this.SHHall.Tag.ToString(), Note = this.tbNote.Text.Trim(), OldID = this.oldID.Text.Trim() };
            ListHear.Pro_AirOutListInfo = new List<API.Pro_AirOutListInfo>();
            foreach (var Proitem in query)
            {
                API.Pro_AirOutListInfo ListInfo = new API.Pro_AirOutListInfo() {  InListID  = Proitem.ProInListID, NewProID = Proitem.NewProID, OldProID = Proitem.ProID, Note = Proitem.NewNote, ProCount = Proitem.Count};

                ListHear.Pro_AirOutListInfo.Add(ListInfo);
            }
            if (PormptPage.PormptMessage("确定新增空冲调拨？", "提示"))
            {
                PublicRequestHelp help = new PublicRequestHelp(this.IsBusy, MethodIDStore.AddAirOut, new object[] { ListHear }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
            }
        }

        private void SubmitCompleted(object sender, API.MainCompletedEventArgs mcea)
        {
            this.IsBusy.IsBusy = false;
            if (mcea.Error == null)
            {
               
                MessageBox.Show(System.Windows.Application.Current.MainWindow, mcea.Result.Message);
                Logger.Log(mcea.Result.Message + "");
                if (mcea.Result.ReturnValue==true)
                {
                    Clear();
                    return;
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

            //this.GHHall.TextBox.SearchText = string.Empty;
            // this.GHHall.Tag = null;

            this.toDate.SelectedValue = DateTime.Now;
            this.oldID.Text = string.Empty;

            //拣货成功 清空数据
            checkedModels.Clear();
            uncheckIMEI.Clear();
            unIMEIModels.Clear();
            GridUnCheckPro.Rebind();
            GridCheckedPro.Rebind();
        }
        #endregion
        #region 添加仓库
        /// <summary>
        /// 选择供货仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GHHall_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(false, ref this.GHHall);
            if (hall == null)
                hall = new List<API.Pro_HallInfo>();
            hAdder.GetHall(hall);
            unIMEIModels.Clear();
            GridUnCheckPro.Rebind();
            checkedModels.Clear();
            GridCheckedPro.Rebind();
        }
        #endregion
        #region 新商品操作
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                if (this.SHHall.Tag == null||string.IsNullOrEmpty(this.SHHall.Text))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加接收仓库！");
                    return;
                }
                List<API.SeleterModel> ProSource = GridCheckedPro.ItemsSource as List<API.SeleterModel>;
                List<API.SeleterModel> SelectSource = new List<API.SeleterModel>();
                foreach (var Item in GridCheckedPro.SelectedItems)
                {
                    SelectSource.Add(Item as API.SeleterModel);
                }
                var query = (from b in ProSource
                             join c in SelectSource on new { b.ProID, b.ProInListID } equals new { c.ProID, c.ProInListID }
                             select b).ToList();

                if (ProSource == null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加商品！");
                    return;
                }                        
                 BaseProFilter adder = new BaseProFilter(ref this.GridCheckedPro, ref query);
                 adder.ProFilter(Store.ProInfo.Where(p=>p.AirHallID==this.SHHall.Tag.ToString()).ToList());
                //PublicRequestHelp help = new PublicRequestHelp(this.IsBusy, MethodIDStore.GetAirPro, new object[] {this.SHHall.Tag.ToString() }, new EventHandler<API.MainCompletedEventArgs>(GetAirProCompleted));             
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "无法获取空冲商品！");
            }
        }
        //private void GetAirProCompleted(object sender, API.MainCompletedEventArgs mcea)
        //{
        //    try
        //    {
        //        List<API.SeleterModel> ProSource = GridCheckedPro.ItemsSource as List<API.SeleterModel>;
        //        List<API.SeleterModel> SelectSource = new List<API.SeleterModel>();
        //        foreach (var Item in GridCheckedPro.SelectedItems)
        //        {
        //            SelectSource.Add(Item as API.SeleterModel);
        //        }
        //        var query = (from b in ProSource
        //                     join c in SelectSource on new { b.ProID, b.ProInListID } equals new { c.ProID, c.ProInListID }
        //                     select b).ToList();

        //        if (ProSource == null)
        //        {
        //            MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加商品！");
        //            return;
        //        }           
        //        if (mcea.Error == null)
        //        {

        //            if (mcea.Result.ReturnValue == true)
        //            {
        //                BaseProFilter adder = new BaseProFilter(ref this.GridCheckedPro, ref query);
        //                adder.ProFilter(mcea.Result.Obj as List<API.Pro_ProInfo>);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show(System.Windows.Application.Current.MainWindow, "加载空中充值商品失败！");
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show(System.Windows.Application.Current.MainWindow, "加载空中充值商品失败！");
        //    }
        //    finally
        //    {
        //        this.IsBusy.IsBusy = false;
        //    }
        //}
        #endregion
        #region
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.SeleterModel> Source = GridCheckedPro.ItemsSource as List<API.SeleterModel>;
            Source.Remove(GridCheckedPro.SelectedItem as API.SeleterModel);
        }
        #endregion

        private void DGCardType_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            API.SeleterModel Item = this.GridUnCheckPro.SelectedItem as API.SeleterModel;

            if (Item.Count < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "商品数量不能为负数");
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
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入正确数值！");
                }
            }
            try
            {
                int value = (int)(Item.Count * 100);
                Item.Count = (decimal)(value * 0.01);

            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入正确的数值！");
                Item.Count = 0;
            }
        }      
    }
}
