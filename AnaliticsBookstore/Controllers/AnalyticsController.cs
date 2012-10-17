using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    public class KeywordsController : Controller
    {

        private KeywordStatsDataContext db = new KeywordStatsDataContext();

        public ActionResult SelectData(GoogleKeywordFilter filter)
        {
            if (filter == null)
            {
                filter = new GoogleKeywordFilter();
            }
            return View(filter);
        }
        /// <summary>
        /// Opisać
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ActionResult GetData(GoogleKeywordFilter filter)
        {
            OAuth2Parameters parameters = new ApiAuth(this.HttpContext).Parameters;
            GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory("apps", "testGData", parameters);
            AnalyticsService service = new AnalyticsService("testGData");
            service.RequestFactory = requestFactory;

            const string dataFeedUrl = "https://www.google.com/analytics/feeds/data";
            DataQuery query = new DataQuery(dataFeedUrl);
            query.Ids = "ga:36743694";
            query.Metrics = "ga:visits, ga:visitBounceRate";
            query.Dimensions = "ga:hostname, ga:landingPagePath, ga:keyword";
            query.Sort = "-ga:visits,ga:keyword";
            query.Filters = "ga:medium==organic;ga:keyword!@not set;ga:keyword!@not provided;ga:landingPagePath!@not set;ga:visits>=" + filter.MinPageViews.ToString() + ";ga:visitBounceRate<=" + filter.MaxBounceRate.ToString();
            query.GAStartDate = filter.StartDate.ToShortDateString();
            query.GAEndDate = filter.EndDate.ToShortDateString();
            DataFeed dataFeed = service.Query(query);
            List<GoogleKeywordAnalytic> result = new List<GoogleKeywordAnalytic>();
            //Clear table
            db.ExecuteCommand("DELETE FROM ItIdea_SEO_MVGooglKeywordAnalytics");
            foreach (DataEntry entry in dataFeed.Entries)
            {
                result.Add(new GoogleKeywordAnalytic(entry));
                db.GoogleKeywordAnalytics.InsertOnSubmit(new GoogleKeywordAnalytic(entry));
            }
            db.SubmitChanges();
            return View(result);
        }

        public ActionResult Index()
        {
            string url = OAuthUtil.CreateOAuth2AuthorizationUrl(new ApiAuth(this.HttpContext).Parameters);
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


        public ActionResult Auth()
        {
            ApiAuth apiAuth = new ApiAuth(this.HttpContext);
            apiAuth.SetTokenCode(Request.QueryString["code"]);
            return RedirectToAction("Index");
        }

    }
}
