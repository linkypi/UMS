using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UserMS.Views.CMS.CardOperate.TC
{
    public partial class MainTC : UserControl
    {
        List<SlModel.VIPModel> vipmodel;
        public MainTC()
        {
            InitializeComponent();
            vipmodel = new List<SlModel.VIPModel>();
            DGVIPInfo.ItemsSource = vipmodel;
            this.SearchVIP.Click += SearchButton_Click;
            this.Accounts.Click += Accounts_Click;
            this.SumbitAduit.Click += SumbitAduit_Click;
            API.Sys_UserInfo user= Store.LoginUserInfo;
            tbtrantor.Text = user.UserName;
            this.tbtime.SelectedValue = DateTime.Now;
        }
        /// <summary>
        /// 验证审批单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SumbitAduit_Click(object sender, RoutedEventArgs e)
        {
            if (DGVIPInfo.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要退卡的会员");
                return;
            }
            SlModel.VIPModel newvipmodel = DGVIPInfo.SelectedItem as SlModel.VIPModel;
            PublicRequestHelp help = new PublicRequestHelp(null, 57, new object[] { AdutiID.Text.Trim(), newvipmodel.ID }, RetrunAduit);

        }
        void RetrunAduit(object sender, API.MainCompletedEventArgs results)
        {

            if (results.Result.ReturnValue == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
                return;
            }
            API.VIP_VIPBackAduit aduitmoney = results.Result.Obj as API.VIP_VIPBackAduit;
            MessageBox.Show(System.Windows.Application.Current.MainWindow,"审批单可用");
            foreach (var index in vipmodel)
            {
                if (aduitmoney.VIP_ID == index.ID)
                {
                    index.AduitMoney = aduitmoney.ToString();
                    return;
                }
            }
            DGVIPInfo.Rebind();
        }
        /// <summary>
        /// 提交退卡model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Accounts_Click(object sender, RoutedEventArgs e)
        {
            if (DGVIPInfo.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中或未添加需要退卡的会员");
            }

            SlModel.VIPModel newvipmodel = DGVIPInfo.SelectedItem as SlModel.VIPModel;
            if (newvipmodel.AduitMoney == null || newvipmodel.AduitMoney.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未提交审批单");
                return;
            }
            if (this.tbtime == null || this.tbtime.SelectedValue.ToString().Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加办理时间");
            }
            API.VIP_VIPBack back = new API.VIP_VIPBack();
            back.VIP_ID = newvipmodel.ID;
            API.Sys_UserInfo user = Store.LoginUserInfo;
            back.UserID = user.UserID;
            back.Return_Money = Convert.ToDecimal(newvipmodel.AduitMoney.Trim());       
            back.AduitID = AdutiID.Text.Trim();
            back.SysDate = this.tbtime.SelectedValue;
            PublicRequestHelp help = new PublicRequestHelp(null, 55, new object[] { user, back }, SubmitCompleted2);

        }
        void SubmitCompleted2(object sender, API.MainCompletedEventArgs results)
        {
            if (results.Result.Message != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
            }
            if (results.Result.ReturnValue == true)
            {
                tbtime.SelectedValue = DateTime.Now;      
                vipmodel.Clear();
                DGVIPInfo.Rebind();
            }
        }
        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region
        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.tbIDCardID.Text.Trim() == "" && this.cardid.Text.Trim() == "" && this.MobilePhone.Text.Trim() == "" && this.cardid.Text.Trim() == "" && this.VIPName.Text.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入您要查询的条件");
                return;
            }
            PublicRequestHelp help = new PublicRequestHelp(null, 3, new object[] { this.cardid.Text.Trim(), tbIDCardID.Text.Trim(), this.VIPName.Text.Trim(), this.MobilePhone.Text.Trim() }, SubmitCompleted);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="results"></param>
        void SubmitCompleted(object sender, API.MainCompletedEventArgs results)
        {
            if (results.Result.ReturnValue == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
            }
            if (results.Result.Obj != null)
            {
                List<API.VIP_VIPInfo> return_vip = results.Result.Obj as List<API.VIP_VIPInfo>;
                if (return_vip != null)
                {
                    foreach (var index in return_vip)
                    {
                        if (!exist(index.IMEI))
                        {
                            SlModel.VIPModel vip = new SlModel.VIPModel();

                            var gettype = Store.VIPType.Where(type => type.ID == index.TypeID);
                            if (gettype != null)
                            {
                                vip.TypeName = gettype.First().Name;
                                vip.Cost_production = Convert.ToDecimal(gettype.First().Cost_production);
                                vip.SPoint = gettype.First().SPoint.ToString();
                            }
                            vip.IMEI = index.IMEI;
                            vip.MemberName = index.MemberName;
                            vip.Sex = index.Sex;
                            vip.Birthday = Convert.ToDateTime(index.Birthday);
                            vip.MobiPhone = index.MobiPhone;
                            vip.TelePhone = index.TelePhone;
                            vip.QQ = index.QQ;
                            vip.Address = index.Address;

                            vip.StartTime = Convert.ToDateTime(index.StartTime);
                            vip.UpdUser = index.UpdUser;
                            vip.Validity = (int)index.Validity;
                            vip.IDCard = index.IDCard;
                            vip.IDCard_ID = (int)index.IDCard_ID;
                            vip.ID = index.ID;
                            vipmodel.Add(vip);
                        }
                    }
                }
            }
            DGVIPInfo.Rebind();
        }
        private bool exist(string imei)
        {
            if (vipmodel == null)
            {
                return false;
            }
            foreach (var index in vipmodel)
            {
                if (index.IMEI == imei)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        private void RadButton_Click_1(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();
            List<SlModel.VIPModel> model = vipmodel;
            foreach (var index in model)
            {
                if (tag == index.IMEI)
                {
                    vipmodel.Remove(index);
                    DGVIPInfo.Rebind();
                    break;
                }
            }                  
        }
    }
}
