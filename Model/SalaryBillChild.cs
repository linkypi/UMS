using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalaryBillChild
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public decimal OverNum { get; set; }
        public decimal OverRatio { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Ratio { get; set; }
        public string Note { get; set; }
        public decimal PriceStep { get; set; }
        private int sellTypeID;
        public decimal Step { get; set; }

        public int SellTypeID
        {
            get { return sellTypeID; }
            set { sellTypeID = value; }
        }

        private bool updateFlag;

        public bool UpdateFlag
        {
            get { return updateFlag; }
            set { updateFlag = value; }
        }

        private int opID;

        public int OpID
        {
            get { return opID; }
            set { opID = value; }
        }

        private decimal specalSalary;

        public decimal SpecalSalary
        {
            get { return specalSalary; }
            set { specalSalary = value; }
        }

        private string sellTypeName;

        public string SellTypeName
        {
            get { return sellTypeName; }
            set { sellTypeName = value; }
        }
        private int day;

        public int Day
        {
            get { return day; }
            set { day = value; }
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


        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        private DateTime oldStartDate;

        public DateTime OldStartDate
        {
            get { return oldStartDate; }
            set { oldStartDate = value; }
        }

        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private DateTime oldEndDate;

        public DateTime OldEndDate
        {
            get { return oldEndDate; }
            set { oldEndDate = value; }
        }

        private decimal baseSalary;

        public decimal BaseSalary
        {
            get { return baseSalary; }
            set { baseSalary = value; }
        }

        private decimal oldbaseSalary;

        public decimal OldBaseSalary
        {
            get { return oldbaseSalary; }
            set { oldbaseSalary = value; }
        }
    }
}
