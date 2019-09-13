using Business.ClassBusiness;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class ConfigsController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private ConfigRedirectUrlBusiness configRedirectUrlBusiness = new ConfigRedirectUrlBusiness();

        public ConfigsController()
        {
        }

        #endregion Khai bao biến

        #region Category

        public ActionResult Index(int? page)
        {
            try
            {
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = configRedirectUrlBusiness.GetByKey().ToPagedList(currentPageIndex, 20);

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
        public ActionResult Update(long id, string btnSubmit, string url, string urlRedirect)
        {
            try
            {
                if (btnSubmit == "Thêm")
                {
                    ConfigRedirectUrl tag = new ConfigRedirectUrl();
                    tag.Url = url;
                    tag.RedirectUrl = urlRedirect;
                    tag.Type = 1;

                    configRedirectUrlBusiness.AddNew(tag);
                }
                else
                {
                    ConfigRedirectUrl tag = new ConfigRedirectUrl();
                    tag.Id = id;
                    tag.Url = url;
                    tag.RedirectUrl = urlRedirect;
                    tag.Type = 1;

                    configRedirectUrlBusiness.Edit(tag);
                }
                Response.Redirect("/Manage/Configs/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/Configs/Index");
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
                    configRedirectUrlBusiness.Remove(id);
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