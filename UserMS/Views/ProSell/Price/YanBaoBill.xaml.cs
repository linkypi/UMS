using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace UserMS.Views.ProSell.Price
{
    public partial class YanBaoBill : BasePage
    {
        private List<API.View_YanBoPriceStepInfo> models = null;
     
        private List<API.View_YanBoPriceStepInfo> oldYanboBill = null;
        private string menuid = "";
        private string Name;
          
         API.View_YanBoPriceStepInfo Proyb;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Page_Loaded;
            try
            {
                menuid = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["MenuID"];
                Name = System.Web.HttpUtility.ParseQueryString(NavigationService.Source.OriginalString.Split('?').Reverse().First())["Name"];
            }
            catch
            {
                //menuid = "111";
                //ProName = "1017";
            }

            oldYanboBill = new List<API.View_YanBoPriceStepInfo>();
            models = new List<API.View_YanBoPriceStepInfo>();
            GridProList.ItemsSource = models;

            #region 初始化延保商品

            var query = (from b in Store.ProInfo
                          join o in Store.Options 
                          on b.ProID equals o.Value
                         where  o.ClassName == Name
                         select b).ToList();
       
            if (query.Count()>0)
            {
                var query2 = (from b in query
                             join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                             join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                             select new
                             {
                                 ProID = b.ProID,
                                 ProName = b.ProName,
                                 ProFormat = b.ProFormat,
                                 IsNeedIMEI = b.NeedIMEI,
                                 ClassName = c.ClassName,
                                 ClassID = c.ClassID,
                                 TypeName = d.TypeName
                             }).ToList();
                   if(query2.Count()>0)
                {
                    Proyb = new API.View_YanBoPriceStepInfo();
                    Proyb.ProFormat = query2.First().ProFormat;
                    Proyb.ProID = query2.First().ProID;
                    Proyb.ProName = query2.First().ProName;
                    Proyb.UpdateFlag= false;
                    Proyb.ProClassName = query2.First().ClassName;
                    Proyb.ProTypeName = query2.First().TypeName;
                    Proyb.OldLowPrice = 0;
                    Proyb.LowPrice = 0;
                    Proyb.OldProCost = 0;
                    Proyb.OldProPrice = 0;
                    Proyb.OldStepPrice = 0;
                    Proyb.ProCost = 0;
                    Proyb.ProPrice = 0;
                    Proyb.StepPrice = 0;
                }
            }
            #endregion 

            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 146, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
   
        }

        public YanBaoBill()
        {
            InitializeComponent();
        }

        #region 添加商品

        /// <summary>
        /// 新增行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPro_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Proyb == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"无延保商品可添加");
                return;
            }
            API.View_YanBoPriceStepInfo yb = new API.View_YanBoPriceStepInfo();
            yb.Name = string.Empty;
            yb.ProID = Proyb.ProID;
            yb.ProName = Proyb.ProName;
            yb.ProClassName = Proyb.ProClassName;
            yb.ProTypeName = Proyb.ProTypeName;
            yb.ProFormat = Proyb.ProFormat;

            models.Add(yb);
            GridProList.Rebind();
        }

        #endregion 

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, " 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            { 
                List<API.View_YanBoPriceStepInfo> list = e.Result.Obj as  List<API.View_YanBoPriceStepInfo>;
                oldYanboBill.Clear();
                oldYanboBill.AddRange(list);
                models.Clear();
                foreach (var item in list)
                {
                    models.Add(item); 
                }
                GridProList.Rebind();
            }
            //else
            //{
            //     MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            //}
        }

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (models.Count == 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"延保单无数据");
                return;
            }
            var query = from m in models
                        where m.LowPrice < 0 || m.ProCost < 0
                        || m.ProPrice < 0 
                        select m;
            if (query.Count() > 0)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请确保输入的数字有效");
                return;
            }

            foreach (var item in models)
            {
                if (!Convert.ToBoolean(item.UpdateFlag))
                {
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"请为商品 "+item.ProName+" 输入延保名称 ");
                        return;
                    }
                }
                foreach (var child in models)
                {
                    if (child == item)
                    {
                        continue;
                    }
                    if ( child.StepPrice == item.StepPrice)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"商品"+child.ProName+"的价格区间重复");
                        return;
                    }
                    if (child != item && child.Name == item.Name)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"延保名称"+child.Name+"重复");
                        return;
                    }
                }
            }

            List<API.Pro_YanbaoPriceStepInfo> models_update = new List<API.Pro_YanbaoPriceStepInfo>();

           #region  添加表头

            API.Pro_PriceChange header = new API.Pro_PriceChange();
            header.Pro_YanbaoPriceStepInfo = new List<API.Pro_YanbaoPriceStepInfo>();
            header.Pro_YanbaoPriceStepInfo_bak = new List<API.Pro_YanbaoPriceStepInfo_bak>();
            header.SysDate = DateTime.Now;
            header.UserID = Store.LoginUserInfo.UserID;

           #endregion

            #region  获取新增数据

            var querym  =from m in models
                         where Convert.ToBoolean(m.UpdateFlag)==false
                         select m;
            foreach (var item in querym)
            {
                API.Pro_YanbaoPriceStepInfo yb = new API.Pro_YanbaoPriceStepInfo();
                yb.LowPrice = (decimal)item.LowPrice;
                yb.Name = item.Name;
                yb.Note = item.Note;
                yb.ProCost = (decimal)item.ProCost;
                yb.ProID = item.ProID;
                yb.ProPrice = (decimal)item.ProPrice;
                yb.StepPrice = (decimal)item.StepPrice;
                header.Pro_YanbaoPriceStepInfo.Add(yb);
            }

            #endregion 

            #region 获取更新的数据

            var query2 = from m in models
                        where Convert.ToBoolean(m.UpdateFlag) && (m.LowPrice != m.OldLowPrice || m.Name!=m.OldName
                        || m.ProPrice != m.OldProPrice || m.OldStepPrice != m.StepPrice|| m.OldProCost != m.ProCost)
                        select m;
            if (query2.Count() > 0)
            {
                foreach (var item in query2)
                {
                    //备份
                    API.Pro_YanbaoPriceStepInfo_bak bak = new API.Pro_YanbaoPriceStepInfo_bak();
                    bak.LowPrice = (decimal)item.OldLowPrice;
                    bak.Name = item.Name;
                    bak.ProID = item.ProID;
                    bak.Note = item.Note;
                    bak.ProCost = (decimal)item.OldProCost;
                    bak.ProPrice = (decimal)item.OldProPrice;
                    bak.StepPrice = (decimal)item.OldStepPrice;
                    header.Pro_YanbaoPriceStepInfo_bak.Add(bak);

                    //更新
                    API.Pro_YanbaoPriceStepInfo yb = new API.Pro_YanbaoPriceStepInfo();
                    yb.ID = item.ID;
                    yb.LowPrice = (decimal)item.LowPrice;
                    yb.Name = item.Name;
                    yb.Note = item.Note;
                    yb.ProCost = (decimal)item.ProCost;
                    yb.ProID = item.ProID;
                    yb.ProPrice = (decimal)item.ProPrice;
                    yb.StepPrice =(decimal) item.StepPrice;

                    models_update.Add(yb);
                }
            }
            #endregion
            PublicRequestHelp prh = new PublicRequestHelp(this.busy, 147, new object[] { header,models_update }, new EventHandler<API.MainCompletedEventArgs>(SaveCompleted));
        }

        /// <summary>
        /// 保存完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 146, new object[] { }, new EventHandler<API.MainCompletedEventArgs>(GetCompleted));
   
                //models.Clear();
                //GridProList.Rebind();
                //oldYanboBill.Clear();
                //if (addYanBaoFlag)
                //{
                //    //若此次为添加操作  则添加完成后只能是对该商品执行修改操作
                //    addYanBaoFlag = false;
                //}
            }
           MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
        }

        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (GridProList.SelectedItems == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"请选择要删除的商品");
                return;
            }

            if (MessageBox.Show(System.Windows.Application.Current.MainWindow,"确定删除吗？")==MessageBoxResult.No)
            {
                return;
            }
            List<int> oldModels = new List<int>();
            List<int> newModels = new List<int>();

            foreach (var child in GridProList.SelectedItems)
            {
                API.View_YanBoPriceStepInfo yb = child as API.View_YanBoPriceStepInfo;

                if (Convert.ToBoolean(yb.UpdateFlag))
                {
                    oldModels.Add(yb.ID);  //删除已存在的数据
                }
                else
                {
                    newModels.Add(yb.ID); //删除新添加的未保存到数据库中的数据
                }
            }
            if (oldModels.Count() != 0)
            {
                PublicRequestHelp prh = new PublicRequestHelp(this.busy, 197, new object[] { oldModels }, new EventHandler<API.MainCompletedEventArgs>(DelCompleted));
            }
            else
            {
                foreach (var child in newModels)
                {
                    foreach (var item in models)
                    {
                        if (child == item.ID)
                        {
                            models.Remove(item);
                            break;
                        }
                    }
                }
                GridProList.Rebind();
            }
           
        }

        /// <summary>
        /// 删除完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelCompleted(object sender, API.MainCompletedEventArgs e)
        {
            this.busy.IsBusy = false;
            if (e.Error != null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "删除失败: 服务器错误\n" + e.Error.Message);
                return;
            }
            if (e.Result.ReturnValue)
            {
                List<int> ids = e.Result.Obj as List<int>;
                foreach (var child in ids)
                {
                    foreach (var item in models)
                    {
                        if (child == item.ID)
                        {
                            models.Remove(item);
                            break;
                        }
                    }
                }
                GridProList.Rebind();
                foreach (var child in GridProList.SelectedItems)
                {
                    API.View_YanBoPriceStepInfo yb = child as API.View_YanBoPriceStepInfo;

                    if (!Convert.ToBoolean(yb.UpdateFlag))
                    {
                        foreach (var item in models)
                        {
                            if (yb.ID == item.ID)
                            {
                                models.Remove(item);
                                break;
                            }
                        }
                    }
                }
                    
                GridProList.Rebind();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,e.Result.Message);
            }
        }

        private void GridProList_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            foreach (var item in models)
            {
                if (item.LowPrice < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"结算价不能小于0");
                    item.LowPrice = Decimal.Truncate(Convert.ToDecimal(item.LowPrice * 100)) / 100;
                    return;
                }
                if (item.ProCost < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"成本不能小于0");
                    item.ProCost = Decimal.Truncate(Convert.ToDecimal(item.ProCost * 100)) / 100;
                    return;
                }
                if (item.ProPrice < 0)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"价格不能小于0");
                    item.ProPrice = Decimal.Truncate(Convert.ToDecimal(item.OldProPrice * 100)) / 100;
                    return;
                }

                item.LowPrice = item.LowPrice < 0 ? 
                    Decimal.Truncate(Convert.ToDecimal(item.OldLowPrice * 100)) / 100
                    : Decimal.Truncate(Convert.ToDecimal(item.LowPrice * 100)) / 100;


                item.ProCost = item.ProCost < 0 ?
                      Decimal.Truncate(Convert.ToDecimal(item.OldProCost * 100)) / 100
                    : Decimal.Truncate(Convert.ToDecimal(item.ProCost * 100)) / 100;

                item.ProPrice = item.ProPrice < 0 ?
                          Decimal.Truncate(Convert.ToDecimal(item.OldProPrice * 100)) / 100
                    : Decimal.Truncate(Convert.ToDecimal(item.ProPrice * 100)) / 100;

                item.StepPrice = Decimal.Truncate(Convert.ToDecimal(item.StepPrice * 100)) / 100;
                
            }
        }

    }
}