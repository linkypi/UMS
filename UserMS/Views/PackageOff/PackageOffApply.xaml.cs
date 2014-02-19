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
    /// PackageOffApply.xaml 的交互逻辑 
    /// </summary>
    public partial class PackageOffApply : Page
    {
        private Mul_HallFilter hAdder;
        private List<API.SelecterIMEI> uncheckIMEI;
       private List<GounpSource> GounpList = new List<GounpSource>();
        
        //private List<API.Package_GroupInfo> GrounpNameList = new List<API.Package_GroupInfo>();
        List<API.Package_SalesNameInfo> SalesNameInfo;
        List<API.VIP_OffList> OffList;

        List<VIPOffModel> models = new List<VIPOffModel>();
       // List<API.Package_GroupInfo> 


        public PackageOffApply()
        {
            InitializeComponent();

            //GridViewComboBoxColumn dc = groupGrid.Columns[3] as GridViewComboBoxColumn;
            //List<SlModel.CkbModel> list = new List<SlModel.CkbModel>(){
            //    new SlModel.CkbModel(false,"否"),new SlModel.CkbModel(true,"是")
            //};
            //dc.ItemsSource = list;


            CreatName.Text = Store.LoginUserName;
            hAdder = new Mul_HallFilter(ref this.HallGrid);
            //uncheckIMEI = new List<API.SelecterIMEI>();
            this.groupGrid.ItemsSource = GounpList;
            headGrid.ItemsSource = models;
          
            GridViewComboBoxColumn cb = groupGrid.Columns[2] as GridViewComboBoxColumn;
            cb.ItemsSource = Store.SellTypes;
            
           //this.groupGrid.AddHandler(RadComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(GridViewComboBoxColumn_PropertyChanged_1));
            //获取所有分组   获取分组完成后初始化数据
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 264, new object[] { },
                new EventHandler<API.MainCompletedEventArgs>(GetGroupTypeCompleted));
          
        }

        /// <summary>
        /// 处理groupGrid中的分组名称选项
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
            //GridViewComboBoxColumn comcol1 = groupGrid.Columns[1] as GridViewComboBoxColumn;
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
                    GridViewComboBoxColumn comcol1 = groupGrid.Columns[1] as GridViewComboBoxColumn;
                    comcol1.ItemsSource = list;
                }
               Initial();
               GetDeptSource();
            }
        }

        #region 添加门店
        private void AddHall_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //var s  = from a  in Store.RoleInfo
            //             where a.RoleID == Store.LoginRoleInfo.Sys_Role_Menu_HallInfo
            var query = (from b in Store.LoginRoleInfo.Sys_Role_Menu_HallInfo
                         join c in Store.ProHallInfo
                         on b.HallID equals c.HallID into hall
                         from b1 in hall.DefaultIfEmpty()
                         select b1).ToList();
            hAdder.GetHall(query.ToList());
        }
        #endregion
        #region 删除门店
        private void DelHall_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //ViewOperate.DelHall(ref HallGrid);
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
            //UserMS.MultSelecter selecter = sender as UserMS.MultSelecter;
            //if (selecter.DialogResult == true)
            //{
            //    List<UserMS.API.VIP_VIPType> phList = selecter.SelectedItems.OfType<API.VIP_VIPType>().ToList();
            //    if (this.dataGridVipType.ItemsSource == null)
            //        this.dataGridVipType.ItemsSource = phList;
            //    else
            //    {
            //        List<UserMS.API.VIP_VIPType> TypeList = this.dataGridVipType.ItemsSource as List<UserMS.API.VIP_VIPType>;

            //        var query = (from b in TypeList
            //                     join c in phList on b.ID equals c.ID
            //                     select c).ToList();
            //        foreach (var Item in query)
            //            phList.Remove(Item);
            //        TypeList.AddRange(phList);
            //        this.dataGridVipType.Rebind();
            //    }
            //}
        }
        #endregion

        #region 删除会员卡类型
        private void DeleteVIPType_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //ViewOperate.DelVIPType(ref dataGridVipType);
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
            //ViewOperate.IMEIAdd(this.txtIMEI.Text.Trim(), ref uncheckIMEI);
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
            //try
            //{

            //    if (e.Result.ReturnValue == true)
            //    {
            //        API.ReportPagingParam pageParem = (API.ReportPagingParam)e.Result.Obj;

            //        this.dataGridVip2.ItemsSource = pageParem.Obj;
            //        TBoxAddVIP();
            //    }
            //    else
            //    {
            //        Logger.Log(e.Result.Message + "");
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Logger.Log(ex.Message);
            //}
            //finally
            //{
            //    this.busy.IsBusy = false;
            //    txtIMEI.Text = string.Empty;
            //}
        }

        private void TBoxAddVIP()
        {
            //if (dataGridVip2.ItemsSource == null)
            //    return;
            //List<API.View_VIPInfo> VIPInfo = dataGridVip2.ItemsSource as List<API.View_VIPInfo>;
            //List<string> ReturnIMEI = (from b in VIPInfo
            //                           select b.IMEI).ToList();
            //foreach (var VIPItem in ReturnIMEI)
            //{
            //    query.Remove(VIPItem);
            //}

            //foreach (var StringItem in query)
            //{
            //    txtIMEI.Text += StringItem + "\r\n";
            //}
        }
        #endregion

        #region 删除会员
        private void DelVIP_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //ViewOperate.DelVIP(ref dataGridVip2);
        }
        #endregion
        #endregion

        #region 分组操作

        private void AddGroup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            GounpSource item = new GounpSource();
            //API.Package_GroupInfo item = new API.Package_GroupInfo();
            item.SellTypeID = Convert.ToInt32(Store.Options.Where(p => p.ClassName == "SalesPromotion").First().Value);
            GridViewComboBoxColumn cb = groupGrid.Columns[1] as GridViewComboBoxColumn;
            List<API.View_PackageGroupTypeInfo> list = cb.ItemsSource as List<API.View_PackageGroupTypeInfo>;

            item.GroupID = list[0].ID;
            item.IsmustText = "否";
            item.ProModel = new List<API.ProModel>();
            //item.Ismust = false;
            GounpList.Add(item);

            GounpSource Gounp = groupGrid.SelectedItem as GounpSource;
            VIPOffModel model = headGrid.SelectedItem as VIPOffModel;
            if (model != null)  
            {
                foreach (var child in models)
                {
                    if (child == model)
                    {
                        if (child.Children == null)
                        {
                            child.Children = new List<GounpSource>();
                        }
                        child.Children.Clear();
                        foreach (var xxd in GounpList)
                        {
                            GounpSource g = CloneGroup(xxd);
                            child.Children.Add(g);
                        }
                        break;
                    }
                }

            }
            this.groupGrid.Rebind();
        }

        private void DelGroup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            GounpSource Gounp = groupGrid.SelectedItem as GounpSource;

            foreach (var Item in groupGrid.SelectedItems)
            {
                GounpList.Remove(Item as GounpSource);
            }

            VIPOffModel model = headGrid.SelectedItem as VIPOffModel;
            if (model != null)   //添加到未有套餐
            {
                foreach (var child in models)
                {
                    if (child == model)
                    {
                        child.Children.Clear();
                        foreach (var xxd in GounpList)
                        {
                            GounpSource mm = new GounpSource();
                            mm.IsmustText = xxd.IsmustText;
                            mm.SellTypeID = xxd.SellTypeID;
                            mm.GroupID = xxd.GroupID;
                            child.Children.Add(mm);
                        }
                    }
                }
            }
        
            this.groupGrid.Rebind();
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

            GridViewComboBoxColumn comcol1 = groupGrid.Columns[1] as GridViewComboBoxColumn;
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
            this.groupGrid.Rebind();
        }

        #region 商品操作
        private void AddPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (groupGrid.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择一个分组！");
                return;
            }
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
                GounpSource Gounp = groupGrid.SelectedItem as GounpSource;
                foreach (var item in GounpList)
                {
                    if (item == Gounp)
                    {
                        if (item.ProModel == null)
                        { item.ProModel = new List<API.ProModel>(); }
                        foreach (var Item in phList)
                        {
                            var query = from b in item.ProModel
                                        where b.ProMainID == Item.ID
                                        select b;
                            if (query.Count() > 0) return;
                            API.ProModel model = new API.ProModel();
                            model.ProMainID = Item.ID;
                            model.ProName = Item.MainName;
                            model.SellTypeID = Gounp.SellTypeID;
                            model.SellTypeName = Gounp.SellTypeName;
                            item.ProModel.Add(model);
                        }
                        ProGridOther.ItemsSource = Gounp.ProModel;
                        ProGridOther.Rebind();
                        break;
                    }
                }


                if (headGrid.SelectedItem != null) 
                {
                    VIPOffModel model = headGrid.SelectedItem as VIPOffModel;
                  
                    bool flag = false;
                    foreach (var item in models)
                    {
                        if (model == item)
                        {
                            item.Children.Clear();
                            foreach (var xxc in GounpList)
                            {
                                GounpSource g = CloneGroup(xxc);
                                item.Children.Add(g);
                            }
                            break;
                            //foreach (var child in item.Children)
                            //{
                            //    if (Gounp == child)
                            //    {
                            //        if (child.ProModel == null)
                            //        {
                            //            child.ProModel = new List<API.ProModel>();
                            //        }
                            //        //foreach (var Item in Gounp.ProModel)
                            //        //{
                            //        //    API.ProModel mm = new API.ProModel();
                            //        //    mm.ProMainID = Item.ProMainID;
                            //        //    mm.ProName = Item.ProName;
                            //        //    mm.SellTypeName = Gounp.SellTypeName;
                            //        //    child.ProModel.Add(mm);
                            //        //}
                            //        foreach (var Item in phList)
                            //        {
                            //            var query = from b in child.ProModel
                            //                        where b.ProMainID == Item.ID
                            //                        select b;
                            //            if (query.Count() > 0) return;
                            //            API.ProModel mm = new API.ProModel();
                            //            mm.ProMainID = Item.ID;
                            //            mm.ProName = Item.MainName;
                            //            mm.SellTypeName = Gounp.SellTypeName;
                            //            child.ProModel.Add(mm);
                            //        }
                            //        ProGridOther.ItemsSource = Gounp.ProModel;
                            //        ProGridOther.Rebind();
                            //        flag = true;
                            //        break;
                            //    }
                            //}
                        }
                        if (flag)
                        {
                            break;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (groupGrid.SelectedItem == null)
            {
                MessageBox.Show("请选择分组！");
                return;
            }
            GounpSource Gounp = groupGrid.SelectedItem as GounpSource;

            foreach (var item in GounpList)
            {
                if (item == Gounp)
                {
                    foreach (var child in ProGridOther.SelectedItems)
                    {
                        item.ProModel.Remove(child as API.ProModel);
                        //Gounp.ProModel.Remove(child as API.ProModel);
                    }
                    break;
                }
            }

            if (headGrid.SelectedItem != null) //删除已有套餐中的商品
            {
                VIPOffModel model = headGrid.SelectedItem as VIPOffModel;
                bool flag = false;

                foreach (var item in models)
                {
                    if (item == model)
                    {
                        flag = true;
                        foreach (var child in item.Children)
                        {
                            if (child == Gounp)
                            {
                                foreach (var Item in ProGridOther.SelectedItems)
                                {
                                    child.ProModel.Remove(Item as API.ProModel);
                                }
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
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
            GounpSource GrounpItem = groupGrid.SelectedItem as GounpSource;
            if (GrounpItem == null)
            {
                this.ProGridOther.ItemsSource = null;
                this.ProGridOther.Rebind();
                return;
            }
            this.ProGridOther.ItemsSource = GrounpItem.ProModel;
            //if (GrounpItem.ProModel == null) return;
            //var query = from b in GrounpItem.ProModel
            //            where string.IsNullOrEmpty(b.ProName)
            //            select b;
            //if (query.Count() > 0)
            //{
            //    List<string> queryProNameList = (from b in GrounpItem.ProModel
            //                   join c in Store.ProNameInfo on b.ProMainID equals c.ID        
            //                   select c.MainName).ToList();
            //    if (queryProNameList.Count() != GrounpItem.ProModel.Count())
            //    {
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow, "初始化数据有误，请重新登陆！");
            //        return;
            //    }
            //    for (int i = 0; i < queryProNameList.Count();i++ )
            //    {
            //        GrounpItem.ProModel[i].ProName = queryProNameList[i];
            //    }
            //}
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

        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.VIP_OffListAduitHeader head = new API.VIP_OffListAduitHeader();

            #region 检查数据有效性
            //if (String.IsNullOrEmpty(this.Name.Text))
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入套餐名称");
            //    return;
            //}
          
            if (string.IsNullOrEmpty(destination.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "目的不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(saleTarget.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "销售目标不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(startDate.DateTimeText))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "开始日期不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(endDate.DateTimeText))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "结束日期不能为空！");
                return;
            }
            if (endDate.SelectedValue <= DateTime.Now)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "活动结束日期必须大于当前日期");
                return;
            }
            if (startDate.SelectedValue < DateTime.Now.Date)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "活动开始日期必须大于等于当前日期");
                return;
            }
            //if (LimitNum.Value==0)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入限制人数或设为默认");
            //    return;
            //}
            //try
            //{
            //    Head.VIPTicketMaxCount = Convert.ToInt32(this.LimitNum.Value);
            //}
            //catch
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow, "人数个数必须为整数");
            //    return;
            //}

            //if (TreeView.Items.Count == 0 || TreeView.SelectedItems.Count() == 0)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择套餐分类");
            //    return;
            //}
            //else
            //{
            //    RadTreeViewItem Item = TreeView.SelectedItem as RadTreeViewItem;
            //    Head.Type = Convert.ToInt32(Item.Tag);
            //}
            if (HallGrid.ItemsSource == null || HallGrid.Items.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加营业厅");
                return;
            }

            var valname = from a in models
                          where string.IsNullOrEmpty(a.Name)
                          select a;
            if (valname.Count() > 0)
            {
                MessageBox.Show("套餐名称不能为空！");
                return;
            }

            var valprice = from a in models
                          where a.Price<=0
                          select a;
            if (valprice.Count() > 0)
            {
                MessageBox.Show("套餐价格无效！");
                return;
            }

            foreach (var item in models)
            {
                if(item.Children==null)
                {
                    MessageBox.Show("套餐 "+item.Name+" 的分组不能为空！");
                    return;
                }

                var valgroup = from a in item.Children
                               where a.ProModel == null || a.ProModel.Count == 0
                               select a;
                if (valgroup.Count() > 0)
                {
                    MessageBox.Show("套餐 " + item.Name +" 分组 "+ valgroup.First().GroupName + " 的商品不能为空！");
                    return;
                }
            }

            if (ReduceCash.Value == 0)
            {
                MessageBox.Show("套餐价格不能为0！"); return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定申请吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            { return; }

            
            #endregion

            #region 会员类型明细
            //if (dataGridVipType.ItemsSource != null && dataGridVipType.Items.Count > 0)
            //{
            //    List<API.VIP_VIPType> VIPTypeSource = dataGridVipType.ItemsSource as List<API.VIP_VIPType>;
            //    foreach (var TypeItem in VIPTypeSource)
            //    {
            //        API.VIP_VIPTypeOffLIst VipType = new API.VIP_VIPTypeOffLIst();
            //        VipType.VIPType = TypeItem.ID;
            //        if (Head.VIP_VIPTypeOffLIst == null)
            //            Head.VIP_VIPTypeOffLIst = new List<API.VIP_VIPTypeOffLIst>();
            //        Head.VIP_VIPTypeOffLIst.Add(VipType);
            //    }
            //}
            #endregion

            #region 会员明细
            //if (dataGridVip2.ItemsSource != null && dataGridVip2.Items.Count > 0)
            //{
            //    List<API.View_VIPInfo> VIPSource = dataGridVip2.ItemsSource as List<API.View_VIPInfo>;
            //    foreach (var VIPItem in VIPSource)
            //    {
            //        API.VIP_VIPOffLIst VIP = new API.VIP_VIPOffLIst();
            //        VIP.VIPID = VIPItem.ID;
            //        if (Head.VIP_VIPOffLIst == null)
            //            Head.VIP_VIPOffLIst = new List<API.VIP_VIPOffLIst>();
            //        Head.VIP_VIPOffLIst.Add(VIP);
            //    }
            //}
            #endregion 

            string scope = "";
            int index = 0;

            #region 营业厅

            List<API.Pro_HallInfo> HallSource = HallGrid.ItemsSource as List<API.Pro_HallInfo>;
            foreach (var item in HallSource)
            {
                index++;
                API.VIP_HallInfoHeader hall = new API.VIP_HallInfoHeader();
                hall.HallID = item.HallID;
                scope += item.HallName;
                if (index < HallSource.Count)
                {
                    scope += " , ";
                }
                if (head.VIP_HallInfoHeader == null)
                { head.VIP_HallInfoHeader = new List<API.VIP_HallInfoHeader>(); }

                head.VIP_HallInfoHeader.Add(hall);
            }

            #endregion

            #region 套餐及套餐明细

            head.VIP_OffListAduit = new List<API.VIP_OffListAduit>();
            foreach (var item in models)
            {
                //套餐
                API.VIP_OffListAduit m = new API.VIP_OffListAduit();
                m.Name = item.Name;
                m.Note = item.Note;
                m.ArriveMoney = item.Price;
                m.Type = item.GroupTypeID;
                m.UpdUser = Store.LoginUserInfo.UserID;
                m.Flag = true;
                m.VIPTicketMaxCount = (int)item.Limit;

                
                //添加仓库
                //m.VIP_HallOffInfo = new List<API.VIP_HallOffInfo>();
                //foreach (var hall in HallSource)
                //{
                //    API.VIP_HallOffInfo h = new API.VIP_HallOffInfo();
                //    h.HallID = hall.HallID;
                //    m.VIP_HallOffInfo.Add(h);
                //}
                //添加分组及商品明细
                m.Package_GroupInfo = new List<API.Package_GroupInfo>();
                foreach (var xxd in item.Children)
                {
                    API.Package_GroupInfo g = new API.Package_GroupInfo();
                    g.SellType = xxd.SellTypeID;
                    g.Note = xxd.Note;
                    g.IsMust = xxd.IsmustText == "否" ? false : true;
                    g.GroupID = xxd.GroupID;
                    g.GroupName = xxd.GroupName;

                    string subnote = "";
                    g.Package_ProInfo = new List<API.Package_ProInfo>();

                    var pros = from a in Store.ProNameInfo
                               join b in xxd.ProModel
                               on a.ID equals b.ProMainID
                               select new
                               {
                                   a.MainName,
                                   b.ProMainID,
                                   b.Salary,
                                   b.SellTypeID,
                                   b.Note
                               };
                    index = 0;
                    foreach (var dd in pros)
                    {
                        index++;
                        API.Package_ProInfo gg = new API.Package_ProInfo();
                        gg.ProMainNameID = dd.ProMainID;
                        gg.Salary = dd.Salary;
                        gg.SellType = dd.SellTypeID;
                        gg.Note = dd.Note;
                        g.Package_ProInfo.Add(gg);
                        subnote += dd.MainName;
                        if (index < pros.Count())
                        {
                            subnote += " / ";
                        }
                    }
                    g.SubNote = subnote;
                    m.Package_GroupInfo.Add(g);
                }
               head.VIP_OffListAduit.Add(m);
            }

            #endregion 

            head.Scope = scope;
            head.Applyer = applyer.Text;
            head.Creater = Store.LoginUserInfo.UserID;
            head.Destination = destination.Text.Trim();
            head.ApplyNote = applynote.Text.Trim();
            head.StartDate = startDate.SelectedDate;
            head.EndDate = ((DateTime)endDate.SelectedValue).AddDays(1).AddSeconds(-1);
            head.SaleTarget = saleTarget.Text.Trim();

            PublicRequestHelp h2 = new PublicRequestHelp(busy, 295, new object[] { head },Save_Completed);
        }

        protected void Save_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {
                if (e.Result.ReturnValue == true)
                {
                    Clear();
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

        void Clear()
        {

            this.endDate.SelectedValue = null;
            this.startDate.SelectedValue = null;
            this.destination.Text = string.Empty;
            this.saleTarget.Text = string.Empty;
            applynote.Text = string.Empty;
            applyer.Text = string.Empty;
            this.Name.Text = string.Empty;
            this.Note.Text = string.Empty;
            this.ReduceCash.Value = 0;
            this.CBLimit.IsChecked = false;
            LimitNum.IsEnabled = false;
            LimitNum.Value = 99999;

            models.Clear();
            headGrid.Rebind();
            HallGrid.ItemsSource = null;
            HallGrid.Rebind();
           
            GounpList.Clear();
            this.groupGrid.Rebind();

            ProGridOther.ItemsSource = null;
            ProGridOther.Rebind();
        }

        #endregion

        #region 获取模板
        private void RadMenuItem_Click_5(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.GetPackageSource, new object[] { }, GetSourceCompleted);

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
                                                         "Name", new string[] { "ID", "Name" },
                                                         new string[] { "优惠编号", "优惠名称" }, true);

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
                    PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.GetPackageSource, new object[] { selected.ID, "" }, ChangeSourceCompleted);
                }
            }

        }
        private void ChangeSourceCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.Obj != null&&results.Result.ReturnValue==true)
                {
                    Clear();
                    API.OffModel Off = results.Result.Obj as API.OffModel;
                    //获取表头
                    ReduceCash.Value = Convert.ToDouble(Off.ArriveMoney);
                    //StartTime.SelectedValue = Off.StartDate;
                    //EndTime.SelectedValue = Off.EndDate;
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
                    this.groupGrid.Rebind();
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
                        //dataGridVipType.ItemsSource = VIPTypeSource;
                        //dataGridVipType.Rebind();
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
                        //dataGridVip2.ItemsSource = VIPSource;
                        //dataGridVip2.Rebind();
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
                    //HallGrid.ItemsSource = HallSource;
                    //HallGrid.Rebind();
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
            Clear();
        }

        #region   套餐管理

        private string GetHeader(RadTreeViewItem item)
        {
            string header = item.Header.ToString();
            while (item.Parent != null)
            {
                item = item.Parent as RadTreeViewItem;
                if (item == null)
                {
                    break;
                }
                header = item.Header + " - " + header;
            }

            return header;
        }

        /// <summary>
        /// 添加套餐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPackage_Click(object sender, RoutedEventArgs e)
        {
            VIPOffModel Head = new VIPOffModel();
            Head.Name = this.Name.Text.Trim();
            foreach (var item in models)
            {
                if (item.Name == Head.Name)
                {
                    MessageBox.Show("已存在相同名称的套餐！");
                    return;
                }
            }


            if (TreeView.Items.Count == 0 || TreeView.SelectedItems.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择套餐分类");
                return;
            }
            else
            {
                RadTreeViewItem Item = TreeView.SelectedItem as RadTreeViewItem;
                Head.GroupTypeID = Convert.ToInt32(Item.Tag);
                Head.GroupType = GetHeader(Item);
            }

            Head.Price = (decimal)ReduceCash.Value; //套餐价格
            if (Head.Price <= 0) {
                MessageBox.Show("套餐价格无效！");
                return;
            }
            Head.Note = Note.Text.Trim();
            Head.Limit = (decimal)LimitNum.Value;
            if (GounpList.Count() <= 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请添加分组！");
                return;
            }
         
            var queryPro = from b in GounpList
                           where b.ProModel == null || b.ProModel.Count() == 0
                           select b;
            if (queryPro.Count()>0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "分组中的商品不能为空！");
                return;
            }
            if (MessageBox.Show("确定添加吗？") == MessageBoxResult.Cancel)
            {
                return;
            }

            Head.Children = new List<GounpSource>();
            foreach (var item in GounpList)
            {
                GounpSource g = CloneGroup(item);
                Head.Children.Add(g);
            }
            models.Add(Head);
            headGrid.Rebind();
        }

        private static GounpSource CloneGroup(GounpSource item)
        {
            GounpSource g = new GounpSource();
            g.IsmustText = item.IsmustText;
            g.GroupName = item.GroupName;
            g.GroupID = item.GroupID;
            g.GrounpNameList = item.GrounpNameList;
            g.Note = item.Note;
            g.sellType = item.sellType;
            g.SellTypeID = item.SellTypeID;
            g.ProModel = new List<API.ProModel>();
            if (item.ProModel!=null)
            {
                foreach (var Item in item.ProModel)
                {
                    API.ProModel mm = new API.ProModel();
                    mm.ProMainID = Item.ProMainID;
                    mm.ProName = Item.ProName;
                    mm.SellTypeID = item.SellTypeID;
                    mm.SellTypeName = item.SellTypeName;
                    g.ProModel.Add(mm);
                }
            }
            return g;
        }

        /// <summary>
        /// 修改套餐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdPackage_Click(object sender, RoutedEventArgs e)
        {
            if (headGrid.SelectedItem == null)
            {
                MessageBox.Show("请选择要修改的套餐！");
                return;
            }
            if (MessageBox.Show("确定修改吗？") == MessageBoxResult.Cancel)
            {
                return;
            }
            VIPOffModel model = headGrid.SelectedItem as VIPOffModel;
            model.Name = this.Name.Text.Trim();
            model.Note = this.Note.Text.Trim();
            model.Price = (decimal)ReduceCash.Value;
            model.Limit = (decimal)LimitNum.Value;
            foreach (var item in models)
            {
                if (item.Name.Equals(item.Name))
                {
                    model.Name = this.Name.Text.Trim();
                    model.Note = this.Note.Text.Trim();
                    model.Price = (decimal)ReduceCash.Value;
                    model.Limit = (decimal)LimitNum.Value;
                    break;
                }
            }
            groupGrid.Rebind();
            headGrid.Rebind();
        }

        /// <summary>
        /// 清空套餐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPackage_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定清空吗？") == MessageBoxResult.Cancel)
            {
                return;
            }
            headGrid.SelectedItem = null;
            this.Name.Text = string.Empty;
            this.ReduceCash.Value = 0;
            Note.Text = string.Empty;
            LimitNum.Value = 0;
            groupGrid.ItemsSource = null;
            GounpList.Clear();
            groupGrid.ItemsSource = GounpList;
            groupGrid.Rebind();
            ProGridOther.ItemsSource = null;
            ProGridOther.Rebind();
        }


        /// <summary>
        /// 套餐详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (headGrid.SelectedItem == null) { return; }
            VIPOffModel model = headGrid.SelectedItem as VIPOffModel;

            this.Name.Text = model.Name;
            this.ReduceCash.Value = (double)model.Price;
            this.Note.Text = model.Note;
         
            GounpList.Clear();
            //GounpList.AddRange(model.Children);
            foreach (var item in model.Children)
            {
                GounpSource g = CloneGroup(item);
                GounpList.Add(g);
            }
            groupGrid.Rebind();

            if (GounpList.Count > 0)
            {
                groupGrid.SelectedItem = GounpList[0];
            }
        }

        /// <summary>
        /// 删除套餐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelPac_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (headGrid.SelectedItem == null) { return; }
            VIPOffModel model = headGrid.SelectedItem as VIPOffModel;

            foreach (var item in models)
            {
                if (item.Name == model.Name)
                {
                    models.Remove(item);
                    break;
                }
            }
            headGrid.Rebind();

            this.Name.Text = string.Empty;
            this.ReduceCash.Value = 0;
            this.Note.Text = string.Empty;
           
            ProGridOther.ItemsSource = null;
            ProGridOther.Rebind();

            groupGrid.ItemsSource = null;
            GounpList.Clear();
            groupGrid.ItemsSource = GounpList;
        }

        #endregion
    }
}
