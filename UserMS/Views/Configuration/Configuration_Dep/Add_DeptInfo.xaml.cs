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
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.Configuration.Configuration_Dep
{
    /// <summary>
    /// Add_DeptInfo.xaml 的交互逻辑
    /// </summary>
    public partial class Add_DeptInfo : BasePage
    {
        List<API.Sys_DeptInfo> DepInfo;
        List<API.SeleterModel> ServiceModel = new List<API.SeleterModel>();
        string r;
        const int MethodList = 78;

        const int DeleteMethod = 100;
        const int UpdateMethod = 80;
        public Add_DeptInfo()
        {
            InitializeComponent();
            AllReadOnly();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
                GetDeptSource();//获取部门信息            
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "获取菜单ID失败！");
            }

        }
        #region 获取左边树
        /// <summary>
        /// 获取左边树
        /// </summary>
        private void GetDeptSource()
        {
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.GetDeptInfo, new object[] { }, SearchCompleted);
        }

        void SearchCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.Obj != null)
                {
                    DepInfo = new List<API.Sys_DeptInfo>();
                    DepInfo = results.Result.Obj as List<API.Sys_DeptInfo>;
                    GetLeftTree(DepInfo);
                }
            }
            else
                Logger.Log("无数据");
        }
        /// <summary>
        /// 获取父级节点
        /// </summary>
        /// <param name="Dept"></param>
        private void GetLeftTree(List<API.Sys_DeptInfo> Dept)
        {
            var ParentInfo = Dept.Where(p => p.Parent == 0);
            foreach (var item in ParentInfo)
            {
                TreeView.Items.Add(GetChildItem(item.DtpID));
            }
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        private RadTreeViewItem GetChildItem(int DeptID)
        {
            var Children = from b in DepInfo
                           where b.Parent == DeptID
                           select b;
            var pnode = from b in DepInfo
                        where b.DtpID == DeptID
                        select b;
            RadTreeViewItem parent = new RadTreeViewItem() { Header = pnode.First().DtpName, Tag = pnode.First().DtpID };
            if (pnode.First().Parent == 0) parent.IsExpanded = true;
            if (Children.Count() != 0)
            {
                foreach (var Item in Children)
                {
                    RadTreeViewItem child = GetChildItem(Item.DtpID);
                    parent.Items.Add(child);
                }
            }
            return parent;
        }
        #endregion


        //#region 重置当前数据
        //void ReSetAll()
        //{
        //    if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "是否重置单前数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
        //    {
        //        this.TreeView.Items.Clear();
        //        GetDeptSource();
        //    }
        //}
        //#endregion




        
        //#region 重置所有
        //private void BtReSet_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    ReSetAll();
        //}
        //#endregion

        #region 新增
        /// <summary>
        /// 新增下级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddChild_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            if (this.TreeView.IsEnabled == false) return;
            RadTreeViewItem Item = new RadTreeViewItem() { Header = "请填写部门信息" };

            RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
            if (TreeModel == null) return;
            TreeModel.Items.Add(Item);
            TreeModel.ExpandAll();
            TreeView.SelectedItems.Add(Item);
            this.TreeView.IsEnabled = false;
        }
        /// <summary>
        /// 新增同级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddParent_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.TreeView.IsEnabled == false) return;
            RadTreeViewItem Item = new RadTreeViewItem() { Header = "请填写部门信息" };
            RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
            if (TreeModel == null) return;
            RadTreeViewItem TreeParent = TreeModel.ParentItem;
            if (TreeParent == null)
                TreeView.Items.Add(Item);
            else
                TreeParent.Items.Add(Item);
            TreeModel.ExpandAll();
            TreeView.SelectedItems.Add(Item);
            this.TreeView.IsEnabled = false;
        }
        #endregion

        #region 选择事件
        private void TreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
            if (TreeModel == null)
            {
                AllReadOnly();
                return;
            }
            Clean();
            if (TreeModel.Tag != null)
            {
                var query = from b in DepInfo
                            where b.DtpID == Convert.ToInt32(TreeModel.Tag)
                            select b;
                if (query.Count() != 0)
                {
                    TbNote.Text = query.First().Note;
                    TbDeptName.Text = query.First().DtpName;
                    TbHead.Text = query.First().Head;
                    HeadTele.Text = query.First().HeadTele;
                }
                UnReadOnly();
            }
        }
        #endregion

        private void BTCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.TreeView.IsEnabled == false)
            {
                RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
                if (TreeModel == null) return;
                RadTreeViewItem TreeParent = TreeModel.ParentItem;
                if (TreeParent == null)
                    TreeView.Items.Remove(TreeModel);
                else
                    TreeParent.Items.Remove(TreeModel);
                TreeModel.IsExpanded = true;
                Clean();
                this.TreeView.IsEnabled = true;
            }
            else
            {
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "是否重置单前数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
                RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
                if (TreeModel == null)
                {
                    AllReadOnly();
                    return;
                }
                Clean();
                if (TreeModel.Tag != null)
                {
                    var query = from b in DepInfo
                                where b.DtpID == Convert.ToInt32(TreeModel.Tag)
                                select b;
                    if (query.Count() != 0)
                    {
                        TbNote.Text = query.First().Note;
                        TbDeptName.Text = query.First().DtpName;
                        TbHead.Text = query.First().Head;
                        HeadTele.Text = query.First().HeadTele;
                    }
                    UnReadOnly();
                }
            }
        }
        #region  文本框初始化
        private void Clean()
        {
            TbDeptName.Text = string.Empty;
            TbNote.Text = string.Empty;
            TbHead.Text = string.Empty;
            HeadTele.Text = string.Empty;
        }
        private void AllReadOnly()
        {
            this.TbDeptName.IsReadOnly = true;
            this.TbNote.IsReadOnly = true;
            this.TbHead.IsReadOnly = true;
            this.HeadTele.IsReadOnly = true;
        }
        private void UnReadOnly()
        {
            this.TbDeptName.IsReadOnly = false;
            this.TbNote.IsReadOnly = false;
            this.TbHead.IsReadOnly = false;
            this.HeadTele.IsReadOnly = false;
        }
        #endregion


        #region 新增或修改操作
        private void BTAdd_Click(object sender, RoutedEventArgs e)
        {
            if (this.TreeView.IsEnabled == false)
            {
                API.Sys_DeptInfo Dep = new API.Sys_DeptInfo();
                RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
                if (TreeModel == null) return;
                RadTreeViewItem TreeParent = TreeModel.ParentItem;
                if (TreeParent != null && TreeParent.Tag != null) Dep.Parent = Convert.ToInt32(TreeParent.Tag);
                Dep.DtpName = TbDeptName.Text.Trim();
                Dep.Head = TbHead.Text.Trim();
                Dep.HeadTele = HeadTele.Text.Trim();
                Dep.Note = TbNote.Text.Trim();
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定新增部门？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
                PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.AddDeptInfo, new object[] { Dep }, AddFinish);
            }
            else
            {
                API.Sys_DeptInfo Dep = new API.Sys_DeptInfo();
                RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
                if (TreeModel == null || TreeModel.Tag == null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "未选中任何项！");
                    return;
                }
                try
                {
                    Dep.DtpID = Convert.ToInt32(TreeModel.Tag);
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "未选中任何项！");
                    return;
                }
                Dep.DtpName = TbDeptName.Text.Trim();
                Dep.Note = TbNote.Text.Trim();
                Dep.Head = TbHead.Text.Trim();
                Dep.HeadTele = HeadTele.Text.Trim();
                if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定修改？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
                PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.UpdateDeptInfo, new object[] { Dep }, UpdateFinish);
            }
        }
        private void AddFinish(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue == true)
                {
                    API.Sys_DeptInfo DepItem = results.Result.Obj as API.Sys_DeptInfo;
                    if (DepItem == null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "返回数据失败！");
                        return;
                    }
                    DepInfo.Add(DepItem);
                    RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
                    TreeModel.Tag = DepItem.DtpID;
                    TreeModel.Header = DepItem.DtpName;
                    TbHead.Text = DepItem.Head;
                    HeadTele.Text = DepItem.HeadTele;
                    this.TreeView.IsEnabled = true;
                }
                Logger.Log(results.Result.Message);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, results.Result.Message);

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器异常！");
            }

        }
        private void UpdateFinish(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue == true)
                {
                    API.Sys_DeptInfo DepItem = results.Result.Obj as API.Sys_DeptInfo;
                    if (DepItem == null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "返回数据失败！");
                        return;
                    }
                    var query = from b in DepInfo
                                where b.DtpID == DepItem.DtpID
                                select b;
                    if (query.Count() == 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "加载数据出错！");
                        return;
                    }
                    DepInfo.Remove(query.First());
                    DepInfo.Add(DepItem);
                    RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
                    TreeModel.Tag = DepItem.DtpID;
                    TreeModel.Header = DepItem.DtpName;
                    TbHead.Text = DepItem.Head;
                    HeadTele.Text = DepItem.HeadTele;
                }
                Logger.Log(results.Result.Message);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, results.Result.Message);

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器异常！");
            }

        }
        #endregion
        #region 删除选中项
        private void DelItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.TreeView.IsEnabled == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "新增操作中！");
                return;
            }

            API.Sys_DeptInfo Dep = new API.Sys_DeptInfo();
            RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
            if (TreeModel == null||TreeModel.Tag==null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未选中任何项！");
                return;
            }
            if (TreeModel.Items != null && TreeModel.Items.Count>0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "无法删除存在子部门的部门！");
                return;
            }
            try
            {
                Dep.DtpID = Convert.ToInt32(TreeModel.Tag);
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未选中任何项！");
                return;
            }
            Dep.DtpName = TbDeptName.Text.Trim();
            Dep.Note = TbNote.Text.Trim();
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定删除选中项目？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodIDStore.DelDeptInfo, new object[] { Dep }, DelFinish);
        }
        private void DelFinish(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue == true)
                {
                    API.Sys_DeptInfo DepItem = results.Result.Obj as API.Sys_DeptInfo;
                    if (DepItem == null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "返回数据失败！");
                        return;
                    }
                    var query = from b in DepInfo where b.DtpID == DepItem.DtpID select b;
                    if (query.Count()==0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "获取数据失败！");
                        return;
                    }
                    DepInfo.Add(query.First());
                    RadTreeViewItem TreeModel = TreeView.SelectedItem as RadTreeViewItem;
                    RadTreeViewItem parent = TreeModel.ParentItem;
                    try
                    {
                        TreeModel.ParentItem.Items.Remove(TreeModel);
                        AllReadOnly();
                        Clean();
                    }
                    catch
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "无法获取父节点！");
                        return;
                    }

                }
                Logger.Log(results.Result.Message);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, results.Result.Message);

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器异常！");
            }

        }
        #endregion
    }
}
