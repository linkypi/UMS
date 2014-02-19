using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Sys_Option : Sys_InitParentInfo
    {
         private int MethodID;

        public Sys_Option()
        {
            this.MethodID = 0;
        }

        public Sys_Option(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
     
        public List<Model.Sys_Option> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_Option>()
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Sys_Option>();
                }
            }
        }

        public Model.WebReturn UpGrade(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_Option>()
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}

                    return new Model.WebReturn() { ReturnValue = true,Obj= query.ToList()};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user,List<Model.Sys_Option> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 权限验证

                        //用户权限
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

                        foreach (var item in models)
                        {
                            var query = from o in lqh.Umsdb.Sys_Option
                                        where o.ID == item.ID
                                        select o;
                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "保存失败，数据库中未找到需要更新的数据" };
                            }

                            query.First().Name = item.Name;
                            query.First().Note = item.Note;
                            query.First().Value = item.Value;
                            query.First().Value2 = item.Value2;
                        }
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
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
