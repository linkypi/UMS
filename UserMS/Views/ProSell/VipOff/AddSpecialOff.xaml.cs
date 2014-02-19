using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SlModel.Temp;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.ProSell.VipOff
{
    public partial class AddSpecialOff : BasePage
    {
      public API.UserMsServiceClient webservice = new API.UserMsServiceClient();
        List<API.Pro_SellTypeProduct> sellTypePro;
        private List<API.SelecterIMEI> uncheckIMEI;
        private List<API.SeleterModel> vmodels = null;//商品列表
        Mul_ProductFileter ProFilter;//商品过滤
        private Mul_HallFilter hAdder;
        const int MethodID = 3;
        const int AddMethodID = 148;
        List<decimal> ProPrice = new List<decimal>();

        List<API.VIP_OffList> offList_List;
        List<AddSell> newSell;
        public AddSpecialOff()
        {
            InitializeComponent();
            Initial();

            sellTypePro = new List<API.Pro_SellTypeProduct>();
            this.webservice.GetSellTyProAsync();
            this.webservice.GetSellTyProCompleted += webservice_GetSellTyProCompleted;

            vmodels = new List<API.SeleterModel>();
            this.ProGrid.ItemsSource = vmodels;
            InitGrid2();

            hAdder = new Mul_HallFilter(ref this.HallGrid);
            uncheckIMEI = new List<API.SelecterIMEI>();
            //this.dataGridVip2.ItemsSource = uncheckIMEI;
            this.txtIMEI.KeyUp += txtIMEI_KeyUp;
        }
        #region 初始化控件
        void Initial()
        {
            CBLimit.IsChecked = false;
            LimitNum.IsReadOnly = true;
            LimitNum.Text = "99999";

    
            if (Store.LoginUserInfo != null)
                CreatName.Text = Store.LoginUserInfo.UserName;
        }
     

        private void CBLimit_Click_1(object sender, RoutedEventArgs e)
        {
            if (CBLimit.IsChecked == true)
            {
                LimitNum.IsReadOnly = false;
                LimitNum.Text = string.Empty;
            }
            else
            {
                LimitNum.IsReadOnly = true;
                LimitNum.Text = "99999";
            }
        }
        #endregion

        #region 测试版获取销售类别（要修改）
        void webservice_GetSellTyProCompleted(object sender, API.GetSellTyProCompletedEventArgs e)
        {
            if (e.Error == null)
            {
           
                sellTypePro.AddRange(e.Result);
                ProFilter = new Mul_ProductFileter(ref vmodels, ref ProGrid, sellTypePro);
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器出错！");
        }
        #endregion

        #region 生成列
        private void InitGrid2()
        {
            //优惠表头GridView
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("Name");
            col.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col.Header = "优惠名称";
            this.dataGridOffList.Columns.Add(col);


            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("StartTime");
            col2.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col2.Header = "开始时间";
            this.dataGridOffList.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("EndTime");
            col3.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col3.Header = "结束时间";
            this.dataGridOffList.Columns.Add(col3);

            GridViewDataColumn col4 = new GridViewDataColumn();
            col4.DataMemberBinding = new System.Windows.Data.Binding("LimitNum");
            col4.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col4.Header = "限制名额";
            this.dataGridOffList.Columns.Add(col4);

            GridViewDataColumn col41 = new GridViewDataColumn();
            col41.DataMemberBinding = new System.Windows.Data.Binding("CreatName");
            col41.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col41.Header = "创建人";
            this.dataGridOffList.Columns.Add(col41);

            GridViewDataColumn col5 = new GridViewDataColumn();
            col5.DataMemberBinding = new System.Windows.Data.Binding("Note");
            col5.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            col5.Header = "备注";
            this.dataGridOffList.Columns.Add(col5);

            //GridViewDataColumn col51 = new GridViewDataColumn();
            //col51.DataMemberBinding = new System.Windows.Data.Binding("Rebate");
            //col51.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            //col51.Header = "折扣";
            //this.dataGridOffList.Columns.Add(col51);


            //GridViewDataColumn col52 = new GridViewDataColumn();
            //col52.DataMemberBinding = new System.Windows.Data.Binding("ReduceCash");
            //col52.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            //col52.Header = "减现金";
            //this.dataGridOffList.Columns.Add(col52);

            //GridViewDataColumn col6 = new GridViewDataColumn();
            //col6.DataMemberBinding = new System.Windows.Data.Binding("PointChanged");
            //col6.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            //col6.Header = "积分兑换";
            //this.dataGridOffList.Columns.Add(col6);

            //会员类别GridView
            GridViewDataColumn VIPTypecol = new GridViewDataColumn();
            VIPTypecol.DataMemberBinding = new System.Windows.Data.Binding("Name");
            VIPTypecol.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            VIPTypecol.Header = "会员类别";
            this.dataGridVipType.Columns.Add(VIPTypecol);

            //会员
            GridViewDataColumn VIPcol = new GridViewDataColumn();
            VIPcol.DataMemberBinding = new System.Windows.Data.Binding("IMEI");
            VIPcol.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            VIPcol.Header = "会员卡号";
            this.dataGridVip2.Columns.Add(VIPcol);

            GridViewDataColumn VIPcol1 = new GridViewDataColumn();
            VIPcol1.DataMemberBinding = new System.Windows.Data.Binding("MemberName");
            VIPcol1.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            VIPcol1.Header = "会员姓名";
            this.dataGridVip2.Columns.Add(VIPcol1);

            //营业厅
            GridViewDataColumn Hallcol = new GridViewDataColumn();
            Hallcol.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            Hallcol.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            Hallcol.Header = "营业厅";
            this.HallGrid.Columns.Add(Hallcol);

        }
        #endregion


        #region 会员卡类型操作
        #region 添加会员卡类别
        private void AddVIPType_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            MultSelecter msFrm = new MultSelecter(
             null,
             Store.VIPType,
            "ID", "Name",
             new string[] { "ID", "Name" },
             new string[] { "ID", "会员类别" },
             true
            );
            msFrm.Closed += VIPTypeWin_Closed;
            msFrm.ShowDialog();

        }
        private void VIPTypeWin_Closed(object sender, EventArgs e)
        {
            UserMS.MultSelecter selecter = sender as UserMS.MultSelecter;
            if (selecter.DialogResult == true)
            {
                List<UserMS.API.VIP_VIPType> phList = selecter.SelectedItems.OfType<API.VIP_VIPType>().ToList();
                this.dataGridVipType.ItemsSource = phList;
            }
        }
        #endregion
        #region 删除会员卡类型
        private void DeleteVIPType_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelVIPType(ref dataGridVipType);
        }
        #endregion
        #endregion

        #region 会员操作
        private void AddVIP_Click_1(object sender, RoutedEventArgs e)
        {
            ActiceAddVIP();
        }
        void txtIMEI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ActiceAddVIP();
            }
        }
        #region 添加会员
        List<string> query;
        void ActiceAddVIP()
        {
            ViewOperate.IMEIAdd(this.txtIMEI.Text.Trim(), ref uncheckIMEI);
            //获取会员卡号
            query = (from b in uncheckIMEI
                     select b.IMEI).ToList();
            if (query.Count() == 0)
            { return; }
            API.ReportPagingParam pageParam = new API.ReportPagingParam();
            pageParam.ParamList = new List<API.ReportSqlParams>();
            //会员卡号查询         
            API.ReportSqlParams_ListString IMEI = new API.ReportSqlParams_ListString();
            IMEI.ParamName = "IMEI_List";
            IMEI.ParamValues = query;
            pageParam.ParamList.Add(IMEI);

            if (pageParam.ParamList.Count() > 0)
            {
                PublicRequestHelp h = new PublicRequestHelp(busy, MethodID, new object[] { pageParam }, MyClient_MainReportCompleted);
            }

        }
        protected void MyClient_MainReportCompleted(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {

                if (e.Result.ReturnValue == true)
                {
                    API.ReportPagingParam pageParem = (API.ReportPagingParam)e.Result.Obj;

                    this.dataGridVip2.ItemsSource = pageParem.Obj;
                    TBoxAddVIP();
                }
                else
                {
                    Logger.Log(e.Result.Message + "");
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                this.busy.IsBusy = false;
            }
        }

        void TBoxAddVIP()
        {
            if (dataGridVip2.ItemsSource == null)
                return;
            List<API.View_VIPInfo> VIPInfo = dataGridVip2.ItemsSource as List<API.View_VIPInfo>;
            List<string> ReturnIMEI = (from b in VIPInfo
                                       select b.IMEI).ToList();
            foreach (var VIPItem in ReturnIMEI)
            {
                query.Remove(VIPItem);
            }
            txtIMEI.Text = string.Empty;
            foreach (var StringItem in query)
            {
                txtIMEI.Text += StringItem + "\r\n";
            }
        }
        #endregion
        #region 删除会员
        private void DelVIP_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelVIP(ref dataGridVip2);
        }
        #endregion
        #endregion

        #region 商品操作
        #region 添加商品
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
          
            ProFilter.ProFilter(0,0,0);
        }

        #endregion
        #region 删除商品
        private void DelPro_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelCheckedPro(ref vmodels, ref ProGrid);
        }

        #endregion
        #endregion

        #region 营业厅操作
        #region 添加营业厅
        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
           // List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo.Where(p => p.CanBack == true).ToList();
            hAdder.GetHall(Store.ProHallInfo);
        }
        #endregion
        #region 删除营业厅
        private void DelHall_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelHall(ref HallGrid);
        }
        #endregion
        #endregion

        #region 清空数据
        void CancelPart()
        {
            this.CreatName.Text = string.Empty;
            this.EndTime.SelectedValue = null;
            this.StartTime.SelectedValue = null;
        
            this.Name.Text = string.Empty;
            this.Note.Text = string.Empty;
    
            //this.Title.Text = string.Empty;
            //this.Content.Text = string.Empty;

            dataGridVipType.ItemsSource = null;
            dataGridVipType.Rebind();

            dataGridVip2.ItemsSource = null;
            dataGridVip2.Rebind();

            vmodels.Clear();
            ProGrid.Rebind();


            HallGrid.ItemsSource = null;
            HallGrid.Rebind();
        }
        #endregion

        #region 提交数据
        private void RadMenuItem_Click_3(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            API.VIP_OffList Head = new API.VIP_OffList();
            #region 检查数据有效性
            if (String.IsNullOrEmpty(this.Name.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入优惠名称");
                return;
            }
            if (ExistName(this.Name.Text.Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"优惠名称已存在");
                return;
            }
            
            if (StartTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入开始日期");
                return;
            }
            if (EndTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入结束日期");
                return;
            }
            if (StartTime.SelectedValue > EndTime.SelectedValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"开始日期不能大于结束日期！");
                return;
            }
            if (StartTime.SelectedValue <= DateTime.Now)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"活动开始日期必须大于当前日期");
                return;
            }
            if (EndTime.SelectedValue <= DateTime.Now)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"活动结束日期必须大于当前日期");
                return;
            }
            if (string.IsNullOrEmpty(LimitNum.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入限制人数或设为默认");
                return;
            }
            try
            {
                Head.VIPTicketMaxCount = int.Parse(this.LimitNum.Text.Trim());
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"人数个数必须为整数");
                return;
            }
                
            if (ProGrid.ItemsSource == null || ProGrid.Items.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加商品");
                return;
            }
            if (vmodels.Where(p => p.OffPrice == 0).Count() > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品组合优惠后价格不能为0");
                return;
            }

            if (HallGrid.ItemsSource == null || HallGrid.Items.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加营业厅");
                return;
            }
            #endregion


            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定增加优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            #region 添加表头
            Head.Type = 1;
            Head.Name = this.Name.Text.Trim();
            Head.StartDate = this.StartTime.SelectedValue;
            Head.EndDate = this.EndTime.SelectedValue;

          
    
            Head.Note = this.Note.Text.Trim();
            //Head.DiscountSynopsis = this.Title.Text.Trim();
            //Head.DiscountInfo = this.Content.Text.Trim();
           
            #endregion

            #region 会员类型明细
            if (dataGridVipType.ItemsSource != null && dataGridVipType.Items.Count > 0)
            {
                List<API.VIP_VIPType> VIPTypeSource = dataGridVipType.ItemsSource as List<API.VIP_VIPType>;
                foreach (var TypeItem in VIPTypeSource)
                {
                    API.VIP_VIPTypeOffLIst VipType = new API.VIP_VIPTypeOffLIst();
                    VipType.VIPType = TypeItem.ID;
                    if (Head.VIP_VIPTypeOffLIst == null)
                        Head.VIP_VIPTypeOffLIst = new List<API.VIP_VIPTypeOffLIst>();
                    Head.VIP_VIPTypeOffLIst.Add(VipType);
                }
            }
            #endregion

            #region 会员明细
            if (dataGridVip2.ItemsSource != null && dataGridVip2.Items.Count > 0)
            {
                List<API.View_VIPInfo> VIPSource = dataGridVip2.ItemsSource as List<API.View_VIPInfo>;
                foreach (var VIPItem in VIPSource)
                {
                    API.VIP_VIPOffLIst VIP = new API.VIP_VIPOffLIst();
                    VIP.VIPID = VIPItem.ID;
                    if (Head.VIP_VIPOffLIst == null)
                        Head.VIP_VIPOffLIst = new List<API.VIP_VIPOffLIst>();
                    Head.VIP_VIPOffLIst.Add(VIP);
                }
            }
            #endregion

            #region 商品明细
            foreach (var ProItem in vmodels)
            {
                if (Head.OffPointMoney==0)
                    Head.OffPointMoney = ProItem.Price;

                API.VIP_ProOffList Pro = new API.VIP_ProOffList();

                Pro.ProID = ProItem.ProID;
                Pro.SellTypeID = ProItem.SellTypeID;
                Pro.AfterOffPrice = ProItem.OffPrice;
                Pro.Price = ProItem.Price;
                Pro.Salary = ProItem.Salary;
                Pro.ProCount = 1;
                if (Head.VIP_ProOffList == null)
                    Head.VIP_ProOffList = new List<API.VIP_ProOffList>();
                Head.VIP_ProOffList.Add(Pro);
            }
            #endregion


            #region 营业厅明细
            List<API.Pro_HallInfo> HallSource = HallGrid.ItemsSource as List<API.Pro_HallInfo>;
            foreach (var HallItem in HallSource)
            {
                API.VIP_HallOffInfo HallOff = new API.VIP_HallOffInfo();
                HallOff.HallID = HallItem.HallID;

                if (Head.VIP_HallOffInfo == null)
                    Head.VIP_HallOffInfo = new List<API.VIP_HallOffInfo>();
                Head.VIP_HallOffInfo.Add(HallOff);
            }
            #endregion
            if (offList_List == null)
                offList_List = new List<API.VIP_OffList>();
            offList_List.Add(Head);

            //添加到GV中
            AddSell Sell = new AddSell()
            {
                CreatName = this.CreatName.Text,
                EndTime = this.EndTime.SelectedValue.ToString(),
                StartTime = this.StartTime.SelectedValue.ToString(),
                LimitNum = this.LimitNum.Text,
                Name = this.Name.Text,
                Note = this.Note.Text,
      
            };
            if (newSell == null)
                newSell = new List<AddSell>();
            newSell.Add(Sell);

            this.dataGridOffList.ItemsSource = newSell;
            this.dataGridOffList.Rebind();
            CancelPart();
            Initial();
           
        }
        bool ExistName(string Name)
        {
            if (offList_List == null)
                return false;
            else
            {
                var query = (from b in offList_List
                            where b.Name == Name
                            select b).ToList();
                if (query.Count() > 0)
                    return true;
                else
                    return false;
            }
        }
        bool ThanPrice(string Price)
        {
            decimal K=decimal.Parse(Price);
            var query = (from b in vmodels
                        where b.Price <= K
                        select b).ToList();
            if (query.Count() == 0)
                return false;
            else
                return true;
        }
        private void RadMenuItem_Click_4(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (offList_List == null || offList_List.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未建优惠单！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定提交所有优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp h = new PublicRequestHelp(busy, AddMethodID, new object[] { offList_List }, MyClient_Completed);
        }
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {

                if (e.Result.ReturnValue == true)
                {
                    CancelPart();
                    Initial();
                    newSell.Clear();
                    this.dataGridOffList.ItemsSource = newSell;
                    this.dataGridOffList.Rebind();
                    offList_List.Clear();         
                }
                Logger.Log(e.Result.Message + "");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                this.busy.IsBusy = false;
            }
        }
        #endregion
        #region 优惠资料设置（暂时无用）
        private void UpLoad_Click_1(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();

            //if (openFileDialog.ShowDialog()==true)
            //{  }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //FileName.Text = openFileDialog.FileName;
                //saveFileDialog
            }
            //UploadFileInfo 

        }
        #endregion
        #region 删除优惠
        private void DelOff_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (dataGridOffList.SelectedItems == null || dataGridOffList.SelectedItems.Count()==0)
                return;
            foreach (var Item in dataGridOffList.SelectedItems)       
            { 
                AddSell sell=Item as AddSell;
                if(offList_List!=null)
                {
                    List<API.VIP_OffList> query = (from b in offList_List
                          where b.Name==sell.Name
                          select b).ToList();
                    if (query.Count() > 0)
                        offList_List.Remove(query.First());
                }
                newSell.Remove(sell);            
            }
            dataGridOffList.ItemsSource = newSell;
            dataGridOffList.Rebind();
        }
        #endregion


        #region DV编辑结束后
        private void ProGrid_CellValidated_1(object sender, GridViewCellValidatedEventArgs e)
        {
            API.SeleterModel ProItem = this.ProGrid.SelectedItem as API.SeleterModel;

            if (e.Cell.Column.Header.ToString() == "组合套餐价格")
            {
                if (ProItem.OffPrice < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写正数！");
                    ProItem.OffPrice = 0;
                    return;
                }
                int value = (int)(ProItem.OffPrice * 100);
                ProItem.OffPrice = (decimal)(value * 0.01);
 
            }
         
            else if (e.Cell.Column.Header.ToString() == "提成")
            {
                if (ProItem.Salary < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写正数！");
                    ProItem.Salary = 0;
                    return;
                }
                int value = (int)(ProItem.Salary * 100);
                ProItem.Salary = (decimal)(value * 0.01);
            }
        }
        #endregion 
    }
}
