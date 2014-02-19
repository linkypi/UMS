using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 等级
    /// </summary>
    public class Pro_LevelInfo : Sys_InitParentInfo
    {
        
          private int _MethodID;
        //private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Pro_LevelInfo()
        {
            this.MethodID = 0;
        }

        public Pro_LevelInfo(int MenthodID)
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
        #region 获取等级实体
        /// <summary>
        /// 获取等级实体
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

                    var inorder_query = from b in lqh.Umsdb.Pro_LevelInfo
                                        select b;
                       
                    #endregion

                    #region 排序
                    inorder_query = from b in inorder_query
                                    orderby b.LevelID descending
                                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.Pro_LevelInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.Pro_LevelInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
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

        #region 修改等级
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Pro_LevelInfo model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否存在区域名称
                        if (!string.IsNullOrEmpty(model.LevelName))
                        {
                            int query = (from b in lqh.Umsdb.Pro_AreaInfo
                                         where b.AreaName == model.LevelName
                                         select b).Count();
                            if (query > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "等级名称已存在！" };
                            }
                        }
                        #endregion

                        var queryArea = from b in lqh.Umsdb.Pro_LevelInfo
                                         where b.LevelID == model.LevelID
                                         select b;
                        if (queryArea.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "等级不存在！" };
                        }
                        Model.Pro_LevelInfo Level = queryArea.First();
                        #region 判断区域是否在使用中
                        if (Level.Flag == true)
                        {
                            if (model.Flag == false)
                            {
                                var queryFlag = from b in lqh.Umsdb.Pro_HallInfo
                                                where b.LevelID == model.LevelID && b.Flag == true
                                                select b;
                                if (queryFlag.Count() == 0)
                                {
                                    return new Model.WebReturn() {  ReturnValue=false, Message = "等级正在使用中，不能修改使用状态！" };
                                }                 
                            }
                        }
                        #endregion 
                        if (!string.IsNullOrEmpty(model.LevelName))
                            Level.LevelName = model.LevelName;
                        Level.Flag = model.Flag;
                        Level.Order = model.Order;
                        Level.Note = model.Note;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = Level, ReturnValue = true, Message = "修改成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        #region 新增等级
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_LevelInfo model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否存在等级名称
                        if (string.IsNullOrEmpty(model.LevelName))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未填写等级名称！" };
                        }
                        #endregion

       
                        #region 判断数据有效性
                        int query = (from b in lqh.Umsdb.Pro_LevelInfo
                                     where b.LevelName == model.LevelName
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "等级名称已存在！" };
                        }
                        #endregion

                        model.Flag = true;
                        lqh.Umsdb.Pro_LevelInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() {ReturnValue = true, Message = "新增成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        #region 删除等级
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.Pro_LevelInfo model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否使用证件类型名称
                        int query = (from b in lqh.Umsdb.Pro_HallInfo
                                     where b.LevelID == model.LevelID&&b.Flag==true
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "等级正在使用，不能删除！" };
                        }
                        #endregion
                        var queryArea = from b in lqh.Umsdb.Pro_LevelInfo
                                         where b.LevelID == model.LevelID
                                         select b;
                        if (queryArea.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "等级不存在！" };
                        }
                        Model.Pro_LevelInfo Level = queryArea.First();
                        lqh.Umsdb.Pro_LevelInfo.DeleteOnSubmit(Level);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = Level, ReturnValue = true, Message = "删除成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion


        public List<Model.Pro_LevelInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
              using(LinQSqlHelper lqh=new LinQSqlHelper())
            {
              
                    try
                    {                    
                        var query = from b in lqh.Umsdb.GetTable<Model.Pro_LevelInfo>()
                                         where b.Flag == true
                                         select b;
                        if (query == null || query.Count() == 0)
                        {
                            return null;
                        }
                        //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                        //arr.AddRange(query);
                        return query.ToList();
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }
         
        
        
    }
}
