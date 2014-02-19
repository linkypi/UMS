using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_IMEITracksInfo
    {
        public long 序号 { get; set; }
        public string 串码 { get; set; }
        public string 类别 { get; set; }
        public string 品牌 { get; set; }
        public string 商品名称 { get; set; }
        public string 商品属性 { get; set; }
        public string 跟踪 { get; set; }
        public Nullable<System.DateTime> 日期 { get; set; }
        public string 操作人 { get; set; }
        public string 系统平台 { get; set; }
        public Nullable<int> 类别编码 { get; set; }
        public string 门店编码 { get; set; }
    }
}
