using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_SalaryList_bak
    {
        public int ID { get; set; }
        public Nullable<int> OpID { get; set; }
        public string ProID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> SellType { get; set; }
        public Nullable<decimal> BaseSalary { get; set; }
        public Nullable<decimal> SpecalSalary { get; set; }
        public Nullable<int> ChangeID { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
        public virtual Sys_SalaryChange Sys_SalaryChange { get; set; }
        public virtual Sys_UserOp Sys_UserOp { get; set; }
    }
}
