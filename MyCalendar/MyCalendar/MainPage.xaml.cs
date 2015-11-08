using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyCalendar.Resources;
using WPCalendar;
using WPCalendar.Models;
using WPCalendar.Helpers;

namespace MyCalendar
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

             Cal.EventsProperty = null;
             Cal.EventsProperty = App.MainViewModel.CustomCalendar;
             Cal.Refresh();
        }

        private void addNewEvent_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Cal_Hold_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string day = (sender as Calendar).SelectedDate.ToString();
           // AppContext.NavigationService.Navigate(new Uri("/Views/DayView.xaml?day=" + day, UriKind.Relative));
        }

       
    }
}