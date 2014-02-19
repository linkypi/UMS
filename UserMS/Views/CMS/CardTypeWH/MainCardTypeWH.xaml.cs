using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using UserMS.Common;

namespace UserMS.Views.CMS.CardTypeWH
{
    public partial class MainCardTypeWH : BasePage
    {
        List<API.VIP_VIPType> VIPtype;
        List<API.SeleterModel> ServiceModel=new List<API.SeleterModel>();
        List<API.SeleterModel> Service;
        List<TreeViewModel> LeftTree;
        private ProductionFilter adder;
    
        bool IsUpdated = false;
        string r;
        const int MethodList = 78;
        const int MethodHead = 77;
        const int DeleteMethod = 100;
        const int UpdateMethod = 80;
        public MainCardTypeWH()
        {
            InitializeComponent();
            Service = new List<API.SeleterModel>();


            GetVIPType();//获取卡类型信息
            this.DGservice.ItemsSource = ServiceModel;
            this.TbAddService.Click += TbAddService_Click;
            this.TbDeleteService.Click += TbDeleteService_Click;
            this.Sumbit.Click += Sumbit_Click;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获取菜单ID失败！");
            }

        }
        #region 重置当前数据
        void ReSetAll()
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否重置单前数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                TreeViewModel s = TreeView.SelectedItem as TreeViewModel;
                if (s != null)
                {
                    if (IsUpdated == false)
                        GetSource();
                    else
                        AfterUpdate();
                }
            }
        }
        #endregion
        #region 提交修改
        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Sumbit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
     

            if (TreeView.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择卡类型！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否更改卡类型资料？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            TreeViewModel TreeModel = TreeView.SelectedItem as TreeViewModel;

            API.VIP_VIPType VipType = new API.VIP_VIPType();
            VipType.ID = int.Parse(TreeModel.ID);
            //卡类别名称发生改变时添加
            if (TbType.Text.Trim() != TreeModel.Title)
            {
                VipType.Name = TbType.Text.Trim();
            }
            VipType.SPoint = Decimal.Parse(TbSPoint.Text.Trim());
            VipType.Validity = int.Parse(TbValidity.Text.Trim());
            VipType.Cost_production = Decimal.Parse(TbMoney.Text.Trim());
            VipType.Note = this.Note.Text;

            VipType.VIP_VIPTypeService = new List<API.VIP_VIPTypeService>();
            if (ServiceModel != null)
            {
                foreach (var item in ServiceModel)
                {
                    API.VIP_VIPTypeService ser = new API.VIP_VIPTypeService();
                    ser.ProID = item.ProID;
                    ser.SCount = item.ProCount;
                    VipType.VIP_VIPTypeService.Add(ser);
                }             
            }
            PublicRequestHelp help = new PublicRequestHelp(this.busy, UpdateMethod, new object[] { VipType }, AccomplishReturn);
        }
        #endregion
        #region 修改完成
        /// <summary>
        /// 修改完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="results"></param>
        void AccomplishReturn(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue == true)
                {

                    TreeViewModel model = TreeView.SelectedItem as TreeViewModel;
                    model.Title = TbType.Text.Trim();
                    IsUpdated = true;

                }
                Logger.Log(results.Result.Message);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
            }

        }
        #endregion
        #region 服务操作
        /// <summary>
        /// 删除服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TbDeleteService_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否要删除服务？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (DGservice.SelectedItems == null)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择商品！");
                    return;
                }
                foreach (var Item in DGservice.SelectedItems)
                {
                    ServiceModel.Remove(Item as API.SeleterModel);
                }
                DGservice.Rebind();
            }
        }
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TbAddService_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            adder = new ProductionFilter(ref ServiceModel, ref DGservice);
            //List<API.Pro_ProInfo> pro = adder.GetPro(int.Parse(r.Trim()));
            //if (pro == null)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"该角色无此权限，请联系管理员 ");
            //    return;
            //}
            List<API.Pro_ProInfo> query = (from b in Store.ProInfo
                                           where b.IsService == true
                                           select b).ToList();
            adder.ProFilter(query, false);
        }
        #endregion
        #region  获取卡类型信息
        /// <summary>
        /// 获取卡类型
        /// </summary>
        void GetVIPType()
        {
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodHead, new object[] { }, SearchCompleted);
        }

        void SearchCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.Obj != null)
                {
                    VIPtype = new List<API.VIP_VIPType>();
                    VIPtype = results.Result.Obj as List<API.VIP_VIPType>;
                    LeftTree = new List<TreeViewModel>();
                    LeftTree = CommonHelper.VIPTypeTreeViewModel(VIPtype);
                    this.TreeView.ItemsSource = LeftTree;
                }
            }
            else
                Logger.Log("无数据");
        }
        #endregion
        #region 获取卡服务信息
        void GetVIPTypeService()
        {

            if (TreeView.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择卡类型！");
                return;
            }
            TreeViewModel TreeModel = TreeView.SelectedItem as TreeViewModel;
            int TypeID = int.Parse(TreeModel.ID);
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodList, new object[] { TypeID }, SearchSumbit);
        }
        #endregion
        #region 清空数据
        void CanEmpty()
        {
            TbType.Text = string.Empty;
            TbSPoint.Text = string.Empty;
            TbValidity.Text = string.Empty;
            TbMoney.Text = string.Empty;
            Note.Text = string.Empty;
            if (ServiceModel != null)
                ServiceModel.Clear();
            DGservice.Rebind();
        }
        #endregion
        #region 获取当前服务列表
        void SearchSumbit(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.Obj != null)
                {
                    List<API.View_VIPTypeService> VIPtypeService = results.Result.Obj as List<API.View_VIPTypeService>;
                    ServiceModel = new List<API.SeleterModel>();

                    foreach (var item in VIPtypeService)
                    {
                        API.SeleterModel model = new API.SeleterModel();
                        model.ProClassName = item.ClassName;
                        model.ProTypeName = item.TypeName;
                        model.ProName = item.ProName;
                        model.ProCount = (int)item.SCount;
                        model.ProID = item.ProID;
                        model.TypeID = item.ServiceID;
                        ServiceModel.Add(model);
                    }
                    this.DGservice.ItemsSource = ServiceModel;
                    this.DGservice.Rebind();
                }
            }
            else
                Logger.Log("无数据");
        }
        #endregion
        #region 显示所有
        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="LeftTree">左边树List, null不显示左树</param>
        /// <param name="Items">右边可选项</param>
        /// <param name="ForeignKey">Item与左树的关联外键</param>
        /// <param name="FilterKey">筛选框筛选的键</param>
        /// <param name="GridColumn">GridView的列</param>
        /// <param name="GridTitle">GridView的标题</param>
        //public void GetSource(List<TreeViewModel> LeftTree, IEnumerable Items, string ForeignKey, string FilterKey, string[] GridColumn, string[] GridTitle)
        public void GetSource()
        {

            if (TreeView.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择卡类型！");
                return;
            }
            TreeViewModel TreeModel = TreeView.SelectedItem as TreeViewModel;        
                //根据选择行获取卡类别        
                var query = from b in VIPtype
                            where b.ID == int.Parse(TreeModel.ID)
                            select b;
                if (query == null || query.Count() == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"加载失败,请联系管理员");
                    return;
                }

                CanEmpty();
                API.VIP_VIPType type = query.First();
                if (type.Name != null)
                    this.TbType.Text = type.Name;
                this.TbValidity.Text = type.Validity.ToString();
                if (type.Validity != null)
                    this.TbSPoint.Text = type.SPoint.ToString();
                if (type.Cost_production != null)
                    this.TbMoney.Text = type.Cost_production.ToString();
                if (type.Note != null)
                    this.Note.Text = type.Note;
            
            GetVIPTypeService();
        }
        #endregion
        #region 选择发生改变时
        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            TreeViewModel s = ((Telerik.Windows.Controls.RadTreeView)sender).SelectedItem as TreeViewModel;
            if (s != null)
            {
                if (IsUpdated == false)
                    GetSource();
                else
                    AfterUpdate();
            }
        }
        #endregion
        #region 数据更新后
        void AfterUpdate()
        {
            PublicRequestHelp help = new PublicRequestHelp(this.busy, MethodHead, new object[] { }, UpdateCompleted);
        }
        void UpdateCompleted(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue == true)
                {
                    if (results.Result.Obj != null)
                    {
                        VIPtype = new List<API.VIP_VIPType>();
                        VIPtype = results.Result.Obj as List<API.VIP_VIPType>;
                        if (VIPtype.Count() == 0)
                        {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow,"更新后加载失败，请联系管理员！");
                            return;
                        }
                        IsUpdated = false;
                    }
                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
            }
            else
                Logger.Log("无数据");
        }
        #endregion
        #region 重置所有
        private void BtReSet_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ReSetAll();
        }
        #endregion
        #region 删除卡类型
        private void TbDelete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否删除卡类型？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            if (TreeView.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择卡类型！");
                return;
            }
            TreeViewModel TreeModel = TreeView.SelectedItem as TreeViewModel;
            API.VIP_VIPType VipType = new API.VIP_VIPType();
            VipType.ID = int.Parse(TreeModel.ID);
            PublicRequestHelp help = new PublicRequestHelp(this.busy, DeleteMethod, new object[] { VipType }, AfterDelete);
        }
        void AfterDelete(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue == true)
                {
                    CanEmpty();
                    GetVIPType();
                }
                Logger.Log(results.Result.Message);
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);

            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
            }
        }
        #endregion
    }
}
