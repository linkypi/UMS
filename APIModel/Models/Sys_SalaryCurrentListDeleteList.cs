using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_SalaryCurrentListDeleteList
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
    }
}
