using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace DataRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            DAL.Pro_IMEI_Utils u = new Pro_IMEI_Utils();
            u.updateAreaAge(561300);
        }
    }
}
