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
using UserMS.Model;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// RepairerProductInfo.xaml 的交互逻辑
    /// </summary>
    public partial class RepairerProductInfo : Page
    {
        public RepairerProductInfo()
        {
            InitializeComponent();
            GridRepList.ItemsSource = models;

            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID == 93 && p.HallID != null
            );
            UserOpList = userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
                          (list, info) => new { op = list, user = info })
             .Join(Store.UserOp, arg => arg.op.OpID, op => op.OpID, (a, t) => new UserOpModel()
             {
                 ID = a.op.ID,
                 HallID = a.op.HallID,
                 OpID = a.op.OpID,
                 UserID = a.op.UserID,
                 Username = a.user.RealName,
                 opname = t.Name
             }).ToList();
        }

        private List<UserOpModel> UserOpList = new List<UserOpModel>();
        List<API.RepairerProInfo> models = new List<API.RepairerProInfo>();

        #region 维修师操作 

        private void addRep_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.RepairerHallTree(),
                UserOpList, "HallID","Username", new string[] { "Username", "opname" },
               new string[] { "用户名", "职位" });
            w.Closed += SellerSearchWindowClose;
            w.ShowDialog();
        }

        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    UserOpModel model = (UserOpModel)window.SelectedItem;

                    API.RepairerProInfo rep = new API.RepairerProInfo();
                    rep.RepairerID = model.UserID;
                    API.Sys_UserInfo u = Store.UserInfos.Where(a => a.UserID == model.UserID).First();
                    if(u==null)
                    {
                        MessageBox.Show("未能找到指定维修师！");return ;
                    }
                    rep.Children = new List<API.TypeInfo>();
                    rep.Name = u.RealName;
                    rep.Dirty = true;
                    models.Add(rep);
                    GridRepList.Rebind();

                }
            }

        }

        private void deleteRep_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridRepList.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选需要删除的数据！"); return;
            }

            List<string> rids = GridRepList.SelectedItems.OfType<API.RepairerProInfo>().Select(a => a.RepairerID).ToList();
          
            if (MessageBox.Show("确定删除选中项吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp p = new PublicRequestHelp(this.busy, 387, new object[] { rids }, DelRepCompledted);
        }

        private void DelRepCompledted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                GridProDetail.ItemsSource = null;
                GridProDetail.Rebind();
            }
        }
        #endregion 

        #region 品牌

        private void deletePro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridRepList.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选需要删除的数据！"); return;
            }

            //if(MessageBox.Show("确定删除选中的商品吗？","",MessageBoxButton.OKCancel)==MessageBoxResult.Cancel)
            //{
            //    return;
            //}

            API.RepairerProInfo rep = GridRepList.SelectedItem as API.RepairerProInfo;
            foreach (var item in GridProDetail.SelectedItems)
            {
                rep.Children.Remove(item as API.TypeInfo);
            }
            rep.Dirty = true;
            GridProDetail.SelectedItem = rep;
            GridProDetail.Rebind();
            //API.RepairerProInfo rep = GridRepList.SelectedItem as API.RepairerProInfo;
            //List<string> ts = GridProDetail.SelectedItems.OfType<API.TypeInfo>().Select(t=>t.TypeID).ToList();

            //PublicRequestHelp p = new PublicRequestHelp(this.busy,387,new object[]{ 
            //    new List<string>(){rep.RepairerID}
            //    ,ts,false },DelCompledted);
        }

        private void DelCompledted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
               // Search();
            }
        }

        void ss_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter selecter = sender as SingleSelecter;

            if (selecter.DialogResult == true)
            {
                API.Pro_TypeInfo pro = selecter.SelectedItem as API.Pro_TypeInfo;

                foreach (var item in GridRepList.SelectedItems)
                {
                    var a = item as API.RepairerProInfo;
                    a.Dirty = true;
                    API.TypeInfo t = new API.TypeInfo();
                    t.TypeID = pro.TypeID.ToString();
                    t.TypeName = pro.TypeName;
                    a.Children.Add(t);
                }
                GridRepList.Rebind();
                GridProDetail.ItemsSource = (GridRepList.SelectedItem as API.RepairerProInfo).Children;
                GridProDetail.Rebind();
            }
        }

        private void add_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridRepList.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择维修师！"); return;
            }
            SingleSelecter msFrm = new SingleSelecter(
             null,
             Store.ProTypeInfo, "TypeID", "TypeName",
             new string[] { "TypeID", "TypeName" },
             new string[] { "商品品牌", "商品名称" });
            msFrm.Closed += ss_Closed;
            msFrm.ShowDialog();
        }

        #endregion 

        private void save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (models.Where(a => a.Dirty == true).Count() == 0)
            {
                MessageBox.Show("找不到可更新的数据！");
            }

            if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            List<API.RepairerProInfo> list = models.Where(a => a.Dirty == true).ToList();
            PublicRequestHelp p = new PublicRequestHelp(this.busy,389,new object[]{
            list},SaveCompleted);
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
 
            }
        }

        private void GridRepList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (GridRepList.SelectedItems.Count == 0)
            { return; }
            GridProDetail.ItemsSource = (GridRepList.SelectedItem as API.RepairerProInfo).Children;
            GridProDetail.Rebind();
        }
    }
}
