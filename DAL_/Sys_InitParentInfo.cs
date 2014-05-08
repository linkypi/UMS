using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    
        public interface Sys_InitParentInfo
        {
            /// <summary>
            /// 将初始化数据返回ArrayList
            /// </summary>
            /// <param name="user"></param>
            /// <param name="dt"></param>
            /// <returns></returns>
            ArrayList GetList(Model.UserInfo user, DateTime dt);
        }
   
}
