using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_IMEIInfo
    {
        public long 序号 { get; set; }
        public string 串码 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public string 门店 { get; set; }
        public string 区域 { get; set; }
        public string 状态 { get; set; }
        public string 备注 { get; set; }
        public int 库龄_天 { get; set; }
        public string 门店编码 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
    }
}
