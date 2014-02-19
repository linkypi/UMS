using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Role_Hall
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<Model.View_HallInfo> HallInfo { get; set; }
    }
}
