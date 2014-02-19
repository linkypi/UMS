using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_StoreInfo
    {
        public long 序号 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public Nullable<decimal> 零售价格 { get; set; }
        public decimal 在库数量 { get; set; }
        public Nullable<decimal> 门店优惠审批中 { get; set; }
        public Nullable<decimal> 调拨中数量 { get; set; }
        public decimal 送修中数量 { get; set; }
        public decimal 借贷中数量 { get; set; }
        public Nullable<decimal> 小计 { get; set; }
        public string 门店 { get; set; }
        public string 区域 { get; set; }
        public string 商品编码 { get; set; }
        public string 门店编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
    }
}
