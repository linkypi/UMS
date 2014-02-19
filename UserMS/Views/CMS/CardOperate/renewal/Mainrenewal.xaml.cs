using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace UserMS.Views.CMS.CardOperate.renewal
{
    public partial class Mainrenewal : UserControl
    {
        private List<API.RenewModel> models = null;
        private string flag;

        public Mainrenewal()
        {
            InitializeComponent();
            models = new List<API.RenewModel>();
            GridVIPCard.ItemsSource = models;
            this.cardType.ItemsSource = Store.CardType;
            this.cardType.SelectedIndex = 0;
            this.tabc.SelectionChanged += tabc_SelectionChanged;
            this.search.Click += search_Click;
            this.btnOK.Click += btnOK_Click;

            this.MobilePhone.KeyDown += SearchVIP_KeyDown;
            this.cardid.KeyDown += SearchVIP_KeyDown;
            this.cardNum.KeyDown += SearchVIP_KeyDown;
            this.VIPName.KeyDown += SearchVIP_KeyDown;
        }

        private void SearchVIP_KeyDown(object sender, KeyEventArgs e)
        {
            SearchVIP();
        }

        void tabc_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            Telerik.Windows.Controls.RadTabItem item = tabc.SelectedItem as Telerik.Windows.Controls.RadTabItem;
            flag = item.Tag.ToString();
        }

        /// <summary>
        /// 确定续期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (models.Count == 0)
            {
                return;
            }

            API.VIP_Renew renew = new API.VIP_Renew();
            renew.RenewDate = DateTime.Now;

            renew.Validity = models[0].Validity;
            renew.VIP_ID = models[0].VIPID;
          
            if (flag.Equals("1"))
            {
                if (this.money.Value==0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入金额");
                    return;
                }
                renew.RenewMoney = decimal.Parse(this.money.Value.ToString());
         
                renew.RenewTypeName = "CashRenew";
            }

            if (flag.Equals("2"))
            {
                 renew.Point = decimal.Parse(this.point.Value.ToString());
                renew.RenewTypeClassName = "PointRenew";
            }

            PublicRequestHelp prh = new PublicRequestHelp(null,54,new object[]{Store.LoginUserInfo,renew},new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
        }

        /// <summary>
        /// 续期完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"续期成功");
                Logger.Log("续期成功");

                models[0].Validity += int.Parse(e.Result.ArrList[0].ToString());
                models[0].Point -= decimal.Parse(e.Result.ArrList[1].ToString());
                GridVIPCard.Rebind();

                this.money.Value = 0;
                this.point.Value = 0;
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"续期失败");
                Logger.Log("续期失败");
            }
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void search_Click(object sender, RoutedEventArgs e)
        {
            SearchVIP();
        }

        private void SearchVIP()
        {
            API.VIP_VIPInfo vip = new API.VIP_VIPInfo();

            vip.MemberName = this.VIPName.Text.ToString();
            vip.IDCard = this.cardid.Text.ToString();
            vip.MobiPhone = this.MobilePhone.Text.ToString();
            vip.IDCard_ID = (int)this.cardType.SelectedValue;  //证件类型
            vip.IMEI = this.cardNum.Text.ToString();

            PublicRequestHelp prh = new PublicRequestHelp(null, 53, new object[] { vip }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        /// <summary>
        /// 查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            models.Clear();
            GridVIPCard.Rebind();

            if (e.Result.ReturnValue)
            {
                List<API.RenewModel> list = e.Result.Obj as List<API.RenewModel>;
                models.AddRange(list);
                this.point.Maximum =double.Parse( models[0].Point.ToString());
                this.point.Value =double.Parse( models[0].Point.ToString());
                GridVIPCard.Rebind();

            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserMS.CMS.Operate_renewal mywin = new UserMS.CMS.Operate_renewal();
//            mywin.Title = new TextBlock()
//            {
//                Text = "新增卡续期",
//                FontSize = 13
//            };
            mywin.Show();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            UserMS.CMS.Operate_renewal mywin = new UserMS.CMS.Operate_renewal();
//            mywin.Title = new TextBlock()
//            {
//                Text = "修改卡续期",
//                FontSize = 13
//            };
            mywin.Show();
        }
    }
}
