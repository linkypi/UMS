using System;
using System.Collections.Generic;
using System.ComponentModel;
using SlModel.Annotations;
using UserMS.API;


namespace UserMS.Model
{
    public class VIPSellGridModel:INotifyPropertyChanged    
    {
        private string _imei;
        private string _memberName;
        private string _mobiPhone;
        private string _telePhone;
        private string _qq;
        private string _address;
        private string _idCard;
        private string _note;
        private string _lzUserName;
        private string _lzUserId;
        private string _oldId;
        private string _sex;
        private DateTime? _birthday;

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

        public string MemberName
        {
            get { return _memberName; }
            set
            {
                if (value == _memberName) return;
                _memberName = value;
                OnPropertyChanged("MemberName");
            }
        }

        public string MobiPhone
        {
            get { return _mobiPhone; }
            set
            {
                if (value == _mobiPhone) return;
                _mobiPhone = value;
                OnPropertyChanged("MobiPhone");
            }
        }

        public string TelePhone
        {
            get { return _telePhone; }
            set
            {
                if (value == _telePhone) return;
                _telePhone = value;
                OnPropertyChanged("TelePhone");
            }
        }

        public string QQ
        {
            get { return _qq; }
            set
            {
                if (value == _qq) return;
                _qq = value;
                OnPropertyChanged("QQ");
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (value == _address) return;
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public string IDCard
        {
            get { return _idCard; }
            set
            {
                if (value == _idCard) return;
                _idCard = value;
                OnPropertyChanged("IDCard");
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

        public string LZUserName
        {
            get { return _lzUserName; }
            set
            {
                if (value == _lzUserName) return;
                _lzUserName = value;
                OnPropertyChanged("LZUserName");
            }
        }

        public string LZUserID
        {
            get { return _lzUserId; }
            set
            {
                if (value == _lzUserId) return;
                _lzUserId = value;
                OnPropertyChanged("LZUserID");
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

        public DateTime? Birthday
        {
            get { return _birthday; }
            set
            {
                if (value.Equals(_birthday)) return;
                _birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public int? IDCard_ID { get; set; }

        public DateTime JoinTime
        {
            get { return _joinTime; }
            set
            {
               
                OnPropertyChanged("JoinTime");
            }
        }

        public string Sex
        {
            get { return _sex; }
            set
            {
                if (value == _sex) return;
                _sex = value;
                OnPropertyChanged("Sex");
            }
        }

        public List<API.VIP_IDCardType> CardType { get; set; }
        private DateTime _joinTime=DateTime.Now;
        private VIP_VIPType _vipType;
        private string _proId;

        public int? Validity
        {
            get
            {
                return VIPType!=null ? VIPType.Validity : null;
            }
            set
            {

            }
        }

        public string TypeName
        {
            get
            {
                return VIPType != null ? VIPType.Name : null;
            }
            set
            {
                
            }
        }

        public int? TypeID
        {
            get { return VIPType != null ? (int?)VIPType.ID : null; }
            set
            {

            }
        }

        public decimal? Point
        {
            get
            {
                return VIPType != null ? VIPType.SPoint : null;
            }
            set
            {

            }
        }

        public decimal? ProPrice
        {
            get
            {
                return VIPType != null ? VIPType.Cost_production : null;
            }
            set
            {

            }
        }

        public VIP_VIPType VIPType
        {
            get { return _vipType; }
            set
            {
                if (Equals(value, _vipType)) return;
                _vipType = value;
                OnPropertyChanged("VIPType");
                OnPropertyChanged("Validity");
                OnPropertyChanged("TypeName");
                OnPropertyChanged("TypeID");
                OnPropertyChanged("Point");
                OnPropertyChanged("ProPrice");
            }
        }

        public List<Sys_UserInfo>  UserInfos { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}