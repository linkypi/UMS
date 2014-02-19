using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace UserMSMobileAPI
{
    public partial class Login : JsonAPI
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.RequireArgs.Add("username");
            this.RequireArgs.Add("password");
            if (this.CheckArgs())
            {
                string username = Request.Params["username"];
                string password = Request.Params["password"];

                using (LinQSqlHelper lqh =new LinQSqlHelper())
                {
                    Model.Sys_UserInfo User = new Model.Sys_UserInfo();
                    User.UserName = username;
                    User.UserPwd = password;

                    DAL.Sys_LoginInfo login = new DAL.Sys_LoginInfo();
                    var webR = login.Add(User);
                    if (webR.Obj != null)
                    {
                        this.Session["User"] = webR.Obj;
                        var LoginSysUserInfo = webR.Obj as Model.Sys_UserInfo;
                        webreturn.JsonReturnDict["response"] = new APIModel.Login(LoginSysUserInfo);
                    }
                    else
                    {
                        webreturn.JsonReturnDict["error"] = 1;
                        webreturn.JsonReturnDict["msg"] = "登录失败";
                    }
                }
                this.OutputJson();
                
            }
        }
    }
}