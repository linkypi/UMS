using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.Configuration.Configuration_Hall
{
    /// <summary>
    /// Add_Hall.xaml 的交互逻辑
    /// </summary>
    public partial class Add_Hall : BasePage
    {

        private List<API.Role_Hall> roleList = new List<API.Role_Hall>();
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<API.Role_Hall> RoleList
        {
            get { return roleList; }
            set { roleList = value; }
        }

        public Add_Hall()
        {
            InitializeComponent();
            AreaSource();
            CreatColumn1();
            GetRoleSourece();
        }
        #region 初始化数据
        protected void AreaSource()
        {
            AreaName.ItemsSource = Store.AreaInfo;
            AreaName.DisplayMemberPath = "AreaName";
            AreaName.SelectedValuePath = "AreaID";

            this.LevelName.ItemsSource = Store.Level;
            LevelName.DisplayMemberPath = "LevelName";
            LevelName.SelectedValuePath = "LevelID";
          
        }
        #region 角色资源
        void GetRoleSourece()
        {
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodIDStore.GetRole, new object[] { }, GetRole_Completed);

        }
        protected void GetRole_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    List<API.Sys_RoleInfo> RoleList = e.Result.Obj as List<API.Sys_RoleInfo>;
                    List<SlModel.RoleModel> RoleModel = new List<SlModel.RoleModel>();
                    foreach (var Item in RoleList)
                    {
                        SlModel.RoleModel model = new SlModel.RoleModel()
                        {
                            RoleID = Item.RoleID,
                            RoleName = Item.RoleName
                        };
                        RoleModel.Add(model);
                        RadTreeViewItem item = new RadTreeViewItem();
                        item.Header = model.RoleName;
                        item.Tag = model.RoleID;
                        RoleTV.Items.Add(item);
                    }
                }

            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion
        #endregion


        #region 生成列
       
        private void CreatColumn1()
        {
            GridViewDataColumn col = new GridViewDataColumn();
            col.DataMemberBinding = new System.Windows.Data.Binding("AreaName");
            col.Header = "区域名称";
            this.dataGrid2.Columns.Add(col);

            GridViewDataColumn col2 = new GridViewDataColumn();
            col2.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            col2.Header = "仓库名称(中)";    
            this.dataGrid2.Columns.Add(col2);

            GridViewDataColumn col3 = new GridViewDataColumn();
            col3.DataMemberBinding = new System.Windows.Data.Binding("ShortName");
            col3.Header = "仓库名称(英)";    
            this.dataGrid2.Columns.Add(col3);
        }
        #endregion
        #region 添加事件
        private void AddHall_Click(object sender, RoutedEventArgs e)
        {
            List<API.View_HallInfo> HallList = dataGrid1.ItemsSource as List<API.View_HallInfo>;
            HallList = HallList == null ? new List<API.View_HallInfo>() : HallList;
            API.View_HallInfo Hall = new API.View_HallInfo();
            if (string.IsNullOrEmpty(this.AreaName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择区域！");
                return;
            }
            if (string.IsNullOrEmpty(this.LevelName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择等级！");
                return;
            }
            if (string.IsNullOrEmpty(HallName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入门店中文名称！");
                return;
            }
            if (string.IsNullOrEmpty(EHallName.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入门店的英文名称！");
                return;
            }
            if (string.IsNullOrEmpty(CanIn.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择可否入库！");
                return;
            }
            if (string.IsNullOrEmpty(CanBack.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择可否退库！");
                return;
            }
            if (!string.IsNullOrEmpty(this.Order.Text))
            {
                try
                {
                     Hall.Order = int.Parse(this.Order.Text.Trim());
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"序号必须为正整数！");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(this.Longitude.Text))
            {
                try
                {
                    Hall.Longitude = decimal.Parse(this.Longitude.Text);
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入有效的经度！");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(this.Latitude.Text))
            {
                try
                {
                    Hall.Latitude = decimal.Parse(this.Latitude.Text);
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入有效的纬度！");
                    return;
                }
            }
           Hall.HallName = this.HallName.Text.Trim();
           Hall.AreaName = this.AreaName.Text.Trim();
           Hall.AreaID=(int)this.AreaName.SelectedValue;
           Hall.LevelName = this.LevelName.Text.Trim();
           Hall.LevelID = (int)this.LevelName.SelectedValue;
           Hall.ShortName=this.EHallName.Text.Trim();
           Hall.DisPlayName=this.ShowHallName.Text.Trim();
           Hall.PrintName = this.PrintName.Text.Trim();
           Hall.IsCanIn=this.CanIn.Text.Trim();
           Hall.IsCanIn = this.CanIn.Text.Trim();
           Hall.CanIn=this.CanIn.Text=="是"?true:false;
           Hall.IsCanback = this.CanBack.Text.Trim();
           Hall.IsCanback=this.CanBack.Text.Trim();
           Hall.CanBack=this.CanBack.Text=="是"?true:false;
           
           Hall.Note = this.Note.Text.Trim();
 
            int query = (from b in HallList
                         where b.HallName == Hall.HallName
                         select b).Count();
            if (query > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"存在门店名称，不能重复添加！");
                return;
            }
            HallList.Add(Hall);
            dataGrid1.ItemsSource = HallList;
            dataGrid1.Rebind();
            dataGrid2.Rebind();
        }
        #endregion 


        #region 事件操作
        private void RoleTV_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
           
        }
        private void RoleTV_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            var query = (from b in RoleList
                         where b.RoleID == (int)item.Tag
                         select b).ToList();
            if (query.Count() > 0)
                RoleList.Remove(query.First());

            RoleTV.SelectedItems.Clear();
            RoleTV.SelectedItem = item.Header.ToString();

        }
        private void RoleTV_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (RoleTV.SelectedItem == null)
                return;
            this.dataGrid2.ItemsSource = null;
            var query = (from b in RoleList
                         where b.RoleName == RoleTV.SelectedItem.ToString()
                         select b).ToList();

            if (query.Count() == 1)
            {
                this.dataGrid2.ItemsSource = query.First().HallInfo;
            }
            this.dataGrid2.Rebind();
        }
        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<API.ProClassModel> ClassList = dataGrid2.ItemsSource as List<API.ProClassModel>;
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除该角色的商品类型？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            foreach (var Item in dataGrid2.SelectedItems)
            {
                ClassList.Remove(Item as API.ProClassModel);
            }
            dataGrid2.Rebind();
        }
        #region 全选 反选
        private void AllCheck_Click(object sender, RoutedEventArgs e)
        {
            foreach (RadTreeViewItem item in this.RoleTV.Items)
            {
                item.CheckState = System.Windows.Automation.ToggleState.On;
            }
        }

        private void CleanCheck_Click(object sender, RoutedEventArgs e)
        {
            foreach (RadTreeViewItem item in this.RoleTV.Items)
            {
                item.CheckState = System.Windows.Automation.ToggleState.Off;
            }
        }
        #endregion
        #region
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSubit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.View_HallInfo> HallList = dataGrid1.ItemsSource as List<API.View_HallInfo>;
            if ( HallList == null || HallList.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加任何项！");
                return;
            }
            //if (RoleList.Count() == 0)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加任何项！");
            //    return;
            //}
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodIDStore.AddHall, new object[] { RoleList,HallList }, Completed);
        }
        private void Completed(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                if (re.Result.ReturnValue == true)
                {
                   Cancel();
                   // this.InitPageEntity(MethodID, this.dataGrid1, this.busy, this.RadPager, pageParam);
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
        #region 清空数据
        void Cancel()
        {
            this.RoleTV.Items.Clear();
          
            RoleList.Clear();
            PrintName.Text = string.Empty;
            LevelName.Text = string.Empty;
            this.dataGrid1.ItemsSource = null;
            dataGrid1.Rebind();
            this.dataGrid2.ItemsSource = null;
            this.dataGrid2.Rebind();
            this.AreaName.Text = string.Empty;
            this.HallName.Text = string.Empty;
            EHallName.Text = string.Empty;
            ShowHallName.Text = string.Empty;
            CanIn.Text = string.Empty;
            CanBack.Text = string.Empty;
            Longitude.Text = string.Empty;
            Latitude.Text = string.Empty;
            Order.Text = string.Empty;
            Note.Text = string.Empty;
            GetRoleSourece();
        }
        #endregion
        #region 仓库应用选中角色
        private void TbAdd_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RoleList.Clear();
            foreach (var Item in RoleTV.CheckedItems)
            {
                RadTreeViewItem item = Item as RadTreeViewItem;
                List<API.View_HallInfo> HallList = dataGrid1.ItemsSource as List<API.View_HallInfo>;
                if (HallList == null || HallList.Count() == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加仓库！");
                    item.CheckState = System.Windows.Automation.ToggleState.Off;
                    return;
                }

                API.Role_Hall Role = new API.Role_Hall();
                Role.RoleID = (int)item.Tag;
                Role.RoleName = item.Header.ToString();


                Role.HallInfo = Role.HallInfo == null ? new List<API.View_HallInfo>() : Role.HallInfo;
                Role.HallInfo.AddRange(HallList);
                RoleList.Add(Role);


                RoleTV.SelectedItems.Clear();
                RoleTV.SelectedItem = item.Header.ToString();
            }
            MessageBox.Show(System.Windows.Application.Current.MainWindow,"已添加！");
        }
        #endregion 

        #region 删除仓库
        private void DelHall_Click(object sender, RoutedEventArgs e)
        {
            List<API.View_HallInfo> ClassList = dataGrid1.ItemsSource as List<API.View_HallInfo>;
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除仓库？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            foreach (var Item in dataGrid1.SelectedItems)
            {
                API.View_HallInfo model = Item as API.View_HallInfo;
                ClassList.Remove(model);

                foreach (var Role in RoleList)
                {
                    var query = (from b in Role.HallInfo
                                 where b.HallName == model.HallName
                                 select b).ToList();
                    if (query.Count() == 1)
                    {
                        Role.HallInfo.Remove(query.First());
                    }
                }
            }
            dataGrid1.Rebind();
            this.dataGrid2.Rebind();
        }
        #endregion 

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<API.View_HallInfo> ClassList = dataGrid2.ItemsSource as List<API.View_HallInfo>;
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除该角色的仓库？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            foreach (var Item in dataGrid2.SelectedItems)
            {
                ClassList.Remove(Item as API.View_HallInfo);
            }
            dataGrid2.Rebind();
        }
    }
}
