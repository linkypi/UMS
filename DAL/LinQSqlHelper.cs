using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DAL
{
    public class LinQSqlHelper : System.IDisposable
    {
        private string conn = ConfigurationManager.AppSettings["ConnectionString"];

        public string Conn
        {
            get { return conn; }
            set { conn = value; }
        }

        private Model._UMSDB umsdb;

        public Model._UMSDB Umsdb
        {
            get { return umsdb; }
            set { umsdb = value; }
        }


        public LinQSqlHelper()
        {
            this.Umsdb = new Model._UMSDB(this.conn);
        }

        public LinQSqlHelper(string conn)
        {
            this.Umsdb = new Model._UMSDB(conn);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            //this.Dispose();
        }
        #endregion
    }
}

