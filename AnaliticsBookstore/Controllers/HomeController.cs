using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(String id = "4782" )
        {
            ViewBag.Message = "Badanie uruchomiono dla id = " + id;

            ViewBag.res = MvcApplication1.Models.TransactionDetailsService.Start(id);

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// id chwilow doowolne
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DoPoznaniaEvents(String id = "-1")
        {
            //ViewBag.Message = "Welcome to ASP.NET MVC!";

            //ViewBag.res = MvcApplication1.Models.DoPoznaniaEventsDetailsService.Start(id);

            return View();
        }
    }
}
