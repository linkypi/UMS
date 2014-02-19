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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using UserMS.Common;
using UserMS.Model.PackageModel;

namespace UserMS.Views.PackageOff
{
    /// <summary>
    /// PackageOffMS.xaml 的交互逻辑
    /// </summary>
    public partial class PackageOffMS : Page
    {
        private Mul_HallFilter hAdder;
        private List<API.SelecterIMEI> uncheckIMEI;
        private List<GounpSource> GounpList = new List<GounpSource>();
        private List<API.Package_GroupInfo> GrounpNameList=new List<API.Package_GroupInfo>();
        List<API.Package_SalesNameInfo> SalesNameInfo;
        public PackageOffMS()
        {
            InitializeComponent();
            Initial();
            CreatName.Text = Store.LoginUserName;
            hAdder = new Mul_HallFilter(ref this.HallGrid);
            uncheckIMEI = new List<API.SelecterIMEI>();
            this.PageGrid.ItemsSource = GounpList;
        }
        void Initial()
        {
            CBLimit.IsChecked = false;
            LimitNum.IsReadOnly = true;
            LimitNum.Text = "99999";
        }
        #region 添加门店
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            hAdder.GetHall(Store.ProHallInfo);
        }
        #endregion
        #region 删除门店
        private void DelHall_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelHall(ref HallGrid);
        }
        #endregion       
        #region 添加会员卡类别
        private void AddVIPType_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MultSelecter msFrm = new MultSelecter(
             null,
             Store.VIPType,
            "ID", "Name",
             new string[] { "ID", "Name" },
             new string[] { "ID", "会员类别" },
             true
            );
            msFrm.Closed += VIPTypeWin_Closed;
            msFrm.ShowDialog();

        }
        private void VIPTypeWin_Closed(object sender, EventArgs e)
        {
            UserMS.MultSelecter selecter = sender as UserMS.MultSelecter;
            if (selecter.DialogResult == true)
            {
                List<UserMS.API.VIP_VIPType> phList = selecter.SelectedItems.OfType<API.VIP_VIPType>().ToList();
                if (this.dataGridVipType.ItemsSource == null)
                    this.dataGridVipType.ItemsSource = phList;
                else
                {
                    List<UserMS.API.VIP_VIPType> TypeList = this.dataGridVipType.ItemsSource as List<UserMS.API.VIP_VIPType>;
                    
                    var query =(from b in TypeList
                                join c in phList on b.ID equals c.ID
                                select c).ToList();
                    foreach(var Item in query)
                        phList.Remove(Item);
                    TypeList.AddRange(phList);
                    this.dataGridVipType.Rebind();
                }
            }
        }
        #endregion
        #region 删除会员卡类型
        private void DeleteVIPType_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelVIPType(ref dataGridVipType);
        }
        #endregion
        
        #region 销售分组操作
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            GounpSource GounpItem = new GounpSource();
            //GounpItem.SellTypeList = Store.Options.Where(p => p.ClassName == "SalesPromotion").ToList();
            GounpItem.SellTypeName = Store.Options.Where(p => p.ClassName == "SalesPromotion").First().Name;
            GounpItem.SellTypeID = Store.Options.Where(p => p.ClassName == "SalesPromotion").First().ID;
            GounpItem.GrounpNameList = new List<API.Package_GroupInfo>() 
            { 
              new  API.Package_GroupInfo(){ GroupName="终端"},
              new  API.Package_GroupInfo(){ GroupName="号码卡"},
              new  API.Package_GroupInfo(){ GroupName="会员"},
              new  API.Package_GroupInfo(){ GroupName="延保"},
              new  API.Package_GroupInfo(){ GroupName="配件"},
              new  API.Package_GroupInfo(){ GroupName="礼品"}
            };
         
            this.GounpList.Add(GounpItem);         
            this.PageGrid.Rebind();          
        }
        private void RadMenuItem_Click_3(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            foreach (var Item in PageGrid.SelectedItems)
            {
                GounpList.Remove(Item as GounpSource);
            }
            this.PageGrid.Rebind();  
        }
        #endregion
        #region 商品操作
        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {      
            SingleSelecter2 msFrm = new SingleSelecter2(
             null,
             Store.ProNameInfo,
            "MainName",
             new string[] { "NameID", "MainName" },
             new string[] { "ID", "商品型号" }
            );
            msFrm.Closed += ProWin_Closed;
            msFrm.ShowDialog();
        }
        private void ProWin_Closed(object sender, EventArgs e)
        {
            UserMS.SingleSelecter2 selecter = sender as UserMS.SingleSelecter2;
            if (selecter.DialogResult == true)
            {
                UserMS.API.Pro_ProNameInfo phList = selecter.SelectedItem as API.Pro_ProNameInfo;
                if (phList == null)
                {                    
                    return;
                }
                GounpSource Gounp = PageGrid.SelectedItem as GounpSource;
                if (PageGrid.SelectedItems.Count != 1 || Gounp == null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择一个分组");
                    return;
                }
                if (Gounp.ProModel == null)
                    Gounp.ProModel = new List<API.ProModel>();
                var query = from b in Gounp.ProModel
                            where b.ProInListID.Contains(phList.NameID)
                            select b;
                if (query.Count() > 0) return;
                API.ProModel model = new API.ProModel();
                model.ProInListID = phList.NameID;
                model.ProName = phList.MainName;
                model.SellTypeName = Gounp.SellTypeName;       
                Gounp.ProModel.Add(model);
                ProGridOther.ItemsSource = Gounp.ProModel;
                ProGridOther.Rebind();
            }
        }
        private void DelPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }
        #endregion
        #region 属性改变
        //private void GridViewComboBoxColumn_PropertyChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    RadComboBox comboBox = (RadComboBox)e.OriginalSource;
        //    if (comboBox.SelectedValue == null)
        //    {
        //        return;
        //    }
        //    ((GounpSource)comboBox.DataContext).GroupName = comboBox.Text.Trim();
        //}
        #endregion     
        
        #region GridView操作
        private void ProGridOther_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            API.ProModel ProItem = this.ProGridOther.SelectedItem as API.ProModel;
            if (e.Cell.Column.Header.ToString() == "提成")
            {
                 if (ProItem.Salary <0)
                {
                    ProItem.Salary = 0;
                }
                int value = (int)(ProItem.Salary * 100);
                ProItem.Salary = (decimal)(value * 0.01);
               
            }
        }
  
        #endregion
        #region 选择人数
        private void CBLimit_Click_1(object sender, RoutedEventArgs e)
        {
            if (CBLimit.IsChecked == true)
            {
                LimitNum.IsReadOnly = false;
                LimitNum.Text = string.Empty;
            }
            else
            {
                LimitNum.IsReadOnly = true;
                LimitNum.Text = "99999";
            }
        }
        #endregion 
        #region 保存
        private void RadMenuItem_Click_4(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            API.VIP_OffList Head = new API.VIP_OffList();
            #region 检查数据有效性
            if (String.IsNullOrEmpty(this.Name.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入套餐名称");
                return;
            }
            if (StartTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入开始日期");
                return;
            }
            if (EndTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入结束日期");
                return;
            }
            if (StartTime.SelectedValue > EndTime.SelectedValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"开始日期不能大于结束日期！");
                return;
            }
            if (EndTime.SelectedValue <= DateTime.Now)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"活动结束日期必须大于当前日期");
                return;
            }
            if (StartTime.SelectedValue <= DateTime.Now)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"活动开始日期必须大于当前日期");
                return;
            }
            if (string.IsNullOrEmpty(LimitNum.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入限制人数或设为默认");
                return;
            }
            try
            {
                Head.VIPTicketMaxCount = int.Parse(this.LimitNum.Text.Trim());
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"人数个数必须为整数");
                return;
            }

           
            if (HallGrid.ItemsSource == null || HallGrid.Items.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加营业厅");
                return;
            }
            #endregion


            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定增加优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            #region 添加表头
            Head.Name = this.Name.Text.Trim();
            Head.StartDate = this.StartTime.SelectedValue;
            Head.EndDate = this.EndTime.SelectedValue;
            Head.ArriveMoney =(decimal)ReduceCash.Value;
            Head.Note = Note.Text.Trim();
            #endregion

            #region 会员类型明细
            if (dataGridVipType.ItemsSource != null && dataGridVipType.Items.Count > 0)
            {
                List<API.VIP_VIPType> VIPTypeSource = dataGridVipType.ItemsSource as List<API.VIP_VIPType>;
                foreach (var TypeItem in VIPTypeSource)
                {
                    API.VIP_VIPTypeOffLIst VipType = new API.VIP_VIPTypeOffLIst();
                    VipType.VIPType = TypeItem.ID;
                    if (Head.VIP_VIPTypeOffLIst == null)
                        Head.VIP_VIPTypeOffLIst = new List<API.VIP_VIPTypeOffLIst>();
                    Head.VIP_VIPTypeOffLIst.Add(VipType);
                }
            }
            #endregion

            #region 会员明细
            if (dataGridVip2.ItemsSource != null && dataGridVip2.Items.Count > 0)
            {
                List<API.View_VIPInfo> VIPSource = dataGridVip2.ItemsSource as List<API.View_VIPInfo>;
                foreach (var VIPItem in VIPSource)
                {
                    API.VIP_VIPOffLIst VIP = new API.VIP_VIPOffLIst();
                    VIP.VIPID = VIPItem.ID;
                    if (Head.VIP_VIPOffLIst == null)
                        Head.VIP_VIPOffLIst = new List<API.VIP_VIPOffLIst>();
                    Head.VIP_VIPOffLIst.Add(VIP);
                }
            }
            #endregion

            #region 添加分组
            Head.Package_GroupInfo = new List<API.Package_GroupInfo>();
            foreach (var GrounpItem in GounpList)
            {
                API.Package_GroupInfo Grounp = new API.Package_GroupInfo();
                Grounp.GroupName = GrounpItem.GroupName;
                if (string.IsNullOrEmpty(Grounp.GroupName))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "分组未添加名称！");
                    return;
                }
                Grounp.IsMust = GrounpItem.IsmustText == "是" ? true : false;
                Grounp.SellType = GrounpItem.SellTypeID;
                Grounp.Note = GrounpItem.Note;
                
                if (GrounpItem.ProModel == null || GrounpItem.ProModel.Count() == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "分组未添加商品！");
                    return;
                }
                Grounp.Package_ProInfo = new List<API.Package_ProInfo>();
                foreach (var Item in GrounpItem.ProModel)
                {
                    API.Package_ProInfo ProItem = new API.Package_ProInfo();
                    ProItem.ProMainNameID = Item.ProMainID;
                    ProItem.Salary = Item.Salary;
                    ProItem.SellType = GrounpItem.SellTypeID;
                    Grounp.Package_ProInfo.Add(ProItem);
                }
                Head.Package_GroupInfo.Add(Grounp);  
            }
            #endregion

            #region 营业厅明细
            List<API.Pro_HallInfo> HallSource = HallGrid.ItemsSource as List<API.Pro_HallInfo>;
            foreach (var HallItem in HallSource)
            {
                API.VIP_HallOffInfo HallOff = new API.VIP_HallOffInfo();
                HallOff.HallID = HallItem.HallID;

                if (Head.VIP_HallOffInfo == null)
                    Head.VIP_HallOffInfo = new List<API.VIP_HallOffInfo>();
                Head.VIP_HallOffInfo.Add(HallOff);
            }
            #endregion

            PublicRequestHelp h = new PublicRequestHelp(busy,MethodIDStore.AddPackage, new object[] { Head }, MyClient_Completed);
        }
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {

                if (e.Result.ReturnValue == true)
                {
                    CancelPart();
                    Initial();
                    //newSell.Clear();
                    //this.dataGridOffList.ItemsSource = newSell;
                    //this.dataGridOffList.Rebind();
                    //offList_List.Clear();
                }
                Logger.Log(e.Result.Message + "");
                MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message);

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                this.busy.IsBusy = false;
            }
        }
        #endregion
        #region 清空数据
        void CancelPart()
        {
            this.EndTime.SelectedValue = null;
            this.StartTime.SelectedValue = null;
            this.Name.Text = string.Empty;
            this.Note.Text = string.Empty;   
            this.ReduceCash.Value = 0;     
            dataGridVipType.ItemsSource = null;
            dataGridVipType.Rebind();

            dataGridVip2.ItemsSource = null;
            dataGridVip2.Rebind();

            HallGrid.ItemsSource = null;
            HallGrid.Rebind();

            GounpList.Clear();
            this.PageGrid.Rebind();

            ProGridOther.ItemsSource = null;
            ProGridOther.Rebind();
        }
        #endregion

        private void PageGrid_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            if (PageGrid.SelectedItem == null)
            {
                return;
            }
            GounpSource GrounpItem = PageGrid.SelectedItem as GounpSource;
            if (GrounpItem == null)
            {
                this.ProGridOther.ItemsSource = null;
                this.ProGridOther.Rebind();
                return;
            }
            this.ProGridOther.ItemsSource = GrounpItem.ProModel;
            this.ProGridOther.Rebind();
        }

    }
}
