using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class OrderController : BaseController
    {
        private readonly OrdersBusiness _orderBussiness = new OrdersBusiness();
        private readonly OrderBuyerBusiness _orderBuyerBusiness = new OrderBuyerBusiness();
        private readonly OrderReciverBusiness _orderReciverBusiness = new OrderReciverBusiness();
        private readonly OrderDetailBusiness _orderDetailBusiness = new OrderDetailBusiness();
        //
        // GET: /Manage/Order/

        #region ORDER

        public ActionResult Index(int? page, long? loadShop = -1, int? stateOrder = -1, int? loadPaid = -1)
        {
            ViewBag.Mes = string.Empty;
            ViewBag.ShopId = loadShop;
            ViewBag.states = stateOrder;
            ViewBag.paid = loadPaid;
            ViewBag.loadShop = LoadCombo.LoadDropShop();
            ViewBag.stateOrder = LoadCombo.LoadDropOrder(stateOrder);
            ViewBag.loadPaid = LoadCombo.LoadDropOrderPaid(loadPaid);
            int currentPageIndex = page.HasValue ? page.Value : 1;
            ViewBag.page = currentPageIndex;
            var listObj = _orderBussiness.GetList(loadShop, stateOrder, loadPaid);

            IPagedList<Order> obj = listObj.ToPagedList(currentPageIndex, 10);

            if (obj.Any())
            {
                //    obj=obj.ToPagedList(currentPageIndex, 20);
                return View(obj);
            }
            {
                ViewBag.Mes = "Chưa có đơn hàng";
                //  obj=(IPagedList<Order>) new List<Order>();
                return View();
            }
        }

        public ActionResult PoupOder(long id)
        {
            var order = _orderBussiness.GetById(id);
            string body = ControllerExtensions.RenderRazorViewToString(this, "RenderOder", order);
            return Json(body, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateOrder(long orderid, int status, int paid)
        {
            try
            {
                var order = _orderBussiness.GetById(orderid);
                order.Status = status;
                order.Paid = paid;
                _orderBussiness.Edit(order);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //write log here
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RenderOder()
        {
            return View();
        }

        public ActionResult Delete(long id = 0)
        {
            try
            {
                if (id > 0)
                {
                    _orderBussiness.Remove(id);
                }
                return RedirectToAction("Index");
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        public List<SelectListItem> InitSelectListItemStatusOrder()
        {
            var listItemStatus = new List<SelectListItem>();
            listItemStatus.Add(new SelectListItem { Text = "--Chọn trạng thái--", Value = "-1" });
            listItemStatus.Add(new SelectListItem { Text = "Chưa hoàn thành", Value = "0" });
            listItemStatus.Add(new SelectListItem { Text = "Đã thanh toán", Value = "1" });
            listItemStatus.Add(new SelectListItem { Text = "Chưa nhận hàng", Value = "2" });
            listItemStatus.Add(new SelectListItem { Text = "Đã nhận hàng", Value = "3" });
            listItemStatus.Add(new SelectListItem { Text = "Đã xóa", Value = "4" });

            return listItemStatus;
        }

        #endregion ORDER

        #region BUYER

        public ActionResult Buyer(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _orderBuyerBusiness.GetByKey(key).ToPagedList(currentPageIndex, 20);

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

        public ActionResult DeleteBuyer(long id = 0)
        {
            try
            {
                if (id > 0)
                {
                    _orderBuyerBusiness.Remove(id);
                }
                return RedirectToAction("Buyer");
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        #endregion BUYER

        #region RECIVER

        public ActionResult Reciver(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _orderReciverBusiness.GetByKey(key).ToPagedList(currentPageIndex, 20);

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

        public ActionResult DeleteReciver(long id = 0)
        {
            try
            {
                if (id > 0)
                {
                    _orderReciverBusiness.Remove(id);
                }
                return RedirectToAction("Reciver");
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        #endregion RECIVER

        #region ORDER DETAIL

        public ActionResult Detail(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _orderDetailBusiness.GetByKey(key).ToPagedList(currentPageIndex, 20);

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

        #endregion ORDER DETAIL

        #region Detail Buyer

        /// <summary>
        /// Render thông tin người bán người mua
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderInfoPersonOrder()
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">type==1: thông tin người bán; type=2: thông tin người nhận</param>
        /// <returns></returns>
        public JsonResult GetInForBuyer(long id)
        {
            var order = new OrdersBusiness().GetById(id);
            //var shop = new ShopsBusiness().GetById(order.IdShop.Value);
            //order.ShopOrder = shop;
            string body = ControllerExtensions.RenderRazorViewToString(this, "RenderInfoPersonOrder", order);
            return Json(body, JsonRequestBehavior.AllowGet);
        }

        #endregion Detail Buyer
    }
}