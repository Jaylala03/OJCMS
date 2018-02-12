using System.Web;
using System.Web.Optimization;

namespace eCMS.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/assets/styles/external").Include(
                "~/assets/styles/external/bootstrap.min.css",
                "~/assets/styles/external/style.css"));

            bundles.Add(new StyleBundle("~/assets/css").Include(
                "~/assets/styles/bootstrap.css",
                "~/assets/styles/style.css",
                "~/assets/styles/custom.css"));

            bundles.Add(new StyleBundle("~/assets/plugins/fancybox/css").Include(
                "~/assets/plugins/fancybox/jquery.fancybox.css?v=2.1.5"));

            bundles.Add(new ScriptBundle("~/assets/plugins/fancybox/js").Include(
                        "~/assets/plugins/fancybox/jquery.fancybox.pack.js?v=2.1.5"));

            // The Kendo CSS bundle
            bundles.Add(new StyleBundle("~/assets/plugins/kendo/css").Include(
                    "~/assets/plugins/kendo/styles/kendo.common.min.css",
                    "~/assets/plugins/kendo/styles/kendo.default.min.css"));

            // The Kendo JavaScript bundle
            bundles.Add(new ScriptBundle("~/assets/plugins/kendo/js").Include(
                    "~/assets/plugins/kendo/scripts/kendo.web.min.js", // or kendo.all.* if you want to use Kendo UI Web and Kendo UI DataViz
                    "~/assets/plugins/kendo/scripts/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/assets/plugins/jquery-ui").Include(
                   "~/assets/plugins/jquery-ui/jquery-ui.css", // or kendo.all.* if you want to use Kendo UI Web and Kendo UI DataViz
                   "~/assets/plugins/jquery-ui/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/assets/scripts/js").Include(
                    "~/assets/scripts/custom3.0.js"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.7.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.8.20.min.js"));
            bundles.Add(new ScriptBundle("~/assets/plugins/fuelux/js").Include(
                    "~/assets/plugins/fuelux/js/spinner.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Assets/scripts/jquery.unobtrusive-ajax.min.js",
                       "~/Assets/scripts/jquery.validate.min.js",
                       "~/Assets/scripts/jquery.validate.unobtrusive.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Assets/pointhr/css").Include(
            //    "~/Assets/pointhr/css/bootstrap.css",
            //    "~/Assets/pointhr/css/style.css",
            //    "~/Assets/pointhr/css/custom.css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //    "~/Content/style2.0.css",
            //    "~/Content/plugins/font-awesome/css/font-awesome.css"));

            //bundles.Add(new StyleBundle("~/Content/css/signin").Include(
            //    "~/Content/external/signin.css"));

            //bundles.Add(new StyleBundle("~/Content/css/signup").Include(
            //    "~/Content/external/signup.css"));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));

            

            //// The Kendo JavaScript bundle
            //bundles.Add(new ScriptBundle("~/bundles/player").Include(
            //        "~/Scripts/player/jwplayer.js"));

            ////Scripts needs to be placed on html head
            //bundles.Add(new ScriptBundle("~/UserInternalJSHeader").Include(
            //        "~/assets/plugins/jquery-1.8.3.min.js"));

            //bundles.Add(new StyleBundle("~/FancyBoxCSS").Include(
            //        "~/Scripts/fancybox/jquery.fancybox.css"
            //        ));

            //bundles.Add(new ScriptBundle("~/FancyBoxJS").Include(
            //        "~/Scripts/fancybox/jquery.mousewheel-3.0.6.pack.js",
            //        "~/Scripts/fancybox/jquery.fancybox.pack.js"
            //        ));

            

            //bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
            //        "~/Scripts/jquery.min.js",
            //        "~/Scripts/bootstrap.min.js",
            //        "~/Scripts/docs.min.js"
            //        ));

            //bundles.Add(new ScriptBundle("~/FancyBoxJS").Include(
            //        "~/Scripts/fancybox/jquery.fancybox-1.3.4.pack.js",
            //        "~/Scripts/fancybox/jquery.easing-1.4.pack.js",
            //        "~/Scripts/fancybox/jquery.fancybox.pack.js"
            //        ));

            //bundles.Add(new StyleBundle("~/FancyBoxCSS").Include(
            //        "~/Scripts/fancybox/jquery.fancybox.css"
            //        ));


            // Clear all items from the ignore list to allow minified CSS and JavaScript files in debug mode
            bundles.IgnoreList.Clear();


            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);

            BundleTable.EnableOptimizations = false;
        }
    }
}