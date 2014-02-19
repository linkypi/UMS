using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_SalaryCurrentList
    {
        public int ID { get; set; }
        public Nullable<int> OpID { get; set; }
        public string ProID { get; set; }
        public Nullable<decimal> BaseSalary { get; set; }
        public Nullable<int> SellType { get; set; }
        public Nullable<decimal> SpecialSalary { get; set; }
        public Nullable<int> SalaryYear { get; set; }
        public Nullable<int> SalaryMonth { get; set; }
        public Nullable<int> SalaryDay { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
        public virtual Sys_UserOp Sys_UserOp { get; set; }
    }
}
