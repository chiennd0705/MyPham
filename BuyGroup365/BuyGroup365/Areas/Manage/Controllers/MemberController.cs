using Business.ClassBusiness;
using BuyGroup365.Models.Member;
using Common.util;
using MvcPaging;
using System;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class MembersController : BaseController
    {
        private readonly MembersBusiness _memberBusiness = new MembersBusiness();
        private readonly MemberProfileBusiness _memberProfileBusiness = new MemberProfileBusiness();
        //
        // GET: /Manage/Member/

        #region QUẢN LÝ MEMBER

        public ActionResult Index(string key, string fromDateSearch, string toDateSearch, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;
                ViewData["fromDate"] = fromDateSearch;
                ViewData["toDate"] = toDateSearch;

                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _memberBusiness.GetByKey(key, fromDateSearch, toDateSearch).OrderByDescending(x => x.CreateDate).ToPagedList(currentPageIndex, 20);
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
        public ActionResult EditMember(long id)
        {
            var memberObj = _memberProfileBusiness.GetById(id);
            LoadDropdown loadDropdown = new LoadDropdown();
            ViewBag.Location = loadDropdown.SearchLocationParenId(1, memberObj.LocationId);
            if (memberObj == null)
            {
                return RedirectToAction("Index");
                //hoac throw ra loi
            }

            return View(memberObj);
        }

        [HttpPost]
        public ActionResult EditMember(Common.MemberProfile objProfileView, long receiverPhuong, int gender, HttpPostedFileBase avatarMember, string datepicker1)
        {
            MemberProfileBusiness mbProfileDB = new MemberProfileBusiness();
            if (ModelState.IsValid && objProfileView != null)
            {
                var objProfileDB = mbProfileDB.GetById(objProfileView.Id);
                objProfileDB.LastName = objProfileView.LastName;
                objProfileDB.FirstName = objProfileView.FirstName;
                objProfileDB.Phone = objProfileView.Phone;
                objProfileDB.Emaill = objProfileView.Emaill;
                objProfileDB.Address = objProfileView.Address;
                objProfileDB.Dob = DateTime.ParseExact(datepicker1, "dd/MM/yyyy", null);

                if (avatarMember != null && avatarMember.ContentLength > 0)
                {
                    string randomImage = Guid.NewGuid().ToString();
                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                    string strurlimage = Function.ResizeImageNew(avatarMember, 300, 300, pathImage, randomImage);
                    Function.ResizeImageNew(avatarMember, 500, 500, pathImage, randomImage);
                    Function.ResizeImageNew(avatarMember, 1000, 1000, pathImage, randomImage);
                    objProfileDB.Photo = strurlimage;
                }

                objProfileDB.Sex = gender;
                objProfileDB.LocationId = objProfileView.LocationId;

                mbProfileDB.Edit(objProfileDB);

                ViewData["Message"] = "Cập nhập thành công!";
            }
            return View();
        }

        #region Delete member

        public ActionResult DeleteMember(long id = 0)
        {
            try
            {
                if (id > 0)
                {
                    _memberBusiness.Remove(id);
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

        #endregion Delete member

        public JsonResult ApprovedMember(long memberid, int status)
        {
            try
            {
                var member = _memberBusiness.GetById(memberid);
                member.Status = status;
                _memberBusiness.Edit(member);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion QUẢN LÝ MEMBER

        #region Member Profile

        #region Delete MemberProfile

        public ActionResult DeleteMemberProfile(long id = 0)
        {
            try
            {
                if (id > 0)
                {
                    _memberProfileBusiness.Remove(id);
                }
                return RedirectToAction("MemberProfile");
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        #endregion Delete MemberProfile

        #endregion Member Profile
    }
}