using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface Sys_InitParentInfo
    {
        /// <summary>
        /// 将初始化数据返回ArrayList
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        ArrayList GetList(Sys_UserInfo user,DateTime dt);
    }
}
