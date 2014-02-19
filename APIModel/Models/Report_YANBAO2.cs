using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_YANBAO2
    {
        public long 序号 { get; set; }
        public Nullable<System.DateTime> 录入时间 { get; set; }
        public string 大区 { get; set; }
        public string 区域 { get; set; }
        public string 店名 { get; set; }
        public string 网店属性 { get; set; }
        public string 销售员 { get; set; }
        public string 仓管员 { get; set; }
        public string 客户姓名 { get; set; }
        public string 联系方式 { get; set; }
        public Nullable<System.DateTime> 延保购买日期 { get; set; }
        public string 合同编号 { get; set; }
        public Nullable<decimal> 销售数量 { get; set; }
        public Nullable<decimal> 延保价格 { get; set; }
        public string 手机型号 { get; set; }
        public Nullable<decimal> 手机价格 { get; set; }
        public string 手机串号 { get; set; }
        public string 手机购买方式 { get; set; }
        public string 发票号码 { get; set; }
        public string 电池编码 { get; set; }
        public string 充电器编码 { get; set; }
        public string 耳机编码 { get; set; }
        public string 备注 { get; set; }
        public string 状态 { get; set; }
        public string 门店编码 { get; set; }
    }
}
