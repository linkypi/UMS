using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_BackInfo
    {
        public Nullable<long> 序号 { get; set; }
        public int 系统自增编号 { get; set; }
        public string 退库单号 { get; set; }
        public string 原始单号 { get; set; }
        public string 退库仓库 { get; set; }
        public Nullable<System.DateTime> 退库日期 { get; set; }
        public string 操作人 { get; set; }
        public string 备注 { get; set; }
        public string 退库仓库编码 { get; set; }
    }
}
