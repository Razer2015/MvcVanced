using System.Web;
using System.Web.Optimization;

namespace MvcVanced
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js").Include(
            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval", "https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.17.0/jquery.validate.min.js").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr", "https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/datatable", "https://cdn.datatables.net/1.10.16/js/jquery.dataTables.min.js").Include(
                      "~/Content/datatables/jquery.dataTables.min.js"));
            bundles.Add(new StyleBundle("~/Content/datatable_bootstrap", "https://cdn.datatables.net/1.10.16/js/dataTables.bootstrap4.min.js").Include(
                      "~/Content/datatables/jquery.dataTables.min.js"));

            // New theme
            bundles.Add(new StyleBundle("~/Content/css/slate", "https://maxcdn.bootstrapcdn.com/bootswatch/4.0.0/slate/bootstrap.min.css").Include(
                      "~/Content/Themes/slate/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/cyborg", "https://maxcdn.bootstrapcdn.com/bootswatch/4.0.0/cyborg/bootstrap.min.css").Include(
                      "~/Content/Themes/cyborg/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/darkly", "https://maxcdn.bootstrapcdn.com/bootswatch/4.0.0/darkly/bootstrap.min.css").Include(
                      "~/Content/Themes/darkly/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/flatly", "https://maxcdn.bootstrapcdn.com/bootswatch/4.0.0/flatly/bootstrap.min.css").Include(
                      "~/Content/Themes/flatly/bootstrap.min.css"));

            //bundles.Add(new StyleBundle("~/Content/css", "https://maxcdn.bootstrapcdn.com/bootswatch/4.0.0/slate/bootstrap.min.css").Include(
            //          "~/Content/slate.bootstrap.css",
            //          "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
