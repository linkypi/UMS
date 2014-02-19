using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 多选的字符串类型参数
    /// </summary>
    public class ReportSqlParams_ListString: ReportSqlParams
    {

        private List<string> _paramValues;
         
        /// <summary>
        /// 参数的取值列表，如果是多个value 那么就是 or 条件
        /// </summary>
        public List<string> ParamValues
        {
            get { return _paramValues; }
            set { _paramValues = value; }
        }
    }
}
