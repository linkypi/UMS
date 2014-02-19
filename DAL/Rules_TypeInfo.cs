using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Rules_TypeInfo:Sys_InitParentInfo
    {
        private int MethodID;

        public Rules_TypeInfo()
        {
            this.MethodID = 0;
        }

        public Rules_TypeInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        public List<Model.Rules_TypeInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var list = from a in lqh.Umsdb.Rules_TypeInfo
                                select a;
                    return  list.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Rules_TypeInfo>();
                }

            }
        }

        public Model.WebReturn GetList(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var list = from a in lqh.Umsdb.Rules_TypeInfo
                                select a;
                    return new Model.WebReturn() { ReturnValue = true, Obj = list.ToList()};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false,Message=ex.Message};
                }

            }
        }

        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Rules_TypeInfo model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 权限验证

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                        if (!result.ReturnValue)
                        { return result; }

                    
                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion

                        lqh.Umsdb.Rules_TypeInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "添加成功" };
                        
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false,Message=ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Rules_TypeInfo model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 权限验证

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                        if (!result.ReturnValue)
                        { return result; }

                    
                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion


                        var tm = from a in lqh.Umsdb.Rules_TypeInfo
                                 where a.ID == model.ID
                                 select a;
                        if (tm.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false,Message="规则已删除，修改失败！"};
                        }
                        Model.Rules_TypeInfo t = tm.First();
                        t.RulesName = model.RulesName;
                        t.ShowToCus = model.ShowToCus;
                        t.CanGetBack = model.CanGetBack;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功" };
                       
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 删除
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
                        #region 权限验证

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                        if (!result.ReturnValue)
                        { return result; }

                      
                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion

                        var tm = from a in lqh.Umsdb.Rules_TypeInfo
                                 where a.ID == id
                                 select a;
                        if (tm.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "规则已删除，删除失败！" };
                        }
                        lqh.Umsdb.Rules_TypeInfo.DeleteOnSubmit(tm.First());
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功" };
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
