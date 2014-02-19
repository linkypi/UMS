using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace UserMS.Views.ProSell.VipOff
{
    public partial class CancelRegister : BasePage
    {
        const int GetVIPMethodID = 3;
        const int GetTypeMothodID = 40;
        const int MethodID_Sumbit = 44;
        const int GetServiceMothodID = 91;
        const int SumbitMethodID = 55;
        const int GetAduit = 57;
        public bool IsCheck = false;
        API.ReportPagingParam pageParam;//会员信息分页全局变量
        API.ReportPagingParam pageParam1;//服务分页全局变量
        public CancelRegister()
        {
            InitializeComponent();
            InitGrid2();
        }

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
                APartVIP();
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
            col61.Header = "加入时间";
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
            CleanPartVIP();
            if (dataGrid1.SelectedItem != null)
            {
                API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
                this.OldCardPanel.DataContext = VIPInfo;
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
                CheckAduit();
            }
        }
        #endregion
        #region  检测审批单有效性
        void CheckAduit()
        {
            if (string.IsNullOrEmpty(TbAduitID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写审批单号");
                return;
            }
            if (this.dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要退卡的会员");
                return;
            }
            API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
            API.VIP_VIPBackAduit Aduit = new API.VIP_VIPBackAduit();
            Aduit.VIP_ID = VIPInfo.ID;
            Aduit.AduitID = TbAduitID.Text.Trim();
            PublicRequestHelp help = new PublicRequestHelp(this.busy, GetAduit, new object[] { Aduit }, SearchCompleted);
        }
        private void SearchCompleted(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                Logger.Log(re.Result.Message);
                if (re.Result.ReturnValue == true)
                {
                    if (re.Result.Obj == null)
                        return;
                    IsCheck = true;
                    API.VIP_VIPBackAduit BackAduit = re.Result.Obj as API.VIP_VIPBackAduit;
                    TbBackMoney.Text = BackAduit.Money.ToString();
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
            this.TbAduitID.Text = string.Empty;
            this.TbBackMoney.Text = string.Empty;
            this.OldCardPanel.DataContext = null;
            dataGrid1.ItemsSource = null;
            dataGrid1.Rebind();
            DGservice.ItemsSource = null;
            DGservice.Rebind();

        }
        #endregion
        #region 清空部分会员内容
        void APartVIP()
        {
            this.TbBackMoney.Text = string.Empty;
            this.OldCardPanel.DataContext = null;
            dataGrid1.ItemsSource = null;
            dataGrid1.Rebind();
            DGservice.ItemsSource = null;
            DGservice.Rebind();

        }
        #endregion
        #region 清空部分内容
        void CleanPartVIP()
        {
            //this.TbAduitID.Text = string.Empty;
            this.TbBackMoney.Text = string.Empty;
            this.OldCardPanel.DataContext = null;
            DGservice.ItemsSource = null;
            DGservice.Rebind();

        }
        #endregion
        #region 提交内容
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbAduitID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未填写审批单号");
                return;
            }
            if (IsCheck == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未检测审批单或审批单不存在");
                return;
            }
            if (this.dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要退卡的会员");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定退卡退款？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
            API.VIP_VIPBack BackVIP = new API.VIP_VIPBack();
            BackVIP.VIP_ID = VIPInfo.ID;
            BackVIP.AduitID = TbAduitID.Text.Trim();
            PublicRequestHelp help = new PublicRequestHelp(this.busy, SumbitMethodID, new object[] { BackVIP }, SubmitCompleted);
        }
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                Logger.Log(re.Result.Message);
                if (re.Result.ReturnValue == true)
                {
                    CleanVIP();
                }
                 MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
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
            this.InitPageEntity(GetVIPMethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
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
        #region 检测
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckAduit();
        }
        #endregion 
    }
}
