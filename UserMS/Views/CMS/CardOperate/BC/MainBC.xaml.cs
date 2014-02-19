using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UserMS.Views.CMS.CardOperate.BC
{
    public partial class MainBC : UserControl
    {
        List<SlModel.VIPModel> vipmodel = null;
        List<SlModel.ServiceModel> servicemodel = null;
        public MainBC()
        {
            InitializeComponent();

            vipmodel = new List<SlModel.VIPModel>();
            servicemodel = new List<SlModel.ServiceModel>();
            this.DGVIPInfo.ItemsSource = vipmodel;
            this.DGCardInfo.ItemsSource = servicemodel;
            this.SearchVIP.Click += SearchButton_Click;
            this.CBBK.Click += Check_Click;
            this.CBHC.Click += Check_Click;
            this.CBSJC.Click += Check_Click;
            this.tbBCCardid.GotFocus += tbBCCardid_GotFocus;
            this.tbHCCardid.GotFocus += tbHCCardid_GotFocus;
            this.tbSJCardid.GotFocus += tbSJCardid_GotFocus;
            this.AddNewCardID.Click += AddNewCardID_Click;
            this.Sumbit.Click += Sumbit_Click;
            API.Sys_UserInfo user = Store.LoginUserInfo;
            tbtrantor.Text = user.UserName;
            this.tbtime.SelectedValue = DateTime.Now;
        }

        void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            if (DGVIPInfo.SelectedItem != null)
            {
                if (IMEI1.Content != null && TypeName1.Content != null && Cost_production1.Content != null && Validity1.Content != null)
                {
                    SlModel.VIPModel newvipmodel = DGVIPInfo.SelectedItem as SlModel.VIPModel;
                    ShowInfo child = new ShowInfo();
                    child.showchild(newvipmodel, IMEI1.Content.ToString().Trim(),
                        TypeName1.Content.ToString().Trim(), Cost_production1.Content.ToString().Trim(),
                        Validity1.Content.ToString().Trim());
                    child.Show();
                    child.Closed += child_Closed;
                }
            }
        }

        void child_Closed(object sender, EventArgs e)
        {
            bool juge = Convert.ToBoolean(((ShowInfo)sender).DialogResult);
            if (juge == true)
            {
             
                SlModel.VIPModel VIPInfo = ((ShowInfo)sender).vipinfo;
                string imei = ((ShowInfo)sender).IMEI;
                if (typeID != 0&&VIPInfo!=null)
                {
                    API.VIP_VIPInfo vip = new API.VIP_VIPInfo();
                    vip.IMEI = VIPInfo.IMEI;
                    vip.TypeID = typeID;
                    vip.ID = VIPInfo.ID;
                    vip.Validity = VIPInfo.Validity;
                    vip.ProPrice = VIPInfo.Cost_production;
                    API.Sys_UserInfo user = Store.LoginUserInfo;
                    PublicRequestHelp help = new PublicRequestHelp(null, 52, new object[] { user, vip,imei}, SubmitCompleted2);
                }
            }
        }
        void SubmitCompleted2(object sender, API.MainCompletedEventArgs results)
        {
            if (results.Result.Message != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
            }
            if (results.Result.ReturnValue == true)
            {
                vipmodel.Clear();
                servicemodel.Clear();
                this.DGVIPInfo.Rebind();
                this.DGCardInfo.Rebind();
                IMEI1.Content = checkcardid;
                TypeName1.Content = string.Empty;
                Cost_production1.Content = string.Empty;
                SPoint1.Content = string.Empty;
                Validity1.Content = string.Empty;
            }
        }

        /// <summary>
        /// 添加新卡号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        string checkcardid = "";
        void AddNewCardID_Click(object sender, RoutedEventArgs e)
        {
            if (DGVIPInfo.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加或选择会员");
                return;
            }
            SlModel.VIPModel model = DGVIPInfo.SelectedItem as SlModel.VIPModel;
            string Note = "";
            if (CBBK.IsChecked == true && tbBCCardid.Text.Trim() != "------请输入新卡号-----" && tbBCCardid.Text.Trim() != "")
            {
                checkcardid = tbBCCardid.Text.Trim();
                Note = "补卡操作";
            }
            if (CBHC.IsChecked == true && tbHCCardid.Text.Trim() != "------请输入新卡号-----" && tbHCCardid.Text.Trim() != "")
            {
                checkcardid = tbHCCardid.Text.Trim();
            }
            if (CBSJC.IsChecked == true && tbSJCardid.Text.Trim() != "------请输入新卡号-----" && tbSJCardid.Text.Trim() != "")
            {
                checkcardid = tbSJCardid.Text.Trim();
            }
            if (checkcardid == "" || checkcardid == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加新卡号");
                return;
            }
            PublicRequestHelp help = new PublicRequestHelp(null, 40, new object[] { checkcardid }, SubmitCompleted1);
        }
       int  typeID = 0;
        void SubmitCompleted1(object sender, API.MainCompletedEventArgs results)
        {
            if (results.Result.Obj == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
            }
            else
            {
                API.VIP_VIPType type = results.Result.Obj as API.VIP_VIPType;
                servicemodel.Clear();
                if (type.VIP_VIPTypeService != null)
                {
                    foreach (var index in type.VIP_VIPTypeService)
                    {
                        SlModel.ServiceModel model = new SlModel.ServiceModel();
                        model.ProID = index.ProID;
                        var pro = Store.ProInfo.Where(p => p.ProID == index.ProID).First();
                        var protypeName = Store.ProTypeInfo.Where(p => p.TypeID == pro.Pro_TypeID);
                        if (protypeName != null)
                        {
                            model.ProTypeName = protypeName.First().TypeName;
                        }
                        model.ProName = pro.ProName;
                        model.SCount = index.SCount.ToString();
                        servicemodel.Add(model);
                    }
                }
                typeID = type.ID;
                IMEI1.Content = checkcardid;
                TypeName1.Content = type.Name;
                Cost_production1.Content = Convert.ToDecimal(type.Cost_production);
                SPoint1.Content = type.SPoint.ToString();
                Validity1.Content = (int)type.Validity;
            }
            DGCardInfo.Rebind();
        }

        #region 输入框操作
        /// <summary>
        /// 初始化输入框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tbSJCardid_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tbSJCardid.Text = string.Empty;
            this.tbHCCardid.Text = "------请输入新卡号-----";
            this.tbBCCardid.Text = "------请输入新卡号-----";
        }

        void tbHCCardid_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tbSJCardid.Text = "------请输入新卡号-----";
            this.tbHCCardid.Text = string.Empty;
            this.tbBCCardid.Text = "------请输入新卡号-----";
        }

        void tbBCCardid_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tbSJCardid.Text = "------请输入新卡号-----";
            this.tbHCCardid.Text = "------请输入新卡号-----";
            this.tbBCCardid.Text = string.Empty;
        }
        /// <summary>
        /// 显示输入框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Check_Click(object sender, RoutedEventArgs e)
        {
            this.tbSJCardid.Text = "------请输入新卡号-----";
            this.tbHCCardid.Text = "------请输入新卡号-----";
            this.tbBCCardid.Text = "------请输入新卡号-----";
            if (this.CBBK.IsChecked == true)
            {
                this.tbBCCardid.Visibility = Visibility.Visible;
            }
            if (this.CBBK.IsChecked == false)
            {
                this.tbBCCardid.Visibility = Visibility.Collapsed;
            }
            if (this.CBHC.IsChecked == true)
            {
                this.tbHCCardid.Visibility = Visibility.Visible;
            }
            if (this.CBHC.IsChecked == false)
            {
                this.tbHCCardid.Visibility = Visibility.Collapsed;
            }
            if (this.CBSJC.IsChecked == true)
            {
                this.tbSJCardid.Visibility = Visibility.Visible;
            }
            if (this.CBSJC.IsChecked == false)
            {
                this.tbSJCardid.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

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
