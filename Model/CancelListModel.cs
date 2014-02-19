using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CancelListModel
    {
        private string proID;

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }

        private string iMEI;

        public string IMEI
        {
            get { return iMEI; }
            set { iMEI = value; }
        }
        private string proFormat;

        public string ProFormat
        {
            get { return proFormat; }
            set { proFormat = value; }
        }

        private string proName;

        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }

        private string typeName;

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        private string note;

        private string inListID;

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        private decimal proCount;

        public decimal ProCount
        {
            get { return proCount; }
            set { proCount = value; }
        }

        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private List<IMEIModel> imeiList = new List<IMEIModel>();

        public List<IMEIModel> ImeiList
        {
            get { return imeiList; }
            set { imeiList = value; }
        }
    }
}
