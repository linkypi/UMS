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

namespace UserMS.Sys_tem
{
    /// <summary>
    /// RemindMag.xaml 的交互逻辑
    /// </summary>
    public partial class RemindMag : Page
    {
        private List<API.View_RemindList> models = new List<API.View_RemindList>();
        private int pageindex = 0;
        private bool flag = false;
        public RemindMag()
        {
            InitializeComponent();
            this.SizeChanged += ProNameMag_SizeChanged;
            flag = true;
        }

        void ProNameMag_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;
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
                menuid = "245";
            }
            page.PageIndex = 1;
            pagesize.Value = 30;
            GridList.ItemsSource = models;

            List<SlModel.CkbModel> ckblist = new List<SlModel.CkbModel>();
            ckblist.Add(new SlModel.CkbModel(true, "是"));
            ckblist.Add(new SlModel.CkbModel(false, "否"));
            addIsInTime.ItemsSource = ckblist;
            this.addIsInTime.SelectedIndex = 0;
            updIsInTime.ItemsSource = ckblist;

           // this.addFlag.ItemsSource = ckblist;
            //addFlag.SelectedIndex = 1;
            updFlag.ItemsSource = ckblist;
            
            GetList();
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
                models.Clear();
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_RemindList> aduitList = pageParam.Obj as List<API.View_RemindList>;
                if (aduitList.Count != 0)
                {
                    models.AddRange(aduitList);
                    GridList.Rebind();

                    this.page.PageSize = (int)pagesize.Value;
                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);

                    this.page.PageIndexChanged -= page_PageIndexChanged;
                    this.page.Source = pcv;
                    this.page.PageIndexChanged += page_PageIndexChanged;
                    this.page.PageIndex = pageindex;
                }
            }
        }

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Upd_Click(object sender, RoutedEventArgs e)
        {
            if (GridList.SelectedItem == null)
            {
                MessageBox.Show("请选择要更新的商品型号！");
                return;
            }
            API.View_RemindList pac = (API.View_RemindList)GridList.SelectedItem;


            if (string.IsNullOrEmpty(updProcName.Text))
            {
                MessageBox.Show("存储过程不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(updMenuID.Text))
            {
                MessageBox.Show("菜单编号不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(updName.Text))
            {
                MessageBox.Show("名称不能为空！");
                return;
            }
            SlModel.CkbModel ckb = updIsInTime.SelectedItem as SlModel.CkbModel;

            SlModel.CkbModel ckbFlag = updFlag.SelectedItem as SlModel.CkbModel;
            if (updProcName.Text != pac.OldProcName || pac.OldNote !=updNote.Text ||
                updName.Text != pac.OldName || updMenuID.Text != pac.OldMenuID.ToString() || ckb.Text != pac.OldIsInTime
                || pac.OldOrder != (decimal)updOrder.Value || ckbFlag.Text !=pac.OldFlag)
            { 
                if (MessageBox.Show("确定修改吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }
                API.View_RemindList updmodel = new API.View_RemindList();
                updmodel.Note = updNote.Text;
                updmodel.Name = updName.Text;
                try
                {
                    updmodel.MenuID = Convert.ToInt32(updMenuID.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("菜单编号有误！");
                    return;
                }
                updmodel.Flag = ckbFlag.Text;
                updmodel.IsInTime =ckb.Text;

                updmodel.Order = (decimal)updOrder.Value;
                updmodel.ProcName = updProcName.Text;
                updmodel.ID = pac.ID;

                PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 257, new object[] { updmodel }, new EventHandler<API.MainCompletedEventArgs>(UpdateCompleted));
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
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            MessageBox.Show(e.Result.Message);
          

            if (e.Result.ReturnValue)
            {
                Clear();
                GetList();
            }
           
        }

        #endregion 

        #region  添加

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(addName.Text))
            {
                MessageBox.Show("名称不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(addProcName.Text))
            {
                MessageBox.Show("存储过程名称不能为空！");
                return;
            } 
            if (string.IsNullOrEmpty(addMenuID.Text))
            {
                MessageBox.Show("菜单编号不能为空！");
                return;
            }

            if (MessageBox.Show("确定添加吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }


            API.Sys_RemindList model = new API.Sys_RemindList();
            model.Note = addNote.Text;
            model.Name = addName.Text;
            model.ProcName = addProcName.Text;

            try
            {
                model.MenuID = Convert.ToInt32(addMenuID.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("菜单编号有误！");
                return;
            }
           // SlModel.CkbModel ckbFlag = addFlag.SelectedItem as SlModel.CkbModel;
            model.Flag = true;
           
            SlModel.CkbModel ckb = addIsInTime.SelectedItem as SlModel.CkbModel;
            model.IsInTime = ckb.Flag;
            model.Order = (decimal)addOrder.Value;
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 255, new object[] { model }, new EventHandler<API.MainCompletedEventArgs>(AddCompleted));

        }

        private void AddCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Clear();
                GetList();
            }
        }

        #endregion 


        /// <summary>
        /// 选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GridList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridList.SelectedItem == null)
            {
                return;
            }

            API.View_RemindList item = GridList.SelectedItem as API.View_RemindList;

            this.updProcName.Text = item.ProcName;
            this.updNote.Text = item.Note;
            this.updName.Text = item.Name;
            this.updOrder.Value = (double)item.Order;
            this.updMenuID.Text = item.MenuID.ToString();

            if(item.IsInTime=="是")
            {
                 this.updIsInTime.SelectedIndex=0;
            }
            else
            {
                 this.updIsInTime.SelectedIndex=1;
            }
            if (item.Flag == "已禁用")
            {
                this.updFlag.SelectedIndex = 1;
            }
            else
            {
                this.updFlag.SelectedIndex = 0;
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
            if (GridList.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的项！");
                return;
            }
            if (MessageBox.Show("确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            API.View_RemindList item = GridList.SelectedItem as API.View_RemindList;
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 256, new object[] { item }, new EventHandler<API.MainCompletedEventArgs>(DelCompleted));

        }

        private void DelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Clear();
                GetList();
            }
        }

        #endregion 

        private void Clear()
        {
            this.updProcName.Text = string.Empty;
            this.updNote.Text = string.Empty;
            this.addProcName.Text = string.Empty;
            this.addNote.Text = string.Empty;
            this.updName.Text = string.Empty;
            this.addName.Text = string.Empty;
            this.addMenuID.Text = string.Empty;
            this.updMenuID.Text = string.Empty;
            this.addOrder.Value = 1;
            this.addIsInTime.SelectedIndex = 0;
            this.updOrder.Value = 1;
            this.updIsInTime.SelectedIndex = -1;
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            GetList();
        }

        private void GetList()
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = page.PageIndex;
            rpp.PageSize = (int)pagesize.Value;
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 254, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

        }

        void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            GetList();
        }
    }
}
