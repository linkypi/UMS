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

namespace UserMS.Sys_tem.Pro
{
    /// <summary>
    /// ProNameMag.xaml 的交互逻辑
    /// </summary>
    public partial class ProNameMag : Page
    {
        List<API.View_ProNameInfo> models = new List<API.View_ProNameInfo>();
        private int pageindex = 0;
        private bool flag = false;

        public ProNameMag()
        {
            InitializeComponent();
            //this.SizeChanged+=ProNameMag_SizeChanged;
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
                menuid = "244";
            }
            //page.PageIndex = 1;
            //pagesize.Value = 30;
            //Store.ProNameInfo
            //GridList.ItemsSource = models;

            //GetList();
            GridList.ItemsSource = Store.ProNameInfo;
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

                List<API.View_ProNameInfo> aduitList = pageParam.Obj as List<API.View_ProNameInfo>;
                if (aduitList.Count != 0)
                {
                    models.AddRange(aduitList);
                    GridList.Rebind();

                    //this.page.PageSize = (int)pagesize.Value;
                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);

                    //this.page.PageIndexChanged -= page_PageIndexChanged;
                    //this.page.Source = pcv;
                    //this.page.PageIndexChanged += page_PageIndexChanged;
                    //this.page.PageIndex = pageindex;
                }
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
                MessageBox.Show("请选择要更新的商品型号！");
                return;
            }
            API.View_ProNameInfo pac = new API.View_ProNameInfo(); //(API.Pro_ProNameInfo)GridList.SelectedItem;
            API.Pro_ProNameInfo pac_old = (API.Pro_ProNameInfo)GridList.SelectedItem;

            if (string.IsNullOrEmpty(updateProName.Text))
            {
                MessageBox.Show("商品型号不能为空！");
                return;
            }
            if (MessageBox.Show("确定修改吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            pac.ID = pac_old.ID;
            pac.Note = updNote.Text;
            pac.MainName = updateProName.Text;


 
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 253, new object[] { pac }, new EventHandler<API.MainCompletedEventArgs>(UpdateCompleted));
            
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
                //Clear();
                //GetList();
                API.Pro_ProNameInfo pac_old = (API.Pro_ProNameInfo)GridList.SelectedItem;
                pac_old.Note = updNote.Text;
                pac_old.MainName = updateProName.Text;
                #region 更新本地缓存总商品
                var mains=Store.ProMainInfo.Where(p=>p.ProNameID==pac_old.ID);
                foreach (var x in mains)
                {
                    x.ProMainName = pac_old.MainName;
                }

                var proinfos = Store.ProInfo.Where(p=> mains.Any(y=>y.ProMainID==p.ProMainID));
                foreach (var x in proinfos)
                {
                    x.ProName = pac_old.MainName;
                }
                #endregion

                
                this.GridList.Rebind();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(addProName.Text))
            {
                MessageBox.Show("商品型号名称不能为空！");
                return;
            }

            if (MessageBox.Show("确定添加吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
        
            API.Pro_ProNameInfo model = new API.Pro_ProNameInfo();
            model.Note = addNote.Text;
            model.MainName = addProName.Text;
          
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 251, new object[] { model }, new EventHandler<API.MainCompletedEventArgs>(AddCompleted));
       
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
                //Clear();
                //GetList();
                API.Pro_ProNameInfo model = (API.Pro_ProNameInfo)e.Result.Obj;
                Store.ProNameInfo.Add(model);
                this.GridList.Rebind();
            }
        }

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

            API.Pro_ProNameInfo item = GridList.SelectedItem as API.Pro_ProNameInfo;

            this.updateProName.Text = item.MainName;
            this.updNote.Text = item.Note;
            this.updateNameID.Text = item.NameID;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridList.SelectedItem == null)
            {
                MessageBox.Show("请选择要删除的商品型号！");
                return;
            }
            if (MessageBox.Show("确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            API.Pro_ProNameInfo proName = GridList.SelectedItem as API.Pro_ProNameInfo;
            API.View_ProNameInfo item = new API.View_ProNameInfo()
            {
                ID = proName.ID,
                MainName = proName.MainName
            };


            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 252, new object[] {item }, new EventHandler<API.MainCompletedEventArgs>(DelCompleted));

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
                //Clear();
                //GetList();
                API.Pro_ProNameInfo proName = GridList.SelectedItem as API.Pro_ProNameInfo;
                Store.ProNameInfo.Remove(proName);
                this.GridList.Rebind();
            }
        }

        private void Clear()
        {
            this.updateProName.Text = string.Empty;
            this.updNote.Text = string.Empty;
            this.addProName.Text = string.Empty;
            this.addNote.Text = string.Empty;
            this.updateNameID.Text = string.Empty;
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
            //rpp.PageIndex = page.PageIndex;
            //rpp.PageSize = (int)pagesize.Value;
            PublicRequestHelp pqh = new PublicRequestHelp(this.busy, 250, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));

        }


        void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            GetList();
        }
    }
}
