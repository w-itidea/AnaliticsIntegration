using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{

    /// <summary>
    /// 
    /// Model pomocniczy słuzący do przekazywania danych z ekranu SelectData do GetData.
    /// 
    /// </summary>

    public enum OutputType { www, CSV };

    public class GoogleKeywordFilter
    {
        public DateTime StartDate { get; set; }     // Data rozpoczęcia okresu z którego chcemy pobrać dane
        public DateTime EndDate { get; set; }       // Data zakończenia okresu z którego chcemy pobrać dane
        public int MinPageViews { get; set; }       // Minimalna ilość odwiedzin\obejrzeń strony
        public float MaxBounceRate { get; set; }    // Maksymalny dopuszczalny BounceRate
        [Display (Name = "ID Analytics")]
        public string GaId { get; set; }            // ID profilu strony na GA
        [Display (Name = "URL")]
        public string UrlLike { get; set; }         // Filtr adresu url; srawdzane jest czy podany string znajduje się w adresie url (bez domeny); opcjonalne
        public GoogleKeywordFilter()                // Konstruktor - ustawia domyślne wartości
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            MinPageViews = 1;
            MaxBounceRate = 100;
        }
        public OutputType Output { get; set; }
        public static List<SelectListItem> AvailableOutput()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var source = Enum.GetValues(typeof(OutputType));
            var displayAttributeType = typeof(DisplayAttribute);
            foreach (var value in source)
            {
                //FieldInfo field = value.GetType().GetField(value.ToString());

               /* DisplayAttribute attrs = (DisplayAttribute)field.
                              GetCustomAttributes(displayAttributeType, false).First();

                items.Add(value, attrs.GetName());*/
                list.Add(new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString()
                });
            }
            return list;
        }
    }
}