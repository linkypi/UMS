using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ReportSqlParams
    {
        private string _paramName;
        
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }
    }
}
