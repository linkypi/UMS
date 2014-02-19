using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using UserMS.Common;

namespace UserMS.Views.CMS.CardTypeWH
{
    public partial class AddVIPType : BasePage
    {
        public List<object> SelectedItems { get; set; }
        public List<object> tempItems { get; set; }
        private readonly List<TreeViewModel> _leftTree;
        List<API.VIP_VIPType> VIPtype;
        List<API.SeleterModel> ServiceModel;

        List<API.SeleterModel> Service;
        List<TreeViewModel> LeftTree;
        private ProductionFilter adder;
        int ID;

        string r;

        int AddMehtod = 79;

        public AddVIPType()
        {
            InitializeComponent();
            Service = new List<API.SeleterModel>();
            this.TbAddService.Click += TbAddService_Click;
            this.TbDeleteService.Click += TbDeleteService_Click;
            this.Sumbit.Click += Sumbit_Click;
            ServiceModel = new List<API.SeleterModel>();
            adder = new ProductionFilter(ref ServiceModel, ref DGservice);
            DGservice.ItemsSource = ServiceModel;

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
        #region 提交操作
        /// <summary>
        /// 提交新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Sumbit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否新增卡类别？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            API.VIP_VIPType VipType = new API.VIP_VIPType();
            VipType.ID = ID;
            VipType.Name = TbType.Text.Trim();
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
                PublicRequestHelp help = new PublicRequestHelp(this.busy, AddMehtod, new object[] { VipType }, AccomplishReturn);
            }
        }
        #endregion
        #region 提交完成
        void AccomplishReturn(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Error == null)
            {
                if (results.Result.ReturnValue == true)
                {
                    CanEmpty();                 
                }
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
                Logger.Log(results.Result.Message);
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
            }

        }
        #endregion
        #region 清空数据
        /// <summary>
        /// 清空数据
        /// </summary>
        void CanEmpty()
        {
            TbType.Text = string.Empty;
            TbSPoint.Text = string.Empty;
            TbValidity.Text = string.Empty;
            TbMoney.Text = string.Empty;
            Note.Text = string.Empty;
            ServiceModel.Clear();
            DGservice.Rebind();
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
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否删除服务？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
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
        #region 取消操作
        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否要清空所有数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                CanEmpty();
        }
        #endregion
    }
}
