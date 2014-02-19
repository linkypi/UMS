using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_SalaryChange
    {
        public Sys_SalaryChange()
        {
            this.Sys_SalaryList_bak = new List<Sys_SalaryList_bak>();
            this.Sys_SalaryList = new List<Sys_SalaryList>();
        }

        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public string ChangeID { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual ICollection<Sys_SalaryList_bak> Sys_SalaryList_bak { get; set; }
        public virtual ICollection<Sys_SalaryList> Sys_SalaryList { get; set; }
    }
}
