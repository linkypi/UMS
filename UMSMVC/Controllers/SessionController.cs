using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace UMSMVC.Controllers
{
    public class SessionController : BasePage
    {
        //
        // GET: /Session/
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpGet]
        public void Login()
        {
            var querystring = Request.RequestUri.ParseQueryString();
            var username = querystring["username"];
            var password = querystring["password"];
           
               

           
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
          
            this.OutputJson();
          
          
        }

    }
}
