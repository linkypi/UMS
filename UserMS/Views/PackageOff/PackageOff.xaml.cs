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
    /// PackageOff.xaml 的交互逻辑
    /// </summary>
    public partial class PackageOff : BasePage
    {
        API.ReportPagingParam pageParam;
        private bool flag = false;
        private int pageindex = 0;

        private List<GounpSource> GounpList = new List<GounpSource>();      
        public PackageOff()
        {
            InitializeComponent();
            flag = true;
            InitGrid2();

            List<API.Package_SalesNameInfo> list = new List<API.Package_SalesNameInfo>();
            foreach (var item in Store.PacSalesNameInfo)
            {
                API.Package_SalesNameInfo p = new API.Package_SalesNameInfo();
                p.ID = item.ID;
                p.Parent = item.Parent;
                p.SalesName = item.SalesName;
                list.Add(p);
            }
            API.Package_SalesNameInfo p2 = new API.Package_SalesNameInfo();
            p2.ID = -1;
            p2.SalesName = "全部";
            list.Add(p2);
            cbPacType.ItemsSource = list;
            if (list.Count > 0)
            {
                cbPacType.SelectedIndex = list.Count - 1;
            }
            PageGrid.ItemsSource = GounpList;
            this.PageGrid.AddHandler(RadComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(GridViewComboBoxColumn_PropertyChanged_1));
            PublicRequestHelp pqh = new PublicRequestHelp(null, 264, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetGroupTypeCompleted));
        
        }

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
            GridViewComboBoxColumn comcol1 = PageGrid.Columns[1] as GridViewComboBoxColumn;
            List<API.View_PackageGroupTypeInfo> list = comcol1.ItemsSource as List<API.View_PackageGroupTypeInfo>;
            if (list != null)
            {
                ((GounpSource)comb.DataContext).GroupID = list[comb.SelectedIndex].ID;
            }
            else
            {
                ((GounpSource)comb.DataContext).GroupID = -1;
            }
        }

        private void GetGroupTypeCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Result.ReturnValue)
            {
                List<API.View_PackageGroupTypeInfo> list = e.Result.Obj as List<API.View_PackageGroupTypeInfo>;
                if (list != null)
                {
                    GridViewComboBoxColumn comcol1 = PageGrid.Columns[1] as GridViewComboBoxColumn;
                    comcol1.ItemsSource = list;
                }
            }
        }


        #region 生成列
        private void InitGrid2()
        {
            GridViewSelectColumn c = new GridViewSelectColumn();
            this.dataGridOffList.Columns.Add(c);
            //id
            GridViewDataColumn colID = new GridViewDataColumn();
            colID.DataMemberBinding = new System.Windows.Data.Binding("OffID");
            colID.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            colID.Header = "套餐编码";
            this.dataGridOffList.Columns.Add(colID);

            //优惠表头GridView
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("OffName");
            col.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col.Header = "优惠名称";
            this.dataGridOffList.Columns.Add(col);

            GridViewDataColumn sacol = new GridViewDataColumn();
            sacol.DataMemberBinding = new System.Windows.Data.Binding("SalesName");
            sacol.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            sacol.Header = "分类名称";
            this.dataGridOffList.Columns.Add(sacol);

            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("StartTime");
            col2.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col2.Header = "开始时间";
            this.dataGridOffList.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("EndTime");
            col3.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col3.Header = "结束时间";
            this.dataGridOffList.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("VIPTicketMaxCount");
            col4.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col4.Header = "限制名额";
            this.dataGridOffList.Columns.Add(col4);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("RealName");
            col41.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col41.Header = "创建人";
            this.dataGridOffList.Columns.Add(col41);

            GridViewDataColumn col42 = new GridViewDataColumn();
            col42.DataMemberBinding = new System.Windows.Data.Binding("State");
            col42.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col42.Header = "活动状态";
            this.dataGridOffList.Columns.Add(col42);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col5.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col5.Header = "备注";
            this.dataGridOffList.Columns.Add(col5);       
        }
        #endregion

        #region 查询
        private void BtSearch_Click_1(object sender, RoutedEventArgs e)
        {
            SearchOff();
        }
        void SearchOff()
        {
            if (!flag)
            {
                return;
            }
            CancelPart();

            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.RadPager.PageIndex;
            pageParam.PageSize = (int)this.pagesize.Value;
            pageParam.ParamList = new List<API.ReportSqlParams>();
            if ((int)cbPacType.SelectedValue!=-1)
            {
                API.ReportSqlParams_String type = new API.ReportSqlParams_String();
                type.ParamName = "Type";
                type.ParamValues = cbPacType.SelectedValue.ToString();
                pageParam.ParamList.Add(type);
            }
            //优惠名称查询
            if (!string.IsNullOrEmpty(this.TbOffName.Text))
            {
                API.ReportSqlParams_String OffName = new API.ReportSqlParams_String();
                OffName.ParamName = "OffName";
                OffName.ParamValues = TbOffName.Text.Trim();
                pageParam.ParamList.Add(OffName);
            }
            //证件号码查询
            if (this.CbOffFlag.SelectedIndex == 0)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "OffFlag";
                Flag.ParamValues = "All";
                pageParam.ParamList.Add(Flag);
                //   NoteText="全部";
            }
            if (this.CbOffFlag.SelectedIndex == 1)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "OffFlag";
                Flag.ParamValues = "Now";
                pageParam.ParamList.Add(Flag);
                //   NoteText="正在进行";
            }
            if (this.CbOffFlag.SelectedIndex == 2)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "OffFlag";
                Flag.ParamValues = "NoStart";
                pageParam.ParamList.Add(Flag);
                //NoteText="未开始";
            }
            if (this.CbOffFlag.SelectedIndex == 3)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "OffFlag";
                Flag.ParamValues = "End";
                pageParam.ParamList.Add(Flag);
                //NoteText="已结束";
            }
            //创建人
            if (!string.IsNullOrEmpty(this.TbCreatName.Text))
            {
                API.ReportSqlParams_String CraetName = new API.ReportSqlParams_String();
                CraetName.ParamName = "RealName";
                CraetName.ParamValues = this.TbCreatName.Text.Trim();
                pageParam.ParamList.Add(CraetName);
            }

            if (pageParam.ParamList.Count() > 0)
            {
                //CancelAll();
               // this.InitPageEntity(MethodIDStore.GetPackageSource, this.dataGridOffList, this.busy, this.RadPager, pageParam);
             
                PublicRequestHelp h = new PublicRequestHelp(this.busy, MethodIDStore.GetPackageSource, new object[] { pageParam },new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
            
            }
        }

        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }

            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.RadPager.PageIndexChanged -= RadPager_PageIndexChanged;
            this.RadPager.Source = pcv1;
            this.RadPager.PageIndexChanged += RadPager_PageIndexChanged;

            dataGridOffList.ItemsSource = null;
            dataGridOffList.Rebind();
            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_OffList> aduitList = pageParam.Obj as List<API.View_OffList>;
                if (aduitList != null)
                {
                    API.ReportPagingParam pageParem = (API.ReportPagingParam)e.Result.Obj;

                    this.dataGridOffList.ItemsSource = pageParem.Obj;
              
                      dataGridOffList.Rebind();
                    this.RadPager.PageSize = (int)pagesize.Value;
                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);

                    this.RadPager.PageIndexChanged -= RadPager_PageIndexChanged;
                    this.RadPager.Source = pcv;
                    this.RadPager.PageIndexChanged += RadPager_PageIndexChanged;
                    this.RadPager.PageIndex = pageindex;
                }
            }
        }
        #endregion

        #region 获取详情
        private void dataGridOffList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            API.View_OffList OffList = dataGridOffList.SelectedItem as API.View_OffList;
            if (OffList == null || dataGridOffList.SelectedItems.Count()!=1)
            {
                CancelPart();
                return;
            }
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.GetPackageSource, 
                new object[] { OffList.OffID, "" }, ChangeSourceCompleted);
        }
        private void ChangeSourceCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.Obj != null && results.Result.ReturnValue == true)
                {
                    CancelPart();
                    API.OffModel Off = results.Result.Obj as API.OffModel;
                    //获取表头
                    //ReduceCash.Value = Convert.ToDouble(Off.ArriveMoney);
                    //StartTime.SelectedValue = Off.StartDate;
                    //EndTime.SelectedValue = Off.EndDate;
                    //LimitNum.Text = Off.VIPTicketMaxCount.ToString() ;
                    //Note.Text = Off.Note;
              
                    //获取分组和商品 
                    GounpList.Clear();
                    GridContent.DataContext = Off;
                    ReduceCash.Text = Convert.ToDouble(Off.ArriveMoney).ToString();
                    Note.Text = Off.Note;
                    PackageType.Text = Off.SalesName;
                    GridViewComboBoxColumn comcol1 = PageGrid.Columns[1] as GridViewComboBoxColumn;
                    List<API.View_PackageGroupTypeInfo> list = comcol1.ItemsSource as List<API.View_PackageGroupTypeInfo>;

                    foreach (var Item in Off.GrounpInfo)
                    {
                        GounpSource GounpItem = new GounpSource();
                        if (list != null)
                        {
                            GounpItem.GroupID = list[0].ID;
                        }
                        GounpItem.GroupName = Item.GrounpName;
                        GounpItem.IsmustText = Item.IsMustName;
                        GounpItem.Note = Item.Note;
                        GounpItem.SellTypeID = Item.SellType;
                        GounpItem.SellTypeName = Item.SellTypeName;
                        GounpItem.ProModel = new List<API.ProModel>();
                        GounpItem.ProModel.AddRange(Item.ProModel);
                        GounpList.Add(GounpItem);
                    }
                    //if (GounpList.Count() > 0)
                    //{
                    //    PageGrid.SelectedItem = GounpList[0];
                    //}
                    this.PageGrid.Rebind();
                    //获取会员类别
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
                    }
                    dataGridVip2.ItemsSource = VIPSource;
                    dataGridVip2.Rebind();
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

        #region 清空数据
        void CancelPart()
        {
            //this.EndTime.SelectedValue = null;
            //this.StartTime.SelectedValue = null;
            //this.Name.Text = string.Empty;
            this.Note.Text = string.Empty;
            //this.ReduceCash.Value = 0;
            GridContent.DataContext = null;
            ReduceCash.Text = string.Empty;
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

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            List<API.View_OffList> OffList = new List<API.View_OffList>();
            foreach (var Item in dataGridOffList.SelectedItems)
            {
                OffList.Add(Item as API.View_OffList);   
            }
            if (OffList.Count()==0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择删除项");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定删除？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.DeletePackage, new object[] { OffList }, GetSourceCompleted);
        }
        private void GetSourceCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue==true)
                {
                    CancelPart();
                    SearchOff(); 
                }
            }
            else
                Logger.Log("无数据");
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
                for (int i = 0; i < queryProNameList.Count(); i++)
                {
                    GrounpItem.ProModel[i].ProName = queryProNameList[i];
                }
            }
            this.ProGridOther.Rebind();
        }

        private void RadPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            SearchOff();
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            SearchOff();
        }

        private void stop_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<int> list = new List<int>();
            foreach (var item in dataGridOffList.SelectedItems)
            {
                API.View_OffList mod = item as API.View_OffList;
                //if (mod.OffFlag == "已结束")
                //{
                //    MessageBox.Show("套餐" + mod.Note + "已结束！");
                //    return;
                //}
                list.Add(mod.OffID);
            }
            StopWindow sw = new StopWindow(290, list);
            sw.OnSearch += sw_OnSearch;
            sw.ShowDialog();
        }

        void sw_OnSearch()
        {
            SearchOff();
        }

        private void TbOffName_KeyUp(object sender, KeyEventArgs e)
        {
            SearchOff();  
        }
    }
}
