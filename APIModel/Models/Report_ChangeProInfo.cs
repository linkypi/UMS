using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_ChangeProInfo
    {
        public Nullable<long> 序号 { get; set; }
        public int 系统自增编号 { get; set; }
        public string 原始单号 { get; set; }
        public string 转类别仓库 { get; set; }
        public Nullable<System.DateTime> 转类别日期 { get; set; }
        public string 操作人 { get; set; }
        public string 备注 { get; set; }
        public string 转类别仓库编码 { get; set; }
    }
}
