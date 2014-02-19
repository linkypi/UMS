using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public  class ReportPagingParam
    {
        private int _PageIndex;
        private int _PageSize=20;
        private int _RecordCount;
        private List<ReportSqlParams> _ParamList;
        private object _obj;
        /// <summary>
        /// 存放数据
        /// </summary>
        public object Obj
        {
            get { return _obj; }
            set { _obj = value; }
        }

        /// <summary>
        /// 传入的参数，取值列表
        /// </summary>
        public List<ReportSqlParams> ParamList
        {
            get { return _ParamList; }
            set { _ParamList = value; }
        }
        /// <summary>
        /// 记录数
        /// </summary>
        public int RecordCount
        {
            get { return _RecordCount; }
            set { _RecordCount = value; }
        }

        /// <summary>
        /// 每一页记录数
        /// </summary>
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        /// <summary>
        /// 索引页
        /// </summary>
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }
    }
}
