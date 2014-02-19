using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_OutOrderIMEIInfo
    {
        public long 序号 { get; set; }
        public string 调拨单号 { get; set; }
        public string 原始单号 { get; set; }
        public string 调出仓库 { get; set; }
        public string 调入仓库 { get; set; }
        public Nullable<System.DateTime> 调拨日期 { get; set; }
        public string 调拨人 { get; set; }
        public string 调拨备注 { get; set; }
        public int 系统自增编号 { get; set; }
        public string 批次号 { get; set; }
        public Nullable<int> 自增外键调拨单编号 { get; set; }
        public int 自增外键调拨明细编号 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public string 串码 { get; set; }
        public string 串码备注 { get; set; }
        public string 已接收 { get; set; }
        public string 接收人 { get; set; }
        public Nullable<System.DateTime> 接收日期 { get; set; }
        public string 调出仓库编码 { get; set; }
        public string 调入仓库编码 { get; set; }
        public string 备注 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
    }
}
