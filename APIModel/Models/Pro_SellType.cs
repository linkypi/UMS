using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellType
    {
        public Pro_SellType()
        {
            this.Off_AduitProInfo = new List<Off_AduitProInfo>();
            this.Package_GroupInfo = new List<Package_GroupInfo>();
            this.Pro_PriceChangeList = new List<Pro_PriceChangeList>();
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
            this.VIP_ProOffList = new List<VIP_ProOffList>();
            this.Pro_SellTypeProduct_bak = new List<Pro_SellTypeProduct_bak>();
            this.Pro_SellTypeProduct = new List<Pro_SellTypeProduct>();
            this.Rules_SellTypeInfo = new List<Rules_SellTypeInfo>();
            this.Sys_SalaryCurrentList = new List<Sys_SalaryCurrentList>();
            this.Sys_SalaryList_bak = new List<Sys_SalaryList_bak>();
            this.Sys_SalaryList = new List<Sys_SalaryList>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Note { get; set; }
        public string TicketRegex { get; set; }
        public Nullable<bool> HaveTicketPrice { get; set; }
        public virtual ICollection<Off_AduitProInfo> Off_AduitProInfo { get; set; }
        public virtual ICollection<Package_GroupInfo> Package_GroupInfo { get; set; }
        public virtual ICollection<Pro_PriceChangeList> Pro_PriceChangeList { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public virtual ICollection<VIP_ProOffList> VIP_ProOffList { get; set; }
        public virtual ICollection<Pro_SellTypeProduct_bak> Pro_SellTypeProduct_bak { get; set; }
        public virtual ICollection<Pro_SellTypeProduct> Pro_SellTypeProduct { get; set; }
        public virtual ICollection<Rules_SellTypeInfo> Rules_SellTypeInfo { get; set; }
        public virtual ICollection<Sys_SalaryCurrentList> Sys_SalaryCurrentList { get; set; }
        public virtual ICollection<Sys_SalaryList_bak> Sys_SalaryList_bak { get; set; }
        public virtual ICollection<Sys_SalaryList> Sys_SalaryList { get; set; }
    }
}
