using Business.ClassBusiness;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class AlbumsController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private AlbumsBusiness albumsBusiness = new AlbumsBusiness();
        private AlbumImagesBusiness albumImagesBusiness = new AlbumImagesBusiness();

        public AlbumsController()
        {
            Album img;
            //img
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

                var list = albumsBusiness.GetList(key).ToPagedList(currentPageIndex, 20);

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
        public ActionResult Add(Album obj)
        {
            try
            {
                obj.CreateDate = DateTime.Now;
                obj.ModifyDate = DateTime.Now;

                albumsBusiness.AddNew(obj);
                Response.Redirect("/Manage/Albums/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/Albums/Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long Id)
        {
            try
            {
                var dbCatalog = albumsBusiness.GetById(Id);
                return View(dbCatalog);
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(long Id, string Title, string Code, int Status, string Attribute, string Description, string Order)
        {
            try
            {
                var dbCatalog = albumsBusiness.GetById(Id);
                dbCatalog.Attribute = Attribute;
                dbCatalog.Code = Code;
                dbCatalog.Description = Description;
                dbCatalog.ModifyDate = DateTime.Now;
                dbCatalog.Status = Status;
                dbCatalog.Title = Title;
                if (!string.IsNullOrEmpty(Order))
                    dbCatalog.Order = int.Parse(Order);
                albumsBusiness.Edit(dbCatalog);
                Response.Redirect("/Manage/Albums/Index");
            }
            catch
            {
                Response.Redirect("/Manage/Albums/Index");
            }
            return View();
        }

        public JsonResult GetAlbum(long id)
        {
            try
            {
                var dbCatalog = albumsBusiness.GetById(id);
                return Json(dbCatalog, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0);//Cập nhật thành công
            }
        }

        public ActionResult DeleteAlbum(long id)
        {
            try
            {
                if (id > 0)
                {
                    //Thay đổi trạng thái danh mục
                    albumsBusiness.Remove(id);
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

        public ActionResult UpdateImages(long Id, long? imgId, string Title, string Link, string Target, string Desctiption, string Order, string file, string btnSubmit)
        {
            if (btnSubmit == "Thêm ảnh")
            {
                AlbumImage imgImage = new AlbumImage();
                imgImage.Title = Title;
                imgImage.Target = Target;
                imgImage.Link = Link;
                imgImage.AlbumId = Id;
                imgImage.Desctiption = Desctiption;
                if (!string.IsNullOrEmpty(Order))
                    imgImage.Order = int.Parse(Order);

                #region image

                if (file != "")
                {
                    imgImage.ImageSource = file;
                }

                #endregion image

                albumImagesBusiness.AddNew(imgImage);
            }
            else if (btnSubmit == "Cập nhật ảnh")
            {
                AlbumImage imgImage = albumImagesBusiness.GetById((long)imgId);
                imgImage.Title = Title;
                imgImage.Target = Target;
                imgImage.Link = Link;
                imgImage.AlbumId = Id;
                imgImage.Desctiption = Desctiption;
                if (!string.IsNullOrEmpty(Order))
                    imgImage.Order = int.Parse(Order);

                if (file != "")
                {
                    imgImage.ImageSource = file;
                }
                albumImagesBusiness.Edit(imgImage);
            }
            Response.Redirect("~/Manage/Albums/Edit/" + Id);
            return View();
        }

        public ActionResult DeleteImage(long id, long imgid)
        {
            try
            {
                if (imgid > 0)
                {
                    //Thay đổi trạng thái danh mục
                    albumImagesBusiness.Remove(imgid);
                    //status: 1(active), 0(private)
                }
                Response.Redirect("/Manage/Albums/Edit/" + id);
            }
            catch (Exception)
            {
                //write log
                throw;
            }
            return View();
        }

        #endregion Category
    }
}