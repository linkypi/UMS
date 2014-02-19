using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserMS.Sys_tem.Package
{
    /// <summary>
    /// PackageGroup.xaml 的交互逻辑
    /// </summary>
    public partial class PackageGroup : Page
    {
        public PackageGroup()
        {
            InitializeComponent();
        }
        List<API.View_PackageGroupTypeInfo> models = new List<API.View_PackageGroupTypeInfo>();

        private string menuid = "";
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch (Exception ex)
            {
                menuid = "248";
            }
            GridList.ItemsSource = models;

            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 264, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.View_PackageGroupTypeInfo> list = e.Result.Obj as List<API.View_PackageGroupTypeInfo>;
                models.Clear();
                models.AddRange(list);
                GridList.Rebind();
            }
        }

        private void Clear()
        {
            this.updateGroupName.Text = string.Empty;
            this.updClassName.Text = string.Empty;
            this.addClassName.Text = string.Empty;
            this.addGroupName.Text = string.Empty;
        }

 
        /// <summary>
        /// 选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (GridList.SelectedItem == null) return;


            API.View_PackageGroupTypeInfo pac = (API.View_PackageGroupTypeInfo)GridList.SelectedItem;

            this.updateGroupName.Text = pac.GroupName;
            this.updClassName.Text = pac.ClassName;
        }

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (GridList.SelectedItem == null)
            {
                MessageBox.Show("请选择要更新的分组套餐！");
                return;
            }
          

            if (string.IsNullOrEmpty(updateGroupName.Text))
            {
                MessageBox.Show("套餐分组名称不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(updClassName.Text))
            {
                MessageBox.Show("套餐分类名称不能为空！");
                return;
            }
            API.View_PackageGroupTypeInfo gt = GridList.SelectedItem as API.View_PackageGroupTypeInfo;
        

            if (gt.OldGroupName != updateGroupName.Text.Trim()|| gt.OldClassName != updClassName.Text.Trim())
            {
                if (MessageBox.Show("确定修改吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }
                API.View_PackageGroupTypeInfo group = new API.View_PackageGroupTypeInfo();
                group.ID = gt.ID;
                 group.ClassName = updClassName.Text.Trim();
                group.GroupName = updateGroupName.Text.Trim();

                PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 267, new object[] { group }, new EventHandler<API.MainCompletedEventArgs>(UpdateCompleted));
            }
            else
            {
                MessageBox.Show("已是最新分组套餐,无需更新！");
            }
        }

        private void UpdateCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            //更新成功后同步数据
            if (e.Result.ReturnValue)
            {
                Clear();
                PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 264, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

            }
            MessageBox.Show(e.Result.Message);

        }


        #endregion

        #region  添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(addGroupName.Text))
            {
                MessageBox.Show("分组名称不能为空！");
                return;
            }

            if (string.IsNullOrEmpty(addClassName.Text))
            {
                MessageBox.Show("类名称不能为空！");
                return;
            }

            if (MessageBox.Show("确定添加吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            API.View_PackageGroupTypeInfo model = new API.View_PackageGroupTypeInfo();
            model.ClassName = addClassName.Text.Trim();
            model.GroupName = addGroupName.Text.Trim();
           
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 265, new object[] { model }, new EventHandler<API.MainCompletedEventArgs>(AddCompleted));

        }

        private void AddCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "添加失败: 服务器错误\n" + e.Error.Message);
                return;
            }

            if (e.Result.ReturnValue)
            {
                Clear();
                PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 264, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

            }
            MessageBox.Show(e.Result.Message);

         
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridList.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的分组套餐！");
                return;
            }
         
            if (MessageBox.Show("确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            API.View_PackageGroupTypeInfo pac = GridList.SelectedItem as API.View_PackageGroupTypeInfo;
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 266, new object[] { pac.ID }, new EventHandler<API.MainCompletedEventArgs>(DelCompleted));

        }

        private void DelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "添加失败: 服务器错误\n" + e.Error.Message);
                return;
            }

            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 264, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

            }
        }


        #endregion 

      
    }
}
