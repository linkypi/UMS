using System;
using System.Linq;
using System.Net.Configuration;
using Model;

namespace DAL
{
    public class Rules_AllCurrentRulesInfo
    {
         private int MethodID;

        public Rules_AllCurrentRulesInfo()
        {
            this.MethodID = 0;
        }

        public Rules_AllCurrentRulesInfo(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        public Model.WebReturn GetList(Model.Sys_UserInfo user, string hallID,Model.Pro_SellListInfo model)
        {

            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    var query =
                        lqh.Umsdb.Rules_AllCurrentRulesInfo.Where(
                            p => p.StartDate < DateTime.Now & p.EndDate > DateTime.Now);
                    query = query.Where(p => p.SellType == model.SellType);
                    var promainquery = lqh.Umsdb.Pro_ProInfo.Where(p => p.ProID == model.ProID);
                    if (!promainquery.Any()) return new Model.WebReturn() { ReturnValue = false, Message = "商品错误" };
                    var promaininfo = promainquery.First();
                    query = query.Where(p => p.ProMainID == promaininfo.ProMainID);
                    query = query.Where(p => p.HallID == hallID);
                    return new WebReturn() {ReturnValue = true, Obj = query.ToList()};
                }
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
            }
        }
    }
}