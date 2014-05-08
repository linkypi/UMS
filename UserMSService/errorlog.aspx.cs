using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Model;

namespace UserMSService
{
    public partial class errorlog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.HttpMethod == "POST")
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    lqh.Umsdb.Sys_ErrorLog.InsertOnSubmit(new Sys_ErrorLog()
                    {
                        Date = DateTime.Now,
                        UserID = this.Request["UserID"],
                        ErrorMessage = this.Request["Msg"],
                    });
                    lqh.Umsdb.SubmitChanges();
                }
            }
        }
    }
}