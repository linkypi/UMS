using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class NoIMEIModel
    {
        public NoIMEIModel()
        { 
        }

        private string proID;
        private decimal proCount;

        public decimal ProCount
        {
            get { return proCount; }
            set { proCount = value; }
        }

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
    
}
