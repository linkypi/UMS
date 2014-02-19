using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ReportModel
{
    [MetadataTypeAttribute(typeof(GetInOutSellInfo_Result.MenuMetadata))]
    public partial class GetInOutSellInfo_Result
    {
        internal sealed class MenuMetadata
        {
            private MenuMetadata()
            {

            }
            [Key]
            public Int64 序号;
        }
    }
}
