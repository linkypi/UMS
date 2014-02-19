using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Pagination
    {
       private int MenthodID;
	    public Pagination()
	    {
		    this.MenthodID = 0;
	    }

        public Pagination(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageSize">多少条为一页</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="id"></param>
        /// <param name="Sign"></param>
        /// <returns></returns>
        public Model.WebReturn Paging(Model.Sys_UserInfo user,int pageIndex, int pageSize, int Sign)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    if (Sign==1)
                    {
                        int PageCount=0;
                        if (pageIndex == 0)
                        {
                            var queryCount = from b in lqh.Umsdb.Pro_HallInfo
                                        select b;
                            PageCount = queryCount.Count();       
                        }
                        var query =(from b in lqh.Umsdb.Pro_HallInfo
                                    select b).Skip(pageIndex).Take(pageSize);
                        ArrayList list = new ArrayList();
                        list.Add(PageCount);
                        if (query != null || query.Count() == 0)
                        {
                  
                            return new Model.WebReturn() { Obj = query.ToList(), ReturnValue = true, Message = "已获取", ArrList=list };
                        }
                    }
                    if (Sign == 2)
                    {
                        int PageCount = 0;
                        if (pageIndex == 0)
                        {
                            var queryCount = from b in lqh.Umsdb.Pro_AreaInfo
                                             select b;
                            PageCount = queryCount.Count();
                        }
                        var query = (from b in lqh.Umsdb.Pro_AreaInfo
                                     select b).Skip(pageIndex).Take(pageSize);
                        ArrayList list = new ArrayList();
                        list.Add(PageCount);
                        if (query != null || query.Count() == 0)
                        {

                            return new Model.WebReturn() { Obj = query.ToList(), ReturnValue = true, Message = "已获取", ArrList = list };
                        }
                    }
                    if (Sign == 3)
                    {
                        int PageCount = 0;
                        if (pageIndex == 0)
                        {
                            var queryCount = from b in lqh.Umsdb.Pro_LevelInfo
                                             select b;
                            PageCount = queryCount.Count();
                        }
                        var query = (from b in lqh.Umsdb.Pro_LevelInfo
                                     select b).Skip(pageIndex).Take(pageSize);
                        ArrayList list = new ArrayList();
                        list.Add(PageCount);
                        if (query != null || query.Count() == 0)
                        {

                            return new Model.WebReturn() { Obj = query.ToList(), ReturnValue = true, Message = "已获取", ArrList = list };
                        }
                    }

                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "更新失败" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "更新失败" };
                }
            }
        }
    }
}
