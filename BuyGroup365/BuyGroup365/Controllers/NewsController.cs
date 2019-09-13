using Business.ClassBusiness;
using BuyGroup365.Models.Common;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Web.Mvc;

namespace BuyGroup365.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsBusiness _newsBusiness = new NewsBusiness();
        private readonly NewsGroupBusiness _newsGroupBusiness = new NewsGroupBusiness();
        private readonly CommentsNewsBusiness _CommentnewsGroupBusiness = new CommentsNewsBusiness();
        private int Page_Size = 40;
        private int Page_Block = 4;
        // GET: /News/

        [OutputCache(CacheProfile = "cacheListNew")]
        public ActionResult Index(long id = 0, int page = 1, int pageSize = 20)
        {
            try
            {
                NewsBusiness newsBusiness = new NewsBusiness();

                int totalRecord = 0;
                int totalPage = 0;
                List<Common.SearchNewByGroupID_Result> model = new List<Common.SearchNewByGroupID_Result>();

                ViewBag.GroupID = id;
                model = _newsBusiness.ListByNewsIdNewsGroup(id, ref totalRecord, page, pageSize);
                ViewBag.NewsGroup = _newsGroupBusiness.GetById(id);

                ViewBag.Total = totalRecord;
                ViewBag.Page = page;

                int maxPage = 1;

                if (totalRecord % pageSize == 0)
                    totalPage = totalRecord / pageSize;
                else
                    totalPage = totalRecord / pageSize + 1;

                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;
                ViewData["ListNewGroup"] = ListNewGroupByParentID(id);
                NewsGroupBusiness newsGroupBusiness = new Business.ClassBusiness.NewsGroupBusiness();

                Common.NewsGroups newsGroup = newsGroupBusiness.GetById(id);

                if (!string.IsNullOrEmpty(newsGroup.SeoTitle))
                    ViewBag.Title = newsGroup.SeoTitle;
                else
                    ViewBag.Title = newsGroup.NewsGroupName;
                ViewBag.Description = newsGroup.SeoDescription;
                ViewBag.Keywords = newsGroup.SeoKeyword;

                ViewBag.ShareTitle = newsGroup.ShareTitle;
                ViewBag.ShareKeyword = newsGroup.ShareKeyword;
                ViewBag.ShareDescription = newsGroup.ShareDescription;

                ViewBag.published_time = newsGroup.CreateDate;
                ViewBag.updated_time = newsGroup.ModifyDate;

                ViewBag.image = newsGroup.ImageSource;

                ViewBag.CatalogUrl = Function.ConvertFileName(newsGroup.NewsGroupName);
                InitBreadcrumbs(newsGroup);
                return View(model);
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        public ActionResult Khuyenmai(long id = 0, int page = 1, int pageSize = 10)
        {
            try
            {
                NewsBusiness newsBusiness = new NewsBusiness();

                int totalRecord = 0;
                int totalPage = 0;
                List<Common.SearchNewByGroupID_Result> model = new List<Common.SearchNewByGroupID_Result>();
                ViewBag.GroupID = id;

                model = _newsBusiness.ListByNewsIdNewsGroup(id, ref totalRecord, page, pageSize);
                ViewBag.NewsGroup = _newsGroupBusiness.GetById(id);

                ViewBag.Total = totalRecord;
                ViewBag.Page = page;

                int maxPage = 1;

                totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;

                NewsGroupBusiness newsGroupBusiness = new Business.ClassBusiness.NewsGroupBusiness();

                Common.NewsGroups newsGroup = newsGroupBusiness.GetById(id);

                if (string.IsNullOrEmpty(newsGroup.SeoTitle))

                    ViewBag.Title = newsGroup.NewsGroupName;
                else
                    ViewBag.Title = newsGroup.SeoTitle;
                ViewBag.Title = newsGroup.SeoTitle;
                ViewBag.Description = newsGroup.SeoDescription;
                ViewBag.Keywords = newsGroup.SeoKeyword;

                ViewBag.ShareTitle = newsGroup.ShareTitle;
                ViewBag.ShareKeyword = newsGroup.ShareKeyword;
                ViewBag.ShareDescription = newsGroup.ShareDescription;
                ViewBag.published_time = newsGroup.CreateDate;
                ViewBag.updated_time = newsGroup.ModifyDate;

                ViewBag.image = newsGroup.ImageSource;

                ViewBag.TotalVote = newsGroup.TotalVote;
                ViewBag.VoteScore = newsGroup.VoteScore;

                ViewBag.CatalogUrl = Function.ConvertFileName(newsGroup.NewsGroupName);

                return View(model);
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
            return View();
        }

        public string ListNewGroupByParentID(long id)
        {
            string ListHtml = "";
            try
            {
                NewsGroupBusiness newsGroupBusiness = new NewsGroupBusiness();
                List<NewsGroups> newsGroups = new List<NewsGroups>();
                newsGroups = newsGroupBusiness.SearchListNewGroups(id);
                if (newsGroups.Count > 0)
                {
                    ListHtml = " <ul class=\"cate-ul\">";
                    foreach (NewsGroups tem in newsGroups)
                    {
                        ListHtml += "<li><a href=\"" + Util.ReturnLinkFull(tem.FriendlyUrl) + "\">" + tem.NewsGroupName + "</a></li>";
                    }
                    ListHtml += "</ul>";
                }
            }
            catch (Exception)
            {
                //Write log
                throw;
            }

            return ListHtml;
        }

        public JsonResult GetDetailNews(long id)
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

        [OutputCache(CacheProfile = "cacheNewDetail")]
        public ActionResult Detail(long id)
        {
            try
            {
                var dbNews = _newsBusiness.GetById(id);
                ViewBag.NewsGroup = _newsGroupBusiness.GetById(dbNews.NewsGroupId);
                ViewBag.NID = id;
                int count = 0;
                count = _CommentnewsGroupBusiness.CountCommentByNewID(id);
                ViewBag.CountComment = count;
                if (string.IsNullOrEmpty(dbNews.SeoTitle))

                    ViewBag.Title = dbNews.Title;
                else
                    ViewBag.Title = dbNews.SeoTitle;

                ViewBag.Description = dbNews.SeoDescription;
                ViewBag.Keywords = dbNews.SeoKeyword;
                ViewBag.NewsGroupId = dbNews.NewsGroupId;
                ViewBag.ShareTitle = dbNews.ShareTitle;
                ViewBag.ShareKeyword = dbNews.ShareKeyword;
                ViewBag.ShareDescription = dbNews.ShareDescription;
                ViewBag.published_time = dbNews.CreateDate;
                ViewBag.updated_time = dbNews.ModifyDate;
                ViewBag.author = dbNews.Author;
                ViewBag.image = dbNews.ImageSource;
                ViewData["ListNewGroup"] = ListNewGroupByParentID(dbNews.NewsGroupId);
                string strTags = InitTags(dbNews.Tags);
                ViewData["Tags"] = strTags;
                Random rnd = new Random();
                int number = rnd.Next(1, 1000);
                ViewData["TotalView"] = number + 1000;
                InitBreadcrumbs(dbNews.NewsGroups);
                return View(dbNews);
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        #region TAGS

        public ActionResult Tags(long id)
        {
            TagsBusiness tagsBusiness = new TagsBusiness();
            NewsBusiness newsBusiness = new Business.ClassBusiness.NewsBusiness();

            Tag t = tagsBusiness.GetById(id);
            ViewBag.Tag = t;
            if (t != null)
            {
                if (!string.IsNullOrEmpty(t.SeoTitle))
                    ViewData["Title"] = t.SeoTitle;
                else
                    ViewData["Title"] = t.TagName;
                ViewBag.Description = t.SeoDescription;
                ViewBag.Description_ = t.Description;
                ViewBag.Keywords = t.SeoKeyword;
                ViewBag.ShareTitle = t.ShareTitle;
                ViewBag.ShareKeyword = t.ShareKeyword;
                ViewBag.ShareDescription = t.ShareDescription;
                ViewBag.published_time = t.DateCreate;
                ViewBag.updated_time = t.ModifyDate;
                ViewBag.image = t.ImageSource;

                ViewData["TagName"] = t.TagName;

                int page = 1;

                if (Request.QueryString["page"] != null)
                {
                    page = int.Parse(Request.QueryString["page"]);
                }
                //  List<News> listNews = new List<News>();
                List<News> listNews = newsBusiness.GetByTag(t.Id.ToString(), page, Page_Size);

                int count = newsBusiness.CountByTag(t.Id.ToString());

                ////Khởi tạo link tag

                string srtUrl = string.Format("/{0}", t.TagUrl);

                Paging(count, Page_Block, Page_Size, page, page, srtUrl);

                return View(listNews);
            }
            else
            {
            }
            return View();
        }

        protected string InitTags(string tags)
        {
            string html = string.Empty;

            if (!string.IsNullOrEmpty(tags))
            {
                string[] arraystrTags = tags.Split(';');

                long[] arrayTags = new long[arraystrTags.Count()];
                int i = 0;
                foreach (string s in arraystrTags)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        try
                        {
                            arrayTags[i] = long.Parse(s);
                            i++;
                        }
                        catch { }
                    }
                }
                TagsBusiness tagsBusiness = new TagsBusiness();
                var temp = tagsBusiness.GetByArrayId(arrayTags);
                foreach (Tag t in temp)
                {
                    html += "<li><a href=\"" + Models.Common.Util.ReturnLinkFull(t.TagUrl) + "\">" + t.TagName + "</a></li> ";
                }

                if (!string.IsNullOrEmpty(html))
                {
                    html = html.Substring(0, html.Length - 2);
                }
            }
            return html;
        }

        [OutputCache(CacheProfile = "cacheListNew")]
        public ActionResult Author(long id)
        {
            TagsBusiness tagsBusiness = new TagsBusiness();
            NewsBusiness newsBusiness = new Business.ClassBusiness.NewsBusiness();

            Tag t = tagsBusiness.GetById(id);
            ViewBag.Tag = t;
            if (t != null)
            {
                if (!string.IsNullOrEmpty(t.SeoTitle))
                    ViewData["Title"] = t.SeoTitle;
                else
                    ViewData["Title"] = t.TagName;
                ViewBag.Description = t.SeoDescription;
                ViewBag.Description_ = t.Description;
                ViewBag.Keywords = t.SeoKeyword;
                ViewBag.ShareTitle = t.ShareTitle;
                ViewBag.ShareKeyword = t.ShareKeyword;
                ViewBag.ShareDescription = t.ShareDescription;
                ViewBag.published_time = t.DateCreate;
                ViewBag.updated_time = t.ModifyDate;
                ViewBag.image = t.ImageSource;

                ViewData["TagName"] = t.TagName;

                int page = 1;

                if (Request.QueryString["page"] != null)
                {
                    page = int.Parse(Request.QueryString["page"]);
                }
                //  List<News> listNews = new List<News>();
                List<News> listNews = newsBusiness.GetByTagAuthor(t.Id.ToString(), page, Page_Size);

                int count = newsBusiness.CountByTag(t.Id.ToString());

                ////Khởi tạo link tag

                string srtUrl = string.Format("/{0}", t.TagUrl);

                Paging(count, Page_Block, Page_Size, page, page, srtUrl);

                return View(listNews);
            }
            else
            {
            }
            return View();
        }

        #endregion TAGS

        #region PAGING

        protected void Paging(int recordNumber, int SIZE_PAGE, int SIZE_OF_PAGE, object obj, int page, string url)
        {
            int numberPage = 0;
            //Tạo mảng các trang: 1 2 3 ...
            int[] arayPage = Common.util.Function.Paging(recordNumber, obj, SIZE_PAGE, SIZE_OF_PAGE, ref page, ref numberPage);
            // stt = (page - 1) * SIZE_OF_PAGE + 1;
            string html = "";

            if (url.Contains("?")) url += "&page=";
            else
                url += "?page=";
            url = BuyGroup365.Models.Common.Util.ReturnLinkFull(url);
            if (page > 1)
            {
                html += "<li class=\"bor-none\"><a class=\"previous i-previous\" href=\"" + url + (page - 1) + "\" title=\"Trang trước\"><i class=\"fa fa-angle-double-left\">&nbsp;</i> Trước</a></li>";
            }
            //int index = 0;
            foreach (int i in arayPage)
            {
                if (i > 0)
                {
                    if (!i.Equals(page))
                        html += string.Format("<li><a href=\"{0}\">{1}</a></li>", url + i, i);
                    else
                        html += string.Format("<li class=\"current\">{0}</li>", i);
                }
            }

            if (page < numberPage)
            {
                html += "<li class=\"bor-none\"><a class=\"previous i-next\" href=\"" + url + (page + 1) + "\" title=\"Trang sau\">Tiếp <i class=\"fa fa-angle-double-right\">&nbsp;</i></a></li>";
            }

            ViewData["Paging"] = string.Format("<div class=\"pager\"><div class=\"pages\"><strong>Page:</strong><ol>{0}</ol></div></div>", html);
        }

        public JsonResult CommentNew(long id, string Name, string Content)
        {
            bool check = false;
            try
            {
                CommentsNewsBusiness commentbusiness = new CommentsNewsBusiness();
                Common.CommentsNew comment = new Common.CommentsNew();
                comment.Content = Content;
                comment.CreateDate = DateTime.Now;
                comment.NewId = id;
                comment.Status = 1;
                comment.NickName = Name;
                commentbusiness.AddNew(comment);
                check = true;
            }
            catch
            {
                check = false;
            }
            return Json(new
            {
                data = true,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion PAGING

        #region FAQ

        public ActionResult FAQ(int page = 1, int pageSize = 6)
        {
            try
            {
                Business.ClassBusiness.CommentsNewsBusiness commentsBusiness = new Business.ClassBusiness.CommentsNewsBusiness();

                int totalRecord = 0;
                int totalPage = 0;
                List<Common.CommentsNew> listcomments = commentsBusiness.ListByCommentsNew(null, ref totalRecord, page, pageSize);
                ViewBag.Total = totalRecord;
                ViewBag.Page = page;

                int maxPage = 1;

                if (totalRecord % pageSize == 0)
                    totalPage = totalRecord / pageSize;
                else
                    totalPage = totalRecord / pageSize + 1;

                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;
                return View(listcomments);
            }
            catch (FaultException ex)
            {
                var exep = Function.GetExeption(ex);
                var codeExp = exep[1];
                string url = "Error/ErrorFunction/" + codeExp;
                return RedirectToActionPermanent(url);
            }
        }

        public ActionResult DetailFAQ(long id)
        {
            try
            {
                Business.ClassBusiness.CommentsNewsBusiness commentsBusiness = new Business.ClassBusiness.CommentsNewsBusiness();

                var faqdetail = commentsBusiness.GetById(id);
                return View(faqdetail);
            }
            catch (Exception)
            {
                //Write log
                throw;
            }
        }

        #endregion FAQ

        #region RSS NEW

        [HttpGet]
        public ActionResult ListRss()
        {
            NewsGroupBusiness newsGroupBusiness = new NewsGroupBusiness();
            List<Common.NewsGroups> model = new List<Common.NewsGroups>();

            model = newsGroupBusiness.GetListNewsGroup();

            return View(model);
        }

        [HttpGet]
        public ActionResult RssFeed(long id)
        {
            NewsBusiness newsBusiness = new NewsBusiness();
            List<Common.SearchNewByGroupID_Result> model = new List<Common.SearchNewByGroupID_Result>();
            int totalRecord = 0;
            model = _newsBusiness.ListByNewsIdNewsGroup(id, ref totalRecord, 1, 10000);

            var items = new List<SyndicationItem>();
            foreach (SearchNewByGroupID_Result re in model)
            {
                var item = new SyndicationItem()
                {
                    Id = re.Id.ToString(),
                    Title = SyndicationContent.CreatePlaintextContent(re.Title),
                    Content = SyndicationContent.CreateHtmlContent(re.Descriptions),
                    PublishDate = re.CreateDate
                };
                string url = Util.ReturnLinkFull(re.FriendlyUrl);
                item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(url)));//Nothing alternate about it. It is the MAIN link for the item.
                items.Add(item);
            }

            return new RssFeed(title: "Greatness",
                               items: items,
                               contentType: "application/rss+xml",
                               description: String.Format("Sooper Dooper {0}", Guid.NewGuid()));
        }

        #endregion RSS NEW

        protected void InitBreadcrumbs(NewsGroups catalog)
        {
            string html = string.Format("<li><a href=\"" + Util.ReturnLinkFull(catalog.FriendlyUrl) + "\" title=\"\" itemprop=\"url\"><strong itemprop=\"title\"> {0}</strong></a></li>", catalog.NewsGroupName);
            NewsGroupBusiness newsGroupgBusiness = new NewsGroupBusiness();
            try
            {
                while (catalog.ParentId != -1)
                {
                    catalog = newsGroupgBusiness.GetById(catalog.ParentId);
                    html = string.Format("<li><a href=\"{1}\" title=\"\" itemprop=\"url\" class=\"active\"><strong itemprop=\"title\">{0}</strong></a></li>", catalog.NewsGroupName, Util.ReturnLinkFull(catalog.FriendlyUrl)) + html;
                }
            }
            catch { }
            ViewData["Breadcrumbs"] = html;
        }
    }
}