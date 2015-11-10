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

namespace WPCalendar
{
   public class PopupAddEditChild:Control
   {
       #region Private
       Button cancelBtn, saveBtn;
       CheckBox cbAllDayEvent;
       DatePicker startDatePicker,endDatePicker;
       TimePicker startTimePicker, endTimePicker;
       Border selectedEventColor;
       Grid eventColoursGrid;
       TextBox tbName, tbLocation;

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


       public Brush EventColor
       {
           get;
           internal set;
       }
    
       public DateTime EventStart
       {
           get;
           internal set;
       }
      
       public DateTime EventEnd
       {
           get;
           internal set;
       }
    


       #endregion

       public PopupAddEditChild(CalendarItem calItem)
       {
           DefaultStyleKey = typeof(PopupAddEditChild);
           EventTitle = ApplicationResources.NewEventTitle;
           EventLocation = ApplicationResources.NewEventLocation;
           EventColor = CustomColor.Aquamarine;

           eventItem = new EventItem()
           {
               EventTitle = ApplicationResources.NewEventTitle,
               EventLocation = ApplicationResources.NewEventLocation,
               EventColor = CustomColor.Aquamarine,
               EventStart = DateTime.Today,
               EventEnd = DateTime.Today,
               EventType = EventType.Allday
           };

           _owningCalendarItem = calItem;
       }

       public PopupAddEditChild(DateTime start, DateTime end)
       {
           DefaultStyleKey = typeof(PopupAddEditChild);
           EventTitle = ApplicationResources.NewEventTitle;
           EventLocation = ApplicationResources.NewEventLocation;
           EventStart = start;
           EventEnd = end;
           EventColor = CustomColor.Aquamarine;

           eventItem = new EventItem()
           {
               EventTitle = ApplicationResources.NewEventTitle,
               EventLocation = ApplicationResources.NewEventLocation,
               EventColor = CustomColor.Aquamarine,
               EventStart = start,
               EventEnd = end,
               EventType = EventType.Allday
           };
       }

       public PopupAddEditChild(EventItem item, CalendarItem calItem)
       {
           DefaultStyleKey = typeof(PopupAddEditChild);
           EventTitle = item.EventTitle;
           EventLocation = item.EventLocation;
           EventColor = item.EventColor;
           EventStart = item.EventStart;
           EventEnd = item.EventEnd;

           eventItem = item;
           _owningCalendarItem = calItem;
       }

       public override void OnApplyTemplate()
       {
           base.OnApplyTemplate();

            AssignUIElements();

            InitializeValues();
       }

       private void InitializeValues()
       {
           selectedEventColor.Background = EventColor;
           startDatePicker.Value = EventStart;
           endDatePicker.Value = EventEnd;
           startTimePicker.Value = EventStart;
           endTimePicker.Value = EventEnd;

           cancelBtn.Background = saveBtn.Background = EventColor;
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
           tbName.GotFocus += TextBoxGotFocus;
           tbName.LostFocus += TextBoxLostFocus;

           tbLocation = GetTemplateChild("TbLocation") as TextBox;
           tbLocation.Loaded += TextBoxLoaded;
           tbLocation.GotFocus += TextBoxGotFocus;
           tbLocation.LostFocus += TextBoxLostFocus;

           cbAllDayEvent = GetTemplateChild("CBAllDayEvents") as CheckBox;
           cbAllDayEvent.Checked += AllDayEventChecked;
           cbAllDayEvent.Unchecked += AllDayEventUnChecked;

           startDatePicker = GetTemplateChild("startDatePicker") as DatePicker;
           startTimePicker = GetTemplateChild("startTimePicker") as TimePicker;

           endDatePicker = GetTemplateChild("endDatePicker") as DatePicker;
           endTimePicker = GetTemplateChild("endTimePicker") as TimePicker;

           selectedEventColor = GetTemplateChild("SelectedEventColor") as Border;

           eventColoursGrid = GetTemplateChild("EventColoursGrid") as Grid;
           foreach (UIElement child in eventColoursGrid.Children)
           {
               if (child.GetType() == typeof(Rectangle))
                   child.Tap += PickColor;
           }
       }

       #region Events

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
           selectedEventColor.Background =
            cancelBtn.Background = 
            saveBtn.Background = chosenColor.Fill;
       }

       void AllDayEventUnChecked(object sender, RoutedEventArgs e)
       {
           startDatePicker.SetValue(Grid.RowSpanProperty, 1);
           endDatePicker.SetValue(Grid.RowSpanProperty, 1);
           startTimePicker.Visibility = endTimePicker.Visibility = Visibility.Visible;
       }
      
       void AllDayEventChecked(object sender, RoutedEventArgs e)
       {
           startDatePicker.SetValue(Grid.RowSpanProperty, 2);
           endDatePicker.SetValue(Grid.RowSpanProperty, 2);
           startTimePicker.Visibility = endTimePicker.Visibility = Visibility.Collapsed; 
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
               EventItem ev = new EventItem()
               {
                   EventTitle = EventTitle,
                   EventLocation = EventLocation,
                   EventStart = EventStart,
                   EventEnd = EventEnd,
                   EventColor = (SolidColorBrush)EventColor,
               };

               //do save
               if (!eventItem.Equals(ev))
               {
                   eventItem = ev;
                   /*
                   _owningCalendarItem._owningCalendar.EventsCalendar.AllEvents.Remove(eventItem);
                   _owningCalendarItem._owningCalendar.EventsCalendar.AllEvents.Add(ev);
                   _owningCalendarItem._owningCalendar.Refresh();*/
               }

               (this.Parent as Popup).IsOpen = false;
           }
           else
               tbName.BorderBrush = new SolidColorBrush(Colors.Red);

           _owningCalendarItem._owningCalendar.RegisterHourGridTap();
       }

       #endregion
   }
}
