using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_SellAduitInfo
    {
        public Nullable<long> 序号 { get; set; }
        public int 系统主键 { get; set; }
        public string 申请单号 { get; set; }
        public Nullable<decimal> 批发总额 { get; set; }
        public string 已审批 { get; set; }
        public string 已通过 { get; set; }
        public string 已使用 { get; set; }
        public Nullable<System.DateTime> 使用日期 { get; set; }
        public string 申请人 { get; set; }
        public Nullable<System.DateTime> 申请日期 { get; set; }
        public Nullable<System.DateTime> 系统日期 { get; set; }
        public string 门店 { get; set; }
        public string 客户姓名 { get; set; }
        public string 客户电话 { get; set; }
        public string 申请备注 { get; set; }
        public string 一级已审批 { get; set; }
        public string 一级已通过 { get; set; }
        public string 一级审批人 { get; set; }
        public Nullable<System.DateTime> 一级审批日期 { get; set; }
        public string 一级备注 { get; set; }
        public string 二级已审批 { get; set; }
        public string 二级已通过 { get; set; }
        public string 二级审批人 { get; set; }
        public Nullable<System.DateTime> 二级审批日期 { get; set; }
        public string 二级备注 { get; set; }
        public string 三级已审批 { get; set; }
        public string 三级已通过 { get; set; }
        public string 三级审批人 { get; set; }
        public Nullable<System.DateTime> 三级审批日期 { get; set; }
        public string 三级备注 { get; set; }
        public string 门店编码 { get; set; }
    }
}
