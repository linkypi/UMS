using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_YanbaoPriceStepInfo_bak
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ProID { get; set; }
        public decimal ProPrice { get; set; }
        public decimal StepPrice { get; set; }
        public string Note { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ProCost { get; set; }
        public int ChangeID { get; set; }
        public virtual Pro_PriceChange Pro_PriceChange { get; set; }
    }
}
