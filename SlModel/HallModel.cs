namespace SlModel
{
    public class HallModel
    {
        private string hallID;
        /// <summary>
        /// 门店ID
        /// </summary>
        public string HallID
        {
            get { return hallID; }
            set { hallID = value; }
        }
        private string hallName;
        /// <summary>
        /// 门店名称
        /// </summary>
        public string HallName
        {
            get { return hallName; }
            set { hallName = value; }
        }
        //List<AreaModel> area;
        ///// <summary>
        ///// 区域
        ///// </summary>
        //public List<AreaModel> Area
        //{
        //    get { return area; }
        //    set { area = value; }
        //}
        private string areaID;
        /// <summary>
        /// 区域ID
        /// </summary>
        public string AreaID
        {
            get { return areaID; }
            set { areaID = value; }
        }
        private string areaName;

        public string AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }
        private bool canIn;
        /// <summary>
        /// 是否能入库
        /// </summary>
        public bool CanIn
        {
            get { return canIn; }
            set { canIn = value; }
        }
        private bool canBack;
        /// <summary>
        /// 是否能退库
        /// </summary>
        public bool CanBack
        {
            get { return canBack; }
            set { canBack = value; }
        }
        private bool flag;
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        private string note;
        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        private string levelID;
        /// <summary>
        /// 等级ID
        /// </summary>
        public string LevelID
        {
            get { return levelID; }
            set { levelID = value; }
        }
        private string levelName;
/// <summary>
/// 等级名称
/// </summary>
        public string LevelName
        {
            get { return levelName; }
            set { levelName = value; }
        }
        private int order;
        /// <summary>
        /// 排序
        /// </summary>
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

    }
}
