using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls.GridView;

using UserMS.Common;

namespace UserMS.Sys_tem.Pro
{
    /// <summary>
    /// AddProMain.xaml 的交互逻辑
    /// </summary>
    public partial class AddProMain : BasePage
    {
        List<API.ProModel> Pros=  new List<API.ProModel>();
        public AddProMain()
        {
            InitializeComponent();
            Get_Class_Type();
            ProNameDG.ItemsSource = Pros;
        }
        #region 获取商品类别和品牌
        private void Get_Class_Type()
        {
            ClassName.ItemsSource = Store.ProClassInfo;
            ClassName.DisplayMemberPath = "ClassName";
            ClassName.SelectedValuePath = "ClassID";

            TypeName.ItemsSource = Store.ProTypeInfo;
            TypeName.DisplayMemberPath = "TypeName";
            TypeName.SelectedValuePath = "TypeID";

            ProName.ItemsSource = Store.ProNameInfo;
            ProName.DisplayMemberPath = "MainName";
            ProName.SelectedValuePath = "ID";
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<API.ProModel> Production = Pros;
            //Production = Production == null ? new List<API.ProModel>() : Production;
            if (ClassName.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择类别或类别不存在！");
                return;
            }
            if (TypeName.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择品牌或品牌不存在！");
                return;
            }
            if (ProName.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择型号或型号不存在！");
                return;
            }
            API.ProModel Pro = new API.ProModel() { 
                ClassID = (int)ClassName.SelectedValue, 
                ProClassName = ClassName.Text, 
                TypeID = (int)TypeName.SelectedValue, 
                ProMainID=Convert.ToInt32(ProName.SelectedValue),
                ProTypeName = TypeName.Text,
                ProName=ProName.Text };
            Production.Add(Pro);
            //ProNameDG.ItemsSource = Production;
            ProNameDG.Rebind();
        }

        private void DelPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            //List<API.ProModel> ProNameSource = ProNameDG.ItemsSource as List<API.ProModel>;
            this.Pros.Remove(this.ProNameDG.SelectedItem as API.ProModel);
            ProNameDG.Rebind();
        }

        private void Sumbit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
    

            List<API.ProModel> Source = ProNameDG.ItemsSource as List<API.ProModel>;
            
            if (Source == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加新商品！");
                return;
            }

            var query = (from b in Source
                        where ((from Item in Source where b.ProClassName == Item.ProClassName && b.ProTypeName == Item.ProTypeName && b.ProName == Item.ProName select Item).Count() > 1)||b.ProName==null
                        select b).ToList();
                      
            if (query.Count() > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"不能添加相同的商品或商品名称不能为空！");
                return;
            }
            
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定提交？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
           PublicRequestHelp helper = new PublicRequestHelp(isbusy, MethodIDStore.AddProMainInfo, new object[] { Source }, Sumbit_Completed);
        }
        protected void Sumbit_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    Store.ProMainInfo.AddRange((List<API.Pro_ProMainInfo>)e.Result.Obj);
                    //this.ProNameDG.ItemsSource = null;
                    this.Pros.Clear();
                    this.ProNameDG.Rebind();
                }
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }

        private void ProNameDG_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
             
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {

                var cell = originalSender.ParentOfType<GridViewCell>();
                //if (cell != null)
                //{
                //    MessageBox.Show("The double-clicked cell is " + cell.Value);
                //}

                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    API.ProModel model = (API.ProModel)row.DataContext;
                    RichTextBoxEditor r = new RichTextBoxEditor(model.Introduction);
                    r.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    bool? x= r.ShowDialog();
                    if (x == true)
                    {
                        model.Introduction = r.Introduction.Text;
                    }
                    this.ProNameDG.Rebind();
                }
            }
        }
     
    }
}
