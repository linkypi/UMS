using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Rules_ImportModel
    {
        private string proName;

        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }
        private string rulesName;

        public string RulesName
        {
            get { return rulesName; }
            set { rulesName = value; }
        }

        private int rulesTypeID;

        public int RulesTypeID
        {
            get { return rulesTypeID; }
            set { rulesTypeID = value; }
        }

        private decimal offPrice;

        public decimal OffPrice
        {
            get { return offPrice; }
            set { offPrice = value; }
        }

        private decimal maxPrice;

        public decimal MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }

        private decimal minPrice;

        public decimal MinPrice
        {
            get { return minPrice; }
            set { minPrice = value; }
        }

    }
}
