using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Print_SellListInfo
    {
        public long 自增主键编号 { get; set; }
        public Nullable<int> 系统自增外键编号 { get; set; }
        public string 商品名称 { get; set; }
        public string 串码_号码_合同号 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public Nullable<decimal> 券面值 { get; set; }
        public string 券号_合约号 { get; set; }
        public Nullable<decimal> 优惠 { get; set; }
        public string 备注 { get; set; }
        public Nullable<decimal> 金额小计 { get; set; }
        public Nullable<int> backid { get; set; }
        public string 销售单号 { get; set; }
        public string 销售员 { get; set; }
        public Nullable<System.DateTime> 销售日期 { get; set; }
        public string 销售门店 { get; set; }
        public string 销售公司 { get; set; }
        public string 会员卡号 { get; set; }
        public string 客户电姓名 { get; set; }
        public string 客户电话 { get; set; }
        public Nullable<decimal> 应收总额 { get; set; }
        public string 优惠券名称 { get; set; }
        public Nullable<decimal> 优惠券金额 { get; set; }
        public Nullable<decimal> 实收总额 { get; set; }
        public string 实收总额大写 { get; set; }
        public Nullable<decimal> 刷卡 { get; set; }
        public Nullable<decimal> 现金 { get; set; }
        public string 原始单号 { get; set; }
        public string 系统单号 { get; set; }
    }
}
