using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using System.Text;

namespace ReportModel
{
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary> 
    [MetadataTypeAttribute(typeof(Report_OutInfo.OutOrderMetadata))]
    public partial class Report_OutInfo
    {
        internal sealed class OutOrderMetadata
        {
            private OutOrderMetadata()
            {

            }
            [Include]
            public EntityCollection<Report_OutOrderListInfo> Report_OutOrderListInfo { get; set; }
        }
    }
}
