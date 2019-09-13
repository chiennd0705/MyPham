using Business.ClassBusiness;
using Common.util;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class LoginController : Controller
    {
        private UserBusiness _userBusiness = new UserBusiness();

        public LoginController()
        {
        }

        #region LOGIN, LOGOUT

        public ActionResult Login()
        {
            ViewBag.Mes = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password, string remember, string returnUrl)
        {
            ViewBag.Mes = "";
            string IPAddress = IPHelper.GetIPAddress(Request.ServerVariables["HTTP_VIA"],
                                                     Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                                                     Request.ServerVariables["REMOTE_ADDR"]);

            UserInfo userInfo = new UserBusiness().CheckLogin(userName, password, "", ObjectClass.GetHostName(Request.ServerVariables["REMOTE_ADDR"]), IPHelper.GetIPAddress(Request.ServerVariables["HTTP_VIA"],
                                                                                                                                                                                           Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                                                                                                                                                                                           Request.ServerVariables["REMOTE_ADDR"]));
            if (userInfo.Status)
            {
                SessionUtility.SetSessionUser(userInfo.UserId.ToString(), userInfo.UserName, userInfo.ScreenName, "0", Session, userInfo.IsSuperUser);

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return Redirect("/Manage/Admin/Index");
                //  return RedirectToRoute(new { controller = "Admin", action = "Index" });
            }
            else
            {
                ViewBag.Mes = userInfo.StatusMessage;
                return View();
            }
        }

        public ActionResult Logout()
        {
            SessionUtility.LogOutAdmin(Session);
            return RedirectToAction("Login", "Login");
        }

        #endregion LOGIN, LOGOUT
    }
}