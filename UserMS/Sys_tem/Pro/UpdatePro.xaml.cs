using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Sys_tem.Pro
{
    public partial class UpdatePro : BasePage
    {
        API.ReportPagingParam pageParam;//全局变量分页内容
        public UpdatePro()
        {
            InitializeComponent();
            Get_Class_Type();
            GetSearch();
        }
        #region 查询
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = (int)this.pagesize.Value,
                ParamList = new List<API.ReportSqlParams>()
            };
            if (!string.IsNullOrEmpty(this.ClassName.Text))
            {
                API.ReportSqlParams_String ClassName = new API.ReportSqlParams_String();
                ClassName.ParamName = "ClassName";
                ClassName.ParamValues = this.ClassName.Text.Trim();
                pageParam.ParamList.Add(ClassName);
            }
            if (!string.IsNullOrEmpty(this.TypeName.Text))
            {
                API.ReportSqlParams_String TypeName = new API.ReportSqlParams_String();
                TypeName.ParamName = "TypeName";
                TypeName.ParamValues = this.TypeName.Text.Trim();
                pageParam.ParamList.Add(TypeName);
            }
            if (!string.IsNullOrEmpty(this.ProName.Text))
            {
                API.ReportSqlParams_String ProName = new API.ReportSqlParams_String();
                ProName.ParamName = "ProMainName";
                ProName.ParamValues = this.ProName.Text.Trim();
                pageParam.ParamList.Add(ProName);
            }
            this.InitPageEntity(MethodIDStore.GetProModel, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
            //PublicRequestHelp helper = new PublicRequestHelp(isbusy, MethodIDStore.UpdatePro, new object[] { pageParam }, MyClient_Completed);
        }

        private void GetSearch()
        {
            //取第一页的数据
            RadPager.PageSize = (int)this.pagesize.Value;
            pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = (int)this.pagesize.Value,
                ParamList = new List<API.ReportSqlParams>()
            };
            IsHall.ItemsSource = Store.ProHallInfo;
            IsHall.DisplayMemberPath = "HallName";
            IsHall.SelectedValuePath = "HallID";
            this.InitPageEntity(MethodIDStore.GetProModel, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
        }
        #endregion
        #region 更改行数
        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (pageParam != null)
            {
                this.RadPager.PageSize = (int)e.NewValue;
                pageParam.PageSize = RadPager.PageSize;
                this.InitPageEntity(MethodIDStore.GetProModel, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
                // PublicRequestHelp helper = new PublicRequestHelp(isbusy, MethodIDStore.UpdatePro, new object[] { pageParam }, MyClient_Completed);
            }

        }
        #region 转model
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {

                if (e.Result.ReturnValue == true)
                {
                    API.ReportPagingParam pageParem = (API.ReportPagingParam)e.Result.Obj;

                    List<API.View_ProInfo> ProList = pageParem.Obj as List<API.View_ProInfo>;
                    // this.ParentRadGrid.ItemsSource = OffList.Distinct();
                    OffChangeModel.ChangeModel(ProList, this.ProNameDG);
                    PagedCollectionView pagedCollection = new PagedCollectionView(new int[pageParem.RecordCount]);
                    this.RadPager.Source = pagedCollection;
                    this.RadPager.PageIndex = pageParem.PageIndex;
                }
                Logger.Log(e.Result.Message + "");
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                this.isbusy.IsBusy = false;
            }
        }
        #endregion
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
            this.InitPageEntity(MethodIDStore.GetProModel, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
            #endregion
        }
        #endregion
        #region 获取商品类别和品牌
        private void Get_Class_Type()
        {
            ClassName.ItemsSource = Store.ProClassInfo;
            ClassName.DisplayMemberPath = "ClassName";
            ClassName.SelectedValuePath = "ClassID";

            TypeName.ItemsSource = Store.ProTypeInfo;
            TypeName.DisplayMemberPath = "TypeName";
            TypeName.SelectedValuePath = "TypeID";

            ProName.ItemsSource = Store.ProMainInfo;
            ProName.DisplayMemberPath = "ProMainName";
            ProName.SelectedValuePath = "ProMainID";
        }
        #endregion

        private void ProNameDG_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            Clean();
            API.View_ProInfo ProInfo = this.ProNameDG.SelectedItem as API.View_ProInfo;
            if (ProInfo == null) return;
            this.ProInfoContent.DataContext = ProInfo;
            if (this.BeforeSep.IsChecked == true)
            {
                this.BeforeRate.IsEnabled = true;
            }
            else
            {
                this.BeforeRate.IsEnabled = false;
            }
            if (this.AfterSep.IsChecked == true)
            {
                this.AfterRate.IsEnabled = true;
            }
            else
            {
                this.AfterRate.IsEnabled = false;
            }
            #region 延保操作
            List<API.Pro_YanbaoPriceStepInfo> YanBaoList = new List<API.Pro_YanbaoPriceStepInfo>();
            foreach (var Item in Store.YanbaoPriceStep)
            {
                API.Pro_YanbaoPriceStepInfo YanBao = new API.Pro_YanbaoPriceStepInfo();
                YanBao.ID = Item.ID;
                YanBao.Name = Item.Name;
                YanBao.ProPrice = decimal.Parse(Item.ProPrice.ToString("#0.0000"));
                YanBao.StepPrice = decimal.Parse(Item.StepPrice.ToString("#0.0000"));
                YanBaoList.Add(YanBao);
            }
            DGYanBao.ItemsSource = YanBaoList;
            var queryItem = from b in YanBaoList
                            where b.ID == ProInfo.YanBaoModelID
                            select b;
            if (queryItem.Count() == 1)
            {
                DGYanBao.SelectedItems.Add(queryItem.First());
            }
            #endregion
        }

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.View_ProInfo Pro = this.ProNameDG.SelectedItem as API.View_ProInfo;
            if (Pro == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择任何项！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            API.Pro_ProInfo ProInfo = new API.Pro_ProInfo() { ProID = Pro.ProID };
            PublicRequestHelp helper = new PublicRequestHelp(isbusy, MethodIDStore.DelProInfo, new object[] { ProInfo }, Sumbit_Completed);
        }
        protected void Sumbit_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                if (e.Result.ReturnValue == true)
                {
                    API.View_ProInfo ProInfo = (API.View_ProInfo)this.ProNameDG.SelectedItem;
                    Store.ProInfo.RemoveAll(p => p.ProID == ProInfo.ProID);
                    
                    Clean();
                    this.InitPageEntity(MethodIDStore.GetProModel, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
                }
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        private void Clean()
        {
            this.ProInfoContent.DataContext = null;
            this.BeforeSep.IsChecked = false;
            this.BeforeRate.IsEnabled = false;
            this.AfterSep.IsChecked = false;
            this.AfterRate.IsEnabled = false;
            this.DGYanBao.ItemsSource = null;
            this.DGYanBao.Rebind();
        }
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定重置所有？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return;
            Clean();
            this.InitPageEntity(MethodIDStore.GetProModel, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
        }

        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.View_ProInfo Pro = this.ProNameDG.SelectedItem as API.View_ProInfo;
            if (Pro == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择任何项！");
                return;
            }
            API.Pro_ProInfo ProModel = new API.Pro_ProInfo();
            ProModel.ProID = Pro.ProID;
            if (string.IsNullOrEmpty(this.PrintName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入打印名称！");
                return;
            }
            ProModel.PrintName = this.PrintName.Text.Trim();
            if (string.IsNullOrEmpty(this.HasNeedIMEI.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择是否需要串码！");
                return;
            }
            ProModel.IsService = this.HasNeedIMEI.Text == "是" ? true : false;
            if (string.IsNullOrEmpty(this.HasService.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择是否需要串码！");
                return;
            }
            ProModel.IsService = this.HasNeedIMEI.Text == "是" ? true : false;

            if (string.IsNullOrEmpty(this.HasDecimals.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择是否需要串码！");
                return;
            }
            ProModel.ISdecimals = this.HasDecimals.Text == "是" ? true : false;
            if (this.SepDate.SelectedValue != null)
                ProModel.SepDate = this.SepDate.SelectedValue;

            if (BeforeSep.IsChecked == true)
            {
                ProModel.BeforeSep = true;
                ProModel.BeforeRate = (decimal)this.BeforeRate.Value;
            }
            else
                ProModel.BeforeSep = false;

            if (AfterSep.IsChecked == true)
            {
                ProModel.AfterSep = true;
                ProModel.AfterRate = (decimal)this.AfterRate.Value;
            }
            else
                ProModel.AfterSep = false;
            ProModel.TicketLevel = (decimal)this.TicketLevel.Value;
            ProModel.BeforeTicket = (decimal)this.BeforeTicket.Value;
            ProModel.BeforeTicket = (decimal)this.BeforeTicket.Value;
            ProModel.AfterTicket = (decimal)this.AfterTicket.Value;
            ProModel.NeedMoreorLess = this.HasNeedMoreorLess.Text == "是" ? true : false;
            ProModel.ISdecimals = this.HasDecimals.Text == "是" ? true : false;
            ProModel.IsService = this.HasService.Text == "是" ? true : false;
            ProModel.NeedIMEI = this.HasNeedIMEI.Text == "是" ? true : false;
            if (this.IsHall.SelectedValue != null)
            {
                ProModel.AirHallID = this.IsHall.SelectedValue.ToString();
            }
            ProModel.Note = this.Note.Text.Trim();
            API.Pro_YanbaoPriceStepInfo yanbao = this.DGYanBao.SelectedItem as API.Pro_YanbaoPriceStepInfo;
            if (yanbao != null)
                ProModel.YanBaoModelID=yanbao.ID;
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定修改？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp helper = new PublicRequestHelp(isbusy, MethodIDStore.UpdatePro, new object[] { ProModel }, Sumbit_CompletedUpdate);
        }
        protected void Sumbit_CompletedUpdate(object sender, API.MainReportCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message);
                if (e.Result.ReturnValue == true)
                {
                    Clean();
                    API.Pro_ProInfo ProModel = (API.Pro_ProInfo)e.Result.Obj;
                    API.View_ProInfo ProInfo = (API.View_ProInfo)this.ProNameDG.SelectedItem;
                    Store.ProInfo.RemoveAll(p=>p.ProID==ProModel.ProID);
                    Store.ProInfo.Add(ProModel);
                    #region MyRegion
                    ProInfo.AfterRate = ProModel.AfterRate;
                    ProInfo.AfterSep = ProModel.AfterSep;
                    ProInfo.AfterTicket = ProModel.AfterTicket;
                    ProInfo.BeforeRate = ProModel.BeforeRate;
                    ProInfo.BeforeSep = ProModel.BeforeSep;
                    ProInfo.BeforeTicket = ProModel.BeforeTicket;
                    ProInfo.ISdecimals = ProModel.ISdecimals;
                    ProInfo.IsService = ProModel.IsService;
                    ProInfo.NeedIMEI = ProModel.NeedIMEI;
                    ProInfo.NeedMoreorLess = ProModel.NeedMoreorLess;
                    ProInfo.Note = ProModel.Note;
                    ProInfo.PrintName = ProModel.PrintName;
                    ProInfo.SepDate = ProModel.SepDate;
                    ProInfo.TicketLevel = ProModel.TicketLevel;
                    ProInfo.YanBaoModelID = ProModel.YanBaoModelID;
                    ProInfo.AirHallID = ProModel.AirHallID;

                    ProInfo.AfterRate = ProModel.AfterRate;
                    ProInfo.AfterSep = ProModel.AfterSep;
                    ProInfo.AfterTicket = ProModel.AfterTicket;
                    ProInfo.BeforeRate = ProModel.BeforeRate;
                    ProInfo.BeforeSep = ProModel.BeforeSep;
                    ProInfo.BeforeTicket = ProModel.BeforeTicket;
                    ProInfo.ISdecimals = ProModel.ISdecimals;
                    ProInfo.IsService = ProModel.IsService;
                    ProInfo.NeedIMEI = ProModel.NeedIMEI;
                    ProInfo.NeedMoreorLess = ProModel.NeedMoreorLess;
                    ProInfo.Note = ProModel.Note;
                    ProInfo.PrintName = ProModel.PrintName;
                    ProInfo.SepDate = ProModel.SepDate;
                    ProInfo.TicketLevel = ProModel.TicketLevel;
                    ProInfo.YanBaoModelID = ProModel.YanBaoModelID;
                    ProInfo.AirHallID = ProModel.AirHallID;
                    #endregion
                    

                    this.InitPageEntity(MethodIDStore.GetProModel, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
                }
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务端出错！");
        }
        #region check 值发生改变时
        private void BeforeSep_Checked(object sender, RoutedEventArgs e)
        {
            if (this.BeforeSep.IsChecked == true)
            {
                this.BeforeRate.IsEnabled = true;
            }
        }
        private void AfterSep_Checked(object sender, RoutedEventArgs e)
        {
            if (this.AfterSep.IsChecked == true)
            {
                this.AfterRate.IsEnabled = true;
            }
        }
        private void BeforeSep_Unchecked(object sender, RoutedEventArgs e)
        {
            this.BeforeRate.IsEnabled = false;
        }

        private void AfterSep_Unchecked(object sender, RoutedEventArgs e)
        {
            this.AfterRate.IsEnabled = false;
        }
        #endregion
    }
}
