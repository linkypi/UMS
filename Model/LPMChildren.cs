using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class LPMChildren
    {
        private string sellType;

        public string SellType
        {
            get { return sellType; }
            set { sellType = value; }
        }

        private int sellTypeID;

        public int SellTypeID
        {
            get { return sellTypeID; }
            set { sellTypeID = value; }
        }

        private double lowPrice;

        public double LowPrice
        {
            get { return lowPrice; }
            set { lowPrice = value; }
        }

        private double currentLowPrice;

        public double CurrentLowPrice
        {
            get { return currentLowPrice; }
            set { currentLowPrice = value; }
        }

        private bool updateFlag;

        public bool UpdateFlag
        {
            get { return updateFlag; }
            set { updateFlag = value; }
        }

    }
}
