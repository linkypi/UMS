using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.ProSell.VipOff
{
    public partial class Change_LevelUp : BasePage
    {
        const int GetVIPMethodID = 3;
        const int GetTypeMothodID = 40;
        const int MethodID_Sumbit = 44;
        const int GetServiceMothodID = 91;
        const int SumbitMethodID = 52;
        public bool IsCheck = false;
        API.ReportPagingParam pageParam;//会员信息分页全局变量
        API.ReportPagingParam pageParam1;//服务分页全局变量
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public Change_LevelUp()
        {

            InitializeComponent();
            InitGrid2();
            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
            UserOpList =
    userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
                          (list, info) => new { op = list, user = info })
         .Join(Store.UserOp, arg => arg.op.OpID, op => op.OpID, (a, t) => new UserOpModel()
         {
             ID = a.op.ID,
             HallID = a.op.HallID,
             OpID = a.op.OpID,
             UserID = a.op.UserID,
             Username = a.user.RealName,
             opname = t.Name
         }).ToList();

            var userinfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model => model.UserID,
                                                 (info, model) => info).ToList();
            this.Seller.ItemsSource = userinfos;
            this.Seller.TextSearchPath = "RealName";
            this.Seller.SearchEvent = SellerSearchEvent;
            this.Seller.SelectionMode = AutoCompleteSelectionMode.Single;
            this.Seller.TextBox_SelectionChanged = SellerSelectEvent;
        }
        

        #region 获取揽装人

        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
            Sys_UserInfo selected = Seller.SelectedItem as Sys_UserInfo;
            if (selected != null)
            {
                this.Seller.Tag = selected.UserID;
            }
            else
            {
                this.Seller.Tag = null;
            }

        }

        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
                                                  "Username", new string[] { "Username", "opname" },
                                                  new string[] { "用户名", "职位" });

            w.Closed += SellerSearchWindowClose;
            w.ShowDialog();
        }

        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    UserOpModel selected = (UserOpModel)window.SelectedItem;
                    this.Seller.Tag = selected.UserID;
                    this.Seller.TextBox.SearchText = selected.Username;
                }
            }
        }

        #endregion
        #region 提交操作
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtSumbit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要操作的会员卡！");
                return;
            }
            if (string.IsNullOrEmpty(TbChangedID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入新的会员卡号！");
                return;
            }
            if (CBBK.IsChecked == false && CBHK.IsChecked == false && CBSJK.IsChecked == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择操作类型！");
                return;
            }
            if (IsCheck == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该卡号不存在或未检测！");
                return;
            }
            if (CBBK.IsChecked == true)
            {
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定进行" + CBBK.Content + "操作？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
            }
            if (CBHK.IsChecked == true)
            {
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定进行" + CBHK.Content + "操作？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
            }
            if (CBSJK.IsChecked == true)
            {
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定进行" + CBSJK.Content + "操作？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
            }
           
            API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
            if (VIPInfo == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择换卡的会员！");
                return;
            }
            API.VIP_VIPInfo_Temp Temp = new API.VIP_VIPInfo_Temp();
            Temp.Note = TbNote.Text.Trim();
            Temp.IMEI = TbChangedID.Text.Trim();
            Temp.OldID = TbOldID.Text.Trim();
            try
            {
                Temp.Point = decimal.Parse(SPoint.Text.Trim());
            }
            catch { }
            if (string.IsNullOrEmpty(Seller.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入揽装人！");
                return;
            }
            try
            {
                Temp.LZUser = Seller.Tag.ToString();
            }
            catch
            {
                if (string.IsNullOrEmpty(Seller.Text))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入揽装人！");
                    return;
                }
                var query = from b in Store.UserInfos
                            where b.UserName == Seller.Text
                            select b;
                if (query.Count() == 0 || string.IsNullOrEmpty(query.First().UserID))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "不存在揽装人！");
                    return;
                }
            }

            PublicRequestHelp help = new PublicRequestHelp(this.busy, SumbitMethodID, new object[] { Temp,VIPInfo.ID }, SubmitCompleted);

        }
        void SubmitCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Result.Message != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
            }
            if (results.Result.ReturnValue == true)
            {
                CleanAllVIP();           
                CleanNewCard();
                IsCheck = false;
                this.Seller.TextBox.SearchText = string.Empty;
                this.Seller.Tag = null;
                this.TbOldID.Text = string.Empty;
                this.TbNote.Text = string.Empty;
            }
            Logger.Log(results.Result.Message);
        }
        #endregion

        #region 查询会员信息
        void BTVIPsearch_Click(object sender, RoutedEventArgs e)
        {
            pageParam = new API.ReportPagingParam();
            pageParam.PageIndex = this.RadPager.PageIndex;
            pageParam.PageSize = this.RadPager.PageSize;
            pageParam.ParamList = new List<API.ReportSqlParams>();
            //会员卡号查询
            if (!string.IsNullOrEmpty(this.TbIMEI.Text))
            {
                API.ReportSqlParams_String IMEI = new API.ReportSqlParams_String();
                IMEI.ParamName = "IMEI";
                IMEI.ParamValues = TbIMEI.Text.Trim();
                pageParam.ParamList.Add(IMEI);
            }
            //证件号码查询
            if (!string.IsNullOrEmpty(this.TbIDCardNun.Text))
            {
                API.ReportSqlParams_String IDCard = new API.ReportSqlParams_String();
                IDCard.ParamName = "IDCard";
                IDCard.ParamValues = this.TbIDCardNun.Text.Trim();
                pageParam.ParamList.Add(IDCard);
            }
            //会员姓名
            if (!string.IsNullOrEmpty(this.VIPName.Text))
            {
                API.ReportSqlParams_String VIPName = new API.ReportSqlParams_String();
                VIPName.ParamName = "MemberName";
                VIPName.ParamValues = this.VIPName.Text.Trim();
                pageParam.ParamList.Add(VIPName);
            }

            if (!string.IsNullOrEmpty(this.MobilePhone.Text))
            {
                API.ReportSqlParams_String MobilePhone = new API.ReportSqlParams_String();
                MobilePhone.ParamName = "MobiPhone";
                MobilePhone.ParamValues = this.MobilePhone.Text.Trim();
                pageParam.ParamList.Add(MobilePhone);
            }

            if (!string.IsNullOrEmpty(this.StartTime.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime StartTime = new API.ReportSqlParams_DataTime();
                StartTime.ParamName = "StartTime";
                StartTime.ParamValues = this.StartTime.SelectedValue;
                pageParam.ParamList.Add(StartTime);
            }

            if (!string.IsNullOrEmpty(this.EndTime.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime EndTime = new API.ReportSqlParams_DataTime();
                EndTime.ParamName = "EndTime";
                EndTime.ParamValues = this.EndTime.SelectedValue;
                pageParam.ParamList.Add(EndTime);
            }
            if (pageParam.ParamList.Count() > 0)
            {
                CleanAllVIP();
                this.InitPageEntity(GetVIPMethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            }       
        }
        #endregion
        #region 生成列
        private void InitGrid2()
        {
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("IMEI");

            col.Header = "卡号";
            this.dataGrid1.Columns.Add(col);


            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("VIPTypeName");
            col2.Header = "卡类型";
            this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("MemberName");
            col3.Header = "会员姓名";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("Sex");
            col4.Header = "性别";
            this.dataGrid1.Columns.Add(col4);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("Birthday");
            col41.Header = "出生日期";
            this.dataGrid1.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("MobiPhone");
            col5.Header = "手机号码";
            this.dataGrid1.Columns.Add(col5);

            GridViewDataColumn col51 = new GridViewDataColumn();
            col51.DataMemberBinding = new System.Windows.Data.Binding("Telephone");
            col51.Header = "电话号码";
            this.dataGrid1.Columns.Add(col51);


            GridViewDataColumn col52 = new GridViewDataColumn();
            col52.DataMemberBinding = new System.Windows.Data.Binding("Address");
            col52.Header = "地址";
            this.dataGrid1.Columns.Add(col52);

            GridViewDataColumn col6 = new GridViewDataColumn();
            col6.DataMemberBinding = new System.Windows.Data.Binding("QQ");
            col6.Header = "QQ";
            this.dataGrid1.Columns.Add(col6);

            GridViewDataColumn col61 = new GridViewDataColumn();
            col61.DataMemberBinding = new System.Windows.Data.Binding("StartTime");
            col61.Header = "注册时间";
            this.dataGrid1.Columns.Add(col61);

            GridViewDataColumn col7 = new GridViewDataColumn();
            col7.DataMemberBinding = new System.Windows.Data.Binding("IDCardName");
            col7.Header = "证件类型";
            this.dataGrid1.Columns.Add(col7);

            GridViewDataColumn col71 = new GridViewDataColumn();
            col71.DataMemberBinding = new System.Windows.Data.Binding("IDCard");
            col71.Header = "证件号码";
            this.dataGrid1.Columns.Add(col71);

            GridViewDataColumn col8 = new GridViewDataColumn();
            col8.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            col8.Header = "揽装门店";
            this.dataGrid1.Columns.Add(col8);

            GridViewDataColumn col81 = new GridViewDataColumn();
            col81.DataMemberBinding = new System.Windows.Data.Binding("UpdUserName");
            col81.Header = "业务员";
            this.dataGrid1.Columns.Add(col81);
        }
        #endregion

        #region 选择项改变时发生
        private void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            CleanVIP();
            if (dataGrid1.SelectedItem != null)
            {
                API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
                this.OldCardPanel.DataContext = VIPInfo;
                //if (!string.IsNullOrEmpty(TbPoint.Text))        
                pageParam1 = new API.ReportPagingParam();
                pageParam1.PageIndex = this.RadPager.PageIndex;
                pageParam1.PageSize = this.RadPager.PageSize;
                pageParam1.ParamList = new List<API.ReportSqlParams>();

                if (VIPInfo.ID > 0)
                {
                    API.ReportSqlParams_String VIPID = new API.ReportSqlParams_String();
                    VIPID.ParamName = "ID";
                    VIPID.ParamValues = VIPInfo.ID.ToString();
                    pageParam1.ParamList.Add(VIPID);
                }            
                this.InitPageEntity(GetServiceMothodID, this.DGservice, this.busy, this.ServiceRadPager, pageParam1);
            }
            TbSPoint.Text = TbPoint.Text;
        }
        #endregion
        
        #region 按下确定键时发生
        /// <summary>
        /// 按下确定键时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckCard();
            }
        }
        #endregion
        #region 检测会员卡是否可用
        void CheckCard()
        {
            if (string.IsNullOrEmpty(TbChangedID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加新卡号");
                return;
            }
            PublicRequestHelp help = new PublicRequestHelp(this.busy, GetTypeMothodID, new object[] { TbChangedID.Text }, SearchCompleted);
        }
        private void SearchCompleted(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            CleanPartCard();
            if (re.Error == null)
            {
                Logger.Log(re.Result.Message);
                if (re.Result.ReturnValue == true)
                {
                    if (re.Result.Obj == null)
                        return;
                    IsCheck = true;//已检测
                    API.VIP_VIPType TypeInfo = re.Result.Obj as API.VIP_VIPType;
                    this.NewCardPanel.DataContext = TypeInfo;
                    if (TypeInfo.VIP_VIPTypeService != null)
                    {
                        new GetTypeService(TypeInfo.VIP_VIPTypeService, ref NewDGService);
                    }
                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");

        }
        #endregion 
        #region 清空会员内容
        void CleanVIP()
        {
            this.OldCardPanel.DataContext = null;
            DGservice.ItemsSource = null;
            DGservice.Rebind();

        }
        void CleanAllVIP()
        {
            this.OldCardPanel.DataContext = null;
            dataGrid1.ItemsSource = null;
            dataGrid1.Rebind();
            DGservice.ItemsSource = null;
            DGservice.Rebind();
        }
        #endregion
        #region 清空新卡内容
        void CleanNewCard()
        {
            this.NewCardPanel.DataContext = null;
            TbChangedID.Text = string.Empty;
            TbSPoint.Text = string.Empty;
            CBBK.IsChecked = false;
            CBHK.IsChecked = false;
            CBSJK.IsChecked = false;
            NewDGService.ItemsSource = null;
            NewDGService.Rebind();
        }
        void CleanPartCard()
        {
            this.NewCardPanel.DataContext = null;
            NewDGService.ItemsSource = null;
            NewDGService.Rebind();
        }
        #endregion

        #region 取服务下一页数据
        private void RadPager_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            #region 取第一页的数据
            pageParam1.PageIndex = e.NewPageIndex;
            this.InitPageEntity(GetServiceMothodID, this.DGservice, this.busy, this.ServiceRadPager, pageParam1);
            #endregion
        }
        #endregion
        #region 取会员信息下一页数据
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取第一页的数据

            pageParam.PageIndex = e.NewPageIndex;
            this.InitPageEntity(GetVIPMethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion
        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定重置当前数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
               // CleanVIP();
                CleanNewCard();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckCard();
        }
    }
}
