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
    /// CancelAir.xaml 的交互逻辑
    /// </summary>
    public partial class CancelAir : BasePage
    {
        API.ReportPagingParam pageParam;//全局变量分页内容
        HallFilter hAdder;//全局变量（参数添加器）
        string r = "";
        List<API.Pro_HallInfo> hall;
        public CancelAir()
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
            this.InitPageEntity(MethodIDStore.GetAirCancelModel, this.dataGrid1, this.busy, this.RadPager, pageParam);
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
            col3.DataMemberBinding = new System.Windows.Data.Binding("Audit");
            col3.Header = "接收状态";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("FromHallName");
            col4.Header = "调出仓库";
            this.dataGrid1.Columns.Add(col4);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("ToHallName");
            col41.Header = "调入仓库";
            this.dataGrid1.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("FromUserName");
            col5.Header = "录单员";
            this.dataGrid1.Columns.Add(col5);

            GridViewDataColumn col51 = new GridViewDataColumn();
            col51.DataMemberBinding = new System.Windows.Data.Binding("OutTime");
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


            #endregion


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
            this.InitPageEntity(MethodIDStore.GetAirCancelModel, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion 

        #region 获取单据明细
        private void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            API.View_AirOutInfo outModel = this.dataGrid1.SelectedItem as API.View_AirOutInfo;
            CleanAll();
            if (outModel == null)
                return;
            //如果该单已经接收，则赋值给判断变量
            ContentSource.DataContext = outModel;
            if (outModel.Audit == "Y")
            {
                Aduti.Text = "已接收";
            }
            else
                Aduti.Text = "未接收";

            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.GetAirDBModel, new object[] { outModel.ID }, BackOutList);
        }
        private void BackOutList(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
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
        
        #region 取消操作
        void Cancel()
        {

            API.View_AirOutInfo outModel = this.dataGrid1.SelectedItem as API.View_AirOutInfo;
            if (outModel == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要取消的调拨单！");
                return;
            }
            if (outModel.Audit == "Y")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"调拨单已接收，无法取消！");
                return;
            }
            List<API.View_AirOutListModel> outList = dataGrid11.ItemsSource as List<API.View_AirOutListModel>;
            if (outList == null || outList.Count==0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"调拨单加载明细失败，无法取消！");
                return;
            }
            API.Pro_AirOutInfo model = new API.Pro_AirOutInfo();
            model.OutOrderID = outModel.OutOrderID;
            model.ID = outModel.ID;
            model.FromHallID = outModel.FromHallID;

            PublicRequestHelp help = new PublicRequestHelp(this.busy,MethodIDStore.CancelAirDB, new object[] { model }, BackResult);
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
                        PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.GetAirCancelModel, new object[] { pageParam }, NewSearchResult);
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
                Logger.Log(e.Result.Message + "");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
                if (e.Result.ReturnValue == true)
                {
                    CleanAll();
                    if (pageParam != null)
                        this.InitPageEntity(MethodIDStore.GetAirCancelModel, this.dataGrid1, this.busy, this.RadPager, pageParam);              
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
            }           
        }
        #endregion
        private  void CleanAll()
        {
            ContentSource.DataContext = null;
         
            Aduti.Text = string.Empty;
            dataGrid11.ItemsSource = null;
 
        }
        private void BtSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定执行取消操作吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Cancel();
            }
        }
        #region 更改行数
        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (pageParam != null)
            {
                RadPager.PageSize = (int)e.NewValue;
                pageParam.PageSize = (int)e.NewValue;
                this.InitPageEntity(MethodIDStore.GetAirCancelModel, this.dataGrid1, this.busy, this.RadPager, pageParam);
            }

        }
        #endregion
    }
}

