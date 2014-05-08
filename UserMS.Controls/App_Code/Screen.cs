using System;
using System.Windows;
using System.Collections.Generic;

namespace UserMS
{
    public static class Screen
    {
        public static double Width
        {
            get { return SystemParameters.WorkArea.Width; }
        }

        public static double Height
        {
            get { return SystemParameters.WorkArea.Height; }
        }
    }
}
