using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ReportSqlParams_Bool:ReportSqlParams
    {
        private bool _paramValues;

        /// <summary>
        /// 参数的取值列表，如果是多个value 那么就是 or 条件
        /// </summary>
        public bool ParamValues
        {
            get { return _paramValues; }
            set { _paramValues = value; }
        }
    }
}
