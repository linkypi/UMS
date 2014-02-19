using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_ReturnIMEIInfo
    {
        public long 序号 { get; set; }
        public int 自主主键编号 { get; set; }
        public int 归还明细外键编号 { get; set; }
        public int 归还外键编号 { get; set; }
        public string 借贷单号 { get; set; }
        public string 审批单号 { get; set; }
        public string 原始单号 { get; set; }
        public string 借贷门店 { get; set; }
        public Nullable<System.DateTime> 借贷日期 { get; set; }
        public string 借贷部门 { get; set; }
        public string 借贷人 { get; set; }
        public string 联系电话 { get; set; }
        public string 借贷方式 { get; set; }
        public Nullable<System.DateTime> 预计归还日期 { get; set; }
        public Nullable<System.DateTime> 归还日期 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public decimal 数量 { get; set; }
        public string 串码 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public string 操作人 { get; set; }
        public string 备注 { get; set; }
        public string 借贷仓库编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
    }
}
