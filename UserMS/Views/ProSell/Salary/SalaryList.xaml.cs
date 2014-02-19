using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Excel;
using Telerik.Windows.Controls;

namespace UserMS.Views.ProSell.Price
{
    public partial class SalaryList : BasePage
    {
        private List<TreeViewModel> treeModels = null;

        List<API.SalaryBill> models = null;
        List<API.SalaryBill> proinfos = null;

        List<TreeViewModel> ParentTree = new List<TreeViewModel>();
        API.SalaryBill CurrentSalary = new API.SalaryBill();

        public SalaryList()
        {
            InitializeComponent();
            //note.Text = "Excel参考格式\n(注：日期只取日份):";
            proinfos = new List<API.SalaryBill>();
            treeModels = new List<TreeViewModel>();
            models = new List<API.SalaryBill>();
            GridSalaryList.ItemsSource = models;

            date.DateTimeText = DateTime.Now.ToString("yyyy-MM");
            //import.ImportType = typeof(API.SalaryImportModel);
            //import.OnImported += import_OnImported;
        }

        #region   导入Excel

        public void Import(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
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
                     
                            List<API.SalaryBill> list = new List<API.SalaryBill>();
                            List<int> days = new List<int>();
                            List<int> rsdays= new List<int>();  //反转日期
                            int index = 0;
                            DateTime sdate = DateTime.Parse(date.DateTimeText);

                            try
                            {
                                #region  读取数据
                                while (excelReader.Read())
                                {
                                    if (index == 0)
                                    {
                                        for (int i = 5; i < excelReader.FieldCount; i++)
                                        {
                                            days.Add(excelReader.GetInt32(i));
                                        }
                                        for (int i = days.Count - 1; i >= 0; i--)
                                        {
                                            rsdays.Add(days[i]);
                                        }
                                        index++;
                                        continue;
                                    }
                                    API.SalaryBill sm = new API.SalaryBill();
                                    try
                                    {
                                        sm.ProMainID = Convert.ToInt32(excelReader.GetString(0));
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Windows.MessageBox.Show("总商品编码有误！");
                                        return;
                                    }
                                    sm.ClassName = excelReader.GetString(1);
                                    sm.TypeName = excelReader.GetString(2);
                                    sm.ProName = excelReader.GetString(3);
                                    string sellTypeName = excelReader.GetString(4);

                                    sm.Children = new List<API.SalaryBillChild>();
                                    API.SalaryBillChild child;

                                    for (int i = 5; i < excelReader.FieldCount; i++)
                                    {
                                        child = new API.SalaryBillChild();
                                        string ss = excelReader.GetString(i);
                                        try
                                        {
                                            child.BaseSalary = Convert.ToDecimal(ss);
                                        }
                                        catch(Exception ex)
                                        {
                                            System.Windows.MessageBox.Show("基本提成有误！");
                                            return;
                                        }
                                        child.SellTypeName = sellTypeName;
                                        child.Year = sdate.Year;
                                        child.Month = sdate.Month;
                                        //查询在此日期前是否存在相同日期 相同日期择取下个月
                                        int count = rsdays.Skip(days.Count - (i - 5) - 1).Take(days.Count + i - 9).Count(p => p == days[i - 5]) - 1;
                                        //if (count > 0)
                                        //{
                                        //    System.Windows.MessageBox.Show("请确保数据只有一个月的数据，日期不可重复！");
                                        //    return;
                                        //}
                                        child.Month = sdate.Month + count;
                                        if (child.Month > 12)
                                        {
                                            child.Month = child.Month - 12;
                                            child.Year += 1;
                                        }
                                        child.Day = days[i - 5];
                                        sm.Children.Add(child);
                                    }
                                    list.Add(sm);
                                }
                                #endregion

                                excelReader.Close();
                                myStream.Close();
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show("Excel格式有误！");
                                return;
                            }

                            #region  验证数据


                            List<string> selltypes = new List<string>();
                            foreach (var xx in Store.SellTypes)
                            {
                                selltypes.Add(xx.Name);
                            }

                            foreach(var item in list)
                            {
                                if(item.ProMainID==0&&( string.IsNullOrEmpty(item.ClassName)
                                    || string.IsNullOrEmpty(item.TypeName)|| string.IsNullOrEmpty(item.ProName)))
                                {
                                    System.Windows.MessageBox.Show("请确保商品信息完整！");
                                    return;
                                }
                                if (item.ProMainID!=0)
                                {
                                    var main = from a in Store.ProMainInfo
                                               where a.ProMainID == item.ProMainID
                                               select a;
                                    if (main.Count() == 0)
                                    {
                                        System.Windows.MessageBox.Show("总商品编码有误："+item.ProMainID +" ！");
                                        return;
                                    }
                                }

                                var d = from a in item.Children
                                           where a.Day > 31 && a.Day < 1
                                           select a;
                                if (d.Count() > 0)
                                {
                                    System.Windows.MessageBox.Show("日期无效: " + d.First().Day + " ！");
                                    return;
                                   
                                }

                                var salary = from a in item.Children
                                             where a.BaseSalary < 0
                                             select a;
                                if (salary.Count() > 0)
                                {
                                    System.Windows.MessageBox.Show("基本提成不能小于零！");
                                    return;
                                }

                                var st =  from a in item.Children
                                            where !selltypes.Contains(a.SellTypeName)
                                            select a;
                                if(st.Count()>0)
                                {
                                    System.Windows.MessageBox.Show("销售类别：" + st.First().SellTypeName + "不存在！");
                                    return;
                                }
                                
                            }

                            if (list.Count() > 0)
                            {
                                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 292, new object[] {  list }, new EventHandler<API.MainCompletedEventArgs>(ImportCompleted));
                            }

                            #endregion 
                      
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "请先关闭Excel再导入！");

                }
            }
        }

        private void ImportCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                models.Clear();
                models.AddRange(e.Result.Obj as List<API.SalaryBill>);
                GridSalaryList.Rebind();
            }
            else
            {
                System.Windows.MessageBox.Show(e.Result.Message);
            }
        }

        #endregion 

        #region 添加商品

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();

            Common.CommonHelper.ProFilterGen(Store.ProInfo.ToList(), ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
              pros, "ProName", new string[] { "ClassName", "TypeName", "ProName", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "商品属性" });
            m.Closed += ProSelect_Closed;
            m.ShowDialog();
        }

        /// <summary>
        /// 确定添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProSelect_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 selecter = sender as UserMS.MultSelecter2;

            if (selecter.DialogResult == true)
            {
                List<SlModel.ProductionModel> piList = selecter.SelectedItems.OfType<SlModel.ProductionModel>().ToList();
                if (piList.Count == 0) return;

                foreach (var item in piList)
                {
                    if (!ValidateProduction(item.ProID))
                    {
                        API.SalaryBill ss = new API.SalaryBill();
                        ss.ProID = item.ProID;
                        ss.TypeName = item.TypeName;
                        ss.ProName = item.ProName;
                        ss.ProFormat = item.ProFormat;
                        ss.ClassName = item.ClassName;
                        models.Add(ss);
                    }
                }
                GridSalaryList.Rebind();
                PublicRequestHelp prh2 = new PublicRequestHelp(this.busy, 159, new object[] {models }, new EventHandler<API.MainCompletedEventArgs>(GetSaralyCompleted));
            }
        }

        private void GetSaralyCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                models.Clear();
                models.AddRange(e.Result.Obj as List<API.SalaryBill>);
                GridSalaryList.Rebind();
            }
        }

        private bool ValidateProduction(string pid)
        {
            if (models == null)
                return false;
            foreach (var vm in models)
            {
                if (vm.ProID == pid)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion 

        #region   添加销售类别

        private void addSellType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (models.Count == 0)
            {
                return;
            }
            if (GridSalaryList.SelectedItem == null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择商品");
                return;
            }

            MultSelecter2 msFrm = new MultSelecter2(
              null, Store.SellTypes, "Name",
              new string[] { "Name" },
              new string[] { "销售类别" });
            msFrm.Closed += SellTSelect_Closed;
            msFrm.ShowDialog();
        }

        private void SellTSelect_Closed(object sender, WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                API.SalaryBill model = GridSalaryList.SelectedItem as API.SalaryBill;
                List<API.Pro_SellType> piList = result.SelectedItems.OfType<API.Pro_SellType>().ToList();
                if (piList.Count == 0) return;
                if (model.Children == null)
                {
                    model.Children = new List<API.SalaryBillChild>();
                }

                int index = model.Children.Count + 1;
                foreach (var item in piList)
                {
                    API.SalaryBillChild pc = new API.SalaryBillChild();
                    pc.SellTypeID = item.ID;
                    pc.StartDate = DateTime.Now.Date;
                    pc.EndDate = DateTime.Now.Date;
                    pc.UpdateFlag = false;
                    //pc.HasPrice = false;
                    pc.SellTypeName = item.Name;
                    pc.ID = index;
                    if (model.Children == null)
                    {
                        model.Children = new List<API.SalaryBillChild>();
                    }
                    model.Children.Add(pc);
                    index++;
                }

                GridSalaryDetail.ItemsSource = model.Children;
                GridSalaryDetail.Rebind();
            }
        }

        #endregion 

        #region   删除

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridSalaryList.SelectedItems == null)
            {
               System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要删除的项");
                return;
            }
            if (System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            foreach (var item in GridSalaryList.SelectedItems)
            {
                API.SalaryBill child = item as API.SalaryBill;
                foreach (var m in models)
                {
                    if (child == m)
                    {
                        models.Remove(m);
                        break;
                    }
                }
            }
            GridSalaryList.Rebind();
        }

        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSellType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridSalaryDetail.SelectedItems == null)
            {
                return;
            }
            if (System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            API.SalaryBill sb = GridSalaryList.SelectedItem as API.SalaryBill;

            List<int> oldids = new List<int>();

            foreach (var child in GridSalaryDetail.SelectedItems)
            {
                API.SalaryBillChild sc = child as API.SalaryBillChild;

                foreach (var item in sb.Children)
                {
                    if (!sc.UpdateFlag && sc.ID == item.ID)
                    {
                        sb.Children.Remove(item);
                        break;
                    }
                    if (sc.UpdateFlag && sc.ID == item.ID)
                    {
                        oldids.Add(item.ID);
                        break;
                    }
                }
            }

            if (oldids.Count > 0)
            {
                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 206, new object[] { oldids }, new EventHandler<API.MainCompletedEventArgs>(DeleteCompleted));
            }
         
            GridSalaryDetail.ItemsSource = sb.Children;
            GridSalaryDetail.Rebind();
        }

        private void DeleteCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "删除失败: 服务器错误\n" + e.Error.Message);
                return;
            } this.busy.IsBusy = false;
           if (e.Result.ReturnValue)
           {
               List<API.SalaryBill> list = GridSalaryList.ItemsSource as List<API.SalaryBill>;

               API.SalaryBill select = new API.SalaryBill();
               foreach (var item in list)
               {
                   if (item.ProID==CurrentSalary.ProID)
                   {
                       select = item;
                       break;
                   }
               }
               List<int> oldids = e.Result.Obj as List<int>;

               foreach (var child in oldids)
               {
                   foreach (var item in select.Children)
                   {
                       if (item.ID==child)
                       {
                           select.Children.Remove(item);
                           break;
                       }
                   }
               }
               GridSalaryDetail.ItemsSource = select.Children;
               GridSalaryDetail.Rebind();
           }
           else
           {
               System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"删除失败");

           }
        }

        #endregion

        #region  保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (models.Count == 0)
            {
                System.Windows.MessageBox.Show("请添加数据！");
                return;
            }

            #region  验证有效性

            var sala = from a in models
                       select a.Children into temp
                       from b in temp
                       where b.BaseSalary < 0
                       select b;
            if (sala.Count() > 0)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "基本提成不能小于0");

                return;
            }

            var st = from a in models
                       select a.Children into temp
                       from b in temp
                      where string.IsNullOrEmpty(b.SellTypeName)
                       select b;
            if (st.Count() > 0)
            {
                System.Windows.MessageBox.Show("销售类别不能为空！");
                return;
            }

            #endregion

            API.Sys_SalaryChange header = new API.Sys_SalaryChange();
            header.Sys_SalaryList = new List<API.Sys_SalaryList>();

            List<string> classids = new List<string>();
            foreach (var item in models)
            {
                classids.Add(item.ClassID);
                foreach (var child in item.Children)
                {
                    API.Sys_SalaryList ss = new API.Sys_SalaryList();
                    ss.BaseSalary = child.BaseSalary;
                    ss.SalaryDay = child.Day;
                    ss.SalaryMonth = child.Month;
                    ss.SalaryYear = child.Year;
                    ss.SysDate = DateTime.Now;
                    ss.ProID = item.ProID;
                    ss.SellType = child.SellTypeID;
                    ss.SpecialSalary = child.SpecalSalary;

                    header.Sys_SalaryList.Add(ss);
                }
            }

            if (System.Windows.MessageBox.Show("确定保存吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp prh = new PublicRequestHelp(this.busy,160,new object[]{ header ,classids},new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                models.Clear();
                GridSalaryList.Rebind();
                GridSalaryDetail.ItemsSource = null;
                GridSalaryDetail.Rebind();
               // PublicRequestHelp prh2 = new PublicRequestHelp(this.busy, 159, new object[] { models }, new EventHandler<API.MainCompletedEventArgs>(GetSaralyCompleted));
   
            }
            else
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存失败: "+e.Result.Message);
            }
        }

        #endregion 

        private void GridSalaryList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (GridSalaryList.SelectedItem == null)
            {
                return;
            }
            API.SalaryBill sb = GridSalaryList.SelectedItem as API.SalaryBill;
            CurrentSalary = sb;
            GridSalaryDetail.ItemsSource = sb.Children;
            GridSalaryDetail.Rebind();
        }

        private void RadDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            date.Culture.DateTimeFormat.ShortDatePattern = "yyyy-MM";
        }

        /// <summary>
        /// 同步提成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void asynSalary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //1.将所有提成同步到最新提成表    2.同步提成到selllistinfo 
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 293, new object []{ },new EventHandler<API.MainCompletedEventArgs>(AsynCompleted));
        }

        private void AsynCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
             if (e.Error != null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
             if (e.Result.ReturnValue)
             {
                 System.Windows.MessageBox.Show(e.Result.Message);
             }
             else
             {
                 System.Windows.MessageBox.Show("数据同步有误！");
             }
        }


    }
}
