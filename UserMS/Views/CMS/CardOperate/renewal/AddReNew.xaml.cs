using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.VipOff
{
    public partial class AddReNew : BasePage
    {
       const int GetVIPMethodID = 3;
        const int GetTypeMothodID = 40;
        const int MethodID_Sumbit = 44;
        const int GetServiceMothodID = 91;
        const int SumbitMethodID = 54;
     
        double Cash_Ratio;//现金续期比例
        double Point_Ratio;//积分续期比例
        int Cash_AddDay=0;//现金续期天数 
        int Point_AddDay = 0;//积分续期天数

        const int MenuID = 36;
        HallFilter hAdder;//仓库添加器
        API.Sys_Option PointOption;
        API.Sys_Option CashOption;
        string r = "";
        API.ReportPagingParam pageParam;//全局变量分页内容
        public AddReNew()
        {
            InitializeComponent();
            InitGrid2();
            TbSellName.Text = Store.LoginUserInfo.UserName;
            this.Add_Cash.IsEnabled = false;
            this.Add_SPoint.IsEnabled = false;
            CheckCash.Checked +=CheckCash_Click;
            CheckPoint.Checked += CheckPoint_Click;
            //添加仓库
            this.TbHall.btnSearch.Click += btnSearch_Click;
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
            }
            finally
            {
                GetFristHall();
            }
        }
        private void GetFristHall()
        {
            hAdder = new HallFilter(true, ref TbHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            List<API.Pro_HallInfo> hall = hAdder.FilterHall(MenuID, HallInfo);
            if (hall.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该角色无此权限，请联系管理员");
                return;
            }
            this.TbHall.Text = hall.First().HallName;
            this.TbHall.Tag = hall.First().HallID;
        }
        #region 添加仓库
        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            hAdder = new HallFilter(true, ref TbHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            List<API.Pro_HallInfo> hall = hAdder.FilterHall(MenuID, HallInfo);
            if (hall.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该角色无此权限，请联系管理员");
                return;
            }
            hAdder.GetHall(hall);
        }
        #endregion
        #region CheckBox的IsChecked改变时发生
        void CheckPoint_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem == null)
            {
                CheckPoint.IsChecked = false;
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择会员！");
                return;
            }
            if (CheckPoint.IsChecked == true)
            {
                this.Add_SPoint.IsEnabled = true;
                this.Add_Cash.IsEnabled = false;
                this.Add_Cash.Value = 0;
            }
 
        }
        void CheckCash_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem == null)
            {
                CheckCash.IsChecked = false;
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择会员！");
                return;
            }
            if (CheckCash.IsChecked == true)
            {
                this.Add_Cash.IsEnabled = true;
                this.Add_SPoint.IsEnabled = false;
                this.Add_SPoint.Value = 0;
            }

        }
        #endregion

        #region 获取天数
        private void Add_Cash_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (Cash_Ratio > 0 )
            {
                if (Add_Cash.Value == null)
                    Add_Cash.Value = 0;
                double? Validity = Add_Cash.Value / Cash_Ratio;
                Cash_AddDay = (int)Validity;
                TextChanged();         
            }
        }


        void Add_SPoint_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {

            if (string.IsNullOrEmpty(TbPoint.Text))
            {
                Add_SPoint.Value = 0;  

            }
            else
            {
                //Add_SPoint.Maximum = int.Parse(TbPoint.Text);
                if (Add_SPoint.Value > int.Parse(TbPoint.Text.Trim()))
                {
                   
                    Add_SPoint.Value = int.Parse(TbPoint.Text.Trim());
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"所填积分不能超过会员积分！");
                    //return;
                }
            }
                if (Point_Ratio > 0)
                {
                    if (Add_SPoint.Value == null)
                        Add_SPoint.Value = 0;
                    double? Validity = Add_SPoint.Value / Point_Ratio;
                    Point_AddDay = (int)Validity;
                    TextChanged();
                }
                
           
        }
       #endregion
        #region 改变文本框数据
        void TextChanged()
        {
            TbAlready.Text = (Point_AddDay + Cash_AddDay).ToString();
            API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
            if (VIPInfo == null)
            {
                return;
            }
            if (VIPInfo.EndTime < DateTime.Now || VIPInfo.EndTime == null)
            {
                try
                {
                    TimeEnd.SelectedValue = DateTime.Now.AddDays(Point_AddDay + Cash_AddDay);
                }
                catch { }
            }
            else
            {
                try
                {
                    TimeEnd.SelectedValue = VIPInfo.EndTime.Value.AddDays(Point_AddDay + Cash_AddDay);
                }
                catch { }
            }
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
                MobilePhone.ParamName = "MobilePhone";
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
                CleanVIP();
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
            SelectChanged();
        }
        void SelectChanged()
        {
            CleanPart();
            if (dataGrid1.SelectedItem != null)
            {
                API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
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
        #region 清空部分内容
        void CleanPart()
        {
           this.OldCardPanel.DataContext = null;
           this.Add_Cash.Value = 0;
           this.Add_SPoint.Value = 0;
           TimeEnd.SelectedValue = null;
           Add_SPoint.IsEnabled = false;
           Add_Cash.IsEnabled = false;
           CheckCash.IsChecked = false;
           CheckPoint.IsChecked = false;            
        }
        #endregion
        #region 清空会员内容
        void CleanVIP()
        {          
            this.OldCardPanel.DataContext = null;
            this.Add_Cash.Value = 0;
            this.Add_SPoint.Value = 0;
            TimeEnd.SelectedValue = null;
            Add_SPoint.IsEnabled = false;
            Add_Cash.IsEnabled = false;
            CheckCash.IsChecked = false;
            CheckPoint.IsChecked = false;
            dataGrid1.ItemsSource = null;
            dataGrid1.Rebind();
        }
        #endregion
        #region 提交内容
        private void TbSumbit_Click(object sender, RoutedEventArgs e)
        {   
            if (this.dataGrid1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要续期的会员");
                return;
            }
            if (this.TbHall.Text.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择营业厅！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定续期？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            API.View_VIPInfo VIPInfo = dataGrid1.SelectedItem as API.View_VIPInfo;
            API.VIP_Renew RenewVIP = new API.VIP_Renew();
 
            RenewVIP.VIP_ID = VIPInfo.ID;
            try
            {
                RenewVIP.HallID = TbHall.Tag.ToString();
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择营业厅！");
                return;
            }
            //积分续期
            if (Add_SPoint.Value > 0)
            {
                RenewVIP.Validity = Point_AddDay;
                RenewVIP.Point = decimal.Parse(Add_SPoint.Value.ToString());
                RenewVIP.RenewTypeClassName = PointOption.ClassName;
                RenewVIP.RenewTypeName = PointOption.Name;
                RenewVIP.RenewValue1 = decimal.Parse(PointOption.Value);
                RenewVIP.RenewValue2 = decimal.Parse(PointOption.Value2);
            }

            //现金续期
            if (Add_Cash.Value > 0)
            {
                RenewVIP.Validity = Cash_AddDay;
                RenewVIP.RenewMoney = decimal.Parse(Add_Cash.Value.ToString());
                RenewVIP.RenewTypeName = CashOption.Name;
                RenewVIP.RenewTypeClassName = CashOption.ClassName;
                RenewVIP.RenewValue1 = decimal.Parse(CashOption.Value);
                RenewVIP.RenewValue2 = decimal.Parse(CashOption.Value2);
            }                       
            PublicRequestHelp help = new PublicRequestHelp(this.busy, SumbitMethodID, new object[] { RenewVIP }, SubmitCompleted);
        }
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.ReturnValue == true)
                {
                    if (re.Result.Obj == null)
                        return;
                    SelectChanged();
                    //this.TbHall.Text=string.Empty;
                    API.View_VIPInfo VIPInfo = re.Result.Obj as API.View_VIPInfo;
                    API.View_VIPInfo SelectVIP = dataGrid1.SelectedItem as API.View_VIPInfo;

                    SelectVIP.EndTime = VIPInfo.EndTime;
                    SelectVIP.Point = VIPInfo.Point;
                    SelectVIP.UpdUser = VIPInfo.UpdUser;
                    SelectVIP.SysDate = VIPInfo.SysDate;
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
        #region 下一页事件
        /// <summary>
        /// 点击下一页时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取下一页的数据
            pageParam = new API.ReportPagingParam()
            {
                PageIndex = e.NewPageIndex,
                PageSize = this.RadPager.PageSize,
                ParamList = new List<API.ReportSqlParams>()
            };
            this.InitPageEntity(GetVIPMethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
            #endregion
        }
        #endregion

        


    }
}
