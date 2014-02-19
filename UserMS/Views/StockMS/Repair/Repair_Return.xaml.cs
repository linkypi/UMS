using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SlModel;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.Repair
{
    public partial class Repair_Return : Page
    {
        private List<API.View_Pro_RepairInfo> models = null;
        private bool flag = false;

        private List<API.IMEIModel> imeiModels = new List<API.IMEIModel>();
     
        /// <summary>
        /// 原串码列表
        /// </summary>
        private List<string> strList = new List<string>();

        /// <summary>
        /// 仓库添加器
        /// </summary>
        private ROHallAdder hAdder;
        private ROHallAdder hAdder2;
        private string menuid = "";
        private int pageindex;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                menuid = "21";
            }
            finally
            {
                if (menuid == null)
                {
                    menuid = "21";
                }
                models = new List<API.View_Pro_RepairInfo>();
                DGrepairInfo.ItemsSource = models;
                this.page.PageSize = (int)pagesize.Value;
                hAdder = new ROHallAdder(ref this.hallid, int.Parse(menuid));
                //hAdder2 = new ROHallAdder(ref this.desHall, int.Parse(menuid));
        
                this.fromDate.SelectedValue = DateTime.Now.Date;
                GridIMEI.ItemsSource = imeiModels;
                List<CkbModel> list = new List<CkbModel>() { 
                new  CkbModel(true,"是"),
                new  CkbModel(false,"否"),
                new  CkbModel(false,"全部") };
                this.ckb.ItemsSource = list;
                this.ckb.SelectedIndex = 1;
                //this.ckbReceive.ItemsSource = list;
               // this.ckbReceive.SelectedIndex = 0;
                flag = true;

                Search();

                this.batchReturn.Click += batchReturn_Click;
             
            }
        }

        private void SearchHall(object sender, RoutedEventArgs e)
        {
            hAdder.SearchHall(sender, e);
        }
     
        void Repair_Return_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WrapPanel wp = this.FindName("panel") as WrapPanel;
            wp.Width = e.NewSize.Width;

            RadDataPager rdp = this.FindName("page") as RadDataPager;
            RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            rdp.Width = e.NewSize.Width - nud.Width;

            //WrapPanel btwp = this.FindName("btcontrol") as WrapPanel;
            //RadGridView detail = this.FindName("GridDetail") as RadGridView;
            //detail.Width = btwp.ActualWidth * 0.4;
            //RadGridView imei = this.FindName("GridIMEI") as RadGridView;
            //imei.Width =(int) (btwp.ActualWidth * 0.25);
            //Button btn = this.FindName("btnAddIMEI") as Button;
            //btn.Width =(int)( btwp.ActualWidth * 0.25);
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        public Repair_Return()
        {
            InitializeComponent();
            this.SizeChanged += Repair_Return_SizeChanged;
        }

      
        /// <summary>
        /// 换页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dataPager_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            Search();
        }

        #region  送修详情

        private void DGrepairInfo_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            if (DGrepairInfo.SelectedItem == null)
            {
                return;
            }
            API.View_Pro_RepairInfo rm = DGrepairInfo.SelectedItem as API.View_Pro_RepairInfo;
            this.repairID.Text = rm.RepairID;
            if (rm.IsReturn == "N" && rm.IsReceive == "Y")
            {
                this.repaireReturn.IsEnabled = true;
            }
            else
            {
                this.repaireReturn.IsEnabled = false;
            }
            this.orderID.Text = rm.OldID;
            this.repairID.Tag = rm.ID;
            this.repairDate.Text = rm.RepairDate;
            this.UserName.Text = rm.UserName;
            this.hallName.Text = rm.HallName;
            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 43, new object[] { rm.ID }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));

        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.View_RepaireRetList> list = e.Result.Obj as List<API.View_RepaireRetList>;
                GridDetail.ItemsSource = list; 
                GridDetail.Rebind();

                strList.Clear();
                foreach (var item in list)
                {
                    if (item.NeedIMEI)
                    {
                        strList.Add(item.IMEI.ToUpper());
                    }
                }
            }
        }

        #endregion

        #region  返  库

        /// <summary>
        /// 批量返库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void batchReturn_Click(object sender, RoutedEventArgs e)
        {
            if (DGrepairInfo.SelectedItems == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项");
                return;
            }
            API.View_Pro_RepairInfo rm = null;
            foreach (var item in DGrepairInfo.SelectedItems)
            {
                rm = item as API.View_Pro_RepairInfo;
                if (rm.IsReturn == "Y")
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"送修单 " + rm.RepairID + "已返库");
                    return;
                }
                if (rm.IsReceive == "N")
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"送修单 "+rm.RepairID+" 未接收");
                    return;
                }
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定全部返库吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            //if (desHall.Tag == null)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择返库仓库！");
            //    return;
            //}
            API.Pro_RepairReturnInfo rrinfo = null;
            List<API.Pro_RepairReturnInfo> list = new List<API.Pro_RepairReturnInfo>();

            foreach (var item in DGrepairInfo.SelectedItems)
            {
                rm = item as API.View_Pro_RepairInfo;
                rrinfo = new API.Pro_RepairReturnInfo();
                rrinfo.HallID = rm.HallID;  //desHall.Tag.ToString();
                rrinfo.IsDelete = false;
                rrinfo.RepairID = rm.ID;
                rrinfo.RepairReturnDate = DateTime.Now;
                rrinfo.UserID = rm.UserID;
                rrinfo.SysDate = DateTime.Now;
                rrinfo.IsReceived = false;
                rrinfo.IsDelete = false;
                list.Add(rrinfo);
            }
       
            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 45, new object[] { list }, new EventHandler<API.MainCompletedEventArgs>(BatchReturnCompleted));
        }

        /// <summary>
        /// 返库完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchReturnCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "返库失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"返库成功");
                this.repairID.Text = string.Empty;
                this.repairID.Tag = null;
                this.repairDate.Text = string.Empty;
                this.UserName.Text = string.Empty;
                this.hallName.Text = string.Empty;

                GridDetail.ItemsSource = null;
                GridDetail.Rebind();
                GridIMEI.ItemsSource = null;
                GridIMEI.Rebind();
                Search();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
        }

        /// <summary>
        /// 单个返库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SingleReturn_Click(object sender, RoutedEventArgs e)
        {
            if (DGrepairInfo.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选中任何项");
                return;
            }
            List<API.View_RepaireRetList> list = GridDetail.ItemsSource as List<API.View_RepaireRetList>;
            //检测是否存在归还的数据
            //bool has = false;
            //foreach (var item in list)
            //{
            //    if (!item.NeedIMEI && item.AduitCount > 0)
            //    {
            //        has = true;
            //        break;
            //    }
            //}
            ////若没有归还的数据则返回
            //if (imeiModels.Count == 0 && (!has))
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加需要归还的数据！");
            //    return;
            //}
            foreach (var item in imeiModels)
            {
                if (!ValidateCorrect(item.OldIMEI))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"旧串码有误: " + item.OldIMEI);
                    return;
                }
            }

            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定返库吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }


           API.View_Pro_RepairInfo rm = DGrepairInfo.SelectedItem as API.View_Pro_RepairInfo;

           #region 添加表头

           API.Pro_RepairReturnInfo rrinfo = new API.Pro_RepairReturnInfo();
           rrinfo.HallID = rm.HallID;
           rrinfo.IsDelete = false;
           rrinfo.RepairID = rm.ID;
           rrinfo.RepairReturnDate = DateTime.Now;
           rrinfo.UserID = Store.LoginUserInfo.UserID;
           rrinfo.SysDate = DateTime.Now;
           rrinfo.IsReceived = false;

           #endregion 

           #region 添加明细

           rrinfo.Pro_RepairReturnListInfo = new List<API.Pro_RepairReturnListInfo>();
           API.Pro_RepairReturnListInfo rrlist = null;

           //添加串码商品   
           foreach (var item in list)
           {
               if (item.NeedIMEI)
               {
                   //if (!Exists(item.IMEI.ToUpper()))
                   //{
                   //    continue;
                   //}
                   rrlist = new API.Pro_RepairReturnListInfo();
                   rrlist.NEW_IMEI = Find(item.IMEI.ToUpper());
                   rrlist.OLD_IMEI = item.IMEI;
                   rrlist.ProCount =1;
               }
               else
               {
                   //if (item.AduitCount == 0)
                   //{
                   //    continue;
                   //}
                   rrlist = new API.Pro_RepairReturnListInfo();
                   rrlist.ProCount = item.ProCount;
               }
              
               rrlist.InListID = item.InListID;
               rrlist.Note = item.Note;
             
               rrlist.ProID = item.ProID;
               rrlist.RepairListID = item.RepairListID;
               rrinfo.Pro_RepairReturnListInfo.Add(rrlist);
           }

           #endregion
      
           PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 17, new object[] { rrinfo }
               , new EventHandler<API.MainCompletedEventArgs>(BatchReturnCompleted));

        }

        /// <summary>
        /// 检测旧串码是否存在
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        private bool Exists(string imei)
        {
            foreach (var item in imeiModels)
            {
                if (item.OldIMEI.Equals(imei))
                {
                    return true;
                }
            }
            return false;
        }

        private string Find(string imei)
        {
            foreach (var item in imeiModels)
            {
                if (imei == item.OldIMEI)
                {
                    return item.NewIMEI;
                }
            }
            return string.Empty;
        }

        #endregion 

        #region 查  询

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            if (!flag) { return; }
         
            imeiModels.Clear();
            GridIMEI.ItemsSource = imeiModels;
            GridIMEI.Rebind();
            GridDetail.ItemsSource = null;
            GridDetail.Rebind();

            this.repairID.Text = "";
            this.hallName.Text = "";
            this.repairDate.Text = "";
            this.UserName.Text = "";

            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = this.page.PageIndex;
            rpp.PageSize = (int)pagesize.Value;
            rpp.ParamList = new List<API.ReportSqlParams>();

            if (!string.IsNullOrEmpty(this.fromDate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime startTime = new API.ReportSqlParams_DataTime();
                startTime.ParamName = "StartTime";
                startTime.ParamValues = this.fromDate.SelectedValue;
                rpp.ParamList.Add(startTime);
            }

            if (!string.IsNullOrEmpty(this.toDate.SelectedValue.ToString()))
            {
                API.ReportSqlParams_DataTime endTime = new API.ReportSqlParams_DataTime();
                endTime.ParamName = "EndTime";
                endTime.ParamValues = this.toDate.SelectedValue;
                rpp.ParamList.Add(endTime);
            }

            if (!string.IsNullOrEmpty(this.hallid.Text))
            {
                API.ReportSqlParams_ListString hall = new API.ReportSqlParams_ListString();
                hall.ParamName = "HallID";
                hall.ParamValues = new List<string>();
                hall.ParamValues.AddRange(this.hallid.Tag.ToString().Split(",".ToCharArray()));
                rpp.ParamList.Add(hall);
            }

            if (this.ckb.SelectedIndex != 2)
            {
                API.ReportSqlParams_String isret = new API.ReportSqlParams_String();
                isret.ParamName = "IsReturn";
                if (this.ckb.SelectedIndex == 1)
                {
                    isret.ParamValues = "N";
                }
                else
                {
                    isret.ParamValues = "Y";
                }
                rpp.ParamList.Add(isret);
            }

           // if (this.ckbReceive.SelectedIndex != 2)
           // {
                API.ReportSqlParams_String receive = new API.ReportSqlParams_String();
                receive.ParamName = "IsReceive";
           
                receive.ParamValues = "Y";
                
                rpp.ParamList.Add(receive);
           // }

            if (!string.IsNullOrEmpty(this.user.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "UserName";
                users.ParamValues = this.user.Text;
                rpp.ParamList.Add(users);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.IsBusy, 68, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));
   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.IsBusy.IsBusy = false;
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= dataPager_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += dataPager_PageIndexChanged;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_Pro_RepairInfo> repairList = pageParam.Obj as List<API.View_Pro_RepairInfo>;
                models.Clear();
                if (repairList!=null)
                {
                    models.AddRange(repairList);
                }
               
                DGrepairInfo.Rebind();

                this.page.PageSize = (int)pagesize.Value;
            
                string[] data = new string[pageParam.RecordCount];

                PagedCollectionView  pcv= new PagedCollectionView(data);
                this.page.PageIndexChanged -= dataPager_PageIndexChanged;
                this.page.Source = pcv;
                this.page.PageIndexChanged += dataPager_PageIndexChanged;
                this.page.PageIndex = pageindex;

                this.DGrepairInfo.Columns[DGrepairInfo.Columns.Count - 1].IsVisible = false;
                if (this.ckb.SelectedIndex == 0)
                {
                    this.DGrepairInfo.Columns[DGrepairInfo.Columns.Count - 2].IsVisible = false;
                    ///  this.DGrepairInfo.Columns[DGrepairInfo.Columns.Count - 1].IsVisible = true;
                    this.batchReturn.IsEnabled = false;
                }
                else
                {
                    this.DGrepairInfo.Columns[DGrepairInfo.Columns.Count - 2].IsVisible = true;
                }

                if (this.ckb.SelectedIndex == 1)
                {
                    this.batchReturn.IsEnabled = true;
                }
            }
            else
            {
                models.Clear();
                DGrepairInfo.Rebind();
            }

        }

        #endregion

        #region 添加,删除串码

        /// <summary>
        /// 添加串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAddIMEI_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtoldIMEI.Text.ToString().Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码不能为空");
                return;
            }
            List<API.View_RepaireRetList> list = GridDetail.ItemsSource as List<API.View_RepaireRetList>;
            string oldIMEI = this.txtoldIMEI.Text.ToString().Trim();
            string newIMEI = this.txtNewIMEI.Text.ToString().Trim();

            List<string> oldList = oldIMEI.ToUpper().Replace("\n","").Split("\r".ToCharArray()).ToList();
            List<string> newList = newIMEI.ToUpper().Replace("\n", "").Split("\r".ToCharArray()).ToList();

            #region 验证串码是否已归还

            //foreach (var item in oldList)
            //{
            //    foreach (var child in list)
            //    {
            //        if (item.Equals(child.IMEI.ToUpper()) && child.IsReturn.Equals("Y"))
            //        {
            //            MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码" + child.IMEI + "已返库！");
            //            return;
            //        }
            //    }
            //}
            #endregion

            #region "验证新旧串码是否重复"

            string old = Validate(oldList);
            if (!string.IsNullOrEmpty(old))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"旧串码重复：" + old);
                return;
            }
            foreach (var item in imeiModels)
            {
                API.IMEIModel imei = item as API.IMEIModel;
                foreach (var child in oldList)
                {
                    if (item.OldIMEI.ToUpper() == child.ToUpper())
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码已存在");
                        return;
                    }
                }
            }
            //验证旧串码是否正确
            foreach (var item in oldList)
            {
                if (!strList.Contains(item))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"旧串码不正确: " + item);
                    return;
                }
            }

            string nmstr = Validate(newList);
            if (!string.IsNullOrEmpty(nmstr))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"新串码重复：" + nmstr);
                return;
            }
            #endregion

     

            API.IMEIModel im = null;
            for (int i = 0; i < oldList.Count; i++)
            {
                im = new API.IMEIModel();
                im.OldIMEI = oldList[i];
                if (i < newList.Count)
                {
                    im.NewIMEI = newList[i];

                }
                else
                {
                    im.NewIMEI = "";
                }
                imeiModels.Add(im);
            }
            GridIMEI.Rebind();
            this.txtoldIMEI.Text = string.Empty;
            this.txtNewIMEI.Text = string.Empty;

            //更新串码的归还数量
       
            //foreach (var item in imeiModels)
            //{
            //    foreach (var child in list)
            //    {
            //        if (child.IMEI.Equals(item.OldIMEI))
            //        {
            //            child.AduitCount = 1;
            //            break;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 验证串码是否重复
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        string Validate(List<string> list)
        {
            int count = 0;
            foreach (var item in list)
            {
                foreach (var child in list)
                {
                    if (item == child)
                    {
                        count++;
                    }
                }
                if (count > 1)
                {
                    return item;
                }
                count = 0;
            }
            return string.Empty;
        }

        /// <summary>
        /// 删除串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delIMEI_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除选中的商品吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            if (GridIMEI.SelectedItems == null)
            {
                return;
            }
            API.IMEIModel im = null;
            foreach (var child in GridIMEI.SelectedItems)
            {
                im = child as API.IMEIModel;
                foreach (var item in imeiModels)
                {
                    if (item.OldIMEI == im.OldIMEI)
                    {
                        imeiModels.Remove(item);
                        break;
                    }
                }
            }
            GridIMEI.Rebind();
        }

        /// <summary>
        /// 验证串码是否正确
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        bool ValidateCorrect(string imei)
        {
            foreach (var child in strList)
            {
                if (imei == child)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion 

        /// <summary>
        /// 删除串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void GridDetail_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            //if (GridDetail.SelectedItem == null)
            //{
            //    return;
            //}
            //API.View_RepaireRetList rm = GridDetail.SelectedItem as API.View_RepaireRetList;
            //GridDetail.Columns[9].IsReadOnly = false;
            //if (rm.NeedIMEI)
            //{
            //    GridDetail.Columns[9].IsReadOnly = true;
            //}
        }

        private void GridDetail_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            //List<API.View_RepaireRetList> list = GridDetail.ItemsSource as List<API.View_RepaireRetList>;

            //foreach (var item in list)
            //{
            //    if (item.AduitCount < 0)
            //    {
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow,"返库数量不能为负数！");
            //        item.AduitCount = 0;
            //        continue;
            //    }
            //    if (item.AduitCount>item.ProCount-item.BackCount)
            //    {
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow,"返库数量不能大于未返库数量！");
            //        item.AduitCount = 0;
            //        return;
            //    }
            //    if ((!item.NeedIMEI) && (!item.ISdecimals)) 
            //    {
            //        item.AduitCount = (int)(Decimal.Truncate(Convert.ToDecimal(item.AduitCount * 100)) / 100);
            //        continue;
            //    } 
            //    if ((!item.NeedIMEI) && (item.ISdecimals)) 
            //    {
            //        item.AduitCount = (Decimal.Truncate(Convert.ToDecimal(item.AduitCount * 100)) / 100);
            //    }
            //}
        }

    }
}
