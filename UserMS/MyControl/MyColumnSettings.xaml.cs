using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using UserMS.API;

namespace UserMS.MyControl
{
    /// <summary>
    /// MyColumnSettings.xaml 的交互逻辑
    /// </summary>
    
    public partial class MyColumnSettings : RadWindow
    {

        public static ObservableCollection<Demo_ReportViewColumnInfo> GetColumns(string reportname)
        {
            try
            {
                DataContractSerializer formatter = new DataContractSerializer(typeof(ObservableCollection<Demo_ReportViewColumnInfo>), new Type[] { typeof(Demo_ReportViewColumnInfo) });
                var store = IsolatedStorageFile.GetUserStoreForAssembly();
                using (var stream = store.OpenFile(reportname+"_settings.cfg", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    return (ObservableCollection<Demo_ReportViewColumnInfo>)formatter.ReadObject(stream);
                }
                
            }
            catch
            {
                return new ObservableCollection<Demo_ReportViewColumnInfo>();
            }
        }

        public bool OpenColumnSetting(string reportname, List<Demo_ReportViewColumnInfo> selection)
        {
            this.Selection = selection;
            var selected = GetColumns(reportname);
            foreach (Demo_ReportViewColumnInfo demoReportViewColumnInfo in selected)
            {
                if (this.Selection2.Any(p => p.ID==demoReportViewColumnInfo.ID))
                {
                    this.Selection2.Remove(this.Selection.First(p => p.ID == demoReportViewColumnInfo.ID));
                }
            }
            this.SelectedItems = selected;
            this.ShowDialog();
            if (this.DialogResult == true)
            {
                try
                {
                    DataContractSerializer formatter = new DataContractSerializer(typeof(ObservableCollection<Demo_ReportViewColumnInfo>), new Type[] { typeof(Demo_ReportViewColumnInfo) });
                    var store = IsolatedStorageFile.GetUserStoreForAssembly();
                    using (var stream = store.OpenFile(reportname + "_settings.cfg", FileMode.Create, FileAccess.Write))
                    {
                        formatter.WriteObject(stream,SelectedItems);
                    }
                }
                catch (Exception)
                {

                    return false;
                }
               

            }
            return this.DialogResult == true;
        }

        private List<Demo_ReportViewColumnInfo> _selection;
        private ObservableCollection<Demo_ReportViewColumnInfo> _selectedItems;


        public ObservableCollection<Demo_ReportViewColumnInfo> SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; }
        }

        public ObservableCollection<Demo_ReportViewColumnInfo> Selection2
        {
            get; set;
        }

        public List<Demo_ReportViewColumnInfo> Selection
        {
            get { return _selection; }
            set
            {
               
                _selection = value;
                Selection2.Clear();
                foreach (Demo_ReportViewColumnInfo dictionaryEntry in _selection)
                {
                    Selection2.Add(dictionaryEntry);
                }
            


            }
        }

        public MyColumnSettings()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            this.SelectedItems = new ObservableCollection<Demo_ReportViewColumnInfo>();
            this.Selection2 = new ObservableCollection<Demo_ReportViewColumnInfo>();
            this.DataContext = this;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
