using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{

    /// <summary>
    /// 类别
    /// 
    /// 
    /// 没有flag这个值
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    public class Pro_ClassInfo : Sys_InitParentInfo
    {

        private int _MethodID;
        //private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Pro_ClassInfo()
        {
            this.MethodID = 0;
        }

        public Pro_ClassInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        //private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
        //    new Model.ReportSqlParams_String(){ParamName="ID" },
        //    new Model.ReportSqlParams_String(){ParamName="Name"},
        //    new Model.ReportSqlParams_String(){ParamName="Flag"}, 
        //};
        //public List<Model.ReportSqlParams> ParamList
        //{
        //    get { return _paramList; }
        //    set { _paramList = value; }
        //}
        #region 获取商品类型实体
        /// <summary>
        /// 获取类型实体
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

                    if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize != 20)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                                    
                    #region 获取数据

                    var inorder_query = from b in lqh.Umsdb.Pro_ClassInfo
                                        where b.IsDeleted==false||b.IsDeleted==null
                                        select b;
                       
                    #endregion

                    #region 排序
                    inorder_query = from b in inorder_query
                                    orderby b.ClassID descending
                                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.Pro_ClassInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.Pro_ClassInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message ="服务端出错 ！" };

                }
            }
        }
        #endregion

        #region 修改商品类别
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Pro_ClassInfo model)
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
                        if (!string.IsNullOrEmpty(model.ClassName))
                        {
                            int query = (from b in lqh.Umsdb.Pro_ClassInfo
                                         where b.ClassName == model.ClassName
                                         select b).Count();
                            if (query > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "商品类别名称已存在！" };
                            }
                        }
                        #endregion

                        var queryArea = from b in lqh.Umsdb.Pro_ClassInfo
                                         where b.ClassID == model.ClassID
                                         select b;
                        if (queryArea.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "商品类别不存在！" };
                        }
                        Model.Pro_ClassInfo ClassInfo = queryArea.First();
                        #region 商品类别是否在使用中
                        if (ClassInfo.IsDeleted == false||ClassInfo.IsDeleted==null)
                        {
                            if (model.IsDeleted == true)
                            {
                                var queryFlag = from b in lqh.Umsdb.Pro_ProInfo
                                                where b.Pro_ClassID == model.ClassID 
                                                select b;
                                if (queryFlag.Count() == 0)
                                {
                                    return new Model.WebReturn() {  ReturnValue=false, Message = "商品类别正在使用中，不能修改使用状态！" };
                                }                 
                            }
                        }
                        #endregion 
                        if (!string.IsNullOrEmpty(model.ClassName))
                            ClassInfo.ClassName = model.ClassName;              
                        ClassInfo.Order = model.Order;
                        ClassInfo.Note = model.Note;
                        ClassInfo.HasSalary = model.HasSalary;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = ClassInfo, ReturnValue = true, Message = "修改成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        #region 新增商品类别
        public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.Role_Pro_Property> RoleList,List<Model.ProClassModel> ClassList)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        List<Model.Pro_ClassInfo> OldProList = (from b in lqh.Umsdb.Pro_ClassInfo select b).ToList();
                        var query = (from b in ClassList
                                     join c in OldProList on b.ClassName equals c.ClassName
                                     select c).ToList();
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已存在类别！" };
                        }
                        List<Model.Pro_ClassInfo> ProList = new List<Model.Pro_ClassInfo>();
                        var classList =(from b in lqh.Umsdb.Pro_ClassInfo
                                        select b.ClassID).ToList();

                        foreach (var ProItem in ClassList)
                        {
                            Model.Pro_ClassInfo Pro = new Model.Pro_ClassInfo();
                            Pro.ClassName = ProItem.ClassName;
                            Pro.Note = ProItem.Note;
                            Pro.Order = ProItem.Order;
                            Pro.HasSalary = ProItem.HasSalary;
                            if (classList.Contains(ProItem.LikeClass))
                            {
                                var x = (from o in lqh.Umsdb.Sys_Role_Menu_ProInfo
                                         where o.ClassID == ProItem.LikeClass
                                         && lqh.Umsdb.Sys_UserInfo.Any(p => p.RoleID == o.RoleID
                                             && ProItem.UserIDS.Contains(p.UserID))
                                         select o).ToList();
                                var y = from b in x
                                        select new Model.Sys_Role_Menu_ProInfo
                                        {
                                            MenuID = b.MenuID,
                                            Note = b.Note,
                                            RoleID = b.RoleID
                                        };

                                Pro.Sys_Role_Menu_ProInfo.AddRange(y);
                            }
                            ProList.Add(Pro);
                            
                        }
                        lqh.Umsdb.Pro_ClassInfo.InsertAllOnSubmit(ProList);
                        lqh.Umsdb.SubmitChanges();


                       
                        //#region 获取并设置商品类别
                        //List<Model.Sys_Role_Menu_ProInfo> Role_Pro = new List<Model.Sys_Role_Menu_ProInfo>();
                        //foreach (var item in RoleList)
                        //{
                        //    if (item.ProClassModel == null)
                        //        continue;                                                        
                        //    #endregion
                        //    #region 获取角色的所有菜单
                        //    List<Model.Sys_Role_MenuInfo> Role_MenuInf = (from b in lqh.Umsdb.Sys_Role_MenuInfo
                        //                                                  where b.RoleID == item.RoleID
                        //                                                  select b).Distinct().ToList();
                        //    #endregion
                        //    #region 为菜单添加商品类别权限
                       

                        //    var Role_Class=(from b in item.ProClassModel
                        //                   join c in ProList on b.ClassName equals c.ClassName
                        //                   select c).ToList();
                            
                        //    foreach (var MenuItem in Role_MenuInf)
                        //    {
                        //        foreach (var ProItem in Role_Class)
                        //        {
                        //            Model.Sys_Role_Menu_ProInfo Role_ProItem = new Model.Sys_Role_Menu_ProInfo();
                        //            Role_ProItem.ClassID = ProItem.ClassID;
                        //            Role_ProItem.MenuID = MenuItem.MenuID;
                        //            Role_ProItem.RoleID = MenuItem.RoleID;
                        //            Role_Pro.Add(Role_ProItem);
                        //        }
                        //    }
                 
                        //    #endregion
                        //}
                        //Role_Pro = (from b in Role_Pro
                        //            where ((from c in lqh.Umsdb.Sys_Role_Menu_ProInfo
                        //                    where b.MenuID == c.MenuID && b.RoleID == c.RoleID&& b.ClassID == c.ClassID
                        //                    select c).Count() == 0)
                        //            select b).ToList();
                        //lqh.Umsdb.Sys_Role_Menu_ProInfo.InsertAllOnSubmit(Role_Pro);
                        //lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "添加成功", Obj = ProList };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "添加失败！" };
                    }
                }
            }
        }
        #endregion

        #region 删除商品类别
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.Pro_ClassInfo model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否使用商品类别
                        int query = (from b in lqh.Umsdb.Pro_ProInfo
                                     where b.Pro_ClassID == model.ClassID
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "商品类别正在使用，不能删除！" };
                        }
                        #endregion
                        var queryArea = from b in lqh.Umsdb.Pro_ClassInfo
                                         where b.ClassID == model.ClassID
                                         select b;
                        if (queryArea.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "商品类别不存在！" };
                        }
                        Model.Pro_ClassInfo ClassInfo = queryArea.First();
                        lqh.Umsdb.Pro_ClassInfo.DeleteOnSubmit(ClassInfo);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = ClassInfo, ReturnValue = true, Message = "删除成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        public  List<Model.Pro_ClassInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using(LinQSqlHelper lqh=new LinQSqlHelper())
            {
             
                    try
                    {                    
                        var query= from b in lqh.Umsdb.GetTable<Model.Pro_ClassInfo>()   
                                    //where b.IsDeleted==false||b.IsDeleted==null
                                    select b;
                        //if (query == null || query.Count() == 0)
                        //{
                        //    return null;
                        //}
                        //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                        //arr.AddRange(query);                
                        return query.ToList();
                    }
                    catch(Exception ex)
                    {
                        return new List<Model.Pro_ClassInfo>();
                    }
                }
            }
         
        
        
    }
}
