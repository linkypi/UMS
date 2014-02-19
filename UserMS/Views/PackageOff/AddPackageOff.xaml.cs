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
using UserMS.Model.PackageModel;

namespace UserMS.Views.PackageOff
{
    /// <summary>
    /// AddHallOff.xaml 的交互逻辑
    /// </summary>
    public partial class AddPackageOff : BasePage
    {
        private Mul_HallFilter hAdder;
        private List<API.SelecterIMEI> uncheckIMEI;
        private List<GounpSource> GounpList = new List<GounpSource>();
        private List<API.Package_GroupInfo> GrounpNameList = new List<API.Package_GroupInfo>();
        List<API.Package_SalesNameInfo> SalesNameInfo;
        List<API.VIP_OffList> OffList;
        public AddPackageOff()
        {
            InitializeComponent();
            delTemplate.IsEnabled = false;
            CreatName.Text = Store.LoginUserName;
            hAdder = new Mul_HallFilter(ref this.HallGrid);
            uncheckIMEI = new List<API.SelecterIMEI>();
            this.PageGrid.ItemsSource = GounpList;
          
            GridViewComboBoxColumn cb = PageGrid.Columns[2] as GridViewComboBoxColumn;
            cb.ItemsSource = Store.SellTypes;
            
           //this.PageGrid.AddHandler(RadComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(GridViewComboBoxColumn_PropertyChanged_1));
            //获取所有分组   获取分组完成后初始化数据
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 264, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetGroupTypeCompleted));
        
        }

        /// <summary>
        /// 处理PageGrid中的分组名称选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewComboBoxColumn_PropertyChanged_1(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox comb = e.OriginalSource as RadComboBox;
            if (comb.DataContext == null)
            {
                return;
            }
            if (comb.SelectedIndex == -1)
            {
                ((GounpSource)comb.DataContext).GroupID = -1;
                return;
            }
            //GridViewComboBoxColumn comcol1 = PageGrid.Columns[1] as GridViewComboBoxColumn;
            //List<API.View_PackageGroupTypeInfo> list = comcol1.ItemsSource as List<API.View_PackageGroupTypeInfo>;
            //if (list != null)
            //{
            //    ((GounpSource)comb.DataContext).GroupID = list[comb.SelectedIndex].ID;
            //}
            //else
            //{
            //    ((GounpSource)comb.DataContext).GroupID = -1;
            //}
        }

        void Initial()
        {
            CBLimit.IsChecked = false;
            LimitNum.IsEnabled = false;
            LimitNum.Value = 99999;
            GetGrounp();
        }

        private void GetGroupTypeCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                List<API.View_PackageGroupTypeInfo> list = e.Result.Obj as List<API.View_PackageGroupTypeInfo>;
                if (list != null)
                {
                    GridViewComboBoxColumn comcol1 = PageGrid.Columns[1] as GridViewComboBoxColumn;
                    comcol1.ItemsSource = list;
                }
               Initial();
               GetDeptSource();
            }
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

                    var query = (from b in TypeList
                                 join c in phList on b.ID equals c.ID
                                 select c).ToList();
                    foreach (var Item in query)
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
        #region 会员操作
        private void AddVIP_Click(object sender, RoutedEventArgs e)
        {
            ActiceAddVIP();
        }
        private void txtIMEI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ActiceAddVIP();
            }
        }
        #region 添加会员
        List<string> query;
        private void ActiceAddVIP()
        {
            ViewOperate.IMEIAdd(this.txtIMEI.Text.Trim(), ref uncheckIMEI);
            //获取会员卡号
            query = (from b in uncheckIMEI
                     select b.IMEI).ToList();
            if (query.Count() == 0)
            { return; }
            API.ReportPagingParam pageParam = new API.ReportPagingParam();
            pageParam.ParamList = new List<API.ReportSqlParams>();
            //会员卡号查询         
            API.ReportSqlParams_ListString IMEI = new API.ReportSqlParams_ListString();
            IMEI.ParamName = "IMEI_List";
            IMEI.ParamValues = query;
            pageParam.ParamList.Add(IMEI);

            if (pageParam.ParamList.Count() > 0)
            {
                PublicRequestHelp h = new PublicRequestHelp(busy, MethodIDStore.GetVIPMethod, new object[] { pageParam }, MyClient_MainReportCompleted);
            }

        }
        protected void MyClient_MainReportCompleted(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {

                if (e.Result.ReturnValue == true)
                {
                    API.ReportPagingParam pageParem = (API.ReportPagingParam)e.Result.Obj;

                    this.dataGridVip2.ItemsSource = pageParem.Obj;
                    TBoxAddVIP();
                }
                else
                {
                    Logger.Log(e.Result.Message + "");
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                this.busy.IsBusy = false;
                txtIMEI.Text = string.Empty;
            }
        }

        private void TBoxAddVIP()
        {
            if (dataGridVip2.ItemsSource == null)
                return;
            List<API.View_VIPInfo> VIPInfo = dataGridVip2.ItemsSource as List<API.View_VIPInfo>;
            List<string> ReturnIMEI = (from b in VIPInfo
                                       select b.IMEI).ToList();
            foreach (var VIPItem in ReturnIMEI)
            {
                query.Remove(VIPItem);
            }

            foreach (var StringItem in query)
            {
                txtIMEI.Text += StringItem + "\r\n";
            }
        }
        #endregion

        #region 删除会员
        private void DelVIP_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelVIP(ref dataGridVip2);
        }
        #endregion
        #endregion

        #region 销售分组操作
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            GounpSource GounpItem = new GounpSource();
            //GounpItem.SellTypeList = Store.Options.Where(p => p.ClassName == "SalesPromotion").ToList();
          
           
            GounpItem.SellTypeID = Convert.ToInt32(Store.Options.Where(p => p.ClassName == "SalesPromotion").First().Value);
            GounpItem.SellTypeName = Store.SellTypes.Where(p => p.ID == GounpItem.SellTypeID).First().Name;
            //GounpItem.GrounpNameList = new List<API.Package_GroupInfo>() 
            //{ 
            //  new  API.Package_GroupInfo(){ GroupName="终端"},
            //  new  API.Package_GroupInfo(){ GroupName="号码卡"},
            //  new  API.Package_GroupInfo(){ GroupName="会员"},
            //  new  API.Package_GroupInfo(){ GroupName="延保"},
            //  new  API.Package_GroupInfo(){ GroupName="配件"},
            //  new  API.Package_GroupInfo(){ GroupName="礼品"}
            //};
            GounpItem.IsmustText = "否";
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
        private void GetGrounp()
        {
           
            int SellTypeID = Convert.ToInt32(Store.Options.Where(p => p.ClassName == "SalesPromotion").First().Value);

            string SellTypeName = Store.SellTypes.Where(p => p.ID == SellTypeID).First().Name;
            //List<API.Package_GroupInfo>  GrounpNameList = new List<API.Package_GroupInfo>() 
            //{ 
            //  new  API.Package_GroupInfo(){ GroupName="终端"},
            //  new  API.Package_GroupInfo(){ GroupName="号码卡"},
            //  new  API.Package_GroupInfo(){ GroupName="会员"},
            //  new  API.Package_GroupInfo(){ GroupName="延保"},
            //  new  API.Package_GroupInfo(){ GroupName="配件"},
            //  new  API.Package_GroupInfo(){ GroupName="礼品"}
            //};

            GridViewComboBoxColumn comcol1 = PageGrid.Columns[1] as GridViewComboBoxColumn;
            List<API.View_PackageGroupTypeInfo> list = comcol1.ItemsSource as List<API.View_PackageGroupTypeInfo>;

            if (list == null)
            {
                return;
            }
            this.GounpList.Clear();
            
            foreach (var Item in list)
            {
                GounpSource GounpItem = new GounpSource();
                //if (list != null)
                //{
                //    GounpItem.GroupID = list[0].ID;
                //}
                GounpItem.SellTypeName = SellTypeName;
                GounpItem.SellTypeID = SellTypeID;
                GounpItem.GroupName = Item.GroupName;
                GounpItem.GroupID = Item.ID;
                GounpItem.IsmustText = "否";
                this.GounpList.Add(GounpItem);
            }
            this.PageGrid.Rebind();
        }

        #region 商品操作
        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MultSelecter2 msFrm = new MultSelecter2(
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
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;
            if (selecter.DialogResult == true)
            {
                List<UserMS.API.Pro_ProNameInfo> phList = selecter.SelectedItems.OfType<API.Pro_ProNameInfo>().ToList();
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
                foreach (var Item in phList)
                {
                    var query = from b in Gounp.ProModel
                                where b.ProMainID == Item.ID
                                select b;
                    if (query.Count() > 0) return;
                    API.ProModel model = new API.ProModel();
                    model.ProMainID = Item.ID;
                    model.ProName = Item.MainName;
                    model.SellTypeName = Gounp.SellTypeName;
                    Gounp.ProModel.Add(model);
                }
                ProGridOther.ItemsSource = Gounp.ProModel;
                ProGridOther.Rebind();
            }
        }
        private void DelPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
          List<API.ProModel> ProModel = ProGridOther.ItemsSource as List<API.ProModel>;
          foreach (var Item in ProGridOther.SelectedItems)
          {
              ProModel.Remove(Item as API.ProModel);
          }
          ProGridOther.Rebind();
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

        #region 获取左边树
        /// <summary>
        /// 获取左边树
        /// </summary>
        private void GetDeptSource()
        {
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.GetSalesName, new object[] { }, SearchCompleted);
        }

        void SearchCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue)
                {

                    SalesNameInfo = results.Result.Obj as List<API.Package_SalesNameInfo>;
                    GetLeftTree(SalesNameInfo);
                }
                else
                {
                    MessageBox.Show("无套餐分类！");
                }

            }
            else
                Logger.Log("无数据");
        }
        /// <summary>
        /// 获取父级节点
        /// </summary>
        /// <param name="Dept"></param>
        private void GetLeftTree(List<API.Package_SalesNameInfo> salesNameInfo)
        {
            var ParentInfo = salesNameInfo.Where(p => p.Parent == 0);
            foreach (var Item in ParentInfo)
            {
                TreeView.Items.Add(GetChildItem(Item.ID));
            }
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        private RadTreeViewItem GetChildItem(int salesNameID)
        {
            var Children = from b in SalesNameInfo
                           where b.Parent == salesNameID
                           select b;
            var pnode = from b in SalesNameInfo
                        where b.ID == salesNameID
                        select b;
            RadTreeViewItem parent = new RadTreeViewItem() { Header = pnode.First().SalesName, Tag = pnode.First().ID };
            if (pnode.First().Parent == 0) parent.IsExpanded = true;
            if (Children.Count() != 0)
            {
                foreach (var Item in Children)
                {
                    RadTreeViewItem child = GetChildItem(Item.ID);
                    parent.Items.Add(child);
                }
            }
            return parent;
        }
        #endregion

        #region GridView操作
        private void ProGridOther_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            API.ProModel ProItem = this.ProGridOther.SelectedItem as API.ProModel;
            if (e.Cell.Column.Header.ToString() == "提成")
            {
                if (ProItem.Salary < 0)
                {
                    ProItem.Salary = 0;
                }
                int value = (int)(ProItem.Salary * 100);
                ProItem.Salary = (decimal)(value * 0.01);

            }
        }
        private void PageGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            GounpSource GrounpItem = PageGrid.SelectedItem as GounpSource;
            if (GrounpItem == null)
            {
                this.ProGridOther.ItemsSource = null;
                this.ProGridOther.Rebind();
                return;
            }
            this.ProGridOther.ItemsSource = GrounpItem.ProModel;
            if (GrounpItem.ProModel == null) return;
            var query = from b in GrounpItem.ProModel
                        where string.IsNullOrEmpty(b.ProName)
                        select b;
            if (query.Count() > 0)
            {
                List<string> queryProNameList = (from b in GrounpItem.ProModel
                               join c in Store.ProNameInfo on b.ProMainID equals c.ID        
                               select c.MainName).ToList();
                if (queryProNameList.Count() != GrounpItem.ProModel.Count())
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "初始化数据有误，请重新登陆！");
                    return;
                }
                for (int i = 0; i < queryProNameList.Count();i++ )
                {
                    GrounpItem.ProModel[i].ProName = queryProNameList[i];
                }
            }
            this.ProGridOther.Rebind();
        }
        #endregion

        #region 选择人数
        private void CBLimit_Click_1(object sender, RoutedEventArgs e)
        {
            if (CBLimit.IsChecked == true)
            {
                LimitNum.IsEnabled = true;
                LimitNum.Value =0;
            }
            else
            {
                LimitNum.IsEnabled = false;
                LimitNum.Value = 99999;
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
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入套餐名称");
                return;
            }
            if (StartTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入开始日期");
                return;
            }
            if (EndTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入结束日期");
                return;
            }
            if (StartTime.SelectedValue > EndTime.SelectedValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "开始日期不能大于结束日期！");
                return;
            }
            if (EndTime.SelectedValue <= DateTime.Now)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "活动结束日期必须大于当前日期");
                return;
            }
            //if (StartTime.SelectedValue < DateTime.Now)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow, "活动开始日期必须大于等于当前日期");
            //    return;
            //}
            if (LimitNum.Value==0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入限制人数或设为默认");
                return;
            }
            try
            {
                Head.VIPTicketMaxCount = Convert.ToInt32(this.LimitNum.Value);
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "人数个数必须为整数");
                return;
            }

            if (TreeView.Items.Count == 0 || TreeView.SelectedItems.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择套餐分类");
                return;
            }
            else
            {
                RadTreeViewItem Item = TreeView.SelectedItem as RadTreeViewItem;
                Head.Type = Convert.ToInt32(Item.Tag);
            }
            if (HallGrid.ItemsSource == null || HallGrid.Items.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加营业厅");
                return;
            }
            #endregion

            if (ReduceCash.Value == 0)
            {
                MessageBox.Show("套餐价格不能为0！"); return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定增加优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            #region 添加表头
          
            Head.Name = this.Name.Text.Trim();
            Head.StartDate = this.StartTime.SelectedValue;
            Head.EndDate = this.EndTime.SelectedValue;
            Head.ArriveMoney = (decimal)ReduceCash.Value;
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
            if (GounpList.Count() <= 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加分组！");
                return;
            }
            var query = from b in GounpList
                        where b.GroupID <= 0
                        select b;
            if (query.Count() > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "分组未添加名称！");
                return;
            }
            var queryPro = from b in GounpList
                           where b.ProModel == null || b.ProModel.Count() == 0
                           select b;
            if (query.Count() == GounpList.Count())
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "必须有一个分组有商品！");
                return;
            }
            if (queryPro.Count() > 0)
            {
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "存在分组未添加商品，是否继续？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
            }
            Head.Package_GroupInfo = new List<API.Package_GroupInfo>();
            foreach (var GrounpItem in GounpList)
            {
                if (GrounpItem.ProModel == null)
                {
                    continue;
                }
                if (GrounpItem.ProModel.Count() == 0)
                {
                    continue;
                }
                API.Package_GroupInfo Grounp = new API.Package_GroupInfo();

                Grounp.GroupID = GrounpItem.GroupID;
                if (GrounpItem.GroupID <= 0)
                {
                    MessageBox.Show("分组名称不能为空！");
                    return;
                }

                //Grounp.GroupName = GrounpItem.GroupName;         
                Grounp.IsMust = GrounpItem.IsmustText == "是" ? true : false;
                Grounp.SellType = GrounpItem.SellTypeID;
                Grounp.Note = GrounpItem.Note;

                if (GrounpItem.ProModel != null && GrounpItem.ProModel.Count() > 0)
                {
                    Grounp.Package_ProInfo = new List<API.Package_ProInfo>();
                    foreach (var Item in GrounpItem.ProModel)
                    {
                        API.Package_ProInfo ProItem = new API.Package_ProInfo();
                        ProItem.ProMainNameID = Item.ProMainID;
                        ProItem.Salary = Item.Salary;
                        ProItem.SellType = GrounpItem.SellTypeID;
                        Grounp.Package_ProInfo.Add(ProItem);
                    }
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

            bool del =false;
            int delID = 0;
            if (delTemplate.IsEnabled && delTemplate.IsChecked==true)
            {
                if (MessageBox.Show("确定删除模版吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    del = true;
                }
                delID = Convert.ToInt32(delTemplate.Tag);
            }
           
            PublicRequestHelp h = new PublicRequestHelp(busy, MethodIDStore.AddPackage,
                new object[] { Head,del,delID }, MyClient_Completed);
        }
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {

                if (e.Result.ReturnValue == true)
                {
                    CancelPart();
                    Initial();
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
            this.CBLimit.IsChecked = false;
            LimitNum.IsEnabled = false;
            LimitNum.Value = 99999;
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

        #region 获取模板
        private void RadMenuItem_Click_5(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            PublicRequestHelp help = new PublicRequestHelp(this.busy, 317, 
                new object[] { }, GetSourceCompleted);

        }
        private void GetSourceCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.Obj != null)
                {           
                    OffList = results.Result.Obj as List<API.VIP_OffList>;
                    var ParentInfo = SalesNameInfo.Where(p => p.Parent == 0);
                    List<TreeViewModel> TreeList = new List<TreeViewModel>();
                    foreach (var Item in ParentInfo)
                    {
                        TreeList.Add(Common.CommonHelper.SalesViewModel(Item.ID, SalesNameInfo));
                    }

                    SingleSelecter w = new SingleSelecter(TreeList, OffList, "Type",
                                                         "Name", new string[] { "ID", "Name","Note","StartDate","EndDate" },
                                                         new string[] { "优惠编号", "优惠名称","说明","开始日期","结束日期" }, true);

                    w.Closed += SellerSearchWindowClose;
                    w.ShowDialog();
                }
            }
            else
                Logger.Log("无数据");
        }
        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    API.VIP_OffList selected = (API.VIP_OffList)window.SelectedItem;
                    PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.GetPackageSource, 
                        new object[] { selected.ID, "" }, ChangeSourceCompleted);
                }
            }

        }
        private void ChangeSourceCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            delTemplate.IsEnabled = false;
            if (results.Error == null)
            {
                if (results.Result.Obj != null&&results.Result.ReturnValue==true)
                {
                    delTemplate.IsEnabled = true;
                   
                    CancelPart();
                    API.OffModel Off = results.Result.Obj as API.OffModel;
                    delTemplate.Tag = Off.ID;
                    //获取表头
                    ReduceCash.Value = Convert.ToDouble(Off.ArriveMoney);
                    StartTime.SelectedValue = Off.StartDate;
                    EndTime.SelectedValue = Off.EndDate;
                    this.Name.Text = Off.Name;
                    this.Note.Text = Off.Note;

                    CBLimit.IsChecked = true;
                    LimitNum.IsEnabled = true;
                    LimitNum.Value = Off.VIPTicketMaxCount;                  
                    //获取分组和商品 
                    GounpList.Clear();
                    foreach (var Item in Off.GrounpInfo)
                    {
                        GounpSource GounpItem = new GounpSource();
                        GounpItem.GroupID = Item.GroupID;
                        GounpItem.GroupName = Item.GrounpName;
                        GounpItem.IsmustText = Item.IsMustName;
                        GounpItem.Note = Item.Note;
                        GounpItem.SellTypeID = Item.SellType;
                        GounpItem.SellTypeName = Item.SellTypeName;
                        GounpItem.ProModel = new List<API.ProModel>();
                        GounpItem.ProModel.AddRange(Item.ProModel);
                        GounpList.Add(GounpItem);
                    }
                    this.PageGrid.Rebind();
                    //获取会员类别
                    // List<API.VIP_VIPType> VIPTypeSource = dataGridVipType.ItemsSource as List<API.VIP_VIPType>;
                    List<API.VIP_VIPType> VIPTypeSource = new List<API.VIP_VIPType>();
                    if (Off.VIPTypeModel != null)
                    {
                        foreach (var Item in Off.VIPTypeModel)
                        {
                            API.VIP_VIPType VIPType = new API.VIP_VIPType();
                            VIPType.Name = Item.VIPTypeName;
                            VIPType.ID = Item.TypeID;
                            VIPTypeSource.Add(VIPType);
                        }
                        dataGridVipType.ItemsSource = VIPTypeSource;
                        dataGridVipType.Rebind();
                    }
                    //获取会员
                    List<API.View_VIPInfo> VIPSource = new List<API.View_VIPInfo>();
                    if (Off.VIPModel != null)
                    {
                        foreach (var Item in Off.VIPModel)
                        {
                            API.View_VIPInfo VIP = new API.View_VIPInfo();
                            VIP.IMEI = Item.IMEI;
                            VIP.MemberName = Item.VIPName;
                            VIP.ID = Item.ID;
                            VIPSource.Add(VIP);
                        }
                        dataGridVip2.ItemsSource = VIPSource;
                        dataGridVip2.Rebind();
                    }
                    //门店 
                    List<API.Pro_HallInfo> HallSource = new List<API.Pro_HallInfo>();
                    foreach (var Item in Off.HallModel)
                    {
                        API.Pro_HallInfo Hall = new API.Pro_HallInfo();
                        Hall.HallID = Item.HallID;
                        Hall.HallName = Item.HallName;
                        HallSource.Add(Hall);
                    }
                    HallGrid.ItemsSource = HallSource;
                    HallGrid.Rebind();

                }
            }
            else
                Logger.Log("无数据");
        }
        #endregion

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show("确定取消吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            CancelPart();
        }

        //private void TreeView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    RadTreeView tv = sender as RadTreeView;
        //    RadTreeViewItem p = tv.SelectedItem as RadTreeViewItem;
        //    packageType.Text = p.Header.ToString();
        //    packageType.Tag = p.Tag;
        //}

    }
}
