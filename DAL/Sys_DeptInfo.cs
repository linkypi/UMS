using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Sys_DeptInfo : Sys_InitParentInfo
    {
         private int MenthodID;

	    public Sys_DeptInfo()
	    {
		    this.MenthodID = 0;
	    }

        public Sys_DeptInfo(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }
        public List<Model.Sys_DeptInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_DeptInfo>()
                                where b.Flag == true
                                select b;
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Sys_DeptInfo>();
                }
            }
        }
        #region 获取部门
        public Model.WebReturn GetModel(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = (from b in lqh.Umsdb.GetTable<Model.Sys_DeptInfo>()
                                 where b.Flag == true
                                 select b).ToList();
                    if (query == null || query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "获取失败" };
                    }
                    return new Model.WebReturn() { Obj = query, ReturnValue = true, Message = "获取成功" };
                }
                catch 
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取失败" };
                }
            }
        }
        #endregion

        #region 新增部门
        public Model.WebReturn Add(Model.Sys_UserInfo user,Model.Sys_DeptInfo model)
        {
            if(model==null) return new Model.WebReturn() { ReturnValue = false, Message = "无参数！" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(model.DtpName))
                            return new Model.WebReturn() { ReturnValue = false, Message = "部门名称不能为空！" };
                        var query = from b in lqh.Umsdb.Sys_DeptInfo
                                    where b.Flag == true && b.DtpName == model.DtpName
                                    select b;
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部门名称已存在" };
                        }

                   
                        model.Flag = true;
                        lqh.Umsdb.Sys_DeptInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = model, ReturnValue = true, Message = "新增成功" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "新增失败" };
                    }
                }
            }
        }
        #endregion

        #region 删除部门
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.Sys_DeptInfo model)
        {
            if (model == null) return new Model.WebReturn() { ReturnValue = false, Message = "无参数！" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        if (model.DtpID==0)
                            return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };

                        var query = from b in lqh.Umsdb.Sys_DeptInfo
                                    where b.Flag == true && b.Parent == model.DtpID
                                    select b;
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "存在子部门，无法删除！" };
                        }

                        var Dep = from b in lqh.Umsdb.Sys_DeptInfo
                                    where b.Flag == true && b.DtpID == model.DtpID
                                    select b;
                        if (Dep.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该部门已删除或不存在！" };
                        }
                        Dep.First().Flag = false;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = model, ReturnValue = true, Message = "删除成功！" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "删除失败" };
                    }
                }
            }
        }
        #endregion

        #region 更新部门
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Sys_DeptInfo model)
        {
            if (model == null) return new Model.WebReturn() { ReturnValue = false, Message = "无参数！" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        if (model.DtpID == 0)
                            return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };

                  

                        var Dep = from b in lqh.Umsdb.Sys_DeptInfo
                                  where b.Flag == true && b.DtpID == model.DtpID
                                  select b;
                        if (Dep.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该部门已删除或不存在！" };
                        }
                        Dep.First().DtpName = model.DtpName;
                        Dep.First().Head = model.Head;
                        Dep.First().HeadTele = model.HeadTele;
                        Dep.First().Note = model.Note;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = model, ReturnValue = true, Message = "更新成功！" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "更新失败" };
                    }
                }
            }
        }
        #endregion
    }
}
