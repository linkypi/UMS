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
    public partial class SingleSelecter
    {
        public object SelectedItem {
            get { return this.radGridView1.SelectedItem; }
        }
        private readonly List<TreeViewModel> _leftTree;
        private readonly IEnumerable _items;
        private readonly string _ForeignKey;
        private readonly string  _FilterKey;
        private FilterDescriptor fd1 = new FilterDescriptor();
        private FilterDescriptor fd2 = new FilterDescriptor();
        public SingleSelecter()
        {
            InitializeComponent();
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
        public SingleSelecter(List<TreeViewModel> LeftTree, IEnumerable Items, string ForeignKey, string FilterKey, string[] GridColumn, string[] GridTitle)
        {
            InitializeComponent();
            this.Height = 422;
            this.Owner = Application.Current.MainWindow;
            _leftTree = LeftTree;
            _items = Items;
            _ForeignKey = ForeignKey;
            _FilterKey = FilterKey;
            this.radGridView1.FilterDescriptors.SuspendNotifications();
            this.radGridView1.FilterDescriptors.Clear();

            
            if (LeftTree != null)
            {
                this.TreeView.ItemsSource = LeftTree;
                fd1.Member = ForeignKey;
                fd1.Operator = FilterOperator.IsEqualTo;
                fd1.MemberType = typeof(string);
                fd1.Value = "";
                
                this.radGridView1.FilterDescriptors.Add(fd1);
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
                GridViewSelectColumn gs = new GridViewSelectColumn();
                this.radGridView1.Columns.Add(gs);
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
        public SingleSelecter(List<TreeViewModel> LeftTree, IEnumerable Items, string ForeignKey, string FilterKey, string[] GridColumn, string[] GridTitle ,bool IsInt)
        {
            InitializeComponent();
            this.Height = 422;
            this.Owner = Application.Current.MainWindow;
            _leftTree = LeftTree;
            _items = Items;
            _ForeignKey = ForeignKey;
            _FilterKey = FilterKey;
            this.radGridView1.FilterDescriptors.SuspendNotifications();
            this.radGridView1.FilterDescriptors.Clear();


            if (LeftTree != null)
            {
                this.TreeView.ItemsSource = LeftTree;
                fd1.Member = ForeignKey;
                fd1.Operator = FilterOperator.IsEqualTo;
                fd1.MemberType = typeof(int);
                fd1.Value = 0;

                this.radGridView1.FilterDescriptors.Add(fd1);
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
                GridViewSelectColumn gs = new GridViewSelectColumn();
                this.radGridView1.Columns.Add(gs);
                for (int i = 0; i < GridColumn.Length; i++)
                {
                    GridViewDataColumn gc1 = new GridViewDataColumn { DataMemberBinding = new Binding(GridColumn[i]) };
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

                if (string.IsNullOrEmpty(s.ID))
                    fd1.Value = s.NewID;
                else
                    fd1.Value = s.ID;
                Inputtxt.Text = "";
            }
            
        }

       

       
        private void Inputtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            fd2.Value = ((TextBox)sender).Text;
            
        }
    }
}

