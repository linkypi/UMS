using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_借贷查询
    {
        public string 标识列 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 型号 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 销售价格 { get; set; }
        public Nullable<decimal> 已还数量 { get; set; }
        public string 未归还借机 { get; set; }
        public string 串号 { get; set; }
        public string 借贷人 { get; set; }
        public string 所属单位 { get; set; }
        public string 所属部门 { get; set; }
        public string 受理人 { get; set; }
        public string 联系方式 { get; set; }
        public Nullable<System.DateTime> 借贷日期 { get; set; }
        public string 借贷周期_天 { get; set; }
        public string 营业厅 { get; set; }
        public int ID { get; set; }
    }
}
