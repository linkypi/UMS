using System.ComponentModel;
using SlModel.Annotations;

namespace UserMS.Model

{
    public class YanbaoModel : INotifyPropertyChanged
    {
        private string _name;
        private string _phone;
        private string _oldId;
        private string _imei;
        private string _class;
        private string _model;
        private decimal _modelPrice;
        private string _invoiceNumber;
        private string _batteryImei;
        private string _chargerImei;
        private string _handphoneImei;
        private string _note;
        private string _proId;
        private string _sellId;
        private decimal _yanbaoPrice;
        private string _yanbaoName;
        public string YanbaoName
        {
            get { return _yanbaoName; }
            set
            {
                if (value == _yanbaoName) return;
                _yanbaoName = value;
                OnPropertyChanged("YanbaoName");
            }
        }

        public decimal YanbaoPrice
        {
            get { return _yanbaoPrice; }
            set
            {
                if (value == _yanbaoPrice) return;
                _yanbaoPrice = value;
                OnPropertyChanged("YanbaoPrice");
            }
        }

        public string SellID
        {
            get { return _sellId; }
            set
            {
                if (value == _sellId) return;
                _sellId = value;
                OnPropertyChanged("SellID");
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

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value == _phone) return;
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string OldID
        {
            get { return _oldId; }
            set
            {
                if (value == _oldId) return;
                _oldId = value;
                OnPropertyChanged("OldID");
            }
        }

        public string IMEI
        {
            get { return _imei; }
            set
            {
                if (value == _imei) return;
                _imei = value;
                OnPropertyChanged("IMEI");
            }
        }

        public string Class
        {
            get { return _class; }
            set
            {
                if (value == _class) return;
                _class = value;
                OnPropertyChanged("Class");
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (value == _model) return;
                _model = value;
                OnPropertyChanged("Model");
            }
        }

        public decimal ModelPrice
        {
            get { return _modelPrice; }
            set
            {
                if (value == _modelPrice) return;
                _modelPrice = value;
                OnPropertyChanged("ModelPrice");
            }
        }

        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set
            {
                if (value == _invoiceNumber) return;
                _invoiceNumber = value;
                OnPropertyChanged("InvoiceNumber");
            }
        }

        public string BatteryIMEI
        {
            get { return _batteryImei; }
            set
            {
                if (value == _batteryImei) return;
                _batteryImei = value;
                OnPropertyChanged("BatteryIMEI");
            }
        }

        public string ChargerIMEI
        {
            get { return _chargerImei; }
            set
            {
                if (value == _chargerImei) return;
                _chargerImei = value;
                OnPropertyChanged("ChargerIMEI");
            }
        }

        public string HandphoneIMEI
        {
            get { return _handphoneImei; }
            set
            {
                if (value == _handphoneImei) return;
                _handphoneImei = value;
                OnPropertyChanged("HandphoneIMEI");
            }
        }

        public string Note
        {
            get { return _note; }
            set
            {
                if (value == _note) return;
                _note = value;
                OnPropertyChanged("Note");
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

    public class YanbaoModelImport : YanbaoModel
    {
        private string _sellOldid;
        private string _custPhone;
        private string _custName;

        public string SellOLDID
        {
            get { return _sellOldid; }
            set
            {
                if (value == _sellOldid) return;
                _sellOldid = value;
                OnPropertyChanged("SellOLDID");
            }
        }

        public string CustName
        {
            get { return _custName; }
            set
            {
                if (value == _custName) return;
                _custName = value;
                OnPropertyChanged("CustName");
            }
        }

        public string CustPhone
        {
            get { return _custPhone; }
            set
            {
                if (value == _custPhone) return;
                _custPhone = value;
                OnPropertyChanged("CustPhone");
            }
        }
    }

  
}