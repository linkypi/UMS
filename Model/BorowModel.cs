using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BorowModel
    {
        private string hallID;
        private string hallName;
        private List<BorowListModel> bList;
        private int aduitCount;
        private string inListID;
        private string note;
        private bool sucess;
        private int borowListID;
        private string borower;
        private DateTime borowDate;
        private string borowType;
        private string borowID;
        private string dept;
        private string userName;
        private int id;

        private string proName;
        private decimal proCount;
        private string proID;
        private string typeName;
        private string className;

        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }
   
        /// <summary>
        /// 商品品牌
        /// </summary>
        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }

        /// <summary>
        /// 商品数量
        /// </summary>
        public decimal ProCount
        {
            get { return proCount; }
            set { proCount = value; }
        }

        public string ProClassName
        {
            get { return className; }
            set { className = value; }
        }

        public string ProTypeName
        {
            get { return typeName; }
            set { typeName = value; }

        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string BorowID
        {
            get { return borowID; }
            set { borowID = value; }
        }

        public string BorowType
        {
            get { return borowType; }
            set { borowType = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Dept
        {
            get { return dept; }
            set { dept = value; }
        }

        public DateTime BorowDate
        {
            get { return borowDate; }
            set { borowDate = value; }
        }
        public string Borower
        {
            get { return borower; }
            set { borower = value; }
        }

        public int BorowListID
        {
            get { return borowListID; }
            set { borowListID = value; }
        }

        public bool Sucess
        {
            get { return sucess; }
            set { sucess = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }
    

        public List<BorowListModel> BList
        {
            get { return bList; }
            set { bList = value; }
        }

        /// <summary>
        /// 审批的数量
        /// </summary>
        public int AduitCount
        {
            get { return aduitCount; }
            set { aduitCount = value; }
        }

        public string HallName
        {
            get { return hallName; }
            set { hallName = value; }
        }

        public string HallID
        {
            get { return hallID; }
            set { hallID = value; }
        }
     
      
 
    }

}
