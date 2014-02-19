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
using UserMS.Common;

namespace UserMS.Views.ProSell.SMS
{
    /// <summary>
    /// SMS_SellBackAduit.xaml 的交互逻辑
    /// </summary>
    public partial class SMS_SellBackAduit : Page
    {
        public SMS_SellBackAduit()
        {
            InitializeComponent();
            List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部"),
            };
            hall.SearchEvent = new RoutedEventHandler(SeachHall);
            this.ckbAduit.ItemsSource = list;
            this.ckbPassed.ItemsSource = list;
            this.ckbAduit.SelectedIndex = 1;
            this.ckbPassed.SelectedIndex = 2;
            hallinfo = new HallFilter(true, ref hall);
            flag = true;
            if (Store.ProHallInfo.Count > 0)
            {
                this.hall.Tag = Store.ProHallInfo[0].HallID;
                this.hall.Text = Store.ProHallInfo[0].HallName;
            }
        }

        private void SeachHall(object sender, RoutedEventArgs e)
        {
            hallinfo.GetHall(Store.ProHallInfo);
        }
        private HallFilter hallinfo;
        private bool flag;
        private int pageindex;

        #region 查询

        private void Search()
        {
            if (!flag)
            {
                return;
            }

            API.ReportPagingParam pageParam;
            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.page.PageIndex;
            pageParam.PageSize = (int)this.pagesize.Value;
            pageParam.ParamList = new List<API.ReportSqlParams>();
            if (!string.IsNullOrEmpty(this.hall.Text))
            {
                API.ReportSqlParams_String hallid = new API.ReportSqlParams_String();
                hallid.ParamName = "HallID";
                hallid.ParamValues = hall.Tag.ToString();
                pageParam.ParamList.Add(hallid);
            }

            if (!string.IsNullOrEmpty(this.sysDate.DateTimeText))
            {
                API.ReportSqlParams_DataTime date = new API.ReportSqlParams_DataTime();
                date.ParamName = "SysDate";
                date.ParamValues = sysDate.SelectedDate;
                pageParam.ParamList.Add(date);
            }

            if (ckbAduit.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool aduit = new API.ReportSqlParams_Bool();
                aduit.ParamName = "Aduited";
                aduit.ParamValues = (ckbAduit.SelectedItem as CkbModel).Flag;
                pageParam.ParamList.Add(aduit);
            }

            if (ckbPassed.SelectedIndex != 2)
            {
                API.ReportSqlParams_Bool pass = new API.ReportSqlParams_Bool();
                pass.ParamName = "Passed";
                pass.ParamValues = (ckbPassed.SelectedItem as CkbModel).Flag;
                pageParam.ParamList.Add(pass);
            }

            //创建人
            if (!string.IsNullOrEmpty(this.txtCus.Text))
            {
                API.ReportSqlParams_String cusName = new API.ReportSqlParams_String();
                cusName.ParamName = "CusName";
                cusName.ParamValues = this.txtCus.Text.Trim();
                pageParam.ParamList.Add(cusName);
            }

            PublicRequestHelp h = new PublicRequestHelp(this.busy, 305, new object[] { pageParam }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            this.busy.IsBusy = false;

            GridAuitList.Rebind();
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;

            if (e.Result.Obj != null)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                if (pageParam == null) { return; }
                List<API.View_SMS_SellBackAduit> list = pageParam.Obj as List<API.View_SMS_SellBackAduit>;
                if (list != null)
                {
                    GridAuitList.ItemsSource = list;
                    GridAuitList.Rebind();
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

        #endregion

        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        private void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            Search();
        }

        #region 审批

        private void aduitPass_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Aduit(true);
        }

        private void Aduit(bool pass)
        {
            if (GridAuitList.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要审批的数据！");
                return;
            }

            API.View_SMS_SellBackAduit model = GridAuitList.SelectedItem as API.View_SMS_SellBackAduit;

            if (model.Used == "Y")
            {
                MessageBox.Show("审批单已使用！");
                return;
            }
            if (model.Aduited == "Y")
            {
                MessageBox.Show("审批单已审批！");
                return;
            }
            if (MessageBox.Show("确定审批吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp phe = new PublicRequestHelp(this.busy, 306, new object[] { model.ID,pass,note.Text??""}, new EventHandler<API.MainCompletedEventArgs>(AduitCompleted));
        }

        private void AduitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Search();
            }
        }

        private void AduitNoPass_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Aduit(false);
        }

        #endregion 
    }
}
