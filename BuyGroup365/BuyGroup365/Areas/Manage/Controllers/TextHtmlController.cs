using Business.ClassBusiness;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class TextHtmlController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private TextHtmlBusiness textHtmlBusiness = new TextHtmlBusiness();

        public TextHtmlController()
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

                var list = textHtmlBusiness.GetList(key).ToPagedList(currentPageIndex, 20);

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
        public ActionResult Add(TextHtml obj)
        {
            try
            {
                obj.CreateDate = DateTime.Now;
                obj.ModifyDate = DateTime.Now;
                obj.Type = 1;

                textHtmlBusiness.AddNew(obj);
                Response.Redirect("/Manage/TextHtml/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/TextHtml/Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long Id)
        {
            try
            {
                var dbCatalog = textHtmlBusiness.GetById(Id);
                return View(dbCatalog);
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(long Id, string Title, string Code, int Status, string Attribute, string Content)
        {
            try
            {
                var dbCatalog = textHtmlBusiness.GetById(Id);
                dbCatalog.Attribute = Attribute;
                dbCatalog.Code = Code;
                dbCatalog.Content = Content;
                dbCatalog.ModifyDate = DateTime.Now;
                dbCatalog.Status = Status;
                dbCatalog.Title = Title;

                textHtmlBusiness.Edit(dbCatalog);
                Response.Redirect("/Manage/TextHtml/Index");
            }
            catch
            {
                Response.Redirect("/Manage/TextHtml/Index");
            }
            return View();
        }

        public JsonResult GetTextHtml(long id)
        {
            try
            {
                var dbCatalog = textHtmlBusiness.GetById(id);
                return Json(dbCatalog, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);//Cập nhật thành công
            }
        }

        public ActionResult DeleteTextHtml(long id)
        {
            try
            {
                if (id > 0)
                {
                    //Thay đổi trạng thái danh mục
                    textHtmlBusiness.Remove(id);
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