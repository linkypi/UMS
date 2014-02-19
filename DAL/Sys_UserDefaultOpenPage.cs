using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Sys_UserDefaultOpenPage
    {
       private int MethodID;

        public Sys_UserDefaultOpenPage()
        {
            this.MethodID = 0;
        }

        public Sys_UserDefaultOpenPage(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 273 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Model.WebReturn GetUserDefaultPage(Model.Sys_UserInfo user,string userid )
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var list = from a in lqh.Umsdb.Sys_UserDefaultOpenPage
                               where a.UserID == userid
                               select a;
                    List<int> menuids = new List<int>();
                    foreach (var item in list)
                    {
                        menuids.Add((int)item.MenuID);
                    }

                    return new Model.WebReturn() { ReturnValue = true, Obj = menuids };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 274
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userid"></param>
        /// <param name="menuids"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, string userid,List<int> adds,List<int> dels)
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

                        //删除数据
                        var delmenus = from a in lqh.Umsdb.Sys_UserDefaultOpenPage
                                        where dels.Contains((int)a.MenuID)
                                        select a;
                        lqh.Umsdb.Sys_UserDefaultOpenPage.DeleteAllOnSubmit(delmenus);

                        //新增数据
                        var addmenus = from a in lqh.Umsdb.Sys_MenuInfo
                                       where adds.Contains(a.MenuID)
                                       select a;
                        if (addmenus.Count() != adds.Count)
                        {
                            return new Model.WebReturn() { ReturnValue=false,Message="部分菜单已删除，请重新打开页面！"};
                        }

                        //添加前检测是否存在相应菜单
                        var usermenus = from a in lqh.Umsdb.Sys_UserDefaultOpenPage
                                        where adds.Contains((int)a.MenuID)
                                        select a;

                        foreach (var item in usermenus)
                        {
                            if (adds.Contains((int)item.MenuID))
                            {
                                adds.Remove((int)item.MenuID);
                            }
                        }

                        List<Model.Sys_UserDefaultOpenPage> models = new List<Model.Sys_UserDefaultOpenPage>();
                        foreach (var item in adds)
                        {
                            Model.Sys_UserDefaultOpenPage m = new Model.Sys_UserDefaultOpenPage();
                            m.UserID = userid;
                            m.MenuID = item;
                            models.Add(m);
                        }

                        lqh.Umsdb.Sys_UserDefaultOpenPage.InsertAllOnSubmit(models);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() {ReturnValue=true,Message="保存成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                    }
                }
            }
        }
    }
}
