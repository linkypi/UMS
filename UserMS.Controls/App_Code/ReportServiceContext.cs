using System;
using System.Configuration;
using System.Data.Services.Client;

namespace UserMS
{
    public class ReportServiceContext:UserMS.ReportService.Entities
    {

        public ReportServiceContext():base (new Uri(Store.ReportServiceURL))
        {
            this.SendingRequest+=ReportServiceContext_SendingRequest;
            this.Timeout = 300;
        }

        private void ReportServiceContext_SendingRequest(object sender, SendingRequestEventArgs e)
        {
            ((System.Net.HttpWebRequest)e.Request).AutomaticDecompression = (System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate);
            e.RequestHeaders.Add("X-UserName",System.Web.HttpUtility.UrlEncode(Store.LoginUserName));
            e.RequestHeaders.Add("X-Password",System.Web.HttpUtility.UrlEncode(Store.LoginUserPassword));
        }
    }
}
