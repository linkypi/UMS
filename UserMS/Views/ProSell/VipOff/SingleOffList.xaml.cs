using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.VipOff
{
    public partial class SingleOffList : BasePage
    {
        API.ReportPagingParam pageParam;
        const int MethodID = 149;
        const int UpdateMethod = 151;
        const int DeleteMethod = 152;

        const int type = 0;


        List<API.VIP_OffList> OffID_List;
        public SingleOffList()
        {
            InitializeComponent();
            InitGrid2();
            //RbOffPoin.Visibility = Visibility.Collapsed;
            //OffPoint.Visibility = Visibility.Collapsed;
        }
        #region 生成列
        private void InitGrid2()
        {
            //优惠表头GridView
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("OffName");
            col.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col.Header = "优惠名称";
            this.dataGridOffList.Columns.Add(col);


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
            col41.DataMemberBinding = new System.Windows.Data.Binding("UpdUser");
            col41.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col41.Header = "创建人";
            this.dataGridOffList.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col5.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col5.Header = "备注";
            this.dataGridOffList.Columns.Add(col5);

            GridViewDataColumn col51 = new GridViewDataColumn();
            col51.DataMemberBinding = new System.Windows.Data.Binding("OffRate");
            col51.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col51.Header = "折扣";
            this.dataGridOffList.Columns.Add(col51);


            GridViewDataColumn col52 = new GridViewDataColumn();
            col52.DataMemberBinding = new System.Windows.Data.Binding("OffMoney");
            col52.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col52.Header = "减现金";
            this.dataGridOffList.Columns.Add(col52);

            GridViewDataColumn col6 = new GridViewDataColumn();
            col6.DataMemberBinding = new System.Windows.Data.Binding("OffPoint");
            col6.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col6.Header = "积分兑换";
            this.dataGridOffList.Columns.Add(col6);

            //会员类别GridView
            GridViewDataColumn VIPTypecol = new GridViewDataColumn();
            VIPTypecol.DataMemberBinding = new System.Windows.Data.Binding("VIPTypeName");
            //   VIPTypecol.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            VIPTypecol.Header = "会员类别";
            this.dataGridVipType.Columns.Add(VIPTypecol);

            //会员
            GridViewDataColumn VIPcol = new GridViewDataColumn();
            VIPcol.DataMemberBinding = new System.Windows.Data.Binding("IMEI");
            VIPcol.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            VIPcol.Header = "会员卡号";
            this.dataGridVip2.Columns.Add(VIPcol);

            GridViewDataColumn VIPcol1 = new GridViewDataColumn();
            VIPcol1.DataMemberBinding = new System.Windows.Data.Binding("VIPName");
            VIPcol1.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            VIPcol1.Header = "会员姓名";
            this.dataGridVip2.Columns.Add(VIPcol1);

            //营业厅
            GridViewDataColumn Hallcol = new GridViewDataColumn();
            Hallcol.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            Hallcol.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            Hallcol.Header = "营业厅";
            this.HallGrid.Columns.Add(Hallcol);

        }
        #endregion

        #region 查询
        private void BtSearch_Click_1(object sender, RoutedEventArgs e)
        {
            SearchOff();
        }
        void SearchOff()
        {
            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.RadPager.PageIndex;
            pageParam.PageSize = this.RadPager.PageSize;
            pageParam.ParamList = new List<API.ReportSqlParams>();
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
                CraetName.ParamName = "OffUpdUser";
                CraetName.ParamValues = this.TbCreatName.Text.Trim();
                pageParam.ParamList.Add(CraetName);
            }

            if (pageParam.ParamList.Count() > 0)
            {
                CancelAll();
                this.InitPageEntity(MethodID, this.dataGridOffList, this.busy, this.RadPager, pageParam, "0");
            }
        }
        #endregion
        #region 取下一页数据
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取下一页的数据

            pageParam.PageIndex = e.NewPageIndex;
            this.InitPageEntity(MethodID, this.dataGridOffList, this.busy, this.RadPager, pageParam, "0");
            #endregion
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
                        dataGridVipType.ItemsSource = OffList.VIPTypeModel;
                        dataGridVip2.ItemsSource = OffList.VIPModel;
                        HallGrid.ItemsSource = OffList.HallModel;
                        ProGrid.ItemsSource = OffList.ProModel;
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
        private void dataGridOffList_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            if (this.dataGridOffList.SelectedItems.Count > 1)
            {
                CancelAll();
                return;
            }
            API.View_OffList offList = this.dataGridOffList.SelectedItem as API.View_OffList;
            if (offList == null)
            {
                CancelAll();//清空数据
                return;
            }
           
            OffListContent.DataContext = offList;
            PublicRequestHelp h = new PublicRequestHelp(busy, MethodID, new object[] { offList.OffID,true }, MyClient_Completed);
     
        }

        private void ManyDel_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            OffID_List = new List<API.VIP_OffList>();
            foreach (var Item in dataGridOffList.SelectedItems)
            {
                API.View_OffList list = Item as API.View_OffList;
                API.VIP_OffList Off = new API.VIP_OffList();
                Off.ID = list.OffID;
                //  Off.Type = 0;
                Off.StartDate = list.StartDate;
                Off.EndDate = list.EndDate;
                OffID_List.Add(Off);
            }
            if (OffID_List == null || OffID_List.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择任何项");
                return;
            }

            //var query1 = from b in OffID_List
            //             where b.UnOver == true && b.EndDate >= DateTime.Now
            //             select b;
            //if (query1.Count() > 0)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"该活动正在进行不能进行其它操作！");
            //    return;
            //}
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定删除选中优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(busy, DeleteMethod, new object[] { OffID_List }, Sumbit_Completed);
        }

        private void ManyStart_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            OffID_List = new List<API.VIP_OffList>();
            foreach (var Item in dataGridOffList.SelectedItems)
            {
                API.View_OffList list = Item as API.View_OffList;
                
                API.VIP_OffList Off = new API.VIP_OffList();
                Off.ID = list.OffID;
                //  Off.Type = 0;
                Off.StartDate = list.StartDate;
                Off.EndDate = list.EndDate;
                OffID_List.Add(Off);
            }

            if (OffID_List == null || OffID_List.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择任何项");
                return;
            }

            var query1 = from b in OffID_List
                         where b.UnOver == true && b.EndDate >= DateTime.Now
                         select b;
            if (query1.Count() > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该活动正在进行不能进行其它操作！");
                return;
            }
            var query = from b in OffID_List
                        where b.EndDate <= DateTime.Now || b.StartDate <= DateTime.Now
                        select b;
            if (query.Count() > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"活动开始时间和结束时间必须大于当前日期！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定重新启用选中优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(busy, UpdateMethod, new object[] { OffID_List }, Sumbit_Completed);
        }
        void CancelAll()
        {
            OffListContent.DataContext = null;
            dataGridVipType.ItemsSource = null;
            dataGridVipType.Rebind();
            dataGridVip2.ItemsSource = null;
            dataGridVip2.Rebind();
            HallGrid.ItemsSource = null;
            HallGrid.Rebind();
            ProGrid.ItemsSource = null;
            ProGrid.Rebind();
        }
        protected void Sumbit_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {               
                    Logger.Log(e.Result.Message + "");
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message); 
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                }
                finally
                {
                    this.busy.IsBusy = false;
                    if (e.Result.ReturnValue == true)
                    {
                        SearchOff();
                    }
                }

            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错");
        }
    }
}
