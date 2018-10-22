using System.Web;
using System.Web.Optimization;

namespace MU.ERP.Portal
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                        "~/Scripts/vue.js",
                        "~/Scripts/axios.js",
                        "~/Scripts/ElementUI/element-ui.js"));

            bundles.Add(new StyleBundle("~/Content/Element").Include(
                      "~/Content/reset.css",
                      "~/Content/ElementUI/element-ui.css",
                      "~/Content/site.css"));
        }
    }
}
