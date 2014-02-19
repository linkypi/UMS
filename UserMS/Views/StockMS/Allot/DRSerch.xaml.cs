using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.Allot
{
    public partial class DRSerch : BasePage
    {
      public int MethodID = 16;
        API.ReportPagingParam pageParam;//全局变量分页内容
        HallFilter hAdder;//全局变量（参数添加器）
        string r = "";
        List<API.Pro_HallInfo> hall;
        public DRSerch()
        {
            InitializeComponent();
            InitGrid2();
  
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

            this.TbProClass.ItemsSource = Store.ProClassInfo;
            this.TbProClass.DisplayMemberPath = "ClassName";
            this.TbProClass.SelectedValuePath = "ClassID";

            this.TbProType.ItemsSource = Store.ProTypeInfo;
            this.TbProType.DisplayMemberPath = "TypeName";
            this.TbProType.SelectedValuePath = "TypeID";

            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];

            }
            catch
            {
                r = "161";
            }
            finally
            {
            //   GetFirstHall();
                Search();
            }
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
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hall = hAdder.FilterHall(int.Parse(r.Trim()), HallInfo);
            if (HallInfo == null)
                HallInfo = new List<API.Pro_HallInfo>();
            hAdder.GetHall(HallInfo);
        }
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(true, ref this.GHHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hall = hAdder.FilterHall(int.Parse(r.Trim()), HallInfo);
            if (hall == null)
                hall = new List<API.Pro_HallInfo>();
            hAdder.GetHall(hall);
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

            if (!string.IsNullOrEmpty(this.TbProClass.Text))
            {
                API.ReportSqlParams_String ClassID = new API.ReportSqlParams_String();
                ClassID.ParamName = "ClassName";
                ClassID.ParamValues = this.TbProClass.Text.Trim();
                pageParam.ParamList.Add(ClassID);
            }
            if (!string.IsNullOrEmpty(this.TbProType.Text))
            {
                API.ReportSqlParams_String TypeID = new API.ReportSqlParams_String();
                TypeID.ParamName = "TypeName";
                TypeID.ParamValues = this.TbProType.Text.Trim();
                pageParam.ParamList.Add(TypeID);
            }
            if (!string.IsNullOrEmpty(this.TbProName.Text))
            {
                API.ReportSqlParams_String ProName = new API.ReportSqlParams_String();
                ProName.ParamName = "ProName";
                ProName.ParamValues = this.TbProType.Text.Trim();
                pageParam.ParamList.Add(ProName);
            }

            if (!string.IsNullOrEmpty(this.SHHall.TextBox.SearchText))
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "Pro_HallName";
                if (hall.ParamValues == null)
                    hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(GetHall_Item(this.SHHall.TextBox.SearchText));
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

 
            this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam,true);
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

            GridViewDataColumn col21 = new GridViewDataColumn();
            col21.DataMemberBinding = new System.Windows.Data.Binding("ProName");
            col21.Header = "型号";
            this.dataGrid1.Columns.Add(col21);

       

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("FromHallName");
            col4.Header = "供货仓库";
            this.dataGrid1.Columns.Add(col4);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("Pro_HallName");
            col41.Header = "收货仓库";
            this.dataGrid1.Columns.Add(col41);

            GridViewDataColumn col51 = new GridViewDataColumn();
            col51.DataMemberBinding = new System.Windows.Data.Binding("OutDate");
            col51.Header = "调拨日期";
            this.dataGrid1.Columns.Add(col51);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("FromUserName");
            col5.Header = "操作人";
            this.dataGrid1.Columns.Add(col5);


            GridViewDataColumn col52 = new GridViewDataColumn();
            col52.DataMemberBinding = new System.Windows.Data.Binding("IMEI");
            col52.Header = "串号";
            this.dataGrid1.Columns.Add(col52);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("Aduit");
            col3.Header = "接收状态";
            this.dataGrid1.Columns.Add(col3);

        
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
            this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam,true);
            #endregion
        }
        #endregion                            
    }
}
