﻿using Business.ClassBusiness;
using BuyGroup365.Models.Home;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BuyGroup365.Controllers
{
    public class LoginController : Controller
    {
        private MembersBusiness _membersBusiness = new MembersBusiness();
        private ShopSupportsBusiness _shopSupportsBusiness = new ShopSupportsBusiness();

        // GET: /Login/
        public ActionResult Login()
        {
            ViewBag.Mes = "Login or Create an Account";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pass, string returnUrl)
        {
            ViewBag.Mes = "";
            var entity = _membersBusiness.CheckLogin(email, pass);
            if (entity != null)
            {
                if (entity.Status == 0)
                {
                    ViewBag.Mes = "Tài khoản chưa được kích hoạt ";
                    return View();
                }
                else if (entity.Status == 3)
                {
                    ViewBag.Mes = "Tài khoản đã bi khóa";
                    return View();
                }
                else
                {
                    SessionUtility.SetSessionMember(entity, Session);
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToRoute(new { controller = "Member", action = "Profile" });
                }
            }
            else
            {
                ViewBag.mes = "Lỗi thông tin đăng nhập";
                return View();
            }
        }

        public ActionResult Logout()
        {
            SessionUtility.LogOut(Session);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            ViewBag.Mes = "Create an Account";
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register obj)
        {
            //verryfy dư liêu trước khi insert.....wating
            //
            //    Member member=new Member();
            // MemberProfile memberProfile=new MemberProfile();
            if (_membersBusiness.CheckDuplicate(obj.Username))
            {
                ViewBag.Mes = "Tên này đã tồn tại trong hệ thống";
                return View(obj);
            }
            else if (_membersBusiness.CheckDuplicate(obj.Email))
            {
                ViewBag.Mes = "Mail này đã tồn tại trong hệ thống";
                return View(obj);
            }
            else
            {
                Random rd = new Random();
                var member = new Member
                {
                    UserName = obj.Username,
                    Password = Common.util.Common.GetMd5Sum(obj.Pass),
                    PasswordTransaction = Common.util.Common.GetMd5Sum(obj.Pass),
                    PasswordEncrypted = 1,
                    PasswordModifyDate = DateTime.Now,
                    PasswordEncryptedMethod = "MD5",
                    LoginDate = DateTime.Now,
                    LastFailedLoginDate = DateTime.Now,
                    Loutout = false,
                    LockoutDate = DateTime.Now,

                    Verify = Common.util.Common.GetMd5Sum(rd.Next(1111, 9999).ToString()),
                    Status = 0,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    ActiveDate = DateTime.Now
                };

                var memberProfile = new MemberProfile
                {
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    LocationId = -1,
                    Address = " ",
                    Emaill = obj.Email,
                    Dob = DateTime.Now,
                    Sex = -1,
                    Pid = " ",
                    ZipCode = " ",
                    Phone = ""
                };
                member.MemberProfile = memberProfile;

                //  #region insert bảng shop

                //  Shop objshop = new Shop();
                //  objshop.ShopName = member.UserName;
                //  objshop.LocationId = -1;//chưa xác định
                //  objshop.Phone = "012345446";//chưa xác định
                //  objshop.BeginDate = DateTime.Now;
                //  objshop.EndDate = DateTime.Now;
                //  objshop.ActiveDate = DateTime.Now;
                //  objshop.CreateDate = DateTime.Now;
                //  objshop.ModifyDate = DateTime.Now;
                //  ShopSupport shopSupport = new ShopSupport();
                //  shopSupport.Email = member.MemberProfile.Emaill;
                //  shopSupport.Facebook = " ";//t
                //  shopSupport.Mobile = " ";
                //  shopSupport.Skype = " ";
                //  shopSupport.Yahoo = " ";
                //  shopSupport.SupportName = member.UserName;
                //  shopSupport.Phone = " ";
                ////  objshop.ShopSupport = shopSupport;
                //  ShopPolicy shopPolicy = new ShopPolicy();
                //  shopPolicy.PaymentPolicy = " ";
                //  shopPolicy.SalesPolicy = " ";
                //  shopPolicy.About = " ";
                //  shopPolicy.PrivacyPolicy = " ";
                //  objshop.ShopPolicy = shopPolicy;
                //  //      shopsBusiness.AddNew(objshop);

                //  // objshop.ShopPolicy=
                //  #endregion

                //member.Shop = objshop;

                _membersBusiness.AddNew(member);
                //shopSupport.Id = member.Id;
                //    _shopSupportsBusiness.AddNew(shopSupport);
                ViewBag.Mes = "Đăng ký thành công! vui lòng check mail để active.";

                #region SendMail

                var ho = Request.ServerVariables["HTTP_HOST"];
                //string sub = "Active tài khoản thành viên";
                var url = "http://" + ho + "/Login/VerifyMember?vr=" + member.Verify;
                var link = "<a href=\"" + url + "\" style=\"color: #0388cd\" target=\"_blank\"><span class=\"il\">BUYGROUP365</span> – Xác nhận đăng ký thành công</a>";
                var html =
                    "<tr><td style=\"padding: 14px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>Chúc mừng Quý khách đã đăng ký thành công tài khoản trên <span class=\"il\">BUYGROUP365</span>!</b></td> </tr>" +
                    "<tr> <td style=\"padding: 4px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>Vui lòng nhấn vào đường dẫn dưới đây để kích hoạt tài khoản:</b></td></tr>" +
                    "<tr> <td style=\"padding: 4px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>" + link + "</td></tr>" +
                    "<tr> <td style=\"padding: 4px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>Tên đăng nhập và mật khẩu của bạn là:  " + obj.Email + " hoặc" + obj.Username + " / " + obj.Pass + "</b></td></tr>" +
                    "<tr><td style=\"padding: 7px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\">Mọi thắc mắc và góp ý, xin Quý khách vui lòng liên hệ với chúng tôi qua:</td></tr>";

                //   var link = "<a href=\"" + url + "\">BUYGROUP365 – Xác nhận đăng ký thành công</a>";
                //  string body = ControllerExtensions.RenderRazorViewToString(MailTempController, "DetailCart", link);
                var contentRegister = new ContentRegister { LinkActive = html, UserName = obj.Username, Pass = obj.Pass };
                string body = ControllerExtensions.RenderRazorViewToString(this, "MesengerRegister", contentRegister);
                Function.ObjMailSend objmail = new Function.ObjMailSend();
                var mailsend = new SystemSettingBusiness().GetDynamicQuery().First(x => x.Key == "mail_noreply");
                var acount = mailsend.Value.Split('|');
                objmail.FromMail = acount[0];
                objmail.PassFromMail = acount[1];
                objmail.ToMail = obj.Email;
                Function.email_send(objmail, "[Buygroup365]Xác nhận đăng ký tài khoản (" + DateTime.Now + ")", body);

                #endregion SendMail

                ViewBag.Mes = "1";
                return View(obj);
            }
        }

        public ActionResult MesengerRegister()
        {
            return View();
        }

        public ActionResult VerifyMember(string vr)
        {
            ViewBag.Mes = "";
            var member = _membersBusiness.GetDynamicQuery().Where(x => x.Verify == vr);
            if (member.Any())
            {
                var entity = member.First();
                if (entity.Status == 1 || entity.Status == 2 || entity.Status == 3)
                {
                    ViewBag.Mes = "1";// thành viên đã dc active
                }
                else
                {
                    var verify = HomeFunction.RandomString(20);
                    entity.Verify = verify;
                    entity.Status = 1;
                    _membersBusiness.Edit(entity);
                    ViewBag.Mes = "0";
                }
            }
            else
            {
                ViewBag.Mes = "2";// Chuỗi sai
            }
            return View();
        }

        public ActionResult VerifyForget(string vr)
        {
            ViewBag.Mes = "";
            var member = _membersBusiness.GetDynamicQuery().Where(x => x.Verify == vr);
            if (member.Any())
            {
                var entity = member.First();
                if (entity.Status == 1)
                {
                    var verify = HomeFunction.RandomString(20);
                    ViewBag.Mes = "1";
                    var pass = BuyGroup365.Models.Home.HomeFunction.RandomString(8);
                    var mdpass = Common.util.Common.GetMd5Sum(pass);
                    entity.Password = mdpass;
                    entity.Verify = verify;
                    entity.PasswordModifyDate = DateTime.Now;
                    _membersBusiness.Edit(entity);

                    #region SendMail

                    var ho = Request.ServerVariables["HTTP_HOST"];
                    //string sub = "Active tài khoản thành viên";
                    var url = "http://" + ho + "/";
                    var link = "<a href=\"" + url +
                               "\" style=\"color: #0388cd\" target=\"_blank\"><span class=\"il\">BUYGROUP365</span> – Buygroup365.vn</a>";
                    var html =
                        "<tr><td style=\"padding: 14px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>Quý khách đã thay đổi mật khẩu thành công <span class=\"il\">BUYGROUP365</span>!</b></td> </tr>" +
                        "<tr> <td style=\"padding: 4px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>Vui lòng nhấn vào đường dẫn dưới đây để tiếp tục mua hàng:</b></td></tr>" +
                        "<tr> <td style=\"padding: 4px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>" +
                        link + "</td></tr>" +
                        "<tr> <td style=\"padding: 4px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>Tên đăng nhập và mật khẩu của bạn là:  " +
                        entity.MemberProfile.Emaill + " hoặc" + entity.UserName + " ; " + pass + "</b></td></tr>" +
                        "<tr><td style=\"padding: 7px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\">Mọi thắc mắc và góp ý, xin Quý khách vui lòng liên hệ với chúng tôi qua:</td></tr>";

                    //   var link = "<a href=\"" + url + "\">BUYGROUP365 – Xác nhận đăng ký thành công</a>";
                    //  string body = ControllerExtensions.RenderRazorViewToString(MailTempController, "DetailCart", link);
                    var contentRegister = new ContentRegister { LinkActive = html, UserName = "", Pass = "" };
                    string body = ControllerExtensions.RenderRazorViewToString(this, "MesengerRegister", contentRegister);
                    Function.ObjMailSend objmail = new Function.ObjMailSend();
                    var mailsend = new SystemSettingBusiness().GetDynamicQuery().First(x => x.Key == "mail_noreply");
                    var acount = mailsend.Value.Split('|');
                    objmail.FromMail = acount[0];
                    objmail.PassFromMail = acount[1];
                    objmail.ToMail = entity.MemberProfile.Emaill;
                    Function.email_send(objmail, "[Buygroup365]Quên mật khẩu (" + DateTime.Now + ")", body);

                    #endregion SendMail
                }
                else
                {
                    //ViewBag.Mes = "<h4>Chào bạn " + entity.MemberProfile.FirstName + ".Tài khoản của bạn đang bị khóa, hoặc chưa được kick hoạt !</h4>";
                    ViewBag.Mes = 0; // tài khoản bị khóa hoặc chưa được kick hoạt
                }
            }
            else
            {
                ViewBag.Mes = "2";//mã chỉ dc sư dụng 1 lần
            }
            return View(member);
        }

        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forgot(string mail)
        {
            ViewBag.Mes = "";
            // Member member = SessionUtility.GetSessionMember(Session);
            var member = _membersBusiness.GetDynamicQuery().Where(x => x.MemberProfile.Emaill == mail).ToList();
            if (member.Any())
            {
                ViewBag.Mes = "Chúng tôi đã gửi link thay đổi mật khẩu của bạn, vui lòng kiểm tra mail để hoàn tất tiến trình";
                var entity = member.First();
                var verify = HomeFunction.RandomString(15);
                entity.Verify = verify;
                _membersBusiness.Edit(entity);

                #region SendMail

                var ho = Request.ServerVariables["HTTP_HOST"];
                //string sub = "Active tài khoản thành viên";
                var url = "http://" + ho + "/Login/VerifyForget?vr=" + verify;
                var link = "<a href=\"" + url + "\" style=\"color: #0388cd\" target=\"_blank\"><span class=\"il\">BUYGROUP365</span> – Buygroup365.vn</a>";
                var html =
                    "<tr><td style=\"padding: 14px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>Xác nhận thay đổi mật khẩu của quý khách trên <span class=\"il\">BUYGROUP365</span>!</b></td> </tr>" +
                    "<tr> <td style=\"padding: 4px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>Vui lòng nhấn vào đường dẫn dưới đây để xác nhận:</b></td></tr>" +
                    "<tr> <td style=\"padding: 4px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\"><b>" + link + "</td></tr>" +
                    "<tr><td style=\"padding: 7px 10px 0 24px; font-family: Arial,Helvetica,sans-serif; font-size: 12px; color: #666666\">Mọi thắc mắc và góp ý, xin Quý khách vui lòng liên hệ với chúng tôi qua:</td></tr>";

                //   var link = "<a href=\"" + url + "\">BUYGROUP365 – Xác nhận đăng ký thành công</a>";
                //  string body = ControllerExtensions.RenderRazorViewToString(MailTempController, "DetailCart", link);
                var contentRegister = new ContentRegister { LinkActive = html, UserName = "", Pass = "" };
                string body = ControllerExtensions.RenderRazorViewToString(this, "MesengerRegister", contentRegister);
                Function.ObjMailSend objmail = new Function.ObjMailSend();
                var mailsend = new SystemSettingBusiness().GetDynamicQuery().First(x => x.Key == "mail_noreply");
                var acount = mailsend.Value.Split('|');
                objmail.FromMail = acount[0];
                objmail.PassFromMail = acount[1];
                objmail.ToMail = entity.MemberProfile.Emaill;

                Function.email_send(objmail, "[Buygroup365]Xác nhận quên mật khẩu (" + DateTime.Now + ")", body);

                #endregion SendMail
            }
            else
            {
                ViewBag.Mes = "";
            }
            return View(member);
        }
    }
}