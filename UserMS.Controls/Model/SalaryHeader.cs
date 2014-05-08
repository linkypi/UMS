using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserMS.Model
{
    public class SalaryHeader
    {
        public string ClassName { get; set; }
        public int ClassID { get; set; }
        public string TypeName { get; set; }

        public int TypeID { get; set; }

        public string ProName { get; set; }
        public string ProID { get; set; }
        public int ProMainID { get; set; }
        public bool IsProMain { get; set; }
        public string ProFormat { get; set; }
        public string SellTypeName { get; set; }
        public int SellType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Model.SalaryChild> Children { get; set; }
    }
}
