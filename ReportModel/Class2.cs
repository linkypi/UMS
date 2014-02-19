using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReportModel
{
    [Serializable()]
    [DataContractAttribute(IsReference = true)]
    public class Class2
    {
        private Int64 _ID;
        [System.ComponentModel.DataAnnotations.Key]
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [DataMemberAttribute()]
        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
    }
}
