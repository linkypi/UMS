namespace SlModel
{
    public class CheckModel
    {
        private string imei;
        private string note;
        private bool sucess;
        private int borowListID;
        private string oldIMEI;
        private string inListID;
        private int repairListID;
        private string proID;
        private static int identity;
        private int id;

        public CheckModel()
        {
            id = identity++;
        }
        public int ID
        {
            get { return id; }
        }

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }

        public int RepairListID
        {
            get { return repairListID; }
            set { repairListID = value; }
        }

        public string InListID
        {
            get { return inListID; }
            set { inListID = value; }
        }

        public string OldIMEI
        {
            get { return oldIMEI; }
            set { oldIMEI = value; }
        }


        public int BorowListID
        {
            get { return borowListID; }
            set { borowListID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Sucess
        {
            get 
            {
                return sucess; 
            }
            set
            {
                sucess = value;
                if (sucess)
                {
                    note = "成功";
                }
                else
                {
                    note = "失败";
                }
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get {
                return note;
            }
            set { note = value; }
        }

        public string IMEI
        {
            get { return imei; }
            set { imei = value; }
        }

    }
}
