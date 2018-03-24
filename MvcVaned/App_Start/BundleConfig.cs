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

            //// New theme
            //bundles.Add(new StyleBundle("~/Content/css/slate").Include(
            //          "~/Content/Themes/slate/bootstrap.min.css"));

            //bundles.Add(new StyleBundle("~/Content/css/cyborg").Include(
            //          "~/Content/Themes/cyborg/bootstrap.min.css"));

            //bundles.Add(new StyleBundle("~/Content/css/darkly").Include(
            //          "~/Content/Themes/darkly/bootstrap.min.css"));

            //bundles.Add(new StyleBundle("~/Content/css/flatly").Include(
            //          "~/Content/Themes/flatly/bootstrap.min.css"));
            // New theme
            bundles.Add(new StyleBundle("~/Content/css/slate", "https://cdn.rawgit.com/Razer2015/01ee5b4b78cea0ddc54716ffdd07a991/raw/d86595492a3fd6da9332b138d4af8fee7ac93e2a/slate.bootstrap.min.css").Include(
                      "~/Content/Themes/slate/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/cyborg", "https://cdn.rawgit.com/Razer2015/4e31c17044691e8e6c294a2e691648be/raw/6e060217e18d498ddf0f08646142266dbe327aef/cyborg.bootstrap.min.css").Include(
                      "~/Content/Themes/cyborg/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/darkly", "https://cdn.rawgit.com/Razer2015/ed543a5558df50381ea20b1906f7b17c/raw/d534a7c4deb35c1ebcf8a04bb47d71f2ce63fc22/darkly.bootstrap.min.css").Include(
                      "~/Content/Themes/darkly/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/flatly", "https://cdn.rawgit.com/Razer2015/7341dcce6ae01678aa28c9096634332b/raw/fea6fa4dc541deb6aa8eb9b8220f4e2d00847fa2/flatly.bootstrap.min.css").Include(
                      "~/Content/Themes/flatly/bootstrap.min.css"));

            //bundles.Add(new StyleBundle("~/Content/css", "https://maxcdn.bootstrapcdn.com/bootswatch/4.0.0/slate/bootstrap.min.css").Include(
            //          "~/Content/slate.bootstrap.css",
            //          "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
