using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
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
using Telerik.Windows;
using Telerik.Windows.Controls.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.SMS
{
    /// <summary>
    /// SmsSend.xaml 的交互逻辑
    /// </summary>
    public partial class SmsSend : Page
    {
        private Pro_HallInfo Hall;
        private int SignAdd_MethodID = 311;
        private int SignGet_MethodID = 312;
        private int SignpayAdd_MethodID = 313;
        private int SignpayDel_MethodID = 314;
        public List<Pro_ProInfo> UserProInfos =
            CommonHelper.GetPro(299);
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public SmsSend()
        {
            InitializeComponent();
            if (CommonHelper.GetHalls(299).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(299)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "权限错误");
                this.IsEnabled = false;
            } 
            this.HallName.DataContext = Hall;
            
            this.New.IsEnabled = true;
            this.Next.IsEnabled = false;
            this.Open.IsEnabled = true;
            this.Add.IsEnabled = false;
            this.Del.IsEnabled = false;
            MainPanel.IsEnabled = false;


            var userops = Store.UserOpList.Where(p => p.Flag == true && p.OpID != null && p.HallID != null);
            UserOpList =
    userops.Join(Store.UserInfos, oplist => oplist.UserID, info => info.UserID,
                          (list, info) => new { op = list, user = info })
         .Join(Store.UserOp, arg => arg.op.OpID, op => op.OpID, (a, t) => new UserOpModel()
         {
             ID = a.op.ID,
             HallID = a.op.HallID,
             OpID = a.op.OpID,
             UserID = a.op.UserID,
             Username = a.user.RealName,
             opname = t.Name
         }).ToList();
            var userinfos = Store.UserInfos.Join(UserOpList, info => info.UserID, model => model.UserID,
                                                 (info, model) => info).ToList();
            this.Seller.ItemsSource = userinfos;
            this.Seller.TextSearchPath = "RealName";
            this.Seller.SearchEvent = SellerSearchEvent;
            this.Seller.SelectionMode = AutoCompleteSelectionMode.Single;
            this.Seller.TextBox_SelectionChanged = SellerSelectEvent;
        }
        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
            Sys_UserInfo selected = Seller.SelectedItem as Sys_UserInfo;
            if (selected != null)
            {
                this.SellerID = selected.UserID;
            }
            else
            {
                this.SellerID = null;
            }

        }

        private void SellerSearchEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
                                                  "Username", new string[] { "Username", "opname" },
                                                  new string[] { "用户名", "职位" });

            w.Closed += SellerSearchWindowClose;
            w.ShowDialog();
        }

        private string SellerID;
        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    UserOpModel selected = (UserOpModel)window.SelectedItem;
                    this.SellerID = selected.UserID;
                    this.Seller.TextBox.SearchText = selected.Username;


                }
            }

        } 

        private void New_Click(object sender, RadRoutedEventArgs e)
        {
            MainPanel.IsEnabled = true;
            this.DataContext = new SMS_SignInfo(){HallID = this.Hall.HallID};
            this.Seller.Text = "";
            
            this.Next.IsEnabled = true;
            this.Add.IsEnabled = false;
            this.Del.IsEnabled = false;
        }

        private void Next_Click(object sender, RadRoutedEventArgs e)
        {
        //TODO: 验证数据有效性

            var smss = this.DataContext as SMS_SignInfo;

            if (
                smss==null||
                string.IsNullOrEmpty(smss.OldSellID) ||
               


                string.IsNullOrEmpty(smss.Industry) ||
                smss.SignDate == null ||
                string.IsNullOrEmpty(smss.CpcName) ||
                string.IsNullOrEmpty(smss.CpcAdd) ||
                string.IsNullOrEmpty(smss.SMSContent) ||
                smss.SignPay <= 0 ||
                smss.SignCount <= 0 ||

                smss.PayAllDate == null ||
                smss.RealPayAllDate == null ||
                smss.PayBack == 0 ||
                string.IsNullOrEmpty(smss.CusName) ||
                string.IsNullOrEmpty(smss.CusPhone) ||
                string.IsNullOrEmpty(smss.BillHeader) ||
                string.IsNullOrEmpty(smss.BillNum) ||
                smss.BillDate == null




                )
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "请填写必填字段");
                return;
            }

            if (string.IsNullOrEmpty(SellerID))
            {
                try
                {
                    smss.Sellor = Store.UserInfos.Where(p => p.RealName == Seller.Text).First().UserID;
                }
                catch (Exception)
                {

                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "销售员不存在");
                    return;
                }

            }
            else
            {
                smss.Sellor = SellerID;
            }

            if (Common.CommonHelper.ButtonNotic(sender)) return;
          var a=  new PublicRequestHelp(this.busy, SignAdd_MethodID, new object[] {smss}, MainEvent);
        }

        private void MainEvent(object sender, MainCompletedEventArgs e)
        {
             this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    MessageBox.Show(Application.Current.MainWindow, "保存成功");
                    this.DataContext = e.Result.Obj;
                    this.MainPanel.IsEnabled=false;
                    this.Add.IsEnabled = true;
                    this.Next.IsEnabled = false;
                    this.Del.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "保存失败");
                }
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "保存失败, 服务器错误\n"+e.Error.Message);
            }
        }

        private void Open_Click(object sender, RadRoutedEventArgs e)
        {
            SellIDWin win=new SellIDWin();
            win.Owner = Application.Current.MainWindow;
            win.Closed += win_Closed;
            win.ShowDialog();

        }

        void win_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SellIDWin win =sender as SellIDWin;
            if (win == null) return;
            if (win.DialogResult == true)
            {
                var a = new PublicRequestHelp(this.busy, SignGet_MethodID, new object[] {win.SellID.Text.Trim()},
                    GetSellID_Comp);
            }
        }

        private void GetSellID_Comp(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    MainPanel.IsEnabled = true;
                    var b = e.Result.Obj as SMS_SignInfo;
                   
                    this.DataContext = e.Result.Obj;
                    this.Seller.Text =Store.UserInfos.Where(p => p.UserID == b.UserID).First().RealName ;
                    this.Next.IsEnabled = false;
                    this.MainPanel.IsEnabled = false;
                    if (b.IsOver == true)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "该合同已经完结");
                        this.Add.IsEnabled = false;
                        this.Del.IsEnabled = false;
                    }
                    else
                    {
                        this.Add.IsEnabled = true;
                        this.Del.IsEnabled = true;
                    }
                 


                    
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "获得失败");
                }
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "获得失败, 服务器错误\n" + e.Error.Message);
            }
        }

        private void Add_Click(object sender, RadRoutedEventArgs e)
        {
            SMSAdd win =new SMSAdd();
            win.Owner = Application.Current.MainWindow;
            win.Closed += sms_add_close;
            win.ComboBox.ItemsSource = UserProInfos;

            win.ShowDialog();
        }

        void sms_add_close(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
             SMSAdd win=sender as SMSAdd;
            if (win == null) return;
            if (win.DialogResult == true)
            {
                SMS_SignSendPayInfo model=new SMS_SignSendPayInfo();
                model.SellID = ((SMS_SignInfo) this.DataContext).ID;
                model.ProID = win.ComboBox.SelectedValue.ToString();
                model.RealPay = Convert.ToDecimal(win.Pay.Value);
                model.RealCount = Convert.ToDecimal(win.Send.Value);
                if (string.IsNullOrEmpty(win.SellerID))
                {
                    try
                    {
                        model.Receiver = Store.UserInfos.Where(p => p.RealName == win.Seller.Text).First().UserID;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "销售员不存在");
                        return;
                    }

                }
                else
                {
                    model.Receiver = SellerID;
                }
                var a = new PublicRequestHelp(this.busy, SignpayAdd_MethodID, new object[] {model}, SignPayAdd_Comp);
            }

        }

        private void SignPayAdd_Comp(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    MessageBox.Show(Application.Current.MainWindow, "保存成功");
                    this.DataContext = e.Result.Obj;
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "保存失败");
                }
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "保存失败, 服务器错误\n" + e.Error.Message);
            }
        }


        private void Del_Click(object sender, RadRoutedEventArgs e)
        {
            SMS_SignSendPayInfo model=Grid.SelectedItem as SMS_SignSendPayInfo;
            if (model == null)
            {
                MessageBox.Show(Application.Current.MainWindow, "未选择");
                return;
            }
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            var a = new PublicRequestHelp(this.busy, SignpayDel_MethodID, new object[] { model }, SignPayDel_Comp);
        }

        private void SignPayDel_Comp(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue == true)
                {
                    MessageBox.Show(Application.Current.MainWindow, "保存成功");
                    this.DataContext = e.Result.Obj;
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "保存失败");
                }
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "保存失败, 服务器错误\n" + e.Error.Message);
            }
        }
    }
}
