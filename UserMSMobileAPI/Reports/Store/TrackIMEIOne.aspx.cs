using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Data.Services;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace UserMSMobileAPI.Reports.Store
{
    public partial class TrackIMEIOne : JsonAPI
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportModel.Entities ent = new ReportModel.Entities();
            string IMEI=(Request.QueryString["IMEI"] + "").ToLower();
            var xx = from b in ent.Report_IMEITracksInfo
                     orderby b.日期
                     where b.串码.ToLower() == IMEI
                     select b;
            JsonFx.Json.JsonWriter jw=new JsonFx.Json.JsonWriter();
             
           webreturn.JsonReturnDict["xx"] = xx;
           OutputJson();
           
        }
    }
}