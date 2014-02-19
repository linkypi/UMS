using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace UserMS.Sys_tem.Property
{
    public partial class PropertyMain : BasePage
    {
        //List<API.Pro_PropertyValue> PropertyList;
        const int AddMethodID=162;
        const int MethodID = 166;
        const int UpdateMethodID = 164;
        public PropertyMain()
        {
            InitializeComponent();
            //PropertyList = new List<API.Pro_PropertyValue>();
            //PropertyValueDG.ItemsSource = PropertyList;
            InitGrid2();
            GetModel();
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
        void GetModel()
        {
            PublicRequestHelp help = new PublicRequestHelp(this.isbusy, MethodID, new object[] { }, SearchCompleted);
        }

        void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {

            this.isbusy.IsBusy = false;
            if (e.Error == null)
            {
                Logger.Log(e.Result.Message + "");

                if (e.Result.ReturnValue == true)
                {
                    PropertyValueDG.ItemsSource = null;
                    this.PropertyValueDG.Rebind();
                    TbPropertyName.Text = string.Empty;
                    TbValue.Text = string.Empty;
                    List<API.Pro_Property> Property = e.Result.Obj as List<API.Pro_Property>;
                    PropertyDG.ItemsSource = Property;

                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");

            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion 
        #region 添加属性值
        private void AddPropertyValue_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            List<API.Pro_PropertyValue> PropertyList = PropertyValueDG.ItemsSource as List<API.Pro_PropertyValue>;
            int Count = 0;
            if (PropertyList != null)
            {
                 Count = (from b in PropertyList
                             where b.Value == TbValue.Text.Trim()
                             select b).Count();             
            }
            if (Count == 0)
            {
                if (PropertyDG.SelectedItem != null)
                {
                    API.Pro_Property Property = PropertyDG.SelectedItem as API.Pro_Property;
                    Property.Flag = false;
                }
                if (PropertyList == null)
                {
                    PropertyList = new List<API.Pro_PropertyValue>();
                    PropertyValueDG.ItemsSource = PropertyList;
                }
                if (!string.IsNullOrEmpty(TbValue.Text))
                {
                    API.Pro_PropertyValue NewProperty = new API.Pro_PropertyValue() { Value = TbValue.Text.Trim() };
                    PropertyList.Add(NewProperty);
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
                if (PropertyDG.SelectedItem != null)
                {
                    API.Pro_Property Property1 = PropertyDG.SelectedItem as API.Pro_Property;
                    Property1.Flag = false;
                }
            }
        }
        #endregion 
        #region 提交添加属性
        private void AddProperty_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {         
            if (string.IsNullOrEmpty(TbPropertyName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加属性名称");
                return;
            }
            List<API.Pro_PropertyValue> PropertyList = PropertyValueDG.ItemsSource as List<API.Pro_PropertyValue>;
            if (PropertyList.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加属性值");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定新增？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            API.Pro_Property Property = new API.Pro_Property();
            Property.Cate = TbPropertyName.Text.Trim();

            Property.Pro_PropertyValue = new List<API.Pro_PropertyValue>();
         
            Property.Pro_PropertyValue.AddRange(PropertyList);
            PublicRequestHelp helper = new PublicRequestHelp(isbusy, AddMethodID, new object[] { Property }, MyClient_Completed);
        }
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {

            this.isbusy.IsBusy = false;
            if (e.Error == null)
            {
                Logger.Log(e.Result.Message + "");
               
                if (e.Result.ReturnValue == true)
                {
                    GetModel();
                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");

            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion 
        #region 提交修改
        private void UpdateProperty_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
                API.Pro_Property PropertyList = PropertyDG.SelectedItem as API.Pro_Property;
                if (PropertyList == null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要提交的商品！");
                    return;
                }
                List<API.Pro_Property> Property = new List<API.Pro_Property>();
                Property.Add(PropertyList);
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定修改数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            PublicRequestHelp helper = new PublicRequestHelp(isbusy, UpdateMethodID, new object[] { Property }, Client_Completed);
        }
        protected void Client_Completed(object sender, API.MainReportCompletedEventArgs e)
        {

            this.isbusy.IsBusy = false;
            if (e.Error == null)
            {
                Logger.Log(e.Result.Message + "");
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
                if (e.Result.ReturnValue == true)
                {
                    GetModel();
                }                         
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
                this.TbPropertyName.Text = Property.Cate;
                this.PropertyValueDG.ItemsSource = Property.Pro_PropertyValue;
            }
        }
        #endregion 
        #region 属性值选择改变时发生
        private void PropertyValueDG_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            this.TbValue.Text = string.Empty;
            API.Pro_PropertyValue PropertyValue = PropertyValueDG.SelectedItem as API.Pro_PropertyValue;
            if (PropertyValue != null)
            {
                this.TbValue.Text = PropertyValue.Value;
            }
        }
        #endregion 
        #region 文本改变时发生
          private void TbPropertyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbPropertyName.Text))
            {
                API.Pro_Property Property = PropertyDG.SelectedItem as API.Pro_Property;
                if (Property != null)
                {
                    if (Property.Cate != TbPropertyName.Text.Trim())
                    {
                        Property.Cate = TbPropertyName.Text.Trim();
                        Property.Flag = false;
             
                        Property.Note = "XG";
                        this.PropertyDG.Rebind();
                    }
       
                }
            }
        }

        private void TbValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbValue.Text))
            {
                API.Pro_PropertyValue PropertyValue = PropertyValueDG.SelectedItem as API.Pro_PropertyValue;
                if (PropertyValue != null )
                {
                    API.Pro_Property Property = PropertyDG.SelectedItem as API.Pro_Property;
                    if (PropertyValue.Value != TbValue.Text.Trim())
                    {
                        PropertyValue.Value = TbValue.Text.Trim();
                   
                        Property.Flag = false;
                        PropertyValue.Note = "XG";
                        this.PropertyValueDG.Rebind();
                    }

                }
            }
        }
        #endregion 
        #region 重置数据
        private void CancelProperty_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"重置所有数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            GetModel();
        }
        #endregion 

    }
}
