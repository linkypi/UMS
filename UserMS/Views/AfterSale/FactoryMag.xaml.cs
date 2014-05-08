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
    /// FactoryMag.xaml 的交互逻辑
    /// </summary>
    public partial class FactoryMag : Page
    {
        bool flag = false;
        List<API.ASP_Factory> models = new List<API.ASP_Factory>();
        int pageIndex = 0;

        public FactoryMag()
        {
            InitializeComponent();
            searchGrid.ItemsSource = models;
            radDataPager1.PageSize = 20;
            flag = true;
            Search();
        }

        public void Clear()
        {
            updaddr.Text = string.Empty;
            updarea.Text = string.Empty;
            updbank.Text = string.Empty;
            updbanknum.Text = string.Empty;
            updcity.Text = string.Empty;
            updcontact.Text = string.Empty;
            updemail.Text = string.Empty;
            updfaccode.Text = string.Empty;
            updfacname.Text = string.Empty;
            updfax.Text = string.Empty;
            updnote.Text = string.Empty;
            updphone.Text = string.Empty;
            updpostcode.Text = string.Empty;
            updpricelevel.Text = string.Empty;
            updprovince.Text = string.Empty;
            updresponser.Text = string.Empty;
            updtaxcode.Text = string.Empty;
        }

        #region 查  询

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            Search();
        }

        private void Ckb_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                Search();
            }
        }

        private void Search()
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageSize = (int)pagesize.Value;
            rpp.PageIndex = radDataPager1.PageIndex;

            rpp.ParamList = new List<API.ReportSqlParams>();

            if (!string.IsNullOrEmpty(this.facname.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "FacName";
                users.ParamValues = this.facname.Text.Trim();
                rpp.ParamList.Add(users);
            }

            if (!string.IsNullOrEmpty(this.faccode.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "FacID";
                bt.ParamValues = this.faccode.Text;
                rpp.ParamList.Add(bt);
            }
         

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 368, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

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

                List<API.ASP_Factory> list = pageParam.Obj as List<API.ASP_Factory>;
                if (list == null) { return; }
                models.Clear();
                models.AddRange(list);
                searchGrid.Rebind();
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
                searchGrid.Rebind();
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

        #endregion

        #region 详情

        private void RepairGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                return;
            }

            API.ASP_Factory model = searchGrid.SelectedItem as API.ASP_Factory;
            if (model == null) { return; }
            updaddr.Text = model.Addr;
            updarea.Text = model.Area;
            updbank.Text = model.Bank;
            updbanknum.Text = model.BankNum;
            updcity.Text = model.City;
            updcontact.Text = model.Contacts;
            updemail.Text = model.Email;
            updfaccode.Text = model.FacID;
            updfacname.Text = model.FacName;
            updfax.Text = model.Fax;
            updnote.Text = model.Note;
            updphone.Text = model.Phone;
            updpostcode.Text = model.PostCode;
            updpricelevel.Text = model.PriceLevel.ToString();
            updprovince.Text = model.Province;
            updresponser.Text = model.Responser;
            updtaxcode.Text = model.TaxCode;
        }

        #endregion

        private void upd_Click(object sender, RoutedEventArgs e)
        {
            API.ASP_Factory model = searchGrid.SelectedItem as API.ASP_Factory;
            if (model == null) { MessageBox.Show("请选择需要修改的数据！"); return; }
               if (string.IsNullOrEmpty(updfacname.Text))
            {
                MessageBox.Show("厂家名称不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(updfaccode.Text))
            {
                MessageBox.Show("厂家编码不能为空！");
                return;
            }

            if (MessageBox.Show("确定修改吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            API.ASP_Factory fac = new API.ASP_Factory();
            fac.ID = model.ID;

            fac.Addr =   updaddr.Text;
            fac.Area = updarea.Text;
            fac.Bank = updbank.Text;
            fac.BankNum = updbanknum.Text;
            fac.City = updcity.Text;
            fac.Contacts = updcontact.Text;
            fac.Email = updemail.Text;
            fac.FacID = updfaccode.Text;
            fac.FacName = updfacname.Text;
            fac.Fax = updfax.Text;
            fac.Note = updnote.Text;
            fac.Phone = updphone.Text;
            fac.PostCode = updpostcode.Text;
            fac.PriceLevel = Convert.ToInt32(updpricelevel.Text);
            fac.Province = updprovince.Text;
            fac.Responser = updresponser.Text;
            fac.TaxCode = updtaxcode.Text;
            PublicRequestHelp p = new PublicRequestHelp(this.busy,369,new object[]{fac},UpdCompleted);

        }

        private void UpdCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Clear();
                Search();
            }
        }

        private void delete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.ASP_Factory model = searchGrid.SelectedItem as API.ASP_Factory;
            if (model == null) { MessageBox.Show("请选择需删除的数据！"); return; }

            if (MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp p = new PublicRequestHelp(this.busy, 370, new object[] { model.ID }, UpdCompleted);
        }               
    }                    
}
