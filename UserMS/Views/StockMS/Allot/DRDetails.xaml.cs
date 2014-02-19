using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.Allot
{
    public partial class DRDetails :BasePage
    {
       public int MethodID = 16;
        API.ReportPagingParam pageParam;//全局变量分页内容
        HallFilter hAdder;//全局变量（参数添加器）
        string r = "";
        List<API.Pro_HallInfo> hall;
        public DRDetails()
        {
            InitializeComponent();
            InitGrid2();
            GHHall.TextBox.IsEnabled = false;
            SHHall.TextBox.IsEnabled = false;
            this.TBSearch.Click += TBSearch_Click;

            this.StartTime.SelectedValue =DateTime.Today;
            TimeSpan span = new TimeSpan(23, 59, 59);
            this.EndTime.SelectedValue = DateTime.Today.Add(span);
         
            #region 添加参数事件
            //添加搜索参数
            this.GHHall.TextBox.IsEnabled = false;
            this.SHHall.TextBox.IsEnabled = false;
            this.GHHall.SearchButton.Click += SearchButton_Click;
            this.SHHall.SearchButton.Click += SearchButton1_Click;
            #endregion
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];

            }
            catch
            {
                r = "12";
            }
            finally
            {
                GetFirstHall();
                Search();
            }
        }
        private void GetFirstHall()
        {
            hAdder = new HallFilter(true, ref this.SHHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hall = hAdder.FilterHall(int.Parse(r.Trim()), HallInfo);
            if (hall.Count == 0)
            {
                return;
            }
           // this.SHHall.TextBox.SearchText = hall.First().HallName;
           // this.SHHall.Tag = hall.First().HallID;
        }

        
        #region 添加查询参数
        /// <summary>
        /// 增加类型参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        void SearchButton1_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(true, ref this.SHHall);
            if (hall == null)
                hall = new List<API.Pro_HallInfo>();
            hAdder.GetHall(hall);
        }
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(true, ref this.GHHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hAdder.GetHall(HallInfo);
        }
        #endregion
        #region 分割字符串
        public string[] GetHall_Item(string HallList)
        {
            string[] HallItem = HallList.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return HallItem;
        }
        #endregion
        #region 执行查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TBSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
        void Search()
        {
            RadPager.PageSize = (int)this.pagesize.Value;
            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.RadPager.PageIndex;
            pageParam.PageSize =(int)this.pagesize.Value;
            pageParam.ParamList = new List<API.ReportSqlParams>();
            if (!string.IsNullOrEmpty(this.CboAccept.Text))
            {
                API.ReportSqlParams_String Aduited = new API.ReportSqlParams_String();
                Aduited.ParamName = "Aduit";
                if(this.CboAccept.Text.Trim()=="已接收")
                    Aduited.ParamValues = "Y";
                if (this.CboAccept.Text.Trim() == "未接收")
                    Aduited.ParamValues = "N";
                if (this.CboAccept.Text.Trim() == "全部")
                    Aduited.ParamValues = "All";
                pageParam.ParamList.Add(Aduited);
            }
            //供货仓库
            if (!string.IsNullOrEmpty(this.GHHall.TextBox.SearchText))
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "FromHallName";
                if (hall.ParamValues == null)
                    hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(GetHall_Item(this.GHHall.TextBox.SearchText));
                pageParam.ParamList.Add(hall);
            }
            //收货仓库
            //if (string.IsNullOrEmpty(this.SHHall.TextBox.SearchText))
            //{
            //   // MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择收货仓库！");
            //    return;
            //}
            if (!string.IsNullOrEmpty(this.SHHall.TextBox.SearchText))
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "Pro_HallName";
                if (hall.ParamValues == null)
                    hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(GetHall_Item(this.SHHall.TextBox.SearchText));
                pageParam.ParamList.Add(hall);
            }

            if (!string.IsNullOrEmpty(this.txt_OrderID.Text))
            {
                API.ReportSqlParams_String orderID = new API.ReportSqlParams_String();
                orderID.ParamName = "OutOrderID";
                orderID.ParamValues = this.txt_OrderID.Text.Trim();
                pageParam.ParamList.Add(orderID);
            }
            if (!string.IsNullOrEmpty(this.StartTime.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime startTime = new API.ReportSqlParams_DataTime();
                startTime.ParamName = "SysDate_start";
                startTime.ParamValues = this.StartTime.SelectedValue;
                pageParam.ParamList.Add(startTime);
            }

            if (!string.IsNullOrEmpty(this.EndTime.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime endTime = new API.ReportSqlParams_DataTime();
                endTime.ParamName = "SysDate_end";
                endTime.ParamValues = this.EndTime.SelectedValue;
                pageParam.ParamList.Add(endTime);
            }

            if (!string.IsNullOrEmpty(this.txt_Transtor.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "UserName";
                users.ParamValues = this.txt_Transtor.Text;
                pageParam.ParamList.Add(users);
            }
            this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
        }
        #endregion
        
        #region 生成列
        private void InitGrid2()
        {
            #region 列
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("OutOrderID");
            col.Header = "调拨单号";
            this.dataGrid1.Columns.Add(col);


            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("OldID");
            col2.Header = "原始单号";
            this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("Aduit");
            col3.Header = "接收状态";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("FromHallName");
            col4.Header = "调出仓库";
            this.dataGrid1.Columns.Add(col4);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("Pro_HallName");
            col41.Header = "调入仓库";
            this.dataGrid1.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("FromUserName");
            col5.Header = "录单员";
            this.dataGrid1.Columns.Add(col5);

            GridViewDataColumn col51 = new GridViewDataColumn();
            col51.DataMemberBinding = new System.Windows.Data.Binding("OutDate");
            col51.DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}";
            col51.Header = "出库日期";
            this.dataGrid1.Columns.Add(col51);


            GridViewDataColumn col7 = new GridViewDataColumn();
            col7.DataMemberBinding = new System.Windows.Data.Binding("ToUserName");
            col7.Header = "收货员";
            this.dataGrid1.Columns.Add(col7);

            GridViewDataColumn col71 = new GridViewDataColumn();
            col71.DataMemberBinding = new System.Windows.Data.Binding("NewToDate");
            col71.Header = "接收时间";
            this.dataGrid1.Columns.Add(col71);

            //GridViewDataColumn col8 = new GridViewDataColumn();
            //col8.DataMemberBinding = new System.Windows.Data.Binding("UserName");
            //col8.Header = "操作员";
            //this.dataGrid1.Columns.Add(col8);


            //GridViewDataColumn col81 = new GridViewDataColumn();
            //col81.DataMemberBinding = new System.Windows.Data.Binding("SysDate");
            //col81.Header = "系统时间";
            //this.dataGrid1.Columns.Add(col81);


            //GridViewDataColumn col9 = new GridViewDataColumn();
            //col9.DataMemberBinding = new System.Windows.Data.Binding("DeleterName");
            //col9.Header = "取消操作人";
            //this.dataGrid1.Columns.Add(col9);


            //GridViewDataColumn col91 = new GridViewDataColumn();
            //col91.DataMemberBinding = new System.Windows.Data.Binding("DeleteDate");
            //col91.Header = "取消时间";
            //this.dataGrid1.Columns.Add(col91);

            #endregion

            //#region 取第一页的数据
            //if (pageParam == null)
            //{
            //    pageParam = new API.ReportPagingParam()
            //   {
            //       PageIndex = 0,
            //       PageSize = this.RadPager.PageSize,
            //       ParamList = new List<API.ReportSqlParams>()
            //   }; 
            //   this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            //}
          
            //#endregion

        }
        #endregion

        #region 下一页事件
        /// <summary>
        /// 下一页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取下一页的数据
            pageParam.PageIndex = e.NewPageIndex;
            this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion 

        #region 获取单据明细
        private void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            API.Pro_OutModel outModel = this.dataGrid1.SelectedItem as API.Pro_OutModel;
            CleanAll();
            if (outModel == null)
                return;
            //如果该单已经接收，则赋值给判断变量
            ContentSource.DataContext = outModel;
            if (outModel.Aduit == "Y")
            {
                Aduti.Text = "已接收";
            }
            else
                Aduti.Text = "未接收";

            if (outModel.OutDate != null)
                date.SelectedValue = outModel.OutDate;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.Method_List, new object[] { outModel.ID }, BackOutList);
        }
        private void BackOutList(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                //    outList = new List<API.View_OutOrderList>();
                //    outList = e.Result.Obj as List<API.View_OutOrderList>;
                    this.dataGrid11.ItemsSource = e.Result.Obj;
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常");
            }
        }
        #endregion
        #region 显示串码
        private void dataGrid11_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            API.View_OutOrderList model = dataGrid11.SelectedItem as API.View_OutOrderList;
            if (model == null) return;
            if (model.NeedIMEI == true)
            {
                PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.Method_CM, new object[] { model.OutListID }, BackOutIMEI);
            }
            else
            {
                this.dataGrid2.ItemsSource = null;
                dataGrid2.Rebind();
            }
        }
        private void BackOutIMEI(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    this.dataGrid2.ItemsSource = e.Result.Obj;
                    dataGrid2.Rebind();
                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常");
            }
        }
        #endregion
        #region 接收操作
        void Accept()
        {

            API.Pro_OutModel outModel = this.dataGrid1.SelectedItem as API.Pro_OutModel;
            if (outModel == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要接收的调拨单！");
                return;
            }
            if (outModel.Aduit == "Y")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"调拨单已接收，无法再次接受！");
                return;
            }
            List<API.View_OutOrderList> outList = dataGrid11.ItemsSource as List<API.View_OutOrderList>;
            if (outList == null || outList.Count==0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无法接收，请联系管理员");
                return;
            }
            API.Pro_OutInfo model = new API.Pro_OutInfo();
            model.OutOrderID = outModel.OutOrderID;
            model.ID = outModel.ID;
            model.Pro_HallID = outModel.Pro_HallID;
            //model.Pro_OutOrderList = new List<API.Pro_OutOrderList>();
            //foreach (var index in outList)
            //{
            //    API.Pro_OutOrderList ListModel = new API.Pro_OutOrderList();
            //    ListModel.OutID = index.OutID;
            //    ListModel.InListID = index.InListID;
            //    ListModel.OutListID = index.OutListID;
            //    ListModel.ProID = index.ProID;
            //    ListModel.ProCount = index.ProCount;
          
            //    model.Pro_OutOrderList.Add(ListModel);
            //}
            PublicRequestHelp help = new PublicRequestHelp(this.busy,MethodIDStore.Method_Accept, new object[] { model }, BackResult);
        }
        private void BackResult(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                Logger.Log(e.Result.Message + "");  
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
                if (e.Result.ReturnValue == true)
                {
                    if (pageParam != null)
                    {
                        PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.MethodID, new object[] { pageParam }, NewSearchResult);
                    }
                   
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
            }
        }
        private void NewSearchResult(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    API.ReportPagingParam pageParem = (API.ReportPagingParam)e.Result.Obj;

                    this.dataGrid1.ItemsSource = pageParem.Obj;
                    PagedCollectionView pagedCollection = new PagedCollectionView(new int[pageParem.RecordCount]);
                    this.RadPager.Source = pagedCollection;
                    this.RadPager.PageIndex = pageParem.PageIndex;

               
                    List<API.Pro_OutModel> outlist = dataGrid1.ItemsSource as List<API.Pro_OutModel>;
                    var query = (from b in outlist
                                 where b.OutOrderID == this.PKid.Text
                                 select b).ToList();
                    if (query.Count > 0)
                    {
                        dataGrid1.SelectedItems.Clear();
                        dataGrid1.SelectedItems.Add(query.First());
                    }
               
                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常");
            }
        }
        #endregion
        private  void CleanAll()
        {
            ContentSource.DataContext = null;
            date.SelectedValue = null;
            Aduti.Text = string.Empty;
            dataGrid11.ItemsSource = null;
            dataGrid2.ItemsSource = null;
        }
        private void BtSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定执行接收操作吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Accept();
            }
        }
        #region 更改行数
        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (pageParam != null)
            {
                RadPager.PageSize = (int)e.NewValue;
                pageParam.PageSize = (int)e.NewValue;
                this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            }

        }
        #endregion
    }
}

