using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using UserMS.Common;

namespace UserMS.Sys_tem.Pro
{
    public partial class AddPro : BasePage
    {
        const int SumbitMethod = 0;
   
     
        private List<API.PropertyModel> propertyList = new List<API.PropertyModel>();
        /// <summary>
        /// 商品属性
        /// </summary>
        public List<API.PropertyModel> PropertyList
        {
            get { return propertyList; }
            set { propertyList = value; }
        }
        private List<string> propertyName = new List<string>();
        /// <summary>
        /// 属性名列表
        /// </summary>
        public List<string> PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }


        public AddPro()
        {
            InitializeComponent();
            VIPTypeDG.ItemsSource = Store.VIPType;
    
            List<API.Pro_YanbaoPriceStepInfo> YanBaoList = new List<API.Pro_YanbaoPriceStepInfo>();
            foreach (var Item in Store.YanbaoPriceStep)
            {
                API.Pro_YanbaoPriceStepInfo YanBao = new API.Pro_YanbaoPriceStepInfo();
                YanBao.ID = Item.ID;
                YanBao.Name = Item.Name;
                YanBao.ProPrice = decimal.Parse(Item.ProPrice.ToString("#0.0000"));
                YanBao.StepPrice = decimal.Parse(Item.StepPrice.ToString("#0.0000"));
                YanBaoList.Add(YanBao);
            }
            DGYanBaoMode.ItemsSource = YanBaoList;
            IsHall.ItemsSource = Store.ProHallInfo;
            IsHall.DisplayMemberPath = "HallName";
            IsHall.SelectedValueMemberPath = "HallID";
            Initialization1();
        }


        #region 初始化商品属性
        private void InitProperty()
        {
            
            API.ProModel Selectmodel = ProNameDG.SelectedItem as API.ProModel;
            PropertyValueDG.Items.Clear();
            try
            {
                PropertyTV.Items.Clear();
                foreach (API.PropertyModel PropertyItem in PropertyList)
                {
                    RadTreeViewItem item = new RadTreeViewItem();
                    List<API.PropertyModel> propertyModel = new List<API.PropertyModel>();
                    if (Selectmodel != null && Selectmodel.PropertyModel != null)
                    {
                        propertyModel = (from b in Selectmodel.PropertyModel
                                         where b.Cate == PropertyItem.Cate
                                         select b).ToList();
                    }
                    if (propertyModel.Count() > 0)
                    {
                        if (PropertyItem.PropertyValueModel.Count() == propertyModel.First().PropertyValueModel.Count())
                            item.CheckState = System.Windows.Automation.ToggleState.On;
                        else
                            item.CheckState = System.Windows.Automation.ToggleState.Indeterminate;
                    }
                    else
                        item.CheckState = System.Windows.Automation.ToggleState.Off;

                    item.Header = PropertyItem.Cate;
                    item.Tag = PropertyItem.ID;
                    item.DataContext = PropertyItem.PropertyValueModel;
                    PropertyTV.Items.Add(item);
                }
            }
            catch
            {
                InitProperty();
            }
        }

        void Initialization1()
        {
            #region 获取商品属性
            PublicRequestHelp help = new PublicRequestHelp(this.isbusy, MethodIDStore.PerytMethodID, new object[] { }, SearchCompleted);
            #endregion
        }

        void SearchCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error == null)
            {
                Logger.Log(e.Result.Message + "");
                if (e.Result.ReturnValue == true)
                {
                    List<API.Pro_Property> AllProperty = e.Result.Obj as List<API.Pro_Property>;
                    foreach (var Item in AllProperty)
                    {
                        API.PropertyModel model = new API.PropertyModel();
                        model.ID = Item.ID;
                        model.Cate = Item.Cate;
                        model.Note = Item.Note;
                        model.PropertyValueModel = new List<API.PropertyValueModel>();
                        foreach (var ValueItem in Item.Pro_PropertyValue)
                        {
                            API.PropertyValueModel ValueModel = new API.PropertyValueModel()
                            {
                                ID = ValueItem.ID,
                                Pvalue = ValueItem.Value,
                                Note = ValueItem.Note

                            };
                            model.PropertyValueModel.Add(ValueModel);
                        }
                        PropertyList.Add(model);
                    }
                    InitProperty();
                }
                else
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message + "");
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion

        #region 添加商品
        /// <summary>
        /// 第一步添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProName_Click_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.ProModel> proModel = this.ProNameDG.ItemsSource as List<API.ProModel>;
            proModel = proModel == null ? new List<API.ProModel>() : proModel;
            Mul_ProductFileter  ProFileter = new Mul_ProductFileter(ref proModel, ref this.ProNameDG);
            ProFileter.GetMainPro();
        }
        #endregion

        private void ProNameDG_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            InitProperty();
       
            this.VIPTypeDG.SelectedItems.Clear();
            this.DGYanBaoMode.SelectedItems.Clear();
            API.ProModel proModel = this.ProNameDG.SelectedItem as API.ProModel;
            if (proModel == null) return;
            if (proModel.VIPTypeID != 0)
            {
                List<API.VIP_VIPType> TypeSource = VIPTypeDG.ItemsSource as List<API.VIP_VIPType>;
                var query = from b in TypeSource
                            where b.ID == proModel.VIPTypeID
                            select b;
                if (query.Count() > 0)
                {
                    this.VIPTypeDG.SelectedItems.Add(query.First());
                }
            }
            if (proModel.YanBaoModelID != 0)
            {
                List<API.Pro_YanbaoPriceStepInfo> YanBaoSource = this.DGYanBaoMode.ItemsSource as List<API.Pro_YanbaoPriceStepInfo>;
                var query = from b in YanBaoSource
                            where b.ID == proModel.YanBaoModelID
                            select b;
                if (query.Count() > 0)
                {
                    this.DGYanBaoMode.SelectedItems.Add(query.First());
                }
            }
       

        }
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.ProModel> ProNameSource = ProNameDG.ItemsSource as List<API.ProModel>;
            if (ProNameSource == null || this.ProNameDG.SelectedItem==null) return;
            try
            {
                ProNameSource.Remove(this.ProNameDG.SelectedItem as API.ProModel);
                this.ProNameDG.Rebind();
            }
            catch
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"删除错误！");
            }
        }
        #region 提交新增
        private void Sumbit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            List<API.ProModel> Source = ProNameDG.ItemsSource as List<API.ProModel>;
            if (Source == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"未添加新商品！");
                return;
            }
            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定新增？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return;
            PublicRequestHelp helper = new PublicRequestHelp(isbusy, MethodIDStore.AddProMethodID, new object[] { Source }, Sumbit_Completed);
        }
        protected void Sumbit_Completed(object sender, API.MainReportCompletedEventArgs e)
        {
            this.isbusy.IsBusy = false;
            if (e.Error == null)
            { 
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
                if (e.Result.ReturnValue == true)
                {
                    List<API.Pro_ProInfo> proinfos=(List<API.Pro_ProInfo>)e.Result.Obj;
                    Store.ProInfo.AddRange(proinfos);
                    this.ProNameDG.ItemsSource=null;
                    this.ProNameDG.Rebind();
                    this.PropertyValueDG.Items.Clear();
                    this.VIPTypeDG.SelectedItems.Clear();
                    this.DGYanBaoMode.SelectedItems.Clear();
                    InitProperty();       
                }
              
            }
            else
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"服务端出错！");
        }
        #endregion
        #region 属性操作
        //属性选择时发生
        private void PropertyTV_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            RadTreeViewItem item = (RadTreeViewItem)e.AddedItems[0];
            List<API.PropertyValueModel> tem = (List<API.PropertyValueModel>)item.DataContext;
            API.ProModel ProItem = ProNameDG.SelectedItem as API.ProModel;
            if (ProNameDG.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择添加属性的商品！");
                return;
            }
            List<API.PropertyModel> query_Property = new List<API.PropertyModel>();
            if (ProItem.PropertyModel != null)
            {
                query_Property = (from b in ProItem.PropertyModel
                                  where b.Cate == item.Header.ToString()
                                  select b).ToList();
            }
            this.PropertyValueDG.Items.Clear();
            foreach (var Value in tem)
            {
                RadTreeViewItem Item = new RadTreeViewItem();
                if (query_Property.Count() > 0)
                {
                    int query_PropertyValue = (from b in query_Property.First().PropertyValueModel
                                               where b.ID == Value.ID
                                               select b).Count();
                    if (query_PropertyValue > 0)
                        Item.CheckState = System.Windows.Automation.ToggleState.On;
                    else
                        Item.CheckState = System.Windows.Automation.ToggleState.Off;
                }
                Item.Header = Value.Pvalue;
                Item.Tag = Value.ID;
                this.PropertyValueDG.Items.Add(Item);
            }
        }


        
        private void PropertyTV_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (ProNameDG.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择添加属性的商品！");
                InitProperty();
                return;
            }
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            List<API.PropertyValueModel> menu = (List<API.PropertyValueModel>)item.DataContext;

            API.ProModel ProItem = ProNameDG.SelectedItem as API.ProModel;
            if (ProItem.PropertyModel == null)
                ProItem.PropertyModel = new List<API.PropertyModel>();

            var query = (from b in ProItem.PropertyModel
                         where b.Cate == item.Header.ToString()
                         select b).ToList();
            if (query.Count() > 0)
                return;
            API.PropertyModel PropertyItem = new API.PropertyModel() { ID = (int)item.Tag, Cate = item.Header.ToString(), PropertyValueModel = menu };
            ProItem.PropertyModel.Add(PropertyItem);


        }

        private void PropertyTV_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            API.ProModel ProItem = ProNameDG.SelectedItem as API.ProModel;
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            if (ProItem==null||ProItem.PropertyModel == null) return;
            try
            {
                var query = (from b in ProItem.PropertyModel
                             where b.ID == (int)item.Tag
                             select b).ToList().First();
                ProItem.PropertyModel.Remove(query);
            }
            catch { }
        }
        #endregion 

        #region 属性值操作
        private void PropertyValueDG_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            API.ProModel ProItem = ProNameDG.SelectedItem as API.ProModel;
            if (ProNameDG.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择添加属性的商品！");
                Initialization1();
                return;
            }

            if (ProItem.PropertyModel == null)
            {
                ProItem.PropertyModel = new List<API.PropertyModel>();
            }
            List<API.PropertyModel> query;
            query = (from b in ProItem.PropertyModel
                     where b.Cate == PropertyTV.SelectedItem.ToString()
                     select b).ToList();
            API.PropertyModel query_Item;
            if (query.Count() > 0)
            {
                query_Item = query.First();
                if (query_Item.PropertyValueModel != null)
                {
                    API.PropertyValueModel value = new API.PropertyValueModel() { ID = (int)item.Tag, Pvalue = item.Header.ToString() };
                    query_Item.PropertyValueModel.Add(value);
                }
                else
                {
                    query_Item.PropertyValueModel = new List<API.PropertyValueModel>()
                    {
                        new API.PropertyValueModel(){ ID=(int)item.Tag, Pvalue=item.Header.ToString()}
                    };
                }
            }
            else
            {
                query_Item = new API.PropertyModel() { Cate = PropertyTV.SelectedItem.ToString() };
                query_Item.PropertyValueModel = new List<API.PropertyValueModel>()
                {
                     new API.PropertyValueModel(){ ID=(int)item.Tag, Pvalue=item.Header.ToString()}
                };
                ProItem.PropertyModel.Add(query_Item);
            }
            var ValueModel = (from b in query_Item.PropertyValueModel
                              select b).ToList();
            foreach (var perty in PropertyTV.Items)
            {
                RadTreeViewItem pertyItem = perty as RadTreeViewItem;
                if (pertyItem.Header.ToString() == PropertyTV.SelectedItem.ToString())
                {
                    List<API.PropertyValueModel> tem = (List<API.PropertyValueModel>)pertyItem.DataContext;
                    if (tem.Count() == ValueModel.Count())
                        pertyItem.CheckState = System.Windows.Automation.ToggleState.On;
                    else
                        pertyItem.CheckState = System.Windows.Automation.ToggleState.Indeterminate;
                }
            }

        }

        private void PropertyValueDG_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = e.Source as RadTreeViewItem;
            API.ProModel ProItem = ProNameDG.SelectedItem as API.ProModel;
            if (ProNameDG.SelectedItem == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"操作有误！");
                Initialization1();
                return;
            }

            var query = (from b in ProItem.PropertyModel
                         where b.Cate == PropertyTV.SelectedItem.ToString()
                         select b).ToList();


            if (query.Count() > 0)
            {
                API.PropertyModel query_Item = query.First();
                if(query_Item.PropertyValueModel!=null)
                {
                var Value = (from b in query_Item.PropertyValueModel
                             where b.Pvalue == item.Header.ToString()
                             select b).First();
                query_Item.PropertyValueModel.Remove(Value);
                }

                var ValueModel = (from b in query_Item.PropertyValueModel
                                  select b).ToList();
                foreach (var perty in PropertyTV.Items)
                {
                    RadTreeViewItem pertyItem = perty as RadTreeViewItem;
                    if (pertyItem.Header.ToString() == PropertyTV.SelectedItem.ToString())
                    {

                        if (query_Item.PropertyValueModel.Count() == 0)
                            pertyItem.CheckState = System.Windows.Automation.ToggleState.Off;
                        else
                            pertyItem.CheckState = System.Windows.Automation.ToggleState.Indeterminate;
                    }
                }
            }

        }
        #endregion

        #region 选择会员卡类型
        private void VIPTypeDG_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            API.ProModel ProItem = ProNameDG.SelectedItem as API.ProModel;
            if (ProNameDG.SelectedItem == null)
            {
                //MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择添加会员卡类别的商品！");
                this.VIPTypeDG.SelectedItems.Clear();
                return;
            }
            API.VIP_VIPType TypeItem = VIPTypeDG.SelectedItem as API.VIP_VIPType;
            if (TypeItem == null)
                ProItem.VIPTypeID = 0;
            else
                ProItem.VIPTypeID = TypeItem.ID;
        }
        #endregion 
        #region 延保操作
        private void DGYanBaoMode_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            API.ProModel ProItem = ProNameDG.SelectedItem as API.ProModel;
            if (ProNameDG.SelectedItem == null)
            {
                this.DGYanBaoMode.SelectedItems.Clear();
                return;
            }
            API.Pro_YanbaoPriceStepInfo YanBaoItem = this.DGYanBaoMode.SelectedItem as API.Pro_YanbaoPriceStepInfo;
            if (YanBaoItem == null)
                ProItem.YanBaoModelID = 0;
            else
                ProItem.YanBaoModelID = YanBaoItem.ID;
        }
        #endregion


        #region DV编辑结束后
        private void ProGrid_CellValidated_1(object sender, GridViewCellValidatedEventArgs e)
        {
            API.ProModel ProItem = this.ProNameDG.SelectedItem as API.ProModel;

            if (e.Cell.Column.Header.ToString() == "加金额")
            {
                //if (ProItem.BeforeRate < 0)
                //{
                //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写正数！");
                //    ProItem.BeforeRate = 0;
                //    return;
                //}
                int value = (int)(ProItem.BeforeRate * 100);
                ProItem.BeforeRate = (decimal)(value * 0.01);
                int value1 = (int)(ProItem.AfterRate * 100);
                ProItem.AfterRate = (decimal)(value1 * 0.01);
            }

            
            else if (e.Cell.Column.Header.ToString() == "券临界值")
            {
                //if (ProItem.TicketLevel < 0)
                //{
                //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写正数！");
                //    ProItem.TicketLevel = 0;
                //    return;
                //}
                int value = (int)(ProItem.TicketLevel * 100);
                ProItem.TicketLevel = (decimal)(value * 0.01);
            }
            else if (e.Cell.Column.Header.ToString() == "小于券临界值")
            {
                //if (ProItem.BeforeTicket < 0)
                //{
                //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写正数！");
                //    ProItem.BeforeTicket = 0;
                //    return;
                //}
                int value = (int)(ProItem.BeforeTicket * 100);
                ProItem.BeforeTicket = (decimal)(value * 0.01);
            }
            else if (e.Cell.Column.Header.ToString() == "大于券临界值")
            {
                //if (ProItem.AfterTicket < 0)
                //{
                //    MessageBox.Show(System.Windows.Application.Current.MainWindow,"请填写正数！");
                //    ProItem.AfterTicket = 0;
                //    return;
                //}
                int value = (int)(ProItem.AfterTicket * 100);
                ProItem.AfterTicket = (decimal)(value * 0.01);
            }
        }
        #endregion 
    }
}
