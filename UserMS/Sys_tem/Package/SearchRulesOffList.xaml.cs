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
using UserMS.Views.PackageOff;

namespace UserMS.Sys_tem.Package
{
    /// <summary>
    /// SearchRulesOffList.xaml 的交互逻辑
    /// </summary>
    public partial class SearchRulesOffList : Page
    {
        private bool flag = false;
        List<API.View_Rules_OffList> models = new List<API.View_Rules_OffList>();

        public SearchRulesOffList()
        {
            InitializeComponent();
            flag = true;
            GridRuleOff.ItemsSource = models;
            this.SizeChanged += SearchRulesOffList_SizeChanged;
            Search();
        }

        void SearchRulesOffList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
        }

        #region  Search

        private void Search()
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = page.PageIndex;
            rpp.PageSize = (int)pagesize.Value;
            rpp.ParamList = new List<API.ReportSqlParams>();

            //if (!string.IsNullOrEmpty(this.StartTime.SelectedValue.ToString()))
            //{
            //    API.ReportSqlParams_DataTime startTime = new API.ReportSqlParams_DataTime();
            //    startTime.ParamName = "StartDate";
            //    startTime.ParamValues = this.StartTime.SelectedValue;
            //    rpp.ParamList.Add(startTime);
            //}

            //if (!string.IsNullOrEmpty(this.EndTime.SelectedValue.ToString()))
            //{
            //    API.ReportSqlParams_DataTime endTime = new API.ReportSqlParams_DataTime();
            //    endTime.ParamName = "EndDate";
            //    endTime.ParamValues = this.EndTime.SelectedValue;
            //    rpp.ParamList.Add(endTime);
            //}
            //全部
            //正在进行
            //未开始
            //已结束
            if (offState.SelectedIndex != 0)
            {
                API.ReportSqlParams_String text = new API.ReportSqlParams_String();
                text.ParamName = "State";
                text.ParamValues = (offState.SelectedItem as ComboBoxItem).Content.ToString();
                rpp.ParamList.Add(text);
            }

            if (!string.IsNullOrEmpty(Note.Text))
            {
                API.ReportSqlParams_String text = new API.ReportSqlParams_String();
                text.ParamName = "Note";
                text.ParamValues = this.Note.Text;
                rpp.ParamList.Add(text);
            }

            if (!string.IsNullOrEmpty(this.CreatName.Text))
            {
                API.ReportSqlParams_String crea = new API.ReportSqlParams_String();
                crea.ParamName = "UserName";
                crea.ParamValues = this.CreatName.Text;
                rpp.ParamList.Add(crea);
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 283, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
        }

        /// <summary>
        /// 查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;

            this.busy.IsBusy = false;
            models.Clear();
            GridRuleOff.Rebind();
      
            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_Rules_OffList> aduitList = pageParam.Obj as List<API.View_Rules_OffList>;
                if (aduitList != null)
                {
                    models.AddRange(aduitList);
                    GridRuleOff.Rebind();

                    this.page.PageSize = (int)pagesize.Value;
                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);

                    this.page.PageIndexChanged -= page_PageIndexChanged;
                    this.page.Source = pcv;
                    this.page.PageIndexChanged += page_PageIndexChanged;
                    this.page.PageIndex = pageindex;
                }
            }
        }

        private int pageindex;
        private void page_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            Search();
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        #endregion 

        #region  删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void del_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            
            if (GridRuleOff.SelectedItem == null)
            {
                MessageBox.Show("请选择需要删除的规则活动！");
                return;
            }
            if (MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            API.View_Rules_OffList m = GridRuleOff.SelectedItem as API.View_Rules_OffList;

            PublicRequestHelp prh = new PublicRequestHelp(this.busy,282,new object[]{m.ID},new EventHandler<API.MainCompletedEventArgs>(DelCompleted));

        }

        private void DelCompleted(object sender, API.MainCompletedEventArgs e)
        {
             if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            this.busy.IsBusy = false;

            if (e.Result.ReturnValue)
            {
                Search();
            }
            MessageBox.Show(e.Result.Message);
        }

        #endregion 

        #region   Get Detail

        private void GridRuleOff_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridRuleOff.SelectedItem == null)
            {
                return;
            }
           API.View_Rules_OffList model = GridRuleOff.SelectedItem as API.View_Rules_OffList;
           PublicRequestHelp prh = new PublicRequestHelp(this.busy, 284, new object []{ model.ID },
               new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,
                    "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            this.busy.IsBusy = false;

            if (e.Result.ReturnValue)
            {
                List<API.RulesProMain> list = e.Result.Obj as List<API.RulesProMain>;
                List<API.Pro_HallInfo> halls = e.Result.ArrList[0] as List<API.Pro_HallInfo>;
                List<API.Pro_SellType> selltype = e.Result.ArrList[1] as List<API.Pro_SellType>;

                GridProMain.ItemsSource = list;
                GridProMain.Rebind();
                if (list.Count() > 0)
                {
                    GridProMain.SelectedItem = list[0];
                }
                GridHall.ItemsSource = halls;
                GridHall.Rebind();
                GridSellType.ItemsSource = selltype;
                GridSellType.Rebind();

            }
        }

        #endregion 


        private void GridProMain_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridProMain.SelectedItem == null)
            {
                return;
            }
            API.RulesProMain m = GridProMain.SelectedItem as API.RulesProMain;
            GridOff.ItemsSource = m.Pro_RulesTypeInfo;
            GridOff.Rebind();
        }

        void Clear()
        {
            GridProMain.ItemsSource = null;
            GridProMain.Rebind();
            GridHall.ItemsSource = null;
            GridHall.Rebind();
            GridSellType.ItemsSource = null;
            GridSellType.Rebind();

            GridOff.ItemsSource = null;
            GridOff.Rebind();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void stop_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<int> list = new List<int>();
            foreach(var item in GridRuleOff.SelectedItems)
            {
                API.View_Rules_OffList mod = item  as API.View_Rules_OffList;
                if(mod.State=="已结束")
                {
                    MessageBox.Show("规则"+mod.Note+"已结束！");
                    return;
                }
                list.Add(mod.ID);
            }
            StopWindow sw = new StopWindow(289,list);
            sw.OnSearch += sw_OnSearch;
            sw.ShowDialog();
        }

        void sw_OnSearch()
        {
            Search();
        }
    }
}
