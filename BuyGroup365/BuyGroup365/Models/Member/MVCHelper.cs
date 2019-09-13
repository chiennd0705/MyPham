using BuyGroup365.Areas.Manage.Controllers;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BuyGroup365.Models.Member
{
    public class MvcHelper
    {
        public static HttpContext Ctx;

        public static string GetRazorViewAsString(object model, string filePath)
        {
            var st = new StringWriter();
            //var context = new HttpContextWrapper(HttpContext.Current);
            var context = new HttpContextWrapper(Ctx);
            var routeData = new RouteData();
            var controllerContext = new ControllerContext(new RequestContext(context, routeData), new BaseController());
            var razor = new RazorView(controllerContext, filePath, null, false, null);
            razor.Render(new ViewContext(controllerContext, razor, new ViewDataDictionary(model), new TempDataDictionary(), st), st);
            return st.ToString();
        }
    }
}