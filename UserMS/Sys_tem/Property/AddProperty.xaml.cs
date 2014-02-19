using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace UserMS.Sys_tem.Property
{
    public partial class AddProperty : BasePage
    {
         //List<API.Pro_PropertyValue> PropertyList;
        const int AddMethodID=162;
        const int MethodID = 166;
        const int UpdateMethodID = 164;
        public AddProperty()
        {
            InitializeComponent();
            //PropertyList = new List<API.Pro_PropertyValue>();
            //PropertyValueDG.ItemsSource = PropertyList;
            InitGrid2();
           // GetModel();
        }

        // 当用户导航到此页面时执行。
        #region 生成列
        private void InitGrid2()
        {
            //优惠表头GridView
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("Value");
            col.Header = "属性值";
            this.PropertyValueDG.Columns.Add(col);

            GridViewDataColumn col1 = new GridViewDataColumn();
            col1.DataMemberBinding = new System.Windows.Data.Binding("Cate");
            col1.Header = "属性";
            this.PropertyDG.Columns.Add(col1);

        }
        #endregion 
        #region 获取数据
        //void GetModel()
        //{
        //    PublicRequestHelp help = new PublicRequestHelp(this.isbusy, MethodID, new object[] { }, SearchCompleted);
        //}

        //void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        //{

        //    this.isbusy.IsBusy = false;
        //    if (e.Error == null)
        //    {
        //        Logger.Log(e.Result.Message + "");

        //        if (e.Result.ReturnValue == true)
        //        {
        //            PropertyValueDG.ItemsSource = null;
        //            this.PropertyValueDG.Rebind();
        //            TbPropertyName.Text = string.Empty;
        //            TbValue.Text = string.Empty;
        //            List<API.Pro_Property> Property = e.Result.Obj as List<API.Pro_Property>;
        //            PropertyDG.ItemsSource = Property;

        //        }
        //        else
        //            MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");

        //    }
        //    else
        //        MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        //}
        #endregion 
        #region 添加属性值

        private void BtAdd_Click_1(object sender, RoutedEventArgs e)
        {

            if (PropertyDG.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择你要添加的属性名称！");
                return;
            }
            if (string.IsNullOrEmpty(TbValue.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未填写属性值！");
                return;
            }
            List<API.Pro_PropertyValue> PropertyList = PropertyValueDG.ItemsSource as List<API.Pro_PropertyValue>;
            int Count = 0;
            if (PropertyList != null)
            {
                 Count = (from b in PropertyList
                             where b.Value == TbValue.Text.Trim()
                             select b).Count();             
            }
            API.Pro_Property PropertyItem = PropertyDG.SelectedItem as API.Pro_Property;
            if (Count == 0)
            {
                if (PropertyList == null)
                {
                  
                    PropertyItem.Pro_PropertyValue = new List<API.Pro_PropertyValue>();
                    PropertyValueDG.ItemsSource = PropertyItem.Pro_PropertyValue;
                }
                if (!string.IsNullOrEmpty(TbValue.Text))
                {
                    API.Pro_PropertyValue NewProperty = new API.Pro_PropertyValue() { Value = TbValue.Text.Trim() };
                    PropertyItem.Pro_PropertyValue.Add(NewProperty);

                
                }
            }
            PropertyValueDG.Rebind();
        }
        #endregion 
        #region 删除属性值
        private void DelPropertyValue_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.Pro_PropertyValue> Property = PropertyValueDG.ItemsSource as List<API.Pro_PropertyValue>;
            if (PropertyValueDG.SelectedItems != null)
            {
                foreach (var Item in PropertyValueDG.SelectedItems)
                {
                    Property.Remove(Item as API.Pro_PropertyValue);
                }
                PropertyValueDG.Rebind();
                //if (PropertyDG.SelectedItem != null)
                //{
                //    API.Pro_Property Property1 = PropertyDG.SelectedItem as API.Pro_Property;
                //    Property1.Flag = false;
                //}
            }
        }
        #endregion 
        #region 提交添加属性
        private void AddProperty_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {         
           
            List<API.Pro_Property> PropertyList = PropertyDG.ItemsSource as List<API.Pro_Property>;
            if (PropertyList==null||PropertyList.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加属性");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定新增？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            PublicRequestHelp helper = new PublicRequestHelp(isbusy, AddMethodID, new object[] { PropertyList }, MyClient_Completed);
        }
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {

            this.isbusy.IsBusy = false;
            if (e.Error == null)
            {
                Logger.Log(e.Result.Message + "");
               
                if (e.Result.ReturnValue == true)
                {
                    PropertyDG.ItemsSource = null;
                    PropertyDG.Rebind();
                    this.TbValue.Text = string.Empty;
                    this.PropertyValueDG.ItemsSource = null;
                    this.TbPropertyName.Text = string.Empty;
                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");

            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion 
        
        #region 属性选择改变时发生
        private void PropertyDG_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            this.TbValue.Text = string.Empty;
            this.PropertyValueDG.ItemsSource = null;
            this.TbPropertyName.Text = string.Empty;
            API.Pro_Property Property = PropertyDG.SelectedItem as API.Pro_Property;
            if (Property != null)
            {
               // this.TbPropertyName.Text = Property.Cate;
                this.PropertyValueDG.ItemsSource = Property.Pro_PropertyValue;
            }
        }
        #endregion 
        
        

        private void AddNewRow_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
         
         
        }

        private void DelNewRow_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (PropertyDG.SelectedItem == null)
                return;
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除属性？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            List<API.Pro_Property> PropertyList = PropertyDG.ItemsSource as List<API.Pro_Property>;
            PropertyList.Remove(PropertyDG.SelectedItem as API.Pro_Property);
            PropertyDG.Rebind();
        }

        private void BtAddProperty_Click(object sender, RoutedEventArgs e)
        {
      
            if (string.IsNullOrEmpty(TbPropertyName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未填写属性名称");
                return;
            }
          
            List<API.Pro_Property> PropertyList = PropertyDG.ItemsSource as List<API.Pro_Property>;
            int Count = 0;
            if (PropertyList != null)
            {
                Count = (from b in PropertyList

                         where b.Cate == TbPropertyName.Text.Trim()
                         select b).Count();
            }
            if (Count == 0)
            {
                if (PropertyList == null)
                {
                    PropertyList = new List<API.Pro_Property>();
                    PropertyDG.ItemsSource = PropertyList;
                }
                if (!string.IsNullOrEmpty(TbPropertyName.Text))
                {
                    API.Pro_Property Property = new API.Pro_Property();
                    Property.Cate = TbPropertyName.Text.Trim();
                    //Property.Pro_PropertyValue = new List<API.Pro_PropertyValue>();
                    //Property.Pro_PropertyValue.AddRange(PropertyValueList);
                    PropertyList.Add(Property);

                    //this.TbValue.Text = string.Empty;
                    this.PropertyValueDG.ItemsSource = null;
                    this.TbPropertyName.Text = string.Empty;
                }
            }
            PropertyDG.Rebind();
      
     
        }

    }
}
