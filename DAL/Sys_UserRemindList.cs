using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Sys_UserRemindList
    {
        private int MethodID;

        public Sys_UserRemindList()
        {
            this.MethodID = 0;
        }

        public Sys_UserRemindList(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 获取用户提醒    259
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetUserRemindList(Model.Sys_UserInfo user,string starttime,string endtime)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region 权限验证

                    //验证用户
                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                    if (!result.ReturnValue)
                    { return result; }

                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();
                    //验证菜单  
                    Model.WebReturn webret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                    if (webret.ReturnValue != true)
                    { return webret; }

                    #endregion 

                    List<Model.GetUserRemindListResult> models = lqh.Umsdb.GetUserRemindList(user.UserID,starttime).ToList();

                    return new Model.WebReturn() {ReturnValue= true,Obj = models };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() {ReturnValue =false,Message=ex.Message };
                }
            }
        }


        /// <summary>
        /// 获取用户所有提醒    272
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetUserAllRemindList(Model.Sys_UserInfo user, string starttime, string endtime)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //验证用户
                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                    if (!result.ReturnValue)
                    { return result; }

             

                    List<Model.GetUserAllRemindListResult> models = lqh.Umsdb.GetUserAllRemindList(user.UserID, starttime).ToList();

                    return new Model.WebReturn() { ReturnValue = true, Obj = models };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }

        /// <summary>
        /// 获取用户指定提醒 262
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Model.WebReturn GetRemindListByUser(Model.Sys_UserInfo user, string userid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var userremind = from a in lqh.Umsdb.Sys_RemindList
                                     join b in lqh.Umsdb.Sys_UserRemindList
                                     on a.ID equals b.Remind
                                     where b.UserID == userid && a.Flag==true 
                                     select a;
                    List<int> list = new List<int>();
                    foreach (var item in userremind)
                    {
                        list.Add((int)item.ID);  //用户定制的提醒列表
                    }

                    var remind = from a in lqh.Umsdb.Sys_RemindList
                                 where a.IsInTime == true
                                     select a;
                    //List<Model.TreeModel> models = new List<Model.TreeModel>();
                    //if (userremind.Count() == 0)
                    //{
                    //    return new Model.WebReturn() { ReturnValue = true, Obj = models };
                    //}
                    //foreach (var item in userremind)
                    //{
                    //    Model.TreeModel t = new Model.TreeModel();
                    //    t.ID = item.MenuID.ToString();
                    //    t.Name = item.Name;
                    //    models.Add(t);
                    //}
                    return new Model.WebReturn() { ReturnValue = true, Obj = list, ArrList = new System.Collections.ArrayList() { remind.ToList() } };

                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false,Message = ex.Message };
                }
            }

        }

        /// <summary>
        /// 获取所有提醒信息  261 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userid">userid="" 则获取所有提醒 否则获取指定用户提醒</param>
        /// <returns></returns>
        public Model.WebReturn GetAllRemind(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var userremind = from a in lqh.Umsdb.Sys_RemindList
                                     where a.Flag == true
                                     select a;
                   
                    //List<  Model.TreeModel> models = new List<Model.TreeModel>();
                    //List<Model.Sys_RemindList> arr = userremind.ToList();

                    //List<string> ids = new List<string>();
                    //foreach(var item in arr)
                    //{
                    //    if (ids.Contains(item.MenuID.ToString()))
                    //    {
                    //        continue;
                    //    }
                    //    List<Model.TreeModel> temps = new List<Model.TreeModel>();
                    //    Model.TreeModel t = new Model.TreeModel();
                    //    t.ID = item.MenuID.ToString();
                    //    t.Name = item.Name;
                    //    temps.Add(t);
                    //    ids.Add(item.MenuID.ToString());
                    //    //找到同一 父节点的所有子集
                    //    var others = from b in lqh.Umsdb.Sys_MenuInfo
                    //                 where b.MenuID == item.MenuID
                    //                 select b into c
                    //                 from a in userremind
                    //                 where a.Sys_MenuInfo.Parent == c.Parent
                    //                 select a;
                    //    foreach (var itemx in others)
                    //    {
                    //        if (item == itemx)
                    //        {
                    //            continue;
                    //        }
                    //        Model.TreeModel t1 = new Model.TreeModel();
                    //        t1.ID = itemx.MenuID.ToString();
                    //        t1.Name = itemx.Name;
                    //        temps.Add(t1);
                    //        ids.Add(itemx.MenuID.ToString());
                    //    }
                    //    //获取父级菜单
                    //    var menu = from a in lqh.Umsdb.Sys_MenuInfo
                    //               join b in lqh.Umsdb.Sys_MenuInfo
                    //               on a.Parent equals b.MenuID
                    //               where a.MenuID == item.MenuID
                    //               select b;
                    //    //if (menu.Count() == 0)
                    //    //{
                    //    //    return t;
                    //    //}
                    //    Model.Sys_MenuInfo m = menu.First();
                    //    Model.TreeModel parent = new Model.TreeModel();
                    //    parent.ID = m.MenuID.ToString();
                    //    parent.Name = m.MenuText;
                    //    parent.Children = new List<Model.TreeModel>();
                    //    parent.Children.AddRange(temps);

                    //    //获取所有父级的父级  返回根节点
                    //    Model.TreeModel p = GetParent(lqh,parent,models);
                    //    if (p != null)
                    //    {
                    //        models.Add(p);
                    //    }
                    //}

                    return new Model.WebReturn() { ReturnValue = true, Obj = userremind.ToList() }; //models
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }

   
    }
}
