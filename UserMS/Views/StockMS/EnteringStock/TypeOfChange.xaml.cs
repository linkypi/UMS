using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Views.StockMS.EnteringStock
{
    /// <summary>
    /// TypeOfChange.xaml 的交互逻辑
    /// </summary>
    public partial class TypeOfChange : BasePage
    {
     
        /// <summary>
        /// 已拣货列表
        /// </summary>
        private List<API.SeleterModel> checkedModels;
        /// <summary>
        /// 无串码商品
        /// </summary>
        private List<API.SeleterModel> unIMEIModels;
        /// <summary>
        /// 未拣货的串码
        /// </summary>
        private List<API.SelecterIMEI> uncheckIMEI;

        /// <summary>
        /// 拣货器
        /// </summary>
        private Pro_GetPro pickder;



        /// <summary>
        /// 仓库添加器
        /// </summary>
        private HallFilter hAdder;

        /// <summary>
        /// 仓库列表
        /// </summary>
        List<API.Pro_HallInfo> hall;


        string r = "";
        public TypeOfChange()
        {
            InitializeComponent();

            uncheckIMEI = new List<API.SelecterIMEI>();
            unIMEIModels = new List<API.SeleterModel>();
            checkedModels = new List<API.SeleterModel>();
            //绑定数据
            GridUnCheckPro.ItemsSource = unIMEIModels;
            GridCheckedPro.ItemsSource = checkedModels;
            GridUnCheckIMEI.ItemsSource = uncheckIMEI;
            //this.GHHall.Tag = "GH";
            //this.SHHall.Tag = "SH";
            //添加搜索资源
            //GHHall.ItemsSource = Store.ProHallInfo;
            //GHHall.DisplayMemberPath = "HallName";


            pickder = new Pro_GetPro(ref this.GHHall, ref this.IsBusy, ref checkedModels, ref unIMEIModels,
                            ref uncheckIMEI, ref GridCheckedPro, ref GridUnCheckPro, ref GridUnCheckIMEI);
    
 
            this.toDate.SelectedValue = DateTime.Now;
            this.userID.Text = Store.LoginUserInfo.UserName;
            this.cancel.Click += cancel_Click;
            this.Sumbit.Click += new Telerik.Windows.RadRoutedEventHandler(Sumbit_Click);
             this.GHHall.SearchButton.Click += new RoutedEventHandler(GHHall_Click);

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {this.Loaded -= Page_Loaded;
            try
            {    
                r = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
               
            }
            catch
            {
                r = "167";
            }
            finally
            {
                GetFirstHall();
            }
        }
        private void GetFirstHall()
        {
            hAdder = new HallFilter(false,ref this.GHHall);
            List<API.Pro_HallInfo> HallInfo = Store.ProHallInfo;
            hall = hAdder.FilterHall(int.Parse(r.Trim()), HallInfo);
            if (hall.Count == 0)
            {
                return;
            }
 
            this.GHHall.TextBox.SearchText = hall.First().HallName;
            this.GHHall.Tag = hall.First().HallID;
        }

        /// <summary>
        /// 添加无串码商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ProductionFilter adder = new ProductionFilter(ref unIMEIModels, ref GridUnCheckPro); 
            if (this.GHHall.TextBox.SearchText.Trim() == "")
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择供货仓库");
                return;
            }
            List<API.Pro_ProInfo> pro = adder.GetPro(int.Parse(r));
            if(pro==null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"该角色无此权限，请联系管理员 ");
                return;
            }
            adder.ProFilter(pro, true);
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (PormptPage.PormptMessage("是否清空数据？", "提示"))
            Clear();
        }



        /// <summary>
        /// 拣货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckProduct_Click(object sender, RoutedEventArgs e)
        {
            if (uncheckIMEI.Count() == 0 && unIMEIModels.Count() == 0) { return; }
            pickder.Packing();
            
        }

        /// <summary>
        /// 选中已拣货码商品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridCheckedPro_SelectionChanged_1(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            ViewOperate.GridSelectChanged(ref GridCheckedPro, ref GridCheckedIMEI);
        }

        #region  "删除操作"



        /// <summary>
        /// 右键删除选中的串码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delIMEI_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //ViewCommon.DeleteSingleIMEI(ref uncheckIMEI, ref GridUnCheckIMEI);
        }
        /// <summary>
        /// 删除Checked选中的商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delCheckedPro_Click(object sender, RoutedEventArgs e)
        {
            ViewOperate.DelCheckedPro(ref unIMEIModels, ref GridUnCheckPro);
        }


        #endregion

        #region 修改方法接口
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region 提交到后台
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sumbit_Click(object sender, RoutedEventArgs e)
        {
            //添加表头
            List<API.SeleterModel> ProSource = GridCheckedPro.ItemsSource as List<API.SeleterModel>;
            if (ProSource == null || ProSource.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加商品！");
                return;
            }
            List<API.SeleterModel> query = (from b in ProSource
                        where !string.IsNullOrEmpty(b.NewProID)
                        select b).ToList();
            if (query.Count() == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加新商品！");
                return;
            }
            if (this.GHHall.Tag == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请添加仓库！");
                return;
            }
            API.Pro_ChangeProInfo ListHear = new API.Pro_ChangeProInfo() { HallID = this.GHHall.Tag.ToString(), Note = this.tbNote.Text.Trim(), OldID = this.oldID.Text.Trim() };
            ListHear.Pro_ChangeProListInfo = new List<API.Pro_ChangeProListInfo>();
            foreach (var Proitem in query)
            {
                API.Pro_ChangeProListInfo ListInfo = new API.Pro_ChangeProListInfo() { InListID = Proitem.ProInListID, NewProID = Proitem.NewProID, OldProID = Proitem.ProID, Note = Proitem.NewNote, ProCount = Proitem.Count};
                ListInfo.Pro_ChangeIMEIInfo = new List<API.Pro_ChangeIMEIInfo>();

                if (Proitem.NewIsNeedIMEI == true)
                    ListInfo.Flag = true;
                else
                    ListInfo.Flag = false;
                if (Proitem.NewIsNeedIMEI == true||Proitem.IsNeedIMEI==true)
                {                  

                    if (Proitem.IsIMEI == null || Proitem.IsIMEI.Count() == 0)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"转成有串码商品必须要添加串码！");
                        return;
                    }
                    if (Proitem.IsNeedIMEI == false && Proitem.NewIsNeedIMEI == true)
                    {
                        if (Proitem.Count != Proitem.NewCount)
                        {
                            MessageBox.Show(System.Windows.Application.Current.MainWindow,"无串码商品转成有串码商品必须要添加同等数量的串码！");
                            return;
                        }
                    }
                    foreach (var IMEIItem in Proitem.IsIMEI)
                    {
                        if (!string.IsNullOrEmpty(IMEIItem.IMEI))
                        {
                            API.Pro_ChangeIMEIInfo IMEI = new API.Pro_ChangeIMEIInfo() { IMEI = IMEIItem.IMEI };
                            ListInfo.Pro_ChangeIMEIInfo.Add(IMEI);
                        }
                        if (!string.IsNullOrEmpty(IMEIItem.OldIMEI))
                        {
                            API.Pro_ChangeIMEIInfo IMEI = new API.Pro_ChangeIMEIInfo() { IMEI = IMEIItem.OldIMEI };
                            ListInfo.Pro_ChangeIMEIInfo.Add(IMEI);
                        }
                        
                    }
                }
                ListHear.Pro_ChangeProListInfo.Add(ListInfo);
            }
            if (PormptPage.PormptMessage("确定转换类型？", "提示"))
            {
                PublicRequestHelp help = new PublicRequestHelp(this.IsBusy, MethodIDStore.AddOfChangedID, new object[] { ListHear }, new EventHandler<API.MainCompletedEventArgs>(SubmitCompleted));
            }
        }

        private void SubmitCompleted(object sender, API.MainCompletedEventArgs mcea)
        {
            this.IsBusy.IsBusy = false;
            if(mcea.Error==null)
            {
                API.WebReturn eb = mcea.Result;
                MessageBox.Show(System.Windows.Application.Current.MainWindow,eb.Message);
                if (!eb.ReturnValue)
                {
                    Logger.Log("转换失败");
                    return;
                }
                Clear();
                Logger.Log("转换成功！");  
            }
            else 
            {
                //MessageBox.Show(System.Windows.Application.Current.MainWindow,mcea.Result.Message);
            }
        }

        private void Clear()
        {
            this.tbNote.Text = string.Empty;
            GetFirstHall();

          //  this.GHHall.TextBox.SearchText = string.Empty;
           // this.GHHall.Tag = null;
            this.toDate.SelectedValue = DateTime.Now;
            this.oldID.Text = string.Empty;
            this.txtIMEI.Text = string.Empty;
            //拣货成功 清空数据
            checkedModels.Clear();
            uncheckIMEI.Clear();
            unIMEIModels.Clear();
            GridCheckedIMEI.ItemsSource = null;      
            GridUnCheckPro.Rebind();
            GridCheckedPro.Rebind();
            GridCheckedIMEI.Rebind();
            GridUnCheckIMEI.Rebind();
        }
        #endregion
        #region 添加仓库
        /// <summary>
        /// 选择供货仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GHHall_Click(object sender, RoutedEventArgs e)
        {
            hAdder = new HallFilter(false, ref this.GHHall);
            if (hall == null)
                hall = new List<API.Pro_HallInfo>();
            hAdder.GetHall(hall);
        }
        #endregion
        #region 新商品操作
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                List<API.SeleterModel> ProSource = GridUnCheckPro.ItemsSource as List<API.SeleterModel>;
                  List<API.SeleterModel> SelectSource=new List<API.SeleterModel>();
                  foreach (var Item in GridUnCheckPro.SelectedItems)
                {
                    SelectSource.Add(Item as API.SeleterModel);
                }
                var query = (from b in SelectSource
                             join c in ProSource on
                             new {
                              b.ProInListID,
                              b.ProID 
                             }
                             equals 
                            new  {
                             c.ProInListID,
                                 c.ProID
                                  } 
                             select c).ToList();
              
                if (ProSource == null||ProSource.Count()==0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择商品！");
                    return;
                }
                BaseProFilter adder = new BaseProFilter(ref this.GridUnCheckPro, ref query); 
                adder.ProFilter(adder.GetPro(int.Parse(r.Trim())), true);
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获取菜单ID失败！");
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.SeleterModel> Source = GridCheckedPro.ItemsSource as List<API.SeleterModel>;
            Source.Remove(GridCheckedPro.SelectedItem as API.SeleterModel);
        }
        #endregion 

        #region 添加新商品串码
        private void AddNewIMEI_Click(object sender, RoutedEventArgs e)
        {
            AddIMEI();
        }
        private void AddIMEI()
        {

            API.SeleterModel model = this.GridUnCheckPro.SelectedItem as API.SeleterModel;
            if (model == null||string.IsNullOrEmpty(model.NewProID))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未选择商品或未添加新商品！");
                return;
            }
            if (string.IsNullOrEmpty(model.NewProID))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未添加要转换的商品！");
                return;
            }
            if (model.IsNeedIMEI == false && model.NewIsNeedIMEI == true)
            {
                if (String.IsNullOrEmpty(this.TextIMEI.Text))
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "商品串码不能为空");
                    return;
                }

                if (model.IsIMEI == null)
                {
                    model.IsIMEI = new List<API.SelecterIMEI>();
                }
                List<string> list = new List<string>(TextIMEI.Text.Split("\r\n".ToCharArray()));
                // model.NewCount = model.NewCount == null ? 0 : model.NewCount;
                foreach (string s in list)
                {
                    if (!string.IsNullOrEmpty(s) && model.NewCount < model.Count)
                    {
                        if (!ValidateIMEI(s))  //去除重复项
                        {
                            API.SelecterIMEI IMEI = new API.SelecterIMEI() { IMEI = s.ToUpper()};
                            model.IsIMEI.Add(IMEI);
                            model.NewCount += 1;
                        }
                    }
                }
                TextIMEI.Text = string.Empty;
                this.GridCheckedIMEI.ItemsSource = model.IsIMEI;
                this.GridCheckedIMEI.Rebind();
                this.GridCheckedPro.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "无串码商品转有串码商品才需要输入串码！");
                return;
            }
        }
        private void AddNewIMEI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddOldIMEI();
            }
        }

        private bool ValidateIMEI(string imei)
        {
           
            API.SeleterModel pinfo = this.GridUnCheckPro.SelectedItem as API.SeleterModel;
            if (PormptPage.IsMax(imei, 12) == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "字符串不能超过24位！");
                return true;
            }
            var query = (from b in pinfo.IsIMEI
                         where b.IMEI == imei
                         select b).ToList();
            if (query.Count() > 0)
            {
                return true;
            }
            return false;
        }
        #region 删除新商品串码
        private void DelIMEI_Click_1(object sender, RoutedEventArgs e)
        {
            API.SeleterModel ProItem = this.GridCheckedPro.SelectedItem as API.SeleterModel;
            if (ProItem == null) return;
            if (ProItem.IsNeedIMEI == false && ProItem.NewIsNeedIMEI == true)
            {
                List<API.SelecterIMEI> IMEIList = this.GridCheckedIMEI.ItemsSource as List<API.SelecterIMEI>;
                if (IMEIList == null) return;
                foreach (var Item in this.GridCheckedIMEI.SelectedItems)
                {
                    IMEIList.Remove(Item as API.SelecterIMEI);
                    ProItem.NewCount--;
                }
                this.GridCheckedIMEI.Rebind();
            }
        }
        #endregion
        #endregion


        #region 添加旧商品串码

        private void BatchAddIMEI_Click(object sender, RoutedEventArgs e)
        {
            AddOldIMEI();
        }
        private void AddOldIMEI()
        {
            if (this.GridUnCheckPro.SelectedItems.Count() > 1)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "只能选择个商品！");
                return;
            }
            API.SeleterModel model = this.GridUnCheckPro.SelectedItem as API.SeleterModel;
            if (model == null||string.IsNullOrEmpty(model.NewProID))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "未选择商品或未添加新商品！");
                return;
            }
 
            
            if (model.IsNeedIMEI == true&&!String.IsNullOrEmpty(this.txtIMEI.Text))
            {
                if (model.IsIMEI == null)
                {
                    model.IsIMEI = new List<API.SelecterIMEI>();
                }
                List<string> list = new List<string>(txtIMEI.Text.Split("\r\n".ToCharArray()));
                foreach (string s in list)
                {
                    if (!string.IsNullOrEmpty(s) )
                    {
                        if (!ValidateIMEI1(s))  //去除重复项
                        {
                            API.SelecterIMEI IMEI = new API.SelecterIMEI() { OldIMEI = s.ToUpper() };
                            model.IsIMEI.Add(IMEI);
                            model.Count += 1;
                        }
                    }
                }                  
            }
            if (model.IsNeedIMEI == false && model.NewIsNeedIMEI == true && !string.IsNullOrEmpty(TextIMEI.Text))
            {
                if (model.Count == 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow, "请输入商品数量！");
                    return;
                }
                if (model.IsIMEI == null)
                {
                    model.IsIMEI = new List<API.SelecterIMEI>();
                }
                List<string> Newlist = new List<string>(TextIMEI.Text.Split("\r\n".ToCharArray()));
                foreach (string s in Newlist)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        if (!string.IsNullOrEmpty(s) && model.NewCount < model.Count)
                        {
                            if (!ValidateIMEI2(s))  //去除重复项
                            {
                                API.SelecterIMEI IMEI = new API.SelecterIMEI() { IMEI = s.ToUpper() };
                                model.IsIMEI.Add(IMEI);
                                model.NewCount += 1;
                            }
                        }
                    }
                }
            }
            txtIMEI.Text = string.Empty;
            TextIMEI.Text = string.Empty;
            this.GridUnCheckIMEI.ItemsSource = model.IsIMEI;
            GridUnCheckIMEI.Rebind();
        
        }
        private void txtIMEI_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddOldIMEI();
            }
        }
        private bool ValidateIMEI2(string imei)
        {
            API.SeleterModel pinfo = this.GridUnCheckPro.SelectedItem as API.SeleterModel;
            if (PormptPage.IsMax(imei, 12) == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "字符串不能超过24位！");
                return true;
            }
            var query = (from b in pinfo.IsIMEI
                         where b.IMEI == imei
                         select b).ToList();
            if (query.Count() > 0)
            {
                return true;
            }
            return false;
        }

        private bool ValidateIMEI1(string imei)
        {
            API.SeleterModel pinfo = this.GridUnCheckPro.SelectedItem as API.SeleterModel;
            if (PormptPage.IsMax(imei, 12) == false)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "字符串不能超过24位！");
                return true;
            }
            var query = (from b in pinfo.IsIMEI
                         where b.OldIMEI == imei
                         select b).ToList();
            if (query.Count() > 0)
            {
                return true;
            }
            return false;
        }

        #region 删除旧商品串码
        private void RadMenuItem_Click_2(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.SeleterModel ProItem = this.GridUnCheckPro.SelectedItem as API.SeleterModel;
            if (ProItem == null) return;

                List<API.SelecterIMEI> IMEIList = this.GridUnCheckIMEI.ItemsSource as List<API.SelecterIMEI>;
                if (IMEIList == null) return;
                foreach (var Item in this.GridUnCheckIMEI.SelectedItems)
                {
                    API.SelecterIMEI SelItem = Item as API.SelecterIMEI;
                    if(!string.IsNullOrEmpty(SelItem.OldIMEI))
                       ProItem.Count--;
                    if (!string.IsNullOrEmpty(SelItem.IMEI))
                       ProItem.NewCount--;
                    IMEIList.Remove(SelItem);
                    
                }
                this.GridUnCheckIMEI.Rebind();
                this.GridUnCheckPro.Rebind();
            
        }
        #endregion
        #endregion

        private void DGCardType_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            API.SeleterModel Item = this.GridUnCheckPro.SelectedItem as API.SeleterModel;

            if (Item.Count < 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品数量不能为负数");
                Item.Count = 0;
                return;
            }
            if (Item.ISdecimals == false || Item.ISdecimals == null)
            {
                try
                {
                    Item.Count = (int)Item.Count;
                }
                catch
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入正确数值！");

                }
            }
            try
            {
                int value = (int)(Item.Count * 100);
                Item.Count = (decimal)(value * 0.01);

            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请输入正确的数值！");
                Item.Count = 0;
            }
        }

        private void GridUnCheckPro_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (this.GridUnCheckPro.SelectedItems.Count() != 1)
            {
                this.GridUnCheckIMEI.ItemsSource = null;
                this.GridUnCheckIMEI.Rebind();
                return;
            }
            API.SeleterModel pinfo = GridUnCheckPro.SelectedItem as API.SeleterModel; // 是否需要串码
            if (pinfo.IsNeedIMEI == false)//不需要串码
                this.GridUnCheckPro.Columns[4].IsReadOnly = false;
            else//需要串码
                this.GridUnCheckPro.Columns[4].IsReadOnly = true;
            GridUnCheckIMEI.ItemsSource = pinfo.IsIMEI;
            GridUnCheckIMEI.Rebind();
        }  
    }
}
