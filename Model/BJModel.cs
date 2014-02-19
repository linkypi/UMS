using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BJModel
    {
        private string proID;
        private string proName;
        private string proFormat;
        private string inListID;
        private bool needIMEI;
        private int pro_ClassID;
        private int pro_TypeID;
        private int proMainID;
        private string iMEI;
        private bool isDecimal;

        public bool IsDecimal
        {
            get { return isDecimal; }
            set { isDecimal = value; }
        }

        public string IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
        }

        public int ProMainID
        {
            get { return proMainID; }
            set { proMainID = value; }
        }

        public int TypeID
        {
            get { return pro_TypeID; }
            set { pro_TypeID = value; }
        }

        public int ClassID
        {
            get { return pro_ClassID; }
            set { pro_ClassID = value; }
        }

        public bool NeedIMEI
        {
            get { return needIMEI; }
            set { needIMEI = value; }
        }

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }

        public string ProFormat
        {
            get { return proFormat; }
            set { proFormat = value; }
        }

        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }
    }

}
