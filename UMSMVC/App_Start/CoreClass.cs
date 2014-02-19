using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using JsonFx.Json;
using System.Web.Mvc;
using System.Web.Http.Controllers;
using System.Web.Http;



namespace UMSMVC
{
    public static class Logger
    {


        public static string filename;
        public static System.IO.StreamWriter ws;
        public static System.IO.FileStream objFileStream;
       
        public static void writelog(string s1, string ip)
        {
            return;
            string nowfilename = DateTime.Now.ToString("yyyyMMddHH");
            if (filename != nowfilename || ws == null)
            {
                filename = nowfilename;
                string filepath = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "logs"), filename + ".log");
                if (ws != null)
                {

                    ws.Close();
                    ws.Dispose();
                    objFileStream.Close();
                    objFileStream.Dispose();
                }

                objFileStream = new System.IO.FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.None);
                ws = new System.IO.StreamWriter(objFileStream, System.Text.Encoding.GetEncoding("gb2312"));

            }

            ws.WriteLine(s1 + " " + SignClass.GetUnixTime(DateTime.Now) + " " + ip);
            ws.Flush();
            objFileStream.Flush();

        }


    }
    public partial class JsonAPI :  ApiController
    {

        public Model.UMSDB context = new Model.UMSDB();
        public JsonWriter writer = new JsonWriter();
        public ResClass webreturn = new ResClass();
        public List<String> RequireArgs = new List<String>();
        //public static  string LogPath=Server.MapPath("./logs/");
        
        
        public bool CheckArgs()
        {
            webreturn.JsonReturnDict["error"] = 0;
            webreturn.JsonReturnDict["msg"] = "";

            return true;
            if (SignClass.CheckArgs(System.Web.HttpContext.Current.Request.QueryString))
            {

                if (SignClass.CheckSign(System.Web.HttpContext.Current.Request.QueryString))
                {
                    try
                    {

                        foreach (string s in RequireArgs)
                        {
                            if (String.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString[s]))
                            {
                                throw new Exception();
                            }



                        }
                        if (System.Math.Abs(SignClass.GetUnixTime(DateTime.Now) - long.Parse(System.Web.HttpContext.Current.Request.QueryString["timestamp"]) / 1000) > 10 * 60)
                        {
                            webreturn.JsonReturnDict["error"] = -3;
                            webreturn.JsonReturnDict["msg"] = "时间戳错误";
                            return false;

                        }

                        Logger.writelog(System.Web.HttpContext.Current.Request.Url.OriginalString, System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                        return true;


                    }
                    catch (System.Exception ex)
                    {
                        webreturn.JsonReturnDict["error"] = -1;
                        webreturn.JsonReturnDict["msg"] = "缺少必备参数";
                        return false;
                    }



                }
                else
                {
                    webreturn.JsonReturnDict["error"] = -1;
                    webreturn.JsonReturnDict["msg"] = "签名错误";
                    return false;

                }
            }
            else
            {

                webreturn.JsonReturnDict["error"] = -1;
                webreturn.JsonReturnDict["msg"] = "缺少必备参数";
                return false;
            }
            return false;
        }
        public void OutputJson()
        {



            System.Web.HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            System.Web.HttpContext.Current.Response.ContentType = "application/json";
            var json = writer.Write(webreturn.JsonReturnDict);
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["callback"]))
            {
                json = string.Format("{0}({1})", System.Web.HttpContext.Current.Request.QueryString["callback"], json);
            }
            else if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["$callback"]))
            {
                json = string.Format("{0}({1})", System.Web.HttpContext.Current.Request.QueryString["$callback"], json);
            }
            System.Web.HttpContext.Current.Response.Write(json);
          
            System.Web.HttpContext.Current.Response.End();

        }

     
    }

    public class ResClass
    {
        public Dictionary<string, object> JsonReturnDict = new Dictionary<string, object>();
        public ResClass()
        {
            JsonReturnDict.Add("response", "");
            JsonReturnDict.Add("error", 0);
            JsonReturnDict.Add("msg", "");



        }

    }


    //public string v { get; set; }
    //public SafeUrlEncoder(string s)
    //{
    //    this.v = s;
    //}



    public static class SignClass
    {
        static string SignKey = "394578347696734954332017";

        public static bool CheckArgs(System.Collections.Specialized.NameValueCollection QueryString)
        {

            //return true;
            try
            {
                if (QueryString["sig"] != null && QueryString["timestamp"] != null)
                {
                    return true;
                   
                }
                else
                {

                    return false;
                }




            }
            catch (System.Exception ex)
            {

                return false;
            }


        }
        //public static string safeurlencode(string s)
        //{
        //    return HttpServerUtility.UrlEncode(s);
        //}

        public static bool CheckSign(System.Collections.Specialized.NameValueCollection QueryString)
        {
            //return true;
            try
            {

                List<string> QueryKeys = QueryString.AllKeys.ToList();
                QueryKeys.Sort();
                string signstring = "";
                //SafeUrlEncoder Encoder = new SafeUrlEncoder() ;
                foreach (string key in QueryKeys)
                {
                    if (key != "sig")
                    {



                        signstring = signstring + key + UrlEncodeUpperCase(QueryString[key]);

                    }


                }
                signstring = signstring + SignKey;
                return (QueryString["sig"] == CreateMD5Hash(signstring));
            }
            catch (System.Exception ex)
            {
                return false;
            }


        }
        public static string UrlEncodeUpperCase(string value)
        {

            value = HttpUtility.UrlEncode(value);
            value = value.Replace("+", "%20");
            value = value.Replace("!", "%21");
            value = value.Replace("'", "%27");
            value = value.Replace("(", "%28");
            value = value.Replace(")", "%29");
            return Regex.Replace(value, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
        }

        public static string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }
        private static DateTime UNIX_EPOCH =
  new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static long GetUnixTime(DateTime targetTime)
        {
            // UTC時間に変換
            targetTime = targetTime.ToUniversalTime();

            // UNIXエポックからの経過時間を取得
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;

            // 経過秒数に変換
            return (long)elapsedTime.TotalSeconds;
        }

    }
}