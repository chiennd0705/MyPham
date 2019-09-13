using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using BuyGroup365.Models.Member;
using Common.util;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace BuyGroup365.Areas.Manage.Controllers
{
    public class NewsController : BaseController
    {
        private readonly NewsBusiness _newsBusines = new NewsBusiness();
        private readonly LoadCombo _loadCombo = new LoadCombo();
        private readonly NewsGroupBusiness _newsGroupBusiness = new NewsGroupBusiness();
        private readonly FriendlyUrlBusiness _friendlyUrlBusines = new FriendlyUrlBusiness();
        private readonly CommentsNewsBusiness _commentsNewBusiness = new CommentsNewsBusiness();
        private string duoilink = System.Configuration.ConfigurationSettings.AppSettings["ExtensionLink"];

        //
        // GET: /Manage/News/
        public ActionResult Index(string key, int? page)
        {
            try
            {
                ViewBag.statusCatg = _loadCombo.InitSelectListItemStatusNews();
                List<LoadCombo.DropdowNews> listDropdowNews = new List<LoadCombo.DropdowNews>();
                ViewBag.parent = _loadCombo.SearchNewsByName(ref listDropdowNews);
                ViewBag.IsPublic = _loadCombo.IsPublicForNewsGroup();

                //ListGroup from name of group
                ViewBag.listGroupNewsGroupWithNamme = _loadCombo.InitSelectListItemStatusNewsGroup();

                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _newsBusines.GetByKey(key).OrderByDescending(p => p.ModifyDate).ToPagedList(currentPageIndex, 20);

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
        public ActionResult CreateNews(string title, string descriptionCreate, string summaryCreate, HttpPostedFileBase imageSource, int statusCatg, int IsPublic, int parent)
        {
            try
            {
                Common.News news = new Common.News();

                news.Title = title;
                news.Descriptions = descriptionCreate;
                news.Summary = summaryCreate;

                if (imageSource != null && imageSource.ContentLength > 0)
                {
                    // TourInfo entity=new TourInfo();
                    //Random rdImage = new Random();
                    string randomImage = Guid.NewGuid().ToString();
                    string fileNameImage = Common.util.Function.ConvertFileName(imageSource.FileName);
                    string pathImage = HttpContext.Server.MapPath("~/FileUpload");
                    var strurlimage = Common.util.Function.ResizeImageNew(imageSource, 300, 300, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(imageSource, 500, 500, pathImage, randomImage);
                    Common.util.Function.ResizeImageNew(imageSource, 1000, 1000, pathImage, randomImage);
                    news.ImageSource = strurlimage;
                }

                news.Status = statusCatg;
                if (IsPublic == 0)
                {
                    news.isPublic = true;
                }
                else
                {
                    news.isPublic = false;
                }

                //lấy tạm -1 sau lấy qua Id của người login
                news.AdminIDApproval = -1;

                if (parent > -1)
                {
                    news.NewsGroupId = parent;
                }
                else
                {
                    //xử lý Lỗi không chọn nhóm tin
                    //không chọn mặc định sẽ là 1
                    var listNewsGroup = _newsGroupBusiness.GetListNewsGroup();
                    if (listNewsGroup == null)
                    {
                        //chưa có nhóm tin
                        //tạo nhóm tin trước khi tạo tin tức
                        return RedirectToAction("NewsGroup");
                    }
                    else
                    {
                        //lấy id của nhóm tin đầu tiên (mặc định) nếu người dùng không chọn
                        var newsGroupFirst = listNewsGroup.First();
                        news.NewsGroupId = newsGroupFirst.Id;
                    }
                }
                news.CreateDate = DateTime.Now;
                news.ModifyDate = DateTime.Now;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //witrelog
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult AddNews()
        {
            List<LoadCombo.DropdowNews> listDropdowNews = new List<LoadCombo.DropdowNews>();
            ViewBag.NewsGroupId = _loadCombo.SearchNewsByName(ref listDropdowNews);

            //ListGroup from name of group
            ViewBag.listGroupNewsGroupWithNamme = _loadCombo.InitSelectListItemStatusNewsGroup();

            ViewBag.Tags = GetTag(0, 2, "");
            ViewBag.Author = GetAuthor(0, 3, "");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNews(Common.News obj, string IsPublic, string ImageSource, string[] tags, string[] Author, string IsActive, string ModifyDate)
        {
            try
            {
                Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();

                string friendly = "tin-tuc";
                obj.Status = 1;
                obj.CreateDate = DateTime.Now;
                try
                {
                    if (ModifyDate != null)
                        obj.ModifyDate = DateTime.ParseExact(ModifyDate.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.CurrentCulture);
                    else
                        obj.ModifyDate = DateTime.Now;
                }
                catch { obj.ModifyDate = DateTime.Now; }

                if (!string.IsNullOrEmpty(IsPublic) && IsPublic == "1") obj.isPublic = true;
                else obj.isPublic = false;

                if (!string.IsNullOrEmpty(IsActive) && IsActive == "1") obj.IsActive = true;
                else obj.IsActive = false;

                //if (string.IsNullOrEmpty(obj.FriendlyUrl))
                //{
                friendly = Function.ConvertFileName(_newsGroupBusiness.GetById(obj.NewsGroupId).NewsGroupName) + "/" + Function.ConvertFileName(obj.Title) + duoilink;
                //}
                obj.FriendlyUrl = friendly;

                if (ImageSource != "")
                {
                    obj.ImageSource = ImageSource;
                }

                if (tags != null && tags.Count() > 0)
                {
                    obj.Tags = "";
                    foreach (string tag in tags)
                    {
                        obj.Tags = obj.Tags + tag + ";";
                    }

                    obj.Tags = obj.Tags.Substring(0, obj.Tags.Count() - 1);
                }
                if (Author != null && Author.Count() > 0)
                {
                    obj.Author = "";
                    foreach (string auth in Author)
                    {
                        obj.Author = obj.Author + auth + ";";
                    }

                    obj.Author = obj.Author.Substring(0, obj.Author.Count() - 1);
                }
                long newsid;
                _newsBusines.AddNew(obj);
                newsid = obj.Id;
                try
                {
                    Random rnd = new Random();
                    int ngaunhien = rnd.Next(1, 100);
                    friendlyUrl.ItemId = newsid;
                    friendlyUrl.Link = friendly;
                    friendlyUrl.ControllerName = "News";
                    friendlyUrl.ActionName = "Detail";
                    friendlyUrl.NameLink = obj.FriendlyUrl + ngaunhien.ToString();
                    friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                    friendlyUrl.Order = 0;

                    _friendlyUrlBusines.InsertLink(friendlyUrl);
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

        [HttpGet]
        public ActionResult EditNews(long id)
        {
            List<LoadCombo.DropdowNews> listDropdowNews = new List<LoadCombo.DropdowNews>();
            ViewBag.NewsGroupId = _loadCombo.SearchNewsByName(ref listDropdowNews);

            //ListGroup from name of group
            ViewBag.listGroupNewsGroupWithNamme = _loadCombo.InitSelectListItemStatusNewsGroup();

            var record = _newsBusines.GetById(id);
            ViewBag.Tags = GetTag(id, 2, "");
            ViewBag.Author = GetAuthor(id, 3, "");
            return View(record);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNews(Common.News obj, string IsPublic, string ImageSource, string[] tags, string[] Author, string FriendlyUrlhd, string IsActive, string ModifyDate)
        {
            try
            {
                var record = _newsBusines.GetById(obj.Id);

                string friendly = "tin-tuc";
                record.Status = obj.Status;
                try
                {
                    if (ModifyDate != null)
                        record.ModifyDate = DateTime.ParseExact(ModifyDate.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.CurrentCulture);
                    else
                        record.ModifyDate = DateTime.Now;
                }
                catch
                {
                    record.ModifyDate = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(IsPublic) && IsPublic == "1") record.isPublic = true;
                else record.isPublic = false;

                if (!string.IsNullOrEmpty(IsActive) && IsActive == "1") record.IsActive = true;
                else record.IsActive = false;

                //if (string.IsNullOrEmpty(obj.FriendlyUrl))
                //{
                friendly = Function.ConvertFileName(_newsGroupBusiness.GetById(obj.NewsGroupId).NewsGroupName) + "/" + Function.ConvertFileName(obj.Title) + duoilink;
                //}
                //else
                //{
                //    friendly = _newsGroupBusiness.GetById(obj.NewsGroupId).FriendlyUrl + "/" + obj.FriendlyUrl;

                //}

                record.FriendlyUrl = friendly;
                if (ImageSource != "")
                {
                    record.ImageSource = ImageSource;
                }
                else
                {
                    //  record.ImageSource = obj.ImageSource;
                }

                record.Title = obj.Title;
                record.Descriptions = obj.Descriptions;
                record.SeoDescription = obj.SeoDescription;
                record.SeoKeyword = obj.SeoKeyword;
                record.SeoTitle = obj.SeoTitle;
                record.ShareDescription = obj.ShareDescription;
                record.ShareKeyword = obj.ShareKeyword;
                record.ShareTitle = obj.ShareTitle;
                record.Summary = obj.Summary;

                if (tags != null && tags.Count() > 0)
                {
                    record.Tags = "";
                    foreach (string tag in tags)
                    {
                        record.Tags = record.Tags + tag + ";";
                    }

                    record.Tags = record.Tags.Substring(0, record.Tags.Count() - 1);
                }
                else
                {
                    record.Tags = "";
                }
                if (Author != null && Author.Count() > 0)
                {
                    record.Author = "";
                    foreach (string auth in Author)
                    {
                        record.Author = record.Author + auth + ";";
                    }

                    record.Author = record.Author.Substring(0, record.Author.Count() - 1);
                }
                else
                {
                    record.Author = "";
                }
                record.NewsGroupId = obj.NewsGroupId;
                _newsBusines.Edit(record);

                try
                {
                    Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();
                    Random rnd = new Random();
                    int ngaunhien = rnd.Next(1, 100);
                    string linkcu = "";

                    friendlyUrl.ItemId = record.Id;
                    friendlyUrl.Link = friendly;
                    friendlyUrl.ControllerName = "News";
                    friendlyUrl.ActionName = "Detail";
                    friendlyUrl.NameLink = record.FriendlyUrl + ngaunhien.ToString();
                    friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                    friendlyUrl.Order = 0;
                    if (FriendlyUrlhd != friendly)
                    {
                        linkcu = FriendlyUrlhd;

                        //  RouteTable.Routes.MapRoute(name: record.FriendlyUrl + ngaunhien.ToString(), url: record.FriendlyUrl, defaults: new { controller = "News", action = "Detail", id = record.Id, }, namespaces: new string[] { "BuyGroup365.Controllers" });
                    }
                    else
                    {
                        linkcu = "";
                    }
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

        [HttpPost]
        public String GetTag(long id, int type, string key)
        {
            try
            {
                TagsBusiness tagsBusiness = new Business.ClassBusiness.TagsBusiness();
                var obj = tagsBusiness.GetList(type, key);

                var htmlcate = "";
                if (id == 0)
                {
                    foreach (var item in obj)
                    {
                        htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"tags\">" + item.TagName + "</label>";
                    }
                }
                else
                {
                    string tags = _newsBusines.GetById(id).Tags;

                    if (!string.IsNullOrEmpty(tags))
                    {
                        string[] arrayTag = tags.Split(';');
                        bool isCheck = false;

                        foreach (var item in obj)
                        {
                            isCheck = false;
                            foreach (var tag in arrayTag)
                            {
                                if (tag == item.Id.ToString()) { isCheck = true; break; }
                            }
                            if (isCheck)
                            {
                                htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"tags\" checked>" + item.TagName + "</label>";
                            }
                            else
                            {
                                htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"tags\">" + item.TagName + "</label>";
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in obj)
                        {
                            htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"tags\">" + item.TagName + "</label>";
                        }
                    }
                }

                return htmlcate;
            }
            catch (Exception)
            {
                return "";
                //write log
                throw;
            }
        }

        [HttpPost]
        public String GetAuthor(long id, int type, string key)
        {
            try
            {
                TagsBusiness tagsBusiness = new Business.ClassBusiness.TagsBusiness();
                var obj = tagsBusiness.GetList(type, key);

                var htmlcate = "";
                if (id == 0)
                {
                    foreach (var item in obj)
                    {
                        htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"Author\">" + item.TagName + "</label>";
                    }
                }
                else
                {
                    string tags = _newsBusines.GetById(id).Author;

                    if (!string.IsNullOrEmpty(tags))
                    {
                        string[] arrayTag = tags.Split(';');
                        bool isCheck = false;

                        foreach (var item in obj)
                        {
                            isCheck = false;
                            foreach (var tag in arrayTag)
                            {
                                if (tag == item.Id.ToString()) { isCheck = true; break; }
                            }
                            if (isCheck)
                            {
                                htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"Author\" checked>" + item.TagName + "</label>";
                            }
                            else
                            {
                                htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"Author\">" + item.TagName + "</label>";
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in obj)
                        {
                            htmlcate += "<label class=\"checkbox-inline\"><input value=\"" + item.Id + "\" type=\"checkbox\" name=\"Author\">" + item.TagName + "</label>";
                        }
                    }
                }

                return htmlcate;
            }
            catch (Exception)
            {
                return "";
                //write log
                throw;
            }
        }

        #region Delete News

        public ActionResult DeleteNews(long id = 0)
        {
            try
            {
                if (id > 0)
                {
                    _newsBusines.Remove(id);
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

        #endregion Delete News

        #region NewsGroup

        public ActionResult NewsGroup(string key, int? page)
        {
            try
            {
                ViewBag.statusCatg = _loadCombo.InitSelectListItemStatusNews();
                List<LoadCombo.DropdowNews> listDropdowNews = new List<LoadCombo.DropdowNews>();
                ViewBag.parent = _loadCombo.SearchNewsByName(ref listDropdowNews);
                ViewBag.IsPublic = _loadCombo.IsPublicForNewsGroup();

                //ListGroup from name of group
                ViewBag.listGroupNewsGroupWithNamme = _loadCombo.InitSelectListItemStatusNewsGroup();

                ViewData["status"] = true;
                ViewData["key"] = key;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                var listobj = _newsGroupBusiness.SearchCategoryByName();

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

        public ActionResult DeleteNewsGroup(long id = 0)
        {
            try
            {
                if (id > 0)
                {
                    _newsGroupBusiness.Remove(id);
                }
                return RedirectToAction("NewsGroup");
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
        public ActionResult AddNewsGroup()
        {
            //ListGroup from name of group
            List<LoadCombo.DropdowNews> listDropdowNews = new List<LoadCombo.DropdowNews>();
            ViewBag.parent = _loadCombo.SearchNewsByName(ref listDropdowNews);

            ViewBag.listGroupNewsGroupWithNamme = _loadCombo.InitSelectListItemStatusNewsGroup();

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNewsGroup(string newsGroupName, string FriendlyUrl, int? IsPublic, int parent, int? IsView1, int? IsView2, string SeoTitle, string SeoKeyword, string SeoDescription, string Descriptions, string ShareKeyword, string ShareTitle, string ShareDescription, string ImageSource)
        {
            try
            {
                string friendly = "";
                Common.NewsGroups newsGroup = new Common.NewsGroups();
                newsGroup.ParentId = parent;
                newsGroup.NewsGroupName = newsGroupName;

                newsGroup.isPublic = (IsPublic == 1);
                friendly = Common.util.Function.ConvertUrlString(newsGroupName) + duoilink;
                newsGroup.FriendlyUrl = friendly;
                // }
                newsGroup.Status = 1;
                newsGroup.CreateDate = DateTime.Now;
                newsGroup.ModifyDate = DateTime.Now;
                newsGroup.AdminIDApproval = 1;
                newsGroup.IsView1 = (IsView1 == 1);
                newsGroup.IsView2 = (IsView2 == 1);
                newsGroup.SeoTitle = SeoTitle;
                newsGroup.SeoKeyword = SeoKeyword;
                newsGroup.SeoDescription = SeoDescription;
                newsGroup.ShareTitle = ShareTitle;
                newsGroup.ShareKeyword = ShareKeyword;
                newsGroup.ShareDescription = ShareDescription;
                newsGroup.Descriptions = Descriptions;

                if (ImageSource != "")
                {
                    newsGroup.ImageSource = ImageSource;
                }
                long Catalogid;
                _newsGroupBusiness.AddNew(newsGroup);
                Catalogid = newsGroup.Id;
                try
                {
                    Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();
                    Random rnd = new Random();
                    int ngaunhien = rnd.Next(1, 100);
                    friendlyUrl.ItemId = Catalogid;
                    friendlyUrl.Link = friendly;
                    friendlyUrl.ControllerName = "News";
                    friendlyUrl.ActionName = "Index";
                    friendlyUrl.NameLink = friendly + ngaunhien.ToString();
                    friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                    friendlyUrl.Order = 0;

                    _friendlyUrlBusines.InsertLink(friendlyUrl);
                    RefreshFriendly.BindataSiteUrl();
                }
                catch { }
                return RedirectToAction("NewsGroup");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult EditNewsgroup(long id)
        {
            //ListGroup from name of group
            var newsGroup = _newsGroupBusiness.GetById(id);
            List<LoadCombo.DropdowNews> listDropdowNews = new List<LoadCombo.DropdowNews>();
            ViewBag.parent = _loadCombo.SearchNewsByName(ref listDropdowNews, newsGroup.ParentId);

            ViewBag.listGroupNewsGroupWithNamme = _loadCombo.InitSelectListItemStatusNewsGroup();

            return View(newsGroup);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNewsgroup(long id, string newsGroupName, string FriendlyUrl, int? IsPublic, int parent, int? IsView1, int? IsView2, string SeoTitle, string SeoKeyword, string SeoDescription, string Descriptions, string FriendlyUrlhd, string ShareKeyword, string ShareTitle, string ShareDescription, string ImageSource)
        {
            try
            {
                string friendly = "";
                var newsGroup = _newsGroupBusiness.GetById(id);

                newsGroup.ParentId = parent;
                newsGroup.NewsGroupName = newsGroupName;
                friendly = Common.util.Function.ConvertUrlString(newsGroupName) + duoilink;
                newsGroup.FriendlyUrl = friendly;
                newsGroup.isPublic = (IsPublic == 1);
                if (ImageSource != "")
                {
                    newsGroup.ImageSource = ImageSource;
                }
                newsGroup.Status = 1;
                newsGroup.CreateDate = DateTime.Now;
                newsGroup.ModifyDate = DateTime.Now;
                newsGroup.AdminIDApproval = 1;
                newsGroup.IsView1 = (IsView1 == 1);
                newsGroup.IsView2 = (IsView2 == 1);
                newsGroup.SeoTitle = SeoTitle;
                newsGroup.SeoKeyword = SeoKeyword;
                newsGroup.SeoDescription = SeoDescription;
                newsGroup.ShareTitle = ShareTitle;
                newsGroup.ShareKeyword = ShareKeyword;
                newsGroup.ShareDescription = ShareDescription;
                newsGroup.Descriptions = Descriptions;
                _newsGroupBusiness.Edit(newsGroup);

                try
                {
                    Common.FriendlyUrl friendlyUrl = new Common.FriendlyUrl();
                    Random rnd = new Random();
                    int ngaunhien = rnd.Next(1, 100);

                    string linkcu = "";

                    if (FriendlyUrlhd != friendly)
                    {
                        linkcu = FriendlyUrlhd;
                    }
                    else
                    {
                        linkcu = "";
                    }
                    friendlyUrl.ItemId = id;
                    friendlyUrl.Link = friendly;
                    friendlyUrl.ControllerName = "News";
                    friendlyUrl.ActionName = "Index";
                    friendlyUrl.NameLink = friendly + ngaunhien.ToString();
                    friendlyUrl.NameSpaces = "BuyGroup365.Controllers";
                    friendlyUrl.Order = 0;

                    _friendlyUrlBusines.UpdateLink(linkcu, friendlyUrl);
                    RefreshFriendly.BindataSiteUrl();
                }
                catch { }
                return RedirectToAction("NewsGroup");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion NewsGroup

        #region Comment

        public ActionResult CommentsNew(string key, int? page)
        {
            try
            {
                ViewData["status"] = true;
                ViewData["key"] = key;

                int currentPageIndex = page.HasValue ? page.Value : 1;

                var list = _commentsNewBusiness.GetListComments(key).ToPagedList(currentPageIndex, 20);

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

        public ActionResult ListFaQ(int? page)
        {
            try
            {
                int currentPageIndex = page.HasValue ? page.Value : 1;
                var listCommebt = _commentsNewBusiness.GetListCommentsNew("").ToPagedList(currentPageIndex, 20);
                return View(listCommebt);
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        public JsonResult PreviewCommnet(long newsid)
        {
            try
            {
                var listCommebt = _commentsNewBusiness.GetDynamicQuery().OrderByDescending(x => x.CreateDate).Where(x => x.NewId == newsid && x.ParentId == null).ToList();
                string body = ControllerExtensions.RenderRazorViewToString(this, "ContentCommentsNew", listCommebt);
                return Json(body, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult DeleteComent(long id)
        {
            try
            {
                _commentsNewBusiness.Remove(id);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ContentCommentsNew()
        {
            return View();
        }

        public JsonResult ApprovedComment(long commentid, int status)
        {
            try
            {
                var Comments = _commentsNewBusiness.GetById(commentid);
                Comments.Status = status;
                _commentsNewBusiness.Edit(Comments);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ReplyComment(long newid, long commentid, int status, string TraLoicontent)
        {
            try
            {
                CommentsNewsBusiness commentbusiness = new CommentsNewsBusiness();
                Common.CommentsNew comment = new Common.CommentsNew();
                comment = commentbusiness.GetCommentsByParentID(commentid);
                if (comment == null)
                {
                    Common.CommentsNew comment1 = new Common.CommentsNew();
                    comment1.Content = TraLoicontent;
                    comment1.CreateDate = DateTime.Now;
                    comment1.NewId = newid;
                    comment1.ParentId = commentid;
                    comment1.Status = 2;
                    comment1.NickName = SessionUtility.GetSessionName(Session);

                    comment1.Email = "";
                    comment1.Rate = 5;
                    commentbusiness.AddNew(comment1);
                }
                else
                {
                    comment.Content = TraLoicontent;
                    commentbusiness.Edit(comment);
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // _logger.Debug(ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Comment
    }
}