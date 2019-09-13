using Business.ClassBusiness;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class ErrorController : Controller
    {
        private UserBusiness _userBusiness = new UserBusiness();

        #region LOGIN, LOGOUT

        public ActionResult ErrorFunction(string codeExp)
        {
            ViewBag.codeExp = codeExp;
            return View();
        }

        public ActionResult NotPermissionVoucher()
        {
            return View();
        }

        public ActionResult NotPermissionLog()
        {
            return View();
        }

        public ActionResult NotPermissionAdmin()
        {
            return View();
        }

        #endregion LOGIN, LOGOUT
    }
}