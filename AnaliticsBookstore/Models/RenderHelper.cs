using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.UI;

namespace System.Web.Mvc
{
    public static class RenderPartialAsStringHelper
    {
        public static string RenderPartialAsString(this ControllerContext ctx, string partialPath, object model)
        {
            var controller = ctx.Controller;
            if (string.IsNullOrEmpty(partialPath))
                partialPath = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialPath);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                // copy model state items to the html helper 
                foreach (var item in viewContext.Controller.ViewData.ModelState)
                    if (!viewContext.ViewData.ModelState.Keys.Contains(item.Key))
                    {
                        viewContext.ViewData.ModelState.Add(item);
                    }


                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}