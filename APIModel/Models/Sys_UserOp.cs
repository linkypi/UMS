using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_UserOp
    {
        public Sys_UserOp()
        {
            this.Sys_SalaryCurrentList = new List<Sys_SalaryCurrentList>();
            this.Sys_SalaryList = new List<Sys_SalaryList>();
            this.Sys_SalaryList_bak = new List<Sys_SalaryList_bak>();
            this.Sys_UserOPList = new List<Sys_UserOPList>();
        }

        public int OpID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public virtual ICollection<Sys_SalaryCurrentList> Sys_SalaryCurrentList { get; set; }
        public virtual ICollection<Sys_SalaryList> Sys_SalaryList { get; set; }
        public virtual ICollection<Sys_SalaryList_bak> Sys_SalaryList_bak { get; set; }
        public virtual ICollection<Sys_UserOPList> Sys_UserOPList { get; set; }
    }
}
