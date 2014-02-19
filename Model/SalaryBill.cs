using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalaryBill
    {
        private string proID;

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }
        public string SellTypeName { get; set; }
        public bool IsProMain { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private string proFromat;

        public string ProFormat
        {
            get { return proFromat; }
            set { proFromat = value; }
        }

        private int proMainID;

        public int ProMainID
        {
            get { return proMainID; }
            set { proMainID = value; }
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

        private List<Model.SalaryBillChild> children;

        public List<Model.SalaryBillChild> Children
        {
            get { return children; }
            set { children = value; }
        }

        private int sellType;

        public int SellType
        {
            get { return sellType; }
            set { sellType = value; }
        }


    }
}
