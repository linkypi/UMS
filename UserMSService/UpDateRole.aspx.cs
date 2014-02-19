using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserMSService
{
    public partial class UpDateRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DAL.Sys_RoleInfo role = new DAL.Sys_RoleInfo();
            Model.WebReturn r= role.UpdateRoleXML();
            Response.Write(r.Message);

        }
    }
}