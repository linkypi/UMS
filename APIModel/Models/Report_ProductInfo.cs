using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_ProductInfo
    {
        public long 序号 { get; set; }
        public string 商品编码 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public string 会员类别 { get; set; }
        public string 有串码 { get; set; }
        public string 属于服务 { get; set; }
        public Nullable<System.DateTime> 兑券临界日期 { get; set; }
        public string 日期之前可用 { get; set; }
        public decimal 日期之前加 { get; set; }
        public string 日期之后可用 { get; set; }
        public decimal 日期之后加 { get; set; }
        public decimal 兑券临界值 { get; set; }
        public decimal 小于临界值加 { get; set; }
        public decimal 大于临界值加 { get; set; }
        public string 需要补差 { get; set; }
        public string 备注 { get; set; }
    }
}
