using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_InOutSellInfo
    {
        public long 序号 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public Nullable<decimal> 期初库存 { get; set; }
        public Nullable<decimal> 本期初始入库 { get; set; }
        public Nullable<decimal> 本期退库 { get; set; }
        public Nullable<decimal> 本期类别转入 { get; set; }
        public Nullable<decimal> 本期类别转出 { get; set; }
        public Nullable<decimal> 本期调入 { get; set; }
        public Nullable<decimal> 本期调出 { get; set; }
        public Nullable<decimal> 本期销售审批_申请_ { get; set; }
        public Nullable<decimal> 本期销售审批_已审批_ { get; set; }
        public Nullable<decimal> 本期销售 { get; set; }
        public Nullable<decimal> 本期退货 { get; set; }
        public Nullable<decimal> 本期送修 { get; set; }
        public Nullable<decimal> 本期返库 { get; set; }
        public Nullable<decimal> 本期借贷 { get; set; }
        public Nullable<decimal> 本期归还 { get; set; }
        public Nullable<decimal> 期末库存 { get; set; }
        public Nullable<decimal> 送修累计 { get; set; }
        public Nullable<decimal> 借贷累计 { get; set; }
        public string 门店 { get; set; }
        public string 区域 { get; set; }
        public string 门店编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
    }
}
