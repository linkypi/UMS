using System;
using System.Windows.Media;

namespace UserMS
{
    public class MyColors
    {
        /// <summary>
        /// 将颜色代码转换成颜色,比如#FFBFBFBF
        /// GetColorFromString("FFBFBFBF")
        /// </summary>
        public static Color GetColorFromString(string color)
        {
            string alpha = color.Substring(0, 2);
            string red = color.Substring(2, 2);
            string green = color.Substring(4, 2);
            string blue = color.Substring(6, 2);

            byte alphaByte = Convert.ToByte(alpha, 16);
            byte redByte = Convert.ToByte(red, 16);
            byte greenByte = Convert.ToByte(green, 16);
            byte blueByte = Convert.ToByte(blue, 16);
            return Color.FromArgb(alphaByte, redByte, greenByte, blueByte);
        }
    }
}
