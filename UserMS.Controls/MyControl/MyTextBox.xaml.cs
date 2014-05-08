using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using Telerik.Windows.Controls;

namespace UserMS.MyControl
{
    /// <summary>
    /// MyTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyTextBox : UserControl
    {
        public MyTextBox()
        {
            InitializeComponent();
           
        }

        public string  MyText
        {
            get {
                return this.GetValue(MyTextProperty) as string; 
                //return this.TextBox.Text;
            }
            set { 
                this.SetValue(MyTextProperty, value);
            this.TextBox.Text = value;
            
            }
        }


        public static readonly DependencyProperty MyTextProperty = DependencyProperty.Register(
          "MyText", typeof(string), typeof(MyTextBox));
   

	    public RoutedEventHandler SearchEvent
	    {
	        get { return _searchEvent; }
	        set
	        {
	            this.SearchButton.Click -= _searchEvent;
                _searchEvent = value;
                this.SearchButton.Click += _searchEvent;
	        }
	    }

	    private RoutedEventHandler _searchEvent;
	    private SelectionChangedEventHandler _textBox_selectionChanged;
	    //public delegate  void _SearchButton_Click

        public string Text {
            get { return this.TextBox.Text; }
            set { this.TextBox.Text = value.ToString(); } }



        public event RoutedEventHandler Click
        {
            add
            {
                _searchEvent = value;
                this.SearchButton.Click += value;
                
            }
            remove
            {
                //lock (_searchEvent)
                //{
                _searchEvent = null;
                this.SearchButton.Click -= value;
                // }
            }

        }

    }
}
