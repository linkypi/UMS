using System;
using System.Net;

namespace UserMS
{
    class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 5 * 1000;
            return w;
        }
    }
}