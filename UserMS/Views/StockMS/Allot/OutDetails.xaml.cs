using System;
using System.Collections.Generic;
using System.Windows;

namespace UserMS.Views.StockMS.Allot
{
    public partial class OutDetails
    {
      
        int Method_List = 69;
        int Method_CM = 70;
        int Method_Cancel = 26;
        int Method_Accept = 19;
        List<API.View_OutOrderList> outList;
        int OperateType = 0;
        bool Aduited = false;
        public OutDetails()
        {
        }
    /// <summary>
    /// 操作入口
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        void Cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
          
            if(Aduited==true)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"调拨单已接收,无法执行其他操作");
                return;
            }
            switch (OperateType)
            {
                case 0://不执行
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"操作失败，请重新操作");
                    break;
                case 1://执行取消操作
                    {
                        if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定执行取消操作吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            Cancle();
                        }                       
                        break;
                    }
                case 2://执行接收操作
                    {
                        if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定执行接收操作吗？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            Accept();
                        }
                        break;
                    }
            }   
           
        }
      
        private void BackResult(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
            }
        }
        #region 取消操作
        void Cancle()
        {
            if (outList == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无法取消，请联系管理员");
                return;
            }
            API.Pro_OutInfo model = new API.Pro_OutInfo();
            model.OutOrderID = PKid.Text.Trim();
            model.Audit = Aduited;
            model.FromHallID = DRHall.Tag.ToString();
            model.Pro_OutOrderList = new List<API.Pro_OutOrderList>();
            foreach (var index in outList)
            {
                API.Pro_OutOrderList ListModel = new API.Pro_OutOrderList();
                ListModel.InListID = index.InListID;
                ListModel.ProID = index.ProID;
                ListModel.ProCount = index.ProCount;
                model.Pro_OutOrderList.Add(ListModel);
            }
            PublicRequestHelp help = new PublicRequestHelp(this.busy, Method_Cancel, new object[] { model }, BackResult);
        }
        #endregion
        #region 接收操作
        void Accept()
        {
            if (outList == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无法取消，请联系管理员");
                return;
            }
            API.Pro_OutInfo model = new API.Pro_OutInfo();
            model.Audit = Aduited;
            model.OutOrderID = PKid.Text.Trim();
            model.Pro_HallID = DCHall.Tag.ToString();
            model.Pro_OutOrderList = new List<API.Pro_OutOrderList>();
            foreach (var index in outList)
            {
                API.Pro_OutOrderList ListModel = new API.Pro_OutOrderList();
                ListModel.OutListID = index.OutListID;
                ListModel.InListID = index.InListID;
                ListModel.ProID = index.ProID;
                ListModel.ProCount = index.ProCount;
                model.Pro_OutOrderList.Add(ListModel);
            }
            PublicRequestHelp help = new PublicRequestHelp(this.busy, Method_Accept, new object[] { model }, BackResult);
        }
        #endregion
        #region 构造函数 获取调拨单
        public OutDetails(API.Pro_OutModel outModel,int TypeID)
        {
            InitializeComponent();
            outList = new List<API.View_OutOrderList>();
            OperateType = TypeID;

            PKid.Text = outModel.OutOrderID;
            send.Text = outModel.FromUserName;
            if (outModel.ToUserName != null)
                recv.Text = outModel.ToUserName;
            //如果该单已经接收，则赋值给判断变量
            if (outModel.Aduit == "Y")
            {
                Aduti.Text = "已接收";
                Aduited = true;
            }
            else
                Aduti.Text = "未接收";
            if (outModel.FromHallName != null)
            {
                DCHall.Text = outModel.FromHallName;
                DCHall.Tag = outModel.FromHallID;
            }
            if (outModel.Pro_HallName != null)
            {
                DRHall.Text = outModel.Pro_HallName;
                DRHall.Tag = outModel.Pro_HallID;
            }
            if (outModel.OutDate != null)
                date.SelectedValue = outModel.OutDate;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, Method_List, new object[] { outModel.ID }, BackOutList);
        }
        private void BackOutList(object sender, API.MainCompletedEventArgs e)
        {

            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {

                    outList = e.Result.Obj as List<API.View_OutOrderList>;
                    this.dataGrid1.ItemsSource = e.Result.Obj;
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常");
            }
        }
        #endregion
        #region 显示串码
        private void dataGrid1_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            API.View_OutOrderList model = dataGrid1.SelectedItem as API.View_OutOrderList;
            if (model.NeedIMEI == true)
            {
                PublicRequestHelp help = new PublicRequestHelp(this.busy, Method_CM, new object[] { model.OutListID }, BackOutIMEI);
            }
            else
            {
                this.dataGrid2.ItemsSource = null;
                dataGrid2.Rebind();
            }
        }
        private void BackOutIMEI(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    this.dataGrid2.ItemsSource = e.Result.Obj;
                    dataGrid2.Rebind();
                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常");
            }
        }
        #endregion
    }
}