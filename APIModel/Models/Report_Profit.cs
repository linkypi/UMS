using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_Profit
    {
        public long 序号 { get; set; }
        public string 销售单号 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public string 销售方式 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public string 门店 { get; set; }
        public Nullable<System.DateTime> 销售日期 { get; set; }
        public Nullable<decimal> 兑换值 { get; set; }
        public string 商品大类 { get; set; }
        public string 门店编码 { get; set; }
        public Nullable<int> 片区编码 { get; set; }
        public Nullable<int> 大区编码 { get; set; }
        public string 商品编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
        public Nullable<int> 商品大类编码 { get; set; }
        public string 批次号 { get; set; }
        public Nullable<decimal> 结算价 { get; set; }
        public Nullable<decimal> 入库成本价 { get; set; }
        public Nullable<decimal> 销售时成本价 { get; set; }
        public decimal 计算成本价 { get; set; }
        public Nullable<decimal> 实收单价 { get; set; }
        public Nullable<decimal> 退货审批利润 { get; set; }
        public Nullable<decimal> 利润 { get; set; }
        public string 片区 { get; set; }
        public string 大区 { get; set; }
        public string 销售时间 { get; set; }
        public string 串码 { get; set; }
    }
}
