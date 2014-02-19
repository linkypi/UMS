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
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.HallOff
{
    /// <summary>
    /// AddHallOff.xaml 的交互逻辑
    /// </summary>
    public partial class AddHallOff : BasePage
    {
        private Mul_HallFilter hAdder;
        List<API.SeleterModel> vmodels = new List<API.SeleterModel>();
        public AddHallOff()
        {
            InitializeComponent();
            InitGrid();
            this.StartTime.SelectedValue = DateTime.Now.AddMinutes(5);
            CreatName.Text = Store.LoginUserName;
            ProGrid.ItemsSource = vmodels;
            CreatTime.SelectedValue = DateTime.Now;
            hAdder = new Mul_HallFilter(ref HallGrid);
        }
        private void InitGrid()
        {
            //营业厅
            GridViewDataColumn Hallcol = new GridViewDataColumn();
            Hallcol.DataMemberBinding = new System.Windows.Data.Binding("HallName");
            Hallcol.DataMemberBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            Hallcol.Header = "营业厅";
            this.HallGrid.Columns.Add(Hallcol);
        }
        #region 商品操作
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Mul_ProductFileter ProFilter = new Mul_ProductFileter(ref vmodels, ref ProGrid,null);
            ProFilter.ProFilter((decimal) this.OffPrice.Value);
        }  
        private void DelPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelCheckedPro(ref vmodels, ref ProGrid);
        }
        #endregion

        #region 门店操作
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
             hAdder.GetHall(Store.ProHallInfo);
        }

        private void DelHall_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ViewOperate.DelHall(ref HallGrid);
        }
        #endregion 

        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.Off_AduitTypeInfo AduitType = new API.Off_AduitTypeInfo();
            AduitType.Name = this.Name.Text.Trim();
            if (string.IsNullOrEmpty(AduitType.Name)) MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入门店优惠名称");
            AduitType.Note = this.Note.Text.Trim();
            AduitType.Price = (decimal)OffPrice.Value;
            if (StartTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请填写开始时间！");
                return;
            }
            AduitType.StartDate =(DateTime)StartTime.SelectedValue;
            if (EndTime.SelectedValue == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请填写结束时间！");
                return;
            }
            AduitType.EndDate = (DateTime)EndTime.SelectedValue;
            //添加商品
            AduitType.Off_AduitProInfo = new List<API.Off_AduitProInfo>();
            if (vmodels.Count == 0) { MessageBox.Show(System.Windows.Application.Current.MainWindow, "未添加商品"); return; }
            foreach (var ProItem in vmodels)
            {
                API.Off_AduitProInfo AduitPro = new API.Off_AduitProInfo() { ProMainID = int.Parse(ProItem.ProID), SellType = ProItem.SellTypeID, Price=ProItem.Price};
                AduitType.Off_AduitProInfo.Add(AduitPro);
            }
            //添加门店
            AduitType.Off_AduitHallInfo = new List<API.Off_AduitHallInfo>();
            List<API.Pro_HallInfo> HallList= HallGrid.ItemsSource as List<API.Pro_HallInfo>;
            if (HallList == null || HallList.Count == 0) { MessageBox.Show(System.Windows.Application.Current.MainWindow, "未添加营业厅"); return; }
            foreach (var HallItem in HallList)
            {
                API.Off_AduitHallInfo AduitHall = new API.Off_AduitHallInfo() { HallID = HallItem.HallID,};
                AduitType.Off_AduitHallInfo.Add(AduitHall);
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定新增门店优惠？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            PublicRequestHelp h = new PublicRequestHelp(busy, MethodIDStore.AddHallOff, new object[] { AduitType }, MyClient_Completed);
        }
        protected void MyClient_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.ReturnValue == true)
                    {
                        Clean();
                    }
                    Logger.Log(e.Result.Message + "");
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, e.Result.Message);
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务端无法返回有效值");
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                this.busy.IsBusy = false;
            }
        }

        private void ProGrid_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
         
            API.SeleterModel ProItem = this.ProGrid.SelectedItem as API.SeleterModel;
            if (ProItem.Price < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "门店优惠限额不能为负数");
                ProItem.Price = 0;
                return;
            }
            if (e.Cell.Column.Header.ToString() == "门店优惠限额")
            {
                int value = (int)(ProItem.Price * 100);
                ProItem.Price = (decimal)(value * 0.01);
            }
        }
        #region 清除所有数据 
        private void Clean()
        {
            this.Name.Text = string.Empty;
            this.EndTime.SelectedValue = null;
            OffPrice.Value = 0;
            this.StartTime.SelectedValue = DateTime.Now.AddMinutes(5);
            CreatTime.SelectedValue = DateTime.Now;
            Note.Text = string.Empty;
            vmodels.Clear();
            ProGrid.Rebind();
            HallGrid.ItemsSource = null;
            HallGrid.Rebind();
        }
        #endregion 
        #region 重置所有
        private void RadMenuItem_Click_3(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow, "确定重置所有？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            Clean();
        }
        #endregion

    }
}
