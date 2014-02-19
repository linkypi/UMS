using System;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Controls.Map;
using UserMS.API;

namespace UserMS.API
{
    public partial class Pro_BillInfo_temp
    {
        public string ProName
        {
            get
            {
                if (string.IsNullOrEmpty(ProID))
                {
                    return null;
                }
                else
                {
                    var query = Store.ProInfo.Where(p => p.ProID == ProID);
                    if (query.Any())
                    {
                        return query.First().ProName;
                    }
                    return null;
                }
            }
        }
    }
    public partial class Pro_BillInfo
    {
        public string ProName
        {
            get
            {
                if (string.IsNullOrEmpty(ProID))
                {
                    return null;
                }
                else
                {
                    var query = Store.ProInfo.Where(p => p.ProID == ProID);
                    if (query.Any())
                    {
                        return query.First().ProName;
                    }
                    return null;
                }
            }
        }
    }

    public partial  class Pro_ProInfo
    {
        public string ProTypeName
        {
         get
         {
             if (Store.ProTypeInfo == null)
             {
                 return null;
             }
             else
             {
                 try
                 {
                     return Store.ProTypeInfo.First(info => info.TypeID == this.Pro_TypeID).TypeName;

                 }
                 catch (Exception)
                 {
                     return null;
                 }
             }
         }   
        }
        public string ProClassName
        {
            get
            {
                if (Store.ProClassInfo == null)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return Store.ProClassInfo.First(info => info.ClassID == this.Pro_ClassID).ClassName;
                    }
                    catch (Exception)
                    {
                        return null;
                       
                    }
                }
            }

        }
    
    
    
    }

    public partial class SMS_SignSendPayInfo
    {
        public string ProName
        {
            get
            {
                if (Store.ProInfo == null)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return Store.ProInfo.First(info => info.ProID == this.ProID).ProName;

                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
        }
    }

    public partial class Asset_UseInfo
    {
        public string ProFormat
        {
            get
            {
                if (string.IsNullOrEmpty(ProID))
                {
                    return null;
                }
                else
                {
                    var query = Store.ProInfo.Where(p => p.ProID == ProID);
                    if (query.Any())
                    {
                        return query.First().ProFormat;
                    }
                    return null;
                }
            }
        }

        public string ProName
        {
            get
            {
                if (string.IsNullOrEmpty(ProID))
                {
                    return null;
                }
                else
                {
                    var query = Store.ProInfo.Where(p => p.ProID == ProID);
                    if (query.Any())
                    {
                        return query.First().ProName;
                    }
                    return null;
                }
            }
        }

        public string AssetForm
        {
            get
            {
                if (string.IsNullOrEmpty(ProID))
                {
                    return null;
                }
                else
                {
                    var query = Store.ProInfo.Where(p => p.ProID == ProID);
                    if (query.Any())
                    {
                        return query.First().AssetFrom;
                    }
                    return null;
                }
            }
        }

        public int? AssetPeriod
        {
            get
            {
                if (string.IsNullOrEmpty(ProID))
                {
                    return null;
                }
                else
                {
                    var query = Store.ProInfo.Where(p => p.ProID == ProID);
                    if (query.Any())
                    {
                        return query.First().AssetPeriod;
                    }
                    return null;
                }
            }
        }
        public decimal? AssetRate
        {
            get
            {
                if (string.IsNullOrEmpty(ProID))
                {
                    return null;
                }
                else
                {
                    var query = Store.ProInfo.Where(p => p.ProID == ProID);
                    if (query.Any())
                    {
                        return query.First().AssetRate;
                    }
                    return null;
                }
            }
        }
        

        
        
    }

  public partial class Pro_AreaInfo
  {
      private static byte ConvertToByte(uint unsignedInteger)
      {
          return Convert.ToByte(unsignedInteger % 256);
      }

      public SolidColorBrush AreaColor
      {
      get
      {
          uint parsedColor = 0;
          if (string.IsNullOrEmpty(this.MapColor) ||
                !uint.TryParse(this.MapColor, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out parsedColor))
          {
              return new SolidColorBrush() {Color = new Color() {A = 128, R = 0, G = 0, B = 0}};
          }

          return new SolidColorBrush(Color.FromArgb(
              ConvertToByte(parsedColor >> 24),
              ConvertToByte(parsedColor >> 16),
              ConvertToByte(parsedColor >> 8),
              ConvertToByte(parsedColor)));
      }
      }

      public double? CenterLongitude
      {
          get
          {
              if (MapPointLists == null || MapPointLists.Count==0)
              {
                  return null;
              }
              return MapPointLists.Average(p => p.Longitude);

          }
      }

      public double? CenterLatitude
      {
          get
          {
              if (MapPointLists == null || MapPointLists.Count == 0)
              {
                  return null;
              }
              return MapPointLists.Average(p => p.Latitude);

          }
      }

      public Location CaptionLocation
      {
          
          get
          {
              if (MapPointLists == null || MapPointLists.Count == 0)
              {
                  return new Location();
              }
              else
              {
                  return new Location(MapPointLists.Average(p => p.Latitude),MapPointLists.Average(p => p.Longitude));
              }
          }
      }
      public LocationCollection MapPointLists
      {
          get
          {
              if (PointList == null)
              {
                  return null;
              }
			  LocationCollection loc=new LocationCollection();
              foreach (var locationInfo in PointList)
              {
				 loc.Add(new Location(locationInfo.Latitude,locationInfo.Longitude));
              }
              return loc;

          }
      }
  }
}

namespace UserMS.ReportService
{
    public partial class Chart_MapReport
    {
        public decimal? ReportValue
        {
            get
            {
                if (this.AsPrice) return this.SellPrice;
                else return this.Sells;

            }
        }
    }




    public partial class Report_Profit
    {
        public decimal 总利润
        {
            get
            {
                return Convert.ToDecimal(this.数量)*Convert.ToDecimal(this.利润);
            }
        }
    }
    public partial class Report_Profit2
    {
        public decimal 总利润
        {
            get
            {
                return Convert.ToDecimal(this.数量) * Convert.ToDecimal(this.利润);
            }
        }
    }

}