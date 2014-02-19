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
using Telerik.Windows.Controls;

namespace UserMS.Sys_tem.Package
{
    /// <summary>
    /// PackageType.xaml 的交互逻辑
    /// </summary>
    public partial class PackageType : Page
    {
        public PackageType()
        {
            InitializeComponent();
        }

        private string menuid = "";
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded_1;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch (Exception ex)
            {
                menuid = "239";
            }


            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 246, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            RadTreeView1.Items.Clear();
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.View_PackageSalesNameInfo> models = (e.Result.Obj as List<API.View_PackageSalesNameInfo>).Where(p=>p.ID>=4).ToList();
                foreach (var item in models)
                {
                    if (item.Parent == 0)
                    {
                        RadTreeViewItem t1 = new RadTreeViewItem();
                        t1.Header = item.SalesName;
                        t1.DataContext = item;

                        Recursion(models, item, t1);
                        RadTreeView1.Items.Add(t1);
                    }
                }
            }
        }

        private void Clear()
        {
            this.updateSalesName.Text = string.Empty;
            this.updNote.Text = string.Empty;
            this.addNote.Text = string.Empty;
            this.addSalesName.Text = string.Empty;
        }

        /// <summary>
        /// 递归获取
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parent"></param>
        private void Recursion(List<API.View_PackageSalesNameInfo> models, API.View_PackageSalesNameInfo item, RadTreeViewItem parent)
        {
            var ss = from a in models
                     where a.Parent == item.ID
                     select a;

            if (ss.Count() != 0)
            {
                foreach (var child in ss)
                {
                    RadTreeViewItem t1 = new RadTreeViewItem();
                    t1.Header = child.SalesName;
                    t1.DataContext = child;
                    Recursion(models, child, t1);
                    parent.Items.Add(t1);
                }
            }
        }

        /// <summary>
        /// 选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadTreeView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            #region 绑定商品列表 并且选中已经选择的商品
            API.View_PackageSalesNameInfo pac = (API.View_PackageSalesNameInfo)((RadTreeViewItem)e.AddedItems[0]).DataContext;

            this.updateSalesName.Text = pac.SalesName;
            this.updNote.Text = pac.Note;
            #endregion

        }

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if(RadTreeView1.SelectedItem == null)
            {
                MessageBox.Show("请选择要更新的套餐！");
                return;
            }
             API.View_PackageSalesNameInfo pac = (API.View_PackageSalesNameInfo)((RadTreeViewItem)RadTreeView1.SelectedItem).DataContext;


             if (string.IsNullOrEmpty(updateSalesName.Text))
             {
                 MessageBox.Show("套餐分类名称不能为空！");
                 return;
             }

             pac.Note = updNote.Text;
             pac.SalesName = updateSalesName.Text;

             if (pac.OldSalesName != updateSalesName.Text || pac.OldNote != updNote.Text)
             {
                 if (MessageBox.Show("确定修改吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                 {
                     return;
                 }
                 
                 PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 249, new object[] { pac }, new EventHandler<API.MainCompletedEventArgs>(UpdateCompleted));
             }
             else
             {
                 MessageBox.Show("当前套餐已是最新套餐,无需更新！");
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
          
            MessageBox.Show(e.Result.Message);

            API.View_PackageSalesNameInfo model = e.Result.Obj as API.View_PackageSalesNameInfo;
            API.Package_SalesNameInfo aa = new API.Package_SalesNameInfo();
            aa.ID = (int)model.ID;
            object obj = Find(RadTreeView1.Items, aa);
            if (obj == null)
            {
                return;
            }
            RadTreeViewItem rr = obj as RadTreeViewItem;
            API.View_PackageSalesNameInfo pac = (API.View_PackageSalesNameInfo)rr.DataContext;


            API.Package_SalesNameInfo parent = new API.Package_SalesNameInfo();
            parent.ID = (int)model.Parent;

          
            //更新成功后同步数据
            if (e.Result.ReturnValue)
            {
                Clear();
                rr.Header = model.SalesName;
                rr.DataContext = model;
                pac.Note = model.Note;
                pac.SalesName = model.SalesName;
            }
            //else 
            //{
            //    pac.Note = model.OldNote;
            //    pac.SalesName = model.OldSalesName;
            //}
         
        }

        /// <summary>
        /// 查找节点
        /// </summary>
        /// <param name="items"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private object Find(ItemCollection items, API.Package_SalesNameInfo model)
        {
            foreach (var item in items)
            {
                API.View_PackageSalesNameInfo pac = (API.View_PackageSalesNameInfo)((RadTreeViewItem)item).DataContext;
                if (pac.ID == model.ID)
                {
                    return item;
                }
                object obj =  Find((item as RadTreeViewItem).Items,model);
                if (obj != null)
                {
                    return obj;
                }
            }
           return null;
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
            if (RadTreeView1.SelectedItem == null)
            {
                MessageBox.Show("请选择父级套餐！");
                return;
            }
            API.View_PackageSalesNameInfo pac = (API.View_PackageSalesNameInfo)((RadTreeViewItem)RadTreeView1.SelectedItem).DataContext;

            if (string.IsNullOrEmpty(addSalesName.Text))
            {
                MessageBox.Show("套餐分类名称不能为空！");
                return;
            }

            if (MessageBox.Show("确定添加吗？","提示",MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            //if (string.IsNullOrEmpty(addNote.Text))
            //{
            //    MessageBox.Show("备注不能为空！");
            //    return;
            //}
            API.Package_SalesNameInfo model = new API.Package_SalesNameInfo();
            model.Note = addNote.Text;
            model.SalesName = addSalesName.Text;
            model.Parent = pac.ID;
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 247, new object[] { model }, new EventHandler<API.MainCompletedEventArgs>(AddCompleted));
        
        }

        private void AddCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "添加失败: 服务器错误\n" + e.Error.Message);
                return;
            }

            MessageBox.Show(e.Result.Message);

            //更新成功后同步数据
            if (e.Result.ReturnValue)
            {
                Clear();
                API.Package_SalesNameInfo model = e.Result.Obj as API.Package_SalesNameInfo;

                API.Package_SalesNameInfo parent = new API.Package_SalesNameInfo();
                parent.ID =(int) model.Parent;

                API.View_PackageSalesNameInfo newModel = new API.View_PackageSalesNameInfo();
                newModel.SalesName = model.SalesName;
                newModel.ID = model.ID;
                newModel.Note = model.Note;
                newModel.OldSalesName = model.SalesName;
                newModel.OldNote = model.Note;
                newModel.Parent = model.Parent;
                object obj = Find(RadTreeView1.Items, parent);
                if (obj == null)
                {
                    return;
                }
                RadTreeViewItem item = new RadTreeViewItem();
                item.DataContext = newModel;
                item.Header = newModel.SalesName;
               ((RadTreeViewItem)obj).Items.Add(item);
               
            }
        }

        #endregion  

        private void Find(RadTreeViewItem item,List<int> idlist)
        {
           
            foreach (var xx in item.Items)
            {
                RadTreeViewItem dd = xx as RadTreeViewItem;
                if (dd.CheckState == System.Windows.Automation.ToggleState.Off)
                {
                    continue;
                }
                if (dd.CheckState == System.Windows.Automation.ToggleState.On)
                {
                    idlist.Add((dd.DataContext as API.View_PackageSalesNameInfo).ID);
                    break;
                }
                Find(dd,idlist);
            }
        }

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<int> idlist = new List<int>();
            foreach (var item in RadTreeView1.Items)
            {
                RadTreeViewItem dd = item as RadTreeViewItem;
                if (dd.CheckState == System.Windows.Automation.ToggleState.Off)
                {
                    continue;
                }
                //if (dd.CheckState == System.Windows.Automation.ToggleState.On)
                //{
                //    idlist.Add((dd.DataContext as API.View_PackageSalesNameInfo).ID);
                //}
                Find(dd,idlist);
            }


            if (idlist.Count == 0)
            {
                MessageBox.Show("请选择要删除的套餐！");
                return;
            }
        
            if (MessageBox.Show("确定删除选中项吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
       
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 248, new object[] { idlist }, new EventHandler<API.MainCompletedEventArgs>(DelCompleted));

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
            //更新成功后同步数据
            if (e.Result.ReturnValue)
            {
                Clear();
                PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 246, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
              
            }
        }


        #endregion 

    }
}
