namespace SlModel
{
    public class ProductionModel
    {
        public bool Isdecimals { get; set; }
        private string  proID;

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
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

        private string proName;

        public string ProName
        {
            get { return proName; }
            set { proName = value; }
        }
        private string proFormat;

        public string ProFormat
        {
            get { return proFormat; }
            set {proFormat = value; }
        }
        private bool isNeedIMEI;

        public bool IsNeedIMEI
        {
            get { return isNeedIMEI; }
            set { isNeedIMEI = value; }
        }
        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private string classID;

        public string ClassID
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
        private string typeID;

        public string TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        private string sellTypeName;

        public string SellTypeName
        {
            get { return sellTypeName; }
            set { sellTypeName = value; }
        }
        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        private int sellTypeID;

        public int SellTypeID
        {
            get { return sellTypeID; }
            set { sellTypeID = value; }
        }
    }
}
