using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

//
//using SelectionChangedEventHandler = Telerik.Windows.Controls.SelectionChangedEventHandler;

namespace UserMS
{
	public partial class MyAutoTextBox : UserControl
	{
	    public  object SelectedItem {
	        get { return this.TextBox.SelectedItem; }
            set { this.TextBox.SelectedItem = value; }
	         }
	    public object SelectedItems
	    {
            get { return this.TextBox.SelectedItems; }
	    }
	    public Telerik.Windows.Controls.Primitives.AutoCompleteSelectionMode SelectionMode
	    {
            get { return this.TextBox.SelectionMode; }
            set{ TextBox.SelectionMode=value;}
	    }

   

   
   
	    public Telerik.Windows.Controls.TextSearchMode TextSearchMode
	    {
            get { return this.TextBox.TextSearchMode; }
            set { TextBox.TextSearchMode = value; }
	    }

	    public System.Collections.IEnumerable ItemsSource
	    {
            get { return this.TextBox.ItemsSource; }
            set { TextBox.ItemsSource = value; }
	    }

	    public string DisplayMemberPath
	    {
            get { return this.TextBox.DisplayMemberPath; }
            set { TextBox.DisplayMemberPath = value; }
	    }
        public string TextSearchPath
        { get { return this.TextBox.TextSearchPath; }
            set { TextBox.TextSearchPath = value; }
        }

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

	    public SelectionChangedEventHandler TextBox_SelectionChanged
	    {
	        get { return _textBox_selectionChanged; }
	        set
	        {
                this.TextBox.SelectionChanged -= _textBox_selectionChanged;
                _textBox_selectionChanged = value;
                this.TextBox.SelectionChanged += _textBox_selectionChanged;
	        }
	    }
        
	    private RoutedEventHandler _searchEvent;
	    private SelectionChangedEventHandler _textBox_selectionChanged;
	    //public delegate  void _SearchButton_Click

        public string Text { get; set; }

       
		public MyAutoTextBox()
		{
			// Required to initialize variables
            InitializeComponent();
            this.TextBox.TextSearchMode = TextSearchMode.Contains;
		    _searchEvent = NullEvent;
		    _textBox_selectionChanged = NullEvent;
            this.SearchButton.Click += _searchEvent;
            this.TextBox.SelectionChanged += TextBoxOnSelectionChanged;
            this.TextBox.SelectionChanged += _textBox_selectionChanged;
            this.TextBox.SearchTextChanged += TextBox_SearchTextChanged;
            
		}

	    private void TextBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
	    {

	        var a=this.TextBox.FindChildByType<TextBox>();
	        this.Text = a.Text;
	    }

	    void TextBox_SearchTextChanged(object sender, EventArgs e)
	    {
	        this.Text = this.TextBox.SearchText;
            this.TextBox.SelectedItems = null;
	        this.TextBox.SelectedItem = null;

	    }

        public MyAutoTextBox(IEnumerable items, string DisplayMemberPath, string TextSearchPath)
        {
            InitializeComponent();
            this.TextBox.ItemsSource = items;
            this.TextBox.DisplayMemberPath = DisplayMemberPath;
            this.TextBox.TextSearchPath = TextSearchPath;
            this.TextBox.TextSearchMode = TextSearchMode.Contains;
            _searchEvent = NullEvent;
            _textBox_selectionChanged = NullEvent;
            this.SearchButton.Click += _searchEvent;
            this.TextBox.SelectionChanged += _textBox_selectionChanged;
            
        }
        public void  NullEvent(object sender, object e)
        {
           // MessageBox.Show(System.Windows.Application.Current.MainWindow,"bb");
        }

	    

	}
}