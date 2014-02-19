using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_退货查询
    {
        public string 原类别 { get; set; }
        public string 原品牌 { get; set; }
        public string 原型号 { get; set; }
        public Nullable<decimal> 退换数 { get; set; }
        public string 原出售价 { get; set; }
        public string 原差价 { get; set; }
        public string 出售厅 { get; set; }
        public string 客户名 { get; set; }
        public string 售货员 { get; set; }
        public string 新类别 { get; set; }
        public string 新品牌 { get; set; }
        public string 新型号 { get; set; }
        public string 新价格 { get; set; }
        public string 新差价 { get; set; }
        public string 换取数量 { get; set; }
        public string 实收金额 { get; set; }
        public string 退货原因 { get; set; }
        public Nullable<System.DateTime> 退换日期 { get; set; }
        public string 录入人 { get; set; }
        public string 退换厅 { get; set; }
        public int ID { get; set; }
    }
}
