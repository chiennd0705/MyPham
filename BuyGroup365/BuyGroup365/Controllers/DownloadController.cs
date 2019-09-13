using Business.ClassBusiness;
using Common.util;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Controllers
{
    public class VideosController : Controller
    {
        private readonly VideosBusiness _videoBusiness = new VideosBusiness();

        private int Page_Size = 40;
        private int Page_Block = 4;

        // GET: /Gallery/
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            try
            {
                int totalRecord = 0;
                int totalPage = 0;
                List<Common.Videos> model = new List<Common.Videos>();

                model = _videoBusiness.ListAllVideo(ref totalRecord, page, pageSize);

                ViewBag.Total = totalRecord;
                ViewBag.Page = page;
                int maxPage = 1;
                if (totalRecord % pageSize == 0)
                    totalPage = totalRecord / pageSize;
                else
                    totalPage = totalRecord / pageSize + 1;
                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;

                return View(model);
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }
    }
}