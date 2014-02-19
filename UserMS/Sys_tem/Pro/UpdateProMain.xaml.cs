using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls.GridView;

using UserMS.Common;
using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace UserMS.Sys_tem.Pro
{
    /// <summary>
    /// UpdateProMain.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateProMain : BasePage
    {
        API.ReportPagingParam pageParam;//全局变量分页内容
        API.View_ProMainInfo CurrentSelectedItem = null;
        public UpdateProMain()
        {
            InitializeComponent();
            Get_Class_Type();
            GetSearch();        
        }
        #region 查询
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = (int)this.pagesize.Value,
                ParamList = new List<API.ReportSqlParams>()
            };
            if (!string.IsNullOrEmpty(this.ClassName.Text))
            {
                API.ReportSqlParams_String ClassName = new API.ReportSqlParams_String();
                ClassName.ParamName = "ClassName";
                ClassName.ParamValues = this.ClassName.Text.Trim();
                pageParam.ParamList.Add(ClassName);
            }
            //在职状态查询
            if (!string.IsNullOrEmpty(this.TypeName.Text))
            {
                API.ReportSqlParams_String TypeName = new API.ReportSqlParams_String();
                TypeName.ParamName = "TypeName";
                TypeName.ParamValues = this.TypeName.Text.Trim();
                pageParam.ParamList.Add(TypeName);
            }
            if (!string.IsNullOrEmpty(this.ProName.Text))
            {
                API.ReportSqlParams_String ProName = new API.ReportSqlParams_String();
                ProName.ParamName = "ProMainName";
                ProName.ParamValues = this.ProName.Text.Trim();
                pageParam.ParamList.Add(ProName);
            }
            this.InitPageEntity(MethodIDStore.GetProMainInfo, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
        }

        private void GetSearch()
        {
            //取第一页的数据
            RadPager.PageSize = (int)this.pagesize.Value;
            pageParam = new API.ReportPagingParam()
            {
                PageIndex = 0,
                PageSize = (int)this.pagesize.Value,
                ParamList = new List<API.ReportSqlParams>()
            };
             this.InitPageEntity(MethodIDStore.GetProMainInfo, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
        }
        #endregion
        #region 更改行数
        private void pagesize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (pageParam != null)
            {
                this.RadPager.PageSize = (int)e.NewValue;
                pageParam.PageSize = RadPager.PageSize;
                this.InitPageEntity(MethodIDStore.GetProMainInfo, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
            }

        }
  
        #endregion
        #region 下一页事件
        /// <summary>
        /// 点击下一页时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadPager_PageIndexChanging_1(object sender, PageIndexChangingEventArgs e)
        {
            #region 取下一页的数据

            pageParam.PageIndex = e.NewPageIndex;
            this.InitPageEntity(MethodIDStore.GetProMainInfo, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
            #endregion
        }
        #endregion
        #region 获取商品类别和品牌
        private void Get_Class_Type()
        {
            ClassName.ItemsSource = Store.ProClassInfo;
            ClassName.DisplayMemberPath = "ClassName";
            ClassName.SelectedValuePath = "ClassID";

            TypeName.ItemsSource = Store.ProTypeInfo;
            TypeName.DisplayMemberPath = "TypeName";
            TypeName.SelectedValuePath = "TypeID";

            ProName.ItemsSource = Store.ProMainInfo;
            ProName.DisplayMemberPath = "ProMainName";
            ProName.SelectedValuePath = "ProMainID";
        }
        #endregion

        #region 提交数据
        private void Sumbit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.ProNameDG.SelectedItem==null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要删除的记录！");
                return;
            }
            if (System.Windows.MessageBox.Show("确定删除吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
            API.View_ProMainInfo MainPro=this.ProNameDG.SelectedItem as API.View_ProMainInfo;
            if (MainPro == null)
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"未选择任何项！");
                return;
            }
            API.Pro_ProMainInfo model = new API.Pro_ProMainInfo() { ProMainID = MainPro.ProMainID};
            PublicRequestHelp help = new PublicRequestHelp(this.isbusy, MethodIDStore.UpdateProMainInfo, new object[] { model ,true}, Completed);
        }
        private void Completed(object sender, API.MainCompletedEventArgs re)
        {
            this.isbusy.IsBusy = false;
            if (re.Error == null)
            {
              
                if (re.Result.Message != null)
                {
                    System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
                    Logger.Log(re.Result.Message);
                }
                if (re.Result.ReturnValue == true)
                {
                    API.View_ProMainInfo MainPro=this.ProNameDG.SelectedItem as API.View_ProMainInfo;
                    Store.ProMainInfo.RemoveAll(p => p.ProMainID == MainPro.ProMainID);
                    this.InitPageEntity(MethodIDStore.GetProMainInfo, this.ProNameDG, this.isbusy, this.RadPager, pageParam);
                }      
            }
            else
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");
        }
        #endregion 

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void ProNameDG_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            //this.ProMainPanel.DataContext = null;
            //if (ProNameDG.SelectedItem != null)
            //{
            //    API.View_ProMainInfo ProMain = this.ProNameDG.SelectedItem as API.View_ProMainInfo;
            //    this.ProMainPanel.DataContext = ProMain;
            //}
        }

        private void ProNameDG_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {

                var cell = originalSender.ParentOfType<GridViewCell>();
                //if (cell != null)
                //{
                //    MessageBox.Show("The double-clicked cell is " + cell.Value);
                //}

                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    API.View_ProMainInfo model = (API.View_ProMainInfo)row.DataContext;
                    CurrentSelectedItem = model;
                    RichTextBoxEditor r = new RichTextBoxEditor(model.Introduction);
                    
                    r.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    bool? x = r.ShowDialog();
                    if (x == true)
                    {
                        //model.Introduction = r.Introduction.Text;
                        submit_updateProMain(model,r.Introduction.Text);
                    }
                    //this.ProNameDG.Rebind();
                }
            }
        }

        private void submit_updateProMain(API.View_ProMainInfo model,string newIntroduction)
        {

            API.Pro_ProMainInfo m = new API.Pro_ProMainInfo() { ProMainID = model.ProMainID, Introduction = newIntroduction };
            PublicRequestHelp help = new PublicRequestHelp(this.isbusy, MethodIDStore.UpdateProMainInfo, new object[] { m }, Completed2);
        }

        private void Completed2(object sender, API.MainCompletedEventArgs re)
        {
            this.isbusy.IsBusy = false;
            if (re.Error == null)
            {

                if (re.Result.Message != null)
                {
                    System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, re.Result.Message);
                    Logger.Log(re.Result.Message);
                }
                if (re.Result.ReturnValue == true)
                {
                    API.Pro_ProMainInfo MainPro = (API.Pro_ProMainInfo)re.Result.Obj;
                    //Store.ProMainInfo.RemoveAll(p => p.ProMainID == MainPro.ProMainID);
                    if (this.CurrentSelectedItem != null)
                        this.CurrentSelectedItem.Introduction = MainPro.Introduction;
                    this.ProNameDG.Rebind();

                }
            }
            else
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器异常！");
        }

        private void export_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 301,new object[]{ },new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
           
        }

        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;

            if (e.Result.ReturnValue)
            {
                //string extension = "xls";
                //API.ReportPagingParam rpp = e.Result.Obj as API.ReportPagingParam;

                SlModel.operateExcel<API.View_ProMainInfo> excel = new SlModel.operateExcel<API.View_ProMainInfo>();

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "xls";
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "xls");
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("ProMainID", "总商品编码");
                        ht.Add("ClassName", "商品类别");
                        ht.Add("TypeName", "商品品牌");
                        ht.Add("ProName", "商品型号");

                        ht.Add("ProMainName", "总商品名称");
                       
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate { excel.getExcel(e.Result.Obj as List<API.View_ProMainInfo>, ht, stream); });
                        System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "导出完成");

                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("导出失败！");
            }
        }

    }

}
