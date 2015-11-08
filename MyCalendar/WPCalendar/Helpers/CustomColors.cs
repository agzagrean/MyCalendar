
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPCalendar.Helpers
{
    public static class CustomColor
    {
        public static SolidColorBrush Blue
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(255, 135, 206, 235));
            }
        }
        public static SolidColorBrush Aqua
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 0, 206, 209)); }
        }
        public static SolidColorBrush Green
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 152, 251, 152)); }
        }
        public static SolidColorBrush DarkGreen
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 50, 205, 50)); }
        }
        public static SolidColorBrush Yellow
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 240, 230, 140)); }
        }
        public static SolidColorBrush Peach
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 250, 128, 114)); }
        }
        public static SolidColorBrush Marsala
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 220, 20, 60)); }
        }
        public static SolidColorBrush Lavender
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 218, 112, 214)); }
        }
        public static SolidColorBrush Gray
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 192, 192, 192)); }
        }
        public static SolidColorBrush DarkBlue
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 65, 105, 225)); }
        }
        public static SolidColorBrush White
        {
            get { return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)); }
        }
    }
}
