using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_BorrowAduitInfo
    {
        public Nullable<long> 序号 { get; set; }
        public int 系统主键 { get; set; }
        public string 申请单号 { get; set; }
        public Nullable<decimal> 借贷总额 { get; set; }
        public string 已审批 { get; set; }
        public string 已通过 { get; set; }
        public string 已使用 { get; set; }
        public Nullable<System.DateTime> 使用日期 { get; set; }
        public string 申请人 { get; set; }
        public Nullable<System.DateTime> 申请日期 { get; set; }
        public Nullable<System.DateTime> 系统日期 { get; set; }
        public string 门店 { get; set; }
        public string 内部借机 { get; set; }
        public string 借贷人 { get; set; }
        public string 借贷方式 { get; set; }
        public string 借贷部门 { get; set; }
        public string 借贷人电话 { get; set; }
        public Nullable<System.DateTime> 预计归还日期 { get; set; }
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
        public string 四级已审批 { get; set; }
        public string 四级已通过 { get; set; }
        public string 四级审批人 { get; set; }
        public Nullable<System.DateTime> 四级审批日期 { get; set; }
        public string 四级备注 { get; set; }
        public string 门店编码 { get; set; }
    }
}
