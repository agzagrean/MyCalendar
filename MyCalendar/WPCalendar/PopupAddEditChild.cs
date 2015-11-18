using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WPCalendar.Resources;
using WPCalendar.Models;
using WPCalendar.Helpers;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WPCalendar
{
   public class PopupAddEditChild:Control, INotifyPropertyChanged
   {
       #region Private
       Button cancelBtn, saveBtn;
       Grid eventColoursGrid;
       TextBox tbName, tbLocation;
       DatePicker startDatePicker, endDatePicker;
       TimePicker startTimePicker, endTimePicker;

       protected CalendarItem _owningCalendarItem;

       private EventItem eventItem;
       #endregion

       #region Properties
       
       public string EventTitle
       {
           get { return (string)GetValue(EventTitleProperty); }
           internal set { SetValue(EventTitleProperty, value); }
       }

       public static readonly DependencyProperty EventTitleProperty =
           DependencyProperty.Register("EventTitle", typeof(string), typeof(PopupAddEditChild), new PropertyMetadata(""));

       public string EventLocation
       {
           get { return (string)GetValue(EventLocationProperty); }
           internal set { SetValue(EventLocationProperty, value); }
       }

       public static readonly DependencyProperty EventLocationProperty =
           DependencyProperty.Register("EventLocation", typeof(string), typeof(PopupAddEditChild), new PropertyMetadata(""));

       private SolidColorBrush _eventColor;
       public SolidColorBrush EventColor
       {
           get
           { return _eventColor; }
           set
           {
               if (value != _eventColor)
                   _eventColor = value;
               NotifyPropertyChanged("EventColor");
           }
       }
    
       public DateTime EventStart
       {
           get;
           private set;
       }
      
       public DateTime EventEnd
       {
           get;
           private set;
       }

       private EventType _eventType;
       public EventType EventType
       {
           get { return _eventType; }
           set 
           {
               if (_eventType != value)
                   _eventType = value;
               NotifyPropertyChanged("EventType");
           }
       }

       #endregion

       #region Event handler
       public event PropertyChangedEventHandler PropertyChanged;
       private void NotifyPropertyChanged(String propertyName)
       {
           PropertyChangedEventHandler handler = PropertyChanged;
           if (null != handler)
           {
               handler(this, new PropertyChangedEventArgs(propertyName));
           }
       }
       #endregion

       public PopupAddEditChild(CalendarItem calItem, DateTime start, DateTime end)
       {
           DefaultStyleKey = typeof(PopupAddEditChild);
           EventTitle = ApplicationResources.NewEventTitle;
           EventLocation = ApplicationResources.NewEventLocation;
           EventColor = CustomColor.Aquamarine;
           EventStart = start;
           EventEnd = end;
           EventType = Models.EventType.Allday;

           eventItem = new EventItem()
           {
               EventId = Guid.Empty,
               EventTitle = EventTitle,
               EventLocation = EventLocation,
               EventColor = (SolidColorBrush)EventColor,
               EventStart = EventStart,
               EventEnd = EventEnd,
               EventType = EventType
           };

           _owningCalendarItem = calItem;
       }

       public PopupAddEditChild(EventItem item, CalendarItem calItem)
       {
           DefaultStyleKey = typeof(PopupAddEditChild);
           EventTitle = item.EventTitle;
           EventLocation = item.EventLocation;
           EventColor = item.EventColor;
           EventStart = item.EventStart;
           EventEnd = item.EventEnd;
           EventType = item.EventType;

           eventItem = item;
           _owningCalendarItem = calItem;
       }

       public override void OnApplyTemplate()
       {
           base.OnApplyTemplate();

            AssignUIElements();
       }

       private void AssignUIElements()
       {
           this.Height = (this.Parent as Popup).Height;
           this.Width = (this.Parent as Popup).Width;


           cancelBtn = GetTemplateChild("CancelButton") as Button;
           if (cancelBtn != null)
               cancelBtn.Click += CancelButtonClick;

           saveBtn = GetTemplateChild("SaveButton") as Button;
           if (saveBtn != null)
               saveBtn.Click += SaveButtonClick;

           tbName = GetTemplateChild("TbName") as TextBox;
           tbName.Loaded += TextBoxLoaded;
           tbName.TextChanged += TextChanged;
           tbName.GotFocus += TextBoxGotFocus;
           tbName.LostFocus += TextBoxLostFocus;

           tbLocation = GetTemplateChild("TbLocation") as TextBox;
           tbLocation.Loaded += TextBoxLoaded;
           tbLocation.TextChanged += TextChanged; 
           tbLocation.GotFocus += TextBoxGotFocus;
           tbLocation.LostFocus += TextBoxLostFocus;

           eventColoursGrid = GetTemplateChild("EventColoursGrid") as Grid;
           foreach (UIElement child in eventColoursGrid.Children)
           {
               if (child.GetType() == typeof(Rectangle))
                   child.Tap += PickColor;
           }

           startDatePicker = GetTemplateChild("startDatePicker") as DatePicker;
           startDatePicker.Tap += DateTimePickerTap;
           startDatePicker.ValueChanged += DatePickerValueChanged;

           endDatePicker = GetTemplateChild("endDatePicker") as DatePicker;
           endDatePicker.Tap += DateTimePickerTap;
           endDatePicker.ValueChanged += DatePickerValueChanged;

           startTimePicker = GetTemplateChild("startTimePicker") as TimePicker;
           startTimePicker.Tap += DateTimePickerTap;
           startTimePicker.ValueChanged += DatePickerValueChanged;

           endTimePicker = GetTemplateChild("endTimePicker") as TimePicker;
           endTimePicker.Tap += DateTimePickerTap;
           endTimePicker.ValueChanged += DatePickerValueChanged;
       }

       void DatePickerValueChanged(object sender, DateTimeValueChangedEventArgs e)
       {
           if (sender.GetType().Equals(typeof(DatePicker)))
           {
               DatePicker datePicker = sender as DatePicker;

               if (e.OldDateTime != e.NewDateTime && e.NewDateTime.HasValue)
               {
                   DateTime date = e.NewDateTime.Value;

                   if (datePicker == startDatePicker)
                   {
                       DateTime newDate = new DateTime(date.Year, date.Month, date.Day, EventStart.Hour, EventStart.Minute, EventStart.Second);
                       EventStart = newDate;
                   }
                   else

                       if (datePicker == endDatePicker)
                       {
                           DateTime newDate = new DateTime(date.Year, date.Month, date.Day, EventEnd.Hour, EventEnd.Minute, EventEnd.Second);
                           EventEnd = newDate;
                       }

                   if (EventStart > EventEnd)
                       EventEnd = EventStart;
               }
           }

           if (sender.GetType().Equals(typeof(TimePicker)))
           {
               TimePicker timePicker = sender as TimePicker;

               if (e.OldDateTime != e.NewDateTime && e.NewDateTime.HasValue)
               {
                   DateTime date = e.NewDateTime.Value;

                   if (timePicker == startTimePicker)
                   {
                       DateTime newDate = new DateTime(EventStart.Year, EventStart.Month, EventStart.Day, date.Hour, date.Minute, date.Second);
                       EventStart = newDate;
                   }
                   else
                       if (timePicker == endTimePicker)
                       {
                           DateTime newDate = new DateTime(EventEnd.Year, EventEnd.Month, EventEnd.Day, date.Hour, date.Minute, date.Second);
                           EventStart = newDate;
                       }
               }
           }
           (this.Parent as Popup).IsOpen = true;
       }

       void DateTimePickerTap(object sender, System.Windows.Input.GestureEventArgs e)
       {
           (this.Parent as Popup).IsOpen = false;
       
       }

       #region Events

       void TextChanged(object sender, TextChangedEventArgs e)
       {
           TextBox tb = (sender as TextBox);
           if (tb == tbName)
               EventTitle = tb.Text;
           if (tb == tbLocation)
               EventLocation = tb.Text;
       }

       void TextBoxLoaded(object sender, RoutedEventArgs e)
       {
           TextBox tb = sender as TextBox;
           if (tb.Text.Equals(ApplicationResources.NewEventTitle) || tb.Text.Equals(ApplicationResources.NewEventLocation))
               tb.Foreground = new SolidColorBrush(Colors.DarkGray);
           else
               tb.Foreground = new SolidColorBrush(Colors.Black);
       }

       void TextBoxLostFocus(object sender, RoutedEventArgs e)
       {
           TextBox tb = sender as TextBox;
           if (string.IsNullOrEmpty(tb.Text))
           {
               if (tb == tbName)
                   tb.Text = ApplicationResources.NewEventTitle;
               if (tb == tbLocation)
                   tb.Text = ApplicationResources.NewEventLocation;
               tb.Foreground = new SolidColorBrush(Colors.DarkGray);
           }
           else
               tb.Foreground = new SolidColorBrush(Colors.Black);
           tb.BorderBrush = new SolidColorBrush(Colors.DarkGray);
       }

       void TextBoxGotFocus(object sender, RoutedEventArgs e)
       {
           TextBox tb = sender as TextBox;
           tb.BorderBrush = new SolidColorBrush(Colors.Black);
           if (tb.Text.Equals(ApplicationResources.NewEventTitle) || tb.Text.Equals(ApplicationResources.NewEventLocation))
               tb.Text = string.Empty;
       }

       void PickColor(object sender, System.Windows.Input.GestureEventArgs e)
       {
           Rectangle chosenColor = sender as Rectangle;
           EventColor = (SolidColorBrush)chosenColor.Fill;
       }

       void CancelButtonClick(object sender, RoutedEventArgs e)
       {
          _owningCalendarItem._owningCalendar.RegisterHourGridTap();

           (this.Parent as Popup).IsOpen = false;
       }
      
       void SaveButtonClick(object sender, RoutedEventArgs e)
       {
           if (EventTitle != ApplicationResources.NewEventTitle)
           {
               AddUpdateEvent();
           }
           else
               tbName.BorderBrush = new SolidColorBrush(Colors.Red);

           _owningCalendarItem._owningCalendar.RegisterHourGridTap();
       }

       void AddUpdateEvent()
       {
           if (eventItem.NeedsUpdate(EventTitle, EventLocation, EventStart, EventEnd, EventColor, EventType))
           {
               if (eventItem.EventId.Equals(Guid.Empty))
               { 
                   //add new
                   eventItem.EventId = Guid.NewGuid();

                   _owningCalendarItem.EventsForDay.Add(eventItem);
                   _owningCalendarItem._owningCalendar.EventsCalendar.AllEvents.Add(eventItem);
               }
               else
                   //update
                   eventItem.UpdateValues(EventTitle, EventLocation, EventStart, EventEnd, EventColor, EventType);

               //refresh calendarItem details
               _owningCalendarItem.Refresh();
               //refresh calendar details
               _owningCalendarItem._owningCalendar.Refresh();
           }

           (this.Parent as Popup).IsOpen = false;
       }


       #endregion
 
   }
}
