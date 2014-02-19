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

namespace UserMS.Views.HallOff
{
    /// <summary>
    /// HallOffMS.xaml 的交互逻辑
    /// </summary>
    public partial class HallOffMS : BasePage
    {
        API.ReportPagingParam pageParam;
        List<API.SeleterModel> vmodels = new List<API.SeleterModel>();
        private Mul_HallFilter hAdder;
        public HallOffMS()
        {
            InitializeComponent();
            this.ProGrid.ItemsSource = vmodels;
            hAdder = new Mul_HallFilter(ref HallGrid);
            InitGrid2();
        }
        #region 生成列
        private void InitGrid2()
        {
            //优惠表头GridView
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("Name");
            col.Header = "店面优惠名称";
            this.dataGridOffList.Columns.Add(col);


            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("StartTime");
            col2.Header = "开始时间";
            this.dataGridOffList.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("EndTime");
            col3.Header = "结束时间";
            this.dataGridOffList.Columns.Add(col3);


            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("RealName");
            col41.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col41.Header = "创建人";
            this.dataGridOffList.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col5.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col5.Header = "备注";
            this.dataGridOffList.Columns.Add(col5);


            //营业厅
            GridViewDataColumn Hallcol = new GridViewDataColumn();
            Hallcol.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            Hallcol.Header = "营业厅";
            this.HallGrid.Columns.Add(Hallcol);

        }
        #endregion
        #region 查询
        private void BtSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchOff();
        }
        private  void SearchOff()
        {
            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.RadPager.PageIndex;
            pageParam.PageSize = this.RadPager.PageSize;
            pageParam.ParamList = new List<API.ReportSqlParams>();
            //优惠名称查询
            if (!string.IsNullOrEmpty(this.TbOffName.Text))
            {
                API.ReportSqlParams_String OffName = new API.ReportSqlParams_String();
                OffName.ParamName = "Name";
                OffName.ParamValues = TbOffName.Text.Trim();
                pageParam.ParamList.Add(OffName);
            }
            //证件号码查询
            if (this.CbOffFlag.SelectedIndex == 0)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "Flag";
                Flag.ParamValues = "All";
                pageParam.ParamList.Add(Flag);
                //   NoteText="全部";
            }
            if (this.CbOffFlag.SelectedIndex == 1)
            {
                API.ReportSqlParams_String Flag = new API.ReportSqlParams_String();
                Flag.ParamName = "Flag";
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
                Flag.ParamName = "Flag";
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
                CancelAll();
                this.InitPageEntity(MethodIDStore.HallOffSearch, this.dataGridOffList, this.busy, this.RadPager, pageParam);
            }
        }
        private void  CancelAll()
        {
            HallGrid.ItemsSource = null;
            HallGrid.Rebind();
            vmodels.Clear();
            ProGrid.Rebind();
        }
        #endregion
        #region 更改行数
        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (pageParam != null)
            {
                RadPager.PageSize = (int)e.NewValue;
                pageParam.PageSize = (int)e.NewValue;
                if (pageParam.ParamList.Count() > 0)
                {
                    CancelAll();
                    this.InitPageEntity(MethodIDStore.HallOffSearch, this.dataGridOffList, this.busy, this.RadPager, pageParam);
                }
            }

        }
        #endregion
        #region 获取明细
        private void dataGridOffList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {

            if (this.dataGridOffList.SelectedItems.Count > 1)
            {
                CancelAll();
                return;
            }
            API.View_Off_AduitTypeInfo offList = this.dataGridOffList.SelectedItem as API.View_Off_AduitTypeInfo;
            if (offList == null)
            {
                CancelAll();//清空数据
                return;
            }

            OffListContent.DataContext = offList;
            PublicRequestHelp h = new PublicRequestHelp(busy, MethodIDStore.HallOffSearch, new object[] { offList.ID }, MyClient_Completed);
        }
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {
                if (e.Result.ReturnValue == true)
                {
                    API.OffModel OffList = e.Result.Obj as API.OffModel;
                    if (OffList != null)
                    {
                        List<API.Pro_HallInfo> HallList = new List<API.Pro_HallInfo>();
                        foreach (var HallItem in OffList.HallModel)
                        {
                            API.Pro_HallInfo Hall = new API.Pro_HallInfo();
                            Hall.HallID = HallItem.HallID;
                            Hall.HallName = HallItem.HallName;
                            HallList.Add(Hall);
                        }
                        HallGrid.ItemsSource = HallList;
                        vmodels.Clear();
                        foreach (var Item in OffList.ProModel)
                        {
                            API.SeleterModel Pro = new API.SeleterModel();
                            Pro.ProID = Item.ProID.ToString();
                            Pro.ProClassName = Item.ProClassName;
                            Pro.ProTypeName = Item.ProTypeName;
                            Pro.ProName = Item.ProName;
                            Pro.SellTypeName = Item.SellTypeName;
                            Pro.OffPrice = Item.OffPrice;
                            Pro.SellTypeID = Item.SellTypeID;
                            vmodels.Add(Pro);
                        }
                        ProGrid.Rebind();

                    }
                }
                Logger.Log(e.Result.Message + "");
            }
            catch (Exception ex)
            {
                Logger.Log("获取失败！");
            }
            finally
            {
                this.busy.IsBusy = false;
            }
        }
        #endregion

   
        #region 商品操作
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Mul_ProductFileter ProFilter = new Mul_ProductFileter(ref vmodels, ref ProGrid, null);
            ProFilter.ProFilter(null);
        }

        private void DelPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelCheckedPro(ref vmodels, ref ProGrid);
        }

        private void ProGrid_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            API.SeleterModel ProItem = this.ProGrid.SelectedItem as API.SeleterModel;
            if (ProItem.OffPrice < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "门店优惠限额不能为负数");
                ProItem.OffPrice = 0;
                return;
            }
            if (e.Cell.Column.Header.ToString() == "门店优惠限额")
            {
                int value = (int)(ProItem.OffPrice * 100);
                ProItem.OffPrice = (decimal)(value * 0.01);
            }
        }
        #endregion

        #region 门店操作
        private void RadMenuItem_Click_3(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            hAdder.GetHall(Store.ProHallInfo);
        }
        private void DelHall_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelHall(ref HallGrid);
        }
        #endregion
        #region 删除操作
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.View_Off_AduitTypeInfo> Aduit = new List<API.View_Off_AduitTypeInfo>();
           
            if (this.dataGridOffList.SelectedItems == null || this.dataGridOffList.SelectedItems.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择要删除的门店优惠！");
                return;
            }
            foreach(var Item in this.dataGridOffList.SelectedItems)
            {
              Aduit.Add(Item as API.View_Off_AduitTypeInfo);
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定删除所选门店优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            List<int> AduitIDList = (from b in Aduit
                                    select b.ID).ToList();
            if (Aduit.Count() != AduitIDList.Count())
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "获取ID出错！");
                return;
            }
            PublicRequestHelp h = new PublicRequestHelp(busy, MethodIDStore.AddHallOff, new object[] { AduitIDList }, MyClient_Completed1);
        }
        #endregion
        #region 保存修改
        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.View_Off_AduitTypeInfo Aduit = this.dataGridOffList.SelectedItem as API.View_Off_AduitTypeInfo;
            if (Aduit == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择要修改的门店优惠！");
                return;
            }
            API.Off_AduitTypeInfo AduitType = new API.Off_AduitTypeInfo();
            AduitType.Name = this.Name.Text.Trim();
            if (string.IsNullOrEmpty(AduitType.Name)) MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入门店优惠名称");
            AduitType.Note = this.Note.Text.Trim();
            if (StartTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请填写开始时间！");
                return;
            }
            AduitType.StartDate = (DateTime)StartTime.SelectedValue;
            if (StartTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请填写结束时间！");
                return;
            }
            AduitType.EndDate = (DateTime)EndTime.SelectedValue;
            //添加商品
            AduitType.Off_AduitProInfo = new List<API.Off_AduitProInfo>();
            if (vmodels.Count == 0) { MessageBox.Show(System.Windows.Application.Current.MainWindow, "未添加商品"); return; }
            foreach (var ProItem in vmodels)
            {
                API.Off_AduitProInfo AduitPro = new API.Off_AduitProInfo() { ProMainID = int.Parse(ProItem.ProID), SellType = ProItem.SellTypeID, Price = ProItem.OffPrice };
                AduitType.Off_AduitProInfo.Add(AduitPro);
            }
            //添加门店
            AduitType.Off_AduitHallInfo = new List<API.Off_AduitHallInfo>();
            List<API.Pro_HallInfo> HallList = HallGrid.ItemsSource as List<API.Pro_HallInfo>;
            if (HallList == null || HallList.Count == 0) { MessageBox.Show(System.Windows.Application.Current.MainWindow, "未添加营业厅"); return; }
            foreach (var HallItem in HallList)
            {
                API.Off_AduitHallInfo AduitHall = new API.Off_AduitHallInfo() { HallID = HallItem.HallID, };
                AduitType.Off_AduitHallInfo.Add(AduitHall);
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定修改门店优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp h = new PublicRequestHelp(busy, MethodIDStore.AddHallOff, new object[] { Aduit.ID,AduitType}, MyClient_Completed1);
        }
        protected void MyClient_Completed1(object sender, API.MainReportCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                Logger.Log(e.Result.Message + "");
                MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message);
                if (e.Result.ReturnValue == true)
                {
                    if (pageParam.ParamList.Count() > 0)
                    {
                        CancelAll();
                        this.InitPageEntity(MethodIDStore.HallOffSearch, this.dataGridOffList, this.busy, this.RadPager, pageParam);
                    }
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "返回值无法获取！");
            }
        }
        #endregion
        private void RadPager_PageIndexChanging(object sender, Telerik.Windows.Controls.PageIndexChangingEventArgs e)
        {
            #region 取下一页的数据
            pageParam.PageIndex = e.NewPageIndex;
            if (pageParam.ParamList.Count() > 0)
            {
                CancelAll();
                this.InitPageEntity(MethodIDStore.HallOffSearch, this.dataGridOffList, this.busy, this.RadPager, pageParam);
            }
            #endregion
        }

       
    }
}
