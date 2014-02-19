using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BAduitModel
    {

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

        public string ProID
        {
            get { return proID; }
            set { proID = value; }
        }

        private int id;

        private string proClassID;
        private string pro_TypeID;
        private decimal proPrice;
        private string proName;
        private decimal proCount;
        private string proformat;
        private string proID;
        private string proClassName;
        private string proTypeName;
        private bool isDecimal;

        public bool IsDecimal
        {
            get { return isDecimal; }
            set { isDecimal = value; }
        }

        public string TypeName
        {
            get { return proTypeName; }
            set { proTypeName = value; }
        } 

        public string ClassName
        {
            get { return proClassName; }
            set { proClassName = value; }
        }


        /// <summary>
        /// 商品属性
        /// </summary>
        public string ProFormat
        {
            get { return proformat; }
            set { proformat = value; }
        }

        /// <summary>
        /// 商品型号
        /// </summary>
        public string Pro_ClassID
        {
            get { return proClassID; }
            set { proClassID = value; }
        }

        /// <summary>
        /// 商品类别
        /// </summary>
        public string Pro_TypeID
        {
            get { return pro_TypeID; }
            set { pro_TypeID = value; }
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

        /// <summary>
        /// 单卖价格
        /// </summary>
        public decimal ProPrice
        {
            get { return proPrice; }
            set { proPrice = value; }
        }
    }
}
