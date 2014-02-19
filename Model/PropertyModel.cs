using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PropertyModel
    {
        private int Id;

        public int ID
        {
            get { return Id; }
            set { Id = value; }
        }
        private string cate;

        public string Cate
        {
            get { return cate; }
            set { cate = value; }
        }

        private string note;

        public string Note
        {
            get { return note; }
            set { note = value; }

        }
        List<PropertyValueModel> propertyValueModel;

        public List<PropertyValueModel> PropertyValueModel
        {
            get { return propertyValueModel; }
            set { propertyValueModel = value; }
        }
    }
}
