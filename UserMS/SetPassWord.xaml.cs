using System.Windows;
using System.Windows.Controls;

namespace UserMS
{
    /// <summary>
    /// SetPassWord.xaml 的交互逻辑
    /// </summary>
    public partial class SetPassWord : Page
    {
        public SetPassWord()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PublicRequestHelp.IsLogin();

            if (string.IsNullOrEmpty(this.OldPwd.Password))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入原密码");
                return;
            }
            if (this.OldPwd.Password.Trim()!=Store.LoginUserInfo.UserPwd)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"原密码输入错误");
                return;
            }
            if (string.IsNullOrEmpty(this.NewPwd_1.Password) || string.IsNullOrEmpty(this.NewPwd_2.Password))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入新密码");
                return;
            }
            if (this.NewPwd_1.Password !=  this.NewPwd_2.Password)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"两次输入新密码不一致");
                return;
            }
            
            PublicRequestHelp.IsLogin();

            Store.wsclient.UpdatePwdCompleted += wsclient_UpdatePwdCompleted;
            Store.wsclient.UpdatePwdAsync(Store.LoginUserInfo.UserName, Store.LoginUserInfo.UserPwd, this.NewPwd_1.Password.Trim(), "");
            this.busy.IsBusy = true;
        }

        void wsclient_UpdatePwdCompleted(object sender, API.UpdatePwdCompletedEventArgs e)
        {
            Store.wsclient.UpdatePwdCompleted -= wsclient_UpdatePwdCompleted;
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"修改失败："+e.Error.Message);
                return;
            }
            API.WebReturn web = e.Result;
            if (web.ReturnValue != true)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,web.Message);
                return;
            }
            else
            {
                Store.LoginUserInfo.UserPwd = this.NewPwd_1.Password.Trim();
                Store.LoginUserPassword = Store.LoginUserInfo.UserPwd;
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"修改成功");
            }
        }
    }
}
