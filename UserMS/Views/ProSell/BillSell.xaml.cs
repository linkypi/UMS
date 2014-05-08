using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UserMS.API;
using UserMS.Common;
using UserMS.Views.ProSell;
using UserMS.Views.ProSell.YanBao;
using Label = System.Windows.Controls.Label;

namespace UserMS
{
	public partial class BillSell
	{
        private int MethodID_IMEIGetInfo = 5;
        private int CheckIMEI_MethodID = 109;
        public List<Pro_ProInfo> UserProInfos = CommonHelper.GetPro(344);
        private int MenuID = 344;
        public API.Pro_HallInfo Hall;
	    private bool jump = false;
        private int MethodID_SellNext = 153;
        private ObservableCollection<Pro_BillInfo_temp> BillLists = new ObservableCollection<Pro_BillInfo_temp>();
		public BillSell()
		{
			this.InitializeComponent();
            if (CommonHelper.GetHalls(344).Count >= 1)
            {
                Hall = CommonHelper.GetHalls(344)[0];
            }
            else
            {
                //HallName_OnMouseLeftButtonUp(null, null);
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "权限错误");
                this.IsEnabled = false;
            }
            this.HallName.DataContext = Hall;
		    this.dataGrid.ItemsSource = BillLists;
		    // 在此点之下插入创建对象所需的代码。
		}

	    public override string ToString()
	    {
	        return "协议销售";
	    }

	    private void Back_OnClick(object sender, RadRoutedEventArgs e)
	    {
	       // throw new NotImplementedException();
	    }

	    private void New_Click(object sender, RadRoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            this.NavigationService.Navigate(new BillSell());
	    }

	    private void Del_Click(object sender, RadRoutedEventArgs e)
	    {

            if (Common.CommonHelper.ButtonNotic(sender)) return;
            if (this.dataGrid.SelectedItem != null)
            {
                this.BillLists.Remove((Pro_BillInfo_temp) this.dataGrid.SelectedItem);

            }
     
	    }

	    private void Next_Click(object sender, RadRoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            jump = true;
            SaveSellInfos();
	    }

	    private void IMEISearch_OnClick(object sender, RoutedEventArgs e)
	    {
	        this.IMEI.Text = this.IMEI.Text.ToUpper();
            PublicRequestHelp helper = new PublicRequestHelp(busy, MethodID_IMEIGetInfo, new object[] { this.IMEI.Text.Trim(), this.Hall.HallID, 0 }, IMEISearchComp);
	    }

	    private void IMEISearchComp(object sender, MainCompletedEventArgs e)
	    {
	        
             this.busy.IsBusy = false;
	        if (e.Error == null)
	        {
	            if (e.Result.ReturnValue)
	            {
                    API.Pro_ProInfo i = (Pro_ProInfo)e.Result.Obj;
	               
                    if (UserProInfos.Select(p => p.ProID).Contains(i.ProID))
	                {
                        var query = Store.BillFields.Where(p => p.ProID == i.ProID);
	                    if (query.Any())
	                    {
	                        foreach (var proBillFieldInfo in query)
	                        {

	                            var num = proBillFieldInfo.BillFieldName.Replace("BillField", "");
	                            var obj = this.FindName("FieldLabel" + num) as Label;
	                            if (obj != null)
	                            {
	                                obj.Content = proBillFieldInfo.BillFieldValue;
	                                var obj2 = this.FindName("StackPanel" + num) as StackPanel;
	                                if (obj2 != null)
	                                {
	                                    obj2.Visibility = Visibility.Visible;
	                                }
	                            }
	                        }


	                    }
	                    else
	                    {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow, "未配置合同内容");
	                        return;

	                    }
                        var newinfo = new API.Pro_BillInfo_temp();
	                    newinfo.ProID = i.ProID;
	                    newinfo.BillIMEI = IMEI.Text.Trim();
	                    this.MainPanel.DataContext = newinfo;
	                    this.IMEI.IsReadOnly = true;
	                    BillName.Text = i.ProName;
	                }
	                else
	                {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "无该商品操作权限");
	                }

	            }
	            else
	            {
	                //Dispatcher.BeginInvoke(() => { MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: " + e.Result.Message); this.ProBusy.IsBusy = false; });
	                RadWindow.Alert(new DialogParameters() {Content = "查询失败: " + e.Result.Message, Header = "错误",});
	            }
	        }
	        else
	        {
	            RadWindow.Alert(new DialogParameters() {Content = "查询失败: 服务器错误\n" + e.Error.Message, Header = "错误",});
	            // Dispatcher.BeginInvoke(() =>
	            //                    {
	            //                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"查询失败: 服务器错误\n" + e.Error.Message);
	            //                        this.ProBusy.IsBusy = false;
	            //                    });
	        }
	    }


	    private void Save_Click(object sender, RadRoutedEventArgs e)
	    {
            if (Common.CommonHelper.ButtonNotic(sender)) return;
            jump = false;
            SaveSellInfos();
	    }

	    private void SaveSellInfos()
	    {
            

	        var selllists = new List<Pro_SellListInfo_Temp>();
	        foreach (var proBillInfoTemp in BillLists)
	        {
                API.Pro_SellListInfo_Temp sellList = new Pro_SellListInfo_Temp();
                var model = proBillInfoTemp;

                var query = Store.BillFields.Where(p => p.ProID == model.ProID);
                if (query.Any())
                {
                    foreach (var proBillFieldInfo in query)
                    {

                        var num = proBillFieldInfo.BillFieldName;
                        var val=model.GetType().GetProperty(num).GetValue(model,null)+"";
                        
                            if (string.IsNullOrEmpty(val))
                            {
                                MessageBox.Show(System.Windows.Application.Current.MainWindow, "有字段未填完");
                                return;
                            }
                       
                    }


                }

                sellList.HallID = this.Hall.HallID;
                sellList.ProID = model.ProID;
                sellList.IMEI = model.BillIMEI;
                sellList.ProCount = 1;
                sellList.Pro_BillInfo_temp = model;
                sellList.InsertDate = DateTime.Now;
                sellList.SellType = 1;
                selllists.Add(sellList);
	        }



            PublicRequestHelp a = new PublicRequestHelp(this.busy, this.MethodID_SellNext, new object[] { selllists }, SellNextComp);
	    }

	    private void SellNextComp(object sender, MainCompletedEventArgs e)
	    {
            this.busy.IsBusy = false;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    //                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"保存成功");
                    //                    this.Yanbaomodellist.Clear();
                    //                    this.dataGrid.Rebind();
                    //                    this.clearform();
                    List<API.Pro_SellListInfo_Temp> models = (List<Pro_SellListInfo_Temp>)e.Result.Obj;
                    if (jump)
                    {
                        var newpage = new NewProSell(models);
                        newpage.oldpage = new BillSell();
                        this.NavigationService.Navigate(newpage);
                    }
                    else
                    {
                        this.NavigationService.Navigate(new BillSell());
                    }
                    //                    this.Content = newpage;
                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存失败: " + e.Result.Message);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "保存失败: 服务器错误\n" + e.Error.Message);
            }
	    }

	    private void _Add_OnClick(object sender, RoutedEventArgs e)
        {
            var model = this.MainPanel.DataContext as API.Pro_BillInfo_temp;
	        if (model == null) return;
	        if (Pro == null)
	        {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未验证终端串码");
	            return;
	        };
	        BillLists.Add(model);
            clearform();
        }

	    private void _Cancel_OnClick(object sender, RoutedEventArgs e)
	    {
	        clearform();
	    }

	    private void clearform()
	    {
	        this.MainPanel.DataContext = null;
	        for (int i = 1; i < 11; i++)
	        {
                var obj2 = this.FindName("StackPanel" + i) as StackPanel;
                if (obj2 != null)
                {
                    obj2.Visibility = Visibility.Collapsed;
                }
	        }
	        this.IMEI.IsReadOnly = false;
	        this.IMEI.Text = "";
            this.MobileIMEI.IsReadOnly = false;
	        this.Pro = null;
	        BillName.Text = "";
	        MobileName.Text = "";
	        ModelClass.Text = "";
	        
	    }

	    private void MobileIMEISearch_Onclick(object sender, RoutedEventArgs e)
	    {
            var model = this.MainPanel.DataContext as API.Pro_BillInfo_temp;
	        if (model == null)
	        {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未验证合同串码");
                return;
	        }
            this.MobileIMEI.Text = this.MobileIMEI.Text.ToUpper();
            PublicRequestHelp helper = new PublicRequestHelp(busy, CheckIMEI_MethodID, new object[] { this.MobileIMEI.Text.Trim() }, CheckIMEI_Completed);
	    }

	    private Pro_ProInfo Pro;
        private void CheckIMEI_Completed(object sender, MainCompletedEventArgs e)
        {
           
            this.busy.IsBusy = false;
            var model = this.MainPanel.DataContext as API.Pro_BillInfo_temp;
            if (model == null)
                return;
            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    try
                    {
                        API.Pro_IMEI imeiinfo = e.Result.ArrList[0] as Pro_IMEI;
                        API.Pro_SellListInfo selllist = e.Result.ArrList[1] as Pro_SellListInfo;
                        if (imeiinfo != null)
                        {
                            this.Pro = Store.ProInfo.First(p => p.ProID == imeiinfo.ProID);

                            this.MobileName.Text = Pro.ProName;


                            this.ModelClass.Text =
                                Store.ProClassInfo.First(p => p.ClassID == Pro.Pro_ClassID).ClassName;
                            this.MobileIMEI.IsReadOnly = true;
                            model.MobileProID = this.Pro.ProID;
                            model.MobileClassID = this.Pro.Pro_ClassID;
                        }
                        else
                        {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 串码不存在");
                            this.MobileIMEI.IsReadOnly = false;
                            return;
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 数据错误\n" + ex.Message);
                        this.MobileIMEI.IsReadOnly = false;
                    }

                }


                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: " + e.Result.Message);
                    this.MobileIMEI.IsReadOnly = false;
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "查询失败: 服务器错误\n" + e.Error.Message);
            }
        }


	}
}