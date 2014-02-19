using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_EveryHallStoreInfo
    {
        public long 序号 { get; set; }
        public string ProID { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public string 制式 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public string 门店等级 { get; set; }
        public string 门店 { get; set; }
        public Nullable<decimal> 等级排序 { get; set; }
        public Nullable<int> 门店排序 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 在途 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
        public Nullable<int> 类别排序 { get; set; }
        public string 门店编码 { get; set; }
        public Nullable<int> 属性排序 { get; set; }
    }
}
