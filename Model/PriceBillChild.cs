using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class PriceBillChild
    {
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        private bool isAduit;

        public bool IsAduit     
        {
            get { return isAduit; }
            set { isAduit = value; }
        }

        private bool oldIsAduit;

        public bool OldIsAduit      //OldIsAduit
        {
            get { return oldIsAduit; }
            set { oldIsAduit = value; }
        }
        private bool isDecimal;

        public bool IsDecimal
        {
            get { return isDecimal; }
            set { isDecimal = value; }
        }
           private int sellTypeID;

           public int SellTypeID
           {
               get { return sellTypeID; }
               set { sellTypeID = value; }
           }

           private string sellTypeName;

           public string SellTypeName
           {
               get { return sellTypeName; }
               set { sellTypeName = value; }
           }

           private bool isTicketUseful;

           public bool IsTicketUseful
           {
               get { return isTicketUseful; }
               set { isTicketUseful = value; }
           }

           private bool oldisTicketUseful;

           public bool OldIsTicketUseful
           {
               get { return oldisTicketUseful; }
               set { oldisTicketUseful = value; }
           }

           private decimal oldprice;

           public decimal OldPrice
           {
               get { return oldprice; }
               set { oldprice = value; }
           }

           private decimal price;

           public decimal Price
           {
               get { return price; }
               set { price = value; }
           }

           private bool hasPrice;

           public bool HasPrice
           {
               get { return hasPrice; }
               set { hasPrice = value; }
           }

           private decimal lowPrice;

           public decimal LowPrice
           {
               get { return lowPrice; }
               set { lowPrice = value; }
           }
           private decimal oldlowPrice;

           public decimal OldLowPrice
           {
               get { return oldlowPrice; }
               set { oldlowPrice = value; }
           }

           private decimal minPrice;

           public decimal MinPrice
           {
               get { return minPrice; }
               set { minPrice = value; }
           }
           private decimal oldminPrice;

           public decimal OldMinPrice
           {
               get { return oldminPrice; }
               set { oldminPrice = value; }
           }

           private decimal maxPrice;

           public decimal MaxPrice
           {
               get { return maxPrice; }
               set { maxPrice = value; }
           }
           private decimal oldmaxPrice;

           public decimal OldMaxPrice
           {
               get { return oldmaxPrice; }
               set { oldmaxPrice = value; }
           }
       
    }
}
