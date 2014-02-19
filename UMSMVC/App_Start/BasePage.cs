using APIModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace UMSMVC
{
    public class BasePage : JsonAPI
    {
        public Model.UMSDB context  ;
        public Model.Sys_UserInfo user;

        public HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;
            }

        }

        public BasePage()
        {
            user = (Model.Sys_UserInfo)System.Web.HttpContext.Current.Session["User"];
            context =new Model.UMSDB();
        }
    }
}