using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_CostPriceListInfo
    {
        public long 序号 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public string 批次号 { get; set; }
        public Nullable<System.DateTime> 入库日期 { get; set; }
        public decimal 成本价 { get; set; }
    }
}
