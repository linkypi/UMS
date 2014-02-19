using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_ProNameInfo : Sys_InitParentInfo
    {
        private int MethodID;

        public Pro_ProNameInfo()
        {
            this.MethodID = 0;
        }

        public Pro_ProNameInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        public List<Model.Pro_ProNameInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_ProNameInfo>()
                                select b;
                    if (query == null || query.Count() == 0)
                    {
                        return null;
                    }
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取所有商品型号   250
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn GetAllList(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var list = from a in lqh.Umsdb.View_ProNameInfo
                               select a;

                    #region 判断是否超过总页数

                    int pagecount = list.Count() / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in lqh.Umsdb.View_ProNameInfo.Take(pageParam.PageSize).ToList()
                                      select a;

                        pageParam.Obj = results.ToList();
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in lqh.Umsdb.View_ProNameInfo.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
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
        /// 更新
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pac"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.View_ProNameInfo pac)
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

                        var package = from a in lqh.Umsdb.Pro_ProNameInfo
                                      where a.ID == pac.ID
                                      select a;
                        if (package.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "没有找到当前商品型号，保存失败！" };
                        }

                        //判断是否已经存在相同型号的商品
                        var pacx = from a in lqh.Umsdb.Pro_ProNameInfo
                                   where a.ID != pac.ID && a.MainName == pac.MainName
                                   select a;
                        if (pacx.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已存在相同商品型号，保存失败！" };
                        }

                        #region 同步更新所有总商品的ProMainName
                        //var AllProMain = (from b in lqh.Umsdb.Pro_ProMainInfo
                        //                  where b.ProNameID == pac.ID
                        //                  select b).ToList();

                        lqh.Umsdb.ExecuteCommand("update Pro_ProMainInfo set ProMainName={0} where ProNameID="+pac.ID, 
                            pac.MainName);
                        #endregion

                        #region 同步更新所有商品的ProName 

                        lqh.Umsdb.ExecuteCommand("update Pro_ProInfo set ProName={0} where ProMainID in(select ProMainID from Pro_ProMainInfo where ProNameID=" + pac.ID+")",
                            pac.MainName);
                        #endregion

                        Model.Pro_ProNameInfo model = package.First();
                        model.Note = pac.Note;
                        model.MainName = pac.MainName;
                        lqh.Umsdb.SubmitChanges();

             
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "保存成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 添加  251
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_ProNameInfo model)
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
                        var pacx = from a in lqh.Umsdb.Pro_ProNameInfo
                                   where a.MainName == model.MainName
                                   select a;
                        if (pacx.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已存在相同名称的商品型号，保存失败！" };
                        }

                        string nameid = "";
                        int i= lqh.Umsdb.OrderMacker(1, "PN", "PN", ref nameid);

                        if (i!=0 || string.IsNullOrEmpty(nameid))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "单号生成出错！" };
                        }
                        model.NameID = nameid;
                        lqh.Umsdb.Pro_ProNameInfo.InsertOnSubmit(model);

                        lqh.Umsdb.SubmitChanges();
                       // Model.WebReturn retlist = GetAllList(user);  //添加完成后返回最新列表
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "添加成功！" , Obj=model};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        //252
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.View_ProNameInfo model)
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

                        var package = from a in lqh.Umsdb.Pro_ProNameInfo
                                      where a.ID == model.ID && a.MainName == model.MainName
                                      select a;
                        if (package.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定商品型号，删除失败！" };
                        }

                        //使用中的套餐分类无法删除
                        var usemodel = from a in lqh.Umsdb.Pro_ProMainInfo
                                       where a.ProNameID == model.ID
                                       select a;
                        if (usemodel.Count() != 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该商品型号使用中无法删除，删除失败！" };
                        }

                        //Model.Package_SalesNameInfo pac = new Model.Package_SalesNameInfo();
                        //pac.ID = model.ID;
                        //pac.Note = model.Note;
                        //pac.SalesName = model.SalesName;
                        //pac.Parent = model.Parent;
                        Model.Pro_ProNameInfo mm = package.First();
                        lqh.Umsdb.Pro_ProNameInfo.DeleteOnSubmit(mm);
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
