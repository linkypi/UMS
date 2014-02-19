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
using SlModel;
using Telerik.Windows.Controls;
using UserMS.Common;
using UserMS.Model.PackageModel;

namespace UserMS.Views.PackageOff
{
    /// <summary>
    /// PackageAduitSearch.xaml 的交互逻辑
    /// </summary>
    public partial class PackageAduitSearch : Page
    {
        public PackageAduitSearch()
        {
            InitializeComponent();

            List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
            };
            this.Cbaduit.ItemsSource = list;
            this.Cbpassed.ItemsSource = list;
            this.Cbaduit2.ItemsSource = list;
            this.Cbpassed2.ItemsSource = list;
            this.Cbaduit3.ItemsSource = list;
            this.Cbpassed3.ItemsSource = list;
            this.Cbaduit.SelectedIndex = 0;
            this.Cbpassed.SelectedIndex = 2;

            this.Cbaduit2.SelectedIndex = 0;
            this.Cbpassed2.SelectedIndex = 2;
            this.Cbaduit3.SelectedIndex = 0;
            this.Cbpassed3.SelectedIndex = 2;
            flag = true;
            SearchOff();
        }
    
         API.ReportPagingParam pageParam;
        private bool flag = false;
        private int pageindex = 0;

        private List<GounpSource> GounpList = new List<GounpSource>();      

    
        #region 查询

        private void BtSearch_Click_1(object sender, RoutedEventArgs e)
        {
            SearchOff();
        }
        void SearchOff()
        {
            if (!flag)
            {
                return;
            }


            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.RadPager.PageIndex;
            pageParam.PageSize = (int)this.pagesize.Value;
            pageParam.ParamList = new List<API.ReportSqlParams>();
            if (this.Cbaduit.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool aduited = new API.ReportSqlParams_Bool();
                aduited.ParamName = "Aduited1";
                aduited.ParamValues = ((CkbModel)Cbaduit.SelectedItem).Flag;
                pageParam.ParamList.Add(aduited);
            }

            if (this.Cbpassed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool aduited = new API.ReportSqlParams_Bool();
                aduited.ParamName = "Passed1";
                aduited.ParamValues = ((CkbModel)Cbpassed.SelectedItem).Flag;
                pageParam.ParamList.Add(aduited);
            }

            if (this.Cbaduit2.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool aduited2 = new API.ReportSqlParams_Bool();
                aduited2.ParamName = "Aduited2";
                aduited2.ParamValues = ((CkbModel)Cbaduit2.SelectedItem).Flag;
                pageParam.ParamList.Add(aduited2);
            }

            if (this.Cbpassed2.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool passed2 = new API.ReportSqlParams_Bool();
                passed2.ParamName = "Passed2";
                passed2.ParamValues = ((CkbModel)Cbpassed2.SelectedItem).Flag;
                pageParam.ParamList.Add(passed2);
            }


            if (this.Cbaduit3.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool aduited3 = new API.ReportSqlParams_Bool();
                aduited3.ParamName = "Aduited3";
                aduited3.ParamValues = ((CkbModel)Cbaduit2.SelectedItem).Flag;
                pageParam.ParamList.Add(aduited3);
            }

            if (this.Cbpassed3.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool passed3 = new API.ReportSqlParams_Bool();
                passed3.ParamName = "Passed3";
                passed3.ParamValues = ((CkbModel)Cbpassed2.SelectedItem).Flag;
                pageParam.ParamList.Add(passed3);
            }

            API.ReportSqlParams_Bool delete = new API.ReportSqlParams_Bool();
            delete.ParamName = "IsDelete";
            delete.ParamValues = false;
            pageParam.ParamList.Add(delete);

            if (!string.IsNullOrEmpty(this.applydate.DateTimeText))
            {
                API.ReportSqlParams_DataTime OffName = new API.ReportSqlParams_DataTime();
                OffName.ParamName = "ApplyDate";
                OffName.ParamValues = applydate.SelectedDate;
                pageParam.ParamList.Add(OffName);
            }

            //创建人
            if (!string.IsNullOrEmpty(this.creater.Text))
            {
                API.ReportSqlParams_String CraetName = new API.ReportSqlParams_String();
                CraetName.ParamName = "Creater";
                CraetName.ParamValues = this.creater.Text.Trim();
                pageParam.ParamList.Add(CraetName);
            }

           PublicRequestHelp h = new PublicRequestHelp(this.busy, 294 , new object[] { pageParam },new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
            
         
        }

        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }

            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.RadPager.PageIndexChanged -= RadPager_PageIndexChanged;
            this.RadPager.Source = pcv1;
            this.RadPager.PageIndexChanged += RadPager_PageIndexChanged;

            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                if (pageParam == null) { return; }
                List<API.View_VIPOffListAduitHeader> aduitList = pageParam.Obj as List<API.View_VIPOffListAduitHeader>;

                if (aduitList != null)
                {
                    listView.DataContext = aduitList;

                    this.RadPager.PageSize = (int)pagesize.Value;
                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);

                    this.RadPager.PageIndexChanged -= RadPager_PageIndexChanged;
                    this.RadPager.Source = pcv;
                    this.RadPager.PageIndexChanged += RadPager_PageIndexChanged;
                    this.RadPager.PageIndex = pageindex;
                }
            }
        }

        #endregion

        private void RadPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            SearchOff();
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            SearchOff();
        }


        private void del_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的数据！");
                return;
            }

            API.View_VIPOffListAduitHeader model = listView.SelectedItem as API.View_VIPOffListAduitHeader;

         
            if (model.Passed2 == "Y")
            {
                MessageBox.Show("审批单已全部通过，删除失败！"); return;
            }
            

            if (MessageBox.Show("确定删除吗？", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 298, new object[] { model.ID }, new EventHandler<API.MainCompletedEventArgs>(DelCompleted));
        }

        private void DelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            SearchOff();
        }
    }
}
