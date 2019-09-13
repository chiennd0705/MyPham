using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class TagsController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private TagsBusiness tagsBusiness = new TagsBusiness();
        private readonly FriendlyUrlBusiness _friendlyUrlBusines = new FriendlyUrlBusiness();
        private string duoilink = System.Configuration.ConfigurationSettings.AppSettings["ExtensionLink"];

        public TagsController()
        {
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

                var list = tagsBusiness.GetList(key).ToPagedList(currentPageIndex, 20);

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
        public ActionResult Update(long id, string btnSubmit, string TagName, string TagUrl, int Type, string Description, string SeoTitle, string SeoKeyword, string SeoDescription, string TagNamecu, string ShareTitle, string ShareKeyword, string ShareDescription, string ImageSource)
        {
            try
            {
                Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();
                string friendly = "";
                if (btnSubmit == "Thêm")
                {
                    Tag tag = new Tag();
                    tag.TagName = TagName;

                    tag.TagUrl = friendly;

                    tag.Description = Description;
                    tag.SeoTitle = SeoTitle;
                    tag.SeoKeyword = SeoKeyword;
                    tag.SeoDescription = SeoDescription;
                    tag.ShareTitle = ShareTitle;
                    tag.ShareKeyword = ShareKeyword;
                    tag.ShareDescription = ShareDescription;
                    tag.Type = Type;
                    tag.DateCreate = DateTime.Now;
                    tag.ModifyDate = DateTime.Now;

                    if (ImageSource != "")
                    {
                        tag.ImageSource = ImageSource;
                    }

                    try
                    {
                        Random rnd = new Random();
                        int ngaunhien = rnd.Next(1, 100);

                        if (tag.Type == 1)
                        {
                            friendlyUrl.ControllerName = "Home";
                            friendlyUrl.ActionName = "Tags";
                            friendly = "san-pham/" + Function.ConvertFileName(TagName) + duoilink;
                        }
                        if (tag.Type == 2)
                        {
                            friendlyUrl.ControllerName = "News";
                            friendlyUrl.ActionName = "Tags";
                            friendly = "tin-tuc/" + Function.ConvertFileName(TagName) + duoilink;
                        }
                        if (tag.Type == 3)
                        {
                            friendlyUrl.ControllerName = "News";
                            friendlyUrl.ActionName = "author";
                            friendly = "author/" + Function.ConvertFileName(TagName) + duoilink;
                        }
                        tag.TagUrl = friendly;
                        tagsBusiness.AddNew(tag);

                        friendlyUrl.ItemId = tag.Id;
                        friendlyUrl.Link = friendly;
                        friendlyUrl.NameLink = friendly + ngaunhien.ToString();
                        friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                        friendlyUrl.Order = 0;

                        _friendlyUrlBusines.InsertLink(friendlyUrl);
                        RefreshFriendly.BindataSiteUrl();
                    }
                    catch { }
                }
                else
                {
                    var tag = tagsBusiness.GetById(id);
                    tag.TagName = TagName;

                    friendly = Function.ConvertFileName(TagName) + duoilink;
                    tag.TagUrl = friendly;
                    tag.Description = Description;
                    tag.SeoTitle = SeoTitle;
                    tag.SeoKeyword = SeoKeyword;
                    tag.SeoDescription = SeoDescription;
                    tag.ShareTitle = ShareTitle;
                    tag.ShareKeyword = ShareKeyword;
                    tag.ShareDescription = ShareDescription;
                    tag.Type = Type;
                    if (ImageSource != "")
                    {
                        tag.ImageSource = ImageSource;
                    }
                    tag.ModifyDate = DateTime.Now;

                    try
                    {
                        string linkcu = "";

                        if (TagNamecu != friendly)
                        {
                            linkcu = Function.ConvertUrlString(TagNamecu);
                        }
                        else
                        {
                            linkcu = "";
                        }
                        Random rnd = new Random();
                        int ngaunhien = rnd.Next(1, 100);
                        friendlyUrl.ItemId = tag.Id;

                        if (tag.Type == 1)
                        {
                            friendlyUrl.ControllerName = "Home";
                            friendlyUrl.ActionName = "Tags";
                            friendly = "san-pham/" + Function.ConvertFileName(TagName) + duoilink;
                        }
                        if (tag.Type == 2)
                        {
                            friendlyUrl.ControllerName = "News";
                            friendlyUrl.ActionName = "Tags";
                            friendly = "tin-tuc/" + Function.ConvertFileName(TagName) + duoilink;
                        }
                        if (tag.Type == 3)
                        {
                            friendlyUrl.ControllerName = "News";
                            friendlyUrl.ActionName = "Author";
                            friendly = "author/" + Function.ConvertFileName(TagName) + duoilink;
                        }
                        friendlyUrl.Link = friendly;
                        tag.TagUrl = friendly;
                        friendlyUrl.NameLink = friendly + ngaunhien.ToString();
                        friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                        friendlyUrl.Order = 0;

                        _friendlyUrlBusines.UpdateLink(linkcu, friendlyUrl);
                        RefreshFriendly.BindataSiteUrl();
                    }
                    catch { }
                    tagsBusiness.Edit(tag);
                }
                Response.Redirect("/Manage/Tags/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/Tags/Index");
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
                    tagsBusiness.Remove(id);
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