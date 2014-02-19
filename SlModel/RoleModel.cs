using System.Collections.Generic;

namespace SlModel
{
    public class RoleModel
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
        private List<SlModel.ProModel> proModel;

        public List<SlModel.ProModel> ProModel
        {
            get { return proModel; }
            set { proModel = value; }
        }
    }
}
