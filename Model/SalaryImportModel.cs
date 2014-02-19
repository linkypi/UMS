using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalaryImportModel
    {
        private string proName;

        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }

        private string proMainID;

        public string ProMainID
        {
            get { return proMainID; }
            set { proMainID = value; }
        }

        private string proID;

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }
        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private int classID;

        public int ClassID
        {
            get { return classID; }
            set { classID = value; }
        }
        private string typeName;

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        private int typeID;

        public int TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }
        private int sellType;

        public int SellType
        {
            get { return sellType; }
            set { sellType = value; }
        }
        private decimal baseSalary;

        public decimal BaseSalary
        {
            get { return baseSalary; }
            set { baseSalary = value; }
        }

        private string sellTypeName;

        public string SellTypeName
        {
            get { return sellTypeName; }
            set { sellTypeName = value; }
        }
        private int month;

        public int Month
        {
            get { return month; }
            set { month = value; }
        }
        private int year;

        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        private int day;

        public int Day
        {
            get { return day; }
            set { day = value; }
        }
    }
}
