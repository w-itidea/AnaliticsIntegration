using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using GoogleFeeds;

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


        public ActionResult TPBProductLinks()
        {
            #region Ustawienia

            //String _waluta = " USD";
            //var p = LibBLL.CMSBLL._context.Portal_GetArticles(null, null, null, 20);
            string TEMP_LINK = "http://thepolishbookstore.com/p/";
            string TEMP_SITE_URI = "http://thepolishbookstore.com";
   
            #endregion


                var context = new FeedsDataContextDataContext();
                var producst = context.ITidea_GoogleFeeds();


                string _txt = "";

                foreach (var item in producst)
                {
                    _txt += TEMP_LINK + item.productID + "/" + BookstorConstDictionary.GetSeName(item.title, true, false) + "\n";


                    //string _desc =
                    //    (String.IsNullOrEmpty(item.tytuloryg) ? "Polish Book '" + item.title + "'" : ("This is a polish edition of " + item.tytuloryg))

                    //     + (String.IsNullOrEmpty(item.author) ? "" : (" by " + item.author))

                    //        ///TODO: Dorobic Cover type, published, book language 

                    //    + " Description in polish: " + item.shortdescription

                    //     ;
                }
                return Content(_txt, "text/plain", UTF8Encoding.UTF8 );
        }

    }
      
}
