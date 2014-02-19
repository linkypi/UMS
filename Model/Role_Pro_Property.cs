using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Role_Pro_Property
    {
        private int roleID;

        public int RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }
        private string roleName;

        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }
        List<ProClassModel> proClassModel;

        public List<ProClassModel> ProClassModel
        {
            get { return proClassModel; }
            set { proClassModel = value; }
        }
    }
}
