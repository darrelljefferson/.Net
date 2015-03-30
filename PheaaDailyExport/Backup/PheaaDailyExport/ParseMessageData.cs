using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace GuidePostWeeklyReporting
{
    public class ParseMessageData
    {
        public string Date { get; set; }
        public string Category { get; set; }
        public string Segment { get; set; }
        public string Campaign { get; set; }
        public string Sent { get; set; }
        public string Delivered { get; set; }
        public decimal PercentDelivered { get; set; }
        public string Opened { get; set; }
        public decimal PercentOpened { get; set; }
        public string UniqueOpened { get; set; }
        public decimal PercentUniqueOpened { get; set; }
        public string Clicks { get; set; }
        public decimal PercentClicks { get; set; }
        public string UniqueClicks { get; set; }



    }

    public class ParseUnsubscribe
    {
        public string url { get; set; }
        public string uniqueUnsubs { get; set; }
        public string totalUnsubs { get; set; }
        
    }

    public class ParseClickData
    {
        public string url { get; set; }
        public string clicks { get; set; }
       
    }
}
