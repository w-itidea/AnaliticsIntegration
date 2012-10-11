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
    public class AnalyticsController : Controller
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

        public ActionResult GetData(GoogleKeywordFilter filter)
        {
            OAuth2Parameters parameters = new OAuth2Parameters()
            {
                ClientId = "788104354832-4f8hbomo43ga2mm92htpcj3i35mni3al.apps.googleusercontent.com",
                ClientSecret = "9fhl9Xa2dM9UbbsAum5ROb9_",
                RedirectUri = "http://localhost:52690/Analytics/Auth",
                Scope = "https://www.googleapis.com/auth/analytics.readonly"
            };
            if (this.HttpContext.Session["code"] != null)
            {
                parameters.AccessCode = this.HttpContext.Session["code"].ToString();
            }
            if (this.HttpContext.Session["token"] != null)
            {
                parameters.AccessToken = this.HttpContext.Session["token"].ToString();
            }

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
            var gKeyowrds = db.GoogleKeywordAnalytics;
            db.GoogleKeywordAnalytics.DeleteAllOnSubmit(gKeyowrds);
            db.SubmitChanges();
            
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
            OAuth2Parameters parameters = new OAuth2Parameters()
            {
                ClientId = "788104354832-4f8hbomo43ga2mm92htpcj3i35mni3al.apps.googleusercontent.com",
                ClientSecret = "9fhl9Xa2dM9UbbsAum5ROb9_",
                RedirectUri = "http://localhost:52690/Analytics/Auth",
                Scope = "https://www.googleapis.com/auth/analytics.readonly"
            };
            string url = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
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
            // build the base parameters
            OAuth2Parameters parameters = new OAuth2Parameters()
            {
                ClientId = "788104354832-4f8hbomo43ga2mm92htpcj3i35mni3al.apps.googleusercontent.com",
                ClientSecret = "9fhl9Xa2dM9UbbsAum5ROb9_",
                RedirectUri = "http://localhost:52690/Analytics/Auth",
                Scope = "https://www.googleapis.com/auth/analytics.readonly"
            };
            if (Request.QueryString["code"] != null)
            {
                this.HttpContext.Session["code"] = Request.QueryString["code"];
                parameters.AccessCode = Request.QueryString["code"];
            }

            if (this.HttpContext.Session["code"] != null)// && this.HttpContext.Session["token"] == null)
            {
                OAuthUtil.GetAccessToken(parameters);
                this.HttpContext.Session["token"] = parameters.AccessToken;
            }

            return RedirectToAction("Index");
        }

    }
}
