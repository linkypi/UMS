using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SlModel.Annotations;
using UserMS.API;

namespace UserMS.Model
{
    public class ProSellGridModel : INotifyPropertyChanged//, IDataErrorInfo 
    {
        private string _proId;
        private string _proName;
        private string _unit;
        private decimal _proCount;
        private string _imei;
        private string _ticketNum;
        private decimal _ticketPrice;
        private decimal _proPrice;

        private List<API.VIP_OffList> _offLists ;

        private int SelectedOffID;


        private API.Pro_SellListInfo _sellListModel;
        private string _inListId;
//        private decimal _offprice;
        private decimal _offticketused;

        private int _specalId;
        private decimal _offPrice;
        private decimal _specalOffPrice;
        private decimal _otherCash;
        private decimal _anBu;

        private decimal _lieShouPrice;
		private decimal _otherOff;
        private decimal _anBuPrice;

        public decimal OtherOff {
            get
            {
                if (SellListModel != null) return SellListModel.OtherOff;
                return _otherOff;
            }
			set
			{
                if (value<0)throw new Exception("不允许负数");
                if (SellListModel != null) SellListModel.OtherOff = decimal.Truncate(value * 100) / 100; ;
                _otherOff = decimal.Truncate(value * 100) / 100; ;
				
				
				OnPropertyChanged("OtherOff");
				OnPropertyChanged("ProMoney");
			}
		}
        public decimal AnBuPrice
        {
            get
            {
                if (SellListModel != null) return SellListModel.AnBuPrice;
                return _anBuPrice;
            }
            set
            {
                if (SellListModel != null) SellListModel.AnBuPrice = value;
                if (value == _anBuPrice) return;
                _anBuPrice = value;
                OnPropertyChanged("AnBuPrice");
            }
        }

        private decimal _ticketUsed;
        public decimal TicketUsed
        {
            get
            {
                if (SellListModel != null) return SellListModel.TicketUsed;
                return _ticketUsed;
            }
            set
            {
                if (SellListModel != null) SellListModel.TicketUsed = value;
                if (value == _ticketUsed) return;
                _ticketUsed = value;
                OnPropertyChanged("TicketUsed");
            }
        }

        public decimal AnBu
        {
            get
            {
                if (SellListModel != null) return SellListModel.AnBu;
                return _anBu;
            }
            set
            {
                if (value < 0)
                {
                    //                    this._errors.Add("OtherCash", "多收金额不可为负数");
                    throw new Exception("暗补金额不可为负数");
                }
                if (value == _anBu) return;
                _anBu = decimal.Truncate(value * 100) / 100; ;
                if (SellListModel != null) SellListModel.AnBu = decimal.Truncate(value * 100) / 100; ;
                OnPropertyChanged("AnBu");
                OnPropertyChanged("ProMoney");
            }
        }
       
        public decimal LieShouPrice
        {
            get
            {
                if (SellListModel != null) return SellListModel.LieShouPrice;
                return _lieShouPrice;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("列收金额不可为负数");
                }
                if (value == _lieShouPrice) return;
                if (SellListModel != null) SellListModel.LieShouPrice = decimal.Truncate(value * 100) / 100; ;
                _lieShouPrice = decimal.Truncate(value * 100) / 100; ;
                OnPropertyChanged("LieShouPrice");
            }
            
        }

        public string ProType {get
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
        }}
        public string Note { get; set; }
        

        public decimal OtherCash
        {
            get
            {
                if (SellListModel != null)
                {
                    return  Convert.ToDecimal(SellListModel.OtherCash);
     
                }
                return _otherCash;
            }
            set
            {
                if (value < 0)
                {
//                    this._errors.Add("OtherCash", "多收金额不可为负数");
                    throw new Exception("多收金额不可为负数");
                }
                if (SellListModel != null)
                {
                    SellListModel.OtherCash = decimal.Truncate(value * 100) / 100;
                    
                }
                if (value == _otherCash) return;
                _otherCash = decimal.Truncate(value * 100) / 100;
                
            OnPropertyChanged("ProMoney");
            OnPropertyChanged("OtherCash");
            }
        }

        public int SpecalId
        {
            get { return _specalId; }
            set
            {
                if (value == _specalId) return;
                _specalId = value;
                OnPropertyChanged("SpecalId");
            }
        }

        public decimal OffPrice
        {
            get { return _offPrice; }
            set
            {
                if (value == _offPrice) return;
                _offPrice = value;
                OnPropertyChanged("OffPrice");
            }
        }

        public decimal SpecalOffPrice
        {
            get { return _specalOffPrice; }
            set
            {
                if (value == _specalOffPrice) return;
                _specalOffPrice = value;
                OnPropertyChanged("SpecalOffPrice");
            }
        }


        public string ProClass
        {
            get { 
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

        public decimal offticketused
        {
            get { return _offticketused; }
            set
            {
                if (value == _offticketused) return;
                _offticketused = value;
                OnPropertyChanged("offticketused");
            }
        }

//        public decimal offprice
//        {
//            get { return _offprice; }
//            set
//            {
//                if (value == _offprice) return;
//                _offprice = value;
//                OnPropertyChanged("offprice");
//            }
//        }

        public string InListID
        {
            get { return _inListId; }
            set
            {
                if (value == _inListId) return;
                _inListId = value;
                OnPropertyChanged("InListID");
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
                OnPropertyChanged("ProClass");
                OnPropertyChanged("ProType");
                OnPropertyChanged("ProName");
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

        public string Unit
        {
            get { return _unit; }
            set
            {
                if (value == _unit) return;
                _unit = value;
                OnPropertyChanged("Unit");
            }
        }


        public decimal ProCount
        {
            get { return _proCount; }
            set
            {

                if (value == _proCount || !string.IsNullOrEmpty(_imei)) return;
                if (SellListModel != null && SellListModel.IsFree) return;

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
            }
        }

        public decimal ProPrice
        {
            get { if (this.SellListModel != null)
            {
                return this.SellListModel.ProPrice;
            }
                return _proPrice;
            }
            set
            {
                if (value == _proPrice) return;
                _proPrice = decimal.Truncate(value * 100) / 100;
                if (this.SellListModel != null)
                {
                    this.SellListModel.ProPrice = decimal.Truncate(value * 100) / 100;
                }
                OnPropertyChanged("ProPrice");
                OnPropertyChanged("ProMoney");
            }
        }


        public decimal ProMoney
        {
            get
            {
                decimal temp = (_proPrice - AnBu - _specalOffPrice - _offPrice - TicketUsed  - OtherOff ) < 0
                                   ? 0
                                   : (_proPrice - AnBu - _specalOffPrice - _offPrice - TicketUsed - OtherOff );

                return (temp + OtherCash) * _proCount;
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

        public string TicketNum
        {
            get { return _ticketNum; }
            set
            {
                if (value == _ticketNum) return;
                _ticketNum = value != null ? value.Trim().ToUpper() : null;
                OnPropertyChanged("TicketNum");
            }
        }

        public decimal TicketPrice
        {
            get { return _ticketPrice; }
            set
            {
                if (value == _ticketPrice) return;
                _ticketPrice = value;
                OnPropertyChanged("TicketPrice");
            }
        }

        public List<VIP_OffList> OffLists
        {
            get { return _offLists; }
            set
            {
                if (Equals(value, _offLists)) return;
                _offLists = value;
                OnPropertyChanged("OffLists");
            }
        }

        public int SelectedOffId
        {
            get { return SelectedOffID; }
            set
            {
                if (value == SelectedOffID) return;
                
                SelectedOffID = value;
                try
                {
                    var off = _offLists.First(p => p.ID == SelectedOffID).VIP_ProOffList.First(p => p.ProID == ProID);
                    _offPrice = _proPrice*(1 - off.Rate) + off.OffMoney;
                    //_offPrice = _offLists.First(p => p.ID == SelectedOffID).VIP_ProOffList.First(p=>p.ProID==ProID).OffMoney;
                }
                catch
                {
                    _offPrice = 0;
                }
                if (SellListModel != null)
                {
                    SellListModel.OffID = value;
                    SellListModel.OffPrice = _offPrice;
                }
                OnPropertyChanged("SelectedOffId");
                OnPropertyChanged("OffPrice");
                OnPropertyChanged("ProMoney");
            }
        }

        public Pro_SellListInfo SellListModel
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

        public bool IsOK { get; set; }
//        public string Error
//        {
//            get { return string.Empty; }
//        }
//
//        public string this[string columnName]
//        {
//            get
//            {
//                if (columnName=="OtherCash")
//                    return OtherCash<0?"多收金额不能为负":string.Empty;
//                return string.Empty;
//            }
//        }
    }
}
