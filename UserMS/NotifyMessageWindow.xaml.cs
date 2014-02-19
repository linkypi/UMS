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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace UserMS
{
    /// <summary>
    /// Interaction logic for NotifyMessageWindow.xaml
    /// </summary>
    public partial class NotifyMessageWindow : Window
    {
        private RadTabControl tab;
        public NotifyMessageWindow()
        {
            InitializeComponent();
        }
        public NotifyMessageWindow(RadTabControl tab)
        {
            InitializeComponent();
            this.tab = tab;
        }
        private void MSGInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NotifyMessageViewModel m = (NotifyMessageViewModel)this.DataContext;
            if (m == null) return;

            Logger.AddPageToTab(this.tab, m.Message);

            this.Close();
        }

        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
