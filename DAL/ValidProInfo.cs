using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ValidProInfo
    {
        #region 验证商品的有效性
        public static Model.WebReturn CheckProInfo(Model.Pro_ProInfo Pro,decimal ProCount)
        {
            bool NoError = true;
            string Message = "";
            if (ProCount <= 0)
            {
                NoError = false;
                Message += "数量不能小于等于0";
            }
            if (Pro == null)
            {
                NoError = false;
                Message += "商品不存在";
            }
            else
            {
                if (Pro.ISdecimals)
                {
                    if (ProCount != Decimal.Truncate(Convert.ToDecimal(ProCount * 100)) / 100)
                    {
                        NoError = false;

                       Message += "请保留2位小数" + Pro.ProName ;
                    }
                }
                else
                {
                    if (ProCount != Convert.ToInt32(ProCount))
                    {
                        NoError = false;
                        Message += "请保留正整数" + Pro.ProName ;
                    }
                }
            }

            return new Model.WebReturn { ReturnValue = NoError, Message = Message };
        }
        #endregion
    }
}
