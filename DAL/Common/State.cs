using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
   public enum State
    {
       /// <summary>
       /// 调拨中
       /// </summary>
       Out = 512,
       /// <summary>
       /// 借贷中
       /// </summary>
       Borrow = 256,
       /// <summary>
       /// 已归还
       /// </summary>
       Return = 128,
       /// <summary>
       /// 送修中
       /// </summary>
       Repair = 64,
       VIP = 32,
       /// <summary>
       /// 已销售
       /// </summary>
       Sell = 16,
       Audit =8,
       Assest=4,
       /// <summary>
       /// 备机中
       /// </summary>
       BackUp = 2,

       /// <summary>
       /// 已用做维修配件
       /// </summary>
       PeiJian=1
    }
}
