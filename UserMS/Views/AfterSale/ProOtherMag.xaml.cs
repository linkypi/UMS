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

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// ProOtherMag.xaml 的交互逻辑
    /// </summary>
    public partial class ProOtherMag : Page
    {
        public ProOtherMag()
        {
            InitializeComponent();
            errGrid.ItemsSource = models;
            radDataPager1.PageSize = 20;
       
            flag = true;

            Search();
        }

        int pageIndex = 0;
        bool flag = false;
        List<API.ASP_ProOther> models = new List<API.ASP_ProOther>();

        private void Search()
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageSize = (int)pagesize.Value;
            rpp.PageIndex = radDataPager1.PageIndex;

            rpp.ParamList = new List<API.ReportSqlParams>();


            //API.ReportSqlParams_Bool HasRepaired = new API.ReportSqlParams_Bool();
            //HasRepaired.ParamName = "HasRepaired";
            //HasRepaired.ParamValues = true;
            //rpp.ParamList.Add(HasRepaired);  //HasFetch

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 352, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.radDataPager1.PageIndexChanged -= radDataPager1_PageIndexChanged;
            this.radDataPager1.Source = pcv1;
            this.radDataPager1.PageIndexChanged += radDataPager1_PageIndexChanged;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.ASP_ProOther> list = pageParam.Obj as List<API.ASP_ProOther>;
                if (list == null) { return; }
                models.Clear();
                models.AddRange(list);
                errGrid.Rebind();
                this.radDataPager1.PageSize = (int)pagesize.Value;
                string[] data = new string[pageParam.RecordCount];
                PagedCollectionView pcv = new PagedCollectionView(data);
                this.radDataPager1.PageIndexChanged -= radDataPager1_PageIndexChanged;
                this.radDataPager1.Source = pcv;
                this.radDataPager1.PageIndexChanged += radDataPager1_PageIndexChanged;
                this.radDataPager1.PageIndex = pageIndex;
            }
            else
            {
                models.Clear();
                errGrid.Rebind();
            }

        }

        private void Clear()
        {
            adderr.Text = string.Empty;
            err.Text = string.Empty;
            errID.Text = string.Empty;
            adderrID.Text = string.Empty;

        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(adderr.Text))
            {
                MessageBox.Show("附件名称不能为空！");
                return;
            }
            API.ASP_ProOther aerr = new API.ASP_ProOther();
            aerr.Name = adderr.Text;
          
            if (MessageBox.Show("确定添加吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp p = new PublicRequestHelp(this.busy, 354, new object[] { aerr }, UpdateCompleted);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(id.Text))
            //{
            //    MessageBox.Show("请选择要修改的项！");
            //    return;
            //}

            if (string.IsNullOrEmpty(err.Text))
            {
                MessageBox.Show("附件名称不能为空！");
                return;
            }

            if (MessageBox.Show("确定修改吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            API.ASP_ProOther merr = errGrid.SelectedItem as API.ASP_ProOther;
            API.ASP_ProOther err2 = new API.ASP_ProOther();
            err2.ID = merr.ID;
            err2.Name = merr.Name;
          
            PublicRequestHelp p = new PublicRequestHelp(this.busy, 355, new object[] { err2 }, UpdateCompleted);

        }

        private void UpdateCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;

            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Search();
            }
        }


        private void radDataPager1_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                pageIndex = e.NewPageIndex;
                Search();
            }
        }

        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                radDataPager1.PageSize = (int)pagesize.Value;
                Search();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (errGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的数据！");
                return;
            }

            List<API.ASP_ProOther> list = new List<API.ASP_ProOther>();
            foreach (var item in errGrid.SelectedItems)
            {
                API.ASP_ProOther err = item as API.ASP_ProOther;
                API.ASP_ProOther a = new API.ASP_ProOther();
                a.ID = err.ID;
                list.Add(a);
            }

            if (MessageBox.Show("确定删除选中项吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prd = new PublicRequestHelp(busy, 353, new object[] { list }, DeleteCompleted);
        }

        private void DeleteCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;

            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Search();
            }
        }

        private void errGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (errGrid.SelectedItems.Count == 0)
            {
                return;
            }

            API.ASP_ProOther model = errGrid.SelectedItem as API.ASP_ProOther;
          
            errID.Text = model.ID.ToString();
            err.Text = model.Name;

        }

    }
}
