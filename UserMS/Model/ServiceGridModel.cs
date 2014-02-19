using System;
using System.ComponentModel;
using System.Linq;
using SlModel.Annotations;
using UserMS.API;

namespace UserMS.Model

{
    public class ServiceGridModel:INotifyPropertyChanged
    {
        private string _proName;
        private decimal _proCount;
        private VIP_VIPService _serviceModel;
        public decimal UsedCount { get; set; }
        public string ProName
        {
            get
            {
                if (ServiceModel != null)
                {
                    try
                    {
                        return Store.ProInfo.First(p => p.ProID == ServiceModel.ProID).ProName;
                    }
                    catch (Exception)
                    {
                        return null;
                        
                       
                    }
                }
                return null;
            }
           
        }
        public decimal ProCount
        {
            get { return _proCount; }
            set
            {
                if (value == _proCount) return;
                _proCount = value;
                OnPropertyChanged("ProCount");
            }
        }
        public API.VIP_VIPService ServiceModel
        {
            get { return _serviceModel; }
            set
            {
                if (Equals(value, _serviceModel)) return;
                _serviceModel = value;
                _proCount = Convert.ToDecimal(value.SCount) - Convert.ToDecimal(value.UsedCount);
                OnPropertyChanged("ServiceModel");
                OnPropertyChanged("ProName");
                OnPropertyChanged("ProCount");
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