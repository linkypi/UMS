using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_OutInfo
    {
        public long 序号 { get; set; }
        public int 系统自增编号 { get; set; }
        public string 调拨单号 { get; set; }
        public string 原始单号 { get; set; }
        public string 调出仓库 { get; set; }
        public string 调入仓库 { get; set; }
        public Nullable<System.DateTime> 调拨日期 { get; set; }
        public string 调拨人 { get; set; }
        public string 备注 { get; set; }
        public string 已接收 { get; set; }
        public string 接收人 { get; set; }
        public Nullable<System.DateTime> 接收日期 { get; set; }
        public string 调出仓库编码 { get; set; }
        public string 调入仓库编码 { get; set; }
        public Nullable<int> 总高度 { get; set; }
    }
}
