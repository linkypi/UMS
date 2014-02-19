using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_LoginInfo
    {
        public int LoginID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public Nullable<int> LoginState { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public string LoginIP { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<int> LoginOutID { get; set; }
        public string LoginOutIP { get; set; }
    }
}
