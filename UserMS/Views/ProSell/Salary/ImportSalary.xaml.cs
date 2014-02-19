using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel;
using SlModel;
using UserMS.Model;

namespace UserMS.Views.ProSell.Salary
{
    /// <summary>
    /// ImportSalary.xaml 的交互逻辑
    /// </summary>
    public partial class ImportSalary : Page
    {
        public ImportSalary()
        {
            InitializeComponent();
            date.DateTimeText = DateTime.Now.ToString("yyyy-MM");
            flag = true;
            //GridSalaryList.ItemsSource = models;
        }
        int menuid = 343;
        bool flag = false;
        int pageindex=0;
        List<API.SalaryBill> models = new List<API.SalaryBill>();
        List<API.Pro_ProInfo> prods = new List<API.Pro_ProInfo>();
        List<SlModel.ProductionModel> proMains = new List<ProductionModel>();
        Dictionary<string, int> sellTypeDic = new Dictionary<string, int>();
        Dictionary<string, int> ProMainInfoDic = new Dictionary<string, int>();
        Dictionary<string, string> ProInfoDic = new Dictionary<string, string>();
        Dictionary<string, string> ClassInfoDic = new Dictionary<string, string>();
        Dictionary<string, string> TypeInfoDic = new Dictionary<string, string>();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ProsGrid.ItemsSource = models;

            foreach (var item in Store.SellTypes)
            {
                if (sellTypeDic.Keys.Contains(item.Name) == false)
                {
                    sellTypeDic.Add(item.Name, item.ID);
                }
            }

            //foreach (var item in Store.ProClassInfo)
            //{
            //    ClassInfoDic.Add(item.ClassName,item.ClassID.ToString());
            //}

            //foreach (var item in Store.ProTypeInfo)
            //{
            //    TypeInfoDic.Add(item.TypeName, item.TypeID.ToString());
            //}
            //foreach (var item in Store.ProMainInfo)
            //{
            //    ProMainInfoDic.Add(item.ProMainName+item.ClassID.ToString()+item.TypeID.ToString(),item.ProMainID);
            //}

            //foreach (var item in Store.ProInfo)
            //{
            //    ProInfoDic.Add(item.ProName+item.Pro_ClassID.ToString()+item.Pro_TypeID.ToString(),item.ProID);
            //}

            //初始化总商品
            #region
            var mains = from a in Store.ProMainInfo
                        join b in Store.ProInfo
                        on a.ProMainID equals b.ProMainID
                        join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                        join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                        where b.ProMainID != null
                        select new
                        {
                            a.ProMainID,
                            b.ProID,
                            c.ClassName,
                            d.TypeName,
                            a.ProMainName,
                            c.ClassID,
                            d.TypeID,
                            b.ProFormat
                        };
            foreach (var index in mains)
            {
                bool flag = false;
                foreach (var vm in proMains)
                {
                    if (vm.ProMainID == index.ProMainID)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    continue;
                }
                SlModel.ProductionModel pro = new SlModel.ProductionModel();
                pro.ProID = index.ProID;
                pro.ProMainID = Convert.ToInt32(index.ProMainID);
                pro.ProName = index.ProMainName;
                //pro.ProFormat = index.ProFormat;
                // pro.IsNeedIMEI = index.IsNeedIMEI;
                pro.ClassName = index.ClassName;
                pro.ClassID = index.ClassID.ToString();
                pro.TypeName = index.TypeName;
                pro.TypeID = index.TypeID.ToString();
                proMains.Add(pro);
            }
            #endregion

           
        }

        private void RadDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            date.Culture.DateTimeFormat.ShortDatePattern = "yyyy-MM";
        }

        #region   导入Excel

        public void import_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
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

                            int index = 0;
                            List<int> days = new List<int>();
                            List<int> rsdays = new List<int>();
                            DateTime sdate = DateTime.Parse(date.DateTimeText);//
                            int dayIndex = 8;  //提成开始所在列的索引
                            try
                            {
                                #region  读取数据
                                while (excelReader.Read())
                                {
                                    if (index == 0)
                                    {
                                        for (int i = dayIndex; i < excelReader.FieldCount; i++)
                                        {
                                            days.Add(excelReader.GetInt32(i));
                                        }
                                        days = days.Where(p => p > 0).ToList();

                                        for (int i = days.Count - 1; i >= 0; i--)
                                        {
                                            rsdays.Add(days[i]);
                                        }
                                        index++;
                                        continue;
                                    }
                                    API.SalaryBill sm = new API.SalaryBill();
                                    //string classname = excelReader.GetString(0);
                                    //string typename = excelReader.GetString(1);
                                    sm.ProID = excelReader.GetString(0);
                                    if (string.IsNullOrEmpty(sm.ProID))
                                    {
                                        sm.IsProMain = true;
                                        string proMainID = excelReader.GetString(1);
                                        try
                                        {
                                            sm.ProMainID =Convert.ToInt32( proMainID);//ProMainInfoDic[proMainName+sm.ClassID+sm.TypeID];
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Windows.MessageBox.Show("总商品名称有误:" + proMainID + "！");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        sm.IsProMain = false;
                                        
                                    }

                                    sm.Children = new List<API.SalaryBillChild>();
                                    API.SalaryBillChild child;
                                    string stName = excelReader.GetString(2);
                                    int stID = 0;
                                    try
                                    {
                                         stID = sellTypeDic[stName];
                                    }
                                    catch (Exception ez)
                                    {
                                        System.Windows.MessageBox.Show("销售类别不存在："+stName+" ！");
                                        return;
                                    }
                                    decimal ratio = Convert.ToDecimal(excelReader.GetString(3));
                                    decimal min = Convert.ToDecimal(excelReader.GetString(4));
                                    decimal max = Convert.ToDecimal(excelReader.GetString(5));
                                    decimal overRatio = Convert.ToDecimal(excelReader.GetString(6));
                                    string note = excelReader.GetString(7);

                                    for (int i = dayIndex; i < dayIndex+days.Count; i++)
                                    {
                                        child = new API.SalaryBillChild();
                                        string ss = excelReader.GetString(i);
                                        try
                                        {
                                            child.BaseSalary = Convert.ToDecimal(ss);
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Windows.MessageBox.Show("基本提成有误！");
                                            return;
                                        }
                                        child.SellTypeName = stName;
                                        child.SellTypeID = sellTypeDic[stName];
                                        child.Ratio =ratio;
                                        child.Min = min;
                                        child.Max = max;
                                        child.OverRatio = overRatio;
                                        child.Note = note;
                                        child.Year = sdate.Year;
                                        child.Month = sdate.Month;
                                        //查询在此日期前是否存在相同日期 相同日期择取下个月
                                        int count = rsdays.Skip(days.Count - (i - dayIndex) - 1).Take(i-dayIndex+1).Count(p => p == days[i - dayIndex]) - 1;
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
                                        child.Day = days[i - dayIndex];
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


                            #region  验证编码是否有误

                            List<string> selltypes = new List<string>();
                            foreach (var xx in Store.SellTypes)
                            {
                                selltypes.Add(xx.Name);
                            }

                            foreach (var item in list)
                            {
                                if (item.IsProMain == false) { continue; }
                                var main = from a in Store.ProMainInfo
                                            where a.ProMainID == item.ProMainID
                                            select a;
                                if (main.Count()==  0)
                                {
                                    System.Windows.MessageBox.Show("总商品编码有误：" + item.ProMainID + " ！");
                                    return;
                                }
                            }

                            if (list.Count() > 0)
                            {
                                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 346, new object[] { list }, new EventHandler<API.MainCompletedEventArgs>(ImportCompleted));
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
                ProsGrid.Rebind();

                GetCurrentSalary();
            }
            else
            {
                System.Windows.MessageBox.Show(e.Result.Message);
            }
        }

        #endregion 

        #region 添加属性商品

        private void load_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();  //.Where(p=>p.ProMainID==null).ToList()

            Common.CommonHelper.ProFilterGen(Store.ProInfo, ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
              pros, "ProName", new string[] { "ClassName", "TypeName", "ProName", "ProFormat" },
            new string[] { "商品类别", "商品品牌", "商品型号", "商品属性" });
            m.Tag = false; //商品
            m.Closed += msFrm_Closed;
            m.ShowDialog();
        }

        private void msFrm_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {
                List<SlModel.ProductionModel> piList = result.SelectedItems.OfType<SlModel.ProductionModel>().ToList();
                if (piList.Count == 0) return;

                List<API.SalaryBill> list = new List<API.SalaryBill>();
                foreach (var item in piList)
                {
                    if ((bool)result.Tag == false)
                    {
                        if (!ValidateProduction(item.ProID, models))
                        {
                            AddProc((bool)result.Tag, item, list);
                        }
                    }
                    else
                    {
                        if (!ValidateMainPro(item.ProMainID, models))
                        {
                            AddProc((bool)result.Tag, item, list);
                        }
                       
                    }
                    this.ProsGrid.Rebind();
                }
                if (models.Count>0)
                {
                    //获取商品当前提成
                    GetCurrentSalary();
                }
            }
        }

        private void AddProc(bool IsMainPro, ProductionModel item, List<API.SalaryBill> list)
        {
            API.SalaryBill p = new API.SalaryBill();
            p.ClassID = item.ClassID;
            p.ClassName = item.ClassName;
            p.TypeID = item.TypeID;
            p.TypeName = item.TypeName;
            p.ProName = item.ProName;
            if (IsMainPro)
            {
                p.ProMainID = item.ProMainID;
            }
            else
            {
                p.ProID = item.ProID;
            }

            p.ProFormat = item.ProFormat;
            p.IsProMain = IsMainPro;
            list.Add(p);
            models.Add(p);
        }

        /// <summary>
        /// 验证总商品是否已经存在
        /// </summary>
        /// <param name="mainid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool ValidateMainPro(int mainid, List<API.SalaryBill> list)
        {
            if (list == null)
                return false;
            foreach (var vm in list)
            {
                if (vm.ProMainID == mainid && vm.IsProMain)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ValidateProduction(string pid, List<API.SalaryBill> list)
        {
            if (list == null)
                return false;
            foreach (var vm in list)
            {
                if (vm.ProID == pid && !vm.IsProMain)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region   获取商品提成

        private void GetCurrentSalary()
        {
            if (flag == false) { return; }
            API.ReportPagingParam rpp = new API.ReportPagingParam();
            if (page == null)
            {
                rpp.PageIndex = 1;
            }
            else
            {
                rpp.PageIndex = page.PageIndex;
            }
            rpp.PageSize = (int)pagesize.Value;
            rpp.ParamList = new List<API.ReportSqlParams>();

            //PublicRequestHelp prh = new PublicRequestHelp(this.busy2, 356, new object[] { models, rpp }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
            //PublicRequestHelp prh = new PublicRequestHelp(this.busy2, 356, new object[] { models, rpp }, new EventHandler<API.MainCompletedEventArgs>(GetDetailCompleted));
            this.busy2.IsBusy = true;
            API.WebReturn ret = Store.wsclient.Main(356, new List<object>() { models, rpp });

            this.busy2.IsBusy = false;

            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;
            SalaryDetailGrid.ItemsSource = null;
            SalaryDetailGrid.Rebind();

            if (ret == null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + ret.Message);
                return;
            }
            if (ret.ReturnValue)
            {
                API.ReportPagingParam pageParam = ret.Obj as API.ReportPagingParam;
                if (pageParam == null) { return; }
                List<API.View_SalaryWithPercent> list = pageParam.Obj as List<API.View_SalaryWithPercent>;
                if (list == null) { return; }
                if (list.Count != 0)
                {
                    SalaryDetailGrid.ItemsSource = list;
                    SalaryDetailGrid.Rebind();

                    page.PageSize = (int)pagesize.Value;

                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);
                    this.page.PageIndexChanged -= page_PageIndexChanged;
                    this.page.Source = pcv;
                    this.page.PageIndexChanged += page_PageIndexChanged;
                    this.page.PageIndex = pageindex;
                }

            }
        }

        private void GetDetailCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy2.IsBusy = false;

            PagedCollectionView pcv1 = new PagedCollectionView(new string[0]);
            this.page.PageIndexChanged -= page_PageIndexChanged;
            this.page.Source = pcv1;
            this.page.PageIndexChanged += page_PageIndexChanged;
            SalaryDetailGrid.ItemsSource = null;
            SalaryDetailGrid.Rebind();

            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                API.ReportPagingParam pageParam = e.Result.Obj as API.ReportPagingParam;
                if (pageParam == null) { return; }
                List<API.View_SalaryWithPercent> list = pageParam.Obj as List<API.View_SalaryWithPercent>;
                if (list == null) { return; }
                if (list.Count != 0)
                {
                    SalaryDetailGrid.ItemsSource = list;
                    SalaryDetailGrid.Rebind();

                    page.PageSize = (int)pagesize.Value;

                    string[] data = new string[pageParam.RecordCount];
                    PagedCollectionView pcv = new PagedCollectionView(data);
                    this.page.PageIndexChanged -= page_PageIndexChanged;
                    this.page.Source = pcv;
                    this.page.PageIndexChanged += page_PageIndexChanged;
                    this.page.PageIndex = pageindex;
                }

            }

        }

        #endregion 

        #region  添加总商品

        private void loadMain_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var pros = new List<SlModel.ProductionModel>();
            var t = new List<TreeViewModel>();

            Common.CommonHelper.ProFilterGen(Store.ProInfo.Where(p => p.ProMainID != null).ToList(), ref pros, ref t);

            MultSelecter2 m = new MultSelecter2(t,
             proMains, "ProName", new string[] { "ClassName", "TypeName", "ProName" },
            new string[] { "商品类别", "商品品牌", "商品型号" });
            m.Tag = true; //总商品
            m.Closed += msFrm_Closed;
            m.ShowDialog();
        }

        #endregion 

        #region 銷售类别

        private void AddST_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (models.Count == 0)
            {
                return;
            }
            if (ProsGrid.SelectedItem == null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "请选择商品");
                return;
            }

            MultSelecter2 msFrm = new MultSelecter2(
              null, Store.SellTypes, "Name",
              new string[] { "Name" },
              new string[] { "销售类别" });
            msFrm.Closed += SellSelect_Closed;
            msFrm.ShowDialog();
        }

        private void SellSelect_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            UserMS.MultSelecter2 result = sender as UserMS.MultSelecter2;
            if (result.DialogResult == true)
            {

                List<API.Pro_SellType> piList = result.SelectedItems.OfType<API.Pro_SellType>().ToList();
                if (piList.Count == 0) return;

                foreach (var childx in ProsGrid.SelectedItems)
                {
                    SalaryHeader model = childx as SalaryHeader;
                    if (model.Children == null)
                    {
                        model.Children = new List<SalaryChild>();
                    }
                    int index = model.Children.Count + 1;
                    foreach (var item in piList)
                    {
                        bool flag = false;
                        //foreach (var child in model.Children)
                        //{
                        //    if (child.SellTypeID == item.ID)
                        //    {
                        //        flag = true;
                        //        break;
                        //    }
                        //}
                        if (flag)
                        {
                            continue;
                        }
                        SalaryChild pc = new SalaryChild();

                        //pc.SellTypeID = item.ID;
                        //pc.SellTypeName = item.Name;
                        //pc.Year = DateTime.Now.Year;
                        //pc.Month = DateTime.Now.Month;
                        //pc.Day = DateTime.Now.Day;
                        pc.ID = index;
                        if (model.Children == null)
                        {
                            model.Children = new List<SalaryChild>();
                        }
                        model.Children.Add(pc);
                        index++;
                    }
                }
                DetailGrid.ItemsSource = (ProsGrid.SelectedItems[0] as SalaryHeader).Children;
                DetailGrid.Rebind();
            }
        }

        private void DelST_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (DetailGrid.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("请选择需要删除的数据！");
                return;
            }
            SalaryHeader model = ProsGrid.SelectedItem as SalaryHeader;

            foreach (var childx in DetailGrid.SelectedItems)
            {
                SalaryChild sc = childx as SalaryChild;
                foreach (var item in model.Children)
                {
                    if (item.ID == sc.ID)
                    {
                        model.Children.Remove(item);
                        break;
                    }
                }
            }
            DetailGrid.Rebind();
            ProsGrid.Rebind();
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

            #region 验证数据
            foreach (var item in models)
            {
                if (item.Children == null)
                {
                    System.Windows.MessageBox.Show("请为商品 " + item.ProName + "添加数据！"); return;
                }
                if (item.Children.Count == 0)
                {
                    System.Windows.MessageBox.Show("请为商品 " + item.ProName + "添加数据！"); return;
                }
                foreach (var child in item.Children)
                {
                    var val = from a in item.Children
                              where a.SellTypeID == child.SellTypeID && a.ID != child.ID
                              select a;
                    if (val.Count() > 0)
                    {
                        API.SalaryBillChild sc = val.First() as API.SalaryBillChild;
                        if (sc.Year == child.Year && sc.Month == child.Month && sc.Day == child.Day)
                        {
                            System.Windows.MessageBox.Show("商品 "+item.ProName+ " 中的 "+child.SellTypeName+" 日期重复！");
                            return;
                        }
                    }
                }
            }
            #endregion 

            #region  

            API.Sys_SalaryBillInfo model = new API.Sys_SalaryBillInfo();
            model.Note = note.Text.Trim();
            
            model.Sys_SalaryBillListInfo = new List<API.Sys_SalaryBillListInfo>();
            foreach (var item in models)
            {
                foreach (var child in item.Children)
                {
                    API.Sys_SalaryBillListInfo sw = new API.Sys_SalaryBillListInfo();
                    //sw.BaseSalary = child.BaseSalary;
                    sw.Note = child.Note;
                    sw.ProID = item.ProID;
                    sw.ProMainID = item.ProMainID;
                    //sw.ClassID = Convert.ToInt32(item.ClassID);
                    //sw.Ratio = child.Ratio;
                    //sw.Day = child.Day;
                    //sw.Month = child.Month;
                    //sw.Year = child.Year;
                    sw.SellType = child.SellTypeID;
                    model.Sys_SalaryBillListInfo.Add(sw);
                }
            }

            #endregion 

            if (System.Windows.MessageBox.Show("确定提交吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            PublicRequestHelp pr = new PublicRequestHelp(this.busy, 347, new object[] { model,note.Text??"" }, SaveCompleted);
            
        }

        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Result.ReturnValue)
            {
                models.Clear();
                ProsGrid.Rebind();
                note.Text = string.Empty;
                DetailGrid.ItemsSource = null;
                DetailGrid.Rebind();
                System.Windows.MessageBox.Show(e.Result.Message);
            }
            else 
            {
                System.Windows.MessageBox.Show(e.Result.Message);
            }
        }

        #endregion 

        private void Delete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            foreach (var item in ProsGrid.SelectedItems)
	        {
                API.SalaryBill head = item as API.SalaryBill;
                foreach (var child in models)
                {
                    if (head.ProID == child.ProID && head.ProMainID == child.ProMainID)
                    {
                        models.Remove(child);
                        break;
                    }
                }
	        }
            ProsGrid.Rebind();
            if (models.Count > 0)
            {
                ProsGrid.SelectedItem = models[0];
            }
            else 
            {
                DetailGrid.ItemsSource = null;
                DetailGrid.Rebind();
            }

        }

        private void asynSalary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void ProsGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (ProsGrid.SelectedItem == null) { return; }
            API.SalaryBill head = ProsGrid.SelectedItem as API.SalaryBill;
           DetailGrid.ItemsSource = head.Children;
           DetailGrid.Rebind();
        }

        private void GridProDetail_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (ProsGrid.SelectedItem == null)
            {
                return;
            }
            SalaryHeader head = ProsGrid.SelectedItem as SalaryHeader;
            foreach (var item in head.Children)
            {
                //if (item.Ratio > 1 || item.Ratio < 0)
                //{
                //    item.Ratio = 0;
                //    System.Windows.MessageBox.Show("百分比无效！");
                //    return;
                //}
            }
        }

        private void pagesize_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            GetCurrentSalary();
        }

        private void page_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            pageindex = e.NewPageIndex;
            GetCurrentSalary();
        }


    }
}
