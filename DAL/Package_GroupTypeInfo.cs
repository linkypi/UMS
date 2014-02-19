using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
   public class Package_GroupTypeInfo
    {
        private int MethodID;

        public Package_GroupTypeInfo()
        {
            this.MethodID = 0;
        }

        public Package_GroupTypeInfo(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

       /// <summary>
       /// 264
       /// </summary>
       /// <param name="user"></param>
       /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                var list = from a in lqh.Umsdb.View_PackageGroupTypeInfo
                           select a;
                return new Model.WebReturn() { ReturnValue = true,Obj=list.ToList()};
            }

        }

       /// <summary>
       /// 添加套餐分组  265
       /// </summary>
       /// <param name="user"></param>
       /// <param name="model"></param>
       /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.View_PackageGroupTypeInfo model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var g = from a in lqh.Umsdb.Package_GroupTypeInfo
                                where a.GroupName == model.GroupName
                                select a;
                        if (g.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已存在相同分组名称，添加失败！" };
                        }

                        Model.Package_GroupTypeInfo group = new Model.Package_GroupTypeInfo();
                        group.GroupName = model.GroupName;
                        group.ClassName = model.ClassName;
                        lqh.Umsdb.Package_GroupTypeInfo.InsertOnSubmit(group);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "添加成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false , Message=ex.Message};
                    }
                }
            }
        }

       /// <summary>
       /// 266
       /// </summary>
       /// <param name="user"></param>
       /// <param name="id"></param>
       /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, int id)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var type = from a in lqh.Umsdb.Package_GroupTypeInfo
                                   where a.ID == id
                                   select a;
                        if(type.Count()==0)
                        {
                            return new Model.WebReturn() { ReturnValue = true,Message="该分组名称已删除！"};
                        }

                        lqh.Umsdb.Package_GroupTypeInfo.DeleteOnSubmit(type.First());
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true,Message="删除成功！"};

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

       /// <summary>
       /// 更新  267
       /// </summary>
       /// <param name="user"></param>
       /// <param name="model"></param>
       /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.View_PackageGroupTypeInfo model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var type = from a in lqh.Umsdb.Package_GroupTypeInfo
                                   where a.ID ==model.ID
                                   select a;
                        if (type.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该分组名称已删除,修改失败！" };
                        }

                        Model.Package_GroupTypeInfo group = type.First();
                        group.ID = model.ID;
                        group.ClassName = model.ClassName;
                        group.GroupName = model.GroupName;
                      
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功！" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }
    }
}
