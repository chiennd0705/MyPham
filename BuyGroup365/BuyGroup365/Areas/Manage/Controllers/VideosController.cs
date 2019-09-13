using Business.ClassBusiness;
using Common;
using Common.util;
using MvcPaging;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class VideosController : BaseController
    {
        //
        // GET: /Manage/CategoryProduct/

        #region Khai bao biến

        private VideosBusiness videosBusiness = new VideosBusiness();
        private readonly FriendlyUrlBusiness _friendlyUrlBusines = new FriendlyUrlBusiness();

        public VideosController()
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

                var list = videosBusiness.GetList(key).ToPagedList(currentPageIndex, 20);

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
        public ActionResult Update(long id, string btnSubmit, string VideoName, string VideoName_EN, string VideoUrl, string VideoLink, string Description, string SeoTitle, string SeoKeyword, string SeoDescription, string VideoNamecu)
        {
            try
            {
                Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();

                if (btnSubmit == "Thêm")
                {
                    Videos video = new Videos();
                    video.VideoName = VideoName;
                    video.VideoName_EN = VideoName_EN;
                    if (string.IsNullOrEmpty(VideoUrl))
                        video.VideoUrl = Function.ConvertFileName(VideoName);
                    else
                        video.VideoUrl = VideoUrl;
                    video.VideoLink = VideoLink;
                    video.VideoDecription = Description;
                    video.SeoTitle = SeoTitle;
                    video.SeoKeyword = SeoKeyword;
                    video.SeoDescription = SeoDescription;
                    video.DateCreate = DateTime.Now;
                    videosBusiness.AddNew(video);
                    //try
                    //{
                    //    Random rnd = new Random();
                    //    int ngaunhien = rnd.Next(1, 100);
                    //    friendlyUrl.ItemId = video.Id;
                    //    friendlyUrl.Link = video.VideoUrl;

                    //        friendlyUrl.ControllerName = "Videos";
                    //        friendlyUrl.ActionName = "VideoDetail";

                    //    friendlyUrl.NameLink = video.VideoUrl+ngaunhien.ToString() ;
                    //    friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                    //    friendlyUrl.Order = 0;

                    //    _friendlyUrlBusines.InsertLink(friendlyUrl);
                    //    RefreshFriendly.BindataSiteUrl();
                    //}
                    //catch { }
                }
                else
                {
                    var video = videosBusiness.GetById(id);
                    video.VideoName = VideoName;
                    if (!String.IsNullOrEmpty(VideoUrl))
                    {
                        video.VideoUrl = VideoUrl;
                    }
                    else
                    {
                        video.VideoUrl = Function.ConvertFileName(VideoName);
                    }
                    video.VideoLink = VideoLink;
                    video.VideoDecription = Description;
                    video.SeoTitle = SeoTitle;
                    video.SeoKeyword = SeoKeyword;
                    video.SeoDescription = SeoDescription;
                    videosBusiness.Edit(video);

                    //try
                    //{
                    //    string linkcu = "";

                    //    if (VideoNamecu != video.VideoUrl)
                    //    {
                    //        linkcu = Function.ConvertUrlString(VideoNamecu);

                    //    }
                    //    else
                    //    {
                    //        linkcu = "";
                    //    }
                    //    Random rnd = new Random();
                    //    int ngaunhien = rnd.Next(1, 100);
                    //    friendlyUrl.ItemId = video.Id;
                    //    friendlyUrl.Link = video.VideoUrl;
                    //    friendlyUrl.ControllerName = "Videos";
                    //    friendlyUrl.ActionName = "VideoDetail";

                    //    friendlyUrl.NameLink = video.VideoUrl +ngaunhien.ToString();
                    //    friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                    //    friendlyUrl.Order = 0;

                    //    _friendlyUrlBusines.UpdateLink(linkcu, friendlyUrl);
                    //    RefreshFriendly.BindataSiteUrl();
                    //}
                    //catch { }
                }
                Response.Redirect("/Manage/Videos/Index");
            }
            catch (Exception ex)
            {
                Response.Redirect("/Manage/Videos/Index");
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
                    videosBusiness.Remove(id);
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