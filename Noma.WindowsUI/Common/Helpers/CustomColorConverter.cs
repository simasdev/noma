using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Noma.WindowsUI.Common.Helpers
{
    public static class CustomColorConverter
    {
        public static int Get32BitArgbValue(int red, int green, int blue)
        {
            return System.Drawing.Color.FromArgb(red, green, blue).ToArgb();
        }

        public static Brush GetBrush(int argbValue)
        {
            byte[] bytes = BitConverter.GetBytes(argbValue);

            return new SolidColorBrush(Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]));
        }

        public static Color GetColor(int argbValue)
        {
            byte[] bytes = BitConverter.GetBytes(argbValue);

            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }
    }
}
