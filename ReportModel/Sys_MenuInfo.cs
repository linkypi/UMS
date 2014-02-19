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
    [MetadataTypeAttribute(typeof(Sys_MenuInfo.MenuMetadata))]
    public partial class Sys_MenuInfo
    {
        internal sealed class MenuMetadata
        {
            private MenuMetadata()
            {

            }
            [Include]
            public EntityCollection<Sys_Role_MenuInfo> Sys_Role_MenuInfo { get; set; }
        }
    }
}
