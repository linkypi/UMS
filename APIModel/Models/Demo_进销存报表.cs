using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_进销存报表
    {
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 型号 { get; set; }
        public string 期初 { get; set; }
        public Nullable<decimal> 本期初始入库 { get; set; }
        public Nullable<decimal> 调入 { get; set; }
        public Nullable<decimal> 本期调出 { get; set; }
        public Nullable<decimal> 本期销售 { get; set; }
        public Nullable<decimal> 本期送修 { get; set; }
        public Nullable<decimal> 本期借贷 { get; set; }
        public Nullable<decimal> 期末库存 { get; set; }
        public Nullable<decimal> 送修累计 { get; set; }
        public Nullable<decimal> 本期返库 { get; set; }
        public Nullable<decimal> 借贷累计 { get; set; }
        public Nullable<decimal> 本期归还 { get; set; }
        public string 营业厅 { get; set; }
        public Nullable<System.DateTime> 开始时间 { get; set; }
        public Nullable<System.DateTime> 结束时间 { get; set; }
        public int ID { get; set; }
    }
}
