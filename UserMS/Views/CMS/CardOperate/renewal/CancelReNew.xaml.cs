using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Views.ProSell.VipOff
{
    public partial class CancelReNew : BasePage
    {
   
        double Cash_Ratio;//现金续期比例
        double Point_Ratio;//积分续期比例
        const int SumbitMethodID=58;
        bool IsCheck=false; 
        API.ReportPagingParam pageParam;//全局变量分页内容
        API.Sys_Option PointOption;
        API.Sys_Option CashOption;
        API.View_VIP_RenewBackAduit VIPInfo;
        public CancelReNew()
        {
            InitializeComponent();
            InitGrid2();
        }

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
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要取消续期的会员");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定取消续期？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            API.View_VIP_RenewBackAduit AduitVIP = dataGrid1.SelectedItem as API.View_VIP_RenewBackAduit;
            API.VIP_RenewBack BackRenew= new API.VIP_RenewBack();
            BackRenew.AduitID = AduitVIP.ID;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, SumbitMethodID, new object[] { BackRenew }, SubmitCompleted);
        }
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.Message != null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
                    Logger.Log(re.Result.Message);
                }
                if (re.Result.ReturnValue == true)
                {
                    if (re.Result.Obj == null)
                        return;
                    SelectChanged();
                    TbAduitID.Text = string.Empty;
                    TbReturnMoney.Text = string.Empty;
                    TbReturnPoint.Text = string.Empty;
                    BackTime.SelectedValue = null;

                    API.View_VIP_RenewBackAduit NewVIPInfo = re.Result.Obj as API.View_VIP_RenewBackAduit;
                    API.View_VIP_RenewBackAduit SelectVIP = dataGrid1.SelectedItem as API.View_VIP_RenewBackAduit;

                    SelectVIP.NewEndTime = NewVIPInfo.NewEndTime;
                    SelectVIP.EndTime = NewVIPInfo.EndTime;
                  //  SelectVIP.Validity = NewVIPInfo.Validity;
                    SelectVIP.Point = NewVIPInfo.Point;
                    SelectVIP.SysDate = NewVIPInfo.SysDate;
                    IsCheck = false;
                    this.InitPageEntity(UserMS.Common.MethodIDStore.Method_GetRenewID, this.dataGrid1, this.busy, this.RadPager, pageParam);
                }
           
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
        }
        #endregion
        // 当用户导航到此页面时执行。
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

            //if (!string.IsNullOrEmpty(this.StartTime.SelectedValue.ToString()))
            //{
               API.ReportSqlParams_String Aduited = new API.ReportSqlParams_String();
               Aduited.ParamName = "Aduited";
               Aduited.ParamValues = "Y";
               pageParam.ParamList.Add(Aduited);

               API.ReportSqlParams_String Passed = new API.ReportSqlParams_String();
               Passed.ParamName = "Passed";
               Passed.ParamValues = "Y";
               pageParam.ParamList.Add(Passed);

               API.ReportSqlParams_String Used = new API.ReportSqlParams_String();
               Used.ParamName = "Used";
               Used.ParamValues = "N";
               pageParam.ParamList.Add(Used);
            //}

            //if (!string.IsNullOrEmpty(this.EndTime.SelectedValue.ToString()))
            //{
            //    API.ReportSqlParams_DataTime EndTime = new API.ReportSqlParams_DataTime();
            //    EndTime.ParamName = "EndTime";
            //    EndTime.ParamValues = this.EndTime.SelectedValue;
            //    pageParam.ParamList.Add(EndTime);
            //}
            if (pageParam.ParamList.Count() > 0)
            {
                //CleanVIP();
                this.InitPageEntity(UserMS.Common.MethodIDStore.Method_GetRenewID, this.dataGrid1, this.busy, this.RadPager, pageParam);
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
            col2.DataMemberBinding = new System.Windows.Data.Binding("VIPType");
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

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("CardTypeName");
            col5.Header = "证件类型";
            this.dataGrid1.Columns.Add(col5);

            GridViewDataColumn col51 = new GridViewDataColumn();
            col51.DataMemberBinding = new System.Windows.Data.Binding("IDCard");
            col51.Header = "证件号码";
            this.dataGrid1.Columns.Add(col51);



            GridViewDataColumn col6 = new GridViewDataColumn();
            col6.DataMemberBinding = new System.Windows.Data.Binding("MobiPhone");
            col6.Header = "手机号码";
            this.dataGrid1.Columns.Add(col6);

            GridViewDataColumn col61 = new GridViewDataColumn();
            col61.DataMemberBinding = new System.Windows.Data.Binding("Point");
            col61.Header = "积分";
            this.dataGrid1.Columns.Add(col61);


            GridViewDataColumn col7 = new GridViewDataColumn();
            col7.DataMemberBinding = new System.Windows.Data.Binding("StartTime");
            col7.Header = "注册时间";
            this.dataGrid1.Columns.Add(col7);

            GridViewDataColumn col71 = new GridViewDataColumn();
            col71.DataMemberBinding = new System.Windows.Data.Binding("NewEndTime");
            col71.Header = "有效期至";
            this.dataGrid1.Columns.Add(col71);



        }
        #endregion
        #region 选择项改变时发生
        private void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            SelectChanged();
        }
        void SelectChanged()
        {
            CleanPart();
            if (dataGrid1.SelectedItem != null)
            {
                VIPInfo = dataGrid1.SelectedItem as API.View_VIP_RenewBackAduit;
                this.OldCardPanel.DataContext = VIPInfo;
                var query = from b in Store.Options
                            where b.ID == 1 || b.ID == 2
                            select b;
                foreach (var next in query)
                {
                    if (next.ID == 1)
                    {
                        CashOption = new API.Sys_Option();
                        CashOption = next;
                        TbRatio_Cash.Text = next.Value + "：" + next.Value2;
                        Cash_Ratio = (double)int.Parse(next.Value) / int.Parse(next.Value2);
                    }
                    if (next.ID == 2)
                    {
                        PointOption = new API.Sys_Option();
                        PointOption = next;
                        TbRatio_Point.Text = next.Value + "：" + next.Value2;
                        Point_Ratio = (double)int.Parse(next.Value) / int.Parse(next.Value2);
                    }
                }
                
            }
        }
        #endregion
        #region 晴空部分内容
        void CleanPart()
        {
            this.OldCardPanel.DataContext = null;
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
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要取消续期的会员");
                return;
            }
           // API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
           //API.VIP_VIPBackAduit Aduit = new API.VIP_VIPBackAduit();
           //Aduit.VIP_ID = VIPInfo.ID; 
           //string s=VIPInfo.AduitID.Trim();
           //int result = String.Compare(TbAduitID.Text.Trim(), s);
            if (VIPInfo.AduitID.Trim() == TbAduitID.Text.Trim())
            {
                IsCheck = true;
                TbReturnMoney.Text = VIPInfo.AduitMoney.ToString();
                TbReturnPoint.Text = VIPInfo.AduitPoint.ToString();
                int days=(int)VIPInfo.BackValidity;
                TimeSpan Span = new TimeSpan(days, 0, 0, 0);
                BackTime.SelectedValue = VIPInfo.EndTime.Subtract(Span);
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批单不属于该会员或审批单不存在！");
            
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
