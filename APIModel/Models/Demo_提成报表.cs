using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_提成报表
    {
        public Nullable<System.DateTime> 日期 { get; set; }
        public string 营销员 { get; set; }
        public string 职位 { get; set; }
        public string 仓库 { get; set; }
        public string 片区 { get; set; }
        public Nullable<decimal> 广信延保销售数量 { get; set; }
        public Nullable<decimal> 广信延保提成 { get; set; }
        public Nullable<decimal> 本人销售数量 { get; set; }
        public Nullable<decimal> 本人销售提成 { get; set; }
        public Nullable<decimal> 本人销售退机 { get; set; }
        public Nullable<decimal> 本人销售退机金额 { get; set; }
        public Nullable<decimal> 非本人销售数量 { get; set; }
        public Nullable<decimal> 非本人销售提成 { get; set; }
        public Nullable<decimal> 非本人销售退机 { get; set; }
        public Nullable<decimal> 非本人销售退机金额 { get; set; }
        public Nullable<decimal> 终端提成总额 { get; set; }
        public int ID { get; set; }
    }
}
