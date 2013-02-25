using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{

    /// <summary>
    /// 
    /// Model pomocniczy słuzący do przekazywania danych z ekranu SelectData do GetData.
    /// 
    /// </summary>

    public class GoogleKeywordFilter
    {
        public DateTime StartDate { get; set; }     // Data rozpoczęcia okresu z którego chcemy pobrać dane
        public DateTime EndDate { get; set; }       // Data zakończenia okresu z którego chcemy pobrać dane
        public int MinPageViews { get; set; }       // Minimalna ilość odwiedzin\obejrzeń strony
        public float MaxBounceRate { get; set; }    // Maksymalny dopuszczalny BounceRate
        public GoogleKeywordFilter()                // Konstruktor - ustawia domyślne wartości
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            MinPageViews = 1;
            MaxBounceRate = 100;
        }
    }
}