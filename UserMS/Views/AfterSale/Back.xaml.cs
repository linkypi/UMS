using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using Telerik.Windows.Controls;
using UserMS.Common;
using UserMS.Model;
using UserMS.MyControl;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// Back.xaml 的交互逻辑
    /// </summary>
    public partial class Back : Page
    {
        int pageIndex = 0;
        bool flag = false;
        private ROHallAdder hadder = null;

        private int menuid = 345;
        List<API.ASP_ErrorInfo> errInfo = new List<API.ASP_ErrorInfo>();
        List<Model.View_ASPRepairInfo> tmpModels = new List<Model.View_ASPRepairInfo>();
        List<API.View_ASPCurrentOrderPros> pros = new List<API.View_ASPCurrentOrderPros>();

        public Back()
        {
            InitializeComponent();
           // hadder = new HallFilter(false, ref hall);
            //List<API.Pro_HallInfo> halls = hadder.FilterHall(menuid, Store.ProHallInfo);
            //if (halls.Count != 0)
            //{
            //    this.hall.Tag = halls.First().HallID;
            //    this.hall.TextBox.SearchText = halls.First().HallName;
            //}
            //this.hall.SearchButton.Click += SearchButton_Click;
            //backSelectNote.SearchButton.Click+=SearchNoteBtn_Click;
            hadder = new ROHallAdder(ref this.hall, menuid);

            searchGrid.ItemsSource = tmpModels;
            radDataPager1.PageSize = 20; flag = true;
            Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);


            List<API.ASP_CheckInfo> list = new List<API.ASP_CheckInfo>();
            list.AddRange(Store.CheckInfo);
            list.Add(new API.ASP_CheckInfo() { ChkName = "全部", ID = 0 });
            chk_fid.ItemsSource = list;
            chk_fid.SelectedIndex = list.Count-1;

            fac_search.SearchEvent += fac_Click;
            //backSelectNote.SearchButton.Click += SearchNoteBtn_Click;

            //var cell = searchGrid.Columns[15] as GridViewDataColumn;

            //StackPanel panel = (StackPanel)cell.CellTemplate.LoadContent();
            //MyTextBox ckbfac = panel.FindName("backNote") as MyTextBox;

            //ckbfac.SearchButton.Click += backNote_Click;
           
        }

        #region 返厂备注
        private void addBackNote_Click(object sender, RoutedEventArgs e)
        {
            List<TreeViewModel> childs = new List<TreeViewModel>() { 
             new TreeViewModel(){ Title="修复",Fields=new string[]{"NewID"}, NewID=1,Values=new object[]{1}},
              new TreeViewModel(){ Title="更换",Fields=new string[]{"NewID"}, NewID=2,Values=new object[]{2}}
            };
            //生成左边树
            List<RepairNote> list = new List<RepairNote>();
            list.AddRange(RepairNote.Generate(
            "修复主机,修复主板,修复LCD,修复听筒,修复麦克风,修复排线,修复电池,修复充电头,修复数据线,修复触屏,修复其他", 1));
            list.AddRange(RepairNote.Generate(
            "更换主机,更换主板,更换LCD,更换听筒,更换麦克风,更换排线,更换电池,更换充电头,更换数据线,更换触屏,更换其他", 2));

            MultSelecter2 msFrm = new MultSelecter2(
             childs, list, "Note",
             new string[] { "ID", "Note" },
             new string[] { "编码", "备注" });
            msFrm.Tag = (sender as Button).Tag;
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();
        }

      
        private void fac_Click(object sender, RoutedEventArgs e)
        {
            MultSelecter2 msFrm = new MultSelecter2(
             null, Store.Factorys, "FacName",
             new string[] { "FacID", "FacName" },
             new string[] { "厂家编码", "厂家名称" });
            msFrm.Closed += msFrm_Closed2;
            msFrm.ShowDialog();
        }

        private void msFrm_Closed2(object sender, WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;
            //backSelectNote.TextBox.SearchText = string.Empty;
            if (selecter.DialogResult == true)
            {
                List<API.ASP_Factory> phList = selecter.SelectedItems.OfType<API.ASP_Factory>().ToList();
                string msg = "";
                int index = 1;
                string names = "";
                foreach (var item in phList)
                {
                    msg += item.ID;
                    names += item.FacName;
                    if (index < phList.Count)
                    {
                        msg += " , ";
                        names += " , ";
                    }
                    index++;
                }
                fac_search.Text = names;
                fac_search.Tag = msg;

            }
        }
   
        private void msFrm_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;
            //backSelectNote.TextBox.SearchText = string.Empty;
            if (selecter.DialogResult == true)
            {
                List<RepairNote> phList = selecter.SelectedItems.OfType<RepairNote>().ToList();
                string msg = "";
                int index = 1;
                foreach (var item in phList)
                {
                    msg += item.Note;
                    if (index < phList.Count)
                    {
                        msg += " , ";
                    }
                    index++;
                }

                foreach (var item in tmpModels)
                {
                    if (item.ID.ToString() == (selecter.Tag ?? "").ToString())
                    {
                        item.BackNote += msg; break;
                    }
                }
                searchGrid.Rebind();
               // backSelectNote.TextBox.SearchText = msg;

            }
        }

        #endregion 

        #region 查询

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);
        }

        private void Ckb_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);
            }
        }

        private void Search(int pagesize, int pageIndex, EventHandler<API.MainCompletedEventArgs> EvenCompleted)
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageSize = pagesize;
            rpp.PageIndex = pageIndex;

            rpp.ParamList = new List<API.ReportSqlParams>();

            //API.ReportSqlParams_Bool NeedToFact = new API.ReportSqlParams_Bool();
            //NeedToFact.ParamName = "NeedToFact";
            //NeedToFact.ParamValues = true;
            //rpp.ParamList.Add(NeedToFact);

            //API.ReportSqlParams_Bool bto = new API.ReportSqlParams_Bool();
            //bto.ParamName = "IsToFact";
            //bto.ParamValues = true;
            //rpp.ParamList.Add(bto);

            if (!string.IsNullOrEmpty(fac_search.Text))
            {
                API.ReportSqlParams_ListString fac = new API.ReportSqlParams_ListString();
                fac.ParamName = "FactName";
                fac.ParamValues = (fac_search.Tag ?? "").ToString().Split(",".ToCharArray()).ToList();
                rpp.ParamList.Add(fac);
            }
            API.ASP_CheckInfo chkmodel = this.chk_fid.SelectedItem as API.ASP_CheckInfo;
            if (chkmodel != null)
            {
                if (chkmodel.ID != 0)
                {
                    API.ReportSqlParams_String fid = new API.ReportSqlParams_String();
                    fid.ParamName = "CHK_FID";
                    fid.ParamValues = (this.chk_fid.SelectedItem as API.ASP_CheckInfo).ID.ToString();
                    rpp.ParamList.Add(fid);
                }
            }

            ////0 未返厂 
            //bool istoback = (this.isBack.SelectedItem as ComboBoxItem).Tag.ToString() == "0" ? true : false;
            //if (istoback)
            //{
            //    API.ReportSqlParams_Bool back = new API.ReportSqlParams_Bool();
            //    back.ParamName = "IsBack";
            //    back.ParamValues = false;
            //    rpp.ParamList.Add(back);
            //}
            //else  //已返厂
            //{
            //    API.ReportSqlParams_Bool back = new API.ReportSqlParams_Bool();
            //    back.ParamName = "IsBack";
            //    back.ParamValues = true;
            //    rpp.ParamList.Add(back);
            //}

            if (state.SelectedIndex != 0)
            {
                API.ReportSqlParams_String repstate = new API.ReportSqlParams_String();
                repstate.ParamName = "RpState";
                object obj = (state.SelectedItem as ComboBoxItem).Content;
                repstate.ParamValues = obj == null ? "" : obj.ToString();
                rpp.ParamList.Add(repstate);
            }

            if (!string.IsNullOrEmpty(this.hall.Tag == null ? "" : hall.Tag.ToString()))
            {
                API.ReportSqlParams_String h = new API.ReportSqlParams_String();
                h.ParamName = "HallID";
                h.ParamValues = this.hall.Tag.ToString();
                rpp.ParamList.Add(h);
            }

            //if (!string.IsNullOrEmpty(this.sysdate.DateTimeText??""))
            //{
            //    API.ReportSqlParams_DataTime date = new API.ReportSqlParams_DataTime();
            //    date.ParamName = "SysDate";
            //    date.ParamValues = this.sysdate.SelectedDate;
            //    rpp.ParamList.Add(date);
            //}

            //if (!string.IsNullOrEmpty(this.oldid.Text.ToString()))
            //{
            //    API.ReportSqlParams_String users = new API.ReportSqlParams_String();
            //    users.ParamName = "OldID";
            //    users.ParamValues = this.oldid.Text.Trim();
            //    rpp.ParamList.Add(users);
            //}
            if (!string.IsNullOrEmpty(this.oldids.Text.ToString()))
            {
                API.ReportSqlParams_ListString users = new API.ReportSqlParams_ListString();
                users.ParamName = "OldIDS";
                users.ParamValues = this.oldids.Text.Trim().Split("\n\r".ToCharArray()).ToList();
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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 325, new object[] { rpp }, EvenCompleted);

        }

        private void Clear()
        {
            //backInlistID.Text = string.Empty;
            //newIMEI.Text = string.Empty;
            //newSN.Text = string.Empty;
            repairer.Text = string.Empty;
            chk_InOut.Text = string.Empty;
            repairHall.Text = string.Empty;
            repairNote.Text = string.Empty;
           // backSelectNote.TextBox.SearchText = string.Empty;

            newErrGrid.ItemsSource = null ;
            newErrGrid.Rebind();

            //配件
            prosGrid.ItemsSource = null;
            prosGrid.Rebind();

            //e.Result.ArrList[2]  受理故障
            oldErrGrid.ItemsSource = null;
            oldErrGrid.Rebind();
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
                List<API.View_ASPRepairInfo> list = pageParam.Obj as List<API.View_ASPRepairInfo>;
                if (list == null) { return; }
    
                tmpModels.Clear();
                foreach (var item in list)
                {
                    Model.View_ASPRepairInfo va = new View_ASPRepairInfo();
                    va.RepKind       =item.RepKind      ;
                    va.RpState       =item.RpState      ;
                    va.Dispatcher    =item.Dispatcher   ;
                    va.HasDispatch   =item.HasDispatch;
                    va.维修完成时间  =item.维修完成时间 ;
                    va.接机日期      =item.接机日期;
                    va.质检日期      =item.质检日期;
                    va.审核日期      =item.审核日期;
                    va.取机日期      =item.取机日期;
                    va.回访日期      =item.回访日期;
                    va.审计日期      =item.审计日期;
                    va.撤销时间     = item.撤销时间;  
                    va.送厂时间      =item.送厂时间     ;
                    va.返厂日期     = item.返厂日期;
                    va.Position      =item.Position     ;
                    va.撤销人        =item.撤销人       ;
                    va.撤销备注      =item.撤销备注     ;
                
                    va.质检人        =item.质检人       ;
                    va.需送厂        =item.需送厂       ;
                
                    va.送厂人        =item.送厂人       ;
                    va.送厂批次      =item.送厂批次     ;
                    va.厂家名称      =item.厂家名称     ;
                  
                    va.返厂批次      =item.返厂批次     ;
                    va.返厂备注      =item.返厂备注     ;
                    va.返厂人        =item.返厂人       ;
                    va.质检备注      =item.质检备注     ;
                    va.劳务费        =Convert.ToDecimal(item.劳务费  )    ;
                    va.配件费        =Convert.ToDecimal(item.配件费  )    ;
                    va.应收          =Convert.ToDecimal(item.应收    )    ;
                    va.实收          =Convert.ToDecimal(item.实收    )    ;
                    va.备机押金      =Convert.ToDecimal(item.备机押金)     ;
                    va.挂账金额     = Convert.ToDecimal(item.挂账金额);
                    va.审计金额     = Convert.ToDecimal(item.审计金额);
                    va.结算金额     = Convert.ToDecimal(item.结算金额);
                    va.审核人        =item.审核人       ;
                    va.审核备注      =item.审核备注     ;
                    va.取机人        =item.取机人       ;
                    va.取机备注      =item.取机备注     ;
              
                    va.挂账类型      =item.挂账类型     ;
                    va.回访人        =item.回访人       ;
                    va.审计人        =item.审计人       ;
             
                    va.审计备注      =item.审计备注     ;
                    va.ChangPros     =item.ChangPros    ;
                    va.Chk_RType     =item.Chk_RType    ;
                    va.Cus_Name = item.Cus_Name;
                    va.ServiceID = item.ServiceID;
                    va.BackNote = "";
                    va.Pro_Type = item.Pro_Type;
                    va.BackInListID = "";
                    va.Pro_Name = item.Pro_Name;
                    va.Pro_Color = item.Pro_Color;
                    va.OldID = item.OldID;
                    va.Cus_Name = item.Cus_Name;
                    va.Cus_Phone = item.Cus_Phone;
                    va.IMEI = item.IMEI;
                    va.AuditDate = item.AuditDate;
                    va.AuditLowMoney = item.AuditLowMoney;
                    va.AuditMoney = item.AuditMoney;
                    va.AuditNote = item.AuditNote;
                    va.AuditUserID = item.AuditUserID;
                    va.BJ_Money = item.BJ_Money;
                    va.BJDate = item.BJDate;
                    va.BJHallID = item.BJHallID;
                    va.RpState = item.RpState;
                    va.BJHallName = item.BJHallName;
                    va.BJUser = item.BJUser;
                    va.BuyDate = item.BuyDate;
                    va.RecHallName = item.RecHallName;
                    va.Pro_HeaderIMEI = item.Pro_HeaderIMEI;
                    va.SysDate = item.SysDate;
                    va.ID = item.ID;
                    va.Repairer = item.Repairer;
                    va.RepairNote = item.RepairNote;
                    va.RecHallName = item.RecHallName;
                    va.Chk_InOut = item.Chk_InOut;
                    va.FacName = item.FacName;
                    va.RepKind = item.RepKind;
                    va.FacInListID = item.FacInListID;
                    tmpModels.Add(va);
                }
                searchGrid.Rebind();
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
                tmpModels.Clear();
                searchGrid.Rebind();
            }

        }

        private void radDataPager1_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                pageIndex = e.NewPageIndex;
                Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);
            }
        }


        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            if (radDataPager1 != null)
            {
                radDataPager1.PageSize = (int)pagesize.Value;
                Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);
            }
        }

        #endregion

        #region 详情

        private void searchGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                return;
            }

            Model.View_ASPRepairInfo model = searchGrid.SelectedItem as Model.View_ASPRepairInfo;
            //if (model.IsToFact == true)
            //{
            //    backSelectNote.TextBox.IsEnabled = false;
            //    backSelectNote.SearchButton.IsEnabled = false;
            //}
            //else
            //{
            //    backSelectNote.TextBox.IsEnabled = true;
            //    backSelectNote.SearchButton.IsEnabled = true;
            //}
            facinlistid.Text = model.FacInListID;
            facName.Text = model.FacName;
            repairer.Text = model.Repairer;
            repairNote.Text = model.RepairNote;
            repairHall.Text = model.RecHallName;
            this.chk_InOut.Text = model.Chk_InOut;

            PublicRequestHelp peh = new PublicRequestHelp(this.isbusy, 326, new object[] { model.ID },
                new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            errInfo.Clear();
            if (e.Result.ReturnValue)
            {

                List<API.ASP_ErrorInfo> list1 = e.Result.Obj as List<API.ASP_ErrorInfo>;
                newErrGrid.ItemsSource = list1;
                newErrGrid.Rebind();

                //配件
                List<API.View_ASPCurrentOrderPros> list = e.Result.ArrList[0] as List<API.View_ASPCurrentOrderPros>;
                prosGrid.ItemsSource = list;
                prosGrid.Rebind();

                //e.Result.ArrList[1]  备机
                // List<API.View_BJModels> bjlist = e.Result.ArrList[1] as List<API.View_BJModels>;
                //bjGrid.ItemsSource = bjlist;
                //bjGrid.Rebind();

                //e.Result.ArrList[2]  受理故障
                List<API.ASP_ErrorInfo> list2 = e.Result.ArrList[2] as List<API.ASP_ErrorInfo>;
                oldErrGrid.ItemsSource = list2;
                oldErrGrid.Rebind();
            }

        }

        #endregion

        #region  保存

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                Clear();
                if (MessageBox.Show("保存成功，是否导出清单？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    List<API.View_ASPRepairInfo> list = e.Result.Obj as List<API.View_ASPRepairInfo>;
                    Export(list);
                }

                Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);
            }
            else
            {
                MessageBox.Show(e.Result.Message);
            }
        }

        /// <summary>
        /// 返厂
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backFac_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择数据！");
            }

            List<API.ASP_RepairInfo> list = new List<API.ASP_RepairInfo>();
            foreach (var item in searchGrid.SelectedItems)
            {
                Model.View_ASPRepairInfo rep = item as Model.View_ASPRepairInfo;
                if (rep.RpState != "待返厂")
                {
                    MessageBox.Show("单号 " + rep.OldID + "处于" + rep.RpState + "状态，无法返厂！");
                    return;
                }
                if (string.IsNullOrEmpty(rep.BackInListID))
                {
                    MessageBox.Show("请为受理单 " + rep.ServiceID + " 输入返厂批号！"); return;
                }
                //if (rep.RepKind != "退回")
                //{
                    //if (string.IsNullOrEmpty(rep.NewSN))
                    //{
                    //    MessageBox.Show("请为受理单 " + rep.ServiceID + " 输入新SN！");
                    //    return;
                    //}
                //}
           
                if (string.IsNullOrEmpty(rep.BackNote))
                {
                    MessageBox.Show("请为受理单 " + rep.ServiceID + " 输入返厂备注！");
                    return;
                }
                //if (rep.BackNote.Contains("更换主板") || rep.BackNote.Contains("更换主机"))
                //{
                    //if (string.IsNullOrEmpty(rep.NewIMEI))
                    //{
                    //    MessageBox.Show("请为受理单 " + rep.ServiceID + " 输入新IMEI！");
                    //    return;
                    //}
                //}

                API.ASP_RepairInfo model = new API.ASP_RepairInfo();
                model.IsBack = true;
                model.BackNote = rep.BackNote;
                model.ID = rep.ID;
                model.BackInListID = rep.BackInListID;
                model.NewIMEI = rep.NewIMEI;
                model.NewSN = rep.NewSN;
                list.Add(model);
            }

            if (MessageBox.Show("确定返厂吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 328, new object[] { list }
                , new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        #endregion 

        #region 导出 

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void export_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (!flag) { return; }
            Search(50000, 1, GetSumCompleted);
        }

        private void GetSumCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;

                List<API.View_ASPRepairInfo> list = pageParam.Obj as List<API.View_ASPRepairInfo>;
                Export(list);

            }
            else
            {
                this.isbusy.IsBusy = false;
                MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message);
            }
        }

        private void Export(List<API.View_ASPRepairInfo> list)
        {
            SlModel.operateExcel<API.View_ASPRepairInfo> excel = new SlModel.operateExcel<API.View_ASPRepairInfo>();

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "xls";
            dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "xls");
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    List<string> strs = new List<string>() { 
                       "受理单号","手工单号","状态", "商品品牌","商品名称","商品属性",
                        "客户姓名","手机号码","会员卡号","服务网点","串码","SN码","受理日期",
                        "厂家名称", "送厂批号", "送厂备注","送厂人","送厂时间","返厂人" ,   "返厂批次",
                        "返厂备注" ,"返厂日期"
                        };

                    List<string> fields = new List<string>() {            
                        "ServiceID","OldID","RpState","Pro_Type","Pro_Name","ProFormat",
                        "Cus_Name","Cus_Phone","IMEI","RecHallName","Pro_HeaderIMEI"
                        ,"Pro_SN","SysDate","FacName","FacInListID","ToFacNote","送厂人","送厂时间",
                        "返厂人" ,   "返厂批次",  "返厂备注" ,"返厂日期"
                        };

                    excel.getExcel(list, strs, fields, stream);
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出完成！");
                    this.isbusy.IsBusy = false;
                }
            }
        }

        #endregion 


        /// <summary>
        /// 应用到所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void applyToAll_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Model.View_ASPRepairInfo tmp = null;
            foreach (var item in tmpModels)
            {
                if (item.ID.ToString() == (btn.Tag ?? "").ToString())
                {
                    tmp = item;
                    break;
                }
            }

            if (tmp != null)
            {
                foreach (var item in tmpModels)
                {
                    item.BackInListID = tmp.BackInListID;
                    item.BackNote = tmp.BackNote;
                }
                searchGrid.Rebind();
            }

        }

   
    }
}
