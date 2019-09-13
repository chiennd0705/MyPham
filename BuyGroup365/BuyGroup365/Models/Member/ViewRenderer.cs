using BuyGroup365.Areas.Manage.Controllers;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BuyGroup365.Models.Member
{
    public class ViewRenderer
    {
        public ControllerContext Context { get; set; }

        public ViewRenderer(ControllerContext controllerContext = null)
        {
            // Create a known controller from HttpContext if no context is passed
            if (controllerContext == null)
            {
                if (HttpContext.Current != null)
                    controllerContext = CreateController<ErrorController>().ControllerContext;
                else
                    throw new InvalidOperationException(
                        "ViewRenderer must run in the context of an ASP.NET " +
                        "Application and requires HttpContext.Current to be present.");
            }
            Context = controllerContext;
        }

        public string RenderView(string viewPath, object model)
        {
            return RenderViewToStringInternal(viewPath, model, false);
        }

        public string RenderPartialView(string viewPath, object model)
        {
            return RenderViewToStringInternal(viewPath, model, true);
        }

        protected string RenderViewToStringInternal(string viewPath, object model, bool partial = false)
        {
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(Context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(Context, viewPath, null);
            if (viewEngineResult == null)
                throw new FileNotFoundException("ViewCouldNotBeFound");
            var view = viewEngineResult.View;
            Context.Controller.ViewData.Model = model;
            string result = null;
            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(Context, view,
                Context.Controller.ViewData,
                Context.Controller.TempData,
                sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }
            return result;
        }

        public static T CreateController<T>(RouteData routeData = null)
               where T : Controller, new()
        {
            T controller = new T();

            // Create an MVC Controller Context
            HttpContextBase wrapper = null;
            if (HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            //else
            //    wrapper = CreateHttpContextBase(writer);

            if (routeData == null)
                routeData = new RouteData();

            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }
    }
}