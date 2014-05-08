using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Objects;
using System.Data.Services.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Model
{
    partial class Sys_Option
    {
    }

 

    public partial class Pro_ProInfo
    {
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public bool IsTicketUsedable
        {
            get;
            set;
        }
    }

    public partial class Pro_SellListInfo
    {
        //����؛�ĕr��ǰ�_��Ҫ�R�r����һ��SellID
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public int? SellID_Temp { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute]
        public string OffName
        {
            get
            {
                if (this.VIP_OffList != null)
                {
                    return VIP_OffList.Name;
                }
                else
                {
                    return null;
                }
            }
            set { }
        }
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public string SpecalOffName
        {
            get
            {
                if (this.Pro_SellSpecalOffList != null)
                {
                    if (this.Pro_SellSpecalOffList.VIP_OffList != null)
                        return this.Pro_SellSpecalOffList.VIP_OffList.Name;

                }

                return null;


            }
            set { }
        }
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public int? SpecalOffId
        {
            get
            {
                if (this.Pro_SellSpecalOffList != null)
                {
                    return this.Pro_SellSpecalOffList.SpecalOffID;
                }
                else
                {
                    return null;
                }
            }
            set { }
        }

        [global::System.Runtime.Serialization.DataMemberAttribute]
        public Model.Pro_SellSpecalOffList SpecalOff
        {
            get
            {
                if (this.Pro_SellSpecalOffList != null)
                {
                    return new Pro_SellSpecalOffList()
                    {
                        BackID = this.Pro_SellSpecalOffList.BackID,
                        ID = this.Pro_SellSpecalOffList.ID,
                        Note = this.Pro_SellSpecalOffList.Note,
                        OffMoney = this.Pro_SellSpecalOffList.OffMoney,
                        SellID = this.Pro_SellSpecalOffList.SellID,
                        SpecalOffID = this.Pro_SellSpecalOffList.SpecalOffID
                    };
                }
                else
                {
                    return null;
                }




            }
            set { }
        }
    }

    public partial class Pro_SellListInfo_Temp
    {
        //    [global::System.Runtime.Serialization.DataMemberAttribute]
        public Pro_Sell_Yanbao_temp YanBaoModel
        {
            get { return this._Pro_Sell_Yanbao_temp.Entity; }
            set { }
        }
    }
    public partial class Pro_SellListInfo
    {
        //  [global::System.Runtime.Serialization.DataMemberAttribute]
        public Pro_Sell_Yanbao YanBaoModel
        {
            get { return this._Pro_Sell_Yanbao.Entity; }
            set { }
        }
    }
    public partial class Pro_AreaInfo
    {
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public List<LocationInfo> PointList
        {
            set { }
            get
            {
                if (Points == null)
                    return null;
                try
                {
                    var result = new List<LocationInfo>();
                    foreach (string s in Points.Trim().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var temp = s.Trim().Split(new char[] { ',' });
                        var l = new LocationInfo
                        {
                            Longitude = float.Parse(temp[0]),
                            Latitude = float.Parse(temp[1])
                        };
                        result.Add(l);

                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        //        [global::System.Runtime.Serialization.DataMemberAttribute]
        //        public float[][] Test
        //        {
        //            get
        //            {
        //                var result = new List<float[]>();
        //                foreach (string s in Points.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
        //                {
        //                    var temp = s.Split(new char[] { ',' });
        //                    var l = new float[]{
        //                        float.Parse(temp[0]),
        //                        float.Parse(temp[1])
        //                    };
        //                    result.Add(l);
        //
        //                }
        //                return result.ToArray();
        //            }
        //        }
    }

    public partial class View_VIPOffListAduitHeader 
    {
        private List<View_VIPOffListAduit> _View_VIPOffListAduit;
         [global::System.Runtime.Serialization.DataMemberAttribute]
        public List<View_VIPOffListAduit> View_VIPOffListAduit
        {
            get { return _View_VIPOffListAduit; }
            set { _View_VIPOffListAduit = value; }
        }
        
    }

    public partial class View_VIPOffListAduit 
    {

        private List<View_Package_GroupInfo> _View_Package_GroupInfo;
         [global::System.Runtime.Serialization.DataMemberAttribute]
        public List<View_Package_GroupInfo> View_Package_GroupInfo
        {
            get { return _View_Package_GroupInfo; }
            set { _View_Package_GroupInfo = value; }
        }
    
    }

    public partial class View_SMS_SignInfo
    {
         [global::System.Runtime.Serialization.DataMemberAttribute]
        public List<Model.View_SMS_SignSendPayInfo> View_SMS_SignSendPayInfo { get; set; }
    }

    public partial class View_ASPCurrentOrderPros
    {
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public List<Model.CHKModel> CHKList;
    }

    //Proc_SalaryReportDetailResult
    public partial class Proc_SalaryReportResult
    {
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public List<Proc_SalaryReportDetailResult> Children;
    }


    public partial class View_ASPReceiveInfo
    {
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public List<View_ASPRepairProgress> Children;
    }

    public partial class View_ASPRepairInfo
    {
        [global::System.Runtime.Serialization.DataMemberAttribute]
        public List<Model.ASP_Factory> Factorys;
    }
}
