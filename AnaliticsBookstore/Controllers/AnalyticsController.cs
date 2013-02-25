using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.IO;
using System.Web.UI;
using Google.GData.Analytics;
using Google.GData.Extensions;
using Google.GData.Client;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    /// <summary>
    /// Ten kontroler słuzy do pobierania listy słów kluczowych z analitics do bazy danych - wykonany przez Jacha
    /// 
    /// TODO opisac
    /// </summary>
    public class AnalyticsController : Controller
    {

        private KeywordStatsDataContext db = new KeywordStatsDataContext();

        /// <summary>
        /// 
        /// Ekran wyboru ustawień zakresu danych
        /// 
        /// </summary>
        /// <param name="filter">Ustawienia w postaci pomocniczego modelu GoogleKeywordFilter.</param>
        
        public ActionResult SelectData(GoogleKeywordFilter filter)
        {
            if (filter == null)
            {
                filter = new GoogleKeywordFilter();
            }

            if (this.HttpContext.Session["code"] != null)
            {
                ViewBag.IsCode = true;
            }
            if (this.HttpContext.Session["token"] != null)
            {
                ViewBag.IsToken = true;
            }
            ViewBag.AvailableOutput = GoogleKeywordFilter.AvailableOutput();
            return View(filter);
            
        }
        
        
        /// <summary>
        /// Ekran danych zwóronych prze GA. Tutaj pobierane są dane z GA.
        /// </summary>
        /// <param name="filter">Ustawienia w postaci pomocniczego modelu GoogleKeywordFilter.</param>
        /// <returns></returns>
        public ActionResult GetData(GoogleKeywordFilter filter)
        {
            OAuth2Parameters parameters = new ApiAuth(this.HttpContext).Parameters; // Pobieramy nasze aktualne dane dostępowe do GA
            GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory("apps", "testGData", parameters); // Na ich podstawie tworzymy obiekt GOAuth2RequestFactory, który będzie potrzebny do wysyłania zapytań do GA.
            AnalyticsService service = new AnalyticsService("testGData"); // Tworzymy obiekt który będzie się łączył z GA
            service.RequestFactory = requestFactory; // Dajemy mu nasze dane dostępowe do GA.

            const string dataFeedUrl = "https://www.google.com/analytics/feeds/data"; // Adres do wysyłania zapytań do google.
            DataQuery query = new DataQuery(dataFeedUrl); // tworzymy zapytanie
            query.Ids = "ga:" + filter.GaId; // podajemy id strony w GA (bardzo ważne)
            query.Metrics = "ga:visits, ga:visitBounceRate"; // podajemy mierzone wartości
            query.Dimensions = "ga:hostname, ga:landingPagePath, ga:keyword"; // oraz wymiary
            query.Sort = "-ga:visits,ga:keyword"; // sposób sortowania
            query.Filters = "ga:medium==organic;ga:keyword!@not set;ga:keyword!@not provided;ga:landingPagePath!@not set;ga:visits>=" + filter.MinPageViews.ToString() + ";ga:visitBounceRate<=" + filter.MaxBounceRate.ToString() + (filter.UrlLike != null ? ";ga:landingPagePath=@" + filter.UrlLike : "");
            // Filtry: znak ";" oznacza AND
            query.GAStartDate = filter.StartDate.ToShortDateString(); // wskazujemy datę rozpoczęcia
            query.GAEndDate = filter.EndDate.ToShortDateString(); // i zakończenia

            DataFeed dataFeed = service.Query(query); // wysyłamy zapytanie do GA

            List<GoogleKeywordAnalytic> result = new List<GoogleKeywordAnalytic>(); // tworzymy listę do przechowywania wyników

            //db.ExecuteCommand("DELETE FROM ItIdea_SEO_MVGooglKeywordAnalytics"); // Czyścimy tablicę do której będą zapisane wyniki
            foreach (DataEntry entry in dataFeed.Entries)
            {
                result.Add(new GoogleKeywordAnalytic(entry)); // Dodajemy wynik do listy
                //db.GoogleKeywordAnalytics.InsertOnSubmit(new GoogleKeywordAnalytic(entry)); // Oraz do bazy danych
            }
            //db.SubmitChanges(); // Wysyłamy zmiany do bazy danych
            if (filter.Output == OutputType.CSV)
            {
                // Konwersja do CSV
                // Renderujemy partiala "_CSV" do stringa
                // Dekodujemy encodowanie html (&amp; => &)
                // Konwertujemy do UTF8 i dalej do array<byte>
                // Konwertujemy do memory string
                // Zwracamy wszystko jako File
                return File(new MemoryStream(Encoding.UTF8.GetBytes(System.Web.HttpUtility.HtmlDecode(ControllerContext.RenderPartialAsString("_CSV", result)))), "text/csv", "analytics.csv");
            }
            return View(result); // Wyświetlamy wyniki na odpowiednim ekranie
        }

        /// <summary>
        /// Ekran główny. Wyświetla informację czy mamy token oraz access code.
        /// </summary>
        public ActionResult Index()
        {
            string url = OAuthUtil.CreateOAuth2AuthorizationUrl(new ApiAuth(this.HttpContext).Parameters); // Generowanie linku autoryzacyjnego (za pierwszym razem wymagane jest potwierdzenie dania dostępu do GA, przy kolejnych próbach dostęp dawany jest automatycznie)
            ViewBag.AuthUrl = url;
            if (this.HttpContext.Session["code"] != null)
            {
                ViewBag.IsCode = true;
            }
            if (this.HttpContext.Session["token"] != null)
            {
                ViewBag.IsToken = true;
            }
            return View();
        }

        /// <summary>
        /// Ekran zwrotny GA. Po udanej autoryzacji google przekierowywuje na tą stronę i podaje nam access code
        /// </summary>
        public ActionResult Auth()
        {
            ApiAuth apiAuth = new ApiAuth(this.HttpContext);
            apiAuth.SetTokenCode(Request.QueryString["code"]); // Zapisujemy access code w sesji
            return RedirectToAction("Index");
        }

    }
}
