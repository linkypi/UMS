using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using Excel;
using Microsoft.Win32;

namespace UserMS.Views.StockMS.Allot
{
    /// <summary>
    /// Import_Allot.xaml 的交互逻辑
    /// </summary>
    public partial class Import_Allot : BasePage
    {
        public Import_Allot()
        {
            InitializeComponent();
            //import.ImportType = typeof(API.OutImportModel);
            //import.OnImported += import_OnImported;
        }

        #region 导入

        public void Import(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Excel 文件 (*.xls, *.xlsx)|*.xlsx;*.xls";
            Stream myStream;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            IExcelDataReader excelReader = null;
                            if (System.IO.Path.GetExtension(openFileDialog.SafeFileName) == ".xls")
                            {
                                excelReader = ExcelReaderFactory.CreateBinaryReader(myStream);
                            }
                            else if (System.IO.Path.GetExtension(openFileDialog.SafeFileName) == ".xlsx")
                            {
                                excelReader = ExcelReaderFactory.CreateOpenXmlReader(myStream);
                            }
                            else
                            {
                                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "导入失败");
                                return;
                            }
                            excelReader.IsFirstRowAsColumnNames = true;

                            List<API.OutImportModel> list = new List<API.OutImportModel>();
                           
                            int index = 0;
                      
                            try
                            {
                                #region  读取数据

                            
                                while (excelReader.Read())
                                {
                                    if(index==0)
                                    {
                                        if (excelReader.FieldCount != 7)
                                        {
                                            System.Windows.Forms.MessageBox.Show("Excel格式有误，导入失败！");
                                            return;
                                        }
                                        index++;
                                        continue;
                                    }
                                    API.OutImportModel om = new API.OutImportModel();
                                    om.OldID = excelReader.GetString(0);
                                    om.FromHall = excelReader.GetString(1);
                                    om.ToHall = excelReader.GetString(2);
                                    om.Note = excelReader.GetString(3);
                                    om.ProID = excelReader.GetString(4);
                                    om.IMEI = excelReader.GetString(6) == null ? "" : excelReader.GetString(6).ToUpper();
                                    try
                                    {
                                        om.ProCount = excelReader.GetDecimal(5);
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Windows.MessageBox.Show("第 "+ (index+1) +" 行商品数量有误，导入失败！");
                                        return;
                                    }

                                    if (!string.IsNullOrEmpty(om.IMEI))
                                    {
                                        om.NeedIMEI = true;
                                        om.ProCount = 1;
                                    }
                                    if (string.IsNullOrEmpty(om.FromHall))
                                    {
                                        System.Windows.Forms.MessageBox.Show("Excel中第 " + (index+1) + " 行的调出仓不能为空！");
                                        return;
                                    }
                                    if (string.IsNullOrEmpty(om.ToHall))
                                    {
                                        System.Windows.Forms.MessageBox.Show("Excel中第 " + (index+1) + " 行调入仓不能为空！");
                                        return;
                                    }
                                    if (string.IsNullOrEmpty(om.ProID) && string.IsNullOrEmpty(om.IMEI))
                                    {
                                        System.Windows.Forms.MessageBox.Show("Excel中第 " + (index+1) + " 行的商品编码或者串码不能为空！");
                                        return;
                                    }
                                    if (om.ProCount == 0)
                                    {
                                        System.Windows.Forms.MessageBox.Show("Excel中第 " + (index+1) + " 行的商品数量不能为0！");
                                        return;
                                    }

                                    list.Add(om);
                                    index++;
                                }
                                #endregion

                                #region  验证数据
                                List<string> imeis = new List<string>();

                                foreach (var item in list)
                                {
                                    if(string.IsNullOrEmpty(item.IMEI)==false)
                                    {
                                       imeis.Add(item.IMEI);
                                    }
                                }
                                if (imeis.Count > 0)
                                {
                                    foreach (var item in imeis)
                                    {
                                        var val = from m in imeis
                                                  where m==item
                                                  select m;
                                        if (val.Count() > 1)
                                        {
                                            System.Windows.MessageBox.Show("串码重复："+ item +" ！");
                                            return;
                                        }
                                    }
                                }
                                //index = 0;
                                //foreach (var item in list)
                                //{
                                //    index++;
                                //    //try
                                //    //{
                                //    //    decimal count = Convert.ToDecimal(item.ProCount);
                                //    //}
                                //    //catch (Exception ex)
                                //    //{
                                //    //    System.Windows.Forms.MessageBox.Show("商品数量无效！");
                                //    //    return;
                                //    //}
                                //    if (!string.IsNullOrEmpty(item.IMEI))
                                //    {
                                //        item.NeedIMEI = true;
                                //        item.ProCount = 1;
                                //    }
                                //    if (string.IsNullOrEmpty(item.FromHall))
                                //    {
                                //        System.Windows.Forms.MessageBox.Show("Excel中第 "+ index +" 行的调出仓不能为空！");
                                //        return;
                                //    }
                                //    if (string.IsNullOrEmpty(item.ToHall))
                                //    {
                                //        System.Windows.Forms.MessageBox.Show("Excel中第 " + index + " 行调入仓不能为空！");
                                //        return;
                                //    }
                                //    if (string.IsNullOrEmpty(item.ProID) && string.IsNullOrEmpty(item.IMEI))
                                //    {
                                //        System.Windows.Forms.MessageBox.Show("Excel中第 " + index + " 行的商品编码或者串码不能为空！");
                                //        return;
                                //    }

                                //    if (item.ProCount == 0)
                                //    {
                                //        System.Windows.Forms.MessageBox.Show("Excel中第 " + index + " 行的商品数量不能为0！");
                                //        return;
                                //    }

                                //}
   
                                #endregion 

                                int RowCount = excelReader.AsDataSet().Tables[0].Rows.Count;
                                if (RowCount == (index - 1))
                                {
                                    System.Windows.MessageBox.Show("全部导入成功,共 " + (index - 1) + " 行数据！");
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("导入失败，有 " + (RowCount-(index - 1)) + " 行数据尚未导入！");
                                }
                                dataGrid1.ItemsSource = list;
                                dataGrid3.ItemsSource = null;
                                dataGrid4.ItemsSource = null;
                                dataGrid2.ItemsSource = null;
                                dataGrid3.Rebind();
                                dataGrid4.Rebind();
                                dataGrid2.Rebind();

                                excelReader.Close();
                                myStream.Close();

                              
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show("Excel格式有误！");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "请先关闭Excel再导入！");
                 }
            }
        }

        void import_OnImported(object sender, DataImportArgs e)
        {
            var a=e.Datas;
            List<API.OutImportModel> list =Common.DataExtensions.ToList<API.OutImportModel>(a).ToList();
            foreach (var item in list)
            {
                try
                {
                    decimal count = Convert.ToDecimal(item.ProCount);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("商品数量无效！");
                    return;
                }
                if (!string.IsNullOrEmpty(item.IMEI))
                {
                    item.NeedIMEI = true;
                    item.ProCount = 1;
                }
                if (string.IsNullOrEmpty(item.FromHall))
                {
                    System.Windows.Forms.MessageBox.Show("调出仓不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(item.ToHall))
                {
                    System.Windows.Forms.MessageBox.Show("调出仓不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(item.ProID) && string.IsNullOrEmpty(item.IMEI))
                {
                    System.Windows.Forms.MessageBox.Show("商品编码或者串码不能为空！");
                    return;
                }
              
                if (item.ProCount == 0)
                {
                    System.Windows.Forms.MessageBox.Show("商品编码 "+item.ProID+" 的数量不能为0！");
                    return;
                }
                
            }
            dataGrid1.ItemsSource = list;

            dataGrid3.ItemsSource = null;
            dataGrid4.ItemsSource = null;
            dataGrid2.ItemsSource = null;
            dataGrid3.Rebind();
            dataGrid4.Rebind();
            dataGrid2.Rebind();
        }

        #endregion 

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
               string r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];

            }
            catch
            {
              
            }
            finally
            {

            }
        }

        #region 调拨数据处理
        private void TBSearch_Click(object sender, RoutedEventArgs e)
        {
            this.busy.IsBusy = true;
            SlModel.operateExcel<API.OutImportModel> excel = new SlModel.operateExcel<API.OutImportModel>();
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog() { Filter = "Excel文件|*.xls;*.xlsx", Multiselect = false };
          
            if (op.ShowDialog() == DialogResult.OK)
            {
               // this.TbFileName.Text = op.FileName;
                using (Stream stream = op.OpenFile())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("OldID", "原始单号");
                    ht.Add("FromHallID", "调出仓编码");
                    ht.Add("ToHallID", "调入仓编码");
                    ht.Add("Note", "备注");
                    ht.Add("ProID", "商品名称");
                    ht.Add("ProCount", "商品数量");
                    ht.Add("IMEI", "串码");
             
                  //  Application.Current.Dispatcher.Invoke((Action)delegate { excel.fromExcel(ht, this.TbFileName.Text.Trim()); });
                    //MessageBox.Show(System.Windows.Application.Current.MainWindow,"导入完成");
                    this.busy.IsBusy = false;
                }
            }

            #region
            //Workbook workbook1;

            //// Open dialog

            //// If user selected a file get the stream of that file
            //FileStream stream = op.File.OpenRead();

            //// Load workbook with data
            //workbook1 = Workbook.Load(stream);

            //stream.Close();
            ////workbook1.Worksheets[0].Rows[0].Cells[0].Value;
            //List<API.Pro_ClassInfo> list = new List<API.Pro_ClassInfo>();
            ////workbook1.Worksheets[0].Columns[0].
            //WorksheetRow firstwr = workbook1.Worksheets[0].Rows[0];
            //DataTable pro_dt = Store.ProClassInfo.Tables[0];
            //DataTable pack_dt = Store.ProClassInfo.Tables[0];


            //if (firstwr.Cells.Count() < 3)
            //{
            //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"导入数据缺少列");
            //    this.busy.IsBusy = false;
            //    return;
            //}
            //for (int i = 1; i < workbook1.Worksheets[0].Rows.Count(); i++)
            //{

            //    WorksheetRow wr = workbook1.Worksheets[0].Rows[i];

            //    Xceed.Wpf.DataGrid.DataRow pro_dr = pro_dt.Rows.First(p => ((DataRow)p)["ProID"].ToLower() == (wr.Cells[0].Value + "").ToLower());
            //    if (pro_dr == null)
            //    {
            //        string s = "第" + (i + 1) + "行的商品编号" + wr.Cells[0] + "不存在";
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow,s);
            //        this.busy.IsBusy = false;
            //        return;
            //    }
            //    System.Collections.Generic.IEnumerable<Silverlight.DataRow> pack_dr_list = pack_dt.Rows.Where(p => ((Silverlight.DataRow)p)["ProID"].ToLower() == (wr.Cells[0].Value + "").ToLower());
            //    if (pack_dr_list == null || pack_dr_list.Count() == 0)
            //    {
            //        string s = "第" + (i + 1) + "行的商品单位获取出错";
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow,s);
            //        this.busy.IsBusy = false;
            //        return;
            //    }
            //    DataRow pack_dr = pack_dr_list.First(p => ((DataRow)p)["LUnit"] == (wr.Cells[2].Value + ""));
            //    if (pack_dr == null)
            //    {
            //        string s = "第" + (i + 1) + "行的商品单位" + wr.Cells[2].Value + "" + "不存在";
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow,s);
            //        this.busy.IsBusy = false;
            //        return;
            //    }
            //    if (!PageValidate.IsDecimal(wr.Cells[1].Value + ""))
            //    {
            //        string s = "第" + (i + 1) + "行的商品数量不正确";
            //        MessageBox.Show(System.Windows.Application.Current.MainWindow,s);
            //        this.busy.IsBusy = false;
            //        return;
            //    }
            //    ItemModel mm = (ItemModel)this.view.AddNew();

            //    mm.ProID = wr.Cells[0].Value + "";
            //    mm.ProName = pro_dr["ProName"];
            //    mm.LUnit = wr.Cells[2].Value + "";
            //    mm.Units = new List<string>();
            //    mm.ProPrice = Convert.ToDecimal(wr.Cells[1].Value);
            //    foreach (DataRow dr in pack_dr_list)
            //    {
            //        mm.Units.Add(dr["LUnit"]);
            //    }
            //    //if(pack_dr["ProID"]
            //}
            //this.view.CommitNew();
            //this.dataGrid1.ItemsSource = this.view;
            //this.dataGrid1.Rebind();    
            //this.busy.IsBusy = false;


            //#region Reading Data From Excel File

            //dynamic objExcel = AutomationFactory.CreateObject("Excel.Application");

            ////Open the Workbook Here
            //dynamic objExcelWorkBook =
            //    objExcel.Workbooks.Open(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            //    + "\\" + fileName);
            ////Read the Worksheet
            //dynamic objActiveWorkSheet = objExcelWorkBook.ActiveSheet();
            ////Cells to Read
            //dynamic objCell_1, objCell_2;

            ////Iterate through Cells
            //for (int count = 2; count < 17; count++)
            //{
            //    objCell_1 = objActiveWorkSheet.Cells[count, 1];
            //    objCell_2 = objActiveWorkSheet.Cells[count, 2];
            //    //populationData.Add
            //    //    (
            //    //        new PopulationClass()
            //    //        {
            //    //            StateName = objCell_1.Value,
            //    //            Population = objCell_2.Value
            //    //        }
            //    //    );
            //}

            ////dgExcelData.ItemsSource = populationData;

            #endregion

        }
        #endregion 

        #region  拣货

        /// <summary>
        /// 拣货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            List<API.OutImportModel> models = dataGrid1.ItemsSource as List<API.OutImportModel>;
            if (models == null)
            {
                System.Windows.Forms.MessageBox.Show("请导入数据！");
                return;
            }
            if (models.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("请导入数据！");
                return;
            }
            foreach (var item in models)
            {
              
            }

            #region  验证仓库

            var hallval = (from a in models
                           select a.FromHall).Distinct().ToList();
         
            var hallids=Store.ProHallInfo.Select(p=>p.HallName);
            var v = hallval.Where(p => !hallids.Contains(p));
            if (v.Count() > 0)
            {
                System.Windows.MessageBox.Show("调出仓有误：" + v.First() + " !");
                return;
            }


            var hallval2 = (from a in models
                           select a.ToHall).Distinct().ToList();
            var h2 = hallval2.Where(p => !hallids.Contains(p));
            if (h2.Count() > 0)
            {
                System.Windows.MessageBox.Show("调入仓有误：" + h2.First() + " !"); return;
            }

            #endregion

            #region  验证商品编码

            var proval = (from a in models
                          where string.IsNullOrEmpty(a.ProID) == false
                          select a.ProID).Distinct().ToList();
            var pros = Store.ProInfo.Select(p => p.ProID);

            var px = proval.Where(p => !pros.Contains(p));
            if (px.Count() > 0)
            {
                System.Windows.MessageBox.Show("商品编码无效：" + px.First() + " !"); return;
            }

            #endregion

        
            PublicRequestHelp prh = new PublicRequestHelp(this.busy,275,new object[]{models},new   EventHandler<API.MainCompletedEventArgs>(CheckCompleted));
        }

        private void CheckCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
      
            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<API.OutImportModel> list = e.Result.Obj as List<API.OutImportModel>;

                #region  验证指定仓库的串码是否都存在
                List<API.OutImportModel> models = dataGrid1.ItemsSource as List<API.OutImportModel>;

                List<string> imeis = new List<string>();
                foreach (var item in models)
                {
                    if (string.IsNullOrEmpty(item.IMEI) == false)
                    {
                        imeis.Add(item.IMEI);
                    }
                }
                if (imeis.Count > 0)
                {
                    var rr = from a in list
                             where !imeis.Contains(a.IMEI)
                             select a;
                    string msg = "";
                    foreach (var item in rr)
                    {
                        msg += item.IMEI + " , ";
                    }
                    if (!string.IsNullOrEmpty(msg))
                    {
                        System.Windows.Forms.MessageBox.Show("串码不存在：" + msg + "拣货失败！");

                        return;
                    }
                }
                #endregion 

                #region 验证仓库是否都存在
                //

                //List<string> oldlist = new List<string>();
                //foreach (var item in models)
                //{
                //    if (!oldlist.Contains(item.ToHall))
                //    {
                //        oldlist.Add(item.ToHall);
                //    }
                //    if (!oldlist.Contains(item.FromHall))
                //    {
                //        oldlist.Add(item.FromHall);
                //    }
                //}

                //List<string> halls = new List<string>();
                //foreach (var item in list)
                //{
                //    if (!halls.Contains(item.ToHall))
                //    {
                //        halls.Add(item.ToHall);
                //    }
                //    if (!halls.Contains(item.FromHall))
                //    {
                //        halls.Add(item.FromHall);
                //    }
                //}

                //var diff = from a in oldlist
                //           where !halls.Contains(a)
                //           select a;
                //if (diff.Count() > 0)
                //{
                //    string msg = "";
                //    int indexs = 1;
                //    foreach (var item in diff)
                //    {
                //        msg += item;
                //        if (indexs < diff.Count())
                //        {
                //            msg += " , ";
                //        }
                //        indexs++;
                //    }
                //    System.Windows.Forms.MessageBox.Show("仓库不存在：" + msg);
                //    return;
                //}

                #endregion

                #region   将数据整合为三层结构

                List<API.OutImportModel> temps = new List<API.OutImportModel>();

                //根据原始单号  调入仓  调出仓  商品名称分组
                var glist = from a in list
                            group a by new { a.OldID, a.FromHallID, a.ToHallID, a.Note } into s
                            select new
                            {
                                OldID = s.Key,
                                groupList = s.ToList()
                            };
                API.OutImportModel mm = new API.OutImportModel();
                int index = 0;
                bool success = true;
                //遍历每一张原始单
                foreach (var item in glist)
                {
                    //根据批次号分组
                    var ss = from a in item.groupList
                             group a by new { a.InListID } into b
                             select new
                             {
                                 Pros = b.ToList()
                             };

                    #region

                    foreach (var child in ss)
                    {
                        foreach (var xxd in child.Pros)
                        {
                            //若是同一调拨单则直接添加到明细中
                            if (mm.FromHallID == xxd.FromHallID && mm.ToHallID == xxd.ToHallID && mm.OldID == xxd.OldID)
                            {
                                //判断是否存在同一批次的同种商品
                                var samePro = from a in mm.Children
                                              where a.InListID == xxd.InListID && a.ProID == xxd.ProID
                                              select a;
                                if (samePro.Count() > 0)
                                {
                                    #region  判断是否是串码
                                    if (xxd.NeedIMEI)
                                    {
                                        API.IMEIModel imei = new API.IMEIModel();
                                        imei.NewIMEI = xxd.IMEI;
                                        imei.CheckNote = xxd.Success ? "成功" : xxd.CheckNote;
                                        success = success & xxd.Success;
                                        samePro.First().IMEIList.Add(imei);
                                        samePro.First().ProCount += 1;
                                    }
                                    else
                                    {
                                        samePro.First().ProCount += xxd.ProCount;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region  添加新明细

                                    API.OutImportModelList childx = new API.OutImportModelList();
                                    var ct = from p in Store.ProInfo
                                             join c in Store.ProClassInfo
                                             on p.Pro_ClassID equals c.ClassID
                                             join t in Store.ProTypeInfo
                                             on p.Pro_TypeID equals t.TypeID
                                             where p.ProID == xxd.ProID
                                             select new
                                             {
                                                 c.ClassName,
                                                 t.TypeName,
                                                 p.ProFormat
                                             };
                                    if (ct.Count() > 0)
                                    {
                                        childx.ClassName = ct.First().ClassName;
                                        childx.TypeName = ct.First().TypeName;
                                        childx.ProFormat = ct.First().ProFormat;
                                    }

                                    childx.ProName = xxd.ProName;
                                    childx.ProID = xxd.ProID;
                                    childx.InListID = xxd.InListID;
                                    childx.ProCount = xxd.ProCount;
                                    childx.Success = xxd.Success;
                                    childx.CheckNote = childx.Success ? "成功" : "失败";
                                    success = success & xxd.Success;
                                    childx.IMEIList = new List<API.IMEIModel>();
                                    if (!string.IsNullOrEmpty(xxd.IMEI))
                                    {
                                        childx.NeedIMEI = true;
                                        API.IMEIModel imei = new API.IMEIModel();
                                        imei.NewIMEI = xxd.IMEI;
                                        imei.CheckNote = xxd.Success ? "成功" : xxd.CheckNote;
                                        childx.IMEIList.Add(imei);
                                    }

                                    if (mm.Children == null)
                                    {
                                        mm.Children = new List<API.OutImportModelList>();
                                    }
                                    mm.Children.Add(childx);
                                    #endregion
                                }
                            }
                            else //否则新增实体
                            {
                                #region

                                if (index != 0)
                                {
                                    temps.Add(mm);
                                }
                                index++;

                                mm = new API.OutImportModel();
                                mm.OldID = xxd.OldID;
                                mm.FromHall = xxd.FromHall;
                                mm.FromHallID = xxd.FromHallID;
                                mm.ToHallID = xxd.ToHallID;
                                mm.ToHall = xxd.ToHall;
                                mm.Note = xxd.Note;
                                mm.Children = new List<API.OutImportModelList>();
                                mm.ProCount = xxd.ProCount;
                                mm.CheckNote = xxd.CheckNote;

                                API.OutImportModelList childx = new API.OutImportModelList();
                                var ct = from p in Store.ProInfo
                                         join c in Store.ProClassInfo
                                         on p.Pro_ClassID equals c.ClassID
                                         join t in Store.ProTypeInfo
                                         on p.Pro_TypeID equals t.TypeID
                                         where p.ProID == xxd.ProID
                                         select new
                                         {
                                             c.ClassName,
                                             t.TypeName,
                                             p.ProFormat
                                         };
                                if (ct.Count() > 0)
                                {
                                    childx.ClassName = ct.First().ClassName;
                                    childx.TypeName = ct.First().TypeName;
                                    childx.ProFormat = ct.First().ProFormat;
                                }

                                childx.ProName = xxd.ProName;
                                childx.ProID = xxd.ProID;
                                childx.InListID = xxd.InListID;
                                childx.Success = xxd.Success;
                                childx.CheckNote = childx.Success ? "成功" : "失败";
                                success = success & xxd.Success;
                                if (!string.IsNullOrEmpty(xxd.IMEI))
                                {
                                    childx.NeedIMEI = true;
                                    childx.ProCount = 1;
                                    childx.IMEIList = new List<API.IMEIModel>();

                                    API.IMEIModel imei = new API.IMEIModel();
                                    imei.NewIMEI = xxd.IMEI;
                                    if (xxd.Success)
                                    {
                                        imei.CheckNote = "成功";
                                    }
                                    else
                                    {
                                        imei.CheckNote = xxd.CheckNote;
                                    }
                                    childx.IMEIList.Add(imei);
                                }
                                else
                                {
                                    childx.ProCount = xxd.ProCount;

                                }
                                mm.Children.Add(childx);
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                temps.Add(mm);

                if (!success)
                {
                    System.Windows.Forms.MessageBox.Show("拣货失败！");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("拣货成功！");
                }

                foreach (var item in temps)
                {
                    item.Success = success;
                    item.CheckNote = item.Success ? "成功" : "失败";
                }
                dataGrid3.ItemsSource = null;
                dataGrid4.ItemsSource = null;
                 dataGrid3.Rebind();
                 dataGrid4.Rebind();
                dataGrid2.ItemsSource = temps;
                dataGrid2.Rebind();
                if (temps.Count > 0)
                {
                    dataGrid2.SelectedItem = temps[0]; ;
                }

                #endregion
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(e.Result.Message);
            }
        }

        #endregion 

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;
            if (dataGrid2.ItemsSource == null)
            {
                System.Windows.Forms.MessageBox.Show("请添加需要导入的数据！");
                return;
            }
            List<API.OutImportModel> models = dataGrid2.ItemsSource as List<API.OutImportModel>;

            if (models.Count == 0) 
            {
                System.Windows.Forms.MessageBox.Show("请添加需要导入的数据！");
                return;
            }
            foreach (var item in models)
            {
                if (!item.Success)
                {
                    success = false;
                    break;
                }
                else 
                {
                    success = true;
                }
            }
            if (!success)
            {
                System.Windows.Forms.MessageBox.Show("拣货未通过！");
                return;
            }

            if (System.Windows.MessageBox.Show("确定导入吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            List<API.Pro_OutInfo> outmodels = new List<API.Pro_OutInfo>();
            foreach (var item in models)
            {
                API.Pro_OutInfo header = new API.Pro_OutInfo();
                header.OldID = item.OldID;
                header.FromHallID = item.FromHallID;
                header.Pro_HallID = item.ToHallID;
                header.UserID = Store.LoginUserInfo.UserID;
                header.FromUserID = Store.LoginUserInfo.UserID;
                header.Note = item.Note;
                header.Pro_OutOrderList = new List<API.Pro_OutOrderList>();
                foreach (var child in item.Children)
                {
                    API.Pro_OutOrderList list = new API.Pro_OutOrderList();
                    list.ProID = child.ProID;
                    list.ProCount = child.ProCount;
                    list.InListID = child.InListID;
                    list.Note = child.Note;
                    list.Pro_OutOrderIMEI = new List<API.Pro_OutOrderIMEI>();
                    if (child.IMEIList != null)
                    {
                        foreach (var xxd in child.IMEIList)
                        {
                            API.Pro_OutOrderIMEI imei = new API.Pro_OutOrderIMEI();
                            imei.IMEI = xxd.NewIMEI;
                            imei.Note = xxd.Note;
                            list.Pro_OutOrderIMEI.Add(imei);
                        }
                    }
                    header.Pro_OutOrderList.Add(list);
                }
                outmodels.Add(header);
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 276, new object[] { outmodels }, new EventHandler<API.MainCompletedEventArgs>(SummitCompleted));


        }

        private void SummitCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            System.Windows.Forms.MessageBox.Show(e.Result.Message);
            if (e.Result.ReturnValue)
            {
                dataGrid1.ItemsSource = null;
                dataGrid1.Rebind();
                dataGrid2.ItemsSource = null;
                dataGrid2.Rebind();
                dataGrid3.ItemsSource = null;
                dataGrid3.Rebind();
                dataGrid4.ItemsSource = null;
                dataGrid4.Rebind();
            }
        }

        private void dataGrid2_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            API.OutImportModel item = dataGrid2.SelectedItem as API.OutImportModel;
            dataGrid4.ItemsSource = null;
            dataGrid4.Rebind();
            dataGrid3.ItemsSource = item.Children;
            dataGrid3.Rebind();
        }

        private void imeiGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            API.OutImportModelList item = dataGrid3.SelectedItem as API.OutImportModelList;

            dataGrid4.ItemsSource = item.IMEIList;
            dataGrid4.Rebind();
        }

        private void Sumbit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }
    }
}
