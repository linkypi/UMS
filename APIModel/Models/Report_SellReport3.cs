using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_SellReport3
    {
        public long 序号 { get; set; }
        public string 销售单号 { get; set; }
        public string 原始单号 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public string 销售方式 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public Nullable<decimal> 实际单价 { get; set; }
        public Nullable<decimal> 实收金额 { get; set; }
        public string 门店 { get; set; }
        public Nullable<System.DateTime> 销售日期 { get; set; }
        public string 代金券编码 { get; set; }
        public Nullable<decimal> 代金券面值 { get; set; }
        public Nullable<decimal> 兑换值 { get; set; }
        public string 串码 { get; set; }
        public string 单品优惠名称 { get; set; }
        public Nullable<decimal> 单品优惠金额 { get; set; }
        public string 组合优惠名称 { get; set; }
        public Nullable<decimal> 组合优惠金额 { get; set; }
        public Nullable<decimal> 批发优惠金额 { get; set; }
        public Nullable<decimal> 多收单价 { get; set; }
        public Nullable<decimal> 暗补 { get; set; }
        public Nullable<decimal> 列收 { get; set; }
        public Nullable<decimal> 终端代销费 { get; set; }
        public string 是否免费服务 { get; set; }
        public string 销售员 { get; set; }
        public string 会员卡号 { get; set; }
        public string 客户姓名 { get; set; }
        public string 客户电话 { get; set; }
        public string 备注 { get; set; }
        public Nullable<decimal> 门店优惠 { get; set; }
        public string 销售时间 { get; set; }
        public string 片区 { get; set; }
        public string 商品大类 { get; set; }
        public string 单头备注 { get; set; }
        public string 大区 { get; set; }
        public string 终端制式 { get; set; }
        public string 门店编码 { get; set; }
        public Nullable<int> 片区编码 { get; set; }
        public Nullable<int> 大区编码 { get; set; }
        public string 商品编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
        public Nullable<int> 商品大类编码 { get; set; }
        public string 批次号 { get; set; }
        public Nullable<decimal> 销售时成本价 { get; set; }
        public Nullable<decimal> 入库成本价 { get; set; }
        public Nullable<decimal> 规则毛利可拿回 { get; set; }
        public Nullable<decimal> 规则毛利不可拿回 { get; set; }
        public Nullable<decimal> 结算价 { get; set; }
        public string ProMainID { get; set; }
        public Nullable<decimal> price { get; set; }
    }
}
