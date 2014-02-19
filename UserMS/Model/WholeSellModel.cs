using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SlModel.Annotations;

namespace UserMS.Model
{
    public class WholeSellModel: INotifyPropertyChanged
    {
        public string ProID
        {
            get { return _proId; }
            set
            {
                if (value == _proId) return;
                _proId = value;
                OnPropertyChanged("ProID");
                OnPropertyChanged("ProName");
                OnPropertyChanged("ProType");
                OnPropertyChanged("ProClass");
                OnPropertyChanged("ProFormat");
            }
        }

        public string ProFormat
        {
            get
            {
                if (_proId != null)
                {
                    try
                    {
                        return Store.ProInfo.First(info => info.ProID == _proId).ProFormat;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        public string ProName
        {
            get
            {
                if (_proId != null)
                {
                    try
                    {
                        return
                            Store.ProInfo.First(p => p.ProID == _proId).ProName;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            //            set
            //            {
            //                if (value == _proName) return;
            //                _proName = value;
            //                OnPropertyChanged("ProName");
            //            }
        }


        public string ProClass
        {
            get
            {
                if (_proId != null)
                {
                    try
                    {
                        return
                            Store.ProClassInfo.First(
                                d => d.ClassID == Store.ProInfo.First(p => p.ProID == _proId).Pro_ClassID).ClassName;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }

        }

        public string ProType
        {
            get
            {
                if (_proId != null)
                {
                    try
                    {
                        return
                            Store.ProTypeInfo.First(
                                d => d.TypeID == Store.ProInfo.First(p => p.ProID == _proId).Pro_TypeID).TypeName;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        public string Note { get; set; }
        




        public List<string> IMEIList
        {
            get { return _imeiList; }
            set
            {
                if (Equals(value, _imeiList)) return;
                _imeiList = value;
                OnPropertyChanged("IMEIList");
            }
        }

        public decimal ProCount
        {
            get
            {
                if (_needImei) {return _imeiList.Count;}
                else
                {
                    return _proCount;
                }
                
            }
            set
            {
                
                if (value == _proCount) return;
                if (_needImei) return;
                if (value > AduitCount)
                {
                    throw new Exception("批发数量不可大于审批数量");
                }
                if (_proId != null)
                {
                    try
                    {
                        if (Store.ProInfo.First(info => info.ProID == _proId).ISdecimals)
                        {
                            _proCount = decimal.Truncate(value * 100) / 100;
                        }
                        else
                        {
                            _proCount = decimal.Truncate(value);

                        }

                    }
                    catch
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                OnPropertyChanged("ProCount");
                OnPropertyChanged("ProMoney");
                OnPropertyChanged("AfterOffPrice");
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
                OnPropertyChanged("ProMoney");
                OnPropertyChanged("AfterOffPrice");
            }
        }

        public decimal ProMoney
        {
            get { return AfterOffPrice * ProCount; }
           
        }

        public decimal OffPrice
        {
            get { return _offPrice; }
            set
            {
                if (value == _offPrice) return;
                _offPrice = value;
                OnPropertyChanged("OffPrice");
                OnPropertyChanged("ProMoney");
                OnPropertyChanged("AfterOffPrice");
            }
        }

        public decimal AduitCount
        {
            get { return _aduitCount; }
            set
            {
                if (value == _aduitCount) return;
                _aduitCount = value;
                OnPropertyChanged("AduitCount");

            }
        }

        public bool NeedIMEI
        {
            get { return _needImei; }
            set
            {
                if (value.Equals(_needImei)) return;
                _needImei = value;
                OnPropertyChanged("NeedIMEI");
            }
        }

        public decimal AfterOffPrice
        {
            get { return _proPrice-_offPrice; }
            
        }

        private string _proId;
        private string _proName;
        private string _proClass;
        private string _proType;
        private List<string> _imeiList;
        public decimal _proCount;
        private decimal _proPrice;
        private decimal _proMoney;
        private decimal _offPrice;
        private decimal _aduitCount;
        private bool _needImei;
        private decimal _afterOffPrice;


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public WholeSellModel()
        {
            _imeiList=new List<string>();
        }
    }
}