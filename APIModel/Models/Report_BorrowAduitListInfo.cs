using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_BorrowAduitListInfo
    {
        public int 系统主键 { get; set; }
        public Nullable<int> 系统外键 { get; set; }
        public string 商品类别 { get; set; }
        public string 商品品牌 { get; set; }
        public string 商品型号 { get; set; }
        public string 属性 { get; set; }
        public Nullable<decimal> 借贷数量 { get; set; }
        public Nullable<decimal> 借贷单价 { get; set; }
        public string 备注 { get; set; }
    }
}
