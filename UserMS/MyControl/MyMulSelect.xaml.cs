using System.Windows;
using System.Windows.Controls;

namespace UserMS.MyControl
{
    public partial class MyMulSelect : UserControl
    {
        public MyMulSelect()
        {
            InitializeComponent();
        }

        private RoutedEventHandler _searchEvent;

        public RoutedEventHandler SearchEvent
        {
            get { return _searchEvent; }
            set
            {
                if (_searchEvent != null)
                {
                    this.btnSearch.Click -= _searchEvent;
                }
                _searchEvent = value;
                this.btnSearch.Click += _searchEvent;
            }
        }

        public  string Text
        {
            get { return this.txt.Text; }
            set { this.txt.Text = value; }
        }

    }
}
