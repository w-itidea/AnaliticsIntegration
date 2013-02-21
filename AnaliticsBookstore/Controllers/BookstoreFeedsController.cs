using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace MvcApplication1.Controllers
{
    public class BookstoreFeedsController : Controller
    {

        public ActionResult TPB(string waluta = "USD")
        {
           // try
            {

                Response.ContentEncoding = Encoding.UTF8;
                Response.ContentType = "text/xml";

                GoogleFeeds.Feeds.GenerateRSS(Response.OutputStream, waluta );

            }
           // catch
            {

            }

            //return Content(st)
            return View();
        }
    }
      
}
