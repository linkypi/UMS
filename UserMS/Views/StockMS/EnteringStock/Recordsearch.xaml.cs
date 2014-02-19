using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.EnteringStock
{
    public partial class Recordsearch : BasePage
    {
        API.ReportPagingParam pageParam;//全局变量分页内容
        HallFilter hAdder;//全局变量（参数添加器）
        string r = "";
        List<API.Pro_HallInfo> hall;
        public Recordsearch()
        {
            InitializeComponent();
            InitGrid2();

            //this.dataGrid1.KeyUp += dataGrid1_KeyUp;
            this.Update.Click += Update_Click;

            this.StartTime.SelectedValue =DateTime.Today;
            TimeSpan span = new TimeSpan(23, 59, 59);
            this.EndTime.SelectedValue = DateTime.Today.Add(span);
         
            #region 添加参数事件
            //添加搜索参数
            this.TbHall.TextBox.IsEnabled = false;
            this.TbHall.SearchButton.Click += SearchButton_Click;
            #endregion
           
        }
        private void BtSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void RadPager_PageIndexChanging(object sender, Telerik.Windows.Controls.PageIndexChangingEventArgs e)
        {

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];

            }
            catch
            {
                r = "9";
            }
            finally
            {
                GetFirstHall();
                Search();
            }
        }
        private void GetFirstHall()
        {
            hAdder = new HallFilter(true, ref this.TbHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hall = hAdder.FilterHall(int.Parse(r.Trim()), HallInfo);
            if (hall.Count == 0)
            {
                return;
            }
            this.TbHall.TextBox.SearchText = hall.First().HallName;
            this.TbHall.Tag = hall.First().HallID;
        }

        #region 更新操作
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Update_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (pageParam != null)
                this.InitPageEntity(int.Parse(r.Trim()), this.dataGrid1, this.busy, this.RadPager, pageParam);
        }
        #endregion 
        #region 添加查询参数
        /// <summary>
        /// 增加类型参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(true, ref this.TbHall);
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
            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.RadPager.PageIndex;
            pageParam.PageSize = this.RadPager.PageSize;
            pageParam.ParamList = new List<API.ReportSqlParams>();
       
            //原始仓库
            if (!string.IsNullOrEmpty(this.TbHall.TextBox.SearchText))
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "Pro_HallID";
                if (hall.ParamValues == null)
                    hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(GetHall_Item(this.TbHall.Tag.ToString()));
                pageParam.ParamList.Add(hall);
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

            if (!string.IsNullOrEmpty(this.TbProName.Text))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "UserName";
                users.ParamValues = this.TbProName.Text;
                pageParam.ParamList.Add(users);
            }
            this.InitPageEntity(MethodIDStore.Method_AllStore, this.dataGrid1, this.busy, this.RadPager, pageParam);
        }
        #endregion
        
        #region 生成列
        private void InitGrid2()
        {
            #region 列
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("ClassName");
            col.Header = "类别";
            this.dataGrid1.Columns.Add(col);


            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("TypeName");
            col2.Header = "品牌";
            this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("ProName");
            col3.Header = "型号";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("ProCount");
            col4.Header = "数量";
            this.dataGrid1.Columns.Add(col4);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("Price");
            col41.Header = "成本价";
            this.dataGrid1.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("InOrderTime");
            col5.Header = "入库日期";
            this.dataGrid1.Columns.Add(col5);

            GridViewDataColumn col51 = new GridViewDataColumn();
            col51.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            col51.Header = "所属仓库";
            this.dataGrid1.Columns.Add(col51);


            GridViewDataColumn col7 = new GridViewDataColumn();
            col7.DataMemberBinding = new System.Windows.Data.Binding("RealName");
            col7.Header = "操作人";
            this.dataGrid1.Columns.Add(col7);

            GridViewDataColumn col71 = new GridViewDataColumn();
            col71.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col71.Header = "备注";
            this.dataGrid1.Columns.Add(col71);

            GridViewDataColumn col8 = new GridViewDataColumn();
            col8.DataMemberBinding = new System.Windows.Data.Binding("IMEI");
            col8.Header = "串号";
            this.dataGrid1.Columns.Add(col8);


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
            this.InitPageEntity(MethodIDStore.Method_AllStore, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion 
    }
}
