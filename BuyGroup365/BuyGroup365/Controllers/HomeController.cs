using Business.ClassBusiness;
using BuyGroup365.Areas.Manage.Models;
using BuyGroup365.Models.Common;
using BuyGroup365.Models.Home;
using BuyGroup365.Models.Member;
using Common;
using Common.util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;

namespace BuyGroup365.Controllers
{
    public class HomeController : Controller
    {
        private static string Database_Name = ConfigurationSettings.AppSettings.Get("database_name");
        private int Page_Size = 20;
        private int Page_Block = 4;
        private FriendlyUrlBusiness friendlyUrlBussiness = new FriendlyUrlBusiness();
        private List<FriendlyUrl> ListFriendlyUrl = new List<FriendlyUrl>();
        private SystemSettingBusiness _systemSettingBusiness = new SystemSettingBusiness();
        private NewsBusiness newsBusiness = new NewsBusiness();
        private News news = new News();

        //
        // GET: /Home/
        private static class FirstRequestInitialisation
        {
            private static string host = null;
            private static Object s_lock = new Object();

            // Initialise only on the first request
            public static string Initialise(HttpContext context)
            {
                if (string.IsNullOrEmpty(host))
                {
                    lock (s_lock)
                    {
                        if (string.IsNullOrEmpty(host))
                        {
                            var uri = context.Request.Url;
                            host = uri.GetLeftPart(UriPartial.Authority);
                        }
                    }
                }

                return host;
            }
        }

        [CompactHtmlFilter]
        public ActionResult Index()
        {
            ViewBag.host = Request.RawUrl;
            try
            {
                ViewBag.Title = _systemSettingBusiness.GetDetailByKey("Title");
                ViewBag.Description = _systemSettingBusiness.GetDetailByKey("Description");
                ViewBag.Keywords = _systemSettingBusiness.GetDetailByKey("Keywords");
                ViewBag.ShareTitle = _systemSettingBusiness.GetDetailByKey("ShareTitle");
                ViewBag.ShareKeyword = _systemSettingBusiness.GetDetailByKey("ShareKeyword");
                ViewBag.ShareDescription = _systemSettingBusiness.GetDetailByKey("ShareDescription");
                ViewBag.published_time = _systemSettingBusiness.GetDetailByKey("published_time");
                ViewBag.updated_time = _systemSettingBusiness.GetDetailByKey("updated_time");
                ViewBag.image = _systemSettingBusiness.GetDetailByKey("image");
                ViewBag.author = _systemSettingBusiness.GetDetailByKey("author");
            }
            catch (Exception ex) { }
            ViewData["JSShermahome"] = JSShermahome();

            return View();
        }

        [CompactHtmlFilter]
        public ActionResult Loi404()
        {
            return View();
        }

        [CompactHtmlFilter]
        public ActionResult Contact()
        {
            return View();
        }

        [CompactHtmlFilter]
        public ActionResult SendMail()
        {
            return View();
        }

        [CompactHtmlFilter]
        public ActionResult Login()
        {
            return View();
        }

        [CompactHtmlFilter]
        public ActionResult Category(string friendUrl, long id, string mid, string price, string order, int? page, string k)
        {
            //try

            {
                Catalog catalog = new Catalog();
                long ids = 0;
                string tableNameCatalogs = "Catalogs";

                string keyCatalogs = "keyCatalogs" + id.ToString();
                if (id != -2)
                {
                    if (HttpContext.Cache[keyCatalogs] == null)
                    {
                        SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, tableNameCatalogs);

                        AggregateCacheDependency aggDep = new AggregateCacheDependency();
                        aggDep.Add(dep1);
                        catalog = new CatalogsBusiness().GetById(id);
                        HttpContext.Cache.Insert(keyCatalogs, catalog, aggDep);
                    }
                    else
                    {
                        catalog = (Catalog)HttpContext.Cache[keyCatalogs];
                    }
                    ids = catalog.Id;
                    ViewData["id"] = ids;
                }
                else
                {
                    catalog.Id = -2;
                    catalog.ParentId = -1;
                    catalog.FriendlyUrl = "tim-kiem";
                    catalog.CatalogName = "Tìm kiếm";
                    catalog.SeoTitle = "Tìm kiếm: " + k;
                    catalog.SeoDescription = "Tìm kiếm: " + k;
                    catalog.SeoKeyword = "Tìm kiếm: " + k;
                }
                ViewData["CatalogName"] = catalog.CatalogName;
                ViewData["TotalVote"] = catalog.TotalVote;
                ViewData["VoteScore"] = catalog.VoteScore;
                ViewBag.Mota = catalog.Description;
                ViewData["Title"] = catalog.SeoTitle;
                ViewBag.Description = catalog.SeoDescription;

                ViewBag.Keywords = catalog.SeoKeyword;

                ViewBag.ShareTitle = catalog.ShareTitle;
                ViewBag.ShareKeyword = catalog.ShareKeyword;
                ViewBag.ShareDescription = catalog.ShareDescription;
                ViewBag.published_time = catalog.CreateDate;
                ViewBag.updated_time = catalog.ModifyDate;
                ViewBag.image = catalog.ImageSource;

                InitBreadcrumbs(catalog);
                ViewData["ListCatalog"] = ListCatalogByParentID(id);
                if (ids == null) ids = 0;

                if (page == null) page = 1;
                if (order == null) order = "new";
                double? priceForm = 0, priceTo = 0;
                double temFrom = 100000000000;
                try
                {
                    string[] pricelist = price.Split(',');
                    foreach (string a in pricelist)
                    {
                        if (double.Parse(a.Split(':')[0]) == 0)
                        {
                            temFrom = 0;
                        }
                        else
                        {
                            if (double.Parse(a.Split(':')[0]) < temFrom)
                                temFrom = double.Parse(a.Split(':')[0]);
                        }

                        if (double.Parse(a.Split(':')[1]) > priceTo && double.Parse(a.Split(':')[1]) != 0)
                            priceTo = double.Parse(a.Split(':')[1]);
                    }
                }
                catch { }
                priceForm = temFrom;

                NameValueCollection n = Request.QueryString;
                string urlParam = string.Empty;
                string urlParam1 = string.Empty;
                if (n.HasKeys())
                {
                    for (int i = 0; i < n.Count; i++)
                    {
                        string k1 = n.GetKey(i);
                        string v = n.Get(i);

                        if (!string.IsNullOrEmpty(k1))
                        {
                            if (k1 != "id" && k1 != "mid" && k1 != "page" && k1 != "price" && k1 != "order" && k1 != "k")
                            {
                                //urlParam += string.Format("	AND (ProductProperties.[Key]='{0}' AND ProductProperties.ValueUrl IN ('{1}')) ", k1, v.Replace(",","','"));

                                urlParam += " AND Products.Id IN (SELECT ProductId FROM ProductProperties WHERE  [Key]='" + k1 + "' and  ValueUrl IN ('" + v.Replace(",", "','") + "'))";
                                urlParam1 += string.Format("{0}={1}&", k1, v);
                            }
                        }
                    }
                }

                string strQuery = BuildQuery(ids, mid, price, order, page, urlParam, k);

                string tableName1 = "Products";
                string tableName2 = "Catalogs";
                string tableName3 = "ProductProperties";
                string tableName4 = "ProductImagesPrice";
                string tableName5 = "ProductImages";
                string tableName6 = "ProductAttributes";
                string tableName7 = "CatalogPropertiesValue";
                string tableName8 = "CatalogProperties";
                string keyListProduct = "keyListProduct" + friendUrl + id.ToString() + mid + price + order + page.ToString() + k + Request.QueryString;
                List<SearchProductByType_Result> ListProduct;
                BuyGroup365Entities entitis = new BuyGroup365Entities();
                if (HttpContext.Cache[keyListProduct] == null)
                {
                    SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, tableName1);
                    SqlCacheDependency dep2 = new SqlCacheDependency(Database_Name, tableName2);
                    SqlCacheDependency dep3 = new SqlCacheDependency(Database_Name, tableName3);
                    SqlCacheDependency dep4 = new SqlCacheDependency(Database_Name, tableName4);
                    SqlCacheDependency dep5 = new SqlCacheDependency(Database_Name, tableName5);
                    SqlCacheDependency dep6 = new SqlCacheDependency(Database_Name, tableName6);
                    SqlCacheDependency dep7 = new SqlCacheDependency(Database_Name, tableName7);
                    SqlCacheDependency dep8 = new SqlCacheDependency(Database_Name, tableName8);
                    AggregateCacheDependency aggDep = new AggregateCacheDependency();
                    aggDep.Add(dep1, dep2, dep3, dep4, dep5, dep6, dep7, dep8);
                    ListProduct = entitis.Database.SqlQuery<SearchProductByType_Result>(strQuery).ToList();
                    HttpContext.Cache.Insert(keyListProduct, ListProduct, aggDep);
                }
                else
                {
                    ListProduct = (List<SearchProductByType_Result>)HttpContext.Cache[keyListProduct];
                }

                if (!string.IsNullOrEmpty(urlParam1))

                    urlParam1 = urlParam1.Substring(0, urlParam1.Length - 1);

                ViewData["Info_Manu"] = string.Format("{0};{1};{2};{3};{4};{5}", ids, mid, price, catalog.FriendlyUrl, urlParam1, k);

                ViewData["ListOrder"] = InitDropdownlistSort((long)ids, catalog.FriendlyUrl, mid, (double)priceForm, (double)priceTo, urlParam1);

                strQuery = BuildQueryCount(ids, mid, price, urlParam, k);

                int count;
                string keyListProductCount = "keyListProductCount" + friendUrl + id.ToString() + mid + price + k + Request.QueryString;
                if (HttpContext.Cache[keyListProductCount] == null)
                {
                    SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, tableName1);
                    SqlCacheDependency dep2 = new SqlCacheDependency(Database_Name, tableName2);
                    SqlCacheDependency dep3 = new SqlCacheDependency(Database_Name, tableName3);
                    SqlCacheDependency dep4 = new SqlCacheDependency(Database_Name, tableName4);
                    SqlCacheDependency dep5 = new SqlCacheDependency(Database_Name, tableName5);
                    SqlCacheDependency dep6 = new SqlCacheDependency(Database_Name, tableName6);
                    SqlCacheDependency dep7 = new SqlCacheDependency(Database_Name, tableName7);
                    SqlCacheDependency dep8 = new SqlCacheDependency(Database_Name, tableName8);
                    AggregateCacheDependency aggDep = new AggregateCacheDependency();
                    aggDep.Add(dep1, dep2, dep3, dep4, dep5, dep6, dep7, dep8);
                    count = entitis.Database.SqlQuery<int>(strQuery).Single(); ;
                    HttpContext.Cache.Insert(keyListProductCount, count, aggDep);
                }
                else
                {
                    count = (int)HttpContext.Cache[keyListProductCount];
                }

                Paging(count, Page_Block, Page_Size, page, (int)page, Request.RawUrl);

                return View(ListProduct);
            }
            // catch
            {
                //    return Redirect("/");
            }
        }

        [CompactHtmlFilter]
        public ActionResult sanphammoi(int page = 1, int pageSize = 12)
        {
            int totalRecord = 0;
            int totalPage = 0;

            string table1 = "Products";
            List<Common.Product> listProducts;
            string key = "Khuyenmai" + page.ToString() + Page_Size.ToString();
            Business.ClassBusiness.ProductsBusiness productsBusiness = new Business.ClassBusiness.ProductsBusiness();
            if (HttpContext.Cache[key] == null)
            {
                SqlCacheDependency dep1 = new SqlCacheDependency(Database_Name, table1);
                AggregateCacheDependency aggDep = new AggregateCacheDependency();
                aggDep.Add(dep1);
                listProducts = productsBusiness.ListByTypeProduct(2, ref totalRecord, page, pageSize);
                HttpContext.Cache.Insert(key, listProducts, aggDep);
            }
            else
            {
                listProducts = (List<Common.Product>)HttpContext.Cache[key];
            }
            productsBusiness.ListByTypeProduct(1, ref totalRecord, page, 100000);
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

            return View(listProducts);
        }

        public JsonResult ListProduct(string mid, string price, string order, int? page, string k)
        {
            //try
            {
                string friendlyUrl;
                Catalog catalog = new Catalog();
                long ids = 0;
                try
                {
                    friendlyUrl = Request.Url.AbsolutePath.Split('/')[3];

                    catalog = new CatalogsBusiness().SearchCategoryByFriendUrl(friendlyUrl);
                    ids = catalog.Id;
                    ViewData["id"] = ids;
                }
                catch
                {
                    catalog.Id = -1;
                    catalog.ParentId = -1;
                    catalog.FriendlyUrl = "tim-kiem";
                    catalog.CatalogName = "Tìm kiếm";
                    catalog.SeoTitle = "Tìm kiếm: " + k;
                    catalog.SeoDescription = "Tìm kiếm: " + k;
                    catalog.SeoKeyword = "Tìm kiếm: " + k;
                }
                ViewData["CatalogName"] = catalog.CatalogName;
                ViewData["TotalVote"] = catalog.TotalVote;
                ViewData["VoteScore"] = catalog.VoteScore;

                ViewData["Title"] = catalog.SeoTitle;
                ViewBag.Description = catalog.SeoDescription;
                ViewBag.Description_ = catalog.Description;
                ViewBag.Keywords = catalog.SeoKeyword;

                InitBreadcrumbs(catalog);

                if (ids == null) ids = 0;

                if (page == null) page = 1;
                if (order == null) order = "new";
                double? priceForm = 0, priceTo = 0;
                double temFrom = 100000000000;
                try
                {
                    string[] pricelist = price.Split(',');
                    foreach (string a in pricelist)
                    {
                        if (double.Parse(a.Split(':')[0]) == 0)
                        {
                            temFrom = 0;
                        }
                        else
                        {
                            if (double.Parse(a.Split(':')[0]) < temFrom)
                                temFrom = double.Parse(a.Split(':')[0]);
                        }

                        if (double.Parse(a.Split(':')[1]) > priceTo && double.Parse(a.Split(':')[1]) != 0)
                            priceTo = double.Parse(a.Split(':')[1]);
                    }
                }
                catch { }
                priceForm = temFrom;

                NameValueCollection n = Request.QueryString;
                string urlParam = string.Empty;
                string urlParam1 = string.Empty;
                if (n.HasKeys())
                {
                    for (int i = 0; i < n.Count; i++)
                    {
                        string k1 = n.GetKey(i);
                        string v = n.Get(i);

                        if (!string.IsNullOrEmpty(k1))
                        {
                            if (k1 != "id" && k1 != "mid" && k1 != "page" && k1 != "price" && k1 != "order" && k1 != "k")
                            {
                                //urlParam += string.Format("	AND (ProductProperties.[Key]='{0}' AND ProductProperties.ValueUrl IN ('{1}')) ", k1, v.Replace(",","','"));

                                urlParam += " AND Products.Id IN (SELECT ProductId FROM ProductProperties WHERE  [Key]='" + k1 + "' and  ValueUrl IN ('" + v.Replace(",", "','") + "'))";
                                urlParam1 += string.Format("{0}={1}&", k1, v);
                            }
                        }
                    }
                }

                string strQuery = BuildQuery(ids, mid, price, order, page, urlParam, k);

                BuyGroup365Entities entitis = new BuyGroup365Entities();
                var result = entitis.Database.SqlQuery<SearchProductByType_Result>(strQuery).ToList();

                if (!string.IsNullOrEmpty(urlParam1))

                    urlParam1 = urlParam1.Substring(0, urlParam1.Length - 1);

                if (string.IsNullOrEmpty(price)) price = "0:0";

                ViewData["Info_Manu"] = string.Format("{0};{1};{2};{3};{4};{5}", ids, mid, price, catalog.FriendlyUrl, urlParam1, k);

                ViewData["ListOrder"] = InitDropdownlistSort((long)ids, catalog.FriendlyUrl, mid, (double)priceForm, (double)priceTo, urlParam1);

                strQuery = BuildQueryCount(ids, mid, price, urlParam, k);

                int count = entitis.Database.SqlQuery<int>(strQuery).Single();

                Paging(count, Page_Block, Page_Size, page, (int)page, Common.util.Function.InitUrl(catalog.FriendlyUrl, (long)ids, mid, (double)priceForm, (double)priceTo, urlParam));

                return Json(BindData(result));
            }
            // catch
            {
                //    return Redirect("/");
            }
        }

        protected string BindData(List<SearchProductByType_Result> ListProduct)
        {
            string htmlProduct = "";

            foreach (SearchProductByType_Result re in ListProduct)
            {
                int phantramKM = 0;
                try
                {
                    phantramKM = Convert.ToInt32((re.Cost - re.Price) / re.Cost * 100);
                }
                catch { }
                htmlProduct += "<div class=\"sp fL\"><div class=\"pro12\"><div class=\"image_sp\">";
                htmlProduct += "<a title=\"" + re.ProductName + "\" href=\"" + re.FriendlyUrl + "\">";
                htmlProduct += "<img src=\"" + re.ImgSource.Split('?')[0] + "medium/" + re.ImgSource.Split('?')[1] + "\" alt=\"" + re.ProductName + "\"> </a> </div>";
                if (phantramKM > 0)
                    htmlProduct += "  <div class=\"percent\"><span>" + phantramKM + "</span></div>";
                if (re.IsVip)
                    htmlProduct += "<span class=\"product-gift\"><i class=\"fa fa-gift\"></i> Quà tặng </span>";
                htmlProduct += "<div class=\"name_sp\"> <a title=\"" + re.ProductName + "\" href=\"" + re.FriendlyUrl + "\"><h3>" + re.ProductName + "</h3> </a></div>";
                htmlProduct += "<div class=\"price_tt\">Giá niêm yết:<span>" + String.Format("{0:#,###}", re.Cost) + "</span></div> <div class=\"price_sp\"><span class=\"red\"> <strong>";
                htmlProduct += String.Format("{0:#,###}", re.Price / 1000) + " </strong> </span><small class=\"nexttop2\">Giá KM</small> <small class=\"nextbottom2\">.000</small></div></div></div>";
            }

            return htmlProduct;
        }

        public JsonResult LoadPageAjax(string mid, string price, string order, int page, string k)
        {
            string url;
            int numberPage = 0;
            NameValueCollection n = Request.QueryString;
            string urlParam = string.Empty;
            string urlParam1 = string.Empty;
            if (n.HasKeys())
            {
                for (int i = 0; i < n.Count; i++)
                {
                    string k1 = n.GetKey(i);
                    string v = n.Get(i);

                    if (!string.IsNullOrEmpty(k1))
                    {
                        if (k1 != "id" && k1 != "mid" && k1 != "page" && k1 != "price" && k1 != "order" && k1 != "k")
                        {
                            //urlParam += string.Format("	AND (ProductProperties.[Key]='{0}' AND ProductProperties.ValueUrl IN ('{1}')) ", k1, v.Replace(",","','"));

                            urlParam += " AND Products.Id IN (SELECT ProductId FROM ProductProperties WHERE  [Key]='" + k1 + "' and  ValueUrl IN ('" + v.Replace(",", "','") + "'))";
                            urlParam1 += string.Format("{0}={1}&", k1, v);
                        }
                    }
                }
            }
            Catalog catalog = new Catalog();
            long ids = 0;
            try
            {
                url = Request.Url.AbsolutePath.Split('/')[3];

                catalog = new CatalogsBusiness().SearchCategoryByFriendUrl(url);
                ids = catalog.Id;
            }
            catch { }

            string strQuery = BuildQueryCount(ids, mid, price, urlParam, k);
            BuyGroup365Entities entitis = new BuyGroup365Entities();
            int count = entitis.Database.SqlQuery<int>(strQuery).Single();
            //Tạo mảng các trang: 1 2 3 ...
            int[] arayPage = Common.util.Function.Paging(count, page, Page_Block, Page_Size, ref page, ref numberPage);
            // stt = (page - 1) * SIZE_OF_PAGE + 1;
            string html = "";
            url = Request.Url.AbsoluteUri;
            try
            {
                if (url.Contains("?")) url += "&page=";
                else
                    url += "?page=";
            }
            catch { }
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
                        html += string.Format("<li><a href=\"{0}\" onclick=\"filter({1})\"></a></li>", url + i, i);
                    else
                        html += string.Format("<li class=\"current\">{0}</li>", i);
                }
            }

            if (page < numberPage)
            {
                html += "<li class=\"bor-none\"><a class=\"previous i-next\" onclick=\"filter(" + page + 1 + ")\" href=\"" + url + (page + 1) + "\" title=\"Trang sau\">Tiếp <i class=\"fa fa-angle-double-right\">&nbsp;</i></a></li>";
            }

            return Json(string.Format("<div class=\"pager\"><div class=\"pages\"><strong>Page:</strong><ol>{0}</ol></div></div>", html));
        }

        public JsonResult ListName(string term)
        {
            var data = new ProductsBusiness().SearchProductName(term);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected string BuildQuery(long? id, string mid, string price, string order, int? page, string param, string k)
        {
            string query = "", buildQuery = "";
            if (mid == "" || mid == null) mid = null;
            else
            {
                mid = "AND ManufacturerId in (" + mid + ")";
            }
            if (price != "" && price != null)
            {
                string[] pricelist = price.Split(',');
                string PriceTemp = "AND (";
                int i = 0;
                foreach (string a in pricelist)
                {
                    if (i == 0)
                        PriceTemp += "((" + a.Split(':')[0] + " <=Price)  AND (" + a.Split(':')[1] + " >=Price)) ";
                    else
                        PriceTemp += "OR ((" + a.Split(':')[0] + " <=Price)  AND (" + a.Split(':')[1] + " >=Price)) ";
                    i++;
                }
                PriceTemp += ")";
                price = PriceTemp;
            }
            if (id != 0)
            {
                query = @"BEGIN
				            WITH temp(id)
			                as (

					                Select Id
					                From Catalogs
					                Where Id ={0}
					                Union All
					                Select b.Id
					                From temp as a, Catalogs as b
					                Where a.Id = b.ParentId

			                )
			                 SELECT distinct *
				                FROM(
				                select *
				                , ROW_NUMBER() OVER(ORDER BY  Id ) RN
								FROM (
								SELECT  distinct Products.Id, ProductName,Summary, FriendlyUrl, [dbo].[FindAvatar](Products.Id) AS ImgSource, ColWidth, Code,Price, Cost, IsNew,IsProductBig,Tags, Model,ManufacturerId, IsAttractive, IsVip,LongImages,IsHome, TotalView, [dbo].[CaculatorRate](Products.Id) AS Rate,ModifyDate,LandingPage

				                       from temp join CatalogProducts ON temp.Id=CatalogProducts.CatalogId join  Products ON CatalogProducts.ProductId = Products.Id
				                           LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                WHERE (Status = 2 OR Status=3 OR Status=4)
				             {1}
                            {2}
                                AND (ProductName Like N'%{8}%' OR Tags Like N'%{8}%'  OR Code Like N'%{8}%')
				              {3} )b
			                ) a
				                WHERE RN BETWEEN ({4}-1)*{7}+1 AND {4}*{7}
				                ORDER BY {6}
		                END";
                if (order.Equals("new"))//Mới nhất
                    buildQuery = string.Format(query, id, mid, price, param, page, "Products.Id desc", "Id desc", Page_Size, k);
                else if (order.Equals("name"))
                    buildQuery = string.Format(query, id, mid, price, param, page, "ProductName desc", "ProductName desc", Page_Size, k);
                else if (order.Equals("view"))
                    buildQuery = string.Format(query, id, mid, price, param, page, "TotalView desc", "TotalView desc", Page_Size, k);
                else if (order.Equals("price_asc"))
                    buildQuery = string.Format(query, id, mid, price, param, page, "Price asc", "Price asc", Page_Size, k);
                else if (order.Equals("price_desc"))
                    buildQuery = string.Format(query, id, mid, price, param, page, "Price desc", "Price desc", Page_Size, k);
            }
            else
            {
                query = @"BEGIN

			                 SELECT *
				                FROM(
				                select *
				                , ROW_NUMBER() OVER(ORDER BY  {6} ) RN
								FROM (
								SELECT  distinct Products.Id, ProductName,Summary, FriendlyUrl, [dbo].[FindAvatar](Products.Id) AS ImgSource, ColWidth, Code,Price, Cost, IsNew,ManufacturerId,IsProductBig,Tags, Model, IsAttractive, IsVip,LongImages,IsHome, TotalView, [dbo].[CaculatorRate](Products.Id) AS Rate,ModifyDate,LandingPage

				                from  Products  LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                WHERE (Status = 2 OR Status=3 OR Status=4)
				              {1}
				              {2}
                                AND (ProductName Like N'%{8}%' OR Tags Like N'%{8}%' OR Code Like N'%{8}%')
				               {3})b
			                  ) a
				                WHERE RN BETWEEN ({4}-1)*{7}+1 AND {4}*{7}
				                ORDER BY {6} {0}
		                END";
                if (order.Equals("new"))//Mới nhất
                    buildQuery = string.Format(query, "", mid, price, param, page, "Products.Id desc", "Id desc", Page_Size, k);
                else if (order.Equals("name"))
                    buildQuery = string.Format(query, "", mid, price, param, page, "ProductName desc", "ProductName desc", Page_Size, k);
                else if (order.Equals("view"))
                    buildQuery = string.Format(query, "", mid, price, param, page, "TotalView desc", "TotalView desc", Page_Size, k);
                else if (order.Equals("price_asc"))
                    buildQuery = string.Format(query, "", mid, price, param, page, "Price asc", "Price asc", Page_Size, k);
                else if (order.Equals("price_desc"))
                    buildQuery = string.Format(query, "", mid, price, param, page, "Price desc", "Price desc", Page_Size, k);
            }

            return buildQuery;
        }

        protected string BuildQueryCount(long? id, string mid, string price, string param, string k)
        {
            string query = "", buildQuery = "";
            if (mid == "" || mid == null) mid = null;
            else
            {
                mid = "AND ManufacturerId in (" + mid + ")";
            }
            if (price != "" && price != null)
            {
                string[] pricelist = price.Split(',');
                string PriceTemp = "AND (";
                int i = 0;
                foreach (string a in pricelist)
                {
                    if (i == 0)
                        PriceTemp += "((" + a.Split(':')[0] + " <=Price)  AND (" + a.Split(':')[1] + " >=Price)) ";
                    else
                        PriceTemp += "OR ((" + a.Split(':')[0] + " <=Price)  AND (" + a.Split(':')[1] + " >=Price)) ";
                    i++;
                }
                PriceTemp += ")";
                price = PriceTemp;
            }
            if (id != 0)
            {
                query = @"
BEGIN
				            WITH temp(id)
			                as (

					                Select Id
					                From Catalogs
					                Where Id ={0}
					                Union All
					                Select b.Id
					                From temp as a, Catalogs as b
					                Where a.Id = b.ParentId

			                )

								SELECT   COUNT(DISTINCT  Products.Id) AS Total

				                     from temp join CatalogProducts ON temp.Id=CatalogProducts.CatalogId join  Products ON CatalogProducts.ProductId = Products.Id
				                           LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                WHERE (Status = 2 OR Status=3 OR Status=4)
				               {1}
				              {2}
                                AND (ProductName Like '%{4}%' OR Tags Like '%{4}%' OR Code Like '%{4}%')
				               {3}

		                END
";

                buildQuery = string.Format(query, id, mid, price, param, k);
            }
            else
            {
                query = @"BEGIN

				                select  COUNT(DISTINCT  Products.Id) AS Total

				                 from  Products
				                           LEFT JOIN ProductProperties ON Products.Id=ProductProperties.ProductId
				                 WHERE (Status = 2 OR Status=3 OR Status=4)
				               {1}
				              {2}
                                AND (ProductName Like '%{4}%' OR Tags Like '%{4}%' OR Code Like '%{4}%')
				               {3} {0}

		                END";

                buildQuery = string.Format(query, "", mid, price, param, k);
            }

            return buildQuery;
        }

        public string InitDropdownlistSort(long id, string catalogUrl, string mid, double priceFrom, double priceTo, string param)
        {
            string url = Request.RawUrl;

            string html = "<div class=\"orderby prevent\" style=\"\"><div class=\"ordertype\"><span>Xếp theo:</span>";
            string selected = "new";
            if (Request.QueryString["order"] != null)
            {
                selected = Request.QueryString["order"];

                if (selected.Equals("new"))
                {
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "price_desc") + "\" data-text=\"Giá cao đến thấp\" class=\"\"> <i class=\"iconcate-radio\"></i>Giá cao đến thấp </a>";
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "price_asc") + "\" data-text=\"Giá thấp đến cao\" class=\"\"> <i class=\"iconcate-radio\"></i>Giá thấp đến cao </a>";
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "name") + "\" data-text=\"Theo tên\" class=\"\"> <i class=\"iconcate-radio\"></i>Theo tên </a>";
                }

                if (selected.Equals("price_asc"))
                {
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "price_desc") + "\" data-text=\"Giá cao đến thấp\" class=\"\"> <i class=\"iconcate-radio\"></i>Giá cao đến thấp </a>";
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "price_asc") + "\" data-text=\"Giá thấp đến cao\" class=\"check\"> <i class=\"iconcate-radio\"></i>Giá thấp đến cao </a>";
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "name") + "\" data-text=\"Theo tên\" class=\"\"> <i class=\"iconcate-radio\"></i>Theo tên </a>";
                }
                if (selected.Equals("price_desc"))
                {
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "price_desc") + "\" data-text=\"Giá cao đến thấp\" class=\"check\"> <i class=\"iconcate-radio\"></i>Giá cao đến thấp </a>";
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "price_asc") + "\" data-text=\"Giá thấp đến cao\" class=\"\"> <i class=\"iconcate-radio\"></i>Giá thấp đến cao </a>";
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "name") + "\" data-text=\"Theo tên\" class=\"\"> <i class=\"iconcate-radio\"></i>Theo tên </a>";
                }
                if (selected.Equals("name"))
                {
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "price_desc") + "\"  data-text=\"Giá cao đến thấp\" class=\"\"> <i class=\"iconcate-radio\"></i>Giá cao đến thấp </a>";
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "price_asc") + "\"  data-text=\"Giá thấp đến cao\" class=\"\"> <i class=\"iconcate-radio\"></i>Giá thấp đến cao </a>";
                    html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, selected, "name") + "\"  data-text=\"Theo tên\" class=\"check\"> <i class=\"iconcate-radio\"></i>Theo tên </a>";
                }
            }
            else
            {
                html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, "", "price_desc") + "\" data-text=\"Giá cao đến thấp\" class=\"\"> <i class=\"iconcate-radio\"></i>Giá cao đến thấp </a>";
                html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, "", "price_asc") + "\" data-text=\"Giá thấp đến cao\" class=\"\"> <i class=\"iconcate-radio\"></i>Giá thấp đến cao </a>";
                html += "<a href=\"" + Common.util.Function.InitUrlOrder(url, "", "name") + "\" data-text=\"Theo tên\" class=\"\"> <i class=\"iconcate-radio\"></i>Theo tên </a>";
            }

            return html + "</div></div>";
        }

        protected void InitBreadcrumbs(Catalog catalog)
        {
            string html = string.Format("<li><a href=\"" + Util.ReturnLinkFull(catalog.FriendlyUrl) + "\" title=\"\" itemprop=\"url\"><strong itemprop=\"title\"> {0}</strong></a></li>", catalog.CatalogName);
            CatalogsBusiness catalogBusiness = new CatalogsBusiness();
            try
            {
                while (catalog.ParentId != -1)
                {
                    catalog = catalogBusiness.GetById(catalog.ParentId);
                    html = string.Format("<li><a href=\"{1}\" title=\"\" itemprop=\"url\" class=\"active\"><strong itemprop=\"title\">{0}</strong></a></li>", catalog.CatalogName, Util.ReturnLinkFull(catalog.FriendlyUrl)) + html;
                }
            }
            catch { }
            ViewData["Breadcrumbs"] = html;
        }

        public string ListCatalogByParentID(long id)
        {
            string ListHtml = "";
            try
            {
                CatalogsBusiness catalogBusiness = new CatalogsBusiness();
                List<Catalog> listCatalog = new List<Catalog>();
                listCatalog = catalogBusiness.SearchCatalogByParentId(id);
                if (listCatalog.Count > 0)
                {
                    ListHtml = " <ul class=\"cate-ul\">";
                    foreach (Catalog tem in listCatalog)
                    {
                        ListHtml += "<li><a href=\"" + Util.ReturnLinkFull(tem.FriendlyUrl) + "\">" + tem.CatalogName + "</a></li>";
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

        public ActionResult Search()
        {
            return View();
        }

        #region PAGING

        protected void Paging(int recordNumber, int SIZE_PAGE, int SIZE_OF_PAGE, object obj, int page, string url)
        {
            int numberPage = 0;
            //Tạo mảng các trang: 1 2 3 ...
            int[] arayPage = Common.util.Function.Paging(recordNumber, obj, SIZE_PAGE, SIZE_OF_PAGE, ref page, ref numberPage);
            // stt = (page - 1) * SIZE_OF_PAGE + 1;
            string html = "";

            if (page > 1)
            {
                html += "  <li class=\"page-item disabled\"><a class=\"page-link\" href=\""+Common.util.Function.InitUrlPage(url, page.ToString(), (page - 1).ToString())+"\">«</a></li>";
            }
            //int index = 0;
            foreach (int i in arayPage)
            {
                if (i > 0)
                {
                    if (!i.Equals(page))
                        html += string.Format("<li class=\"page-item\"><a class=\"page-link\" href=\"{0}\">{1}</a></li>", Common.util.Function.InitUrlPage(url, page.ToString(), i.ToString()), i);
                    else
                        html += string.Format("<li class=\"active page-item disabled\">{0}</li>", i);
                }
            }

            if (page < numberPage)
            {
                html += "<li class=\"page-item\"><a class=\"page-link\" href=\"" + Common.util.Function.InitUrlPage(url, page.ToString(), (page + 1).ToString()) + "\">»</a></li>";
            }

            ViewData["Paging"] = string.Format("<ul class=\"pagintn pt-10\">{0}</ul>", html);
        }

        #endregion PAGING

        #region PRODUCT DETAIL

        [CompactHtmlFilter]
        [OutputCache(CacheProfile = "cacheProductDetail")]
        public ActionResult ProductDetail(long id)
        {
             AddProductViews(id);
            double totalStar;
            int totalVote = 0;
            string star = "";
            TextHtmlBusiness textHtmlBusiness = new TextHtmlBusiness();
            CommentsBusiness commentBussiness = new CommentsBusiness();
          
            ProductsBusiness productBusiness = new ProductsBusiness();
            Product item = productBusiness.GetById(id);
            ViewBag.ProductID = id;

            //item.TotalView = item.TotalView + 1;
            //productBusiness.Edit(item);
            ViewData["ProductName"] = item.ProductName;
            ViewData["ProductNameShort"] = item.ProductNameShort;
            ViewData["PromotionProductDetail"] = HomeFunction.InitPromotion(item);
            totalStar = commentBussiness.CalculatorByProductId(id, ref totalVote);

            for (int i = 1; i <= totalStar; i++)
            {
                star += "<li class='star selected hover' title='Poor' data-value='" + i.ToString() + "'>  <i class='fa fa-star fa-fw'></i></li>";
            }
            for (int i = 1; i <= 5 - totalStar; i++)
            {
                star += "  <li class='star ' title='WOW!!!' data-value='" + (i + totalStar).ToString() + "'> <i class='fa fa-star fa-fw'></i></li>";
            }

            ViewData["TotalStar"] = star;
            ViewData["Khuyenmai1"] = InitKhuyenmai(id, 1);
            ViewData["Thongtinthem"] = InitKhuyenmai(id, 2);
            ViewData["TotalVote"] = totalVote;

            ViewBag.ID = item.Id;
            ViewBag.PID = item.Id;
            ViewBag.NewgroupS = item.NewGroupID;
            try
            {
                InitBreadcrumbs(item.CatalogProducts.FirstOrDefault().CatalogId);
            }
            catch { }

            #region METATAGS

            if (string.IsNullOrEmpty(item.SeoTitle))

                ViewBag.Title = item.ProductName;
            else
                ViewBag.Title = item.SeoTitle;

            ViewBag.Description = item.SeoDescription;
            ViewBag.Keywords = item.SeoKeyword;

            ViewBag.ShareTitle = item.ShareTitle;
            ViewBag.ShareKeyword = item.ShareKeyword;
            ViewBag.ShareDescription = item.ShareDescription;
            ViewBag.published_time = item.CreateDate;
            ViewBag.updated_time = item.ModifyDate;
            // ViewBag.author = item.Author;

            try
            {
                ViewBag.image = item.ProductImages.First().ImgSource;
            }
            catch { }

            #endregion METATAGS

            CommentsBusiness commentBusiness = new CommentsBusiness();
            int count = 0;
            double rate = 0;
            rate = commentBusiness.CalculatorByProductId(id, ref count);
            ViewData["rating"] = InitStar(rate);
            ViewData["ratingso"] = rate;
            @ViewData["count"] = count;

            #region PRICE_STATUS

            if (item.Cost > item.Price)
                ViewData["OldPrice"] = item.Cost.ToString("N0").Replace(',', '.');
            if (item.Cost == 0)
                ViewData["OldPrice"] = "";
            if (item.Price == 0)
                ViewData["Price"] = "Liên hệ";
            else
                ViewData["Price"] = item.Price.ToString("N0").Replace(',', '.');

            if (item.State == 1) ViewData["State"] = "Còn hàng";
            else ViewData["State"] = "Hết hàng";

            #endregion PRICE_STATUS

            #region ABOUT NEWS

            try
            {
                if (item.NewAbout != null || item.NewAbout != "")
                {
                    string[] aboutnew = item.NewAbout.Split(';');
                    string htmlAboutNew = "<div class=\"title-cate\"><h4 class=\"text\">Tin tức về " + item.ProductNameShort + "</h4></div><div class=\"cate-list\">";

                    foreach (string re in aboutnew)
                    {
                        try
                        {
                            news = newsBusiness.GetById(long.Parse(re));
                            htmlAboutNew += "<div class=\"per-news\"><div class=\"img\">" +
                                   "<a href=\"" + Util.ReturnLinkFull(news.FriendlyUrl) + "\" >" +
                           "<img data-src=\"" + Util.ReturnLinkCDN(news.ImageSource, Request.Browser.IsMobileDevice) + "\" class=\"lazyload\" alt=\"" + news.Title + "\"></a></div><div class=\"text\">" +
                           "<a href=" + Util.ReturnLinkFull(news.FriendlyUrl) + ">" +
                                        "<h3 class=\"title\">" + news.Title + "</h3></a></div></div> ";
                        }
                        catch { }
                    }
                    htmlAboutNew += "</div>";
                    ViewData["AboutNews"] = htmlAboutNew;
                }
            }
            catch
            {
            }

            #endregion ABOUT NEWS

            #region INTRO NEWS

            try
            {
                if (item.IntroProduct != null || item.IntroProduct != "")
                {
                    string[] introProduct = item.IntroProduct.Split(';');
                    string htmlIntroNew = "<div class=\"title-cate\"><h4 class=\"text\">Hướng dẫn sử dụng</h4></div><div class=\"cate-list\">";

                    foreach (string re in introProduct)
                    {
                        try
                        {
                            news = newsBusiness.GetById(long.Parse(re));
                            htmlIntroNew += "<div class=\"per-news\"><div class=\"img\">" +
                                   "<a href=\"" + Util.ReturnLinkFull(news.FriendlyUrl) + "\" >" +
                           "<img data-src=\"" + Util.ReturnLinkCDN(news.ImageSource, Request.Browser.IsMobileDevice) + "\" class=\"lazyload\" alt=\"" + news.Title + "\"></a></div><div class=\"text\">" +
                           "<a href=" + Util.ReturnLinkFull(news.FriendlyUrl) + ">" +
                                        "<h3 class=\"title\">" + news.Title + "</h3></a></div></div> ";
                        }
                        catch { }
                    }
                    htmlIntroNew += "</div>";
                    ViewData["IntroNews"] = htmlIntroNew;
                }
            }
            catch
            {
            }

            #endregion INTRO NEWS

            #region RELATIVE PRODUCT

            try
            {
                if (item.ProductRelative != null || item.ProductRelative != "")
                {
                    string[] introProduct = item.ProductRelative.Split(';');
                    string htmlRelativePro = "<div class=\"title-cate\"><h4 class=\"text\">Tin tức về " + item.ProductNameShort + "</h4></div><div class=\"product-list\">";

                    foreach (string re in introProduct)
                    {
                        try
                        {
                            Product _product = productBusiness.GetById(int.Parse(re));
                            int phantramKM = 0;
                            try
                            {
                                phantramKM = Convert.ToInt32((_product.Cost - _product.Price) / _product.Cost * 100);
                            }
                            catch { }
                            htmlRelativePro += "<div class=\"per-news\"><div class=\"img\">" +
                                   "<a href=\"" + Util.ReturnLinkFull(_product.FriendlyUrl) + "\" >" +
                           "<img data-src=\"" + Util.ReturnLinkCDN(_product.ProductImages.FirstOrDefault().ImgSource, Request.Browser.IsMobileDevice) + "\" class=\"lazyload\" alt=\"" + _product.ProductName + "\"></a></div><div class=\"text\">" +
                           "<a href=" + Util.ReturnLinkFull(_product.FriendlyUrl) + ">" +
                                        "<h5 class=\"title\">" + _product.ProductName + "</h5>" +
                                        "<h6 class=\"price\">" + _product.Price.ToString("N0") + "<span class=\"under\">đ</span></h6>" +
                                         "<h6 class=\"discount\">Giảm " + phantramKM + "%</h6>" +
                                        "</a>" +

                                        "</div></div> ";
                        }
                        catch { }
                    }
                    htmlRelativePro += "</div>";
                    ViewData["RelativeProducts"] = htmlRelativePro;
                }
            }
            catch
            {
            }

            #endregion RELATIVE PRODUCT

            ViewData["Summary"] = item.Summary;
            ViewData["RawUrl"] = "http://" + Request.Url.Host + Request.RawUrl;
            try
            {
                ViewData["Manufacturer"] = string.Format("<a class=\"text-danger\" href=\"/search-dm0?mid={0}\">{1}</a>", /*Common.util.Function.ConvertUrlString(item.Manufacturer.ManufacturerName), */item.ManufacturerId, item.Manufacturers.ManufacturerName);
            }
            catch { }

            ViewData["TotalView"] = item.TotalView.ToString("N0");
            ViewData["LastUpdate"] = item.ModifyDate.ToString("dd/MM/yyyy");
            ViewData["DescriptionDetail"] = item.Description;

            #region IMAGES

            ProductImagesBusiness productImageBusiness = new ProductImagesBusiness();

            var listImg = productImageBusiness.FindProductId(id);
            string strImg1 = string.Empty, strImg2 = string.Empty;

            foreach (ProductImage img in listImg)
            {
                if (img.IsAvatar == 1)
                {
                    ViewData["ImgMain"] = img.ImgSource;
                    ViewData["ImgMainBig"] = img.ImgSource;
                }
                strImg1 += "<div class=\"item\">  <a class=\"thumb-link\" href=\"javascript: void(0);\" data-image=\""+Util.ReturnLinkCDN(img.ImgSource, Request.Browser.IsMobileDevice)+"\" data-zoom-image=\""+Util.ReturnLinkCDN(img.ImgSource, Request.Browser.IsMobileDevice) + "\" ><img itemprop=\"image\" src = '" + Util.ReturnLinkCDN(img.ImgSource, Request.Browser.IsMobileDevice) + "' border = '0' /></a></div>";
            }
            ViewData["Img1"] = strImg1;

            #endregion IMAGES

            #region IMAGES Price

            ProductImagesPriceBusiness productImagePriceBusiness = new ProductImagesPriceBusiness();

            var listImgPrice = productImagePriceBusiness.FindProductId(id);
            string imagePrice = "<ul>";
            foreach (ProductImagesPrice imgPrice in listImgPrice)
            {
                if (Request.Url.AbsolutePath == imgPrice.Note)
                    imagePrice += "<li id=\"pr_" + Function.ConvertFileName(imgPrice.PriceName) + "\" class=\"activeli \"><a href=\"" + imgPrice.Note + "\"> <p><i class=\"fa fa-check-circle-o\" aria-hidden=\"true\"></i> <span>" + imgPrice.PriceName + "</span></p>   <p> " + imgPrice.Price.ToString("N0") + "</p></a></li>";
                else
                    imagePrice += "<li id=\"pr_" + Function.ConvertFileName(imgPrice.PriceName) + "\"><a href=\"" + imgPrice.Note + "\"> <p><i class=\"fa fa-check-circle-o\" aria-hidden=\"true\"></i> <span>" + imgPrice.PriceName + "</span></p>   <p> " + imgPrice.Price.ToString("N0") + "</p></a></li>";
            }
            imagePrice += "</ul>";
            if (listImgPrice.Count() > 0)
                ViewData["ImgPrice"] = imagePrice;

            #endregion IMAGES Price

            string strTags = InitTags(item.Tags);
            ViewData["Tags"] = strTags;

            //  var obj=productBusiness.GetById()
            ViewData["JSSherma"] = JSSherma(item, rate.ToString(), totalVote.ToString(), totalStar.ToString());
            ProductRelated(item.CatalogId, null, null, null, null, string.Empty);
            return View(item);
        }

        protected void InitBreadcrumbs(long catalogId)
        {
            CatalogsBusiness catalogBusiness = new CatalogsBusiness();

            Catalog catalog = catalogBusiness.GetById(catalogId);

            string html = string.Format("<li><a href=\"/{1}\" title=\"\" itemprop=\"url\">{0}</a></li>", catalog.CatalogName, catalog.FriendlyUrl);
            while (catalog.ParentId != -1)
            {
                catalog = catalogBusiness.GetById(catalog.ParentId);
                html = string.Format("<li><a href=\"/{1}\" class=\"active\" title=\"\" itemprop=\"url\">{0}</a></li>", catalog.CatalogName, catalog.FriendlyUrl) + html;
            }

            ViewData["Breadcrumbs"] = html;
        }

        protected string InitStar(double? diem)
        {
            string html = "";
            for (int i = 0; i < 5; i++)
            {
                if (i < diem)
                {
                    html += " <i data-alt=\"1\" class=\"star-on-png\" title=\"Not rated yet!\"></i>&nbsp;";
                }
                else
                {
                    html += " <i data-alt=\"1\" class=\"star-off-png\" title=\"Not rated yet!\"></i>&nbsp;";
                }
            }

            return html;
        }

        protected string InitKhuyenmai(long id, int i)
        {
            string html = "";
            ProductAttributesBusiness productAttributesBusiness = new ProductAttributesBusiness();
            List<ProductAttribute> ListproductAttributes = new List<ProductAttribute>();
            ListproductAttributes = productAttributesBusiness.ListProductAttribute(id, i);
            if (i == 1 && ListproductAttributes.Count > 0)
            {
                html += " <ul class=\"info-ul\">";
                foreach (ProductAttribute re in ListproductAttributes)
                {
                    if (i == re.TypePP)
                        html += "<li><img src=\"" + Util.ReturnLinkCDN(re.Key, Request.Browser.IsMobileDevice) + "\" alt=\"\">" + re.Value + "</li>";
                }
                html += "</ul>";
            }
            if (i == 2)
            {
                html += "<ul class=\"box-ul\">";
                foreach (ProductAttribute re in ListproductAttributes)
                {
                    if (i == re.TypePP)
                        html += "<li><img class=\"lazyload\" src=\"" + Util.ReturnLinkCDN(re.Key, Request.Browser.IsMobileDevice) + "\" alt=\"\">" + re.Value + "</li>";
                }
                html += "</ul>";
            }
            return html;
        }

        //protected float TinhPhantramtheotungsao(int sao, long productID)
        //{
        //    try
        //    {
        //        CommentsBusiness commentsBusiness = new CommentsBusiness();
        //        return commentsBusiness.Getphantramsao(sao, productID);
        //    }
        //    catch { }
        //    return 0;
        //}

        public JsonResult ViewCart(long id, int sl)
        {
            var entity = NlCheckout.GetSessionCard(Session);
            List<CartItem> listitems = new List<CartItem>();
            if (entity != null)
            {
                listitems.AddRange(entity);
            }

            CartItem cart = new CartItem();
            cart.Quantity = sl;
            var product = new ProductsBusiness().GetById(id);
            cart.Product = product;
            listitems.Add(cart);
            NlCheckout.SetSessionCard(listitems, Session);
            return Json(listitems.Count, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TestAjax()
        {
            return Json(1);
        }

        #endregion PRODUCT DETAIL

        #region LANDING PAGE

        public ActionResult LandingPage(long id)
        {
            ProductsBusiness productBusiness = new ProductsBusiness();
            Product item = productBusiness.GetById(id);

            if (!string.IsNullOrEmpty(item.SeoTitle))
                ViewData["Title"] = item.SeoTitle;
            else
                ViewData["Title"] = item.ProductName;

            if (!string.IsNullOrEmpty(item.SeoDescription))

                ViewBag.Description = item.SeoDescription;
            else
                ViewBag.Description = item.ProductName;

            if (!string.IsNullOrEmpty(item.SeoKeyword))
                ViewBag.Keywords = item.SeoKeyword;
            else
                ViewBag.Keywords = item.ProductName;

            return View(item);
        }

        #endregion LANDING PAGE

        #region TAGS

        public ActionResult Tags(long id)
        {
            TagsBusiness tagsBusiness = new TagsBusiness();
            ProductsBusiness productsBusiness = new Business.ClassBusiness.ProductsBusiness();

            Tag t = tagsBusiness.GetById(id);

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
                ViewBag.image = Request.Url.Host + t.ImageSource;

                ViewData["TagName"] = t.TagName;

                int page = 1;

                if (Request.QueryString["page"] != null)
                {
                    page = int.Parse(Request.QueryString["page"]);
                }

                List<Product> listProduct = productsBusiness.GetByTag(t.Id.ToString(), page, Page_Size);

                int count = productsBusiness.CountByTag(t.Id.ToString());

                //Khởi tạo link tag

                string srtUrl = string.Format("/{0}", t.TagUrl);

                Paging(count, Page_Block, Page_Size, page, page, srtUrl);

                return View(listProduct);
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
                    html += "<a href=\"" + Models.Common.Util.ReturnLinkFull(t.TagUrl) + "\">" + t.TagName + "</a>, ";
                }

                if (!string.IsNullOrEmpty(html))
                {
                    html = html.Substring(0, html.Length - 2);
                }
            }
            return html;
        }

        #endregion TAGS

        #region SP LIÊN QUAN

        public void ProductRelated(long? id, string mid, string price, string order, int? page, string k)
        {
            ViewData["id"] = id;

            if (id == null) id = 0;

            if (page == null) page = 1;
            if (order == null) order = "new";
            double? priceForm = 0, priceTo = 0;

            if (!string.IsNullOrEmpty(price))
            {
                priceForm = double.Parse(price.Split(':')[0]);
                priceTo = double.Parse(price.Split(':')[1]);
            }

            Page_Size = 4;
            string strQuery = BuildQuery(id, mid, price, order, page, "", k);

            BuyGroup365Entities entitis = new BuyGroup365Entities();
            var result = entitis.Database.SqlQuery<Common.SearchProductByType_Result>(strQuery).ToList();

            ViewData["ProductRelated"] = result;
        }

        public JsonResult CommentProduct(long id, string Name, string Phone, string Email, string Content, int Star)
        {
            bool check = false;
            try
            {
                CommentsBusiness commentbusiness = new CommentsBusiness();
                Common.Comment comment = new Common.Comment();
                comment.Content = Content;
                comment.CreateDate = DateTime.Now;
                comment.ProductId = id;
                comment.Status = 1;
                comment.NickName = Name;
                comment.Phone = Phone;
                comment.Email = Email;
                comment.Rate = Star;
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

        #endregion SP LIÊN QUAN
        public void AddProductViews(long id)
        {

            var entity = NlCheckout.GetSessionProductViewed(Session);
            List<long> listitems = new List<long>();
            if (entity != null)
            {
                listitems.AddRange(entity);
            }
            bool a = false;
            foreach (var re in listitems)
            {
                if (re == id)
                {
                    a = true;
                    break;
                }

            }
            if (a == false)
                listitems.Add(id);
            NlCheckout.SetSessionProductViewed(listitems, Session);
        }
        public IReadOnlyCollection<SitemapNode> GetSitemapNodes()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();

            ListFriendlyUrl = friendlyUrlBussiness.searchAllLink();
            foreach (var friendly in ListFriendlyUrl)
            {
                nodes.Add(
                   new SitemapNode()
                   {
                       Url = ConfigurationManager.AppSettings["HostUrl"] + "/" + friendly.Link,
                       Frequency = SitemapFrequency.Weekly,
                       Priority = 0.8
                   });
            }
            return nodes;
        }

        public string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");
            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                xmlns + "url",
                new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                sitemapNode.LastModified == null ? null : new XElement(
                xmlns + "lastmod",
                sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                sitemapNode.Frequency == null ? null : new XElement(
                xmlns + "changefreq",
                sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                sitemapNode.Priority == null ? null : new XElement(
                xmlns + "priority",
                sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }
            XDocument document = new XDocument(root);
            return document.ToString();
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult SitemapXml()
        {
            var sitemapNodes = GetSitemapNodes();
            string xml = GetSitemapDocument(sitemapNodes);
            return this.Content(xml, "text/xml", Encoding.UTF8);
        }

        public FileStreamResult SaveData()
        {
            //todo: add some data from your database into that string:
            var string_with_your_data = "ABC";

            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "your_file_name.txt");
        }

        public string JSSherma(Product product, string ratingValue, string reviewCount, string aggregateRating)
        {
            string thuonghieu = "vinaplaza";
            string namecomment = "";
            try
            {
                thuonghieu = product.Manufacturers.ManufacturerName;
            }
            catch { }
            try
            {
                namecomment = product.Comments.FirstOrDefault().NickName;
            }
            catch { }
            string sherma = "<script type = \"application/ld+json\">" +
                "{\"@context\": \"https://schema.org/\"," +
                 "\"@type\": \"Product\"," +
                     "\"name\": \"" + product.ProductName + "\"," +
                    "\"image\": [" +
                      "\"" + @Util.ReturnLinkCDN(product.ProductImages.First().ImgSource, Request.Browser.IsMobileDevice) + "\"," +
                     "\"" + @Util.ReturnLinkCDN(product.ProductImages.First().ImgSource, Request.Browser.IsMobileDevice) + "\"," +
                      "\"" + @Util.ReturnLinkCDN(product.ProductImages.First().ImgSource, Request.Browser.IsMobileDevice) + "\"" +
                          "]," +
                         "\"description\": \"" + product.ShareDescription + "\"," +
                     "\"sku\": \"" + product.Code + "\"," +
                    "\"mpn\": \"" + product.Code + "\"," +
                     "\"brand\": {" +
                   "\"@type\": \"Thing\"," +
                  "\"name\": \"" + thuonghieu + "\"" +
                     "}," +
                     "\"review\": {" +
                  "\"@type\": \"Review\"," +
                  "\"reviewRating\": {" +
                   "\"@type\": \"Rating\"," +
                   "\"ratingValue\": \"" + ratingValue + "\"," +
                       "\"bestRating\": \"5\"" +
                      "}," +
                       "\"author\": {" +
                "\"@type\": \"Person\"," +
                 "\"name\": \"" + namecomment + "\"" +
                "}}," +
            "\"aggregateRating\": {" +
                  "\"@type\": \"AggregateRating\"," +
                "\"ratingValue\": \"" + aggregateRating + "\"," +
                "\"reviewCount\": \"" + reviewCount + "\"" +
              "}," +
              "\"offers\": {" +
                "\"@type\": \"Offer\"," +
                "\"url\": \"" + @Util.ReturnLinkFull(product.FriendlyUrl) + "\"," +
                "\"priceCurrency\": \"VND\"," +
                "\"price\": \"" + product.Price + "\"," +
                "\"priceValidUntil\": \"2020-11-05\"," +
                "\"itemCondition\": \"https://schema.org/UsedCondition\"," +
                "\"availability\": \"http://schema.org/InStock\"," +
                "\"seller\": {" +
                  "\"@type\": \"Organization\"," +
                  "\"name\": \"vinaplaza\"" +
                "}" +
              "}" +
            "}" +
            "</script>";
            return sherma;
        }

        public string JSShermahome()
        {
            string html = "<script type=\"application/ld+json\">{" +
  "\"@context\": \"http://schema.org\", " +
  "\"@type\": \"HealthAndBeautyBusiness\", " +
  "\"@id\": \"https://www.vinaplaza.com/\", " +
  "\"url\": \"https://www.vinaplaza.com/\", " +
  "\"logo\": \"https://cdn.vinaplaza.vn/Images/img/logo.png\", " +
   "\"image\":\"https://cdn.vinaplaza.vn/Images/img/logo.png\"," +
    "\"priceRange\":\"1.000.000VND-20.000.000VND\"," +
  "\"hasMap\": \"https://www.google.com/maps/place/Th%E1%BA%BF+Gi%E1%BB%9Bi+Dinh+D%C6%B0%E1%BB%A1ng+Vinaplaza/@20.9754771,105.8482728,17z/data=!3m1!4b1!4m5!3m4!1s0x3135ade338cff39f:0x44d946b5f4c7f86b!8m2!3d20.9754721!4d105.8504615\",	" +
  "\"email\": \"mailto:dinhduongvinaplaza@gmail.com\", " +
  "\"founder\": { " +
  "\"@type\": \"Person\", " +
  "\"name\": \"Nguyen Hoa\", " +
  "\"jobTitle\": \"CEO\", " +
  "\"worksFor\": \"Vinaplaza\", " +
  "\"sameAs\": [ " +
  "\"https://www.facebook.com/dinhduongvinaplaza/\" " +
  "] " +
  "}, " +
  "\"address\": { " +
  "\"@type\": \"PostalAddress\", " +
  "\"addressLocality\": \"Hoàng Mai\", " +
  "\"addressCountry\": \"VIỆT NAM\", " +
  "\"addressRegion\": \"Hà Nội\", " +
  "\"postalCode\":\"100000\", " +
  "\"streetAddress\": \"56C Ngõ 143/34 Nguyễn Chính, Hoàng Mai, Hà Nội 100000\" " +
  "}, " +
  "\"description\": \"Thế giới dinh dưỡng Vinaplaza - Cửa hàng bán lẻ sản phẩm chăm sóc sức khỏe như An cung ngưu hoàng, nhân sâm tươi, cao hồng sâm, yến sào khánh hòa, nhung hươu\", " +
  "\"name\": \"Vinaplaza\", " +
  "\"telephone\": \"+84 936689967\", " +
  "\"openingHoursSpecification\": { " +
  "\"@type\": \"OpeningHoursSpecification\", " +
  "\"dayOfWeek\": [ " +
  "\"Monday\", " +
  "\"Tuesday\", " +
  "\"Wednesday\", " +
  "\"Thursday\", " +
  "\"Friday\", " +
  "\"Saturday\", " +
  "\"Sunday\" " +
  "], " +
  "\"opens\": \"08:00\", " +
  "\"closes\": \"20:00\"" +
  "}, " +
  "\"sameAs\" : [ \"https://www.facebook.com/dinhduongvinaplaza/\"," +
  "\"https://www.youtube.com/channel/UCZq7suXTEoETXPvWsDHTRzA\"," +
  "\"https://twitter.com/vinaplazavn\", " +
  "\"https://www.instagram.com/ceovinaplaza/\", " +
  "\"https://vn.linkedin.com/in/vinaplaza\", " +
  "\"https://www.pinterest.com/vinaplaza/\", " +
  "\"https://vinaplaza.blogspot.com/\", " +
  "\"https://medium.com/@vinaplaza\"]" +
  "} " +
  "</script>";
            return html;
        }
    }
}