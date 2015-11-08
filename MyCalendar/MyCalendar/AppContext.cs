using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MyCalendar
{
    public static class AppContext
    {
        internal static NavigationService NavigationService
        {
            get
            {
                var frame = App.Current.RootVisual as PhoneApplicationFrame;
                var page = frame.Content as PhoneApplicationPage;

                return page.NavigationService;
            }
        }

     
    }
}
