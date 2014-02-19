using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_PriceInfo
    {
        public long 序号 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public string 销售类别 { get; set; }
        public decimal 价格 { get; set; }
        public decimal 结算价 { get; set; }
        public decimal 最低价格 { get; set; }
        public decimal 最高价格 { get; set; }
        public string 可以兑券 { get; set; }
        public string 需要审批单 { get; set; }
    }
}
