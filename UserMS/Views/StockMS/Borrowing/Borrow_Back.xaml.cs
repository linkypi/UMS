using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SlModel;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.Borrowing
{
    public partial class Borrow_Back : Page
    {
        private List<API.View_BorowReturnInfo> models = null;
        private List<int> listID = new List<int>();

        private List<API.IMEIModel> checkImei = null;
        /// <summary>
        /// 借贷详情列表
        /// </summary>
        private List<API.BorowListModel> detailModels = null;
        /// <summary>
        /// 无串码商品
        /// </summary>
        private List<SlModel.ViewModel> unIMEIModels = null;
        /// <summary>
        /// 未拣货的串码
        /// </summary>
        private List<SlModel.CheckModel> uncheckIMEI = null;

        private bool flag = false;

        /// <summary>
        /// 串码数量
        /// </summary>
        private int IMEICount = 0;
        private ProAdder<SlModel.ViewModel> adder;
        private HallFilter hAdder;
        private string menuid = "";
        private int pageIndex = 0;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
         
            menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            if (menuid == null)
            {
                menuid = "16";
            }
        
            models = new List<API.View_BorowReturnInfo>();
            GridBorowList.ItemsSource = models;
            this.radDataPager1.PageSize = (int)pagesize.Value;
            this.fromDate.DateTimeText = DateTime.Now.ToShortDateString();
            hAdder = new HallFilter(true, ref this.hallid);

            uncheckIMEI = new List<SlModel.CheckModel>();
            unIMEIModels = new List<SlModel.ViewModel>();
            detailModels = new List<API.BorowListModel>();
            checkImei = new List<API.IMEIModel>();
            //绑定数据
            GridUnCheckPro.ItemsSource = unIMEIModels;
            GridCheckedPro.ItemsSource = detailModels;
            GridUnCheckIMEI.ItemsSource = uncheckIMEI;

            adder = new ProAdder<SlModel.ViewModel>(ref unIMEIModels, ref GridUnCheckPro, int.Parse(menuid), 2);

            List<CkbModel> list = new List<CkbModel>();
            CkbModel ck1 = new CkbModel(true, "是");
            CkbModel ck2 = new CkbModel(false, "否");
            CkbModel ck3 = new CkbModel(false, "全部");
            list.Add(ck1);
            list.Add(ck2);
            list.Add(ck3);
            this.ckb.ItemsSource = list;
            this.ckb.UpdateLayout();
            this.ckb.DisplayMemberPath = "Text";
            this.ckb.SelectedValuePath = "Flag";
            this.ckb.SelectedIndex = 1;
            this.batchReturn.IsEnabled = true;
            flag = true;
            Search();

            this.ckb.KeyDown += Ckb_KeyDown;
            this.user.KeyDown += Ckb_KeyDown;
            this.hallid.KeyDown += Ckb_KeyDown;
            this.fromDate.KeyDown += Ckb_KeyDown;
            this.toDate.KeyDown += Ckb_KeyDown;

            this.hallid.SearchButton.Click += SearchButton_Click;
            GridCheckedPro.SelectionChanged += GridCheckedPro_SelectionChanged;
            this.delIMEI.Click += delCheckedIMEI_Click; //删除选中的串码
            this.delPro.Click += delCheckedPro_Click;//删除选中的商品
            this.addNoIMEIPros.Click += Add_Click;
            this.cancel.Click += cancel_Click;
            this.Sumbit.Click += new Telerik.Windows.RadRoutedEventHandler(Sumbit_Click);
            this.txtIMEI.KeyUp += txtIMEI_KeyUp;
          
        }

        void Borrow_Back_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //StackPanel sp = this.FindName("spanel") as StackPanel;
         
            //WrapPanel wp = this.FindName("panel") as WrapPanel;
            //wp.Width = sp.ActualWidth;

            //RadDataPager rdp = this.FindName("radDataPager1") as RadDataPager;
            //RadNumericUpDown nud = this.FindName("pagesize") as RadNumericUpDown;
            //rdp.Width = sp.ActualWidth - nud.Width;
        }

        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            Search();
        }

        public Borrow_Back()
        {
            InitializeComponent();
            this.SizeChanged += Borrow_Back_SizeChanged;
        }

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            hAdder.GetHall(hAdder.FilterHall(int.Parse(menuid),Store.ProHallInfo));
        }

        #region  详情

        /// <summary>
        /// 选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GridBorowList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridBorowList.SelectedItem == null)
            {
                return;
            }

            API.View_BorowReturnInfo bm = GridBorowList.SelectedItem as API.View_BorowReturnInfo;
            this.GridCheckedPro.Tag = bm.ID;
            if (bm.IsReturn == "Y")
            {
                this.BatchAddIMEI.IsEnabled = false;
                this.addNoIMEIPros.IsEnabled = false;
                this.batchReturn.IsEnabled = false;
                this.Sumbit.IsEnabled = false;
            }
            else
            {
                this.BatchAddIMEI.IsEnabled = true;
                this.addNoIMEIPros.IsEnabled = true;
                this.batchReturn.IsEnabled = true;
                this.Sumbit.IsEnabled = true;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 39, new object[] { bm.ID.ToString() }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
           
        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器错误\n" + e.Error.Message);
                return;
            }
            detailModels.Clear();

            if (e.Result.ReturnValue)
            {
                List<API.BorowListModel> list = e.Result.Obj as List<API.BorowListModel>;
               
                detailModels.AddRange(list);
                IMEICount = 0;
                foreach (var item in list)
                {
                    if (item.NeedIMEI)
                    {
                        IMEICount += item.IIMEIList.Count;
                    }
                }
                GridCheckedIMEI.ItemsSource = null;
                uncheckIMEI.Clear();
                GridCheckedIMEI.Rebind();
                GridUnCheckIMEI.Rebind();
                GridCheckedPro.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Error.Message);
            }
        }

        #endregion 

        #region 事件


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定取消吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            Clear();
        }

        private void Clear()
        {
            this.txtIMEI.Text = String.Empty;
            IMEICount = 0;

            detailModels.Clear();
            uncheckIMEI.Clear();
            unIMEIModels.Clear();
            GridCheckedIMEI.ItemsSource = null;
            GridUnCheckPro.Rebind();
            GridCheckedPro.Rebind();
            GridCheckedIMEI.Rebind();
            GridUnCheckIMEI.Rebind();
        }

        /// <summary>
        /// 添加无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            adder.Add();
        }

        /// <summary>
        /// 选中已拣货码商品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridCheckedPro_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridCheckedPro.SelectedItem == null)
            {
                return;
            }
            API.BorowListModel list = GridCheckedPro.SelectedItem as API.BorowListModel;

            GridCheckedIMEI.ItemsSource = list.IIMEIList;
            GridCheckedIMEI.Rebind();
          
            //ViewCommon.GridSelectChanged(ref checkedModels, ref GridCheckedPro, ref GridCheckedIMEI);
        }

        #endregion

        #region  部分归还

        /// <summary>
        /// 部分归还
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            if (detailModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"详情无数据");
                return;
            }
            if (uncheckIMEI.Count == 0 && unIMEIModels.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无数据可归还");
                return;
            } 
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定归还您当前所添加的数据吗？", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }
            //借贷详情中的串码集合
            List<string> imeis = new List<string>();
            //借贷详情中的无串码商品
            List<API.BorowListModel> prolist = new List<API.BorowListModel>();
            //串码集合
            List<API.IMEIModel> IMEImodels = new List<API.IMEIModel>(); 

            foreach (var item in detailModels)
            {
                if (item.NeedIMEI)  //对比串码
                {
                    IMEImodels.AddRange(item.IIMEIList);
                    foreach (var child in item.IIMEIList)
                    {
                        imeis.Add(child.OldIMEI);
                    }
                }
                else
                {
                    prolist.Add(item);
                }
            }

            #region  验证串码的正确性及是否归还
  
            bool flag = false;
            if (uncheckIMEI.Count != 0)
            {
                //检查借贷和归还的串码是否一致
                foreach (var child in uncheckIMEI)
                {
                    foreach (var item in imeis)
                    {
                        if (child.IMEI.ToUpper() == item.ToUpper())
                        {
                            flag = true;
                         
                            break;
                        }
                    }
                    if (!flag)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"归还串码与借贷串码不匹配");
                        return;
                    }
                     flag = false;
                }

                #region  验证串码是否已经归还
                foreach (var child in uncheckIMEI)
                {
                    foreach (var childx in IMEImodels)
                    {
                        if (child.IMEI.ToUpper() == childx.OldIMEI.ToUpper())
                        {
                            if (childx.Note == "Y")
                            {
                                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码 " + child.IMEI + " 已经归还");
                                return;
                            }
                        }
                    }
                }
                #endregion

            }
            #endregion

            #region 若有无串号商品归还   则判断归还商品是否正确  数量是否在借贷数量的范围内

            foreach (var item in unIMEIModels)
            {
                if (item.ProCount <= 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"归还商品"+ item.ProName+"的数量无效");
                    return;
                }
            }
            if (unIMEIModels.Count != 0)
            {
                flag = false;
                foreach (var item in unIMEIModels)
                {
                    item.AduitCount = item.ProCount;
                }
                foreach (var item in unIMEIModels)
                {
                    foreach (var child in prolist)
                    {
                        if (child.ProID == item.ProID)
                        {
                            flag = true;
                            item.AduitCount -= child.UnReturnCount;
                            if (item.AduitCount <= 0)
                            {
                                break;
                            }
                        }
                    }
                    //若拣货后的商品数量大于0 
                    if (flag && item.AduitCount > 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品 " + item.ProName + " 的数量已超过了归还数量");
                        return;
                    }
                    if (!flag)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"借贷单中不存在商品 "+item.ProName);
                        return;
                    }
                    flag = false;
                }
            }
            #endregion

            //添加表头
            API.Pro_ReturnInfo rinfo = new API.Pro_ReturnInfo();
   
            rinfo.UserID = Store.LoginUserInfo.UserID;
            rinfo.ReturnDate = DateTime.Now;
            rinfo.SysDate = DateTime.Now;
           
            //添加串码 和串码明细
            API.Pro_ReturnOrderIMEI b_imei = null;
            API.Pro_ReturnListInfo rlist = null;
          
            #region  添加无串码商品
            decimal returnCount = 0;
            foreach (var child in unIMEIModels)
            {
                if (child.ProCount<=0)
                {
                    continue;
                }
                returnCount = child.ProCount;
             
                foreach (var item in detailModels)
                {
                    if (item.ProID == child.ProID)
                    {
                        if (item.UnReturnCount == 0)
                        {
                            continue;
                        }
                        rlist = new API.Pro_ReturnListInfo();
                        //添加入库明细
                        rlist.BorowListID = item.BorowListID;

                        //*************************************
                        if (returnCount > item.UnReturnCount) //   if (returnCount > item.ProCount)
                        {
                            rlist.ProCount = item.UnReturnCount;
                                returnCount -= item.UnReturnCount;
                        }
                        else
                        {
                            rlist.ProCount = returnCount;
                            returnCount = 0;
                        }
                        rlist.ProID = item.ProID;
                        rlist.InListID = item.InListID;

                        if (rinfo.Pro_ReturnListInfo == null)
                        {
                            rinfo.Pro_ReturnListInfo = new List<API.Pro_ReturnListInfo>();
                        }
                        rinfo.Pro_ReturnListInfo.Add(rlist);
                        if (returnCount == 0)
                        {
                            break;
                        }
                    }
                }
            }
            #endregion

            #region  添加归还串码
            if (rinfo.Pro_ReturnListInfo == null)
            {
                rinfo.Pro_ReturnListInfo= new List<API.Pro_ReturnListInfo>();
            }
            foreach (var child in uncheckIMEI)
            {
                foreach (var item in detailModels)
                {
                    if (!item.NeedIMEI )
                    {
                        continue;
                    }
                    bool found = false;
                    foreach (var item2 in item.IIMEIList)
                    {
                        if (item2.OldIMEI.ToUpper() == child.IMEI.ToUpper())
                        {
                            found = true;
                            #region  添加前判断是否已经有该商品存在

                            b_imei = new API.Pro_ReturnOrderIMEI();
                            b_imei.IMEI = child.IMEI;
                             
                            bool exist = false;
                         
                                foreach (var xitem in rinfo.Pro_ReturnListInfo)
                                {
                                    if (xitem.ProID == item.ProID && xitem.InListID == item.InListID)
                                    {
                                        exist = true;
                                        xitem.ProCount++;
                                        xitem.Pro_ReturnOrderIMEI.Add(b_imei);
                                        break;
                                    }
                                }
                                if (!exist)
                                {
                                    rlist = new API.Pro_ReturnListInfo();
                                    rlist.BorowListID = item.BorowListID;
                                    if (string.IsNullOrEmpty(rlist.ProCount.ToString()))
                                    {
                                        rlist.ProCount = 1;
                                    }
                                    else
                                    {
                                        rlist.ProCount++;
                                    }
                                    rlist.ProID = item.ProID;
                                    rlist.InListID = item.InListID;
                                    if (rlist.Pro_ReturnOrderIMEI == null)
                                    {
                                        rlist.Pro_ReturnOrderIMEI = new List<API.Pro_ReturnOrderIMEI>();
                                    }
                                    rlist.Pro_ReturnOrderIMEI.Add(b_imei);
                                    rinfo.Pro_ReturnListInfo.Add(rlist);
                                }
                            
                            #endregion 
                            break;
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                }
            }
            #endregion

            #region 判断该单是否全部归还完毕

            //bool returnFlag = true;

            // decimal RCount=0;
            // decimal unretCount = 0;
            //foreach (var item in rinfo.Pro_ReturnListInfo)
            //{
            //    RCount +=(decimal) item.ProCount;
            //}

            //foreach (var child in detailModels)
            //{
            //    unretCount += (decimal)child.UnReturnCount;
            //}
            //if (RCount < unretCount)
            //{
            //    returnFlag = false;
            //}

            #endregion 

            PublicRequestHelp help = new PublicRequestHelp(this.isbusy, 15,
                 new object[] {  rinfo },
                 new EventHandler<API.MainCompletedEventArgs>(SubmitSingleCompleted)
                 );

        }

        /// <summary>
        /// 提交完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitSingleCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "归还失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            try
            {
                API.WebReturn eb = e.Result;

                if (!e.Result.ReturnValue)
                {
                    Logger.Log("归还失败！");
                    if (eb.Message != null)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,eb.Message);
                    }
                }
                else
                {
                    ///清空数据
                    Clear();
                    Search();
                  
                    foreach (var item in models)
                    {
                        if (item.ID.ToString() == GridCheckedPro.Tag.ToString())
                        {
                            GridCheckedPro.SelectedItem = item;
                        }
                    }
                    GridCheckedPro.Tag = null;
                    Logger.Log("归还成功！");
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,eb.Message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 

        #region  批量添加商品串码

        void txtIMEI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BacthAddIMEI();
            }
        }

        /// <summary>
        /// 批量添加商品串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BatchAddIMEI_Click(object sender, RoutedEventArgs e)
        {
            BacthAddIMEI();
        }

        private void BacthAddIMEI()
        {
            if (String.IsNullOrEmpty(this.txtIMEI.Text.Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码不能为空");
                return;
            }

            List<string> list = new List<string>(txtIMEI.Text.Trim().Replace("\n", "").Split("\r".ToCharArray()));
            if (uncheckIMEI.Count + list.Count > IMEICount)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"串码数量已经超出借贷的数量");
                return;
            }

            SlModel.CheckModel cm = null;
            foreach (string s in list)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (!ViewCommon.ValidateIMEI(s, uncheckIMEI))  //去除重复项
                    {
                        cm = new SlModel.CheckModel();
                        cm.IMEI = s;
                        uncheckIMEI.Add(cm);
                    }
                }
            }

            GridUnCheckIMEI.Rebind();

            this.txtIMEI.Text = string.Empty;
        }

        #endregion 

        #region 删除

        /// <summary>
        /// 删除Checked选中的商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedPro_Click(object sender, RoutedEventArgs e)
        {
            ViewCommon.DelCheckedPro(ref unIMEIModels, ref GridUnCheckPro);
        }

        /// <summary>
        /// 删除Checked选中的串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedIMEI_Click(object sender, RoutedEventArgs e)
        {
            ViewCommon.DelCheckedIMEI(ref  uncheckIMEI, ref GridUnCheckIMEI);
        }

        #endregion 

        #region 查询

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Ckb_KeyDown(object sender, KeyEventArgs e)
        {
            if(Key.Enter==e.Key)
            {
                Search();
            }
        }

        private void Search()
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageIndex = radDataPager1.PageIndex;
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

            if (!string.IsNullOrEmpty(this.hallid.TextBox.SearchText))
            {
                API.ReportSqlParams_String hall = new API.ReportSqlParams_String();
                hall.ParamName = "HallName";
                hall.ParamValues = this.hallid.TextBox.SearchText;
                rpp.ParamList.Add(hall);
            }
            if (this.ckb.SelectedIndex != 2)
            {
                API.ReportSqlParams_String aduit = new API.ReportSqlParams_String();
                aduit.ParamName = "IsReturn";
                aduit.ParamValues = Convert.ToBoolean(this.ckb.SelectedValue.ToString()) ? "Y" : "N";
                rpp.ParamList.Add(aduit);
            }

            if (!string.IsNullOrEmpty(this.user.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "UserID";
                users.ParamValues = this.user.Text;
                rpp.ParamList.Add(users);
            }

            if (!string.IsNullOrEmpty(this.borowid.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "BorowID";
                users.ParamValues = this.borowid.Text.Trim();
                rpp.ParamList.Add(users);
            }

            if (!string.IsNullOrEmpty(this.borowType.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "BorowType";
                bt.ParamValues = this.borowType.Text;
                rpp.ParamList.Add(bt);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 71, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            ///清除分页数目
            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.radDataPager1.PageIndexChanged -= radDataPager1_PageIndexChanged;
            this.radDataPager1.Source = pcv1;
            this.radDataPager1.PageIndexChanged += radDataPager1_PageIndexChanged;

            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_BorowReturnInfo> list = pageParam.Obj as List<API.View_BorowReturnInfo>;
                if (list == null) { return; }
                models.Clear();
                models.AddRange(list);
                GridBorowList.Rebind();

                this.radDataPager1.PageSize = (int)pagesize.Value;
                string[] data = new string[pageParam.RecordCount];
                PagedCollectionView pcv = new PagedCollectionView(data);
                this.radDataPager1.PageIndexChanged -= radDataPager1_PageIndexChanged;
                this.radDataPager1.Source = pcv;
                this.radDataPager1.PageIndexChanged += radDataPager1_PageIndexChanged;
                this.radDataPager1.PageIndex = pageIndex;
            }
            else
            {
                models.Clear();
                GridBorowList.Rebind();
            }

        }

        #endregion

        #region 换页

        /// <summary>
        /// 换页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void radDataPager1_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            pageIndex = e.NewPageIndex;
            Search();
        }

       #endregion 

        #region 全部归还

        /// <summary>
        /// 全部归还
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void batchReturn_Click(object sender, RoutedEventArgs e)
        {
            if (models.Count==0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"列表无数据，无法执行该操作！");
                return;
            }

            if (GridBorowList.SelectedItems == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择需要归还的数据！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定将您所选的数据执行全部归还吗？","提示",MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }
            List<API.Pro_ReturnInfo> list = new List<API.Pro_ReturnInfo>();
            API.Pro_ReturnInfo rinfo = null;
            foreach (var item in GridBorowList.SelectedItems)
            {
                API.View_BorowReturnInfo bm = item as API.View_BorowReturnInfo;
                listID.Add(bm.ID);
                rinfo = new API.Pro_ReturnInfo();
                rinfo.BorowID = bm.ID;
                rinfo.UserID = Store.LoginUserInfo.UserID;
                rinfo.ReturnDate = DateTime.Parse(DateTime.Today.ToString().Trim());
                rinfo.SysDate = DateTime.Now;
                list.Add(rinfo);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 38, new object[] { list }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));

        }

        private void SubmitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "归还失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                Search();
               
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"归还成功");
                Logger.Log("归还成功");
                //foreach (var item in listID)
                //{
                //    foreach (var child in models)
                //    {
                //        if (child.ID == item)
                //        {
                //            models.Remove(child);
                //            break;
                //        }
                //    }
                //}
                //GridBorowList.Rebind();
                listID.Clear();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                Logger.Log(e.Result.Message);
            }
        }

        #endregion

        private void GridUnCheckPro_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            foreach (var item in unIMEIModels)
            {
                 if (item.ProCount<0)
                 {
                     MessageBox.Show(System.Windows.Application.Current.MainWindow,"归还数量不能为负数！");
                     item.ProCount = 0;
                     return;
                 }
                if (!item.IsDecimal)
                {
                    item.ProCount = (int)(Decimal.Truncate(Convert.ToDecimal(item.ProCount * 100)) / 100);
                    continue;
                }
                item.ProCount = Decimal.Truncate(Convert.ToDecimal(item.ProCount * 100)) / 100;
            }
        }
    }
}
