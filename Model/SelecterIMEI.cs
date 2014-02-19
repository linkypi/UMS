using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SelecterIMEI
    {
        public string OldIMEI { get; set; }
        private string note;
        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        private string iMEI;
        /// <summary>
        /// 串码列表
        /// </summary>
        public string IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
        }
    }
}
