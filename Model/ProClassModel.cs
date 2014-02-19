using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    public class ProClassModel
    {
        private int classID;

        public int ClassID
        {
            get { return classID; }
            set { classID = value; }
        }
        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private int order;

        public int Order
        {
            get { return order; }
            set { order = value; }
        }
        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        public bool HasSalary { get; set; }
        public string IsSalary { get; set; }

        /// <summary>
        /// 权限所参照的类别，比如新增代金D 参照的权限和代金A一样
        /// </summary>
        public int LikeClass { get; set; }
        public string LikeClassName { get; set; }
        /// <summary>
        /// 类别所授权的用户
        /// </summary>
        public List<string> UserIDS { get; set; }
    }
}
