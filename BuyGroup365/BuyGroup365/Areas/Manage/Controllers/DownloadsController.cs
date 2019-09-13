using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class DownloadsController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private DownloadsBusiness DownloadsBusiness = new DownloadsBusiness();
        private CatalogDownloadsBusiness CatalogDownloadsBusiness = new CatalogDownloadsBusiness();
        private readonly LoadCombo _loadCombo = new LoadCombo();
        private readonly FriendlyUrlBusiness _friendlyUrlBusines = new FriendlyUrlBusiness();

        public DownloadsController()
        {
        }

        #endregion Khai bao biến

        #region Category

        public ActionResult Index(string key, int? page)
        {
            try
            {
                List<LoadCombo.DropdowCatalogDownloads> listDropdowCatalogDownload = new List<LoadCombo.DropdowCatalogDownloads>();
                ViewBag.CatalogDownloadID = _loadCombo.SearchCatalogDownloadByName(ref listDropdowCatalogDownload);
                ViewData["status"] = true;
                if (string.IsNullOrEmpty(key))
                    key = string.Empty;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = DownloadsBusiness.GetList(key).ToPagedList(currentPageIndex, 20);

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
        public ActionResult Update(long id, string btnSubmit, string FileName, string Description, string SoureFile, int CatalogDownloadID)
        {
            try
            {
                Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();

                if (btnSubmit == "Thêm")
                {
                    Downloads Download = new Downloads();
                    Download.FileName = FileName;
                    if (SoureFile != "")
                    {
                        Download.SoureFile = SoureFile;
                    }
                    Download.CatalogDownloadID = CatalogDownloadID;
                    string file = HttpContext.Server.MapPath(SoureFile);
                    FileInfo info = new FileInfo(file);
                    //      int fileCount = Directory.GetFiles(file, "*.*", SearchOption.AllDirectories).Length;
                    Download.Size = Math.Round(info.Length / 1024f / 1024f, 2);
                    Download.Description = Description;
                    DownloadsBusiness.AddNew(Download);
                }
                else
                {
                    var Download = DownloadsBusiness.GetById(id);
                    Download.FileName = FileName;
                    if (SoureFile != "")
                    {
                        Download.SoureFile = SoureFile;
                    }
                    string file = HttpContext.Server.MapPath(SoureFile);
                    FileInfo info = new FileInfo(file);
                    //      int fileCount = Directory.GetFiles(file, "*.*", SearchOption.AllDirectories).Length;
                    Download.Size = Math.Round(info.Length / 1024f / 1024f, 2);
                    Download.CatalogDownloadID = CatalogDownloadID;

                    Download.Description = Description;
                    DownloadsBusiness.Edit(Download);
                }
                Response.Redirect("/Manage/Downloads/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/Downloads/Index");
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
                    DownloadsBusiness.Remove(id);
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

        #region CatalogDownloads

        public ActionResult IndexCatalogDownloads(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                if (string.IsNullOrEmpty(key))
                    key = string.Empty;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = CatalogDownloadsBusiness.GetList(key).ToPagedList(currentPageIndex, 20);

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
        public ActionResult UpdateCatalogDownloads(long id, string btnSubmit, string CatalogDownloadName, string Description, string Icon)
        {
            try
            {
                Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();

                if (btnSubmit == "Thêm")
                {
                    CatalogDownloads catalogDownload = new CatalogDownloads();
                    catalogDownload.CatalogDownloadName = CatalogDownloadName;
                    catalogDownload.ParentId = -1;
                    if (Icon != "")
                    {
                        catalogDownload.Icon = Icon;
                    }
                    catalogDownload.FriendlyUrl = Common.util.Function.ConvertFileName(CatalogDownloadName);
                    catalogDownload.Status = 1;
                    catalogDownload.Description = Description;
                    CatalogDownloadsBusiness.AddNew(catalogDownload);
                }
                else
                {
                    var catalogDownload = CatalogDownloadsBusiness.GetById(id);
                    catalogDownload.CatalogDownloadName = CatalogDownloadName;
                    if (Icon != "")
                    {
                        catalogDownload.Icon = Icon;
                    }
                    catalogDownload.Status = 1;
                    catalogDownload.FriendlyUrl = Common.util.Function.ConvertFileName(CatalogDownloadName);

                    catalogDownload.Description = Description;
                    CatalogDownloadsBusiness.Edit(catalogDownload);
                }
                Response.Redirect("/Manage/Downloads/IndexCatalogDownloads");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/Downloads/IndexCatalogDownloads");
            }
            return View();
        }

        public ActionResult DeleteCatalogDownloads(long id)
        {
            try
            {
                if (id > 0)
                {
                    //Thay đổi trạng thái danh mục
                    CatalogDownloadsBusiness.Remove(id);
                    //status: 1(active), 0(private)
                }
                return RedirectToAction("IndexCatalogDownloads");
            }
            catch (Exception)
            {
                //write log
                throw;
            }
        }

        #endregion CatalogDownloads
    }
}