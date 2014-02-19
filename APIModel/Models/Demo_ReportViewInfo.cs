using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_ReportViewInfo
    {
        public Demo_ReportViewInfo()
        {
            this.Demo_ReportViewColumnInfo = new List<Demo_ReportViewColumnInfo>();
        }

        public int ID { get; set; }
        public string ReportViewName { get; set; }
        public Nullable<int> MenuID { get; set; }
        public virtual ICollection<Demo_ReportViewColumnInfo> Demo_ReportViewColumnInfo { get; set; }
        public virtual Sys_MenuInfo Sys_MenuInfo { get; set; }
    }
}
