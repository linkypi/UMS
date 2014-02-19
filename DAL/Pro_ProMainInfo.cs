using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class Pro_ProMainInfo : Sys_InitParentInfo
    {
        private int MethodID;

        public Pro_ProMainInfo()
        {
            this.MethodID = 0;
        }
        private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
              new Model.ReportSqlParams_String(){ParamName="ClassName" },
            new Model.ReportSqlParams_String(){ParamName="TypeName" },
            new Model.ReportSqlParams_String(){ParamName="ProMainName"}
        
        };

        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }
        public List<Model.Pro_ProMainInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.Pro_ProMainInfo
                                select b;
                    return query.ToList();
                }
                catch 
                {
                    return new List<Model.Pro_ProMainInfo>();
                }
            }
        }
        public Pro_ProMainInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        #region 新增总商品
        public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.ProModel> ProModelList)
        {
            if (ProModelList == null)
                return new WebReturn() { ReturnValue = false, Message = "无数据" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证数据有效性并添加商品
      
                        List<Model.Pro_ProMainInfo> ProList = new List<Model.Pro_ProMainInfo>();
                        foreach (var ProItem in ProModelList)
                        {
                    
                            Model.Pro_ProMainInfo Pro = new Model.Pro_ProMainInfo();          
                            Pro.ClassID = ProItem.ClassID;
                            Pro.TypeID = ProItem.TypeID;
                            Pro.ProMainName = ProItem.ProName;
                            Pro.ProNameID = ProItem.ProMainID;
                            Pro.Introduction = ProItem.Introduction;
                            ProList.Add(Pro);
                   
                        }
                        List<Model.Pro_ProMainInfo> OldProList = (from b in lqh.Umsdb.Pro_ProMainInfo
                                                              select b).ToList();
                        int count = (from b in OldProList
                                     join c in ProList on
                                        new
                                        {
                                            b.ClassID,
                                            b.TypeID,
                                            b.ProMainName,
                                            b.ProNameID
                                        }
                                        equals
                               new
                               {
                                   c.ClassID,
                                   c.TypeID,
                                   c.ProMainName,
                                   c.ProNameID
                               }
                                     select c).Count();
                        if (count > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分商品已存在！" };
                        }
                        lqh.Umsdb.Pro_ProMainInfo.InsertAllOnSubmit(ProList);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        #endregion
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

        /// <summary>
        ///  301
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetAllList(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                var inorder_query = from b in lqh.Umsdb.View_ProMainInfo
                                    select b;

                return new WebReturn() { ReturnValue = true,Obj=inorder_query.ToList()};
            }
        }

        #region 获取总商品
        /// <summary>
        /// 获取总商品实体
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


                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }

                    #region 将传入的参数与指定的参数了左连接

                    var param_join = from b in pageParam.ParamList
                                     join c in this.ParamList
                                     on new { b.ParamName, t = b.GetType() }
                                     equals
                                     new { c.ParamName, t = c.GetType() }
                                     into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     select new
                                     {
                                         ParamFront = b,
                                         ParamBehind = c1
                                     };

                    #endregion

                    #region 获取数据

                    var inorder_query = from b in lqh.Umsdb.View_ProMainInfo                                
                                        select b;
                    foreach (var m in param_join)
                    {
                  

                        switch (m.ParamFront.ParamName)
                        {
                            case "ClassName":
                                Model.ReportSqlParams_String mm1 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm1.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.ClassName.Contains(mm1.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "TypeName":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.TypeName.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "ProMainName":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm2.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.ProMainName.Contains(mm2.ParamValues)
                                                    select b;
                                    break;
                                }
                         

                            default: break;
                        }
                    }
                    #endregion

                    #region 排序
                    inorder_query = from b in inorder_query
                                    orderby b.ProMainID descending
                                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.View_ProMainInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list; 
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.View_ProMainInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch 
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错 ！" };
                }
            }
        }
        #endregion

        #region 修改总商品
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Pro_ProMainInfo model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        DataLoadOptions d = new DataLoadOptions();
                        d.LoadWith<Model.Pro_ProMainInfo>(c => c.Pro_ProInfo);            
                        lqh.Umsdb.LoadOptions = d;
                        #region 判断是否存在类别名称
                        //if (!string.IsNullOrEmpty(model.ProMainName))
                        //{
                        //    int query = (from b in lqh.Umsdb.Pro_ProMainInfo
                        //                 where b.ProMainName == model.ProMainName && b.ProMainID != model.ProMainID&&b.ClassID==model.ClassID&&b.TypeID==model.TypeID
                        //                 && b.ProNameID==model.ProNameID
                        //                 select b).Count();
                        //    if (query > 1)
                        //    {
                        //        return new Model.WebReturn() { ReturnValue = false, Message = "总商品名称已存在！" };
                        //    }
                        //}
                        #endregion

                        var queryProMain = (from b in lqh.Umsdb.Pro_ProMainInfo
                                         where b.ProMainID == model.ProMainID
                                         select b).ToList();
                        if (queryProMain.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "总商品不存在！" };
                        }
                        Model.Pro_ProMainInfo ProMain = queryProMain.First();
                        ProMain.Introduction = model.Introduction;
                        #region 修改总商品和子商品
                        //ProMain.ProMainName = model.ProMainName;
                        //if (ProMain.Pro_ProInfo != null)
                        //{
                        //    foreach (var Item in ProMain.Pro_ProInfo)
                        //    {
                        //        Item.ProName = model.ProMainName;
                        //    }
                        //}
                        #endregion
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功！" , Obj=model};
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        /// <summary>
        /// 删除总商品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Pro_ProMainInfo model,bool IsDelete)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        DataLoadOptions d = new DataLoadOptions();
                        d.LoadWith<Model.Pro_ProMainInfo>(c => c.Pro_ProInfo);
                        lqh.Umsdb.LoadOptions = d;
                        
                        var queryProMain = (from b in lqh.Umsdb.Pro_ProMainInfo
                                            where b.ProMainID == model.ProMainID
                                            select b).ToList();
                        if (queryProMain.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "总商品不存在！" };
                        }
                        Model.Pro_ProMainInfo ProMain = queryProMain.First();
                        if (ProMain.Pro_ProInfo.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "总商品正在使用中不能删除！" };
                        }

                        lqh.Umsdb.Pro_ProMainInfo.DeleteOnSubmit(ProMain);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！" };
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
