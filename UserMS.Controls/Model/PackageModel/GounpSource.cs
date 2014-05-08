using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SlModel.Annotations;

namespace UserMS.Model.PackageModel
{
    public class GounpSource : INotifyPropertyChanged
    {
        private List<API.Sys_Option> _sellTypeList;
        private string groupName;
        public List<bool> Ismust { get; set; }
        public  int sellType{get ;set;}
        public string Note { get; set; }
        public int SellTypeID { get; set; }
        public string SellTypeName { get; set; }
        public string IsmustText { get; set; }
        public List<API.ProModel> ProModel { get; set; }
        private List<API.Package_GroupInfo> grounpNameList;

        private int groupID;

        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        public string GroupName
        {
            get { return groupName; }
            set
            {
                if (Equals(value, groupName)) return;
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }
        public List<API.Package_GroupInfo> GrounpNameList
        {
            get { return grounpNameList; }
            set
            {
                if (Equals(value, grounpNameList)) return;
                grounpNameList = value;
                OnPropertyChanged("GrounpNameList");
            }
        }
   
        public List<API.Sys_Option> SellTypeList
        {
            get { return _sellTypeList; }
            set
            {
                if (Equals(value, _sellTypeList)) return;
                _sellTypeList = value;
                OnPropertyChanged("SellTypeList");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
