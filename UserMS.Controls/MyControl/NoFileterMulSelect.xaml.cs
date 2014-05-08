using System;
using System.Collections;
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
using Telerik.Windows.Data;

namespace UserMS.MyControl
{
    /// <summary>
    /// UoFileterMulSelect.xaml 的交互逻辑
    /// </summary>
    public partial class NoFileterMulSelect : RadWindow
    {

            public List<object> SelectedItems { get; set; }
        public List<object> tempItems { get; set; }
        private readonly List<TreeViewModel> _leftTree;
        private readonly IEnumerable _items;

        private readonly string _FilterKey;

        private FilterDescriptor fd2 = new FilterDescriptor();
        public NoFileterMulSelect()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LeftTree">左边树List, null不显示左树</param>
        /// <param name="Items">右边可选项</param>
        /// <param name="ForeignKey">Item与左树的关联外键</param>
        /// <param name="FilterKey">筛选框筛选的键</param>
        /// <param name="GridColumn">GridView的列</param>
        /// <param name="GridTitle">GridView的标题</param>
        public NoFileterMulSelect(List<TreeViewModel> LeftTree, IEnumerable Items, string FilterKey, string[] GridColumn, string[] GridTitle)
        {
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            this.Height = 422;
            SelectedItems = new List<dynamic>();
            tempItems = new List<dynamic>();
            _leftTree = LeftTree;
            _items = Items;

            _FilterKey = FilterKey;
            this.radGridView1.FilterDescriptors.SuspendNotifications();
            this.radGridView1.FilterDescriptors.Clear();


            if (LeftTree != null)
            {
              this.TreeView.ItemsSource = LeftTree;
//                fd1.Member = ForeignKey;
//                fd1.Operator = FilterOperator.IsEqualTo;
//                fd1.MemberType = typeof(string);
//                fd1.Value = "";
//
//                this.radGridView1.FilterDescriptors.Add(fd1);
            }
            else
            {
                this.TreeView.Visibility = Visibility.Collapsed;
            }


            fd2.Member = FilterKey;
            fd2.Operator = FilterOperator.Contains;
            fd2.Value = "";
            this.radGridView1.FilterDescriptors.Add(fd2);
            this.radGridView1.FilterDescriptors.ResumeNotifications();

            if (GridTitle != null && GridColumn.Length != GridTitle.Length)
            {
                throw new Exception("列数和标题数量不一致");
            }
            else
            {
                GridViewSelectColumn sc1 = new GridViewSelectColumn();
                GridViewSelectColumn sc2 = new GridViewSelectColumn();
                this.radGridView1.Columns.Add(sc1);
                this.radGridView2.Columns.Add(sc2);
                for (int i = 0; i < GridColumn.Length; i++)
                {
                    GridViewDataColumn gc1 = new GridViewDataColumn { DataMemberBinding = new Binding(GridColumn[i]) };
                    if (GridTitle != null) gc1.Header = GridTitle[i];
                    gc1.IsFilterable = false;
                    GridViewDataColumn gc2 = new GridViewDataColumn { DataMemberBinding = new Binding(GridColumn[i]) };
                    if (GridTitle != null) gc2.Header = GridTitle[i];
                    gc2.IsFilterable = false;
                    this.radGridView1.Columns.Add(gc1);
                    this.radGridView2.Columns.Add(gc2);

                }
            }

            this.radGridView1.ItemsSource = Items;
            this.radGridView2.ItemsSource = this.tempItems;
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            tempItems.Clear();
            Close();
        }

        private void TreeView_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            TreeViewModel s = ((Telerik.Windows.Controls.RadTreeView)sender).SelectedItem as TreeViewModel;
            if (s != null)
            {
                this.radGridView1.FilterDescriptors.SuspendNotifications();
                this.radGridView1.FilterDescriptors.Clear();

                for (int i = 0; i < s.Fields.Length; i++)
                {
                    FilterDescriptor fd = new FilterDescriptor();
                    fd.Member = s.Fields[i];
                    fd.MemberType = s.Values[i].GetType();
                    fd.Value = s.Values[i];
                    this.radGridView1.FilterDescriptors.Add(fd);

                }

                this.radGridView1.FilterDescriptors.Add(fd2);
                this.radGridView1.FilterDescriptors.ResumeNotifications();

                Inputtxt.Text = "";
            }
            this.radGridView2.Rebind();
        }


        private void ButtonDown_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var selectedItem in this.radGridView1.SelectedItems)
            {
                this.tempItems.Add(selectedItem);
            }
            this.radGridView2.Rebind();
        }

        private void ButtonUp_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var selectedItem in this.radGridView2.SelectedItems)
                {
                    this.tempItems.Remove(selectedItem);
                }


            }
            catch (Exception)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"选定项目不存在");

            }
            this.radGridView2.Rebind();
        }
        private void Inputtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.radGridView1.FilterDescriptors.SuspendNotifications();
            fd2.Value = ((TextBox)sender).Text;

            this.radGridView1.FilterDescriptors.ResumeNotifications();
        }

        private void RadWindow_Closed_1(object sender, WindowClosedEventArgs e)
        {
            SelectedItems.Clear();
            foreach (var item in tempItems)
            {
                SelectedItems.Add(item);
            }
            this.tempItems.Clear();
            radGridView2.Rebind();
        }
    }
}
