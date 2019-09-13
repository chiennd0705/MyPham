using System.Configuration;
using System.Web.Optimization;

namespace BuyGroup365
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Content/Csspublic/js/jquery-1.12.4.js",
                      "~/Content/Mypham/js/jquery-ui.js"));

         

            bundles.Add(new ScriptBundle("~/bundles/jqueryheader").Include(
                "~/Content/Mypham/js/jquery-3.3.1.min.js",
                "~/Content/Mypham/js/bootstrap.min.js",
                "~/Content/Vinaplaza/js/owl.carousel.min.js",
                "~/Content/Vinaplaza/js/slider-banner.js",
                "~/Content/Vinaplaza/js/lazysizes.min.js",
                "~/Content/Vinaplaza/js/custom.js"));

            bundles.Add(new StyleBundle("~/bundles/headercss").Include(
                   //"~/Content/Vinaplaza/css/jquery-ui.css"
                   "~/Content/Vinaplaza/css/plugin.css"
                    //"~/Content/Vinaplaza/css/homepage.css"
                    // "~/Content/Vinaplaza/css/header.css"
                    ));

            bundles.Add(new StyleBundle("~/bundles/NewDetailcss").Include(
                 "~/Content/Vinaplaza/css/jquery-ui.css",
                  "~/Content/Vinaplaza/css/plugin.css",
                  "~/Content/Vinaplaza/css/news-detail.css",
                  "~/Content/Vinaplaza/css/header.css"));

            bundles.Add(new StyleBundle("~/bundles/Newcss").Include(
                 "~/Content/Vinaplaza/css/jquery-ui.css",
                  "~/Content/Vinaplaza/css/plugin.css",
                  "~/Content/Vinaplaza/css/news.css",
                  "~/Content/Vinaplaza/css/header.css"));
            bundles.Add(new StyleBundle("~/bundles/CartSuccessCss").Include(
                 "~/Content/Vinaplaza/css/jquery-ui.css",
                 "~/Content/Vinaplaza/css/plugin.css",
                 "~/Content/Vinaplaza/css/cart-success.css",
                 "~/Content/Vinaplaza/css/header.css"));

            bundles.Add(new StyleBundle("~/bundles/cartCss").Include(
                 "~/Content/Vinaplaza/css/jquery-ui.css",
                 "~/Content/Vinaplaza/css/plugin.css",
                 "~/Content/Vinaplaza/css/cart.css",
                 "~/Content/Vinaplaza/css/header.css"));

            bundles.Add(new StyleBundle("~/bundles/ProductCss").Include(
                 "~/Content/Vinaplaza/css/jquery-ui.css",
                             "~/Content/Vinaplaza/css/plugin.css",
                             "~/Content/Vinaplaza/css/product.css",
                             "~/Content/Vinaplaza/css/header.css"));

            bundles.Add(new StyleBundle("~/bundles/ProductDetailCss").Include(
                 "~/Content/Vinaplaza/css/jquery-ui.css",
                             "~/Content/Vinaplaza/css/plugin.css",
                             "~/Content/Vinaplaza/css/magnific-popup.css",
                             "~/Content/Vinaplaza/css/lightgallery.min.css",
                               "~/Content/Vinaplaza/css/product-detail.min.css",
                             "~/Content/Vinaplaza/css/header.css"));

            bundles.Add(new ScriptBundle("~/bundles/ProductDetailjs").Include(
              "~/Content/Vinaplaza/js/jquery-3.3.1.min.js",
              "~/Content/Vinaplaza/js/bootstrap.min.js",
              "~/Content/Vinaplaza/js/owl.carousel.min.js",
              "~/Content/Vinaplaza/js/slider-banner.js",
              "~/Content/Vinaplaza/js/lazysizes.min.js",
               "~/Content/Vinaplaza/js/lightgallery-all.min.js",
              "~/Content/Vinaplaza/js/custom.js"));

            BundleTable.EnableOptimizations = true;
            BundleTable.Bundles.UseCdn = false;
        }
    }

    public class SiteKeys
    {
        public static string StyleVersion
        {
            get
            {
                return "<link rel=\"stylesheet\" href=\"{0}\" media=\"all\" ></link>";
            }
        }
        public static string ScriptVersion
        {
            get
            {
                return "<script src=\"{0}\" defer></script>";
            }
        }
        public static string ScriptNoDeferVersion
        {
            get
            {
                return "<script src=\"{0}\"></script>";
            }
        }
    }
}