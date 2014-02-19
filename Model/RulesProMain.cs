using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class RulesProMain
    {
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private int proMainID;

        public int ProMainID
        {
            get { return proMainID; }
            set { proMainID = value; }
        }

        private string proMainName;

        public string ProMainName
        {
            get { return proMainName; }
            set { proMainName = value; }
        }

        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string typeName;

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        List<Model.Pro_RulesTypeInfo> pro_RulesTypeInfo;

        public List<Model.Pro_RulesTypeInfo> Pro_RulesTypeInfo
        {
            get { return pro_RulesTypeInfo; }
            set { pro_RulesTypeInfo = value; }
        }
    }

    public class Pro_RulesTypeInfo
    {
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private int rulesTypeID;

        public int RulesTypeID
        {
            get { return rulesTypeID; }
            set { rulesTypeID = value; }
        }

        private bool canGetBack;

        public bool CanGetBack
        {
            get { return canGetBack; }
            set { canGetBack = value; }
        }
        private bool showToCus;

        public bool ShowToCus
        {
            get { return showToCus; }
            set { showToCus = value; }
        }

        private string rulesTypeName;

        public string RulesTypeName
        {
            get { return rulesTypeName; }
            set { rulesTypeName = value; }
        }

        private decimal offPrice;

        public decimal OffPrice
        {
            get { return offPrice; }
            set { offPrice = value; }
        }
        private decimal minPrice;

        public decimal MinPrice
        {
            get { return minPrice; }
            set { minPrice = value; }
        }
        private decimal maxPrice;

        public decimal MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }

        private decimal orderBy;

        public decimal OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }
    }
}
