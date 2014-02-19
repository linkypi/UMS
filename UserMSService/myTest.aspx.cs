using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Model;
using DAL;
namespace UserMSService
{
    public partial class myTest : System.Web.UI.Page
    {
        private static void gen(ref List<List<string>>m ,ref List<string> list, List<List<string>> s)
        {
            if (s.Count == 0)
            {
                m.Add(list.ToList());
                
                return;
            }
            List<string> n = s[0];
            s.Remove(n);
            foreach (var v in n)
            {
                list.Add(v);
                gen(ref m,ref list,s.ToList());
                list.RemoveAt(list.Count - 1);
                
                
            }
            
            //list=new List<string>();
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            //UserMsService d = new UserMsService();
            //d.Main(1, new object[2] { null, null });
            //XmlDocument d=new XmlDocument();
            //d.LoadXml(@"<MsgList><UnReadSMSSenderView 流水号='1197378' 用户名='gysoft88' 信息内容='15 32【国宇软件】' 接收号码='13688877060' 回执='0' 发送日期='2013-08-07T15:36:20.027' 发送状态='已发送'/><UnReadSMSSenderView 流水号='1197416' 用户名='gysoft88' 信息内容='15 37【国宇软件】' 接收号码='13688877296' 回执='0' 发送日期='2013-08-07T15:37:50.970' 发送状态='已发送'/></MsgList>");


            return;
            throw new NotImplementedException();
            using (LinQSqlHelper lqh=new LinQSqlHelper() )

            
            {
                foreach (var temp in lqh.Umsdb.Temp.GroupBy(temp => temp.优惠名称))
                {
                    
                    List<List<string>> proids=new List<List<string>>();
                    List<List<string>> idlist=new List<List<string>>();
                    foreach (var temp1 in temp)
                    {
                        Temp temp2 = temp1;
                        proids.Add(
                            lqh.Umsdb.Pro_ProInfo.Where(p => p.ProName.ToUpper() == temp2.商品型号.ToUpper()).Select(p => p.ProID).ToList());
                    }
                    List<string> blank=new List<string>();
                    gen(ref idlist, ref blank, proids);
                    int i = 1;
                    foreach (var list in idlist)
                    {
                        
                     Model.VIP_OffList off=new Model.VIP_OffList();
                        off.Name = temp.Key + i;
                        off.Note = temp.First().备注;
                        off.Type = 1;
                        off.UpdDate = DateTime.Now;
                        off.StartDate = DateTime.Parse(temp.First().开始时间);
                        off.EndDate = DateTime.Parse(temp.First().结束时间);
                        off.UpdUser = "1";
                        off.VIPTicketMaxCount = 99999;
                        foreach (var proid in list)
                        {
                            string proname = lqh.Umsdb.Pro_ProInfo.First(p => p.ProID == proid).ProName;
                            var t = temp.First(p => p.商品型号.ToUpper() == proname.ToUpper());
                            var prooff = new Model.VIP_ProOffList();
                            prooff.ProID = proid;
                            prooff.SellTypeID = 1;
                            prooff.ProCount = 1;
                            prooff.AfterOffPrice = Convert.ToDecimal(t.组合套餐价格);
                            prooff.Salary = Convert.ToDecimal(t.提成);
                            off.VIP_ProOffList.Add(prooff);
                        }
                        lqh.Umsdb.VIP_OffList.InsertOnSubmit(off);

                        i += 1;

                    }


                }
                
                lqh.Umsdb.SubmitChanges();
            }
        }
       
    }
}