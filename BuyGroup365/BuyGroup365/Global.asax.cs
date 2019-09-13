using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BuyGroup365
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private int ik = 0;
        internal static readonly string IsMobileCustomVaryByString = "IsMobile";
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new RazorViewEngine());
            // ConfigurationLogging();
            //  Session.Timeout = 20;

            #region SQL Cache Dependency

            try
            {
                string[] tables = { "News", "NewsGroups", "Products", "Catalogs", "Menus", "CatalogProducts", "ProductProperties", "ProductImagesPrice", "ProductImages", "ProductAttributes", "CatalogPropertiesValue", "CatalogProperties", "TextHtml", "FriendlyUrl", "CommentsNew", "ConfigRedirectUrl", "Comments" };
                string connectionString = ConfigurationManager.ConnectionStrings["database_dienmaymienbac_Conn"].ConnectionString;
                System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(connectionString);
                System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connectionString, tables);
            }
            catch { }

            #endregion SQL Cache Dependency
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // if(System.Web.Caching.SqlCacheDependencyAdmin.GetTablesEnabledForNotifications.indexOf("turtles")

            CheckUrlRedirect();
        }
        [OutputCache(CacheProfile = "ConfigRedirectUrl")]
        protected void CheckUrlRedirect()
        {
            Business.ClassBusiness.ConfigRedirectUrlBusiness configRedirectUrlBusiness = new Business.ClassBusiness.ConfigRedirectUrlBusiness();
            List<Common.ConfigRedirectUrl> listUrl = configRedirectUrlBusiness.GetByKey();
            string urlRedirect = string.Empty;
            bool isRedirect = false;
            foreach (Common.ConfigRedirectUrl item in listUrl)
            {
                if (Request.Url.PathAndQuery.Contains(item.Url))
                {
                    isRedirect = true;
                    urlRedirect = item.RedirectUrl;
                    break;
                }
            }
            if (isRedirect)
            {
                Response.Redirect("https://www.vinaplaza.vn" + urlRedirect);
            }
        }

        public void ConfigurationLogging()
        {
            string sLogFile = HttpRuntime.AppDomainAppPath + "log4net.config";
            if ((System.IO.File.Exists(sLogFile)))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(sLogFile));
            }
        }

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

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (Request.RawUrl.Contains("/bundles/"))
            {
                // My bundles all have a /bundles/ prefix in the URL
                Response.Cache.SetExpires(DateTime.Now.AddHours(240));
            }
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            // this is for the output cache
            if (context != null)
            {
                switch (custom)
                {
                    case "Mobile":
                        return GetMobileCustomString(context);
                }
            }

            return base.GetVaryByCustomString(context, custom);
        }

        private static string GetMobileCustomString(HttpContext context)
        {
            if (context.Request.Browser.IsMobileDevice)
            {
                return "IsMobile";
            }
            else
            {
                return "IsDesktop";
            }
        }

    }
}

