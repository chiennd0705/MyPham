using System.Web.Mvc;
using System.Web.Routing;

namespace BuyGroup365.Areas.Manage.Models
{
    public class RefreshFriendly
    {
        public static void BindataSiteUrl()
        {
            RouteTable.Routes.Clear();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}