using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    public class OffChangeModel
    {
        #region 商品优惠视图转换
        public static void ChangeModel(List<API.View_VIP_OffList> OffList, RadGridView DataGrid)
        {
            var queryOffList = (from b in OffList
                                select new
                                {
                                    b.OffID,
                                    b.OffName,
                                    b.StartDate,
                                    b.StartTime,
                                    b.EndDate,
                                    b.EndTime,
                                    b.VIPTicketMaxCount,
                                    b.OffUpdUser,

                                    b.OffRate,
                                    b.OffPoint,
                                    b.OffMoney,
                                    b.SendPoint,
                                    b.Note
                                }).Distinct();
            List<API.View_VIP_OffList> NewOffList = new List<API.View_VIP_OffList>();
            foreach (var OffListItem in queryOffList)
            {
                API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();
                View_OffList.OffID = OffListItem.OffID;
                View_OffList.OffName = OffListItem.OffName;
                View_OffList.StartDate = OffListItem.StartDate;
                View_OffList.EndDate = OffListItem.EndDate;
                View_OffList.StartTime = OffListItem.StartTime;
                View_OffList.EndTime = OffListItem.EndTime;
                View_OffList.VIPTicketMaxCount = OffListItem.VIPTicketMaxCount;
                View_OffList.OffUpdUser = OffListItem.OffUpdUser;

                View_OffList.OffRate = OffListItem.OffRate;
                View_OffList.OffPoint = OffListItem.OffPoint;
                View_OffList.OffMoney = OffListItem.OffMoney;
                View_OffList.SendPoint = OffListItem.SendPoint;
                View_OffList.Note = OffListItem.Note;
                NewOffList.Add(View_OffList);
            }
            DataGrid.ItemsSource = NewOffList;
            DataGrid.Rebind();
        }
        public static void ChangeModel(List<API.View_VIP_OffList> OffList, RadGridView GridVIPType, RadGridView GridVIP, RadGridView GridPro, RadGridView GridHall)
        {
            if (OffList == null)
                return;
            var queryVIPType = (from b in OffList
                             //   where b.OffID == OffID
                                select new
                                {

                                    b.VIPTypeName,
                                }).Distinct();
            List<API.View_VIP_OffList> NewOffList;
            if (queryVIPType.Count() > 0)
            {
                NewOffList = new List<API.View_VIP_OffList>();
                foreach (var OffListItem in queryVIPType)
                {
                    if (OffListItem.VIPTypeName == null)
                        break;
                    API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();
                    View_OffList.VIPTypeName = OffListItem.VIPTypeName;
                    NewOffList.Add(View_OffList);
                }
                GridVIPType.ItemsSource = NewOffList;
            }
            var queryVIP = (from b in OffList
                          //  where b.OffID == OffID
                            select new
                            {
                                b.VIPID,
                                b.OffVIPNote,
                                b.IMEI,
                                b.MemberName
                            }).Distinct();
            if (queryVIP.Count() > 0)
            {
                NewOffList = new List<API.View_VIP_OffList>();
                foreach (var OffListItem in queryVIP)
                {
                    if (OffListItem.VIPID == null)
                        break;
                    API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();
                    View_OffList.IMEI = OffListItem.IMEI;
                    View_OffList.VIPID = OffListItem.VIPID;
                    View_OffList.MemberName = OffListItem.MemberName;
                    NewOffList.Add(View_OffList);
                }
                GridVIP.ItemsSource = NewOffList;
            }
            var queryPro = (from b in OffList
                          //  where b.OffID == OffID
                            select new
                            {

                                b.ClassName,
                                b.TypeName,
                                b.ProName,
                                b.SellTypeID,
                                b.Name,
                                b.Price,
                                b.ProFormat,
                                b.Rate,
                                b.Point,
                                b.ProOffMoney,
                                b.Salary,
                                b.AfterOffPrice
                            }).Distinct();
            if (queryPro.Count() > 0)
            {
                NewOffList = new List<API.View_VIP_OffList>();
                foreach (var OffListItem in queryPro)
                {
                    API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();

                    View_OffList.ClassName = OffListItem.ClassName;
                    View_OffList.TypeName = OffListItem.TypeName;
                    View_OffList.ProName = OffListItem.ProName;
                    View_OffList.SellTypeID = OffListItem.SellTypeID;
                    View_OffList.Name = OffListItem.Name;
                    View_OffList.Price = OffListItem.Price;
                    View_OffList.ProFormat = OffListItem.ProFormat;
                    View_OffList.Rate = OffListItem.Rate;
                    View_OffList.ProOffMoney = OffListItem.ProOffMoney;
                    View_OffList.Point = OffListItem.Point;
                    View_OffList.Salary = OffListItem.Salary;
                    View_OffList.AfterOffPrice = OffListItem.AfterOffPrice;
                    NewOffList.Add(View_OffList);
                }
                GridPro.ItemsSource = NewOffList;
            }
            var queryHall = (from b in OffList
                           //  where b.OffID == OffID
                             select new
                             {
                                 b.OffHallID,
                                 b.HallName
                             }).Distinct();
            if (queryHall.Count() > 0)
            {
                NewOffList = new List<API.View_VIP_OffList>();
                foreach (var OffListItem in queryHall)
                {
                    API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();

                    View_OffList.OffHallID = OffListItem.OffHallID;
                    View_OffList.HallName = OffListItem.HallName;
                    NewOffList.Add(View_OffList);
                }
                GridHall.ItemsSource = NewOffList;
            }
        }
        #endregion


        #region 商品视图转换
        public static void ChangeModel(List<API.View_ProInfo> ProList, RadGridView DataGrid)
        {
            var queryOffList = (from b in ProList
                                select new
                                {
                                    b.ProID,
                                    b.ClassName,
                                    b.TypeName,
                                    b.ProName,
                                    b.HasNeedIMEI,
                                    b.HasService,
                                    b.HasDecimals,
                                    b.ProFormat,
                                    b.Note,

                                    b.PrintName,
                                    b.SepDate,
                                    b.BeforeRate,
                                    b.BeforeSep,
                                    b.AfterSep,
                                    b.AfterRate,
                                    b.TicketLevel,
                                    b.BeforeTicket,
                                    b.AfterTicket,
                                    b.HasNeedMoreorLess

                                }).Distinct();
            List<API.View_ProInfo> NewProList = new List<API.View_ProInfo>();
            foreach (var ProListItem in queryOffList)
            {
                API.View_ProInfo View_ProList = new API.View_ProInfo();
                View_ProList.ProID = ProListItem.ProID;
                View_ProList.ClassName = ProListItem.ClassName;
                View_ProList.TypeName = ProListItem.TypeName;
                View_ProList.ProName = ProListItem.ProName;
                View_ProList.HasNeedIMEI = ProListItem.HasNeedIMEI;
                View_ProList.HasService = ProListItem.HasService;
                View_ProList.HasDecimals = ProListItem.HasDecimals;
                View_ProList.ProFormat = ProListItem.ProFormat;
                View_ProList.Note = ProListItem.Note;


                View_ProList.PrintName = ProListItem.PrintName;
                View_ProList.SepDate = ProListItem.SepDate;
                View_ProList.BeforeRate = ProListItem.BeforeRate;
                View_ProList.BeforeSep = ProListItem.BeforeSep;
                View_ProList.AfterSep = ProListItem.AfterSep;
                View_ProList.AfterRate = ProListItem.AfterRate;
                View_ProList.TicketLevel = ProListItem.TicketLevel;
                View_ProList.BeforeTicket = ProListItem.BeforeTicket;
                View_ProList.AfterTicket = ProListItem.AfterTicket;
                View_ProList.HasNeedMoreorLess = ProListItem.HasNeedMoreorLess;
                NewProList.Add(View_ProList);
            }
            DataGrid.ItemsSource = NewProList;
            DataGrid.Rebind();
        }
        //public static void ChangeModel(List<API.View_VIP_OffList> OffList, int OffID, RadGridView GridVIPType, RadGridView GridVIP, RadGridView GridPro, RadGridView GridHall)
        //{
        //    if (OffList == null)
        //        return;
        //    var queryVIPType = (from b in OffList
        //                        where b.OffID == OffID
        //                        select new
        //                        {

        //                            b.VIPTypeName,
        //                        }).Distinct();
        //    List<API.View_VIP_OffList> NewOffList;
        //    if (queryVIPType.Count() > 0)
        //    {
        //        NewOffList = new List<API.View_VIP_OffList>();
        //        foreach (var OffListItem in queryVIPType)
        //        {
        //            if (OffListItem.VIPTypeName == null)
        //                break;
        //            API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();
        //            View_OffList.VIPTypeName = OffListItem.VIPTypeName;
        //            NewOffList.Add(View_OffList);
        //        }
        //        GridVIPType.ItemsSource = NewOffList;
        //    }
        //    var queryVIP = (from b in OffList
        //                    where b.OffID == OffID
        //                    select new
        //                    {
        //                        b.VIPID,
        //                        b.OffVIPNote,
        //                        b.IMEI,
        //                        b.MemberName
        //                    }).Distinct();
        //    if (queryVIP.Count() > 0)
        //    {
        //        NewOffList = new List<API.View_VIP_OffList>();
        //        foreach (var OffListItem in queryVIP)
        //        {
        //            if (OffListItem.VIPID == null)
        //                break;
        //            API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();
        //            View_OffList.IMEI = OffListItem.IMEI;
        //            View_OffList.VIPID = OffListItem.VIPID;
        //            View_OffList.MemberName = OffListItem.MemberName;
        //            NewOffList.Add(View_OffList);
        //        }
        //        GridVIP.ItemsSource = NewOffList;
        //    }
        //    var queryPro = (from b in OffList
        //                    where b.OffID == OffID
        //                    select new
        //                    {

        //                        b.ClassName,
        //                        b.TypeName,
        //                        b.ProName,
        //                        b.SellTypeID,
        //                        b.Name,
        //                        b.Price,
        //                        b.ProFormat,
        //                        b.Rate,
        //                        b.Point,
        //                        b.ProOffMoney,
        //                        b.Salary,
        //                        b.AfterOffPrice
        //                    }).Distinct();
        //    if (queryPro.Count() > 0)
        //    {
        //        NewOffList = new List<API.View_VIP_OffList>();
        //        foreach (var OffListItem in queryPro)
        //        {
        //            API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();

        //            View_OffList.ClassName = OffListItem.ClassName;
        //            View_OffList.TypeName = OffListItem.TypeName;
        //            View_OffList.ProName = OffListItem.ProName;
        //            View_OffList.SellTypeID = OffListItem.SellTypeID;
        //            View_OffList.Name = OffListItem.Name;
        //            View_OffList.Price = OffListItem.Price;
        //            View_OffList.ProFormat = OffListItem.ProFormat;
        //            View_OffList.Rate = OffListItem.Rate;
        //            View_OffList.ProOffMoney = OffListItem.ProOffMoney;
        //            View_OffList.Point = OffListItem.Point;
        //            View_OffList.Salary = OffListItem.Salary;
        //            View_OffList.AfterOffPrice = OffListItem.AfterOffPrice;
        //            NewOffList.Add(View_OffList);
        //        }
        //        GridPro.ItemsSource = NewOffList;
        //    }
        //    var queryHall = (from b in OffList
        //                     where b.OffID == OffID
        //                     select new
        //                     {
        //                         b.OffHallID,
        //                         b.HallName
        //                     }).Distinct();
        //    if (queryHall.Count() > 0)
        //    {
        //        NewOffList = new List<API.View_VIP_OffList>();
        //        foreach (var OffListItem in queryHall)
        //        {
        //            API.View_VIP_OffList View_OffList = new API.View_VIP_OffList();

        //            View_OffList.OffHallID = OffListItem.OffHallID;
        //            View_OffList.HallName = OffListItem.HallName;
        //            NewOffList.Add(View_OffList);
        //        }
        //        GridHall.ItemsSource = NewOffList;
        //    }
        //}
        #endregion
    }
}
