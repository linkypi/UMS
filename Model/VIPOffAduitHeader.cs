using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public  class VIPOffAduitHeader
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string note;
       
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        private decimal limit;

        public decimal Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        private int typeID;

        public int TypeID
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

        private List<Model.VIP_OffListAduit> children;

        public List<Model.VIP_OffListAduit> Children
        {
            get { return children; }
            set { children = value; }
        }

    }
}
