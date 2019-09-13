using Business.ClassBusiness;
using Common;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace BuyGroup365
{
    public class RouteConfig
    {
        [OutputCache(CacheProfile = "friendlyUrl")]
        public static void RegisterRoutes(RouteCollection routes)
        {
            try
            {
                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                FriendlyUrlBusiness a = new FriendlyUrlBusiness();
                List<FriendlyUrl> listFriendly = new List<FriendlyUrl>();
                listFriendly = a.searchAllLink();
                //    routes.Clear();
                foreach (FriendlyUrl re in listFriendly)
                {
                    routes.MapRoute(name: re.NameLink, url: re.Link, defaults: new { controller = re.ControllerName, action = re.ActionName, id = re.ItemId, }, namespaces: new string[] { re.NameSpaces });
                }
                //       routes.MapRoute(name: "them-lua-tinh-yeu", url: "danh-muc/them-lua-tinh-yeu-voi-nhung-mau-giuong-ngu-dep-mat.html", defaults: new { controller = "News", action = "Detail", nid = 26, }, namespaces: new string[] { "BuyGroup365.Controllers" });

                //     routes.MapRoute(name: "Catalog", url: "danh-muc/{friendUrl}.html", defaults: new { controller = "Home", action = "Category", friendUrl = UrlParameter.Optional, });
                //routes.MapRoute(name: "Manufacturer", url: "{Content}-m{id}", defaults: new { controller = "Home", action = "Manufacturer", id = UrlParameter.Optional, slug = "" });
                //     routes.MapRoute(name: "ProductDetail", url: "{Content}-ct{id}.html", defaults: new { controller = "Home", action = "ProductDetail", id = UrlParameter.Optional, });
                //      routes.MapRoute(name: "GetProductByShop", url: "{Content}-mc{id}.html", defaults: new { controller = "Home", action = "GetProductByShop", id = UrlParameter.Optional, });
                //       routes.MapRoute(name: "NewsDetail", url: "help/{Content}-n{nid}.html", defaults: new { controller = "News", action = "Detail", nid = UrlParameter.Optional, },
                //                namespaces: new string[] { "BuyGroup365.Controllers" });
                try
                {
                    CatalogsBusiness cata = new CatalogsBusiness();
                    ManufacturersBusiness manu = new ManufacturersBusiness();
                    List<Catalog> ListCatalog = new List<Catalog>();

                    ListCatalog = cata.SearchCatalogALL();
                    //    routes.Clear();
                    foreach (Catalog re in ListCatalog)
                    {
                        List<Manufacturer> ListManuFater = new List<Manufacturer>();
                        ListManuFater = manu.GetList();
                        foreach (Manufacturer mu in ListManuFater)
                        {
                            routes.MapRoute(name: re.CatalogName + re.Id + "hang" + mu.Id, url: re.FriendlyUrl + "-" + Common.util.Function.ConvertFileName(mu.ManufacturerName), defaults: new { controller = "Home", action = "Category", id = re.Id });
                        }
                    }
                }
                catch { }
                routes.MapRoute(name: "Search", url: "search.html", defaults: new { controller = "Home", action = "Category", id = -2 });

                //          routes.MapRoute(name: "NewsGroup", url: "tin-tuc/{Content}-nt{nt}.html", defaults: new { controller = "News", action = "Index", nt = UrlParameter.Optional, },
                //              namespaces: new string[] { "BuyGroup365.Controllers" });
                //       routes.MapRoute(name: "NewsDetail2", url: "tin-tuc/{Content}-nid{nid}.html", defaults: new { controller = "News", action = "Detail", nid = UrlParameter.Optional, },
                //                namespaces: new string[] { "BuyGroup365.Controllers" });
                //routes.MapRoute(name: "tags", url: "san-pham/{tag}.html", defaults: new { controller = "Home", action = "Tags", tag = UrlParameter.Optional, },
                //          namespaces: new string[] { "BuyGroup365.Controllers" });
                //routes.MapRoute(name: "tagsnew", url: "tin-tuc-tag/{tag}.html", defaults: new { controller = "News", action = "Tags", tag = UrlParameter.Optional, },
                //     namespaces: new string[] { "BuyGroup365.Controllers" });
                routes.MapRoute(name: "video", url: "videos.html", defaults: new { controller = "Videos", action = "index" }, namespaces: new string[] { "BuyGroup365.Controllers" });
                routes.MapRoute(name: "download", url: "downloads.html", defaults: new { controller = "Downloads", action = "index" }, namespaces: new string[] { "BuyGroup365.Controllers" });
                routes.MapRoute(name: "Contact", url: "lien-he.html", defaults: new { controller = "Home", action = "Contact" });
                routes.MapRoute(name: "sitemap", url: "sitemap.xml", defaults: new { controller = "Home", action = "SitemapXml" });
                routes.MapRoute(name: "sanphammoi", url: "san-pham-moi.html", defaults: new { controller = "Home", action = "sanphammoi" });
                routes.MapRoute(name: "loi404", url: "loi404", defaults: new { controller = "Home", action = "loi404" });
                routes.MapRoute(name: "giohang", url: "gio-hang", defaults: new { controller = "PayGoods", action = "CheckOut" });
                //routes.MapRoute(name: "faqlist", url: "hoi-dap.html", defaults: new { controller = "News", action = "FAQ" }, namespaces: new string[] { "BuyGroup365.Controllers" });
                //routes.MapRoute(name: "faqdetail", url: "chi-tiet-cau-hoi.html", defaults: new { controller = "News", action = "DetailFAQ", id = UrlParameter.Optional }, namespaces: new string[] { "BuyGroup365.Controllers" });
                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                       namespaces: new string[] { "BuyGroup365.Controllers" }
                );
            }
            catch { }
        }
    }
}