using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.UsersellMS.Usercenter
{
    public partial class VIPMS : BasePage
    {
        List<SlModel.VIPModel> vipmodel = null;
        int MethodID = 3;
        int GetServiceMothodID = 91;
        int MethodID_Sumbit = 44;
        int ID = 0;
        API.ReportPagingParam pageParam;//会员信息分页全局变量
        API.ReportPagingParam pageParam1;//服务分页全局变量
        public VIPMS()
        {
            InitializeComponent();
            InitGrid2();
            this.BTVIPsearch.Click += BTVIPsearch_Click;
            this.BtSumbit.Click += BtSumbit_Click;
            //给身份类型添加资源
            tbIDtype.ItemsSource = Store.CardType;
            tbIDtype.SelectedValuePath = "ID";
            tbIDtype.DisplayMemberPath = "Name";

        }
        #region 提交修改
        void BtSumbit_Click(object sender, RoutedEventArgs e)
        {

            API.VIP_VIPInfo vipinfo = new API.VIP_VIPInfo();
            if (ID == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择你要修改的会员！");
                return;
            }

            if (this.tbName.Text == null || this.tbName.Text.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"会员姓名不能为空");
                return;
            }
            //if (this.cbsex.Text == null || this.cbsex.Text.Trim() == "")
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择性别");
            //    return;
            //}
            if (this.cbsex.Text != null)
            {
                vipinfo.Sex = this.cbsex.Text.Trim();
            }
            //if (this.birthday == null || this.birthday.SelectedValue.ToString().Trim() == "")
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择生日");
            //    return;
            //}
            if (this.birthday.SelectedValue != null)
            {
                vipinfo.Birthday = this.birthday.SelectedValue;
            }

            if (this.phoneNum.Text == null || this.phoneNum.Text.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写手机号码");
                return;
            }
            if (!PormptPage.isNumeric(phoneNum.Text.Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写正确的手机号码！");
                return;
            }


            vipinfo.ID = ID;
            vipinfo.MemberName = this.tbName.Text.Trim();
            //vipinfo.Sex = this.cbsex.Text.Trim();
            //vipinfo.Birthday = this.birthday.SelectedValue;
            vipinfo.MobiPhone = this.phoneNum.Text.Trim();
            vipinfo.TelePhone = this.telephone.Text.Trim();
            vipinfo.QQ = this.QQ.Text.Trim();
            vipinfo.Address = this.tbaddress.Text.Trim();
            vipinfo.IDCard = this.tbIDnum.Text.Trim();
            if (tbIDtype.SelectedValue != null)
            {
                vipinfo.IDCard_ID = (int)tbIDtype.SelectedValue;
            }
            vipinfo.Note = tbNote.Text.Trim();
            //vipinfo.UpdUser = this.tbtranstor.Text.Trim();
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否修改会员资料？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodID_Sumbit, new object[] { vipinfo }, Completed);
        }
        #endregion
        #region 提交完成
        private void Completed(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.ReturnValue == true)
                {
                    if (re.Result.Obj == null)
                        return;
                    API.View_VIPInfo VIPInfo = re.Result.Obj as API.View_VIPInfo;
                    API.View_VIPInfo SelectVIP = dataGrid1.SelectedItem as API.View_VIPInfo;

                    SelectVIP.MemberName = VIPInfo.MemberName;
                    SelectVIP.Sex = VIPInfo.Sex;
                    SelectVIP.Birthday = VIPInfo.Birthday;
                    SelectVIP.TelePhone = VIPInfo.TelePhone;
                    SelectVIP.MobiPhone = VIPInfo.MobiPhone;
                    SelectVIP.QQ = VIPInfo.QQ;
                    SelectVIP.IDCard = VIPInfo.IDCard;
                    SelectVIP.IDCardName = VIPInfo.IDCardName;
                }
                if (re.Result.Message != null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
                    Logger.Log(re.Result.Message);
                }
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
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
            if (pageParam.ParamList.Count() > 0)
            {
                AllClean();
                this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            }
        }
        #endregion
        #region 生成列
        private void InitGrid2()
        {
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("IMEI");
            col.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col.Header = "卡号";
            this.dataGrid1.Columns.Add(col);


            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("VIPTypeName");
            col2.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col2.Header = "卡类型";
            this.dataGrid1.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("MemberName");
            col3.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col3.Header = "会员姓名";
            this.dataGrid1.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("Sex");
            col4.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col4.Header = "性别";
            this.dataGrid1.Columns.Add(col4);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("Birthday");
            col41.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col41.Header = "出生日期";
            this.dataGrid1.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("MobiPhone");
            col5.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col5.Header = "手机号码";
            this.dataGrid1.Columns.Add(col5);

            GridViewDataColumn col51 = new GridViewDataColumn();
            col51.DataMemberBinding = new System.Windows.Data.Binding("Telephone");
            col51.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col51.Header = "电话号码";
            this.dataGrid1.Columns.Add(col51);


            GridViewDataColumn col52 = new GridViewDataColumn();
            col52.DataMemberBinding = new System.Windows.Data.Binding("Address");
            col52.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col52.Header = "地址";
            this.dataGrid1.Columns.Add(col52);

            GridViewDataColumn col6 = new GridViewDataColumn();
            col6.DataMemberBinding = new System.Windows.Data.Binding("QQ");
            col6.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col6.Header = "QQ";
            this.dataGrid1.Columns.Add(col6);

            GridViewDataColumn col61 = new GridViewDataColumn();
            col61.DataMemberBinding = new System.Windows.Data.Binding("StartTime");
            col61.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col61.Header = "注册日期";
            this.dataGrid1.Columns.Add(col61);

            GridViewDataColumn col7 = new GridViewDataColumn();
            col7.DataMemberBinding = new System.Windows.Data.Binding("IDCardName");
            col7.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col7.Header = "证件类型";
            this.dataGrid1.Columns.Add(col7);

            GridViewDataColumn col71 = new GridViewDataColumn();
            col71.DataMemberBinding = new System.Windows.Data.Binding("IDCard");
            col71.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col71.Header = "证件号码";
            this.dataGrid1.Columns.Add(col71);

            GridViewDataColumn col8 = new GridViewDataColumn();
            col8.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            col8.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col8.Header = "揽装门店";
            this.dataGrid1.Columns.Add(col8);

            GridViewDataColumn col81 = new GridViewDataColumn();
            col81.DataMemberBinding = new System.Windows.Data.Binding("UpdUserName");
            col81.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col81.Header = "业务员";
            this.dataGrid1.Columns.Add(col81);
        }
        #endregion

        #region 选择项改变时发生
        private void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            SelectedChange();
        }
        #endregion
        #region 选择不同项时发生
        void SelectedChange()
        {
            PartClean();
            API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
            if (VIPInfo != null)
            {
                this.DataContext = VIPInfo;
                this.tbIDtype.Text = VIPInfo.IDCardName;
                pageParam1 = new API.ReportPagingParam();
                pageParam1.PageIndex = this.ServiceRadPager.PageIndex;
                pageParam1.PageSize = this.ServiceRadPager.PageSize;
                pageParam1.ParamList = new List<API.ReportSqlParams>();

                if (VIPInfo.ID > 0)
                {
                    ID = VIPInfo.ID;
                    API.ReportSqlParams_String VIPID = new API.ReportSqlParams_String();
                    VIPID.ParamName = "ID";
                    VIPID.ParamValues = VIPInfo.ID.ToString();
                    pageParam1.ParamList.Add(VIPID);
                }
                this.InitPageEntity(GetServiceMothodID, this.DGservice, this.busy, this.ServiceRadPager, pageParam1);
            }

        }
        #endregion
        #region 部分清空
        void PartClean()
        {
            this.tbIDtype.Text = string.Empty;
            this.DataContext = null;
            DGservice.ItemsSource = null;
            DGservice.Rebind();
        }
        #endregion
        #region 清空所有项
        void AllClean()
        {

            this.tbIDtype.Text = string.Empty;
            this.DataContext = null;
            dataGrid1.ItemsSource = null;
            dataGrid1.Rebind();
            DGservice.ItemsSource = null;
            DGservice.Rebind();
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
            //API.ReportPagingParam pageParam = new API.ReportPagingParam()
            //{
            //    PageIndex = e.NewPageIndex,
            //    PageSize = this.RadPager.PageSize,
            //    ParamList = new List<API.ReportSqlParams>()
            //};
            pageParam.PageIndex = e.NewPageIndex;
            this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion
        private void BtReset_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否重置所有数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                SelectedChange();
            }
        }
    }
}
