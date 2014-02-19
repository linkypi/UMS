using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS.Views.UsersellMS.Usercenter
{
    public partial class UserRegister : BasePage
    {
        int Method_GetService = 40;
        int Mehtod_Register = 41;
        string r = "";
        HallFilter hAdder;

        //仓库添加器
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
        public UserRegister()
        {
            InitializeComponent();
  
            this.Sumbit.Click += Sumbit_Click;
            this.tbcardid.LostFocus += tbcardid_LostFocus;
            this.dtjoinTime.SelectedValue = DateTime.Now;
            API.Sys_UserInfo user = Store.LoginUserInfo;
      
            //按下取消键时发生
            this.Cancel.Click += Cancel_Click;
            //为Commonbox添加资源
            tbIDtype.ItemsSource = Store.CardType;
            tbIDtype.SelectedValuePath = "ID";
            tbIDtype.DisplayMemberPath = "Name";
            //按下添加仓库时发生
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
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
            }
            catch
            {
                r = "16";
            }
        }

        #region 获取揽装人

        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
            Sys_UserInfo selected = Seller.SelectedItem as Sys_UserInfo;
            if (selected != null)
            {
                this.Seller.Tag = selected.UserID;
            }
            else
            {
                this.Seller.Tag = null;
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

        void SellerSearchWindowClose(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    UserOpModel selected = (UserOpModel)window.SelectedItem;
                    this.Seller.Tag = selected.UserID;
                    this.Seller.TextBox.SearchText = selected.Username;
                }
            }

        } 

        #endregion



        #region 清空所有数据
        void AllClean()
        {
            this.TbHall.Text = string.Empty;
            this.TbHall.Tag = null;
            this.TbOldID.Text = string.Empty;
            this.tbcardid.Text = string.Empty;
            this.tbName.Text = string.Empty;
            this.cbsex.Text = string.Empty;
            this.birthday.SelectedValue = null;
            this.phoneNum.Text = string.Empty;
            this.telephone.Text = string.Empty;
            this.QQ.Text = string.Empty;
            this.tbaddress.Text = string.Empty;
            this.tbIDtype.Text = string.Empty;
            this.tbIDnum.Text = string.Empty;
            this.dtjoinTime.SelectedValue = DateTime.Now;
            this.NewCardPanel.DataContext = null;
            this.Seller.Tag = null;
            this.Seller.TextBox.SearchText = string.Empty;

            DGservice.ItemsSource = null;
            DGservice.Rebind();
        }
        #endregion 
        #region 清空部分数据
        void PartClean()
        {     
            this.tbcardid.Text = string.Empty;
            this.NewCardPanel.DataContext = null;
            DGservice.ItemsSource = null;
            DGservice.Rebind();
        }
        #endregion 
        void Cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否清空所有数据？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                AllClean();
            }
        }
        #region  失去焦点时发生
        /// <summary>
        /// 离开卡号文本框发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tbcardid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbcardid.Text))
            {
                PublicRequestHelp help = new PublicRequestHelp(this.busy, Method_GetService, new object[] { tbcardid.Text.Trim() }, SubmitCompleted);
            }
        }
        void SubmitCompleted(object sender, API.MainCompletedEventArgs re)
        {
            this.busy.IsBusy = false;
            if (re.Error == null)
            {
                Logger.Log(re.Result.Message);
              //  MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message);
                if (re.Result.ReturnValue == true)
                {
                    API.VIP_VIPType TypeInfo = re.Result.Obj as API.VIP_VIPType;
                    this.NewCardPanel.DataContext = TypeInfo;
                    if (TypeInfo.VIP_VIPTypeService != null)
                    {
                        new GetTypeService(TypeInfo.VIP_VIPTypeService, ref DGservice);
                       
                        if (re.Result.ArrList[0] != null)
                        {
                            try
                            {
                                TbHall.Tag = re.Result.ArrList[0].ToString();
                                string query = (from b in Store.ProHallInfo
                                                where b.HallID == re.Result.ArrList[0].ToString()
                                                select b.HallName).First();
                                TbHall.Text = query;
                            }
                            catch
                            {
                                MessageBox.Show(System.Windows.Application.Current.MainWindow,"初始化数据中没有该卡仓库！");
                                return;
                            }
                        }

                    }
                }
                else
                {
                    PartClean();
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,re.Result.Message + "");
                }
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务器异常！");    
        }
        #endregion
        #region 注册会员
        void Sumbit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.VIP_VIPInfo_Temp vipinfo = new API.VIP_VIPInfo_Temp();
            if (this.tbcardid.Text == null || this.tbcardid.Text.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"卡号不能为空");
                return;
            }
            if (this.tbName.Text == null || this.tbName.Text.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"会员姓名不能为空");
                return;
            }

            if (!string.IsNullOrEmpty(this.cbsex.Text))
            {
                vipinfo.Sex = this.cbsex.Text.Trim();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员性別不能为空");
                return;
            }

  
            if (this.birthday.SelectedValue != null)
            {
                vipinfo.Birthday = this.birthday.SelectedValue;
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "会员生日不能为空");
                return;
            }

       
            if (string.IsNullOrEmpty(this.phoneNum.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写手机号码");
                return;
            }
            if (!PormptPage.isNumeric(phoneNum.Text.Trim()))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写正确的手机号码！");
                return;
            }


            if (tbIDtype.SelectedValue != null)
            {
                vipinfo.IDCard_ID = (int)tbIDtype.SelectedValue;
            }

            if (this.dtjoinTime == null || this.dtjoinTime.SelectedValue.ToString().Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择加入日期");
                return;
            }
            if (this.TbHall.Text.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写揽装门店！");
                return;
            }
            if (string.IsNullOrEmpty(Seller.Text))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写揽装人名称");
                return;
            }
       
            vipinfo.IMEI = this.tbcardid.Text.Trim();
            vipinfo.MemberName = this.tbName.Text.Trim();
               
            vipinfo.MobiPhone = this.phoneNum.Text.Trim();
            vipinfo.TelePhone = this.telephone.Text.Trim();
            vipinfo.QQ = this.QQ.Text.Trim();
            vipinfo.Address = this.tbaddress.Text.Trim();
            vipinfo.IDCard = this.tbIDnum.Text.Trim();
            vipinfo.HallID = this.TbHall.Text.Trim();
            vipinfo.OldID = this.TbOldID.Text.Trim();
            //添加仓库ID
            string hallID= TbHall.Tag.ToString();
            vipinfo.HallID = hallID;
            vipinfo.Note = TbNote.Text.Trim();
            try
            {
                vipinfo.LZUser = Seller.Tag.ToString();
            }
            catch
            {
                if (string.IsNullOrEmpty(Seller.Text))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入揽装人！");
                    return;
                }
                var query = from b in Store.UserInfos
                            where b.UserName == Seller.Text
                            select b;
                if (query.Count() == 0 || string.IsNullOrEmpty(query.First().UserID))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "不存在揽装人！");
                    return;
                }
            }

           
            vipinfo.StartTime = this.dtjoinTime.SelectedValue;
            if (this.cardtype == null || this.cardtype.Text.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该卡号不存在");
                return;
            }
            API.VIP_VIPType gettype = Store.VIPType.Where(type => type.Name == this.cardtype.Text.Trim()).First();
            API.Sys_UserInfo user = Store.LoginUserInfo;
            vipinfo.Validity = gettype.Validity;
            vipinfo.TypeID = gettype.ID;
            vipinfo.Point = gettype.SPoint;
           
            vipinfo.ProPrice = gettype.Cost_production;
            PublicRequestHelp help = new PublicRequestHelp(this.busy, Mehtod_Register, new object[] {vipinfo}, BackResults);
 
        }
        void BackResults(object sender, API.MainCompletedEventArgs results)
        {
            this.busy.IsBusy = false;
            if (results.Result.Message != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,results.Result.Message);
            }
            if (results.Result.ReturnValue == true)
            {
                AllClean();
            }
            Logger.Log(results.Result.Message);
        }
        #endregion

    }
}
