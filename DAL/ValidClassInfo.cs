using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Model;
namespace DAL
{
    public class ValidClassInfo
    { 
       private int MenthodID;

	    public ValidClassInfo()
	    {
		    this.MenthodID = 0;
	    }

        public ValidClassInfo(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }

        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Isnumeric(string str)
        {
            if (str == "")
                return false;
            else
            {
                char[] ch = new char[str.Length];
                ch = str.ToCharArray();
                for (int i = 0; i < ch.Length; i++)
                {
                    if (ch[i] < 48 || ch[i] > 57)
                        return false;
                }
                return true;
            }
        } 
        /// <summary>
        /// 验证日期格式
        /// </summary>
        /// <returns></returns>
        public static bool IsDateTime(string times,string strExp) 
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(strExp);
            return reg.IsMatch(times);
            
        }
   
        /// <summary>
        /// 验证完整日期格式
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public static bool IsDateTime(string times)
        {
            DateTime temp;
            return DateTime.TryParse(times, out temp);
            
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^((?!0000)[0-9]{4}-((0?[1-9]|1[0-2])-(0?[1-9]|1[0-9]|2[0-8])|(0?[13-9]|1[0-2])-(29|30)|(0?[13578]|1[02])-31)|([0-9]{2}(0?[48]|[2468][048]|[13579][26])|(0?[48]|[2468][048]|[13579][26])00)-0?2-29)$");
            return reg.IsMatch(times);
        }
        /// <summary>
        /// 获取指定菜单中 有权限的仓库 和 商品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="MethodID"></param>
        /// <param name="HallIDs"></param>
        /// <param name="ProIDs"></param>
        /// <returns></returns>
        public static Model.WebReturn GetHall_ProIDFromRole(Model.Sys_UserInfo user,int MethodID ,List<string> HallIDs, List<string> ProIDs)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                 
                try
                {

                

                    var MenuList =  from b in lqh.Umsdb.Sys_MethodInfo
                                   where b.MethodID == MethodID
                                   select b.Sys_MenuInfo;
                        
                        //from b in user.Sys_RoleInfo.Sys_Role_MenuInfo
                        //           where b.Sys_MenuInfo.Sys_MethodInfo.Where(p => p.MethodID == MethodID).Count() > 0
                        //           select b.Sys_MenuInfo;
                    if (MenuList.Count() == 0)
                        return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };

                    Model.Sys_MenuInfo menu = MenuList.First();

                    var Menu_role = from b in lqh.Umsdb.Sys_Role_MenuInfo
                                    where b.MenuID == menu.MenuID && b.RoleID==user.RoleID
                                    select b;
                    if (Menu_role.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };
                    }

                    if (menu.HasHallRole != true && menu.HasProRole != true)
                        return new Model.WebReturn() { ReturnValue = true };
                    if (menu.HasHallRole == true)
                    {
                        if(user.RoleID==1)
                            HallIDs.AddRange((from b in lqh.Umsdb.Pro_HallInfo
                                       where b.Flag==true 
                                       select b.HallID));
                        else
                            HallIDs.AddRange((from b in lqh.Umsdb.Sys_Role_Menu_HallInfo
                                   //where b.MenuID == menu.MenuID && b.Pro_HallInfo.Flag==true && b.RoleID==user.RoleID
                                              where  b.Pro_HallInfo.Flag == true && b.RoleID == user.RoleID
                                   select b.HallID));
                        if (HallIDs.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "门店无权操作" };
                    }
                    if (menu.HasProRole==true)
                    {
                        if(user.RoleID==1)
                            ProIDs.AddRange( (from b in lqh.Umsdb.Pro_ClassInfo
                                      
                                      select b.ClassID+""));
                        else
                            ProIDs.AddRange( (from b in lqh.Umsdb.Sys_Role_Menu_ProInfo
                                              where b.MenuID == menu.MenuID && b.RoleID == user.RoleID
                                  select b.ClassID+""));

                        if (ProIDs.Count() == 0) return new Model.WebReturn() {   ReturnValue=false , Message="商品无权操作"};
                    }
                    return new Model.WebReturn() { ReturnValue = true };

                }
                catch (Exception ex) {
                    return new Model.WebReturn() { ReturnValue = false , Message="服务器出错"+ex.Message };
                }
            }

        }
        /// <summary>
        /// 获取指定菜单中 有权限的仓库 和 商品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="MethodID"></param>
        /// <param name="HallIDs"></param>
        /// <param name="ProIDs"></param>
        /// <returns></returns>
        public static Model.WebReturn GetHall_ProIDFromRole(Model.Sys_UserInfo user, int MethodID, List<string> HallIDs, List<string> ProIDs, LinQSqlHelper lqh)
        {
            //using (LinQSqlHelper lqh = new LinQSqlHelper())
            //{

                try
                {



                    var MenuList = from b in lqh.Umsdb.Sys_MethodInfo
                                   where b.MethodID == MethodID
                                   select b.Sys_MenuInfo;

                    //from b in user.Sys_RoleInfo.Sys_Role_MenuInfo
                    //           where b.Sys_MenuInfo.Sys_MethodInfo.Where(p => p.MethodID == MethodID).Count() > 0
                    //           select b.Sys_MenuInfo;
                    if (MenuList.Count() == 0)
                        return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };

                    Model.Sys_MenuInfo menu = MenuList.First();

                    var Menu_role = from b in lqh.Umsdb.Sys_Role_MenuInfo
                                    where b.MenuID == menu.MenuID && b.RoleID == user.RoleID
                                    select b;
                    if (Menu_role.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };
                    }

                    if (menu.HasHallRole != true && menu.HasProRole != true)
                        return new Model.WebReturn() { ReturnValue = true };
                    if (menu.HasHallRole == true)
                    {
                        if (user.RoleID == 1)
                            HallIDs.AddRange((from b in lqh.Umsdb.Pro_HallInfo
                                              where b.Flag == true
                                              select b.HallID));
                        else
                            HallIDs.AddRange((from b in lqh.Umsdb.Sys_Role_Menu_HallInfo
                                              //where b.MenuID == menu.MenuID && b.Pro_HallInfo.Flag == true && b.RoleID == user.RoleID
                                              where  b.Pro_HallInfo.Flag == true && b.RoleID == user.RoleID
                                              select b.HallID));
                        if (HallIDs.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "门店无权操作" };
                    }
                    if (menu.HasProRole == true)
                    {
                        if (user.RoleID == 1)
                            ProIDs.AddRange((from b in lqh.Umsdb.Pro_ClassInfo

                                             select b.ClassID+""));
                        else
                            ProIDs.AddRange((from b in lqh.Umsdb.Sys_Role_Menu_ProInfo
                                             where b.MenuID == menu.MenuID && b.RoleID == user.RoleID
                                             select b.ClassID+""));
                        if (ProIDs.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                    }
                    return new Model.WebReturn() { ReturnValue = true };

                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "服务器出错" + ex.Message };
                }
            //}

        }


        public static Model.WebReturn GetHall_ProIDFromRole(Model.Sys_UserInfo user, int MethodID, out IQueryable<Model.Pro_HallInfo> HallIDs, out IQueryable<Model.Pro_ClassInfo> ProIDs, LinQSqlHelper lqh)
        {
            //using (LinQSqlHelper lqh = new LinQSqlHelper())
            //{
            HallIDs = null;
            ProIDs = null;
            try
            {



                var MenuList = from b in lqh.Umsdb.Sys_MethodInfo
                               where b.MethodID == MethodID
                               select b.Sys_MenuInfo;

                //from b in user.Sys_RoleInfo.Sys_Role_MenuInfo
                //           where b.Sys_MenuInfo.Sys_MethodInfo.Where(p => p.MethodID == MethodID).Count() > 0
                //           select b.Sys_MenuInfo;
                if (MenuList.Count() == 0)
                    return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };

                Model.Sys_MenuInfo menu = MenuList.First();

                var Menu_role = from b in lqh.Umsdb.Sys_Role_MenuInfo
                                where b.MenuID == menu.MenuID && b.RoleID == user.RoleID
                                select b;
                if (Menu_role.Count() == 0)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };
                }

                if (menu.HasHallRole != true && menu.HasProRole != true)
                    return new Model.WebReturn() { ReturnValue = true };
                if (menu.HasHallRole == true)
                {
                    if (user.RoleID == 1)
                        HallIDs=((from b in lqh.Umsdb.Pro_HallInfo
                                          where b.Flag == true
                                          select b));
                    else
                        HallIDs = ((from b in lqh.Umsdb.Pro_HallInfo
                                          //where (from c in b.Sys_Role_Menu_HallInfo where c.RoleID==user.RoleID && c.MenuID==menu.MenuID select c).Count()>0
                                    where (from c in b.Sys_Role_Menu_HallInfo where c.RoleID == user.RoleID   select c).Count() > 0
                                          && b.Flag == true 
                                          select b));
                    if (HallIDs.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "门店无权操作" };
                }
                if (menu.HasProRole == true)
                {
                    if (user.RoleID == 1)
                        ProIDs=((from b in lqh.Umsdb.Pro_ClassInfo

                                         select b));
                    else
                        ProIDs = ((from b in lqh.Umsdb.Pro_ClassInfo
                                         where (from c in b.Sys_Role_Menu_ProInfo where b.ClassID == c.ClassID && c.RoleID == user.RoleID && c.MenuID==menu.MenuID select c).Count()>0
                                         select b));
                    if (ProIDs.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                }
                return new Model.WebReturn() { ReturnValue = true };

            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务器出错" + ex.Message };
            }
            //}

        }

        public static Model.WebReturn GetHall_ProIDFromRole(Model.Sys_UserInfo user, int MenuID, out IQueryable<Model.Pro_HallInfo> HallIDs, LinQSqlHelper lqh)
        {
            //using (LinQSqlHelper lqh = new LinQSqlHelper())
            //{
            HallIDs = null;
            
            try
            {



                var MenuList = from b in lqh.Umsdb.Sys_MenuInfo
                               where b.MenuID == MenuID
                               select b;

                //from b in user.Sys_RoleInfo.Sys_Role_MenuInfo
                //           where b.Sys_MenuInfo.Sys_MethodInfo.Where(p => p.MethodID == MethodID).Count() > 0
                //           select b.Sys_MenuInfo;
                if (MenuList.Count() == 0)
                    return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };

                Model.Sys_MenuInfo menu = MenuList.First();

                var Menu_role = from b in lqh.Umsdb.Sys_Role_MenuInfo
                                where b.MenuID == menu.MenuID && b.RoleID == user.RoleID
                                select b;
                if (Menu_role.Count() == 0)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };
                }

               
                if (menu.HasHallRole == true)
                {
                    if (user.RoleID == 1)
                        HallIDs = ((from b in lqh.Umsdb.Pro_HallInfo
                                    where b.Flag == true
                                    select b));
                    else
                        HallIDs = ((from b in lqh.Umsdb.Pro_HallInfo
                                    //where (from c in b.Sys_Role_Menu_HallInfo where c.RoleID==user.RoleID && c.MenuID==menu.MenuID select c).Count()>0
                                    where (from c in b.Sys_Role_Menu_HallInfo where c.RoleID == user.RoleID select c).Count() > 0
                                          && b.Flag == true
                                    select b));
                    if (HallIDs.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "门店无权操作" };
                }
                
                return new Model.WebReturn() { ReturnValue = true };

            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务器出错" + ex.Message };
            }
            //}

        }
       
        /// <summary>
        /// 获取指定菜单中 有权限的仓库 和 商品
        /// </summary>
        /// <param name="user"></param>
        /// <param name="MethodID"></param>
        /// <param name="HallIDs"></param>
        /// <param name="ProIDs"></param>
        /// <returns></returns>
        public static Model.WebReturn GetHall_ProIDFromRole(Model.Sys_UserInfo user, string ReportViewName, out IQueryable<ReportModel.Pro_HallInfo> HallIDs, out IQueryable<ReportModel.Pro_ClassInfo> ProIDs, ReportModel.Entities ObejectContext)
        {
            HallIDs = null;
            ProIDs = null;
            //using (LinQSqlHelper lqh = new LinQSqlHelper())
            //{

                try
                {

                    var MenuList = from b in ObejectContext.Demo_ReportViewInfo
                                   where b.ReportViewName == ReportViewName
                                   select b.Sys_MenuInfo;
                    //from b in user.Sys_RoleInfo.Sys_Role_MenuInfo
                    //           where b.Sys_MenuInfo.Sys_MethodInfo.Where(p => p.MethodID == MethodID).Count() > 0
                    //           select b.Sys_MenuInfo;
                    if (MenuList.Count() == 0)
                        return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };

                    ReportModel.Sys_MenuInfo menu = MenuList.First();

                    var Menu_role = from b in ObejectContext.Sys_Role_MenuInfo
                                    where b.MenuID == menu.MenuID && b.RoleID == user.RoleID
                                    select b;

                    if (Menu_role.Count() == 0 && user.RoleID !=1)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "菜单不存在或无权操作" };
                    }

                    if (menu.HasHallRole != true && menu.HasProRole != true)
                        return new Model.WebReturn() { ReturnValue = true };

                    if (menu.HasHallRole == true)
                    {
                        if (user.RoleID == 1)
                            HallIDs = ((from b in ObejectContext.Pro_HallInfo
                                        where b.Flag == true
                                        select b));
                        else
                            HallIDs = ((from b in ObejectContext.Pro_HallInfo
                                        where
                                        //(from c in b.Sys_Role_Menu_HallInfo where c.HallID==b.HallID && c.MenuID == menu.MenuID && c.Pro_HallInfo.Flag == true && c.RoleID == user.RoleID select c).Count() > 0
                                        (from c in b.Sys_Role_Menu_HallInfo where c.HallID == b.HallID && c.Pro_HallInfo.Flag == true && c.RoleID == user.RoleID select c).Count() > 0
                                        select b));
                        if (HallIDs.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "门店无权操作" };
                    }
                    if (menu.HasProRole == true)
                    {
                        if (user.RoleID == 1)
                            ProIDs = ((from b in ObejectContext.Pro_ClassInfo

                                             select b));
                        else
                            ProIDs = ((from b in ObejectContext.Pro_ClassInfo
                                             where (from c in b.Sys_Role_Menu_ProInfo where c.ClassID==b.ClassID && c.MenuID == menu.MenuID && c.RoleID == user.RoleID select c).Count()>0
                                             select b));
                        if (ProIDs.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                    }
                    return new Model.WebReturn() { ReturnValue = true };

                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "服务器出错" + ex.Message };
                }
           // }

        }
       
        /// <summary>
        /// 验证用户是否可以登录，是否已禁用
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Model.WebReturn ValidateUser(Model.Sys_UserInfo user, LinQSqlHelper lqh)
        {
            var cuser = from a in lqh.Umsdb.Sys_UserInfo
                        where a.UserID == user.UserID
                        select a;
            if (cuser.Count() == 0)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "当前用户不存在，操作失败！" };
            }
            user = cuser.First();
           // Model.Sys_UserInfo current = cuser.First();
            if (!Convert.ToBoolean(user.CanLogin))
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "您当前状态不可登录，审批失败！" };
            }
            if (!Convert.ToBoolean(user.Flag))
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "当前帐号已禁用，审批失败！" };
            }
            return new Model.WebReturn() { ReturnValue = true};
        }
    }
}