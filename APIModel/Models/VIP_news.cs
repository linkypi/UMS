using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_news
    {
        public int newsId { get; set; }
        public string newsTitle { get; set; }
        public string newsAbstract { get; set; }
        public string newsContent { get; set; }
        public string newsAuthor { get; set; }
        public string newsSfrom { get; set; }
        public string newsSpic { get; set; }
        public string newsPicList { get; set; }
        public Nullable<System.DateTime> newsCreatetime { get; set; }
        public Nullable<int> weeklyId { get; set; }
    }
}
