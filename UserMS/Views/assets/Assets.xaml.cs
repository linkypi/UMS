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
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using UserMS.API;
using UserMS.Common;
using UserMS.Model;

namespace UserMS
{
	public partial class Assets
	{

	    private int MethodAdd = 330;
	    private int MethodDel = 331;
        private int MenuID = 336;
        private int MethodID_IMEIGetInfo = 5;
        public List<Pro_ProInfo> UserProInfos =
           CommonHelper.GetPro(336);
        public API.Pro_HallInfo Hall;
        private List<UserMS.Model.UserOpModel> UserOpList = new List<UserOpModel>();
	    private string UseUserID;
	    private string RespUserID;
	    private API.Sys_DeptInfo dept1;
        private API.Sys_DeptInfo dept2;
        private API.Sys_DeptInfo dept3;


		public Assets()
		{
			this.InitializeComponent();
            if (CommonHelper.GetHalls(336).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(336)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "权限错误");
                this.IsEnabled = false;
            } 
            this.HallName.DataContext = Hall;
			// 在此点之下插入创建对象所需的代码。
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
            this.UseUser.ItemsSource = userinfos;
            this.UseUser.TextSearchPath = "RealName";
            this.UseUser.SearchEvent = SellerSearchEvent;
            this.UseUser.SelectionMode = AutoCompleteSelectionMode.Single;
            this.UseUser.TextBox_SelectionChanged = SellerSelectEvent;


            this.RespUser.ItemsSource = userinfos;
            this.RespUser.TextSearchPath = "RealName";
            this.RespUser.SearchEvent = SellerSearchEvent2;
            this.RespUser.SelectionMode = AutoCompleteSelectionMode.Single;
            this.RespUser.TextBox_SelectionChanged = SellerSelectEvent2;
		}
        private void SellerSelectEvent(object sender, SelectionChangedEventArgs e)
        {
            Sys_UserInfo selected = UseUser.SelectedItem as Sys_UserInfo;
            if (selected != null)
            {
                this.UseUserID = selected.UserID;
            }
            else
            {
                this.UseUserID = null;
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
                    this.UseUserID = selected.UserID;
                    this.UseUser.TextBox.SearchText = selected.Username;


                }
            }

        } 

        private void SellerSelectEvent2(object sender, SelectionChangedEventArgs e)
        {
            Sys_UserInfo selected = RespUser.SelectedItem as Sys_UserInfo;
            if (selected != null)
            {
                this.RespUserID = selected.UserID;
            }
            else
            {
                this.RespUserID = null;
            }

        }

        private void SellerSearchEvent2(object sender, RoutedEventArgs routedEventArgs)
        {
            SingleSelecter w = new SingleSelecter(Common.CommonHelper.HallTreeViewModel(), UserOpList, "HallID",
                                                  "Username", new string[] { "Username", "opname" },
                                                  new string[] { "用户名", "职位" });

            w.Closed += SellerSearchWindowClose2;
            w.ShowDialog();
        }

        void SellerSearchWindowClose2(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            SingleSelecter window = sender as SingleSelecter;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    UserOpModel selected = (UserOpModel)window.SelectedItem;
                    this.RespUserID = selected.UserID;
                    this.RespUser.TextBox.SearchText = selected.Username;


                }
            }

        } 

	    private void w_OnSelectedPro(object sender, AssetSelectedProInfoArgs e)
	    {
          
	    }


	    private void Back_OnClick(object sender, RadRoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
	        throw new NotImplementedException();
	    }

	    private void New_Click(object sender, RadRoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(new Assets());
	    }

	    private void Del_Click(object sender, RadRoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
	        throw new NotImplementedException();
	    }

	    private void Next_Click(object sender, RadRoutedEventArgs e)
	    {
	        var m = this.MainPanel.DataContext as API.Asset_UseInfo;
	        if (m == null) return;
            

            if (Common.CommonHelper.ButtonNotic(m.ID==0?"下发资产":"收回资产")) return;

	        var method = m.ID == 0 ? MethodAdd : MethodDel;
	        if (m.ID == 0)
	        {
	            m.UseUser = UseUserID;
	            m.RespUser = RespUserID;
	        }
            PublicRequestHelp helper = new PublicRequestHelp(busy, method, new object[] {m }, SaveComp);


	    }

	    private void SaveComp(object sender, MainCompletedEventArgs e)
	    {
	        busy.IsBusy = false;
	        if (e.Error == null)
	        {
	            if (e.Result.ReturnValue)
	            {
	                MessageBox.Show(Application.Current.MainWindow, "保存成功");
	                this.NavigationService.Navigate(new Assets());
	            }
	            else
	            {
                    MessageBox.Show(Application.Current.MainWindow, "保存失败: " + e.Result.Message);
	            }
	        }
	        else
	        {
	            MessageBox.Show(Application.Current.MainWindow, "保存失败: 服务器错误\n" + e.Error.Message);
	        }
	    }

	    private void HallName_OnClick(object sender, RoutedEventArgs e)
	    {
            
            var AreaList = (from b in Store.ProHallInfo
                            join c in Store.AreaInfo on b.AreaID equals c.AreaID
                            select c).Distinct().ToList();
            List<TreeViewModel> parent = CommonHelper.AreaTreeViewModel(AreaList);

            SingleSelecter m = new SingleSelecter(parent, Store.ProHallInfo, "AreaID", "HallName", new string[] { "HallID", "HallName" }, new string[] { "仓库编号", "仓库名称" }, true);
            m.Closed += Hall_select_closed;
            m.ShowDialog();
	    }

	    private void Hall_select_closed(object sender, WindowClosedEventArgs e)
	    {
            SingleSelecter m = (SingleSelecter)sender;

            if (e.DialogResult == true)
            {
                this.Hall = (Pro_HallInfo)m.SelectedItem;
                this.HallName.DataContext = this.Hall;

            }
	    }


	    private void IMEISearch_OnClick(object sender, RoutedEventArgs e)
	    {
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodID_IMEIGetInfo, new object[] { this.IMEI.Text.Trim(), this.Hall.HallID, 0 }, IMEISearchComp);
	    }

        private void IMEISearchComp(object sender, MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    this.HallName.IsEnabled = false;
                    API.Pro_ProInfo i = (Pro_ProInfo)e.Result.Obj;

                    AssetSelectedProInfoArgs a = new AssetSelectedProInfoArgs(i, this.IMEI.Text.Trim(), e.Result.ArrList[0] as Pro_InOrder, e.Result.ArrList[1] as Asset_UseInfo, e.Result.ArrList[2] as Pro_InOrderList);
                    if (UserProInfos.Select(p => p.ProID).Contains(a.ProInfo.ProID))
                    {

                        if (a.AssUseInfo != null)
                        {
                            this.MainPanel.DataContext = a.AssUseInfo;
                            this.MainPanel.IsEnabled = false;
                            this.InOrderDate.Text = a.InOrder.InDate.ToString();
                            this.ProPrice.Text = a.InOrderList.Price.ToString();
                            var query1 = Store.UserInfos.Where(p => p.UserID == a.AssUseInfo.UseUser);
                            var query2 = Store.UserInfos.Where(p => p.UserID == a.AssUseInfo.RespUser);
                            if (query1.Any())
                            {
                                this.UseUser.TextBox.SearchText = query1.First().RealName;
                            }
                            if (query2.Any())
                            {
                                this.RespUser.TextBox.SearchText = query2.First().RealName;
                            }
                            var queryd1 = Store.DeptInfo.Where(p => p.DtpID == a.AssUseInfo.Dept1);
                            var queryd2 = Store.DeptInfo.Where(p => p.DtpID == a.AssUseInfo.Dept2);
                            var queryd3 = Store.DeptInfo.Where(p => p.DtpID == a.AssUseInfo.Dept3);
                            if (queryd1.Any())
                            {
                                this.Dept1.DataContext = queryd1.First();
                            }
                            if (queryd2.Any())
                            {
                                this.Dept2.DataContext = queryd2.First();
                            }
                            if (queryd3.Any())
                            {
                                this.Dept3.DataContext = queryd3.First();
                            }

                        }
                        else
                        {
                            var newinfo = new API.Asset_UseInfo();
                            newinfo.IMEI = a.IMEI;
                            newinfo.ProID = a.ProInfo.ProID;
                            newinfo.ProCount = 1;
                            newinfo.HallID = this.Hall.HallID;
                            this.MainPanel.DataContext = newinfo;
                            this.MainPanel.IsEnabled = true;
                            this.Dept1.DataContext = null;
                            this.Dept2.DataContext = null;
                            this.Dept3.DataContext = null;

                        }
                    }
                    else
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "无该商品操作权限");
                    }

                }
                else
                {
                    //Dispatcher.BeginInvoke(() => { MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message); this.ProBusy.IsBusy = false; });
                    RadWindow.Alert(new DialogParameters() { Content = "查询失败: " + e.Result.Message, Header = "错误", });
                }
            }
            else
            {
                RadWindow.Alert(new DialogParameters() { Content = "查询失败: 服务器错误\n" + e.Error.Message, Header = "错误",  });
                // Dispatcher.BeginInvoke(() =>
                //                    {
                //                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
                //                        this.ProBusy.IsBusy = false;
                //                    });
            }


            //Dispatcher.BeginInvoke(() => this.IMEI.Focus());



        }

	    private void Dept1_OnClick(object sender, RoutedEventArgs e)
	    {
            var m = this.MainPanel.DataContext as API.Asset_UseInfo;
            if (m == null) return;
            
	        SingleSelecter w = new SingleSelecter(null, Store.DeptInfo, null, "", new string[] {"DptName"}, new string[] {"名称"});
	        w.Closed += (o, args) =>
	        {
                SingleSelecter s=o as SingleSelecter;
	            if (s == null) return;
	            if (s.DialogResult == true)
	            {
	                API.Sys_DeptInfo selected = s.SelectedItem as Sys_DeptInfo;
	                if (selected == null) return;
	                this.Dept1.DataContext = selected;
	                m.Dept1 = selected.DtpID;
	            }

	        };
	        w.ShowDialog();
	    }
        private void Dept2_OnClick(object sender, RoutedEventArgs e)
        {

            var m = this.MainPanel.DataContext as API.Asset_UseInfo;
            if (m == null) return;
            SingleSelecter w = new SingleSelecter(null, Store.DeptInfo, null, "", new string[] { "DptName" }, new string[] { "名称" });
            w.Closed += (o, args) =>
            {
                SingleSelecter s = o as SingleSelecter;
                if (s == null) return;
                if (s.DialogResult == true)
                {
                    API.Sys_DeptInfo selected = s.SelectedItem as Sys_DeptInfo;
                    if (selected == null) return;
                    this.Dept2.DataContext = selected;
                    m.Dept2 = selected.DtpID;
                }

            };
            w.ShowDialog();
        }
        private void Dept3_OnClick(object sender, RoutedEventArgs e)
        {
            var m = this.MainPanel.DataContext as API.Asset_UseInfo;
            if (m == null) return;
            SingleSelecter w = new SingleSelecter(null, Store.DeptInfo, null, "", new string[] { "DptName" }, new string[] { "名称" });
            w.Closed += (o, args) =>
            {
                SingleSelecter s = o as SingleSelecter;
                if (s == null) return;
                if (s.DialogResult == true)
                {
                    API.Sys_DeptInfo selected = s.SelectedItem as Sys_DeptInfo;
                    if (selected == null) return;
                    this.Dept3.DataContext = selected;
                    m.Dept3 = selected.DtpID;
                }

            };
            w.ShowDialog();
        }
	}
}