using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace UserMS.Views.StockMS.Borrowing
{
    public partial class InputAduit : RadWindow
    {
        public API.Pro_BorowAduit am;
        public List<API.BorowListModel> models;

        public InputAduit()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            this.aduitid.SelectAll();
            this.OKButton.KeyDown += OKButton_KeyDown;
            this.aduitid.KeyDown += OKButton_KeyDown;
        }

        public new void Show()
        {
            base.Show();
            this.aduitid.SelectAll();
        }

        void OKButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                if (string.IsNullOrEmpty(this.aduitid.Text.ToString().Trim()))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入审批单");
                    return;
                }
                //AduitID = this.aduitid.Text.ToString().Trim();
                ValidateAduitID();
                this.DialogResult = true;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.aduitid.Text.ToString().Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入审批单");
                return;
            }
            //AduitID = this.aduitid.Text.ToString().Trim();
            ValidateAduitID();
           this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// 验证审批单是否有效
        /// </summary>
        private void ValidateAduitID()
        {
            PublicRequestHelp prh = new PublicRequestHelp(this.isbusy, 72, new object[] { this.aduitid.Text.ToString().Trim() }, new EventHandler<API.MainCompletedEventArgs>(ValidateCompleted));
        }

        private void ValidateCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                am = e.Result.Obj as API.Pro_BorowAduit;
                models = new List<API.BorowListModel>();

                //List<API.BorowListModel> list 
                models = e.Result.ArrList[0] as List<API.BorowListModel>;

                //SlModel.ViewModel vm = null; 
                //foreach(var item in list)
                //{
                //    vm = new SlModel.ViewModel();
                //    vm.ProClassName = item.ProClassName;
                //    vm.ProName = item.ProName;
                //    vm.ProTypeName = item.ProTypeName;
                //    vm.AduitCount =(int) item.ProCount;
                //    vm.ProFormat = item.ProFormat;
                //    vm.ProID = item.ProID;
                //    vm.IsNeedIMEI = item.NeedIMEI;
                //    models.Add(vm);
                //}
                this.Close();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
            this.DialogResult = e.Result.ReturnValue;
           
        }
    }
}

