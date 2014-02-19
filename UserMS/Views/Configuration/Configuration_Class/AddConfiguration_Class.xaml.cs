using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.Configuration.Configuration_Class
{
    public partial class AddConfiguration_Class : BasePage
    {
        public int MethodID = 124;
        public int AddMethodID = 125;
        public API.ProClassModel CurrentClassModel;

        private List<API.Role_Pro_Property> roleList = new List<API.Role_Pro_Property>();
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<API.Role_Pro_Property> RoleList
        {
            get { return roleList; }
            set { roleList = value; }
        }

        public AddConfiguration_Class()
        {
            InitializeComponent();

            InitGrid2();

            InitDeptTree();

            this.UserGrid.ItemsSource = Store.UserInfos;
        }
        #region 生成部门树
        
       
        private void InitDeptTree()
        {
            var x = from b in Store.DeptInfo
                    where b.Parent == null || b.Parent == 0
                    select b;
            foreach (var m in x)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                item.Header = m.DtpName;
                item.DataContext = m;
                item.IsExpanded = true;
                InitDeptTree(item, m.DtpID);
                this.DeptTree.Items.Add(item);

            }
        }

        private void InitDeptTree(RadTreeViewItem item,int parent)
        {
            var x = from b in Store.DeptInfo
                    where b.Parent == parent
                    select b;
            foreach (var m in x)
            {
                RadTreeViewItem childitem = new RadTreeViewItem();
                childitem.Header = m.DtpName;
                childitem.DataContext = m;
                InitDeptTree(childitem, m.DtpID);
                item.Items.Add(childitem);

            }
        }


        private void DeptTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DeptTree.SelectedItem == null)
            {
                this.UserGrid.ItemsSource = Store.UserInfos;
            }
            else
            {
                List<int?> DeptID = new List<int?>();
                API.Sys_DeptInfo Dept = (API.Sys_DeptInfo)(((RadTreeViewItem)this.DeptTree.SelectedItem).DataContext);
                DeptID.Add(Dept.DtpID);
                foreach(RadTreeViewItem item in ((RadTreeViewItem)this.DeptTree.SelectedItem).Items)
                {
                    DeptID.Add(((API.Sys_DeptInfo)(item.DataContext)).DtpID);
                    DeptID.AddRange(GetChildDeptID(item));
                }
                var x = from b in Store.UserInfos
                        where DeptID.Contains(b.DtpID)
                        select b;
                this.UserGrid.ItemsSource = x;
            }
        }

        private List<int?> GetChildDeptID(RadTreeViewItem item)
        {
            List<int?> DeptID = new List<int?>();
            foreach (RadTreeViewItem m in item.Items)
            {
                DeptID.Add(((API.Sys_DeptInfo)(m.DataContext)).DtpID);

            }
            return DeptID;
        }
        #endregion
        
        #region 新增数据
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TbSubit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            List<API.ProClassModel> ClassList = dataGrid1.ItemsSource as List<API.ProClassModel>;
            if ( ClassList == null || ClassList.Count() == 0) //if (RoleList.Count() == 0 || ClassList == null || ClassList.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加任何项！");
                return;
            }
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodIDStore.AddClassMethodID, new object[] { RoleList, ClassList }, Completed);
        }
        private void Completed(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.ReturnValue == true)
                {
                    Cancel();
                    List<API.Pro_ClassInfo> classs = (List<API.Pro_ClassInfo>)re.Result.Obj;
                    Store.ProClassInfo.AddRange(classs);
                    
                    //this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
                }
                if (re.Result.Message != null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
                    Logger.Log(re.Result.Message);
                }
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
        }
        #endregion

        #region 生成列
       
        private void InitGrid2()
        {
            
            this.LikeClass.ItemsSource = Store.ProClassInfo;
            this.LikeClass.DisplayMemberPath = "ClassName";
            this.LikeClass.SelectedValuePath = "ClassID";
        }
        #endregion
       
        #region 清空数据
        void Cancel()
        {
             
            RoleList.Clear();
            this.dataGrid1.ItemsSource = null;
            
            this.IsSalary.Text = string.Empty;
            ClassName.Text = string.Empty;
            ClassOrder.Text = string.Empty;
            ClassNote.Text = string.Empty;
            this.DeptTree.SelectedItems.Clear();
        }
        #endregion

        private void Cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"清空数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            Cancel();
        }
       
     

        #region 添加商品类型到gridView
        private void AddClass_Click(object sender, RoutedEventArgs e)
        {
            List<API.ProClassModel> ClassList = dataGrid1.ItemsSource as List<API.ProClassModel>;
            ClassList = ClassList == null ? new List<API.ProClassModel>() : ClassList;
            if (string.IsNullOrEmpty(this.ClassName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加类型名称！");
                return;
            }
            if (string.IsNullOrEmpty(this.IsSalary.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择仓管提成+1！");
                return;
            }
            if (this.LikeClass.SelectedValue==null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择同等权限的类别！");
                return;
            }
            int i = 0;
            if (!string.IsNullOrEmpty(this.ClassOrder.Text))
            {
                try
                {
                    i = int.Parse(this.ClassOrder.Text.Trim());
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"序号必须为正整数！");
                    return;
                }
            }
            API.ProClassModel ProClass = new API.ProClassModel() { 
                ClassName = this.ClassName.Text.Trim(), 
                Note = this.ClassNote.Text.Trim(), 
                Order = i, 
                IsSalary = this.IsSalary.Text.Trim(), 
                HasSalary = this.IsSalary.Text=="是"?true:false,
                LikeClass=Convert.ToInt32(this.LikeClass.SelectedValue),
                LikeClassName=this.LikeClass.Text,
                UserIDS=new List<string>()
            };
            int query = (from b in ClassList
                         where b.ClassName == ProClass.ClassName
                         select b).Count();
            if (query > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"存在商品类别，不能重复添加！");
                return;
            }
            ClassList.Add(ProClass);
            dataGrid1.ItemsSource = ClassList;
            dataGrid1.Rebind();
        }
        #endregion 

      

        #region 删除商品类型
        private void DelClass_Click(object sender, RoutedEventArgs e)
        {
            List<API.ProClassModel> ClassList = dataGrid1.ItemsSource as List<API.ProClassModel>;
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除商品类型？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            foreach (var Item in dataGrid1.SelectedItems)
            {
                API.ProClassModel model = Item as API.ProClassModel;
                ClassList.Remove(model);

                foreach (var Role in RoleList)
                {
                    var query = (from b in Role.ProClassModel
                                where b.ClassName == model.ClassName
                                select b).ToList();
                    if (query.Count() == 1)
                    {
                        Role.ProClassModel.Remove(query.First());
                    }
                }
            }     
            dataGrid1.Rebind();
             
        }
        #endregion 

        private void UserGrid_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (this.dataGrid1.SelectedItem == null) return;
            API.Sys_UserInfo user = (API.Sys_UserInfo)e.DataElement;
            e.Row.IsSelected = (this.CurrentClassModel.UserIDS.Contains(user.UserID));
        }
        /// <summary>
        /// 选中添加的类别时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid1_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            
                CurrentClassModel = (API.ProClassModel)this.dataGrid1.SelectedItem;
                this.DeptTree.SelectedItems.Clear();
             
        }
        /// <summary>
        /// 选中或者反选中用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserGrid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (this.dataGrid1.SelectedItem == null) return;
            foreach (var x in e.AddedItems)
            {
                string userid = ((API.Sys_UserInfo)x).UserID;
                if(!CurrentClassModel.UserIDS.Contains(userid))     
                    CurrentClassModel.UserIDS.Add(userid);
            }
            foreach (var x in e.RemovedItems)
            {
                CurrentClassModel.UserIDS.RemoveAll(p=>p==((API.Sys_UserInfo)x).UserID);
            }
        }

       
 
        
    }
}
