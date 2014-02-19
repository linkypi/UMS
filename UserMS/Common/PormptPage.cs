using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace UserMS.Common
{
    public class PormptPage
    {
        public static bool PormptMessage(string Msg, string title)
        {
            if (MessageBoxResult.OK == MessageBox.Show(System.Windows.Application.Current.MainWindow,Msg, title, MessageBoxButton.OKCancel))
                return true;
            else
                return false;
        }
        public static bool isNumeric(string StringNum)
        {
            //通过正则表达式来判断是否为手机号码
            if (Regex.IsMatch(StringNum, @"^\d{11}$"))
                return true;
            return false;
        }
        public static bool IsMax(string strTitle, int length)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = strTitle.ToCharArray();
            string sb = "";
            int nLength = 0;
            for (int i = 0; i < stringChar.Length; i++)
            {
              
                    //判断是否超过要求长度 
                    if (nLength >= length * 2 )
                    {
                        return false;
                    }
                    //如果为汉字  
                    if (regex.IsMatch((stringChar[i]).ToString()))
                    {
                        sb += stringChar[i];
                        nLength += 2;
                    }
                    else
                    {
                        //如果为全角字母 
                        if (!GetLength(stringChar[i].ToString()))
                        {
                            sb += stringChar[i];
                            nLength += 2;
                        }
                        //小写字母  
                        else
                        {
                            sb += stringChar[i];
                            nLength = nLength + 1;
                        }
                    }
                }        
          // return sb.ToString();
            return true;
        }
        /// <summary>
        /// 判断全角或者半角
        /// </summary>
        /// <param name="StringNum"></param>
        /// <returns></returns>
        public static bool GetLength(string checkString)
        {
            //判断是否属半角
            if (checkString.Length == Encoding.Default.GetByteCount(checkString))
            {
                return true;
            }
            else
            {
                return false;
            }
            //属于全角
            //if (2 * checkString.Length == Encoding.Default.GetByteCount(checkString))
            //{ 
            //    return true; 
            //} 
            //else 
            //{ 
            //    return false;
            //} 
        }
        public static bool isVaule(string StringNum)
        {
            try
            {
                double b = Convert.ToDouble(StringNum);
                if (b < 0)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
