using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Sys_RemindList
    {
        private int MethodID;

        public Sys_RemindList()
        {
            this.MethodID = 0;
        }

        public Sys_RemindList(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        /// <summary>
        /// 获取所有商品型号  254  
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var list = from a in lqh.Umsdb.View_RemindList
                               select a;

                    #region 判断是否超过总页数

                    int pagecount = list.Count() / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in lqh.Umsdb.View_RemindList.Take(pageParam.PageSize).ToList()
                                      select a;

                        pageParam.Obj = results.ToList();
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in lqh.Umsdb.View_RemindList.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;


                        pageParam.Obj = results.ToList();
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                 
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }

        /// <summary>
        /// 更新  257
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pac"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.View_RemindList pac)
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

                        var package = from a in lqh.Umsdb.Sys_RemindList
                                      where a.ID == pac.ID
                                      select a;
                        if (package.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "没有找到当前提醒数据，保存失败！" };
                        }

                        //判断是否已经存在相同型号的商品
                        var pacx = from a in lqh.Umsdb.Sys_RemindList
                                   where a.ID != pac.ID && a.Name == pac.Name
                                   select a;
                        if (pacx.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已存在相同提醒标题，保存失败！" };
                        }
                        Model.Sys_RemindList model = package.First();
                        model.Note = pac.Note;
                        model.Name = pac.Name;
                        model.ProcName = pac.ProcName;
                        model.Order = pac.Order;
                        model.Note = pac.Note;
                        if (pac.IsInTime == "是")
                        {
                            model.IsInTime = true;
                        }
                        else
                        {
                            model.IsInTime = false;
                        }
                        if (pac.Flag == "是")
                        {
                            model.Flag = true;
                        }
                        else
                        {
                            model.Flag = false;
                        }
                        lqh.Umsdb.SubmitChanges();

                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功！"};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 添加  255
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Sys_RemindList model)
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

                        //判断是否已经存在同名套餐分类
                        var pacx = from a in lqh.Umsdb.Sys_RemindList
                                   where a.Name == model.Name 
                                   select a;
                        if (pacx.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已存在相同提醒标题，保存失败！" };
                        }

                        lqh.Umsdb.Sys_RemindList.InsertOnSubmit(model);

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "添加成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        //256
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.View_RemindList model)
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

                        var package = from a in lqh.Umsdb.Sys_RemindList
                                      where a.ID == model.ID 
                                      select a;
                        if (package.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定提醒数据，删除失败！" };
                        }

                        Model.Sys_RemindList mm = package.First();
                        lqh.Umsdb.Sys_RemindList.DeleteOnSubmit(mm);
                        lqh.Umsdb.SubmitChanges();

                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！" };
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
