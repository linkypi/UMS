using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace UserMS.Views.CMS.CardOperate.renewal
{
    public partial class RenewBack : Page
    {
        private string flag;
        private List<API.RenewModel> models = null;
       
        public RenewBack()
        {
            InitializeComponent();
            this.cardType.ItemsSource = Store.CardType;
            this.cardType.SelectedIndex = 0;
            models = new List<API.RenewModel>();
            GridVIPCard.ItemsSource = models;

            this.search.Click += search_Click;
            this.btnAduit.Click += btnAduit_Click;
            this.tabc.SelectionChanged += tabc_SelectionChanged;
            this.btnSave.Click += btnSave_Click;
            this.aduitID.KeyDown += aduitID_KeyDown;
            this.MobilePhone.KeyDown += SearchVIP_KeyDown;
            this.cardid.KeyDown += SearchVIP_KeyDown;
            this.cardNum.KeyDown += SearchVIP_KeyDown;
            this.VIPName.KeyDown += SearchVIP_KeyDown;
        }

        void SearchVIP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchVIP();
            }
        }

        void aduitID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchAduitInfo();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (models.Count == 0)
            {
                return;
            }
            if (!models[0].Flag)
            {
                return;
            }
            API.VIP_RenewBack renBack = new API.VIP_RenewBack();
            renBack.AduitID = models[0].AduitID;

            if (flag == "1")
            {
                renBack.Money =decimal.Parse( this.money.Text.ToString());
                renBack.Validity = 0;
            }
            if (flag == "2")
            {
                renBack.Validity = int.Parse(this.validity.Text.ToString());
                renBack.Money = 0;
            }
            renBack.Old_Renew_ID = models[0].OldRenewID;
            renBack.AduitID = models[0].AduitID;
            renBack.Note = models[0].Note;

            PublicRequestHelp prh = new PublicRequestHelp(null,58,new object[]{Store.LoginUserInfo,renBack},new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));

        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"取消续期成功");
                Logger.Log("取消续期成功");

                models[0].Validity = (int)e.Result.ArrList[0];
                models[0].Point = (decimal)e.Result.ArrList[1];
                GridVIPCard.Rebind();

                this.aduitID.Text = string.Empty;
                this.validity.Text = string.Empty;
            }
            else 
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"取消续期失败");
                Logger.Log("取消续期失败");
            }

        }

        void tabc_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            Telerik.Windows.Controls.RadTabItem item = tabc.SelectedItem as Telerik.Windows.Controls.RadTabItem;
            flag = item.Tag.ToString();
        }

        /// <summary>
        /// 查询审批单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAduit_Click(object sender, RoutedEventArgs e)
        {
            SearchAduitInfo();
       }

        private void SearchAduitInfo()
        {
            if (models.Count == 0)
            {
                return;
            }
            if (!string.IsNullOrEmpty(this.aduitID.Text))
            {
                PublicRequestHelp prh = new PublicRequestHelp(null, 56, new object[] { this.aduitID.Text, models[0].VIPID }, new EventHandler<API.MainCompletedEventArgs>(GetAduitCompleted));
            }
        }

        private void GetAduitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            if (e.Result.ReturnValue)
            {
                API.RenewModel rm = e.Result.Obj as API.RenewModel;

                this.money.Text = rm.AduitMoney.ToString();
                this.validity.Text = rm.AduitValidity.ToString();
                if(models.Count()!=0)
                {
                    models[0].OldRenewID = rm.OldRenewID;
                    models[0].AduitID = rm.AduitID;
                    models[0].Flag = rm.Flag;
                    models[0].AduitMoney = rm.AduitMoney;
                    models[0].AduitValidity = rm.AduitValidity;
                    GridVIPCard.Rebind();
                }
                if (rm.AduitMoney != 0)
                {
                    this.tabc.SelectedIndex = 0;
                }
                else
                {
                    this.tabc.SelectedIndex = 1;
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
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
             //   this.point.Text = models[0].Point.ToString();
              //  this.money.Text = models[0].Point.ToString();
                //this.point.Maximum = double.Parse(models[0].Point.ToString());
                //this.point.Value = double.Parse(models[0].Point.ToString());
                GridVIPCard.Rebind();
            }
        }

        /// <summary>
        /// 搜索会员
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


     

    }
}
