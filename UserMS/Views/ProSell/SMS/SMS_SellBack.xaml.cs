using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserMS.Views.ProSell.SMS
{
    /// <summary>
    /// SMS_SellBack.xaml 的交互逻辑
    /// </summary>
    public partial class SMS_SellBack : Page
    {
        public SMS_SellBack()
        {
            InitializeComponent();
            aduitID.SearchEvent = new RoutedEventHandler(SearchAduit);
        }

        private void SearchAduit(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.aduitID.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入审批单号");
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(aduitBusy,
                309, new object[] { aduitID.Text}, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
          
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            aduitBusy.IsBusy = false;

            if (e.Result.ReturnValue)
            {
                API.View_SMS_SellBackAduit aduit = e.Result.Obj as API.View_SMS_SellBackAduit;

                applyCount.Text = String.Format("{0:F}", aduit.ApplyCount);
                applyDate.Text = aduit.ApplyDate.ToString();
                applyMoney.Text = String.Format("{0:F}", aduit.ApplyMoney);
                cusName.Text = aduit.CusName;
                cusphone.Text = aduit.CusPhone;
                hall.Text = aduit.HallName;

                API.View_SMS_SignInfo model = e.Result.ArrList[0] as API.View_SMS_SignInfo;

                signList.ItemsSource = new List<API.View_SMS_SignInfo>(){model};
                signList.Rebind();

                detailGrid.ItemsSource = model.View_SMS_SignSendPayInfo;
                detailGrid.Rebind();
            }
            else
            {
                MessageBox.Show(e.Result.Message); 
            }

        }

        private void back_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show("确定退货吗？","",MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
        
            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 310, new object[] { aduitID.Text.Trim() ,note.Text??""},
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            MessageBox.Show(e.Result.Message);

            aduitID.Text = string.Empty;
            applyCount.Text = string.Empty;
            applyDate.Text = string.Empty;
            applyMoney.Text = string.Empty;
            cusName.Text = string.Empty;
            cusphone.Text = string.Empty;
            hall.Text = string.Empty;
            signList.ItemsSource = null;
            signList.Rebind();

            detailGrid.ItemsSource = null;
            detailGrid.Rebind();

        }
    }
}
