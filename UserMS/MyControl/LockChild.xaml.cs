using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS
{
    public partial class LockChild : RadWindow
    {
        public LockChild()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }


        private void Unlock_OnClick(object sender, RoutedEventArgs e)
        {
            if (Store.LoginUserInfo.UserPwd == Password.Password)
            {
                this.DialogResult = true;
                Close();
            }
            else
            {
                DialogParameters p= new DialogParameters();
                p.Content = "密码错误";
               
                RadWindow.Alert(p);
            }
        }
    }
}

