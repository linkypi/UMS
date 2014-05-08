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

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// AddFactory.xaml 的交互逻辑
    /// </summary>
    public partial class AddFactory : Page
    {
        public AddFactory()
        {
            InitializeComponent();
        }

        private void add_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            #region 
            if (string.IsNullOrEmpty(facname.Text))
            {
                MessageBox.Show("厂家名称不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(faccode.Text))
            {
                MessageBox.Show("厂家编码不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(area.Text))
            {
                MessageBox.Show("区域不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(province.Text))
            {
                MessageBox.Show("省份不能为空！");
                return;
            }

            if (string.IsNullOrEmpty(city.Text))
            {
                MessageBox.Show("城市不能为空！");
                return;
            }
            //if (string.IsNullOrEmpty(contact.Text))
            //{
            //    MessageBox.Show("联系人不能为空！");
            //    return;
            //}
            //if (string.IsNullOrEmpty(addr.Text))
            //{
            //    MessageBox.Show("地址不能为空！");
            //    return;
            //}
            #endregion

            if (MessageBox.Show("确定添加吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }


            API.ASP_Factory facinfo = new API.ASP_Factory();
            facinfo.Addr = addr.Text;
            facinfo.Area = area.Text;
            facinfo.Bank = bank.Text;
            facinfo.BankNum = banknum.Text;
            facinfo.City = city.Text;
            facinfo.Contacts = contact.Text;
            facinfo.Email = email.Text;
            facinfo.FacID = faccode.Text;
            facinfo.FacName = facname.Text;
            facinfo.Fax = fax.Text;
            facinfo.Note = note.Text.Trim();
            facinfo.Phone = phone.Text;
            facinfo.PostCode = postcode.Text;
            facinfo.PriceLevel =Convert.ToInt32( pricelevel.Text);
            facinfo.Province = province.Text;
            facinfo.Responser = responser.Text;
            facinfo.TaxCode = taxcode.Text;


            PublicRequestHelp ph = new PublicRequestHelp(this.busy, 367, new object []{facinfo },SaveCompleted);
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                addr.Text = string.Empty;
                area.Text= string.Empty;
                bank.Text =string.Empty;
                banknum.Text =string.Empty;
                city.Text= string.Empty;
                contact.Text= string.Empty;
                email.Text= string.Empty;
                faccode.Text= string.Empty;
                facname.Text= string.Empty;
                fax.Text= string.Empty;
                note.Text= string.Empty;
                phone.Text= string.Empty;
                postcode.Text= string.Empty;
                pricelevel.Text= string.Empty;
                province.Text = string.Empty;
                responser.Text = string.Empty;
                taxcode.Text = string.Empty;
            }

        }
    }
}
