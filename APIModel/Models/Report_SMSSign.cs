using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_SMSSign
    {
        public long 序号 { get; set; }
        public string 销售单号 { get; set; }
        public Nullable<System.DateTime> 系统时间 { get; set; }
        public string 行业 { get; set; }
        public Nullable<System.DateTime> 合同日期 { get; set; }
        public string 单位名称 { get; set; }
        public string 单位地址 { get; set; }
        public string 业务内容 { get; set; }
        public decimal 合同金额 { get; set; }
        public decimal 合同发送数量 { get; set; }
        public decimal 实收金额 { get; set; }
        public decimal 实际发送数量 { get; set; }
        public Nullable<System.DateTime> 合同结清日期 { get; set; }
        public Nullable<System.DateTime> 实际结清日期 { get; set; }
        public Nullable<decimal> 佣金 { get; set; }
        public string 联系人 { get; set; }
        public string 联系电话 { get; set; }
        public string 发票抬头 { get; set; }
        public string 发票代码 { get; set; }
        public Nullable<System.DateTime> 发票日期 { get; set; }
        public Nullable<decimal> 税率 { get; set; }
        public string 备注 { get; set; }
        public Nullable<bool> 已结清 { get; set; }
        public string 原始单号 { get; set; }
        public string 录单员 { get; set; }
        public string 销售员 { get; set; }
        public string 仓库 { get; set; }
    }
}
