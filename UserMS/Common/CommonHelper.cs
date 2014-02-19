using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using UserMS.API;

namespace UserMS.Common
{
    public class CommonHelper
    {
        public static bool IsDateTime(string times)
        {
            DateTime temp;
            return DateTime.TryParse(times, out temp);

          
        }

        public static decimal CheckProCashTicket(API.Pro_SellListInfo model, API.Pro_ProInfo band)
        {

            if (band == null)
            {
                //model.TicketID = null;
                //model.TicketUsed = 0;
                //return 0;
                throw new Exception("商品不存在");
            }
            switch (model.SellType)
            {
                case 10:
                case 14:
                case 13:
                case 16:
                {
                    if (string.IsNullOrEmpty(model.TicketID))
                    {
                        return 0;
                        break;
                    }
                    else
                    {
                        try
                        {
                            {
                                string ZSREG = @"^7[56]020(\d\d)(\d\d)(\d\d)(\d{11})(\+\d){0,1}$";
                                string ZSREG2 = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{6})(\+\d){0,1}$";
                                Regex ex = new Regex(ZSREG);
                                Regex ex2 = new Regex(ZSREG2);
                                Regex ok;
                                if (!ex.IsMatch(model.TicketID))
                                {
                                    if (!ex2.IsMatch(model.TicketID))
                                    {
                                        throw new Exception(model.TicketID + "合约码或购机送费码格式不对");
                                    }
                                    else
                                    {
                                        ok = ex2;
                                    }
                                }
                                else
                                {
                                    ok = ex;
                                }
                                MatchCollection match = ok.Matches(model.TicketID);
                                string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" +
                                              match[0].Groups[3];
                                decimal? ticket = model.CashTicket;
                                ;
                                if (!IsDateTime(date))
                                {
                                    throw new Exception(model.TicketID + "合约码或购机送费码格式不对");
                                }
                                return 0;
                              
                            }
                        }
                        catch
                        {
                             goto case 2;
                        }
                       
                    }
                }
                case 2:
                case 7:
                case 9:
                
                    {
                        decimal TicketUsed = 0;
                        string ZSREG = @"^S7[56]0(\d\d)(\d\d)(\d\d)([0-9A-Z]{8})$";
                        Regex ex = new Regex(ZSREG);
                        if (!ex.IsMatch(model.TicketID))
                        {
                            throw new Exception(model.TicketID + "券编码格式不对");
                        }
                        MatchCollection match = ex.Matches(model.TicketID);
                        string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                        //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                        decimal ticket = model.CashTicket;

                        if (!IsDateTime(date))
                        {
                            throw new Exception(model.TicketID + "券编码格式不对");
                        }
                        if (Convert.ToDateTime(date) < band.SepDate && !band.BeforeSep)
                        {
                            throw new Exception(band.SepDate + "之前的券不能兑换" + band.ProName);
                        }
                        else if (Convert.ToDateTime(date) > band.SepDate && !band.AfterSep)
                        {
                            throw new Exception(band.SepDate + "之后的券不能兑换" + band.ProName);
                        }
                        else if (Convert.ToDateTime(date) < band.SepDate)
                            ticket += band.BeforeRate;
                        else
                            ticket += band.AfterRate;


                        if (model.CashTicket < band.TicketLevel) //小于临界值
                        {
                            ticket += band.BeforeTicket;
                            TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                            //lbl.Text += "1";

                        }
                        else //大于等于临界值
                        {
                            ticket += band.AfterTicket;
                            if (band.NeedMoreorLess == true) //需要补差
                                TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                            else
                            {
                                TicketUsed = model.ProPrice; //<= ticket ? ticket : model.ProPrice;
                                //if(band.TicketLevel> 0)
                                //    model.ProPrice = model.TicketPrice;
                            }
                        }
                        //model.CashPrice = model.ProPrice - model.TicketUsed;

                        return TicketUsed;
                        break;
                    }
                case 4:
                    {
                        decimal TicketUsed = 0;
                        string ZSREG = @"^7[56]020(\d\d)(\d\d)(\d\d)(\d{11})(\+\d){0,1}$";
                        Regex ex = new Regex(ZSREG);
                        if (!ex.IsMatch(model.TicketID))
                        {
                            throw new Exception(model.TicketID + "编码格式不对");
                        }
                        MatchCollection match = ex.Matches(model.TicketID);
                        string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                        //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                        decimal ticket = model.CashTicket;
                        ;
                        if (!IsDateTime(date))
                        {
                            throw new Exception(model.TicketID + "编码格式不对");
                        }
                    }
                    return 0;
                    break;
                case 5:
                    {
                        decimal TicketUsed = 0;
                        string ZSREG = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{9})$";
                        Regex ex = new Regex(ZSREG);
                        if (!ex.IsMatch(model.TicketID))
                        {
                            throw new Exception(model.TicketID + "券编码格式不对");
                        }
                        MatchCollection match = ex.Matches(model.TicketID);
                        string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                        //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                        decimal ticket = model.CashTicket;
                        ;
                        if (!IsDateTime(date))
                        {
                            throw new Exception(model.TicketID + "券编码格式不对");
                        }
                        //                    if (Convert.ToDateTime(date) < band.SepDate && band.BeforeRate == 0)
                        //                    {
                        //                        throw new Exception(band.SepDate + "之前的券不能兑换" + band.ProName);
                        //                    }
                        //                    else if (Convert.ToDateTime(date) > band.SepDate && band.AfterRate == 0)
                        //                    {
                        //                        throw new Exception(band.SepDate + "之后的券不能兑换" + band.ProName);
                        //                    }
                        //                    else if (Convert.ToDateTime(date) < band.SepDate)
                        //                        ticket += band.BeforeRate;
                        //                    else
                        //                        ticket += band.AfterRate;
                        //
                        //
                        //                    if (model.CashTicket < band.TicketLevel) //小于临界值
                        //                    {
                        //                        ticket += band.BeforeTicket;
                        //                        TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                        //                        //lbl.Text += "1";
                        //
                        //                    }
                        //                    else //大于等于临界值
                        //                    {
                        //                        ticket += band.AfterTicket;
                        //                        if (band.NeedMoreorLess == true) //需要补差
                        //                            TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                        //                        else
                        //                        {
                        //                            TicketUsed = model.ProPrice; //<= ticket ? ticket : model.ProPrice;
                        //                            //if(band.TicketLevel> 0)
                        //                            //    model.ProPrice = model.TicketPrice;
                        //                        }
                        //                    }
                        //                    //model.CashPrice = model.ProPrice - model.TicketUsed;
                        //
                        //                    return TicketUsed;
                        return 0;
                        break;
                    }
                default:
                    return 0;
                    break;
            }
        }


        public static bool ButtonNotic(object sender)
        {
            string btntext = "";
            if (sender is RadButton)
            {
                btntext = ((RadButton)sender).Content.ToString();
            }
            else if (sender is Button)
            {
                btntext = ((Button)sender).Content.ToString();
            }
            else if (sender is RadMenuItem)
            {
                btntext = ((RadMenuItem) sender).Header.ToString();
            }
            else
            {
                btntext = sender.ToString();
            }
            MessageBoxResult rsltMessageBox = MessageBox.Show(System.Windows.Application.Current.MainWindow,"是否" + btntext, "提示", MessageBoxButton.YesNo,
                                                              MessageBoxImage.Warning);
            if (rsltMessageBox == MessageBoxResult.No)
            {
                return true;
            }
            return false;
        }

        public static List<API.Pro_HallInfo> FilterHall(int menuID, List<API.Pro_HallInfo> hallList)
        {
            return
               hallList.Join(
                   Store.LoginRoleInfo.Sys_Role_Menu_HallInfo.Select(info => info.HallID).Distinct(),
                   info => info.HallID, s => s, (info, s) => info).Distinct().ToList();

            var menuInfo = Store.RoleInfo.First().Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
            if (menuInfo.Any())
            {

                // 左连接获取仓库列表              
                var query = (from b in Store.RoleInfo.First().Sys_Role_Menu_HallInfo
                             where b.MenuID == menuID
                             join c in hallList
                             on b.HallID equals c.HallID into hall
                             from b1 in hall.DefaultIfEmpty()
                             select b1).ToList();
                if (query.Any())
                    return query;

            }

            return new List<Pro_HallInfo>();
        }

        public static List<TreeViewModel> AreaTreeViewModel(List<API.Pro_AreaInfo> areaInfo)
        {
            List<TreeViewModel> result = new List<TreeViewModel>();
            foreach (var proAreaInfo in areaInfo)
            {
                TreeViewModel p = new TreeViewModel();

                p.NewID = proAreaInfo.AreaID;
                p.Title = proAreaInfo.AreaName;
                p.Fields = new string[] { "AreaID" };
                p.Values = new object[] { proAreaInfo.AreaID };
                result.Add(p);
            }

            return result;
        }

      
        /// <summary>
        /// 获取区域树 包括大区   传入hallinfo是为了让其关联大区
        /// </summary>
        /// <param name="areaInfo"></param>
        /// <param name="hallinfo"></param>
        /// <returns></returns>
        public static List<TreeViewModel> AreaTreeModel(List<API.Pro_AreaInfo> areaInfo, List<API.Pro_HallInfo> hallinfo)
        {
            List<TreeViewModel> Parent = new List<TreeViewModel>();

            foreach (var item in Store.BigAreaInfo)
            {
                TreeViewModel parent = new TreeViewModel()
                {
                    Title = item.BigAreaName,
                    NewID = item.BigAreaID
                };
                parent.Fields = new string[] { "Order" };
                parent.Values = new object[] { item.BigAreaID };
                parent.Children = new List<TreeViewModel>();
                var area = from a in areaInfo
                           where a.BigAreaID == item.BigAreaID
                           select a;
                foreach (var proAreaInfo in area)
                {
                    TreeViewModel p = new TreeViewModel();
                    p.Fields = new string[] { "AreaID" };
                    p.Values = new object[] { proAreaInfo.AreaID };
                    p.NewID = proAreaInfo.AreaID;
                    p.Title = proAreaInfo.AreaName;
                    parent.Children.Add(p);

                    var hall = from a in hallinfo
                               where a.AreaID == proAreaInfo.AreaID
                               select a;
                    foreach (var child in hall)
                    {
                        child.Order = item.BigAreaID;
                    }
                }
                Parent.Add(parent);
            }

            return Parent;
        }



        public static List<TreeViewModel> AreaTreeModel(List<API.Pro_AreaInfo> areaInfo)
        {
            List<TreeViewModel> Parent = new List<TreeViewModel>();
            TreeViewModel parent = new TreeViewModel()
            {
                Title = "片区"
            };
            parent.Fields = new string[] {  };
            parent.Values = new object[] {  };
            parent.Children = new List<TreeViewModel>();
            foreach (var proAreaInfo in areaInfo)
            {         
                TreeViewModel p = new TreeViewModel();
                p.Fields = new string[] { "AreaID" };
                p.Values = new object[] { proAreaInfo.AreaID };
                p.NewID = proAreaInfo.AreaID;
                p.Title = proAreaInfo.AreaName;
                parent.Children.Add(p);
            }
            Parent.Add(parent);
            return Parent;
        }
        /// <summary>
        /// 卡类型树
        /// </summary>
        /// <param name="areaInfo"></param>
        /// <returns></returns>
        public static List<TreeViewModel> VIPTypeTreeViewModel(List<API.VIP_VIPType> Info)
        {
            List<TreeViewModel> result = new List<TreeViewModel>();
            foreach (var Item in Info)
            {
                TreeViewModel p = new TreeViewModel();
                p.ID = Item.ID.ToString();
                p.Title = Item.Name;
                result.Add(p);
            }

            return result;
        }
     
        public static List<TreeViewModel> ProTypeTreeViewModel(List<Pro_ProInfo> proInfo, bool hasIMEI)
        {
            List<TreeViewModel> result = new List<TreeViewModel>();

            if (hasIMEI == true)
            {
                var classList = (from b in proInfo
                                 join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                                 select c).Distinct().ToList();

                if (classList.Count() != 0)
                {
                    TreeViewModel tvm = null;
                    foreach (var ti in classList)
                    {
                        tvm = new TreeViewModel();
                        tvm.ID = ti.ClassID.ToString();
                        tvm.Title = ti.ClassName;
                        result.Add(tvm);
                    }
                }
            }
            else
            {
                var test = proInfo.Where(b => b!=null &&  b.NeedIMEI != true);//BUG: b爲什麽可以是null
                var classList = (proInfo.Where(b => b != null && b.NeedIMEI != true)//BUG: b爲什麽可以是null
                                        .Join(Store.ProClassInfo, b => b.Pro_ClassID, c => c.ClassID, (b, c) => c)).Distinct().ToList();
                if (classList.Count() != 0)
                {
                    TreeViewModel tvm = null;
                    foreach (var ti in classList)
                    {
                        tvm = new TreeViewModel();
                        tvm.ID = ti.ClassID.ToString();
                        tvm.Title = ti.ClassName;
                        result.Add(tvm);
                    }
                }
            }
            return result;


        }


        public static List<API.Pro_HallInfo> GetHalls(int menuID)
        {
            return
                Store.ProHallInfo.Join(
                    Store.LoginRoleInfo.Sys_Role_Menu_HallInfo.Select(info => info.HallID).Distinct(),
                    info => info.HallID, s => s, (info, s) => info).Distinct().ToList();


            var menuInfo = Store.LoginRoleInfo.Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
            if (menuInfo.Any())
            {
                if (menuInfo.First().Note == null)
                {
                    // 左连接获取商品列表              
                    var query = (from b in Store.LoginRoleInfo.Sys_Role_Menu_HallInfo
                                 where b.MenuID == menuID
                                 join c in Store.ProHallInfo
                                 on b.HallID equals c.HallID into Production
                                 from b1 in Production.DefaultIfEmpty()
                                 select b1).ToList();
                    if (query.Any())
                        return query;
                }
            }
            return new List<Pro_HallInfo>();
        }

        /// <summary>
        /// 获取可操作权限商品
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public static List<API.Pro_ProInfo> GetPro(int menuID)
        {
            //var query=from b in 
            var menuInfo = Store.LoginRoleInfo.Sys_Role_MenuInfo.Where(p => p.MenuID == menuID);
            if (menuInfo.Any())
            {
               
                    // 左连接获取商品列表              
                    var query = (Store.LoginRoleInfo.Sys_Role_Menu_ProInfo.Where(b => b.MenuID == menuID)
                                      .GroupJoin(Store.ProInfo, b => b.ClassID, c => c.Pro_ClassID,
                                                 (b, Production) => new {b, Production})
                                      .SelectMany(@t => @t.Production.DefaultIfEmpty())).Where(p=>p!=null).ToList();
                    if (query.Any())
                        return query;
               
            }
            return new List<Pro_ProInfo>();
        }
        public static List<TreeViewModel> HallTreeViewModel()
        {
            List<TreeViewModel> result = new List<TreeViewModel>();
            foreach (var proAreaInfo in Store.AreaInfo)
            {
                TreeViewModel p = new TreeViewModel();
                p.ID = "";
                p.Title = proAreaInfo.AreaName;
                p.Children = new List<TreeViewModel>();
                Pro_AreaInfo info = proAreaInfo;
                foreach (var hall in Store.ProHallInfo.Where(q => q.AreaID == info.AreaID))
                {
                    TreeViewModel c = new TreeViewModel();
                    c.ID = hall.HallID;
                    c.Title = hall.HallName;
                    p.Children.Add(c);

                }
                result.Add(p);

            }
            return result;
        }

        public static TreeViewModel SalesViewModel(int salesNameID, List<API.Package_SalesNameInfo> SalesNameInfo)
        {
            var Children = from b in SalesNameInfo
                           where b.Parent == salesNameID
                           select b;
            var pnode = from b in SalesNameInfo
                        where b.ID == salesNameID
                        select b;
            TreeViewModel parent = new TreeViewModel() { Title = pnode.First().SalesName, NewID = pnode.First().ID };
            if (Children.Count() != 0)
            {
                foreach (var Item in Children)
                {
                    TreeViewModel child = SalesViewModel(Item.ID, SalesNameInfo);
                    if (parent.Children == null) parent.Children = new List<TreeViewModel>();
                    parent.Children.Add(child);
                }
            }
            return parent;
        }



        public static void ProFilterGen(List<API.Pro_ProInfo> proInfo, ref  List<SlModel.ProductionModel> production, ref List<TreeViewModel> ParentTree)
        {
            if (proInfo == null || proInfo.Count() == 0)
            {
                
                return;
            }
            //获取左边树
            ParentTree = new List<TreeViewModel>();


            var classList = from b in proInfo
                            join c in Store.ProClassInfo on b.Pro_ClassID equals c.ClassID
                            join d in Store.ProTypeInfo on b.Pro_TypeID equals d.TypeID
                            group d by c into TypeList
                            select new
                            {
                                TypeList.Key,
                                k = TypeList.Distinct().ToList()
                            };
            if (classList.Count() == 0)
            {
                return;
            }
            foreach (var list in classList)
            {
                if (list.Key == null)
                {
                    continue;
                }
                TreeViewModel ParentModel = new TreeViewModel();

                ParentModel.Title = list.Key.ClassName;

                if (ParentModel.Fields == null)
                {
                    ParentModel.Fields = new string[] { "ClassID" };
                    ParentModel.Values = new object[] { list.Key.ClassID.ToString() };

                }

                ParentModel.Children = new List<TreeViewModel>();

                foreach (var Newlist in list.k)
                {
                    TreeViewModel ChildModel = new TreeViewModel() { ID = Newlist.TypeID.ToString(), Title = Newlist.TypeName };
                    if (ChildModel.Fields == null)
                    {
                        ChildModel.Fields = new string[] { "ClassID", "TypeID" };
                        ChildModel.Values = new object[] { list.Key.ClassID.ToString(), Newlist.TypeID.ToString() };

                    }
                    ParentModel.Children.Add(ChildModel);
                }
                ParentTree.Add(ParentModel);
            }

          
                production = new List<SlModel.ProductionModel>();
                var query = (from b in proInfo
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
                                 TypeID = d.TypeID,
                                 TypeName = d.TypeName,
                                 ProMainID = b.ProMainID
                             }).ToList();
                foreach (var index in query)
                {
                    SlModel.ProductionModel pro = new SlModel.ProductionModel();
                    pro.ProID = index.ProID;
                    pro.ProMainID = Convert.ToInt32(index.ProMainID);
                    pro.ProName = index.ProName;
                    pro.ProFormat = index.ProFormat;
                    pro.IsNeedIMEI = index.IsNeedIMEI;
                    pro.ClassName = index.ClassName;
                    pro.ClassID = index.ClassID.ToString();
                    pro.TypeName = index.TypeName;
                    pro.TypeID = index.TypeID.ToString();
                    production.Add(pro);

                }
            }


   

    }
}