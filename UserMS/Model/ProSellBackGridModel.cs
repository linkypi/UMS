using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UserMS.Model
{
    public class GridViewRowSytleSelector:StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ProSellBackGridModel)
            {
                ProSellBackGridModel model = item as ProSellBackGridModel;

                if (model.Status == ProSellBackGridModel.SellListStatus.Back)
                {
                    return RedStyle;
                }
                if (model.Status == ProSellBackGridModel.SellListStatus.New)
                {
                    return GreenStyle;
                }
                return null;


            }
            if (item is ProSellGridModel)
            {
                ProSellGridModel model = item as ProSellGridModel;
                
                if (!model.IsOK)
                {
                    return TomatoStyle;
                }
            }

            if (item is View_SellOffAduitInfo)
            {
                View_SellOffAduitInfo model = item as View_SellOffAduitInfo;
                if (model.IsAduited == "N")
                {
                    return RedStyle;
                }
            }

            return null;
        }
        public Style RedStyle { get; set; }
        public Style GreenStyle { get; set; }
        public Style TomatoStyle { get; set; }
    }

    //public class ProSellOffAduitModel: 

    public class ProSellBackGridModel : ProSellGridModel
    {
        private decimal _otherOff;
        public new decimal OtherOff
        {
            get
            {
                if (SellListModel != null) return SellListModel.OtherOff;
                return _otherOff;
            }
            set
            {
                if (value < 0) throw new Exception("不允许负数");
                if (BackCount > 0) return;
                if (SellListModel != null && SellListModel.ID > 0) return;
                if (SellListModel != null) SellListModel.OtherOff = value;
                _otherOff = value;


                OnPropertyChanged("OtherOff");
                OnPropertyChanged("ProMoney");
            }
        }

        private decimal _otherCash;

        public new decimal OtherCash
        {
            get
            {
                if (SellListModel != null)
                {
                    return Convert.ToDecimal(SellListModel.OtherCash);

                }
                return _otherCash;
            }
            set
            {
                if (BackCount > 0) return;
                if (SellListModel != null && SellListModel.ID > 0) return;
                if (value < 0)
                {
                    //                    this._errors.Add("OtherCash", "多收金额不可为负数");
                    throw new Exception("多收金额不可为负数");
                }
                if (SellListModel != null)
                {
                    SellListModel.OtherCash = decimal.Truncate(value*100)/100;

                }
                if (value == _otherCash) return;
                _otherCash = decimal.Truncate(value*100)/100;

                OnPropertyChanged("ProMoney");
                OnPropertyChanged("OtherCash");
            }
        }

        private decimal _backCount;

        public decimal BackCount
        {
            get { return _backCount; }
            set
            {
                if (value == _backCount) return;
                if (value > ProCount) throw new Exception("不能大于销售数");
                if (this.ProID != null)
                {
                    try
                    {
                        if (Store.ProInfo.First(info => info.ProID == this.ProID).ISdecimals)
                        {
                            _backCount = decimal.Truncate(value*100)/100;
                        }
                        else
                        {
                            _backCount = decimal.Truncate(value);

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

                OnPropertyChanged("BackCount");
            }
        }

        public decimal _backmoney;

        public decimal BackMoney
        {
            get { return _backmoney; }
            set
            {
                if (value == _backmoney)
                {
                    return;
                }
                else
                {
                    _backmoney = value;
                    OnPropertyChanged("BackMoney");
                }



            }
        }

        public decimal BackPrice
        {
            get
            {
                try
                {
                    decimal temp = _backmoney - this.AnBu - this.OffPrice - this.SellListModel.OffSepecialPrice - this.SellListModel.CashTicket +
                                   this.OtherCash - this.SellListModel.WholeSaleOffPrice - this.OtherOff;
                    if (temp < 0) temp = 0;

                    return (temp-this.SellListModel.LieShouPrice)*_backCount;
                }

                catch (
                    Exception)
                {
                    return (_backmoney - this.OffPrice + this.OtherCash-this.OtherOff) * _backCount;

                }
            }
        }

        public enum SellListStatus  
        {
            New,Old,Back
        }
        public SellListStatus Status { get; set; }
        
    }
}