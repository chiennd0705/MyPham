using Business.ClassBusiness;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class PromotionListController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private PromotionListBusiness promotionListBusiness = new PromotionListBusiness();
        private PromotionItemBusiness promotionItemBusiness = new PromotionItemBusiness();

        public PromotionListController()
        {
            //   PromotionList img;
            //img
        }

        #endregion Khai bao biến

        public ActionResult Index(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                if (string.IsNullOrEmpty(key))
                    key = string.Empty;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = promotionListBusiness.GetList(key).ToPagedList(currentPageIndex, 20);

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

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(PromotionList obj)
        {
            try
            {
                obj.CreateDate = DateTime.Now;
                obj.ModifyDate = DateTime.Now;

                promotionListBusiness.AddNew(obj);
                Response.Redirect("/Manage/PromotionList/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/PromotionList/Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long Id)
        {
            try
            {
                var dbCatalog = promotionListBusiness.GetById(Id);
                return View(dbCatalog);
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(long Id, string Title, string Code, int Status, string Description)
        {
            try
            {
                var dbCatalog = promotionListBusiness.GetById(Id);

                dbCatalog.Code = Code;
                dbCatalog.Description = Description;
                dbCatalog.ModifyDate = DateTime.Now;
                dbCatalog.Status = Status;
                dbCatalog.Title = Title;
                promotionListBusiness.Edit(dbCatalog);
                Response.Redirect("/Manage/PromotionList/Index");
            }
            catch
            {
                Response.Redirect("/Manage/PromotionList/Index");
            }
            return View();
        }

        public JsonResult GetPromotionList(long id)
        {
            try
            {
                var dbCatalog = promotionListBusiness.GetById(id);
                return Json(dbCatalog, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);//Cập nhật thành công
            }
        }

        public ActionResult DeletePromotionList(long id)
        {
            try
            {
                if (id > 0)
                {
                    //Thay đổi trạng thái danh mục
                    promotionListBusiness.Remove(id);
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

        public ActionResult UpdatePromotionItem(long Id, long? imgId, string Title, string btnSubmit)
        {
            if (btnSubmit == "Thêm mục khuyến mãi")
            {
                PromotionItem imgPromotionItem = new PromotionItem();
                imgPromotionItem.Title = Title;
                imgPromotionItem.PromotionID = Id;
                promotionItemBusiness.AddNew(imgPromotionItem);
            }
            else if (btnSubmit == "Cập nhật mục khuyến mãi")
            {
                PromotionItem imgimgPromotionItem = promotionItemBusiness.GetById((long)imgId);
                imgimgPromotionItem.Title = Title;
                promotionItemBusiness.Edit(imgimgPromotionItem);
            }
            Response.Redirect("~/Manage/PromotionList/Edit/" + Id);
            return View();
        }

        public ActionResult DeleteimgimgPromotionItem(long id, long imgid)
        {
            try
            {
                if (imgid > 0)
                {
                    //Thay đổi trạng thái danh mục
                    promotionItemBusiness.Remove(imgid);
                    //status: 1(active), 0(private)
                }
                Response.Redirect("/Manage/PromotionList/Edit/" + id);
            }
            catch (Exception)
            {
                //write log
                throw;
            }
            return View();
        }
    }
}