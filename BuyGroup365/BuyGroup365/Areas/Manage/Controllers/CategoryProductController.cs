using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using BuyGroup365.Controllers;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class CategoryProductController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private CatalogsBusiness _catalogsBusiness = new CatalogsBusiness();
        private readonly LoadCombo _loadCombo = new LoadCombo();
        private readonly ManufacturersBusiness _manufacturersBusiness = new ManufacturersBusiness();
        private readonly Catalogs_ManufacturersBusiness _catalogsManufacturersBusiness = new Catalogs_ManufacturersBusiness();
        private readonly CatalogPropertiesBusiness _catalogPropertiesBusiness = new CatalogPropertiesBusiness();
        private readonly CatalogPropertiesValueBusiness _catalogPropertiesValueBusiness = new CatalogPropertiesValueBusiness();
        private readonly FriendlyUrlBusiness _friendlyUrlBusines = new FriendlyUrlBusiness();
        public static List<Catalog> ListCate = null;
        public static List<Manufacturer> ListManufacturers = null;
        private string duoilink = System.Configuration.ConfigurationSettings.AppSettings["ExtensionLink"];

        public CategoryProductController()
        {
            if (ListCate == null)
            {
                ListCate = _catalogsBusiness.GetDynamicQuery().ToList();
            }
            if (ListManufacturers == null)
            {
                ListManufacturers = _manufacturersBusiness.GetDynamicQuery().ToList();
            }
        }

        #endregion Khai bao biến

        #region Category

        public ActionResult Index(string key, int? page)
        {
            try
            {
                ViewBag.statusCatg = _loadCombo.InitSelectListItemStatusCreateCategory();
                ViewBag.PromotionListID = _loadCombo.InitSelectListItemStatusNewsGroup();
                List<LoadDropdown.DropdowCate> listDropdowCate = new List<LoadDropdown.DropdowCate>();
                ViewBag.parent = _loadCombo.SearchCategoryByName(ref listDropdowCate);

                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;
                List<Catalog> listcate = new List<Catalog>();
                var listobj = _catalogsBusiness.SearchCategoryByName(ListCate);
                //    var list = _catalogsBusiness.GetDynamicQuery().ToList().ToPagedList(currentPageIndex, 20);
                var list = listobj.ToPagedList(currentPageIndex, 20);

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
        public ActionResult CreateCate(string categoryname, HttpPostedFileBase icon, HttpPostedFileBase Banner, int statusCatg, int? PromotionListID, int parent, string seoTitle, string seoKeyword, string seoDescription, string description, string ShareTitle, string ShareKeyword, string ShareDescription, string ImageSource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string friendly = "";
                    Random rd = new Random();
                    Catalog obj = new Catalog();
                    // obj.FriendlyUrl = Function.ConvertFileName(categoryname);
                    friendly = Function.ConvertFileName(categoryname) + duoilink;
                    obj.FriendlyUrl = friendly;
                    obj.ParentId = parent;
                    obj.Status = statusCatg;
                    obj.CatalogName = categoryname;
                    obj.Code = rd.Next(100000, 999999).ToString();
                    if (icon != null && icon.ContentLength > 0)
                    {
                        // TourInfo entity=new TourInfo();
                        //Random rdImage = new Random();
                        string randomImage = Guid.NewGuid().ToString();
                        //string fileNameImage = Common.util.Function.ConvertFileName(icon.FileName);
                        string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                        var strurlimage = Common.util.Function.ResizeImageNew(icon, 300, 300, pathImage, randomImage);
                        Common.util.Function.ResizeImageNew(icon, 500, 500, pathImage, randomImage);
                        Common.util.Function.ResizeImageNew(icon, 1000, 1000, pathImage, randomImage);
                        //  Common.util.Function.ReSizeImage(pathImage, fileNameImage, randomImage, 1000, 1000, icon);
                        obj.Icon = strurlimage;
                    }
                    if (Banner != null && Banner.ContentLength > 0)
                    {
                        // TourInfo entity=new TourInfo();
                        //Random rdImage = new Random();
                        string randomImage = Guid.NewGuid().ToString();
                        //string fileNameImage = Common.util.Function.ConvertFileName(icon.FileName);
                        string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                        var strurlimage = Common.util.Function.ResizeImageNew(Banner, 1500, 1500, pathImage, randomImage);
                        //Common.util.Function.ResizeImageNew(Banner, 500, 500, pathImage, randomImage);
                        //Common.util.Function.ResizeImageNew(Banner, 1000, 1000, pathImage, randomImage);
                        //  Common.util.Function.ReSizeImage(pathImage, fileNameImage, randomImage, 1000, 1000, icon);
                        obj.Banner = strurlimage;
                    }
                    if (ImageSource != "")
                    {
                        obj.ImageSource = ImageSource;
                    }
                    obj.SeoKeyword = seoKeyword;
                    obj.SeoTitle = seoTitle;
                    obj.SeoDescription = seoDescription;
                    obj.ShareTitle = ShareTitle;
                    obj.ShareKeyword = ShareKeyword;
                    obj.ShareDescription = ShareDescription;
                    obj.Description = description;
                    //obj.PromotionID = PromotionListID;

                    long Catalogid;
                    _catalogsBusiness.AddNew(obj);
                    Catalogid = obj.Id;
                    try
                    {
                        Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();
                        Random rnd = new Random();
                        int ngaunhien = rnd.Next(1, 100);
                        friendlyUrl.ItemId = Catalogid;
                        friendlyUrl.Link = friendly;
                        friendlyUrl.ControllerName = "Home";
                        friendlyUrl.ActionName = "Category";
                        friendlyUrl.NameLink = friendly + ngaunhien.ToString();
                        friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                        friendlyUrl.Order = 0;

                        _friendlyUrlBusines.InsertLink(friendlyUrl);
                        RefreshFriendly.BindataSiteUrl();
                    }
                    catch { }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Tạo mới không thành công");
                    return View();
                }
            }
            catch (Exception ex)
            {
                //witrelog
                throw;
            }
        }

        public ActionResult DeleteCategory(long id)
        {
            try
            {
                if (id > 0)
                {
                    //Thay đổi trạng thái danh mục
                    _catalogsBusiness.Remove(id);
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

        public JsonResult GetManuface(long id)
        {
            try
            {
                var obj = _catalogsManufacturersBusiness.GetDynamicQuery().Where(x => x.CatalogId == id);
                if (obj.Any())
                {
                    var htm = "";
                    var index = 1;
                    foreach (var item in obj)
                    {
                        htm += "<tr>" +
                               "<td  style=\"text-align: center\">" + index + "</td> " +
                               "<td>" + item.Manufacturer.ManufacturerName + "</td>" +
                                "<td  style=\"text-align: center\"><input type=\"checkbox\" onclick=\"setValue('" + item.Id + "')\" id=\"ck" + item.Id + "\" name=\"inbox\" value=\"" + item.Id + "\"/></td>" +
                               "</tr>";
                        index++;
                    }
                    return Json(htm);
                }
                else
                {
                    return Json(2);
                }
            }
            catch (Exception)
            {
                //witre long
                return Json(0);
                //    throw;
            }
        }

        public JsonResult AddManuToCategory(long cateId, long manufaceid)
        {
            try
            {
                Catalogs_Manufacturers obj = new Catalogs_Manufacturers();
                obj.CatalogId = cateId;
                obj.ManufacturerId = manufaceid;
                _catalogsManufacturersBusiness.AddNew(obj);

                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
                //write log
                throw;
            }
        }

        public JsonResult DeleteCateManu(string[] array)
        {
            try
            {
                if (array != null && array.Any())
                {
                    foreach (var s in array)
                    {
                        _catalogsManufacturersBusiness.Remove(long.Parse(s));
                    }
                    return Json(1);//thành công
                }
                else
                {
                    return Json(2);// chưa chon ô check
                }
            }
            catch (Exception)
            {
                return Json(0);//lỗi
                //write log
                throw;
            }
        }

        public ActionResult Properties(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _catalogPropertiesBusiness.GetDynamicQuery().ToList().ToPagedList(currentPageIndex, 20);

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

        public JsonResult GetProperty(long cateId)
        {
            try
            {
                var htm = "";
                var property = _catalogPropertiesBusiness.GetDynamicQuery().Where(x => x.CatalogId == cateId);
                CatalogsBusiness catabusinness = new CatalogsBusiness();
                string friendlyUrl = catabusinness.GetById(cateId).FriendlyUrl;
                foreach (var item in property)
                {
                    htm += "<div class=\"panel panel-default tht_panel  panlel_" + item.Id + "\">" +
                          "<div class=\"panel-heading\">" +
                          "<h3 class=\"panel-title\">" +
                          "  <input id=\"cataproname" + item.Id + "\" type=\"text\" value=\"" + item.Name + "\" />" +
                          " <input id=\"cataprokey" + item.Id + "\" type=\"text\" value=\"" + item.Key + "\" />";
                    if (item.TypeDesign == 1)
                    {
                        if (item.Type == 1)
                        {
                            htm += "<label style=\"display:none;\"><input type=\"checkbox\"  id =\"typeproperty" + item.Id + "\" checked value=\"" + item.Type + "\">Cho phép chọn nhiều? </label><input type=\"checkbox\" id =\"typedesignproperty" + item.Id + "\" checked value=\"" + item.TypeDesign + "\">Hiển thị 1 dòng </label><a class=\"del_1\" onclick=\"UpdateProperties(" + item.Id + ")\">Sửa</a><a class=\"del_1\" onclick=\"DeleteProperties(" + item.Id + ")\">Xóa</a></div>" +
                               "<div class=\"panel-body\">";
                        }
                        else
                        {
                            htm += "<label style=\"display:none;\"><input type=\"checkbox\"  id =\"typeproperty" + item.Id + "\" value=\"" + item.Type + "\">Cho phép chọn nhiều? </label><input type=\"checkbox\" id =\"typedesignproperty" + item.Id + "\" checked value=\"" + item.TypeDesign + "\">Hiển thị 1 dòng </label><a class=\"del_1\" onclick=\"UpdateProperties(" + item.Id + ")\">Sửa</a><a class=\"del_1\" onclick=\"DeleteProperties(" + item.Id + ")\">Xóa</a></div>" +
                                                           "<div class=\"panel-body\">";
                        }
                    }
                    else
                    {
                        if (item.Type == 1)
                        {
                            htm += "<label style=\"display:none;\"><input type=\"checkbox\"   id =\"typeproperty" + item.Id + "\" checked  value=\"" + item.Type + "\">Cho phép chọn nhiều? </label><input type=\"checkbox\" id =\"typedesignproperty" + item.Id + "\"  value=\"" + item.TypeDesign + "\">Hiển thị 1 dòng </label><a class=\"del_1\" onclick=\"UpdateProperties(" + item.Id + ")\">Sửa</a><a class=\"del_1\" onclick=\"DeleteProperties(" + item.Id + ")\">Xóa</a></div>" +
                                                  "<div class=\"panel-body\">";
                        }
                        else
                        {
                            htm += "<label style=\"display:none;\"><input type=\"checkbox\"   id =\"typeproperty" + item.Id + "\" value=\"" + item.Type + "\">Cho phép chọn nhiều? </label><input type=\"checkbox\" id =\"typedesignproperty" + item.Id + "\"  value=\"" + item.TypeDesign + "\">Hiển thị 1 dòng </label><a class=\"del_1\" onclick=\"UpdateProperties(" + item.Id + ")\">Sửa</a><a class=\"del_1\" onclick=\"DeleteProperties(" + item.Id + ")\">Xóa</a></div>" +
                                                 "<div class=\"panel-body\">";
                        }
                    }
                    foreach (var item2 in item.CatalogPropertiesValues)
                    {
                        htm += "<p id=\"properties_value_" + item2.Id + "\">+  <input id=\"properties_value_edit_" + item2.Id + "\" type=\"text\" value=\"" + item2.Value + "\" /><a class=\"update_2\" onclick=\"UpdatePropertiesValue(" + item2.Id + ")\">Sửa</a><a class=\"del_2\" onclick=\"DeletePropertiesValue(" + item2.Id + ")\">Xóa</a></p>";
                    }
                    htm += " <div id=\"valueUpdate\"></div> <div style=\"float:right\"><a onclick=\"AddvalueFormUpdate(" + item.Id + ")\" class=\"btn btn-primary btn-sm\">Thêm giá trị</a></div></div></div>";
                }
                return Json(htm);
            }
            catch (Exception)
            {
                //write log
                return Json(0);
                throw;
            }
        }

        public JsonResult AddProperty(string nameproperty, int typevalue, int typedesgin, long cate, string allvalue)
        {
            try
            {
                CatalogProperty objCatalogProperty = new CatalogProperty();
                objCatalogProperty.CatalogId = cate;
                objCatalogProperty.Type = typevalue;
                objCatalogProperty.TypeDesign = typedesgin;
                objCatalogProperty.Name = nameproperty;
                objCatalogProperty.Key = Function.ConvertFileName(nameproperty);
                var vl = allvalue.TrimEnd('|');
                var arrValue = vl.Split('|');
                List<CatalogPropertiesValue> catalogPropertiesValues = new List<CatalogPropertiesValue>();
                foreach (var s in arrValue)
                {
                    CatalogPropertiesValue objCatalogPropertiesValue = new CatalogPropertiesValue();
                    objCatalogPropertiesValue.Value = s;
                    catalogPropertiesValues.Add(objCatalogPropertiesValue);
                }
                objCatalogProperty.CatalogPropertiesValues = catalogPropertiesValues;
                _catalogPropertiesBusiness.AddNew(objCatalogProperty);

                string htm = "";

                htm += "<div class=\"panel panel-default tht_panel  panlel_" + objCatalogProperty.Id + "\">" +
                         "<div class=\"panel-heading\">" +
                         "<h3 class=\"panel-title\">" +
                        "  <input id=\"cataproname" + objCatalogProperty.Id + "\" type=\"text\" value=\"" + objCatalogProperty.Name + "\" />" +
                          " <input id=\"cataprokey" + objCatalogProperty.Id + "\" type=\"text\" value=\"" + objCatalogProperty.Key + "\" />";
                if (objCatalogProperty.TypeDesign == 1)
                {
                    if (objCatalogProperty.Type == 1)
                    {
                        htm += "<label ><input type=\"checkbox\" style=\"display:none;\" checked id =\"typeproperty\" value=\"" + objCatalogProperty.Type + "\">Cho phép chọn nhiều? </label><input type=\"checkbox\" id =\"typedesignproperty\" checked value=\"" + objCatalogProperty.TypeDesign + "\">Hiển thị 1 dòng </label><a class=\"del_1\" onclick=\"UpdateProperties(" + objCatalogProperty.Id + ")\">Sửa</a><a class=\"del_1\" onclick=\"DeleteProperties(" + objCatalogProperty.Id + ")\">Xóa</a></div>" +
                           "<div class=\"panel-body\">";
                    }
                    else
                    {
                        htm += "<label ><input type=\"checkbox\" style=\"display:none;\" id =\"typeproperty\" value=\"" + objCatalogProperty.Type + "\">Cho phép chọn nhiều? </label><input type=\"checkbox\" id =\"typedesignproperty\" checked value=\"" + objCatalogProperty.TypeDesign + "\">Hiển thị 1 dòng </label><a class=\"del_1\" onclick=\"UpdateProperties(" + objCatalogProperty.Id + ")\">Sửa</a><a class=\"del_1\" onclick=\"DeleteProperties(" + objCatalogProperty.Id + ")\">Xóa</a></div>" +
                           "<div class=\"panel-body\">";
                    }
                }
                else
                {
                    if (objCatalogProperty.Type == 1)
                    {
                        htm += "<label><input type=\"checkbox\" style=\"display:none;\" id =\"typeproperty\" checked value=\"" + objCatalogProperty.Type + "\">Cho phép chọn nhiều? </label><input type=\"checkbox\" id =\"typedesignproperty\"  value=\"" + objCatalogProperty.TypeDesign + "\">Hiển thị 1 dòng </label><a class=\"del_1\" onclick=\"UpdateProperties(" + objCatalogProperty.Id + ")\">Sửa</a><a class=\"del_1\" onclick=\"DeleteProperties(" + objCatalogProperty.Id + ")\">Xóa</a></div>" +
                                              "<div class=\"panel-body\">";
                    }
                    else
                    {
                        htm += "<label><input type=\"checkbox\" style=\"display:none;\" id =\"typeproperty\" value=\"" + objCatalogProperty.Type + "\">Cho phép chọn nhiều? </label><input type=\"checkbox\" id =\"typedesignproperty\"  value=\"" + objCatalogProperty.TypeDesign + "\">Hiển thị 1 dòng </label><a class=\"del_1\" onclick=\"UpdateProperties(" + objCatalogProperty.Id + ")\">Sửa</a><a class=\"del_1\" onclick=\"DeleteProperties(" + objCatalogProperty.Id + ")\">Xóa</a></div>" +
                                             "<div class=\"panel-body\">";
                    }
                }

                foreach (var item2 in objCatalogProperty.CatalogPropertiesValues)
                {
                    htm += "<p id=\"properties_value_" + item2.Id + "\">+  <input id=\"properties_value_edit_" + item2.Id + "\" type=\"text\" value=\"" + item2.Value + "\" /><a class=\"update_2\" onclick=\"UpdatePropertiesValue(" + item2.Id + ")\">Sửa</a><a class=\"del_2\" onclick=\"DeletePropertiesValue(" + item2.Id + ")\">Xóa</a></p>";
                }
                htm += " </div></div>";

                return Json(htm);//add susess
            }
            catch (Exception)
            {
                return Json(0);
                //write log
                throw;
            }
        }

        public JsonResult AddPropertyUpdate(long id, string propertyvalue)
        {
            try
            {
                CatalogPropertiesBusiness catalogPropertiesBusiness = new CatalogPropertiesBusiness();
                CatalogProperty catalogProperty = new CatalogProperty();
                catalogProperty = catalogPropertiesBusiness.GetById(id);

                CatalogPropertiesValue objCatalogPropertiesValue = new CatalogPropertiesValue();
                objCatalogPropertiesValue.Value = propertyvalue;
                objCatalogPropertiesValue.CatalogPropertieId = id;
                catalogProperty.CatalogPropertiesValues.Add(objCatalogPropertiesValue);
                catalogPropertiesBusiness.Edit(catalogProperty);

                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                return Json(0);
                //write log
                throw;
            }
        }

        public JsonResult DeleteProperty(long id)
        {
            try
            {
                CatalogPropertiesBusiness catalogPropertiesBusiness = new CatalogPropertiesBusiness();
                catalogPropertiesBusiness.Remove(id);
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }

        public JsonResult UpdateProperty(long id, string cataproname, string cataprokey, int typeproperty, int typedesignproperty)
        {
            try
            {
                CatalogPropertiesBusiness catalogPropertiesBusiness = new CatalogPropertiesBusiness();
                CatalogProperty catalogProperty = new CatalogProperty();
                catalogProperty = catalogPropertiesBusiness.GetById(id);
                catalogProperty.Key = cataprokey;
                catalogProperty.Type = typeproperty;
                catalogProperty.TypeDesign = typedesignproperty;
                catalogProperty.Name = cataproname;
                catalogPropertiesBusiness.Edit(catalogProperty);
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }

        public JsonResult DeletePropertyValue(long id)
        {
            try
            {
                CatalogPropertiesValueBusiness catalogPropertiesValueBusiness = new CatalogPropertiesValueBusiness();
                catalogPropertiesValueBusiness.Remove(id);
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }

        public JsonResult UpdatePropertyValue(long id, string properties_value_edit)
        {
            try
            {
                CatalogPropertiesValueBusiness catalogPropertiesValueBusiness = new CatalogPropertiesValueBusiness();
                CatalogPropertiesValue catalogPropertiesValue = new CatalogPropertiesValue();
                catalogPropertiesValue = catalogPropertiesValueBusiness.GetById(id);
                catalogPropertiesValue.Value = properties_value_edit;
                catalogPropertiesValueBusiness.Edit(catalogPropertiesValue);
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }

        #endregion Category

        #region Manufaceture

        public ActionResult Manufaceture(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _manufacturersBusiness.GetDynamicQuery().ToList().ToPagedList(currentPageIndex, 20);

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
        public ActionResult CreateManufaceture(string manufaceturename, string dicription, HttpPostedFileBase icon)
        {
            try
            {
                Manufacturer obj = new Manufacturer();
                obj.Code = Guid.NewGuid().ToString().Substring(0, 18);
                obj.ManufacturerName = manufaceturename;
                //     obj.Order = oder;
                obj.Description = dicription;
                if (icon != null && icon.ContentLength > 0)
                {
                    // TourInfo entity=new TourInfo();
                    //Random rdImage = new Random();
                    string randomImage = Guid.NewGuid().ToString();
                    string fileNameImage = Common.util.Function.ConvertFileName(icon.FileName);
                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                    var strurlimage = Common.util.Function.ResizeImageNew(icon, 300, 300, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(icon, 500, 500, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(icon, 1000, 1000, pathImage, randomImage);
                    obj.Icon = strurlimage;
                }
                _manufacturersBusiness.AddNew(obj);
                return RedirectToAction("Manufaceture");
            }
            catch (Exception ex)
            {
                // Write log
                throw;
            }
        }

        [HttpGet]
        public ActionResult EditManufaceture(long id)
        {
            try
            {
                Manufacturer obj = _manufacturersBusiness.GetById(id);

                return View(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditManufaceture(Common.Manufacturer manu, string manufaceturename, string dicription, HttpPostedFileBase icon)
        {
            try
            {
                Common.Manufacturer obj = _manufacturersBusiness.GetById(manu.Id);

                obj.ManufacturerName = manufaceturename;
                //     obj.Order = oder;
                obj.Description = dicription;
                if (icon != null && icon.ContentLength > 0)
                {
                    // TourInfo entity=new TourInfo();
                    //Random rdImage = new Random();
                    string randomImage = Guid.NewGuid().ToString();
                    string fileNameImage = Common.util.Function.ConvertFileName(icon.FileName);
                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                    var strurlimage = Common.util.Function.ResizeImageNew(icon, 300, 300, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(icon, 500, 500, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(icon, 1000, 1000, pathImage, randomImage);
                    obj.Icon = strurlimage;
                }
                _manufacturersBusiness.Edit(obj);
                return RedirectToAction("Manufaceture");
            }
            catch (Exception ex)
            {
                // Write log
                throw;
            }
        }

        public ActionResult DeleteManuface(long id)
        {
            try
            {
                _manufacturersBusiness.Remove(id);
                return RedirectToAction("Manufaceture");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Manufaceture

        public ActionResult UpdateCache()
        {
            ListCate = _catalogsBusiness.GetDynamicQuery().ToList();
            ListManufacturers = _manufacturersBusiness.GetDynamicQuery().ToList();
            MemberController.List = _catalogsBusiness.GetDynamicQuery().ToList();
            BuyGroup365.Controllers.MemberController.List = _catalogsBusiness.GetDynamicQuery().ToList();
            BuyGroup365.Controllers.MemberController.ListCatalogsManufacturerses = _catalogsManufacturersBusiness.GetDynamicQuery().ToList();
            BuyGroup365.Controllers.MemberController.ListCatalogProperties = _catalogPropertiesBusiness.GetDynamicQuery().ToList();
            BuyGroup365.Controllers.MemberController.ListCatalogPropertiesValues = _catalogPropertiesValueBusiness.GetDynamicQuery().ToList();

            return View();
        }

        [HttpGet]
        public ActionResult UpdateCatalog(long id)
        {
            var obj = _catalogsBusiness.GetById(id);

            List<LoadDropdown.DropdowCate> listDropdowCates = new List<LoadDropdown.DropdowCate>();
            ViewBag.parent = _loadCombo.SearchCategoryByName(ref listDropdowCates, obj.ParentId);
            ViewBag.statusProduct = _loadCombo.InitSelectListItemStatusCreateCategory();
            ViewBag.PromotionListID = _loadCombo.InitSelectListItemStatusNewsGroup(obj.PromotionID);

            Common.Catalog objentity = new Common.Catalog();
            objentity.Id = obj.Id;
            objentity.ParentId = obj.ParentId;
            objentity.Code = obj.Code;
            objentity.CatalogName = obj.CatalogName;
            objentity.FriendlyUrl = obj.FriendlyUrl;
            objentity.Icon = obj.Icon;
            objentity.ImageSource = obj.ImageSource;
            objentity.Banner = obj.Banner;
            objentity.Description = obj.Description;
            objentity.Order = obj.Order;
            objentity.Status = obj.Status;
            objentity.CreateDate = obj.CreateDate;
            objentity.ModifyDate = obj.ModifyDate;
            objentity.IsLast = obj.IsLast;
            objentity.SeoTitle = obj.SeoTitle;
            objentity.SeoKeyword = obj.SeoKeyword;
            objentity.SeoDescription = obj.SeoDescription;
            objentity.ShareTitle = obj.ShareTitle;
            objentity.ShareKeyword = obj.ShareKeyword;
            objentity.ShareDescription = obj.ShareDescription;
            objentity.PromotionID = obj.PromotionID;
            return View(objentity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateCatalog(Common.Catalog viewCatalog, long parent, long? PromotionListID, string FriendlyHD, HttpPostedFileBase Icon, HttpPostedFileBase Banner, string ImageSource)
        {
            try
            {
                string friendly = "";
                CatalogsBusiness catalogsBusiness = new CatalogsBusiness();
                var dbCatalog = catalogsBusiness.GetById(viewCatalog.Id);
                dbCatalog.ParentId = parent;
                dbCatalog.Code = viewCatalog.Code;
                dbCatalog.CatalogName = viewCatalog.CatalogName;

                friendly = Function.ConvertFileName(viewCatalog.CatalogName) + duoilink;

                dbCatalog.FriendlyUrl = friendly;
                if (Icon != null && Icon.ContentLength > 0)
                {
                    // TourInfo entity=new TourInfo();
                    //Random rdImage = new Random();
                    string randomImage = Guid.NewGuid().ToString();
                    //string fileNameImage = Common.util.Function.ConvertFileName(icon.FileName);
                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                    var strurlimage = Common.util.Function.ResizeImageNew(Icon, 300, 300, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(Icon, 500, 500, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(Icon, 1000, 1000, pathImage, randomImage);
                    //  Common.util.Function.ReSizeImage(pathImage, fileNameImage, randomImage, 1000, 1000, icon);
                    dbCatalog.Icon = strurlimage;
                }
                if (Banner != null && Banner.ContentLength > 0)
                {
                    // TourInfo entity=new TourInfo();
                    //Random rdImage = new Random();
                    string randomImage = Guid.NewGuid().ToString();
                    //string fileNameImage = Common.util.Function.ConvertFileName(icon.FileName);
                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                    var strurlimage = Common.util.Function.ResizeImageNew(Banner, 1500, 1500, pathImage, randomImage);
                    //Common.util.Function.ResizeImageNew(Banner, 500, 500, pathImage, randomImage);
                    //Common.util.Function.ResizeImageNew(Banner, 1000, 1000, pathImage, randomImage);
                    //  Common.util.Function.ReSizeImage(pathImage, fileNameImage, randomImage, 1000, 1000, icon);
                    dbCatalog.Banner = strurlimage;
                }
                if (ImageSource != "")
                {
                    dbCatalog.ImageSource = ImageSource;
                }
                dbCatalog.Description = viewCatalog.Description;
                dbCatalog.Order = viewCatalog.Order;
                dbCatalog.Status = viewCatalog.Status;
                //dbCatalog.PromotionID = PromotionListID;
                dbCatalog.IsLast = viewCatalog.IsLast;
                dbCatalog.SeoTitle = viewCatalog.SeoTitle;
                dbCatalog.SeoKeyword = viewCatalog.SeoKeyword;
                dbCatalog.SeoDescription = viewCatalog.SeoDescription;
                dbCatalog.ShareTitle = viewCatalog.ShareTitle;
                dbCatalog.ShareKeyword = viewCatalog.ShareKeyword;
                dbCatalog.ShareDescription = viewCatalog.ShareDescription;

                catalogsBusiness.Edit(dbCatalog);

                try
                {
                    Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();
                    Random rnd = new Random();
                    int ngaunhien = rnd.Next(1, 100);
                    string linkcu = "";

                    if (FriendlyHD != friendly)
                    {
                        linkcu = FriendlyHD;
                    }
                    else
                    {
                        linkcu = "";
                    }
                    friendlyUrl.ItemId = viewCatalog.Id;
                    friendlyUrl.Link = friendly;
                    friendlyUrl.ControllerName = "Home";
                    friendlyUrl.ActionName = "Category";
                    friendlyUrl.NameLink = friendly + ngaunhien;
                    friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                    friendlyUrl.Order = 0;

                    _friendlyUrlBusines.UpdateLink(linkcu, friendlyUrl);
                    RefreshFriendly.BindataSiteUrl();
                }
                catch { }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}