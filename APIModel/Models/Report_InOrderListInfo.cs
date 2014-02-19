using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_InOrderListInfo
    {
        public Nullable<long> 序号 { get; set; }
        public string 批次号 { get; set; }
        public Nullable<int> 系统自增外键编号 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public Nullable<decimal> 成本价 { get; set; }
        public string 入库单号 { get; set; }
        public string 原始单号 { get; set; }
        public string 入库仓库 { get; set; }
        public Nullable<System.DateTime> 入库日期 { get; set; }
        public string 操作人 { get; set; }
        public string 入库仓库编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
        public string 备注 { get; set; }
    }
}
