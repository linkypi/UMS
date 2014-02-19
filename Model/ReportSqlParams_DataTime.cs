using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 字符串类型的参数
    /// </summary>
    public class ReportSqlParams_DataTime: ReportSqlParams
    {

        private DateTime? _paramValues;
         
        /// <summary>
        /// 参数的取值列表，如果是多个value 那么就是 or 条件
        /// </summary>
        public DateTime? ParamValues
        {
            get { return _paramValues; }
            set { _paramValues = value; }
        }
    }
}
