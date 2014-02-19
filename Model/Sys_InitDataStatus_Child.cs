using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 初始化列表子类
    /// </summary>
    public class Sys_InitDataStatus_Child
    {
        private ArrayList _InitArray;

        public ArrayList InitArray
        {
            get { return _InitArray; }
            set { _InitArray = value; }
        }
    }
}
