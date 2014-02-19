using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public  class LowPriceModel
    {
        private string proID;

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
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

        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string proFormat;

        public string ProFormat
        {
            get { return proFormat; }
            set { proFormat = value; }
        }

        private string classID;

        public string ClassID
        {
            get { return classID; }
            set { classID = value; }
        }

        private double lowPrice;

        public double LowPrice
        {
            get { return lowPrice; }
            set { lowPrice = value; }
        }

        private List<LPMChildren> children;

        public List<LPMChildren> Children
        {
            get { return children; }
            set { children = value; }
        }
    }
}
