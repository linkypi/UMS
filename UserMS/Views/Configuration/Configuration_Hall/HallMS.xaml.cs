using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.Configuration.Configuration_Hall
{
    /// <summary>
    /// HallMS.xaml 的交互逻辑
    /// </summary>
    public partial class HallMS : BasePage
    {
        API.ReportPagingParam pageParam;//全局变量分页内容
        public HallMS()
        {
            InitializeComponent();
            AreaSource();
            GetHallInfo();
        }
        #region 初始化数据
        protected void AreaSource()
        {
            AreaName.ItemsSource = Store.AreaInfo;
            AreaName.DisplayMemberPath = "AreaName";
            AreaName.SelectedValuePath = "AreaID";

            this.LevelName.ItemsSource = Store.Level;
            LevelName.DisplayMemberPath = "LevelName";
            LevelName.SelectedValuePath = "LevelID";

        }   
        #endregion

        private void GetHallInfo()
        {         
             //取第一页的数据
            RadPager.PageSize = (int)this.pagesize.Value;
            pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = (int)this.pagesize.Value,
                ParamList = new List<API.ReportSqlParams>()
            };
            this.InitPageEntity(MethodIDStore.SelectHall, this.dataGrid1, this.busy, this.RadPager, pageParam);
        }
        #region 更改行数
        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (pageParam != null)
            {
                RadPager.PageSize = (int)e.NewValue;
                pageParam.PageSize = RadPager.PageSize;
                this.InitPageEntity(MethodIDStore.SelectHall, this.dataGrid1, this.busy, this.RadPager, pageParam);
            }

        }
        #endregion
        #region 下一页事件
        /// <summary>
        /// 点击下一页时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取下一页的数据

            pageParam.PageIndex = e.NewPageIndex;
            this.InitPageEntity(MethodIDStore.SelectHall, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion

        private void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            this.HallPanel.DataContext = null;
           
            API.View_HallInfo Hall = this.dataGrid1.SelectedItem as API.View_HallInfo;
            HallPanel.DataContext = Hall;
           
        }

        private void TbSubit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定修改？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            API.View_HallInfo model = this.dataGrid1.SelectedItem as API.View_HallInfo;
            if (model == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择任何项！");
                return;
            }
            API.Pro_HallInfo HallInfo = new API.Pro_HallInfo();
            if (string.IsNullOrEmpty(this.AreaName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择区域！");
                return;
            }
            if (string.IsNullOrEmpty(this.LevelName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择等级！");
                return;
            }
            if (string.IsNullOrEmpty(HallName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入门店中文名称！");
                return;
            }
            if (string.IsNullOrEmpty(EHallName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入门店的英文名称！");
                return;
            }
            if (string.IsNullOrEmpty(CanIn.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择可否入库！");
                return;
            }
            if (string.IsNullOrEmpty(CanBack.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择可否退库！");
                return;
            }
            if (!string.IsNullOrEmpty(this.Order.Text))
            {
                try
                {
                    HallInfo.Order = int.Parse(this.Order.Text.Trim());
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"序号必须为正整数！");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(this.Longitude.Text))
            {
                try
                {
                    HallInfo.Longitude = decimal.Parse(this.Longitude.Text);
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入有效的经度！");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(this.Latitude.Text))
            {
                try
                {
                    HallInfo.Latitude = decimal.Parse(this.Latitude.Text);
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入有效的纬度！");
                    return;
                }
            } 
            HallInfo.HallID = model.HallID;
            HallInfo.AreaID = (int)AreaName.SelectedValue;
            HallInfo.CanBack = CanBack.Text == "是" ? true : false;
            HallInfo.CanIn = CanIn.Text == "是" ? true : false;
            HallInfo.DisPlayName = model.DisPlayName;
            HallInfo.Flag = model.Flag;
            HallInfo.HallName = model.HallName;
            HallInfo.LevelID =(int)LevelName.SelectedValue;   
            HallInfo.Note = model.Note;    
            HallInfo.PrintName = model.PrintName;
            HallInfo.SellNum = model.SellNum;
            HallInfo.ShortName = model.ShortName;

            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.UpdateHall, new object[] { HallInfo }, Completed);
        }
        private void Completed(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.Message != null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
                    Logger.Log(re.Result.Message);
                }
                if (re.Result.ReturnValue == true)
                {
                    this.HallPanel.DataContext = null;
                    this.InitPageEntity(MethodIDStore.SelectHall, this.dataGrid1, this.busy, this.RadPager, pageParam);

                    //API.View_HallInfo Hall = this.dataGrid1.SelectedItem as API.View_HallInfo;
                    //GetHallInfo();
                    //List<API.View_HallInfo> HallList = this.dataGrid1.ItemsSource as List<API.View_HallInfo>;
                    //var query = (from b in HallList
                    //            where b.HallID == Hall.HallID
                    //            select b).ToList();
                    //if (query.Count() != 1)
                    //{
                    //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"数据出错！");
                    //    return;
                    //}
                    //this.dataGrid1.SelectedItems.Add(query.First());
                }
               
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
        }

        private void TbCancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            GetHallInfo();
            this.HallPanel.DataContext = null;
        }
    }
}
