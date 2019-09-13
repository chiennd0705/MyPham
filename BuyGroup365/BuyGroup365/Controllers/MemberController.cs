﻿using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Controllers;
using BuyGroup365.Areas.Manage.Models;
using BuyGroup365.Models.Common;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using MvcPaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BuyGroup365.Controllers
{
    public class MemberController : Controller
    {
        #region Khai bao

        private readonly CatalogPropertiesBusiness _catalogPropertiesBusiness = new CatalogPropertiesBusiness();
        private readonly CatalogsBusiness _catalogsBusiness = new CatalogsBusiness();
        private readonly CatalogPropertiesValueBusiness _catalogPropertiesValueBusiness = new CatalogPropertiesValueBusiness();
        private readonly LoadCombo _loadCombo = new LoadCombo();
        private readonly MembersBusiness _membersBusiness = new MembersBusiness();
        private readonly OrdersBusiness _ordersBusiness = new OrdersBusiness();
        private readonly ProductImagesBusiness _productImagesBusiness = new ProductImagesBusiness();
        private readonly ProductPropertiesBusiness _productPropertiesBusiness = new ProductPropertiesBusiness();
        private readonly ProductsBusiness _productsBusiness = new ProductsBusiness();
        private readonly ShopPolicysBusiness _shopPolicysBusiness = new ShopPolicysBusiness();
        private readonly ShopSettingsBusiness _shopSettingsBusiness = new ShopSettingsBusiness();
        private readonly Catalogs_ManufacturersBusiness _catalogsManufacturersBusiness = new Catalogs_ManufacturersBusiness();
        private readonly ShopsBusiness _shopsBusiness = new ShopsBusiness();
        private readonly MemberCareProductBusiness _memberCareProductBusiness = new MemberCareProductBusiness();
        public static List<Catalog> List = null;
        public static List<Catalogs_Manufacturers> ListCatalogsManufacturerses = null;
        public static List<CatalogProperty> ListCatalogProperties = null;
        public static List<CatalogPropertiesValue> ListCatalogPropertiesValues = null;

        #endregion Khai bao

        public MemberController()
        {
            if (List == null)
            {
                List = _catalogsBusiness.GetDynamicQuery().ToList();
            }
            if (ListCatalogsManufacturerses == null)
            {
                ListCatalogsManufacturerses = _catalogsManufacturersBusiness.GetDynamicQuery().ToList();
            }
            if (ListCatalogProperties == null)
            {
                ListCatalogProperties = _catalogPropertiesBusiness.GetDynamicQuery().ToList();
            }
            if (ListCatalogPropertiesValues == null)
            {
                ListCatalogPropertiesValues = _catalogPropertiesValueBusiness.GetDynamicQuery().ToList();
            }
        }

        protected override void Initialize(RequestContext rc)
        {
            base.Initialize(rc);

            if (!SessionUtility.CheckLoginMember(Session))
            {
                Response.Redirect("/Login/Login?returnUrl=" + rc.HttpContext.Request.Url.PathAndQuery);
            }
        }

        #region LikeProduct

        public ActionResult LikeProduct(string key, int? page)
        {
            ViewBag.Mes = string.Empty;
            Member member = SessionUtility.GetSessionMember(Session);
            int currentPageIndex = page.HasValue ? page.Value : 1;
            IPagedList<MemberCareProduct> obj =
                _memberCareProductBusiness.GetDynamicQuery()
                    .Where(x => x.MemberId == member.Id)
                    .OrderByDescending(x => x.CreateDate)
                    .ToPagedList(currentPageIndex, 10);

            if (obj.Any())
            {
                //    obj=obj.ToPagedList(currentPageIndex, 20);
                return View(obj);
            }
            else
            {
                ViewBag.Mes = "Bạn chưa quan tâm tơi sản phẩm nào!";
                return View();
            }
            //     obj = (IPagedList<Order>)new List<Order>();
        }

        public ViewResult ProductCompare(string key, int? page)
        {
            ViewBag.Mes = string.Empty;
            //   Member member = SessionUtility.GetSessionMember(Session);
            int currentPageIndex = page.HasValue ? page.Value : 1;
            var mycookies = new NlCheckout().GetvalueAppsetting("my_cookies");
            var objcookies = Common.util.SessionUtility.ReadCookie(mycookies);

            var arr = new List<ProductItem>();
            if (objcookies != null)
            {
                arr = JsonConvert.DeserializeObject<List<ProductItem>>(objcookies);
            }
            IPagedList<ProductItem> obj = arr.ToPagedList(currentPageIndex, 10);

            if (obj.Any())
            {
                //    obj=obj.ToPagedList(currentPageIndex, 20);
                return View(obj);
            }
            else
            {
                ViewBag.Mes = "Chưa có sản phâm Compare!";
                return View();
            }
        }

        public JsonResult DisLike(long id)
        {
            Member member = SessionUtility.GetSessionMember(Session);
            try
            {
                var listObjLike =
              _memberCareProductBusiness.GetDynamicQuery().Where(x => x.MemberId == member.Id && x.Id == id).ToList();
                if (listObjLike.Any())
                {
                    _memberCareProductBusiness.Remove(id);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(2, JsonRequestBehavior.AllowGet);// Không thuộc phạm vi của ban
                }
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);// Lỗi hệ thống
                //write log ở đây
            }
        }

        #endregion LikeProduct

        public new ActionResult Profile()
        {
            ViewBag.Mes = string.Empty;

            try
            {
                if (Session != null)
                {
                    Member entity = SessionUtility.GetSessionMember(Session);
                    LoadDropdown loadDropdown = new LoadDropdown();
                    ViewBag.Location = loadDropdown.SearchLocationParenId(1, entity.MemberProfile.LocationId);
                    Member obj = _membersBusiness.GetById(entity.Id);
                    return View(obj);
                }
                return RedirectToRoute(new { controller = "Login", action = "Login" });
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Login", action = "Login" });
            }
        }

        [HttpPost]
        public ActionResult Profile(Member obj, long receiverPhuong, int gender, HttpPostedFileBase avatarmember, int day, int mouth, int year)
        {
            LoadDropdown loadDropdown = new LoadDropdown();
            ViewBag.Location = loadDropdown.SearchLocationParenId(1, receiverPhuong);
            var check = Util.CheckFileImage(avatarmember);
            if (check == 0)
            {
                ViewBag.Mes = "File quá lớn";
                return View(obj);
            }
            else if (check == 1)
            {
                ViewBag.Mes = "File không đúng định dạng";
                return View(obj);
            }
            Member entity = _membersBusiness.GetById(obj.Id);
            entity.MemberProfile.LastName = obj.MemberProfile.LastName;
            entity.MemberProfile.FirstName = obj.MemberProfile.FirstName;
            entity.MemberProfile.Phone = obj.MemberProfile.Phone;
            if (day == -1 || mouth == -1 || year == -1)
            {
                ViewBag.Mes = "Cập nhật không thành công vui lòng chọn ngày, tháng , năm sinh!";

                return View(entity);
            }
            else
            {
                string datepicker1 = mouth + "/" + day + "/" + year;
                entity.MemberProfile.Dob = DateTime.Parse(datepicker1);
            }

            entity.MemberProfile.Address = obj.MemberProfile.Address;
            entity.Question = obj.Question;
            entity.Answer = obj.Answer;
            entity.MemberProfile.Sex = gender;
            entity.MemberProfile.LocationId = receiverPhuong;

            if (avatarmember != null && avatarmember.ContentLength > 0)
            {
                string randomImage = Guid.NewGuid().ToString();
                string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                string strurlimage = Function.ResizeImageNew(avatarmember, 300, 300, pathImage, randomImage);
                Function.ResizeImageNew(avatarmember, 500, 500, pathImage, randomImage);
                Function.ResizeImageNew(avatarmember, 1000, 1000, pathImage, randomImage);
                entity.MemberProfile.Photo = strurlimage;
            }
            _membersBusiness.Edit(entity);
            SessionUtility.SetSessionMember(entity, Session);
            ViewBag.Mes = "Cập nhật thông tin cá nhân thành công!";

            return View(entity);
        }

        /// <summary>
        /// lấy danh sách vùng miền
        /// </summary>
        /// <param name="type">type: mb sử dụng cho thành viên, type= sp : sữ dụng cho shop</param>
        /// <returns></returns>
        public JsonResult LoadLocation(string type)
        {
            if (type == "mb")
            {
                Member entity = SessionUtility.GetSessionMember(Session);
                var obj = new LoadDropdown().SearchLocationParentByChild(entity.MemberProfile.LocationId);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Member entity = SessionUtility.GetSessionMember(Session);
                var shop = _shopsBusiness.GetById(entity.Id);
                var obj = new LoadDropdown().SearchLocationParentByChild(shop.LocationId);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckPass(string pass)
        {
            Member entity = SessionUtility.GetSessionMember(Session);
            var pas = Common.util.Common.GetMd5Sum(pass);
            if (entity.Password != pas)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePass(string pass)
        {
            Member entity = SessionUtility.GetSessionMember(Session);
            var pas = Common.util.Common.GetMd5Sum(pass);

            var member = _membersBusiness.GetById(entity.Id);
            member.Password = pas;
            _membersBusiness.Edit(member);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Invoice(string key, int? page)
        {
            //Member member = SessionUtility.GetSessionMember(Session);
            //List<Order> obj = _ordersBusiness.GetDynamicQuery().Where(x => x.IdShop == member.Id).ToList();
            //return View(obj);
            ViewBag.Mes = string.Empty;
            Member member = SessionUtility.GetSessionMember(Session);
            int currentPageIndex = page.HasValue ? page.Value : 1;
            IPagedList<Order> obj =
                _ordersBusiness.GetDynamicQuery()
                    .Where(x => x.IdShop == member.Id)
                    .OrderByDescending(x => x.CreateDate)
                    .ToPagedList(currentPageIndex, 10);

            if (obj.Any())
            {
                //    obj=obj.ToPagedList(currentPageIndex, 20);
                return View(obj);
            }
            else
            {
                ViewBag.Mes = "Chưa có hóa đơn";
                return View();
            }
            //     obj = (IPagedList<Order>)new List<Order>();
        }

        public ActionResult EditProduct(long id)
        {
            try
            {
                Member member = SessionUtility.GetSessionMember(Session);
                var memberproduct =
                    _productsBusiness.GetDynamicQuery().Where(x => x.MemberId == member.Id && x.Id == id);
                if (memberproduct.Any())
                {
                    var loadDropdown = new LoadDropdown();
                    var listDropdowCate = new List<LoadDropdown.DropdowCate>();
                    ViewBag.stateProduct = loadDropdown.SearchCategoryByName(ref listDropdowCate);

                    Product obj = _productsBusiness.GetById(id);
                    ViewBag.stateProducttwo = _loadCombo.InitSelectListItemState(obj.State);
                    ViewBag.Manufaceture = loadDropdown.LoadComBoManufaceture(obj.CatalogId, obj.ManufacturerId);
                    string html = HtmlCate(-1, obj.CatalogId);
                    ViewBag.htmlCate = html;
                    return View(obj);
                }
                else
                {
                    return RedirectToAction("Error", "Notification", new { mes = "areas" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetListParent(long id)
        {
            var listid = new List<long>();
            List<long> obj = FuntionMember.GetIdPrentById(id, ref listid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditProduct(Product obj, long categoryproduct,
            long manufaceproduct, HttpPostedFileBase avatar, string description, HttpPostedFileBase[] file,
            string[] proprertyvalue)
        {
            try
            {
                Member member = SessionUtility.GetSessionMember(Session);
                var memberproduct =
                    _productsBusiness.GetDynamicQuery().Where(x => x.MemberId == member.Id && x.Id == obj.Id);
                if (memberproduct.Any())
                {
                    using (var ts = new TransactionScope())
                    {
                        Product entity = _productsBusiness.GetById(obj.Id);
                        entity.CatalogId = categoryproduct;
                        entity.ManufacturerId = manufaceproduct;
                        entity.ProductProperties = obj.ProductProperties;
                        entity.ProductName = obj.ProductName;
                        entity.Model = obj.Model;
                        entity.Price = obj.Price;
                        entity.Summary = obj.Summary;
                        entity.Description = Server.HtmlDecode(description);
                        entity.Cost = obj.Cost;
                        entity.Weight = obj.Weight;
                        entity.TypeOfCurrency = obj.TypeOfCurrency;
                        entity.State = obj.State; // Check lỗi này
                        //TODDO
                        entity.Tags = obj.Tags;
                        Product objMes = _productsBusiness.GetById(obj.Id);// dùng đê bind lên view khi xảy ra lỗi
                        var loadDropdown = new LoadDropdown();
                        var listDropdowCate = new List<LoadDropdown.DropdowCate>();
                        ViewBag.stateProduct = loadDropdown.SearchCategoryByName(ref listDropdowCate);
                        ViewBag.stateProducttwo = _loadCombo.InitSelectListItemState(obj.State);
                        ViewBag.Manufaceture = loadDropdown.LoadComBoManufaceture(obj.CatalogId, obj.ManufacturerId);
                        string html = HtmlCate(-1, obj.CatalogId);
                        ViewBag.htmlCate = html;
                        List<ProductImage> productImages = entity.ProductImages.ToList();
                        //End
                        //  productImages
                        if (avatar != null && avatar.ContentLength > 0)
                        {
                            var check = Util.CheckFileImage(avatar);
                            if (check == 0)
                            {
                                //   Product objMes = _productsBusiness.GetById(obj.Id);
                                ViewBag.Mes = "File quá lớn";
                                return View(objMes);
                            }
                            else if (check == 1)
                            {
                                ViewBag.Mes = "File không đúng định dạng";
                                return View(objMes);
                            }
                            ProductImage productImage = entity.ProductImages.First(x => x.IsAvatar == 1);
                            string randomImage = Guid.NewGuid().ToString();
                            string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                            string strurlimage = Function.ResizeImageNew(avatar, 300, 300, pathImage, randomImage);
                            Function.ResizeImageNew(avatar, 500, 500, pathImage, randomImage);
                            Function.ResizeImageNew(avatar, 1000, 1000, pathImage, randomImage);
                            productImage.ImgSource = strurlimage;
                            productImages.Add(productImage);
                        }
                        if (file != null)
                        {
                            foreach (HttpPostedFileBase item in file)
                            {
                                if (item != null && item.ContentLength > 0)
                                {
                                    var check = Util.CheckFileImage(item);
                                    if (check == 0)
                                    {
                                        ViewBag.Mes = "File quá lớn";
                                        return View(objMes);
                                    }
                                    else if (check == 1)
                                    {
                                        ViewBag.Mes = "File không đúng định dạng";
                                        return View(objMes);
                                    }
                                    var productImage = new ProductImage();
                                    string randomImage = Guid.NewGuid().ToString();
                                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                                    string strurlimage = Function.ResizeImageNew(item, 300, 300, pathImage, randomImage);
                                    Function.ResizeImageNew(item, 500, 500, pathImage, randomImage);
                                    Function.ResizeImageNew(item, 1000, 1000, pathImage, randomImage);
                                    productImage.ImgSource = strurlimage;
                                    productImages.Add(productImage);
                                }
                            }
                        }
                        entity.ProductImages = productImages;

                        #region Property

                        ICollection<ProductProperty> listproduc = entity.ProductProperties;
                        foreach (ProductProperty productProperty in listproduc)
                        {
                            _productPropertiesBusiness.Remove(productProperty.Id);
                        }
                        var productProperties = new List<ProductProperty>();

                        if (proprertyvalue != null)
                        {
                            List<string> listobjproperty = proprertyvalue.ToList();
                            foreach (string item in listobjproperty)
                            {
                                string[] splitobj = item.Split('|');
                                CatalogProperty cateproperty = _catalogPropertiesBusiness.GetById(long.Parse(splitobj[0]));
                                for (int i = 1; i < splitobj.Count(); i++)
                                {
                                    var valuepro = splitobj[i].Split('#');
                                    var productProperty = new ProductProperty
                                    {
                                        Name = cateproperty.Name,
                                        Key = cateproperty.Key,
                                        Value = valuepro[1],
                                        CatalogPropertyValueId = long.Parse(valuepro[0]),
                                        ProductId = entity.Id
                                    };
                                    productProperties.Add(productProperty);
                                }
                            }
                        }

                        #endregion Property

                        _productPropertiesBusiness.AddRange(productProperties);
                        _productsBusiness.Edit(entity);
                        ts.Complete();
                        return RedirectToAction("ProductList");
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Notification", new { mes = "areas" });
                }
            }
            catch (Exception ex)
            {
                //Write log
                throw;
            }
        }

        private readonly CardListBussiness _cardListBussiness = new CardListBussiness();

        #region Shop

        public ActionResult Shop()
        {
            Member member = SessionUtility.GetSessionMember(Session);
            Shop obj = _shopsBusiness.GetById(member.Id);
            LoadDropdown loadDropdown = new LoadDropdown();
            ViewBag.Location = loadDropdown.SearchLocationParenId(1, obj.LocationId);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Shop(Shop obj, long receiverPhuong)
        {
            LoadDropdown loadDropdown = new LoadDropdown();
            ViewBag.Location = loadDropdown.SearchLocationParenId(1, obj.LocationId);
            try
            {
                Shop entity = _shopsBusiness.GetById(obj.Id);
                entity.Address = obj.Address;
                entity.Phone = obj.Phone;
                entity.ShopName = obj.ShopName;
                entity.LocationId = receiverPhuong;
                ShopSupport shopSupport = entity.ShopSupport;
                shopSupport.SupportName = obj.ShopSupport.SupportName;
                shopSupport.Mobile = obj.ShopSupport.Mobile;
                shopSupport.Phone = obj.ShopSupport.Phone;
                shopSupport.Skype = obj.ShopSupport.Skype;
                shopSupport.Yahoo = obj.ShopSupport.Yahoo;
                shopSupport.Email = obj.ShopSupport.Email;
                shopSupport.Facebook = obj.ShopSupport.Facebook;
                entity.ShopSupport = shopSupport;
                _shopsBusiness.Edit(entity);
                ViewBag.Mes = "Cập nhật Shop thành công!";

                return View(obj);
            }
            catch (FaultException ex)
            {
                string[] exep = Function.GetExeption(ex);
                string codeExp = exep[1];
                ViewBag.Mes = codeExp;

                return View(obj);
            }
        }

        public ActionResult ShopPolicy()
        {
            Member member = SessionUtility.GetSessionMember(Session);
            ShopPolicy obj = _shopPolicysBusiness.GetById(member.Id);
            if (obj != null)
            {
                return View(obj);
            }
            var shopPolicy = new ShopPolicy();
            return View(shopPolicy);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ShopPolicy(string about, string privatepolicy, string salespolicy, string paymentspolicy)
        {
            Member member = SessionUtility.GetSessionMember(Session);
            ShopPolicy obj = _shopPolicysBusiness.GetById(member.Id);
            if (obj != null)
            {
                obj.About = Server.HtmlDecode(about);
                obj.PrivacyPolicy = Server.HtmlDecode(privatepolicy);
                obj.SalesPolicy = Server.HtmlDecode(salespolicy);
                obj.PaymentPolicy = Server.HtmlDecode(paymentspolicy);
                _shopPolicysBusiness.Edit(obj);
                return View(obj);
            }
            var shopPolicy = new ShopPolicy
            {
                Id = member.Id,
                About = Server.HtmlDecode(about),
                PrivacyPolicy = Server.HtmlDecode(privatepolicy),
                SalesPolicy = Server.HtmlDecode(salespolicy),
                PaymentPolicy = Server.HtmlDecode(paymentspolicy)
            };
            _shopPolicysBusiness.AddNew(shopPolicy);
            return View(shopPolicy);
        }

        public ActionResult ShopSetting(string key, int? page)
        {
            try
            {
                Member member = SessionUtility.GetSessionMember(Session);
                ViewData["key"] = key;
                ViewBag.Mes = "";
                int currentPageIndex = page.HasValue ? page.Value : 1;
                IPagedList<ShopSetting> listSetting =
                    _shopSettingsBusiness.GetDynamicQuery()
                        .Where(x => x.ShopId == member.Id)
                        .OrderBy(x => x.Id)
                        .ToPagedList(currentPageIndex, 20);
                return View(listSetting);
            }
            catch (Exception)
            {
                return null;
                // throw;
            }
        }

        public JsonResult CreateShopSetting(string key, string value)
        {
            Member member = SessionUtility.GetSessionMember(Session);
            if (string.IsNullOrEmpty(key))
            {
                return Json(1, JsonRequestBehavior.AllowGet); //Vui long nhập key
            }
            if (string.IsNullOrEmpty(value))
            {
                return Json(2, JsonRequestBehavior.AllowGet); // vui long nhập vale
            }
            try
            {
                var entity = new ShopSetting { Key = key, Value = value, ShopId = member.Id };
                _shopSettingsBusiness.AddNew(entity);
                return Json(3, JsonRequestBehavior.AllowGet); // thêm được rồi e
            }
            catch (Exception)
            {
                return Json(4, JsonRequestBehavior.AllowGet); //Lỗi hệ thống
                // throw;
            }
        }

        public JsonResult EditShopsetting(long id)
        {
            Member member = SessionUtility.GetSessionMember(Session);
            IQueryable<ShopSetting> listShopsetting =
                _shopSettingsBusiness.GetDynamicQuery().Where(x => x.Id == id && x.ShopId == member.Id);
            if (listShopsetting.Any())
            {
                try
                {
                    var entity = new ShopSettingEntity
                    {
                        Key = listShopsetting.First().Key,
                        Value = listShopsetting.First().Value
                    };
                    return Json(entity, JsonRequestBehavior.AllowGet); // xóa thành công
                }
                catch (Exception)
                {
                    return Json(1, JsonRequestBehavior.AllowGet); // Lỗi hệ thống
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet); //Không xem đươc thông tin đâu, key này không fai của bạn
        }

        public JsonResult EditShopsettingUpdate(long id, string key, string value)
        {
            // var member = SessionUtility.GetSessionMember(Session);
            if (string.IsNullOrEmpty(key))
            {
                return Json(1, JsonRequestBehavior.AllowGet); //Vui long nhập key
            }
            if (string.IsNullOrEmpty(value))
            {
                return Json(2, JsonRequestBehavior.AllowGet); // vui long nhập vale
            }
            try
            {
                ShopSetting entity = _shopSettingsBusiness.GetById(id);
                entity.Key = key;
                entity.Value = value;
                _shopSettingsBusiness.Edit(entity);
                return Json(3, JsonRequestBehavior.AllowGet); // cập nhật được rồi e
            }
            catch (Exception)
            {
                return Json(4, JsonRequestBehavior.AllowGet); //Lỗi hệ thống
                // throw;
            }
        }

        public JsonResult DeleteShopSetting(long id)
        {
            Member member = SessionUtility.GetSessionMember(Session);
            IQueryable<ShopSetting> listShopsetting =
                _shopSettingsBusiness.GetDynamicQuery().Where(x => x.Id == id && x.ShopId == member.Id);
            if (listShopsetting.Any())
            {
                try
                {
                    _shopSettingsBusiness.Remove(id);
                    return Json(1, JsonRequestBehavior.AllowGet); // xóa thành công
                }
                catch (Exception)
                {
                    return Json(2, JsonRequestBehavior.AllowGet); // Lỗi hệ thống
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet); //Không xóa dc đâu e, dây k fai key của e
        }

        public class ShopSettingEntity
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        #endregion Shop

        #region Json

        public JsonResult DeleteImage(long id)
        {
            try
            {
                List<ProductImage> listobj = _productImagesBusiness.GetDynamicQuery().Where(x => x.Id == id).ToList();
                if (listobj.Any())
                {
                    _productImagesBusiness.Remove(id);
                    return Json(1); //Xóa thành công.
                }
                return Json(0); //Xóa k thành công.
            }
            catch (Exception ex)
            {
                return Json(0); //Xóa k thành công.
                throw;
            }
        }

        #endregion Json

        #region Đang bấn sản phẩm

        public ActionResult ProductList(string key, int? page, long? parent, int? stateProduct, int? statusProduct,
            int? day)
        {
            try
            {
                string id = Request.QueryString["st"];
                //   int status=0;
                ViewBag.Id = id;
                if (!string.IsNullOrEmpty(id))
                {
                    statusProduct = int.Parse(id);
                    //   status = int.Parse(id);
                    //stateProduct = -1;
                    //parent = -1;
                    //day = -1;
                }
                //else
                //{
                //    statusProduct = 2; //mặc đinh láy danh sach nhưng san phâm đang bán
                //}
                var listDropdowCate = new List<LoadDropdown.DropdowCate>();
                ViewBag.parent = new LoadDropdown().SearchCategoryByName(ref listDropdowCate);
                ViewBag.statusProduct = _loadCombo.InitSelectListItemStatus();
                ViewBag.stateProduct = _loadCombo.InitSelectListItemState(null);
                var listcate = new List<long>();
                if (parent != null)
                {
                    _catalogsBusiness.GetListCateByParent((long)parent, ref listcate, List);
                }
                Member member = SessionUtility.GetSessionMember(Session);
                ViewData["key"] = key;
                ViewBag.Mes = "";

                int currentPageIndex = page.HasValue ? page.Value : 1;
                var totallist = new List<Product>();
                IPagedList<Product> listproduct =
                    _productsBusiness.GetList(ref totallist, member.Id, key, listcate, stateProduct, statusProduct, day)
                        .OrderBy(x => x.Id)
                        .ToPagedList(currentPageIndex, 10);
                ViewBag.Number = totallist;
                return View(listproduct);
            }
            catch (Exception)
            {
                return null;
                // throw;
            }
        }

        public JsonResult ChangeNameProduct(long id, string name)
        {
            try
            {
                var obj = _productsBusiness.GetById(id);
                obj.ProductName = name;
                _productsBusiness.Edit(obj);
                return Json(1, JsonRequestBehavior.AllowGet);// Cập nhật thành công
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);//Lỗi hệ thống
            }
        }

        public ActionResult Postsale()
        {
            ViewBag.Mes = "";
            //  string obj = HtmlCate(-1, null);
            //    ViewBag.htmlCate = obj;
            //ViewBag.categoryproduct = _loadCombo.InitDropCategorys(1);
            //     ViewBag.categoryproductedit = _loadCombo.InitDropCategorys(1);
            // ViewBag.categoryproductParent = _loadCombo.InitDropCategorysParent();
            //  ViewBag.statusProduct = _loadCombo.InitSelectListItemStatus();
            ViewBag.stateProduct = _loadCombo.InitSelectListItemState(null);
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Postsale(string name, string model, long categoryproduct,
            long manufaceproduct, string pice, string cost, string weight, HttpPostedFileBase avatar, int stateProduct, string summary, string description, HttpPostedFileBase[] file,
            string[] proprertyvalue)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    pice = pice.Replace(",", "");
                    var pi = long.Parse(pice);
                    cost = cost.Replace(",", "");
                    var co = long.Parse(cost);
                    weight = weight.Replace(",", "");
                    var we = long.Parse(weight);
                    var objproduct = new Product
                    {
                        MemberId = SessionUtility.GetSessionMember(Session).Id,
                        ProductName = name,
                        Code = Guid.NewGuid().ToString(),
                        Status = 1,// sản phẩm chưa được duyệt
                        Model = model,
                        Cost = co,
                        CatalogId = categoryproduct,
                        Description = Server.HtmlDecode(description),
                        Summary = summary,
                        FriendlyUrl = Function.ConvertFileName(name),
                        ManufacturerId = manufaceproduct,
                        Tags = Function.ConvertFileNameNotVietNamce(name),

                        // Guid gd;
                        //  objproduct.MemberId = 0; //Fix
                        Price = pi,
                        State = stateProduct,
                        TypeOfCurrency = "VNĐ",// Fix code
                        Weight = we
                    };

                    #region Property

                    var productProperties = new List<ProductProperty>();

                    if (proprertyvalue != null)
                    {
                        List<string> listobjproperty = proprertyvalue.ToList();
                        foreach (string item in listobjproperty)
                        {
                            string[] splitobj = item.Split('|');
                            CatalogProperty cateproperty = _catalogPropertiesBusiness.GetById(long.Parse(splitobj[0]));
                            for (int i = 1; i < splitobj.Count(); i++)
                            {
                                var valuepro = splitobj[i].Split('#');
                                var productProperty = new ProductProperty
                                {
                                    Name = cateproperty.Name,
                                    Key = cateproperty.Key,
                                    Value = valuepro[1],
                                    CatalogPropertyValueId = long.Parse(valuepro[0]),
                                    CatalogPropertyId = cateproperty.Id,
                                    ValueUrl = Function.ConvertFileName(valuepro[1])
                                };
                                productProperties.Add(productProperty);
                            }
                        }
                    }

                    //for (int i = 1; i <= index; i++)
                    //{
                    //  var  productProperty=new ProductProperty();
                    //    var namepro = formCollections["nameproperty"+i];
                    //    var keypro = formCollections["keyproperty"+i];
                    //    var valpro = formCollections["valeproperty"+i];
                    //    productProperty.Name = namepro;
                    //    productProperty.Key = keypro;
                    //    productProperty.Value = valpro;
                    //    productProperties.Add(productProperty);
                    //}

                    #endregion Property

                    #region image

                    var productImages = new List<ProductImage>();
                    if (file != null)
                    {
                        foreach (HttpPostedFileBase item in file)
                        {
                            if (item != null && item.ContentLength > 0)
                            {
                                var check = Util.CheckFileImage(item);
                                if (check == 0)
                                {
                                    ViewBag.Mes = "File quá lớn";
                                    return View();
                                }
                                else if (check == 1)
                                {
                                    ViewBag.Mes = "File không đúng định dạng";
                                    return View();
                                }
                                var productImage = new ProductImage();
                                string randomImage = Guid.NewGuid().ToString();
                                string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                                string strurlimage = Function.ResizeImageNew(item, 300, 300, pathImage, randomImage);
                                Function.ResizeImageNew(item, 500, 500, pathImage, randomImage);
                                Function.ResizeImageNew(item, 1000, 1000, pathImage, randomImage);
                                productImage.ImgSource = strurlimage;
                                productImages.Add(productImage);
                            }
                        }
                    }
                    if (avatar != null && avatar.ContentLength > 0)
                    {
                        var check = Util.CheckFileImage(avatar);
                        if (check == 0)
                        {
                            ViewBag.Mes = "File quá lớn";
                            return View();
                        }
                        else if (check == 1)
                        {
                            ViewBag.Mes = "File không đúng định dạng";
                            return View();
                        }
                        var productImage = new ProductImage();
                        string randomImage = Guid.NewGuid().ToString();
                        string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                        string strurlimage = Function.ResizeImageNew(avatar, 300, 300, pathImage, randomImage);
                        Function.ResizeImageNew(avatar, 500, 500, pathImage, randomImage);
                        Function.ResizeImageNew(avatar, 1000, 1000, pathImage, randomImage);
                        productImage.ImgSource = strurlimage;
                        productImage.IsAvatar = 1;
                        productImages.Add(productImage);
                    }

                    #endregion image

                    objproduct.SeoKeyword = string.Empty; //Fix code
                    objproduct.SeoTitle = string.Empty; //fix code
                    objproduct.SeoDescription = string.Empty; //fix code
                    objproduct.ProductProperties = productProperties;
                    objproduct.ProductImages = productImages;
                    _productsBusiness.AddNew(objproduct);
                    ViewBag.Mes = "Đăng thành công!";
                    ts.Complete();
                    // return RedirectToAction("Postsale");
                    //  ViewBag.Mes = "";
                    //  string obj = HtmlCate(-1, null);
                    //    ViewBag.htmlCate = obj;
                    //ViewBag.categoryproduct = _loadCombo.InitDropCategorys(1);
                    //ViewBag.categoryproductedit = _loadCombo.InitDropCategorys(1);
                    //ViewBag.categoryproductParent = _loadCombo.InitDropCategorysParent();
                    //  ViewBag.statusProduct = _loadCombo.InitSelectListItemStatus();
                    ViewBag.stateProduct = _loadCombo.InitSelectListItemState(null);
                    return View();
                }
            }
            catch (FaultException ex)
            {
                string[] exep = Function.GetExeption(ex);
                string codeExp = exep[1];
                ViewBag.Mes = codeExp;
                return View();
            }
        }

        public string HtmlCate(long prentid, long? id)
        {
            int i = 0;
            string html = "<ul>";
            // List<Catalog> listobj = _catalogsBusiness.GetDynamicQuery().Where(x => x.ParentId == prentid).ToList();
            List<Catalog> listobj = List.Where(x => x.ParentId == prentid).ToList();//Test Cache dữ liêu.
            {
                foreach (Catalog item in listobj)
                {
                    if (id != null && item.Id == id)
                    {
                        html += "<li><input type=\"checkbox\" checked=\"checked\" id=\"item-" + item.Id +
                                "\"   onclick=\"GetValueManuface(" + item.Id +
                                ")\" /><label class=\"selectcate\" id=\"label-" + item.Id + "\"  for=\"item-" + item.Id +
                                "\"  onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";

                        i++;
                        string str = HtmlbyCate(item.Id, ref i, id);
                        html += str;

                        html += "</li>";
                    }
                    else
                    {
                        html += "<li><input type=\"checkbox\" id=\"item-" + item.Id + "\"   onclick=\"GetValueManuface(" +
                                item.Id + ")\" /><label id=\"label-" + item.Id + "\"  for=\"item-" + item.Id +
                                "\"  onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";

                        i++;
                        string str = HtmlbyCate(item.Id, ref i, id);
                        html += str;

                        html += "</li>";
                    }
                }
            }
            html += "</ul>";
            return html;
        }

        public ActionResult JsonHtmlCate(long prentid, long? id)
        {
            int i = 0;
            string html = "<ul>";
            //List<Catalog> listobj = _catalogsBusiness.GetDynamicQuery().Where(x => x.ParentId == prentid).ToList();
            List<Catalog> listobj = List.Where(x => x.ParentId == prentid).ToList();//Test cache dữ liệu
            {
                foreach (Catalog item in listobj)
                {
                    if (id != null && item.Id == id)
                    {
                        html += "<li><input type=\"checkbox\" checked=\"checked\" id=\"item-" + item.Id +
                                "\"   onclick=\"GetValueManuface(" + item.Id +
                                ")\" /><label class=\"selectcate\" id=\"label-" + item.Id + "\"  for=\"item-" + item.Id +
                                "\"  onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";

                        i++;
                        string str = HtmlbyCate(item.Id, ref i, id);
                        html += str;

                        html += "</li>";
                    }
                    else
                    {
                        html += "<li><input type=\"checkbox\" id=\"item-" + item.Id + "\"   onclick=\"GetValueManuface(" +
                                item.Id + ")\" /><label id=\"label-" + item.Id + "\"  for=\"item-" + item.Id +
                                "\"  onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";

                        i++;
                        string str = HtmlbyCate(item.Id, ref i, id);
                        html += str;

                        html += "</li>";
                    }
                }
            }
            html += "</ul>";
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetManufaceByCategory(long cate)
        {
            try
            {
                ProductController.ObjGetPropertyAndManuByCate objGetPropertyAndManuByCate = new ProductController.ObjGetPropertyAndManuByCate();
                var htm = "";
                var listobj = MemberController.ListCatalogsManufacturerses.Where(x => x.CatalogId == cate).ToList();
                var a = listobj.Where(x => x.ManufacturerId == 454).ToList();
                if (listobj.Any())
                {
                    foreach (var item in listobj)
                    {
                        htm += " <option value=\"" + item.ManufacturerId + "\" onclick=\"GetManuface(" + item.ManufacturerId + ")\">" + item.Manufacturer.ManufacturerName +
                               "</option>";
                    }

                    //return Json(htm);
                }
                else
                {
                    htm += " <option value=\"" + -1 + "\" onclick=\"GetManuface(" + -1 + ")\">Not found Manuface</option>";
                }
                objGetPropertyAndManuByCate.HtmlManuface = htm;
                var listproperty = MemberController.ListCatalogProperties.Where(x => x.CatalogId == cate).ToList();
                var listjJsonProperties = new List<ProductController.JsonProperty>();
                if (listproperty.Any())
                {
                    foreach (var item in listproperty)
                    {
                        ProductController.JsonProperty entity = new ProductController.JsonProperty();
                        entity.Id = item.Id;
                        entity.Name = item.Name;
                        entity.HtmlType = HtmlTypeProperty(item.Id);
                        entity.HtmlInputHidden = "<input  type=\"hidden\" id=\"proprerty" + item.Id + "\" name=\"proprertyvalue\" value=\"" + item.Id + "\"/>";
                        listjJsonProperties.Add(entity);
                    }
                    objGetPropertyAndManuByCate.JsonProperties = listjJsonProperties;
                }
                else
                {
                    objGetPropertyAndManuByCate.JsonProperties = listjJsonProperties;
                }
                return Json(objGetPropertyAndManuByCate);
            }
            catch (Exception)
            {
                return Json(0);// Lỗi rồi e

                //Write log
                throw;
            }
        }

        public string HtmlTypeProperty(long proId)
        {
            var htm = "";
            try
            {
                var listObj = MemberController.ListCatalogPropertiesValues.Where(x => x.CatalogPropertieId == proId).ToList();
                if (listObj.Any())
                {
                    var objproperty = _catalogPropertiesBusiness.GetById(proId);

                    if (objproperty.Type == 1)
                    {
                        foreach (var item in listObj)
                        {
                            htm += "<label class='radio-inline'> <input onclick='SetvaluProperty(" + item.Id + "," + proId + ")' type='radio' name='optionsRadiosInline" + proId + "' id='optionsRadiosInline" + item.Id + "'  value='" + item.Value + "' >" + item.Value + "</label>";
                        }
                    }
                    else
                    {
                        foreach (var item in listObj)
                        {
                            htm += "<label class='checkbox-inline'>   <input  value='" + item.Value + "' onclick='SetvaluProperty(" + item.Id + "," + proId + ")'  type='checkbox'  id='optionsRadiosInline" + item.Id + "'>" + item.Value + "</label>";
                        }

                        //multiple
                    }
                    return htm;
                }
                else
                {
                    return "Not results value property!";
                }
            }
            catch (Exception)
            {
                //write log
                return "Not results value property!";
            }
        }

        public string HtmlbyCate(long cateId, ref int i, long? idmember)
        {
            string html = "<ul>";
            //   List<Catalog> listobj = _catalogsBusiness.GetDynamicQuery().Where(x => x.ParentId == cateId).ToList();
            List<Catalog> listobj = List.Where(x => x.ParentId == cateId).ToList();//Test cache dữ liệu
            if (listobj.Any())
            {
                foreach (Catalog item in listobj)
                {
                    if (idmember != null && item.Id == idmember)
                    {
                        html += "<li><input type=\"checkbox\" checked=\"checked\" id=\"item-" + item.Id +
                                "\" onclick=\"GetValueManuface(" + item.Id +
                                ")\" /><label class=\"selectcate\"  id=\"label-" + item.Id + "\" for=\"item-" + item.Id +
                                "\"   onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";
                        i++;
                        string str = HtmlbyCate(item.Id, ref i, idmember);
                        html += str;
                        html += "</li>";
                    }
                    else
                    {
                        html += "<li><input type=\"checkbox\" id=\"item-" + item.Id + "\" onclick=\"GetValueManuface(" +
                                item.Id + ")\" /><label  id=\"label-" + item.Id + "\" for=\"item-" + item.Id +
                                "\"   onclick=\"GetValueManuface(" + item.Id + ")\" >" + item.CatalogName + "</label>";
                        i++;
                        string str = HtmlbyCate(item.Id, ref i, idmember);
                        html += str;
                        html += "</li>";
                    }
                }
            }

            html += "</ul>";
            return html;
        }

        #endregion Đang bấn sản phẩm

        #region Oder

        public ActionResult OderProduct(string key, int? page)
        {
            ViewBag.Mes = string.Empty;
            Member member = SessionUtility.GetSessionMember(Session);
            int currentPageIndex = page.HasValue ? page.Value : 1;
            IPagedList<Order> obj =
                _ordersBusiness.GetDynamicQuery()
                    .Where(x => x.OrderBuyer.IdMember == member.Id)
                    .OrderByDescending(x => x.CreateDate)
                    .ToPagedList(currentPageIndex, 10);

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

        public JsonResult DeleteProduct(string[] array)
        {
            Member member = SessionUtility.GetSessionMember(Session);
            try
            {
                if (array != null && array.Any())
                {
                    var bt = _productsBusiness.CheckArayId(array, member.Id);
                    if (bt)
                    {
                        _productsBusiness.RemoveAll(array);
                        return Json(1); //thành công
                    }
                    else
                    {
                        return Json(3); //không thuộc phạm vi
                    }
                }
                return Json(2); // chưa chon ô check
            }
            catch (Exception)
            {
                return Json(0); //lỗi
            }
        }

        public JsonResult ContentOder(int st)
        {
            Member member = SessionUtility.GetSessionMember(Session);
            const int currentPageIndex = 1;
            IPagedList<Order> obj = null;
            if (st == -1)
            {
                obj =
                _ordersBusiness.GetDynamicQuery()
                    .Where(x => x.OrderBuyer.IdMember == member.Id)
                    .OrderByDescending(x => x.CreateDate)
                    .ToPagedList(currentPageIndex, 10);
            }
            else
            {
                obj =
                _ordersBusiness.GetDynamicQuery()
                    .Where(x => x.OrderBuyer.IdMember == member.Id && x.Status == st)
                    .OrderByDescending(x => x.CreateDate)
                    .ToPagedList(currentPageIndex, 10);
            }

            if (obj.Any())
            {
                string body = ControllerExtensions.RenderRazorViewToString(this, "ViewRendererOder", obj);
                return Json(body, JsonRequestBehavior.AllowGet);
            }
            var mes = Function.MessengerBox("success-msg", "Chưa có đơn hàng");
            return Json(mes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContentOderShop(int st)
        {
            Member member = SessionUtility.GetSessionMember(Session);
            const int currentPageIndex = 1;
            IPagedList<Order> obj = null;
            if (st == -1)
            {
                obj =
                _ordersBusiness.GetDynamicQuery()
                    .Where(x => x.IdShop == member.Id)
                    .OrderByDescending(x => x.CreateDate)
                    .ToPagedList(currentPageIndex, 10);
            }
            else
            {
                obj =
                _ordersBusiness.GetDynamicQuery()
                    .Where(x => x.IdShop == member.Id && x.Status == st)
                    .OrderByDescending(x => x.CreateDate)
                    .ToPagedList(currentPageIndex, 10);
            }

            if (obj.Any())
            {
                string body = ControllerExtensions.RenderRazorViewToString(this, "ViewRendererOderShop", obj);
                return Json(body, JsonRequestBehavior.AllowGet);
            }
            var mes = Function.MessengerBox("success-msg", "Chưa có hóa đơn");
            return Json(mes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CountOder()
        {
            var objoder = new ObjOder();
            Member member = SessionUtility.GetSessionMember(Session);
            var obj = _ordersBusiness.GetDynamicQuery().Where(x => x.OrderBuyer.IdMember == member.Id);
            if (obj.Any())
            {
                objoder.TotalMoney = obj.Sum(item => (double)item.OrderDetails.Sum(x => x.Price)).ToString("N0") + " VNĐ";
                objoder.CountAll = obj.Count().ToString("N0");
                objoder.CountNotPay = obj.Count(x => x.Status == 0).ToString("N0");
                objoder.CountPay = obj.Count(x => x.Status == 1).ToString("N0");
                objoder.CountNotGetProduct = obj.Count(x => x.Status == 2).ToString("N0");
                objoder.CountGetProduct = obj.Count(x => x.Status == 3).ToString("N0");
                objoder.CountRecy = obj.Count(x => x.Status == 4).ToString("N0");
            }
            else
            {
                objoder.TotalMoney = "0";
                objoder.CountAll = "0";
                objoder.CountNotPay = "0";
                objoder.CountPay = "0";
                objoder.CountNotGetProduct = "0";
                objoder.CountGetProduct = "0";
                objoder.CountRecy = "0";
            }

            return Json(objoder, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CountOderShop()
        {
            var objoder = new ObjOder();
            Member member = SessionUtility.GetSessionMember(Session);
            var obj = _ordersBusiness.GetDynamicQuery().Where(x => x.IdShop == member.Id);
            if (obj.Any())
            {
                objoder.TotalMoney = obj.Sum(item => (double)item.OrderDetails.Sum(x => x.Price)).ToString("N0") +
                                     " VNĐ";
                objoder.CountAll = obj.Count().ToString("N0");
                objoder.CountNotPay = obj.Count(x => x.Status == 0).ToString("N0");
                objoder.CountPay = obj.Count(x => x.Status == 1).ToString("N0");
                objoder.CountNotGetProduct = obj.Count(x => x.Status == 2).ToString("N0");
                objoder.CountGetProduct = obj.Count(x => x.Status == 3).ToString("N0");

                objoder.CountRecy = obj.Count(x => x.Status == 4).ToString("N0");
                return Json(objoder, JsonRequestBehavior.AllowGet);
            }
            else
            {
                objoder.TotalMoney = "0 VNĐ";
                objoder.CountAll = "0";
                objoder.CountNotPay = "0";
                objoder.CountPay = "0";
                objoder.CountNotGetProduct = "0";
                objoder.CountGetProduct = "0";

                objoder.CountRecy = "0";
                return Json(objoder, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteOder(long id)
        {
            try
            {
                Order obj = _ordersBusiness.GetById(id); //
                if (obj.Status == 4)
                {
                    _ordersBusiness.Remove(id); // xoa hẳn
                }
                else
                {
                    obj.Status = 4; //Trạng thái đơn hàng bị xóa
                    _ordersBusiness.Edit(obj);
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet); // lỗi không xóa được
                // write log               throw;
            }
        }

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
            var order = _ordersBusiness.GetById(id);
            string body = ControllerExtensions.RenderRazorViewToString(this, "RenderInfoPersonOrder", order);
            return Json(body, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStateOrder(long orderid, int state)
        {
            var order = _ordersBusiness.GetById(orderid);
            order.Status = state;
            _ordersBusiness.Edit(order);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion Oder
    }
}