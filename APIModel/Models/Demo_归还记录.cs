using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_归还记录
    {
        public string 借贷人 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 型号 { get; set; }
        public Nullable<decimal> 归还数量 { get; set; }
        public Nullable<decimal> 售出数量 { get; set; }
        public Nullable<decimal> 未售出数量_好_ { get; set; }
        public Nullable<decimal> 未售出数量_坏_ { get; set; }
        public string 串号 { get; set; }
        public Nullable<System.DateTime> 借贷日期 { get; set; }
        public Nullable<System.DateTime> 归还日期 { get; set; }
        public string 受理人 { get; set; }
        public string 营业厅 { get; set; }
        public int ID { get; set; }
    }
}
