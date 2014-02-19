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
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.AfterSale
{
    /// <summary>
    /// ToFactory.xaml 的交互逻辑
    /// </summary>
    public partial class ToFactory : Page
    {
        int pageIndex = 0;
        bool flag = false;
        List<API.ASP_ErrorInfo> errInfo = new List<API.ASP_ErrorInfo>();
        List<API.View_ASPRepairInfo> models = new List<API.View_ASPRepairInfo>();
        List<API.View_ASPCurrentOrderPros> pros = new List<API.View_ASPCurrentOrderPros>();

        public ToFactory()
        {
            InitializeComponent();
            hadder = new ROHallAdder(ref this.hall, menuid);

            //hadder = new HallFilter(false, ref hall);
            //List<API.Pro_HallInfo> halls = hadder.FilterHall(menuid, Store.ProHallInfo);
            //if (halls.Count != 0)
            //{
            //    this.hall.Tag = halls.First().HallID;
            //    this.hall.TextBox.SearchText = halls.First().HallName;
            //}
            //this.hall.SearchButton.Click += SearchButton_Click;
            //backSelectNote.SearchButton.Click+=SearchNoteBtn_Click;

            List<API.ASP_CheckInfo> list = new List<API.ASP_CheckInfo>();
            list.AddRange(Store.CheckInfo);
            list.Add(new API.ASP_CheckInfo() { ChkName="全部",ID=0});
            chk_fid.ItemsSource = list;
            chk_fid.SelectedIndex = list.Count - 1;

             List<SlModel.CkbModel> list2 = new List<SlModel.CkbModel>(){
              new   SlModel.CkbModel(false,"未送厂"),
              new   SlModel.CkbModel(false,"返厂中"),
              new   SlModel.CkbModel(false,"已送回待质检")
             };
             isToFac.ItemsSource = list2;
             isToFac.SelectedIndex = 0;

            searchGrid.ItemsSource = models;
            radDataPager1.PageSize = 20; flag = true;
            Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);
        }

        private void SearchNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            List<TreeViewModel> childs = new List<TreeViewModel>() { 
             new TreeViewModel(){ Title="修复",Fields=new string[]{"NewID"}, NewID=1,Values=new object[]{1}},
              new TreeViewModel(){ Title="更换",Fields=new string[]{"NewID"}, NewID=2,Values=new object[]{2}}
            };
            //生成左边树
            List<RepairNote> list = new List<RepairNote>();
            list.AddRange(RepairNote.Generate(
            "修复主机,修复主板,修复LCD,修复听筒,修复麦克风,修复排线,修复电池,修复充电头,修复数据线,修复触屏,修复其他",1));
            list.AddRange(RepairNote.Generate(
            "更换主机,更换主板,更换LCD,更换听筒,更换麦克风,更换排线,更换电池,更换充电头,更换数据线,更换触屏,更换其他",2));
           
            MultSelecter2 msFrm = new MultSelecter2(
             childs, list, "Note",
             new string[] { "ID", "Note" },
             new string[] { "编码", "备注" } );
            msFrm.Closed += msFrm_Closed;
            msFrm.ShowDialog();
        }

        private void msFrm_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
 	         UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;
            // backSelectNote.TextBox.SearchText = string.Empty;
             if (selecter.DialogResult == true)
             {
                 List<RepairNote> phList = selecter.SelectedItems.OfType<RepairNote>().ToList();
                 string msg = "";
                 int index = 1;
                 foreach (var item in phList)
                 {
                     msg += item.Note ;
                     if (index < phList.Count)
                     {
                         msg += " , ";
                     }
                     index++;
                 }
                
                // backSelectNote.TextBox.SearchText = msg;

             }
        }

        private ROHallAdder hadder = null;

        private int menuid = 321;

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           // hadder.GetHall(hadder.FilterHall(menuid, Store.ProHallInfo));
        }

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

        private void Search(int pagesize,int pageIndex,EventHandler<API.MainCompletedEventArgs> EvenCompleted)
        {
            if (!flag) { return; }
            Clear();
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            rpp.PageSize = pagesize;
            rpp.PageIndex = pageIndex;

            rpp.ParamList = new List<API.ReportSqlParams>();

            API.ASP_CheckInfo chkmodel =  this.chk_fid.SelectedItem as API.ASP_CheckInfo;
            if (chkmodel.ID != 0)
            {
                API.ReportSqlParams_String fid = new API.ReportSqlParams_String();
                fid.ParamName = "CHK_FID";
                fid.ParamValues = (this.chk_fid.SelectedItem as API.ASP_CheckInfo).ID.ToString();
                rpp.ParamList.Add(fid);
            }

            API.ReportSqlParams_Bool NeedToFact = new API.ReportSqlParams_Bool();
            NeedToFact.ParamName = "NeedToFact";
            NeedToFact.ParamValues = true;
            rpp.ParamList.Add(NeedToFact);
            

            switch (this.isToFac.SelectedValue.ToString())
            {
                case "未送厂"://0 未送厂 

                    API.ReportSqlParams_Bool bto = new API.ReportSqlParams_Bool();
                    bto.ParamName = "IsToFact";
                    bto.ParamValues = false;
                    rpp.ParamList.Add(bto);
                    break;

                case "返厂中": //1 返厂中

                    API.ReportSqlParams_Bool bto2 = new API.ReportSqlParams_Bool();
                    bto2.ParamName = "IsToFact";
                    bto2.ParamValues = true;
                    rpp.ParamList.Add(bto2);

                    API.ReportSqlParams_Bool back = new API.ReportSqlParams_Bool();
                    back.ParamName = "IsBack";
                    back.ParamValues = false;
                    rpp.ParamList.Add(back);
                    break;

                case "已送回待质检":  //已送回待质检
                    API.ReportSqlParams_Bool back2 = new API.ReportSqlParams_Bool();
                    back2. ParamName = "IsBack";
                    back2.ParamValues = true;
                    rpp.ParamList.Add(back2);
                    break;
            }
         
        
            if (!string.IsNullOrEmpty(this.hall.Tag.ToString()))
            {
                API.ReportSqlParams_String hall = new API.ReportSqlParams_String();
                hall.ParamName = "HallID";
                hall.ParamValues = this.hall.Tag.ToString();
                rpp.ParamList.Add(hall);
            }

            //if (!string.IsNullOrEmpty(this.sysdate.DateTimeText??""))
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

            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 325, new object[] { rpp },EvenCompleted);

        }

        private void Clear()
        {

            this.facName.Text = string.Empty;
            this.toFacListID.Text = string.Empty;
        
            repairer.Text = string.Empty;
            chk_InOut.Text = string.Empty;
            repairHall.Text = string.Empty;
            repairNote.Text = string.Empty;

            newErrGrid.ItemsSource = null;
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

                List<API.View_ASPRepairInfo> list = pageParam.Obj as List<API.View_ASPRepairInfo>;
                if (list == null) { return; }
                models.Clear();
                models.AddRange(list);
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
                models.Clear();
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

        private void RepairGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                return;
            }

            API.View_ASPRepairInfo model = searchGrid.SelectedItem as API.View_ASPRepairInfo;
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
            repairer.Text = model.Repairer;
            repairNote.Text = model.RepairNote;
            repairHall.Text = model.RecHallName;
            this.chk_InOut.Text = model.Chk_InOut;
           

            this.repairer.Text = model.Repairer;

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

        /// <summary>
        /// 送厂
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toFac_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (searchGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择数据！");
            }
            if (string.IsNullOrEmpty(facName.Text))
            {
                MessageBox.Show("请输入厂家名称！");
                return;
            }
            if (string.IsNullOrEmpty(this.toFacListID.Text))
            {
                MessageBox.Show("请输入送厂批号！");
                return;
            }
            List<API.ASP_RepairInfo> list = new List<API.ASP_RepairInfo>();
            foreach (var item in searchGrid.SelectedItems)
            {
                API.ASP_RepairInfo model = new API.ASP_RepairInfo();
                API.View_ASPRepairInfo rep = item as API.View_ASPRepairInfo;
                model.ToUserID = Store.LoginUserInfo.UserID;
                model.IsToFact = true;
                model.ID = rep.ID;
                model.FacName = facName.Text.Trim();
                model.FacInListID = toFacListID.Text.Trim();
                list.Add(model);
            }
            if (MessageBox.Show("确定送厂吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy,327 ,new object[]{ list }
                ,new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }


        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                Clear();
               
                Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);
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

            if (string.IsNullOrEmpty(backInlistID.Text))
            {
                MessageBox.Show("请输入返厂批号！"); return;
            }
            if (string.IsNullOrEmpty(newIMEI.Text))
            {
                MessageBox.Show("请输入新IMEI！");
                return;
            }
            if (string.IsNullOrEmpty(newSN.Text))
            {
                MessageBox.Show("请输入新SN！");
                return;
            }

            List<API.ASP_RepairInfo> list = new List<API.ASP_RepairInfo>();
            foreach (var item in searchGrid.SelectedItems)
            {
                API.ASP_RepairInfo model = new API.ASP_RepairInfo();
                API.View_ASPRepairInfo rep = item as API.View_ASPRepairInfo;
                model.IsBack = true;
                model.BackNote = this.backSelectNote.TextBox.SearchText;
                model.ID = rep.ID;
                model.BackInListID = backInlistID.Text.Trim();
                model.NewIMEI = newIMEI.Text.Trim();
                model.NewSN = newSN.Text.Trim();
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
                SlModel.operateExcel<API.View_ASPRepairInfo> excel = new SlModel.operateExcel<API.View_ASPRepairInfo>();

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "xls";
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "xls");
                dialog.FilterIndex = 1;
               
                if (dialog.ShowDialog() == true)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        //Hashtable ht = new Hashtable();
                        
                        //ht.Add("Pro_Type", "商品品牌");
                        //ht.Add("Pro_Name","商品名称");
                        //ht.Add("ProFormat", "商品属性");
                        //ht.Add("ServiceID", "受理单号");
                        //ht.Add("OldID", "手工单号");
                        //ht.Add("Cus_Name", "客户姓名");
                        //ht.Add("Cus_Phone", "手机号码");
                        //ht.Add("IMEI", "会员卡号");
                        //ht.Add("RecHallName", "服务网点");
                        //ht.Add("Pro_HeaderIMEI", "串码");
                        //ht.Add("SysDate", "受理日期");

                        List<string> strs = new List<string>() { 
                        "商品品牌","商品名称","商品属性","受理单号","手工单号",
                        "客户姓名","手机号码","会员卡号","服务网点","串码","受理日期"
                        };

                        List<string> fields = new List<string>() { 
                        "Pro_Type","Pro_Name","ProFormat","ServiceID","OldID",
                        "Cus_Name","Cus_Phone","IMEI","RecHallName","Pro_HeaderIMEI","SysDate"
                        };

                        excel.getExcel(list, strs,fields, stream);

                        //Application.Current.Dispatcher.Invoke((Action)delegate { excel.getExcel(pageParam.Obj as List<API.View_ASPRepairInfo>, ht, stream); });
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出完成！");
                        this.isbusy.IsBusy = false;
                    }
                }

            }
            else
            {
                this.isbusy.IsBusy = false;
                MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message);
            }
        }

        private void isToFac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            toFac.IsEnabled = false;
            backFac.IsEnabled = false;
            if (isToFac.SelectedValue.ToString() == "未送厂")
            {
                toFac.IsEnabled = true;
                backFac.IsEnabled = false;
            }
            else if (isToFac.SelectedValue.ToString() == "返厂中")
            {
                toFac.IsEnabled = false;
                backFac.IsEnabled = true;
            }
            Search((int)pagesize.Value, radDataPager1.PageIndex, SearchCompleted);
        }
    }
}
