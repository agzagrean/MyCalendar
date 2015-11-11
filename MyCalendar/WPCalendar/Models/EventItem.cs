using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows;
using System.ComponentModel;
using WPCalendar.Helpers;
using System.Windows.Media;

namespace WPCalendar.Models
{
    [DataContract]
    public class EventItem
    {
        [DataMember]
        public Guid EventId { get; set; }


        [DataMember]
        public DateTime EventStart{ get;set;}

        [DataMember]
        public DateTime EventEnd { get;set;}

        [DataMember]
        public string EventTitle { get; set; }

        [DataMember]
        public string EventLocation { get; set; }

        [DataMember]
        public SolidColorBrush EventColor { get; set; }

        [DataMember]
        public EventType EventType { get; set; }

        public EventItem() { }

        public EventItem(Guid id, DateTime eventStart, DateTime eventEnd, string eventDescription, string eventLocation, SolidColorBrush eventColor, EventType eventType)
        {
            this.EventId = id;
            this.EventStart = eventStart;
            this.EventEnd = eventEnd;
            this.EventTitle = eventDescription;
            this.EventLocation = eventLocation;
            this.EventColor = eventColor;
            this.EventType = eventType;
           
        }

        
    }
}