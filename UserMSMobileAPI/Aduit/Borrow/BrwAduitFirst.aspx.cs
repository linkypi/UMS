using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserMSMobileAPI.Aduit.Borrow
{
    public partial class BrwAduitFirst :  JsonAPI
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                Model.Sys_UserInfo User = new Model.Sys_UserInfo();
                User.UserName = "1";
                User.UserPwd = "1";

                DAL.Sys_LoginInfo login = new DAL.Sys_LoginInfo();
                var webR = login.Add(User);
                System.Web.HttpContext.Current.Session["User"] = webR.Obj;
            }

            ReportModel.Entities ent = new ReportModel.Entities();
            int pageIndex = Convert.ToInt32(Request.QueryString["pageIndex"])-1;
            int pageSize =  Convert.ToInt32(Request.QueryString["pageSize"]);
             

            if (webreturn.JsonReturnDict["error"]+"" != "0")
            {
                webreturn.JsonReturnDict["JsonArray"] = new List<Model.View_BorowAduit>();
                webreturn.JsonReturnDict["RecordCount"] = 0;
                OutputJson();
            }
            else
            {
                DAL.BorowAduit ba = new DAL.BorowAduit(67);
                Model.WebReturn r = ba.Search((Model.Sys_UserInfo)Session["User"], new Model.ReportPagingParam()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize, 
                    ParamList = new List<Model.ReportSqlParams>() { 
                     new Model.ReportSqlParams_Bool(){ ParamName="Aduited1" , ParamValues=false}
                    } });
                if (r.ReturnValue == false)
                {
                    webreturn.JsonReturnDict["JsonArray"] = new List<Model.View_BorowAduit>();
                    webreturn.JsonReturnDict["RecordCount"] = 0;
                    OutputJson();

                }
                else
                {
                    webreturn.JsonReturnDict["JsonArray"] = ((Model.ReportPagingParam)r.Obj).Obj;
                    webreturn.JsonReturnDict["RecordCount"] = ((Model.ReportPagingParam)r.Obj).RecordCount;
                    OutputJson();
                }
            }
            

            
        }
    }
}