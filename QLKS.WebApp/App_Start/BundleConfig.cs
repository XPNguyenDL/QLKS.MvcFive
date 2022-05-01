using System.Web;
using System.Web.Optimization;

namespace QLKS.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            // Admin Area 
            bundles.Add(new ScriptBundle("~/bundles/admmainjs").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                "~/Scripts/plugins/slimsroll/jquery.slimscroll.js"));

            bundles.Add(new ScriptBundle("~/bundles/admjqueryval").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/admcustomjs").Include(
                "~/Scripts/cheapdeal.js",
                "~/Scripts/plugins/pace/pace.min.js"));

            bundles.Add(new StyleBundle("~/Content/admbootstrap").Include(
                "~/Content/bootstrap-3.3.0.min.css",
                "~/Content/font-awasome.css"));
            bundles.Add(new StyleBundle("~/Content/admstyle").Include(
                "~/Content/animate.css",
                "~/Content/Admsite.css"));

            // User area
            bundles.Add(new StyleBundle("~/Content/hotelstyle").Include(
                "~/Content/hotel_style/css/bootstrap.min.css",
                "~/Content/hotel_style/css/owl.carousel.min.css",
                "~/Content/hotel_style/css/gijgo.css",
                "~/Content/hotel_style/css/slicknav.css",
                "~/Content/hotel_style/css/animate.min.css",
                "~/Content/hotel_style/css/magnific-popup.css",
                "~/Content/hotel_style/css/fontawesome-all.min.css",
                "~/Content/hotel_style/css/themify-icons.css",
                "~/Content/hotel_style/css/slick.css",
                "~/Content/hotel_style/css/nice-select.css",
                "~/Content/hotel_style/css/style.css",
                "~/Content/hotel_style/css/responsive.css",
                "~/Content/hotel_style/css/main.css"));

            bundles.Add(new ScriptBundle("~/Script/hotelscript").Include(
                    "~/hotelJs/js/vendor/modernizr-3.5.0.min.js",

                    "~/hotelJs/js/vendor/jquery-1.12.4.min.js",
                    "~/hotelJs/js/popper.min.js",
                    "~/hotelJs/js/bootstrap.min.js",

                    "~/hotelJs/js/gijgo.min.js",

                    "~/hotelJs/js/wow.min.js",
                    "~/hotelJs/js/animated.headline.js",
                    "~/hotelJs/js/jquery.magnific-popup.js",

                    "~/hotelJs/js/jquery.scrollUp.min.js",
                    "~/hotelJs/js/jquery.nice-select.min.js",
                    "~/hotelJs/js/jquery.sticky.js",

                    "~/hotelJs/js/contact.js",
                    "~/hotelJs/js/jquery.form.js",
                    "~/hotelJs/js/jquery.validate.min.js",
                    "~/hotelJs/js/mail-script.js",
                    "~/hotelJs/js/jquery.ajaxchimp.min.js",

                    "~/hotelJs/js/plugins.js",
                    "~/hotelJs/js/main.js"));

            bundles.Add(new ScriptBundle("~/Script/HotelSlick").Include(
                "~/hotelJs/js/jquery.slicknav.min.js",
                "~/hotelJs/js/owl.carousel.min.js",
                "~/hotelJs/js/slick.min.js"));
        }
    }
}
