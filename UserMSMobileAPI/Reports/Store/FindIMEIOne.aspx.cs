using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserMSMobileAPI.Reports.Store
{
    public partial class FindIMEIOne : JsonAPI
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportModel.Entities ent = new ReportModel.Entities();
            string IMEI = (Request.QueryString["IMEI"] + "").ToLower();
            var xx = from b in ent.Report_IMEIInfo
                     
                     where b.串码.ToLower() == IMEI
                     select b;
            JsonFx.Json.JsonWriter jw = new JsonFx.Json.JsonWriter();

            webreturn.JsonReturnDict["xx"] = xx;
            OutputJson();
        }
    }
}