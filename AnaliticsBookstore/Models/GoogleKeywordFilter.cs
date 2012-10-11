using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class GoogleKeywordFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinPageViews { get; set; }
        public float MaxBounceRate { get; set; }
        public GoogleKeywordFilter()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            MinPageViews = 1;
            MaxBounceRate = 100;
        }
    }
}