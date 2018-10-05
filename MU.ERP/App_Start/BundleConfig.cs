using System.Web;
using System.Web.Optimization;

namespace MU.ERP
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                        "~/Scripts/vue.js",
                        "~/Scripts/axios.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/element").Include(
                        "~/Content/element-ui/lib/index.js"));

            bundles.Add(new StyleBundle("~/Content/elementui").Include(
                      "~/Content/element-ui/lib/theme-chalk/index.css",
                      "~/Content/fontawesome/css/animation.css",
                      "~/Content/fontawesome/css/fontello.css"));
        }
    }
}
