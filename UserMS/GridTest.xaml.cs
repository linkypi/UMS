using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using UserMS.API;
using UserMS.MyControl;

namespace UserMS
{
    public partial class GridTest : Page
    {

        public List<API.Pro_ProInfo> proInfoList = new List<Pro_ProInfo>(); 
  
        public GridTest()
        {
            InitializeComponent();
        

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MyColumnSettings my=new MyColumnSettings();
            Hashtable v=new Hashtable();
            v.Add("a","b");
            v.Add("b", "1");
            v.Add("c", "2");
            v.Add("d", "3");
            //my.Selection = v;
            //bool result=my.OpenColumnSetting("abcd", v);
            //ObservableCollection<DictionaryEntry> columns=MyColumnSettings.GetColumns("abcd");
            
        }
    }
}
