using System.Windows;

namespace UserMS.Views.LMS
{
    public partial class AlterRole
    {
        public AlterRole()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            //PublicRequestHelp prh = new PublicRequestHelp(,,,);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

