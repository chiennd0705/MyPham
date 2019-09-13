﻿using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using Common.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class JsonController : Controller
    {
        private readonly UserBusiness _userBusiness = new UserBusiness();
        private readonly UserProfileBusiness _userProfileBusiness = new UserProfileBusiness();

        private readonly OrdersBusiness _ordersBusiness = new OrdersBusiness();
        private readonly OrderDetailBusiness _orderDetailBussiness = new OrderDetailBusiness();
        private readonly OrderBuyerBusiness _orderBuyerBusiness = new OrderBuyerBusiness();
        private readonly OrderReciverBusiness _orderReciverBusiness = new OrderReciverBusiness();

        private readonly ShopSettingsBusiness _shopSettingsBusiness = new ShopSettingsBusiness();
        private readonly ShopsBusiness _shopsBussiness = new ShopsBusiness();

        private readonly LocationsBusiness _locationsBusiness = new LocationsBusiness();

        private readonly CatalogsBusiness _catalogsBusiness = new CatalogsBusiness();
        private readonly SystemSettingBusiness _systemSettingBussiness = new SystemSettingBusiness();

        private readonly MemberProfileBusiness _memberProfileBusiness = new MemberProfileBusiness();
        private readonly MembersBusiness _membersBusiness = new MembersBusiness();

        private readonly NewsBusiness _newsBusiness = new NewsBusiness();
        private readonly NewsGroupBusiness _newsGroupBusiness = new NewsGroupBusiness();

        private readonly ProductsBusiness _productsBusiness = new ProductsBusiness();

        private readonly ManufacturersBusiness _manuBusiness = new ManufacturersBusiness();

        #region USER

        public JsonResult GetUserById(long id)
        {
            try
            {
                var obj = _userBusiness.GetById(id);
                if (obj != null)
                {
                    User objentity = new User();
                    objentity.Code = obj.UserProfile.Code;
                    objentity.Address = obj.UserProfile.Address;
                    objentity.Dob = obj.UserProfile.Dob;
                    objentity.Email = obj.UserProfile.Email;
                    objentity.Name = obj.UserProfile.Name;
                    objentity.Phone = obj.UserProfile.Phone;
                    objentity.ScreenName = obj.Screenname;
                    objentity.UserId = obj.Id;
                    objentity.UserId = obj.Id;
                    objentity.Discount = obj.DiscountPercent;

                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateUser(User obj)
        {
            try
            {
                UserBusiness userBusiness = new UserBusiness();
                var entity = userBusiness.GetById(obj.UserId);
                entity.UserProfile.Address = obj.Address;
                entity.UserProfile.Code = obj.Code;
                entity.UserProfile.Dob = obj.Dob;
                entity.UserProfile.Name = obj.Name;
                entity.UserProfile.Phone = obj.Phone;
                entity.Screenname = obj.ScreenName;
                entity.DiscountPercent = obj.Discount;
                entity.UserProfile.Email = obj.Email;
                userBusiness.Edit(entity);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ChangePasss(Objchangepass obj)
        {
            try
            {
                UserBusiness userBusiness = new UserBusiness();
                if (obj.NewPass.Equals(obj.RepeatPass))
                {
                    userBusiness.ChangePassword(obj.MemberId, obj.NewPass);
                    return Json(1);//Cập thật thành công
                }
                else
                {
                    return Json(2);//nhập lại mật khẩu không đúng
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region UserProfile

        public JsonResult GetUserProfileById(long id)
        {
            try
            {
                var obj = _userProfileBusiness.GetById(id);
                if (obj != null)
                {
                    Common.UserProfile objentity = new Common.UserProfile();
                    objentity.Code = obj.Code;
                    objentity.Address = obj.Address;
                    objentity.Dob = obj.Dob;
                    objentity.Email = obj.Email;
                    objentity.Name = obj.Name;
                    objentity.Phone = obj.Phone;
                    objentity.Id = obj.Id;
                    objentity.Dob = obj.Dob;
                    objentity.DepartmentId = obj.DepartmentId;
                    objentity.Description = obj.Description;

                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateUserProfile(Common.UserProfile obj)
        {
            try
            {
                UserProfileBusiness _userProfileBusiness = new UserProfileBusiness();
                var entity = _userProfileBusiness.GetById(obj.Id);
                entity.Code = obj.Code;
                entity.Address = obj.Address;
                entity.Dob = obj.Dob;
                entity.Email = obj.Email;
                entity.Name = obj.Name;
                entity.Phone = obj.Phone;
                entity.Id = obj.Id;
                entity.Dob = obj.Dob;
                entity.DepartmentId = obj.DepartmentId;
                entity.Description = obj.Description;
                _userProfileBusiness.Edit(entity);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UserProfile

        #endregion USER

        #region ORDER

        public JsonResult GetOrderById(long id)
        {
            try
            {
                var obj = _ordersBusiness.GetById(id);
                if (obj != null)
                {
                    Common.Order objentity = new Common.Order();
                    objentity.Id = obj.Id;
                    objentity.CreateDate = obj.CreateDate;
                    objentity.CreateBy = obj.CreateBy;
                    objentity.ModifyDate = obj.ModifyDate;
                    objentity.ModifyBy = obj.ModifyBy;
                    objentity.DateDeliver = obj.DateDeliver;
                    objentity.Status = obj.Status;
                    objentity.KmRoad = obj.KmRoad;
                    objentity.NoteAboutOrder = obj.NoteAboutOrder;
                    objentity.GramGood = obj.GramGood;
                    objentity.IdPayForm = obj.IdPayForm;
                    objentity.IdShop = obj.IdShop;
                    objentity.TotalMoney = obj.TotalMoney;
                    objentity.FeeOfTranspot = obj.FeeOfTranspot;
                    objentity.UrlBtnPayNL = obj.UrlBtnPayNL;

                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateOrder(Common.Order obj)
        {
            try
            {
                OrdersBusiness orderBusiness = new OrdersBusiness();
                var objentity = orderBusiness.GetById(obj.Id);
                //objentity.Id = obj.Id;
                objentity.CreateDate = obj.CreateDate;
                objentity.CreateBy = obj.CreateBy;
                objentity.ModifyDate = DateTime.Now;
                objentity.ModifyBy = obj.ModifyBy;
                objentity.DateDeliver = obj.DateDeliver;
                objentity.Status = obj.Status;
                objentity.KmRoad = obj.KmRoad;
                objentity.NoteAboutOrder = obj.NoteAboutOrder;
                objentity.GramGood = obj.GramGood;
                objentity.IdPayForm = obj.IdPayForm;
                objentity.IdShop = obj.IdShop;
                objentity.TotalMoney = obj.TotalMoney;
                objentity.FeeOfTranspot = obj.FeeOfTranspot;
                objentity.UrlBtnPayNL = obj.UrlBtnPayNL;
                orderBusiness.Edit(objentity);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetOrderDetailById(long id)
        {
            try
            {
                var obj = (List<Common.OrderDetail>)_orderDetailBussiness.GetListOrderDetailByIdOrder(id);
                if (obj != null)
                {
                    List<Common.OrderDetail> listODT = new List<Common.OrderDetail>();
                    foreach (var item in obj)
                    {
                        var odt = new Common.OrderDetail();
                        odt.Id = item.Id;
                        odt.IdOrder = item.IdOrder;
                        odt.IdProduct = item.IdProduct;
                        odt.NameProduct = item.NameProduct;
                        odt.Price = item.Price;
                        odt.Quantity = item.Quantity;
                        odt.Description = item.Description;
                        odt.PathImage = item.PathImage;

                        listODT.Add(odt);
                    }
                    return Json(listODT, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateStatus(long idOrderPost, int selectedItem)
        {
            try
            {
                OrdersBusiness orderBusiness = new OrdersBusiness();
                var objentity = orderBusiness.GetById(idOrderPost);

                objentity.Status = selectedItem;

                orderBusiness.Edit(objentity);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region OrderBuyer

        public JsonResult GetOrderBuyerById(long id)
        {
            try
            {
                var obj = _orderBuyerBusiness.GetById(id);
                if (obj != null)
                {
                    Common.OrderBuyer objentity = new Common.OrderBuyer();
                    objentity.Id = obj.Id;
                    objentity.Name = obj.Name;
                    objentity.PhoneNumber = obj.PhoneNumber;
                    objentity.Email = obj.Email;
                    objentity.LocationId = obj.LocationId;
                    objentity.Address = obj.Address;
                    objentity.CreateDate = obj.CreateDate;
                    objentity.CreateBy = obj.CreateBy;
                    objentity.ModifyDate = DateTime.Now;
                    objentity.ModifyBy = obj.ModifyBy;
                    objentity.NoteAboutBuyer = obj.NoteAboutBuyer;
                    objentity.IdMember = obj.IdMember;

                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        #endregion OrderBuyer

        #region OrderReciver

        public JsonResult GetOrderReciverById(long id)
        {
            try
            {
                var obj = _orderReciverBusiness.GetById(id);
                if (obj != null)
                {
                    Common.OrderReciver objentity = new Common.OrderReciver();
                    objentity.Id = obj.Id;
                    objentity.Name = obj.Name;
                    objentity.PhoneNumber = obj.PhoneNumber;
                    objentity.Email = obj.Email;
                    objentity.Address = obj.Address;
                    objentity.CreateDate = obj.CreateDate;
                    objentity.CreateBy = obj.CreateBy;
                    objentity.ModifyDate = DateTime.Now;
                    objentity.ModifyBy = obj.ModifyBy;
                    objentity.NoteAboutReciver = obj.NoteAboutReciver;

                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        #endregion OrderReciver

        #endregion ORDER

        #region SHOP

        public JsonResult GetInfoShopSetting(long id)
        {
            try
            {
                var obj = _shopSettingsBusiness.GetById(id);
                if (obj != null)
                {
                    Common.ShopSetting objentity = new Common.ShopSetting();
                    objentity.Id = obj.Id;
                    objentity.ShopId = obj.ShopId;
                    objentity.Key = obj.Key;
                    objentity.Value = obj.Value;
                    objentity.CreateDate = obj.CreateDate;
                    objentity.ModifyDate = obj.ModifyDate;

                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        #region Update Shop

        public JsonResult UpdateShop(Common.Shop obj)
        {
            try
            {
                ShopsBusiness _shopsBusiness = new ShopsBusiness();
                var objentity = _shopsBusiness.GetById(obj.Id);
                objentity.Id = obj.Id;
                objentity.ShopName = obj.ShopName;
                objentity.Icon = obj.Icon;
                objentity.Address = obj.Address;
                objentity.LocationId = obj.LocationId;
                objentity.Phone = obj.Phone;
                objentity.Rate = obj.Rate;
                objentity.TotalView = obj.TotalView;
                objentity.Type = obj.Type;
                objentity.Status = obj.Status;
                objentity.BeginDate = obj.BeginDate;
                objentity.EndDate = obj.EndDate;
                objentity.ActiveDate = obj.ActiveDate;
                objentity.CreateDate = obj.CreateDate;
                objentity.ModifyDate = DateTime.Now;
                _shopsBusiness.Edit(objentity);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update Shop

        public JsonResult GetInforShop(long id)
        {
            try
            {
                var obj = _shopsBussiness.GetById(id);
                if (obj != null)
                {
                    Common.Shop objentity = new Common.Shop();
                    objentity.Id = obj.Id;
                    objentity.ShopName = obj.ShopName;
                    objentity.Icon = obj.Icon;
                    objentity.Address = obj.Address;
                    objentity.LocationId = obj.LocationId;
                    objentity.Phone = obj.Phone;
                    objentity.Rate = obj.Rate;
                    objentity.TotalView = obj.TotalView;
                    objentity.Type = obj.Type;
                    objentity.Status = obj.Status;
                    objentity.BeginDate = obj.BeginDate;
                    objentity.EndDate = obj.EndDate;
                    objentity.ActiveDate = obj.ActiveDate;
                    objentity.CreateDate = obj.CreateDate;
                    objentity.ModifyDate = obj.ModifyDate;

                    //   // Common.ShopSupport shopSupport = new Common.ShopSupport();

                    objentity.Email = obj.ShopSupport.Email;
                    objentity.Skype = obj.ShopSupport.Skype;
                    objentity.Facebook = obj.ShopSupport.Facebook;

                    //get số sản phẩm và số đơn hàng của shop
                    var numProductOfShopDb = _productsBusiness.GetDynamicQuery().Count(x => x.MemberId == id);
                    if (numProductOfShopDb != 0)
                    {
                        objentity.NumberOfProduct = numProductOfShopDb;
                    }
                    else
                    {
                        objentity.NumberOfProduct = 0;
                    }
                    var numOrderOfShopDb = _ordersBusiness.GetDynamicQuery().Count(x => x.IdShop == id);
                    if (numOrderOfShopDb != 0)
                    {
                        objentity.NumberOfOrder = numOrderOfShopDb;
                    }
                    else
                    {
                        objentity.NumberOfOrder = 0;
                    }

                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        #endregion SHOP

        #region LOCATION

        public JsonResult GetLocationById(long id)
        {
            try
            {
                var location = _locationsBusiness.GetById(id);
                if (location != null)
                {
                    Common.Location obj = new Common.Location();
                    obj.Id = location.Id;
                    obj.ParentId = location.ParentId;
                    obj.Name = location.Name;
                    obj.Icon = location.Icon;
                    obj.Order = location.Order;

                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion LOCATION

        #region Catalog

        public JsonResult GetCatalogProuctById(long id)
        {
            try
            {
                var obj = _catalogsBusiness.GetById(id);
                if (obj != null)
                {
                    Common.Catalog objentity = new Common.Catalog();
                    objentity.Id = obj.Id;
                    objentity.ParentId = obj.ParentId;
                    objentity.Code = obj.Code;
                    objentity.CatalogName = obj.CatalogName;
                    objentity.FriendlyUrl = obj.FriendlyUrl;
                    objentity.Icon = obj.Icon;
                    objentity.ImageSource = obj.ImageSource;
                    objentity.Description = obj.Description;
                    objentity.Order = obj.Order;
                    objentity.Status = obj.Status;
                    objentity.CreateDate = obj.CreateDate;
                    objentity.ModifyDate = obj.ModifyDate;
                    objentity.IsLast = obj.IsLast;
                    objentity.SeoTitle = obj.SeoTitle;
                    objentity.SeoKeyword = obj.SeoKeyword;
                    objentity.SeoDescription = obj.SeoDescription;

                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateCatalog(Common.Catalog viewCatalog)
        {
            try
            {
                CatalogsBusiness catalogsBusiness = new CatalogsBusiness();
                var dbCatalog = catalogsBusiness.GetById(viewCatalog.Id);
                dbCatalog.ParentId = viewCatalog.ParentId;
                dbCatalog.Code = viewCatalog.Code;
                dbCatalog.CatalogName = viewCatalog.CatalogName;
                dbCatalog.FriendlyUrl = viewCatalog.FriendlyUrl;
                dbCatalog.Icon = viewCatalog.Icon;
                dbCatalog.Description = viewCatalog.Description;
                dbCatalog.Order = viewCatalog.Order;
                dbCatalog.Status = viewCatalog.Status;

                dbCatalog.IsLast = viewCatalog.IsLast;
                dbCatalog.SeoTitle = viewCatalog.SeoTitle;
                dbCatalog.SeoKeyword = viewCatalog.SeoKeyword;
                dbCatalog.SeoDescription = viewCatalog.SeoDescription;

                catalogsBusiness.Edit(dbCatalog);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ChangeStatusCatalog(long id)
        {
            //1: trạng thái active
            //0: trạng thái khóa
            var result = _catalogsBusiness.ChangeStatusCatalog(id);
            return Json(new
            {
                status = result
            });
        }

        #endregion Catalog

        #region Member

        public JsonResult ChangePasssMember(Objchangepass obj)
        {
            try
            {
                MembersBusiness memberBusiness = new MembersBusiness();
                if (obj.NewPass.Equals(obj.RepeatPass))
                {
                    //1 thay mat khau dang nhap
                    // khac 1 mat khau giao dich
                    memberBusiness.ChangePassword(obj.MemberId, obj.NewPass, 1);
                    return Json(1);//Cập thật thành công
                }
                else
                {
                    return Json(2);//nhập lại mật khẩu không đúng
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ChangePasssTransMember(Objchangepass obj)
        {
            try
            {
                MembersBusiness memberBusiness = new MembersBusiness();
                if (obj.NewPass.Equals(obj.RepeatPass))
                {
                    //1 thay mat khau dang nhap
                    // khac 1 mat khau giao dich
                    memberBusiness.ChangePassword(obj.MemberId, obj.NewPass, 2);
                    return Json(1);//Cập thật thành công
                }
                else
                {
                    return Json(2);//nhập lại mật khẩu không đúng
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetMemberById(long id)
        {
            try
            {
                var member = _membersBusiness.GetById(id);
                if (member != null)
                {
                    Common.Member mb = new Common.Member();
                    mb.Id = member.Id;
                    mb.UserName = member.UserName;
                    mb.Password = member.Password;
                    mb.PasswordTransaction = member.PasswordTransaction;
                    mb.PasswordEncrypted = member.PasswordEncrypted;
                    mb.PasswordEncryptedMethod = member.PasswordEncryptedMethod;
                    mb.LoginDate = member.LoginDate;
                    mb.LastFailedLoginDate = member.LastFailedLoginDate;
                    mb.Loutout = member.Loutout;
                    mb.LockoutDate = member.LockoutDate;
                    mb.Verify = member.Verify;
                    mb.Status = mb.Status;
                    mb.CreateDate = member.CreateDate;
                    mb.ModifyDate = member.ModifyDate;
                    mb.ActiveDate = member.ActiveDate;
                    mb.Question = member.Question;
                    mb.Answer = member.Answer;

                    return Json(mb, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ChangeStatusMember(long id)
        {
            //1: trạng thái active
            //0: trạng thái khóa
            var result = _membersBusiness.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        #region Memmber profile

        public JsonResult GetMemberProfileById(long id)
        {
            try
            {
                var mbProfile = _memberProfileBusiness.GetById(id);
                if (mbProfile != null)
                {
                    Common.MemberProfile mb = new Common.MemberProfile();
                    mb.Id = mbProfile.Id;
                    mb.FirstName = mbProfile.FirstName;
                    mb.LastName = mbProfile.LastName;
                    mb.LocationId = mbProfile.LocationId;
                    mb.Address = mbProfile.Address;
                    mb.Emaill = mbProfile.Emaill;
                    mb.Phone = mbProfile.Phone;
                    mb.Photo = mbProfile.Photo;
                    mb.Dob = mbProfile.Dob;
                    mb.Sex = mbProfile.Sex;
                    mb.Pid = mbProfile.Pid;
                    mb.ZipCode = mbProfile.ZipCode;

                    return Json(mb, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult UpdateMemberProfile(Common.MemberProfile ViewMemberProfile)
        {
            try
            {
                MemberProfileBusiness memberProfileBusiness = new MemberProfileBusiness();
                var dbMemberProfile = memberProfileBusiness.GetById(ViewMemberProfile.Id);
                dbMemberProfile.Id = ViewMemberProfile.Id;
                dbMemberProfile.FirstName = ViewMemberProfile.FirstName;
                dbMemberProfile.LastName = ViewMemberProfile.LastName;
                dbMemberProfile.LocationId = ViewMemberProfile.LocationId;
                dbMemberProfile.Address = ViewMemberProfile.Address;
                dbMemberProfile.Emaill = ViewMemberProfile.Emaill;
                dbMemberProfile.Phone = ViewMemberProfile.Phone;
                dbMemberProfile.Dob = ViewMemberProfile.Dob;
                dbMemberProfile.Sex = ViewMemberProfile.Sex;
                dbMemberProfile.Pid = ViewMemberProfile.Pid;
                dbMemberProfile.ZipCode = ViewMemberProfile.ZipCode;
                dbMemberProfile.Photo = ViewMemberProfile.ZipCode;

                memberProfileBusiness.Edit(dbMemberProfile);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetLocationByParent(long parentId)
        {
            var listlocation = _locationsBusiness.GetDynamicQuery().Where(x => x.ParentId == parentId).OrderBy(x => x.Name).ToList();
            var htm = "";
            if (listlocation.Any())
            {
                foreach (var item in listlocation)
                {
                    htm += "<option value=\"" + item.Id + "\">" + item.Name + "</option>";
                }
            }
            else
            {
                htm = "Không tìm thấy kết quả!";
            }
            return Json(htm, JsonRequestBehavior.AllowGet);
        }

        #endregion Memmber profile

        #endregion Member

        #region News

        public JsonResult GetNewsname(string term)
        {
            NewsBusiness _newsBusiness1 = new NewsBusiness();
            try
            {
                List<Common.SearchNewByName_Result> data = _newsBusiness1.SearchListNewByName(term);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult GetNewsById(long id)
        {
            try
            {
                var dbNews = _newsBusiness.GetById(id);
                if (dbNews != null)
                {
                    Common.News viewNews = new Common.News();
                    viewNews.Id = dbNews.Id;
                    viewNews.NewsGroupId = dbNews.NewsGroupId;
                    viewNews.Title = dbNews.Title;
                    viewNews.ImageSource = dbNews.ImageSource;
                    viewNews.Summary = dbNews.Summary;
                    viewNews.Descriptions = dbNews.Descriptions;
                    viewNews.Status = dbNews.Status;
                    viewNews.isPublic = dbNews.isPublic;
                    viewNews.TotalView = dbNews.TotalView;
                    viewNews.AdminIDApproval = dbNews.AdminIDApproval;

                    return Json(viewNews, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateNews(Common.News viewNews)
        {
            try
            {
                NewsBusiness newsBusiness = new NewsBusiness();
                var dbNews = newsBusiness.GetById(viewNews.Id);
                dbNews.NewsGroupId = viewNews.NewsGroupId;
                dbNews.Title = viewNews.Title;
                dbNews.ImageSource = viewNews.ImageSource;
                dbNews.Summary = viewNews.Summary;
                dbNews.Descriptions = viewNews.Descriptions;
                dbNews.Status = viewNews.Status;
                dbNews.isPublic = viewNews.isPublic;
                dbNews.ModifyDate = DateTime.Now;
                dbNews.AdminIDApproval = viewNews.AdminIDApproval;

                newsBusiness.Edit(dbNews);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region NewsGroup

        public JsonResult GetNewsGroupById(long id)
        {
            try
            {
                var dbNewsGroup = _newsGroupBusiness.GetById(id);
                if (dbNewsGroup != null)
                {
                    Common.NewsGroups viewNewsGroup = new Common.NewsGroups();
                    viewNewsGroup.Id = dbNewsGroup.Id;
                    viewNewsGroup.ParentId = dbNewsGroup.ParentId;
                    viewNewsGroup.NewsGroupName = dbNewsGroup.NewsGroupName;
                    viewNewsGroup.isPublic = viewNewsGroup.isPublic;
                    viewNewsGroup.Status = dbNewsGroup.Status;
                    viewNewsGroup.CreateDate = dbNewsGroup.CreateDate;
                    viewNewsGroup.ModifyDate = dbNewsGroup.ModifyDate;
                    viewNewsGroup.AdminIDApproval = dbNewsGroup.AdminIDApproval;

                    return Json(viewNewsGroup, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateNewsGroup(Common.NewsGroups viewNewsGroup)
        {
            try
            {
                NewsGroupBusiness newsGroupBusiness = new NewsGroupBusiness();
                var dbNewsGroup = newsGroupBusiness.GetById(viewNewsGroup.Id);
                dbNewsGroup.ParentId = viewNewsGroup.ParentId;
                dbNewsGroup.NewsGroupName = viewNewsGroup.NewsGroupName;
                dbNewsGroup.isPublic = viewNewsGroup.isPublic;
                dbNewsGroup.Status = viewNewsGroup.Status;
                dbNewsGroup.ModifyDate = DateTime.Now;
                dbNewsGroup.AdminIDApproval = viewNewsGroup.AdminIDApproval;
                dbNewsGroup.IsView1 = viewNewsGroup.IsView1;
                dbNewsGroup.IsView2 = viewNewsGroup.IsView2;

                newsGroupBusiness.Edit(dbNewsGroup);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion NewsGroup

        #endregion News

        #region Product

        public JsonResult GetProductsname(string term)
        {
            ProductsBusiness _productBusiness = new ProductsBusiness();
            try
            {
                List<Common.SearchProductByName_Result> data = _productBusiness.SearchListProductByName(term);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult GetInforIsProduct(long idOfProValue)
        {
            try
            {
                ProductsBusiness _productsBusiness = new ProductsBusiness();
                var dBproduct = _productsBusiness.GetById(idOfProValue);
                if (dBproduct != null)
                {
                    var viewProductReturn = new Common.Product();
                    viewProductReturn.IsNew = dBproduct.IsNew;
                    viewProductReturn.IsVip = dBproduct.IsVip;
                    viewProductReturn.IsAttractive = dBproduct.IsAttractive;

                    return Json(viewProductReturn, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ChangeStatusOfProduct(long idOfProduct, int selectedItem)
        {
            try
            {
                ProductsBusiness productBusiness = new ProductsBusiness();
                var objentity = productBusiness.GetById(idOfProduct);
                if (selectedItem != -1)
                {
                    objentity.Status = selectedItem;
                }
                else
                {
                    return Json(2);
                }

                productBusiness.Edit(objentity);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult ChangeIsOfProduct(long idOfProValue, bool isNewCkeckValue, bool isAttactiveCheckValue, bool isVipCheckValue)
        {
            try
            {
                ProductsBusiness productBusiness = new ProductsBusiness();
                var objentity = productBusiness.GetById(idOfProValue);
                objentity.IsNew = isNewCkeckValue;
                objentity.IsAttractive = isAttactiveCheckValue;
                objentity.IsVip = isVipCheckValue;

                productBusiness.Edit(objentity);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Product

        #region ManuFacter

        public JsonResult GetManu(long id)
        {
            try
            {
                var obj = _manuBusiness.GetById(id);
                if (obj != null)
                {
                    Manufacturers objentity = new Manufacturers();
                    objentity.ManuId = obj.Id;
                    objentity.ManuName = obj.ManufacturerName;
                    objentity.icon = obj.Icon;
                    objentity.Description = obj.Description;
                    objentity.Order = obj.Order;
                    return Json(objentity, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateManu(Manufacturers obj)
        {
            try
            {
                ManufacturersBusiness manuBusiness = new ManufacturersBusiness();
                var entity = manuBusiness.GetById(obj.ManuId);
                entity.ManufacturerName = obj.ManuName;
                entity.Icon = obj.icon;
                entity.Description = obj.Description;
                entity.Order = obj.Order;

                manuBusiness.Edit(entity);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion ManuFacter

        #region WEBSETTING

        public JsonResult GetSettingByID(long id)
        {
            try
            {
                var obj = _systemSettingBussiness.GetById(id);
                SystemSettings sys = new SystemSettings();
                if (obj != null)
                {
                    sys.Id = obj.Id;
                    sys.Key = obj.Key;
                    sys.ModifyDate = obj.ModifyDate;
                    sys.Title = obj.Title;
                    sys.Value = obj.Value;

                    return Json(sys, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        public JsonResult UpdateWebSetting(Common.SystemSetting systemSetting)
        {
            try
            {
                SystemSettingBusiness systemSettingBussiness = new SystemSettingBusiness();
                var system = systemSettingBussiness.GetById(systemSetting.Id);
                system.Id = systemSetting.Id;
                system.Title = systemSetting.Title;
                system.Value = systemSetting.Value;
                system.Key = systemSetting.Key;
                system.ModifyDate = systemSetting.ModifyDate;
                system.DesCription = systemSetting.DesCription;
                systemSettingBussiness.Edit(system);
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult DeleteSettingByID(long id)
        {
            try
            {
                if (id > 0)
                {
                    //Thay đổi trạng thái danh mục
                    _systemSettingBussiness.Remove(id);
                    //status: 1(active), 0(private)
                }
                return Json(1);//Cập nhật thành công
            }
            catch (Exception)
            {
                //write log
                throw;
            }
        }

        #endregion WEBSETTING
    }
}