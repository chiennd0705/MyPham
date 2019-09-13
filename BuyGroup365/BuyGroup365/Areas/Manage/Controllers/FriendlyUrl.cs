using Business.ClassBusiness;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class FriendlyUrlController : BaseController
    {
        private readonly FriendlyUrlBusiness _friendlyUrlBussiness = new FriendlyUrlBusiness();

        //
        // GET: /Manage/FriendlyUrl/

        #region FriendlyUrl

        public ActionResult Index(string name, int? page)
        {
            try
            {
                int currentPageIndex = page.HasValue ? page.Value : 1;
                var list = _friendlyUrlBussiness.searchByName(name).ToPagedList(currentPageIndex, 20);
                ViewBag.Name = name;
                ViewBag.Page = currentPageIndex;

                return View(list);
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        public ActionResult Delete(long id)
        {
            try
            {
                _friendlyUrlBussiness.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        #endregion FriendlyUrl
    }
}