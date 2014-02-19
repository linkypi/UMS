using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace Common
{
    public class MyWebClient
    {
        //The cookies will be here.
        private CookieContainer _cookies = new CookieContainer();

        //In case you need to clear the cookies
        public void ClearCookies()
        {
            _cookies = new CookieContainer();
        }

        public HtmlDocument GetPage(string url, Dictionary<string, string> data = null)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = _cookies;
            if (data != null)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                StringBuilder sb = new StringBuilder();
                foreach (var v in data)
                {
                    sb.Append(HttpUtility.UrlEncode(v.Key) + "=" + HttpUtility.UrlEncode(v.Value) + "&");
                }
                sb.Length -= 1;
                request.ContentLength = sb.Length;
                var req = request.GetRequestStream();
                var ascii = new ASCIIEncoding();
                var bytes = ascii.GetBytes(sb.ToString());
                req.Write(bytes, 0, bytes.Length);
                req.Close();
            }
            else
            {
                request.Method = "GET";

            }


            //Set more parameters here...
            //...

            //This is the important part.


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();

            //When you get the response from the website, the cookies will be stored
            //automatically in "_cookies".

            using (var reader = new StreamReader(stream))
            {
                string html = reader.ReadToEnd();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc;
            }
        }
    }

    public class IllegalModel
    {
        public string 管辖区域 { get; set; }
        public string 违章信息 { get; set; }
        public string 违章扣分 { get; set; }
        public string 罚款金额 { get; set; }

        public static List<IllegalModel> GetModels(string CName, string CID)
        {
            string loginurl = "http://202.96.188.81:8800/Login.aspx";
            string chaurl = "http://202.96.188.81:8800/Web/IllegalInfo.aspx?";
            MyWebClient webClient = new MyWebClient();
            HtmlDocument doc = webClient.GetPage(loginurl);
            /*    ScriptManager1:UpdatePanelLogin|Btn_Sub
__EVENTTARGET:
__EVENTARGUMENT:
__VIEWSTATE:/wEPDwUKLTY3NTAyMjIzMmRkun0W4ZXv0vfZC72T/W702pA4b88=
__EVENTVALIDATION:/wEWBALa6fq0CAK5g7vTCwK7zqrYDQLam6qxCUDCkeJBI8b18aGBP1mw4ZnHPSl5
txt_Name:jmcq
txt_Pwd:888
__ASYNCPOST:true
Btn_Sub:"*/

            string __EVENTTARGET = "";
            try
            {
                __EVENTTARGET =
                    doc.DocumentNode.SelectSingleNode("//*[@id=\"__EVENTTARGET\"]").Attributes["value"].Value;
            }
            catch (Exception)
            {
            }

            string __EVENTARGUMENT = "";
            try
            {
                __EVENTARGUMENT =
                    doc.DocumentNode.SelectSingleNode("//*[@id=\"__EVENTARGUMENT\"]").Attributes["value"].Value;
            }
            catch (Exception)
            {
            }
            string __VIEWSTATE = "";
            try
            {
                __VIEWSTATE =
                    doc.DocumentNode.SelectSingleNode("//*[@id=\"__VIEWSTATE\"]").Attributes["value"].Value;
            }
            catch (Exception)
            {
            }
            string __EVENTVALIDATION = "";
            try
            {
                __EVENTVALIDATION =
                    doc.DocumentNode.SelectSingleNode("//*[@id=\"__EVENTVALIDATION\"]").Attributes["value"].Value;
            }
            catch (Exception)
            {
            }
            try
            {
                var doc2 = webClient.GetPage(loginurl, new Dictionary<string, string>()
                {
                    {"__EVENTTARGET", __EVENTTARGET},
                    {"__EVENTARGUMENT", __EVENTARGUMENT},
                    {"__VIEWSTATE", __VIEWSTATE},
                    {"txt_Name", "jmcq"},
                    {"txt_Pwd", "888"},
                    {"__ASYNCPOST", "false"},
                    {"Btn_Sub", ""},
                    {"ScriptManager1", "UpdatePanelLogin|Btn_Sub"},
                    {"__EVENTVALIDATION", __EVENTVALIDATION},




                });
                if (!doc2.DocumentNode.InnerText.Contains("欢迎您"))
                {
                    return new List<IllegalModel>();
                }

                //state=NotVIP&YS=G&CP=粤T2E799&CJH=223342
                StringBuilder sb = new StringBuilder();
                sb.Append(chaurl);
                sb.Append("state=NotVIP&YS=G&");
                sb.Append("CP=" + HttpUtility.UrlEncode(CName.Trim()) + "&");
                sb.Append("CJH=" + HttpUtility.UrlEncode(CID.Trim()));
                var doc3 = webClient.GetPage(sb.ToString());
                var nodes = doc3.DocumentNode.SelectNodes("//*[@class=\"illegal\"]");
                if (nodes != null && nodes.Count > 0)
                {
                    StringBuilder sbs = new StringBuilder();
                    List<IllegalModel> list = new List<IllegalModel>();
                    foreach (var node in nodes)
                    {
                        node.SelectNodes("td");
                        list.Add(new IllegalModel()
                        {
                            管辖区域 = node.SelectNodes("td")[1].InnerText.Trim(),
                            违章信息 =

                                node.SelectNodes("td")[2].InnerText.Trim(),
                            违章扣分 =
                                node.SelectNodes("td")[3].InnerText.Trim(),
                            罚款金额 =
                                node.SelectNodes("td")[4].InnerText.Trim()
                        });





                    }
                    return list;

                }
                else
                {
                    var node = doc3.DocumentNode.SelectSingleNode("//*[@id=\"span_Msg\"]");
                    if (node != null)
                    {
                        throw new Exception(node.InnerText);

                    }
                    else
                    {
                        throw new Exception("查询错误");

                    }
                }



            }
            catch
            {
                throw new Exception("查询错误");
            }
        }

    }




}