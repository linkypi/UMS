using System;
using System.Collections;
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
using SlModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// Repair.xaml 的交互逻辑
    /// </summary>
    public partial class Repair : Page
    {
        //缺料情况下  一定要填写缺料备注
        //审计完成后才算完成一次维修
        List<API.View_BJModels> bjModels = new List<API.View_BJModels>();
        List<SlModel.CkbModel> chkmodels = new List<SlModel.CkbModel>();
        int pageIndex = 0;
       // private bool lackSave = false;
        bool flag = false;
        List<API.View_ASPCurrentOrderInfo> models = new List<API.View_ASPCurrentOrderInfo>();
        List<API.ASP_ErrorInfo> errinfo = new List<API.ASP_ErrorInfo>();
        List<API.View_ASPCurrentOrderPros> proModels = new List<API.View_ASPCurrentOrderPros>();
        List<API.View_ASPCurrentOrderPros> lackProModels = new List<API.View_ASPCurrentOrderPros>();
        ROHallAdder pros_hadder = null;
        private List<UserOpModel> UserOpList = new List<UserOpModel>();
        private int index = 0;  //配件计数

        /// <summary>
        /// 受理网点
        /// </summary>
        private ROHallAdder hadder = null;
        private ProDetailAdder<API.View_BJModels> bjAdder = null;
        private ProDetailAdder<API.View_ASPCurrentOrderPros> adder = null;

        private ProDetailAdder<API.View_ASPCurrentOrderPros> LackAdder = null;
        private ROHallAdder bjHadder = null;

        private int menuid = 324;

        public Repair()
        {
            InitializeComponent();

            bjGrid.ItemsSource = bjModels;
            chkmodels.AddRange( new   List<SlModel.CkbModel>(){
            new SlModel.CkbModel(false,"保内"),
             new SlModel.CkbModel(false,"保外")} );
            chk_InOut.ItemsSource = chkmodels;
            chk_InOut.SelectedIndex = 1;

            bj_date.Text = DateTime.Now.ToShortDateString();
           // sysdate.DateTimeText = DateTime.Now.ToShortDateString();
            repairer.Text = Store.LoginUserName;
            newErrGrid.ItemsSource = errinfo;
            RepairGrid.ItemsSource = models;
            prosGrid.ItemsSource = proModels;
            prosGrid2.ItemsSource = lackProModels;

            bjHadder = new ROHallAdder(ref this.bjHall, menuid, new EventHandler<MyEventArgs>(bjHadder_AddCompleted));

            //bjHadder = new HallFilter(menuid.ToString(), false, bjHall, new EventHandler<MyEventArgs>(bjHadder_AddCompleted));
            //bjHall.SearchButton.Click += bjHall_Click;
            pros_hadder = new ROHallAdder(ref this.proHall, menuid, new EventHandler<MyEventArgs>(pros_hadder_AddCompleted));

            //pros_hadder = new HallFilter(menuid.ToString(), false, proHall,new EventHandler<MyEventArgs>(pros_hadder_AddCompleted));
            //proHall.SearchButton.Click += ProHall_Click;

            hadder = new ROHallAdder(ref this.hall, menuid);

            List<API.BJModel> arr = new List<API.BJModel>();
            foreach (var item in  Store.ProInfo)
            {
                API.BJModel bj = new API.BJModel();
                bj.ClassID = Convert.ToInt32(item.Pro_ClassID);
                bj.NeedIMEI = item.NeedIMEI;
                bj.ProFormat = item.ProFormat;
                bj.ProID = item.ProID;
                bj.ProMainID = Convert.ToInt32(item.ProMainID);
                bj.ProName = item.ProName;
                bj.TypeID = Convert.ToInt32(item.Pro_TypeID);
                bj.IsDecimal = item.ISdecimals;
                arr.Add(bj);
            }
            LackAdder = new ProDetailAdder<API.View_ASPCurrentOrderPros>(lackProModels, prosGrid2,arr,
            new EventHandler<MyEventArgs>(AddLackProsCompleted));

            //List<CkbModel> list = new List<CkbModel>() {  new  CkbModel(false,"配件") ,
            //new  CkbModel(true,"机头"),
            //new  CkbModel(false,"主板")};
            //GridViewComboBoxColumn comcol1 = prosGrid.Columns[11] as GridViewComboBoxColumn;
            //comcol1.ItemsSource = list;
            //GridViewComboBoxColumn comcol2 = prosGrid.Columns[12] as GridViewComboBoxColumn;
            //comcol2.ItemsSource = list;

           // this.prosGrid.AddHandler(RadComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(GridViewComboBoxColumn_PropertyChanged_1));

         

            flag = true;
            List<SlModel.CkbModel> list2 = new List<SlModel.CkbModel>(){
              new   SlModel.CkbModel(false,"软件升级"),
              new   SlModel.CkbModel(false,"修复"),
              new   SlModel.CkbModel(false,"更换配件"),
               new   SlModel.CkbModel(false,"退回")
             };
            repKind.ItemsSource = list2;
            repKind.SelectedIndex = 0;

            radDataPager1.PageSize = 20;

            //var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
            //UserOpList = userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
            //              (list, info) => new { op = list, user = info })
            // .Join(Store.UserOp, arg => arg.op.OpID, op => op.OpID, (a, t) => new UserOpModel()
            // {
            //     ID = a.op.ID,
            //     HallID = a.op.HallID,
            //     OpID = a.op.OpID,
            //     UserID = a.op.UserID,
            //     Username = a.user.RealName,
            //     opname = t.Name
            // }).ToList();
            //var userinfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model => model.UserID,
            //                          (info, model) => info).ToList();
            Search();
        
        }

        //private void GridViewComboBoxColumn_PropertyChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    RadComboBox comboBox = (RadComboBox)e.OriginalSource;
        //    comboBox.SelectedIndex = 0;

        //    API.View_ASPCurrentOrderInfo model = RepairGrid.SelectedItem as API.View_ASPCurrentOrderInfo;
        //    foreach (var item in proModels)
        //    {
        //        if (comboBox.Tag == null) { continue; }
        //        if (item.IMEI.ToUpper() == comboBox.Tag.ToString())
        //        {
        //            if (item.TName == "主板")
        //            {
        //                item.OldIMEI = model.Pro_IMEI;
        //            }
        //            else if (item.TName == "机头")
        //            {
        //                item.OldIMEI = model.Pro_HeaderIMEI;
        //            }
        //            break;
        //        }
        //    }
        //    //prosGrid.Rebind();
        //}

        private void bjHadder_AddCompleted(object sender, MyEventArgs e)
        {
            API.WebReturn web = Store.wsclient.Main(318, new List<object>() { e.Value == null ? "" : e.Value.ToString() ,true});

            List<API.BJModel> pros = web.Obj as List<API.BJModel>;
            bjAdder = new ProDetailAdder<API.View_BJModels>(bjModels, bjGrid, pros, AddBJCompleted);
       
        }

         ///<summary>
         ///配件仓库选择完成后 请求商品数据
         ///</summary>
        private void pros_hadder_AddCompleted(object sender, MyEventArgs e)
        {
            API.WebReturn web = Store.wsclient.Main(318, new List<object>() { e.Value == null ? "" : e.Value.ToString(),false });

            List<API.BJModel> pros = web.Obj as List<API.BJModel>;
            adder = new ProDetailAdder<API.View_ASPCurrentOrderPros>(proModels, prosGrid, pros,
                new EventHandler<MyEventArgs>(AddProsCompleted));
        
        }

     
        #region 仓库操作

        void ProHall_Click(object sender, RoutedEventArgs e)
        {
            //pros_hadder.GetHall(hadder.FilterHall(Store.ProHallInfo));
        }

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           // hadder.GetHall(hadder.FilterHall(Store.ProHallInfo));
        }

        private void bjHall_Click(object sender, RoutedEventArgs e)
        {
            //bjHadder.GetHall(hadder.FilterHall(Store.ProHallInfo));
        }

        #endregion

        #region 备机

        private void delBJ_Click(object sender, RoutedEventArgs e)
        {
            if (bjGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的商品！");
                return;
            }

            foreach (var item in bjGrid.SelectedItems)
            {
                bjModels.Remove(item as API.View_BJModels);
            }
            bjGrid.Rebind();
        }

        private void addBJ_Click(object sender, RoutedEventArgs e)
        {
            if (bjAdder == null)
            {
                bjAdder = new ProDetailAdder<API.View_BJModels>(bjModels, bjGrid, new List<API.BJModel>(), AddBJCompleted);
            }
            if (RepairGrid.SelectedItem == null)
            {
                MessageBox.Show("请选维修单！");
                return;
            }
            bjAdder.Add();
        }

        private void AddBJCompleted(object sender, MyEventArgs e)
        {
            foreach (var item in bjModels)
            {
                item.ProCount = 1;
            }
            bjGrid.Rebind();
        }

        #endregion 

        #region 查询

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Ckb_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                Search();
            }
        }

        private void Search()
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageSize = (int)pagesize.Value;
            rpp.PageIndex = radDataPager1.PageIndex;
           
            rpp.ParamList = new List<API.ReportSqlParams>();

            API.ReportSqlParams_String Repairer = new API.ReportSqlParams_String();
            Repairer.ParamName = "Repairer";
            Repairer.ParamValues = Store.LoginUserInfo.UserID;
            rpp.ParamList.Add(Repairer);

            API.ReportSqlParams_Bool HasRepaired = new API.ReportSqlParams_Bool();
            HasRepaired.ParamName = "HasRepaired";  
            HasRepaired.ParamValues = false;
            rpp.ParamList.Add(HasRepaired);

            if (!string.IsNullOrEmpty(this.hall.Tag == null ? "" : this.hall.Tag.ToString()))
            {
                API.ReportSqlParams_String hall = new API.ReportSqlParams_String();
                hall.ParamName = "HallID";
                hall.ParamValues = this.hall.Tag.ToString();
                rpp.ParamList.Add(hall);
            }

            //if (!string.IsNullOrEmpty(this.sysdate.DateTimeText.ToString()))
            //{
            //    API.ReportSqlParams_DataTime date = new API.ReportSqlParams_DataTime();
            //    date.ParamName = "SysDate";
            //    date.ParamValues = this.sysdate.SelectedDate;
            //    rpp.ParamList.Add(date);
            //}

            if (!string.IsNullOrEmpty(this.oldid.Text.ToString()))
            {
                API.ReportSqlParams_String users = new API.ReportSqlParams_String();
                users.ParamName = "OldID";
                users.ParamValues = this.oldid.Text.Trim();
                rpp.ParamList.Add(users);
            }

            if (!string.IsNullOrEmpty(this.pro_imei.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Pro_IMEI";
                bt.ParamValues = this.pro_imei.Text;
                rpp.ParamList.Add(bt);
            }
            if (!string.IsNullOrEmpty(this.vipimei.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "VIP_IMEI";
                bt.ParamValues = this.vipimei.Text;
                rpp.ParamList.Add(bt);
            }

            if (!string.IsNullOrEmpty(this.cus_name.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Cus_Name";
                bt.ParamValues = this.cus_name.Text;
                rpp.ParamList.Add(bt);
            }

            if (!string.IsNullOrEmpty(this.cus_phone.Text))
            {
                API.ReportSqlParams_String bt = new API.ReportSqlParams_String();
                bt.ParamName = "Cus_Phone";
                bt.ParamValues = this.cus_phone.Text;
                rpp.ParamList.Add(bt);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 321, new object[] { rpp }, new EventHandler<API.MainCompletedEventArgs>(SearchCompleted));

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
                if (pageParam == null) { return; }
                List<API.View_ASPCurrentOrderInfo> list = pageParam.Obj as List<API.View_ASPCurrentOrderInfo>;
                if (list == null) { return; }
                models.Clear();
                models.AddRange(list);
                RepairGrid.Rebind();
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
                RepairGrid.Rebind();
            }

        }

        private void radDataPager1_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                pageIndex = e.NewPageIndex;
                Search();
            }
        }


        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                radDataPager1.PageSize = (int)pagesize.Value;
                Search();
            }
        }

        #endregion

        #region 详情

        private void RepairGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (RepairGrid.SelectedItems.Count == 0)
            {
                return;
            }

            API.View_ASPCurrentOrderInfo model = RepairGrid.SelectedItem as API.View_ASPCurrentOrderInfo;
           
            repCount.Text = model.RepairCount.ToString();

            limitPrice.Text = model.Chk_Price.ToString();
            OldID.Text = model.OldID;
            hallname.Text = model.HallName;
            
            foreach (var item in chk_InOut.ItemsSource)
            {
                if ((item as SlModel.CkbModel).Text == model.Chk_InOut)
                {
                    chk_InOut.SelectedItem = item;
                    break;
                }

            }

            List<SlModel.CkbModel> list = repKind.ItemsSource as List<SlModel.CkbModel>;
            if (model.RepKind != null)
            {
                repKind.SelectedItem = list.Where(a => a.Text == model.RepKind).First();
            }
            else
            {
                repKind.SelectedItem = list.Where(a => a.Text == "软件升级").First();
            }

            chk_price.Text = model.Chk_Price == null ? "" : model.Chk_Price.ToString();
            zjNote.Text = model.ZJNote;
            repCount.Text = model.RepairCount.ToString();
            receiver.Text = model.Receiver;
            bj_user.Text = model.BJ_UserID;
            bj_money.Value = Convert.ToDouble(model.BJ_Money);
            bj_date.Text = model.BJ_Date==null? DateTime.Now.ToShortDateString():((DateTime)model.BJ_Date).ToShortDateString();
            if (model.HasBJ==true)
            {
                bj_money.IsEnabled = false;
                bj_user.IsReadOnly = true;
                bjHall.btnSearch.IsEnabled = false;
                //bjHall.TextBox.IsEnabled = false;
                addBJ.IsEnabled = false;
                delBJ.IsEnabled = false;
                //已经备机则获取备机信息
                //PublicRequestHelp roh = new PublicRequestHelp(null,323,new object[]{},
                //    new EventHandler<API.MainCompletedEventArgs>(GetBJInfoCompleted));
            }
            else
            {
                bj_money.IsEnabled = true;
                bj_user.IsReadOnly = false;
                bjHall.btnSearch.IsEnabled = true;
                //bjHall.TextBox.IsEnabled = true;
                addBJ.IsEnabled = true;
                delBJ.IsEnabled = true;
            }
            
            PublicRequestHelp peh = new PublicRequestHelp(this.isbusy,322,new object[]{model.ID,model.HasBJ},
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            errinfo.Clear();
            if (e.Result.ReturnValue)
            {
                errinfo.AddRange(e.Result.Obj as List<API.ASP_ErrorInfo>);
                oldErrGrid.ItemsSource = errinfo;
                newErrGrid.ItemsSource = errinfo;
                oldErrGrid.Rebind();
                newErrGrid.Rebind();

                List<API.View_BJModels> list = e.Result.ArrList[0] as List<API.View_BJModels>;
                bjModels.Clear();
                bjModels.AddRange(list);
                bjGrid.Rebind();

                List<API.View_ASPCurrentOrderPros> list2 = e.Result.ArrList[1] as List<API.View_ASPCurrentOrderPros>;
                this.proModels.Clear();
                lackProModels.Clear();

                List<API.CHKModel> chklist = new List<API.CHKModel>() {  new  API.CHKModel(){ Text="配件"},
                new  API.CHKModel(){ Text="机头"}};
                //new  API.CHKModel(){ Text="主板"}};

                foreach (var item in list2)
                {
                    item.CHKList = new List<API.CHKModel>();
                    item.CHKList.AddRange(chklist);
                    if (item.IsLack == true)
                    {
                        lackProModels.Add(item);
                    }
                    else
                    {
                        proModels.Add(item);
                    }

                }
                prosGrid2.Rebind();
                prosGrid.Rebind();
            }

        }

        #endregion

        #region  故障操作

        /// <summary>
        /// 添加故障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addErr_Click(object sender, RoutedEventArgs e)
        {
            if (RepairGrid.SelectedItem == null)
            {
                MessageBox.Show("请选维修单！");
                return;
            }
            List<TreeViewModel> list = new List<TreeViewModel>();
            foreach (var item in Store.ErrorTypes)
            {
                TreeViewModel p = new TreeViewModel();
                p.Fields = new string[] { "TypeID" };
                p.Values = new object[] { item.ID };
                p.NewID = item.ID;
                p.Title = item.Name;
                list.Add(p);

            }

            MultSelecter2 msFrm = new MultSelecter2(
              list,
              Store.ErrorInfo, "ErrorName",
              new string[] { "ErrorID", "ErrorName" },
              new string[] { "编码", "故障名称" });
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();
        }

        private void msFrm_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            List<API.ASP_ErrorInfo> piList = ((UserMS.MultSelecter2)sender).SelectedItems.OfType<API.ASP_ErrorInfo>().ToList();
            if (piList.Count == 0) return;

            foreach (var item in piList)
            {
                var p = from a in errinfo
                        where a.ID == item.ID
                        select a;
                if (p.Count() == 0)
                {
                    errinfo.Add(item);
                }
              
            }
            this.newErrGrid.Rebind();
        }

        /// <summary>
        /// 删除故障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delErr_Click(object sender, RoutedEventArgs e)
        {
            if (newErrGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需删除的数据！");
                return;
            }

            foreach (var item in newErrGrid.SelectedItems)
            {
                errinfo.Remove(item as API.ASP_ErrorInfo);
            }
            newErrGrid.Rebind();
        }

        #endregion 

        #region 配件caozuo

        #region 缺料配件

        private void addLackPros_Click(object sender, RoutedEventArgs e)
        {
            if (RepairGrid.SelectedItem == null)
            {
                MessageBox.Show("请选维修单！");
                return;
            }
            this.LackAdder.Add(new string[] { "ClassName", "TypeName", "ProName", "ProFormat", "NeedIMEI"},
               new string[] { "商品品牌", "商品类别", "商品型号", "商品属性", "需要串码" });
        }

        private void delLackClick_Click(object sender, RoutedEventArgs e)
        {
            if (prosGrid2.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需删除的商品！");
                return;
            }
            foreach (var item in prosGrid2.SelectedItems)
            {
                this.lackProModels.Remove(item as API.View_ASPCurrentOrderPros);
            }
            prosGrid2.Rebind();
           
        }

        private void AddLackProsCompleted(object sender, MyEventArgs e)
        {
            foreach (var item in lackProModels)
            {
                item.IsLack = true;
                item.ProCount = 1;
            }
        }

        #endregion 

        
        private void addPros_Click(object sender, RoutedEventArgs e)
        {
            if (adder == null)
            {
                adder = new ProDetailAdder<API.View_ASPCurrentOrderPros>(proModels, prosGrid, new List<API.BJModel>(),
                    new EventHandler<MyEventArgs>(AddProsCompleted));
            }
            if (RepairGrid.SelectedItem == null)
            {
                MessageBox.Show("请选维修单！");
                return;
            }
            adder.Add();
        }

        /// <summary>
        /// 添加商品完成后获取其单卖价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProsCompleted(object sender, MyEventArgs e)
        {
            List<API.CHKModel> list = new List<API.CHKModel>() {  new  API.CHKModel(){ Text="配件"},
                new  API.CHKModel(){ Text="机头"},
                //new  API.CHKModel(){ Text="主板"}
            };

            foreach (var item in proModels)
            {
                if (string.IsNullOrEmpty(item.TName))
                {
                    item.TName = "配件";
                    item.ProCount = 1;
                    item.CHKList = new List<API.CHKModel>();
                    item.CHKList.AddRange(list);
                }
            }
            prosGrid.Rebind();
            if ((chk_InOut.SelectedItem as SlModel.CkbModel).Text == "保外")
            {
                GetProPrice();
            }
        }

        private void GetProPrice()
        {
            List<string> proids = new List<string>();

            foreach (var item in proModels)
            {
                if (item.ID == 0)
                {
                    item.ID = ++index;
                }
                item.IsHeader = false;
                item.NotHeader = false;
                proids.Add(item.ProID);
            }
            API.WebReturn ret = Store.wsclient.Main(323, new List<object>() { proids });

            if (ret.ReturnValue)
            {
                List<API.Pro_SellTypeProduct> list = ret.Obj as List<API.Pro_SellTypeProduct>;
                if (list != null)
                {
                    decimal total = 0;
                    foreach (var item in list)
                    {
                        total += item.Price;
                    }
                    total = Decimal.Truncate(Convert.ToDecimal(total * 100)) / 100;
                    proMoney.Tag = total;
                    proMoney.Text = total.ToString();

                    totalMoney.Text = (workMoney.Value + (double)total).ToString();
                }
            }
        }

        private void delClick_Click(object sender, RoutedEventArgs e)
        {
            if (prosGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择需删除的商品！");
                return;
            }
            foreach (var item in prosGrid.SelectedItems)
            {
                proModels.Remove(item as API.View_ASPCurrentOrderPros);
            }
            prosGrid.Rebind();
            GetProPrice();
        }

        #endregion 

        #region 保存

        private void save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lackSave"></param>
        private void Save()
        {

            if (RepairGrid.SelectedItem == null)
            {
                MessageBox.Show("请选择需要保存的数据！");
                return;
            }


            API.View_ASPCurrentOrderInfo order = RepairGrid.SelectedItem as API.View_ASPCurrentOrderInfo;
            API.ASP_RepairInfo rep = new API.ASP_RepairInfo();

            rep.RepKind = (repKind.SelectedItem as SlModel.CkbModel).Text.ToString();
            rep.Chk_InOut = (this.chk_InOut.SelectedItem as SlModel.CkbModel).Text.ToString();
            rep.OldID = order.OldID;
            rep.RepairCount = order.RepairCount;
            rep.RepairerHallID = proHall.Tag.ToString();
            bool isToFact = (repType.SelectedItem as ComboBoxItem).Tag.ToString() == "1" ? true : false;
            rep.Chk_RType = (repType.SelectedItem as ComboBoxItem).Content.ToString();

            if (order.CurrentRepairID>0)
            {
                rep.ID = Convert.ToInt32(order.CurrentRepairID);
            }
            if (!isToFact)  //自行维修
            {
                #region 验证数据

                if (errinfo.Count == 0)
                {
                    MessageBox.Show("请添加故障信息！");
                    return;
                }
                if ((this.chk_InOut.SelectedItem as SlModel.CkbModel).Text.ToString() == "保内")
                {
                    if (workMoney.Value == 0)
                    {
                        MessageBox.Show("人工费用不能为0！");
                        return;
                    }
                }

                foreach (var item in proModels)
                {
                    if (string.IsNullOrEmpty(item.IMEI)&& item.NeedIMEI)
                    {
                        MessageBox.Show("请添加配件串码！");
                        return;
                    }
                    //if (string.IsNullOrEmpty(item.OldIMEI) && item.NeedIMEI)
                    //{
                    //    MessageBox.Show("请添加配件旧串码！");
                    //    return;
                    //}
                }

                #endregion

                #region 添加故障信息
                rep.ASP_CurrentOrder_ErrorInfo = new List<API.ASP_CurrentOrder_ErrorInfo>();
                foreach (var item in errinfo)
                {
                    API.ASP_CurrentOrder_ErrorInfo er = new API.ASP_CurrentOrder_ErrorInfo();
                    er.ErrorID = item.ID;
                    
                    rep.ASP_CurrentOrder_ErrorInfo.Add(er);
                }
                #endregion

                #region 添加配件信息
                if ((repKind.SelectedItem as SlModel.CkbModel).Text.ToString() == "更换配件")
                {
                    if (proModels.Count == 0)
                    {
                        MessageBox.Show("请添加配件信息！"); return;
                    }
                    foreach (var item in proModels)
                    {
                        if (item.NeedIMEI && string.IsNullOrEmpty(item.OldIMEI))
                        {
                            MessageBox.Show("商品 "+item.ProName+" 的旧串码不能为空！");
                            return;
                        }
                    }
                }
                rep.ASP_CurrentOrder_Pros = new List<API.ASP_CurrentOrder_Pros>();
                foreach (var item in proModels)
                {
                    API.ASP_CurrentOrder_Pros p = new API.ASP_CurrentOrder_Pros();
                 
                   // p.IsHeader = item.IsHeader; //是否是机身串码
                    p.TName = item.TName;
                    p.OrderID = order.ID;
                    p.ID = item.ID;
                    if (item.NeedIMEI)
                    {
                        p.OldIMEI = item.OldIMEI;
                        p.IMEI = item.IMEI;
                    }
                    p.InListID = item.InListID;
                    p.ProCount = item.ProCount;
                    p.ProID = item.ProID;
                    rep.ASP_CurrentOrder_Pros.Add(p);
                }
                foreach (var item in lackProModels)
                {
                    API.ASP_CurrentOrder_Pros p = new API.ASP_CurrentOrder_Pros();
                    p.OrderID = order.ID;
                    p.TName = item.TName;
                    p.ProCount = item.ProCount;
                    p.ProID = item.ProID;
                    p.ID = item.ID;
                    p.IsLack = item.IsLack;
                    rep.ASP_CurrentOrder_Pros.Add(p);
                }
                #endregion

                rep.LackNote = qlNote.Text;
                rep.HasRepaired = true;
                rep.NeedToFact = false;
                //费用信息
                rep.WorkMoney = Convert.ToDecimal(workMoney.Value);
                rep.ProMoney = Convert.ToDecimal(string.IsNullOrEmpty(proMoney.Text.ToString()) ? "0" : proMoney.Text.ToString());
                //rep.Price = Convert.ToDecimal(price.Value);
                //rep.ShouldPay = rep.WorkMoney + rep.ProMoney;
                //rep.RealPay = rep.WorkMoney + rep.ProMoney - rep.BJ_Money;
            }
            else          //送厂维修
            {
                rep.NeedToFact = true;
            }
           
            rep.OrderID = order.ID;
            rep.OrderHallID = order.HallID;
            rep.RepairNote = repairNote.Text;
            //rep.Repairer = Store.LoginUserInfo.UserID;

            #region 添加备机信息

            rep.ASP_CurrentOrder_BackupPhoneInfo = new List<API.ASP_CurrentOrder_BackupPhoneInfo>();
            rep.BJHallID = bjHall.Tag.ToString();
            if (order.HasBJ == false)
            {
                rep.BJUserID = bj_user.Text;
                rep.BJ_Money = Convert.ToDecimal(bj_money.Value);
                foreach (var item in bjModels)
                {
                    API.ASP_CurrentOrder_BackupPhoneInfo m = new API.ASP_CurrentOrder_BackupPhoneInfo();
                    m.IMEI = item.IMEI;
                    //m.NeedIMEI = item.NeedIMEI;
                    m.InListID = item.InListID;
                    m.ProCount = 1;
                    m.ProID = item.ProID;
                    rep.ASP_CurrentOrder_BackupPhoneInfo.Add(m);
                }
            }
            #endregion


            if (MessageBox.Show("确定保存吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 324, new object[] { rep },
                new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                MessageBox.Show(e.Result.Message);
                Clear();
                Search();
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }
        }

        #endregion 

        private void workMoney_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            decimal total = (decimal)workMoney.Value + Convert.ToDecimal(string.IsNullOrEmpty(proMoney.Text.ToString()) ? "0" : proMoney.Text.ToString());
            totalMoney.Text = string.Format("{0:0000.0000}", total.ToString());
        }

        void Clear()
        {
            limitPrice.Text = string.Empty;
            zjNote.Text = string.Empty;
            OldID.Text = string.Empty;
            repCount.Text = string.Empty;
           // chkinout.Text = string.Empty;
            hallname.Text = string.Empty;
            repairNote.Text = string.Empty;
            receiver.Text = string.Empty;
            qlNote.Text = string.Empty;
            workMoney.Value = 0;
           // price.Value = 0;
            totalMoney.Text = string.Empty;
            proMoney.Text = string.Empty;

            proModels.Clear();
            prosGrid.Rebind();

            oldErrGrid.ItemsSource = null;
            oldErrGrid.Rebind();
            errinfo.Clear();
            newErrGrid.Rebind();

            bj_money.Value = 0;
            bjModels.Clear();
            bjGrid.Rebind();
            bj_user.Text = string.Empty;

            lackProModels.Clear();
            prosGrid2.Rebind();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            int id = Convert.ToInt32(cb.Tag??0);

            foreach (var item in proModels)
            {
                if (item.ID == id)
                {
                    item.NotHeader = true;
                    item.IsHeader = false;
                    break;
                }
            }
            prosGrid.Rebind();
        }

        private void header_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            int id = Convert.ToInt32(cb.Tag ?? 0);
            foreach (var item in proModels)
            {
                if (item.ID == id)
                {
                    item.IsHeader = true;
                    item.NotHeader = false;
                    break;
                }
            }
            prosGrid.Rebind();
        }

        private void chk_InOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((chk_InOut.SelectedItem as SlModel.CkbModel).Text == "保外")
            {
                GetProPrice();
            }
            else
            {
                proMoney.Tag = 0;
                proMoney.Text = "";

                totalMoney.Text = workMoney.Value.ToString();
            }

        }

        private void header_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            
            API.View_ASPCurrentOrderInfo model = RepairGrid.SelectedItem as API.View_ASPCurrentOrderInfo;

            API.CHKModel itemx = comboBox.SelectedItem as API.CHKModel;
            if (itemx == null) { return; }
            foreach (var item in proModels)
            {
                if (itemx.Text == item.TName)
                {
                    item.TName = "配件";
                    return;
                }
            }

            foreach (var item in proModels)
            {
                 if (comboBox.Tag == null) { continue; }
                if ((item.IMEI+"").ToUpper() == comboBox.Tag.ToString())
                {
                    if (itemx.Text == "主板")
                    {
                        item.OldIMEI = model.Pro_IMEI;
                    }
                    else if (itemx.Text == "机头")
                    {
                        item.OldIMEI = model.Pro_HeaderIMEI;
                    }
                    else
                    {
                        item.OldIMEI = "";
                    }
                    break;
                }
            }
            //prosGrid.Rebind();
        }

        private void repKind_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false) { return; }
            ComboBox cb = sender as ComboBox;
            addPros.IsEnabled = false;
            delClick.IsEnabled = false;
            if (cb.SelectedValue == null) { return; }
            if (cb.SelectedValue.ToString() == "更换配件")
            {
                addPros.IsEnabled = true;
                delClick.IsEnabled = true;
            }
        }

    }
}
