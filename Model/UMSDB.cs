namespace Model
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Data;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.ComponentModel;
    using System;
    using System.Collections;
    using System.Configuration;
 

    public partial class UMSDB : global::Model._UMSDB
    {
        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();


        

        public UMSDB(string connection) :
               base(connection, mappingSource)
        {

        }
        public UMSDB() :
            base(ConfigurationManager.AppSettings["ConnectionString"]+"", mappingSource)
        {

        }

        //[global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.GetInOutSellInfo")]
        //[ResultType(typeof(Model.Report_InOutSellInfo))]
        //new public ISingleResult<GetInOutSellInfoResult> GetInOutSellInfo([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> t1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> t2)
        //{
        //    IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), t1, t2);
        //    return (((ISingleResult<GetInOutSellInfoResult>)result.ReturnValue));
        //}
        //[global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.GetInOutSellInfo")]
        //public ISingleResult<GetInOutSellInfoResult> GetInOutSellInfo([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> t1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> t2)
        //{
        //    IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), t1, t2);
        //    return ((ISingleResult<GetInOutSellInfoResult>)(result.ReturnValue));
        //}
    }
}

