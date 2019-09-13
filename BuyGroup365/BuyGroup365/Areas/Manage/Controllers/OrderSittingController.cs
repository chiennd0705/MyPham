using Business.ClassBusiness;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class OrderSittingController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private OrderSittingBusiness orderSittingBusiness = new OrderSittingBusiness();

        public OrderSittingController()
        {
        }

        #endregion Khai bao biến

        #region Category

        public ActionResult Index(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                if (string.IsNullOrEmpty(key))
                    key = string.Empty;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = orderSittingBusiness.GetList(key).ToPagedList(currentPageIndex, 20);

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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(long id, string btnSubmit, string FullName, string SDT, string TimeNote, string City)
        {
            try
            {
                Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();

                if (btnSubmit == "Thêm")
                {
                    OrderSitting orderSitting = new OrderSitting();
                    orderSitting.FullName = FullName;
                    orderSitting.SDT = SDT;
                    orderSitting.TimeNote = TimeNote;
                    orderSitting.DateCreate = DateTime.Now;
                    orderSitting.City = City;

                    orderSittingBusiness.AddNew(orderSitting);
                }
                else
                {
                    var orderSitting = orderSittingBusiness.GetById(id);
                    orderSitting.FullName = FullName;
                    orderSitting.SDT = SDT;
                    orderSitting.TimeNote = TimeNote;
                    orderSitting.City = City;

                    orderSittingBusiness.Edit(orderSitting);
                }
                Response.Redirect("/Manage/OrderSitting/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/OrderSitting/Index");
            }
            return View();
        }

        public ActionResult Delete(long id)
        {
            try
            {
                if (id > 0)
                {
                    //Thay đổi trạng thái danh mục
                    orderSittingBusiness.Remove(id);
                    //status: 1(active), 0(private)
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //write log
                throw;
            }
        }

        #endregion Category
    }
}