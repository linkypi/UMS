using System;
using System.Linq;
using UserMS.API;

namespace UserMS.Model
{
    public class NewSellListInfoGridModel
    {
        public string ProID
        {
            get
            {
                if (selllist == null)
                {
                    return null;
                }
                else
                {
                    return selllist.ProID;
                }
            }
        }
        public string ProFormat
        {
    get
    {
        if (selllist == null)
        {
            return null;
        }
        else
        {
            return Store.ProInfo.First(p => p.ProID == this.ProID).ProFormat;
        }
    }
    
            
        }
        public string Note
        {

    get
    {
        if (selllist == null)
        {
            return null;
        }
        else
        {
           
                return selllist.Note;
            
        }
    }
            
    }

        public string ProType
        {
            get
            {
                if (selllist == null)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return
                            Store.ProTypeInfo.First(
                                d => d.TypeID == Store.ProInfo.First(p => p.ProID == selllist.ProID).Pro_TypeID)
                                 .TypeName;
                    }
                    catch (Exception)
                    {
                        return null;
                        throw;
                    }
                }
            }
        }
        public string ProClass
        {
            get
            {
                if (selllist == null)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return
                            Store.ProClassInfo.First(
                                d => d.ClassID == Store.ProInfo.First(p => p.ProID == selllist.ProID).Pro_ClassID)
                                 .ClassName;
                    }
                    catch (Exception)
                    {
                        return null;
                        throw;
                    }
                }
            }
        }

        public string ProName {
            get
            {
                if (selllist == null)
                {
                    return null;
                }
                else
                {
                    return Store.ProInfo.First(p => p.ProID == this.ProID).ProName;
                }
            }
        }
        public decimal ProCount{
            get
            {
                if (selllist == null)
                {
                    return 0;
                }
                else
                {
                    return selllist.ProCount;
                }
            }
        }
        public string IMEI {
            get
            {
                if (selllist == null)
                {
                    return null;
                }
                else
                {
                    return selllist.IMEI;
                }
            }
        }
        public string ChargePhone {
            get
            {
                if (selllist == null)
                {
                    return null;
                }
                else
                {
                    return selllist.ChargePhoneNum;
                }
            }
        }
        public string SellType
        {
            get
            {
                if (selllist == null)
                {
                    return null;
                }
                else
                {
                    var query = Store.SellTypes.Where(p => p.ID == selllist.SellType);
                    if (query.Any())
                    {
                        return query.First().Name;
                    }
                    else
                    {
                        return null;
                    }
                   
                   
                }
            }
        }
        public string TicketID
        {
            get
            {
                if (selllist==null)
                {
                    return null;
                }
                else
                {
                    return selllist.TicketID;
                }
            }
        }
        public decimal TicketPrice
        {
            get
            {
                if (selllist == null)
                {
                    return 0;
                }
                else
                {
                    return selllist.CashTicket;
                }
            }
        }

    
        public Pro_SellListInfo_Temp selllist { get; set; }
    }

    }
