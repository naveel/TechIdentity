using System.Web;
using System.Web.Optimization;

namespace HBL_MLDV_APP
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/CommonScripts").Include(
                      "~/bower_components/jquery/dist/jquery.min.js",
                      "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                      "~/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/bower_components/jquery-sparkline/dist/jquery.sparkline.js",
                      "~/bower_components/fastclick/lib/fastclick.js",

                      "~/dist/js/adminlte.min.js",
                      "~/dist/js/demo.js",
                      "~/Scripts/LoadTree.js",
                      "~/Content/plugins/toastr/toastr.min.js",
                      "~/Content/plugins/toastr/notify.js", "~/Content/plugins/switchery/switchery.min.js"));

            bundles.Add(new StyleBundle("~/CommonStyles").Include(
                      "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      //"~/Content/site.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                      "~/dist/css/skins/_all-skins.min.css",
                      "~/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/Content/plugins/toastr/toastr.min.css",
   "~/Content/plugins/switchery/switchery.min.css"));
        }
    }
}
