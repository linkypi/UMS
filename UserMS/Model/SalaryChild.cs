using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserMS.Model
{
    public class SalaryChild
    {
        public int ID { get; set; }
        public decimal PriceStep { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal OverRatio { get; set; }
        public decimal OverNum { get; set; }
        public string Note { get; set; }
        public bool NewStep { get; set; }
        public decimal Step { get; set; }
        public int StepID { get; set; }
    }
}
