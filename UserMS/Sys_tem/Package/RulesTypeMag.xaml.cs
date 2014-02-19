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
    /// RulesMag.xaml 的交互逻辑
    /// </summary>
    public partial class RulesTypeMag : Page
    {
        public RulesTypeMag()
        {
            InitializeComponent();
            Search();
        }


        private void Search()
        {
            Clear();
            PublicRequestHelp prh = new PublicRequestHelp(this.busy,278,new object[]{},new EventHandler<API.MainCompletedEventArgs>(GetCompledted));
        }

        private void GetCompledted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                GridList.ItemsSource = e.Result.Obj as List<API.Rules_TypeInfo>;
                GridList.Rebind();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (GridList.SelectedItem == null)
            {
                return;
            }
            API.Rules_TypeInfo rt = GridList.SelectedItem as API.Rules_TypeInfo;

            rt.CanGetBack =(bool) cangetback.IsChecked;
            rt.ShowToCus = (bool) ShowToCus.IsChecked;

            if (string.IsNullOrEmpty(rname.Text))
            {
                MessageBox.Show("规则名称不能为空！");
                return;
            }

            rt.RulesName = rname.Text;
            rt.Note = this.note.Text;
            if (MessageBox.Show("确定修改规则吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 277, new object[] { rt },new EventHandler<API.MainCompletedEventArgs>(UpdateCompleted));
        }

        private void UpdateCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Search();
            }
        }

        private void GridList_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            if (GridList.SelectedItem == null)
            {
                return;
            }
            API.Rules_TypeInfo rt = GridList.SelectedItem as API.Rules_TypeInfo;
            this.rname.Text = rt.RulesName;
            this.ShowToCus.IsChecked = rt.ShowToCus;
            this.cangetback.IsChecked = rt.CanGetBack;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridList.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的规则！");
                return;
            }

            if (MessageBox.Show("确定删除规则吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            API.Rules_TypeInfo rt = GridList.SelectedItem as API.Rules_TypeInfo;
            PublicRequestHelp prh = new PublicRequestHelp(this.busy,280,new object[]{rt.ID},new EventHandler<API.MainCompletedEventArgs>(DeleteCompleted));
        }

        private void DeleteCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Search();
            }
        }


        private void Clear()
        {
            add_cangetback.IsChecked = false;
            add_rname.Text = string.Empty;
            add_ShowToCus.IsChecked = false;
            cangetback.IsChecked = false;
            rname.Text = string.Empty;
            ShowToCus.IsChecked = false;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            API.Rules_TypeInfo rt =new API.Rules_TypeInfo();

            rt.CanGetBack = (bool)add_cangetback.IsChecked;
            rt.ShowToCus = (bool)add_ShowToCus.IsChecked;
            rt.Note = add_note.Text;
            if (string.IsNullOrEmpty(add_rname.Text))
            {
                MessageBox.Show("规则名称不能为空！");
                return;
            }

            rt.RulesName = add_rname.Text;

            if (MessageBox.Show("确定添加规则吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 279, new object[] { rt }, new EventHandler<API.MainCompletedEventArgs>(UpdateCompleted));
        }
    }
}
