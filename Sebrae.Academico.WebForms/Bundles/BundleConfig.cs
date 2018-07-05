using System.Web.Optimization;

namespace Sebrae.Academico.WebForms.Bundles
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/master").Include(
                        "~/js/jquery/jquery-{version}.js",
                        "~/js/jquery/jquery.cookie.js",
                        "~/js/bootstrap.js",
                        "~/js/tooltip.js",
                        "~/js/noty/packaged/jquery.noty.packaged.min.js",
                        "~/js/spin.min.js",
                        "~/js/jquery/jquery.maskedinput.min.js",
                        "~/js/jquery/jquery.autocomplete.js",
                        "~/js/jquery.redirect.min.js",
                        "~/js/jquery/jquery.markall.js",
                        "~/js/helpdialogs.js",
                        "~/js/image-upload-control.js",
                        "~/js/funcoesglobais.js",
                        "~/js/jqueryUI/jquery-ui.min.js",
                        "~/js/jquery-are-you-sure/jquery.are-you-sure.js",
                        "~/js/autocomplete-functions.js",
                        "~/js/combobox-autocomplete.js",
                        "~/js/linq.min.js",
                        "~/js/jquery/jquery.tablednd.js",
                        "~/js/elessar.min.js",
                        "~/js/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/css/bootstrap.css",
                        "~/css/font-awesome-ext.css",
                        "~/css/autocomplete-style.css",
                        "~/css/markall.css",
                        "~/js/jqueryUI/jquery-ui.min.css",
                        "~/css/autocomplete-style.css",
                        "~/css/elessar.css",
                        "~/css/fonts.css",
                        "~/css/style-sgus20.css"));

            #if !DEBUG
                BundleTable.EnableOptimizations = true;
            #endif
        }
    }
}