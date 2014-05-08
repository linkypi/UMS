using System.ComponentModel;
using SlModel.Annotations;
using UserMS.API;

namespace UserMS.Model
{
    public class AirChargeModel:INotifyPropertyChanged
    {
        private string _proName;
        private decimal _proCount;
        private string _chargePhone;
        private string _name;
        private string _proId;
        private decimal _proPrice;
        private decimal _proMoney;
        private Pro_SellListInfo_Temp _sellListModel;

        public string ProName
        {
            get { return _proName; }
            set
            {
                if (value == _proName) return;
                _proName = value;
                OnPropertyChanged("ProName");
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

        public string ChargePhone
        {
            get { return _chargePhone; }
            set
            {
                if (value == _chargePhone) return;
                _chargePhone = value;
                OnPropertyChanged("ChargePhone");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string ProID
        {
            get { return _proId; }
            set
            {
                if (value == _proId) return;
                _proId = value;
                OnPropertyChanged("ProID");
            }
        }

        public decimal ProPrice
        {
            get { return _proPrice; }
            set
            {
                if (value == _proPrice) return;
                _proPrice = value;
                OnPropertyChanged("ProPrice");
            }
        }

        public decimal ProMoney
        {
            get { return _proMoney; }
            set
            {
                if (value == _proMoney) return;
                _proMoney = value;
                OnPropertyChanged("ProMoney");
            }
        }

        private string _OldID;

        public string OldID
        {
            get { return _OldID; }
            set
            {
                if (value == _OldID) return;
                _OldID = value;
                OnPropertyChanged("OldID");
            }
        }

        private string _Note;
        public string Note
        {
            get { return _Note; }
            set { if (value == _Note) return;
                _Note = value;
                OnPropertyChanged("Note");
            }
        }

        

        public API.Pro_SellListInfo_Temp SellListModel
        {
            get { return _sellListModel; }
            set
            {
                if (Equals(value, _sellListModel)) return;
                _sellListModel = value;
                OnPropertyChanged("SellListModel");
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