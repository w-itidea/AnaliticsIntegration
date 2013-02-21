using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        
        /// <summary>
        /// Domyslna akcja służy do pobierania danych o tranzakcji z GoogleAnalitics.
        /// TODO - upożadkować i przenieść do odpowiedniego kontrolera.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(String id = "brak_Id") //defaut id="4782"
        {
            if (id == "brak_Id")
            {
                //nie podałes kontrolera - przenosimy na abiyt
                this.RedirectToAction("About");
            }

            
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
