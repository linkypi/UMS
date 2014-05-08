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

namespace UserMS.Views
{
    /// <summary>
    /// DealerManager.xaml 的交互逻辑
    /// </summary>
    public partial class DealerManager : Page
    {
        public DealerManager()
        {
            InitializeComponent();
            errGrid.ItemsSource = models;
            radDataPager1.PageSize = 20;
            flag = true;
            Search();
        }

        bool flag = false;
        int pageIndex = 0;
        List<API.ASP_Dealer> models = new List<API.ASP_Dealer>();

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

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 379, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

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

                List<API.ASP_Dealer> list = pageParam.Obj as List<API.ASP_Dealer>;
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


        private void Clear()
        {
            this.updDelunit.Text = "";
            this.updnote.Text ="";
            this.updPhone.Text = "";
            this.updUserName.Text = "";

            this.addDelunit.Text = "";
            this.note.Text = "";
            this.addPhone.Text = "";
            this.addUserName.Text = "";
           
        }

        private void errGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (errGrid.SelectedItems.Count == 0)
            {
                return;
            }

            API.ASP_Dealer model = errGrid.SelectedItem as API.ASP_Dealer;
            this.updDelunit.Text = model.Dealer;
            this.updnote.Text = model.Note;
            this.updPhone.Text = model.Phone;
            this.updUserName.Text = model.UserName;
            updAddr.Text = model.Addr;

        }

        private void delete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (errGrid.SelectedItems.Count() == 0)
            {
                MessageBox.Show("请选择需要删除的数据！"); return;
            }

            API.ASP_Dealer model = errGrid.SelectedItem as API.ASP_Dealer;

            if (MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp p = new PublicRequestHelp(this.busy,376,new object[]{model.ID},DelCompleted);
        }

        private void DelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                
                Search();
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.addDelunit.Text))
            {
                MessageBox.Show("经销商名称不能为空！"); return;
            }
            if (string.IsNullOrEmpty(this.addUserName.Text))
            {
                MessageBox.Show("姓名不能为空！"); return;
            }
          
            API.ASP_Dealer model = new API.ASP_Dealer();
            model.UserName = this.addUserName.Text;
            model.Dealer = addDelunit.Text;
            model.Phone = addPhone.Text;
            model.Note = note.Text;
            model.Addr = addAddr.Text.Trim();
            if (MessageBox.Show("确定添加吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp p = new PublicRequestHelp(this.busy, 377, new object[] { model }, DelCompleted);
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (errGrid.SelectedItems.Count() == 0)
            {
                MessageBox.Show("请选择需要修改的数据！"); return;
            }
            if (string.IsNullOrEmpty(this.updDelunit.Text))
            {
                MessageBox.Show("经销商名称不能为空！"); return;
            }
            if (string.IsNullOrEmpty(this.updUserName.Text))
            {
                MessageBox.Show("姓名不能为空！"); return;
            }
            API.ASP_Dealer selectmodel = errGrid.SelectedItem as API.ASP_Dealer;
            API.ASP_Dealer model = new API.ASP_Dealer();
            model.UserName = updUserName.Text;
            model.Dealer = updDelunit.Text;
            model.Phone = updPhone.Text;
            model.Note = updnote.Text;
            model.ID = selectmodel.ID;
            model.Addr = updAddr.Text.Trim();
            if (MessageBox.Show("确定修改吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp p = new PublicRequestHelp(this.busy, 378, new object[] { model }, DelCompleted);
        }
    }
}
