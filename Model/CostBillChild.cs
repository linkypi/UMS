using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public  class CostBillChild
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private bool updateFlag;

        public bool UpdateFlag
        {
            get { return updateFlag; }
            set { updateFlag = value; }
        }

        private decimal retailPrice;

        public decimal RetailPrice
        {
            get { return retailPrice; }
            set { retailPrice = value; }
        }

        private decimal oldretailPrice;

        public decimal OldRetailPrice
        {
            get { return oldretailPrice; }
            set { oldretailPrice = value; }
        }

        private decimal oldCostPrice;

        public decimal OldCostPrice
        {
            get { return oldCostPrice; }
            set { oldCostPrice = value; }
        }

        private decimal curCostPrice;

        public decimal CurCostPrice
        {
            get { return curCostPrice; }
            set { curCostPrice = value; }
        }

        private decimal newCostPrice;

        public decimal NewCostPrice
        {
            get { return newCostPrice; }
            set { newCostPrice = value; }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        private bool isDecimal;

        public bool IsDecimal
        {
            get { return isDecimal; }
            set { isDecimal = value; }
        }
        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private DateTime oldstartDate;

        public DateTime OldStartDate
        {
            get { return oldstartDate; }
            set { oldstartDate = value; }
        }

        private DateTime oldendDate;

        public DateTime OldEndDate
        {
            get { return oldendDate; }
            set { oldendDate = value; }
        }

    }
}
