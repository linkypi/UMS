using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UserMS.Views.ProSell.Price
{
    /// <summary>
    /// LowPriceManager.xaml 的交互逻辑
    /// </summary>
    public partial class LowPriceManager : Page
    {
        private List<TreeViewModel> treeModels = null;
        List<API.LowPriceModel> models = null;
        List<TreeViewModel> ParentTree = new List<TreeViewModel>();

        List<API.LowPriceModel> PricePro = new List<API.LowPriceModel>();
        private string menuid = "185";

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
        }
        public LowPriceManager()
        {
            InitializeComponent();

            models = new List<API.LowPriceModel>();
            GridProList.ItemsSource = models;
            treeModels = new List<TreeViewModel>();
            List<API.Pro_ProInfo> prods = new List<API.Pro_ProInfo>();

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 189, new object[] {  }, new EventHandler<API.MainCompletedEventArgs>(LoadCompleted));
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.LowPriceModel> list = e.Result.Obj as List<API.LowPriceModel>;
                models.Clear();
                models.AddRange(list);
                GridProList.Rebind();
            }
        }

        #region  保存数据

        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.LowPriceModel> updatemodels = new List<API.LowPriceModel>();

            foreach (var item in models)
            {
                API.LowPriceModel lp = new API.LowPriceModel();
                lp.ProID = item.ProID;
                lp.Children = new List<API.LPMChildren>();

                var invalidmodels = from m in item.Children
                         where  m.CurrentLowPrice<0
                         select m;
                if (invalidmodels.Count() > 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,item.ProName+" 结算价不能小于0");
                    return;
                }

                var um = from m in item.Children
                         where  m.LowPrice != m.CurrentLowPrice
                         select m;
                foreach (var m in um)
                {
                    lp.Children.Add(m);
                }
                if (lp.Children.Count != 0)
                {
                    updatemodels.Add(lp);
                }

            }
            if (updatemodels.Count == 0)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.busy,190,new object[]{updatemodels},new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 189, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(LoadCompleted));
     
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败");
            }
        }

        #endregion 

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridProList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (GridProList.SelectedItem == null)
            {
                return;
            }

            API.LowPriceModel model = GridProList.SelectedItem as API.LowPriceModel;
            GridDetailList.ItemsSource = model.Children;
            GridDetailList.Rebind();
        }
    }
}
