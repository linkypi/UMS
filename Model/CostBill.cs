using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CostBill
    {
        private string proID;

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
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

        private string typeID;

        public string TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        private string typeName;

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        private string classID;

        public string ClassID
        {
            get { return classID; }
            set { classID = value; }
        }

        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private List<CostBillChild> children;

        public List<CostBillChild> Children
        {
            get { return children; }
            set { children = value; }
        }

        private bool isDecimal;

        public bool IsDecimal
        {
            get { return isDecimal; }
            set { isDecimal = value; }
        }

    }
}
