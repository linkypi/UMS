using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UserMS
{
    public class TreeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string ID { get; set; }
        public int NewID { get; set; }
        private string _title;
        public string[] Fields { get; set; }
        public object[] Values { get; set; }
        public string Title
        {
            get { return _title; }
            set
            {
                if (value != this._title)
                {
                    this._title = value;
                    this.NotifyPropertyChanged("Title");
                }
            }
        }

        //public string Title;
        public Uri Address { get; set; }
        public List<TreeViewModel> Children { get; set; }
        public bool cando { get; set; }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}