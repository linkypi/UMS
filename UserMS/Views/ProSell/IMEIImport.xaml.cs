using System.Windows;
using System.Windows.Input;

namespace UserMS.Views.ProSell
{
    public partial class IMEIImport
    {
        public IMEIImport()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        private void IMEI_OnKeyUp(object sender, KeyEventArgs e)
        {
//            if (e.Key == Key.Enter)
//            {
//                this.DialogResult = true;
//                this.Close();
//            }
        }

        private void OK_OnClick(object sender, RoutedEventArgs e)
        {
            this.IMEI.Text = (this.IMEI.Text + "").ToUpper();
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
} 
