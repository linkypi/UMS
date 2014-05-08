namespace UserMS.Model
{
    public class ServiceSellGridModel:ProSellGridModel
    {
        private string _sProName;
        private string _sProClass;
        private string _simei;
        private decimal _freeCount;


        public decimal FreeCount
        {
            get { return _freeCount; }
            set
            {
                if (value == _freeCount) return;
                _freeCount = value;
                OnPropertyChanged("FreeCount");
            }
        }

        public string SIMEI
        {
            get { return _simei; }
            set
            {
                if (value == _simei) return;
                _simei = value;
                OnPropertyChanged("SIMEI");
            }
        }

        public string SProClass
        {
            get { return _sProClass; }
            set
            {
                if (value == _sProClass) return;
                _sProClass = value;
                OnPropertyChanged("SProClass");
            }
        }

        public string SProName
        {
            get { return _sProName; }
            set
            {
                if (value == _sProName) return;
                _sProName = value;
                OnPropertyChanged("SProName");
            }
        }
    }
}