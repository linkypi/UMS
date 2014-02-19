using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_RepairReturnInfo
    {
        public long 序号 { get; set; }
        public int 系统自增编号 { get; set; }
        public string 批次号 { get; set; }
        public int 系统自增外键编号 { get; set; }
        public string 送修单号 { get; set; }
        public string 返库单号 { get; set; }
        public string 原始返库单号 { get; set; }
        public string 门店 { get; set; }
        public Nullable<System.DateTime> 返库日期 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public string 串码 { get; set; }
        public string 返修串码 { get; set; }
        public string 操作人 { get; set; }
        public string 备注 { get; set; }
        public string 仓库编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
    }
}
