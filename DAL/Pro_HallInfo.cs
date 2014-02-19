using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;


namespace DAL
{
    /// <summary>
    /// 门店
    /// </summary>
    public class Pro_HallInfo : Sys_InitParentInfo
    {

        private int MethodID;

        public Pro_HallInfo()
        {
            this.MethodID = 0;
        }

        public Pro_HallInfo(int methodID)
        {
            this.MethodID = methodID;
        }


        public List<Model.Pro_HallInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_HallInfo>()
                                where b.Flag == true
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}
                    //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query);

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Pro_HallInfo>();
                }
            }
        }
        #region 获取仓库model
        /// <summary>
        ///  获取仓库model
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {

                    #region 权限
                    //List<string> ValidHallIDS = new List<string>();
                    ////有权限的商品
                    //List<string> ValidProIDS = new List<string>();

                    //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    //if (ret.ReturnValue != true)
                    //{ return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0 )
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }

                    #region 获取数据

                    var inorder_query = from b in lqh.Umsdb.View_HallInfo
                                        where b.Flag == true 
                                        select b;
                    #endregion

                    #region 排序
                    inorder_query = from b in inorder_query
                                    orderby b.HallID descending
                                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.View_HallInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.View_HallInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错 ！" };

                }
            }
        }
        #endregion
        #region 新增仓库
        public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.Role_Hall> RoleList, List<Model.View_HallInfo> HallList)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        List<Model.Pro_HallInfo> OldHallList = (from b in lqh.Umsdb.Pro_HallInfo select b).ToList();
                        var query = (from b in HallList
                                     join c in OldHallList on b.HallName equals c.HallName
                                     select c).ToList();
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已存在门店！" };
                        }
                        string msg = "";
                        lqh.Umsdb.OrderMacker(HallList.Count(), "CK", "CK", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "生成门店编号出错" };
                        }
                        string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (InListIDStr.Count() != HallList.Count())
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "生成门店编号数量与门店数量不一致" };
                        }
                        List<Model.Pro_HallInfo> NewHallList = new List<Model.Pro_HallInfo>();
                        int i = 0;
                        foreach (var HallItem in HallList)
                        {
                            Model.Pro_HallInfo Hall = new Model.Pro_HallInfo();
                            Hall.HallID = InListIDStr[i];
                            Hall.HallName = HallItem.HallName;
                            Hall.AreaID = HallItem.AreaID;
                            Hall.CanBack = HallItem.CanBack;
                            Hall.CanIn = HallItem.CanIn;
                            Hall.DisPlayName = HallItem.DisPlayName;
                            Hall.Flag = true;
                            Hall.Latitude = HallItem.Latitude;
                            Hall.LevelID = HallItem.LevelID;
                            Hall.Longitude = HallItem.Longitude;
                            Hall.Note = HallItem.Note;
                            Hall.Order = HallItem.Order;
                            Hall.PrintName = HallItem.PrintName;
                            Hall.SellNum = HallItem.SellNum;
                            Hall.ShortName = HallItem.ShortName;

                            NewHallList.Add(Hall);
                            i++;

                        }
                        lqh.Umsdb.Pro_HallInfo.InsertAllOnSubmit(NewHallList);
                     //   lqh.Umsdb.SubmitChanges();



                        #region 为角色添加仓库
                        List<Model.Sys_Role_Menu_HallInfo> Role_HallList = new List<Model.Sys_Role_Menu_HallInfo>();
                        if (RoleList != null)
                        {
                            foreach (var item in RoleList)
                            {
                                if (item.HallInfo == null)
                                    continue;
                        #endregion

                                #region 为菜单添加商品类别权限


                                var Role_Hall = (from b in item.HallInfo
                                                 join c in NewHallList on b.HallName equals c.HallName
                                                 select c).ToList();


                                foreach (var HallItem in Role_Hall)
                                {
                                    Model.Sys_Role_Menu_HallInfo Role_HallItem = new Model.Sys_Role_Menu_HallInfo();
                                    Role_HallItem.HallID = HallItem.HallID;

                                    Role_HallItem.RoleID = item.RoleID;
                                    Role_HallList.Add(Role_HallItem);
                                }


                                #endregion
                            }
                            Role_HallList = (from b in Role_HallList
                                             where ((from c in lqh.Umsdb.Sys_Role_Menu_HallInfo
                                                     where b.RoleID == c.RoleID && b.HallID == c.HallID
                                                     select c).Count() == 0)
                                             select b).ToList();
                            lqh.Umsdb.Sys_Role_Menu_HallInfo.InsertAllOnSubmit(Role_HallList);
                            lqh.Umsdb.SubmitChanges();
                        }
                    
                            ts.Complete();

                            return new Model.WebReturn() { ReturnValue = true, Message = "添加成功" };
                        }
                    
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "添加失败！" };
                    }
                }
            }
        }
        #endregion

        #region 修改仓库
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Pro_HallInfo model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否存在类别名称
                        if (!string.IsNullOrEmpty(model.HallName))
                        {
                            int query = (from b in lqh.Umsdb.Pro_HallInfo
                                         where b.HallName == model.HallName&&b.HallID!=model.HallID
                                         select b).Count();
                            if (query > 1)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "门店名称已存在！" };
                            }
                        }
                        #endregion

                        var queryHall= (from b in lqh.Umsdb.Pro_HallInfo
                                        where b.HallID == model.HallID
                                        select b).ToList();
                        if (queryHall.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "门店不存在！" };
                        }
                        Model.Pro_HallInfo HallInfo = queryHall.First();
                        #region 修改门店
                        HallInfo.AreaID = model.AreaID;
                        HallInfo.CanBack = model.CanBack;
                        HallInfo.CanIn = model.CanIn;
                        HallInfo.DisPlayName = model.DisPlayName;
                        HallInfo.Flag = model.Flag;
                        HallInfo.HallName = model.HallName;
                        HallInfo.Latitude = model.Latitude;
                        HallInfo.LevelID = model.LevelID;
                        HallInfo.Longitude = model.Longitude;
                        HallInfo.Note = model.Note;
                        HallInfo.Order = model.Order;
                        HallInfo.PrintName = model.PrintName;
                        HallInfo.SellNum = model.SellNum;
                        HallInfo.ShortName = model.ShortName;
                        
                        #endregion
                
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() {  ReturnValue = true, Message = "修改成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

    }
}
