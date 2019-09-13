using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class ShopController : BaseController
    {
        #region Khai báo business

        private readonly ShopsBusiness _shopsBusiness = new ShopsBusiness();
        private readonly ShopSettingsBusiness _shopSettingsBusiness = new ShopSettingsBusiness();
        private readonly ShopCatalogsBusiness _shopCatalogsBusiness = new ShopCatalogsBusiness();
        private readonly ShopPolicysBusiness _shopPolicysBusiness = new ShopPolicysBusiness();
        private readonly ShopSupportsBusiness _shopSupportsBusiness = new ShopSupportsBusiness();
        private readonly ShopNewsBusiness _shopNewsBusiness = new ShopNewsBusiness();
        private readonly ShopNewsGroupsBusiness _shopNewsGroupsBusiness = new ShopNewsGroupsBusiness();

        private readonly ProductsBusiness _productsBusiness = new ProductsBusiness();
        private readonly OrdersBusiness _ordersBusiness = new OrdersBusiness();

        #endregion Khai báo business

        //protected override void Initialize(System.Web.Routing.RequestContext rc)
        //{
        //    base.Initialize(rc);

        //    if (!SessionUtility.CheckLogin(Session))
        //        Response.Redirect("/Login?returnUrl=" + rc.HttpContext.Request.Url.PathAndQuery);
        //    else
        //    {
        //        if (!SessionUtility.CheckPermission(Session, rc.HttpContext.Request.Url.PathAndQuery))
        //        {
        //            Response.Redirect("/Error/NotPermissionAdmin");
        //        }
        //    }

        //}

        //
        // GET: /Login/
        //
        // GET: /Manage/Shop/
        public ActionResult Index(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _shopsBusiness.GetByKey(key).ToPagedList(currentPageIndex, 20);

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

        #region EditShop (AllShop: shopSetting, shopPolicy, shopSupport )

        [HttpGet]
        public ActionResult EditAllInforAboutShop(long id)
        {
            var objShop = _shopsBusiness.GetById(id);

            if (objShop != null)
            {
                LoadDropdown loadDropdown = new LoadDropdown();
                ViewBag.Location = loadDropdown.SearchLocationParenId(1, objShop.LocationId);

                LoadCombo loadDropStatus = new LoadCombo();
                ViewBag.StatusShop = loadDropStatus.LoadStatus();

                return View(objShop);
            }

            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditAllInforAboutShop(Shop objShop, long receiverPhuong, HttpPostedFileBase iconShop)
        {
            ShopsBusiness shopBusiness = new ShopsBusiness();
            ShopSettingsBusiness shopSettingBusiness = new ShopSettingsBusiness();

            LoadDropdown loadDropdown = new LoadDropdown();
            ViewBag.Location = loadDropdown.SearchLocationParenId(1, objShop.LocationId);
            LoadCombo loadDropStatus = new LoadCombo();
            ViewBag.StatusShop = loadDropStatus.LoadStatus();
            if (ModelState.IsValid && objShop != null)
            {
                var objShopDB = shopBusiness.GetById(objShop.Id);

                objShopDB.ShopName = objShop.ShopName;
                objShopDB.Address = objShop.Address;
                objShopDB.Phone = objShop.Phone;
                ////temp, sửa lại là dropdown
                //objShopDB.Type = objShop.Type;
                //objShopDB.Status = objShop.Status;
                objShopDB.LocationId = receiverPhuong;
                //objShopDB.ActiveDate = DateTime.ParseExact(datepicker1, "dd/MM/yyyy", null);

                if (iconShop != null && iconShop.ContentLength > 0)
                {
                    string randomImage = Guid.NewGuid().ToString();
                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                    string strurlimage = Function.ResizeImageNew(iconShop, 300, 300, pathImage, randomImage);
                    Function.ResizeImageNew(iconShop, 500, 500, pathImage, randomImage);
                    Function.ResizeImageNew(iconShop, 1000, 1000, pathImage, randomImage);
                    objShopDB.Icon = strurlimage;
                }

                shopBusiness.Edit(objShopDB);
                return View(objShopDB);
                ////cập nhật shopSetting
                //var objShopSettingDB = shopSettingBusiness.GetDynamicQuery().Where(x => x.ShopId == objShop.Id).FirstOrDefault();
                //if(objShopSettingDB != null)
                //{
                //    objShopSettingDB.Key = keyShopSetting;
                //    objShopSettingDB.Value = valueShopSetting;

                //    shopSettingBusiness.Edit(objShopSettingDB);
                //}
                //else
                //{
                //    ShopSetting objShopSetting = new ShopSetting();
                //    objShopSetting.ShopId = objShop.Id;
                //    objShopSetting.Key = keyShopSetting;
                //    objShopSetting.Value = valueShopSetting;

                //    shopSettingBusiness.AddNew(objShopSetting);
                //}
            }
            else
            {
                return View();
            }
        }

        public ActionResult ListProductOfShop(long id)
        {
            var listProduct = _productsBusiness.GetDynamicQuery().Where(x => x.MemberId == id).ToList();
            if (listProduct != null)
            {
                return View(listProduct);
            }
            return View();
        }

        public ActionResult ListOrderOfShop(long id)
        {
            var listOrder = _ordersBusiness.GetDynamicQuery().Where(x => x.IdShop == id).ToList();
            if (listOrder != null)
            {
                return View(listOrder);
            }
            return View();
        }

        #endregion EditShop (AllShop: shopSetting, shopPolicy, shopSupport )

        #region Create shop

        public ActionResult CreateShop()
        {
            LoadDropdown loadDropdown = new LoadDropdown();
            ViewBag.Location = loadDropdown.SearchLocationParenId(1, null);
            return View();
        }

        [HttpPost]
        public ActionResult CreateShop(Shop obj, long receiverPhuong, string acount)
        {
            ViewBag.Mes = "";
            LoadDropdown loadDropdown = new LoadDropdown();
            ViewBag.Location = loadDropdown.SearchLocationParenId(1, null);
            var check = new MembersBusiness().CheckDuplicate(acount);
            if (!check)
            {
                ViewBag.Mes =
                    "Thành viên này chưa phải là một thành viên của hệ thống, vui lòng đăng ký thành viên viên trước khi mở shop";
                return View();
            }
            else
            {
                var idshop =
                    new MembersBusiness().GetDynamicQuery()
                        .First(x => x.UserName == acount || x.MemberProfile.Emaill == acount)
                        .Id;
                var checkshop = new ShopsBusiness().GetDynamicQuery().Where(x => x.Id == idshop);
                if (checkshop.Any())
                {
                    ViewBag.Mes = "Shop này đã được active trước đó!";
                    return View(obj);
                }
                else
                {
                    obj.Id = idshop;
                    obj.LocationId = receiverPhuong;
                    obj.ActiveDate = DateTime.Now;
                    obj.BeginDate = DateTime.Now;
                    obj.EndDate = DateTime.Now.AddYears(10);
                    obj.ShopSupport.Id = idshop;
                    if (obj.ShopSupport.Facebook == null)
                    {
                        obj.ShopSupport.Facebook = string.Empty;
                    }
                    if (obj.ShopSupport.Yahoo == null)
                    {
                        obj.ShopSupport.Yahoo = string.Empty;
                    }
                    var shopPolicy = new ShopPolicy
                    {
                        PaymentPolicy = string.Empty,
                        Id = idshop,
                        SalesPolicy = string.Empty,
                        About = string.Empty,
                        PrivacyPolicy = string.Empty
                    };
                    obj.ShopPolicy = shopPolicy;
                    _shopsBusiness.AddNew(obj);
                    ViewBag.Mes = "Active thành công!";
                    return View(obj);
                }
            }
        }

        #endregion Create shop

        #region Delete Shop

        public ActionResult DeleteShop(long id = 0)
        {
            try
            {
                if (id > 0)
                {
                    _shopsBusiness.Remove(id);
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

        #endregion Delete Shop

        #region Admin loginshop

        public JsonResult LoginShop(long shopid)
        {
            var adminid = long.Parse(SessionUtility.GetSessionUserId(Session));
            var user = new UserBusiness().GetById(adminid);
            var url = System.Web.Hosting.HostingEnvironment.MapPath("~/LogFolder/infoAdmin.txt");
            var listReadFile = Common.util.Function.ReadFile(url);

            try
            {
                ViewBag.Mes = "";
                var entity = new MembersBusiness().GetById(shopid);
                var shop = _shopsBusiness.GetById(shopid);
                if (entity != null)
                {
                    if (entity.Status == 0)
                    {
                        //ViewBag.Mes = "Tài khoản chưa được kích hoạt ";
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else if (entity.Status == 3)
                    {
                        //ViewBag.Mes = "Tài khoản đã bi khóa";
                        return Json(3, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //listReadFile.Add("start"+DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        SessionUtility.SetSessionMember(entity, Session);
                        //listReadFile.Add("end"+DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                        #region write log

                        listReadFile.Add("*****************************************************************************************");
                        listReadFile.Add(string.Format(
                                "Tài khoản {0}, tên đây đủ là {1}, đã  đăng  nhập vào shop {2}, vào lúc {3}",
                                user.Username, user.UserProfile.Name, shop.ShopName, DateTime.Now));
                        Common.util.Function.WriteFile(url, listReadFile.ToArray());

                        #endregion write log

                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    // ViewBag.mes = "Lỗi thông tin đăng nhập";
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(4, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Admin loginshop
    }
}