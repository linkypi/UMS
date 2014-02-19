using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_ReNewInfo
    {
        public long 序号 { get; set; }
        public string 原始单号 { get; set; }
        public string 续期方式 { get; set; }
        public string 续期比列 { get; set; }
        public Nullable<decimal> 续期金额 { get; set; }
        public Nullable<decimal> 续期积分 { get; set; }
        public Nullable<int> 续期天数 { get; set; }
        public string 备注 { get; set; }
        public Nullable<System.DateTime> 续期日期 { get; set; }
        public string 会员姓名 { get; set; }
        public string 会员电话 { get; set; }
        public string 门店 { get; set; }
        public string 拦装人 { get; set; }
        public string 门店编码 { get; set; }
    }
}
