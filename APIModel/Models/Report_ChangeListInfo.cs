using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_ChangeListInfo
    {
        public Nullable<long> 序号 { get; set; }
        public string 旧批次号 { get; set; }
        public string 新批次号 { get; set; }
        public int 系统自增外键编号 { get; set; }
        public Nullable<int> 自增外键转类别编号 { get; set; }
        public string 原类别 { get; set; }
        public string 原品牌 { get; set; }
        public string 原商品名称 { get; set; }
        public string 原商品属性 { get; set; }
        public string 现类别 { get; set; }
        public string 现品牌 { get; set; }
        public string 现商品名称 { get; set; }
        public string 现商品属性 { get; set; }
        public decimal 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public string 原始单号 { get; set; }
        public string 转类别仓库 { get; set; }
        public Nullable<System.DateTime> 转类别日期 { get; set; }
        public string 操作人 { get; set; }
        public string 转类别仓库编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
        public string 备注 { get; set; }
    }
}
