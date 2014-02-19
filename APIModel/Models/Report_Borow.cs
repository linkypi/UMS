using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_Borow
    {
        public string 原始单号 { get; set; }
        public string 审批单 { get; set; }
        public string 借贷人 { get; set; }
        public string 借贷日期 { get; set; }
        public string 借贷部门 { get; set; }
        public string 借贷方式 { get; set; }
        public string 商品类别 { get; set; }
        public string 商品品牌 { get; set; }
        public string 商品型号 { get; set; }
        public string 批次号 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public string 营业厅 { get; set; }
        public string 商品属性 { get; set; }
        public string 串码 { get; set; }
        public int ID { get; set; }
        public string 门店编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
        public string 商品编码 { get; set; }
    }
}
