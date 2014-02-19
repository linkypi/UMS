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
    /// AduitPackageOff2.xaml 的交互逻辑
    /// </summary>
    public partial class AduitPackageOff2 : Page
    {
          public AduitPackageOff2()
        {
            InitializeComponent();

            List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
            };
            this.Cbaduit.ItemsSource = list;
            this.Cbpassed.ItemsSource = list;
            this.Cbaduit.SelectedIndex = 1;
             this.Cbpassed.SelectedIndex=2;

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
            if (this.Cbaduit.SelectedIndex!=2)
            {
                API.ReportSqlParams_Bool aduited = new API.ReportSqlParams_Bool();
                aduited.ParamName = "Aduited2";
                aduited.ParamValues = ((CkbModel)Cbaduit.SelectedItem).Flag;
                pageParam.ParamList.Add(aduited);
            }

            if (!string.IsNullOrEmpty(this.applydate.DateTimeText))
            {
                API.ReportSqlParams_DataTime OffName = new API.ReportSqlParams_DataTime();
                OffName.ParamName = "ApplyDate";
                OffName.ParamValues = applydate.SelectedDate;
                pageParam.ParamList.Add(OffName);
            }
      

            if (this.Cbpassed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool aduited = new API.ReportSqlParams_Bool();
                aduited.ParamName = "Passed2";
                aduited.ParamValues = ((CkbModel)Cbpassed.SelectedItem).Flag;
                pageParam.ParamList.Add(aduited);
            }

            API.ReportSqlParams_Bool p1 = new API.ReportSqlParams_Bool();
            p1.ParamName = "Passed1";
            p1.ParamValues =true;
            pageParam.ParamList.Add(p1);


            API.ReportSqlParams_Bool a1 = new API.ReportSqlParams_Bool();
            a1.ParamName = "Aduited1";
            a1.ParamValues = true;
            pageParam.ParamList.Add(a1);

         
            API.ReportSqlParams_Bool delete = new API.ReportSqlParams_Bool();
            delete.ParamName = "IsDelete";
            delete.ParamValues = false;
            pageParam.ParamList.Add(delete);


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

        void sw_OnSearch()
        {
            SearchOff();
        }

        /// <summary>
        /// 批量审批通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void batchAduitPassed_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        /// <summary>
        /// 批量审批不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchAduitNoPassed_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
             
        }

        /// <summary>
        /// 单个审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AduitPassed_Click(object sender, RoutedEventArgs e)
        {
            Aduit(true);
        }

        private void Aduit(bool passed)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择审批单！");
                return;
            }

            API.View_VIPOffListAduitHeader model = listView.SelectedItem as API.View_VIPOffListAduitHeader;

            if (model.Aduited == "Y")
            {
                MessageBox.Show("审批单已审批！"); return;
            }
            if (model.Passed == "Y")
            {
                MessageBox.Show("审批单已通过！"); return;
            }
            if (model.Aduited2 == "Y")
            {
                MessageBox.Show("审批单已审批！"); return;
            }
            if (model.Passed2 == "Y")
            {
                MessageBox.Show("审批单已通过！"); return;
            }

            if (MessageBox.Show("确定审批吗？", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }

            PublicRequestHelp peh = new PublicRequestHelp(this.busy, 297, new object[] {
                model.ID, passed, model.AduitNote2??"" }, new EventHandler<API.MainCompletedEventArgs>(AduitCompleted));

        }

        private void AduitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "审批失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                SearchOff();
            }
            MessageBox.Show(e.Result.Message);
        }

        private void AduitUnPassed_Click(object sender, RoutedEventArgs e)
        {
            Aduit(false);
        }
    }
}
