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

        /// <summary>
        /// Przykłady:
        /// 
        /// Australia: http://thepolishbookstore.com:8354/BookstoreFeeds/TPB/?waluta=AUD
        /// 
        /// Canada: http://polskaksiegarniainternetowa.com:8124/BookstoreFeeds/TPB/?waluta=CAD
        /// USA: http://polskaksiegarniainternetowa.com:8124/BookstoreFeeds/TPB
        /// Austria: http://polskaksiegarniainternetowa.com:8124/BookstoreFeeds/TPB/?waluta=AUD
        /// 
        /// 
        /// </summary>
        /// <param name="waluta"></param>
        /// <returns></returns>
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
                //TODO - zgłaszanie błedów mailem.
            }

            //return Content(st)
            return View();
        }
    }
      
}
