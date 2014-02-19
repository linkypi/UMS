using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;
using System.Data.Linq;

namespace DAL
{
    /// <summary>
    /// 登录类
    /// </summary>
    public class Sys_LoginInfo
    {
        
        private int MenthodID;

	    public Sys_LoginInfo()
	    {
		    this.MenthodID = 0;
	    }

        public Sys_LoginInfo(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <remarks>用户登录</remarks>
        /// <param name="user">只需要用户名和密码</param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user)
        {

            //角色信息
            //方法列表
            //菜单列表        
            //菜单_门店列表   
            //菜单_产品类别列表  
            //用户信息        

            //返回userInfo  
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region dataentiry

                        DataLoadOptions dataload = new DataLoadOptions();
                        //dataload.LoadWith<Model.Sys_UserInfo>(c => c.Sys_RoleInfo);
                        //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_MenuInfo);
                        //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_Menu_ProInfo);;
                        //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_Menu_HallInfo);
                        //dataload.LoadWith<Model.Sys_Role_MenuInfo>(c => c.Sys_MenuInfo);
                        //dataload.LoadWith<Model.Sys_MenuInfo>(c => c.Sys_MethodInfo); 
                        lqh.Umsdb.LoadOptions = dataload;
                        
                        #endregion

                        #region 获取用户信息
                        var getuser = from b in lqh.Umsdb.GetTable<Model.Sys_UserInfo>()
                                      where b.UserName == user.UserName && b.UserPwd == user.UserPwd
                                      && b.CanLogin==true && b.Flag==true
                                      select b;
                        if (getuser.Count() == 0)
                        {
                            return new Model.WebReturn() {  ReturnValue = false, Message = "登陆失败，用户名或密码错误" };
                        }
                        Model.Sys_UserInfo userinfo = getuser.First();
                        if (userinfo.Flag != true || userinfo.CanLogin != true)
                        {
                            return new Model.WebReturn() {  ReturnValue = false, Message = "此账号已禁用，请联系管理员" };
                        }
                         
                         
                        #endregion
                         
                        #region 保存登陆日志
                        //user = userinfo;
                        Model.Sys_LoginInfo login = new Model.Sys_LoginInfo()
                        {
                            UserID=userinfo.UserID,
                            LoginState = 0,
                            LoginIP = user.UserIP,
                            LoginDate = DateTime.Now
                        };
                        
                        #endregion
                        lqh.Umsdb.Sys_LoginInfo.InsertOnSubmit(login);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { Obj = userinfo, ReturnValue = true, Message = "登录成功" };
                    }

                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };
                        
                    }
                }
            }
        }
        /// <summary>
        /// 用户验证 报表专用
        /// </summary>
        /// <remarks>用户登录</remarks>
        /// <param name="user">只需要用户名和密码</param>
        /// <returns></returns>
        public Model.UserInfo CheckUserByReport(Model.Sys_UserInfo user)
        {

            //角色信息
            //方法列表
            //菜单列表        
            //菜单_门店列表   
            //菜单_产品类别列表  
            //用户信息        

            //返回userInfo  
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
               
                    
                        #region 获取用户信息
                        var getuser = from b in lqh.Umsdb.GetTable<Model.Sys_UserInfo>()
                                      where b.UserName == user.UserName && b.UserPwd == user.UserPwd
                                      //&& b.CanLogin==true && b.Flag==true
                                      select b;
                        if (getuser.Count() == 0)
                        {
                           throw new Exception("用户名或密码错误" );
                        }
                        Model.Sys_UserInfo userinfo = getuser.First();
                        if (userinfo.Flag != true || userinfo.CanLogin != true)
                        {
                            throw new Exception("此账号已禁用，请联系管理员");
                            //return new Model.WebReturn() { ReturnValue = false, Message = "此账号已禁用，请联系管理员" };
                        }


                        #endregion

                        #region 保存登陆日志
                        //user = userinfo;
                        Model.Sys_LoginInfo login = new Model.Sys_LoginInfo()
                        {
                            UserID = userinfo.UserID,
                            LoginState = 0,
                            LoginIP = user.UserIP,
                            LoginDate = DateTime.Now
                            
                        };

                        #endregion
                        lqh.Umsdb.Sys_LoginInfo.InsertOnSubmit(login);
                        lqh.Umsdb.SubmitChanges();


                        return new UserInfo() { UserName=userinfo.UserName,
                                                UserPwd = userinfo.UserPwd,
                                                RoleID = userinfo.RoleID,
                                                UserID=userinfo.UserID
                        };
                 
                 
            }
        }
    }
}
