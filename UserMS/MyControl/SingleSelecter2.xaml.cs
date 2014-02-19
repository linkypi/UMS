using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace UserMS
{
    public partial class SingleSelecter2
    {
        public object SelectedItem {
            get { return this.radGridView1.SelectedItem; }
        }
        private readonly List<TreeViewModel> _leftTree;
        private readonly IEnumerable _items;
        private readonly string  _FilterKey;
        private FilterDescriptor fd2 = new FilterDescriptor();
        public SingleSelecter2()
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
        public SingleSelecter2(List<TreeViewModel> LeftTree, IEnumerable Items,  string FilterKey, string[] GridColumn, string[] GridTitle)
        {
            InitializeComponent();
            this.Height = 422;
            this.Owner = Application.Current.MainWindow;
            _leftTree = LeftTree;
            _items = Items;

            _FilterKey = FilterKey;
            this.radGridView1.FilterDescriptors.SuspendNotifications();
            this.radGridView1.FilterDescriptors.Clear();

            
            if (LeftTree != null)
            {
            }
            else
            {
                this.TreeView.Visibility = Visibility.Collapsed;
            }

            
            fd2.Member = FilterKey;
            fd2.Operator=FilterOperator.Contains;
            fd2.Value = "";
            this.radGridView1.FilterDescriptors.Add(fd2);
            this.radGridView1.FilterDescriptors.ResumeNotifications();




            if (GridTitle != null && GridColumn.Length != GridTitle.Length)
            {
                throw new Exception("列数和标题数量不一致");
            }
            else
            {
                for (int i = 0; i < GridColumn.Length; i++)
                {
                    GridViewDataColumn gc1 = new GridViewDataColumn {DataMemberBinding = new Binding(GridColumn[i])};
                    if (GridTitle != null) gc1.Header = GridTitle[i];
                    gc1.IsFilterable = false;
                    
                    this.radGridView1.Columns.Add(gc1);
                    

                }
            }


            this.radGridView1.ItemsSource = Items;
            

        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.radGridView1.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择项目");
                return;
            }
            this.DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void TreeView_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            TreeViewModel s = ((Telerik.Windows.Controls.RadTreeView)sender).SelectedItem as TreeViewModel;
            if (s!=null){

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
            
        }

       

       
        private void Inputtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            fd2.Value = ((TextBox)sender).Text;
            
        }
    }
}

